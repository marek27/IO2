using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InzOp2zad8
{
    class Program
    {
        delegate int DelegateType(int licz);
        static DelegateType iteracja;
        static int iteracja(int liczba)
        {
            int r = 1;
            for (int i = 1; i <= liczba; i++)
            {
                r *= i;
            }
            return r;
        }

        static int rekur(int l)
        {
            if (l < 1)
                return 1;
            else
                return l * rekurencja(l - 1);
        }

        static void Main(string[] args)
        {

            int n = 5;

            iteracja = new DelegateType(iteracja);
            IAsyncResult z1 = iteracja.BeginInvoke(n, null, null);
            int i = iteracja.EndInvoke(z1);

            iteracja = new DelegateType(rekur);
            IAsyncResult z2 = iteracja.BeginInvoke(n, null, null);
            int rek = iteracja.EndInvoke(z2);

            Console.WriteLine("Wynik iteracji: " + i);
            Console.WriteLine("Wynik rekurencji: " + rek);
        }
    }
}