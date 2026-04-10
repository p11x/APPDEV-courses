/*
 * TOPIC: Async/Await Fundamentals
 * SUBTOPIC: Real-World Examples Part 2
 * FILE: 08_AsyncAwait_RealWorld_Part2.cs
 * PURPOSE: More real-world async patterns including pipeline, batching, and streaming
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CSharp_MasterGuide._07_AsyncProgramming._01_AsyncAwait
{
    public class AsyncAwaitRealWorldPart2
    {
        public static async Task Main()
        {
            Console.WriteLine("=== Async/Await Real-World Part 2 Demo ===\n");

            var demo = new AsyncAwaitRealWorldPart2();

            // Example 1: Async pipeline pattern
            Console.WriteLine("1. Async pipeline pattern:");
            var pipelineResult = await demo.ProcessPipelineAsync(new[] { "item1", "item2", "item3" });
            Console.WriteLine($"   Processed: {pipelineResult} items");

            // Example 2: Batched async processing
            Console.WriteLine("\n2. Batched async processing:");
            await demo.ProcessBatchAsync(Enumerable.Range(1, 25).Select(i => $"Item{i}"));

            // Example 3: Async initialization pattern
            Console.WriteLine("\n3. Async lazy initialization:");
            await demo.AsyncLazyInitializationAsync();

            // Example 4: Producer-consumer with async
            Console.WriteLine("\n4. Producer-consumer pattern:");
            await demo.ProducerConsumerAsync();

            // Example 5: Async semaphore for rate limiting
            Console.WriteLine("\n5. Rate limiting with SemaphoreSlim:");
            await demo.RateLimitedOperationsAsync();

            // Example 6: Parallel async processing
            Console.WriteLine("\n6. Parallel async processing:");
            var results = await demo.ParallelProcessingAsync(new[] { 1, 2, 3, 4, 5 });
            Console.WriteLine($"   Results: {string.Join(", ", results)}");

            // Example 7: Async reader-writer lock
            Console.WriteLine("\n7. Async reader-writer pattern:");
            await demo.ReadWriteOperationsAsync();

            // Example 8: Structured async logging
            Console.WriteLine("\n8. Async logging:");
            await demo.AsyncLoggingDemoAsync();

            Console.WriteLine("\n=== End of Demo ===");
        }

        public async Task<int> ProcessPipelineAsync(IEnumerable<string> items)
        {
            var processed = 0;
            foreach (var item in items)
            {
                var step1 = await TransformAsync(item);
                var step2 = await ValidateAsync(step1);
                var step3 = await SaveAsync(step2);
                processed++;
            }
            return processed;
        }

        private async Task<string> TransformAsync(string input)
        {
            await Task.Delay(10);
            return input.ToUpper();
        }

        private async Task<string> ValidateAsync(string input)
        {
            await Task.Delay(10);
            return input.Length > 0 ? input : throw new Exception("Invalid");
        }

        private async Task<bool> SaveAsync(string input)
        {
            await Task.Delay(10);
            return true;
        }

        public async Task ProcessBatchAsync(IEnumerable<string> items)
        {
            const int batchSize = 5;
            var batch = new List<string>();
            
            foreach (var item in items)
            {
                batch.Add(item);
                if (batch.Count >= batchSize)
                {
                    await ProcessBatch(batch);
                    batch.Clear();
                }
            }
            
            if (batch.Count > 0)
                await ProcessBatch(batch);
        }

        private async Task ProcessBatch(List<string> batch)
        {
            Console.WriteLine($"   Processing batch of {batch.Count} items");
            await Task.Delay(50);
        }

        public async Task AsyncLazyInitializationAsync()
        {
            var service = new AsyncLazyService();
            var result1 = await service.GetDataAsync();
            var result2 = await service.GetDataAsync(); // Should be cached
            Console.WriteLine($"   Data: {result1}, Cached: {result1 == result2}");
        }

        public async Task ProducerConsumerAsync()
        {
            var queue = new System.Collections.Concurrent.ConcurrentQueue<string>();
            var producer = Task.Run(async () =>
            {
                for (int i = 0; i < 5; i++)
                {
                    queue.Enqueue($"Item{i}");
                    await Task.Delay(50);
                }
            });

            var consumer = Task.Run(async () =>
            {
                while (!producer.IsCompleted || queue.Count > 0)
                {
                    if (queue.TryDequeue(out var item))
                        Console.WriteLine($"   Consumed: {item}");
                    else
                        await Task.Delay(10);
                }
            });

            await Task.WhenAll(producer, consumer);
        }

        public async Task RateLimitedOperationsAsync()
        {
            var semaphore = new SemaphoreSlim(3); // Max 3 concurrent
            var tasks = Enumerable.Range(1, 10).Select(async i =>
            {
                await semaphore.WaitAsync();
                try
                {
                    await Task.Delay(50);
                    Console.WriteLine($"   Task {i} processed");
                }
                finally
                {
                    semaphore.Release();
                }
            });

            await Task.WhenAll(tasks);
        }

        public async Task<List<int>> ParallelProcessingAsync(IEnumerable<int> items)
        {
            var tasks = items.Select(async i =>
            {
                await Task.Delay(30);
                return i * 2;
            });
            
            return await Task.WhenAll(tasks).ToList();
        }

        private readonly SemaphoreSlim _readWriteLock = new SemaphoreSlim(1, 1);
        private int _readCount = 0;

        public async Task ReadWriteOperationsAsync()
        {
            var readTasks = Enumerable.Range(1, 3).Select(_ => ReadAsync());
            var writeTask = WriteAsync();
            
            await Task.WhenAll(readTasks.Concat(new[] { writeTask }));
        }

        private async Task ReadAsync()
        {
            while (!await TryAcquireReadAsync())
                await Task.Delay(10);
            
            Console.WriteLine($"   Reading... (readers: {Interlocked.Increment(ref _readCount)})");
            await Task.Delay(50);
            Interlocked.Decrement(ref _readCount);
        }

        private async Task<bool> TryAcquireReadAsync()
        {
            await _readWriteLock.WaitAsync();
            try
            {
                return true;
            }
            finally
            {
                _readWriteLock.Release();
            }
        }

        private async Task WriteAsync()
        {
            await _readWriteLock.WaitAsync();
            try
            {
                Console.WriteLine("   Writing...");
                await Task.Delay(50);
            }
            finally
            {
                _readWriteLock.Release();
            }
        }

        public async Task AsyncLoggingDemoAsync()
        {
            var logger = new AsyncLogger();
            await logger.LogAsync("Info", "Application started");
            await logger.LogAsync("Warning", "Low memory");
            await logger.LogAsync("Error", "Connection failed");
            Console.WriteLine("   Logs written asynchronously");
        }
    }

    public class AsyncLazyService
    {
        private Task<string> _cachedTask;
        
        public async Task<string> GetDataAsync()
        {
            _cachedTask ??= LoadDataAsync();
            return await _cachedTask;
        }

        private async Task<string> LoadDataAsync()
        {
            await Task.Delay(100);
            return "Lazy Loaded Data";
        }
    }

    public class AsyncLogger
    {
        private readonly string _logFile = "app.log";
        
        public async Task LogAsync(string level, string message)
        {
            var logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {message}";
            await File.AppendAllTextAsync(_logFile, logEntry + Environment.NewLine);
        }
    }

    // Additional patterns

    public class AsyncCache<T>
    {
        private readonly Dictionary<string, Task<T>> _cache = new();
        private readonly SemaphoreSlim _lock = new(1, 1);

        public async Task<T> GetOrAddAsync(string key, Func<Task<T>> factory)
        {
            if (_cache.TryGetValue(key, out var existingTask))
                return await existingTask;

            await _lock.WaitAsync();
            try
            {
                if (_cache.TryGetValue(key, out existingTask))
                    return await existingTask;

                var task = factory();
                _cache[key] = task;
                return await task;
            }
            finally
            {
                _lock.Release();
            }
        }
    }

    public class ResilientOperation
    {
        public async Task<T> ExecuteAsync<T>(Func<Task<T>> operation, int retries = 3)
        {
            for (int i = 0; i < retries; i++)
            {
                try
                {
                    return await operation();
                }
                catch when (i < retries - 1)
                {
                    await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, i)));
                }
            }
            throw new Exception("All retries failed");
        }
    }
}