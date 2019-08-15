using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Listening_Changes_DP
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //第二种方法，通过OverrideMetadata
            DependencyPropertyDescriptor descriptor = DependencyPropertyDescriptor.FromProperty(TextBox.TextProperty, typeof(TextBox));
            descriptor.AddValueChanged(tbxEditMe, tbxEditMe_TextChanged);
        }

        private void tbxEditMe_TextChanged(object sender, EventArgs e)
        {
            MessageBox.Show("", "Changed");
        }
    }

    public class MyTextBox : TextBox
    {
        public MyTextBox()
            : base()
        {
        }

        static MyTextBox()
        {
            //第一种方法，通过OverrideMetadata
            TextProperty.OverrideMetadata(typeof(MyTextBox), new FrameworkPropertyMetadata(new PropertyChangedCallback(TextPropertyChanged)));
        }

        private static void TextPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            MessageBox.Show("", "Changed");
        }
    }
}
