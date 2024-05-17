using SocketMessagingServer.ServerData.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketMessagingServer.ServerData.Groups
{
    public class GroupData : ConfigFile
    {
        public override string Directory => DataManager.GroupDataDirectory;

        public override string Filename => PermanentID + ".json";

        public string PermanentID { get; set; } = string.Empty;

        public string Name { get; set; }

        public PermissionData Permissions { get; set; } = new PermissionData();
    }
}
