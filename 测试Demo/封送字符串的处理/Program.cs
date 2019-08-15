using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace 封送字符串的处理
{
    // 托管函数中的返回值封送回托管函数的例子
    //class Program
    //{

    //    // Win32 GetTempPath函数的定义如下：   
    //    //DWORD WINAPI GetTempPath(
    //    //  _In_   DWORD nBufferLength,
    //    //  _Out_  LPTSTR lpBuffer
    //    //);    // 主要是注意如何在托管代码中定义该函数原型       
    //    [DllImport("Kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    //    public static extern uint GetTempPath(int bufferLength, StringBuilder buffer);
    //    static void Main(string[] args)
    //    {
    //        StringBuilder buffer = new StringBuilder(300);
    //        uint tempPath = GetTempPath(300, buffer);
    //        string path = buffer.ToString();
    //        if (tempPath == 0)
    //        {
    //            int errorcode = Marshal.GetLastWin32Error();
    //            Win32Exception win32expection = new Win32Exception(errorcode);
    //            Console.WriteLine("调用非托管函数发生异常，异常信息为：" + win32expection.Message);
    //        }

    //        Console.WriteLine("调用非托管函数成功。");
    //        Console.WriteLine("Temp 路径为：" + buffer);
    //        Console.Read();
    //    }
    //}

    class Program
    {
        // 对GetVersionEx进行托管定义
        // 为了传递指向结构体的指针并将初始化的信息传递给非托管代码，需要用ref关键字修饰参数
        // 这里不能使用out关键字，如果使用了out关键字，CLR就不会对参数进行初始化操作，这样就会导致调用失败
        [DllImport("Kernel32", CharSet = CharSet.Unicode, EntryPoint = "GetVersionEx")]
        private static extern Boolean GetVersionEx_Struct(ref OSVersionInfo osVersionInfo);

        // 因为Win32 GetVersionEx函数参数lpVersionInformation是一个指向 OSVERSIONINFO的数据结构
        // 所以托管代码中定义个结构体，把结构体对象作为非托管函数参数
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct OSVersionInfo
        {
            public UInt32 OSVersionInfoSize; // 结构的大小，在调用方法前要初始化该字段
            public UInt32 MajorVersion; // 系统主版本号
            public UInt32 MinorVersion; // 系统此版本号
            public UInt32 BuildNumber;  // 系统构建号
            public UInt32 PlatformId;  // 系统支持的平台

            // 此属性用于表示将其封送成内联数组
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string CSDVersion; // 系统补丁包的名称
            public UInt16 ServicePackMajor; // 系统补丁包的主版本
            public UInt16 ServicePackMinor;  // 系统补丁包的次版本
            public UInt16 SuiteMask;   //标识系统上的程序组
            public Byte ProductType;    //标识系统类型
            public Byte Reserved;  //保留,未使用
        }

        // 获得操作系统信息
        private static string GetOSVersion()
        {
            // 定义一个字符串存储版本信息
            string versionName = string.Empty;

            // 初始化一个结构体对象
            OSVersionInfo osVersionInformation = new OSVersionInfo();

            // 调用GetVersionEx 方法前，必须用SizeOf方法设置结构体中OSVersionInfoSize 成员
            osVersionInformation.OSVersionInfoSize = (UInt32)Marshal.SizeOf(typeof(OSVersionInfo));

            // 调用Win32函数
            Boolean result = GetVersionEx_Struct(ref osVersionInformation);

            if (!result)
            {
                // 如果调用失败，获得最后的错误码
                int errorcode = Marshal.GetLastWin32Error();
                Win32Exception win32Exc = new Win32Exception(errorcode);
                Console.WriteLine("调用失败的错误信息为： " + win32Exc.Message);

                // 调用失败时返回为空字符串
                return string.Empty;
            }
            else
            {
                Console.WriteLine("调用成功");
                switch (osVersionInformation.MajorVersion)
                {
                    // 这里仅仅讨论 主版本号为6的情况，其他情况是一样讨论的
                    case 6:
                        switch (osVersionInformation.MinorVersion)
                        {
                            case 0:
                                if (osVersionInformation.ProductType == (Byte)0)
                                {
                                    versionName = " Microsoft Windows Vista";
                                }
                                else
                                {
                                    versionName = "Microsoft Windows Server 2008"; // 服务器版本
                                }
                                break;
                            case 1:
                                if (osVersionInformation.ProductType == (Byte)0)
                                {
                                    versionName = " Microsoft Windows 7";
                                }
                                else
                                {
                                    versionName = "Microsoft Windows Server 2008 R2";
                                }
                                break;
                            case 2:
                                versionName = "Microsoft Windows 8";
                                break;
                        }
                        break;
                    default:
                        versionName = "未知的操作系统";
                        break;
                }
                return versionName;
            }
        }

        static void Main(string[] args)
        {
            string OS = GetOSVersion();
            Console.WriteLine("当前电脑安装的操作系统为：{0}", OS);
            Console.Read();
        }
    }
}
