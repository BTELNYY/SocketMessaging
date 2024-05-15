using SocketMessagingShared;
using SocketNetworking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SocketMessagingTestClient
{
    public class Program
    {
        static MessagingClient MyClient;

        public static void Main(string[] args)
        {
            Log.OnLog += HandleNetworkLog;
            Log.SetHiddenFlag(LogSeverity.Debug);
            MyClient = new MessagingClient();
            MyClient.InitLocalClient();
            MyClient.Connect("127.0.0.1", 7777, "");
            NetworkClient.ClientConnectionStateChanged += ClientFullyConnected;
            MyClient.EventHandler = new CustomClientEventHandler();
        }

        private static void ClientFullyConnected(NetworkClient obj)
        {
            if(obj.CurrentConnectionState != ConnectionState.Connected)
            {
                return;
            }
            if(obj != MyClient)
            {
                return;
            }
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
