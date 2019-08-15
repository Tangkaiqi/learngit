using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Com.First
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this.Name);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CompoentConfig.AppContext.AppFormContainer.FormClosing += AppFormContainer_FormClosing;
        }

        void AppFormContainer_FormClosing(object sender, FormClosingEventArgs e)
        {
            MessageBox.Show(label1.Text + ",我还没有关闭，不允许应用程序退出！");
            e.Cancel = true;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            CompoentConfig.AppContext.AppFormContainer.FormClosing -= AppFormContainer_FormClosing;
        }
    }
}
