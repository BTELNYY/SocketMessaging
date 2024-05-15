using SocketNetworking;
using SocketNetworking.Attributes;
using System;
using System.Collections.Generic;
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
        private void PerformLoginCommand(LoginData data)
        {
            Log.GlobalDebug(data.ToString());
            User.ServerSetUsername(data.Username);
            Ready = true;
            return;
        }

        public void ClientLogin(string username, string password) 
        {
            LoginData loginData = new LoginData();
            loginData.Username = username;
            loginData.SetPassword(password);
            NetworkInvoke(this, nameof(PerformLoginCommand), new object[] { loginData });
        }

        [NetworkInvocable(PacketDirection.Server)]
        private void ClientAddedRpc(int id)
        {
            MessagingUser user = new MessagingUser(id);
            NetworkManager.AddNetworkObject(user);
            Log.GlobalDebug($"Client added rpc {id}");
            ClientLogin("Username", "Password");
        }

        public void ServerNotifyClientAdded(MessagingUser user)
        {
            NetworkInvoke(nameof(ClientAddedRpc), new object[] { user.NetworkID });
        }
    }
}