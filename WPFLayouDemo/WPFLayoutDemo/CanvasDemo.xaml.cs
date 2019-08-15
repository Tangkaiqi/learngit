using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WPFLayoutDemo
{
    /// <summary>
    /// CanvasDemo.xaml 的交互逻辑
    /// </summary>
    public partial class CanvasDemo : Window
    {
        public CanvasDemo()
        {
            InitializeComponent();

            Canvas canv = new Canvas();
            canv.Margin = new Thickness(10, 10, 10, 10);
            canv.Background = new SolidColorBrush(Colors.White);

            // 把canv添加为窗体的子控件
            this.Content = canv;

            // Rectangle
            Rectangle rect = new Rectangle();
            rect.Fill = new SolidColorBrush(Colors.Black);
            rect.Stroke = new SolidColorBrush(Colors.Red);
            rect.Width = 200;
            rect.Height = 200;
            rect.SetValue(Canvas.LeftProperty, (double)300);
            rect.SetValue(Canvas.TopProperty, (double)180);
            canv.Children.Add(rect);

            // Ellipse
            Ellipse el = new Ellipse();
            el.Fill = new SolidColorBrush(Colors.Azure);
            el.Stroke = new SolidColorBrush(Colors.Green);
            el.Width = 180;
            el.Height = 180;
            el.SetValue(Canvas.LeftProperty, (double)160);
            // 必须转换为double，否则执行会出现异常
            // 详细介绍见：http://msdn.microsoft.com/zh-cn/library/system.windows.controls.canvas.top(v=vs.110).aspx
            el.SetValue(Canvas.TopProperty, (double)150);
            el.SetValue(Panel.ZIndexProperty, -1);
            canv.Children.Add(el);

            // Print Zindex Value 
            int zRectIndex = (int)rect.GetValue(Panel.ZIndexProperty);
            int zelIndex = (int)el.GetValue(Panel.ZIndexProperty);
            Debug.WriteLine("Rect ZIndex is: {0}", zRectIndex);
            Debug.WriteLine("Ellipse ZIndex is: {0}", zelIndex);
        }
    }
}
