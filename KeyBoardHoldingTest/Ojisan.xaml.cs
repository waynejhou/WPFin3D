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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KeyBoardHoldingTest
{
    /// <summary>
    /// Ojisan.xaml 的互動邏輯
    /// </summary>
    public partial class Ojisan : UserControl
    {
        public Ojisan()
        {
            InitializeComponent();
        }
    }

    public partial class Ojisan : IControllableCharacter
    {
        Vector _v = new Vector(0d, 0d);
        Vector _a = new Vector(0d, 0d);
        Vector _f = new Vector(0d, 0d);
        Rect? _rigidbody = null;
        Rect? _hitbox = null;
        double _m = 1;


        public Point Point
        {
            get => this.GetPoint();
            set => this.SetPoint(value);
        }
        public double X { get => this.GetX(); set => this.SetX(value); }
        public double Y { get => this.GetY(); set => this.SetY(value); }
        public Vector V { get => _v; set => _v = value; }
        public double Vx { get => V.X; set => V = new Vector(value, V.Y); }
        public double Vy { get => V.Y; set => V = new Vector(V.X, value); }
        public Vector A { get => _a; set => _a = value; }
        public double Ax { get => A.X; set => A = new Vector(value, A.Y); }
        public double Ay { get => A.Y; set => A = new Vector(A.X, value); }
        public Vector F { get => _f; set => _f = value; }
        public double Fx { get => F.X; set => F = new Vector(value, F.Y); }
        public double Fy { get => F.Y; set => F = new Vector(F.X, value); }
        public double M { get => _m; set => _m = value; }
        public Rect Hitbox {
            get
            {
                if (_hitbox != null)
                    return new Rect(
                        Point, _hitbox.Value.Size
                        );
                _hitbox = new Rect(
                    new Point(0, 0),
                    new Size(ActualWidth, ActualHeight));
                return new Rect(
                        Point, _hitbox.Value.Size
                        );
            }
            set
            {
                _hitbox = value;
            }
        }
        public Rect Rigidbody
        {
            get
            {
                if (_rigidbody != null)
                    return new Rect(
                        Point.Add(_rigidbody.Value.Location), _rigidbody.Value.Size
                        );
                _rigidbody = new Rect(
                    new Point(0, 0),
                    new Size(ActualWidth, ActualHeight));
                return new Rect(
                        Point, _rigidbody.Value.Size
                        );
            }
            set
            {
                _rigidbody = value;
            }
        }

        public double Top { get => Rigidbody.Top; set => this.SetY(value); }

        public double Left { get => Rigidbody.Left; set => this.SetX(value); }

        public double Bottom { get => Rigidbody.Bottom; set => this.SetY(value-Rigidbody.Height); }

        public double Right { get => Rigidbody.Right; set => this.SetX(value-Rigidbody.Width); }


    }
}
