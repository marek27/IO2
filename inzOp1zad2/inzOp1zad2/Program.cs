using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace inzOp1zad2
{
    class Program
    {
        static void Main(string[] args)
        { 
            ThreadPool.QueueUserWorkItem(ServerThread);
            ThreadPool.QueueUserWorkItem(Client1Thread);
            ThreadPool.QueueUserWorkItem(Client1Thread);
            Thread.Sleep(1000);

        }

        static void ServerThread(Object stateInfo)
        {
            TcpListener server = new TcpListener(IPAddress.Any, 2048);
            server.Start();
            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                byte[] buffer = new byte[1024];
                client.GetStream().Read(buffer, 0, 1024);
                client.GetStream().Write(buffer, 0, buffer.Length);
                client.Close();
            }
        }

        static void Client1Thread(Object stateInfo)
        {
            TcpClient client = new TcpClient();
            client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2048));
            byte[] message = new ASCIIEncoding().GetBytes("wiadomosc");
            client.GetStream().Write(message, 0, message.Length);

        }


    }
}
