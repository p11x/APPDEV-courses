/*
 * TOPIC: Task Parallel Library
 * SUBTOPIC: Task Parallelism Part 2
 * FILE: 05_TaskParallel_Part2.cs
 * PURPOSE: Advanced parallelism patterns, dataflow, and task coordination
 */
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CSharp_MasterGuide._07_AsyncProgramming._02_Tasks
{
    public class TaskParallelPart2
    {
        public static void Main()
        {
            Console.WriteLine("=== Task Parallelism Part 2 Demo ===\n");

            var demo = new TaskParallelPart2();

            // Example 1: Parallel async iteration
            Console.WriteLine("1. Parallel async iteration:");
            await demo.ParallelAsyncIterationAsync();

            // Example 2: Task to async operation with result
            Console.WriteLine("\n2. Converting sync to async parallel:");
            var results = await demo.ConvertSyncToAsyncParallelAsync();
            Console.WriteLine($"   Results: {string.Join(", ", results)}");

            // Example 3: Partitioned parallel work
            Console.WriteLine("\n3. Partitioned parallel work:");
            await demo.PartitionedWorkAsync();

            // Example 4: Concurrent collection with parallel tasks
            Console.WriteLine("\n4. Concurrent collection usage:");
            await demo.ConcurrentCollectionDemoAsync();

            // Example 5: Parallel with thread-local data
            Console.WriteLine("\n5. Thread-local data pattern:");
            await demo.ThreadLocalDataAsync();

            // Example 6: Exception aggregation
            Console.WriteLine("\n6. Exception aggregation:");
            await demo.ExceptionAggregationAsync();

            // Example 7: Cancellation in parallel
            Console.WriteLine("\n7. Cancellation in parallel:");
            await demo.CancellationInParallelAsync();

            // Example 8: Fan-out fan-in pattern
            Console.WriteLine("\n8. Fan-out fan-in pattern:");
            await demo.FanOutFanInAsync();

            Console.WriteLine("\n=== End of Demo ===");
        }

        public async Task ParallelAsyncIterationAsync()
        {
            var items = new[] { "A", "B", "C", "D" };
            
            await Task.WhenAll(items.Select(async item =>
            {
                await Task.Delay(50);
                Console.WriteLine($"   Processed: {item}");
            }));
        }

        public async Task<List<int>> ConvertSyncToAsyncParallelAsync()
        {
            var inputs = Enumerable.Range(1, 5).ToList();
            
            var tasks = inputs.Select(i => Task.Run(() => i * i)).ToList();
            
            return (await Task.WhenAll(tasks)).ToList();
        }

        public async Task PartitionedWorkAsync()
        {
            var data = Enumerable.Range(1, 100).ToList();
            int partitionSize = 25;
            
            var partitions = data
                .Select((item, index) => new { item, index })
                .GroupBy(x => x.index / partitionSize)
                .Select(g => g.Select(x => x.item).ToList())
                .ToList();

            var tasks = partitions.Select(async partition =>
            {
                int sum = 0;
                foreach (var item in partition)
                {
                    await Task.Delay(10);
                    sum += item;
                }
                return sum;
            });

            var results = await Task.WhenAll(tasks);
            Console.WriteLine($"   Partition sums: {string.Join(", ", results)}");
        }

        public async Task ConcurrentCollectionDemoAsync()
        {
            var bag = new ConcurrentBag<string>();
            var queue = new ConcurrentQueue<int>();
            
            var tasks = Enumerable.Range(1, 10).Select(async i =>
            {
                bag.Add($"Item{i}");
                queue.Enqueue(i);
                await Task.Delay(10);
            });
            
            await Task.WhenAll(tasks);
            
            Console.WriteLine($"   Bag count: {bag.Count}, Queue count: {queue.Count}");
        }

        public async Task ThreadLocalDataAsync()
        {
            var tasks = Enumerable.Range(1, 5).Select(async i =>
            {
                int localValue = i * 10;
                await Task.Delay(20);
                Console.WriteLine($"   Task {i}: {localValue}");
                return localValue;
            });
            
            await Task.WhenAll(tasks);
        }

        public async Task ExceptionAggregationAsync()
        {
            var tasks = new List<Task>
            {
                Task.Run(() => { throw new Exception("Error 1"); }),
                Task.Run(() => { throw new Exception("Error 2"); }),
                Task.Run(() => { throw new Exception("Error 3"); })
            };

            try
            {
                await Task.WhenAll(tasks);
            }
            catch
            {
                var exceptions = tasks
                    .Where(t => t.IsFaulted)
                    .SelectMany(t => t.Exception.InnerExceptions);
                
                foreach (var ex in exceptions)
                    Console.WriteLine($"   Exception: {ex.Message}");
            }
        }

        public async Task CancellationInParallelAsync()
        {
            var cts = new CancellationTokenSource();
            
            var tasks = Enumerable.Range(1, 5).Select(i =>
                Task.Run(async () =>
                {
                    for (int j = 0; j < 10; j++)
                    {
                        cts.Token.ThrowIfCancellationRequested();
                        await Task.Delay(20);
                    }
                    return i;
                }, cts.Token));

            cts.CancelAfter(100);
            
            try
            {
                await Task.WhenAll(tasks);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("   Cancelled!");
            }
        }

        public async Task FanOutFanInAsync()
        {
            var items = Enumerable.Range(1, 4).ToList();
            
            // Fan-out: distribute to multiple workers
            var phase1Tasks = items.Select(item => 
                Task.Run(() =>
                {
                    Thread.Sleep(50);
                    return item * 2;
                }));
            
            var phase1Results = await Task.WhenAll(phase1Tasks);
            
            // Fan-in: aggregate results
            var phase2Task = Task.Run(() => phase1Results.Sum());
            
            Console.WriteLine($"   Fan-in result: {await phase2Task}");
        }
    }

    // Real-world patterns for data processing
    public class ParallelDataProcessor
    {
        public async Task<ProcessingReport> ProcessLargeDatasetAsync(
            IEnumerable<DataItem> items, int maxConcurrency = 4)
        {
            var semaphore = new SemaphoreSlim(maxConcurrency);
            var results = new ConcurrentBag<ProcessedItem>();
            var errors = new ConcurrentBag<Exception>();

            var tasks = items.Select(async item =>
            {
                await semaphore.WaitAsync();
                try
                {
                    var result = await ProcessItemAsync(item);
                    results.Add(result);
                }
                catch (Exception ex)
                {
                    errors.Add(ex);
                }
                finally
                {
                    semaphore.Release();
                }
            });

            await Task.WhenAll(tasks);

            return new ProcessingReport
            {
                ProcessedCount = results.Count,
                ErrorCount = errors.Count
            };
        }

        private async Task<ProcessedItem> ProcessItemAsync(DataItem item)
        {
            await Task.Delay(10);
            return new ProcessedItem { Id = item.Id, Result = "Processed" };
        }
    }

    public class DataItem { public int Id { get; set; } }
    public class ProcessedItem { public int Id { get; set; } public string Result { get; set; } }
    public class ProcessingReport { public int ProcessedCount { get; set; } public int ErrorCount { get; set; } }

    // Parallel aggregation pattern
    public class ParallelAggregator
    {
        public async Task<Dictionary<string, int>> AggregateParallelAsync(
            IEnumerable<string> items)
        {
            var counts = new ConcurrentDictionary<string, int>();
            
            var tasks = items.Select(async item =>
            {
                await Task.Delay(10);
                counts.AddOrUpdate(item, 1, (_, count) => count + 1);
            });
            
            await Task.WhenAll(tasks);
            
            return new Dictionary<string, int>(counts);
        }

        public async Task<List<GroupedResult>> GroupAndCountAsync<T>(
            IEnumerable<T> items, Func<T, string> keySelector)
        {
            var groups = new ConcurrentDictionary<string, List<T>>();
            
            await Task.WhenAll(items.Select(async item =>
            {
                var key = keySelector(item);
                groups.AddOrUpdate(key, 
                    new List<T> { item },
                    (_, list) => { list.Add(item); return list; });
            }));
            
            return groups.Select(g => new GroupedResult { Key = g.Key, Count = g.Value.Count }).ToList();
        }
    }

    public class GroupedResult { public string Key { get; set; } public int Count { get; set; } }
}