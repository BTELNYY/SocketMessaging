﻿using Newtonsoft.Json;
using SocketMessagingServer.ServerData.Channels;
using SocketMessagingServer.ServerData.Groups;
using SocketMessagingServer.ServerData.Users;
using SocketMessagingShared.CustomTypes;
using SocketNetworking;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketMessagingServer.ServerData
{
    public class DataManager
    {

        public static string ChannelDataDirectory
        {
            get
            {
                string dir = "./data/channels/";
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                return dir;
            }
        }

        public static string GroupDataDirectory
        {
            get
            {
                string dir = "./data/groups/";
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                return dir;
            }
        }

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

        static Dictionary<string, string> GroupNameToGuid = new Dictionary<string, string>();

        static Dictionary<string, string> ChannelNameToGuidd = new Dictionary<string, string>();

        public static void SyncLists()
        {
            foreach (string dir in Directory.GetDirectories(UserDataDirectory))
            {
                UserProfile profile = GetProfile(dir.Split('/').Last());
                if (profile != null)
                {
                    UsernameToGuid.Add(profile.Username, profile.UUID);
                }
            }
            foreach (string channel in Directory.GetFiles(ChannelDataDirectory))
            {
                ChannelData dataChannel = new ChannelData();
                dataChannel.PermanentID = channel.Split('.')[0];
                ChannelData actualData = GetConfigItem<ChannelData>(dataChannel);
                if(actualData == null)
                {
                    Log.GlobalWarning("Channnel Data wasn't loaded correctly.");
                }
                ChannelNameToGuidd.Add(actualData.ChannelName, actualData.PermanentID);
            }
            foreach (string group in Directory.GetFiles(GroupDataDirectory))
            {
                GroupData fakeGroup = new GroupData();
                fakeGroup.PermanentID = group.Split('.')[0];
                GroupData actualGroup = GetConfigItem<GroupData>(fakeGroup);
                ChannelNameToGuidd.Add(actualGroup.Name, actualGroup.PermanentID);
            }
        }

        public static T GetConfigItem<T>(T item) where T : ConfigFile
        {
            if (!Directory.Exists(item.Directory))
            {
                return default(T);
            }
            string filePath = Path.Combine(item.Directory, item.Filename);
            if (File.Exists(filePath))
            {
                string data = File.ReadAllText(filePath);
                T thing = JsonConvert.DeserializeObject<T>(data);
                return thing;
            }
            else
            {
                return default(T);
            }
        }

        public static T GetConfigItem<T>(string path) where T : ConfigFile
        {
            if (File.Exists(path))
            {
                string data = File.ReadAllText(path);
                try
                {
                    T thing = JsonConvert.DeserializeObject<T>(data);
                    return thing;
                }
                catch(Exception ex)
                {
                    Log.GlobalError(ex.ToString());
                    return null;
                }
            }
            else
            {
                return default(T);
            }
        }

        public static T WriteConfigFile<T>(T item, bool allowOverwrite = false) where T : ConfigFile
        {
            if (!Directory.Exists(item.Directory))
            {
                Directory.CreateDirectory(item.Directory);
            }
            string text = JsonConvert.SerializeObject(item);
            Log.GlobalDebug(text);
            string path = Path.Combine(item.Directory, item.Filename);
            if (File.Exists(path) && !allowOverwrite)
            {
                T thing = JsonConvert.DeserializeObject<T>(path);
                return thing;
            }
            if (allowOverwrite)
            {
                File.Delete(path);
            }
            if (!File.Exists(path))
            {
                File.WriteAllText(path, text);
            }
            return item;
        }

        public static bool DeleteConfigItem<T>(T item) where T : ConfigFile
        {
            if (!Directory.Exists(item.Directory))
            {
                return false;
            }
            string filePath = Path.Combine(item.Directory, item.Filename);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void WriteProfileData(UserProfile profile)
        {
            string dir = UserDataDirectory + profile.UUID;
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
            profile.UUID = Guid.NewGuid().ToString();
            if (UsernameToGuid.ContainsKey(username))
            {
                result = "Username is already taken.";
                return false;
            }
            WriteProfileData(profile);
            UsernameToGuid.Add(profile.Username, profile.UUID);
            result = string.Empty;
            return true;
        }

        public static List<NetworkChannel> GetNetworkChannels()
        {
            List<string> dirs = Directory.GetDirectories(ChannelDataDirectory).ToList();
            List<NetworkChannel> Channels = new List<NetworkChannel>();
            foreach (string dir in dirs)
            {
                ChannelData data = new ChannelData();
                string permId = dir.Split('/').Last().Trim('/');
                data.PermanentID = permId;
                ChannelData actualData = GetConfigItem(data);
                NetworkChannel netChannel = new NetworkChannel();
                netChannel.UUID = permId;
                netChannel.Description = actualData.Description;
                netChannel.Name = actualData.ChannelName;
                Channels.Add(netChannel);
            }
            return Channels;
        }
    }
}
