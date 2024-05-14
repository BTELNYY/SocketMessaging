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
        public string _username = string.Empty;

        public string Username
        {
            get
            {
                return _username;
            }
            set
            {
                if(NetworkManager.WhereAmI == ClientLocation.Remote)
                {
                    ServerSetUsername(value);
                    return;
                }
            }
        }

        [NetworkInvocable(PacketDirection.Server)]
        private void SetUsernameRPC(string username)
        {
            _username = username;
        }

        public void ServerSetUsername(string username)
        {
            NetworkServer.NetworkInvokeOnAll(this, nameof(SetUsernameRPC), new object[] { username });
        }

        [NetworkInvocable(PacketDirection.Client)]
        private bool SetUsernameCommand(string username)
        {
            Username = username;
            return true;
        }

        public bool ClientSetUsername(string username)
        {
            return NetworkInvoke<bool>(nameof(SetUsernameCommand), new object[] { username });
        }

        [NetworkInvocable(PacketDirection.Client)]
        private bool PerformLoginCommand(LoginData data)
        {
            ServerSetUsername(data.Username);
            return true;
        }

        public bool ClientLogin(string username, string password) 
        {
            LoginData loginData = new LoginData();
            loginData.Username = username;
            loginData.SetPassword(password);
            return NetworkInvoke<bool>(nameof(PerformLoginCommand), new object[] { loginData });
        }
    }
}
