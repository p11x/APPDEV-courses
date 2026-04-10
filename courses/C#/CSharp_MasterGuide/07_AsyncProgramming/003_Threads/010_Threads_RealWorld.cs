/*
 * TOPIC: Threading Fundamentals
 * SUBTOPIC: Threads Real-World
 * FILE: 10_Threads_RealWorld.cs
 * PURPOSE: Real-world thread patterns and applications
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace CSharp_MasterGuide._07_AsyncProgramming._03_Threads
{
    public class ThreadsRealWorld
    {
        public static void Main()
        {
            Console.WriteLine("=== Threads Real-World Demo ===\n");

            var demo = new ThreadsRealWorld();

            // Example 1: File processor with worker threads
            Console.WriteLine("1. File processor:");
            demo.FileProcessorDemo();

            // Example 2: Background task scheduler
            Console.WriteLine("\n2. Background scheduler:");
            demo.BackgroundSchedulerDemo();

            // Example 3: Thread-safe logger
            Console.WriteLine("\n3. Thread-safe logger:");
            demo.ThreadSafeLoggerDemo();

            // Example 4: Producer-consumer pattern
            Console.WriteLine("\n4. Producer-consumer:");
            demo.ProducerConsumerPattern();

            // Example 5: Parallel image processing
            Console.WriteLine("\n5. Image processing:");
            demo.ImageProcessingDemo();

            // Example 6: Batch processing with threads
            Console.WriteLine("\n6. Batch processing:");
            demo.BatchProcessingDemo();

            Console.WriteLine("\n=== End of Demo ===");
        }

        public void FileProcessorDemo()
        {
            var processor = new AsyncFileProcessor(4);
            var files = new[] { "f1.txt", "f2.txt", "f3.txt" };
            
            foreach (var f in files)
            {
                processor.Enqueue(f, () =>
                {
                    // Simulate processing
                    Thread.Sleep(30);
                    Console.WriteLine($"   Processed {f}");
                });
            }

            Thread.Sleep(200);
        }

        public void BackgroundSchedulerDemo()
        {
            var scheduler = new BackgroundScheduler();
            
            scheduler.Schedule(() => Console.WriteLine("   Task A"), TimeSpan.FromMilliseconds(50));
            scheduler.Schedule(() => Console.WriteLine("   Task B"), TimeSpan.FromMilliseconds(100));
            scheduler.Schedule(() => Console.WriteLine("   Task C"), TimeSpan.FromMilliseconds(150));

            Thread.Sleep(300);
            scheduler.Stop();
            Console.WriteLine("   Scheduler stopped");
        }

        public void ThreadSafeLoggerDemo()
        {
            var logger = new ThreadSafeLogger("log.txt");
            
            var threads = new Thread[4];
            for (int i = 0; i < 4; i++)
            {
                int id = i;
                threads[i] = new Thread(() =>
                {
                    for (int j = 0; j < 5; j++)
                    {
                        logger.Log($"Message from thread {id} - {j}");
                        Thread.Sleep(20);
                    }
                });
            }

            foreach (var t in threads) t.Start();
            foreach (var t in threads) t.Join();
            
            Console.WriteLine("   Logger completed");
        }

        public void ProducerConsumerPattern()
        {
            var queue = new System.Collections.Concurrent.ConcurrentQueue<int>();
            var produced = 0;
            var consumed = 0;

            var producer = new Thread(() =>
            {
                for (int i = 0; i < 20; i++)
                {
                    queue.Enqueue(i);
                    Interlocked.Increment(ref produced);
                    Thread.Sleep(15);
                }
            });

            var consumer = new Thread(() =>
            {
                int item;
                while (Interlocked.CompareExchange(ref produced, 0, 0) < 20 ||
                       queue.Count > 0)
                {
                    if (queue.TryDequeue(out item))
                    {
                        Interlocked.Increment(ref consumed);
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

        public void ImageProcessingDemo()
        {
            var images = new[] { "img1", "img2", "img3", "img4" };
            var processor = new ImageProcessor(2);
            var results = new List<string>();

            var threads = new Thread[images.Length];
            for (int i = 0; i < images.Length; i++)
            {
                int idx = i;
                threads[i] = new Thread(() =>
                {
                    var result = processor.Process(images[idx]);
                    lock (results) results.Add(result);
                });
            }

            foreach (var t in threads) t.Start();
            foreach (var t in threads) t.Join();
            
            Console.WriteLine($"   Processed: {results.Count} images");
        }

        public void BatchProcessingDemo()
        {
            var batchProcessor = new BatchProcessor(3);
            var processed = new List<int>();
            var lock_ = new object();

            var items = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            
            foreach (var item in items)
            {
                int id = item;
                batchProcessor.Enqueue(() =>
                {
                    Thread.Sleep(30);
                    lock (lock_)
                    {
                        processed.Add(id);
                    }
                });
            }

            Thread.Sleep(400);
            Console.WriteLine($"   Batch processed: {processed.Count}");
        }
    }

    public class AsyncFileProcessor
    {
        private readonly Thread[] _workers;
        private readonly System.Collections.Concurrent.ConcurrentQueue<(string, Action)> _queue = new();

        public AsyncFileProcessor(int workerCount)
        {
            _workers = new Thread[workerCount];
            for (int i = 0; i < workerCount; i++)
            {
                _workers[i] = new Thread(ProcessLoop)
                {
                    IsBackground = true,
                    Name = $"FileWorker-{i}"
                };
                _workers[i].Start();
            }
        }

        public void Enqueue(string file, Action work)
        {
            _queue.Enqueue((file, work));
        }

        private void ProcessLoop()
        {
            while (true)
            {
                if (_queue.TryDequeue(out var item))
                {
                    item.Item2();
                }
                else
                {
                    Thread.Sleep(10);
                }
            }
        }
    }

    public class BackgroundScheduler
    {
        private readonly List<(Timer Timer, Action Work)> _tasks = new();
        private readonly object _lock = new();

        public void Schedule(Action work, TimeSpan delay)
        {
            var timer = new Timer(_ =>
            {
                work();
            }, null, delay, Timeout.InfiniteTimeSpan);

            lock (_lock)
            {
                _tasks.Add((timer, work));
            }
        }

        public void Stop()
        {
            lock (_lock)
            {
                foreach (var (timer, _) in _tasks)
                {
                    timer.Dispose();
                }
                _tasks.Clear();
            }
        }
    }

    public class ThreadSafeLogger
    {
        private readonly string _filePath;
        private readonly object _lock = new();

        public ThreadSafeLogger(string filePath)
        {
            _filePath = filePath;
        }

        public void Log(string message)
        {
            lock (_lock)
            {
                File.AppendAllText(_filePath, $"{DateTime.Now}: {message}\n");
            }
        }
    }

    public class ImageProcessor
    {
        private readonly SemaphoreSlim _semaphore;

        public ImageProcessor(int maxConcurrency)
        {
            _semaphore = new SemaphoreSlim(maxConcurrency);
        }

        public string Process(string imageName)
        {
            _semaphore.Wait();
            try
            {
                Thread.Sleep(50);
                return $"{imageName}_processed";
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }

    public class BatchProcessor
    {
        private readonly Thread[] _workers;
        private readonly System.Collections.Concurrent.ConcurrentQueue<Action> _queue = new();
        private bool _running = true;

        public BatchProcessor(int workerCount)
        {
            _workers = new Thread[workerCount];
            for (int i = 0; i < workerCount; i++)
            {
                _workers[i] = new Thread(ProcessLoop)
                {
                    IsBackground = true,
                    Name = $"BatchWorker-{i}"
                };
                _workers[i].Start();
            }
        }

        public void Enqueue(Action work)
        {
            _queue.Enqueue(work);
        }

        private void ProcessLoop()
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