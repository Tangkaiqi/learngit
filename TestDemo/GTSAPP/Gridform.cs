using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestDemo.UserContrl;
using System.Threading;

namespace TestDemo
{
    public partial class Gridform : Form
    {
        public Gridform()
        {
            InitializeComponent();
        }

        private void Gridform_Load(object sender, EventArgs e)
        {
            //Thread.Sleep(TimeSpan.FromSeconds(2));
            //ThreadPool 
        }

        private void btnEsc_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            GridHole gridhole = new GridHole();
            string str = "";
            for (int i = 0; i < gridCtrl1.Grids.Count; i++)
            {
                gridhole = gridCtrl1.Grids[i];
                if (gridhole.IsSelected)
                {
                    str += gridhole.DridLocation + ",";
                }
            }
            MessageBox.Show("选中的行有：" + str);
        }
    }
}
