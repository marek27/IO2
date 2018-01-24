using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace InzOp2zad6
{
    class Program
    {
        static void Main(string[] args)
        {
            FileStream fs = File.Open("pliczek.txt", FileMode.OpenOrCreate);
            byte[] buffer = new byte[1024];
            fs.BeginRead(buffer, 0, buffer.Length, funkcja, new object[] { fs, buffer });
        }

        static void funkcja(IAsyncResult state)
        {
            object[] stan = (object[])state.AsyncState;
            string result = Encoding.ASCII.GetString((byte[])stan[1]);
            Console.WriteLine(result);
            FileStream fs = (FileStream)stan[0];

            fs.EndRead(state);
        }
    }
}