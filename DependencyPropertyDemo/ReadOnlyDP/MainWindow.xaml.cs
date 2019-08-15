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
using System.Windows.Threading;

namespace ReadOnlyDP
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // 内部使用SetValue来设置值
            SetValue(counterKey, 8);
        }

        // 属性包装器，只提供GetValue，你也可以设置一个private的SetValue进行限制。
        public int Counter
        {
            get { return (int)GetValue(counterKey.DependencyProperty); }
        }

        // 使用RegisterReadOnly来代替Register来注册一个只读的依赖属性
        private static readonly DependencyPropertyKey counterKey =
            DependencyProperty.RegisterReadOnly("Counter",
            typeof(int),
            typeof(MainWindow),
            new PropertyMetadata(0));
    }
}
