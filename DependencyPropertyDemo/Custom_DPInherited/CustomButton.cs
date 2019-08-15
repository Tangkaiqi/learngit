using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


namespace Custom_DPInherited
{
    public class CustomStackPanel : StackPanel
    {
        public static readonly DependencyProperty MinDateProperty;

        static CustomStackPanel()
        {
            MinDateProperty = DependencyProperty.Register("MinDate", typeof(DateTime), typeof(CustomStackPanel), new FrameworkPropertyMetadata(DateTime.MinValue, FrameworkPropertyMetadataOptions.Inherits));
        }

        public DateTime MinDate
        {
            get { return (DateTime)GetValue(MinDateProperty); }
            set { SetValue(MinDateProperty, value); }
        }
    }

    public class CustomButton :Button
    {
        private static readonly DependencyProperty MinDateProperty;

        static CustomButton()
        {
            // AddOwner方法指定依赖属性的所有者，从而实现依赖属性的继承，即CustomStackPanel的MinDate属性被CustomButton控件继承。
            // 注意FrameworkPropertyMetadataOptions的值为Inherits
            MinDateProperty = CustomStackPanel.MinDateProperty.AddOwner(typeof(CustomButton), new FrameworkPropertyMetadata(DateTime.MinValue, FrameworkPropertyMetadataOptions.Inherits));
        }

        public DateTime MinDate
        {
            get { return (DateTime)GetValue(MinDateProperty); }
            set { SetValue(MinDateProperty, value); }
        }
    }
}
