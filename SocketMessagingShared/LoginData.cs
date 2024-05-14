using SocketNetworking;
using SocketNetworking.PacketSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketMessagingShared
{
    public struct LoginData : IPacketSerializable
    {
        public string Username;

        public string PasswordHash;

        public void SetPassword(string password)
        {
            PasswordHash = password.GetStringHash();
        }

        public int Deserialize(byte[] data)
        {
            ByteReader reader = new ByteReader(data);
            Username = reader.ReadString();
            PasswordHash = reader.ReadString();
            return reader.ReadBytes;
        }

        public int GetLength()
        {
            return Username.SerializedSize() + PasswordHash.SerializedSize();
        }

        public byte[] Serialize()
        {
            ByteWriter writer = new ByteWriter();
            writer.WriteString(Username);
            writer.WriteString(PasswordHash);
            return writer.Data;
        }
    }
}
