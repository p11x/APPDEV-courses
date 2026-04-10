/*
 * TOPIC: Async/Await Fundamentals
 * SUBTOPIC: Async/Await Basics Part 2
 * FILE: 02_AsyncAwaitBasics_Part2.cs
 * PURPOSE: Advanced async/await patterns including ValueTask, cancellation, and best practices
 */
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CSharp_MasterGuide._07_AsyncProgramming._01_AsyncAwait
{
    public class AsyncAwaitBasicsPart2
    {
        public static async Task Main()
        {
            Console.WriteLine("=== Async/Await Basics Part 2 Demo ===\n");

            var demo = new AsyncAwaitBasicsPart2();

            // Example 1: Using CancellationToken
            Console.WriteLine("1. Using CancellationToken:");
            var cts = new CancellationTokenSource();
            try
            {
                await demo.CancellableOperationAsync(cts.Token);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("   Operation was cancelled");
            }

            // Example 2: ValueTask for hot path optimization
            Console.WriteLine("\n2. ValueTask for hot path:");
            var hotPathResult = await demo.TryGetCachedValueAsync();
            Console.WriteLine($"   ValueTask result: {hotPathResult}");

            // Example 3: Async method with timeout
            Console.WriteLine("\n3. Async method with timeout:");
            bool completed = await demo.OperationWithTimeoutAsync(TimeSpan.FromMilliseconds(200));
            Console.WriteLine($"   Completed: {completed}");

            // Example 4: Using WhenAll for parallel execution
            Console.WriteLine("\n4. Using WhenAll for parallel execution:");
            await demo.ParallelOperationsAsync();

            // Example 5: Using WhenAny for race conditions
            Console.WriteLine("\n5. Using WhenAny for race conditions:");
            string winner = await demo.RaceConditionDemoAsync();
            Console.WriteLine($"   Winner: {winner}");

            // Example 6: Async disposal pattern
            Console.WriteLine("\n6. Using await using for async disposal:");
            await demo.AsyncDisposeDemoAsync();

            // Example 7: Mixing async and synchronous code
            Console.WriteLine("\n7. Mixing async and synchronous:");
            demo.MixedExecutionDemo();

            // Example 8: Async state machine demonstration
            Console.WriteLine("\n8. Async state machine:");
            await demo.StateMachineDemoAsync();

            Console.WriteLine("\n=== End of Demo ===");
        }

        public async Task CancellableOperationAsync(CancellationToken token)
        {
            Console.WriteLine("   Starting cancellable operation");
            for (int i = 0; i < 5; i++)
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(100, token);
                Console.WriteLine($"   Step {i + 1} completed");
            }
            Console.WriteLine("   Cancellable operation finished");
        }

        public async ValueTask<int> TryGetCachedValueAsync()
        {
            // Simulate checking cache (hot path)
            bool cached = false; // Would check actual cache
            
            if (cached)
            {
                return 42; // Return synchronously if cached
            }
            
            await Task.Delay(100);
            return 100;
        }

        public async Task<bool> OperationWithTimeoutAsync(TimeSpan timeout)
        {
            var cts = new CancellationTokenSource(timeout);
            try
            {
                await Task.Delay(300, cts.Token);
                return true;
            }
            catch (OperationCanceledException)
            {
                return false;
            }
        }

        public async Task ParallelOperationsAsync()
        {
            var task1 = Task.Delay(100).ContinueWith(_ => "Result1");
            var task2 = Task.Delay(50).ContinueWith(_ => "Result2");
            var task3 = Task.Delay(150).ContinueWith(_ => "Result3");

            string[] results = await Task.WhenAll(task1, task2, task3);
            Console.WriteLine($"   Results: {string.Join(", ", results)}");
        }

        public async Task<string> RaceConditionDemoAsync()
        {
            var task1 = Task.Delay(200).ContinueWith(_ => "Fast Source");
            var task2 = Task.Delay(100).ContinueWith(_ => "Slow Source");

            var completed = await Task.WhenAny(task1, task2);
            return await completed;
        }

        public async Task AsyncDisposeDemoAsync()
        {
            await using var resource = new AsyncDisposableResource();
            await resource.OperationAsync();
            Console.WriteLine("   Resource disposed");
        }

        public void MixedExecutionDemo()
        {
            Console.WriteLine("   Starting sync method");
            
            // Starting async from sync (fire and forget pattern)
            Task.Run(async () =>
            {
                await Task.Delay(100);
                Console.WriteLine("   Async work completed in background");
            });

            Console.WriteLine("   Sync method continuing immediately");
        }

        public async Task StateMachineDemoAsync()
        {
            var steps = new[]
            {
                async (int i) => { await Task.Delay(50); Console.WriteLine($"   Step {i}"); }
            };

            for (int i = 1; i <= 3; i++)
            {
                await steps[0](i);
            }
            Console.WriteLine("   State machine complete");
        }
    }

    public class AsyncDisposableResource : IAsyncDisposable
    {
        public async Task OperationAsync()
        {
            await Task.Delay(50);
            Console.WriteLine("   Performing async operation");
        }

        public async ValueTask DisposeAsync()
        {
            await Task.Delay(50);
            Console.WriteLine("   AsyncDisposeResource disposed");
        }
    }

    // Real-world example: Data processing pipeline
    public class DataPipeline
    {
        public async Task<string> ProcessDataAsync(string input)
        {
            await Task.Delay(50);
            return input.ToUpper();
        }

        public async Task<string> FetchDataAsync(CancellationToken token = default)
        {
            await Task.Delay(100, token);
            return "Fetched Data";
        }

        public async Task SaveDataAsync(string data)
        {
            await Task.Delay(50);
            Console.WriteLine($"   Saved: {data}");
        }

        public async Task<string> TransformDataAsync(string data)
        {
            await Task.Delay(50);
            return $"Transformed: {data}";
        }

        public async Task<(string Data, bool Success)> TryFetchWithRetryAsync(int maxRetries = 3)
        {
            for (int i = 0; i < maxRetries; i++)
            {
                try
                {
                    var data = await FetchDataAsync();
                    return (data, true);
                }
                catch when (i < maxRetries - 1)
                {
                    await Task.Delay(100 * (i + 1));
                }
            }
            return (null, false);
        }
    }
}