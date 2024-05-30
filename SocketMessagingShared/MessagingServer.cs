using SocketNetworking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocketMessagingShared.Components;

namespace SocketMessagingShared
{
    public class MessagingServer : NetworkServer
    {
        public static NetworkChannelController NetworkChannelController { get; private set; }

        public static void PrepareServer()
        {
            NetworkChannelController = new NetworkChannelController();
        }

        public new static void StartServer()
        {
            NetworkServer.StartServer();
        }
    }
}
