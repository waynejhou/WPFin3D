using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace KeyBoardHoldingTest
{
    interface IRigidbody
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
        double Top { get; set; }
        double Left { get; set; }
        double Bottom { get; set; }
        double Right { get; set; }
    }

    static class UIElementInCanvasExtensionMethods
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
            => Canvas.GetTop(element as UIElement);
        public static void SetY(this UIElement element, double y)
            => Canvas.SetTop(element as UIElement, y);

        public static double GetLeft(this UIElement element)
            => Canvas.GetLeft(element as UIElement);
        public static void SetLeft(this UIElement element, double left)
            => Canvas.GetLeft(element as UIElement);

        public static double GetRight(this UIElement element)
            => (VisualTreeHelper.GetParent(element) as Canvas).ActualWidth - Canvas.GetRight(element as UIElement);
        public static void SetRight(this UIElement element, double right)
            => Canvas.SetRight(element as UIElement, (VisualTreeHelper.GetParent(element) as Canvas).ActualWidth - right);

        public static double GetTop(this UIElement element)
            => Canvas.GetTop(element as UIElement);
        public static void SetTop(this UIElement element, double top)
            => Canvas.SetRight(element as UIElement, top);

        public static double GetBottom(this UIElement element)
            => Canvas.GetBottom(element as UIElement);
        public static void SetBottom(this UIElement element, double bottom)
            => Canvas.SetBottom(element as UIElement, bottom);
    }
    static class PointCalculateExtensionMethods
    {
        public static Point Add(this Point a, Point b)
        {
            return new Point(a.X + b.X, a.Y + b.Y);
        }
    }
    static class VectorCalculateExtensionMethods
    {
        public static Vector Div(this Vector a, double scale)
        {
            return new Vector(a.X / scale, a.Y / scale);
        }
        public static Vector Mul(this Vector a, double scale)
        {
            return new Vector(a.X * scale, a.Y * scale);
        }
        public static Vector Sub(this Vector a, double scale)
        {
            if (a.Length != 0)
            {
                double offset = 0;
                if ((offset = a.Length - scale) <= 0)
                    return new Vector();
                return new Vector(a.X / a.Length * offset, a.Y / a.Length * offset);
            }
            return new Vector();
        }
        public static Vector Negative(this Vector a)
        {
            return new Vector(-a.X, -a.Y);
        }
        public static Vector NegativeX(this Vector a)
        {
            return new Vector(-a.X, a.Y);
        }
        public static Vector NegativeY(this Vector a)
        {
            return new Vector(a.X, -a.Y);
        }
        public static Vector UnitVector(this Vector a)
        {
            if (a.Length == 0)
                return new Vector();
            return new Vector(a.X / a.Length, a.Y / a.Length);
        }
        public static Vector SetX(this Vector a, double x)
        {
            return new Vector(x, a.Y);
        }
        public static Vector SetY(this Vector a, double y)
        {
            return new Vector(a.X, y);
        }
        public static Vector ModX(this Vector a, double x)
        {
            return new Vector(a.X + x, a.Y);
        }
        public static Vector ModY(this Vector a, double y)
        {
            return new Vector(a.X, a.Y + y);
        }
    }
    static class ToStringExtensionMethods
    {
        public static string ToString(this Vector v, bool nothing, string format = "[{0:000.00}, {1:000.00}]")
        {
            return string.Format(format, v.X, v.Y);
        }
        public static string ToString(this Point v, bool nothing, string format = "[{0:000.00}, {1:000.00}]")
        {
            return string.Format(format, v.X, v.Y);
        }
    }
}
