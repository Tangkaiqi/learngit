using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;

using System.Windows.Forms;

namespace TestReader.Modules
{
    public enum TCommandCode : byte
    {
        CMD_INFO_1              = 0x00,
        CMD_INFO_2              = 0x01,
        CMD_INFO_3              = 0x02,
        CMD_INFO_4              = 0x03,
        CMD_INFO_5              = 0x04,
        CMD_INFO_6              = 0x05,
        CMD_QUERY_NODE          = 0x08,
        CMD_IDENTIFY_NODE       = 0x09,
        CMD_SET_ADDR            = 0x0A,
        CMD_SET_DEF_ADDR        = 0x0B,
        CMD_SET_GROUP           = 0x0C,
        CMD_EXEC_GROUP          = 0x0D,
        CMD_INIT                = 0x0E,
        CMD_INFO_ALL            = 0x0F,
        CMD_SET_SHARE_EEPROM    = 0x10,
        CMD_GET_SHARE_EEPROM    = 0x11,
        CMD_SET_SW_EEPROM       = 0x12,
        CMD_GET_SW_EEPROM       = 0x13,
        CMD_SET_FW_EEPROM       = 0x14,
        CMD_GET_FW_EEPROM       = 0x15,
        CMD_RESTORE_EEPROM      = 0x16,
        CMD_DOWNLOAD_START      = 0x17,
        CMD_DOWNLOAD            = 0x18,
        CMD_UPLOAD              = 0x19,
        CMD_UPLOAD_EX           = 0x1A,
        CMD_RESET               = 0x1B,
        CMD_SET_PORT            = 0x1C,
        CMD_GET_PORT            = 0x1D,
        CMD_GO_BOOT_LOADER      = 0x1E,
        CMD_GO_MAIN_FW          = 0x1F,
        CMD_GET_MEM             = 0x20,
        CMD_SET_MEM             = 0x21,
        CMD_COM_PORT_STATUS     = 0x22,
        CMD_COM_PORT_IO         = 0x23,
        CMD_GET_OTHER_INFO      = 0x24,
        CMD_EXTEND_CODE         = 0x2F
    }


    public enum TCommandEvent: Byte
    {
        CV_MOUDLE_GENGERL       = 0x01,
        CV_MODULE_STATE         = 0x02,
        CV_MODULE_PARAM         = 0x04,
        CV_MODULE_ERROR         = 0X08,
        CV_MODULE_WARNING       = 0X10,
        CV_MODULE_PORT          = 0X20,
        CV_MODULE_DETECT        = 0X40
    }

    /// <summary>
    /// 串口的配置
    /// </summary>
    public struct TPortCfgParam
    {
        public byte Port;      //串口号
        public byte StopBit;   //停止位
        public byte CheckBit;  //校验位  0:None1 ODD 2:EVEN
        public byte DataBit;   //数据位
        public byte BaudIndex; //波特率索引号

    }


    /************************************************************************/
    /* 硬件基类模块                                                               */
    /************************************************************************/
    public delegate void ModuleEventHandler(TCommandEvent EventID, uint id, string code);

    

    public delegate bool WriteParamWorker(byte[] Buff, TDataAreaType DataType);

    public delegate bool ReadParamWorker(TDataAreaType DataType);

    //读写EEPROM时的委托通知事件
    public delegate void ReadWriteNotifyEvent(bool IsOk, int mode, byte[] Param);

    public class TBaseModule:IDisposable
    {
        private WorkThread _WorkThread;

        private delegate bool AsyncSendWorker(string Msg, bool NeedWaitFinished);  //自定义的委托

        private ManualResetEvent _ReponseEvent = new ManualResetEvent(false);
        private ManualResetEvent _FinishedEvent = new ManualResetEvent(true);
        private ManualResetEvent _PrepareEvent = new ManualResetEvent(true);

        //这是一个线程安全的队列
        private ConcurrentQueue<TCANDATA> _MsgQueue = new ConcurrentQueue<TCANDATA>();
        private static CancellationTokenSource cancelTokenSource = new CancellationTokenSource();

        //public object SyncObject;

        //private int _SendTime = 0;
        //private int _RecTime;

        private Byte[] _SendCmd;
        private TModuleState _ModuleState;

         
        private uint _NodeID;
        private string _ModuleName;
        private string _SerialNo;
        private string _FireWareVer;
        private Byte _ModuleIndex;
        private Byte _ErrorCode;
        private Byte _WaringCode;
        private TModuleType _ModuleType;
        private Byte _ExecMask;

        private bool _PortOpened;
        private TPortCfgParam _PortCfg;

        private BitArray _State = new BitArray(8);
        
        private int _HaveState = 0;

        //private AsyncSendWorker Dlgt;
        //private IAsyncResult Ar;

        private WriteParamWorker WriteDlgt;
        private IAsyncResult WriteAr;

        private ReadParamWorker ReadDlgt;
        private IAsyncResult ReadAr;

        //private byte[] _DataBuff;
        private StringBuilder _DataBuff = new StringBuilder(2048);

        public event ModuleEventHandler WorkHander;

        //读写硬件参数完成后的事件
        public event ReadWriteNotifyEvent OnReadWriteFinished;

        public uint NodeID { get { return _NodeID; } }
        public string ModuleName { get { return _ModuleName; } }
        public string SerialNo { get { return _SerialNo; } }
        public string FireWareVer { get { return _FireWareVer; } }
        public Byte ModuleIndex { get { return _ModuleIndex; } }
        public Byte ErrorCode { get { return _ErrorCode; } }
        public Byte WaringCode { get { return _WaringCode; } }
        public TModuleType ModuleType { get { return _ModuleType; } }
        public Byte ExecMask { get { return _ExecMask; } }
        public TModuleState ModuleState { get { return _ModuleState; } }


        /*
        public uint NodeID { get { return _NodeID; } }
        public string ModuleName { get { return _ModuleName; } }
        public string SerialNo { get { lock (SyncObject) return _SerialNo; } }
        public string FireWareVer { get { lock (SyncObject) return _FireWareVer; } }
        public Byte ModuleIndex { get { lock (SyncObject) return _ModuleIndex; } }
        public Byte ErrorCode { get { lock (SyncObject) return _ErrorCode; } }
        public Byte WaringCode { get { lock (SyncObject) return _WaringCode; } }
        public TModuleType ModuleType { get { lock (SyncObject) return _ModuleType; } }
        public Byte ExecMask { get { lock (SyncObject) return _ExecMask; } }
        public TModuleState ModuleState { get { lock (SyncObject) return _ModuleState; } }
        */

        public TBaseModule(uint ID, string Name)
        {
            //SyncObject = new object();

            _NodeID = ID;
            _ModuleName = Name;
            _ModuleType = TModuleType.mtNone;
            _ExecMask = 0;

            _ModuleState = TModuleState.msNone;

            _PortOpened = false;

            _WorkThread = WorkThread.CreateInstance();
            _WorkThread.AddModuleToList(this);

            //Dlgt = new AsyncSendWorker(SendMsgMethod);

            WriteDlgt = new WriteParamWorker(WriteDataToEEPRom);
            ReadDlgt = new ReadParamWorker(ReadDataFromEEPRom);

            _SendCmd = new byte[8];

            //_SendCmd = System.Text.Encoding.Unicode.GetBytes();

            //处理该模块硬件响应数据的线程
            //Task.Factory.StartNew(DataHandlerMethod, cancelTokenSource.Token);
        }

        private int _disposed;
        public void Dispose()
        {

            cancelTokenSource.Cancel();

            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (Interlocked.CompareExchange(ref _disposed, 1, 0) != 0)
            {
                return;
            }
            if (disposing)
            {
                Destroy();
            }
        }

        virtual protected void Destroy()
        {
            // 
        }

        ~TBaseModule()
        {
            Dispose(false);
        }

        //查询硬件节点是否存在
        public void QueryModuleExist()
        {
            byte[] Cmd = new byte[2 + SerialNo.Length];

            Cmd[0] = (byte)TCommandCode.CMD_QUERY_NODE;
            Cmd[1] = (byte)ModuleIndex;


            byte[] s = GetSerialNo(SerialNo);

            for (int i = 0; i < s.Length; i++)
            {
                Cmd[i + 2] = s[i];
            }

            _WorkThread.AddMsgToQueue(0, Cmd);  //广播码

        }

        //验证模块是否存在，发送命令后，如果该模块可以运动则往复运动
        public void IdentifyModule()
        {
            byte[] Cmd = new byte[2 + SerialNo.Length];

            Cmd[0] = (byte)TCommandCode.CMD_IDENTIFY_NODE;
            Cmd[1] = (byte)ModuleIndex;

            byte[] s = GetSerialNo(SerialNo);

            for (int i = 0; i < s.Length; i++)
            {
                Cmd[i + 2] = s[i];
            }

            _WorkThread.AddMsgToQueue(0, Cmd);  //广播码
        }

        //修改模块的ID
        public bool SetModuleID(Byte NewID, bool IsTemp = false)
        {
            byte[] Cmd = new byte[2 + SerialNo.Length];

            Cmd[0] = IsTemp ? (byte)TCommandCode.CMD_SET_ADDR : (byte)TCommandCode.CMD_SET_DEF_ADDR;
            Cmd[1] = (byte)NewID;
            byte[] s = GetSerialNo(SerialNo);

            for (int i = 0; i < s.Length; i++)
            {
                Cmd[i + 2] = s[i];
            }

            SendMsg(Cmd);

            //todo 修改模块的ID有可能失败，可能没有应答返回数据，所以需要判断是否超时失败
            return WaitReponse();

            
 
        }

        //设置运行组掩码
        public void SetExecuteMask(Byte Mask)
        {
            byte[] Cmd = new byte[2];

            Cmd[0] = (byte)TCommandCode.CMD_SET_GROUP;
            Cmd[1] = (byte)Mask;

            _ExecMask = Mask;

            SendMsg(Cmd);
            // WaitReponse();
            //WaitAsyncFinished();
        }



        public bool GetModuleInfo(TModuleInfoFrame InfoFrame)
        {
            byte[] code = new byte[2];

            code[0] = (byte)TCommandCode.CMD_EXTEND_CODE;

            if ((InfoFrame &  TModuleInfoFrame.mfOne) == TModuleInfoFrame.mfOne)
            {
                code[1] = (byte)TCommandCode.CMD_INFO_1;
                SendMsg(code);

                if (!WaitReponse()) return false;
            }

            if ((InfoFrame & TModuleInfoFrame.mfTwo) == TModuleInfoFrame.mfTwo)
            {
                code[1] = (byte)TCommandCode.CMD_INFO_2;
                SendMsg(code);

                if (!WaitReponse()) return false;
            }

            if ((InfoFrame & TModuleInfoFrame.mfThree) == TModuleInfoFrame.mfThree)
            {

                code[1] = (byte)TCommandCode.CMD_INFO_3;
                SendMsg(code);

                //if (!WaitReponse()) return false;

            }
            if ((InfoFrame & TModuleInfoFrame.mfFour) == TModuleInfoFrame.mfFour)
            {
                code[1] = (byte)TCommandCode.CMD_INFO_4;
                SendMsg(code);

                //if (!WaitReponse()) return false;

            }

            if ((InfoFrame & TModuleInfoFrame.mfFive) == TModuleInfoFrame.mfFive)
            {
                code[1] = (byte)TCommandCode.CMD_INFO_5;
                SendMsg(code);

                //if (!WaitReponse()) return false;

            }

            if ((InfoFrame & TModuleInfoFrame.mfSix) == TModuleInfoFrame.mfSix)
            {
                code[1] = (byte)TCommandCode.CMD_INFO_6;
                SendMsg(code);

                //if (!WaitReponse()) return false;
            }

            return true;
        }


        //执行预存命令或广播执行本模块的预存指令
        public void ExecutePrepare(bool IsBoardcast = true)
        {
            byte[] Cmd = new byte[2];

            Cmd[0] = (byte)TCommandCode.CMD_EXEC_GROUP;
            Cmd[1] = (byte)ExecMask;

            if (!IsBoardcast)
                _WorkThread.AddMsgToQueue(NodeID, Cmd);
            else
                _WorkThread.AddMsgToQueue(0, Cmd);  //广播码
        }

        //模块进行初始化，如果该模块可以运动则回到原点位置
        public virtual void InitModule(TExecuteMode mode, ushort Speed, ushort Ramp)
        {
            byte[] Cmd = new byte[6];

            Cmd[0] = (byte)TCommandCode.CMD_INIT;
            Cmd[1] = (mode == TExecuteMode.emExecute) ? (byte)0 : (byte)0x80;
            Cmd[2] = (byte)(Speed & 0xFF);  //低字节
            Cmd[3] = (byte)(Speed >> 8);    //高字节
            Cmd[4] = (byte)(Ramp & 0xFF);   //低字节
            Cmd[5] = (byte)(Ramp >> 8);     //高字节

            SendMsg(Cmd);

            //需要等待初始化动作完成
        }



        //写数据到硬件的EEPROM中
        private bool WriteDataToEEPRom(byte[] Buff, TDataAreaType DataType)
        {
            byte[] Cmd = new byte[8];

            string Msg = string.Empty;

            switch (DataType)
            {
                case TDataAreaType.drShare:
                    Cmd[0] = (byte)TCommandCode.CMD_SET_SHARE_EEPROM;

                    break;
                case TDataAreaType.drSwShare:
                    Cmd[0] = (byte)TCommandCode.CMD_SET_SW_EEPROM;
                    break;
                default:
                    Cmd[0] = (byte)TCommandCode.CMD_SET_FW_EEPROM;
                    break;

            }

            ushort Len = (ushort)Buff.Length;  //<65535

            byte CRC = Globals.CrcCheck(Buff, Len);

            Cmd[1] = (byte)0x00;            //地址低
            Cmd[2] = (byte)0x00;            //地址高
            Cmd[3] = (byte)0x04;            //数据长度

            Cmd[4] = (byte)CRC;
            Cmd[5] = (byte)(Len & 0xFF);
            Cmd[6] = (byte)((Len & 0xFF00) >> 8);
            Cmd[7] = (byte)0x85;

            SendMsg(Cmd);

            if (!WaitReponse()) return false;


            ushort I = 0;
            ushort Address = 0x04;
            while (Len > 0)
            {

                for (int i = 1; i < 8; i++)
                {
                    Cmd[i] = 0x00;
                }


                Cmd[1] = (byte)(Address & 0xFF);
                Cmd[2] = (byte)((Address & 0xFF00) >> 8);

                switch (Len)
                {
                    case 1:
                        Cmd[3] = (byte)0x01;            //数据长度
                        Cmd[4] = (byte)Buff[I];
                        break;
                    case 2:
                        Cmd[3] = (byte)0x02;            //数据长度
                        Cmd[4] = (byte)Buff[I];
                        Cmd[5] = (byte)Buff[I+1];
                        break;
                    case 3:
                        Cmd[3] = (byte)0x03;            //数据长度
                        Cmd[4] = (byte)Buff[I];
                        Cmd[5] = (byte)Buff[I + 1];
                        Cmd[6] = (byte)Buff[I + 2];

                        break;
                    default:
                        Cmd[3] = (byte)0x04;            //数据长度
                        Cmd[4] = (byte)Buff[I];
                        Cmd[5] = (byte)Buff[I + 1];
                        Cmd[6] = (byte)Buff[I + 2];
                        Cmd[7] = (byte)Buff[I + 3];
                        break;
                }

                SendMsg(Cmd);
                if (!WaitReponse()) return false;


                if (Len >= 4)
                {
                    I += 4;
                    Len -= 4;
                    Address += 4;

                }
                else break;
            }

            return true;
        }

        //读写EEPROM完成后的回调事件
        private void ReadWriteCallBack(IAsyncResult iAr)
        {
            if (OnReadWriteFinished!= null)
            {
                AsyncResult ar = (AsyncResult)iAr;

                int P = (int)iAr.AsyncState;
                //int p = Convert.ToInt16(iAr.AsyncState);

                byte[] Value;
                bool IsOk;

                if (P == 0)
                {
                    Value = new byte[1];
                    WriteParamWorker temp = (WriteParamWorker)ar.AsyncDelegate; //获取委托的引用

                    IsOk = temp.EndInvoke(iAr);  //获取委托执行后的结果(执行是否成功)
                }else
                {
                    ReadParamWorker temp = (ReadParamWorker)ar.AsyncDelegate;
 
                    IsOk = temp.EndInvoke(iAr);  //获取委托执行后的结果(执行是否成功)

                    Value = new byte[_DataBuff.Length];

                    Value = System.Text.Encoding.Unicode.GetBytes(_DataBuff.ToString());

                }

                OnReadWriteFinished(IsOk, P, Value);  //通知
            }
        }


        //异步写数据到EEPROM中
        public void SyncWriteDataToEEPRom(byte[] Buff, TDataAreaType DataType)
        {
            WriteAr = WriteDlgt.BeginInvoke(Buff, DataType, ReadWriteCallBack, 0);
                
        }

        //轮循方式等待异步写数据完成
        public void WaitSyncWriteDataFinish()
        {
            while (!WriteAr.IsCompleted)
            {
                Thread.Sleep(1);

                //DoEvents有副作用，当把主界面阻塞时（如弹出式菜单、或按住滚动条）
                //则导致消息不能处理而影响动作的执行
                if (IsMainThread)
                    Application.DoEvents();   //?????????????
            }
        }


        //从硬件的EEPROM中读取数据
        private bool ReadDataFromEEPRom(TDataAreaType DataType)
        {
            //_DataBuff.Clear();

            byte[] Cmd = new byte[4];

            string Msg = string.Empty;

            switch (DataType)
            {
                case TDataAreaType.drShare:
                    Cmd[0] = (byte)TCommandCode.CMD_GET_SHARE_EEPROM;

                    break;
                case TDataAreaType.drSwShare:
                    Cmd[0] = (byte)TCommandCode.CMD_GET_SW_EEPROM;
                    break;
                default:
                    Cmd[0] = (byte)TCommandCode.CMD_GET_FW_EEPROM;
                    break;
            }

            Cmd[1] = (byte)0x00;            //地址低
            Cmd[2] = (byte)0x00;            //地址高
            Cmd[3] = (byte)0x04;            //数据长度

            SendMsg(Cmd);

            if (!WaitReponse()) return false;

            if (_DataBuff.Length < 3) return false;

            byte[] bytes = System.Text.Encoding.Unicode.GetBytes(_DataBuff.ToString());

            ushort DataLen = (ushort)(bytes[1] + (bytes[2] << 8));

            if (DataLen == 0xFFFF) return false;


            ushort Address = 0x04;
            ushort I = 0;

            _DataBuff.Clear();

            while(I < DataLen)
            {
                if ((DataLen - I) >= 6)
                {
                    Cmd[1] = (byte)(Address & 0xFF);
                    Cmd[2] = (byte)((Address & 0xFF00) >> 8);
                    Cmd[3] = (byte)0x06;            //数据长度
                }
                else
                {
                    Cmd[1] = (byte)(Address & 0xFF);
                    Cmd[2] = (byte)((Address & 0xFF00) >> 8);
                    Cmd[3] = (byte)(DataLen - I);  //数据长度
                }

                SendMsg(Cmd);
                if (!WaitReponse()) return false;

                I += 6;

                Address += 6;
            }

            return true;
        }


        /// <summary>
        /// 异步从硬件中读取硬件设置参数
        /// </summary>
        /// <param name="DataType"></param>
        public void SyncReadDataFromEEPRom(TDataAreaType DataType)
        {
            ReadAr = ReadDlgt.BeginInvoke(DataType, ReadWriteCallBack, 1);
        }


        //轮循方式等待异步读数据完成
        public void WaitSyncReadDataFinish()
        {
            while (!ReadAr.IsCompleted)
            {
                Thread.Sleep(1);

                //DoEvents有副作用，当把主界面阻塞时（如弹出式菜单、或按住滚动条）
                //则导致消息不能处理而影响动作的执行
                if (IsMainThread)
                    Application.DoEvents();   //?????????????
            }
        }


        //写Fireware程序到硬件
        private void WriteFireToHardWare(UInt32 Address, byte[] Buff, byte Kind = 0)
        {

        }

        //读取硬件的Fireware程序
        private void ReadFireFromHardware(UInt32 Address, ref byte[] Buff, byte Kind = 0)
        {



        }




        //读取硬件的参数数据项
        public virtual void ReadModuleParam(TDataAreaType DataType)
        {




        }

        //写硬件配置参数到硬件
        public virtual void WriteModuleParam(TDataAreaType DataType)
        {


        }


        //设置硬件的相应区域的参数到默认设置
        public void RestoreDataToDefault(TDataAreaType DataType)
        {


        }

        //设置硬件对应端口的状态数据
        public void SetIOPortState(byte Port, byte Data)
        {
            byte[] Cmd = new byte[3];
            Cmd[0] = (byte)TCommandCode.CMD_SET_PORT;
            Cmd[1] = Port;
            Cmd[2] = Data;

            SendMsg(Cmd);
            //WaitAsyncFinished();

        }

        //读取硬件对应端口的状态数据
        public void GetIOPortState(byte Port, ref string Data)
        {
            byte[] Cmd = new byte[2];
            Cmd[0] = (byte)TCommandCode.CMD_GET_PORT;
            Cmd[1] = (byte)Port;

            _DataBuff.Clear();

            SendMsg(Cmd);
            if (WaitReponse())
            {
                Data = _DataBuff.ToString();
            } else
            {
                Data = "";
            }
            //WaitAsyncFinished();

            
        }

        /// <summary>
        /// 打开串口
        /// </summary>
        /// <param name="cfgParam">串口的配置</param>
        /// <param name="IsAutoUp">是否自动上传串口的数据</param>
        public void OpenPort(TPortCfgParam cfgParam, bool IsAutoUp = true)
        {
            if (!_PortOpened)
            {
                byte[] Cmd = new byte[4];
                Cmd[0] = (byte)TCommandCode.CMD_COM_PORT_STATUS;
                Cmd[1] = (byte)0x00;

                byte byte3, byte4;

                if (IsAutoUp)
                {
                    byte3 = (byte)((cfgParam.Port << 5) + (cfgParam.StopBit << 3) + (cfgParam.CheckBit << 1) + 1);
                }else
                {
                    byte3 = (byte)((cfgParam.Port << 5) + (cfgParam.StopBit << 3) + (cfgParam.CheckBit << 1));
                }

                byte4 = (byte)((cfgParam.DataBit << 4) + cfgParam.BaudIndex);

                Cmd[2] = (byte)byte3;
                Cmd[3] = (byte)byte4;

                SendMsg(Cmd);

                _PortOpened = true;
                _PortCfg = cfgParam;
            }
        }

        /// <summary>
        /// 从串口读取数据
        /// </summary>
        public void ReadDataFromPort()
        {
            if (_PortOpened)
            {
                byte[] Cmd = new byte[2];
                Cmd[0] = (byte)TCommandCode.CMD_COM_PORT_IO;

                byte byte1 = (byte)((_PortCfg.Port << 5) + 1);
                Cmd[1] = (byte)byte1;

                SendMsg(Cmd);
            }
        }

        /// <summary>
        /// 向串口写数据
        /// </summary>
        /// <param name="Buff"></param>
        public bool WriteDataToPort(byte[] Buff)
        {
            if (_PortOpened)
            {
                int DataSize = Buff.Length;

                int bitvalue = 0;
                byte Bytes;

                byte[] Cmd;

                _DataBuff.Clear();

                while (DataSize > 0)
                {

                    if (DataSize > 6)
                    {

                        Cmd = new byte[8];
                        Cmd[0] = (byte)TCommandCode.CMD_COM_PORT_IO;

                        Cmd[1] = (byte)((_PortCfg.Port << 5) + (0x06 << 2));

                        for (int i = 0; i < 6; i++)
                        {
                            Cmd[i + 2] = (byte)Buff[bitvalue++];
                        }
                        DataSize -= 6;

                    }else
                    {
                        Bytes = (byte)DataSize;

                        Cmd = new byte[2 + Bytes];
                        Cmd[0] = (byte)TCommandCode.CMD_COM_PORT_IO;
                        Cmd[1] = (byte)((_PortCfg.Port << 5) + (Bytes << 2));

                        for (int i = 0; i < Bytes; i++)
                        {
                            Cmd[i + 2] = (byte)Buff[bitvalue++];
                        }
                        DataSize -= Bytes;
                    }
 
                    SendMsg(Cmd);

                    if (!WaitReponse()) break;
                }

                return true;
            }else
            {
                return false;
            }
        }

        /// <summary>
        /// 获取串口返回的数据
        /// </summary>
        /// <returns></returns>
        public string GetPortData()
        {
            return _DataBuff.ToString();
        }



        //硬件的返回数据处理方法
        public virtual void DoMsg(byte[] Msg)
        {
            int Cnt;

            string MsgData;
            //下位机有响应数据返回

            if (_SendCmd != null)
            {
                if ((_SendCmd[0] == Msg[0]) ||
                    ((_SendCmd[0] == (byte)TCommandCode.CMD_EXTEND_CODE)
                    && (Msg[0] == _SendCmd[1])))
                {
                    //_RecTime = Environment.TickCount;
                    _ReponseEvent.Set();  //事件置为有信号
                }
            }


            switch (Msg[0])
            {
                case (byte)TCommandCode.CMD_INFO_1:
                    _ModuleIndex = Msg[1];
                    _SerialNo    = ConvertSerail(Msg, 2);
                    break;
                case (byte)TCommandCode.CMD_INFO_2:
                    try
                    {
                        SetModuleState(Msg);

                        //_ModuleState = TModuleState.msNone;
                        //for (int i = 0; i < 8; i++)
                        //{
                        //    _State.Set(i, (Msg[1] & (1 << i)) > 0);
                        //}
                        ////
                        //if (_State.Get(0)) _ModuleState = _ModuleState | TModuleState.msInited;
                        //if (_State.Get(1)) _ModuleState = _ModuleState | TModuleState.msPrepared;
                        //if (_State.Get(2)) _ModuleState = _ModuleState | TModuleState.msActive;
                        //if (_State.Get(3)) _ModuleState = _ModuleState | TModuleState.msHasErr;
                        //if (_State.Get(4)) _ModuleState = _ModuleState | TModuleState.msHasWarn;
                        //if (_State.Get(5)) _ModuleState = _ModuleState | TModuleState.msMoving;
                        //if (_State.Get(6)) _ModuleState = _ModuleState | TModuleState.msDetect;
                        //if (_State.Get(7)) _ModuleState = _ModuleState | TModuleState.msData;

                        //if (((_ModuleState & TModuleState.msHasErr) == TModuleState.msHasErr) ||
                        //    ((_ModuleState & TModuleState.msHasWarn) == TModuleState.msHasWarn))
                        //{
                        //    _ErrorCode = Msg[2];
                        //    _WaringCode = Msg[3];

                        //    MsgData = System.Text.Encoding.Unicode.GetString(Msg);

                        //    DoHander(TCommandEvent.CV_MODULE_ERROR, NodeID, MsgData);
                        //}
                        //else
                        //{
                        //    _ErrorCode = 0;
                        //    _WaringCode = 0;
                        //}

                    }
                    catch(NullReferenceException e)
                    {
                        LogHelper.WriteLog(e.ToString());
                    }

                    break;
                case (byte)TCommandCode.CMD_INFO_3:
                    _ModuleType = GetModuleType(Msg[1]);  //模块的类型
                    _FireWareVer = System.Text.Encoding.Unicode.GetString(Msg, 2, 6); //Fireware版本号

                    break;
                case (byte)TCommandCode.CMD_INFO_4:
                    _ExecMask = Msg[1];
                    break;
                case (byte)TCommandCode.CMD_GET_SHARE_EEPROM:
                case (byte)TCommandCode.CMD_UPLOAD:
                case (byte)TCommandCode.CMD_UPLOAD_EX:
                case (byte)TCommandCode.CMD_GET_MEM:
                case (byte)TCommandCode.CMD_GET_SW_EEPROM:
                case (byte)TCommandCode.CMD_GET_FW_EEPROM:
                    if (Msg[1] == 0)
                    {
                        MsgData = System.Text.Encoding.Unicode.GetString(Msg);
                        DoHander(TCommandEvent.CV_MODULE_ERROR, NodeID, MsgData);
                    }
                       
                    else
                    {
                        MsgData = System.Text.Encoding.Unicode.GetString(Msg, 2, 6);
                        _DataBuff.Append(MsgData);
                    }
                    break;
                case (byte)TCommandCode.CMD_GET_PORT:
                    MsgData = System.Text.Encoding.Unicode.GetString(Msg, 1, 7);
                    _DataBuff.Append(MsgData);

                    break;
                case (byte)TCommandCode.CMD_COM_PORT_IO:
                    if ((Msg[1] & 0x01) != 0x01)
                    {
                        MsgData = System.Text.Encoding.Unicode.GetString(Msg);
                        DoHander(TCommandEvent.CV_MODULE_ERROR, NodeID, MsgData);
                    }
                        
                    else if ((Msg[1] & 0x02) != 0x02)
                    {
                        Cnt = (Msg[2] & 0x1C) >> 2;
                        MsgData = System.Text.Encoding.Unicode.GetString(Msg,2, Cnt);
                        _DataBuff.Append(MsgData);
                        DoHander(TCommandEvent.CV_MODULE_PORT, NodeID, _DataBuff.ToString());
                    }
                    break;
                default:
                    if (Enum.IsDefined(typeof(TCommandCode), Msg[0]))
                    {
                        if (Msg[1] == 0)
                        {
                            MsgData = System.Text.Encoding.Unicode.GetString(Msg);
                            DoHander(TCommandEvent.CV_MODULE_ERROR, NodeID, MsgData);
                        }
                            
                    }
                    break;

            }
        }

        protected void DoHander(TCommandEvent EventID, uint id, string code)
        {
            if (WorkHander != null)
            {
                WorkHander(EventID, id, code);
            }
        }


        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        //发送数据到缓冲队列，不等待硬件的响应直接返回
        public void SendMsg(byte[] Msg)
        {
#if SIMU
            return;
#else
            //暂停时阻塞发送过程

            if (Globals.g_SendEvent.WaitOne(Timeout.Infinite))
            {

                if (Globals.g_AbortEvent.WaitOne(1)) return;

                _ReponseEvent.Reset();
                _FinishedEvent.Reset();
                _PrepareEvent.Reset();

                Interlocked.Exchange(ref _HaveState, 0);

                Array.Copy(Msg, _SendCmd, Msg.Length);

                //发送命令到线程的队列中
                _WorkThread.AddMsgToQueue(NodeID, Msg);
            }


#endif

        }


        public void ResetFinishedEvent()
        {
            _FinishedEvent.Reset();
        }

        //指令发送到硬件后，等待硬件的响应应答返回
        public bool WaitReponse()
        {
#if SIMU
            return true;
#else

            if (Globals.g_AbortEvent.WaitOne(1)) return true;

            return _ReponseEvent.WaitOne(Globals.REPONSE_TIME_OUT);
#endif
        }


        //发送动作指令后等待动作完成
        public bool WaitFinished(int Timeout)
        {
#if SIMU
            return true;
#else
            if (Globals.g_AbortEvent.WaitOne(1)) return true;

            return _FinishedEvent.WaitOne(Timeout);
#endif
        }

        //等待预备指令发送后，预备信息帧返回

        //等待预备的运动动作已经处于预备状态，如果下位机还没有预备好，直接发送广播执行指令可能导致动作不能执行
        //这个异常有可能出现吗？，发送预备指令后，不等待响应信息上传，上位机立即发送执行指令
        //下位机不是串行执行指令的吗？
        public bool WaitPrepareFinished(int Timeout = 2000)
        {
#if SIMU
            return true;
#else
            if (Globals.g_AbortEvent.WaitOne(1)) return true;

            return _PrepareEvent.WaitOne(Timeout);
#endif
        }


        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        //委托调用方法
        private bool SendMsgMethod(byte[] Msg, bool NeedWaitFinished = false)
        {
#if SIMU
            return true;
#else
            SendMsg(Msg);
            bool NoTimeout = true;

            //等待下位机的应答响应
            //该方法是在委托线程中执行的，不会阻塞UI主线程
            NoTimeout = _ReponseEvent.WaitOne(500);

            //等待动作执行完成
            if (NeedWaitFinished)
            {
                _FinishedEvent.WaitOne();
            }

            return NoTimeout;
#endif
        }


        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        //使用异步发送指令，线程不停的产生又释放是否有性能上的问题?????
        //这是个异步委托调用，不会阻塞调用线程，调用后等待响应信息返回或者等待动作完成
        //必须在等待上一个指令硬件有响应数据返回后才能再调用
        //在发送下一个指令前必须调用WaitAsyncFinished等待异步调用函数完成
        public void SendMsgAsync(string Msg, bool NeedWaitFinished = false)
        {
            //如果动作正在执行，则不能发送下一个指令

            //委托方法的执行是在调用线程中执行的吗？---是在新的线程中执行的
            //异步执行
            //Ar = Dlgt.BeginInvoke(Msg, NeedWaitFinished, null, null); 

        }

        //该方法会阻塞调用线程，如果在主线程中调用会阻塞主线程
        //应该在线程中调用
        //该方法是等待指令有响应数据返回或动作指令发送后动作执行完成
        //等待异步委托执行完成
        public void WaitAsyncFinished()
        {
            //while (!Ar.IsCompleted)
            //{
            //    Thread.Sleep(1);

            //    //DoEvents有副作用，当把主界面阻塞时（如弹出式菜单、或按住滚动条）则导致消息不能处理而影响动作的执行
            //    if (IsMainThread)
            //        Application.DoEvents();   //?????????????
            //}
        }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        //async await 编译指令在C# 5.0才有 .net4.5以上
       /*public async Task AsyncSendMsg(string Msg)
        {
            _ReponseEvent.Reset();
            _FinishedEvent.Reset();
            _PrepareEvent.Reset();

            await Task.Run(() =>
            {
                _SendTime = Environment.TickCount;
                _SendCmd = System.Text.Encoding.Unicode.GetBytes(Msg);

                //发送命令到线程的队列中
                _WorkThread.AddMsgToQueue(NodeID, Msg);

                //等待下位机的应答响应
                _ReponseEvent.WaitOne(2000);
            }
            );
        }

        */
        //--------------------------------------------------------------------------------------------

        //调用子类的实现，在子类中对子类的属性进行赋值完后，再调用父类中实现
        protected virtual void SetModuleState(byte[] RecMsg)
        {
            _ModuleState = TModuleState.msNone;

            byte State = RecMsg[1];

            string MsgData;

            for(int i = 0; i<8; i++)
            {
                _State.Set(i, (State & (1 << i)) > 0);
            }
            //
            if (_State.Get(0)) _ModuleState = _ModuleState | TModuleState.msInited;
            if (_State.Get(1)) _ModuleState = _ModuleState | TModuleState.msPrepared;
            if (_State.Get(2)) _ModuleState = _ModuleState | TModuleState.msActive;
            if (_State.Get(3)) _ModuleState = _ModuleState | TModuleState.msHasErr;
            if (_State.Get(4)) _ModuleState = _ModuleState | TModuleState.msHasWarn;
            if (_State.Get(5)) _ModuleState = _ModuleState | TModuleState.msMoving;
            if (_State.Get(6)) _ModuleState = _ModuleState | TModuleState.msDetect;
            if (_State.Get(7)) _ModuleState = _ModuleState | TModuleState.msData;

            if (((_ModuleState & TModuleState.msHasErr) == TModuleState.msHasErr) ||
                ((_ModuleState & TModuleState.msHasWarn) == TModuleState.msHasWarn))
            {
                _ErrorCode = RecMsg[2];
                _WaringCode = RecMsg[3];

                MsgData = System.Text.Encoding.Unicode.GetString(RecMsg);

                DoHander(TCommandEvent.CV_MODULE_ERROR, NodeID, MsgData);
            }
            else
            {
                _ErrorCode = 0;
                _WaringCode = 0;
            }


            if (((_ModuleState & TModuleState.msActive) == TModuleState.msActive) ||
                ((_ModuleState & TModuleState.msPrepared) == TModuleState.msPrepared))
            {
              //  _FinishedEvent.Reset();
                Interlocked.Exchange(ref _HaveState, 1);    //表示已经收到忙碌信息帧上传
            }

            if ((_ModuleState & TModuleState.msPrepared) == TModuleState.msPrepared)
            {
                _PrepareEvent.Set();   //有预备完成响应指令上传
            }

            int t = 0;
            Interlocked.Exchange(ref t, _HaveState);
            //模块不处于运动状态
            //该帧是空闲的帧并且上帧是忙碌帧，表示是由忙碌转为了空闲（动作执行完成)
            if (((_ModuleState & TModuleState.msActive) != TModuleState.msActive ) && 
                    ((_ModuleState & TModuleState.msPrepared) != TModuleState.msPrepared) && (t==1)) 
            {
                _FinishedEvent.Set();  //完成事件置为有信号
            }
        }


        private TModuleType GetModuleType(Byte T)
        {

            switch (T)
            {
                case 0x01:
                    return TModuleType.mtXMotor;
                case 0x02:
                    return TModuleType.mtYZMotor;
                case 0x03:
                    return TModuleType.mtPump;
                case 0x04:
                    return TModuleType.mtShaker;
                case 0x05:
                    return TModuleType.mtWasher;
                case 0x06:
                    return TModuleType.mtCentrifuge;
                default:
                     return TModuleType.mtNone;
            }

        }

        private string ConvertSerail(Byte[] b, int Index)
        {
            string serail = string.Empty;
            for (int i = Index; i<b.Length; i++)
            {
                serail += Convert.ToString(b[i], 16);
            }
            return serail;
        }

        private byte[] GetSerialNo(string s)
        {
            byte[] b = new byte[s.Length];

            
            //把16进制的字符串字符转为10进制
            for(int i = 0; i< s.Length; i++)
            {
                b[i] = Convert.ToByte(s.Substring(i, 1),  16);
            }

            return b;
        }

        public static bool IsMainThread
        {
            get { return System.Threading.Thread.CurrentThread.ManagedThreadId == Globals.MAIN_THREAD_ID; }
        }

        public void AddMsgToQueue(uint ID, byte[] Msg)
        {
            TCANDATA CanMsg = new TCANDATA(Msg.Length);
            CanMsg.ID = ID;
            Array.Copy(Msg, CanMsg.Data, Msg.Length);
            _MsgQueue.Enqueue(CanMsg);
        }



        //线程执行函数
        private void DataHandlerMethod()
        {
            TCANDATA MsgData = new TCANDATA();

            while (!cancelTokenSource.IsCancellationRequested)
            {
                Thread.Sleep(1);

                if (_MsgQueue.TryDequeue(out MsgData))
                {
                    //一个线程在对模块的属性进行处理，另外的线程在读取模块的属性，
                    //需要进行同步处理,如获取马达位置属性时，不锁定则获取位置错误

                    //lock (SyncObject)
                    //{
                        DoMsg(MsgData.Data);
                    //}
                    
                }
            }
        }




    }
}
