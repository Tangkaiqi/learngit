using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestDemo.Common;

namespace TestDemo
{
    public partial class TaskForm : Form
    {
        public TaskForm()
        {
            InitializeComponent();

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.  
            SetStyle(ControlStyles.DoubleBuffer, true); // 双缓冲
        }


        private void TaskForm_Load(object sender, EventArgs e)
        {
            panel1.MouseDown += Panel1_MouseDown;
            panel1.MouseUp += Panel1_MouseUp;
            panel1.MouseMove += Panel1_MouseMove;
            panel1.Paint += Panel1_Paint;
            panelCen.Paint += Panel2_Paint;
            LstObjresfanction();//加载行头的起始点
            LstObjresX();//加载列头起始点

            panelCen.MouseDown += PanelCen_MouseDown;
            panelCen.MouseUp += PanelCen_MouseUp;
            panelCen.MouseMove += PanelCen_MouseMove;

        }


        //定义两个变量 
        private bool MouseIsDown = false;
        private Rectangle MouseRect = Rectangle.Empty; //矩形（为鼠标画出矩形选区）
        private Point pointStart;
        private Point pointEnd;


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
            //((Panel)sender).Capture = true;
            //Cursor.Clip = ((Panel)sender).RectangleToScreen(((Panel)sender).Bounds);
            MouseRect = new Rectangle(StartPoint.X, StartPoint.Y, 0, 0);
        }

        private void PanelCen_MouseMove(object sender, MouseEventArgs e)
        {

            label1.Text = e.Location.ToString();

            Thread.Sleep(6);//减少cpu占用率
            if (MouseIsDown)
            {
                ResizeToRectangle(sender, e.Location);
                label2.Text = MouseRect.ToString();
                DrowMouseClick(e.Location);
            }
        }

        private void PanelCen_MouseUp(object sender, MouseEventArgs e)
        {
            this.Capture = false;
            Cursor.Clip = Rectangle.Empty;
            MouseIsDown = false;
            DrawRectangles(sender);
            MouseRect = Rectangle.Empty;
            pointEnd = e.Location;

        }

        private void PanelCen_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MouseIsDown = true;
                DrawStart(sender, e.Location);
                pointStart = e.Location;
                DrowMouseClick();
            }

        }

        private void DrowMouseClick()
        {
            Graphics g = panelCen.CreateGraphics();
            int x1 = 0;
            int y1 = 0;
            Point _wh = new Point();
            int x2 = 0;
            int y2 = 0;
            for (int i = 0; i < lstGdiRectangle.Count; i++)
            {
                Rectangle rec = lstGdiRectangle[i].Rec;
                if (lstGdiRectangle[i].CheckFlas)
                {
                    InitializeRec(lstGdiRectangle[i]);
                }
                x1 = rec.X;
                y1 = rec.Y;
                _wh = wh(rec);
                x2 = _wh.X;
                y2 = _wh.Y;
                if ((x1 < pointStart.X && pointStart.X < x2) && (y1 < pointStart.Y && pointStart.Y < y2))
                {
                    DrowCenter(lstGdiRectangle[i]);
                }
            }
            g.Dispose();
        }

        private void DrowMouseClick(Point p)
        {
            int x1 = 0;
            int y1 = 0;
            Point _wh = new Point();
            int x2 = 0;
            int y2 = 0;

            //计算矩形面积
            int fx = pointStart.X;
            int fy = pointStart.Y;
            int ex = p.X;
            int ey = p.Y;

            for (int i = 0; i < lstGdiRectangle.Count; i++)
            {
                Rectangle rec = lstGdiRectangle[i].Rec;
                x1 = rec.X;
                y1 = rec.Y;
                _wh = wh(rec);
                x2 = _wh.X;
                y2 = _wh.Y;
                if (((fx < x2 && x2 < ex) && (fy < y2 && y2 < ey)) ||
                ((fx < x1) && (x1 < ex) && (fy < y1) && (y1 < ey)) ||
                ((fx < x2) && (x2 < ex) && (fy < y1) && (y1 < ey)) ||
                ((fx < x1 && x1 < ex) && (fy < y2) && (y2 < ey))
                )
                {
                    DrowCenter(lstGdiRectangle[i]);
                }
            }
        }

        private void InitializeRec(GDIRectangle rec)
        {
            if (rec.CheckFlas)
            {
                rec.CheckFlas = false;
                Graphics g = panelCen.CreateGraphics();
                Pen pen = rec.pen;
                pen.Color = Color.Black;
                pen.Width = 1.0f;
                Rectangle rect = new Rectangle(rec.Rec.X, rec.Rec.Y, rec.Rec.Width + 1, rec.Rec.Height + 1);
                rec.NZRec = rect;
                g.FillRectangle(new SolidBrush(this.BackColor), rect);
                rect = new Rectangle(rec.Rec.X, rec.Rec.Y, rec.Rec.Width - 2, rec.Rec.Height - 2);
                g.DrawRectangle(pen, rect);
                g.Dispose();
            }
        }

        private Point wh(Rectangle _rec)
        {
            Point worh = new Point(_rec.X + _rec.Width, _rec.Y + _rec.Height);
            return worh;
        }

        private void Panel2_Paint(object sender, PaintEventArgs e)
        {
            Panel2All(e.Graphics);
        }

        private void Panel2All(Graphics _g)
        {
            Graphics g = _g;

            Pen pen = new Pen(Color.Black);
            #region 画内容格子
            intX = 0;
            intY = 0;
            GDIRectangle gdirectangle = new GDIRectangle();
            pen.Width = 1.5f;
            pen.Color = Color.FromArgb(128, 128, 128);
            gdirectangle.First = new Point(0, 0);
            gdirectangle.Rec = new Rectangle(0, 0, intWidth * 12 + 23, intHeight * 8 + 23);
            gdirectangle.g = g;//必须放在画笔之后??
            gdirectangle.pen = pen;
            gdirectangle.DrawRec();

            pen.Color = Color.Black;

            for (int i = 0; i < 96; i++)
            {
                if (i % 12 == 0 && i == 0)
                {
                    gdirectangle = new GDIRectangle();
                    gdirectangle.Id = i;
                    gdirectangle.g = g;
                    gdirectangle.pen = pen;
                    gdirectangle.FillRec = new Rectangle(intX + 4, intY + 4, intWidth - 8, intHeight - 8);
                    gdirectangle.FillDrawRec();
                    gdirectangle.Rec = new Rectangle(intX + 2, intY + 2, (intWidth * 12 + 22) / 12 - 5, (intHeight * 8 + 22) / 8 - 5);
                    gdirectangle.DrawRec();

                    intX += intWidth + 2;
                    lstGdiRectangle.Add(gdirectangle);
                }
                else
                {
                    if (i % 12 == 0) { intX = 0; intY += intHeight + 3; }
                    gdirectangle = new GDIRectangle();
                    gdirectangle.g = g;
                    gdirectangle.pen = pen;
                    gdirectangle.Id = i;
                    gdirectangle.FillRec = new Rectangle(intX + 4, intY + 4, intWidth - 8, intHeight - 8);
                    gdirectangle.FillDrawRec();
                    gdirectangle.Rec = new Rectangle(intX + 2, intY + 2, (intWidth * 12 + 22) / 12 - 5, (intHeight * 8 + 22) / 8 - 5);
                    gdirectangle.DrawRec();

                    intX += intWidth + 2;
                    lstGdiRectangle.Add(gdirectangle);
                }
                #endregion
            }
        }

        private void Panel1_Paint(object sender, PaintEventArgs e)
        {
            All(e.Graphics);
        }
        private void All(Graphics _g)
        {
            using (Graphics g = _g)
            {
                Pen pen = new Pen(Color.Black);
                pen.Width = 2;
                #region 画列头
                Rectangle Rec = new Rectangle(2, 2, 30, 15);
                g.DrawEllipse(pen, Rec);
                int num = 1;
                for (int i = 0; i < 12; i++)
                {
                    intX += intSpacing + intWidth;
                    Rec = new Rectangle(intX, intY, 40, 25);
                    g.DrawRectangle(pen, Rec);
                    Font myFont = new Font("宋体", 12, FontStyle.Bold);
                    Brush bush = new SolidBrush(Color.Black);//填充的颜色
                    g.DrawString(num.ToString(), myFont, bush, intX + 8, intY + 3);
                    num++;
                }
                #endregion

                #region 画行头
                for (int j = 0; j < 8; j++)
                {
                    intX = 0 + intSpacing;//初始化X
                    intY += intHeight + intSpacing;
                    Rec = new Rectangle(intX, intY, 40, 25);
                    g.DrawRectangle(pen, Rec);
                    Font myFont = new Font("宋体", 12, FontStyle.Bold);
                    Brush bush = new SolidBrush(Color.Black);//填充的颜色
                    g.DrawString(character[j].ToString(), myFont, bush, intX + 5, intY + 3);
                }
                #endregion
                g.Dispose();
            }
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
                }
            }
            Point p = e.Location;
            if (p.X < intWidth)
            {
                CommonCalculated(p, 0);
            }
            if (p.Y < intHeight && p.X > intWidth)
            {
                CommonCalculated(p, 1);
            }
        }

        /// <summary>
        /// 全选
        /// </summary>
        private void SelectAll()
        {
            InitializelstGdiRectangle();
            #region 画内容格子
            for (int i = 0; i < 96; i++)
            {
                DrowCenter(lstGdiRectangle[i]);
            }
            #endregion
        }

        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="rec"></param>
        private void DrowCenter(GDIRectangle rec)
        {
            rec.CheckFlas = true;
            Graphics g = panelCen.CreateGraphics();
            Pen pen = rec.pen;
            pen.Color = Color.Red;
            pen.Width = 2.0f;
            Rectangle rect = new Rectangle(rec.Rec.X + 2, rec.Rec.Y + 2, rec.Rec.Width - 3, rec.Rec.Height - 3);
            g.DrawRectangle(pen, rect);
            g.Dispose();
        }

        private void Panel1_MouseUp(object sender, MouseEventArgs e)
        {
            //this.Capture = false;
            //Cursor.Clip = Rectangle.Empty;
            //MouseIsDown = false;
            //DrawRectangle(sender);
            //MouseRect = Rectangle.Empty;
        }



        private void Panel1_MouseDown(object sender, MouseEventArgs e)
        {
            //MouseIsDown = true;
            //DrawStart(sender, e.Location);

            Point p = e.Location;
            SelectionRow(p);

        }



        /// <summary>
        /// 选中行或者列
        /// </summary>
        private void SelectionRow(Point p)
        {
            Graphics g = panelCen.CreateGraphics();
            if ((p.X > 0 && p.X < RowW) && (p.Y > LstObjres[0].yks && p.Y < LstObjres[0].yend)) SelectionRow(0, 12);
            else if ((p.X > 0 && p.X < RowW) && (p.Y > LstObjres[1].yks && p.Y < LstObjres[1].yend)) SelectionRow(12, 24);
            else if ((p.X > 0 && p.X < RowW) && (p.Y > LstObjres[2].yks && p.Y < LstObjres[2].yend)) SelectionRow(24, 36);
            else if ((p.X > 0 && p.X < RowW) && (p.Y > LstObjres[3].yks && p.Y < LstObjres[3].yend)) SelectionRow(36, 48);
            else if ((p.X > 0 && p.X < RowW) && (p.Y > LstObjres[4].yks && p.Y < LstObjres[4].yend)) SelectionRow(48, 60);
            else if ((p.X > 0 && p.X < RowW) && (p.Y > LstObjres[5].yks && p.Y < LstObjres[5].yend)) SelectionRow(60, 72);
            else if ((p.X > 0 && p.X < RowW) && (p.Y > LstObjres[6].yks && p.Y < LstObjres[6].yend)) SelectionRow(72, 84);
            else if ((p.X > 0 && p.X < RowW) && (p.Y > LstObjres[7].yks && p.Y < LstObjres[7].yend)) SelectionRow(84, 96);
            else if ((p.X > 0 && p.X < 40) && (p.Y > 0 && p.Y < 30)) SelectAll();
            else if ((p.Y > 0 && p.Y < ColH) && (p.X > LstXObjres[0].Xks && p.X < LstXObjres[0].Xend)) SelectionColumn(0);
            else if ((p.Y > 0 && p.Y < ColH) && (p.X > LstXObjres[1].Xks && p.X < LstXObjres[1].Xend)) SelectionColumn(1);
            else if ((p.Y > 0 && p.Y < ColH) && (p.X > LstXObjres[2].Xks && p.X < LstXObjres[2].Xend)) SelectionColumn(2);
            else if ((p.Y > 0 && p.Y < ColH) && (p.X > LstXObjres[3].Xks && p.X < LstXObjres[3].Xend)) SelectionColumn(3);
            else if ((p.Y > 0 && p.Y < ColH) && (p.X > LstXObjres[4].Xks && p.X < LstXObjres[4].Xend)) SelectionColumn(4);
            else if ((p.Y > 0 && p.Y < ColH) && (p.X > LstXObjres[5].Xks && p.X < LstXObjres[5].Xend)) SelectionColumn(5);
            else if ((p.Y > 0 && p.Y < ColH) && (p.X > LstXObjres[6].Xks && p.X < LstXObjres[6].Xend)) SelectionColumn(6);
            else if ((p.Y > 0 && p.Y < ColH) && (p.X > LstXObjres[7].Xks && p.X < LstXObjres[7].Xend)) SelectionColumn(7);
            else if ((p.Y > 0 && p.Y < ColH) && (p.X > LstXObjres[8].Xks && p.X < LstXObjres[8].Xend)) SelectionColumn(8);
            else if ((p.Y > 0 && p.Y < ColH) && (p.X > LstXObjres[9].Xks && p.X < LstXObjres[9].Xend)) SelectionColumn(9);
            else if ((p.Y > 0 && p.Y < ColH) && (p.X > LstXObjres[10].Xks && p.X < LstXObjres[10].Xend)) SelectionColumn(10);
            else if ((p.Y > 0 && p.Y < ColH) && (p.X > LstXObjres[11].Xks && p.X < LstXObjres[11].Xend)) SelectionColumn(11);
            else InitializelstGdiRectangle();//初始化表格
        }
        /// <summary>
        /// 选中一行后的界面效果处理
        /// </summary>
        /// <param name="key"></param>
        private void SelectionRow(int start, int end)
        {
            Graphics g = panelCen.CreateGraphics();
            InitializelstGdiRectangle();//初始化表格
            int first = start;
            int count = end;
            for (int i = first; i < count; i++)
            {
                DrowCenter(lstGdiRectangle[i]);
            }
            g.Dispose();
        }

        /// <summary>
        /// 选中一列
        /// </summary>
        private void SelectionColumn(int p)
        {
            Graphics g = panelCen.CreateGraphics();
            InitializelstGdiRectangle();//初始化表格
            int Lisint = lstGdiRectangle.Count;
            int num = p;
            for (int i = 0; i < Lisint; i++)
            {
                if (i % 12 == num)
                {
                    DrowCenter(lstGdiRectangle[i]);
                }
            }
            g.Dispose();
        }


        /// <summary>
        /// 行头与列头鼠标样式
        /// </summary>
        private void CommonCalculated(Point p, int k)
        {
            if (k == 0)
            {
                if ((p.X > 0 && p.X < 40) && (p.Y > 0 && p.Y < 30)) panel1.Cursor = Cursors.Hand;
                else if ((p.X > 0 && p.X < RowW) && (p.Y > LstObjres[0].yks && p.Y < LstObjres[0].yend)) panel1.Cursor = Cursors.Hand;
                else if ((p.X > 0 && p.X < RowW) && (p.Y > LstObjres[1].yks && p.Y < LstObjres[1].yend)) panel1.Cursor = Cursors.Hand;
                else if ((p.X > 0 && p.X < RowW) && (p.Y > LstObjres[2].yks && p.Y < LstObjres[2].yend)) panel1.Cursor = Cursors.Hand;
                else if ((p.X > 0 && p.X < RowW) && (p.Y > LstObjres[3].yks && p.Y < LstObjres[3].yend)) panel1.Cursor = Cursors.Hand;
                else if ((p.X > 0 && p.X < RowW) && (p.Y > LstObjres[4].yks && p.Y < LstObjres[4].yend)) panel1.Cursor = Cursors.Hand;
                else if ((p.X > 0 && p.X < RowW) && (p.Y > LstObjres[5].yks && p.Y < LstObjres[5].yend)) panel1.Cursor = Cursors.Hand;
                else if ((p.X > 0 && p.X < RowW) && (p.Y > LstObjres[6].yks && p.Y < LstObjres[6].yend)) panel1.Cursor = Cursors.Hand;
                else if ((p.X > 0 && p.X < RowW) && (p.Y > LstObjres[7].yks && p.Y < LstObjres[7].yend)) panel1.Cursor = Cursors.Hand;
                else panel1.Cursor = Cursors.Default;
            }
            else
            {
                if ((p.Y > 0 && p.Y < ColH) && (p.X > LstXObjres[0].Xks && p.X < LstXObjres[0].Xend)) panel1.Cursor = Cursors.Hand;
                else if ((p.Y > 0 && p.Y < ColH) && (p.X > LstXObjres[1].Xks && p.X < LstXObjres[1].Xend)) panel1.Cursor = Cursors.Hand;
                else if ((p.Y > 0 && p.Y < ColH) && (p.X > LstXObjres[2].Xks && p.X < LstXObjres[2].Xend)) panel1.Cursor = Cursors.Hand;
                else if ((p.Y > 0 && p.Y < ColH) && (p.X > LstXObjres[3].Xks && p.X < LstXObjres[3].Xend)) panel1.Cursor = Cursors.Hand;
                else if ((p.Y > 0 && p.Y < ColH) && (p.X > LstXObjres[4].Xks && p.X < LstXObjres[4].Xend)) panel1.Cursor = Cursors.Hand;
                else if ((p.Y > 0 && p.Y < ColH) && (p.X > LstXObjres[5].Xks && p.X < LstXObjres[5].Xend)) panel1.Cursor = Cursors.Hand;
                else if ((p.Y > 0 && p.Y < ColH) && (p.X > LstXObjres[6].Xks && p.X < LstXObjres[6].Xend)) panel1.Cursor = Cursors.Hand;
                else if ((p.Y > 0 && p.Y < ColH) && (p.X > LstXObjres[7].Xks && p.X < LstXObjres[7].Xend)) panel1.Cursor = Cursors.Hand;
                else if ((p.Y > 0 && p.Y < ColH) && (p.X > LstXObjres[8].Xks && p.X < LstXObjres[8].Xend)) panel1.Cursor = Cursors.Hand;
                else if ((p.Y > 0 && p.Y < ColH) && (p.X > LstXObjres[9].Xks && p.X < LstXObjres[9].Xend)) panel1.Cursor = Cursors.Hand;
                else if ((p.Y > 0 && p.Y < ColH) && (p.X > LstXObjres[10].Xks && p.X < LstXObjres[10].Xend)) panel1.Cursor = Cursors.Hand;
                else if ((p.Y > 0 && p.Y < ColH) && (p.X > LstXObjres[11].Xks && p.X < LstXObjres[11].Xend)) panel1.Cursor = Cursors.Hand;
                else panel1.Cursor = Cursors.Default;
            }
        }


        /// <summary>
        /// 行头计算
        /// </summary>
        /// <returns></returns>
        private List<objResY> LstObjresfanction()
        {
            LstObjres = new List<objResY>();
            objResY or = new objResY();
            int yFirst = 40;
            int yLase = yFirst + ColH;
            or.yks = yFirst;
            or.yend = yLase;
            LstObjres.Add(or);//1

            yLase = yLase + spg;
            yFirst = yLase + ColH;
            or.yks = yLase;
            or.yend = yFirst;
            LstObjres.Add(or);//2

            yFirst = yFirst + spg;
            yLase = yFirst + ColH;
            or.yks = yFirst;
            or.yend = yLase;
            LstObjres.Add(or);//3

            yLase = yLase + spg;
            yFirst = yLase + ColH;
            or.yks = yLase;
            or.yend = yFirst;
            LstObjres.Add(or);//4

            yFirst = yFirst + spg;
            yLase = yFirst + ColH;
            or.yks = yFirst;
            or.yend = yLase;
            LstObjres.Add(or);//5

            yLase = yLase + spg;
            yFirst = yLase + ColH;
            or.yks = yLase;
            or.yend = yFirst;
            LstObjres.Add(or);//6

            yFirst = yFirst + spg;
            yLase = yFirst + ColH;
            or.yks = yFirst;
            or.yend = yLase;
            LstObjres.Add(or);//7

            yLase = yLase + spg;
            yFirst = yLase + ColH;
            or.yks = yLase;
            or.yend = yFirst;
            LstObjres.Add(or);//8
            return LstObjres;
        }

        private List<objResX> LstObjresX()
        {
            LstXObjres = new List<objResX>();
            objResX oX = new objResX();
            int xFirst = 54;
            int xLase = xFirst + RowW;
            oX.Xks = xFirst;
            oX.Xend = xLase;
            LstXObjres.Add(oX);//1

            xLase = xLase + spg;
            xFirst = xLase + RowW;
            oX.Xks = xLase;
            oX.Xend = xFirst;
            LstXObjres.Add(oX);//2

            xLase = xFirst + spg;
            xFirst = xLase + RowW;
            oX.Xks = xLase;
            oX.Xend = xFirst;
            LstXObjres.Add(oX);//3

            xLase = xFirst + spg;
            xFirst = xLase + RowW;
            oX.Xks = xLase;
            oX.Xend = xFirst;
            LstXObjres.Add(oX);//4

            xLase = xFirst + spg;
            xFirst = xLase + RowW;
            oX.Xks = xLase;
            oX.Xend = xFirst;
            LstXObjres.Add(oX);//5

            xLase = xFirst + spg;
            xFirst = xLase + RowW;
            oX.Xks = xLase;
            oX.Xend = xFirst;
            LstXObjres.Add(oX);//6

            xLase = xFirst + spg;
            xFirst = xLase + RowW;
            oX.Xks = xLase;
            oX.Xend = xFirst;
            LstXObjres.Add(oX);//7

            xLase = xFirst + spg;
            xFirst = xLase + RowW;
            oX.Xks = xLase;
            oX.Xend = xFirst;
            LstXObjres.Add(oX);//8

            xLase = xFirst + spg;
            xFirst = xLase + RowW;
            oX.Xks = xLase;
            oX.Xend = xFirst;
            LstXObjres.Add(oX);//9

            xLase = xFirst + spg;
            xFirst = xLase + RowW;
            oX.Xks = xLase;
            oX.Xend = xFirst;
            LstXObjres.Add(oX);//10

            xLase = xFirst + spg;
            xFirst = xLase + RowW;
            oX.Xks = xLase;
            oX.Xend = xFirst;
            LstXObjres.Add(oX);//11

            xLase = xFirst + spg;
            xFirst = xLase + RowW;
            oX.Xks = xLase;
            oX.Xend = xFirst;
            LstXObjres.Add(oX);//12
            return LstXObjres;
        }

        private List<objResY> LstObjres = null;
        private List<objResX> LstXObjres = null;
        private struct objResY
        {
            public int yks;
            public int yend;
        }
        private struct objResX
        {
            public int Xks;
            public int Xend;
        }

        private void InitializelstGdiRectangle()
        {
            for (int i = 0; i < lstGdiRectangle.Count; i++)
            {
                if (lstGdiRectangle[i].CheckFlas)
                {
                    InitializeRec(lstGdiRectangle[i]);
                }
            }
        }

        /// <summary>
        /// 矩形对象 每个对象对应一个内容的小格子  列头行头除开
        /// </summary>
        private List<GDIRectangle> lstGdiRectangle = new List<GDIRectangle>();

        private int intX = 0, intY = 0, intSpacing = 3, intWidth = 53, intHeight = 38, intSpac = 4;
        private int ColH = 27, RowW = 42, spg = 14;
        private char[] character = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H' };
    }
}
