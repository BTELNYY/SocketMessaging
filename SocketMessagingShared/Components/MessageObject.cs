using SocketNetworking;
using SocketNetworking.PacketSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketMessagingShared.Components
{
    public class MessageObject : INetworkObject
    {
        public virtual int OwnerClientID { get; set; }

        public virtual OwnershipMode OwnershipMode { get; set; }

        public virtual int NetworkID => 0;

        public virtual bool IsEnabled => true;

        public void EnsureRegistered(bool modifyNetworId = true)
        {
            if (NetworkManager.IsRegistered(this))
            {
                if (modifyNetworId)
                {
                    NetworkManager.ModifyNetworkID(this);
                }
                return;
            }
            NetworkManager.AddNetworkObject(this);
        }

        ~MessageObject()
        {
            if(NetworkManager.IsRegistered(this))
            {
                NetworkManager.RemoveNetworkObject(this);
            }
        }

        public virtual void OnAdded(INetworkObject addedObject)
        {

        }

        public virtual void OnConnected(NetworkClient client)
        {
            
        }

        public virtual void OnDisconnected(NetworkClient client)
        {

        }

        public virtual void OnReady(NetworkClient client, bool isReady)
        {

        }

        public virtual void OnRemoved(INetworkObject removedObject)
        {
            
        }
    }
}
