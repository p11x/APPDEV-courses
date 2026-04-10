/*
 * TOPIC: Parallel Programming
 * SUBTOPIC: Parallel.For Part 2
 * FILE: 02_Parallel_For_Part2.cs
 * PURPOSE: Advanced Parallel.For patterns and optimizations
 */
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CSharp_MasterGuide._07_AsyncProgramming._04_ParallelProgramming
{
    public class ParallelForPart2
    {
        public static void Main()
        {
            Console.WriteLine("=== Parallel.For Part 2 Demo ===\n");

            var demo = new ParallelForPart2();

            // Example 1: Chunked parallel for
            Console.WriteLine("1. Chunked parallel for:");
            demo.ChunkedParallelFor();

            // Example 2: Partitioned processing
            Console.WriteLine("\n2. Partitioned processing:");
            demo.PartitionedProcessing();

            // Example 3: Nested Parallel.For
            Console.WriteLine("\n3. Nested Parallel.For:");
            demo.NestedParallelFor();

            // Example 4: Cancellation in Parallel.For
            Console.WriteLine("\n4. Cancellation:");
            demo.CancellationDemo();

            // Example 5: Thread-local storage optimization
            Console.WriteLine("\n5. Thread-local storage:");
            demo.ThreadLocalStorageDemo();

            // Example 6: Dynamic parallelism
            Console.WriteLine("\n6. Dynamic parallelism:");
            demo.DynamicParallelismDemo();

            Console.WriteLine("\n=== End of Demo ===");
        }

        public void ChunkedParallelFor()
        {
            var data = Enumerable.Range(1, 100).ToList();
            int chunkSize = 25;
            var processed = new ConcurrentBag<int>();

            Parallel.For(0, (data.Count + chunkSize - 1) / chunkSize, partition =>
            {
                int start = partition * chunkSize;
                int end = Math.Min(start + chunkSize, data.Count);
                
                for (int i = start; i < end; i++)
                {
                    processed.Add(data[i] * 2);
                }
            });

            Console.WriteLine($"   Processed: {processed.Count} items");
        }

        public void PartitionedProcessing()
        {
            var rangePartitioner = Partitioner.Create(0, 100);
            var results = new ConcurrentBag<int>();

            Parallel.ForEach(rangePartitioner, (range, loopState) =>
            {
                for (int i = range.Item1; i < range.Item2; i++)
                {
                    results.Add(Compute(i));
                }
            });

            Console.WriteLine($"   Results: {results.Count}");
        }

        private int Compute(int value)
        {
            Thread.SpinWait(1000);
            return value * 2;
        }

        public void NestedParallelFor()
        {
            var sw = Stopwatch.StartNew();
            int count = 0;
            
            Parallel.For(0, 10, i =>
            {
                Parallel.For(0, 10, j =>
                {
                    Interlocked.Increment(ref count);
                });
            });
            
            sw.Stop();
            Console.WriteLine($"   Nested iterations: {count}, Time: {sw.ElapsedMilliseconds}ms");
        }

        public void CancellationDemo()
        {
            var cts = new CancellationTokenSource();
            
            try
            {
                Parallel.For(0, 100, new ParallelOptions { CancellationToken = cts.Token }, i =>
                {
                    if (i > 20)
                    {
                        cts.Cancel();
                        return;
                    }
                    Thread.Sleep(10);
                });
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("   Cancelled!");
            }
        }

        public void ThreadLocalStorageDemo()
        {
            var sw = Stopwatch.StartNew();
            
            long total = 0;
            Parallel.For(0, 1000000,
                () => 0L,
                (i, loop, localSum) => localSum + i % 10,
                localSum => Interlocked.Add(ref total, localSum)
            );
            
            sw.Stop();
            Console.WriteLine($"   Sum: {total}, Time: {sw.ElapsedMilliseconds}ms");
        }

        public void DynamicParallelismDemo()
        {
            var results = new ConcurrentBag<string>();
            
            Parallel.For(0, 10, i =>
            {
                results.Add($"Task {i}");
                
                // Dynamically add more work
                if (i % 3 == 0)
                {
                    results.Add($"  Sub-task {i}.1");
                    results.Add($"  Sub-task {i}.2");
                }
            });

            Console.WriteLine($"   Total items: {results.Count}");
        }
    }

    // Real-world advanced patterns
    public class ParallelAggregation
    {
        public int SumWithChunks(int[] data, int chunkSize)
        {
            int totalSum = 0;
            
            Parallel.For(0, (data.Length + chunkSize - 1) / chunkSize, partition =>
            {
                int start = partition * chunkSize;
                int end = Math.Min(start + chunkSize, data.Length);
                int sum = 0;
                
                for (int i = start; i < end; i++)
                    sum += data[i];
                
                Interlocked.Add(ref totalSum, sum);
            });
            
            return totalSum;
        }

        public Dictionary<int, int> GroupAndCount(int[] data)
        {
            var counts = new ConcurrentDictionary<int, int>();
            
            Parallel.For(0, data.Length, i =>
            {
                counts.AddOrUpdate(data[i], 1, (k, v) => v + 1);
            });
            
            return new Dictionary<int, int>(counts);
        }
    }

    public class ParallelMatrixOps
    {
        public double[] MultiplyVector(double[,] matrix, double[] vector)
        {
            int rows = matrix.GetLength(0);
            var result = new double[rows];
            
            Parallel.For(0, rows, i =>
            {
                double sum = 0;
                for (int j = 0; j < vector.Length; j++)
                    sum += matrix[i, j] * vector[j];
                result[i] = sum;
            });
            
            return result;
        }

        public double[,] Transpose(double[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            var result = new double[cols, rows];
            
            Parallel.For(0, rows, i =>
            {
                for (int j = 0; j < cols; j++)
                    result[j, i] = matrix[i, j];
            });
            
            return result;
        }
    }

    public class ParallelDataSearch
    {
        public int FindFirstParallel(int[] data, Func<int, bool> predicate)
        {
            int result = -1;
            
            Parallel.For(0, data.Length, (i, loopState) =>
            {
                if (predicate(data[i]))
                {
                    result = i;
                    loopState.Stop();
                }
            });
            
            return result;
        }

        public int CountParallel(int[] data, Func<int, bool> predicate)
        {
            int count = 0;
            
            Parallel.For(0, data.Length, i =>
            {
                if (predicate(data[i]))
                    Interlocked.Increment(ref count);
            });
            
            return count;
        }
    }

    public class ParallelImageFilter
    {
        public byte[,] ApplyConvolution(byte[,] image, double[,] kernel)
        {
            int rows = image.GetLength(0);
            int cols = image.GetLength(1);
            var result = new byte[rows, cols];
            int kSize = kernel.GetLength(0) / 2;
            
            Parallel.For(kSize, rows - kSize, r =>
            {
                for (int c = kSize; c < cols - kSize; c++)
                {
                    result[r, c] = ComputeConvolution(image, kernel, r, c, kSize);
                }
            });
            
            return result;
        }

        private byte ComputeConvolution(byte[,] image, double[,] kernel, int row, int col, int kSize)
        {
            double sum = 0;
            for (int kr = -kSize; kr <= kSize; kr++)
            {
                for (int kc = -kSize; kc <= kSize; kc++)
                {
                    sum += image[row + kr, col + kc] * kernel[kr + kSize, kc + kSize];
                }
            }
            return (byte)Math.Min(255, Math.Max(0, sum));
        }
    }
}