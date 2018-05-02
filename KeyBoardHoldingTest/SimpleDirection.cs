using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KeyBoardHoldingTest
{

    /// <summary>
    /// 基本方位的列舉 0, 8, 4, 2, 1 對應 無, 上, 下, 左, 右 及 二進位 0000, 1000, 0100, 0010, 0001
    /// </summary>
    public enum SimpleDirection { None = 0, Top = 8, Bottom = 4, Left = 2, Right = 1 }
    /// <summary>
    /// 相對方位 基本方位的疊加 有兩組
    /// </summary>
    /// touched   untouched
    /// t b l r   t b l r
    /// 0 0 0 0   0 0 0 0
    /// 1000 0000         0x80
    ///  0100 0000        0x40
    ///   0010 0000       0x20
    ///    0001 0000      0x10
    ///      0000 1000    0x08
    ///       0000 0100   0x04
    ///        0000 0010  0x02
    ///         0000 0001 0x01
    struct SimpleRelativeDirection
    {
        private int Direct;
        /// <summary>
        /// 宣告新的相對方位 由於 C# 對於 byte 型別的計算十分不友善 所以這裡用 int 但是超出範圍的不受理
        /// </summary>
        /// <param name="direct"> 方向值 基本上範圍為 0x00~0xff 或是 0b0000_0000~0b1111_1111 或是 0~255 </param>
        public SimpleRelativeDirection(int direct)
        {
            Direct = direct;
        }
        static public SimpleRelativeDirection None => new SimpleRelativeDirection(0x00);
        static public SimpleRelativeDirection Top => new SimpleRelativeDirection(0x80);
        static public SimpleRelativeDirection Bottom => new SimpleRelativeDirection(0x40);
        static public SimpleRelativeDirection Left => new SimpleRelativeDirection(0x20);
        static public SimpleRelativeDirection Right => new SimpleRelativeDirection(0x10);
        static public SimpleRelativeDirection TopLeft => new SimpleRelativeDirection(0xa0);
        static public SimpleRelativeDirection TopRight => new SimpleRelativeDirection(0x90);
        static public SimpleRelativeDirection BottomLeft => new SimpleRelativeDirection(0x60);
        static public SimpleRelativeDirection BottomRight => new SimpleRelativeDirection(0x50);
        static public bool operator ==(SimpleRelativeDirection A, SimpleRelativeDirection B) => A.Direct == B.Direct;
        static public bool operator !=(SimpleRelativeDirection A, SimpleRelativeDirection B) => A.Direct != B.Direct;
        public bool Is1stDirection(SimpleDirection bd)
        {
            return (Direct & (int)bd * 0x10) == (int)bd * 0x10;
        }
        public bool Is2ndDirection(SimpleDirection bd)
        {
            return (Direct & (int)bd) == (int)bd;
        }
        public SimpleRelativeDirection Add1stDirection(SimpleDirection bd)
        {
            Direct = Direct + (int)bd * 0x10;
            return this;
        }
        public SimpleRelativeDirection Add2ndDirection(SimpleDirection bd)
        {
            Direct = Direct + (int)bd;
            return this;
        }
        public SimpleRelativeDirection Minus1stDirection(SimpleDirection bd)
        {
            Direct = Direct - (int)bd * 0x10;
            return this;
        }
        public SimpleRelativeDirection Minus2ndDirection(SimpleDirection bd)
        {
            Direct = Direct - (int)bd;
            return this;
        }
        public SimpleRelativeDirection Invert()
        {
            int a = 0, b = 0, c = 0, d = 0;
            if ((Direct & 0x80) != (Direct & 0x40))
                a = (~(Direct & 0xc0)) & 0xc0;
            if ((Direct & 0x20) != (Direct & 0x10))
                b = (~(Direct & 0x30)) & 0x30;
            if ((Direct & 0x08) != (Direct & 0x04))
                c = (~(Direct & 0x0c)) & 0x0c;
            if ((Direct & 0x02) != (Direct & 01))
                d = (~(Direct & 0x03)) & 0x03;
            return new SimpleRelativeDirection(a + b + c + d);
        }
        public override string ToString()
        {
            string f = "", a = "";
            if ((Direct & 0x80) == 0x80)
                f += "Top";
            if ((Direct & 0x40) == 0x40)
                f += "Bottom";
            if ((Direct & 0x20) == 0x20)
                f += "Left";
            if ((Direct & 0x10) == 0x10)
                f += "Right";
            if ((Direct & 0x08) == 0x08)
                a += "Top";
            if ((Direct & 0x04) == 0x04)
                a += "Bottom";
            if ((Direct & 0x02) == 0x02)
                a += "Left";
            if ((Direct & 0x01) == 0x01)
                a += "Right";
            return f + "-" + a;
        }
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    static class CollisionTestMethods
    {
        public static SimpleRelativeDirection InternalCollisionTest(this Rect outSide, Rect inSide)
        {
            SimpleRelativeDirection ret = new SimpleRelativeDirection(0x00);
                if (outSide.IntersectsWith(inSide))
                {
                    var TopOffset = Math.Abs(outSide.Top - inSide.Top);
                    var LeftOffset = Math.Abs(outSide.Left - inSide.Left);
                    var BottomOffset = Math.Abs(outSide.Bottom - inSide.Bottom);
                    var RightOffset = Math.Abs(outSide.Right - inSide.Right);
                    if (TopOffset <= 0)
                        ret.Add1stDirection(SimpleDirection.Top);
                    if (LeftOffset <= 0)
                        ret.Add1stDirection(SimpleDirection.Left);
                    if (BottomOffset <= 0)
                        ret.Add1stDirection(SimpleDirection.Bottom);
                    if (RightOffset <= 0)
                        ret.Add1stDirection(SimpleDirection.Right);
                }

                return ret;
        }
        static private Rect Dilate(this Rect rect, double pixel)
        {
            rect.X -= pixel;
            rect.Y -= pixel;
            rect.Width += pixel * 2;
            rect.Height += pixel * 2;
            return rect;
        }
    }
}
