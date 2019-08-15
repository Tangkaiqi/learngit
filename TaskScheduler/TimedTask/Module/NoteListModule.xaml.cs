using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TimedTask.Service;
using TimedTask.Utility;
using TimedTask.ViewModel;

namespace TimedTask.Module
{
    /// <summary>
    /// NoteListModule.xaml 的交互逻辑
    /// </summary>
    public partial class NoteListModule : UserControl
    {
        private Bll.Note _bllNote = new Bll.Note();
        private TimedTask.Model.Note _model = new TimedTask.Model.Note();

        public NoteListModule()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(Window_Loaded);
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.uPager.PageSize = 10;
            this.uPager.EventPaging += new EventHandler(cPager_OnPageChanged);
            BindList();
            this.uPager.Bind();
        }
        //翻页
        private void cPager_OnPageChanged(object sender, EventArgs e)
        {
            BindList();
        }
        private void Bind(string whereStr)
        {
            int rowCount = 0;
            if (String.IsNullOrEmpty(whereStr))
                whereStr = "1=1";

            List<Model.Note> list = this._bllNote.GetList(null, null, uPager.PageSize, uPager.PageIndex, "*", whereStr, " ModifyDate DESC", out rowCount);
            this.uPager.RecordCount = rowCount;
            this.uPager.list = list;
            this.lstNote.ItemsSource = list;
            this.uPager.Bind();
        }
        protected void Delete_Click(object sender, EventArgs e)
        {
            TimedTask.Model.Note note = (sender as Button).DataContext as TimedTask.Model.Note;
            Delete(note.Id);
            ((ViewModel.NoteViewModel)base.DataContext).LoadCommand.Execute(null);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Id"></param>
        private void Delete(object id)
        {
            MessageBoxResult mbr = MessageBox.Show("确定删除？", "警告", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (mbr == MessageBoxResult.Yes)
            {
                try
                {
                    int result = _bllNote.Delete(" Id=" + id);
                    MessageBox.Show(result > 0 ? "操作成功！" : "操作失败！");
                }
                catch (Exception ex)
                {
                    Log.SaveLog("MainWindow DropList 删除选中项", ex.ToString());
                }
            }
            BindList();
        }
        private void btnQuery_Click(object sender, RoutedEventArgs e)
        {
            if (this.txtQueryTitle.Text.Trim().Length == 0 && this.cboQueryType.SelectedValue == null)
            {
                MessageBox.Show("查询条件不能为空！");
                return;
            }
            uPager.PageIndex = 1;
            Control.Loading load = new Control.Loading(BindList);
            load.Msg = "稍等。。。";
            load.Start();
            load.ShowDialog();
        }
        private string GetWhere()
        {
            string whereStr = " 1=1 ";
            string title = this.txtQueryTitle.Text.Trim();
            string typeId = "";
            if (this.cboQueryType.SelectedValue != null)
                typeId = this.cboQueryType.SelectedValue.ToString();

            if (title.Length > 0)
                whereStr += " AND Title LIKE '%" + title + "%' ";
            if (typeId.Length > 0 && typeId != "0")
                whereStr += " AND TypeId= " + typeId;

            return whereStr;
        }
        private void BindList()
        {
            string str = GetWhere();
            Bind(str);
        }
        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            Model.Note model = new Model.Note();
            if ((sender as Button).CommandParameter != null)
            {
                model.Id = Int64.Parse((sender as Button).CommandParameter.ToString());
            }
            model.Title = this.txtTitle.Text;
            model.Content = this.txtContent.Text;
            if (model.Title.Length == 0 || model.Content.Length == 0)
            {
                MessageBox.Show("标题或内容不能为空！", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            model.ModifyDate = DateTime.Now;
            model.TypeId = Int64.Parse(this.cboType.SelectedValue.ToString());
            string result = Save(model);
            MessageBox.Show(result);
        }
        private string Save(Model.Note model)
        {
            string result = "";

            if (model == null || model.Title.Length == 0 || model.Content.Length == 0)
            {
                result = "标题或内容不能为空！";
                return result;
            }
            try
            {
                if (model.Id == 0)//新增
                {
                    model.CreateDate = DateTime.Now;
                    _bllNote.Add(model);
                    return "添加成功！";
                }

                model.ModifyDate = DateTime.Now;
                _bllNote.Update(model, " Id=" + model.Id);
                result = "修改成功！";
            }
            catch (Exception ex)
            {
                Log.SaveLog("NoteListModule btnOK_Click", ex.ToString());
                result = "系统异常，操作失败！";
            }
            this.cboType.SelectedIndex = 0;
            return result;
        }

        private void btnQuery_Click(object sender, SelectionChangedEventArgs e)
        {
            BindList();
        }
    }
}
