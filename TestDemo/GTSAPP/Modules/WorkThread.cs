using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace TestReader.Modules
{

    public enum TWorkMode
    {
        mmLoadMode      = 0x01,    //从配置文件中加载对应的硬件节点（必须保证硬件节点是存在的）
        mmSearchMode    = 0x02     //发送广播码搜索节点后创建对应的节点
    }

    public struct TCANDATA
    {
       public  uint   ID;

        public  byte[] Data;
        public int DataLen;

        public TCANDATA(int Size)
        {
            ID = 0;
            DataLen = Size;
            Data = new byte[Size];
        }
    }


    /// <summary>
    /// 调度硬件模块与CAN进行通讯的工作类
    /// </summary>

    public class WorkThread : IDisposable
    {
        private volatile static WorkThread _Instance = null;
        private ICom _CanObj;
        private static readonly object lockHelper = new object();

        //这是一个线程安全的队列
        private ConcurrentQueue<TCANDATA> _MsgQueue = new ConcurrentQueue<TCANDATA>();

        //未考虑重复节点的问题,使用泛型中的SortList
        private SortedList<uint, TBaseModule> _ModuleList;

        //private TWorkMode _RunMode = TWorkMode.mmLoadMode;

        private static CancellationTokenSource cancelTokenSource = new CancellationTokenSource();

        private static CancellationTokenSource cancelTokenSource2 = new CancellationTokenSource();

        public bool IsConnected
        {
            get
            {
                return ((ZLGCan)_CanObj).IsOpen;
            }
        }


        private WorkThread()
        {

#if PEAKCAN
            _CanObj = Peakcan.CreateInstance();
#else
            _CanObj = ZLGCan.CreateInstance();
#endif

            _ModuleList = new SortedList<uint, TBaseModule>();

            //写线程
            Task.Factory.StartNew(WriteCanBusThread, cancelTokenSource.Token);

            //读线程
            Task.Factory.StartNew(ReadCanBusThread, cancelTokenSource2.Token);
        }


        public void Dispose()
        {
            cancelTokenSource.Cancel();

            //等待线程被释放


            //释放列表中的模块对象
            lock (lockHelper)
            {
                _ModuleList.Clear();
            }
            _CanObj.Dispose();


        }

        public static WorkThread CreateInstance()
        {
            if (_Instance == null)
            {
                lock (lockHelper)
                {
                    if (_Instance == null)
                        _Instance = new WorkThread();
                }
            }
            return _Instance;
        }

        public void AddMsgToQueue(uint ID, byte[] Msg)
        {
            //=========================
            TCANDATA CanData = new TCANDATA(Msg.Length);

            CanData.ID = ID;
            CanData.DataLen = Msg.Length;
            Array.Copy(Msg, CanData.Data, Msg.Length);

            _MsgQueue.Enqueue(CanData);

        }

        public void AddModuleToList(TBaseModule Module)
        {
            lock(lockHelper)
            {
                _ModuleList.Add(Module.NodeID, Module);

                //每个模块一个数据处理线程

                
            }
            
        }

        //按名称查找对应的模块
        public TBaseModule GetModuleFromList(string ModuleName)
        {
            lock (lockHelper)
            {
                TBaseModule M = null;

                foreach(uint Index in _ModuleList.Keys)
                {
                    M = _ModuleList[Index];

                    if (M.ModuleName.ToLower() == ModuleName.ToLower())
                    {
                        return M;
                    }
                }

                //没有找到对应的模块
                return null;

            }
        }


        private String GetInfoMsg(uint ID, byte[] CanMsg)
        {
            String Str = String.Empty;
            String s;

            for (int i = 0; i < CanMsg.Length; i++)
            {

                //s = Convert.ToString(CanMsg[i], 16);
                s = CanMsg[i].ToString("X2");

                //if (s.Length == 1) s = "0" + s;

                Str += " " + s;
            }

            Str = "[0x" + ID.ToString("X2") + "]" + Str;

            return Str;
        }


        //线程执行体，读写CAN总线
        private void WriteCanBusThread()
        {
            TCANDATA MsgData;

            while (!cancelTokenSource.IsCancellationRequested)
            {
                Thread.Sleep(1);

                //把数据写到总线上
                if (_MsgQueue.TryDequeue(out MsgData))
                {
                    bool IsSucc = _CanObj.WriteCanMsg(MsgData.ID, MsgData.Data);

#if (DEBUG)
                    string Loginfo;
                    if (IsSucc)
                    {
                        Loginfo = "Send:" + GetInfoMsg(MsgData.ID, MsgData.Data);
                        LogHelper.WriteLog(Loginfo);
                    }else
                    {
#if (!SIMU)
                        Loginfo = "Send Error:" + GetInfoMsg(MsgData.ID, MsgData.Data);
                        LogHelper.WriteLog(Loginfo);
#endif
                    }
#endif
                    }


            }
        }

        private void ReadCanBusThread()
        {

            byte[] Msg;


            TBaseModule Module;
            uint ID = 0;
            string Loginfo;


            while (!cancelTokenSource2.IsCancellationRequested)
            {
                Thread.Sleep(1);

                //从总线上读取数据，需要并发处理
                if (_CanObj.ReadCanMsg(ref ID, out Msg))
                {
                    //Todo 使用异步委托执行,在新的线程中执行
                    //但线程不停的创建并释放耗时
                    //思考：如果把数据处理部分放到线程池中处理，效率是否能够提升？

                    lock (lockHelper)
                    {
                        _ModuleList.TryGetValue(ID, out Module);

                        if (Module != null)
                        {
                            //如果其它线程也在操作该模块对象，如读取该模块的属性值，而本线程正在执行DoMsg，
                            //该如何同步该模块？
                            //这是个虚方法，如果被子类覆盖则调用子类的实现
                            //这个方法中不能有同步到主线程处理的动作，不然会阻塞CAN总线的读写

                            Module.DoMsg(Msg);

                            //Module.AddMsgToQueue(ID, Msg); //数据压入每个模块自己的处理队列中
#if DEBUG
                            Loginfo = "Rece:" + GetInfoMsg(ID, Msg);
                            LogHelper.WriteLog(Loginfo);
#endif

                        }


                    }


                }

            }


        }




    }
}
