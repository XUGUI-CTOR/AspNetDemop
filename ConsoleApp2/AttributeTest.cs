using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class AttributeTest
    {
        public static void Main(string[] args)
        {
        }
       
        private static bool IsVaild(Order order)
        {
            foreach (var propertyInfo in order.GetType().GetProperties(BindingFlags.Public|BindingFlags.Instance))
            {
                if (IsMemberValid(order.OrderID.Length, propertyInfo))
                    return true;
            }
            return false;
        }

        private static bool IsMemberValid(int inputLength, MemberInfo member)
        {
            foreach (var attribute in member.GetCustomAttributes(true))
            {
                if (attribute is MyStringLengthAttribute stringlength)
                {
                    if (inputLength < stringlength.MinLength || inputLength > stringlength.MaxLength)
                    {
                        Console.WriteLine(stringlength.ErrorMessage, stringlength.DisplayName, stringlength.MinLength, stringlength.MaxLength);
                        return false;
                    }
                    return true;
                }
            }
            return false;
        }
    }
    [AttributeUsage(AttributeTargets.Property)]
    public class MyStringLengthAttribute : Attribute
    {
        //显示的名称，对外是只读的，所以不能通过可选参数来赋值，必须在构造函数中对其初始化。
        public string DisplayName { get; private set; }

        //长度最大值，对外是只读的，所以不能通过可选参数来赋值，必须在构造函数中对其初始化。
        public int MaxLength { get; private set; }

        //错误信息，标注时可作为可选命名参数来使用。
        public string ErrorMessage { get; set; }

        //长度最小值，标注时可作为可选命名参数来使用。
        public int MinLength { get; set; }

        public MyStringLengthAttribute(string displayName, int maxLength)
        {
            DisplayName = displayName;
            MaxLength = maxLength;
        }
    }

    public struct Order
    {
        public Order(string orderid)
        {
            OrderID = orderid;
        }
        [MyStringLength("订单编号",12,ErrorMessage = "{0}长度必须在{1}到{2}之间",MinLength = 10)]
        public string OrderID { get; set; }
    }

    public interface IValueCalculator
    {
        //decimal ValueProducts(params Product)
    }
}
