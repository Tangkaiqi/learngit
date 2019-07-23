using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDemo.Common
{
    public class TablesInfo
    {
        public TablesInfo()
        { }

        private int id;

        private int sWidth = 0;
        private int sHeight = 0;
        private int x;
        private int y;
        private Rectangle rec;//XY 宽高

        private bool colFla = false;//是否是列头
        private bool rowFla = false;//是否是行头
        private bool checkedFla = false;//是否被选中

        private bool centerTable = false;//是否全选

        private bool allCheckedFla = false;//是否全选

        private Color bgColor;

        private string strTitle = "";


        public Color BgColor
        {
            get
            {
                return bgColor;
            }

            set
            {
                bgColor = value;
            }
        }

        public int X
        {
            get
            {
                return x;
            }

            set
            {
                x = value;
            }
        }

        public int Y
        {
            get
            {
                return y;
            }

            set
            {
                y = value;
            }
        }

        public int SWidth
        {
            get
            {
                return sWidth;
            }

            set
            {
                sWidth = value;
            }
        }

        public int SHeight
        {
            get
            {
                return sHeight;
            }

            set
            {
                sHeight = value;
            }
        }

        /// <summary>
        /// 是否是列头
        /// </summary>
        public bool ColFla
        {
            get
            {
                return colFla;
            }

            set
            {
                colFla = value;
            }
        }

        /// <summary>
        /// 是否是行头
        /// </summary>
        public bool RowFla
        {
            get
            {
                return rowFla;
            }

            set
            {
                rowFla = value;
            }
        }
        /// <summary>
        /// 是否被选中
        /// </summary>
        public bool CheckedFla
        {
            get
            {
                return checkedFla;
            }

            set
            {
                checkedFla = value;
            }
        }
        /// <summary>
        /// 是否全选
        /// </summary>
        public bool AllCheckedFla
        {
            get
            {
                return allCheckedFla;
            }

            set
            {
                allCheckedFla = value;
            }
        }

        public string StrTitle
        {
            get
            {
                return strTitle;
            }

            set
            {
                strTitle = value;
            }
        }

        public bool CenterTable
        {
            get
            {
                return centerTable;
            }

            set
            {
                centerTable = value;
            }
        }
    }
}
