/*
 * TOPIC: Threading Fundamentals
 * SUBTOPIC: Thread Basics Part 2
 * FILE: 02_ThreadBasics_Part2.cs
 * PURPOSE: Advanced thread operations, sleep, yield, and thread communication
 */
using System;
using System.Threading;

namespace CSharp_MasterGuide._07_AsyncProgramming._03_Threads
{
    public class ThreadBasicsPart2
    {
        public static void Main()
        {
            Console.WriteLine("=== Thread Basics Part 2 Demo ===\n");

            var demo = new ThreadBasicsPart2();

            // Example 1: Thread.Sleep
            Console.WriteLine("1. Thread.Sleep:");
            demo.SleepDemo();

            // Example 2: Thread.Yield
            Console.WriteLine("\n2. Thread.Yield:");
            demo.YieldDemo();

            // Example 3: Thread.SpinWait
            Console.WriteLine("\n3. Thread.SpinWait:");
            demo.SpinWaitDemo();

            // Example 4: Thread local storage
            Console.WriteLine("\n4. Thread local storage:");
            demo.ThreadLocalStorageDemo();

            // Example 5: Thread interruption
            Console.WriteLine("\n5. Thread interruption:");
            demo.InterruptDemo();

            // Example 6: Thread apartment state
            Console.WriteLine("\n6. Thread apartment state:");
            demo.ApartmentStateDemo();

            // Example 7: Suspend and Resume (deprecated)
            Console.WriteLine("\n7. Thread state control:");
            demo.StateControlDemo();

            Console.WriteLine("\n=== End of Demo ===");
        }

        public void SleepDemo()
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();
            Thread.Sleep(100);
            sw.Stop();
            Console.WriteLine($"   Sleep took: {sw.ElapsedMilliseconds}ms");
        }

        public void YieldDemo()
        {
            int counter = 0;
            var threads = new Thread[3];
            
            for (int i = 0; i < 3; i++)
            {
                int id = i;
                threads[i] = new Thread(() =>
                {
                    while (counter < 30)
                    {
                        Interlocked.Increment(ref counter);
                        Console.WriteLine($"   Thread {id}: {counter}");
                        Thread.Yield();
                    }
                });
            }

            foreach (var t in threads) t.Start();
            foreach (var t in threads) t.Join();
        }

        public void SpinWaitDemo()
        {
            bool completed = false;
            var thread = new Thread(() =>
            {
                Thread.SpinWait(1000);
                completed = true;
            });

            thread.Start();
            thread.Join();
            Console.WriteLine($"   SpinWait completed: {completed}");
        }

        public void ThreadLocalStorageDemo()
        {
            var local = new ThreadLocal<int>(() =>
            {
                Console.WriteLine($"   Initialized TLS for thread {Thread.CurrentThread.ManagedThreadId}");
                return Thread.CurrentThread.ManagedThreadId * 10;
            });

            var threads = new Thread[3];
            for (int i = 0; i < 3; i++)
            {
                threads[i] = new Thread(() =>
                {
                    Console.WriteLine($"   Thread {Thread.CurrentThread.ManagedThreadId}: {local.Value}");
                    local.Value = local.Value + 1;
                    Console.WriteLine($"   After increment: {local.Value}");
                });
            }

            foreach (var t in threads) t.Start();
            foreach (var t in threads) t.Join();
        }

        public void InterruptDemo()
        {
            var thread = new Thread(() =>
            {
                try
                {
                    Console.WriteLine("   Thread sleeping...");
                    Thread.Sleep(5000);
                    Console.WriteLine("   Thread woke up normally");
                }
                catch (ThreadInterruptedException)
                {
                    Console.WriteLine("   Thread was interrupted!");
                }
            });

            thread.Start();
            Thread.Sleep(100);
            thread.Interrupt();
            thread.Join();
        }

        public void ApartmentStateDemo()
        {
            var staThread = new Thread(() =>
            {
                Console.WriteLine($"   STA thread: {Thread.CurrentThread.GetApartmentState()}");
            });
            staThread.SetApartmentState(ApartmentState.STA);
            staThread.Start();
            staThread.Join();

            var mtaThread = new Thread(() =>
            {
                Console.WriteLine($"   MTA thread: {Thread.CurrentThread.GetApartmentState()}");
            });
            mtaThread.SetApartmentState(ApartmentState.MTA);
            mtaThread.Start();
            mtaThread.Join();
        }

        public void StateControlDemo()
        {
            var thread = new Thread(() =>
            {
                for (int i = 0; i < 5; i++)
                {
                    Console.WriteLine($"   Working: {i}");
                    Thread.Sleep(50);
                }
            });

            thread.Start();
            Thread.Sleep(50);
            
            // Check if thread is alive
            Console.WriteLine($"   IsAlive: {thread.IsAlive}");
            
            thread.Join();
            Console.WriteLine($"   After join - IsAlive: {thread.IsAlive}");
        }
    }

    // Real-world patterns
    public class ThreadSafeCounter
    {
        private int _count;
        private readonly object _lock = new();

        public void Increment()
        {
            lock (_lock)
            {
                _count++;
            }
        }

        public int Count
        {
            get
            {
                lock (_lock)
                {
                    return _count;
                }
            }
        }
    }

    public class ThreadPoolController
    {
        public void SetMinThreads(int workerThreads, int ioThreads)
        {
            ThreadPool.SetMinThreads(workerThreads, ioThreads);
            Console.WriteLine($"   Min threads set: {workerThreads} workers, {ioThreads} I/O");
        }

        public void SetMaxThreads(int workerThreads, int ioThreads)
        {
            ThreadPool.SetMaxThreads(workerThreads, ioThreads);
            Console.WriteLine($"   Max threads set: {workerThreads} workers, {ioThreads} I/O");
        }

        public void GetAvailableThreads()
        {
            int workerThreads, ioThreads;
            ThreadPool.GetAvailableThreads(out workerThreads, out ioThreads);
            Console.WriteLine($"   Available: {workerThreads} workers, {ioThreads} I/O");
        }
    }

    public class SpinLockExample
    {
        private readonly System.Threading.SpinLock _lock = new();
        private int _value;

        public void Increment()
        {
            bool taken = false;
            try
            {
                _lock.Enter(ref taken);
                _value++;
            }
            finally
            {
                if (taken) _lock.Exit();
            }
        }

        public int Value => _value;
    }
}