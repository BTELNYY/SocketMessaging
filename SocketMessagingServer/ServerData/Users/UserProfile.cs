using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketMessagingServer.ServerData.Users
{
    public class UserProfile : ConfigFile
    {
        public override string Filename => "profile.json";

        public override string Directory 
        {
            get
            {
                return Path.Combine(DataManager.UserDataDirectory, UUID);
            }
        }

        public string UUID { get; set; } = string.Empty;

        public string Username { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public List<string> GroupMemberships { get; set; } = new List<string>();
    }
}
