/*
 * TOPIC: Threading Fundamentals
 * SUBTOPIC: ThreadPool Part 2
 * FILE: 04_ThreadPool_Part2.cs
 * PURPOSE: Advanced ThreadPool patterns and async programming with ThreadPool
 */
using System;
using System.Threading;

namespace CSharp_MasterGuide._07_AsyncProgramming._03_Threads
{
    public class ThreadPoolPart2
    {
        public static void Main()
        {
            Console.WriteLine("=== ThreadPool Part 2 Demo ===\n");

            var demo = new ThreadPoolPart2();

            // Example 1: ThreadPool with async/await
            Console.WriteLine("1. ThreadPool with async/await:");
            demo.AsyncAwaitThreadPoolDemo();

            // Example 2: Blocking the ThreadPool (avoid!)
            Console.WriteLine("\n2. Blocking the ThreadPool (danger):");
            demo.BlockingDemo();

            // Example 3: I/O completion ports
            Console.WriteLine("\n3. I/O completion ports:");
            demo.IoCompletionDemo();

            // Example 4: ThreadPool thread starvation handling
            Console.WriteLine("\n4. Thread starvation handling:");
            demo.StarvationDemo();

            // Example 5: Custom ThreadPool (alternative)
            Console.WriteLine("\n5. Custom work queue:");
            demo.CustomWorkQueueDemo();

            // Example 6: ThreadPool metrics
            Console.WriteLine("\n6. ThreadPool metrics:");
            demo.MetricsDemo();

            Console.WriteLine("\n=== End of Demo ===");
        }

        public async void AsyncAwaitThreadPoolDemo()
        {
            await System.Threading.Tasks.Task.Run(() =>
            {
                Console.WriteLine($"   Running on pool: {Thread.CurrentThread.IsThreadPoolThread}");
            });
        }

        public void BlockingDemo()
        {
            // This is BAD - blocks a thread pool thread
            // ThreadPool.QueueUserWorkItem(_ => Thread.Sleep(1000));
            
            // Better - use dedicated thread for blocking work
            var thread = new Thread(() =>
            {
                Console.WriteLine("   Dedicated thread blocking");
                Thread.Sleep(100);
            });
            thread.Start();
            thread.Join();
            Console.WriteLine("   Done");
        }

        public void IoCompletionDemo()
        {
            // Register wait handle (uses I/O completion ports)
            var evt = new ManualResetEvent(false);
            
            ThreadPool.RegisterWaitForSingleObject(evt, (state, timedOut) =>
            {
                Console.WriteLine($"   Wait completed, timed out: {timedOut}");
            }, null, 1000, true);

            Thread.Sleep(1500);
        }

        public void StarvationDemo()
        {
            // Avoid creating too many concurrent blocking operations
            // Instead, limit concurrency
            var semaphore = new SemaphoreSlim(4);

            for (int i = 0; i < 20; i++)
            {
                int id = i;
                ThreadPool.QueueUserWorkItem(_ =>
                {
                    semaphore.Wait();
                    try
                    {
                        Thread.Sleep(30);
                        Console.WriteLine($"   Task {id} done");
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                });
            }

            Thread.Sleep(500);
        }

        public void CustomWorkQueueDemo()
        {
            var queue = new SimpleWorkQueue(4);
            
            for (int i = 0; i < 10; i++)
            {
                int id = i;
                queue.Enqueue(() => Console.WriteLine($"   Work {id}"));
            }

            Thread.Sleep(300);
        }

        public void MetricsDemo()
        {
            int workerThreads, ioThreads;
            ThreadPool.GetAvailableThreads(out workerThreads, out ioThreads);
            ThreadPool.GetMaxThreads(out int maxWorker, out int maxIo);
            
            Console.WriteLine($"   Available workers: {workerThreads}");
            Console.WriteLine($"   Max workers: {maxWorker}");
            Console.WriteLine($"   Available I/O: {ioThreads}");
        }
    }

    public class SimpleWorkQueue
    {
        private readonly Thread[] _workers;
        private readonly System.Collections.Concurrent.ConcurrentQueue<Action> _queue = new();

        public SimpleWorkQueue(int workerCount)
        {
            _workers = new Thread[workerCount];
            for (int i = 0; i < workerCount; i++)
            {
                _workers[i] = new Thread(ProcessLoop)
                {
                    IsBackground = true,
                    Name = $"Worker-{i}"
                };
                _workers[i].Start();
            }
        }

        public void Enqueue(Action work) => _queue.Enqueue(work);

        private void ProcessLoop()
        {
            while (true)
            {
                if (_queue.TryDequeue(out var work))
                    work();
                else
                    Thread.SpinWait(100);
            }
        }
    }

    // Real-world patterns
    public class ParallelProcessingWithThreadPool
    {
        public void ProcessInParallel<T>(IEnumerable<T> items, Action<T> process)
        {
            var semaphore = new SemaphoreSlim(Environment.ProcessorCount);
            
            foreach (var item in items)
            {
                semaphore.Wait();
                ThreadPool.QueueUserWorkItem(_ =>
                {
                    try
                    {
                        process(item);
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                });
            }
        }

        public void ProcessWithCompletion(IEnumerable<Action> workItems)
        {
            var remaining = workItems.Count();
            var lock_ = new object();

            foreach (var work in workItems)
            {
                ThreadPool.QueueUserWorkItem(_ =>
                {
                    work();
                    lock (lock_)
                    {
                        remaining--;
                        if (remaining == 0)
                            Console.WriteLine("   All done!");
                    }
                });
            }
        }
    }

    public class ThreadPoolWarmUp
    {
        public void WarmUp()
        {
            // Pre-warm the thread pool
            ThreadPool.SetMinThreads(10, 10);
            
            // Run some dummy work to initialize threads
            for (int i = 0; i < 10; i++)
            {
                ThreadPool.QueueUserWorkItem(_ => { });
            }
        }
    }
}