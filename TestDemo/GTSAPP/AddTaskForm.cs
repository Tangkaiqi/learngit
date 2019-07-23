using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestDemo.Common;
using TestDemo.UserContrl;

namespace TestDemo
{
    public partial class AddTaskForm : Form
    {
        public AddTaskForm()
        {
            InitializeComponent();
            InitializeStyle();
            InitializeFunction();
        }

        private void InitializeStyle()
        {

        }

        private void InitializeFunction()
        {
            panel1.MouseDown += Panel1_MouseDown; ;
            panel1.MouseUp += Panel1_MouseUp; ;
            panel1.MouseMove += Panel1_MouseMove; ;

            panel1.Paint += Panel1_Paint;
        }

        //定义两个变量 
        bool MouseIsDown = false;
        Rectangle MouseRect = Rectangle.Empty; //矩形（为鼠标画出矩形选区）

        #region mouseMove
        //定义三个方法
        private void ResizeToRectangle(object sender, Point p)
        {
            DrawRectangles(sender);
            MouseRect.Width = p.X - MouseRect.Left;
            MouseRect.Height = p.Y - MouseRect.Top;
            DrawRectangles(sender);
        }
        private void DrawRectangles(object sender)
        {
            Rectangle rect = ((Panel)sender).RectangleToScreen(MouseRect);
            ControlPaint.DrawReversibleFrame(rect, Color.White, FrameStyle.Dashed);
        }

        private void DrawStart(object sender, Point StartPoint)
        {
            ((Panel)sender).Capture = true;
            Cursor.Clip = ((Panel)sender).RectangleToScreen(((Panel)sender).Bounds);
            MouseRect = new Rectangle(StartPoint.X, StartPoint.Y, 0, 0);
        }

        private void Panel1_MouseDown(object sender, MouseEventArgs e)
        {
            MouseIsDown = true;
            DrawStart(sender, e.Location);
        }

        private void Panel1_MouseUp(object sender, MouseEventArgs e)
        {
            if (MouseIsDown)
                ResizeToRectangle(sender, e.Location);

            foreach (Control button in ((Panel)sender).Controls)
            {
                if (MouseRect.IntersectsWith(button.Bounds)) //相交( MouseRect.Contains  完全包含)
                {
                    button.BackColor = Color.Blue;
                }
            }

            this.Capture = false;
            Cursor.Clip = Rectangle.Empty;
            MouseIsDown = false;
            DrawRectangles(sender);
            MouseRect = Rectangle.Empty;
        }

        private void Panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (MouseIsDown)
                ResizeToRectangle(sender, e.Location);


            foreach (Control button in ((Panel)sender).Controls)
            {
                if (MouseRect.IntersectsWith(button.Bounds)) //相交( MouseRect.Contains  完全包含)
                {
                    button.BackColor = Color.Blue;
                    //Graphics g= button.CreateGraphics();
                    //Pen pen = new Pen(Color.Red);
                    //Rectangle rec = new Rectangle(button.Location.X-1 ,button.Location.Y-1,button.Width-2,button.Height-2);
                    //g.DrawRectangle(pen,rec);
                }
                else
                {
                    button.BackColor = Color.White;
                }
                //if (!MouseRect.IntersectsWith(button.Bounds))
                //{
                //    button.BackColor = Color.White;
                //}
            }

        }
        #endregion

        private void Panel1_Paint(object sender, PaintEventArgs e)
        {
            using (Graphics g = e.Graphics)
            {
                Pen pen = new Pen(Color.FromArgb(128, 128, 128));
                pen.Width = 1.0f;
                Point p1 = new Point(58, 39);
                Point p2 = new Point(685 + intWidth + 3, 39);
                g.DrawLine(pen, p1, p2); //上
                p1 = new Point(58, 39 + intHeight * 8 + 7 * 4+4);
                p2 = new Point(685 + intWidth + 3, 39 + intHeight * 8 + 7 * 4 +4);
                g.DrawLine(pen, p1, p2); //下
                p1 = new Point(58, 39);
                p2 = new Point(58, 39 + intHeight * 8 + 7 * 4+4);
                g.DrawLine(pen, p1, p2); //左
                p1 = p2 = new Point(685 + intWidth + 3, 39);
                p2 = new Point(685 + intWidth + 3, 39 + intHeight * 8 + 7 * 4 +4);
                g.DrawLine(pen, p1, p2); //右

                g.Dispose();
            }
        }

        private void AddTaskForm_Load(object sender, EventArgs e)
        {
            AddTable();
        }

        private void AddTable()
        {
            AddCol();//列
            AddRow();//行

            lstTable = new List<TablesInfo>();
            int _x = 0;
            int _y = 0;
            int intRecord = 0;
            for (int i = 0; i < 96; i++)
            {
                if (i % 12 == 0 && i == 0)
                {
                    _x = 59;
                    _y = 40;
                }
                else
                {
                    if (i % 12 == 0)
                    {
                        _x = 59;
                        _y += intHeight + 4;
                    }
                    else
                    {
                        _x += intWidth + 4;
                    }
                }
                SquareControl SquControl = new SquareControl();
                TablesInfo tbleEs = new TablesInfo();
                tbleEs.BgColor = Color.White;
                tbleEs.X = _x;
                tbleEs.Y = _y;
                tbleEs.SWidth = 53;
                tbleEs.SHeight = 38;
                tbleEs.ColFla = false;
                tbleEs.RowFla = false;
                tbleEs.AllCheckedFla = false;
                tbleEs.CheckedFla = false;
                tbleEs.CenterTable = true;
                tbleEs.StrTitle = "";
                SquControl.TablesInfoEs = tbleEs;
                panel1.Controls.Add(SquControl);
                lstTable.Add(tbleEs);
            }
        }

        /// <summary>
        /// 添加列头
        /// </summary>
        private void AddCol()
        {
            lstCol = new List<SquareControl>();
            int index = 0;
            for (int i = 0; i < 13; i++)
            {
                if (i == 0)
                {
                    SquareControl SquControl = new SquareControl();
                    TablesInfo tbleEs = new TablesInfo();
                    tbleEs.BgColor = Color.Transparent;
                    tbleEs.X = 3;
                    tbleEs.Y = 5;
                    tbleEs.SWidth = 42;
                    tbleEs.SHeight = 27;
                    tbleEs.ColFla = true;
                    tbleEs.RowFla = false;
                    tbleEs.AllCheckedFla = true;
                    tbleEs.CheckedFla = false;
                    tbleEs.StrTitle = "";
                    SquControl.TablesInfoEs = tbleEs;
                    panel1.Controls.Add(SquControl);
                    lstCol.Add(SquControl);
                }
                else
                {
                    //intX =(intWidth *13 +24 ) + (intWidth - 42);
                    index += intWidth + 5;
                    SquareControl SquControl = new SquareControl();
                    TablesInfo tbleEs = new TablesInfo();
                    tbleEs.BgColor = Color.Transparent;
                    tbleEs.X = index;
                    tbleEs.Y = 5;
                    tbleEs.SWidth = 42;
                    tbleEs.SHeight = 27;
                    tbleEs.ColFla = true;
                    tbleEs.RowFla = false;
                    tbleEs.AllCheckedFla = false;
                    tbleEs.CheckedFla = false;
                    tbleEs.StrTitle = i.ToString();
                    SquControl.TablesInfoEs = tbleEs;
                    panel1.Controls.Add(SquControl);
                    lstCol.Add(SquControl);
                }
            }
        }
        /// <summary>
        /// 添加行头
        /// </summary>
        private void AddRow()
        {
            lstRow = new List<SquareControl>();
            int index = 0;

            for (int i = 0; i < 8; i++)
            {
                SquareControl SquControl = new SquareControl();
                TablesInfo tbleEs = new TablesInfo();
                tbleEs.BgColor = Color.Transparent;
                tbleEs.X = 3;
                tbleEs.Y = index + intHeight;
                tbleEs.SWidth = 42;
                tbleEs.SHeight = 27;
                tbleEs.ColFla = false;
                tbleEs.RowFla = true;
                tbleEs.AllCheckedFla = false;
                tbleEs.StrTitle = character[i].ToString();
                SquControl.TablesInfoEs = tbleEs;
                panel1.Controls.Add(SquControl);
                lstRow.Add(SquControl);
                index += intHeight + 4;
            }
        }

        /// <summary>
        /// 列头
        /// </summary>
        private List<SquareControl> lstCol = null;
        private List<SquareControl> lstRow = null;
        private List<TablesInfo> lstTable = null;

        private int intX = 0, intY = 0, intSpacing = 3, intWidth = 53, intHeight = 38, intSpac = 4;
        private int ColH = 27, RowW = 42, spg = 14;
        private char[] character = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H' };
    }
}
