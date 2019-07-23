using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;

namespace TestReader.Modules
{
    /// <summary>
    /// 与ZLG通讯卡进行通讯的类
    /// </summary>
   public  class ZLGCan: ICom, IDisposable
    {

        private bool _IsOpen;
        private volatile static ZLGCan _instance = null;
        private static readonly object lockHelper = new object();


        private const uint m_DevType  = 4;
        private const uint m_DevIndex = 0;
        private const uint m_CanIndex = 0;

        private IntPtr pt;


        public bool IsOpen
        {
            get
            {
                return _IsOpen;
            }
        }

        private ZLGCan()
        {

            //pt = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(VCI_CAN_OBJ)));
            _IsOpen = OpenUsbCan();
        }

        public static ZLGCan CreateInstance()
        {
            if (_instance == null)
            {
                lock (lockHelper)
                {
                    if (_instance == null)
                        _instance = new ZLGCan();
                }
            }
            return _instance;
        }




        private bool OpenUsbCan()
        {

            if (ZLGBasic.VCI_OpenDevice(m_DevType, m_DevIndex, 0) != ZLGBasic.STATUS_ERR)
            {
                VCI_INIT_CONFIG Cfg = new VCI_INIT_CONFIG();

                Cfg.AccCode = 0x00000000; //验收码
                Cfg.AccMask = 0xFFFFFFFF; //验收屏蔽码
                Cfg.Filter  = 1;          //滤波方式 1: 单滤波 0: 双滤波
                Cfg.Mode    = 0;          //0:正常模式 1:只听模式
                Cfg.Timing0 = 0x00;       //通讯速率  1Mbps
                Cfg.Timing1 = 0x14;

               //判断CAN是否能初始化
                if(ZLGBasic.VCI_InitCAN(m_DevType, m_DevIndex, m_CanIndex, ref Cfg) == ZLGBasic.STATUS_OK)
                {

                    ZLGBasic.VCI_ClearBuffer(m_DevType, m_DevIndex, m_CanIndex);

                    return (ZLGBasic.VCI_StartCAN(m_DevType, m_DevIndex, m_CanIndex) == ZLGBasic.STATUS_OK);

                }

            }

            return false;
        }



        public bool ReadCanMsg(ref uint ID, out byte[] Msg)
        {

            if(_IsOpen)
            {

                pt = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(VCI_CAN_OBJ)));

                Marshal.WriteByte(pt, 0x00);

                uint Cnt = ZLGBasic.VCI_Receive(m_DevType, m_DevIndex, m_CanIndex, pt, 1, 0);

                if (Cnt > 0)
                {
                    VCI_CAN_OBJ obj = (VCI_CAN_OBJ)Marshal.PtrToStructure((IntPtr)((UInt32)pt), typeof(VCI_CAN_OBJ));

                    ID = obj.ID;

                    Msg = new byte[obj.DataLen];

                    Array.Copy(obj.Data, Msg, obj.DataLen);


                    Marshal.FreeHGlobal(pt);

                    return true;
                }

                Marshal.FreeHGlobal(pt);

            }


            ID = 0;
            Msg = new byte[8];

            return false;

        }


        public bool WriteCanMsg(uint ID, byte[] Msg)
        {

            //重试的次数
            int retrycnt = 0;



            if (_IsOpen)
            {

                try
                {

                    VCI_CAN_OBJ CanMsg = new VCI_CAN_OBJ();
                    CanMsg.ID = Globals._NODE_ | ID;
                    CanMsg.SendType = 0;            //0:正常发送 1:单次发送 2:自发自收 3:单次自发自收
                    CanMsg.RemoteFlag = 0;          //0:数据帧 1: 远程帧
                    CanMsg.ExternFlag = 0;          //0:标准帧 1:扩展帧

                    CanMsg.DataLen = (byte) Msg.Length;
                    CanMsg.Data = new byte[8];
                    Array.Copy(Msg, CanMsg.Data, CanMsg.DataLen);


                    VCI_ERR_INFO ErrorInfo = new VCI_ERR_INFO();
                    VCI_CAN_STATUS Vcs = new VCI_CAN_STATUS();

                    do
                    {
                        if (ZLGBasic.VCI_Transmit(m_DevType, m_DevIndex, m_DevIndex, ref CanMsg, 1) == ZLGBasic.STATUS_OK)
                        {
                            return true;
                        }
                        else
                        {
                            retrycnt++;

                            ZLGBasic.VCI_ReadErrInfo(m_DevType, m_DevIndex, m_CanIndex, ref ErrorInfo);

                            ZLGBasic.VCI_ReadCANStatus(m_DevType, m_DevIndex, m_CanIndex, ref Vcs);

                            //写总线异常日志


                            Thread.Sleep(10);
                        }

                    } while (retrycnt < 3);

                }
                catch(Exception ex)
                {
                    LogHelper.WriteLog(ex.Message);
                }
 
            }

            return false;
        }


        public void Dispose()
        {
            Dispose(false);
        }



        private void Dispose(bool fromDestructor)
        {
            if (_IsOpen)
            {
                ZLGBasic.VCI_CloseDevice(m_DevType, m_DevIndex);
                //Marshal.FreeHGlobal(pt);
            }


            if (!fromDestructor)
            {
                GC.SuppressFinalize(this);
            }

        }

        ~ZLGCan()
        {
            Dispose(true);
        }
    }
}
