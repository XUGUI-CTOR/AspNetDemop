using System;
using System.Threading;

namespace AsyncDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            //GetClosureFunction()(30);
            new Thread(() => throw new Exception("NUMD")).Start();
            
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine(e.ExceptionObject);
        }

        static Action<int> GetClosureFunction()
        {
            int val = 10;
            void InteralAdd(int x) => Console.WriteLine(x+val);
            InteralAdd(10);
            val = 30;
            InteralAdd(20);
            return InteralAdd;
        }
    }
}
