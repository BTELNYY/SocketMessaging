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

        public override string Filename => "meta" + ".json";

        public string PermanentID { get; set; } =  string.Empty;

        public string ChannelName { get; set; } = string.Empty; 

        public string Description { get; set; } = string.Empty;

        public Dictionary<string, PermissionData> GroupToPermissions { get; set; } = new Dictionary<string, PermissionData>();
    }
}
