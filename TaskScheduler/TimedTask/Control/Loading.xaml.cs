using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace TimedTask.Control
{
    /// <summary>
    /// 加载框
    /// </summary>
    public partial class Loading : Window
    {
        /* 使用方法
          Control.Loading load = new Control.Loading(Test);
          load.Msg = "稍等。。。";
          load.Start();
          load.ShowDialog();
         
          private void Test()
          {
              System.Threading.Thread.Sleep(15000);
          }
         */
        public Action WorkMethod;

        private string _msg = "正在处理，请稍等...";
        private string message = string.Empty;
        /// <summary>
        /// 提示信息
        /// </summary>
        public string Msg
        {
            get { return _msg; }
            set { _msg = value; }
        }

        public Loading()
        {
            InitializeComponent();
        }
        public Loading(Action workMethod)
        {
            InitializeComponent();
            this.lblMsg.Content = this.Msg;
            this.WorkMethod = workMethod;
        }
        public void Start()
        {
            using (BackgroundWorker bw = new BackgroundWorker())
            {
                bw.DoWork += (obj, e) =>
                {
                    try
                    {
                        if (this.WorkMethod != null)
                            this.WorkMethod();
                    }
                    catch { }
                };
                bw.RunWorkerCompleted += (s, e) =>
                {
                    this.Close();
                };
                bw.RunWorkerAsync();
            }
        }
        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Close();
        }
    }
}
