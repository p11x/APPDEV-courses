/*
 * TOPIC: Threading Fundamentals
 * SUBTOPIC: Threads Real-World Part 2
 * FILE: 11_Threads_RealWorld_Part2.cs
 * PURPOSE: More real-world thread patterns and advanced scenarios
 */
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace CSharp_MasterGuide._07_AsyncProgramming._03_Threads
{
    public class ThreadsRealWorldPart2
    {
        public static void Main()
        {
            Console.WriteLine("=== Threads Real-World Part 2 Demo ===\n");

            var demo = new ThreadsRealWorldPart2();

            // Example 1: Thread pool warm-up
            Console.WriteLine("1. Thread pool warm-up:");
            demo.ThreadPoolWarmUpDemo();

            // Example 2: Work stealing queue
            Console.WriteLine("\n2. Work stealing queue:");
            demo.WorkStealingDemo();

            // Example 3: Thread-affinitized processing
            Console.WriteLine("\n3. Thread-affinitized processing:");
            demo.ThreadAffinitizedDemo();

            // Example 4: Graceful shutdown
            Console.WriteLine("\n4. Graceful shutdown:");
            demo.GracefulShutdownDemo();

            // Example 5: Thread monitoring
            Console.WriteLine("\n5. Thread monitoring:");
            demo.ThreadMonitoringDemo();

            Console.WriteLine("\n=== End of Demo ===");
        }

        public void ThreadPoolWarmUpDemo()
        {
            // Warm up thread pool before heavy load
            ThreadPool.SetMinThreads(8, 8);
            
            var sw = System.Diagnostics.Stopwatch.StartNew();
            
            // First batch - slower (thread creation)
            for (int i = 0; i < 4; i++)
            {
                ThreadPool.QueueUserWorkItem(_ => Thread.Sleep(20));
            }
            Thread.Sleep(100);
            
            // Second batch - faster (warm pool)
            sw.Restart();
            for (int i = 0; i < 4; i++)
            {
                ThreadPool.QueueUserWorkItem(_ => Thread.Sleep(20));
            }
            Thread.Sleep(50);
            
            Console.WriteLine($"   Thread pool warm-up completed");
        }

        public void WorkStealingDemo()
        {
            var localQueues = new ConcurrentQueue<Action>[4];
            for (int i = 0; i < 4; i++)
                localQueues[i] = new ConcurrentQueue<Action>();

            // Enqueue work to local queues
            for (int w = 0; w < 20; w++)
            {
                localQueues[w % 4].Enqueue(() =>
                {
                    Thread.Sleep(20);
                });
            }

            // Workers steal from other queues when empty
            var threads = new Thread[4];
            for (int i = 0; i < 4; i++)
            {
                int workerId = i;
                threads[i] = new Thread(() =>
                {
                    int stolen = 0;
                    while (stolen < 5)
                    {
                        // Try local first
                        if (!localQueues[workerId].TryDequeue(out var work))
                        {
                            // Steal from others
                            for (int j = 0; j < 4; j++)
                            {
                                if (j != workerId && localQueues[j].TryDequeue(out work))
                                {
                                    stolen++;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            work?.Invoke();
                        }
                    }
                });
            }

            foreach (var t in threads) t.Start();
            foreach (var t in threads) t.Join();
            Console.WriteLine("   Work stealing completed");
        }

        public void ThreadAffinitizedDemo()
        {
            int processorCount = Environment.ProcessorCount;
            var threads = new Thread[Math.Min(4, processorCount)];

            for (int i = 0; i < threads.Length; i++)
            {
                int id = i;
                threads[i] = new Thread(() =>
                {
                    // Set thread affinity to specific processor
                    var processor = id % processorCount;
                    // Note: In real code, use ProcessThread.ProcessorAffinity
                    
                    Console.WriteLine($"   Thread {id} assigned to processor");
                    Thread.Sleep(50);
                });
                threads[i].Start();
            }

            foreach (var t in threads) t.Join();
        }

        public void GracefulShutdownDemo()
        {
            var processor = new GracefulShutdownProcessor();
            var shutdownComplete = false;

            // Add work
            for (int i = 0; i < 10; i++)
            {
                int workId = i;
                processor.Enqueue(() =>
                {
                    Thread.Sleep(20);
                    Console.WriteLine($"   Processed {workId}");
                });
            }

            // Request shutdown
            processor.Shutdown();
            
            // Wait for completion
            Thread.Sleep(300);
            Console.WriteLine("   Graceful shutdown completed");
        }

        public void ThreadMonitoringDemo()
        {
            int workerThreads, ioThreads;
            ThreadPool.GetAvailableThreads(out workerThreads, out ioThreads);
            ThreadPool.GetMaxThreads(out int maxWorker, out int maxIo);
            
            Console.WriteLine($"   Available workers: {workerThreads}");
            Console.WriteLine($"   Max workers: {maxWorker}");
            Console.WriteLine($"   Available I/O: {ioThreads}");
            Console.WriteLine($"   Max I/O: {maxIo}");

            // Monitor active threads
            var processes = System.Diagnostics.Process.GetCurrentProcess();
            Console.WriteLine($"   Thread count: {processes.Threads.Count}");
        }
    }

    public class GracefulShutdownProcessor
    {
        private readonly ConcurrentQueue<Action> _queue = new();
        private volatile bool _shuttingDown;
        private readonly object _lock = new();
        private int _pendingWork;

        public void Enqueue(Action work)
        {
            if (_shuttingDown) return;
            
            Interlocked.Increment(ref _pendingWork);
            _queue.Enqueue(() =>
            {
                work();
                Interlocked.Decrement(ref _pendingWork);
            });
        }

        public void Shutdown()
        {
            _shuttingDown = true;
            
            // Wait for pending work
            while (Interlocked.CompareExchange(ref _pendingWork, 0, 0) > 0)
            {
                Thread.Sleep(10);
            }
        }
    }

    // Real-world implementations
    public class PipelineProcessor<TInput, TOutput>
    {
        private readonly Thread[] _stage1;
        private readonly Thread[] _stage2;
        private readonly ConcurrentQueue<TInput> _inputQueue = new();
        private readonly ConcurrentQueue<TOutput> _outputQueue = new();
        private volatile bool _running = true;

        public PipelineProcessor(int stage1Count, int stage2Count)
        {
            _stage1 = new Thread[stage1Count];
            _stage2 = new Thread[stage2Count];

            for (int i = 0; i < stage1Count; i++)
            {
                _stage1[i] = new Thread(Stage1Loop) { IsBackground = true };
                _stage1[i].Start();
            }

            for (int i = 0; i < stage2Count; i++)
            {
                _stage2[i] = new Thread(Stage2Loop) { IsBackground = true };
                _stage2[i].Start();
            }
        }

        public void Enqueue(TInput input) => _inputQueue.Enqueue(input);

        private void Stage1Loop()
        {
            while (_running)
            {
                if (_inputQueue.TryDequeue(out var input))
                {
                    var output = Transform(input);
                    _outputQueue.Enqueue(output);
                }
                else
                {
                    Thread.Sleep(5);
                }
            }
        }

        private void Stage2Loop()
        {
            while (_running)
            {
                if (_outputQueue.TryDequeue(out var output))
                {
                    ProcessOutput(output);
                }
                else
                {
                    Thread.Sleep(5);
                }
            }
        }

        private TOutput Transform(TInput input) => default;
        private void ProcessOutput(TOutput output) { }

        public void Stop()
        {
            _running = false;
            foreach (var t in _stage1) t.Join();
            foreach (var t in _stage2) t.Join();
        }
    }

    public class LoadBalancer
    {
        private readonly List<Worker> _workers;
        private int _currentIndex;

        public LoadBalancer(int workerCount)
        {
            _workers = new List<Worker>();
            for (int i = 0; i < workerCount; i++)
            {
                _workers.Add(new Worker { Id = i });
            }
        }

        public Worker GetNextWorker()
        {
            var worker = _workers[Interlocked.Increment(ref _currentIndex) % _workers.Count];
            return worker;
        }

        public void UpdateWorkerStatus(int workerId, bool healthy)
        {
            _workers[workerId].Healthy = healthy;
        }
    }

    public class Worker
    {
        public int Id { get; set; }
        public bool Healthy { get; set; } = true;
    }

    public class CacheInvalidator
    {
        private readonly Dictionary<string, (DateTime Time, Action Invalidate)> _cache = new();
        private readonly Thread _invalidationThread;
        private volatile bool _running = true;

        public CacheInvalidator()
        {
            _invalidationThread = new Thread(InvalidationLoop)
            {
                IsBackground = true,
                Name = "CacheInvalidator"
            };
            _invalidationThread.Start();
        }

        public void AddToCache(string key, TimeSpan expiration, Action invalidate)
        {
            lock (_cache)
            {
                _cache[key] = (DateTime.Now.Add(expiration), invalidate);
            }
        }

        private void InvalidationLoop()
        {
            while (_running)
            {
                var now = DateTime.Now;
                List<string> expired = new();
                
                lock (_cache)
                {
                    foreach (var kvp in _cache)
                    {
                        if (kvp.Value.Time <= now)
                            expired.Add(kvp.Key);
                    }
                }

                foreach (var key in expired)
                {
                    lock (_cache)
                    {
                        if (_cache.TryGetValue(key, out var value))
                        {
                            value.Invalidate?.Invoke();
                            _cache.Remove(key);
                        }
                    }
                }

                Thread.Sleep(1000);
            }
        }

        public void Stop() => _running = false;
    }
}