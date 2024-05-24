﻿using SocketMessagingShared.CustomTypes;
using SocketNetworking;
using SocketNetworking.PacketSystem;
using SocketNetworking.PacketSystem.TypeWrappers;
using SocketNetworking.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using Microsoft.SqlServer.Server;
using System.Security.Policy;

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

        /// <summary>
        /// Invoked when the client gets updated <see cref="NetworkChannel"/>s from the server.
        /// </summary>
        public event Action<List<NetworkChannel>> ClientReceiveChannels;

        SerializableList<NetworkChannel> _channels { get; set; } = new SerializableList<NetworkChannel>();

        /// <summary>
        /// Invoked when the Server has added a <see cref="NetworkChannel"/> and it is avilable on the clients.
        /// </summary>
        public event Action<NetworkChannel> ServerChannelCreated;

        /// <summary>
        /// Called on the Server when a <see cref="NetworkChannel"/> is being destroyed and removed from the clients.
        /// </summary>
        public event Action<NetworkChannel> ServerChannelDestroyed;

        /// <summary>
        /// Called on the server when: a message has been replicated accross the network. Called on the client when the message has been approved by the server.
        /// </summary>
        public event Action<NetworkChannel, NetworkMessage> MessageSent;

        /// <summary>
        /// Called on the Client only when a new message has been receieved from the server.
        /// </summary>
        public event Action<NetworkChannel, NetworkMessage> MessegeRecieved;

        public delegate bool ClientCreateChannelReciever(MessagingClient client, string name, string Descrption);
        public ClientCreateChannelReciever OnClientCreateChannel { get; set; } = null;

        public delegate bool ClientSendMessageRequestReciver(MessagingClient client, NetworkChannel target, NetworkMessage message);
        public ClientSendMessageRequestReciver OnClientMessageRequest { get; set; } = null;

        public List<NetworkChannel> Channels
        {
            get
            {
                return _channels.ToList();
            }
        }

        public MessagingClient LocalClient
        {
            get
            {
                if(NetworkManager.WhereAmI == ClientLocation.Remote)
                {
                    throw new InvalidOperationException("tried getting the LocalClient on the server!");
                }
                return _localClient;
            }
            set
            {
                if (NetworkManager.WhereAmI == ClientLocation.Remote)
                {
                    throw new InvalidOperationException("tried setting the LocalClient on the server!");
                }
                _localClient = value;
            }
        }

        private MessagingClient _localClient;

        public void ServerAddNetworkChannel(NetworkChannel channel)
        {
            if (_channels.Contains(channel))
            {
                return;
            }
            _channels.Add(channel);
            ServerSyncChannels();
        }

        public void ServerRemoveNetworkChannel(NetworkChannel channel)
        {
            if(!_channels.Contains(channel))
            {
                return;
            }
            _channels.Remove(channel);
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
                NetworkServer.NetworkInvokeOnAll(this, nameof(ClientGetChannelList), new object[] { _channels }, readyOnly: true);
            }
            else
            {
                target.NetworkInvoke(this, nameof(ClientGetChannelList), new object[] { _channels });
            }
        }

        [NetworkInvocable(PacketDirection.Server)]
        private void ClientGetChannelList(SerializableList<NetworkChannel> channels)
        {
            Log.GlobalInfo("Got new channel list from server. Channels: " + channels.Count);
            _channels = channels;
            ClientReceiveChannels?.Invoke(channels.ToList());
        }

        public bool ClientSendMessage(NetworkChannel target, NetworkMessage message)
        {
            if (target == null || message == null)
            {
                return false;
            }
            if (NetworkManager.WhereAmI != ClientLocation.Local)
            {
                return false;
            }
            bool result = NetworkManager.NetworkInvoke<bool>(this, LocalClient, nameof(ServerGetMessageRequest), new object[] { target, message });
            if (result)
            {
                MessageSent?.Invoke(target, message);
            }
            return result;
        }

        [NetworkInvocable(PacketDirection.Client)]
        private bool ServerGetMessageRequest(NetworkClient client, NetworkChannel target, NetworkMessage message)
        {
            if(OnClientMessageRequest != null)
            {
                MessagingClient mClient = (MessagingClient)client;
                bool result = OnClientMessageRequest(mClient, target, message);
                if (!result)
                {
                    return false;
                }
            }
            MessageSent?.Invoke(target, message);
            ServerSendMessage(target, message);
            return true;
        }

        public void ServerSendMessage(NetworkChannel channel, NetworkMessage message)
        {
            MessagingServer.NetworkInvokeOnAll(this, nameof(ClientGetMessageUpdate), new object[] { channel, message });
        }

        [NetworkInvocable(PacketDirection.Server)]
        private void ClientGetMessageUpdate(NetworkChannel target, NetworkMessage message)
        {
            NetworkChannel localTarget = _channels.Where(x => x.GUID == target.GUID).FirstOrDefault();
            if(localTarget == null)
            {
                Log.GlobalWarning("failed to find channel with GUID: " + target.GUID + "\n Client and server are out of sync, some messages have been lost.");
                _channels.OverwriteContained(ClientGetChannels());
                return;
            }
            int index = _channels.IndexOf(localTarget);
            _channels[index].NetworkMessages.Add(message);
            MessegeRecieved?.Invoke(target, message);
        }

        public List<NetworkChannel> ClientGetChannels()
        {
            return LocalClient.NetworkInvoke<SerializableList<NetworkChannel>>(nameof(ServerSendChannelsRequest), new object[] { }).ToList();
        }

        [NetworkInvocable(PacketDirection.Client)]
        private SerializableList<NetworkChannel> ServerSendChannelsRequest(NetworkClient client)
        {
            if (!client.Ready)
            {
                return new SerializableList<NetworkChannel>();
            }
            SerializableList<NetworkChannel> channels = new SerializableList<NetworkChannel>();
            channels.OverwriteContained(_channels);
            return channels;
        }

        public bool ClientCreateChannel(string name, string description, out NetworkChannel channel)
        {
            NetworkChannel createdChannel = NetworkManager.NetworkInvoke<NetworkChannel>(this, LocalClient, nameof(ServerGetChannelCreationRequest), new object[] { name, description });
            channel = createdChannel;
            if(createdChannel == null)
            {
                return false;
            }
            return true;
        }

        [NetworkInvocable(PacketDirection.Client)]
        private NetworkChannel ServerGetChannelCreationRequest(NetworkClient client, string name, string description)
        {
            if (OnClientCreateChannel != null)
            {
                MessagingClient mClient = (MessagingClient)client;
                bool result = OnClientCreateChannel(mClient, name, description);
                if (!result)
                {
                    return null;
                }
            }
            NetworkChannel channel = new NetworkChannel();
            channel.Name = name;
            channel.Description = description;
            channel.GUID = Guid.NewGuid().ToString();
            ServerAddNetworkChannel(channel);
            return channel;
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
