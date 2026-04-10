/*
 * TOPIC: Task Parallel Library
 * SUBTOPIC: Task Parallelism
 * FILE: 04_TaskParallel.cs
 * PURPOSE: Understanding Task.WhenAll and Task.WhenAny for parallel execution
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CSharp_MasterGuide._07_AsyncProgramming._02_Tasks
{
    public class TaskParallel
    {
        public static void Main()
        {
            Console.WriteLine("=== Task Parallelism Demo ===\n");

            var demo = new TaskParallel();

            // Example 1: Task.WhenAll - wait for all tasks
            Console.WriteLine("1. Task.WhenAll:");
            var sw = Stopwatch.StartNew();
            await demo.WhenAllDemoAsync();
            sw.Stop();
            Console.WriteLine($"   Elapsed: {sw.ElapsedMilliseconds}ms");

            // Example 2: Task.WhenAny - wait for first task
            Console.WriteLine("\n2. Task.WhenAny:");
            sw.Restart();
            var winner = await demo.WhenAnyDemoAsync();
            sw.Stop();
            Console.WriteLine($"   Winner: {winner}, Time: {sw.ElapsedMilliseconds}ms");

            // Example 3: WhenAll with results
            Console.WriteLine("\n3. WhenAll with results:");
            var results = await demo.WhenAllWithResultsAsync();
            Console.WriteLine($"   Results: {string.Join(", ", results)}");

            // Example 4: Sequential vs parallel comparison
            Console.WriteLine("\n4. Sequential vs Parallel:");
            await demo.SequentialVsParallelAsync();

            // Example 5: WhenAll with exceptions
            Console.WriteLine("\n5. WhenAll with exceptions:");
            await demo.WhenAllWithExceptionsAsync();

            // Example 6: WhenAny practical use - timeout
            Console.WriteLine("\n6. WhenAny - Timeout pattern:");
            await demo.TimeoutWithWhenAnyAsync();

            // Example 7: Multiple when all nested
            Console.WriteLine("\n7. Nested WhenAll:");
            await demo.NestedWhenAllAsync();

            // Example 8: Large scale parallel processing
            Console.WriteLine("\n8. Large scale processing:");
            await demo.LargeScaleProcessingAsync();

            Console.WriteLine("\n=== End of Demo ===");
        }

        public async Task WhenAllDemoAsync()
        {
            var task1 = Task.Run(() => { Thread.Sleep(100); return 1; });
            var task2 = Task.Run(() => { Thread.Sleep(100); return 2; });
            var task3 = Task.Run(() => { Thread.Sleep(100); return 3; });

            await Task.WhenAll(task1, task2, task3);
            Console.WriteLine($"   All completed: {task1.Result}, {task2.Result}, {task3.Result}");
        }

        public async Task<string> WhenAnyDemoAsync()
        {
            var tasks = new[]
            {
                Task.Run(async () =>
                {
                    await Task.Delay(200);
                    return "Slow";
                }),
                Task.Run(async () =>
                {
                    await Task.Delay(50);
                    return "Fast";
                }),
                Task.Run(async () =>
                {
                    await Task.Delay(100);
                    return "Medium";
                })
            };

            var completedTask = await Task.WhenAny(tasks);
            return await completedTask;
        }

        public async Task<List<int>> WhenAllWithResultsAsync()
        {
            var tasks = Enumerable.Range(1, 5).Select(i =>
                Task.Run(() =>
                {
                    Thread.Sleep(50);
                    return i * 10;
                }));

            return (await Task.WhenAll(tasks)).ToList();
        }

        public async Task SequentialVsParallelAsync()
        {
            // Sequential
            var sw = Stopwatch.StartNew();
            await Task.Run(() => { Thread.Sleep(100); });
            await Task.Run(() => { Thread.Sleep(100); });
            await Task.Run(() => { Thread.Sleep(100); });
            sw.Stop();
            Console.WriteLine($"   Sequential: {sw.ElapsedMilliseconds}ms");

            // Parallel
            sw.Restart();
            await Task.WhenAll(
                Task.Run(() => { Thread.Sleep(100); }),
                Task.Run(() => { Thread.Sleep(100); }),
                Task.Run(() => { Thread.Sleep(100); })
            );
            sw.Stop();
            Console.WriteLine($"   Parallel: {sw.ElapsedMilliseconds}ms");
        }

        public async Task WhenAllWithExceptionsAsync()
        {
            var tasks = new[]
            {
                Task.Run(() => 1),
                Task.Run(() => { throw new Exception("Error!"); }),
                Task.Run(() => 3)
            };

            try
            {
                await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"   Caught: {ex.Message}");
                var faulted = tasks.Where(t => t.IsFaulted).Count();
                Console.WriteLine($"   Faulted tasks: {faulted}");
            }
        }

        public async Task TimeoutWithWhenAnyAsync()
        {
            var workTask = Task.Run(async () =>
            {
                await Task.Delay(500);
                return "Done";
            });

            var timeoutTask = Task.Delay(100);

            var completed = await Task.WhenAny(workTask, timeoutTask);
            
            if (completed == timeoutTask)
                Console.WriteLine("   Timeout!");
            else
                Console.WriteLine($"   Result: {await workTask}");
        }

        public async Task NestedWhenAllAsync()
        {
            var batch1 = new[] { Task.Run(() => 1), Task.Run(() => 2) };
            var batch2 = new[] { Task.Run(() => 3), Task.Run(() => 4) };

            var allResults = await Task.WhenAll(
                Task.WhenAll(batch1),
                Task.WhenAll(batch2)
            );

            Console.WriteLine($"   Combined: {string.Join(", ", allResults.SelectMany(x => x))}");
        }

        public async Task LargeScaleProcessingAsync()
        {
            var items = Enumerable.Range(1, 20).ToList();
            
            var tasks = items.Select(item =>
                Task.Run(() =>
                {
                    Thread.Sleep(30);
                    return item * 2;
                }));

            var results = await Task.WhenAll(tasks);
            Console.WriteLine($"   Processed {results.Length} items");
        }
    }

    // Real-world patterns
    public class ParallelDataLoader
    {
        public async Task<Dictionary<string, T>> LoadMultipleAsync<T>(
            IEnumerable<string> urls, Func<string, Task<T>> loader)
        {
            var tasks = urls.Select(async url =>
            {
                var data = await loader(url);
                return (url, data);
            });

            var results = await Task.WhenAll(tasks);
            return results.ToDictionary(r => r.url, r => r.data);
        }

        public async Task<string> FetchFirstAvailableAsync(
            IEnumerable<string> urls, Func<string, Task<string>> fetcher)
        {
            var tasks = urls.Select(url => fetcher(url)).ToArray();
            
            while (tasks.Length > 0)
            {
                var completed = await Task.WhenAny(tasks);
                try
                {
                    return await completed;
                }
                catch
                {
                    tasks = tasks.Where(t => t != completed).ToArray();
                }
            }
            
            throw new Exception("All sources failed");
        }

        public async Task ProcessBatchWithThrottleAsync<T>(
            IEnumerable<T> items, int maxConcurrency, Func<T, Task> processor)
        {
            var semaphore = new SemaphoreSlim(maxConcurrency);
            
            var tasks = items.Select(async item =>
            {
                await semaphore.WaitAsync();
                try
                {
                    await processor(item);
                }
                finally
                {
                    semaphore.Release();
                }
            });

            await Task.WhenAll(tasks);
        }
    }

    public class AggregateDataFetcher
    {
        public async Task<AggregateResult> FetchAllDataAsync()
        {
            var usersTask = Task.Run(() => new[] { "User1", "User2" });
            var ordersTask = Task.Run(() => new[] { "Order1", "Order2" });
            var productsTask = Task.Run(() => new[] { "Product1", "Product2" });

            await Task.WhenAll(usersTask, ordersTask, productsTask);

            return new AggregateResult
            {
                Users = usersTask.Result,
                Orders = ordersTask.Result,
                Products = productsTask.Result
            };
        }
    }

    public class AggregateResult
    {
        public string[] Users { get; set; }
        public string[] Orders { get; set; }
        public string[] Products { get; set; }
    }
}