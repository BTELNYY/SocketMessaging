using SocketMessagingServer.ServerData;
using SocketMessagingServer.ServerData.Channels;
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
            //Log.SetHiddenFlag(LogSeverity.Debug);
            DataManager.SyncLists();
            NetworkServer.ClientType = typeof(MessagingClient);
            NetworkServer.DefaultReady = false;
            NetworkServer.ClientConnected += ClientConnected;
            MessagingServer.PrepareServer();
            foreach(string dir in Directory.GetDirectories(DataManager.ChannelDataDirectory))
            {
                string guid = dir.Split(',').Last().Trim('\\');
                ChannelData data = new ChannelData();
                data.PermanentID = guid;
                ChannelData actualData = DataManager.GetConfigItem(data);
                NetworkChannel netChannel = new NetworkChannel();
                netChannel.GUID = guid;
                netChannel.Name = data.ChannelName;
                netChannel.Description = actualData.Description;
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
                        channel.GUID = Guid.NewGuid().ToString();
                        MessagingServer.NetworkChannelController.ServerAddNetworkChannel(channel);
                        Console.WriteLine("Done!");
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
