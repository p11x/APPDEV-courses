/*
 * TOPIC: Language Integrated Query (LINQ)
 * SUBTOPIC: Parallel LINQ (PLINQ)
 * FILE: ParallelLINQ_PLINQ.cs
 * PURPOSE: AsParallel, WithDegreeOfParallelism for parallel processing
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CSharp_MasterGuide._03_Advanced_OOP._04_LINQ
{
    public class ParallelLINQ_PLINQ
    {
        public static void Main()
        {
            // AsParallel - enables parallel query execution
            // Splits data into partitions and processes them concurrently
            
            Console.WriteLine("=== AsParallel ===");
            
            var numbers = Enumerable.Range(1, 100);
            
            // Sequential processing
            var sw = Stopwatch.StartNew();
            var sequential = numbers.Select(n => Compute(n)).ToList();
            sw.Stop();
            Console.WriteLine($"Sequential: {sw.ElapsedMilliseconds}ms");
            
            // Parallel processing
            sw.Restart();
            var parallel = numbers.AsParallel().Select(n => Compute(n)).ToList();
            sw.Stop();
            Console.WriteLine($"Parallel: {sw.ElapsedMilliseconds}ms");
            // Note: Parallel may be slower for simple operations due to overhead
            
            // AsOrdered - maintain ordering (default for some operations)
            Console.WriteLine("\n=== AsOrdered ===");
            
            var orderedParallel = numbers
                .AsParallel()
                .AsOrdered() // Maintain original order
                .Select(n => n * 2)
                .Take(10);
            
            Console.WriteLine("First 10 (ordered):");
            foreach (var n in orderedParallel)
            {
                Console.WriteLine(n); // Output: 2, 4, 6, 8, 10, 12, 14, 16, 18, 20
            }
            
            // AsUnordered - better performance when order doesn't matter
            Console.WriteLine("\n=== AsUnordered ===");
            
            var unorderedResult = numbers
                .AsParallel()
                .AsUnordered() // Better performance, no order guaranteed
                .Where(n => n % 10 == 0)
                .Take(5);
            
            Console.WriteLine("Divisible by 10 (unordered):");
            foreach (var n in unorderedResult)
            {
                Console.WriteLine(n); // Output: 10, 20, 30, 40, 50 (likely in order)
            }

            // WithDegreeOfParallelism - limit number of threads
            Console.WriteLine("\n=== WithDegreeOfParallelism ===");
            
            var limitedParallel = numbers
                .AsParallel()
                .WithDegreeOfParallelism(4) // Use at most 4 threads
                .Select(n => Compute(n))
                .Take(20);
            
            Console.WriteLine("Limited to 4 threads:");
            foreach (var n in limitedParallel)
            {
                Console.Write(n + " "); // Output: various values
            }

            // WithCancellation - support cancellation
            Console.WriteLine("\n=== WithCancellation ===");
            
            var cts = new System.Threading.CancellationTokenSource();
            
            try
            {
                var cancellationEnabled = numbers
                    .AsParallel()
                    .WithCancellation(cts.Token)
                    .Select(n =>
                    {
                        if (n > 50)
                        {
                            cts.Cancel(); // Cancel after 50
                        }
                        return n * 2;
                    });
                
                foreach (var n in cancellationEnabled)
                {
                    Console.Write(n + " "); // Output: 2, 4, 6... up to 100
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("\nOperation cancelled!");
            }

            // REAL WORLD EXAMPLE: Process large dataset in parallel
            Console.WriteLine("\n=== Real World: Large Dataset Processing ===");
            
            var products = new List<Product>();
            for (int i = 0; i < 1000; i++)
            {
                products.Add(new Product 
                { 
                    Id = i, 
                    Name = $"Product {i}", 
                    Price = (i + 1) * 10.0m 
                });
            }
            
            // Process expensive products in parallel
            var expensiveProducts = products
                .AsParallel()
                .Where(p => p.Price > 5000)
                .Select(p => new 
                { 
                    p.Name, 
                    PriceWithTax = p.Price * 1.1m 
                })
                .Take(50);
            
            Console.WriteLine("Expensive products (first 10):");
            foreach (var p in expensiveProducts.Take(10))
            {
                Console.WriteLine($"  {p.Name}: ${p.PriceWithTax:F2}");
            }

            // REAL WORLD EXAMPLE: Multiple API calls
            Console.WriteLine("\n=== Real World: Parallel API Calls ===");
            
            var userIds = new List<int> { 1, 2, 3, 4, 5 };
            
            // Simulate parallel user data fetching
            var userData = userIds
                .AsParallel()
                .Select(id => FetchUserData(id))
                .Where(u => u != null);
            
            Console.WriteLine("Fetched users:");
            foreach (var user in userData)
            {
                Console.WriteLine($"  User {user.Id}: {user.Name}");
            }
        }

        // Simulate CPU-bound work
        static int Compute(int n)
        {
            int result = 0;
            for (int i = 0; i < 1000; i++)
            {
                result += i * n;
            }
            return result;
        }

        static User FetchUserData(int id)
        {
            // Simulate API delay
            System.Threading.Thread.Sleep(10);
            return new User { Id = id, Name = $"User {id}" };
        }
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}