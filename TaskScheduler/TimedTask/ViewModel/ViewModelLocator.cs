using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;//控制反转，用来创建实例
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
//using System.ServiceModel:分析rss和atom都是非常便捷的
using System.Linq;
using System.Text;

namespace TimedTask.ViewModel
{
    /// <summary>
    /// 用来管理ViewModel类的
    /// </summary>
    public class ViewModelLocator : ViewModelBase
    {
        static ViewModelLocator()
        {
            // 方法执行顺序：
            // 1.App构造函数
            // 2.ViewModelLocator构造函数（App.xaml中的资源添加了ViewModelLocator）
            // 3.App的Application_Launching方法
            // 4.Navigate方法(App.RootFrame不为空)
            // 5.取得对应的ViewModel(MainViewModel)，执行对应的依赖注入的委托
            // 正确写法,不立即注入(默认值)
            // 此处每个ViewModel中的navigationService相同（可考虑改进，适合使用单例模式）

            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);//添加 Ioc 容器
            SimpleIoc.Default.Register<MainViewModel>();//添加M ainViewModel 到管理器当中
            SimpleIoc.Default.Register<NoteViewModel>();
            SimpleIoc.Default.Register<SysLogViewModel>();
            SimpleIoc.Default.Register<ConfigViewModel>();
            SimpleIoc.Default.Register<TaskListViewModel>();
            SimpleIoc.Default.Register<TaskEditViewModel>();
            SimpleIoc.Default.Register<PageImageDownViewModel>();
        }
        public MainViewModel MainVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }
        public PageImageDownViewModel ImageDownVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<PageImageDownViewModel>();
            }
        }
        public NoteViewModel NoteVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<NoteViewModel>();
            }
        }
        public SysLogViewModel SysLogVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SysLogViewModel>();
            }
        }
        public ConfigViewModel ConfigVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ConfigViewModel>();
            }
        }
        public TaskListViewModel TaskListVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<TaskListViewModel>();
            }
        }
        public TaskEditViewModel TaskEditVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<TaskEditViewModel>();
            }
        }
    }
}
