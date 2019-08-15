using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CustomLayoutControl
{
    public class CustomPanel :Panel
    {
        public CustomPanel()
            : base()
        { 
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            double totalWidth = 0;
            double totalHeight = 0;
            foreach (UIElement child in InternalChildren)
            {
                child.Measure(availableSize);
                Size childSize = child.DesiredSize;
                totalWidth +=childSize.Width;
                totalHeight+=childSize.Height;
            }

            // 返回所有子元素在布局过程中所需的大小
            return new Size(totalWidth, totalHeight);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            Point currentPosition = new Point();
            int elementIndex = 0;
            int parentIndex = 0;
            double parentWidth = 0;
            foreach (UIElement child in InternalChildren)
            {
                if (elementIndex % 3 == 0)
                {
                    // 第一个子元素
                    Rect childRect = new Rect(currentPosition, child.DesiredSize);

                    // 排列第一个子元素
                    child.Arrange(childRect);

                    // 添加偏移量
                    currentPosition.Offset(childRect.Width, childRect.Height / 2 - childRect.Height);
                    parentIndex = elementIndex;
                    parentWidth = child.DesiredSize.Width;
                }
                else if (elementIndex - parentIndex == 1)
                {
                    // 第2个子元素
                    Rect childRect = new Rect(currentPosition, child.DesiredSize);
                    // 排列第2个子元素 
                    child.Arrange(childRect);
                    // 添加偏移量
                    currentPosition.Offset(0, childRect.Height);
                }
                else if (elementIndex - parentIndex == 2)
                {
                    // 第3个子元素
                    Rect childRect = new Rect(currentPosition, child.DesiredSize);
                    // 排列第3个子元素  
                    child.Arrange(childRect);
                    // 添加偏移量以为第四个子元素设置初始位置
                    currentPosition.Offset(-parentWidth, childRect.Height + childRect.Height / 2);
                }
                elementIndex++;
            }
      
            // 返回用来布局子元素的实际大小
            return finalSize; 
        }
    }
}
