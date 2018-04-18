using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }

        ObservableCollection<Key> _keyPressing = new ObservableCollection<Key>();
        ObservableCollection<Key> KeyPressing => _keyPressing;

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            Console.WriteLine($"KeyDown {e.Key}");
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            Console.WriteLine($"KeyUp {e.Key}");
        }
    }
}
