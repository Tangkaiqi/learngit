using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TestDemo.Common
{
    public class GridInfo
    {
        public GridInfo()
        {
            
        }

        /// <summary>
        /// 是否选中
        /// </summary>
        private bool selected = false;
        /// <summary>
        /// 列头 -1 内容 0 行头 1
        /// </summary>
        private int grid = 0;
        private Rectangle rec;
    }
}
