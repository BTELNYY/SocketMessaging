using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketMessagingServer.ServerData.Users
{
    public class UserProfile
    {
        public string PermanentID { get; set; } = string.Empty;

        public string Username { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public List<string> GroupMemberships { get; set; } = new List<string>();
    }
}
