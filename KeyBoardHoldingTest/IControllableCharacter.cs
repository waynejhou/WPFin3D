using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace KeyBoardHoldingTest
{
    interface IControllableCharacter
    {
        Point Point { get; set; }
        double X { get; set; }
        double Y { get; set; }
        Vector V { get; set; }
        double Vx { get; set; }
        double Vy { get; set; }
        Vector A { get; set; }
        double Ax { get; set; }
        double Ay { get; set; }
        double M { get; set; }
        Vector F { get; set; }
        Rect Hitbox { get; set; }
        Rect Rigidbody { get; set; }
        double Top { get; }
        double Left { get; }
        double Bottom { get; }
        double Right { get; }
    }

    static class CanvasExtensionMethods
    {
        public static Point GetPoint(this UIElement element)
            => new Point(Canvas.GetLeft(element as UIElement), Canvas.GetTop(element as UIElement));
        public static void SetPoint(this UIElement element, Point value)
        {
            Canvas.SetLeft(element as UIElement, value.X);
            Canvas.SetTop(element as UIElement, value.Y);
        }
        public static double GetX(this UIElement element)
            => Canvas.GetLeft(element as UIElement);
        public static void SetX(this UIElement element, double x)
            => Canvas.SetLeft(element as UIElement, x);
        public static double GetY(this UIElement element)
            => Canvas.GetLeft(element as UIElement);
        public static void SetY(this UIElement element, double y)
            => Canvas.SetLeft(element as UIElement, y);
        public static Point Add(this Point a,Point b)
        {
            return new Point(a.X + b.X, a.Y + b.Y);
        }
        public static Vector Div(this Vector a, double scale)
        {
            return new Vector(a.X / scale, a.Y / scale);
        }
        public static Vector Sub(this Vector a, double scale)
        {
            return new Vector(a.X - scale, a.Y - scale);
        }
    }
}
