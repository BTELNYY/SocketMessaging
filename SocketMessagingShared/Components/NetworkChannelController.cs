using SocketMessagingShared.CustomTypes;
using SocketNetworking;
using SocketNetworking.PacketSystem;
using SocketNetworking.PacketSystem.TypeWrappers;
using SocketNetworking.Attributes;
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

        public delegate void NewChannelReceiver(List<NetworkChannel> channels);

        public NewChannelReceiver OnClientGetNewChannels { get; set; } = null;

        SerializableList<NetworkChannel> Channels { get; set; } = new SerializableList<NetworkChannel>();

        public void ServerAddNetworkChannel(NetworkChannel channel)
        {
            if (Channels.Contains(channel))
            {
                return;
            }
            Channels.Add(channel);
            ServerSyncChannels();
        }

        public void ServerRemoveNetworkChannel(NetworkChannel channel)
        {
            if(!Channels.Contains(channel))
            {
                return;
            }
            Channels.Remove(channel);
            ServerSyncChannels();
        }

        public void ServerSyncChannels(NetworkClient target = null)
        {
            if (!NetworkServer.Active)
            {
                return;
            }
            if(target == null)
            {
                NetworkServer.NetworkInvokeOnAll(this, nameof(ClientGetChannelList), new object[] { Channels }, readyOnly: true);
            }
            else
            {
                target.NetworkInvoke(this, nameof(ClientGetChannelList), new object[] { Channels });
            }
        }

        [NetworkInvocable(PacketDirection.Server)]
        private void ClientGetChannelList(SerializableList<NetworkChannel> channels)
        {
            Log.GlobalInfo("Got new channel list from server. Channels: " + channels.Count);
            Channels = channels;
            OnClientGetNewChannels?.Invoke(channels.ToList());
        }

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

        public override void OnReady(NetworkClient client, bool isReady)
        {
            base.OnReady(client, isReady);
            if(NetworkManager.WhereAmI != ClientLocation.Remote)
            {
                return;
            }
            if (isReady)
            {
                ServerSyncChannels(client);
            }
        }
    }
}
