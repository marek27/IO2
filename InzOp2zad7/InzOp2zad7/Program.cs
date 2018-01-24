using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InzOp2zad7
{
    class Program
    {
        static void Main(string[] args)
        {

            FileStream fs = File.Open("pliczek.txt", FileMode.OpenOrCreate);
            byte[] buffer = new byte[1024];
            IAsyncResult test = fs.BeginRead(buffer, 0, buffer.Length, null, null);

            fs.EndRead(test);

            string result = Encoding.UTF8.GetString(buffer);

            Console.WriteLine(result);

        }
    }
}