using SocketMessagingServer.ServerData;
using SocketMessagingServer.ServerData.Channels;
using SocketMessagingServer.ServerData.Messages;
using SocketMessagingServer.ServerData.Users;
using SocketMessagingShared;
using SocketMessagingShared.CustomTypes;
using SocketNetworking;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocketMessagingServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.OnLog += HandleNetworkLog;

            Log.SetHiddenFlag(LogSeverity.Debug);
#if DEBUG
            Log.RemoveHiddenFlag(LogSeverity.Debug);
#endif
            DataManager.SyncLists();
            NetworkServer.ClientType = typeof(MessagingClient);
            NetworkServer.DefaultReady = false;
            NetworkServer.ClientConnected += ClientConnected;
            //TODO: Change to read file.
            ServerConfig.Instance = new ServerConfig();
            MessagingServer.PrepareServer();
            foreach (string dir in Directory.GetDirectories(DataManager.ChannelDataDirectory))
            {
                string guid = dir.Split('/').Last().Trim('/');
                ChannelData data = new ChannelData();
                data.PermanentID = guid;
                ChannelData actualData = DataManager.GetConfigItem(data);
                if(actualData == null)
                {
                    Log.GlobalError("Failed to load Channel! UUID: " + guid);
                    continue;
                }
                NetworkChannel netChannel = new NetworkChannel();
                netChannel.UUID = guid;
                netChannel.Name = actualData.ChannelName;
                netChannel.Description = actualData.Description;
                foreach (NetworkMessage message in actualData.DiskChunks.Last().Messages)
                {
                    netChannel.NetworkMessages.Add(message);
                }
                MessagingServer.NetworkChannelController.ServerAddNetworkChannel(netChannel);
            }
            MessagingServer.StartServer();
            Thread console = new Thread(HandleConsole);
            console.Start();
        }

        private static void HandleConsole()
        {
            while (true)
            {
                string input = Console.ReadLine();
                string[] inputparsed = input.Split(' ');
                switch(inputparsed[0])
                {
                    case "createchannel":
                        if(inputparsed.Length < 3) 
                        {
                            WriteLineColor("Incorrect Syntax! Use: createchannel <channel> <description>", ConsoleColor.Red);
                            break;
                        }
                        string name = inputparsed[1];
                        string description = inputparsed[2];
                        NetworkChannel channel = new NetworkChannel();
                        channel.Name = name;
                        channel.Description = description;
                        channel.UUID = Guid.NewGuid().ToString();
                        ChannelData data = new ChannelData();
                        data.ChannelName = name;
                        data.Description = description;
                        data.PermanentID = channel.UUID;
                        DataManager.WriteConfigFile(data);
                        MessagingServer.NetworkChannelController.ServerAddNetworkChannel(channel);
                        Console.WriteLine("Done!");
                        break;
                    case "sendmessage":
                        if(inputparsed.Length < 3)
                        {
                            WriteLineColor("Invalid Syntax. Use: sendmessage <channelname> <message>", ConsoleColor.Red);
                            break;
                        }
                        break;
                    default:
                        WriteLineColor($"No such command '{inputparsed[0]}'", ConsoleColor.Red);
                        break;
                }
            }
        }

        private static void ClientConnected(int obj)
        {
            MessagingClient client = (MessagingClient)NetworkServer.GetClient(obj);
            client.OnValidateLogin = ValidateLogin;
            client.OnUserCreateAccount = CreateAccount;
        }

        private static bool ValidateLogin(MessagingClient client, LoginData data, out string reason)
        {
            UserProfile profile = DataManager.GetProfileByUsername(data.Username);
            if (profile == null)
            {
                reason = "Username or password is incorrect.";
                return false;
            }
            if (profile.PasswordHash != data.PasswordHash)
            {
                reason = "Username or password is incorrect.";
                return false;
            }
            client.User.UUID = profile.UUID;
            client.User.Username = profile.Username;
            reason = string.Empty;
            return true;
        }
        
        private static bool CreateAccount(MessagingClient client, LoginData data, out string reason)
        {
            return DataManager.CreateProfile(data.Username, data.PasswordHash, out reason);
        }

        private static void HandleNetworkLog(LogData data)
        {
            ConsoleColor color = ConsoleColor.White;
            switch (data.Severity)
            {
                case LogSeverity.Debug:
                    color = ConsoleColor.Gray;
                    break;
                case LogSeverity.Info:
                    color = ConsoleColor.White;
                    break;
                case LogSeverity.Warning:
                    color = ConsoleColor.Yellow;
                    break;
                case LogSeverity.Error:
                    color = ConsoleColor.Red;
                    break;
            }
            WriteLineColor(data.Message, color);
        }

        public static void WriteLineColor(string msg, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(msg);
            Console.ResetColor();
        }
    }
}
