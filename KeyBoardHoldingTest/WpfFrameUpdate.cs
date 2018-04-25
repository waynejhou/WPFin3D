using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
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
    public class WpfFrameUpdate : IDisposable
    {
        bool _isUpdateStop = false;
        bool _isUpdatePause = false;
        DateTime lastFrameDT = DateTime.Now;
        TimeSpan frameTS = TimeSpan.Zero;
        readonly TimeSpan OneSecond = TimeSpan.FromSeconds(1);

        public event FpsControlHandle FpsControlEvent;
        public event FpsDrawHandle FpsDrawEvent;
        public event FpsUpdateHandle FpsUpdateEvent;
        public Window ControlledWindow { get; set; }
        public Thread UpdateThread { get; set; }
        public FpsMode FpsMode { get; private set; }
        public double FpsCounter => OneSecond.TotalMilliseconds / ((frameTS.TotalMilliseconds) == 0 ? 0.00001 : frameTS.TotalMilliseconds);
        public bool IsUpdateStop { get => _isUpdateStop;
            set {
                if (value == false)
                    _isUpdateStop = value;
                else
                {
                    if (value == true && _isUpdateStop == false)
                        throw new InvalidOperationException("No starting again after stop thread, use IsPause instead;");
                }

            } }
        public bool IsUpdatePause { get => _isUpdatePause; set => _isUpdatePause = value; }
        public FrameUpdateEventArgs UpdateEventArgs { get; set; }

        public WpfFrameUpdate(Window window, FpsMode mode = FpsMode.fps30)
        {
            if (window == null)
                return;
            ControlledWindow = window;
            FpsMode = mode;
            UpdateEventArgs = new FrameUpdateEventArgs();
            ControlledWindow.KeyUp += ControlledWindow_KeyUp;
            ControlledWindow.KeyDown += ControlledWindow_KeyDown;
            ControlledWindow.MouseUp += ControlledWindow_MouseUp;
            ControlledWindow.MouseDown += ControlledWindow_MouseDown;
            ControlledWindow.MouseWheel += ControlledWindow_MouseWheel;
            UpdateThread = new Thread(() =>
            {
                while (!IsUpdateStop)
                {
                    while (IsUpdatePause) { Thread.Sleep(1000 / (int)FpsMode); };
                    Thread.Sleep(1000 / (int)FpsMode);
                    DateTime now;
                    frameTS = (now=DateTime.Now) - lastFrameDT;
                    lastFrameDT = now;
                    ControlledWindow?.Dispatcher.Invoke(() =>
                    {
                        FpsControlEvent?.Invoke(this, UpdateEventArgs);
                        FpsDrawEvent?.Invoke(this, UpdateEventArgs);
                        FpsUpdateEvent?.Invoke(this, UpdateEventArgs);
                    });
                    UpdateEventArgs.MouseDelta.Clear();
                }
            });
            UpdateThread.Start();
        }

        private void ControlledWindow_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            MouseDelta md = (e.Delta > 0) ? MouseDelta.Up : (e.Delta < 0) ? MouseDelta.Down : MouseDelta.None;
            if (!UpdateEventArgs.MouseDelta.Contains(md))
                UpdateEventArgs.MouseDelta.Add(md);
        }

        private void ControlledWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!UpdateEventArgs.MousePressing.Contains(e.ChangedButton))
                UpdateEventArgs.MousePressing.Add(e.ChangedButton);
        }

        private void ControlledWindow_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (UpdateEventArgs.MousePressing.Contains(e.ChangedButton))
                UpdateEventArgs.MousePressing.Remove(e.ChangedButton);
        }

        private void ControlledWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (!UpdateEventArgs.KeyPressing.Contains(e.Key))
                UpdateEventArgs.KeyPressing.Add(e.Key);
        }

        private void ControlledWindow_KeyUp(object sender, KeyEventArgs e)
        {
            if (UpdateEventArgs.KeyPressing.Contains(e.Key))
                UpdateEventArgs.KeyPressing.Remove(e.Key);
        }

        public void Dispose()
        {
            UpdateThread?.Abort();
            ControlledWindow.KeyUp -= ControlledWindow_KeyUp;
            ControlledWindow.KeyDown -= ControlledWindow_KeyDown;
            ControlledWindow.MouseUp -= ControlledWindow_MouseUp;
            ControlledWindow.MouseDown -= ControlledWindow_MouseDown;
            ControlledWindow.MouseWheel -= ControlledWindow_MouseWheel;
            ControlledWindow = null;
        }
    }
    public delegate void FpsControlHandle(object sender, FrameUpdateEventArgs e);
    public delegate void FpsDrawHandle(object sender, FrameUpdateEventArgs e);
    public delegate void FpsUpdateHandle(object sender, FrameUpdateEventArgs e);
    public class FrameUpdateEventArgs
    {
        Vector _mouseAxis = new Vector(0, 0);
        Point _mousePoint = new Point(0, 0);
        List<Key> _keyPressing = new List<Key>();
        List<MouseButton> _mousePressing = new List<MouseButton>();
        List<MouseDelta> _mouseDelta = new List<MouseDelta>();

        public Vector MouseAxis { get => _mouseAxis; set => _mouseAxis = value; }
        public Point MousePoint { get => _mousePoint; set => _mousePoint = value; }
        public List<Key> KeyPressing { get => _keyPressing; set => _keyPressing = value; }
        public List<MouseButton> MousePressing { get => _mousePressing; set => _mousePressing = value; }
        public List<MouseDelta> MouseDelta { get => _mouseDelta; set => _mouseDelta = value; }

        public bool ContainsKey(Key key) => _keyPressing.Contains(key);
        public bool ContainsMouseBtn(MouseButton mouseButton) => _mousePressing.Contains(mouseButton);
        public bool ContainsMouseUp(MouseDelta mouseDelta) => _mouseDelta.Contains(mouseDelta);
        public bool IsControlling => KeyPressing.Count == 0 && MousePressing.Count == 0 && MouseDelta.Count == 0 && MouseAxis.Length == 0;
    }
    public enum MouseDelta { Up = 120, Down = -120, None = 0 }
    public enum FpsMode { fps30 = 30, fps60 = 60 }

}
