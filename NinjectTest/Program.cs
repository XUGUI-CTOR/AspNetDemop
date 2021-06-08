using Ninject;
using Ninject.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NinjectTest
{
    class Program
    {
        static void Main(string[] args)
        {
            IKernel ninjectKernel = new StandardKernel();
            ninjectKernel.Bind<IValueCalculator>().To<LinqValueCalculator>();
            ninjectKernel.Bind<IDiscountHelper>().To<DefaultDiscountHelper>().WithConstructorArgument(0m);
            ninjectKernel.Bind<ShoppingCart>().To<LimitShoppingCart>().WithPropertyValue(x => x.PriceLimit, 3m);
            ninjectKernel.Bind<IValueCalculator>().To<IterativeValueCalculator>().WhenInjectedInto<LimitShoppingCart>();
            var shoppingCart = ninjectKernel.Get<ShoppingCart>();
            Console.WriteLine(shoppingCart.CalculateStockValue());
            Console.ReadKey();
        }
    }

    class ShoppingCart
    {
        protected readonly List<decimal> list = new List<decimal>() { 1, 2, 3, 4, 5, 6 };
        protected readonly IValueCalculator calculator;

        public ShoppingCart(IValueCalculator calculator)
        {
            this.calculator = calculator;
        }

        public virtual decimal CalculateStockValue()
        => calculator.Sum(list);
    }
    class LimitShoppingCart : ShoppingCart
    {
        public decimal PriceLimit { get; set; }
        public LimitShoppingCart(IValueCalculator calculator) : base(calculator)
        {
        }
        public override decimal CalculateStockValue()
        {
            var filterValues = list.Where(x => x < PriceLimit);
            return calculator.Sum(filterValues);
        }
    }
    public static class IBindingWhenInNamedWithOrOnSyntaxExtention
    {
        public static IBindingWithOrOnSyntax<T> WithPropertyValue<T,TP>(this IBindingWhenInNamedWithOrOnSyntax<T> val, Expression<Func<T, TP>> expression, TP value)
        {
            var body = expression.Body.ToString();
            return val.WithPropertyValue(body.Substring(body.IndexOf('.')+1),value);
        }
    }

    public interface IValueCalculator
    {
        decimal Sum(IEnumerable<decimal> list);
    }
    public class LinqValueCalculator : IValueCalculator
    {
        private readonly IDiscountHelper discountHelper;

        public LinqValueCalculator(IDiscountHelper discountHelper)
        {
            this.discountHelper = discountHelper;
        }
        public decimal Sum(IEnumerable<decimal> list)
        {
            return discountHelper.ApplyDiscount(list.Sum());
        }
    }

    public class IterativeValueCalculator : IValueCalculator
    {
        public decimal Sum(IEnumerable<decimal> list)
        {
            var total = 0m;
            foreach (var d in list)
            {
                total += d;
            }
            return total;
        }
    }
    //折扣计算接口
    public interface IDiscountHelper
    {
        decimal ApplyDiscount(decimal totalParam);
    }

    //默认折扣计算器
    public class DefaultDiscountHelper : IDiscountHelper
    {
        private readonly decimal discountRate;

        public DefaultDiscountHelper(decimal discountRate)
        {
            this.discountRate = discountRate;
        }
        //public decimal DiscountSize { get; set; }
        public decimal ApplyDiscount(decimal totalParam)
        {
            return (totalParam - (discountRate / 10m * totalParam));
        }
    }
}
