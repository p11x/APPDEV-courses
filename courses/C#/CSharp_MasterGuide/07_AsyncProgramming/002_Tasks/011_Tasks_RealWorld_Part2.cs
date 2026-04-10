/*
 * TOPIC: Task Parallel Library
 * SUBTOPIC: Tasks Real-World Part 2
 * FILE: 11_Tasks_RealWorld_Part2.cs
 * PURPOSE: More real-world Task patterns including advanced scenarios
 */
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CSharp_MasterGuide._07_AsyncProgramming._02_Tasks
{
    public class TasksRealWorldPart2
    {
        public static async Task Main()
        {
            Console.WriteLine("=== Tasks Real-World Part 2 Demo ===\n");

            var demo = new TasksRealWorldPart2();

            // Example 1: Rate-limited API calls
            Console.WriteLine("1. Rate-limited API calls:");
            await demo.RateLimitedCallsAsync();

            // Example 2: Circuit breaker pattern
            Console.WriteLine("\n2. Circuit breaker pattern:");
            await demo.CircuitBreakerDemoAsync();

            // Example 3: Bulkhead pattern
            Console.WriteLine("\n3. Bulkhead pattern:");
            await demo.BulkheadPatternAsync();

            // Example 4: Work queue with workers
            Console.WriteLine("\n4. Work queue with workers:");
            await demo.WorkQueueDemoAsync();

            // Example 5: Barrier synchronization
            Console.WriteLine("\n5. Barrier synchronization:");
            await demo.BarrierDemoAsync();

            // Example 6: Semaphore for concurrency control
            Console.WriteLine("\n6. Semaphore concurrency control:");
            await demo.SemaphoreDemoAsync();

            // Example 7: Task timeout wrapper
            Console.WriteLine("\n7. Task timeout wrapper:");
            await demo.TimeoutWrapperAsync();

            Console.WriteLine("\n=== End of Demo ===");
        }

        public async Task RateLimitedCallsAsync()
        {
            var semaphore = new SemaphoreSlim(3); // Max 3 concurrent
            var results = new ConcurrentBag<string>();

            var tasks = Enumerable.Range(1, 10).Select(async i =>
            {
                await semaphore.WaitAsync();
                try
                {
                    await Task.Delay(50);
                    results.Add($"Task {i} completed");
                }
                finally
                {
                    semaphore.Release();
                }
            });

            await Task.WhenAll(tasks);
            Console.WriteLine($"   Completed: {results.Count}");
        }

        public async Task CircuitBreakerDemoAsync()
        {
            var breaker = new CircuitBreaker(3, TimeSpan.FromSeconds(2));
            
            for (int i = 0; i < 10; i++)
            {
                var result = await breaker.ExecuteAsync(() => Task.Run(() =>
                {
                    if (i % 3 == 0) throw new Exception("Fail");
                    return "Success";
                }));
                
                Console.WriteLine($"   Attempt {i + 1}: {result}");
                await Task.Delay(100);
            }
        }

        public async Task BulkheadPatternAsync()
        {
            var bulkhead = new Bulkhead(2, 5); // 2 concurrent, 5 queued

            var tasks = Enumerable.Range(1, 8).Select(async i =>
            {
                var result = await bulkhead.ExecuteAsync(async () =>
                {
                    await Task.Delay(50);
                    return $"Task {i}";
                });
                Console.WriteLine($"   {result}");
            });

            await Task.WhenAll(tasks);
        }

        public async Task WorkQueueDemoAsync()
        {
            var queue = new WorkQueue(3);
            var results = new ConcurrentBag<string>();

            // Add work
            for (int i = 0; i < 10; i++)
            {
                int taskId = i;
                queue.Enqueue(async () =>
                {
                    await Task.Delay(30);
                    results.Add($"Task {taskId} done");
                });
            }

            await queue.Completion;
            Console.WriteLine($"   Processed: {results.Count}");
        }

        public async Task BarrierDemoAsync()
        {
            var barrier = new Barrier(3);
            var phaseResults = new List<string>();

            var tasks = Enumerable.Range(1, 3).Select(async i =>
            {
                for (int p = 0; p < 3; p++)
                {
                    await Task.Delay(30);
                    phaseResults.Add($"Task {i} phase {p}");
                    barrier.SignalAndWait();
                }
            });

            await Task.WhenAll(tasks);
            Console.WriteLine($"   Phases completed: {phaseResults.Count / 3}");
        }

        public async Task SemaphoreDemoAsync()
        {
            var semaphore = new SemaphoreSlim(2, 2);
            var tasks = Enumerable.Range(1, 4).Select(async i =>
            {
                Console.WriteLine($"   Task {i} waiting...");
                await semaphore.WaitAsync();
                try
                {
                    Console.WriteLine($"   Task {i} executing");
                    await Task.Delay(50);
                }
                finally
                {
                    semaphore.Release();
                    Console.WriteLine($"   Task {i} released");
                }
            });

            await Task.WhenAll(tasks);
        }

        public async Task TimeoutWrapperAsync()
        {
            var result = await WithTimeout(Task.Run(async () =>
            {
                await Task.Delay(200);
                return "Done";
            }), TimeSpan.FromMilliseconds(100));

            Console.WriteLine($"   Result: {result ?? "Timeout"}");
        }

        private async Task<string> WithTimeout(Task<string> task, TimeSpan timeout)
        {
            var timeoutTask = Task.Delay(timeout).ContinueWith(_ => (string)null);
            return await Task.WhenAny(task, timeoutTask).Unwrap();
        }
    }

    public class CircuitBreaker
    {
        private int _failureCount;
        private readonly int _threshold;
        private readonly TimeSpan _resetTimeout;
        private DateTime _lastFailure;
        private CircuitState _state = CircuitState.Closed;

        public CircuitBreaker(int threshold, TimeSpan resetTimeout)
        {
            _threshold = threshold;
            _resetTimeout = resetTimeout;
        }

        public async Task<string> ExecuteAsync(Func<Task<string>> operation)
        {
            if (_state == CircuitState.Open)
            {
                if (DateTime.Now - _lastFailure > _resetTimeout)
                    _state = CircuitState.HalfOpen;
                else
                    throw new Exception("Circuit open");
            }

            try
            {
                var result = await operation();
                _state = CircuitState.Closed;
                _failureCount = 0;
                return result;
            }
            catch
            {
                _failureCount++;
                _lastFailure = DateTime.Now;
                if (_failureCount >= _threshold)
                    _state = CircuitState.Open;
                throw;
            }
        }
    }

    public enum CircuitState { Closed, Open, HalfOpen }

    public class Bulkhead
    {
        private readonly SemaphoreSlim _concurrent;
        private readonly SemaphoreSlim _queue;
        private int _queued;

        public Bulkhead(int maxConcurrent, int maxQueued)
        {
            _concurrent = new SemaphoreSlim(maxConcurrent);
            _queue = new SemaphoreSlim(maxQueued);
        }

        public async Task<T> ExecuteAsync<T>(Func<Task<T>> operation)
        {
            if (!await _queue.WaitAsync(0))
                throw new Exception("Queue full");

            try
            {
                await _concurrent.WaitAsync();
                return await operation();
            }
            finally
            {
                _concurrent.Release();
                _queue.Release();
            }
        }
    }

    public class WorkQueue
    {
        private readonly SemaphoreSlim _sem;
        private readonly Queue<Func<Task>> _work = new();
        private readonly Task[] _workers;
        private readonly TaskCompletionSource<bool> _tcs = new();

        public WorkQueue(int workerCount)
        {
            _sem = new SemaphoreSlim(0);
            _workers = Enumerable.Range(0, workerCount)
                .Select(_ => Task.Run(ProcessAsync))
                .ToArray();
        }

        public void Enqueue(Func<Task> work)
        {
            lock (_work)
            {
                _work.Enqueue(work);
            }
            _sem.Release();
        }

        public Task Completion => _tcs.Task;

        private async Task ProcessAsync()
        {
            while (true)
            {
                await _sem.WaitAsync();
                Func<Task> work;
                lock (_work)
                {
                    if (_work.Count == 0)
                    {
                        if (_workers.All(w => w.IsCompleted))
                            _tcs.SetResult(true);
                        return;
                    }
                    work = _work.Dequeue();
                }
                await work();
            }
        }
    }

    // Real-world service implementations
    public class ResilientHttpClient
    {
        private readonly HttpClient _client;
        private readonly CircuitBreaker _breaker;

        public ResilientHttpClient()
        {
            _client = new HttpClient();
            _breaker = new CircuitBreaker(5, TimeSpan.FromSeconds(30));
        }

        public async Task<string> GetAsync(string url)
        {
            return await _breaker.ExecuteAsync(async () =>
            {
                await Task.Delay(50);
                return $"Response from {url}";
            });
        }

        public async Task<T> GetWithFallbackAsync<T>(
            string primaryUrl, string fallbackUrl, Func<string, T> parser)
        {
            try
            {
                var result = await GetAsync(primaryUrl);
                return parser(result);
            }
            catch
            {
                var fallback = await GetAsync(fallbackUrl);
                return parser(fallback);
            }
        }
    }

    public class ParallelDataProcessor
    {
        public async Task<ProcessingResult> ProcessInBatchesAsync<T>(
            IEnumerable<T> items, int batchSize, int maxConcurrency,
            Func<T[], Task> batchProcessor)
        {
            var batches = items
                .Select((item, index) => new { item, index })
                .GroupBy(x => x.index / batchSize)
                .Select(g => g.Select(x => x.item).ToArray())
                .ToList();

            var semaphore = new SemaphoreSlim(maxConcurrency);
            var completed = 0;
            var failed = 0;

            var tasks = batches.Select(async batch =>
            {
                await semaphore.WaitAsync();
                try
                {
                    await batchProcessor(batch);
                    Interlocked.Increment(ref completed);
                }
                catch
                {
                    Interlocked.Increment(ref failed);
                }
                finally
                {
                    semaphore.Release();
                }
            });

            await Task.WhenAll(tasks);

            return new ProcessingResult
            {
                Completed = completed,
                Failed = failed
            };
        }
    }

    public class ProcessingResult
    {
        public int Completed { get; set; }
        public int Failed { get; set; }
    }
}