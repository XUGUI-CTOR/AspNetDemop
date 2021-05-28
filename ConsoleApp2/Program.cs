using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace ConsoleApp2
{
    class Program
    {
        static void Main2(string[] args)
        {
            int timeout = -1;
            string uri = "http://csfile2.bcunite.com/temp/2021/5/21/2021052112490758361579_tmp287604list1621601342.jpg";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            if (timeout != -1) request.Timeout = timeout;
            var response = request.GetResponse();
            List<byte> datas = new List<byte>(1048576);
            byte[] data = default;
            using (System.IO.Stream stream = response.GetResponseStream())
            {
                int length = 0;
                do
                {
                    data = new byte[10000];
                    length = stream.Read(data, 0, data.Length);
                    if (length == data.Length)
                        datas.AddRange(data);
                    else
                    {
                        var newdata = new byte[length];
                        Array.Copy(data, newdata, length);
                        datas.AddRange(newdata);
                    }
                } while (length != 0);
            }
            Console.WriteLine(datas.Count);
            //var datas = Url_To_Byte(uri);
            string name = $"F:\\迅雷下载\\{Guid.NewGuid().ToString("N")}.jpg";
            using (FileStream fileStream = new FileStream(name, FileMode.Create, FileAccess.Write))
            {
                fileStream.Write(datas.ToArray(), 0, datas.Count);
            }
        }
        private static readonly List<Person> PersonLists = new List<Person>()
    {
                new Person { Name = "张三",Age = 20,Gender = "男",
                    Phones = new List<Phone> {
                        new Phone { Country = "中国", City = "北京", Name = "小米" },
                        new Phone { Country = "中国",City = "北京",Name = "华为"},
                        new Phone { Country = "中国",City = "北京",Name = "联想"},
                        new Phone { Country = "中国",City = "台北",Name = "魅族"},
                        }
                },
                new Person { Name = "松下",Age = 30,Gender = "男",
                    Phones = new List<Phone> {
                        new Phone { Country = "日本",City = "东京",Name = "索尼"},
                        new Phone { Country = "日本",City = "大阪",Name = "夏普"},
                        new Phone { Country = "日本",City = "东京",Name = "松下"},
                    }
                },
                new Person { Name = "克里斯",Age = 40,Gender = "男",
                    Phones = new List<Phone> {
                        new Phone { Country = "美国",City = "加州",Name = "苹果"},
                        new Phone { Country = "美国",City = "华盛顿",Name = "三星"},
                        new Phone { Country = "美国",City = "华盛顿",Name = "HTC"}
                    }
                }
    };
        static void Main()
        {
            Expression<Func<Person, bool>> expression = x => x.Gender == "男" && x.Name == "克里斯";
            expression = expression.ExpressionAnd(x=>x.Age > 30);
            var res = PersonLists.Where(expression.Compile());
            Console.WriteLine(res.Count());
            Console.WriteLine();
        }
        public static List<byte> Url_To_Byte(String filePath)
        {
            //第一步：读取图片到byte数组
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(filePath);

            byte[] bytes;
            using (Stream stream = request.GetResponse().GetResponseStream())
            {
                using (MemoryStream mstream = new MemoryStream())
                {
                    int count = 0;
                    byte[] buffer = new byte[1024];
                    int readNum = 0;
                    while ((readNum = stream.Read(buffer, 0, 1024)) > 0)
                    {
                        count = count + readNum;
                        mstream.Write(buffer, 0, readNum);
                    }
                    mstream.Position = 0;
                    using (BinaryReader br = new BinaryReader(mstream))
                    {
                        bytes = br.ReadBytes(count);
                    }
                }
            }
            return bytes.ToList();
        }
        public static string GetMD5(string s, string _input_charset)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(
            Encoding.GetEncoding(_input_charset).GetBytes(s)
            );
            StringBuilder sb = new StringBuilder(32);
            for (int i = 0; i < t.Length; i++)
            {
                sb.Append(t[i].ToString("x").PadLeft(2, '0'));
            }
            return sb.ToString();
        }

        /// <summary>
        /// DES 加密。
        /// </summary>
        /// <param name="pToEncrypt">明文。</param>
        /// <param name="sKey">密钥。</param>
        /// <returns>返回密文。</returns>
        static string DESEnCode(string pToEncrypt, string sKey = "12345678")
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.GetEncoding("UTF-8")
            .GetBytes(pToEncrypt);
            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(),
           CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            return ret.ToString(); ;
        }

    }
    public class Person
    {
        public string Name { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public List<Phone> Phones { get; set; }

    }
    public class Phone
    {
        public string Country { get; set; }
        public string City { get; set; }
        public string Name { get; set; }
    }
    public static class ExpresstionExtension
    {
        private static Expression<T> Combine<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
        {
            MyExpressionVisitor visitor = new MyExpressionVisitor(first.Parameters[0]);
            Expression bodyone = visitor.Visit(first.Body);
            Expression bodytwo = visitor.Visit(second.Body);
            return Expression.Lambda<T>(merge(bodyone, bodytwo), first.Parameters[0]);
        }
        public static Expression<Func<T, bool>> ExpressionAnd<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Combine(second, Expression.And);
        }
        public static Expression<Func<T, bool>> ExpressionOr<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Combine(second, Expression.Or);
        }
    }

    internal class MyExpressionVisitor : ExpressionVisitor
    {
        public ParameterExpression _Parameter { get; set; }

        public MyExpressionVisitor(ParameterExpression Parameter)
        {
            _Parameter = Parameter;
        }
        protected override Expression VisitParameter(ParameterExpression p)
        {
            return _Parameter;
        }

        public override Expression Visit(Expression node)
        {
            return base.Visit(node);//Visit会根据VisitParameter()方法返回的Expression修改这里的node变量
        }
    }
}
