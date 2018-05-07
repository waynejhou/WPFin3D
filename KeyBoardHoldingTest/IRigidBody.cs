using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

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

    static class IRigidbodyInCanvasExtensionMethods
    {
        public static Point GetPoint(this IRigidbody element)
            => new Point(Canvas.GetLeft(element as UIElement), Canvas.GetTop(element as UIElement));
        public static void SetPoint(this IRigidbody element, Point value)
        {
            Canvas.SetLeft(element as UIElement, value.X);
            Canvas.SetTop(element as UIElement, value.Y);
        }

        public static double GetX(this IRigidbody element)
            => Canvas.GetLeft(element as UIElement);
        public static void SetX(this IRigidbody element, double x)
            => Canvas.SetLeft(element as UIElement, x);

        public static double GetY(this IRigidbody element)
            => Canvas.GetTop(element as UIElement);
        public static void SetY(this IRigidbody element, double y)
            => Canvas.SetTop(element as UIElement, y);

        public static double GetLeft(this IRigidbody element)
            => Canvas.GetLeft(element as UIElement);
        public static void SetLeft(this IRigidbody element, double left)
            => Canvas.SetLeft(element as UIElement, left);

        public static double GetRight(this IRigidbody element)
            => Canvas.GetLeft(element as UIElement) + element.Rigidbody.Width;
        public static void SetRight(this IRigidbody element, double right)
            => Canvas.SetLeft(element as UIElement, right - element.Rigidbody.Width);

        public static double GetTop(this IRigidbody element)
            => Canvas.GetTop(element as UIElement);
        public static void SetTop(this IRigidbody element, double top)
            => Canvas.SetTop(element as UIElement, top);

        public static double GetBottom(this IRigidbody element)
            => Canvas.GetTop(element as UIElement) + element.Rigidbody.Width;
        public static void SetBottom(this IRigidbody element, double bottom)
            => Canvas.SetTop(element as UIElement, bottom - element.Rigidbody.Height);
    }

    static class ShapeInCanvasExtensionMethods
    {
        public static Point GetPoint(this Shape element)
            => new Point(Canvas.GetLeft(element as UIElement), Canvas.GetTop(element as UIElement));
        public static void SetPoint(this Shape element, Point value)
        {
            Canvas.SetLeft(element as UIElement, value.X);
            Canvas.SetTop(element as UIElement, value.Y);
        }

        public static double GetX(this Shape element)
            => Canvas.GetLeft(element as UIElement);
        public static void SetX(this Shape element, double x)
            => Canvas.SetLeft(element as UIElement, x);

        public static double GetY(this Shape element)
            => Canvas.GetTop(element as UIElement);
        public static void SetY(this Shape element, double y)
            => Canvas.SetTop(element as UIElement, y);

        public static double GetLeft(this Shape element)
            => Canvas.GetLeft(element as UIElement);
        public static void SetLeft(this Shape element, double left)
            => Canvas.SetLeft(element as UIElement, left);

        public static double GetRight(this Shape element)
            => Canvas.GetLeft(element as UIElement) + element.Width;
        public static void SetRight(this Shape element, double right)
            => Canvas.SetLeft(element as UIElement, right - element.Width);

        public static double GetTop(this Shape element)
            => Canvas.GetTop(element as UIElement);
        public static void SetTop(this Shape element, double top)
            => Canvas.SetTop(element as UIElement, top);

        public static double GetBottom(this Shape element)
            => Canvas.GetTop(element as UIElement) + element.Width;
        public static void SetBottom(this Shape element, double bottom)
            => Canvas.SetTop(element as UIElement, bottom - element.Height);

        public static Rect GetRigidbody(this Shape element)
        {
            return new Rect(element.GetPoint(), new Size(element.ActualWidth, element.ActualHeight));
        }
    }

    static class CanvasInCanvasExtensionMethods
    {
        public static Point GetPoint(this Canvas element)
            => new Point(Canvas.GetLeft(element as UIElement), Canvas.GetTop(element as UIElement));
        public static void SetPoint(this Canvas element, Point value)
        {
            Canvas.SetLeft(element as UIElement, value.X);
            Canvas.SetTop(element as UIElement, value.Y);
        }

        public static double GetX(this Canvas element)
            => Canvas.GetLeft(element as UIElement);
        public static void SetX(this Canvas element, double x)
            => Canvas.SetLeft(element as UIElement, x);

        public static double GetY(this Canvas element)
            => Canvas.GetTop(element as UIElement);
        public static void SetY(this Canvas element, double y)
            => Canvas.SetTop(element as UIElement, y);

        public static double GetLeft(this Canvas element)
            => Canvas.GetLeft(element as UIElement);
        public static void SetLeft(this Canvas element, double left)
            => Canvas.SetLeft(element as UIElement, left);

        public static double GetRight(this Canvas element)
            => Canvas.GetLeft(element as UIElement) + element.Width;
        public static void SetRight(this Canvas element, double right)
            => Canvas.SetLeft(element as UIElement, right - element.Width);

        public static double GetTop(this Canvas element)
            => Canvas.GetTop(element as UIElement);
        public static void SetTop(this Canvas element, double top)
            => Canvas.SetTop(element as UIElement, top);

        public static double GetBottom(this Canvas element)
            => Canvas.GetTop(element as UIElement) + element.Width;
        public static void SetBottom(this Canvas element, double bottom)
            => Canvas.SetTop(element as UIElement, bottom - element.Height);
    }

    static class PointCalculateExtensionMethods
    {
        public static Point Add(this Point a, Point b)
        {
            return new Point(a.X + b.X, a.Y + b.Y);
        }
        public static Point Sub(this Point a, Point b)
        {
            return new Point(a.X - b.X, a.Y - b.Y);
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

    static class RectCalculateExtensionMethods
    {
        public static Rect Erode(this Rect rect, double pixel)
        {
            return new Rect(rect.X + pixel, rect.Y + pixel, rect.Width - pixel * 2, rect.Height * 2);
        }
    }
    static class ToStringExtensionMethods
    {
        public static string ToFString(this Vector v, string format = "[{0:000.00}, {1:000.00}]")
        {
            return string.Format(format, v.X, v.Y);
        }
        public static string ToFString(this Point v, string format = "[{0:000.00}, {1:000.00}]")
        {
            return string.Format(format, v.X, v.Y);
        }
        public static string ToFString(this Shape v,
            string format= "[top: {0:000.00}, left: {1:000.00}, bottom: {2:000.00}, right: {3:000.00}] [w: {4:000.00}, h: {5:000.00}]")
        {
            return string.Format(format, v.GetTop(), v.GetLeft(), v.GetBottom(), v.GetRight(), v.ActualWidth, v.ActualHeight);
        }
        public static string ToFString<T>(this List<T> v)
        {
            string str="";
            v.ForEach(x => str += x+" ");
            return  str;
        }
    }
}
