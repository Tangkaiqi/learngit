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

namespace WPFCommand
{
    /// <summary>
    /// CustomCommand.xaml 的交互逻辑
    /// </summary>
    public partial class CustomCommand : Window
    {
        public CustomCommand()
        {
            InitializeComponent();
        }

        private void RequeryCommand_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Requery");
        }
    }

    public class CustomCommands
    {
        private static RoutedUICommand requeryCommand;
        static CustomCommands()
        {
            InputGestureCollection inputs = new InputGestureCollection();
            inputs.Add(new KeyGesture(Key.R, ModifierKeys.Control, "Ctrl+R"));
            requeryCommand = new RoutedUICommand("Requery", "Requery", typeof(CustomCommands), inputs);
        }

        public static RoutedUICommand Requery
        {
            get { return requeryCommand; }
        }
    }
}
