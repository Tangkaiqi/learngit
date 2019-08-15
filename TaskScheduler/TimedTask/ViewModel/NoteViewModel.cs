
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using TimedTask.Model;
using TimedTask.Module;
using TimedTask.Utility;

namespace TimedTask.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public class NoteViewModel : ViewModelBase
    {
        private TimedTask.Model.Note _note = new Note();

        private Bll.Note _bllNote = new Bll.Note();
        private Bll.TypeList _bllType = new Bll.TypeList();

        /// <summary>
        /// 构造
        /// </summary>
        public NoteViewModel()
        {
            if (!IsInDesignMode)
            {
                NoteTypeList = new ObservableCollection<TimedTask.Model.TypeList>(_bllType.GetList(" FatherId=1 ", "Id,Name", "Id"));
            }
            this.AddBtnText = "添加";
            this._note = new TimedTask.Model.Note();
            CloseCommand = new RelayCommand(Close);
            LoadCommand = new RelayCommand(Load);
            ResetCommand = new RelayCommand(() =>
            {
                this.NoteModel = new Note();
                this.NoteModel.TypeId = 3;
                this.AddBtnText = "添加";
            });
            if (!IsInDesignMode)
            {

            }
            //创建一个异步线程 
            //System.Threading.Tasks.Task.Factory.StartNew(() =>
            //{

            //});
        }

        #region 命令

        public RelayCommand<TimedTask.Model.Note> _noteSelectedChangedCommand;
        public RelayCommand ResetCommand { get; set; }
        /// <summary>
        /// 加载
        /// </summary>
        public RelayCommand LoadCommand { set; get; }
        /// <summary>
        /// 关闭
        /// </summary>
        public RelayCommand CloseCommand { set; get; }

        /// <summary>
        /// 列表选择
        /// </summary>
        public RelayCommand<TimedTask.Model.Note> NoteSelectedChangedCommand
        {
            get
            {
                if (_noteSelectedChangedCommand == null)
                {
                    _noteSelectedChangedCommand = new RelayCommand<TimedTask.Model.Note>((n) =>
                    {
                        this.NoteModel = (TimedTask.Model.Note)n;
                        this.AddBtnText = "修改";
                    });
                }
                return _noteSelectedChangedCommand;
            }
        }

        #endregion

        #region 属性
        public List<TimedTask.Model.Note> _notelist;
        /// <summary>
        /// 笔记列表
        /// </summary>
        public List<TimedTask.Model.Note> NoteList
        {
            get { return _notelist; }
            set
            {
                _notelist = value;
                base.RaisePropertyChanged("NoteList");
            }
        }
        /// <summary>
        /// 笔记类型
        /// </summary>
        public ObservableCollection<TimedTask.Model.TypeList> NoteTypeList { get; set; }

        private string _btnText = "添加";
        /// <summary>
        /// 添加按钮文字
        /// </summary>
        public string AddBtnText
        {
            get { return _btnText; }
            set
            {
                this._btnText = value;
                base.RaisePropertyChanged("AddBtnText");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public TimedTask.Model.Note NoteModel
        {
            get { return _note; }
            set
            {
                _note = value;
                base.RaisePropertyChanged("NoteModel");
            }
        }

        #endregion

        #region 方法

        /// <summary>
        /// 关闭
        /// </summary>
        private void Close()
        {
            //this.SettingsService.SaveSettings(this.UISettings);
        }
        /// <summary>
        /// 加载
        /// </summary>
        private void Load()
        {
            //Bind();
        }

        #endregion
    }
}
