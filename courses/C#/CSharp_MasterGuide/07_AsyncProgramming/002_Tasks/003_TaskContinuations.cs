/*
 * TOPIC: Task Parallel Library
 * SUBTOPIC: Task Continuations
 * FILE: 03_TaskContinuations.cs
 * PURPOSE: Understanding ContinueWith for task chaining and composition
 */
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CSharp_MasterGuide._07_AsyncProgramming._02_Tasks
{
    public class TaskContinuations
    {
        public static void Main()
        {
            Console.WriteLine("=== Task Continuations Demo ===\n");

            var demo = new TaskContinuations();

            // Example 1: Basic ContinueWith
            Console.WriteLine("1. Basic ContinueWith:");
            Task.Run(() =>
            {
                Console.WriteLine("   Initial task");
            })
            .ContinueWith(t =>
            {
                Console.WriteLine("   Continuation executed");
            })
            .Wait();

            // Example 2: ContinueWith with result
            Console.WriteLine("\n2. ContinueWith passing result:");
            Task<int> continuationWithResult = Task.Run(() => 100)
                .ContinueWith(t => t.Result * 2);
            Console.WriteLine($"   Result: {continuationWithResult.Result}");

            // Example 3: ContinueWith with different task types
            Console.WriteLine("\n3. ContinueWith transforming result:");
            Task<string> transformed = Task.Run(() => "Hello")
                .ContinueWith(t => $"{t.Result} World");
            Console.WriteLine($"   Transformed: {transformed.Result}");

            // Example 4: Multiple continuations
            Console.WriteLine("\n4. Multiple continuations:");
            Task.Run(() => 10)
                .ContinueWith(t => t.Result + 5)
                .ContinueWith(t => t.Result * 2)
                .ContinueWith(t => Console.WriteLine($"   Final: {t.Result}"))
                .Wait();

            // Example 5: ContinueWith with TaskScheduler
            Console.WriteLine("\n5. ContinueWith TaskScheduler:");
            Task.Run(() => "Task")
                .ContinueWith(t =>
                {
                    Console.WriteLine($"   Continued on thread {Thread.CurrentThread.ManagedThreadId}");
                }, TaskScheduler.Default)
                .Wait();

            // Example 6: ContinueWith options - NotOnFaulted
            Console.WriteLine("\n6. ContinueWith options (NotOnFaulted):");
            var failingTask = Task.Run(() => { throw new Exception("Error!"); });
            failingTask.ContinueWith(t =>
            {
                Console.WriteLine("   Faulted continuation - should not run");
            }, TaskContinuationOptions.NotOnFaulted);
            
            Thread.Sleep(100); // Let failing task complete

            // Example 7: ContinueWith options - OnlyOnFaulted
            Console.WriteLine("\n7. ContinueWith options (OnlyOnFaulted):");
            var failingTask2 = Task.Run(() => { throw new Exception("Error!"); });
            failingTask2.ContinueWith(t =>
            {
                Console.WriteLine($"   Exception caught: {t.Exception.InnerException.Message}");
            }, TaskContinuationOptions.OnlyOnFaulted);
            
            Thread.Sleep(100);

            // Example 8: ContinueWith options - OnlyOnRanToCompletion
            Console.WriteLine("\n8. ContinueWith options (OnlyOnRanToCompletion):");
            Task.Run(() => 42)
                .ContinueWith(t =>
                {
                    Console.WriteLine($"   Completed with result: {t.Result}");
                }, TaskContinuationOptions.OnlyOnRanToCompletion)
                .Wait();

            // Example 9: Chained continuations
            Console.WriteLine("\n9. Chained continuations:");
            demo.ChainOperationsAsync().Wait();

            // Example 10: ContinueWith with cancellation
            Console.WriteLine("\n10. ContinueWith with cancellation:");
            demo.ContinueWithCancellationAsync().Wait();

            Console.WriteLine("\n=== End of Demo ===");
        }

        public async Task ChainOperationsAsync()
        {
            var result = await Task.Run(() => 10)
                .ContinueWith(t => t.Result + 5)
                .ContinueWith(t => t.Result * 2)
                .ContinueWith(t => t.Result.ToString());
            Console.WriteLine($"   Chained result: {result}");
        }

        public async Task ContinueWithCancellationAsync()
        {
            var cts = new CancellationTokenSource();
            await Task.Run(() =>
            {
                Thread.Sleep(50);
            }, cts.Token)
            .ContinueWith(t =>
            {
                Console.WriteLine($"   Cancelled: {t.IsCanceled}");
            }, CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.Default);
        }
    }

    // Real-world continuation patterns
    public class DataProcessingPipeline
    {
        public Task<string> ProcessAsync(string input)
        {
            return Task.Run(() => input)
                .ContinueWith(t => t.Result.ToUpper())
                .ContinueWith(t => $"Processed: {t.Result}")
                .ContinueWith(t => t.Result);
        }

        public Task<ProcessingResult> FetchAndProcessAsync()
        {
            var fetch = Task.Run(() =>
            {
                Thread.Sleep(50);
                return "Raw data";
            });

            return fetch.ContinueWith(t =>
            {
                var data = t.Result;
                return new ProcessingResult
                {
                    Data = data,
                    ProcessedAt = DateTime.Now,
                    Success = true
                };
            });
        }

        public Task LogAsync(string message)
        {
            return Task.Run(() => Console.WriteLine($"   Log: {message}"))
                .ContinueWith(t => { }); // Discard result
        }
    }

    public class ProcessingResult
    {
        public string Data { get; set; }
        public DateTime ProcessedAt { get; set; }
        public bool Success { get; set; }
    }

    public class AsyncWorkflow
    {
        public Task ExecuteAsync()
        {
            var step1 = Task.Run(() => { Console.WriteLine("   Step 1"); });
            var step2 = step1.ContinueWith(_ => { Console.WriteLine("   Step 2"); });
            var step3 = step2.ContinueWith(_ => { Console.WriteLine("   Step 3"); });
            return step3;
        }

        public Task<string> FetchDataWithRetryAsync()
        {
            var attempt = 0;
            return Task.Run(() => "Data")
                .ContinueWith(t =>
                {
                    if (++attempt < 3)
                        throw new Exception("Retry");
                    return t.Result;
                });
        }
    }
}