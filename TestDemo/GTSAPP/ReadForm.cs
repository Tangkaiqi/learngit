using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace TestDemo
{

    /// <summary>
    /// 
    /// </summary>
    public partial class ReadForm : Form
    {
        public ReadForm()
        {
            InitializeComponent();
            InitializeStyle();
            //this.MouseMove += ReadForm_MouseMove;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {

            //Rectangle rect = new Rectangle(2, 2, 10, 20);
            //if (rect.Contains(e.Location))
            //{
            //    MessageBox.Show("1111");
            //    this.Cursor = Cursors.Hand;
            //}
            //else
            //{
            //    this.Cursor = Cursors.Default;
            //}
            //base.OnMouseMove(e);
        }

        const int WM_SYSCOMMAND = 0x112;
        const int SC_CLOSE = 0xF060;
        const int SC_MINIMIZE = 0xF020;
        const int SC_MAXIMIZE = 0xF030;

        protected override void WndProc(ref Message m)
        {

            //if (m.Msg == WM_SYSCOMMAND)
            //{

                //if (m.WParam.ToInt32() == SC_MINIMIZE)
                //{
                //    this.OnClick(EventArgs.Empty);
                //MessageBox.Show("11");
                // 做一些操作。。。
                // return
            //}

            base.WndProc(ref m);
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
           // MessageBox.Show("222");
        }

        public void InitializeStyle()
        {
            this.WindowState = FormWindowState.Maximized;
        }

        private void ReadForm_Load(object sender, EventArgs e)
        {
            
        }



        private void AddUserContrl()
        {

            //panelRead.Controls.Add();
        }

        private void AddTask_Click(object sender, EventArgs e)
        {
            Gridform gridform = new Gridform();
            gridform.ShowDialog();
        }

        private void btnAddTask_Click(object sender, EventArgs e)
        {
            AddTaskForm frmAddTask = new AddTaskForm();
            frmAddTask.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            TaskForm taskform = new TaskForm();
            taskform.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
