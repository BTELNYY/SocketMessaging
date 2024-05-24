using SocketNetworking.PacketSystem;
using SocketNetworking.PacketSystem.TypeWrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketMessagingShared.CustomTypes
{
    public class NetworkChannel : IPacketSerializable
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string GUID { get; set; } = string.Empty;

        public List<NetworkMessage> NetworkMessages { get; set; } = new List<NetworkMessage>();

        public int Deserialize(byte[] data)
        {
            ByteReader reader = new ByteReader(data);
            Name = reader.ReadString();
            Description = reader.ReadString();
            GUID = reader.ReadString();
            NetworkMessages = reader.Read<SerializableList<NetworkMessage>>().ToList();
            return reader.ReadBytes;
        }

        public int GetLength()
        {
            return Serialize().Length;
        }

        public byte[] Serialize()
        {
            ByteWriter writer = new ByteWriter();
            writer.WriteString(Name);
            writer.WriteString(Description);
            writer.WriteString(GUID);
            SerializableList<NetworkMessage> messages = new SerializableList<NetworkMessage>();
            messages.OverwriteContained(messages);
            writer.Write<SerializableList<NetworkMessage>>(messages);
            return writer.Data;
        }
    }
}
