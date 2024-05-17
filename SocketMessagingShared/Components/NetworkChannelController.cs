using SocketNetworking;
using SocketNetworking.PacketSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketMessagingShared.Components
{
    public class NetworkChannelController : MessageObject
    {
        public NetworkChannelController() 
        {
            _netId = NetworkManager.GetNextNetworkID();
        }

        private int _netId = 0;

        public void SetNetID(int id)
        {
            _netId = id;
        }

        public override int NetworkID => _netId;

        public override OwnershipMode OwnershipMode { get => OwnershipMode.Server; }

        public override void OnConnected(NetworkClient client)
        {
            base.OnConnected(client);
            if(NetworkManager.WhereAmI != ClientLocation.Remote)
            {
                return;
            }
            MessagingClient msgClient = client as MessagingClient;
            if(msgClient == null)
            {
                return;
            }
            NetworkManager.NetworkInvoke(msgClient, msgClient, "ClientCreateChannelController", new object[] { NetworkID });
        }
    }
}
