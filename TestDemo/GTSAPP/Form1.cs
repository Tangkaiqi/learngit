using MdrApp.WinControl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestDemo
{

    /// <summary>
    /// 画虚线矩形 调用API画
    /// </summary>
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();           
            //激活双缓冲技术
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);


            button1.Click += button1_Click;
            button2.Click += button2_Click;
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        [DllImport("Gdi32.dll")]

        static extern IntPtr CreatePen(int fnPenStyle, int width, int color);

        [DllImport("Gdi32.dll")]

        static extern int SetROP2(System.IntPtr hdc, int rop);

        [DllImport("Gdi32.dll")]

        static extern int MoveToEx(IntPtr hdc, int x, int y, IntPtr lppoint);

        [DllImport("Gdi32.dll")]

        static extern int LineTo(IntPtr hdc, int X, int Y);

        [DllImport("Gdi32.dll")]

        static extern IntPtr SelectObject(IntPtr hdc, IntPtr obj);

        private void button1_Click(object sender, EventArgs e)
        {
            Graphics grfx = base.CreateGraphics();

            System.IntPtr hdc = grfx.GetHdc();

            //interop and good old GDI

            System.IntPtr hpen =
            CreatePen(0, 1, System.Drawing.ColorTranslator.ToWin32(Color.White));

            int rop = SetROP2(hdc, 5);

            IntPtr oldpen = SelectObject(hdc, hpen);

            MoveToEx(hdc, 20, 20, IntPtr.Zero);

            LineTo(hdc, 170, 20);

            LineTo(hdc, 170, 170);

            LineTo(hdc, 20, 170);

            LineTo(hdc, 20, 20);

            SelectObject(hdc, oldpen);

            SetROP2(hdc, rop);

            grfx.ReleaseHdc(hdc);

            //IntPtr hdc = Win32API.GetDC(Handle);//获取句柄
            //IntPtr rop = Win32API.SetROP2(hdc, (IntPtr)Win32API.R2_NOTXORPEN);//设置当前为异或画法 
            //IntPtr pen = (IntPtr)Win32API.CreatePen(Win32API.PS_DOT, 1, ColorTranslator.ToWin32(Color.Red));//创建画笔

            //IntPtr oldPen = Win32API.SelectObject(hdc, pen);//添加画笔


            //Win32API.MoveToEx(hdc,10, 10, 0);
            //Win32API.LineTo(hdc, 10, 10);

            //Win32API.SelectObject(hdc, oldPen);
            //Win32API.SetROP2(hdc, rop);//还原默认画法
            //Win32API.DeleteObject(pen);
            //Win32API.ReleaseDC(Handle, hdc);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Graphics grfx = base.CreateGraphics();

            Rectangle theRect = this.ClientRectangle;

            theRect.Inflate(-this.Width / 4, -this.Height / 4);

            grfx.FillRectangle(new SolidBrush(Color.Blue), theRect);
        }

        bool isStart = false;
        Point current;
        Point temp;
        IntPtr hdc;

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            isStart = false;
            this.MyRectangleDc(current, temp);
        }

        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (!isStart)
            {
                isStart = true;
                current = new Point(e.X, e.Y);
                temp = new Point(e.X, e.Y);
                hdc = Apis.GetDC(this.Handle);
                Apis.SetROP2(hdc, R2.R2_NOTXORPEN);
            }
            else
            {
                MyRectangle(current, temp);
                MyRectangle(current, new Point(e.X, e.Y));
                Apis.ReleaseDC(this.Handle, hdc);
            }
        }

        protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (isStart)
            {
                MyRectangle(current, temp);
                MyRectangle(current, new Point(e.X, e.Y));
                temp = new Point(e.X, e.Y);
            }
        }

        void MyRectangle(Point pt1, Point pt2)
        {
            Apis.MoveToEx(hdc, pt1.X, pt1.Y, IntPtr.Zero);
            //Apis.LineTo(hdc, pt2.X, pt2.Y);
            int left, top, right, bottom;
            left = pt1.X;
            top = pt1.Y;
            right = pt2.X;
            bottom = pt2.Y;
            Apis.Rectangle(hdc , left ,top, right, bottom);
        }
        void MyRectangleDc(Point pt1, Point pt2)
        {
            Apis.MoveToEx(hdc, pt1.X, pt1.Y, IntPtr.Zero);
            //Apis.LineTo(hdc, pt2.X, pt2.Y);
            int left, top, right, bottom;
            left = pt1.X;
            top = pt1.Y;
            right = pt2.X;
            bottom = pt2.Y;
            Apis.Rectangle(hdc, left, top, right, bottom);
        }

    }
    public enum R2 : int
    {
        R2_NOT = 6,
        R2_NOTXORPEN = 10
    }

    [StructLayout(LayoutKind.Sequential)]
    public class POINT
    {
        public int x;
        public int y;
    }

    public class Apis
    {
        [DllImport("USER32.DLL", EntryPoint = "GetDC")]
        public static extern IntPtr GetDC(IntPtr hwnd);
        [DllImport("USER32.DLL", EntryPoint = "ReleaseDC")]
        public static extern IntPtr ReleaseDC(IntPtr hwnd, IntPtr hdc);
        [DllImport("GDI32.DLL", EntryPoint = "SetROP2")]
        public static extern IntPtr SetROP2(IntPtr hdc, R2 nDrawMode);
        [DllImport("GDI32.DLL", EntryPoint = "MoveToEx")]
        public static extern IntPtr MoveToEx(IntPtr hdc, int x, int y, IntPtr lppt);
        [DllImport("GDI32.DLL", EntryPoint = "LineTo")]
        public static extern IntPtr LineTo(IntPtr hdc, int x, int y);

        [DllImport("GDI32.DLL", EntryPoint = "Rectangle")]
        public static extern IntPtr Rectangle(IntPtr hdc,int left,int top,int right,int bottom);
    }
}
