using SocketMessagingShared;
using SocketMessagingShared.CustomTypes;
using SocketNetworking;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocketMessagingTestClient
{
    public class Program
    {
        static MessagingClient MyClient;

        public static void Main(string[] args)
        {
            Log.OnLog += HandleNetworkLog;
            //Log.SetHiddenFlag(LogSeverity.Debug);
            MyClient = new MessagingClient();
            MyClient.InitLocalClient();
            MyClient.Connect("127.0.0.1", 7777, "");
            MyClient.ConnectionStateUpdated += MyClient_ConnectionStateUpdated;
            NetworkClient.ClientConnectionStateChanged += ClientFullyConnected;
            Thread console = new Thread(HandleConsole);
            console.Start();
        }

        private static void MyClient_ConnectionStateUpdated(ConnectionState obj)
        {
            if(obj != ConnectionState.Connected)
            {
                return;
            }
            //Do login
        }

        static void HandleConsole()
        {
            while (true)
            {
                string input = Console.ReadLine();
                string[] command = input.Split(' ');
                if(command.Length == 0)
                {
                    continue;
                }
                switch(command[0])
                {
                    case "login":
                        if(command.Length < 3)
                        {
                            WriteLineColor("Incorrect syntax. use: login <username> <password>", ConsoleColor.Red);
                        }
                        string username = command[1];
                        string password = command[2];
                        bool success = MyClient.ClientLogin(username, password);
                        if (success)
                        {
                            WriteLineColor("Login completed without errors, you are now logged in as: " + username, ConsoleColor.White);
                        }
                        break;
                    case "createaccount":
                        if (command.Length < 3)
                        {
                            WriteLineColor("Incorrect syntax. use: createaccount <username> <password>", ConsoleColor.Red);
                        }
                        string newUsername = command[1];
                        string newPassword = command[2];
                        bool creationSuccess = MyClient.ClientCreateAccount(newUsername, newPassword);
                        if (creationSuccess)
                        {
                            Console.WriteLine("Account created! Username: " + newUsername);
                        }
                        break;
                    case "channels":
                        foreach(NetworkChannel channel in MyClient.ClientChannelController.Channels)
                        {
                            Console.WriteLine($"Name: {channel.Name}, GUID: {channel.Description}, Description: {channel.Description}");
                        }
                        break;
                    case "createchannel":
                        break;
                    default:
                        WriteLineColor("Unkown Command: " + command[0], ConsoleColor.Red);
                        break;
                }
            }
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
