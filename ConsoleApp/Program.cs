using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        /// <summary>
        /// Since C# 7.1 we can write async Main methods.
        /// The difference is that it has to have return type of
        /// <see cref="Task"/> or <see cref="Task{TResult}"/>
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        static async Task Main(string[] args)
        {
            await Demo();
        }

        static async Task Demo()
        {
            Console.WriteLine($"{nameof(Demo)} 1");

            var t = NotTrulyAsync();
            
            Console.WriteLine($"{nameof(Demo)} 2");

            await t;
        }

        private static async Task<int> NotTrulyAsync()
        {
            Console.WriteLine($"{nameof(NotTrulyAsync)} 1");

            Task.Delay(10).Wait();

            Console.WriteLine($"{nameof(NotTrulyAsync)} 2");

            return 24;
        }

        private async Task DemoAsync()
        {

        }
        /// <summary>
        /// Stephen Toub's code sample of cancelling Task.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="task"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async Task<T> WithCancellation<T>(this Task<T> task, CancellationToken token)
        {
            var tcs = new TaskCompletionSource<bool>();
            using (token.Register(o => ((TaskCompletionSource<bool>)o).TrySetResult(true), tcs))
                if (task != await Task.WhenAny(tcs.Task, task))
                    throw new OperationCanceledException();
            return await task;
        }
    }
}
