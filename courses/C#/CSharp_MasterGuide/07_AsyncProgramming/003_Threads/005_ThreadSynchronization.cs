/*
 * TOPIC: Threading Fundamentals
 * SUBTOPIC: Thread Synchronization
 * FILE: 05_ThreadSynchronization.cs
 * PURPOSE: Understanding lock and Monitor for thread-safe operations
 */
using System;
using System.Threading;

namespace CSharp_MasterGuide._07_AsyncProgramming._03_Threads
{
    public class ThreadSynchronization
    {
        public static void Main()
        {
            Console.WriteLine("=== Thread Synchronization Demo ===\n");

            var demo = new ThreadSynchronization();

            // Example 1: Basic lock
            Console.WriteLine("1. Basic lock:");
            demo.BasicLockDemo();

            // Example 2: Lock with resource
            Console.WriteLine("\n2. Lock with resource:");
            demo.LockWithResourceDemo();

            // Example 3: Lock vs Interlocked
            Console.WriteLine("\n3. Lock vs Interlocked:");
            demo.LockVsInterlockedDemo();

            // Example 4: Nested locks
            Console.WriteLine("\n4. Nested locks:");
            demo.NestedLockDemo();

            // Example 5: Monitor usage
            Console.WriteLine("\n5. Monitor usage:");
            demo.MonitorDemo();

            // Example 6: Producer-consumer with lock
            Console.WriteLine("\n6. Producer-consumer:");
            demo.ProducerConsumerDemo();

            // Example 7: Reader-writer with lock
            Console.WriteLine("\n7. Reader-writer pattern:");
            demo.ReaderWriterDemo();

            Console.WriteLine("\n=== End of Demo ===");
        }

        private readonly object _lock = new();
        private int _counter;

        public void BasicLockDemo()
        {
            var threads = new Thread[5];
            for (int i = 0; i < 5; i++)
            {
                threads[i] = new Thread(() =>
                {
                    for (int j = 0; j < 1000; j++)
                    {
                        lock (_lock)
                        {
                            _counter++;
                        }
                    }
                });
            }

            foreach (var t in threads) t.Start();
            foreach (var t in threads) t.Join();
            Console.WriteLine($"   Counter: {_counter}");
        }

        public void LockWithResourceDemo()
        {
            var account = new BankAccount();
            var threads = new Thread[5];

            for (int i = 0; i < 5; i++)
            {
                threads[i] = new Thread(() =>
                {
                    for (int j = 0; j < 100; j++)
                    {
                        account.Deposit(10);
                        account.Withdraw(5);
                    }
                });
            }

            foreach (var t in threads) t.Start();
            foreach (var t in threads) t.Join();
            Console.WriteLine($"   Final balance: {account.Balance}");
        }

        public void LockVsInterlockedDemo()
        {
            int counter = 0;
            var lock_ = new object();

            // Using lock
            var sw = System.Diagnostics.Stopwatch.StartNew();
            var threads = new Thread[4];
            for (int i = 0; i < 4; i++)
            {
                threads[i] = new Thread(() =>
                {
                    for (int j = 0; j < 250000; j++)
                    {
                        lock (lock_) counter++;
                    }
                });
            }
            foreach (var t in threads) t.Start();
            foreach (var t in threads) t.Join();
            sw.Stop();
            Console.WriteLine($"   Lock: {sw.ElapsedMilliseconds}ms, counter: {counter}");

            // Using Interlocked
            counter = 0;
            sw.Restart();
            for (int i = 0; i < 4; i++)
            {
                threads[i] = new Thread(() =>
                {
                    for (int j = 0; j < 250000; j++)
                    {
                        Interlocked.Increment(ref counter);
                    }
                });
            }
            foreach (var t in threads) t.Start();
            foreach (var t in threads) t.Join();
            sw.Stop();
            Console.WriteLine($"   Interlocked: {sw.ElapsedMilliseconds}ms, counter: {counter}");
        }

        public void NestedLockDemo()
        {
            var outer = new object();
            var inner = new object();
            int counter = 0;

            var t1 = new Thread(() =>
            {
                lock (outer)
                {
                    Thread.Sleep(10);
                    lock (inner)
                    {
                        counter = 1;
                    }
                }
            });

            var t2 = new Thread(() =>
            {
                lock (inner)
                {
                    Thread.Sleep(10);
                    lock (outer)
                    {
                        counter = 2;
                    }
                }
            });

            t1.Start();
            t2.Start();
            t1.Join();
            t2.Join();
            Console.WriteLine($"   Counter: {counter}");
        }

        public void MonitorDemo()
        {
            var lock_ = new object();
            bool ready = false;

            var consumer = new Thread(() =>
            {
                lock (lock_)
                {
                    while (!ready)
                    {
                        Console.WriteLine("   Waiting...");
                        Monitor.Wait(lock_);
                    }
                    Console.WriteLine("   Consumed!");
                }
            });

            var producer = new Thread(() =>
            {
                lock (lock_)
                {
                    ready = true;
                    Console.WriteLine("   Produced!");
                    Monitor.Pulse(lock_);
                }
            });

            consumer.Start();
            Thread.Sleep(50);
            producer.Start();
            consumer.Join();
        }

        public void ProducerConsumerDemo()
        {
            var queue = new System.Collections.Concurrent.ConcurrentQueue<int>();
            int produced = 0, consumed = 0;
            var lock_ = new object();

            var producer = new Thread(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    queue.Enqueue(i);
                    lock (lock_) produced++;
                    Thread.Sleep(20);
                }
            });

            var consumer = new Thread(() =>
            {
                while (produced < 10 || queue.Count > 0)
                {
                    if (queue.TryDequeue(out var item))
                    {
                        lock (lock_) consumed++;
                    }
                    else
                    {
                        Thread.Sleep(10);
                    }
                }
            });

            producer.Start();
            consumer.Start();
            producer.Join();
            consumer.Join();
            Console.WriteLine($"   Produced: {produced}, Consumed: {consumed}");
        }

        public void ReaderWriterDemo()
        {
            var data = new ReaderWriterLock();
            var results = new int[2];

            var readers = new Thread[4];
            for (int i = 0; i < 4; i++)
            {
                int id = i;
                readers[i] = new Thread(() =>
                {
                    data.Read(() =>
                    {
                        Thread.Sleep(10);
                        results[0]++;
                    });
                });
            }

            var writer = new Thread(() =>
            {
                Thread.Sleep(50);
                data.Write(() =>
                {
                    Thread.Sleep(10);
                    results[1]++;
                });
            });

            foreach (var r in readers) r.Start();
            writer.Start();
            foreach (var r in readers) r.Join();
            writer.Join();
            Console.WriteLine($"   Reads: {results[0]}, Writes: {results[1]}");
        }
    }

    public class BankAccount
    {
        private readonly object _lock = new();
        private decimal _balance;

        public decimal Balance
        {
            get
            {
                lock (_lock) return _balance;
            }
        }

        public void Deposit(decimal amount)
        {
            lock (_lock)
            {
                _balance += amount;
            }
        }

        public void Withdraw(decimal amount)
        {
            lock (_lock)
            {
                if (_balance >= amount)
                    _balance -= amount;
            }
        }
    }

    public class ReaderWriterLock
    {
        private readonly ReaderWriterLockSlim _lock = new();

        public void Read(Action action)
        {
            _lock.EnterReadLock();
            try
            {
                action();
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        public void Write(Action action)
        {
            _lock.EnterWriteLock();
            try
            {
                action();
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }
    }
}