using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFBindingDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Binding binding = new Binding();
            binding.Source = sliderFontSize;
            binding.Path = new PropertyPath("Value");
            binding.Mode = BindingMode.TwoWay;
            lbtext.SetBinding(TextBlock.FontSizeProperty, binding);
        }

        private void cmd_SetSmall(object sender, RoutedEventArgs e)
        {
            
            // 仅仅在双向模式下工作
            lbtext.FontSize = 5;
        }

        private void cmd_SetNormal(object sender, RoutedEventArgs e)
        {
            sliderFontSize.Value = 20;
        }

        private void cmd_SetLarge(object sender, RoutedEventArgs e)
        {
            // 仅仅在双向模式下工作
            lbtext.FontSize = 40;
        }
    }
}
