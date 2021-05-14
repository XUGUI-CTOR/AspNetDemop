using System;
using System.IO;
using System.Text.RegularExpressions;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            //string dic = @"E:\BcDownLoad\Root";
            string filename = @"202010\Iamges\Pic10215848212.png";
            //Console.WriteLine(Path.Join(dic,filename));
            Console.WriteLine(DateTime.Now.Ticks.ToString("X"));
            Console.WriteLine(Path.GetExtension(filename));
            Console.WriteLine(ToCommnStr(filename));
        }

        private static string CreateServiceRelativePath(string path, string RootDir, string timestamp)
        {

            //string path = fileInfo.FullName;
            var relativePath = Path.GetRelativePath(RootDir, path);
            //Console.WriteLine(Path.GetDirectoryName(relativePath));
            var firstDir = relativePath.Substring(0, relativePath.IndexOf('\\'));//Path.GetPathRoot(relativePath); 
            var productNo = firstDir.Substring(0, firstDir.IndexOf('-'));
            var fileName = Path.GetFileNameWithoutExtension(relativePath);
            fileName = fileName + '-' + timestamp;
            var dirLength = Path.GetDirectoryName(relativePath).Length - firstDir.Length;
            //Console.WriteLine(Path.Combine(productNo, Path.GetDirectoryName(relativePath).Substring(firstDir.Length, dirLength)));
            //productNo += '\\';
            var otherDir = dirLength > 0 ? Path.GetDirectoryName(relativePath).Substring(firstDir.Length, dirLength) + '\\' : "";
            fileName += Path.GetExtension(relativePath);
            var serviceRelativePath = Path.Join(productNo, otherDir, fileName);
            return serviceRelativePath;
        }

        public static string GetTimeStamp(DateTime now)
        {
            TimeSpan ts = now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        public static string ToCommnStr(string str)
        {
            //[^A-Za-z0-9_.]
            if (str == String.Empty)
                return String.Empty; str = str.Replace("'", "");
            //str = str.Replace(";", "");
            //str = str.Replace(",", "");
            //str = str.Replace("?", "");
            //str = str.Replace("<", "");
            //str = str.Replace(">", "");
            //str = str.Replace("(", "");
            //str = str.Replace(")", "");
            //str = str.Replace("）", "");
            //str = str.Replace("（", "");
            //str = str.Replace("@", "");
            //str = str.Replace("=", "");
            //str = str.Replace("+", "");
            //str = str.Replace("*", "");
            //str = str.Replace("&", "");
            //str = str.Replace("#", "");
            //str = str.Replace("%", "");
            //str = str.Replace("$", "");
            //str = str.Replace("/", "");
            //str = str.Replace("\\", "");
            Regex reg = new Regex(@"[a-zA-Z0-9\w.\u4e00-\u9fa5]");
            var m = reg.Matches(str);
            if (m.Count > 0)
            {
                var str2 = "";
                foreach (Match t in m)
                {
                    str2 += t.Value;

                }
                str = str2;
            }
            return str;
        }
    }
}
