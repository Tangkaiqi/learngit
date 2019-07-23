
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TestReader.Modules
{
    
    interface ICom:IDisposable
    {
        //从CAN总线上读取数据
        bool ReadCanMsg(ref uint ID, out byte[] Msg);

        //把数据发送到CAN总线上
        bool WriteCanMsg(uint ID, byte[] Msg);

    }
    
    /// <summary>
    /// 与PEAK卡进行通讯的类
    /// </summary>

    class Peakcan: ICom
    {
        private bool _IsOpen;
        private volatile static Peakcan _instance = null;
        private static readonly object lockHelper = new object();

        private Peakcan() {
            _IsOpen = OpenUsbCan();
        }
        public static Peakcan CreateInstance()
        {
            if (_instance == null)
            {
                lock (lockHelper)
                {
                    if (_instance == null)
                        _instance = new Peakcan();
                }
            }
            return _instance;
        }


        private bool OpenUsbCan()
        {
            if (PCANBasic.Initialize(PCANBasic.PCAN_USBBUS1, TPCANBaudrate.PCAN_BAUD_1M) == TPCANStatus.PCAN_ERROR_OK)
            {
                if (PCANBasic.Reset(PCANBasic.PCAN_USBBUS1) == TPCANStatus.PCAN_ERROR_OK)
                {
                    return true;
                }
            }

            return false;
        }

        public bool ReadCanMsg(ref uint ID, out byte[] Msg)
        {
            if (_IsOpen)
            {
                TPCANMsg CanMsg;

                CanMsg.DATA = new byte[8];


                bool Ret = PCANBasic.Read(PCANBasic.PCAN_USBBUS1, out CanMsg) == TPCANStatus.PCAN_ERROR_OK;

                Msg = new byte[CanMsg.LEN];
                ID = CanMsg.ID;
                Array.Copy(CanMsg.DATA, Msg, CanMsg.LEN);

                return Ret;

            }else
            {
                ID = 0;
                Msg = new byte[8];
                return false;
            }

           
            
        }

        public bool WriteCanMsg(uint ID, byte[] Msg)
        {
            int retrycnt = 0;

            TPCANMsg CanMsg = new TPCANMsg();
            CanMsg.ID = Globals._NODE_ | ID;
            CanMsg.MSGTYPE = TPCANMessageType.PCAN_MESSAGE_STANDARD;

            CanMsg.LEN = (byte) Msg.Length;
            CanMsg.DATA = new byte[8];
            Array.Copy(Msg, CanMsg.DATA, CanMsg.LEN);

            if (_IsOpen)
            {
                do
                {
                    if (PCANBasic.Write(PCANBasic.PCAN_USBBUS1, ref CanMsg) == TPCANStatus.PCAN_ERROR_OK)
                    {
                        return true;
                    }
                    else
                    {
                        retrycnt++;
                        TPCANStatus Status = PCANBasic.GetStatus(PCANBasic.PCAN_USBBUS1);
                        //写总线异常日志


                        Thread.Sleep(10);
                    }

                } while (retrycnt < 3);
            }

            return false;
        }

        /**
         * 实现IDisposable接口的方法
         * 
         * 
         **/
        public void Dispose()
        {
            Dispose(false);

        }


        private void Dispose(bool fromDestructor)
        {
            if (_IsOpen)
            {
                PCANBasic.Uninitialize(PCANBasic.PCAN_USBBUS1);
            }


            if (!fromDestructor)
            {
                GC.SuppressFinalize(this);
            }

            
        }

        ~Peakcan()
        {
            Dispose(true);
        }

        public bool IsOpen
        {
            get
            { 
                return _IsOpen;
            
            }
        }
    
    }

}
