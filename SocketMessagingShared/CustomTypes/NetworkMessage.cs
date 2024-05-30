using SocketNetworking.PacketSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketMessagingShared.CustomTypes
{
    public class NetworkMessage : IPacketSerializable
    {
        public string Content { get; set; } = string.Empty;

        public string AuthorUUID { get; set; } = string.Empty;

        public string AuthorName { get; set; } = string.Empty;

        public long Timestamp { get; set; } = 0;

        public virtual int Deserialize(byte[] data)
        {
            ByteReader reader = new ByteReader(data);
            Content = reader.ReadString();
            AuthorUUID = reader.ReadString();
            AuthorName = reader.ReadString();
            Timestamp = reader.ReadLong();
            return reader.ReadBytes;
        }

        public virtual int GetLength()
        {
            return Serialize().Length;
        }

        public virtual byte[] Serialize()
        {
            ByteWriter writer = new ByteWriter();
            writer.WriteString(Content);
            writer.WriteString(AuthorUUID);
            writer.WriteString(AuthorName);
            writer.WriteLong(Timestamp);
            return writer.Data;
        }
    }
}
