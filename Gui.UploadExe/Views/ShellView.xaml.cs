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
using System.Windows.Shapes;

namespace Gui.UploadExe.Views
{
    /// <summary>
    /// ShellView.xaml 的交互逻辑
    /// </summary>
    public partial class ShellView : Window
    {
        public ShellView()
        {
            InitializeComponent();
            this.Loaded += ShellView_Loaded;
        }

        private void ShellView_Loaded(object sender, RoutedEventArgs e)
        {
            Binding binding = new Binding();
            binding.Converter = new HandyControl.Tools.Converter.Boolean2BooleanReConverter();
        }
    }
}
