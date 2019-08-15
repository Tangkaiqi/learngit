using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using TimedTask.Service;
using TimedTask.Utility;

namespace TimedTask.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private XmlHelper _xml = null;
        private Service.Weather _balWeather = new Service.Weather();

        /// <summary>
        /// 初始化 MainViewModel
        /// </summary>
        public MainViewModel()
        {
            if (!IsInDesignMode)
            {
                Loading();
                StartTask();
                StartSpiderTask();
            }
        }

        #region 属性
        private string _WeatherInfo = "";
        private string _WeatherImg;
        private string _City;
        /// <summary> 天气信息 </summary>
        public string WeatherInfo
        {
            get { return _WeatherInfo; }
            set
            {
                this._WeatherInfo = value;
                base.RaisePropertyChanged("WeatherInfo");
            }
        }
        private string _SurveyInfo = "";
        /// <summary> 天气详细信息 </summary>
        public string SurveyInfo
        {
            get { return _SurveyInfo; }
            set
            {
                this._SurveyInfo = value;
                base.RaisePropertyChanged("SurveyInfo");
            }
        }
        /// <summary> 程序版本 </summary>
        public string Verson { get; set; }
        /// <summary> 城市 </summary>
        public string City
        {
            get { return _City; }
            set
            {
                this._City = value;
                base.RaisePropertyChanged("City");
            }
        }
        /// <summary> 天气图片 </summary>
        public string WeatherImg
        {
            get { return _WeatherImg; }
            set
            {
                this._WeatherImg = value;
                base.RaisePropertyChanged("WeatherImg");
            }
        }
        #endregion

        #region 方法

        /// <summary>
        /// 初始化
        /// </summary>
        private void Loading()
        {
            #region 温馨提示

            Service.Calendar calender = new Service.Calendar(DateTime.Now);
            string calendar = "农历：" + calender.ChineseDateString + "\r\n";
            calendar += " 时辰：" + calender.ChineseHour + "\r\n";
            calendar += " 属相：" + calender.AnimalString + "\r\n";
            calendar += (calender.ChineseTwentyFourDay.Length > 0) ? " 节气：" + calender.ChineseTwentyFourDay + "\r\n" : "";
            calendar += (calender.DateHoliday.Length > 0) ? " 节日：" + calender.DateHoliday + "\r\n" : "";
            calendar += " 星座：" + calender.Constellation + "\r\n";

            System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                View.PopUP pop = new View.PopUP();
                pop.Subject = "";
                pop.Info = calendar;
                pop.PopTitle = "温馨提示！";
                pop.Show();
            }));
            #endregion

            #region 异步

            //var action = new Action(() =>
            //{
            //    //删除4天前日志文件
            //    Bll.SysLog taskLog = new Bll.SysLog();
            //    Helper.Instance.DropFiles(Log.LogPath, null, new string[] { ".txt", ".log" }, 4);
            //    taskLog.DeleteHistory();
            //    this.Verson = "版本 V" + Helper.Instance.GetVersion();
            //});
            //action.BeginInvoke(ar =>
            //{
            //    action.EndInvoke(ar);
            //}, null);
            #endregion

            //System.Threading.Tasks.Task.Factory.StartNew(() =>
            //{

            //});
            Bll.SysLog taskLog = new Bll.SysLog();
            Helper.Instance.DropFiles(Log.LogPath, null, new string[] { ".txt", ".log" }, 4);//删除4天前日志文件
            taskLog.DeleteHistory();
            this.Verson = "版本 V" + Helper.Instance.GetVersion();

            Task.Instance.GetTaskList(true);
            Helper.Instance.CreateFolder(Log.LogPath);
            _xml = new XmlHelper(TimedTask.Model.PM.TaskConfig);

            TimedTask.Model.Area area = this._balWeather.GetCurrentArea();

            if (area != null)
                AsyncWeather(area.Name);
        }
        //抓取新闻
        private void StartSpiderTask()
        {
            System.Timers.Timer timerTask = new System.Timers.Timer();
            timerTask.Interval = 7200000;//120分钟执行一次
            timerTask.Elapsed += new System.Timers.ElapsedEventHandler(TimerSpider_Task);
            timerTask.Start();
        }
        //抓取新闻
        private void TimerSpider_Task(object sender, System.Timers.ElapsedEventArgs e)
        {
            Service.Task.Instance.SpiderNewsToHtml();
        }
        //启动任务
        private void StartTask()
        {
            System.Timers.Timer timerTask = new System.Timers.Timer();
            timerTask.Interval = 60000;//1分钟执行一次
            timerTask.Elapsed += new System.Timers.ElapsedEventHandler(Timer_Task);
            timerTask.Start();
        }
        //
        private void Timer_Task(object sender, System.Timers.ElapsedEventArgs e)
        {
            Service.Task.Instance.StartTask();
        }

        #region 异步获取天气信息

        private void AsyncWeather(string citiName)
        {
            try
            {
                Func<string, string[]> func = new Func<string, string[]>(this._balWeather.GetWeather);
                func.BeginInvoke(citiName, ar =>
                {
                    try
                    {
                        string[] weatherArr = func.EndInvoke(ar);
                        if (weatherArr != null && weatherArr.Length > 0)
                        {
                            Service.WeatherInfo _balWeatherInfo = new WeatherInfo(weatherArr);
                            if (_balWeatherInfo.CityName == null)
                                return;

                            this.City = _balWeatherInfo.CityName + "：";
                            this.WeatherInfo = _balWeatherInfo.TodaySurvey + _balWeatherInfo.TodayTemperature + " " + _balWeatherInfo.TomorrowWind;
                            string img = _balWeatherInfo.TodayStartImage.Replace("a_", "");
                            this.WeatherImg = "/Theme/Images/Weather" + img.Substring(img.LastIndexOf("/"));

                            this.SurveyInfo =
                                 _balWeatherInfo.TomorrowSurvey + _balWeatherInfo.TomorrowTemperature + " " + _balWeatherInfo.TomorrowWind + "\r\n"
                                 + _balWeatherInfo.HtSurvey + _balWeatherInfo.HtTemperature + " " + _balWeatherInfo.HtWind + "\r\n\r\n"
                                 + _balWeatherInfo.TodayWeatherSummary;
                            //this.WeatherImg = new System.Windows.Media.Imaging.BitmapImage(
                            //     new Uri(_balWeatherInfo.TodayStartImage.Replace("a_", ""), UriKind.Absolute));
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.SaveLog("MainWindow.xaml.cs EndInvokeWeather", ex.ToString());
                        this.WeatherInfo = "天气获取失败...";
                    }

                }, null);
            }
            catch (Exception ex)
            {
                Log.SaveLog("MainWindow.xaml.cs EndInvokeWeather", ex.ToString());
                this.WeatherInfo = "天气获取失败...";
            }
        }

        #endregion
        #endregion
    }
}
