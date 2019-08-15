using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Xml;
using TimedTask.Model;
using TimedTask.Utility;

namespace TimedTask
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        //单实例运行代码
        Mutex mutex;
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            bool startupFlag;
            Mutex mutex = new Mutex(true, PM.ApplicationName, out startupFlag);
            if (!startupFlag)
            {
                if (!PM.IsMainWinShow)
                {
                    PM.IsMainWinShow = true;
                    //激活已运行实例
                    //
                    //
                }
                MessageBox.Show("程序已经启动！");
                Environment.Exit(0);

            }
            else
            {
                if (e.Args.Length > 0)//启动参数
                {

                }
                OnInit();
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
            }
        }
        /// <summary>
        /// 系统初始化
        /// </summary>
        private void OnInit()
        {
            PM.StartPath = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            PM.Config = PM.StartPath + "\\config.xml";
            XmlHelper xml = new XmlHelper(PM.Config);
            Log.LogPath = PM.StartPath + "\\Log\\";
            if (!System.IO.File.Exists(PM.Config))
            {
                MessageBox.Show("配置文件丢失！", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                Application.Current.Shutdown();
            }
            try
            {
                string minutes = Helper.Instance.GetValue(xml, "LockMinute");
                PM.AppBgImg = Helper.Instance.GetValue(xml, "AppBgImg");
                PM.LockBgImg = Helper.Instance.GetValue(xml, "LockBgImg");
                PM.MinToTray = Helper.Instance.GetValue(xml, "MinToTray") == "1" ? true : false;
                PM.ShowNews = Helper.Instance.GetValue(xml, "ShowNews") == "1" ? true : false;
                PM.LockMinute = minutes.Length == 0 ? 3 : Convert.ToInt32(minutes);
                PM.SaveLog = Helper.Instance.GetValue(xml, "SaveLog") == "1" ? true : false;
                PM.NewsUrl = Helper.Instance.GetValue(xml, "NewsUrl");

                XmlNode node = xml.GetXmlNode("Configuration/NewsTag");
                PM.NewsTag = new SortedList();
                string key = String.Empty;
                if (node != null && node.HasChildNodes)
                {
                    foreach (XmlElement v in node.ChildNodes)
                    {
                        key = v.Attributes["title"].InnerText;
                        if (PM.NewsTag.ContainsKey(key))
                            continue;

                        PM.NewsTag.Add(key, v.InnerText.Replace("<![CDATA[", "").Replace("]]>", ""));
                    }
                }
                if (PM.AppBgImg.Length == 0)
                    PM.AppBgImg = PM.StartPath + "\\Bg\\bg.jpg";
                if (PM.LockBgImg.Length == 0)
                    PM.LockBgImg = PM.StartPath + "\\Bg\\lock.jpg";
            }
            catch (Exception ex)
            {
                Log.SaveLog("MainWindow.xaml.cs Window_Loaded", ex.ToString());
            }

            #region 数据库配置

            MSL.Tool.Data.DbHelper.ConnectionString = string.Format(Helper.Instance.GetValue(xml, "Conn"), PM.StartPath + @"\");
            MSL.Tool.Data.DbHelper.DatabaseType = MSL.Tool.Data.DataBaseType.SQLite;
            #endregion
        }
    }
}
