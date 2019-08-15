using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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

namespace TimedTask.Module
{
    /// <summary>
    /// TaskListModule.xaml 的交互逻辑
    /// </summary>
    public partial class TaskListModule : UserControl
    {
        public TaskListModule()
        {
            InitializeComponent();
        }
      
        protected void ShowMenuItem_Click(object sender, EventArgs e)
        {
            TimedTask.Model.AutoTask task = (sender as Button).DataContext as TimedTask.Model.AutoTask;
            Service.Task.Instance.ItemClick("1", task);
            ((ViewModel.TaskListViewModel)base.DataContext).LoadCommand.Execute(null);
        }
        protected void DeleteMenuItem_Click(object sender, EventArgs e)
        {
            TimedTask.Model.AutoTask task = (sender as Button).DataContext as TimedTask.Model.AutoTask;
            Service.Task.Instance.ItemClick("2", task);
            ((ViewModel.TaskListViewModel)base.DataContext).LoadCommand.Execute(null);
        }
        protected void LockMenuItem_Click(object sender, EventArgs e)
        {
            TimedTask.Model.AutoTask task = (sender as Button).DataContext as TimedTask.Model.AutoTask;
            Service.Task.Instance.ItemClick("3", task);
            ((ViewModel.TaskListViewModel)base.DataContext).LoadCommand.Execute(null);
        }
    }
}
