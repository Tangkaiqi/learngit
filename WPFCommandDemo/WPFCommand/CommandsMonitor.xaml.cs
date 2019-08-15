using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPFCommand
{
    /// <summary>
    /// CommandsMonitor.xaml 的交互逻辑
    /// </summary>
    public partial class CommandsMonitor : Window
    {
        private static RoutedUICommand undo;
        public static RoutedUICommand Undo
        {
            get { return CommandsMonitor.undo; }
        }

        static CommandsMonitor()
        {
            undo = new RoutedUICommand("Undo", "Undo", typeof(CommandsMonitor));
        }

        public CommandsMonitor()
        {
            InitializeComponent();
            // 按下菜单栏按钮时，PreviewExecutedEvent事件会被触发2次，即CommandExecuted事件处理程序被触发了2次
            // 一次是菜单栏按钮本身，一次是目标源触发命令的执行，所以在CommandExecuted要过滤掉不关心的命令源
            this.AddHandler(CommandManager.PreviewExecutedEvent, new ExecutedRoutedEventHandler(CommandExecuted));
        }

        public void CommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            // 过滤掉命令源是菜单按钮的，因为我们只关心Textbox触发的命令
            if (e.Source is ICommandSource)
                return;
            // 过滤掉Undo命令
            if (e.Command == CommandsMonitor.Undo)
                return;

            TextBox txt = e.Source as TextBox;
            if (txt != null)
            {
                RoutedCommand cmd = e.Command as RoutedCommand;
                if (cmd != null)
                {
                    CommandHistoryItem historyItem = new CommandHistoryItem()
                    {
                        CommandName = cmd.Name,
                        ElementActedOn = txt,
                        PropertyActedOn = "Text",
                        PreviousState = txt.Text
                    };

                    ListBoxItem item = new ListBoxItem();
                    item.Content = historyItem;
                    lstHistory.Items.Add(item);
                }

            }
        }

        private void window_Unloaded(object sender, RoutedEventArgs e)
        {
            this.RemoveHandler(CommandManager.PreviewExecutedEvent, new ExecutedRoutedEventHandler(CommandExecuted));
        }

        private void UndoCommand_Executed(object sender, RoutedEventArgs e)
        {
            ListBoxItem item = lstHistory.Items[lstHistory.Items.Count - 1] as ListBoxItem;

            CommandHistoryItem historyItem = item.Content as CommandHistoryItem;
            if (historyItem == null)
            {
                return;
            }
                
            if (historyItem.CanUndo)
            {
                historyItem.Undo();
            }
            lstHistory.Items.Remove(item);
        }

        private void UndoCommand_CanExecuted(object sender, CanExecuteRoutedEventArgs e)
        {
            if (lstHistory == null || lstHistory.Items.Count == 0)
            {
                e.CanExecute = false;
            }
            else
            {
                e.CanExecute = true;
            }
        }
    }

    public class CommandHistoryItem
    {
        public String CommandName { get; set; }
        public UIElement ElementActedOn { get; set; }

        public string PropertyActedOn { get; set; }

        public object PreviousState { get; set; }

        public bool CanUndo
        {
            get { return (ElementActedOn != null && PropertyActedOn != ""); }
        }

        public void Undo()
        {
            Type elementType = ElementActedOn.GetType();
            PropertyInfo property = elementType.GetProperty(PropertyActedOn);
            property.SetValue(ElementActedOn, PreviousState, null);
        }
    }
}
