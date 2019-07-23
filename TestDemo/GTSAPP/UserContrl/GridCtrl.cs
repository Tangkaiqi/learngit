using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;

namespace TestDemo.UserContrl
{

    /*
     * ************************************************
        网格可选控件 
        控件为可选网格控件，默认为9行13列（默认算上列头与行头）
        属性:列ColCount  行Rowcount 
     **************************************************
    */
    public partial class GridCtrl : UserControl
    {
        #region private variable
        private List<GridHole> _Grids = null;//网格集合
        private int m_Left;
        private int m_Top;
        private int m_Width;
        private int m_Height;
        private int _gridSpacing = 8;//表格间距默认为2
        private int _rowCount = 13;
        private int _colCount = 9;
        private Rectangle SketchpadPosition; //记录虚线框位置大小
        private Point StartLocation;//鼠标按下 开始坐标
        private Point EndLocation; //鼠标抬起 结束坐标
        private Rectangle CurrentPosition;
        private bool m_isdown = false; //记录鼠标是否已经按下
        private IntPtr hdc;
        #endregion


        /// <summary>
        /// 列
        /// </summary>
        public int ColCount
        {
            get { return _colCount; }
            set { _colCount = value; }
        }
        /// <summary>
        /// 类型  列：-1 行：1 内容格子：0 全选按钮：2
        /// </summary>
        public int Type { get; set; }

        public int GridSpacing
        {
            get { return _gridSpacing; }
            set { _gridSpacing = value; }
        }

        /// <summary>
        /// 行
        /// </summary>
        public int Rowcount
        {
            get { return _rowCount; }
            set { _rowCount = value; }
        }

        public Rectangle ClientRect
        {
            get { return new Rectangle(m_Left, m_Top, m_Width, m_Height); }

            set
            {
                m_Left = value.X;
                m_Top = value.Y;
                m_Width = value.Width;
                m_Height = value.Height;
            }
        }

        [Bindable(false), Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<GridHole> Grids
        {
            get { return _Grids; }
            set { _Grids = value; }
        }

        public GridCtrl()
        {
            InitializeComponent();
            InitializeEvent();
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.  
            SetStyle(ControlStyles.DoubleBuffer, true); // 双缓冲
        }

        private void InitializeEvent()
        {
            this.Load += GridCtrl_Load;
            panel1.MouseDown += Panel1_MouseDown;
            panel1.MouseUp += Panel1_MouseUp;
            panel1.MouseMove += Panel1_MouseMove;
            panel1.Paint += Panel1_Paint;
        }

        private void Panel1_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                if (Grids != null)
                {
                    for (int i = 0; i < Grids.Count; i++)
                    {
                        DrawGrids(e, Grids[i]);
                    }
                    e.Graphics.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        /// <summary>
        /// Panel1当做画板,画出网格
        /// </summary>
        /// <param name="e"></param>
        /// <param name="_gh"></param>
        private void DrawGrids(PaintEventArgs e, GridHole _gh)
        {
            GridHole gridhole = _gh;
            int type = gridhole.Type;
            Pen pen = new Pen(Color.Black);
            switch (type)
            {
                case -1:
                    {
                        pen.Width = 1.5f;
                        DrawRectangles(e.Graphics, pen, gridhole.Rect, gridhole.DridTitle);
                        break;
                    }
                case 0:
                    {
                        pen.Width = 1.0f;
                        DrawRectangles(e.Graphics, pen, gridhole.Rect, gridhole.IsSelected);
                        break;
                    }
                case 1:
                    {
                        pen.Width = 1.5f;
                        DrawRectangles(e.Graphics, pen, gridhole.Rect, gridhole.DridTitle);
                        break;
                    }
                case 2:
                    {
                        pen.Width = 1.5f;
                        DrawRectangles(e.Graphics, pen, gridhole.Rect, 2);
                        break;
                    }
            }
        }

        //画内容格子
        private void DrawRectangles(Graphics _g, Pen _pen, Rectangle _rect, bool iselected)
        {
            Graphics g = _g;
            Pen pen = _pen;
            Rectangle ClientRect = new Rectangle(_rect.Left, _rect.Top, _rect.Width, _rect.Height);
            SolidBrush brush = new SolidBrush(Color.White);
            g.DrawRectangle(pen, _rect);

            if (iselected)
            {
                brush.Color = Color.Red;
            }
            else
            {
                brush.Color = Color.White;
            }
            Rectangle RackClientRect = ClientRect;

            RackClientRect.Inflate(-1, -1);
            g.FillRectangle(brush, RackClientRect);

            brush.Dispose();
        }

        //画列头与行头
        private void DrawRectangles(Graphics _g, Pen _pen, Rectangle _rect, string _str)
        {
            Graphics g = _g;
            Pen pen = _pen;
            Rectangle rect = new Rectangle(_rect.X + 4, _rect.Y + 4, _rect.Width - 8, _rect.Height - 8);
            g.DrawRectangle(pen, rect);
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            g.DrawString(_str, new Font("宋体", rect.Width / 3, FontStyle.Bold), Brushes.Black, rect, sf);

        }

        //画全选按钮
        private void DrawRectangles(Graphics _g, Pen _pen, Rectangle _rect, int _type)
        {
            Graphics g = _g;
            Pen pen = _pen;
            g.DrawEllipse(pen, new Rectangle(_rect.X + 4, _rect.Y + 4, _rect.Width - 8, _rect.Height - 8));
        }

        /// <summary>
        /// ＂鼠标移动＂事件处理方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Panel1_MouseMove(object sender, MouseEventArgs e)
        {
            Thread.Sleep(6);
            if (m_isdown)
            {
                MyRectangle(StartLocation, EndLocation);
                MyRectangle(StartLocation, new Point(e.X, e.Y));
                EndLocation = new Point(e.X, e.Y);
                //BoxSelection(e.Location);
                //Console.WriteLine(SketchpadPosition.ToString());
                //Console.WriteLine(SketchpadPosition.Left+","+ SketchpadPosition.Right);
            }

        }
        //相交算法  计算垂直于水平坐标
        private bool CheckCross(Rectangle r1, Rectangle r2)
        {
            PointF c1 = new PointF(r1.Left + r1.Width / 2.0f, r1.Top + r1.Height / 2.0f);
            PointF c2 = new PointF(r2.Left + r2.Width / 2.0f, r2.Top + r2.Height / 2.0f);
            return (Math.Abs(c1.X - c2.X) <= r1.Width / 2.0 + r2.Width / 2.0 && Math.Abs(c2.Y - c1.Y) <= r1.Height / 2.0 + r2.Height / 2.0);
        }

        private void BoxSelection(Point endp)
        {
            CurrentPosition = new Rectangle(StartLocation.X, StartLocation.Y, endp.X - StartLocation.X, endp.Y - StartLocation.Y);
            Console.WriteLine(CurrentPosition.ToString());
            for (int i = 0; i < _Grids.Count; i++)
            {
                //Point[] arrposition = new Point[4];
                //arrposition[0] = new Point(_Grids[i].Rect.X, _Grids[i].Rect.Y);
                //arrposition[1] = new Point(_Grids[i].Rect.X + _Grids[i].Rect.Width, _Grids[i].Rect.Y);
                //arrposition[2] = new Point(_Grids[i].Rect.X, _Grids[i].Rect.Y + _Grids[i].Rect.Height);
                //arrposition[3] = new Point(_Grids[i].Rect.X + _Grids[i].Rect.Width, _Grids[i].Rect.Y + _Grids[i].Rect.Height);
                if (_Grids[i].IsSelected) continue;
                //if (CurrentPosition.Contains(arrposition[0]))
                //{
                //    _Grids[i].IsSelected = true;
                //    panel1.Invalidate(_Grids[i].Rect); continue;
                //}
                //if (CurrentPosition.Contains(arrposition[1]))
                //{
                //    _Grids[i].IsSelected = true;
                //    panel1.Invalidate(_Grids[i].Rect); continue;
                //}
                //if (CurrentPosition.Contains(arrposition[2]))
                //{
                //    _Grids[i].IsSelected = true;
                //    panel1.Invalidate(_Grids[i].Rect); continue;
                //}
                //if (CurrentPosition.Contains(arrposition[3]))
                //{
                //    _Grids[i].IsSelected = true;
                //    panel1.Invalidate(_Grids[i].Rect); continue;
                //}
                //if (arrposition[0].X + CurrentPosition.X - arrposition[0].X - CurrentPosition.X <= Xa2 - Xa1 + Xb2 - Xb1  && Yb2 + Yb1 - Ya2 - Ya1 | <= Y a2 - Ya1 + Yb2 - Yb1)
                if (CheckCross(CurrentPosition, _Grids[i].Rect))
                {
                    _Grids[i].IsSelected = true;
                    panel1.Invalidate(_Grids[i].Rect); continue;
                }
            }
        }

        /// <summary>
        /// ＂鼠标松开＂事件处理方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Panel1_MouseUp(object sender, MouseEventArgs e)
        {
            Thread.Sleep(6);
            m_isdown = false;
            MyRectangle(StartLocation, EndLocation);
            BoxSelection(e.Location);
        }

        /// <summary>
        /// ＂鼠标按下＂事件处理方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (!m_isdown)
            {
                m_isdown = true;
                StartLocation = new Point(e.X, e.Y);
                EndLocation = new Point(e.X, e.Y);
                hdc = Apis.GetDC(this.panel1.Handle);
                Apis.SetROP2(hdc, R2.R2_NOTCOPYPEN);
            }
            else
            {
                MyRectangle(StartLocation, EndLocation);
                MyRectangle(StartLocation, new Point(e.X, e.Y));
                Apis.ReleaseDC(this.panel1.Handle, hdc);
            }

            #region 鼠标按下网格处理代码

            bool isclick = false;
            if (e.Button == MouseButtons.Left)
            {
                for (int i = 0; i < Grids.Count; i++)
                {
                    Point mouseLocation = e.Location;

                    if ((Grids[i].Type == -1 || Grids[i].Type == 2 || Grids[i].Type == 1) && Grids[i].Rect.Contains(mouseLocation))
                    {
                        for (int k = 0; k < Grids.Count; k++)
                        {
                            bool isSelected = SelectedRowAndCol(Grids[i].Type, Grids[i].DridLocation, Grids[k].DridLocation);
                            if (isSelected)
                            {
                                Grids[k].IsSelected = true;
                                isclick = true;
                                panel1.Invalidate(Grids[k].Rect);
                            }
                            else
                            {
                                if (Grids[k].IsSelected)
                                {
                                    Grids[k].IsSelected = false;
                                    panel1.Invalidate(Grids[k].Rect);
                                }
                            }
                        }
                    }

                    if (Grids[i].Type == 0)
                    {
                        if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
                        {
                            if (Grids[i].Rect.Contains(mouseLocation))
                            {
                                Grids[i].IsSelected = true;
                                panel1.Invalidate(Grids[i].Rect);
                            }
                        }
                        else
                        {

                            if (Grids[i].Rect.Contains(mouseLocation))
                            {
                                Grids[i].IsSelected = true;
                                isclick = false;
                                panel1.Invalidate(Grids[i].Rect);
                            }
                            else
                            {
                                if (isclick)
                                {
                                    continue;
                                }
                                if (Grids[i].IsSelected)
                                {
                                    Grids[i].IsSelected = false;
                                    panel1.Invalidate(Grids[i].Rect);
                                }
                            }
                        }
                    }

                }

            }
            #endregion
        }

        private void MyRectangle(Point startP, Point endP)
        {
            Apis.MoveToEx(hdc, startP.X, startP.Y, IntPtr.Zero);
            //Apis.LineTo(hdc, pt2.X, pt2.Y);
            SketchpadPosition = new Rectangle(startP.X, startP.Y, endP.X, endP.Y);
            Apis.Rectangle(hdc, SketchpadPosition.X, SketchpadPosition.Y, SketchpadPosition.Width, SketchpadPosition.Height);
        }

        /// <summary>
        /// 计算鼠标点击的是几行几列(先行后列)
        /// </summary>
        /// <param name="_int">网格类型</param>
        /// <param name="_str1">选中的网格类型</param>
        /// <param name="_str2">网格行列</param>
        /// <returns></returns>
        private bool SelectedRowAndCol(int _int, string _str1, string _str2)
        {
            bool isSelected = false;
            string[] arrstr = new string[2];
            string intcol = null, introw = null;
            if (_str2.IndexOf("|") > 0)
            {
                arrstr = _str2.Split('|');
                introw = arrstr[0];
                intcol = arrstr[1];
            }
            if (_int == -1)
            {
                isSelected = (_str1 == intcol) ? true : false;
            }
            if (_int == 1)
            {
                isSelected = (_str1 == introw) ? true : false;
            }
            if (_int == 2)
            {
                isSelected = true;
            }
            return isSelected;
        }

        /// <summary>
        /// 控件加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridCtrl_Load(object sender, EventArgs e)
        {
            _Grids = new List<GridHole>();
            CreateGrid();
        }

        /// <summary>
        /// 初始化网格
        /// </summary>
        private void CreateGrid()
        {
            this.panel1.BackColor = Color.White;
            int RowH, ColW, _gridSpacing = 6;
            ColW = ((this.panel1.Width) / _rowCount) - _gridSpacing;
            RowH = ((this.panel1.Height) / _colCount) - _gridSpacing;
            m_Left = 0;
            m_Top = 0;
            GridHole gridhole = new GridHole();
            gridhole.Type = 2;
            gridhole.Rect = new Rectangle(m_Left, m_Top, ColW, RowH);
            gridhole.DridLocation = "0";
            _Grids.Add(gridhole);
            //列头
            for (int i = 1; i < _rowCount; i++)
            {
                gridhole = new GridHole();
                gridhole.Type = -1;
                gridhole.DridTitle = i.ToString();
                m_Left += ColW + _gridSpacing;
                gridhole.Rect = new Rectangle(m_Left, m_Top, ColW, RowH);
                gridhole.DridLocation = i.ToString();
                _Grids.Add(gridhole);
            }
            m_Left = 0;
            //行头
            for (int i = 1; i < _colCount; i++)
            {
                gridhole = new GridHole();
                gridhole.Type = 1;
                gridhole.DridTitle = i.ToString();
                m_Top += RowH + _gridSpacing;
                gridhole.Rect = new Rectangle(m_Left, m_Top, ColW, RowH);
                gridhole.DridLocation = i.ToString();
                _Grids.Add(gridhole);
            }

            m_Left = 0;
            m_Top = 0;

            int RowLocation = 1, ColLocation = 1;

            for (int i = 0; i < (_rowCount - 1) * (_colCount - 1); i++)
            {
                if (i % (_rowCount - 1) == 0 && i == 0)
                {
                    m_Left += ColW + _gridSpacing;
                    m_Top = RowH + _gridSpacing;
                    gridhole = new GridHole();
                    gridhole.Type = 0;
                    gridhole.Rect = new Rectangle(m_Left, m_Top, ColW, RowH);
                    string Loca = "1" + "|" + ColLocation.ToString();
                    gridhole.DridLocation = Loca;
                    _Grids.Add(gridhole);
                }
                else
                {
                    if (i % (_rowCount - 1) == 0)
                    {
                        m_Left = 0; m_Top += RowH + _gridSpacing; RowLocation += 1; ColLocation = 0;
                    }
                    m_Left += ColW + _gridSpacing;
                    gridhole = new GridHole();
                    gridhole.Type = 0;
                    gridhole.Rect = new Rectangle(m_Left, m_Top, ColW, RowH);
                    ColLocation++;
                    string Loca = RowLocation.ToString() + "|" + ColLocation.ToString();
                    gridhole.DridLocation = Loca;
                    _Grids.Add(gridhole);
                }
            }
        }
    }


    [Serializable]
    public class GridHole
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public Rectangle Rect { get; set; }
        public bool IsSelected { get; set; }
        /// <summary>
        /// 类型  -1 列 0 内容 1行 2全选
        /// </summary>
        public int Type { get; set; }

        public string DridTitle { get; set; }

        public string DridLocation { get; set; }
    }

    public enum R2 : int
    {
        R2_BLACK,//像素始终为0。
        R2_COPYPEN,//像素是笔的颜色。
        R2_MASKNOTPEN, //像素是屏幕和笔的反转共有颜色的组合。
        R2_MASKPEN,  //像素是笔和屏幕共有的颜色组合。
        R2_MASKPENNOT,  //像素是笔和屏幕反面共有的颜色组合。
        R2_MERGENOTPEN,  //像素是屏幕颜色和笔颜色的倒数的组合。
        R2_MERGEPEN,  //像素是笔颜色和屏幕颜色的组合。
        R2_MERGEPENNOT,  //像素是笔颜色和屏幕颜色的倒数的组合。
        R2_NOP,  //像素保持不变。
        R2_NOT, // 像素是屏幕颜色的反转。
        R2_NOTCOPYPEN,  //像素是笔颜色的倒数。
        R2_NOTMASKPEN,  //像素是R2_MASKPEN颜色的反转。
        R2_NOTMERGEPEN,  //像素是R2_MERGEPEN颜色的反转。
        R2_NOTXORPEN,  //像素是R2_XORPEN颜色的反转。
        R2_WHITE, //像素总是1。
        R2_XORPEN //像素是笔和屏幕中颜色的组合，但两者都不是。
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
        public static extern IntPtr Rectangle(IntPtr hdc, int left, int top, int right, int bottom);
    }
}
