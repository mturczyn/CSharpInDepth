using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Exceptions
{
    class Program
    {
        static void Main(string[] args)
        {
            //AggregateExceptionDemo();
            ExceptionHandlingWithAwait();
        }

        /// <summary>
        /// Method calculates length of string, but completes immediately in case
        /// of exception (null argument).
        /// Jon Skeet note:
        /// Now when ComputeLengthAsync is called with a null argument, the exception is thrown synchronously rather than returning a faulted task.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static Task<int> ComputeLengthAsync(string str)
        {
            // To throw exception as soon as possible and not start async operation.
            if (str is null) throw new ArgumentNullException(nameof(str));
            return ComputeLengthAsyncImpl(str);
        }
        /// <summary>
        /// Implementation of a method.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static async Task<int> ComputeLengthAsyncImpl(string str)
        {
            // To imitate async operation.
            return await Task.FromResult(str.Length);
        }
        /// <summary>
        /// Preferable way of "eager" exception throwing.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static async Task<int> ComputeLengthAsyncV2(string str)
        {
            if (str is null) throw new ArgumentNullException(nameof(str));
            return await impl();
            // Implementation as local async method.
            async Task<int> impl() => await Task.FromResult(str.Length);
        }
        private static void ExceptionHandlingWithAwait()
        {
            var task = FetchFirstSuccessfulAsync(new string[] 
            {
                "https://wsww.meczyki.pl/",
                "https://www.meczyki.pl/",
            });
            //var result = task.Result;
            task.GetAwaiter().GetResult();
        }

        private static async Task<string> FetchFirstSuccessfulAsync(IEnumerable<string> urls)
        {
            var client = new HttpClient();
            foreach(var url in urls)
            {
                try
                {
                    // Returns the string if successful.
                    return await client.GetStringAsync(url);
                }
                // Catches and displays the failure.
                catch(HttpRequestException ex)
                {
                    Console.WriteLine("Failed to fetch {0}: {1}", url, ex.Message);
                }
            }
            throw new HttpRequestException("No URLs succeeded");
        }

        private static void AggregateExceptionDemo()
        {
            try
            {
                var r = Parallel.For(0, 20, i =>
                {
                    if (i > 10)
                        throw new InvalidOperationException($"Exception on {i} iteration");
                });
                
            }
            catch (AggregateException ae)
            {
                ae.Handle(e =>
                {
                    if (e is InvalidOperationException)
                        return true;
                    return false;
                });
            }
        }
    }
}
