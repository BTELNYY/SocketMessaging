using SocketNetworking;
using SocketNetworking.Attributes;
using SocketNetworking.PacketSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketMessagingShared
{
    public class MessagingUser : INetworkObject
    {
        public static List<MessagingUser> Users = new List<MessagingUser>();

        public MessagingUser() { }

        public MessagingUser(MessagingClient client)
        {
            Client = client;
            _id = NetworkManager.GetNextNetworkID();
        }

        public MessagingUser(int id)
        {
            _id = id;
        }

        private int _id = 0;

        public MessagingClient Client { get; private set; }

        public int NetworkID => _id;

        public bool IsEnabled => true;

        public string _username = string.Empty;

        public string Username
        {
            get
            {
                return _username;
            }
            set
            {
                if (NetworkManager.WhereAmI == ClientLocation.Remote)
                {
                    ServerSetUsername(value);
                    return;
                }
                ClientSetUsername(value);
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
            return NetworkManager.NetworkInvoke<bool>(this, Client, nameof(SetUsernameCommand), new object[] { username });
        }

        public void OnAdded(INetworkObject addedObject)
        {
            if(addedObject == this)
            {
                Users.Add(this);
            }
        }

        public void OnDisconnected(NetworkClient client)
        {
            
        }

        public void OnReady(NetworkClient client, bool isReady)
        {
            
        }

        public void OnRemoved(INetworkObject removedObject)
        {
            if(removedObject == this)
            {
                Users.Remove(this);
            }
        }
    }
}
