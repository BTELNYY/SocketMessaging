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
    public class MessagingUser : MessageObject
    {
        public static List<MessagingUser> Users = new List<MessagingUser>();

        public MessagingUser() { }

        public MessagingUser(MessagingClient client)
        {
            Client = client;
            _id = NetworkManager.GetNextNetworkID();
            _ownerId = Client.ClientID;
        }

        public MessagingUser(int id)
        {
            _id = id;
        }

        public MessagingUser(int id, int ownerId)
        {
            _id = id;
            _ownerId = ownerId;
        }

        private int _id = 0;

        private int _ownerId = 0;

        public override int OwnerClientID
        {
            get
            {
                return _ownerId;
            }
            set
            {
                if(NetworkManager.WhereAmI != ClientLocation.Remote)
                {
                    return;
                }
                _ownerId = value;
                NetworkServer.NetworkInvokeOnAll(this, nameof(SetOwnerIdRPC), new object[] { value });
            }
        }

        [NetworkInvocable(PacketDirection.Server)]
        private void SetOwnerIdRPC(int owner)
        {
            _ownerId = owner;
        }

        public override OwnershipMode OwnershipMode
        {
            get
            {
                return OwnershipMode.Client;
            }
            set
            {
                throw new InvalidOperationException("Only clients may own this object.");
            }
        }

        public override int NetworkID => _id;

        public override bool IsEnabled => true;

        public MessagingClient Client { get; private set; }

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

        public override void OnDisconnected(NetworkClient client)
        {
            base.OnDisconnected(client);
            if(client.ClientID == OwnerClientID)
            {
                
            }
        }

        public override void OnAdded(INetworkObject addedObject)
        {
            if(addedObject == this)
            {
                Users.Add(this);
            }
        }

        public override void OnRemoved(INetworkObject removedObject)
        {
            if(removedObject == this)
            {
                Users.Remove(this);
            }
        }
    }
}
