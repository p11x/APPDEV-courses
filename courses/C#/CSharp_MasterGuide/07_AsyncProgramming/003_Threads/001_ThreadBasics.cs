/*
 * TOPIC: Threading Fundamentals
 * SUBTOPIC: Thread Basics
 * FILE: 01_ThreadBasics.cs
 * PURPOSE: Understanding Thread creation, Start, and Join operations
 */
using System;
using System.Threading;

namespace CSharp_MasterGuide._07_AsyncProgramming._03_Threads
{
    public class ThreadBasics
    {
        public static void Main()
        {
            Console.WriteLine("=== Thread Basics Demo ===\n");

            var demo = new ThreadBasics();

            // Example 1: Basic thread creation
            Console.WriteLine("1. Basic thread creation:");
            demo.BasicThreadCreation();

            // Example 2: Thread with parameter (ParameterizedThreadStart)
            Console.WriteLine("\n2. Thread with parameter:");
            demo.ThreadWithParameter();

            // Example 3: Thread.Join
            Console.WriteLine("\n3. Thread.Join:");
            demo.ThreadJoinDemo();

            // Example 4: Thread name and priority
            Console.WriteLine("\n4. Thread name and priority:");
            demo.ThreadPropertiesDemo();

            // Example 5: Background thread
            Console.WriteLine("\n5. Background thread:");
            demo.BackgroundThreadDemo();

            // Example 6: Multiple threads
            Console.WriteLine("\n6. Multiple threads:");
            demo.MultipleThreadsDemo();

            // Example 7: Thread abort (deprecated but demonstrated)
            Console.WriteLine("\n7. Thread state exploration:");
            demo.ThreadStateDemo();

            Console.WriteLine("\n=== End of Demo ===");
        }

        public void BasicThreadCreation()
        {
            var thread = new Thread(() =>
            {
                Console.WriteLine($"   Thread {Thread.CurrentThread.ManagedThreadId} running");
                Thread.Sleep(100);
                Console.WriteLine($"   Thread {Thread.CurrentThread.ManagedThreadId} completed");
            });

            Console.WriteLine($"   Main thread: {Thread.CurrentThread.ManagedThreadId}");
            thread.Start();
            thread.Join();
            Console.WriteLine("   Main thread resumed");
        }

        public void ThreadWithParameter()
        {
            var thread = new Thread((parameter) =>
            {
                string msg = parameter as string;
                Console.WriteLine($"   Received: {msg}");
                Thread.Sleep(50);
            });

            thread.Start("Hello from main thread");
            thread.Join();
        }

        public void ThreadJoinDemo()
        {
            var t1 = new Thread(() =>
            {
                Console.WriteLine("   T1 starting");
                Thread.Sleep(100);
                Console.WriteLine("   T1 done");
            });

            var t2 = new Thread(() =>
            {
                Console.WriteLine("   T2 starting");
                Thread.Sleep(50);
                Console.WriteLine("   T2 done");
            });

            t1.Start();
            t2.Start();
            
            Console.WriteLine("   Waiting for T1...");
            t1.Join();
            Console.WriteLine("   T1 finished, waiting for T2...");
            t2.Join();
            Console.WriteLine("   All threads completed");
        }

        public void ThreadPropertiesDemo()
        {
            var thread = new Thread(() =>
            {
                Console.WriteLine($"   Thread name: {Thread.CurrentThread.Name}");
                Console.WriteLine($"   Is background: {Thread.CurrentThread.IsBackground}");
                Console.WriteLine($"   Priority: {Thread.CurrentThread.Priority}");
            });

            thread.Name = "DemoThread";
            thread.Priority = ThreadPriority.AboveNormal;
            thread.Start();
            thread.Join();
        }

        public void BackgroundThreadDemo()
        {
            var foreground = new Thread(() =>
            {
                for (int i = 0; i < 5; i++)
                {
                    Console.WriteLine($"   Foreground: {i}");
                    Thread.Sleep(30);
                }
            });
            foreground.Name = "Foreground";

            var background = new Thread(() =>
            {
                for (int i = 0; i < 50; i++)
                {
                    Console.WriteLine($"   Background: {i}");
                    Thread.Sleep(10);
                }
            });
            background.IsBackground = true;
            background.Name = "Background";

            foreground.Start();
            background.Start();
            
            foreground.Join();
            Console.WriteLine("   Foreground done, app can exit");
            // Background thread will be terminated when main exits
        }

        public void MultipleThreadsDemo()
        {
            var threads = new Thread[5];
            for (int i = 0; i < 5; i++)
            {
                int id = i;
                threads[i] = new Thread(() =>
                {
                    Console.WriteLine($"   Thread {id} started");
                    Thread.Sleep(50);
                    Console.WriteLine($"   Thread {id} finished");
                });
            }

            foreach (var t in threads) t.Start();
            foreach (var t in threads) t.Join();
            Console.WriteLine("   All threads done");
        }

        public void ThreadStateDemo()
        {
            var thread = new Thread(() => Thread.Sleep(50));
            
            Console.WriteLine($"   Before start: {thread.ThreadState}");
            thread.Start();
            Console.WriteLine($"   After start: {thread.ThreadState}");
            
            thread.Join();
            Console.WriteLine($"   After join: {thread.ThreadState}");
        }
    }

    // Real-world examples
    public class BackgroundTaskRunner
    {
        public void RunInBackground(Action work)
        {
            var thread = new Thread(() =>
            {
                try
                {
                    work();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"   Error: {ex.Message}");
                }
            })
            { IsBackground = true, Name = "BackgroundWorker" };
            
            thread.Start();
        }

        public Thread StartLongRunningTask(Action<long> progressCallback)
        {
            var thread = new Thread(() =>
            {
                for (long i = 0; i < 1000000; i++)
                {
                    if (i % 100000 == 0)
                        progressCallback(i);
                    Thread.Sleep(1);
                }
            })
            { IsBackground = true, Name = "LongRunning" };

            thread.Start();
            return thread;
        }
    }

    public class WorkerThreadPool
    {
        private readonly Thread[] _workers;
        private readonly System.Collections.Concurrent.ConcurrentQueue<Action> _queue = new();
        private bool _running = true;

        public WorkerThreadPool(int workerCount)
        {
            _workers = new Thread[workerCount];
            for (int i = 0; i < workerCount; i++)
            {
                _workers[i] = new Thread(WorkerLoop)
                {
                    Name = $"Worker-{i}",
                    IsBackground = true
                };
                _workers[i].Start();
            }
        }

        public void Enqueue(Action work) => _queue.Enqueue(work);

        public void Shutdown()
        {
            _running = false;
            foreach (var w in _workers) w.Join();
        }

        private void WorkerLoop()
        {
            while (_running)
            {
                if (_queue.TryDequeue(out var work))
                    work();
                else
                    Thread.Sleep(10);
            }
        }
    }
}