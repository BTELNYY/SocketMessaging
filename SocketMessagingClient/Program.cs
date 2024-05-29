using SocketNetworking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SocketMessagingShared.Components;
using SocketMessagingShared;


namespace SocketMessagingClient
{
    internal static class Program
    {
        public static MessagingClient MyClinet;


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MyClinet = new MessagingClient();
            MyClinet.InitLocalClient();
            MyClinet.ConnectionStateUpdated += HandleLogin;
            Application.Run(new Connection());
            NetworkClient.ClientCreated += ClientCreated;
        }

        private static void HandleLogin(ConnectionState obj)
        {
            if(obj != ConnectionState.Connected)
            {
                return;
            }
            Application.Run(new Login());
        }

        private static void ClientCreated(NetworkClient obj)
        {
            
        }
    }
}
