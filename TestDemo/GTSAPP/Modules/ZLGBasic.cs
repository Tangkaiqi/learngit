using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace TestReader.Modules
{

    //1.ZLGCAN系列接口卡信息的数据类型。
    public struct VCI_BOARD_INFO
    {
        public UInt16 hw_Version;//硬件版本
        public UInt16 fw_Version;
        public UInt16 dr_Version;
        public UInt16 in_Version;
        public UInt16 irq_Num;
        public byte can_Num;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public byte[] str_Serial_Num;//序列号
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 40)]
        public byte[] str_hw_Type;//硬件类型
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] Reserved;//保留
    }

    //2.定义CAN信息帧的数据类型。
    public struct VCI_CAN_OBJ
    {
        public UInt32 ID;
        public UInt32 TimeStamp;//时间戳
        public byte TimeFlag;//时间标志
        public byte SendType;//发送类型
        public byte RemoteFlag;//是否是远程帧
        public byte ExternFlag;//是否是扩展帧
        public byte DataLen; //数据长度
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] Data;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public byte[] Reserved;
        public void Init()
        {
            Data = new byte[8];
            Reserved = new byte[3];
        }
    }

    //3.定义CAN控制器状态的数据类型。
    public struct VCI_CAN_STATUS
    {
        public byte ErrInterrupt;
        public byte regMode;
        public byte regStatus;
        public byte regALCapture;
        public byte regECCapture;
        public byte regEWLimit;
        public byte regRECounter;
        public byte regTECounter;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] Reserved;
    }

    //4.定义错误信息的数据类型。
    public struct VCI_ERR_INFO
    {
        public UInt32 ErrCode;
        public byte Passive_ErrData1;
        public byte Passive_ErrData2;
        public byte Passive_ErrData3;
        public byte ArLost_ErrData;
    }

    //5.定义初始化CAN的数据类型
    public struct VCI_INIT_CONFIG
    {
        public UInt32 AccCode;
        public UInt32 AccMask;
        public UInt32 Reserved;
        public byte Filter;
        public byte Timing0;
        public byte Timing1;
        public byte Mode;
    }

    public struct CHGDESIPANDPORT
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public byte[] szpwd;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public byte[] szdesip;
        public Int32 desport;

        public void Init()
        {
            szpwd = new byte[10];
            szdesip = new byte[20];
        }
    }

    ///////// new add struct for filter /////////
    //typedef struct _VCI_FILTER_RECORD{
    //    DWORD ExtFrame;	//是否为扩展帧
    //    DWORD Start;
    //    DWORD End;
    //}VCI_FILTER_RECORD,*PVCI_FILTER_RECORD;
    public struct VCI_FILTER_RECORD
    {
        public UInt32 ExtFrame;
        public UInt32 Start;
        public UInt32 End;
    }



    public static class ZLGBasic
    {

        public const int VCI_PCI5121     = 1;
        public const int VCI_PCI9810     = 2;
        public const int VCI_USBCAN1     = 3;
        public const int VCI_USBCAN2     = 4;
        public const int VCI_USBCAN2A    = 4;
        public const int VCI_PCI9820     = 5;
        public const int VCI_CAN232      = 6;
        public const int VCI_PCI5110     = 7;
        public const int VCI_CANLITE     = 8;
        public const int VCI_ISA9620     = 9;
        public const int VCI_ISA5420     = 10;
        public const int VCI_PC104CAN    = 11;
        public const int VCI_CANETUDP    = 12;
        public const int VCI_CANETE      = 12;
        public const int VCI_DNP9810     = 13;
        public const int VCI_PCI9840     = 14;
        public const int VCI_PC104CAN2   = 15;
        public const int VCI_PCI9820I    = 16;
        public const int VCI_CANETTCP    = 17;
        public const int VCI_PEC9920     = 18;
        public const int VCI_PCI5010U    = 19;
        public const int VCI_USBCAN_E_U  = 20;
        public const int VCI_USBCAN_2E_U = 21;
        public const int VCI_PCI5020U    = 22;
        public const int VCI_EG20T_CAN   = 23;


        public const uint STATUS_OK      = 1;
        public const uint STATUS_ERR     = 0;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DeviceType"></param>
        /// <param name="DeviceInd"></param>
        /// <param name="Reserved"></param>
        /// <returns></returns>

        [DllImport("controlcan.dll")]
        public static extern UInt32 VCI_OpenDevice(UInt32 DeviceType, UInt32 DeviceInd, UInt32 Reserved);//打开驱动
        [DllImport("controlcan.dll")]
        public static extern UInt32 VCI_CloseDevice(UInt32 DeviceType, UInt32 DeviceInd);//关闭驱动
        [DllImport("controlcan.dll")]
        public static extern UInt32 VCI_InitCAN(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd, ref VCI_INIT_CONFIG pInitConfig);//初始化CAN
        [DllImport("controlcan.dll")]
        public static extern UInt32 VCI_ReadBoardInfo(UInt32 DeviceType, UInt32 DeviceInd, ref VCI_BOARD_INFO pInfo);//读线路板的信息
        [DllImport("controlcan.dll")]
        public static extern UInt32 VCI_ReadErrInfo(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd, ref VCI_ERR_INFO pErrInfo);//读错误信息
        [DllImport("controlcan.dll")]
        public static extern UInt32 VCI_ReadCANStatus(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd, ref VCI_CAN_STATUS pCANStatus);
        [DllImport("controlcan.dll")]
        public static extern UInt32 VCI_GetReference(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd, UInt32 RefType, ref byte pData);//获取引用（引用什么？？？）
        [DllImport("controlcan.dll")]
        public static extern UInt32 VCI_SetReference(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd, UInt32 RefType, ref byte pData);//设置引用（引用什么？？？）
        [DllImport("controlcan.dll")]
        public static extern UInt32 VCI_GetReceiveNum(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd);//获取接受数量？？？
        [DllImport("controlcan.dll")]
        public static extern UInt32 VCI_ClearBuffer(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd);//清除缓冲
        [DllImport("controlcan.dll")]
        public static extern UInt32 VCI_StartCAN(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd);//
        [DllImport("controlcan.dll")]
        public static extern UInt32 VCI_ResetCAN(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd);//重置CAN
        [DllImport("controlcan.dll")]
        public static extern UInt32 VCI_Transmit(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd, ref VCI_CAN_OBJ pSend, UInt32 Len);//数据传送
        [DllImport("controlcan.dll", CharSet = CharSet.Ansi)]
        public static extern UInt32 VCI_Receive(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd, IntPtr pReceive, UInt32 Len, Int32 WaitTime);
    }
}
