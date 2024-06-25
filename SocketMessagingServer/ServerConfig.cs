using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketMessagingServer
{
    public class ServerConfig
    {
        public static ServerConfig Instance;

        public int MaxChunksToLoadPerChannel = 2;
    }
}
