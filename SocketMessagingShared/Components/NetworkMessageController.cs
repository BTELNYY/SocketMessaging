using SocketNetworking.PacketSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace SocketMessagingShared.Components
{
    public class NetworkMessageController : MessageObject
    {
        public override OwnershipMode OwnershipMode { get => OwnershipMode.Server; }

        
    }
}
