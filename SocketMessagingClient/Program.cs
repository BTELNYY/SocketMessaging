using SocketNetworking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SocketMessagingShared.Components;
using SocketMessagingShared;
using System.Runtime.InteropServices;


namespace SocketMessagingClient
{
    internal static class Program
    {
        public static MessagingClient MyClient;

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
#if DEBUG
            AllocConsole();
#endif
            Log.OnLog += HandleNetworkLog;
            MyClient = new MessagingClient();
            MyClient.InitLocalClient();
            MyClient.ConnectionStateUpdated += HandleLogin;
            Application.Run(new Connection());
            NetworkClient.ClientCreated += ClientCreated;
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

        private static void HandleLogin(ConnectionState obj)
        {
            if (obj != ConnectionState.Connected)
            {
                return;
            }
            else 
            {

            }
            
        }

        private static void ClientCreated(NetworkClient obj)
        {
            
        }
    }
}
