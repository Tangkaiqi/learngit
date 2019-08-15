using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Text;
using TimedTask.Service;
using TimedTask.ViewModel;
using TimedTask.WeatherService;
using TimedTask.Utility;
using System.Collections;
using System.IO;

namespace TimedTask
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public static TaskbarIcon TaskIcon { get; set; }

        private WindowState _lastWinState = WindowState.Normal;//记录上一次WindowState 
        private bool _reallyExit = false;//是否真的关闭窗口  

        private Module.TaskListModule _tlModule = new Module.TaskListModule();//任务列表模块
        private Module.NoteListModule _nlModule = new Module.NoteListModule();//记事本列表模块
        private Module.MainModule _mmModule = new Module.MainModule();        //主页模块
        public MainWindow()
        {
            InitializeComponent();
            TaskIcon = new TaskbarIcon();
            XamlHelper.Instance.SetBackground(this.mainBoder, TimedTask.Model.PM.AppBgImg);
            TaskIcon.ShowBalloonTip("提示", "程序运行成功！", BalloonIcon.Info);

            if (this.brMain.Child != this._tlModule)
                this.brMain.Child = this._tlModule;

            //弹出资讯
            if (Model.PM.ShowNews)
            {
                btnMin_Click(null, null);
                View.NewsList newsList = new View.NewsList();
                newsList.Show();
            }
        }
        #region 托盘相关


        /// <summary>
        /// 菜单项"显示主窗口"点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void miShow_Click(object sender, RoutedEventArgs e)
        {
            if (this.IsVisible)
            {
                this.Hide();
                this.miShowWindow.Header = "显示窗口";
            }
            else
            {
                this.Show();
                this.miShowWindow.Header = "隐藏窗口";
            }
            if (this.WindowState == System.Windows.WindowState.Minimized)
            {
                this.WindowState = _lastWinState;
            }
            this.Activate();
        }
        /// <summary>
        /// 任务栏单击击图标
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TaskBarLeftDown_Click(object sender, RoutedEventArgs e)
        {
            this.Show();
            this.miShowWindow.Header = "隐藏窗口";
            if (this.WindowState == System.Windows.WindowState.Minimized)
            {
                this.WindowState = _lastWinState;
            }
            this.Activate();
        }
        //托盘
        private void Window_Close(object sender, RoutedEventArgs e)
        {
            if (System.Windows.MessageBox.Show("确定要退出托盘应用程序吗？",
                                               "托盘应用程序",
                                               MessageBoxButton.YesNo,
                                               MessageBoxImage.Question,
                                               MessageBoxResult.No) == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
                this.tbIcon.Visibility = Visibility.Hidden;
            }
        }
        /// <summary>
        /// 关闭时,判断是缩小到托盘还是退出程序
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if (!_reallyExit)
            {
                e.Cancel = true;
                _lastWinState = this.WindowState;
                this.Hide();
            }
            if (this.tbIcon != null)
                this.tbIcon.Dispose();
        }

        #endregion

        #region 窗体

        /// <summary>
        /// 窗体移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
        /// <summary>
        /// 窗体最小化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMin_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
            this.Visibility = Visibility.Hidden;//最小化到托盘
            this.miShowWindow.Header = "显示窗口";
        }
        /// <summary>
        /// 窗口最小化
        /// </summary>
        private void Minimized()
        {
            this.WindowState = System.Windows.WindowState.Minimized;
            this.Visibility = Visibility.Hidden;//最小化到托盘
            this.miShowWindow.Header = "显示窗口";
        }
        /// <summary>
        /// 窗体关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            if (TimedTask.Model.PM.MinToTray)
            {
                Minimized();
                TaskIcon.ShowBalloonTip("提示", "程序已经最小化到系统托盘！", BalloonIcon.Info);
                return;
            }
            System.Windows.Application.Current.Shutdown();
        }
        #endregion

        #region 菜单事件
        //下拉菜单
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            string arg = ((sender as MenuItem).CommandParameter).ToString();
            if (arg.Length == 0)
                return;

            switch (arg)
            {
                case "1"://启动宠物
                    MessageBox.Show("开发中，敬请期待！");
                    break;
                case "2"://检查更新
                    MessageBox.Show("开发中，敬请期待！");
                    break;
                case "3"://新闻资讯
                    View.NewsList news = new View.NewsList();
                    news.ShowDialog();
                    break;
                case "4"://关于
                    View.About about = new View.About();
                    about.Show();
                    break;
                case "5"://退出
                    Application.Current.Shutdown();
                    break;
                case "0-1"://网页图片下载
                    View.PageImageDown pageImage = new View.PageImageDown();
                    pageImage.Show();
                    break;
                default:
                    break;
            }
            if (this.brMain.Child != this._tlModule)
                this.brMain.Child = this._tlModule;
        }
        //任务
        private void mibtnTask_Click(object sender, RoutedEventArgs e)
        {
            if (this.brMain.Child != this._tlModule)
                this.brMain.Child = this._tlModule;
        }
        //记事
        private void mibtnNote_Click(object sender, RoutedEventArgs e)
        {
            if (this.brMain.Child != this._nlModule)
                this.brMain.Child = this._nlModule;
        }
        //首页
        private void mibtnMain_Click(object sender, RoutedEventArgs e)
        {
            //if (this.brMain.Child != this._mmModule)
            //    this.brMain.Child = this._mmModule;

            View.TabTest tab = new View.TabTest();
            tab.Show();
        }
        //设置
        private void miSet_Click(object sender, RoutedEventArgs e)
        {
            View.Config config = new View.Config();
            config.ShowDialog();
        }

        #endregion

        #region xml监控

        //文件监控
        //Thread tWork = new Thread(XmlMonitor);
        //tWork.IsBackground = true;
        //tWork.Start();

        /// <summary>
        /// 
        /// </summary>
        //public void XmlMonitor()
        //{
        //    FileSystemWatcher fw = new FileSystemWatcher();
        //    fw.Path = TimedTask.Model.App.TaskConfig.Substring(0, TimedTask.Model.App.TaskConfig.LastIndexOf("\\"));
        //    fw.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite;
        //    fw.Filter = "*.xml";
        //    fw.Changed += new FileSystemEventHandler(OnChanged);
        //    fw.EnableRaisingEvents = true;//是否提交事
        //}

        //private void OnChanged(object source, FileSystemEventArgs e)
        //{
        //    try
        //    {
        //        Bind();
        //    }
        //    catch (IOException)
        //    {
        //        Thread.Sleep(1000);
        //        Bind();
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.SaveLog("MainWindow OnChanged", ex.ToString());
        //    }
        //}
        #endregion
    }
}
