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

        private List<MessageFile> _messages = new List<MessageFile>();

        public List<MessageFile> DiskChunks
        {
            get
            {
                List<MessageFile> chunks = new List<MessageFile>();
                foreach(string file in System.IO.Directory.GetFiles(Directory))
                {
                    MessageFile chunk = DataManager.GetConfigItem<MessageFile>(file);
                    if(chunk == null)
                    {
                        continue;
                    }
                    chunks.Insert(chunk.ChunkID, chunk);
                }
                _messages = chunks;
                return chunks;
            }
        }

        public List<MessageFile> CachedChunks
        {
            get
            {
                return _messages;
            }
        }


        public override string Filename => "meta" + ".json";

        public string PermanentID { get; set; } =  string.Empty;

        public string ChannelName { get; set; } = string.Empty; 

        public string Description { get; set; } = string.Empty;

        public Dictionary<string, PermissionData> GroupToPermissions { get; set; } = new Dictionary<string, PermissionData>();
    }
}
