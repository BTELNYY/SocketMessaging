using SocketNetworking;
using SocketNetworking.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace SocketMessagingShared
{
    public class MessagingClient : NetworkClient
    {
        public override void Init()
        {
            base.Init();
            this.ConnectionStateUpdated += CreateUserObject;
        }

        public ClientEventHandler EventHandler { get; set; } = new ClientEventHandler();

        private void CreateUserObject(ConnectionState obj)
        {
            if(obj != ConnectionState.Connected)
            {
                return;
            }
            _user = new MessagingUser(this);
            NetworkManager.AddNetworkObject(_user);
            if(CurrnetClientLocation == ClientLocation.Remote)
            {
                ServerNotifyClientAdded(_user);
            }
        }

        private MessagingUser _user;

        public MessagingUser User 
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

        public bool ClientCreateAccount(string username, string password)
        {
            LoginData loginData = new LoginData();
            loginData.Username = username;
            loginData.SetPassword(password);
            return NetworkInvoke<bool>(nameof(ServerCreateAccountCommand), new object[] { loginData });
        }

        public void ServerNotifyClientAdded(MessagingUser user)
        {
            NetworkInvoke(nameof(ClientAddedRpc), new object[] { user.NetworkID, user.OwnerClientID });
        }

        public void ServerNotifyClientRemoved(MessagingUser user)
        {
            NetworkInvoke(nameof(ClientRemovedRpc), new object[] { user.NetworkID });
        }

        [NetworkInvocable(PacketDirection.Client)]
        private bool ServerPerformLoginCommand(LoginData data)
        {
            if (!EventHandler.ValidateLogin(data, out string reason))
            {
                Log.GlobalInfo($"Rejecting client login from {ClientID} becuase: {reason}");
                NetworkInvoke(this, nameof(ClientLoginFail), new string[] { reason });
                return false;
            }
            Log.GlobalInfo($"User '{data.Username}' logged in with ClientID {ClientID}");
            User.ServerSetUsername(data.Username);
            Ready = true;
            return true;
        }

        [NetworkInvocable(PacketDirection.Server)]
        private void ClientLoginFail(string reason)
        {
            Log.GlobalError("Login failed: " + reason);
            EventHandler.ClientFailLogin(reason);
        }

        [NetworkInvocable(PacketDirection.Client)]
        private bool ServerCreateAccountCommand(LoginData data)
        {
            if(!EventHandler.ServerCreateAccount(data.Username, data.PasswordHash, out string reason))
            {
                NetworkInvoke(nameof(ClientFailAccountCreation), new object[] { reason });
                return false;
            }
            return true;
        }

        [NetworkInvocable(PacketDirection.Server)]
        private void ClientFailAccountCreation(string reason)
        {
            Log.GlobalError("Failed to create account. Reason: " + reason);
            EventHandler.ClientFailCreatingNewAccount(reason);
        }

        [NetworkInvocable(PacketDirection.Server)]
        void ClientAddedRpc(int id, int ownerId)
        {
            MessagingUser user = new MessagingUser(id, ownerId);
            NetworkManager.AddNetworkObject(user);
        }

        [NetworkInvocable(PacketDirection.Server)]
        void ClientRemovedRpc(int id)
        {
            MessagingUser user = MessagingUser.Users.Where(x => x.NetworkID == id).FirstOrDefault();
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