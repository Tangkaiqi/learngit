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

namespace CustomLayoutControl
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //CustomSimplePanelTest();
            //CustomPanelTest();
        }
        public void CustomSimplePanelTest()
        {
            CustomSimplePanel customSimplePanel = new CustomSimplePanel();
            customSimplePanel.Margin = new Thickness(10);
            customSimplePanel.Background = new SolidColorBrush(Colors.Red);
            this.Content = customSimplePanel;

            // Button 1
            Button btn1 = new Button();
            btn1.Content = "Button 1";
            customSimplePanel.Children.Add(btn1);
        }
        public void CustomPanelTest()
        {
            CustomPanel customPanel1 = new CustomPanel();
            customPanel1.Margin = new Thickness(10);
            customPanel1.Background = new SolidColorBrush(Colors.Green);
            this.Content = customPanel1;

            // Button 1
            Button btn1 = new Button();
            btn1.Content = "Button 1";
            customPanel1.Children.Add(btn1);

            // Button 2
            Button btn2 = new Button();
            btn2.Content = "Button 2";
            customPanel1.Children.Add(btn2);

            // Button 3
            Button btn3 = new Button();
            btn3.Content = "Button 3";
            customPanel1.Children.Add(btn3);

            // Button 3
            Button btn4 = new Button();
            btn4.Content = "Button 4";
            customPanel1.Children.Add(btn4);
        }
    }
}
