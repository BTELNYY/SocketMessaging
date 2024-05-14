using SocketMessagingShared;
using SocketNetworking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SocketMessagingServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            NetworkServer.ClientType = typeof(MessagingClient);
            NetworkServer.DefaultReady = false;
        }
    }
}
