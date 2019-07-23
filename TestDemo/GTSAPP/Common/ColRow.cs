using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDemo.Common
{
    public class GDIRectangle
    {
        //图形种类、大小、位置、方向、选中情况
        public int Id;
        public int Rectang;//图形种类 1矩形
        public int Width;//宽
        public int height;//高
        public Point First; //起点位置
        public Point Lase;//结束点位置
        public int Direction; //方向
        public bool CheckFlas;//是否选中
        public float PenWidth;//画笔的宽度
        public Color PenColor;//画笔的颜色
        public Graphics g;
        public Rectangle Rec;//记录初始化XY加宽高
        public Rectangle FillRec;//记录初始化填充时候的XY加宽高
        public Rectangle NZRec;//纪律选中的宽高
        public Pen pen;

        private bool colFla = false;//是否是列头
        private bool rowFla = false;//是否是行头

        private bool centerTable = false;//是否全选

        private bool allCheckedFla = false;//是否全选

        /// <summary>
        /// 画矩形
        /// </summary>
        public void DrawRec()
        {
            g.DrawRectangle(pen, Rec);
        }
        /// <summary>
        /// 填充矩形
        /// </summary>
        public void FillDrawRec()
        {
            g.FillRectangle(new SolidBrush(Color.White), Rec);
        }
    }
}
