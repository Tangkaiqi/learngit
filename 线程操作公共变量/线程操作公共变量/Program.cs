//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace 线程操作公共变量
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
//            for (int i = 0; i < 10; i++)
//            {
//                Thread testthread = new Thread(Add);
//                testthread.Start();
//            }

//            Console.Read();
//        }

//        // 共享资源
//        public static int number = 1;

//        public static void Add()
//        {

//            Thread.Sleep(1000);
//            Monitor.Enter(number);
//            //Console.WriteLine("the current value of number is:{0}", Interlocked.Increment(ref number));
//            Console.WriteLine("the current value of number is:{0}", number++);
//            Monitor.Exit(number);
//        }
//    }
//}

/*
using System;
using System.Collections.Generic;
using System.Threading;

namespace ReaderWriterLockSample
{
    class Program
    {
        public static List<int> lists = new List<int>();

        // 创建一个对象
        public static ReaderWriterLock readerwritelock = new ReaderWriterLock();
        static void Main(string[] args)
        {
            //创建一个线程读取数据
            Thread t1 = new Thread(Write);
            t1.Start();
            // 创建10个线程读取数据
            for (int i = 0; i < 10; i++)
            {
                Thread t = new Thread(Read);
                t.Start();
            }

            Console.Read();

        }

        // 写入方法
        public static void Write()
        {
            // 获取写入锁，以10毫秒为超时。
            readerwritelock.AcquireWriterLock(10);
            Random ran = new Random();
            int count = ran.Next(1, 10);
            lists.Add(count);
            Console.WriteLine("Write the data is:" + count);
            // 释放写入锁
            readerwritelock.ReleaseWriterLock();
        }

        // 读取方法
        public static void Read()
        {
            // 获取读取锁
            readerwritelock.AcquireReaderLock(10);

            foreach (int li in lists)
            {
                // 输出读取的数据
                Console.WriteLine(li);
            }

            // 释放读取锁
            readerwritelock.ReleaseReaderLock();
        }
    }
}

*/

using System;
using System.Threading;

class Program
{
    static void Main(string[] args)
    {
        // 创建一个新线程（默认为前台线程）
        Thread backthread = new Thread(Worker);

        // 使线程成为一个后台线程
        //backthread.IsBackground = true;

        // 通过Start方法启动线程
        backthread.Start();

        // 如果backthread是前台线程，则应用程序大约5秒后才终止
        // 如果backthread是后台线程，则应用程序立即终止
        Console.WriteLine("Return from Main Thread");
    }
    private static void Worker()
    {
        // 模拟做10秒
        Thread.Sleep(5000);

        // 下面语句，只有由一个前台线程执行时，才会显示出来
        Console.WriteLine("Return from Worker Thread");
    }
}
