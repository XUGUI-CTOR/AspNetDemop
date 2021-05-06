using Caliburn.Micro;
using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using MessageBox = HandyControl.Controls.MessageBox;

namespace Gui.UploadExe.ViewModels
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class CalculatorViewModel : Screen
    {
        public double Left { get; set; }
        public double Right { get; set; }
        public double Result { get; set; }
        public CalculatorViewModel()
        {

        }
        public bool CanDivide(double left, double right)
        {
            return right != 0;
        }

        public void Divide(double left, double right)
        {
            Thread.Sleep(600);
            if (CanDivide(left, right) == true)
                Result = left / right;
            else MessageBox.Show("Divider cannot be zero.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public void Plus(double left, double right)
        {
            Result = left + right;
        }

        public void Minus(double left, double right)
        {
            Result = left - right;
        }

        public void Multipy(double left, double right)
        {
            Result = left * right;
        }
    }
}
