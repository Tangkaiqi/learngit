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
    public partial class MuemForm : Form
    {
        private Color SetColor = Color.AliceBlue;

        private int a = 0;
        public MuemForm()
        {
            InitializeComponent();
            //listBox1.DrawItem = DrawMode.OwnerDrawFixed;
            listBox1.DrawItem += ListBox1_DrawItem;
            
        }

        private void ListBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            ListBox lb = sender as ListBox;
            e.DrawBackground();
            Brush myBrush = Brushes.Black;
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                myBrush = new SolidBrush(SetColor);
                e.Graphics.FillRectangle(myBrush, e.Bounds);
            }
            //可针对每一列数据更改背景色
            switch (e.Index)
            {
                case 0:
                    myBrush = Brushes.Red;
                    break;
                case 1:
                    myBrush = Brushes.Orange;
                    break;
                case 2:
                    myBrush = Brushes.Purple;
                    break;
            }
            StringFormat strFormat = new StringFormat();
            strFormat.Alignment = StringAlignment.Center;
            strFormat.LineAlignment = StringAlignment.Center;
            e.DrawFocusRectangle();
            e.Graphics.DrawString(lb.Items[e.Index].ToString(), e.Font, myBrush, e.Bounds, strFormat);
        }

        private void MuemForm_Load(object sender, EventArgs e)
        {
            LoadList();
        }

        private void LoadList()
        {
            button1.BringToFront();
            button1.Dock = DockStyle.Top;
            listBox1.BringToFront();
            listBox1.Dock = DockStyle.Top;
            listBox1.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (a == 0)
            {
                listBox1.Visible = true;
                a = 1;
            }
            else
            {
                a = 0;
                listBox1.Visible = false;
            }
        }
    }
}
