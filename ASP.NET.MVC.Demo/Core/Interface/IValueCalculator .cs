using ASP.NET.MVC.Demo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASP.NET.MVC.Demo.Core.Interface
{
    public interface IValueCalculator
    {
        decimal ValueProducts(params Product[] products);
    }
}
