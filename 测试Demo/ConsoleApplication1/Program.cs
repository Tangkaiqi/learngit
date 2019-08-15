using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        // 在托管代码中对非托管函数进行声明，并且附加平台调用所需要属性
        // 在默认情况下，CharSet为CharSet.Ansi
        // 指定调用哪个版本的方法有两种——通过DllImport属性的CharSet字段和通过EntryPoint字段指定
        // 在托管函数中声明注意一定要加上 static 和extern 这两个关键字
        //[DllImport("user32.dll")]
        //public static extern int MessageBox1(IntPtr hWnd, String text, String caption, uint type);

        // 在默认情况下，CharSet为CharSet.Ansi
        [DllImport("user32.dll")]
        public static extern int MessageBoxA(IntPtr hWnd, String text, String caption, uint type);

        // 在默认情况下，CharSet为CharSet.Ansi
        [DllImport("user32.dll")]
        public static extern int MessageBox(IntPtr hWnd, String text, String caption, uint type);

        // 第一种指定方式，通过CharSet字段指定
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern int MessageBox2(IntPtr hWnd, String text, String caption, uint type);

        // 通过EntryPoint字段指定
        [DllImport("user32.dll", EntryPoint = "MessageBoxA")]
        public static extern int MessageBox3(IntPtr hWnd, String text, String caption, uint type);

        [DllImport("user32.dll", EntryPoint = "MessageBoxW")]
        public static extern int MessageBox4(IntPtr hWnd, String text, String caption, uint type);
        //static void Main(string[] args)
        //{
        //    // 在托管代码中直接调用声明的托管函数
        //    // 使用CharSet字段指定的方式，要求在托管代码中声明的函数名必须与非托管函数名一样
        //    // 否则就会出现找不到入口点的运行时错误
        //    //MessageBox1(new IntPtr(0), "Learning Hard", "欢迎", 0);

        //    // 下面的调用都可以运行正确
        //    //MessageBoxA(new IntPtr(0), "Learning Hard", "欢迎", 0);
        //    //MessageBox(new IntPtr(0), "Learning Hard", "欢迎", 0);

        //    // 使用指定函数入口点的方式调用
        //    MessageBox3(new IntPtr(0), "Learning Hard", "欢迎", 0);

        //    // 调用Unicode版本的会出现乱码
        //    //MessageBox4(new IntPtr(0), "Learning Hard", "欢迎", 0);
        //}

        // 在托管代码中对非托管函数进行声明，并且附加平台调用所需要属性
        // 在默认情况下，CharSet为CharSet.Ansi
        // 指定调用哪个版本的方法有两种——通过DllImport属性的CharSet字段和通过EntryPoint字段指定
        [DllImport("user32.dll")]
        public static extern int MessageBox1(IntPtr hWnd, String text, String caption, uint type);
        static void Main(string[] args)
        {
            try
            {
                MessageBox1(new IntPtr(0), "Learning Hard", "欢迎", 0);
            }
            catch (DllNotFoundException dllNotFoundExc)
            {
                Console.WriteLine("DllNotFoundException 异常发生，异常信息为： " + dllNotFoundExc.Message);
            }
            catch (EntryPointNotFoundException entryPointExc)
            {
                Console.WriteLine("EntryPointNotFoundException 异常发生，异常信息为： " + entryPointExc.Message);
            }
            Console.Read();
        }
    }

    class firts
    {
        public firts()
        {
            Console.WriteLine("父类");
        }

        protected virtual void ni()
        {
            Console.WriteLine("虚方法");
        }
    }

    class boyes :firts
    {
        public boyes()
        {
            Console.WriteLine("子类");
            ni();
        }

        protected override void ni()
        {
            base.ni();
            Console.WriteLine("重写");
        }
    }




    public class demoClass
    {
        public int intnum { get; set; }
        public string str { get; set; }

        private Dictionary<string, jiClass> m_Lay = new Dictionary<string, jiClass>();

        public demoClass()
        {
            Listdemo listdemo = new Listdemo();
            m_Lay.Add("nihao",listdemo);
        }

    }

    public class Listdemo : jiClass
    {
        string ssre = "";
        private void dfunction()
        {
            ssre = Ss;
        }
    }

    public class jiClass
    {
        protected string ss = "鸡肋";
        protected string Ss
        {
            get
            {
                return ss;
            }

            set
            {
                ss = value;
            }
        }
    }


   public class Shape
    {
        public void setWidth(int w)
        {
            width = w;
        }
        public void setHeight(int h)
        {
            height = h;
        }
        protected int width;
        protected int height;
    }

    // 派生类
  public  class Rectangle : Shape
    {
        public int getArea()
        {
            return (width * height);
        }
    }

}


