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
using System.Windows.Shapes;

namespace WPFLayoutDemo
{
    /// <summary>
    /// StackPanel.xaml 的交互逻辑
    /// </summary>
    public partial class StackPanelDemo : Window
    {
        public StackPanelDemo()
        {
            InitializeComponent();
            StackPanel sp = new StackPanel();
            sp.Orientation = Orientation.Horizontal;
            sp.Margin = new Thickness(10, 10, 10, 10);
            sp.Background = new SolidColorBrush(Colors.Azure);
            //sp.Orientation = Orientation.Vertical;
            // 把sp添加为窗体的子控件
            this.Content = sp;

            // Label 
            Label lb = new Label();
            lb.Content = "A Button Stack";
            sp.Children.Add(lb);

            // Button 1
            Button btn1 = new Button();
            btn1.Content = "Button 1";
            sp.Children.Add(btn1);

            // Button 2
            Button btn2 = new Button();
            btn2.Content = "Button 2";
            sp.Children.Add(btn2);

            // Button 3
            Button btn3 = new Button();
            btn3.Content = "Button 3";
            sp.Children.Add(btn3);

            // Button 4
            Button btn4 = new Button();
            btn4.Content = "Button 4";
            sp.Children.Add(btn4);
        }
    }
}
