using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DPSample
{
    // 1. 使类型继承DependencyObject类
    public class Person : DependencyObject
    {
        // 2. 声明一个静态只读的DependencyProperty 字段
        public static readonly DependencyProperty nameProperty;
       
        static Person()
        {
            // 3. 注册定义的依赖属性
            nameProperty = DependencyProperty.Register("Name", typeof(string), typeof(Person), 
                new PropertyMetadata("Learning Hard",OnValueChanged)); 
        }

        // 4. 属性包装器，通过它来读取和设置我们刚才注册的依赖属性
        public string Name
        {
            get { return (string)GetValue(nameProperty); }
            set { SetValue(nameProperty, value); }
        }

        private static void OnValueChanged(DependencyObject dpobj, DependencyPropertyChangedEventArgs e)
        {
            // 当只发生改变时回调的方法
        }

    }
   
    public class SamplePropertyClass
    {
        private string sampleProperty;
        // .NET 中的属性定义
        public string SampleProperty
        {
            get { return sampleProperty; }
            set 
            {
                if (value != null)
                {
                    sampleProperty = value;
                }
                else
                {
                    sampleProperty = "Learning Hard";
                }
            }
        }
    }
}
