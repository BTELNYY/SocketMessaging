using SocketMessagingShared.CustomTypes;
using System.Collections.Generic;
using System.IO;

namespace SocketMessagingServer.ServerData.Messages
{
    public class MessageChunkFile : ConfigFile
    {
        public override string Directory => Path.Combine(DataManager.ChannelDataDirectory, ChannelUUID);

        public override string Filename => $"chunk-{ChunkID}.json";

        public string ChannelUUID { get; set; } = string.Empty;

        public int ChunkID { get; set; } = 0;

        public List<NetworkMessage> Messages { get; set; } = new List<NetworkMessage>();
    }
}
