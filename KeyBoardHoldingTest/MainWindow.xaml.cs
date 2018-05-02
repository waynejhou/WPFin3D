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
        }

        public string FpsCounter
        {
            get
            {
                if (wpffupd != null)
                    return
                        $"Fps: {wpffupd.FpsCounter.ToString("00.000")}\n" +
                        $"F: {Ojisan.F.ToString(true)}\n" +
                        $"A: {Ojisan.A.ToString(true)}\n" +
                        $"V: {Ojisan.V.ToString(true)} {(Ojisan.V.Length* wpffupd.FpsCounter).ToString("000")} pixel/sec \n" +
                        $"P: {Ojisan.Point.ToString(true)}";
                else
                    return "";
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
            Ojisan.F = new Vector(0, 0);
            if (e.ContainsKey(Key.Left))
                Ojisan.Ax = -0.05;
            if (e.ContainsKey(Key.Right))
                Ojisan.Ax = 0.05;
            if (e.ContainsKey(Key.Up))
                Ojisan.Ay = Gravity.NegativeY().Mul(1.25).Y;
            if (e.ContainsKey(Key.Down))
                Ojisan.Ay = 0.05;

        }
        private void Wpffupd_FpsUpdateEvent(object sender, FrameUpdateEventArgs e)
        {
            //Ojisan.A = Ojisan.F.Div(Ojisan.M);
            Ojisan.V += Ojisan.A;
            if (Ojisan.Right > board.ActualWidth)
            {
                Ojisan.Right = board.ActualWidth;
                Ojisan.V = new Vector();
            }
            if (Ojisan.Left < 0)
            {
                Ojisan.Left = 0;
                Ojisan.V = new Vector();
            }
            if (Ojisan.Top < 0)
            {
                Ojisan.Top = 0;
                Ojisan.V = new Vector();
            }
            if (Ojisan.Bottom > board.ActualHeight)
            {
                Ojisan.Bottom = board.ActualHeight;
                Ojisan.V = new Vector();
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

}
