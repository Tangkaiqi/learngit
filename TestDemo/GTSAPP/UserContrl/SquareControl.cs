using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestDemo.Common;

namespace TestDemo.UserContrl
{
    /// <summary>
    /// 表格的每个格子
    /// </summary>
    public partial class SquareControl : UserControl
    {
        public SquareControl()
        {
            InitializeComponent();
            InitializeStyle();
            InitializeFunction();
        }

        private void InitializeStyle()
        {
            this.BackColor = tablesInfoEs.BgColor;
            this.Location = new Point(tablesInfoEs.X, tablesInfoEs.Y);
            this.Width = tablesInfoEs.SWidth;
            this.Height = tablesInfoEs.SHeight;
        }

        private void InitializeFunction()
        {
            this.Paint += SquareControl_Paint;
            this.Click += SquareControl_Click;
        }

        private void SquareControl_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("1111111");
        }

        private void SquareControl_Paint(object sender, PaintEventArgs e)
        {
            using (Graphics g = e.Graphics)
            {
                if (tablesInfoEs.ColFla) //是否是列头
                {
                    PaintCol(g);
                }
                if (tablesInfoEs.RowFla)  //是否是行头
                {
                    PaintRow(g);
                }
                if (tablesInfoEs.CenterTable)
                {
                    PaintSquare(g);
                }

                g.Dispose();
            }
        }

        private void SquareControl_Load(object sender, EventArgs e)
        {
            InitializeStyle();
        }

        /// <summary>
        /// 绘制列头
        /// </summary>
        private void PaintCol(Graphics _g)
        {
            using (Graphics g = _g)
            {
                if (tablesInfoEs.AllCheckedFla)  //是否是全选按钮
                {
                    Pen pen = new Pen(Color.FromArgb(128, 128, 128));
                    pen.Width = 2.0f;
                    Rectangle Rec = new Rectangle(tablesInfoEs.X, tablesInfoEs.Y, 30, 15);
                    g.DrawEllipse(pen, Rec);
                    label1.Text = "";
                }
                else
                {
                    Pen pen = new Pen(Color.FromArgb(128, 128, 128));
                    pen.Width = 2.0f;
                    Rectangle rect = new Rectangle(3, 3, tablesInfoEs.SWidth - 3, tablesInfoEs.SHeight - 3);
                    g.DrawRectangle(pen, rect);
                    label1.Text = tablesInfoEs.StrTitle;
                }
            }
        }

        /// <summary>
        /// 绘制行头
        /// </summary>
        /// <param name="_g"></param>
        private void PaintRow(Graphics _g)
        {
            using (Graphics g=_g)
            {
                Pen pen = new Pen(Color.FromArgb(128, 128, 128));
                pen.Width = 2.0f;
                Rectangle rect = new Rectangle(3, 3, tablesInfoEs.SWidth - 3, tablesInfoEs.SHeight - 3);
                g.DrawRectangle(pen, rect);
                label1.Text = tablesInfoEs.StrTitle;
            }
        }

        /// <summary>
        /// 绘制表格
        /// </summary>
        public void PaintSquare(Graphics _g)
        {
            using (Graphics g = _g)
            {
                Pen pen = new Pen(Color.FromArgb(128, 128, 128));
                pen.Width = 1.5f;
                Rectangle rect = new Rectangle(2, 2, tablesInfoEs.SWidth - 3, tablesInfoEs.SHeight - 3);
                g.DrawRectangle(pen, rect);
                label1.Text = "";
            }   
        }


        private TablesInfo tablesInfoEs = new TablesInfo();

        public TablesInfo TablesInfoEs
        {
            get
            {
                return tablesInfoEs;
            }
            set
            {
                tablesInfoEs = value;
            }
        }

        //private int id;

        //private int sWidth = 0;
        //private int sHeight = 0;
        //private int x;
        //private int y;
        //private Rectangle rec;//XY 宽高

        //private bool colFla = false;//是否是列头
        //private bool rowFla = false;//是否是行头
        //private bool checkedFla = false;//是否被选中

        //private bool allCheckedFla = false;//是否全选

        //private Color bgColor;

        //private string strTitle = "";


        //public Color BgColor
        //{
        //    get
        //    {
        //        return bgColor;
        //    }

        //    set
        //    {
        //        bgColor = value;
        //    }
        //}

        //public int X
        //{
        //    get
        //    {
        //        return x;
        //    }

        //    set
        //    {
        //        x = value;
        //    }
        //}

        //public int Y
        //{
        //    get
        //    {
        //        return y;
        //    }

        //    set
        //    {
        //        y = value;
        //    }
        //}

        //public int SWidth
        //{
        //    get
        //    {
        //        return sWidth;
        //    }

        //    set
        //    {
        //        sWidth = value;
        //    }
        //}

        //public int SHeight
        //{
        //    get
        //    {
        //        return sHeight;
        //    }

        //    set
        //    {
        //        sHeight = value;
        //    }
        //}

        ///// <summary>
        ///// 是否是列头
        ///// </summary>
        //public bool ColFla
        //{
        //    get
        //    {
        //        return colFla;
        //    }

        //    set
        //    {
        //        colFla = value;
        //    }
        //}

        ///// <summary>
        ///// 是否是行头
        ///// </summary>
        //public bool RowFla
        //{
        //    get
        //    {
        //        return rowFla;
        //    }

        //    set
        //    {
        //        rowFla = value;
        //    }
        //}
        ///// <summary>
        ///// 是否被选中
        ///// </summary>
        //public bool CheckedFla
        //{
        //    get
        //    {
        //        return checkedFla;
        //    }

        //    set
        //    {
        //        checkedFla = value;
        //    }
        //}
        ///// <summary>
        ///// 是否全选
        ///// </summary>
        //public bool AllCheckedFla
        //{
        //    get
        //    {
        //        return allCheckedFla;
        //    }

        //    set
        //    {
        //        allCheckedFla = value;
        //    }
        //}

        //public string StrTitle
        //{
        //    get
        //    {
        //        return strTitle;
        //    }

        //    set
        //    {
        //        strTitle = value;
        //    }
        //}
    }
}
