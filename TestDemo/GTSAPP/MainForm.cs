using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestDemo
{
    public partial class MainForm : SystemForm
    {
        public int[] s = { 0, 0, 0 };//用来记录窗体是否打开过
        public MainForm()
        {
            InitializeComponent();
            InitializeEvent();
            InitializeStyle();
        }

        private void InitializeEvent()
        {
            tabControl1.SelectedIndexChanged += TabControl1_SelectedIndexChanged;
            this.Resize += MainForm_Resize;
        }
        private void InitializeStyle()
        {
            this.MaximizedBounds = Screen.PrimaryScreen.WorkingArea;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            tabControl1.Location = new Point(0, 20);
            tabControl1.Width = this.MaximizedBounds.Width;
            tabControl1.Height = this.MaximizedBounds.Height - 20;
            tabExperimentTask.Width = tabControl1.Width;
            tabExperimentTask.Height = tabControl1.Height;
            tabExperimentHistory.Width = tabControl1.Width;
            tabExperimentHistory.Height = tabControl1.Height;
            tabSetUp.Width = tabControl1.Width;
            tabSetUp.Height = tabControl1.Height;
            tabHelp.Width = tabControl1.Width;
            tabHelp.Height = tabControl1.Height;
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {

        }

        private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            tabControl1.TabPages[tabControl1.SelectedIndex].Focus();
            //只生成一次
            //if (s[tabControl1.SelectedIndex] == 0)
            //{
            //    btn_Click(sender, e);
            //}
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.Focus();
            //默认加载CNC窗体
            //string formClass = "TestDemo.Form3";
            //GenerateForm(formClass, tabControl1);
        }

        private void GenerateForm(string form, object sender)
        {
            ////反射生成窗体
            //Form fm = (Form)Assembly.GetExecutingAssembly().CreateInstance(form);
            ////设置窗体没有边框，加入到选项卡中
            ////fm.FormBorderStyle = FormBorderStyle.None;
            //fm.TopLevel = false;
            //fm.Parent = ((TabControl)sender).SelectedTab;
            //fm.ControlBox = false;
            //fm.Dock = DockStyle.Fill;
            //fm.Show();
            //s[((TabControl)sender).SelectedIndex] = 1;
        }

        /// <summary>  
        /// 通用按钮点击选项卡 在选项卡上显示对应的窗体  
        /// </summary>  
        //private void btn_Click(object sender, EventArgs e)
        //{
        //    string formClass = ((TabControl)sender).SelectedTab.Tag.ToString();
        //    GenerateForm(formClass, sender);
        //}

        private void btnNewTask_Click(object sender, EventArgs e)
        {
            Gridform gridform = new Gridform();
            gridform.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MuemForm mf = new MuemForm();
            mf.ShowDialog();
        }
    }

    #region Button按钮重写
    //class newButton : Button //继承之系统按钮控件
    //{
    //    protected override void OnPaint(PaintEventArgs e)
    //    {//重写
    //        base.OnPaint(e);
    //        System.Drawing.Pen pen = new Pen(this.BackColor, 3);
    //        e.Graphics.DrawRectangle(pen, 0, 0, this.Width, this.Height);//填充
    //        pen.Dispose();

    //    }
    //}
    #endregion
}
