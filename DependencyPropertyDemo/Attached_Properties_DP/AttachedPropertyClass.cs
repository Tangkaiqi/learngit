using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Attached_Properties_DP
{
    public class AttachedPropertyClass
    {
        // 通过使用RegisterAttached来注册一个附加属性
        public static readonly DependencyProperty IsAttachedProperty =
            DependencyProperty.RegisterAttached("IsAttached", typeof(bool), typeof(AttachedPropertyClass),
            new FrameworkPropertyMetadata((bool)false));

        // 通过静态方法的形式暴露读的操作
        public static bool GetIsAttached(DependencyObject dpo)
        {
            return (bool)dpo.GetValue(IsAttachedProperty);
        }

        public static void SetIsAttached(DependencyObject dpo, bool value)
        {
            dpo.SetValue(IsAttachedProperty, value);
        }
    }
}
