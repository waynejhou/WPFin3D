using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFin3D
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Viewport3D_KeyDown(object sender, KeyEventArgs e)
        {
            var c = vp3d.Camera as PerspectiveCamera;
            var p = c.Position;
            var ld = c.LookDirection;
            KeyBoardMovedPosition(e.Key, c, 0.5, true);
            KeyBoardMovedLookDir(e.Key, c, 0.1, true);
        }
        private void KeyBoardMovedPosition(Key key, PerspectiveCamera camera, double scale, bool isPrintPosi = false)
        {
            if (key == Key.W)
            {
                camera.Position += Vector3D.Multiply(camera.LookDirection,scale);
            }
            if (key == Key.S)
            {
                camera.Position -= Vector3D.Multiply(camera.LookDirection, scale);
            }
            if (key == Key.A)
            {
                camera.Position -= Vector3D.Multiply(camera.LeftDirection(),scale);
            }
            if (key == Key.D)
            {
                camera.Position += Vector3D.Multiply(camera.LeftDirection(), scale);
            }
            if (key == Key.Space)
            {
                camera.Position += Vector3D.Multiply(camera.UpDirection,scale);
            }
            if (key == Key.LeftShift)
            {
                camera.Position -= Vector3D.Multiply(camera.UpDirection, scale);
            }
            if (isPrintPosi)
                Console.WriteLine($"{camera.Position.X}, {camera.Position.Y}, {camera.Position.Z}");
        }
        double xzPlain = -90*Math.PI/180;
        double yr = 0 * Math.PI / 18;
        private void KeyBoardMovedLookDir(Key key, PerspectiveCamera camera, double scale, bool isPrintPosi = false)
        {
            double x = camera.LookDirection.X, y = camera.LookDirection.Y, z = camera.LookDirection.Z;
            Console.WriteLine(key);
            if (key == Key.Left)
            {
                xzPlain -= scale;
            }
            if (key == Key.Right)
            {
                xzPlain += scale;
            }
            if (key == Key.Up)
            {
                yr += scale;
            }
            if (key == Key.Down)
            {
                yr -= scale;
            }
            x = Math.Cos(xzPlain);
            z= Math.Sin(xzPlain);
            y = Math.Tan(yr) * Math.Sqrt(Math.Pow(x, 2) + Math.Pow(z, 2));
            if (isPrintPosi)
            {
                Console.WriteLine($" {xzPlain*180/Math.PI} \n{x} {y} {z}\n{yr * 180 / Math.PI}");
            }
            camera.LookDirection = new Vector3D(x, y, z);
        }
        private void Window_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            var c = vp3d.Camera as PerspectiveCamera;
            var p = c.Position;
            double x = p.X, y = p.Y, z = p.Z;
            Console.WriteLine(e.Delta);
            if (e.Delta > 0)
            {
                x -= x / 3d;
                y -= y / 3d;
                z -= z / 3d;
            }
            if (e.Delta < 0)
            {
                x += x / 3d;
                y += y / 3d;
                z += z / 3d;
            }
            Console.WriteLine($"{x}, {y}, {z}");
            c.Position = new Point3D(x, y, z);
        }


    }
    static class ExtendMethod
    {
        public static Vector3D LeftDirection(this PerspectiveCamera camera)
        {
            return Vector3D.CrossProduct(camera.LookDirection, camera.UpDirection);
        }
    }
}
