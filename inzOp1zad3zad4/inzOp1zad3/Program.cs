using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace inzOp1zad3
{
    class Program
    {
        private static Object thisLock = new Object();


        static void Main(string[] args)
        { 
            ThreadPool.QueueUserWorkItem(ServerThread);
            ThreadPool.QueueUserWorkItem(ClientThread);
            ThreadPool.QueueUserWorkItem(ClientThread);
            Console.ReadKey();

        }

        static void ServerThread(Object stateInfo)
        {
            TcpListener server = new TcpListener(IPAddress.Any, 2048);
            server.Start();
            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                ThreadPool.QueueUserWorkItem(ClientServingThread, client);
                            }
        }

        static void ClientThread(Object stateInfo)
        {
            TcpClient client = new TcpClient();
            client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2048));
            byte[] message = new ASCIIEncoding().GetBytes("wiadomosc");
            client.GetStream().Write(message, 0, message.Length);
            byte[] buffer = new byte[1024];
            NetworkStream stream = client.GetStream();
            stream.Read(buffer, 0, message.Length);
            lock (thisLock)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                System.Console.WriteLine("Klient odebral: " + System.Text.Encoding.Default.GetString(buffer));
                Console.ForegroundColor = ConsoleColor.White;
            }
            


        }

        static void ClientServingThread(Object stateInfo)
        {
            TcpClient client = (TcpClient)stateInfo;
            byte[] buffer = new byte[1024];
            client.GetStream().Read(buffer, 0, 1024);
            lock (thisLock)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                System.Console.WriteLine("Serwer odebral: " + System.Text.Encoding.Default.GetString(buffer));
                Console.ForegroundColor = ConsoleColor.White;
            }
           
            client.GetStream().Write(buffer, 0, buffer.Length);
            client.Close();

        }
    }
    
}
