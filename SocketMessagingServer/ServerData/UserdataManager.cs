using Newtonsoft.Json;
using SocketMessagingServer.ServerData.Users;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketMessagingServer.ServerData
{
    public class UserdataManager
    {
        public static string UserDataDirectory
        {
            get
            {
                string dir = "./data/users/";
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                return dir;
            }
        }

        static Dictionary<string, string> UsernameToGuid = new Dictionary<string, string>();

        public static void SyncUsersToList()
        {
            foreach(string dir in Directory.GetDirectories(UserDataDirectory))
            {
                UserProfile profile = GetProfile(dir.Split('/').Last());
                if (profile != null)
                {
                    UsernameToGuid.Add(profile.Username, profile.PermanentID);
                }
            }
        }

        public static void WriteProfileData(UserProfile profile)
        {
            string dir = UserDataDirectory + profile.PermanentID;
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            string file = Path.Combine(dir, "profile.json");
            string text = JsonConvert.SerializeObject(profile);
            if (!File.Exists(file))
            {
                File.WriteAllText(file, text);
            }
            else
            {
                File.Delete(file);
                File.WriteAllText(file, text);
            }
        }

        public static UserProfile GetProfileByUsername(string username)
        {
            if (UsernameToGuid.ContainsKey(username))
            {
                return GetProfile(UsernameToGuid[username]);
            }
            else
            {
                return null;
            }
        }

        public static UserProfile GetProfile(string userid)
        {
            string dir = UserDataDirectory + userid;
            if(!Directory.Exists(dir))
            {
                return null;
            }
            string file = Path.Combine(dir, "profile.json");
            if (!File.Exists(file))
            {
                return null;
            }
            string text = File.ReadAllText(file);
            UserProfile profile = JsonConvert.DeserializeObject<UserProfile>(text);
            return profile;
        }

        public static bool CreateProfile(string username, string password, out string result)
        {
            UserProfile profile = new UserProfile();
            profile.Username = username;
            profile.PasswordHash = password;
            profile.PermanentID = Guid.NewGuid().ToString();
            if (UsernameToGuid.ContainsKey(username))
            {
                result = "Username is already taken.";
                return false;
            }
            WriteProfileData(profile);
            result = string.Empty;
            return true;
        }
    }
}
