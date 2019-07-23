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

namespace TestDemo
{
    public partial class TaskForm_bf : Form
    {
        public TaskForm_bf()
        {
            InitializeComponent();
            gr = this.CreateGraphics();
            PenPoint = new Point(0, 0);
        }

        public Point PenPoint;
        public Graphics gr;

        private void TaskForm_Load(object sender, EventArgs e)
        {
            panel1.MouseDown += Panel1_MouseDown;
            panel1.MouseUp += Panel1_MouseUp;
            panel1.MouseMove += Panel1_MouseMove;
            panel1.Paint += Panel1_Paint;
            LstObjresfanction();//加载行头的起始点
            LstObjresX();//加载列头起始点
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
                    //Label labColumnHead = new Label();
                    //labColumnHead.AutoSize = false;
                    //labColumnHead.TextAlign = ContentAlignment.MiddleCenter;
                    //labColumnHead.Text = num.ToString();
                    intX += intSpacing + intWidth;
                    //labColumnHead.Location = new Point(intX, intY);
                    //labColumnHead.Width = intWidth;
                    //labColumnHead.Height = intHeight;
                    //labColumnHead.BackColor = Color.Transparent;
                    //panel1.Controls.Add(labColumnHead);
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

                #region 画内容格子
                intX = intWidth;
                intY = intHeight;
                GDIRectangle gdirectangle = new GDIRectangle();
                pen.Width = 1.5f;
                pen.Color = Color.FromArgb(128, 128, 128);
                gdirectangle.First = new Point(intX, intY);
                gdirectangle.Rec = new Rectangle(intX, intY, intWidth * 12 + 23, intHeight * 8 + 23);
                gdirectangle.g = g;//必须放在画笔之后??
                gdirectangle.pen = pen;
                gdirectangle.DrawRec();

                //intX = intWidth;
                //intY = intHeight;
                //pen.Width = 1.5f;
                //pen.Color = Color.FromArgb(128, 128, 128);
                //Rec = new Rectangle(intX, intY, intWidth * 12 + 23, intHeight * 8 + 23);
                //g.DrawRectangle(pen, Rec);

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

                        //Rec = new Rectangle(intX + 2, intY + 2, (intWidth * 12 + 22) / 12 - 5, (intHeight * 8 + 22) / 8 - 5);
                        //g.DrawRectangle(pen, Rec);
                        //Rec = new Rectangle(intX + 4, intY + 4, intWidth - 8, intHeight - 8);
                        //g.FillRectangle(new SolidBrush(Color.White), Rec);
                        intX += intWidth + 2;
                        lstGdiRectangle.Add(gdirectangle);
                    }
                    else
                    {
                        if (i % 12 == 0) { intX = intWidth; intY += intHeight + 3; }
                        gdirectangle = new GDIRectangle();
                        gdirectangle.g = g;
                        gdirectangle.pen = pen;
                        gdirectangle.Id = i;
                        gdirectangle.FillRec = new Rectangle(intX + 4, intY + 4, intWidth - 8, intHeight - 8);
                        gdirectangle.FillDrawRec();
                        gdirectangle.Rec = new Rectangle(intX + 2, intY + 2, (intWidth * 12 + 22) / 12 - 5, (intHeight * 8 + 22) / 8 - 5);
                        gdirectangle.DrawRec();

                        //Rec = new Rectangle(intX + 2, intY + 2, (intWidth * 12 + 22) / 12 - 5, (intHeight * 8 + 22) / 8 - 5);
                        //g.DrawRectangle(pen, Rec);
                        //Rec = new Rectangle(intX + 4, intY + 4, intWidth - 8, intHeight - 8);
                        //g.FillRectangle(new SolidBrush(Color.White), Rec);
                        intX += intWidth + 2;
                        lstGdiRectangle.Add(gdirectangle);
                    }
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
            using (Graphics g = panel1.CreateGraphics())
            {
                Rectangle Rec = new Rectangle(2, 2, 30, 15);
                Pen pen = new Pen(Color.Black);
                pen.Width = 2;

                #region 画内容格子
                intX = intWidth;
                intY = intHeight;
                pen.Color = Color.Red;

                for (int i = 0; i < 96; i++)
                {
                    if (i % 12 == 0 && i == 0)
                    {
                        Rec = new Rectangle(intX + intSpac, intY + intSpac, (intWidth * 12 + 22) / 12 - 8, (intHeight * 8 + 22) / 8 - 8);
                        g.DrawRectangle(pen, Rec);
                        //Rec = new Rectangle(intX + 2, intY + 2, intWidth - 2, intHeight - 2);
                        //g.FillRectangle(new SolidBrush(Color.White), Rec);
                        intX += intWidth + 2;
                    }
                    else
                    {
                        if (i % 12 == 0) { intX = intWidth; intY += intHeight + 3; }
                        Rec = new Rectangle(intX + intSpac, intY + intSpac, (intWidth * 12 + 22) / 12 - 8, (intHeight * 8 + 22) / 8 - 8);
                        g.DrawRectangle(pen, Rec);
                        //Rec = new Rectangle(intX + 2, intY + 2, intWidth - 2, intHeight - 2);
                        //g.FillRectangle(new SolidBrush(Color.White), Rec);
                        intX += intWidth + 2;
                    }
                }
                #endregion
                g.Dispose();
            }
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
            this.PenPoint = e.Location;

            Point p = e.Location;
            if ((p.X > 0 && p.X < intWidth) && (p.Y > 0 && p.Y < intHeight))
            {
                SelectAll();// 全选
            }
            if (p.X < intWidth)
            {
                SelectionRow(p);//选中一行
            }
            if (p.Y < intHeight && p.X > intWidth)
            {
                SelectionColumn(p);//选中一列
            }

        }

        //定义两个变量 
        bool MouseIsDown = false;
        Rectangle MouseRect = Rectangle.Empty; //矩形（为鼠标画出矩形选区）

        //定义三个方法
        private void ResizeToRectangle(object sender, Point p)
        {
            DrawRectangle(sender);
            MouseRect.Width = p.X - MouseRect.Left;
            MouseRect.Height = p.Y - MouseRect.Top;
            DrawRectangle(sender);
        }
        private void DrawRectangle(object sender)
        {
            Rectangle rect = ((Panel)sender).RectangleToScreen(MouseRect);
            ControlPaint.DrawReversibleFrame(rect, Color.White, FrameStyle.Dashed);

            //ControlPaint.DrawButton();
        }

        private void DrawStart(object sender, Point StartPoint)
        {
            ((Panel)sender).Capture = true;
            Cursor.Clip = ((Panel)sender).RectangleToScreen(((Panel)sender).Bounds);
            MouseRect = new Rectangle(StartPoint.X, StartPoint.Y, 0, 0);
        }

        /// <summary>
        /// 选中一行
        /// </summary>
        private void SelectionRow(Point p)
        {
            if ((p.X > 0 && p.X < RowW) && (p.Y > LstObjres[0].yks && p.Y < LstObjres[0].yend))
            {
                SelectionRow(1);
            }
            else if ((p.X > 0 && p.X < RowW) && (p.Y > LstObjres[1].yks && p.Y < LstObjres[1].yend)) panel1.Cursor = Cursors.Hand;
            else if ((p.X > 0 && p.X < RowW) && (p.Y > LstObjres[2].yks && p.Y < LstObjres[2].yend)) panel1.Cursor = Cursors.Hand;
            else if ((p.X > 0 && p.X < RowW) && (p.Y > LstObjres[3].yks && p.Y < LstObjres[3].yend)) panel1.Cursor = Cursors.Hand;
            else if ((p.X > 0 && p.X < RowW) && (p.Y > LstObjres[4].yks && p.Y < LstObjres[4].yend)) panel1.Cursor = Cursors.Hand;
            else if ((p.X > 0 && p.X < RowW) && (p.Y > LstObjres[5].yks && p.Y < LstObjres[5].yend)) panel1.Cursor = Cursors.Hand;
            else if ((p.X > 0 && p.X < RowW) && (p.Y > LstObjres[6].yks && p.Y < LstObjres[6].yend)) panel1.Cursor = Cursors.Hand;
            else if ((p.X > 0 && p.X < RowW) && (p.Y > LstObjres[7].yks && p.Y < LstObjres[7].yend)) panel1.Cursor = Cursors.Hand;
            else panel1.Cursor = Cursors.Default;
        }
        /// <summary>
        /// 选中一行后的界面效果处理
        /// </summary>
        /// <param name="key"></param>
        private void SelectionRow(int key)
        {
            using (Graphics g = panel1.CreateGraphics())
            {
                InitializelstGdiRectangle(g);//初始化表格
                                             //计算是哪一行
                int count = key * 12;
                int first = count - 12;
                for (int i = first; i < count; i++)
                {

                    lstGdiRectangle[i].g = g;
                    //Pen pen = new Pen(Color.Red);
                    //pen.Width = 1.5f;
                    lstGdiRectangle[i].pen.Width = 2.0f;
                    lstGdiRectangle[i].pen.Color = Color.Red;
                    lstGdiRectangle[i].FillDrawRec();
                    lstGdiRectangle[i].DrawRec();
                }
            }
        }

        /// <summary>
        /// 选中一列
        /// </summary>
        private void SelectionColumn(Point p)
        {

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

        ///// <summary>
        ///// 格子的每个参数
        ///// </summary>
        //private struct GDIRectangle
        //{
        //    //图形种类、大小、位置、方向、选中情况
        //    public int Id;
        //    public int Rectang;//图形种类 1矩形
        //    public int Width;//宽
        //    public int height;//高
        //    public Point First; //起点位置
        //    public Point Lase;//结束点位置
        //    public int Direction; //方向
        //    public bool CheckFlas;//是否选中
        //    public float PenWidth;//画笔的宽度
        //    public Color PenColor;//画笔的颜色
        //    public Graphics g;
        //    public Rectangle Rec;//绘图对象
        //    public Rectangle FillRec;//填充绘图对象
        //    public Pen pen;

        //    /// <summary>
        //    /// 画矩形
        //    /// </summary>
        //    public void DrawRec()
        //    {
        //        g.DrawRectangle(pen, Rec);
        //    }
        //    /// <summary>
        //    /// 填充矩形
        //    /// </summary>
        //    public void FillDrawRec()
        //    {
        //        g.FillRectangle(new SolidBrush(Color.White), Rec);
        //    }
        //}
        private void InitializelstGdiRectangle(Graphics _g)
        {
            Graphics g =_g;
            g.Clear(this.BackColor);
            All(g);
            for (int i = 0; i < lstGdiRectangle.Count; i++)
            {

                //lstGdiRectangle[i].g = g;
                //lstGdiRectangle[i].FillDrawRec();
                //lstGdiRectangle[i].DrawRec();
                //lstGdiRectangle[i].g
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
