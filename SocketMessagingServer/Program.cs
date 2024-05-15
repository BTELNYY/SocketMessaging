using SocketMessagingServer.ServerData;
using SocketMessagingShared;
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
            UserdataManager.SyncUsersToList();
            NetworkServer.ClientType = typeof(MessagingClient);
            NetworkServer.DefaultReady = false;
            NetworkServer.StartServer();
            NetworkServer.ClientConnected += ClientConnected;
        }

        private static void ClientConnected(int obj)
        {
            MessagingClient client = (MessagingClient)NetworkServer.GetClient(obj);
            client.EventHandler = new CustomServerSideClientEventHandler();
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
