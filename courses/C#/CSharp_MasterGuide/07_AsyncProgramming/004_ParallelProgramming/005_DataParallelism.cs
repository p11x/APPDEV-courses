/*
 * TOPIC: Parallel Programming
 * SUBTOPIC: Data Parallelism
 * FILE: 05_DataParallelism.cs
 * PURPOSE: Understanding data parallelism patterns and patterns
 */
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CSharp_MasterGuide._07_AsyncProgramming._04_ParallelProgramming
{
    public class DataParallelismDemo
    {
        public static void Main()
        {
            Console.WriteLine("=== Data Parallelism Demo ===\n");

            var demo = new DataParallelismDemo();

            // Example 1: Map pattern (element-wise transformation)
            Console.WriteLine("1. Map pattern:");
            demo.MapPattern();

            // Example 2: Reduce pattern (aggregation)
            Console.WriteLine("\n2. Reduce pattern:");
            demo.ReducePattern();

            // Example 3: Filter pattern
            Console.WriteLine("\n3. Filter pattern:");
            demo.FilterPattern();

            // Example 4: Scan pattern (parallel prefix)
            Console.WriteLine("\n4. Scan pattern:");
            demo.ScanPattern();

            // Example 5: Partitioned reduction
            Console.WriteLine("\n5. Partitioned reduction:");
            demo.PartitionedReduction();

            // Example 6: Fork-join pattern
            Console.WriteLine("\n6. Fork-join pattern:");
            demo.ForkJoinPattern();

            Console.WriteLine("\n=== End of Demo ===");
        }

        public void MapPattern()
        {
            var data = Enumerable.Range(1, 1000).ToArray();
            var result = new int[data.Length];

            Parallel.For(0, data.Length, i =>
            {
                result[i] = data[i] * 2 + 1;
            });

            Console.WriteLine($"   First: {result[0]}, Last: {result[999]}");
        }

        public void ReducePattern()
        {
            var data = Enumerable.Range(1, 1000).ToArray();
            long sum = 0;

            Parallel.For(0, data.Length, 
                () => 0L,
                (i, loop, local) => local + data[i],
                local => Interlocked.Add(ref sum, local)
            );

            Console.WriteLine($"   Sum: {sum}");
        }

        public void FilterPattern()
        {
            var data = Enumerable.Range(1, 100).ToArray();
            var results = new ConcurrentBag<int>();

            Parallel.For(0, data.Length, i =>
            {
                if (data[i] % 3 == 0)
                    results.Add(data[i]);
            });

            Console.WriteLine($"   Filtered count: {results.Count}");
        }

        public void ScanPattern()
        {
            var data = new[] { 1, 2, 3, 4, 5 };
            var result = new int[data.Length];
            int sum = 0;

            Parallel.For(0, data.Length, i =>
            {
                Interlocked.Add(ref sum, data[i]);
                result[i] = sum;
            });

            Console.WriteLine($"   Prefix sums: {string.Join(", ", result)}");
        }

        public void PartitionedReduction()
        {
            var data = Enumerable.Range(1, 10000).ToArray();
            int chunkSize = 1000;
            int numChunks = (data.Length + chunkSize - 1) / chunkSize;
            var partialSums = new int[numChunks];

            Parallel.For(0, numChunks, chunk =>
            {
                int start = chunk * chunkSize;
                int end = Math.Min(start + chunkSize, data.Length);
                int sum = 0;
                for (int i = start; i < end; i++)
                    sum += data[i];
                partialSums[chunk] = sum;
            });

            int total = partialSums.Sum();
            Console.WriteLine($"   Total sum: {total}");
        }

        public void ForkJoinPattern()
        {
            var results = new ConcurrentBag<int>();

            Parallel.Invoke(
                () => results.Add(Compute(1)),
                () => results.Add(Compute(2)),
                () => results.Add(Compute(3)),
                () => results.Add(Compute(4))
            );

            int combined = results.Sum();
            Console.WriteLine($"   Combined: {combined}");
        }

        private int Compute(int value)
        {
            Thread.Sleep(30);
            return value * 10;
        }
    }

    // Real-world data parallelism patterns
    public class ParallelStatistics
    {
        public double Mean(int[] data)
        {
            long sum = 0;
            Parallel.For(0, data.Length, 
                () => 0L,
                (i, loop, local) => local + data[i],
                local => Interlocked.Add(ref sum, local)
            );
            return (double)sum / data.Length;
        }

        public double StandardDeviation(int[] data)
        {
            double mean = Mean(data);
            double sumSquares = 0;

            Parallel.For(0, data.Length,
                () => 0.0,
                (i, loop, local) => local + Math.Pow(data[i] - mean, 2),
                local => Interlocked.Add(ref sumSquares, local)
            );

            return Math.Sqrt(sumSquares / data.Length);
        }

        public (int Min, int Max) MinMax(int[] data)
        {
            int min = int.MaxValue;
            int max = int.MinValue;

            Parallel.For(0, data.Length, i =>
            {
                int val = data[i];
                lock (_minMaxLock)
                {
                    if (val < min) min = val;
                    if (val > max) max = val;
                }
            });

            return (min, max);
        }

        private readonly object _minMaxLock = new();
    }

    public class ParallelSort
    {
        public void QuickSort(int[] data, int low, int high)
        {
            if (low < high)
            {
                int pivot = Partition(data, low, high);
                
                // Parallelize recursive calls
                Parallel.Invoke(
                    () => QuickSort(data, low, pivot - 1),
                    () => QuickSort(data, pivot + 1, high)
                );
            }
        }

        private int Partition(int[] data, int low, int high)
        {
            int pivot = data[high];
            int i = low - 1;
            for (int j = low; j < high; j++)
            {
                if (data[j] < pivot)
                {
                    i++;
                    Swap(ref data[i], ref data[j]);
                }
            }
            Swap(ref data[i + 1], ref data[high]);
            return i + 1;
        }

        private void Swap(ref int a, ref int b)
        {
            int temp = a;
            a = b;
            b = temp;
        }
    }

    public class ParallelSearch
    {
        public int BinarySearchParallel(int[] sortedData, int target)
        {
            int left = 0;
            int right = sortedData.Length - 1;

            while (left <= right)
            {
                int mid = left + (right - left) / 2;
                
                if (sortedData[mid] == target)
                    return mid;
                
                if (sortedData[mid] < target)
                    left = mid + 1;
                else
                    right = mid - 1;
            }

            return -1;
        }

        public List<int> SearchMultipleTargets(int[] data, int[] targets)
        {
            var results = new ConcurrentBag<int>();

            Parallel.ForEach(targets, target =>
            {
                int index = BinarySearchParallel(data, target);
                if (index >= 0)
                    results.Add(index);
            });

            return results.ToList();
        }
    }

    public class ParallelMatrixOperations
    {
        public double[,] Multiply(double[,] a, double[,] b)
        {
            int rows = a.GetLength(0);
            int cols = b.GetLength(1);
            int inner = a.GetLength(1);
            var result = new double[rows, cols];

            Parallel.For(0, rows, i =>
            {
                for (int j = 0; j < cols; j++)
                {
                    double sum = 0;
                    for (int k = 0; k < inner; k++)
                        sum += a[i, k] * b[k, j];
                    result[i, j] = sum;
                }
            });

            return result;
        }

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
    }
}