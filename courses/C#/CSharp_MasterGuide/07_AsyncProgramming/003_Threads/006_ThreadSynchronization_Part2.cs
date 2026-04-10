/*
 * TOPIC: Threading Fundamentals
 * SUBTOPIC: Thread Synchronization Part 2
 * FILE: 06_ThreadSynchronization_Part2.cs
 * PURPOSE: Advanced synchronization patterns and best practices
 */
using System;
using System.Collections.Generic;
using System.Threading;

namespace CSharp_MasterGuide._07_AsyncProgramming._03_Threads
{
    public class ThreadSynchronizationPart2
    {
        public static void Main()
        {
            Console.WriteLine("=== Thread Synchronization Part 2 Demo ===\n");

            var demo = new ThreadSynchronizationPart2();

            // Example 1: ReaderWriterLockSlim
            Console.WriteLine("1. ReaderWriterLockSlim:");
            demo.ReaderWriterLockSlimDemo();

            // Example 2: Double-check locking
            Console.WriteLine("\n2. Double-check locking:");
            demo.DoubleCheckLockingDemo();

            // Example 3: Lock hints and timeouts
            Console.WriteLine("\n3. Lock timeouts:");
            demo.LockTimeoutDemo();

            // Example 4: Monitor.Pulse and Wait
            Console.WriteLine("\n4. Monitor.Pulse/Wait:");
            demo.MonitorPulseWaitDemo();

            // Example 5: SpinWait
            Console.WriteLine("\n5. SpinWait:");
            demo.SpinWaitDemo();

            // Example 6: Synchronization context
            Console.WriteLine("\n6. Synchronization context:");
            demo.SyncContextDemo();

            Console.WriteLine("\n=== End of Demo ===");
        }

        private readonly ReaderWriterLockSlim _rwLock = new();
        private readonly Dictionary<string, int> _data = new();

        public void ReaderWriterLockSlimDemo()
        {
            var threads = new Thread[6];
            
            // 4 readers
            for (int i = 0; i < 4; i++)
            {
                int id = i;
                threads[i] = new Thread(() =>
                {
                    for (int j = 0; j < 5; j++)
                    {
                        _rwLock.EnterReadLock();
                        try
                        {
                            var count = _data.Count;
                            Console.WriteLine($"   Reader {id}: {count} items");
                        }
                        finally
                        {
                            _rwLock.ExitReadLock();
                        }
                        Thread.Sleep(10);
                    }
                });
            }

            // 2 writers
            for (int i = 0; i < 2; i++)
            {
                int id = i;
                threads[4 + i] = new Thread(() =>
                {
                    for (int j = 0; j < 3; j++)
                    {
                        _rwLock.EnterWriteLock();
                        try
                        {
                            _data[$"key{id}_{j}"] = j;
                            Console.WriteLine($"   Writer {id} wrote");
                        }
                        finally
                        {
                            _rwLock.ExitWriteLock();
                        }
                        Thread.Sleep(20);
                    }
                });
            }

            foreach (var t in threads) t.Start();
            foreach (var t in threads) t.Join();
        }

        private readonly object _lazyLock = new();
        private volatile bool _initialized;
        private int _value;

        public void DoubleCheckLockingDemo()
        {
            var threads = new Thread[4];
            for (int i = 0; i < 4; i++)
            {
                threads[i] = new Thread(() =>
                {
                    if (!_initialized)
                    {
                        lock (_lazyLock)
                        {
                            if (!_initialized)
                            {
                                Thread.Sleep(10);
                                _value = 42;
                                _initialized = true;
                                Console.WriteLine("   Initialized");
                            }
                        }
                    }
                    Console.WriteLine($"   Value: {_value}");
                });
            }

            foreach (var t in threads) t.Start();
            foreach (var t in threads) t.Join();
        }

        public void LockTimeoutDemo()
        {
            var lock_ = new object();

            var t1 = new Thread(() =>
            {
                lock (lock_)
                {
                    Console.WriteLine("   T1 acquired lock");
                    Thread.Sleep(200);
                }
            });

            var t2 = new Thread(() =>
            {
                bool acquired = Monitor.TryEnter(lock_, TimeSpan.FromMilliseconds(50));
                if (acquired)
                {
                    try
                    {
                        Console.WriteLine("   T2 acquired lock");
                    }
                    finally
                    {
                        Monitor.Exit(lock_);
                    }
                }
                else
                {
                    Console.WriteLine("   T2 timeout - could not acquire");
                }
            });

            t1.Start();
            Thread.Sleep(50);
            t2.Start();
            t1.Join();
            t2.Join();
        }

        private readonly object _monitorLock = new();
        private readonly Queue<string> _queue = new();

        public void MonitorPulseWaitDemo()
        {
            var consumer = new Thread(() =>
            {
                lock (_monitorLock)
                {
                    while (_queue.Count == 0)
                    {
                        Console.WriteLine("   Consumer waiting...");
                        Monitor.Wait(_monitorLock);
                    }
                    var item = _queue.Dequeue();
                    Console.WriteLine($"   Consumed: {item}");
                }
            });

            consumer.Start();
            Thread.Sleep(50);

            lock (_monitorLock)
            {
                _queue.Enqueue("item1");
                Console.WriteLine("   Produced item");
                Monitor.Pulse(_monitorLock);
            }

            consumer.Join();
        }

        public void SpinWaitDemo()
        {
            bool completed = false;
            var sw = new SpinWait();

            var t = new Thread(() =>
            {
                Thread.Sleep(50);
                completed = true;
            });
            t.Start();

            var sw2 = System.Diagnostics.Stopwatch.StartNew();
            while (!completed)
            {
                sw.SpinOnce();
            }
            sw2.Stop();
            Console.WriteLine($"   SpinWait took: {sw2.ElapsedMilliseconds}ms");
        }

        public void SyncContextDemo()
        {
            var context = SynchronizationContext.Current;
            Console.WriteLine($"   Current context: {context?.GetType().Name ?? "null"}");

            // Post back to synchronization context
            context?.Post(_ =>
            {
                Console.WriteLine("   Posted to sync context");
            }, null);

            Thread.Sleep(50);
        }
    }

    // Real-world patterns
    public class LazyInitializerDemo<T> where T : class
    {
        private T _value;
        private readonly object _lock = new();

        public T Value
        {
            get
            {
                if (_value == null)
                {
                    lock (_lock)
                    {
                        if (_value == null)
                        {
                            _value = CreateValue();
                        }
                    }
                }
                return _value;
            }
        }

        private T CreateValue()
        {
            Console.WriteLine("   Creating value...");
            return Activator.CreateInstance<T>();
        }
    }

    public class ThreadSafeList<T>
    {
        private readonly List<T> _list = new();
        private readonly object _lock = new();

        public void Add(T item)
        {
            lock (_lock)
            {
                _list.Add(item);
            }
        }

        public bool Remove(T item)
        {
            lock (_lock)
            {
                return _list.Remove(item);
            }
        }

        public T GetAt(int index)
        {
            lock (_lock)
            {
                return index >= 0 && index < _list.Count ? _list[index] : default;
            }
        }

        public int Count
        {
            get
            {
                lock (_lock)
                {
                    return _list.Count;
                }
            }
        }

        public void ForEach(Action<T> action)
        {
            lock (_lock)
            {
                foreach (var item in _list)
                {
                    action(item);
                }
            }
        }
    }

    public class BoundedBuffer<T>
    {
        private readonly T[] _buffer;
        private int _head, _tail, _count;
        private readonly object _lock = new();

        public BoundedBuffer(int capacity)
        {
            _buffer = new T[capacity];
        }

        public void Put(T item)
        {
            lock (_lock)
            {
                while (_count >= _buffer.Length)
                {
                    Monitor.Wait(_lock);
                }
                _buffer[_tail] = item;
                _tail = (_tail + 1) % _buffer.Length;
                _count++;
                Monitor.Pulse(_lock);
            }
        }

        public T Get()
        {
            lock (_lock)
            {
                while (_count == 0)
                {
                    Monitor.Wait(_lock);
                }
                var item = _buffer[_head];
                _head = (_head + 1) % _buffer.Length;
                _count--;
                Monitor.Pulse(_lock);
                return item;
            }
        }
    }
}