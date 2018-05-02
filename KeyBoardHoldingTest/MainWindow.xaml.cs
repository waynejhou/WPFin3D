using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
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
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        WpfFrameUpdate wpffupd;
        Queue<Rect> trackLine = new Queue<Rect>();

        public MainWindow()
        {
            InitializeComponent();
            wpffupd = new WpfFrameUpdate(this,FpsMode.fps60);
            wpffupd.FpsControlEvent += Wpffupd_FpsControlEvent;
            wpffupd.FpsDrawEvent += Wpffupd_FpsDrawEvent;
            wpffupd.FpsUpdateEvent += Wpffupd_FpsUpdateEvent;
            GridMap.BeginInit();
            var gridSize = 200;
            int c = ((int)board.Width / gridSize), r = ((int)board.Height / gridSize);
            for (int i = 0; i < c ; i++)
                GridMap.ColumnDefinitions.Add(new ColumnDefinition());
            for (int i = 0; i < r; i++)
                GridMap.RowDefinitions.Add(new RowDefinition());
            for (int i = 0; i < c ; i++)
                for(int j = 0; j < r; j++)
                {
                    Border newone;
                    GridMap.Children.Add(newone=new Border() { Height = gridSize, Width = gridSize, BorderThickness = new Thickness(1), BorderBrush = Brushes.Black });
                    Grid.SetRow(newone, i);
                    Grid.SetColumn(newone, j);
                    newone.Child = new Label() { Content = $"[{i}, {j}]" };
                }

            GridMap.EndInit();
            Console.WriteLine("init");
        }

        public string FpsCounter
        {
            get
            {
                string x="";
                if (wpffupd != null)
                    x=
                        $"Fps: {wpffupd.FpsCounter.ToString("00.000")}\n" +
                        $"F: {Ojisan.F.ToString(true)}\n" +
                        $"A: {Ojisan.A.ToString(true)}\n" +
                        $"V: {Ojisan.V.ToString(true)} {(Ojisan.V.Length* wpffupd.FpsCounter).ToString("000")} pixel/sec \n" +
                        $"P: {Ojisan.Point.ToString(true)}\n" +
                        $"Rigbody:{Ojisan.Rigidbody.Location.ToString(true)}  {Ojisan.Rigidbody.Size}";
                else
                    x=
                    "";
                Console.WriteLine(x);
                return x;
            }
        }


        private void MainWin_Closing(object sender, CancelEventArgs e)
        {
            wpffupd.Dispose();
        }

        double GroundFriction = 0.1 ;
        double AirFriction = 0.02;
        Vector Gravity = new Vector(0, 0.98);
        private void Wpffupd_FpsControlEvent(object sender, FrameUpdateEventArgs e)
        {
            if (e.ContainsKey(Key.Left))
                Ojisan.Vx = -2;
            if (e.ContainsKey(Key.Right))
                Ojisan.Vx = 2;
            if (e.ContainsKey(Key.Up))
                Ojisan.Vy = -2;
            if (e.ContainsKey(Key.Down))
                Ojisan.Vy = 2;

        }
        private void Wpffupd_FpsUpdateEvent(object sender, FrameUpdateEventArgs e)
        {
            Ojisan.V = Ojisan.V.Sub(AirFriction);
            //Console.WriteLine(BorderLine);
            if (Ojisan.Right > BorderLine.GetRight())
            {
                Ojisan.V = Ojisan.V.SetX(0);
                Ojisan.A = Ojisan.A.SetX(0);
                Ojisan.Right = BorderLine.GetRight();
            }
            if (Ojisan.Left < BorderLine.GetLeft())
            {
                Ojisan.V = Ojisan.V.SetX(0);
                Ojisan.A = Ojisan.A.SetX(0);
                Ojisan.Left = BorderLine.GetLeft();
            }
            if (Ojisan.Top < BorderLine.GetTop())
            {
                Ojisan.V = Ojisan.V.SetY(0);
                Ojisan.A = Ojisan.A.SetY(0);
                Ojisan.Top = BorderLine.GetTop();
            }
            if (Ojisan.Bottom > BorderLine.GetBottom())
            {
                Ojisan.V = Ojisan.V.SetY(0);
                Ojisan.A = Ojisan.A.SetY(0);
                Ojisan.Bottom = BorderLine.GetBottom();
            }
        }

        private void Wpffupd_FpsDrawEvent(object sender, FrameUpdateEventArgs e)
        {
            NotifyPropertyChanged(nameof(FpsCounter));
            Ojisan.Point += Ojisan.V;
            board.SetX(-(Ojisan.GetX() - camera.ActualWidth / 2));
            board.SetY(-(Ojisan.GetY() - camera.ActualHeight / 2));
        }

        

    }


    class InternalCollision
    {
        List<IRigidbody> _outsideBox = new List<IRigidbody>();
        List<IRigidbody> _insideBody = new List<IRigidbody>();

        internal List<IRigidbody> OutsideBox { get => _outsideBox; set => _outsideBox = value; }
        internal List<IRigidbody> InsideBody { get => _insideBody; set => _insideBody = value; }

        public void TestCollision()
        {
            foreach(var ob in OutsideBox)
            {
                foreach(var ib in InsideBody)
                {
                    Console.WriteLine();
                }
            }
        }
    }

}
