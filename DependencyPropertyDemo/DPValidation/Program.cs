using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DPValidation
{
    class Program
    {
        static void Main(string[] args)
        {
            SimpleDPClass sDPClass = new SimpleDPClass();
            sDPClass.SimpleDP = 2;
            Console.ReadLine();
        }
    }

    public class SimpleDPClass : DependencyObject
    {
        public static readonly DependencyProperty SimpleDPProperty =
            DependencyProperty.Register("SimpleDP", typeof(double), typeof(SimpleDPClass),
                new FrameworkPropertyMetadata((double)0.0,
                    FrameworkPropertyMetadataOptions.None,
                    new PropertyChangedCallback(OnValueChanged),
                    new CoerceValueCallback(CoerceValue)),
                    new ValidateValueCallback(IsValidValue));

        public double SimpleDP
        {
            get { return (double)GetValue(SimpleDPProperty); }
            set { SetValue(SimpleDPProperty, value); }
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Console.WriteLine("当值改变时，我们可以做的一些操作，具体可以在这里定义： {0}", e.NewValue);
        }

        private static object CoerceValue(DependencyObject d, object value)
        {
            Console.WriteLine("对值进行限定，强制值： {0}", value);
            return value;
        }

        private static bool IsValidValue(object value)
        {
            Console.WriteLine("验证值是否通过，返回bool值，如果返回True表示验证通过，否则会以异常的形式暴露： {0}", value);
            return true;
        }
    }
}
