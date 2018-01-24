using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        private static int result = 0;

        static List<WaitHandle> waitHandles = new List<WaitHandle>();

        static void Main(string[] args)
        {
            int rozmiarTablicy = 250;
            int wielkoscFfragmentu = 15;

            CreateHandlerForEachFragment(rozmiarTablicy, wielkoscFfragmentu);
            List<int[]> fragmenty = utworzFragmenty(ref rozmiarTablicy, wielkoscFfragmentu);



            Stopwatch stopwatch = Stopwatch.StartNew();
            UruchomProcesyDlaFragmentow(fragmenty);
            System.Threading.Thread.Sleep(500);
            stopwatch.Stop();
            Console.WriteLine("Czas w ms: "+stopwatch.ElapsedMilliseconds);

            WaitHandle.WaitAll(waitHandles.ToArray());
            Console.Out.WriteLine("Wynik: "+result);
            Console.ReadKey();
        }

        private static void UruchomProcesyDlaFragmentow(List<int[]> fragmenty)
        {
            int j = 0;
            foreach (int[] fragment in fragmenty)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(CountingThread), new HandleAndNumber(fragment, waitHandles[j]));
                j++;
            }
        }

        private static List<int[]> utworzFragmenty(ref int rozmiarTablicy, int wielkoscFfragmentu)
        {
            List<int[]> fragmenty = new List<int[]>();
            while (rozmiarTablicy > wielkoscFfragmentu - 1)
            {
                int[] fragment = new int[wielkoscFfragmentu];
                fillTableWithRandom(fragment);
                fragmenty.Add(fragment);
                rozmiarTablicy -= wielkoscFfragmentu;
            }

            int[] ostatniFragment = new int[rozmiarTablicy];
            fillTableWithRandom(ostatniFragment);
            fragmenty.Add(ostatniFragment);
            return fragmenty;
        }

        private static void CreateHandlerForEachFragment(int rozmiarTablicy, int wielkoscFfragmentu)
        {
            for (int i = 0; i < rozmiarTablicy / wielkoscFfragmentu + 1; i++)
            {
                waitHandles.Add(new AutoResetEvent(false));
            }
        }

        static void CountingThread(Object stateInfo)
        {
            HandleAndNumber handleAndNumber = (HandleAndNumber)stateInfo;
            AutoResetEvent are = (AutoResetEvent)handleAndNumber.waitHandle;
            lock (thisLock)
            {
                for(int i = 0; i < handleAndNumber.fragment.Length; i++)
                {
                    result += handleAndNumber.fragment[i];
                }
            }
            are.Set();
        }

        static void fillTableWithRandom(int [] table)
        {
            Random rnd = new Random();
            for (int i = 0; i < table.Length; i++)
            {
                table[i]= rnd.Next(0, 101);
        }
        }



    }

    public class HandleAndNumber
    {
        public int [] fragment;
        public WaitHandle waitHandle;

        public HandleAndNumber(int [] fragment, WaitHandle waitHandle)
        {
            this.fragment = fragment;
            this.waitHandle = waitHandle;
        }
    }

}
