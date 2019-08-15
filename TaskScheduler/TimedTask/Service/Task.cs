using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading;
using TimedTask.Model;
using System.Windows;
using TimedTask.Utility;

namespace TimedTask.Service
{
    public class Task
    {
        private static Task _instance;
        private static object _lock = new object();//使用static object作为互斥资源
        private static readonly object _obj = new object();
        private Bll.AutoTask _bllTask = new Bll.AutoTask();
        private Bll.SysLog _bllLog = new Bll.SysLog();

        #region 单例
        /// <summary>
        /// 
        /// </summary>
        private Task() { }

        /// <summary>
        /// 返回唯一实例
        /// </summary>
        public static Task Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_obj)
                    {
                        //if (_instance == null)
                        //{
                        _instance = new Task();
                        //}
                    }
                }
                return _instance;
            }
        }
        #endregion

        /// <summary>
        /// 抓取百度新闻内容
        /// </summary>
        public void SpiderNewsToHtml()
        {
            if (String.IsNullOrEmpty(Model.PM.NewsUrl) ||
                Model.PM.NewsTag == null ||
                Model.PM.NewsTag.Count == 0)
                return;

            string path = Model.PM.StartPath + "\\News\\news.htm";
            if (File.Exists(path))
            {
                System.IO.FileInfo fi = new System.IO.FileInfo(path);
                if (fi.CreationTime < DateTime.Now.AddHours(-2))//两小时前生成 删除
                    Helper.Instance.DeleteFile(path);
            }
            if (File.Exists(path))
                return;

            string html = Utility.HtmlHelper.Instance.GetHtml(Model.PM.NewsUrl);
            Utility.HtmlHelper.Instance.FixUrl(Model.PM.NewsUrl, html);
            #region #

            //string src = "http://news.baidu.com/";//百度新闻首页
            //Hashtable ht = new Hashtable();
            //ht.Add("焦点", "<ul class=\"ulist focuslistnews\">⊙</ul>");
            //ht.Add("国内", "<div class=\"l-left-col col-mod\" alog-group=log-civil-left>⊙</ul>");
            #endregion

            StringBuilder sbTag = new StringBuilder();
            StringBuilder sbList = new StringBuilder();
            string templatePath = Model.PM.StartPath + "\\News\\news.html";
            string newsHtml = Helper.Instance.ReadFile(templatePath, null);
            int i = 1;
            string time = DateTime.Now.ToString("MM-dd");
            string no = String.Empty;
            foreach (DictionaryEntry m in Model.PM.NewsTag)
            {
                string content = Helper.Instance.CutString(html, m.Value.ToString().Split('⊙')[0], m.Value.ToString().Split('⊙')[1], false);
                ArrayList al = Utility.HtmlHelper.Instance.GetLinks(content);

                if (al != null && al.Count > 0)
                {
                    sbTag.AppendFormat("<li id='one{0}' onmouseover=\"setTab('one',{1},{2})\" {4}><a href='#'>{3}</a></li>\r\n", i, i, Model.PM.NewsTag.Count, m.Key, (i == 1) ? "class='hover'" : "");
                    string title;
                    string href;
                    sbList.AppendFormat("<div id='con_one_{0}' class='hover'>\r\n<ul class='news_list news_list2'>\r\n", i);
                    int k = 1;
                    foreach (string[] hyperLink in al)
                    {
                        if (k > 14)
                            break;

                        no = "<h2>" + k + "</h2>";
                        if (k < 5)
                            no = "<h1>" + k + "</h1>";

                        title = hyperLink[0];
                        href = hyperLink[1];
                        sbList.AppendFormat("<li><span>{2}</span>{3}<a href='{0}' target='_blank'>{1}</a></li>\r\n", href, title, time, no);
                        //sbList.AppendFormat("<li><span>05-06</span>·<a href='{0}'>{1}</a></li>\r\n", href, title);

                        k++;
                    }
                    sbList.Append("</ul>\r\n</div>\r\n");
                }
                i++;
            }
            newsHtml = newsHtml.Replace("$[NEWS_TAG]", sbTag.ToString());
            newsHtml = newsHtml.Replace("$[NEWS_LIST]", sbList.ToString());

            Helper.Instance.DeleteFile(path);
            Helper.Instance.WriteFile(path, newsHtml);
        }

        /// <summary>
        /// 任务启动
        /// </summary>
        public void StartTask()
        {
            string proccessName = "";
            bool isTask = true;//是否是定时任务
            try
            {
                List<TimedTask.Model.AutoTask> list = _bllTask.GetList(" 1=1 ", null, "CreateDate DESC ");
                if (list == null || list.Count == 0)
                {
                    return;
                }
                foreach (AutoTask model in list)
                {
                    isTask = true;

                    if (model.TaskType.Length > 0 && model.TaskType != "0")//声音、窗口提醒 
                    {
                        isTask = false;
                    }
                    #region 路径不存在 或 不到时间

                    if (isTask && (
                            model.ApplicationPath.Length == 0
                            || model.NextStartDate == null
                            || (model.RunType == RunType.Month.ToString() && model.Dayth != DateTime.Now.Day)
                        ))
                    {
                        continue;
                    }
                    if (isTask && !File.Exists(model.ApplicationPath))
                    {
                        Log.SaveLog("exe_not_exists", "Task StartTask", "任务路径错误，名称：" + model.Title + ",路径：" + model.ApplicationPath + "\r\n");
                        model.Status = "路径不存在";
                        model.Enable = "2";//失效
                        _bllTask.Update(model, " Id=" + model.Id);
                        continue;
                    }
                    #endregion

                    try
                    {
                        #region 失效

                        if (model.StopDate != null && DateTime.Now >= model.StopDate)
                        {
                            model.Status = "任务过期";
                            model.Enable = "3";
                            _bllTask.Update(model, " Id=" + model.Id);

                            continue;
                        }
                        else if (model.Enable != "1")
                        {
                            model.Status = "任务禁用";
                            model.Enable = "0";
                            _bllTask.Update(model, " Id=" + model.Id);
                            continue;
                        }
                        #endregion

                        #region 结束进程

                        if (isTask)
                        {
                            proccessName = model.ApplicationPath.Substring(model.ApplicationPath.LastIndexOf("\\") + 1).Replace(".exe", "");
                            Helper.Instance.EndApp(proccessName);
                        }
                        #endregion

                        if (model.NextStartDate != null && DateTime.Now >= model.NextStartDate)
                        {
                            bool result = true;
                            if (isTask)
                            {
                                result = StartApp(model, proccessName);
                            }
                            else
                            {
                                result = StartWarn(model, false);
                            }
                            string nextSTime = Task.Instance.GetNextStartDateByType(Int64.Parse(model.RunType), model.Dayth, null, model.Interval);

                            model.NextStartDate = Convert.ToDateTime(nextSTime);
                            model.Status = result ? "正常" : "启动失败";
                            model.Enable = (model.RunType == "5") ? "0" : "1";//运行一次 的执行后设置不可用 
                            this._bllTask.Update(model, " Id=" + model.Id);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.SaveLog("Task StartTask", "更新启动列表配置出错" + ex.ToString() + "\r\n");
                    }
                }
            }
            catch (Exception e)
            {
                Log.SaveLog("Task StartTask", "任务出现异常" + e.ToString());
            }
        }

        /// <summary>
        /// 开始提醒
        /// </summary>
        /// <param name="taskType">任务类别</param>
        /// <param name="title">标题</param>
        /// <param name="remark">任务说明</param>
        /// <param name="audioName">声音名称</param>
        /// <param name="isTest">是否测试，测试关机时只提醒不关机</param>
        /// <returns></returns>
        public bool StartWarn(AutoTask model, bool isTest)
        {
            bool result = true;
            string msg = "";
            string command = "";
            bool isShutdown = false;//是否关机

            TimedTask.Model.SysLog modLog = new TimedTask.Model.SysLog();
            modLog.TaskId = model.Id;
            modLog.TaskType = model.TaskType;
            modLog.RunType = model.RunType;
            modLog.IsRun = "1";
            modLog.Title = model.Title;
            modLog.CreateDate = DateTime.Now;

            #region 关机/显示器/锁屏

            if (model.TaskType == ((Int32)TaskType.Shutdown).ToString())//关机
            {
                msg = "系统将于 120 秒后关闭，此操作不能撤销，请保存好您的工作！";
                command = "shutdown -s -t 120";
                isShutdown = true;
            }
            else if (model.TaskType == ((Int32)TaskType.TurnOffMonitor).ToString())//关闭显示器
            {
                Helper.Instance.CloseMonitor();
                this._bllLog.Add(modLog);
                return true;
            }
            else if (model.TaskType == ((Int32)TaskType.TurnOnMonitor).ToString())//打开显示器
            {
                Helper.Instance.OpenMonitor();
                this._bllLog.Add(modLog);
                return true;
            }
            else if (model.TaskType == ((Int32)TaskType.LockMonitor).ToString())//锁屏
            {
                if (!TimedTask.Model.PM.IsScreenLock)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        View.ScreenLock lockScreen = new View.ScreenLock();
                        lockScreen.IsTest = isTest;
                        lockScreen.PointText = model.Remark.Contains("⊙") ? model.Remark.Split('⊙')[1] : model.Remark;
                        lockScreen.ShowDialog();
                    }));
                }
                this._bllLog.Add(modLog);
                return true;
            }
            #endregion

            #region 声音 POP提醒
            try
            {
                if (model.AudioEnable == null)
                    model.AudioEnable = "0";

                if (model.AudioEnable == "1" || model.AudioEnable.Equals("True", StringComparison.CurrentCultureIgnoreCase))
                {
                    //创建异步线程
                    System.Threading.Tasks.Task.Factory.StartNew(() =>
                    {
                        Helper.Instance.PalyAudio(model.AudioPath, model.AudioVolume);
                    });
                }
                if (!isShutdown)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        View.PopUP pop = new View.PopUP();
                        if (model.Remark.Contains(TimedTask.Model.PM.SpiderChar))
                        {
                            pop.Subject = model.Remark.Split(TimedTask.Model.PM.SpiderChar)[0];
                            pop.Info = model.Remark.Split(TimedTask.Model.PM.SpiderChar)[1] + msg;
                        }
                        else
                        {
                            pop.Subject = model.Remark + msg;
                        }
                        pop.PopTitle = model.Title;
                        pop.Show();
                    }));
                }
            }
            catch (Exception ex)
            {
                Log.SaveLog("Task StartWarn", ex.ToString());
                result = false;
                modLog.IsRun = "0";
            }
            #endregion

            #region 关机

            if (!isTest)
            {
                MainWindow.TaskIcon.ShowBalloonTip("温馨提示", msg, Hardcodet.Wpf.TaskbarNotification.BalloonIcon.Info);
                Helper.Instance.Run(command);
            }
            #endregion

            this._bllLog.Add(modLog);
            return result;
        }
        /// <summary>
        /// 启动程序
        /// </summary>
        /// <param name="name">任务名称</param>
        /// <param name="proccessName">进程名</param>
        /// <param name="path">程序路径</param>
        /// <param name="startParameters">启动参数</param>
        /// <returns>是否启动成功</returns>
        public bool StartApp(AutoTask model, string proccessName)
        {
            //杀死
            Helper.Instance.EndApp(proccessName);
            //启动
            System.Threading.Thread.Sleep(2000);
            if (!File.Exists(model.ApplicationPath))//不存在
            {
                return false;
            }
            try
            {
                if (model.StartParameters.Length > 0)
                {
                    System.Diagnostics.Process.Start(model.ApplicationPath, model.StartParameters);
                }
                else
                {
                    System.Diagnostics.Process.Start(model.ApplicationPath);
                }
            }
            catch (Exception ex)
            {
                string msg = "程序启动错误，路径：" + model.ApplicationPath + (model.StartParameters.Length == 0 ? "" :
                   ",参数为：" + model.StartParameters) + ex.ToString();
                Log.SaveLog("Task StartApplication", msg);
            }

            TimedTask.Model.SysLog log = new TimedTask.Model.SysLog();
            log.TaskId = model.Id;
            log.Title = model.Title;
            log.IsRun = "0";
            log.RunType = model.RunType;
            log.TaskType = model.TaskType;
            log.CreateDate = DateTime.Now;

            #region 检测程序启动信息

            foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcesses())
            {
                if (proccessName == p.ProcessName)
                {
                    log.IsRun = "1";
                    this._bllLog.Add(log);
                    return true;
                }
            }

            return false;

            #endregion
        }

        /// <summary>
        /// 获取下次启动时间
        /// </summary>
        /// <param name="timeType">启动类型</param>
        /// <param name="dayth">每月第几日</param>
        /// <param name="interval"></param>
        /// <returns></returns>
        public string GetNextStartDateByType(long timeType, long dayth, string weeks, long interval)
        {
            string result = "";
            /*
                Year = 0,
                Month = 1,
                Day = 2,
                Hour = 3,
                Minute = 4,
                Once = 5
                Week=6
                 */
            string dateFormat = "yyyy-MM-dd HH:mm:00";
            switch (timeType)
            {
                case 5://一次
                    result = DateTime.Now.ToString(dateFormat);
                    break;
                case 1://每月
                    result = Convert.ToDateTime(String.Format(DateTime.Now.AddMonths(1).ToString("yyyy-MM-{0} HH:mm:00"), dayth)).ToString(dateFormat); //DateTime.Now.AddMonths(1).ToString(dateFormat).ToString();
                    break;
                case 2:
                    result = (DateTime.Now.AddDays(1)).ToString(dateFormat);
                    break;
                case 3:
                    result = (DateTime.Now.AddHours(1)).ToString(dateFormat);
                    break;
                case 4:
                    result = (DateTime.Now.AddMinutes(interval)).ToString(dateFormat);
                    break;
                case 6:
                    result = GetNextStartDateByWeek(weeks, null);
                    break;
            }
            return result;
        }
        /// <summary>
        /// 获取 周启动 下次启动时间
        /// </summary>
        /// <param name="weekList">选中的星期 格式为：1|2|5|7</param>
        /// <param name="startTime">第一次启动时间</param>
        /// <returns></returns>
        public string GetNextStartDateByWeek(string weekList, DateTime? startTime)
        {
            string result = "";

            string dateFormat = "yyyy-MM-dd HH:mm:00";
            int currWeek = ((int)DateTime.Now.DayOfWeek);
            if (String.IsNullOrEmpty(weekList))
                return String.Empty;
            if (startTime == null)
                startTime = DateTime.Now;

            string[] weeks = weekList.Split('|');
            if (weeks.Contains(currWeek.ToString()))
            {
                if (startTime != null && startTime > DateTime.Now)
                    return Convert.ToDateTime(startTime).ToString(dateFormat);//当天启动
            }
            // >currWeek 的 第一个星期
            int first7Week = 0;
            foreach (var m in weeks)
            {
                if (Int32.Parse(m) > currWeek)
                {
                    first7Week = Int32.Parse(m);
                    break;
                }
            }
            //明天 currWeek<x< 7
            if (first7Week != 0)
                result = Convert.ToDateTime(startTime).AddDays(1).ToString(dateFormat);
            //明天 x<currWeek< 7
            else if (Int32.Parse(weeks.First()) < currWeek)
                result = Convert.ToDateTime(startTime).AddDays(7 - currWeek + Int32.Parse(weeks.First())).ToString(dateFormat);// 下周时间

            return result;
        }
        /// <summary>
        /// 获取任务列表
        /// </summary>
        /// <param name="isFirstLoad">是否是第一次加载</param>
        /// <returns></returns>
        public List<TimedTask.Model.AutoTask> GetTaskList(bool isFirstLoad)
        {
            Bll.AutoTask _bllTask = new Bll.AutoTask();
            List<TimedTask.Model.AutoTask> list = _bllTask.GetList(" 1=1 ", null, " CreateDate DESC"); //Helper.Instance.GetTaskList(_xml);//任务列表
            if (list == null || list.Count == 0)
                return null;

            if (isFirstLoad)
            {
                List<TimedTask.Model.AutoTask> listTmp = list.Where(m => m.TaskType == "5").ToList<TimedTask.Model.AutoTask>();//锁屏任务 下次启动时间从打开软件算起
                if (listTmp.Count > 0)
                {
                    foreach (TimedTask.Model.AutoTask m in listTmp)
                    {
                        m.NextStartDate = Convert.ToDateTime(Task.Instance.GetNextStartDateByType(Int32.Parse(m.RunType), m.Dayth, null, m.Interval));
                        _bllTask.Update(m, " Id=" + m.Id);
                    }
                }
                listTmp = list.Where(m => m.TaskType == "2").ToList<TimedTask.Model.AutoTask>();//关机任务 日期换成当前年月日 
                if (listTmp.Count > 0)
                {
                    foreach (TimedTask.Model.AutoTask m in listTmp)
                    {
                        string nextDate = ((DateTime)m.NextStartDate).ToString("yyyy-MM-dd");
                        m.NextStartDate = Convert.ToDateTime(
                            ((DateTime)m.NextStartDate).ToString("yyyy-MM-dd HH:mm:00").Replace(nextDate, DateTime.Now.ToString("yyyy-MM-dd"))
                            );
                        _bllTask.Update(m, " Id=" + m.Id);
                    }
                }
            }
            return list;
        }

        public void ItemClick(string type, AutoTask mod)
        {
            // 1:查看 2：删除 3：禁用 
            if (type == "1")
            {
                View.TaskEdit vTask = new View.TaskEdit();
                vTask.ID = mod.Id;
                vTask.ShowDialog();
            }
            else if (type == "2")
            {
                MessageBoxResult mbr = MessageBox.Show("确定删除？", "警告", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (mbr == MessageBoxResult.Yes)
                {
                    try
                    {
                        _bllTask.Delete(" Id=" + mod.Id);
                        MessageBox.Show("操作成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("操作成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        Log.SaveLog("MainWindow DropList 删除选中项", ex.ToString());
                    }
                }
            }
            else if (type == "3")
            {
                mod.Enable = (mod.Enable == "0") ? "1" : "0";
                _bllTask.Update(mod, " Id=" + mod.Id);
                MessageBox.Show("操作成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
