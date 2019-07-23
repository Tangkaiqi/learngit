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
    public partial class SystemForm : Form
    {
        public SystemForm()
        {
            InitializeComponent();
            InitializeEvent();
            InitializeStyle();
        }

        private void InitializeEvent()
        {
            this.Resize += SystemForm_Resize;
        }

        private void InitializeStyle()
        {
            this.MaximizedBounds = Screen.PrimaryScreen.WorkingArea;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            panelTitle.Location = new Point(0, 0);
            panelTitle.Width = this.MaximizedBounds.Width;
            panelTitle.Height = 20;
            panelTitle.BackgroundImage = TestDemo.Properties.Resources.bg;
            labExc.Location = new Point(this.MaximizedBounds.Width - labExc.Width, 0);
        }

        private void SystemForm_Resize(object sender, EventArgs e)
        {

        }

        private void SystemForm_Load(object sender, EventArgs e)
        {

        }

        private void labExc_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要退出吗?", "退出系统", MessageBoxButtons.OKCancel);
            if (dr == DialogResult.OK)//如果点击“确定”按钮

            {
                System.Environment.Exit(0);
            }
        }
    }
}
