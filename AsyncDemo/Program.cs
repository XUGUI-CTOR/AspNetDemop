using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncDemo
{
    class Program
    {
        static void Main1(string[] args)
        {
            //AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            //GetClosureFunction()(30);
            //new Thread(() => throw new Exception("NUMD")).Start();
            Task<int> primeNumberTask = Task.Run<int>(() => Enumerable.Range(2, 3000000)
            .Count(n => Enumerable.Range(2, (int)Math.Sqrt(n) - 1).All(i => n % i > 0))
            );
            Console.WriteLine("Task Running......");
            Console.WriteLine($"The answer is {primeNumberTask.Result}");
        }

        public static void Main2(string[] args)
        {
            //Delay(3000).GetAwaiter().OnCompleted(() => { Console.WriteLine("OK"); });
            //Task.Delay(5000).con
            //Console.ReadKey();
        }

        public static async Task Main(string[] args)
        {
            Enumerable.Range(1, 10).ToList().ForEach(Console.WriteLine);
        }
    
        

        static Task<int> GetAnswerToLife()
        {
            var tcs = new TaskCompletionSource<int>();
            var timer = new System.Timers.Timer(5000) { AutoReset = false };
            timer.Elapsed += (s,e) => { timer.Dispose(); tcs.SetResult(42); };
            timer.Start();
            return tcs.Task;
        }

        public static Task Delay(int milliseconds)
        {
            var tcs = new TaskCompletionSource<object>();
            var timer = new System.Timers.Timer(milliseconds) { AutoReset = false };
            timer.Elapsed += (s, e) => { timer.Dispose(); tcs.SetResult(default); };
            timer.Start();
            return tcs.Task;
        }

        public static async void dosomething()
        {
            int dosometing()
            {
                Thread.Sleep(1000);
                return 2;
            }
            Console.WriteLine(await Run(dosometing));
           
        }

        static Task<TResult> Run<TResult>(Func<TResult> func)
        {
            TaskCompletionSource<TResult> tcs = new TaskCompletionSource<TResult>();
            new Thread(() => {
                try
                {
                    tcs.SetResult(func());
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            }) { IsBackground = true, }.Start();
            return tcs.Task;
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
