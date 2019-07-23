using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestDemo
{

    /// <summary>
    /// 画虚线矩形 GDI
    /// </summary>
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();            
            //激活双缓冲技术
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
        }
        
        private void Form2_Load(object sender, EventArgs e)
        {
            this.MouseDown += Form2_MouseDown;
            this.MouseMove += Form2_MouseMove;
            this.MouseUp += Form2_MouseUp;
        }

        private bool m_down = false;
        private Point basepoint;
        private Bitmap i;
        private Pen p;
        private Graphics g;
        //鼠标按下事件
        private void Form2_MouseDown(object sender, MouseEventArgs e)
        {
            //记录按下位置
            basepoint = e.Location;
            //按下标志true
            m_down = true;
        }

        //鼠标移动事件
        private void Form2_MouseMove(object sender, MouseEventArgs e)
        {
            //鼠标有按下才绘图
            if (m_down)
            {
                //实例化一个和窗口一样大的位图
                i = new Bitmap(this.Width, this.Height);
                //创建位图的gdi对象
                g = Graphics.FromImage(i);
                //创建画笔
                p = new Pen(Color.Black, 1.0f);
                //指定线条的样式为划线段
                p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                //根据当前位置画图，使用math的abs()方法求绝对值
                if (e.X < basepoint.X && e.Y < basepoint.Y)
                    g.DrawRectangle(p, e.X, e.Y, System.Math.Abs(e.X - basepoint.X), System.Math.Abs(e.Y - basepoint.Y));
                else if (e.X > basepoint.X && e.Y < basepoint.Y)
                    g.DrawRectangle(p, basepoint.X, e.Y, System.Math.Abs(e.X - basepoint.X), System.Math.Abs(e.Y - basepoint.Y));
                else if (e.X < basepoint.X && e.Y > basepoint.Y)
                    g.DrawRectangle(p, e.X, basepoint.Y, System.Math.Abs(e.X - basepoint.X), System.Math.Abs(e.Y - basepoint.Y));
                else
                    g.DrawRectangle(p, basepoint.X, basepoint.Y, System.Math.Abs(e.X - basepoint.X), System.Math.Abs(e.Y - basepoint.Y));

                //将位图贴到窗口上
                this.BackgroundImage = i;
                //释放gid和pen资源
                g.Dispose();
                p.Dispose();

            }
        }

        //鼠标释放事件
        private void Form2_MouseUp(object sender, MouseEventArgs e)
        {
            //清除图像
            i = new Bitmap(this.Width, this.Height);
            g = Graphics.FromImage(i);
            g.Clear(Color.Transparent);
            this.BackgroundImage = i;
            g.Dispose();

            //标志位置低
            m_down = false;
        }
    }
}
