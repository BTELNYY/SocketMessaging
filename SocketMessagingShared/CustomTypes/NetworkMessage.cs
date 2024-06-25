using SocketNetworking.PacketSystem;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketMessagingShared.CustomTypes
{
    public class NetworkMessage : IPacketSerializable, IEqualityComparer<NetworkMessage>
    {
        public string Content { get; set; } = string.Empty;

        public string AuthorUUID { get; set; } = string.Empty;

        public string AuthorName { get; set; } = string.Empty;

        public byte[] MessageColorRGBA { get; set; } = new byte[4];

        public long Timestamp { get; set; } = 0;

        public virtual int Deserialize(byte[] data)
        {
            ByteReader reader = new ByteReader(data);
            Content = reader.ReadString();
            AuthorUUID = reader.ReadString();
            AuthorName = reader.ReadString();
            MessageColorRGBA = reader.ReadByteArray();
            Timestamp = reader.ReadLong();
            return reader.ReadBytes;
        }

        public bool Equals(NetworkMessage x, NetworkMessage y)
        {
            if(x == null || y == null) return false;
            if(x.GetHashCode() != y.GetHashCode()) return false;
            return true;
        }

        public int GetHashCode(NetworkMessage obj)
        {
            int hashCode = obj.Content.GetHashCode() + (int)obj.Timestamp + obj.AuthorUUID.GetHashCode();
            return hashCode;
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
            writer.WriteByteArray(MessageColorRGBA);
            writer.WriteLong(Timestamp);
            return writer.Data;
        }

        public override string ToString()
        {
            return $"{AuthorName}: {Content}";
        }
    }
}
