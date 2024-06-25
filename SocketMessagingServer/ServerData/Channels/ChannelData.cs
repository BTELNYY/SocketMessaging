using Newtonsoft.Json;
using SocketMessagingServer.ServerData.Messages;
using SocketMessagingServer.ServerData.Permissions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketMessagingServer.ServerData.Channels
{
    public class ChannelData : ConfigFile
    {
        public override string Directory
        {
            get
            {
                return Path.Combine(DataManager.ChannelDataDirectory, PermanentID);
            }
        }

        [JsonIgnore]
        private List<MessageChunkFile> _messages = new List<MessageChunkFile>();

        [JsonIgnore]
        public List<MessageChunkFile> DiskChunks
        {
            get
            {
                List<MessageChunkFile> chunks = new List<MessageChunkFile>();
                foreach(string file in System.IO.Directory.GetFiles(Directory))
                {
                    if (file.Split(Path.PathSeparator).Last() == "meta.json")
                    {
                        continue;
                    }
                    MessageChunkFile chunk = DataManager.GetConfigItem<MessageChunkFile>(file);
                    if(chunk == null)
                    {
                        continue;
                    }
                    chunks.Insert(chunk.ChunkID, chunk);
                }
                _messages = chunks;
                return chunks;
            }
            set
            {
                CachedChunks = value;
                SaveChunks();
            }
        }

        [JsonIgnore]
        public List<MessageChunkFile> CachedChunks
        {
            get
            {
                return _messages;
            }
            set
            {
                _messages = value;
                foreach(MessageChunkFile chunk in _messages)
                {
                    chunk.ChannelUUID = PermanentID;
                }
            }
        }

        public void SaveChunks()
        {
            foreach(MessageChunkFile chunk in _messages)
            {
                DataManager.WriteConfigFile(chunk, true);
            }
        }

        public override string Filename => "meta.json";

        public string PermanentID { get; set; } =  string.Empty;

        public string ChannelName { get; set; } = string.Empty; 

        public string Description { get; set; } = string.Empty;

        public int LastChunk { get; set; } = 0;

        public Dictionary<string, PermissionData> GroupToPermissions { get; set; } = new Dictionary<string, PermissionData>();
    }
}
