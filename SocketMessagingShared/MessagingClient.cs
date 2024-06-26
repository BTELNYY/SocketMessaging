﻿using SocketNetworking;
using SocketNetworking.Attributes;
using SocketMessagingShared.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using SocketMessagingShared.CustomTypes;
using System.Security;
using System.Security.Permissions;
using SocketNetworking.PacketSystem.TypeWrappers;

namespace SocketMessagingShared
{
    public class MessagingClient : NetworkClient
    {
        public override void Init()
        {
            base.Init();
            this.ConnectionStateUpdated += CreateUserObject;
        }

        public delegate bool ValidateLoginDelegate(MessagingClient client, LoginData data, out string reason);
        public ValidateLoginDelegate OnValidateLogin { get; set; } = null;

        public delegate bool UserCreateAccountDelegate(MessagingClient client, LoginData data, out string reason);
        public UserCreateAccountDelegate OnUserCreateAccount { get; set; } = null;

        public delegate void UserFailLogin(NetworkClient client, string reason);
        public UserFailLogin OnUserLoginFail { get; set; } = null;

        public delegate void UserFailAccountCreation(NetworkClient client, string reason);
        public UserFailAccountCreation OnUserFailAccountCreation { get; set; } = null;

        public NetworkChannelController ClientChannelController
        {
            get
            {
                if(NetworkManager.WhereAmI == ClientLocation.Remote)
                {
                    return MessagingServer.NetworkChannelController;
                }
                return _controller;
            }
            set
            {
                _controller = value;
            }
        }
        
        private NetworkChannelController _controller;

        [NetworkInvocable(PacketDirection.Server)]
        private void ClientCreateChannelController(int networkId)
        {
            _controller = new NetworkChannelController();
            _controller.SetNetID(networkId);
            _controller.LocalClient = this;
        }

        public string Username
        {
            get
            {
                return User.Username;
            }
        }

        public string UUID
        {
            get
            {
                return User.UUID;
            }
        }

        private void CreateUserObject(ConnectionState obj)
        {
            if(obj != ConnectionState.Connected)
            {
                return;
            }
            _user = new NetworkUser(this);
            NetworkManager.AddNetworkObject(_user);
            if(CurrnetClientLocation == ClientLocation.Remote)
            {
                ServerNotifyClientAdded(_user);
            }
        }

        private NetworkUser _user;

        public NetworkUser User 
        {
            get    
            {
                return _user;
            } 
        }

        public bool ClientLogin(string username, string password) 
        {
            LoginData loginData = new LoginData();
            loginData.Username = username;
            loginData.SetPassword(password);
            return NetworkInvoke<bool>(this, nameof(ServerPerformLoginCommand), new object[] { loginData });
        }

        public void ClientLogout()
        {
            NetworkInvoke(this, nameof(ServerPreformLogout), new object[] { });
        }

        public bool ClientCreateAccount(string username, string password)
        {
            LoginData loginData = new LoginData();
            loginData.Username = username;
            loginData.SetPassword(password);
            return NetworkInvoke<bool>(nameof(ServerCreateAccountCommand), new object[] { loginData });
        }

        public void ServerNotifyClientAdded(NetworkUser user)
        {
            NetworkServer.NetworkInvokeOnAll(this, nameof(ClientAddedRpc), new object[] { user.NetworkID, user.OwnerClientID });
        }

        public void ServerNotifyClientRemoved(NetworkUser user)
        {
            NetworkInvoke(nameof(ClientRemovedRpc), new object[] { user.NetworkID });
        }

        public void ServerLogout(NetworkClient client)
        {
            MessagingServer.NetworkChannelController.ServerSyncChannels(client, new SerializableList<NetworkChannel>());
            client.Ready = false;
        }

        [NetworkInvocable(PacketDirection.Client)]
        private void ServerPreformLogout(NetworkClient client)
        {
            ServerLogout(client);
        }

        [NetworkInvocable(PacketDirection.Client)]
        private bool ServerPerformLoginCommand(LoginData data)
        {
            if (OnValidateLogin != null)
            {
                bool result = OnValidateLogin.Invoke(this, data, out string reason);
                if (!result)
                {
                    Log.GlobalWarning($"Rejecting client login from {ClientID} becuase: {reason}");
                    NetworkInvoke(this, nameof(ClientLoginFail), new string[] { reason });
                    OnUserLoginFail?.Invoke(this, reason);
                    return false;
                }
            }
            Ready = true;
            Log.GlobalInfo($"User '{data.Username}' logged in with ClientID {ClientID}");
            User.ServerSetUsername(data.Username);
            return true;
        }

        [NetworkInvocable(PacketDirection.Server)]
        private void ClientLoginFail(string reason)
        {
            Log.GlobalError("Login failed: " + reason);
            OnUserLoginFail?.Invoke(this, reason);
        }

        [NetworkInvocable(PacketDirection.Client)]
        private bool ServerCreateAccountCommand(LoginData data)
        {
            if (OnUserCreateAccount != null)
            {
                bool result = OnUserCreateAccount.Invoke(this, data, out string message);
                if (!result)
                {
                    OnUserFailAccountCreation?.Invoke(this, message);
                    NetworkInvoke(nameof(ClientFailAccountCreation), new string[] { message });
                    return false;
                }
            }
            return true;
        }

        [NetworkInvocable(PacketDirection.Server)]
        private void ClientFailAccountCreation(string reason)
        {
            OnUserFailAccountCreation?.Invoke(this, reason);
            Log.GlobalError("Failed to create account. Reason: " + reason);
        }

        [NetworkInvocable(PacketDirection.Server)]
        void ClientAddedRpc(int id, int ownerId)
        {
            Log.GlobalDebug($"New Remote Client added! ID: {id}, OwnerID: {ownerId}");
            NetworkUser user = new NetworkUser(id, ownerId);
            NetworkManager.AddNetworkObject(user);
        }

        [NetworkInvocable(PacketDirection.Server)]
        void ClientRemovedRpc(int id)
        {
            NetworkUser user = NetworkUser.Users.Where(x => x.NetworkID == id).FirstOrDefault();
            if(user == null)
            {
                return;
            }
            else
            {
                NetworkManager.RemoveNetworkObject(user);
            }
        }
    }
}