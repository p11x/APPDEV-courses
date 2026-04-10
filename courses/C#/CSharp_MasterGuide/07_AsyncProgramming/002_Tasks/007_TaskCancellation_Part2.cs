/*
 * TOPIC: Task Parallel Library
 * SUBTOPIC: Task Cancellation Part 2
 * FILE: 07_TaskCancellation_Part2.cs
 * PURPOSE: Advanced cancellation patterns and best practices
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CSharp_MasterGuide._07_AsyncProgramming._02_Tasks
{
    public class TaskCancellationPart2
    {
        public static void Main()
        {
            Console.WriteLine("=== Task Cancellation Part 2 Demo ===\n");

            var demo = new TaskCancellationPart2();

            // Example 1: Graceful cancellation with cleanup
            Console.WriteLine("1. Graceful cancellation with cleanup:");
            demo.GracefulCancellationDemo();

            // Example 2: Cancellation with partial results
            Console.WriteLine("\n2. Cancellation with partial results:");
            demo.PartialResultsDemo();

            // Example 3: Cancel after multiple operations
            Console.WriteLine("\n3. Cancel after multiple operations:");
            demo.MultiOperationCancelDemo();

            // Example 4: Concurrent cancellation
            Console.WriteLine("\n4. Concurrent cancellation:");
            demo.ConcurrentCancelDemo();

            // Example 5: Task cancellation with custom exception
            Console.WriteLine("\n5. Custom cancellation exception:");
            demo.CustomCancellationDemo();

            // Example 6: Cancel with timeout extension
            Console.WriteLine("\n6. Cancel with timeout extension:");
            demo.TimeoutExtensionDemo();

            // Example 7: Selective cancellation
            Console.WriteLine("\n7. Selective cancellation:");
            demo.SelectiveCancelDemo();

            Console.WriteLine("\n=== End of Demo ===");
        }

        public void GracefulCancellationDemo()
        {
            var cts = new CancellationTokenSource();
            var completed = new List<int>();

            var task = Task.Run(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    if (cts.Token.IsCancellationRequested)
                    {
                        Console.WriteLine($"   Cancelled at {i}, completed: {string.Join(",", completed)}");
                        return;
                    }
                    Thread.Sleep(30);
                    completed.Add(i);
                }
            }, cts.Token);

            cts.CancelAfter(150);
            task.Wait();
        }

        public void PartialResultsDemo()
        {
            var cts = new CancellationTokenSource();
            var results = new List<int>();

            var task = Task.Run(() =>
            {
                try
                {
                    for (int i = 0; i < 10; i++)
                    {
                        cts.Token.ThrowIfCancellationRequested();
                        Thread.Sleep(30);
                        results.Add(i);
                    }
                }
                catch (OperationCanceledException)
                {
                    // Store partial results before throwing
                }
                return results.ToList();
            }, cts.Token);

            cts.CancelAfter(150);
            task.Wait();
            
            Console.WriteLine($"   Partial results: {string.Join(",", task.Result)}");
        }

        public void MultiOperationCancelDemo()
        {
            var cts = new CancellationTokenSource();
            var tasks = new List<Task<int>>();

            for (int i = 0; i < 5; i++)
            {
                int id = i;
                tasks.Add(Task.Run(() =>
                {
                    for (int j = 0; j < 5; j++)
                    {
                        cts.Token.ThrowIfCancellationRequested();
                        Thread.Sleep(20);
                    }
                    return id * 10;
                }, cts.Token));
            }

            cts.CancelAfter(200);
            
            try
            {
                Task.WaitAll(tasks.ToArray());
            }
            catch (AggregateException)
            {
                var completed = tasks.Where(t => t.IsCompletedSuccessfully).ToList();
                Console.WriteLine($"   Completed: {completed.Count}, Canceled: {tasks.Count(t => t.IsCanceled)}");
            }
        }

        public void ConcurrentCancelDemo()
        {
            var cts = new CancellationTokenSource();
            var lock_ = new object();
            int counter = 0;

            var tasks = Enumerable.Range(1, 5).Select(_ =>
                Task.Run(() =>
                {
                    for (int i = 0; i < 10; i++)
                    {
                        if (cts.Token.IsCancellationRequested)
                            return;
                        
                        lock (lock_)
                        {
                            counter++;
                        }
                        Thread.Sleep(20);
                    }
                }, cts.Token)).ToArray();

            cts.CancelAfter(100);
            Task.WaitAll(tasks);
            
            Console.WriteLine($"   Counter: {counter}");
        }

        public void CustomCancellationDemo()
        {
            var cts = new CancellationTokenSource();

            var task = Task.Run(() =>
            {
                try
                {
                    for (int i = 0; i < 10; i++)
                    {
                        Thread.Sleep(30);
                    }
                }
                finally
                {
                    Console.WriteLine("   Cleanup executed");
                }
            }, cts.Token);

            cts.Cancel();
            task.Wait();
            
            Console.WriteLine($"   Status: {task.Status}");
        }

        public void TimeoutExtensionDemo()
        {
            var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));
            
            var task = Task.Run(() =>
            {
                for (int i = 0; i < 20; i++)
                {
                    cts.Token.ThrowIfCancellationRequested();
                    Thread.Sleep(20);
                }
            }, cts.Token);

            // Extend timeout
            cts.CancelAfter(TimeSpan.FromMilliseconds(200));
            
            task.Wait();
            Console.WriteLine($"   Completed: {task.IsCompleted}");
        }

        public void SelectiveCancelDemo()
        {
            var cts1 = new CancellationTokenSource();
            var cts2 = new CancellationTokenSource();
            
            var t1 = Task.Run(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    cts1.Token.ThrowIfCancellationRequested();
                    Thread.Sleep(30);
                }
            }, cts1.Token);

            var t2 = Task.Run(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    cts2.Token.ThrowIfCancellationRequested();
                    Thread.Sleep(30);
                }
            }, cts2.Token);

            cts1.CancelAfter(150);
            
            Task.WaitAll(t1, t2);
            Console.WriteLine($"   T1: {t1.Status}, T2: {t2.Status}");
        }
    }

    // Real-world cancellation patterns
    public class BatchProcessor
    {
        private CancellationTokenSource _cts;

        public async Task<BatchResult> ProcessBatchAsync(
            IEnumerable<WorkItem> items, int maxConcurrency = 4)
        {
            _cts = new CancellationTokenSource();
            var completed = new List<WorkItem>();
            var errors = new List<Exception>();
            var semaphore = new SemaphoreSlim(maxConcurrency);

            var tasks = items.Select(async item =>
            {
                await semaphore.WaitAsync(_cts.Token);
                try
                {
                    await ProcessItemAsync(item, _cts.Token);
                    completed.Add(item);
                }
                catch (OperationCanceledException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    lock (errors) errors.Add(ex);
                }
                finally
                {
                    semaphore.Release();
                }
            });

            try
            {
                await Task.WhenAll(tasks);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine($"   Cancelled! Processed: {completed.Count}");
            }

            return new BatchResult
            {
                Completed = completed.Count,
                Errors = errors.Count
            };
        }

        private async Task ProcessItemAsync(WorkItem item, CancellationToken token)
        {
            await Task.Delay(50, token);
        }

        public void Cancel() => _cts?.Cancel();
    }

    public class WorkItem
    {
        public int Id { get; set; }
        public string Data { get; set; }
    }

    public class BatchResult
    {
        public int Completed { get; set; }
        public int Errors { get; set; }
    }

    public class ParallelSearchEngine
    {
        public async Task<SearchResult> SearchAsync(
            IEnumerable<string> sources, string query, int timeoutMs, CancellationToken externalToken)
        {
            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(externalToken);
            linkedCts.CancelAfter(timeoutMs);

            var results = new List<string>();
            var tasks = sources.Select(async source =>
            {
                var result = await SearchSourceAsync(source, query, linkedCts.Token);
                lock (results) results.Add(result);
            });

            try
            {
                await Task.WhenAll(tasks);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("   Search cancelled");
            }

            return new SearchResult { Results = results, Query = query };
        }

        private async Task<string> SearchSourceAsync(string source, string query, CancellationToken token)
        {
            await Task.Delay(50, token);
            return $"Result from {source}";
        }
    }

    public class SearchResult
    {
        public List<string> Results { get; set; }
        public string Query { get; set; }
    }
}