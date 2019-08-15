using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskDemo
{

    class Program
    {
        public enum TPipetteExecute
        {
            peNone, peSmp, peRgn, peSubstrate, peDetect, peDrop
        }

        static TPipetteExecute s;

        static Program()
        {
            s = TPipetteExecute.peSmp;
        }

        private static readonly object synobject = new object();

        static bool fal = false;

        private static CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
        static void Main(string[] args)
        {
            Task.Factory.StartNew(tasknew, cancelTokenSource.Token);
            //fal = true;
            string red = Console.ReadLine();
            if (red == "1")
            {
                Invoke();
                s = TPipetteExecute.peDrop;
                //fal = false;
            }
            Console.ReadKey();
        }


        private static void tasknew()
        {

            TPipetteExecute enumtpi;


            lock (synobject)
            {
                enumtpi = s;
            }

            while (!cancelTokenSource.IsCancellationRequested)
            {

                Thread.Sleep(1000);
                switch (enumtpi)
                {
                    case TPipetteExecute.peSmp:
                        demo1();
                        break;
                    case TPipetteExecute.peDrop:
                        demo2();
                        break;
                }
            }
        }

        private static void demo1()
        {
            for (int i = 0; i < 100; i++)
            {
                Console.WriteLine(i.ToString());
            }
            //Thread th = new Thread(demo3);
            //th.Start();
        }

        private static void demo2()
        {
            for (int i = 0; i < 100; i++)
            {
                Console.WriteLine("线程2："+i.ToString());
            }
        }

        private static void demo3()
        {
            for (int i = 0; i < 100; i++)
            {
                Console.WriteLine("线程3："+i.ToString());
            }
        }
    }
}
