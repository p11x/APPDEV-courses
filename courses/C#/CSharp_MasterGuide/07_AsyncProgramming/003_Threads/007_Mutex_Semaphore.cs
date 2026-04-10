/*
 * TOPIC: Threading Fundamentals
 * SUBTOPIC: Mutex and Semaphore
 * FILE: 07_Mutex_Semaphore.cs
 * PURPOSE: Understanding Mutex and Semaphore for process and thread synchronization
 */
using System;
using System.Threading;

namespace CSharp_MasterGuide._07_AsyncProgramming._03_Threads
{
    public class MutexSemaphoreDemo
    {
        public static void Main()
        {
            Console.WriteLine("=== Mutex and Semaphore Demo ===\n");

            var demo = new MutexSemaphoreDemo();

            // Example 1: Basic Mutex
            Console.WriteLine("1. Basic Mutex:");
            demo.BasicMutexDemo();

            // Example 2: Semaphore basic
            Console.WriteLine("\n2. Basic Semaphore:");
            demo.BasicSemaphoreDemo();

            // Example 3: SemaphoreSlim (async-friendly)
            Console.WriteLine("\n3. SemaphoreSlim:");
            demo.SemaphoreSlimDemo();

            // Example 4: Named Mutex for inter-process
            Console.WriteLine("\n4. Named Mutex:");
            demo.NamedMutexDemo();

            // Example 5: Releasing Semaphore
            Console.WriteLine("\n5. Semaphore release:");
            demo.SemaphoreReleaseDemo();

            // Example 6: Counting Semaphore
            Console.WriteLine("\n6. Counting Semaphore:");
            demo.CountingSemaphoreDemo();

            // Example 7: Semaphore with wait handle
            Console.WriteLine("\n7. Semaphore wait handle:");
            demo.SemaphoreWaitHandleDemo();

            Console.WriteLine("\n=== End of Demo ===");
        }

        public void BasicMutexDemo()
        {
            var mutex = new Mutex();
            int counter = 0;
            var threads = new Thread[4];

            for (int i = 0; i < 4; i++)
            {
                threads[i] = new Thread(() =>
                {
                    for (int j = 0; j < 1000; j++)
                    {
                        mutex.WaitOne();
                        try
                        {
                            counter++;
                        }
                        finally
                        {
                            mutex.ReleaseMutex();
                        }
                    }
                });
            }

            foreach (var t in threads) t.Start();
            foreach (var t in threads) t.Join();
            Console.WriteLine($"   Counter: {counter}");
        }

        public void BasicSemaphoreDemo()
        {
            var semaphore = new Semaphore(2, 2); // Initial count 2, max 2
            int completed = 0;

            var threads = new Thread[4];
            for (int i = 0; i < 4; i++)
            {
                int id = i;
                threads[i] = new Thread(() =>
                {
                    Console.WriteLine($"   Thread {id} waiting...");
                    semaphore.WaitOne();
                    try
                    {
                        Console.WriteLine($"   Thread {id} entered");
                        Thread.Sleep(50);
                        Interlocked.Increment(ref completed);
                        Console.WriteLine($"   Thread {id} exiting");
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                });
            }

            foreach (var t in threads) t.Start();
            foreach (var t in threads) t.Join();
            Console.WriteLine($"   Completed: {completed}");
        }

        public void SemaphoreSlimDemo()
        {
            var semaphore = new SemaphoreSlim(2, 2);
            var tasks = new System.Threading.Tasks.Task[4];

            for (int i = 0; i < 4; i++)
            {
                int id = i;
                tasks[i] = System.Threading.Tasks.Task.Run(async () =>
                {
                    Console.WriteLine($"   Task {id} waiting...");
                    await semaphore.WaitAsync();
                    try
                    {
                        Console.WriteLine($"   Task {id} executing");
                        await System.Threading.Tasks.Task.Delay(50);
                    }
                    finally
                    {
                        semaphore.Release();
                        Console.WriteLine($"   Task {id} released");
                    }
                });
            }

            System.Threading.Tasks.Task.WaitAll(tasks);
        }

        public void NamedMutexDemo()
        {
            bool createdNew;
            var mutex = new Mutex(false, "CSharp_MasterGuide_Mutex", out createdNew);

            if (createdNew)
            {
                Console.WriteLine("   Created new mutex");
                // Do work
                Thread.Sleep(100);
                mutex.ReleaseMutex();
            }
            else
            {
                Console.WriteLine("   Mutex already exists");
            }
        }

        public void SemaphoreReleaseDemo()
        {
            var semaphore = new Semaphore(1, 3);
            
            Console.WriteLine($"   Initial count: {semaphore.Release()}"); // Releases 1, returns previous
            Console.WriteLine($"   Current count: {semaphore.Release(2)}"); // Releases 2 more
        }

        public void CountingSemaphoreDemo()
        {
            var semaphore = new Semaphore(0, 5); // Start at 0, max 5
            
            var threads = new Thread[5];
            for (int i = 0; i < 5; i++)
            {
                int id = i;
                threads[i] = new Thread(() =>
                {
                    semaphore.WaitOne();
                    Console.WriteLine($"   Thread {id} acquired");
                    Thread.Sleep(30);
                    semaphore.Release();
                });
            }

            // Release all permits
            semaphore.Release(5);
            
            foreach (var t in threads) t.Start();
            foreach (var t in threads) t.Join();
            Console.WriteLine("   All done");
        }

        public void SemaphoreWaitHandleDemo()
        {
            var semaphore = new Semaphore(1, 1);
            
            var result = semaphore.WaitOne(100); // Wait with timeout
            if (result)
            {
                Console.WriteLine("   Acquired semaphore");
                semaphore.Release();
            }
            else
            {
                Console.WriteLine("   Timeout waiting");
            }
        }
    }

    // Real-world patterns
    public class ResourcePool<T> where T : class
    {
        private readonly SemaphoreSlim _semaphore;
        private readonly T[] _resources;

        public ResourcePool(int size, Func<T> factory)
        {
            _semaphore = new SemaphoreSlim(size, size);
            _resources = new T[size];
            for (int i = 0; i < size; i++)
            {
                _resources[i] = factory();
            }
        }

        public async System.Threading.Tasks.Task<T> AcquireAsync()
        {
            await _semaphore.WaitAsync();
            return _resources[0]; // Simplified - real impl would track available
        }

        public void Release(T resource)
        {
            _semaphore.Release();
        }
    }

    public class ThrottledExecutor
    {
        private readonly SemaphoreSlim _semaphore;

        public ThrottledExecutor(int maxConcurrency)
        {
            _semaphore = new SemaphoreSlim(maxConcurrency);
        }

        public async System.Threading.Tasks.Task ExecuteAsync(Action work)
        {
            await _semaphore.WaitAsync();
            try
            {
                work();
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }

    public class ProcessMutexExample
    {
        private Mutex _mutex;

        public bool TryAcquire(TimeSpan timeout)
        {
            try
            {
                return _mutex.WaitOne(timeout);
            }
            catch
            {
                return false;
            }
        }

        public void Release()
        {
            _mutex?.ReleaseMutex();
        }
    }
}