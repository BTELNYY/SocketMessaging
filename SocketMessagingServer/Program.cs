using SocketMessagingServer.ServerData;
using SocketMessagingServer.ServerData.Users;
using SocketMessagingShared;
using SocketMessagingShared.CustomTypes;
using SocketNetworking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SocketMessagingServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.OnLog += HandleNetworkLog;
            Log.SetHiddenFlag(LogSeverity.Debug);
            DataManager.SyncLists();
            NetworkServer.ClientType = typeof(MessagingClient);
            NetworkServer.DefaultReady = false;
            NetworkServer.ClientConnected += ClientConnected;
            MessagingServer.StartServer();
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
