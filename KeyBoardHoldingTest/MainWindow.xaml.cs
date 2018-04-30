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

        public MainWindow()
        {
            InitializeComponent();
            wpffupd = new WpfFrameUpdate(this,FpsMode.fps60);
            wpffupd.FpsControlEvent += Wpffupd_FpsControlEvent;
            wpffupd.FpsDrawEvent += Wpffupd_FpsDrawEvent;
            wpffupd.FpsUpdateEvent += Wpffupd_FpsUpdateEvent;
        }

        public string FpsCounter
        {
            get
            {
                if (wpffupd != null)
                    return
                        $"{wpffupd.FpsCounter.ToString("00.000")}\n" +
                        $"F: {Ojisan.F}\n" +
                        $"A: {Ojisan.A}\n" +
                        $"V: {Ojisan.V}\n" +
                        $"P: {Ojisan.Point}";
                else
                    return $"{-1}";
            }
        }


        private void MainWin_Closing(object sender, CancelEventArgs e)
        {
            wpffupd.Dispose();
        }

        double friction = 1;
        private void Wpffupd_FpsControlEvent(object sender, FrameUpdateEventArgs e)
        {
            if (e.ContainsKey(Key.Left))
                Ojisan.Fx = -5;
            if (e.ContainsKey(Key.Right))
                Ojisan.Fx = 5;
            if (e.ContainsKey(Key.Up))
                Ojisan.Fy = -5;
            if (e.ContainsKey(Key.Down))
                Ojisan.Fy = 5;
            Ojisan.A = Ojisan.F.Div(Ojisan.M).Sub(friction * Ojisan.M);
            Ojisan.V += Ojisan.A;
            Ojisan.Point += Ojisan.V;
        }

        private void Wpffupd_FpsDrawEvent(object sender, FrameUpdateEventArgs e)
        {
            NotifyPropertyChanged(nameof(FpsCounter));
            
        }

        private void Wpffupd_FpsUpdateEvent(object sender, FrameUpdateEventArgs e)
        {
            Ojisan.F = new Vector(0,0);
        }

    }

}
