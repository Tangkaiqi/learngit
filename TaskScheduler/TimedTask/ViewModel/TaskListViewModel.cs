using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Windows;
using TimedTask.Utility;

//----------------------------------------------------------------*/
// 版权所有：
// 文 件 名：TaskListViewModel.cs
// 功能描述：
// 创建标识：m.sh.lin0328@163.com 2014/6/22 14:33:16
// 修改描述：
//----------------------------------------------------------------*/
namespace TimedTask.ViewModel
{
    public class TaskListViewModel : ViewModelBase
    {
        private Bll.AutoTask _bllTask = new Bll.AutoTask();
        public TaskListViewModel()
        {
            if (!IsInDesignMode)
            {
                Load();
                this._autoTask = new TimedTask.Model.AutoTask();
                this.LoadCommand = new RelayCommand(() => Load());
                this.SaveCommand = new RelayCommand(() => Save());
                this.AddCommand = new RelayCommand(() => new View.TaskEdit().ShowDialog());
                this.ContextMenuCommand = new RelayCommand<string>(n => ContextClick(n));
            }

            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 60000;//1分钟执行一次
            timer.Elapsed += new System.Timers.ElapsedEventHandler(Timer_Task);
            timer.Start();
        }
        private void Timer_Task(object sender, System.Timers.ElapsedEventArgs e)
        {
            Load();
        }

        #region 属性

        private TimedTask.Model.AutoTask _autoTask;
        private List<TimedTask.Model.AutoTask> _taskList;
        private string _taskMsg = "您可在右侧面板添加或修改提醒铃声...";

        /// <summary> 任务信息 </summary>
        public string TaskMsg
        {
            get { return _taskMsg; }
            set
            {
                this._taskMsg = value;
                base.RaisePropertyChanged("TaskCount");
            }
        }
        /// <summary> 任务 </summary>
        public TimedTask.Model.AutoTask AutoTask
        {
            get { return _autoTask; }
            set
            {
                this._autoTask = value;
                base.RaisePropertyChanged("AutoTask");
            }
        }

        /// <summary>
        /// 任务列表
        /// </summary>
        public List<TimedTask.Model.AutoTask> TaskList
        {
            get { return _taskList; }
            set
            {
                _taskList = value;
                base.RaisePropertyChanged("TaskList");
            }
        }
        /// <summary>
        /// 声音列表
        /// </summary>
        public ObservableCollection<TimedTask.Model.Audio> AudioList { get; set; }
        #endregion

        #region 命令

        public RelayCommand<TimedTask.Model.AutoTask> _taskSelectedChangedCommand;
        public RelayCommand _audioSelectedChangedCommand;
        /// <summary> 声音列表选择 </summary>
        public RelayCommand AudioSelectedChangedCommand
        {
            get
            {
                if (_audioSelectedChangedCommand == null)
                {
                    _audioSelectedChangedCommand = new RelayCommand(() =>
                    {
                        Helper.Instance.StopAudio();
                        if (AutoTask == null)
                            return;
                        Helper.Instance.PalyAudio(AutoTask.AudioPath, AutoTask.AudioVolume);
                    });
                }
                return _audioSelectedChangedCommand;
            }
        }
        /// <summary> 列表选择 </summary>
        public RelayCommand<TimedTask.Model.AutoTask> TaskSelectedChangedCommand
        {
            get
            {
                if (_taskSelectedChangedCommand == null)
                {
                    _taskSelectedChangedCommand = new RelayCommand<TimedTask.Model.AutoTask>((n) =>
                    {
                        this.AutoTask = (TimedTask.Model.AutoTask)n;
                    });
                }
                return _taskSelectedChangedCommand;
            }
        }
        /// <summary> 保存  </summary>
        public RelayCommand SaveCommand { get; set; }
        /// <summary> 添加  </summary>
        public RelayCommand AddCommand { get; set; }
        /// <summary> 加载  </summary>
        public RelayCommand LoadCommand { get; set; }
        /// <summary> 上下文菜单  </summary>
        public RelayCommand<string> ContextMenuCommand { get; set; }
        #endregion

        #region 方法

        private void ContextClick(string type)
        {
            string proccessName = "";
            if (type == "1" || type == "2" || type == "3" || type == "4")
            {
                if (this.AutoTask.ApplicationPath != null)
                    proccessName = this.AutoTask.ApplicationPath.Substring(this.AutoTask.ApplicationPath.LastIndexOf("\\") + 1).Replace(".exe", "");
            }
            try
            {
                switch (type)
                {
                    case "1"://运行
                        StartItem(proccessName);
                        break;
                    case "2"://停止实例 0：定时任务 结束进程 否则 停止播放声音
                        if (this.AutoTask.TaskType == "0") Helper.Instance.EndApp(proccessName);
                        else Helper.Instance.StopAudio();
                        break;
                    case "3": //运行记录
                        View.TaskRunLog trl = new View.TaskRunLog();
                        trl.ID = this.AutoTask.Id;
                        trl.Show();
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.SaveLog("MainWindow cmClick 运行", ex.ToString());
                MessageBox.Show("操作失败！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        //运行选中项
        private void StartItem(string proccessName)
        {
            if (this.AutoTask == null)
                return;

            if (this.AutoTask.TaskType != null && this.AutoTask.TaskType != "0")
            {
                Service.Task.Instance.StartWarn(this.AutoTask, true);
                return;
            }
            if (!File.Exists(this.AutoTask.ApplicationPath))
            {
                MessageBox.Show("运行失败，程序没有找到！", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {   //杀死进程
                Helper.Instance.EndApp(proccessName);
                if (this.AutoTask.StartParameters.Length > 0)
                {
                    System.Diagnostics.Process.Start(this.AutoTask.ApplicationPath, this.AutoTask.StartParameters);
                }
                else
                {
                    System.Diagnostics.Process.Start(this.AutoTask.ApplicationPath);//可调用bat文件
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("操作失败！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                Log.SaveLog("MainWindow DropList 删除选中项", ex.ToString());
            }
        }
        private void Save()
        {
            if (this.AutoTask == null)
            {
                MessageBox.Show("没有任何选中项！", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            try
            {
                _bllTask.Update(this.AutoTask, " Id=" + this.AutoTask.Id);
                MessageBox.Show("操作成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                Log.SaveLog("NoteListModule btnOK_Click", ex.ToString());
                MessageBox.Show("系统异常，操作失败！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Load()
        {
            this.TaskList = TimedTask.Service.Task.Instance.GetTaskList(false);
            this.AudioList = new ObservableCollection<TimedTask.Model.Audio>();
            if (this.TaskList.Count > 0)
            {
                this._taskMsg = "共有 " + this.TaskList.Count + " 条记录，您可在右侧面板添加或修改提醒铃声...";
            }
            if (this.AudioList.Count == 0)
            {
                foreach (DictionaryEntry de in TimedTask.Model.PM.AudioHt)
                {
                    this.AudioList.Add(new TimedTask.Model.Audio { Name = de.Key.ToString(), Path = de.Key.ToString() });
                }
            }
        }
        #endregion
    }
}
