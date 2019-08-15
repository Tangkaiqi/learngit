using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using TimedTask.Utility;

//----------------------------------------------------------------*/
// 版权所有：
// 文 件 名：ConfigViewModel.cs
// 功能描述：
// 创建标识：m.sh.lin0328@163.com 2014/6/21 17:05:39
// 修改描述：
//----------------------------------------------------------------*/
namespace TimedTask.ViewModel
{
    public class ConfigViewModel : ViewModelBase
    {

        private readonly string areaPath = Model.PM.StartPath + @"\Weather\AreaList.txt";
        private readonly string zonePath = Model.PM.StartPath + @"\Weather\ZoneList.txt";
        private XmlHelper _xml = null;
        private Service.Weather _balWeather = new Service.Weather();

        public ConfigViewModel()
        {
            this.ZoneList = new List<TimedTask.Model.Zone>();

            if (!IsInDesignMode)// 不是在使用Blend设计的模式下
            {
                Loading();
            }
            this.RadioCommand = new RelayCommand<int>((m) =>
            {
                if (m == 1 || m == 2)
                    this.BtnOKText = "保存";
                else if (m == 3)
                    this.BtnOKText = "保存";
            });
            this.SaveCommand = new RelayCommand(() => Save());
            this.SelectionChangedCommand = new RelayCommand<TimedTask.Model.Zone>((m) =>
            {
                this.CurrentZone = m;
                ChangeCity(this.CurrentZone.ID.ToString());
            });
            this.SelectionCityChangedCommand = new RelayCommand<TimedTask.Model.Area>((m) =>
            {
                this.CurrentArea = m;
            });
            this.SelectionMinuteChangedCommand = new RelayCommand<int>((m) =>
            {
                this.LockMinute = m;
            });
            this.OpenAppImgCommand = new RelayCommand<string>((type) => OpenImage(type));
        }

        #region 属性

        private string _btnText = "保存";
        private string _bgImg = TimedTask.Model.PM.AppBgImg;
        private string _lockImg = TimedTask.Model.PM.LockBgImg;
        private bool _isAutoRun = false;
        private bool _minToTray = false;
        private bool _saveLog = false;
        private bool _showNews = true;//启动时是否显示资讯

        private int _lockMinute = TimedTask.Model.PM.LockMinute;
        private List<int> _lstMinute;
        private List<TimedTask.Model.Area> _areaList;
        private TimedTask.Model.Area _area = new TimedTask.Model.Area();
        private TimedTask.Model.Zone _zone = new TimedTask.Model.Zone();

        /// <summary> 关闭最小化 </summary>
        public bool ShowNews
        {
            get { return _showNews; }
            set
            {
                this._showNews = value;
                base.RaisePropertyChanged("ShowNews");
            }
        }
        /// <summary> 锁屏时间 </summary>
        public List<int> LstMinute
        {
            get { return _lstMinute; }
            set
            {
                this._lstMinute = value;
                base.RaisePropertyChanged("LstMinute");
            }
        }
        /// <summary> 天气 城市 </summary>
        public TimedTask.Model.Area CurrentArea
        {
            get { return _area; }
            set
            {
                this._area = value;
                base.RaisePropertyChanged("CurrentArea");
            }
        }
        /// <summary> 天气 省 </summary>
        public TimedTask.Model.Zone CurrentZone
        {
            get { return _zone; }
            set
            {
                this._zone = value;
                base.RaisePropertyChanged("CurrentZone");
            }
        }
        /// <summary> 关闭最小化 </summary>
        public bool MinToTray
        {
            get { return _minToTray; }
            set
            {
                this._minToTray = value;
                base.RaisePropertyChanged("MinToTray");
            }
        }
        /// <summary> 锁屏时间 分钟 </summary>
        public int LockMinute
        {
            get { return _lockMinute; }
            set
            {
                this._lockMinute = value;
                base.RaisePropertyChanged("LockMinute");
            }
        }
        /// <summary> 是否保存运行日志 </summary>
        public bool SaveLog
        {
            get { return _saveLog; }
            set
            {
                this._saveLog = value;
                base.RaisePropertyChanged("SaveLog");
            }
        }
        /// <summary> 是否自启 </summary>
        public bool IsAutoRun
        {
            get { return _isAutoRun; }
            set
            {
                this._isAutoRun = value;
                base.RaisePropertyChanged("IsAutoRun");
            }
        }
        /// <summary> 保存文字 </summary>
        public string BtnOKText
        {
            get { return _btnText; }
            set
            {
                this._btnText = value;
                base.RaisePropertyChanged("BtnOKText");
            }
        }
        /// <summary> 窗体背景 </summary>
        public string BgImg
        {
            get { return _bgImg; }
            set
            {
                this._bgImg = value;
                base.RaisePropertyChanged("BgImg");
            }
        }
        /// <summary> 锁屏背景 </summary>
        public string LockImg
        {
            get { return _lockImg; }
            set
            {
                this._lockImg = value;
                base.RaisePropertyChanged("LockImg");
            }
        }
        /// <summary> 省份列表 </summary>
        public List<TimedTask.Model.Zone> ZoneList { get; set; }
        /// <summary> 城市总列表 </summary>
        public List<TimedTask.Model.Area> AllAreaList { get; set; }
        /// <summary> 城市列表 用于界面绑定 </summary>
        public List<TimedTask.Model.Area> AreaList
        {
            get { return _areaList; }
            set
            {
                this._areaList = value;
                base.RaisePropertyChanged("AreaList");
            }
        }

        #endregion

        #region 命令

        public RelayCommand<int> RadioCommand { set; get; }
        /// <summary> 保存  </summary>
        public RelayCommand SaveCommand { get; set; }

        /// <summary> 选择图片  </summary>
        public RelayCommand<string> OpenAppImgCommand { set; get; }

        /// <summary> 选择省  </summary>
        public RelayCommand<TimedTask.Model.Zone> SelectionChangedCommand { set; get; }
        /// <summary> 选择市  </summary>
        public RelayCommand<TimedTask.Model.Area> SelectionCityChangedCommand { set; get; }
        /// <summary> 锁屏  </summary>
        public RelayCommand<int> SelectionMinuteChangedCommand { set; get; }
        #endregion

        #region 方法

        private void OpenImage(string type)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.Filter = "JPG文件|*.jpg|PNG文件|*.png";//全部|*.*||批处理|*.bat";
            if (ofd.ShowDialog() == true)
            {
                if (type == "1")//窗体
                {
                    this.BgImg = ofd.FileName;
                }
                else
                {
                    this.LockImg = ofd.FileName;
                }
            }
        }
        private void Save()
        {
            if (this.BtnOKText == "确定")
            {
                //this.Close();
                return;
            }
            SysHelper sys = new SysHelper();
            sys.AutoStartup(this.IsAutoRun);
            TimedTask.Model.PM.MinToTray = this.MinToTray;

            Helper.Instance.SetValue(_xml, "LockMinute", this.LockMinute.ToString());
            Helper.Instance.SetValue(_xml, "SaveLog", this.SaveLog ? "1" : "0");
            Helper.Instance.SetValue(_xml, "ShowNews", this.ShowNews ? "1" : "0");
            try
            {
                if (!String.IsNullOrEmpty(this.BgImg))
                {
                    Helper.Instance.SetValue(_xml, "AppBgImg", this.BgImg);
                }
                if (!String.IsNullOrEmpty(this.LockImg))
                {
                    Helper.Instance.SetValue(_xml, "LockBgImg", this.LockImg);
                }
                Helper.Instance.SetValue(_xml, "MinToTray", TimedTask.Model.PM.MinToTray ? "1" : "0");
                _xml.Save();

                //保存城市信息
                if (this.CurrentArea != null && this.CurrentArea.Name != null)
                    _balWeather.SaveCurrentArea(this.CurrentArea);

                System.Windows.MessageBox.Show("保存成功，重启后生效！", "提示", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Asterisk);
            }
            catch (Exception ex)
            {
                Log.SaveLog("Config btnOK_Click", ex.ToString());
                System.Windows.MessageBox.Show("保存失败！", "警告", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        private void ChangeCity(string zoneId)
        {
            if (zoneId == null || zoneId.Length == 0
                || this.AllAreaList == null || this.AllAreaList.Count == 0)
                return;
            if (this.AreaList != null)
                this.AreaList.Clear();

            try
            {
                var citys = from m
                            in this.AllAreaList
                            where m.ZoneID == Int32.Parse(zoneId)
                            select m;
                this.AreaList = new List<TimedTask.Model.Area>(citys);
            }
            catch (Exception)
            {

            }
        }

        void LoadAreaData()
        {
            this.CurrentArea = _balWeather.GetCurrentArea();
            if (this.CurrentArea != null)
            {
                this.CurrentZone.ID = this.CurrentArea.ZoneID;
                this.CurrentArea.ID = this.CurrentArea.ID;
            }
            #region 文件中读取地域

            if (System.IO.File.Exists(areaPath) && System.IO.File.Exists(zonePath))
            {
                InitZoneFromDataSet(null, true);
                return;
            }
            #endregion
            #region 网络获取地域

            TimedTask.WeatherService.WeatherWebServiceSoapClient client = new TimedTask.WeatherService.WeatherWebServiceSoapClient("WeatherWebServiceSoap");
            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                var ds = client.getSupportDataSet();
                #region 保存到文件

                Helper.Instance.DeleteFile(areaPath);
                Helper.Instance.DeleteFile(zonePath);
                Helper.Instance.WriteFile(areaPath, Helper.Instance.JsonSerializer<DataTable>(ds.Tables[1]));
                Helper.Instance.WriteFile(zonePath, Helper.Instance.JsonSerializer<DataTable>(ds.Tables[0]));
                #endregion
                InitZoneFromDataSet(ds, false);
            });
            #endregion
        }

        /// <summary>
        /// 从DataSet中加载城市信息
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="isRadFromTxt">是否从文件中诈取数据</param>
        public void InitZoneFromDataSet(DataSet ds, bool isRadFromTxt)
        {
            this.ZoneList = new List<TimedTask.Model.Zone>();
            this.AllAreaList = new List<Model.Area>();
            DataTable dtZone = null;
            DataTable dtArea = null;

            if (isRadFromTxt)
            {
                dtArea = Helper.Instance.JsonDeserialize<DataTable>(Helper.Instance.ReadFile(areaPath));
                dtZone = Helper.Instance.JsonDeserialize<DataTable>(Helper.Instance.ReadFile(zonePath));
            }
            else
            {
                dtZone = ds.Tables[0];
                dtArea = ds.Tables[1];
            }
            this.ZoneList.Clear();
            foreach (DataRow dr in dtZone.Rows)
            {
                var zone = new TimedTask.Model.Zone()
                {
                    ID = Convert.ToInt32(dr["ID"]),
                    Name = dr["Zone"].ToString(),
                };
                var drs = dtArea.Select("ZoneID=" + zone.ID);
                if (drs == null || drs.Count() == 0)
                    continue;

                foreach (DataRow drArea in drs)
                {
                    var area = new TimedTask.Model.Area()
                    {
                        ID = Convert.ToInt32(drArea["ID"]),
                        ZoneID = Convert.ToInt32(drArea["ZoneID"]),
                        Name = drArea["Area"].ToString(),
                        AreaCode = drArea["AreaCode"].ToString()
                    };
                    this.AllAreaList.Add(area);
                }
                this.ZoneList.Add(zone);
            }
            if (this.CurrentZone != null)
                ChangeCity(this.CurrentZone.ID.ToString());
        }
        private void Loading()
        {
            this._xml = new XmlHelper(TimedTask.Model.PM.Config);
            this.BgImg = Helper.Instance.GetValue(_xml, "AppBgImg");
            this.LockImg = Helper.Instance.GetValue(_xml, "LockBgImg");

            //自动启动
            SysHelper sys = new SysHelper();
            if (sys.IsAutoStartup())
                this.IsAutoRun = true;

            this.LstMinute = new List<int>();
            for (int i = 1; i < 20; i++)
            {
                this.LstMinute.Add(i);
            }
            LoadAreaData();

            this.MinToTray = TimedTask.Model.PM.MinToTray;
            this.SaveLog = TimedTask.Model.PM.SaveLog;
            this.ShowNews = TimedTask.Model.PM.ShowNews;
        }
        #endregion
    }
}
