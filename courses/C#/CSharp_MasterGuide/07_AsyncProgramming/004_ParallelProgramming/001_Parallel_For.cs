/*
 * TOPIC: Parallel Programming
 * SUBTOPIC: Parallel.For
 * FILE: 01_Parallel_For.cs
 * PURPOSE: Understanding Parallel.For for parallel iteration
 */
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace CSharp_MasterGuide._07_AsyncProgramming._04_ParallelProgramming
{
    public class ParallelForDemo
    {
        public static void Main()
        {
            Console.WriteLine("=== Parallel.For Demo ===\n");

            var demo = new ParallelForDemo();

            // Example 1: Basic Parallel.For
            Console.WriteLine("1. Basic Parallel.For:");
            demo.BasicParallelFor();

            // Example 2: Parallel.For with local state
            Console.WriteLine("\n2. Parallel.For with local state:");
            demo.ParallelForWithLocalState();

            // Example 3: Parallel.For with index
            Console.WriteLine("\n3. Parallel.For with index:");
            demo.ParallelForWithIndex();

            // Example 4: Breaking out of Parallel.For
            Console.WriteLine("\n4. Breaking out of Parallel.For:");
            demo.ParallelForBreak();

            // Example 5: Parallel.For vs regular for
            Console.WriteLine("\n5. Performance comparison:");
            demo.PerformanceComparison();

            // Example 6: Parallel.For with exception handling
            Console.WriteLine("\n6. Exception handling:");
            demo.ParallelForExceptionHandling();

            // Example 7: Parallel.For with options
            Console.WriteLine("\n7. Parallel.For options:");
            demo.ParallelForOptions();

            Console.WriteLine("\n=== End of Demo ===");
        }

        public void BasicParallelFor()
        {
            var values = new int[100];
            
            Parallel.For(0, 100, i =>
            {
                values[i] = i * 2;
            });

            Console.WriteLine($"   First value: {values[0]}, Last: {values[99]}");
        }

        public void ParallelForWithLocalState()
        {
            int sum = 0;
            
            Parallel.For(0, 1000, 
                () => 0, // Local init
                (i, loop, localSum) => localSum + i, // Body
                localSum => Interlocked.Add(ref sum, localSum) // Local finally
            );
            
            Console.WriteLine($"   Sum: {sum}");
        }

        public void ParallelForWithIndex()
        {
            var results = new string[10];
            
            Parallel.For(0, 10, (i, state) =>
            {
                results[i] = $"Item {i} on thread {Thread.CurrentThread.ManagedThreadId}";
            });

            foreach (var r in results)
                Console.WriteLine($"   {r}");
        }

        public void ParallelForBreak()
        {
            int sum = 0;
            
            Parallel.For(0, 100, (i, loop) =>
            {
                if (i > 20)
                {
                    loop.Break();
                    return;
                }
                Interlocked.Add(ref sum, i);
            });
            
            Console.WriteLine($"   Sum (stopped early): {sum}");
        }

        public void PerformanceComparison()
        {
            const int iterations = 10000000;
            
            // Sequential
            var sw = Stopwatch.StartNew();
            long sequentialSum = 0;
            for (int i = 0; i < iterations; i++)
            {
                sequentialSum += i;
            }
            sw.Stop();
            Console.WriteLine($"   Sequential: {sw.ElapsedMilliseconds}ms, sum: {sequentialSum}");
            
            // Parallel
            sw.Restart();
            long parallelSum = 0;
            Parallel.For(0, iterations, i =>
            {
                Interlocked.Add(ref parallelSum, i);
            });
            sw.Stop();
            Console.WriteLine($"   Parallel: {sw.ElapsedMilliseconds}ms, sum: {parallelSum}");
        }

        public void ParallelForExceptionHandling()
        {
            try
            {
                Parallel.For(0, 10, i =>
                {
                    if (i == 5)
                        throw new Exception($"Error at {i}");
                });
            }
            catch (AggregateException ae)
            {
                Console.WriteLine($"   Caught: {ae.InnerExceptions.Count} exceptions");
            }
        }

        public void ParallelForOptions()
        {
            var options = new ParallelOptions
            {
                MaxDegreeOfParallelism = 4, // Limit to 4 threads
                CancellationToken = CancellationToken.None
            };

            int count = 0;
            Parallel.For(0, 20, options, i =>
            {
                Interlocked.Increment(ref count);
                Thread.Sleep(10);
            });

            Console.WriteLine($"   Processed: {count}");
        }
    }

    // Real-world examples
    public class MatrixMultiplier
    {
        public double[,] Multiply(double[,] a, double[,] b, int size)
        {
            var result = new double[size, size];

            Parallel.For(0, size, i =>
            {
                for (int j = 0; j < size; j++)
                {
                    for (int k = 0; k < size; k++)
                    {
                        result[i, j] += a[i, k] * b[k, j];
                    }
                }
            });

            return result;
        }

        public double[,] MultiplyOptimized(double[,] a, double[,] b, int size)
        {
            var result = new double[size, size];

            Parallel.For(0, size, i =>
            {
                for (int k = 0; k < size; k++)
                {
                    double ai = a[i, k];
                    for (int j = 0; j < size; j++)
                    {
                        result[i, j] += ai * b[k, j];
                    }
                }
            });

            return result;
        }
    }

    public class DataProcessor
    {
        public void ProcessArray(double[] data, Func<double, double> transform)
        {
            Parallel.For(0, data.Length, i =>
            {
                data[i] = transform(data[i]);
            });
        }

        public double[] ProcessWithLocalState(double[] data)
        {
            var results = new double[data.Length];
            
            Parallel.For(0, data.Length,
                () => 0.0,
                (i, loop, localMax) => 
                {
                    var value = data[i];
                    results[i] = value;
                    return Math.Max(localMax, value);
                },
                localMax => { } // Aggregation
            );

            return results;
        }
    }

    public class ImageProcessor
    {
        public byte[,] ApplyFilter(byte[,] pixels, Func<byte, byte> filter)
        {
            int rows = pixels.GetLength(0);
            int cols = pixels.GetLength(1);
            var result = new byte[rows, cols];

            Parallel.For(0, rows, r =>
            {
                for (int c = 0; c < cols; c++)
                {
                    result[r, c] = filter(pixels[r, c]);
                }
            });

            return result;
        }
    }
}