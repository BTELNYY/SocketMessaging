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

        public static void CreateProfile(string username, string password)
        {
            UserProfile profile = new UserProfile();
            profile.Username = username;
            profile.PasswordHash = password;
            profile.PermanentID = Guid.NewGuid().ToString();
            WriteProfileData(profile);
        }
    }
}
