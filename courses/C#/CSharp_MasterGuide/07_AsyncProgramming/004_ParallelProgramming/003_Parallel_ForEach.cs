/*
 * TOPIC: Parallel Programming
 * SUBTOPIC: Parallel.ForEach
 * FILE: 03_Parallel_ForEach.cs
 * PURPOSE: Understanding Parallel.ForEach for parallel iteration over collections
 */
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CSharp_MasterGuide._07_AsyncProgramming._04_ParallelProgramming
{
    public class ParallelForEachDemo
    {
        public static void Main()
        {
            Console.WriteLine("=== Parallel.ForEach Demo ===\n");

            var demo = new ParallelForEachDemo();

            // Example 1: Basic Parallel.ForEach
            Console.WriteLine("1. Basic Parallel.ForEach:");
            demo.BasicParallelForEach();

            // Example 2: Parallel.ForEach with Partitioner
            Console.WriteLine("\n2. With Partitioner:");
            demo.ParallelForEachWithPartitioner();

            // Example 3: Ordered Parallel.ForEach
            Console.WriteLine("\n3. Ordered processing:");
            demo.OrderedParallelForEach();

            // Example 4: Breaking in Parallel.ForEach
            Console.WriteLine("\n4. Break and stop:");
            demo.BreakAndStopDemo();

            // Example 5: Parallel.ForEach vs foreach
            Console.WriteLine("\n5. Performance comparison:");
            demo.PerformanceComparison();

            // Example 6: Exception handling
            Console.WriteLine("\n6. Exception handling:");
            demo.ExceptionHandlingDemo();

            // Example 7: Thread-local state
            Console.WriteLine("\n7. Thread-local state:");
            demo.ThreadLocalStateDemo();

            Console.WriteLine("\n=== End of Demo ===");
        }

        public void BasicParallelForEach()
        {
            var items = Enumerable.Range(1, 20).ToList();
            var results = new ConcurrentBag<int>();

            Parallel.ForEach(items, item =>
            {
                results.Add(item * 2);
            });

            Console.WriteLine($"   Processed: {results.Count} items");
        }

        public void ParallelForEachWithPartitioner()
        {
            var range = Partitioner.Create(0, 100);
            var results = new ConcurrentBag<int>();

            Parallel.ForEach(range, range2 =>
            {
                for (int i = range2.Item1; i < range2.Item2; i++)
                {
                    results.Add(i * 2);
                }
            });

            Console.WriteLine($"   Partitioned results: {results.Count}");
        }

        public void OrderedParallelForEach()
        {
            var items = Enumerable.Range(1, 10).ToList();
            var results = new ConcurrentBag<(int Index, int Value)>();

            Parallel.ForEach(items, item =>
            {
                results.Add((item, item * 10));
            });

            var ordered = results.OrderBy(x => x.Index).ToList();
            Console.WriteLine($"   First: {ordered[0].Value}, Last: {ordered[^1].Value}");
        }

        public void BreakAndStopDemo()
        {
            var items = Enumerable.Range(1, 100).ToList();
            int processed = 0;
            bool found = false;

            Parallel.ForEach(items, (item, loopState) =>
            {
                if (item == 25)
                {
                    found = true;
                    loopState.Stop();
                    return;
                }
                Interlocked.Increment(ref processed);
            });

            Console.WriteLine($"   Found: {found}, Processed: {processed}");
        }

        public void PerformanceComparison()
        {
            var items = Enumerable.Range(1, 10000).ToList();
            
            // Sequential
            var sw = System.Diagnostics.Stopwatch.StartNew();
            int seqSum = 0;
            foreach (var item in items)
            {
                seqSum += item;
            }
            sw.Stop();
            Console.WriteLine($"   Sequential: {sw.ElapsedMilliseconds}ms, sum: {seqSum}");
            
            // Parallel
            sw.Restart();
            int parSum = 0;
            Parallel.ForEach(items, item =>
            {
                Interlocked.Add(ref parSum, item);
            });
            sw.Stop();
            Console.WriteLine($"   Parallel: {sw.ElapsedMilliseconds}ms, sum: {parSum}");
        }

        public void ExceptionHandlingDemo()
        {
            var items = new[] { 1, 2, 3, 4, 5 };

            try
            {
                Parallel.ForEach(items, item =>
                {
                    if (item == 3)
                        throw new Exception($"Error at {item}");
                });
            }
            catch (AggregateException ae)
            {
                Console.WriteLine($"   Caught: {ae.InnerExceptions.Count} exceptions");
            }
        }

        public void ThreadLocalStateDemo()
        {
            var items = Enumerable.Range(1, 1000).ToList();
            long total = 0;

            Parallel.ForEach(items,
                () => 0L,
                (item, loop, localSum) => localSum + item,
                localSum => Interlocked.Add(ref total, localSum)
            );

            Console.WriteLine($"   Total: {total}");
        }
    }

    // Real-world examples
    public class ParallelDataProcessor
    {
        public void ProcessFiles(IEnumerable<string> files, Action<string> processFile)
        {
            Parallel.ForEach(files, file =>
            {
                processFile(file);
            });
        }

        public Dictionary<string, int> CountWords(IEnumerable<string> texts)
        {
            var counts = new ConcurrentDictionary<string, int>();

            Parallel.ForEach(texts, text =>
            {
                var words = text.Split(' ');
                foreach (var word in words)
                {
                    if (!string.IsNullOrEmpty(word))
                    {
                        counts.AddOrUpdate(word, 1, (k, v) => v + 1);
                    }
                }
            });

            return new Dictionary<string, int>(counts);
        }

        public List<T> FilterParallel<T>(IEnumerable<T> items, Func<T, bool> predicate)
        {
            var results = new ConcurrentBag<T>();

            Parallel.ForEach(items, item =>
            {
                if (predicate(item))
                    results.Add(item);
            });

            return results.ToList();
        }
    }

    public class ParallelImageLoader
    {
        public Dictionary<string, byte[]> LoadImagesParallel(IEnumerable<string> paths)
        {
            var images = new ConcurrentDictionary<string, byte[]>();

            Parallel.ForEach(paths, path =>
            {
                // Simulate loading
                Thread.Sleep(10);
                var data = new byte[100];
                images.TryAdd(path, data);
            });

            return new Dictionary<string, byte[]>(images);
        }

        public void ProcessImagesInBatches(IEnumerable<string> paths, int batchSize, Action<string[]> batchProcessor)
        {
            var batches = paths
                .Select((path, index) => new { path, index })
                .GroupBy(x => x.index / batchSize)
                .Select(g => g.Select(x => x.path).ToArray());

            Parallel.ForEach(batches, batch => batchProcessor(batch));
        }
    }

    public class ParallelAggregator<T>
    {
        public T Aggregate(IEnumerable<T> items, Func<T, T, T> aggregator)
        {
            T result = default;
            bool first = true;

            Parallel.ForEach(items, item =>
            {
                // Simple (not perfect) aggregation
                lock (aggregator)
                {
                    if (first)
                    {
                        result = item;
                        first = false;
                    }
                    else
                    {
                        result = aggregator(result, item);
                    }
                }
            });

            return result;
        }
    }
}