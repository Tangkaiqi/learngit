using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TestReader.Modules
{

    public struct TReaderCoord
    {
        public int X;
        public int Y;
    }

    class MdrReaderEx
    {
        private TReaderCoord _First;
        private TReaderCoord _Last;

        private int _TrayDistance;


        private TMotorModule _XMotor;
        private TMotorModule _YMotor;


        public int XPos
        {
            get
            {
              return  _XMotor.Pos1;
            }
        }

        public int YPos
        {
            get
            {
                return _YMotor.Pos1;
            }
        }


        public MdrReaderEx()
        {

            _XMotor = new TMotorModule(0x2C, "XMotor");
            _YMotor = new TMotorModule(0x2B, "YMotor");


            LoadCoord();
        }
        
        private void LoadCoord()
        {

        }

        public void InitMotorAll()
        {
            _XMotor.InitModule(TExecuteMode.emExecute, 0, 0);
            _YMotor.InitModule(TExecuteMode.emExecute, 0, 0);

            _XMotor.WaitFinished(5000);
            _YMotor.WaitFinished(5000);
        }

        public void InitXMotor()
        {
            _XMotor.InitModule(TExecuteMode.emExecute, 0, 0);
            _XMotor.WaitFinished(5000);
        }

        public void InitYMotor()
        {
            _YMotor.InitModule(TExecuteMode.emExecute, 0, 0);
            _YMotor.WaitFinished(5000);
        }
        private void  GetHoleCoord(int HoleIndex, ref TReaderCoord HoleCoord)
        {
          //HoleCoord.X =1000;
          



        }
        public void MoveToHole(int HoleIndex)
        {
            TReaderCoord currCoord;
            currCoord.X = 0;
            currCoord.Y = 0;
            GetHoleCoord(HoleIndex, ref currCoord);

            _XMotor.MotorMove(TExecuteMode.emExecute, TMoveMode.MOVE_ABS, (short) currCoord.X);
            _YMotor.MotorMove(TExecuteMode.emExecute, TMoveMode.MOVE_ABS, (short)currCoord.Y);

            _XMotor.WaitFinished(10000);
            _YMotor.WaitFinished(10000);
        }


        public void MoveTrayOut()
        {

        }

        public void MoveTrayIn()
        {

        }

        public void MoveX(int X)
        {
            _XMotor.MotorMove(TExecuteMode.emExecute, TMoveMode.MOVE_REL, (short)X);
            _XMotor.WaitFinished(10000);
        }
        public void MoveY(int Y)
        {
            _YMotor.MotorMove(TExecuteMode.emExecute, TMoveMode.MOVE_REL, (short)Y);
            _YMotor.WaitFinished(10000);
        }

        public void SnapImage(ref string FileName)
        {

        }

    }
}
