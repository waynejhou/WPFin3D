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
        UIElement Main;

        public MainWindow()
        {
            InitializeComponent();
            wpffupd = new WpfFrameUpdate(this,FpsMode.fps60);
            wpffupd.FpsControlEvent += Wpffupd_FpsControlEvent;
            wpffupd.FpsDrawEvent += Wpffupd_FpsDrawEvent;
            wpffupd.FpsUpdateEvent += Wpffupd_FpsUpdateEvent;
            Main = Dot;
        }

        public string FpsCounter
        {
            get
            {
                if (wpffupd != null)
                    return wpffupd.FpsCounter.ToString("00.000");
                else
                    return $"{-1}";
            }
        }
        double vX = 0, vY = 0;
        double aX = 0, aY = 0;
        const double g = 0.98;
        const double airFriction = 0.002;
        const double groundFriction = 1;
        const double moveAcceleration = 2;
        const double jumpAcceleration = 5;
        double jumpCounter = 1;
        const double maxMoveSpeed = 5;


        private void MainWin_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            wpffupd.Dispose();
        }

        private void Wpffupd_FpsControlEvent(object sender, FrameUpdateEventArgs e)
        {
            if(jumpCounter>0)
                if (e.ContainsKey(Key.Space))
                {
                    aX = -jumpAcceleration;
                    jumpCounter -= 0.1;
                }

            if (e.ContainsKey(Key.Left))
                aY = -moveAcceleration;
            if (e.ContainsKey(Key.Right))
                aY = moveAcceleration;
            vX = Math.Max(Math.Min(vX + aX + g, maxMoveSpeed), -maxMoveSpeed);
            vY = Math.Max(Math.Min(vY + aY, maxMoveSpeed), -maxMoveSpeed);
        }

        private void Wpffupd_FpsDrawEvent(object sender, FrameUpdateEventArgs e)
        {
            NotifyPropertyChanged(nameof(FpsCounter));
            Canvas.SetTop(Main, Canvas.GetTop(Main) + vX);
            Canvas.SetLeft(Main, Canvas.GetLeft(Main) + vY);
        }

        private void Wpffupd_FpsUpdateEvent(object sender, FrameUpdateEventArgs e)
        {

            if (Canvas.GetTop(Main) >= board.ActualHeight - 1)
            {
                jumpCounter = 1;
                if (vY < 0)
                    vY = Math.Min(vY + groundFriction, 0);
                if (vY > 0)
                    vY = Math.Max(vY - groundFriction, 0);
            }
            if (vX < 0)
                vX = Math.Min(vX + airFriction, 0);
            if (vX > 0)
                vX = Math.Max(vX - airFriction, 0);
            if (vY < 0)
                vY = Math.Min(vY + airFriction, 0);
            if (vY > 0)
                vY = Math.Max(vY - airFriction, 0);

            if (Canvas.GetTop(Main) <= 0)
            {
                Canvas.SetTop(Main, 0);
                vX = -vX;
            }
            if (Canvas.GetTop(Main) >= board.ActualHeight)
            {
                Canvas.SetTop(Main, board.ActualHeight);
                vX = -vX;
            }
            if (Canvas.GetLeft(Main) <= 0)
            {
                Canvas.SetLeft(Main, 0);
                vY = -vY;
            }
            if (Canvas.GetLeft(Main) >= board.ActualWidth)
            {
                Canvas.SetLeft(Main, board.ActualWidth);
                vY = -vY;
            }
            aX = 0;
            aY = 0;
        }

    }

}
