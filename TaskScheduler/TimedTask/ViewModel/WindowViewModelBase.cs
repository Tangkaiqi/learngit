using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

//----------------------------------------------------------------*/
// 版权所有：
// 文 件 名：WindowViewModelBase.cs
// 功能描述：
// 创建标识：m.sh.lin0328@163.com 2014/7/22 21:30:27
// 修改描述：
//----------------------------------------------------------------*/
namespace TimedTask.ViewModel
{
    public class WindowViewModelBase : ViewModelBase
    {
        public WindowViewModelBase(string windowName)
            : base()
        {
            this.WindowName = windowName;
        }

        #region 属性

        private bool _isShow;
        private string _title;
        /// <summary>
        /// 窗口注册的名称
        /// </summary>
        public string WindowName { get; private set; }

        /// <summary>
        /// 窗口实体
        /// </summary>
        public Window Window { get; private set; }

        /// <summary>
        /// 窗口是否在显示窗台
        /// </summary>
        public bool IsShow
        {
            get { return _isShow; }
            set
            {
                if (_isShow != value)
                {
                    _isShow = value;
                    this.RaisePropertyChanged("IsShow");
                }
            }
        }

        /// <summary>
        /// 窗体的标题
        /// </summary>
        public virtual string Title
        {
            get { return _title; }
            set
            {
                if (_title != value)
                {
                    _title = value;
                    this.RaisePropertyChanged("Title");
                }
            }
        }
        #endregion

        #region 命令

        private RelayCommand _showCommand;
        private RelayCommand _closeCommand;
        private RelayCommand _minimizeCommand;
        private RelayCommand _maximizeCommand;
        private RelayCommand _normalCommand;
        private RelayCommand _showDialogCommand;

        /// <summary> 打开命令 </summary>
        public RelayCommand ShowCommand
        {
            get
            {
                if (_showCommand == null)
                {
                    _showCommand = new RelayCommand(this.Show);
                }
                return _showCommand;
            }
        }
        /// <summary> 关闭命令 </summary>
        public RelayCommand CloseCommand
        {
            get
            {
                if (_closeCommand == null)
                {
                    _closeCommand = new RelayCommand(this.Close);
                }
                return _closeCommand;
            }
        }
        /// <summary> 使窗体最小化命令 </summary>
        public RelayCommand MinimizeCommand
        {
            get
            {
                if (_minimizeCommand == null)
                {
                    _minimizeCommand = new RelayCommand(this.Minimize);
                }
                return _minimizeCommand;
            }
        }
        /// <summary> 使窗体最大化命令 </summary>
        public RelayCommand MaximizeCommand
        {
            get
            {
                if (_maximizeCommand == null)
                {
                    _maximizeCommand = new RelayCommand(this.Maximize);
                }
                return _maximizeCommand;
            }
        }
        /// <summary> 使窗体正常命令 </summary>
        public RelayCommand NormalCommand
        {
            get
            {
                if (_normalCommand == null)
                {
                    _normalCommand = new RelayCommand(this.Normal);
                }
                return _normalCommand;
            }
        }
        /// <summary> 打开模式窗口命令 </summary>
        public RelayCommand ShowDialogCommand
        {
            get
            {
                if (_showDialogCommand == null)
                {
                    _showDialogCommand = new RelayCommand(this.ShowDialog);
                }
                return _showDialogCommand;
            }
        }

        #endregion

        #region 方法

        //private Window CreateWindow()
        //{
        //    try
        //    {
        //        var window = this.UnityContainer.Resolve<Window>(this.WindowName);
        //        window.Closed += window_Closed;
        //        window.DataContext = this;
        //        window.Owner = System.Windows.Application.Current.MainWindow;
        //        this.Window = window;
        //        return window;
        //    }
        //    catch
        //    {
        //        string message = string.Format("打开窗口 {0} 失败!", this.Title);
        //        throw new Exception(message);
        //    }
        //}

        void window_Closed(object sender, EventArgs e)
        {
            this.Window = null;
            this.IsShow = false;
        }

        /// <summary>
        /// 显示窗口
        /// </summary>
        public virtual void Show()
        {
            if (this.Window != null)
            {
                this.Window.Show();
            }
            else
            {
                //var window = this.CreateWindow();
                //window.Show();
            }
            this.IsShow = true;
        }

        /// <summary>
        /// 打开模式窗口
        /// </summary>
        public virtual void ShowDialog()
        {
            if (this.Window != null)
            {
                this.Window.ShowDialog();
            }
            else
            {
                //var window = this.CreateWindow();
                //window.ShowDialog();
            }
            this.IsShow = true;
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        public virtual void Close()
        {
            if (this.Window != null)
            {
                this.Window.Close();
            }
        }

        /// <summary>
        /// 使窗体最小化
        /// </summary>
        public virtual void Minimize()
        {
            this.Window.WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// 使窗体最大化
        /// </summary>
        public virtual void Maximize()
        {
            this.Window.WindowState = WindowState.Maximized;
        }

        /// <summary>
        /// 使窗体正常
        /// </summary>
        public virtual void Normal()
        {
            this.Window.WindowState = WindowState.Normal;
        }
        #endregion
    }
}
