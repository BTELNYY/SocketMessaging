using SocketNetworking.PacketSystem;
using SocketNetworking.PacketSystem.TypeWrappers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketMessagingShared.CustomTypes
{
    public class NetworkChannel : IPacketSerializable
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string UUID { get; set; } = string.Empty;

        public List<NetworkMessage> NetworkMessages { get; set; } = new List<NetworkMessage>();

        public int Deserialize(byte[] data)
        {
            ByteReader reader = new ByteReader(data);
            Name = reader.ReadString();
            Description = reader.ReadString();
            UUID = reader.ReadString();
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
            writer.WriteString(UUID);
            List<NetworkMessage> trimmed = NetworkMessages.Skip(Math.Max(0, NetworkMessages.Count() - 50)).ToList();
            SerializableList<NetworkMessage> messages = new SerializableList<NetworkMessage>();
            messages.OverwriteContained(trimmed);
            writer.Write<SerializableList<NetworkMessage>>(messages);
            return writer.Data;
        }
    }
}
