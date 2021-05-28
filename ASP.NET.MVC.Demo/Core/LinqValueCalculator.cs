using ASP.NET.MVC.Demo.Core.Interface;
using ASP.NET.MVC.Demo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP.NET.MVC.Demo.Core
{
    public class LinqValueCalculator : IValueCalculator
    {
        public decimal ValueProducts(params Product[] products)
        {
            return products.Sum(x => x.Price);
        }
    }
}