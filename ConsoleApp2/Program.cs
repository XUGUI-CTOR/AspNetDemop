using System;
using System.Text.RegularExpressions;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            string str = @"拜访客户:讯捷
拜访类型：客户回访 
拜访内容:面推机油活动易损件 成功一家 爱信变速箱油合作修理厂 现场下单一万二




拜访客户:大连
拜访类型：客户回访 ";
            string pattern = "^拜访客户(:|：).+\n拜访类型(:|：).+\n拜访内容(:|：).+\n{0,1}$";
            var matchs = Regex.Matches(str, pattern, RegexOptions.Multiline);
            Console.WriteLine(matchs.Count);
            foreach (Match item in matchs)
            {
                Console.WriteLine(item.Value);
                Console.WriteLine("--------------");
            }
        }
    }
}
