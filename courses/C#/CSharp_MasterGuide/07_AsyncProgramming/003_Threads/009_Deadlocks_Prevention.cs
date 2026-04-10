/*
 * TOPIC: Threading Fundamentals
 * SUBTOPIC: Deadlocks Prevention
 * FILE: 09_Deadlocks_Prevention.cs
 * PURPOSE: Understanding and avoiding deadlock patterns in multithreaded code
 */
using System;
using System.Threading;

namespace CSharp_MasterGuide._07_AsyncProgramming._03_Threads
{
    public class DeadlocksPrevention
    {
        public static void Main()
        {
            Console.WriteLine("=== Deadlocks Prevention Demo ===\n");

            var demo = new DeadlocksPrevention();

            // Example 1: Lock ordering deadlock
            Console.WriteLine("1. Lock ordering deadlock (demonstration):");
            demo.LockOrderingDeadlockDemo();

            // Example 2: Prevention with consistent ordering
            Console.WriteLine("\n2. Consistent lock ordering:");
            demo.PreventionWithOrdering();

            // Example 3: Timeout to avoid deadlock
            Console.WriteLine("\n3. Timeout prevention:");
            demo.TimeoutPrevention();

            // Example 4: Avoiding lock during async wait
            Console.WriteLine("\n4. Async/lock deadlock:");
            demo.AsyncLockDeadlockDemo();

            // Example 5: Using async-friendly primitives
            Console.WriteLine("\n5. Async-friendly primitives:");
            demo.AsyncFriendlyPrimitives();

            // Example 6: Lock-free approach
            Console.WriteLine("\n6. Lock-free approach:");
            demo.LockFreeApproach();

            // Example 7: Coarse-grained locking
            Console.WriteLine("\n7. Coarse-grained locking:");
            demo.CoarseGrainedLocking();

            Console.WriteLine("\n=== End of Demo ===");
        }

        private readonly object _lock1 = new();
        private readonly object _lock2 = new();

        public void LockOrderingDeadlockDemo()
        {
            // Thread 1: lock1 then lock2
            var t1 = new Thread(() =>
            {
                lock (_lock1)
                {
                    Console.WriteLine("   T1: acquired lock1");
                    Thread.Sleep(50);
                    Console.WriteLine("   T1: waiting for lock2");
                    lock (_lock2)
                    {
                        Console.WriteLine("   T1: acquired lock2");
                    }
                }
            });

            // Thread 2: lock2 then lock1 (opposite order!)
            var t2 = new Thread(() =>
            {
                lock (_lock2)
                {
                    Console.WriteLine("   T2: acquired lock2");
                    Thread.Sleep(50);
                    Console.WriteLine("   T2: waiting for lock1");
                    lock (_lock1)
                    {
                        Console.WriteLine("   T2: acquired lock1");
                    }
                }
            });

            t1.Start();
            t2.Start();
            
            // In real scenario, this could deadlock - we're using timeout to prevent
            t1.Join(500);
            t2.Join(500);
            Console.WriteLine("   Demo completed (potential deadlock avoided by timing)");
        }

        public void PreventionWithOrdering()
        {
            // Consistent ordering: always lock1 then lock2
            var t1 = new Thread(() =>
            {
                lock (_lock1)
                {
                    Console.WriteLine("   T1: lock1");
                    Thread.Sleep(50);
                    lock (_lock2)
                    {
                        Console.WriteLine("   T1: lock2");
                    }
                }
            });

            var t2 = new Thread(() =>
            {
                // Same order - no deadlock possible
                lock (_lock1)
                {
                    Console.WriteLine("   T2: lock1");
                    Thread.Sleep(50);
                    lock (_lock2)
                    {
                        Console.WriteLine("   T2: lock2");
                    }
                }
            });

            t1.Start();
            t2.Start();
            t1.Join();
            t2.Join();
            Console.WriteLine("   No deadlock with consistent ordering");
        }

        public void TimeoutPrevention()
        {
            var lock1 = new object();
            var lock2 = new object();
            bool completed = false;

            var t1 = new Thread(() =>
            {
                if (Monitor.TryEnter(lock1, TimeSpan.FromMilliseconds(100)))
                {
                    try
                    {
                        Console.WriteLine("   T1: got lock1");
                        Thread.Sleep(50);
                        if (Monitor.TryEnter(lock2, TimeSpan.FromMilliseconds(100)))
                        {
                            try
                            {
                                Console.WriteLine("   T1: got lock2");
                            }
                            finally
                            {
                                Monitor.Exit(lock2);
                            }
                        }
                        else
                        {
                            Console.WriteLine("   T1: timeout on lock2");
                        }
                    }
                    finally
                    {
                        Monitor.Exit(lock1);
                    }
                }
            });

            var t2 = new Thread(() =>
            {
                if (Monitor.TryEnter(lock2, TimeSpan.FromMilliseconds(100)))
                {
                    try
                    {
                        Console.WriteLine("   T2: got lock2");
                        Thread.Sleep(50);
                        if (Monitor.TryEnter(lock1, TimeSpan.FromMilliseconds(100)))
                        {
                            try
                            {
                                Console.WriteLine("   T2: got lock1");
                            }
                            finally
                            {
                                Monitor.Exit(lock1);
                            }
                        }
                        else
                        {
                            Console.WriteLine("   T2: timeout on lock1");
                        }
                    }
                    finally
                    {
                        Monitor.Exit(lock2);
                    }
                }
            });

            t1.Start();
            t2.Start();
            t1.Join();
            t2.Join();
            Console.WriteLine("   Timeout prevention completed");
        }

        private readonly SemaphoreSlim _asyncLock = new(1);

        public async void AsyncLockDeadlockDemo()
        {
            // BAD: Holding lock during await
            lock (_lock1)
            {
                // This can deadlock with synchronous callers
                Thread.Sleep(50);
            }

            // GOOD: Async-friendly lock
            await _asyncLock.WaitAsync();
            try
            {
                await System.Threading.Tasks.Task.Delay(50);
            }
            finally
            {
                _asyncLock.Release();
            }
        }

        public void AsyncFriendlyPrimitives()
        {
            // Use ReaderWriterLockSlim for read-heavy workloads
            var rwLock = new ReaderWriterLockSlim();
            
            var threads = new Thread[4];
            for (int i = 0; i < 4; i++)
            {
                int id = i;
                threads[i] = new Thread(() =>
                {
                    if (id % 2 == 0)
                    {
                        rwLock.EnterReadLock();
                        Thread.Sleep(30);
                        rwLock.ExitReadLock();
                    }
                    else
                    {
                        rwLock.EnterWriteLock();
                        Thread.Sleep(30);
                        rwLock.ExitWriteLock();
                    }
                });
            }

            foreach (var t in threads) t.Start();
            foreach (var t in threads) t.Join();
            Console.WriteLine("   ReaderWriterLockSlim completed");
        }

        public void LockFreeApproach()
        {
            // Use Interlocked for simple operations
            long counter = 0;
            var threads = new Thread[4];
            
            for (int i = 0; i < 4; i++)
            {
                threads[i] = new Thread(() =>
                {
                    for (int j = 0; j < 10000; j++)
                    {
                        Interlocked.Increment(ref counter);
                    }
                });
            }

            foreach (var t in threads) t.Start();
            foreach (var t in threads) t.Join();
            Console.WriteLine($"   Lock-free counter: {counter}");
        }

        private int _sharedValue = 0;

        public void CoarseGrainedLocking()
        {
            // Single lock is simpler and avoids deadlocks
            var lock_ = new object();
            var threads = new Thread[4];

            for (int i = 0; i < 4; i++)
            {
                threads[i] = new Thread(() =>
                {
                    for (int j = 0; j < 1000; j++)
                    {
                        lock (lock_)
                        {
                            _sharedValue++;
                        }
                    }
                });
            }

            foreach (var t in threads) t.Start();
            foreach (var t in threads) t.Join();
            Console.WriteLine($"   Coarse-grained result: {_sharedValue}");
        }
    }

    // Best practices and real-world patterns
    public class DeadlockSafeService
    {
        private readonly object _stateLock = new();
        private readonly SemaphoreSlim _asyncLock = new(1);

        // Always acquire locks in same order
        private readonly object[] _orderedLocks = new object[2];

        public DeadlockSafeService()
        {
            _orderedLocks[0] = new object();
            _orderedLocks[1] = new object();
        }

        public async System.Threading.Tasks.Task Operation1Async()
        {
            // Use async-friendly primitives
            await _asyncLock.WaitAsync();
            try
            {
                await System.Threading.Tasks.Task.Delay(50);
            }
            finally
            {
                _asyncLock.Release();
            }
        }

        public void SynchronizedOperation()
        {
            lock (_orderedLocks[0])
            {
                Thread.Sleep(20);
            }
        }

        public bool TryAcquireLock(TimeSpan timeout)
        {
            return Monitor.TryEnter(_orderedLocks[0], timeout);
        }
    }

    public class AsyncSemaphoreExample
    {
        private readonly SemaphoreSlim _semaphore = new(1);
        private int _value;

        public async System.Threading.Tasks.Task<int> GetValueAsync()
        {
            await _semaphore.WaitAsync();
            try
            {
                await System.Threading.Tasks.Task.Delay(10);
                return _value;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async System.Threading.Tasks.Task SetValueAsync(int value)
        {
            await _semaphore.WaitAsync();
            try
            {
                await System.Threading.Tasks.Task.Delay(10);
                _value = value;
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}