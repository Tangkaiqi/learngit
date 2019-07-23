using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Threading;
using System.Security.Cryptography;
using System.Runtime.InteropServices;

namespace TestReader.Modules
{
    public struct TArmCoord
    {
        public int X;
        public int Y;
        public int Z;
        public int DY;     //Y通道的间距
    }
    public struct TGrap
    {
        public int GOpen;
        public int GClose;
    }

    //抓手坐标定义
    public struct TGripperCoord
    {
        public int X;
        public int Y;
        public int Z;
        public int G1;   //Open Value
        public int G2;   //Close Value
    }


    //脱针位置定义
    public struct TPipetteDropPosition
    {
        public int DropX;
        public int DropY;
        public int DY;
        public int UPZ;
        public int DownZ;
        public int DropXOfs;
        
    }



    public struct TPosArr
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public int[] Pos;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public string[] TipData;

        public TPosArr(int First, int Step)
        {

            Pos = new int[6];

            TipData = new string[6];

            for (int i = 0; i < 6; i++)
            {
                Pos[i] = First + i * Step;

                TipData[i] = "";


            }
        }

        public void SetPosArr(uint Tips, int ZPos)
        {
            Pos = new int[6];
            for (int i = 0; i < 6; i++)
            {
                if ((Tips & (1 << i)) > 0)//???
                {
                    Pos[i] = ZPos;
                }
            }
        }

        public void SetPosArr(uint UsedTips, int First, int Step)
        {
            Pos = new int[6];
            for (int i = 0; i < 6; i++)
            {
                if ((UsedTips & (1 << i)) > 0)
                {
                    Pos[i] = First;
                    First += Step;
                }

            }
        }


        public void SetPosArr(uint Tips, int ZPos, bool IsAdd)
        {
            for (int i = 0; i < 6; i++)
            {
                if ((Tips & (1 << i)) > 0)
                {
                    if (IsAdd)
                    {
                        Pos[i] += ZPos;
                    }
                    else
                    {
                        Pos[i] = ZPos;
                    }

                }
            }
        }

    }


    public struct TVolArr
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public double[] Vol;


        public TVolArr(double Volume)
        {
            Vol = new double[6];

            for (int i = 0; i < 6; i++)
            {
                Vol[i] = Volume;
            }
        }

        public void SetVol(uint UsedTips, double Volume, bool IsAdd = false)
        {
            for (int i = 0; i < 6; i++)
            {
                if ((UsedTips & (1 << i)) > 0)
                {
                    if (IsAdd)
                        Vol[i] += Volume;
                    else
                        Vol[i] = Volume;
                }

            }
        }


    }


    #region Enumerations
    public enum TLQErrorMode : uint
    {
        LQ_OFF = 0,
        LQ_DisplayError = 1,
        LQ_OnErrorGotoZMAX = 2,
    }

    public enum TAspMode : uint
    {
        ASP_FROM_ADJUSTED_Z_POS = 0, //从ZDsp位置+ZPos位置开始吸液
        ASP_FROM_LAST_LQD_LEVEL_WITH_SUBMERGE = 1, //从最后的液面位置开始吸液
        ASP_WITH_TRACKING = 2, //吸液时液面跟随

        ASP_CLOT_DETECTION_ON = 4, //凝块探测
        ASP_CLOT_ERROR_AUTO_RETRY = 8, //凝块探测错误自动重试
        ASP_CLOT_ERROR_AUTO_IGNORE = 16,//凝块探测错误自动忽略
        ASP_CLOT_ERROR_AUTO_SKIP = 32,//凝块探测错误自动跳过该针
        ASP_FROM_ZMAX = 64 //从最低位置开始吸液
    }

    public enum TDspMode : uint
    {
        DISP_AT_RACK_ZDISP = 0, //从ZDsp位置开始注液
        DISP_AT_ADJUSTED_ZPOS = 1, //从ZDsp位置+ZPos位置开始注液
        DISP_AT_LAST_LQDLEVEL = 2,  //在最后液面位置开始注液

        DISP_WITH_TRACKING = 4,  //注液时液面跟随(边注液边提起Z)
        ASP_CLOT_ERROR_AUTO_RETRY = 8, //凝块探测错误自动重试
        ASP_CLOT_ERROR_AUTO_IGNORE = 16,//凝块探测错误自动忽略
        ASP_CLOT_ERROR_AUTO_SKIP = 32,//凝块探测错误自动跳过该针
        ASP_FROM_ZMAX = 64 //从最低位置开始吸液
    }

    public enum TPipettingMode
    {
        pmSingle,               //单次注液
        pmMutil                 //多次注液
    }



    public enum TRackZPos : uint
    {
        rzZTravel = 0,
        rzZScan = 1,
        rzZDisp = 2,
        rzZMax = 3,
    }


    public enum TMachineType
    {
        MDR1800,
        MDR2800,
        MDR3800,
        MDR4800,
        MDR5800,
        MDR6800,
        MDR7800,
        MDR8800
    }

    public enum TDockModule
    {
       dmZMotor,
       dmYMotor,
    }

    //加样避初始化后停泊的位置
    public enum TArmAbsPos
    {
        HomePosition,
        WastePosition,
        ParkPosition,
        DropPosition
    }

    //取吸头的模式
    public enum TGetTipMode
    {
        gmTogether,         //同时取针
        gmOneByOne,         //依次取针
        gmOneTogether,      //交错取针
        gmTwoTogether       //对半取针
    }

    public enum TTipType : uint
    {
        DITI250 = 1024,
        DITI400 = 2048,
        DITI1000 = 4096
    }

    public enum TTipErrorMode
    {
        GT_IgnoreError,             //忽略错误
        GT_DisplayError,            //弹出警告对话框
        GT_AutoRetry,               //自动跳到下一个位置,不是原位重试
    }

    //加样臂上的模块
    public enum TArmModule
    {
        mnXMotor,
        mnYMotor,
        mnZMotor,
        mnPump
    }
    public enum TCentrifugModule
    {
        mnCentrifuge,
        mnCentrifugeCover,
        mnCentrifugeMotor,
        Incubator
    }

    //抓手臂上的模块
    public enum TGripperModule
    {
        gmGXMotor,
        gmGYMotor,
        gmGZMotor,
        gmGGMotor
    }

    //模块类型码
    public enum TModuleType
    {
        mtNone = 0,
        mtXMotor = 1,
        mtYZMotor = 2,
        mtPump = 3,
        mtShaker = 4,
        mtCentrifuge = 5,//离心机
        mtWasher = 6
    }

    public enum TModuleError
    {
        ERR_SUCCESS,
        ERR_TIMEOUT,
        ERR_NOT_INIT,
        ERR_SUSPEND,
        ERR_UNKNOWN
    }

    public enum TErrorType
    {
        etFW,                   //硬件错误
        etWarn,               //警告性错误
        etSW                    //软件错误

    }
    //????

    public enum TModuleInfoFrame
    {
        mfOne,
        mfTwo,
        mfThree,
        mfFour,
        mfFive,
        mfSix

    }
    //???
    public enum TDataAreaType
    {
        drShare, drSwShare, drFwShare
    }

    public enum TProgramMode
    {
        pmBoot, pmMain
    }

    public enum TExecuteMode : byte
    {

        /// <summary>
        /// 预备状态
        /// </summary>
        emPrepared,
        /// <summary>
        /// 执行状态
        /// </summary>
        emExecute 

    }
    public enum TMoveMode
    {
        /// <summary>
        /// 绝对移动
        /// </summary>
        MOVE_ABS,
        /// <summary>
        /// 相对移动
        /// </summary>
        MOVE_REL,
        /// <summary>
        /// 扫描移动
        /// </summary>
        MOVE_SCAN,   
        MOVE_AS_HOME,
        MOVE_SCAN_REL
    }

    public enum TMovePowerGrp
    {
        MOVE_POWER_DEF = 0,
        /// <summary>
        /// 运动力矩组
        /// </summary>
        MOVE_POWER1 = 1,
        MOVE_POWER2 = 2,
        MOVE_POWER3 = 3
    }




    public enum TModuleState : uint
    {
        msNone = 0x00,
        msInited = 0x01,
        msPrepared = 0x02,
        msActive = 0x04,
        msHasErr = 0x08,
        msHasWarn = 0x10,
        msMoving = 0x20,
        msDetect = 0x40,
        msData = 0x80,

    }


    public enum TGroupMask : byte
    {
        GROUP_MASK1 = 0x10,         //加样臂1的执行组
        GROUP_MASK2 = 0x20,         //加样臂2的执行组
        GROUP_MASK3 = 0x40,         //抓手臂1的执行组
        GROUP_MASK4 = 0x80          //抓手臂2的执行组


    }


    //加样臂上所有的模块

    public enum TExecuteMask : byte
    {

        EXEC_XMOTOR = 0x01,
        EXEC_YMOTOR = 0X02,
        EXEC_ZMOTOR = 0x04,
        EXEC_PUMP = 0X08,
    }


    public enum TMachineStauts
    {
        msNone,                            //机器处于未知状态
        msReady,                          //处于准备状态,设备已经初始化
        msRunning,                      //设备处于运行状态
        msPause,                          //设备处于暂停状态
        msAbort                            //设备运行被终止了
    }

    public enum TTipOperation
    {
        tpGetTip,
        tpDropTip,
    }

    //通道当前的状态
    public enum TChannelTipState
    {
        ctNoTip,   //没有取到吸头
        ctClot     //通道上检测到凝块
    }


    //针通道当前的操作类型
    public enum TChannelAct
    {
        caNone,
        caAsp,
        caDsp,
        caMix,
    }
    #endregion
    public static class Globals
    {
        public struct TArmCoord
        {
            public int X;
            public int Y;
            public int Z;
            public int DY;     //Y通道的间距
        }
        public const string XMOTOR_NAME = "XMotor";
        public const string YMOTOR_NAME = "YMotor";
        public const string ZMOTOR_NAME = "ZMotor";
        public const string PUMP_NAME = "PUMP";

        public const string ARM1 = "Arm1";    //第1个加样臂的前缀
        public const string ARM2 = "Arm2";    //第2个加样臂的前缀

        public const string GRIPPER1 = "Grp1";  //第1个抓手臂的前缀
        public const string GRIPPER2 = "Grp2";  //第2个抓手臂的前缀

        public const string GRIPPER_X = "GX";
        public const string GRIPPER_Y = "GY";
        public const string GRIPPER_Z = "GZ";
        public const string GRIPPER_G = "GG";

        public const string Dock_Y = "DockY";
        public const string Dock_Z = "DockZ";

        public const string Centrifuge = "Centrifuge";
        public const string CentrifugeCover = "CentrifugeCover";
        public const string CentrifugeMotor = "CentrifugeMotor";
        public const string Incubator = "Incubator";

        public const uint TIP_INTERVAL = 90;    //通道的间距值9mm
        public const uint ALLOW_FIX = 10;
        public const short YINIT_OFFSET_VALUE = 320;
        public const uint _NODE_ = 0X400;            //CAN节点的下行命令帧ID的前缀

        public const byte ALLOW_DEV = 3;             //马达运动允许的误差

        public const short ZMOVE_ABS = 200;          //取针后通道提到的绝对位置

        public const short MARGIN_WIDTH = 3;

        public static long MAIN_THREAD_ID;           //主线程的ID（UI线程）
        public static int MAX_CHANNEL_CNT = 8;       //最大的通道数

#if DEBUG
        public static int REPONSE_TIME_OUT = 100;    //等待硬件响应的超时时间
#else 
        public static int REPONSE_TIME_OUT = 2000;    //等待硬件响应的超时时间
#endif
        public static int MAX_PLATE_COUNT = 16;

        public static TMachineType g_MachineType = TMachineType.MDR1800;

        public static bool SUPPORT_IGE_METHOD = false;  //是否支持过敏原

        public static bool SUPPORT_CLIA = true;      //是否支持化学发光设备

        public static bool HAVE_TUBE_SCAN = false;      //是否有试管扫描器

        public static string THEME_NAME = "TelerikMetroBlue";

        public static bool g_CheckRange = true;  //在运行时检查运动距离的误差，如果运动的距离在设定的最小误差内则不运动

        //public static TaskItems g_Items;         //实验项目集合
        //public static TaskGroups g_Groups;       //实验组集合

        //public static Liquids g_Liquid;          //液体定义集合
        // public static TReagentLists g_RgnList;   //试剂定义集合

        //public static WasherMethod g_WashMethod; //洗板方法集合

        //public static ReaderAssembly g_ReaderAss; //读数仪方法集合

       //public static WorkList g_WorkList = new WorkList();        //样本的工作列表

        //public static TStdQC g_StdQC;             //标准曲线

        //public static TReagentAssemble g_ReagentAssemble;  //试剂装载信息

       //public static MdrMachine g_Machine1;      //前处理机器

       // public static MdrMachine g_Machine2;      //后处理机器

       // public static TMySql Fmysql;

        //public static TUserParam g_userparam;     //用户参数



       //public static MdrSchedule g_ScheduleDriver;  //设备的调度控制类

        public static ReaderWriterLockSlim g_RwLock = new ReaderWriterLockSlim();

        //当暂停时，设置此事件为无信号，阻塞发送过程（所有操作硬件的动作都必须在子线程中进行操作，
        //不能在主线程中操作，不然则可能导致界面僵死）
        public static ManualResetEvent g_SendEvent = new ManualResetEvent(true);

        //终止调度过程的事件
        public static ManualResetEvent g_AbortEvent = new ManualResetEvent(false);

        //微板是否已经启动加热孵育了（化学放光中使用，在第1个试剂分配后开始孵育器的孵育,在整个实验过程中孵育器都是启动的）
        public static Dictionary<string, bool> g_PlateHeat = new Dictionary<string, bool>();
        public static byte CrcCheck(byte[] Buff, UInt32 Len)
        {

            byte Bit0, Cbit, R, Bytes;


            byte Ret = 0;

            for (UInt32 J = 0; J < Len; J++)
            {
                Bytes = Buff[J];

                for (int i = 0; i < 7; i++)
                {
                    Cbit = (byte)(Ret & 0x01);

                    Bit0 = (byte)(Bytes & 0x01);

                    Ret >>= 1;

                    R = (byte)(Cbit ^ Bit0);

                    if (R == 1)
                    {
                        Ret ^= 0x8C;
                    }
                    Bytes >>= 1;

                }

            }
            return Ret;

        }
       
        public static byte GetTipCount(byte Tips)
        {
            byte Cnt = 0;

            for (int i = 0; i < MAX_CHANNEL_CNT; i++)
            {
                if ((Tips & (1 << i)) > 0)
                {
                    Cnt++;
                }
            }

            return Cnt;
        }

        /// <summary>
        /// 根据数量
        /// </summary>
        /// <param name="Tips"></param>
        /// <param name="Count"></param>
        /// <returns></returns>
        public static byte GetUsedTips(byte Tips, int Count)
        {
            int TipCount = GetTipCount(Tips);

            if (TipCount <= Count)
            {
                return Tips;
            }
            else
            {
                int J = 0;
                byte UsedTips = 0;

                for (int i = 0; i < MAX_CHANNEL_CNT; i++)
                {
                    if ((Tips & (1 << i)) > 0)
                    {
                        if (J < Count)
                        {
                            UsedTips += (byte)(1 << i);

                            J++;
                        }
                        else break;

                    }
                }

                return UsedTips;

            }
        }


        /*


            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]//字节对齐方式
            public struct MyStruct
            {
                public byte bt;
                public int i;
                public short s;
            }

            unsafe

            {
                size = Marshal.SizeOf(sd);
            }

            byte[] b = StructToBytes(sd,size);
            ByteToStruct(b, typeof(StructDemo));

            */


        //将Byte转换为结构体类型

        public static byte[] StructToBytes(object structObj, int size)

        {

            byte[] bytes = new byte[size];

            IntPtr structPtr = Marshal.AllocHGlobal(size);

            //将结构体拷到分配好的内存空间

            Marshal.StructureToPtr(structObj, structPtr, false);

            //从内存空间拷贝到byte 数组
            Marshal.Copy(structPtr, bytes, 0, size);

            //释放内存空间
            Marshal.FreeHGlobal(structPtr);

            return bytes;

        }



        //将Byte转换为结构体类型
        public static object ByteToStruct(byte[] bytes, Type type)

        {

            int size = Marshal.SizeOf(type);

            if (size > bytes.Length)

            {

                return null;

            }

            //分配结构体内存空间

            IntPtr structPtr = Marshal.AllocHGlobal(size);

            //将byte数组拷贝到分配好的内存空间

            Marshal.Copy(bytes, 0, structPtr, size);

            //将内存空间转换为目标结构体

            object obj = Marshal.PtrToStructure(structPtr, type);

            //释放内存空间

            Marshal.FreeHGlobal(structPtr);

            return obj;

        }


        /// <summary>
        /// 将byte[]还原为指定的struct,该函数的泛型仅用于自定义结构
        /// startIndex：数组中 Copy 开始位置的从零开始的索引。
        /// length：要复制的数组元素的数目。
        /// </summary>
        public static T BytesToStructs<T>(byte[] bytes, int startIndex, int length)
        {
            if (bytes == null) return default(T);
            if (bytes.Length <= 0) return default(T);
            IntPtr buffer = Marshal.AllocHGlobal(length);
            try//struct_bytes转换
            {
                Marshal.Copy(bytes, startIndex, buffer, length);
                return (T)Marshal.PtrToStructure(buffer, typeof(T));
            }
            catch (Exception ex)
            {
                throw new Exception("Error in BytesToStruct ! " + ex.Message);
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }


        /// <summary>
        /// 将struct类型转换为byte[]
        /// </summary>
        public static byte[] StructToByte(object structObj, int size)
        {
            IntPtr buffer = Marshal.AllocHGlobal(size);
            try//struct_bytes转换
            {
                Marshal.StructureToPtr(structObj, buffer, false);
                byte[] bytes = new byte[size];
                Marshal.Copy(buffer, bytes, 0, size);
                return bytes;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in StructToBytes ! " + ex.Message);
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }


        //把值转换为补码形式
        public static int ConvertDistance(short Distance)
        {
            if (Distance > 0)
                return (int)Distance;
            else
            {
                int T = Math.Abs(Distance);

                T = T ^ 0xFFFF;


                return T + 1;
            }
        }

        //补码求值
        public static int ConvertValue(Byte L, Byte H)
        {
            int Value = (int)((H << 8) | L);

            if ((H >> 7) == 1)
            {
                int T = Value ^ 0xFFFF;
                return -(T + 1);
            }
            else
                return Value;

        }

        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public static string GetTimeStamp(System.DateTime time)
        {
            long ts = ConvertDateTimeToInt(time);
            return ts.ToString();
        }
        /// <summary>  
        /// 将c# DateTime时间格式转换为Unix时间戳格式  
        /// </summary>  
        /// <param name="time">时间</param>  
        /// <returns>long</returns>  
        public static long ConvertDateTimeToInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            long t = (time.Ticks - startTime.Ticks) / 10000;   //除10000调整为13位      
            return t;
        }
        /// <summary>        
        /// 时间戳转为C#格式时间        
        /// </summary>        
        /// <param name=”timeStamp”></param>        
        /// <returns></returns>        
        private static DateTime ConvertStringToDateTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }

        public static byte CheckSum(byte[] Buff, UInt32 Len)
        {

            byte Ret = 0;
            for (UInt32 i = 0; i < Len; i++)
            {
                Ret += Buff[i];
            }
            Ret = (byte)(0xFF - Ret + 1);

            return Ret;
        }


        /*  public static void AddLiquidItemToList(RadDropDownList DropList)
          {
              DropList.Items.Clear();

              for (int i = 0; i < g_Liquid.Count; i++)
              {
                  DropList.Items.Add(g_Liquid.GetNames(i));
              }
          }


          public static void AddReagentNameToList(RadDropDownList DropList)
          {
              DropList.Items.Clear();

              for (int i = 0; i < g_RgnList.Count; i++)
              {
                  DropList.Items.Add(g_RgnList.GetNames(i));
              }
          }

          public static void AddWashMethodToList(RadDropDownList DropList)
          {
              DropList.Items.Clear();

              List<string> lstMthdName = g_WashMethod.GetListMthdName();

              for (int i = 0; i < lstMthdName.Count; i++)
              {
                  DropList.Items.Add(lstMthdName[i]);
              }
          }*/

        public static string GetMD5(string myString)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = System.Text.Encoding.Unicode.GetBytes(myString);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("X");
            }

            return byte2String;
        }

        public static string MD5Encrypt(string strText)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(System.Text.Encoding.Default.GetBytes(strText));
            return System.Text.Encoding.Default.GetString(result);
        }


        public static bool IsNumeric(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return false;
            return Regex.IsMatch(value, @"^[0-9].*$");
        }




    }



}
