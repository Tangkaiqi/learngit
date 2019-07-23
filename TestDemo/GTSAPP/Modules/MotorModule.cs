using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;

namespace TestReader.Modules
{

    //马达命令
    public enum TMotorCommand: byte
    {
        CMD_MOTOR_MOVE          = 0x30,
        CMD_MOTOR_GET_IO        = 0x31,
        CMD_MOTOR_SET_POWER     = 0x32,
        CMD_MOTOR_GET_POWER     = 0x33
    }


    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct TPowerRecord
    {
        public Byte FreePower;
        public Byte KeepPower;
        public Byte MovePower;
    }



    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct TDCMotorParam {
        public UInt16 CodecLine;        //编码器线数
        public Byte CounterDirect;      //编码器计数方向
        public Byte AutoFix;            //是否自动修正
        public UInt16 FixErrLine;       //修正误差线数
        public Byte FixErrCount;        //修正次数
        public UInt16 LoseStep;         //初始化编码器误差步数
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct TMotorParam
    {
        public Byte CoordDirect;            //坐标方向
        public Byte InitDirect;             //初始化运动方向
        public Byte SwitchMode;             //光电开关模式
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public TPowerRecord[] MotorPower;   //马达力矩
        public UInt16 HomeOffs;             //原点偏移量 有符号数
        public UInt16 SwitchDistance;       //光电开关有效距离
        public UInt32 MaxDistance;          //马达的最大行程
        public UInt16 DefSpd;               //默认速度
        public UInt16 DefRamp;              //默认加速度
        public UInt16 DefScanSpd;           //默认探测速度
        public UInt16 DefInitSpd;           //默认初始化速度
        public UInt16 DefInitRamp;          //默认初始化加速度
        public Byte LqdDetect;              //是否有液面探测功能
        public Byte DetectMode;             //探测模式
        public Byte DetectBack;             //探测后回退距离
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public Byte[] Scaling;               //比例尺
        ////4V马达专用参数
        public TDCMotorParam DCParam;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 40)]
        public Byte[] IOStatus;
    }

    /// <summary>
    /// 马达硬件类
    /// </summary>
    public class TMotorModule:TBaseModule
    {

        private int _Pos1 = 0;
        private int _Pos2 = 0;
        private int _Pos3 = 0;
        private bool _IRState = false;


        //马达的绝对位置值
        public int Pos1
        {
            get
            {

                    return _Pos1;
                   

            }


        }

        //相对位置值
        public int Pos2
        {

            get
            {
                return _Pos2;
            }
        }

        //码盘值
        public int Pos3 { get { return _Pos3; } }

        //光电开关的状态 1： 被挡住 0：没有被挡住（这取决于光电开关的极性）
        public bool IRState { get { return _IRState; } }



        public TMotorModule(uint ID, string Name):base(ID, Name)
        {

        }


        protected override void SetModuleState(byte[] RecMsg)
        {

            _Pos1 = Globals.ConvertValue(RecMsg[4], RecMsg[5]);
            _Pos2 = Globals.ConvertValue(RecMsg[6], RecMsg[7]);

            base.SetModuleState(RecMsg);
        }


        public override void DoMsg(byte[] MsgByte)
        {

            base.DoMsg(MsgByte);

            string MsgData;

            switch (MsgByte[0])
            {

                case (byte)TCommandCode.CMD_INFO_4:

                   long  T = (MsgByte[7] << 24) | (MsgByte[6] << 16) | (MsgByte[5] << 8) | MsgByte[4];
                   long  V = 0xFFFFFFFF;
                    //最高位是1则表明是负值
                    if ((MsgByte[7] >> 7) == 1)
                        _Pos3 = (int)-(T ^ V + 1);
                    else
                        _Pos3 = (int) T;

                    break;
                case (byte)TMotorCommand.CMD_MOTOR_MOVE:
                    if (MsgByte[1] == 0)
                    {
                        MsgData = System.Text.Encoding.Unicode.GetString(MsgByte);
                        DoHander(TCommandEvent.CV_MODULE_ERROR, NodeID, MsgData);
                    }
                    break;
                case (byte)TMotorCommand.CMD_MOTOR_GET_IO:
                    if (MsgByte.Length == 2)  //主动查询后的响应数据
                    {
                        _IRState = (MsgByte[1] != 0);
                    }else if (MsgByte.Length == 4)
                    {
                        //MsgByte[2] : 0 close状态 1：Open状态
                        DoHander(TCommandEvent.CV_MODULE_STATE, NodeID, Convert.ToString(MsgByte[2]));
                    }
                    break;
                case (byte)TMotorCommand.CMD_MOTOR_SET_POWER:

                    break;
                case (byte)TMotorCommand.CMD_MOTOR_GET_POWER:

                    break;
            }

        }

        public override void InitModule(TExecuteMode mode, UInt16 Speed, UInt16 Ramp)
        {

            base.InitModule(mode, Speed, Ramp);
        }

        //Distance: -32768~32767
        public bool MotorMove(TExecuteMode ExecuteMode, TMoveMode MoveMode, TMovePowerGrp MovePower,
                short Distance, UInt16 Speed, UInt16 Ramp)
        {

            //当前马达所在的坐标与需要移动到的位置的误差在误差范围内则认为是马达不需要移动
            if (Globals.g_CheckRange)
            {
                //如果是相对移动
                if (MoveMode == TMoveMode.MOVE_ABS)
                {
                    //判断距离值是否在误差允许范围内
                    if (Math.Abs(_Pos1 - Distance) <= Globals.ALLOW_DEV)
                    {
                        return true;
                    }
                }
            }

            byte[] Cmd = new byte[8];

            //0为执行状态，0x80为预备状态
            byte Byte1 = (ExecuteMode==TExecuteMode.emExecute) ? (byte) 0 :(byte) 0x80;

            switch(MoveMode)
            {
                case TMoveMode.MOVE_REL:
                    Byte1 += 0x01;//0x81
                    break;
                case TMoveMode.MOVE_SCAN:
                    Byte1 += 0x02;//0x82
                    break;
                case TMoveMode.MOVE_AS_HOME:
                    Byte1 += 0x03;//0x83
                    break;
                case TMoveMode.MOVE_SCAN_REL:
                    Byte1 += 0x04;//0x84
                    break;
            }

            switch (MovePower)
            {
                case TMovePowerGrp.MOVE_POWER1:
                    Byte1 += 0x10;
                    break;
                case TMovePowerGrp.MOVE_POWER2:
                    Byte1 += 0x20;
                    break;
                case TMovePowerGrp.MOVE_POWER3:
                    Byte1 += 0x40;
                    break;
                        
            }



            int Dis = Globals.ConvertDistance(Distance);

            Cmd[0] = (byte)TMotorCommand.CMD_MOTOR_MOVE;
            //移动的情况
            Cmd[1] = (byte)Byte1;
            Cmd[2] = (byte)(Dis & 0x00FF);  //低字节
            Cmd[3] = (byte)(Dis >> 8);      //高字节
            Cmd[4] = (byte)(Speed & 0xFF);
            Cmd[5] = (byte)(Speed >> 8);
            Cmd[6] = (byte)(Ramp & 0xFF);
            Cmd[7] = (byte)(Ramp >> 8);


            SendMsg(Cmd);

            return true;
            //发送运动指令后需要等待运动完成，在调用线程中等待运动结束


        }


        public bool MotorMove(TExecuteMode ExecuteMode, TMoveMode MoveMode, short Distance)
        {
            return  MotorMove(ExecuteMode, MoveMode, TMovePowerGrp.MOVE_POWER_DEF, Distance, 0, 0);
        }

        public bool CheckIRState()
        {
            byte[] Cmd = new byte[1];
            Cmd[0] = (byte)TMotorCommand.CMD_MOTOR_GET_IO;

            _IRState = false;

            SendMsg(Cmd);
            WaitReponse();

            return _IRState;
            //WaitAsyncFinished();
        }




    }
}
