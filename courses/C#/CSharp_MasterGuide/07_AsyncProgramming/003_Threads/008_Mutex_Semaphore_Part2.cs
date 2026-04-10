/*
 * TOPIC: Threading Fundamentals
 * SUBTOPIC: Mutex and Semaphore Part 2
 * FILE: 08_Mutex_Semaphore_Part2.cs
 * PURPOSE: Advanced Mutex and Semaphore patterns and practices
 */
using System;
using System.Threading;

namespace CSharp_MasterGuide._07_AsyncProgramming._03_Threads
{
    public class MutexSemaphorePart2
    {
        public static void Main()
        {
            Console.WriteLine("=== Mutex and Semaphore Part 2 Demo ===\n");

            var demo = new MutexSemaphorePart2();

            // Example 1: Async Semaphore pattern
            Console.WriteLine("1. Async Semaphore pattern:");
            demo.AsyncSemaphorePattern();

            // Example 2: Semaphore as rate limiter
            Console.WriteLine("\n2. Rate limiter:");
            demo.RateLimiterDemo();

            // Example 3: Mutex with abandon detection
            Console.WriteLine("\n3. Abandoned mutex detection:");
            demo.AbandonedMutexDemo();

            // Example 4: Multiple permit release
            Console.WriteLine("\n4. Multiple permit release:");
            demo.MultiPermitReleaseDemo();

            // Example 5: Semaphore with cancellation
            Console.WriteLine("\n5. Semaphore with cancellation:");
            demo.SemaphoreCancellationDemo();

            // Example 6: Read-write with Semaphore
            Console.WriteLine("\n6. Read-write pattern:");
            demo.ReadWriteSemaphoreDemo();

            Console.WriteLine("\n=== End of Demo ===");
        }

        public async void AsyncSemaphorePattern()
        {
            var semaphore = new SemaphoreSlim(3);
            var tasks = new System.Threading.Tasks.Task[10];

            for (int i = 0; i < 10; i++)
            {
                int id = i;
                tasks[i] = System.Threading.Tasks.Task.Run(async () =>
                {
                    await semaphore.WaitAsync();
                    try
                    {
                        Console.WriteLine($"   Task {id} executing");
                        await System.Threading.Tasks.Task.Delay(30);
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                });
            }

            await System.Threading.Tasks.Task.WhenAll(tasks);
            Console.WriteLine("   All async tasks completed");
        }

        public void RateLimiterDemo()
        {
            var rateLimiter = new RateLimiter(2, TimeSpan.FromMilliseconds(100));
            var results = new System.Collections.Concurrent.ConcurrentBag<int>();
            var threads = new Thread[10];

            for (int i = 0; i < 10; i++)
            {
                int id = i;
                threads[i] = new Thread(() =>
                {
                    if (rateLimiter.TryAcquire())
                    {
                        results.Add(id);
                        Console.WriteLine($"   Request {id} allowed");
                    }
                    else
                    {
                        Console.WriteLine($"   Request {id} rejected");
                    }
                });
            }

            foreach (var t in threads) t.Start();
            foreach (var t in threads) t.Join();
            Console.WriteLine($"   Allowed: {results.Count}");
        }

        public void AbandonedMutexDemo()
        {
            var mutex = new Mutex(false, "TestMutex");
            
            // Simulate abandoned mutex
            ThreadPool.QueueUserWorkItem(_ =>
            {
                mutex.WaitOne();
                Thread.Sleep(50);
                // Without release - abandoned
            });

            Thread.Sleep(100);

            try
            {
                // This may throw AbandonedMutexException
                mutex.WaitOne(100);
                mutex.ReleaseMutex();
            }
            catch (AbandonedMutexException ex)
            {
                Console.WriteLine($"   Abandoned: {ex.Message}");
            }
        }

        public void MultiPermitReleaseDemo()
        {
            var semaphore = new Semaphore(0, 5);
            
            // Acquire 3
            semaphore.WaitOne();
            semaphore.WaitOne();
            semaphore.WaitOne();
            Console.WriteLine($"   Acquired 3, count: 0");
            
            // Release 3 at once
            semaphore.Release(3);
            Console.WriteLine($"   Released 3, count: 3");
        }

        public async void SemaphoreCancellationDemo()
        {
            var semaphore = new SemaphoreSlim(1);
            var cts = new CancellationTokenSource();

            var task = System.Threading.Tasks.Task.Run(async () =>
            {
                await semaphore.WaitAsync(cts.Token);
                try
                {
                    await System.Threading.Tasks.Task.Delay(50);
                }
                finally
                {
                    semaphore.Release();
                }
            });

            cts.CancelAfter(10);
            
            try
            {
                await task;
            }
            catch (System.OperationCanceledException)
            {
                Console.WriteLine("   Cancelled");
            }
        }

        public void ReadWriteSemaphoreDemo()
        {
            var readSemaphore = new Semaphore(1, 1);
            var writeSemaphore = new Semaphore(1, 1);
            int readers = 0;
            object readerLock = new();
            int operations = 0;

            var threads = new Thread[6];
            
            // 4 readers
            for (int i = 0; i < 4; i++)
            {
                int id = i;
                threads[i] = new Thread(() =>
                {
                    lock (readerLock)
                    {
                        if (readers == 0)
                            writeSemaphore.WaitOne();
                        readers++;
                    }
                    
                    // Reading
                    Thread.Sleep(20);
                    Interlocked.Increment(ref operations);
                    
                    lock (readerLock)
                    {
                        readers--;
                        if (readers == 0)
                            writeSemaphore.Release();
                    }
                });
            }

            // 2 writers
            for (int i = 0; i < 2; i++)
            {
                int id = i;
                threads[4 + i] = new Thread(() =>
                {
                    writeSemaphore.WaitOne();
                    // Writing
                    Thread.Sleep(30);
                    Interlocked.Increment(ref operations);
                    writeSemaphore.Release();
                });
            }

            foreach (var t in threads) t.Start();
            foreach (var t in threads) t.Join();
            Console.WriteLine($"   Operations: {operations}");
        }
    }

    public class RateLimiter
    {
        private readonly SemaphoreSlim _semaphore;
        private readonly TimeSpan _interval;
        private DateTime _nextAvailableTime = DateTime.Now;

        public RateLimiter(int maxRequests, TimeSpan interval)
        {
            _semaphore = new SemaphoreSlim(maxRequests);
            _interval = interval;
        }

        public bool TryAcquire()
        {
            if (!_semaphore.Wait(0))
                return false;

            var now = DateTime.Now;
            if (now >= _nextAvailableTime)
            {
                _nextAvailableTime = now.Add(_interval);
                _semaphore.Release();
                return true;
            }

            _semaphore.Release();
            return false;
        }
    }

    // Real-world implementations
    public class AsyncRateLimiter
    {
        private readonly SemaphoreSlim _semaphore;
        private readonly System.Timers.Timer _timer;

        public AsyncRateLimiter(int maxConcurrent)
        {
            _semaphore = new SemaphoreSlim(maxConcurrent);
            _timer = new System.Timers.Timer(100);
            _timer.Elapsed += (s, e) => _semaphore.Release();
            _timer.Start();
        }

        public async System.Threading.Tasks.Task<T> ExecuteAsync<T>(Func<System.Threading.Tasks.Task<T>> operation)
        {
            await _semaphore.WaitAsync();
            try
            {
                return await operation();
            }
            finally
            {
                // Release is handled by timer
            }
        }
    }

    public class DistributedLock
    {
        private Mutex _mutex;
        private readonly string _name;

        public DistributedLock(string name)
        {
            _name = name;
        }

        public bool Acquire(TimeSpan timeout)
        {
            try
            {
                _mutex = new Mutex(false, _name);
                return _mutex.WaitOne(timeout);
            }
            catch
            {
                return false;
            }
        }

        public void Release()
        {
            try
            {
                _mutex?.ReleaseMutex();
                _mutex?.Dispose();
            }
            catch { }
        }
    }

    public class ConnectionPool
    {
        private readonly SemaphoreSlim _semaphore;
        private readonly System.Collections.Concurrent.ConcurrentBag<Connection> _connections;

        public ConnectionPool(int maxSize)
        {
            _semaphore = new SemaphoreSlim(maxSize, maxSize);
            _connections = new System.Collections.Concurrent.ConcurrentBag<Connection>();

            for (int i = 0; i < maxSize; i++)
            {
                _connections.Add(new Connection { Id = i });
            }
        }

        public async System.Threading.Tasks.Task<Connection> AcquireAsync()
        {
            await _semaphore.WaitAsync();
            _connections.TryTake(out var conn);
            return conn;
        }

        public void Release(Connection conn)
        {
            _connections.Add(conn);
            _semaphore.Release();
        }
    }

    public class Connection
    {
        public int Id { get; set; }
    }
}