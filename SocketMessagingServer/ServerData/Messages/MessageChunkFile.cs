using SocketMessagingShared.CustomTypes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketMessagingServer.ServerData.Messages
{
    public class MessageChunkFile : ConfigFile
    {
        public override string Directory => Path.Combine(DataManager.ChannelDataDirectory, Channel);

        public override string Filename => $"chunk-{ChunkID}.json";

        public string Channel { get; set; } = string.Empty;

        public int ChunkID { get; set; } = 0;

        public List<NetworkMessage> Messages { get; set; } = new List<NetworkMessage>();
    }
}
