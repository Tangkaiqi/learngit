using System;
using System.ComponentModel;
// 使用平台调用技术进行互操作性之前，首先需要添加这个命名空间
using System.Runtime.InteropServices;

namespace 处理Win32函数返回的错误
{
    class Program
    {
        // Win32 API 
        //  DWORD WINAPI GetFileAttributes(
        //  _In_  LPCTSTR lpFileName
        //);

        // 在托管代码中对非托管函数进行声明
        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern uint GetFileAttributes(string filename);

        static void Main(string[] args)
        {
            // 试图获得一个不存在文件的属性
            // 此时调用Win32函数会发生错误
            GetFileAttributes("FileNotexist.txt");

            // 在应用程序的Bin目录下存在一个test.txt文件,此时调用会成功
            //GetFileAttributes("test.txt");

            // 获得最后一次获得的错误
            int lastErrorCode = Marshal.GetLastWin32Error();

            // 将Win32的错误码转换为托管异常
            //Win32Exception win32exception = new Win32Exception();
            Win32Exception win32exception = new Win32Exception(lastErrorCode);
            if (lastErrorCode != 0)
            {
                Console.WriteLine("调用Win32函数发生错误，错误信息为 : {0}", win32exception.Message);
            }
            else
            {
                Console.WriteLine("调用Win32函数成功,返回的信息为： {0}", win32exception.Message);
            }

            Console.Read();
        }
    }
}