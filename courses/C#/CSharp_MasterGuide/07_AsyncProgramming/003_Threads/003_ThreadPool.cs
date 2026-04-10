/*
 * TOPIC: Threading Fundamentals
 * SUBTOPIC: ThreadPool
 * FILE: 03_ThreadPool.cs
 * PURPOSE: Understanding ThreadPool basics for efficient thread management
 */
using System;
using System.Threading;

namespace CSharp_MasterGuide._07_AsyncProgramming._03_Threads
{
    public class ThreadPoolDemo
    {
        public static void Main()
        {
            Console.WriteLine("=== ThreadPool Basics Demo ===\n");

            var demo = new ThreadPoolDemo();

            // Example 1: Queue work item
            Console.WriteLine("1. QueueUserWorkItem:");
            demo.QueueWorkItemDemo();

            // Example 2: ThreadPool operations
            Console.WriteLine("\n2. ThreadPool operations:");
            demo.ThreadPoolOperationsDemo();

            // Example 3: ThreadPool vs dedicated thread
            Console.WriteLine("\n3. ThreadPool vs dedicated thread:");
            demo.ComparisonDemo();

            // Example 4: ThreadPool configuration
            Console.WriteLine("\n4. ThreadPool configuration:");
            demo.ConfigurationDemo();

            // Example 5: ThreadPool callbacks
            Console.WriteLine("\n5. ThreadPool callbacks:");
            demo.CallbackDemo();

            // Example 6: Task uses ThreadPool
            Console.WriteLine("\n6. Task uses ThreadPool:");
            demo.TaskThreadPoolDemo();

            Console.WriteLine("\n=== End of Demo ===");
        }

        public void QueueWorkItemDemo()
        {
            ThreadPool.QueueUserWorkItem(state =>
            {
                Console.WriteLine($"   WorkItem executed with state: {state}");
                Thread.Sleep(50);
            }, "TestState");

            Thread.Sleep(100);
        }

        public void ThreadPoolOperationsDemo()
        {
            int workerThreads, ioThreads;
            ThreadPool.GetMinThreads(out workerThreads, out ioThreads);
            Console.WriteLine($"   Min: {workerThreads} workers, {ioThreads} I/O");

            ThreadPool.GetMaxThreads(out workerThreads, out ioThreads);
            Console.WriteLine($"   Max: {workerThreads} workers, {ioThreads} I/O");

            ThreadPool.GetAvailableThreads(out workerThreads, out ioThreads);
            Console.WriteLine($"   Available: {workerThreads} workers, {ioThreads} I/O");
        }

        public void ComparisonDemo()
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();

            // ThreadPool - fast to start
            for (int i = 0; i < 5; i++)
            {
                ThreadPool.QueueUserWorkItem(_ =>
                {
                    Thread.Sleep(10);
                });
            }

            Thread.Sleep(50);
            sw.Stop();
            Console.WriteLine($"   ThreadPool time: {sw.ElapsedMilliseconds}ms");

            // Dedicated threads - slower to create
            sw.Restart();
            var threads = new Thread[5];
            for (int i = 0; i < 5; i++)
            {
                threads[i] = new Thread(() => Thread.Sleep(10));
                threads[i].Start();
            }
            foreach (var t in threads) t.Join();

            sw.Stop();
            Console.WriteLine($"   Dedicated threads time: {sw.ElapsedMilliseconds}ms");
        }

        public void ConfigurationDemo()
        {
            // Set minimum threads for better initial performance
            ThreadPool.SetMinThreads(4, 4);
            Console.WriteLine("   Set min threads to 4");

            // Query current settings
            int workers, io;
            ThreadPool.GetMinThreads(out workers, out io);
            Console.WriteLine($"   Current min: {workers} workers, {io} I/O");
        }

        public void CallbackDemo()
        {
            ThreadPool.QueueUserWorkItem(state =>
            {
                Console.WriteLine($"   Callback 1: {Thread.CurrentThread.IsThreadPoolThread}");
                Thread.Sleep(30);
            });

            ThreadPool.QueueUserWorkItem(state =>
            {
                Console.WriteLine($"   Callback 2: {Thread.CurrentThread.IsThreadPoolThread}");
                Thread.Sleep(30);
            });

            Thread.Sleep(100);
        }

        public void TaskThreadPoolDemo()
        {
            var task = Task.Run(() =>
            {
                Console.WriteLine($"   Task running on pool: {Thread.CurrentThread.IsThreadPoolThread}");
                return 42;
            });

            Console.WriteLine($"   Task result: {task.Result}");
        }
    }

    // Real-world ThreadPool usage
    public class AsyncProcessor
    {
        public void ProcessAsync(Action callback)
        {
            ThreadPool.QueueUserWorkItem(_ =>
            {
                // Do work
                Thread.Sleep(50);
                
                // Callback on thread pool
                callback?.Invoke();
            });
        }

        public void ProcessWithState(object state)
        {
            var data = state as WorkData;
            ThreadPool.QueueUserWorkItem(_ =>
            {
                if (data != null)
                {
                    Console.WriteLine($"   Processing: {data.Name}");
                    Thread.Sleep(30);
                }
            }, new WorkData { Name = "TestWork" });
        }

        public void ProcessWithTimeout(Action action, int timeoutMs)
        {
            var completed = false;
            
            ThreadPool.QueueUserWorkItem(_ =>
            {
                action();
                completed = true;
            });

            Thread.Sleep(timeoutMs);
            if (!completed)
            {
                Console.WriteLine("   Timeout!");
            }
        }
    }

    public class WorkData
    {
        public string Name { get; set; }
        public object Payload { get; set; }
    }

    public class BatchProcessor
    {
        private readonly int _maxConcurrency;
        private readonly System.Collections.Concurrent.ConcurrentQueue<_workItem> _queue = new();
        private int _running;

        public BatchProcessor(int maxConcurrency)
        {
            _maxConcurrency = maxConcurrency;
        }

        public void AddWork(Action work)
        {
            _queue.Enqueue(new _workItem { Work = work });
            TryProcess();
        }

        private void TryProcess()
        {
            while (_running < _maxConcurrency && _queue.TryDequeue(out var item))
            {
                Interlocked.Increment(ref _running);
                ThreadPool.QueueUserWorkItem(_ =>
                {
                    try
                    {
                        item.Work();
                    }
                    finally
                    {
                        Interlocked.Decrement(ref _running);
                        TryProcess();
                    }
                });
            }
        }

        private struct _workItem
        {
            public Action Work { get; set; }
        }
    }
}