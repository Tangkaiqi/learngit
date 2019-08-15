using System;
using System.Collections.Generic;
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
using TimedTask.ViewModel;

namespace MSL.TimedTask.Control
{
    /// <summary>
    /// UCPager.xaml 的交互逻辑
    /// </summary>
    public partial class UCPager : UserControl
    {
        public PagerViewModel PagerVM { get; private set; }

        public UCPager()
        {
            InitializeComponent();
            PagerVM = new PagerViewModel();
            this.DataContext = PagerVM;
        }
    }
}
