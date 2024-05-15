using SocketNetworking;
using SocketNetworking.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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

        [NetworkInvocable(PacketDirection.Client)]
        private void ServerPerformLoginCommand(LoginData data)
        {
            if (!EventHandler.ValidateLogin(data, out string reason))
            {
                NetworkInvoke(this, nameof(ClientLoginFail), new string[] { reason });
                return;
            }
            User.ServerSetUsername(data.Username);
            Ready = true;
            return;
        }

        [NetworkInvocable(PacketDirection.Server)]
        private void ClientLoginFail(string reason)
        {
            Log.GlobalError("Login failed: " + reason);
        }

        public void ClientLogin(string username, string password) 
        {
            LoginData loginData = new LoginData();
            loginData.Username = username;
            loginData.SetPassword(password);
            NetworkInvoke(this, nameof(ServerPerformLoginCommand), new object[] { loginData });
        }

        [NetworkInvocable(PacketDirection.Server)]
        void ClientAddedRpc(int id, int ownerId)
        {
            MessagingUser user = new MessagingUser(id, ownerId);
            NetworkManager.AddNetworkObject(user);
            Log.GlobalDebug($"Client added rpc {id}");
            ClientLogin("Username", "Password");
        }

        public void ServerNotifyClientAdded(MessagingUser user)
        {
            NetworkInvoke(nameof(ClientAddedRpc), new object[] { user.NetworkID, user.OwnerClientID });
        }
    }
}