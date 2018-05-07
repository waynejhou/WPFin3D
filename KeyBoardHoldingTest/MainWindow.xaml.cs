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
            wpffupd = new WpfFrameUpdate(this,board,FpsMode.fps60);
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
            InsideBody.Add(Ojisan);
            OutsideBox.Add(BorderLine);
        }

        public string RealTimeInfo
        {
            get
            {
                string x="";
                if (wpffupd != null)
                    x = $"Mouse: {wpffupd.UpdateEventArgs.MousePoint.ToFString()}\n" +
                        $"Fps: {wpffupd.FpsCounter.ToString("00.000")}\n" +
                        $"F: {Ojisan.F.ToFString()}\n" +
                        $"A: {Ojisan.A.ToFString()}\n" +
                        $"V: {Ojisan.V.ToFString()} {(Ojisan.V.Length * wpffupd.FpsCounter).ToString("000")} pixel/sec \n" +
                        $"P: {Ojisan.Point.ToFString()}\n" +
                        $"Rigbody:{Ojisan.Rigidbody.Location.ToFString()}  {Ojisan.Rigidbody.Size}\n" +
                        $"Border line: {BorderLine.ToFString()}\n" +
                        $"MousePressing: {wpffupd.UpdateEventArgs.MousePressing.ToFString()}\n" +
                        $"KeyPressing: {wpffupd.UpdateEventArgs.KeyPressing.ToFString()}\n";
                else
                    x =
                    "";
                Console.WriteLine(x);
                return x;
            }
        }
        public ObservableCollection<string> InfoLogList { get => _infoLogList; set => _infoLogList = value; }
        ObservableCollection<string> _infoLogList = new ObservableCollection<string>();
        void AddLog(string message, string tag = "Debug")
        {
            InfoLogList.Add($"[{DateTime.Now.ToString("hh:mm.ss.fff")}, {tag}]: {message}");
            InfoLog.Items.MoveCurrentToLast();
            InfoLog.ScrollIntoView(InfoLog.Items.CurrentItem);
            NotifyPropertyChanged(nameof(InfoLogList));
        }

        private void MainWin_Closing(object sender, CancelEventArgs e)
        {
            wpffupd.Dispose();
        }

        double GroundFriction = 0.1 ;
        double AirFriction = 0.0001;
        Vector Gravity = new Vector(0, 0.98);
        Vector v ;
        private void Wpffupd_FpsControlEvent(object sender, FrameUpdateEventArgs e)
        {
            if (e.ContainsKeyPressing(Key.Left))
                Ojisan.Vx = -2;
            if (e.ContainsKeyPressing(Key.Right))
                Ojisan.Vx = 2;
            if (e.ContainsKeyPressing(Key.Up))
                Ojisan.Vy = -2;
            if (e.ContainsKeyPressing(Key.Down))
                Ojisan.Vy = 2;
            if (e.ContainsMouseBtnDown(MouseButton.Left))
                AddLog($"Mouse Left Down at {e.MousePoint.ToFString()}");
            if (e.ContainsMouseBtnPressing(MouseButton.Left))
            {
                Ojisan.V = (e.MousePoint - Ojisan.Point).UnitVector().Mul(2);
            }


        }
        private void Wpffupd_FpsUpdateEvent(object sender, FrameUpdateEventArgs e)
        {
            Ojisan.V = Ojisan.V.Sub(AirFriction);
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
            NotifyPropertyChanged(nameof(RealTimeInfo));
            Ojisan.Point += Ojisan.V;
            board.SetX(-(Ojisan.GetX() - camera.ActualWidth / 2));
            board.SetY(-(Ojisan.GetY() - camera.ActualHeight / 2));
            TestCollision();
        }


    }


    partial class MainWindow
    {
        List<object> _outsideBox = new List<object>();
        List<object> _insideBody = new List<object>();

        private List<object> OutsideBox { get => _outsideBox; set => _outsideBox = value; }
        private List<object> InsideBody { get => _insideBody; set => _insideBody = value; }

        public void TestCollision()
        {
            foreach (var ob in OutsideBox)
            {
                foreach (var ib in InsideBody)
                {
                    Rect ir=new Rect(), or= new Rect();
                    if (ib is IRigidbody)
                        ir = (ib as IRigidbody).Rigidbody;
                    if (ib is Shape)
                        ir = (ib as Shape).GetRigidbody();
                    if (ob is IRigidbody)
                        or = (ob as IRigidbody).Rigidbody;
                    if (ob is Shape)
                        or = (ob as Shape).GetRigidbody();
                    if (ir.Right > or.Right)
                    {
                        AddLog($"Collision at Right");
                    }
                    if (ir.Left < or.Left)
                    {
                        AddLog($"Collision at Left");
                    }
                    if (ir.Top < or.Top)
                    {
                        AddLog($"Collision at Top");
                    }
                    if (ir.Bottom > or.Bottom)
                    {
                        AddLog($"Collision at Bottom");
                    }
                }
            }
        }
    }

}
