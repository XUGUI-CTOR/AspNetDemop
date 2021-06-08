using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

namespace WPFAsync
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            Trace.WriteLine(Thread.CurrentThread.ManagedThreadId + " Gui0");
            await async1();
            Trace.WriteLine(Thread.CurrentThread.ManagedThreadId + " Gui0.5");
        }

        private async Task async1()
        {
            Trace.WriteLine(Thread.CurrentThread.ManagedThreadId + " Gui1");
            await async2();
            Trace.WriteLine(Thread.CurrentThread.ManagedThreadId + " Gui3");
        }
        private async Task async2()
        {
            Trace.WriteLine(Thread.CurrentThread.ManagedThreadId + " Gui2");
            await Task.Delay(1000).ConfigureAwait(false);
            Trace.WriteLine(Thread.CurrentThread.ManagedThreadId + " Gui2.5");
        }
    }
}
