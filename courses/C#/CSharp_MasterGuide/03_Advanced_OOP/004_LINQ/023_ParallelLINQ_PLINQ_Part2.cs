/*
 * TOPIC: Language Integrated Query (LINQ)
 * SUBTOPIC: Parallel LINQ (PLINQ) - Part 2
 * FILE: ParallelLINQ_PLINQ_Part2.cs
 * PURPOSE: PLINQ ordering, merging, and advanced patterns
 */
using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharp_MasterGuide._03_Advanced_OOP._04_LINQ
{
    public class ParallelLINQ_PLINQ_Part2
    {
        public static void Main()
        {
            // Merge options - how results are combined
            Console.WriteLine("=== Merge Options ===");
            
            var numbers = Enumerable.Range(1, 20);
            
            // DefaultMergeOptions.Auto - lets PLINQ decide
            var autoMerged = numbers
                .AsParallel()
                .Select(n => n * 2);
            
            // NotBuffered - yields results as soon as ready
            Console.WriteLine("NotBuffered (yields immediately):");
            var notBuffered = numbers
                .AsParallel()
                .WithMergeOptions(ParallelMergeOptions.NotBuffered)
                .Select(n => 
                {
                    Console.WriteLine($"  Processing {n}");
                    return n * 2;
                })
                .Take(5);
            
            foreach (var n in notBuffered)
            {
                // Results stream as they complete
            }
            
            // FullyBuffered - collects all before returning
            Console.WriteLine("\nFullyBuffered (all at once):");
            var fullyBuffered = numbers
                .AsParallel()
                .WithMergeOptions(ParallelMergeOptions.FullyBuffered)
                .Select(n => n * 2)
                .Take(5);
            
            foreach (var n in fullyBuffered)
            {
                Console.WriteLine(n); // Output: 2, 4, 6, 8, 10
            }

            // ForAll - iterate without ordering overhead
            Console.WriteLine("\n=== ForAll ===");
            
            var data = Enumerable.Range(1, 100).ToList();
            
            // ForAll processes in parallel, no ordering guaranteed
            data
                .AsParallel()
                .Where(n => n % 2 == 0)
                .ForAll(n => 
                {
                    // This runs in parallel
                    Console.WriteLine($"  Processed {n} on thread {System.Threading.Thread.CurrentThread.ManagedThreadId}");
                });

            // ForAll vs foreach
            Console.WriteLine("\nForAll vs ToList + foreach:");
            
            var results = new List<int>();
            
            // ForAll - parallel
            data.AsParallel()
                .Where(n => n > 50)
                .ForAll(r => results.Add(r));
            
            // ToList + foreach - sequential
            var sequentialResults = data
                .Where(n => n > 50)
                .ToList();
            
            Console.WriteLine($"ForAll results count: {results.Count}");
            Console.WriteLine($"Sequential results count: {sequentialResults.Count}");

            // Order preservation with AsOrdered
            Console.WriteLine("\n=== Order Preservation ===");
            
            var unordered = new List<int> { 5, 2, 8, 1, 9, 3, 7, 4, 6 };
            
            // AsOrdered maintains sequence
            var ordered = unordered
                .AsParallel()
                .AsOrdered()
                .Select(n => n * 2)
                .Take(5);
            
            Console.WriteLine("Ordered parallel:");
            foreach (var n in ordered)
            {
                Console.WriteLine(n); // Output: 2, 4, 6, 8, 10 (in order)
            }
            
            // AsSequential - convert back to sequential
            Console.WriteLine("\n=== AsSequential ===");
            
            var sequential = unordered
                .AsParallel()
                .Select(n => n * 2)         // Parallel
                .AsSequential()            // Back to sequential
                .Where(n => n > 5);       // Sequential
            
            foreach (var n in sequential)
            {
                Console.WriteLine(n); // Output: 6, 8, 10, 12, 14, 16, 18
            }

            // Aggregate with seed - parallel reduction
            Console.WriteLine("\n=== Parallel Aggregate ===");
            
            var values = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            
            // Parallel aggregate - each thread accumulates locally, then combines
            var parallelSum = values
                .AsParallel()
                .Aggregate(
                    seed: 0,                          // Initial value
                    (localSum, item) => localSum + item // Local accumulation
                );
            
            Console.WriteLine($"Parallel sum: {parallelSum}"); // Output: 55
            
            // Aggregate with multiple accumulators
            var stats = values
                .AsParallel()
                .Aggregate(
                    seed: (Sum: 0, Count: 0, Min: int.MaxValue, Max: int.MinValue),
                    (acc, n) => (acc.Sum + n, acc.Count + 1, Math.Min(acc.Min, n), Math.Max(acc.Max, n)),
                    (acc1, acc2) => (acc1.Sum + acc2.Sum, acc1.Count + acc2.Count, Math.Min(acc1.Min, acc2.Min), Math.Max(acc1.Max, acc2.Max))
                );
            
            Console.WriteLine($"Stats: Sum={stats.Sum}, Count={stats.Count}, Min={stats.Min}, Max={stats.Max}");
            // Output: Sum=55, Count=10, Min=1, Max=10

            // REAL WORLD EXAMPLE: Parallel document processing
            Console.WriteLine("\n=== Real World: Document Processing ===");
            
            var documents = new List<Document>
            {
                new Document { Id = 1, Content = "Important report" },
                new Document { Id = 2, Content = "Meeting notes" },
                new Document { Id = 3, Content = "Project plan" },
                new Document { Id = 4, Content = "Budget analysis" }
            };
            
            var processedDocs = documents
                .AsParallel()
                .Select(doc => new 
                {
                    doc.Id,
                    WordCount = CountWords(doc.Content),
                    HasKeyword = doc.Content.Contains("project")
                })
                .ToList();
            
            Console.WriteLine("Processed documents:");
            foreach (var doc in processedDocs)
            {
                Console.WriteLine($"  Doc {doc.Id}: {doc.WordCount} words, Has 'project': {doc.HasKeyword}");
            }

            // REAL WORLD EXAMPLE: Parallel price calculation
            Console.WriteLine("\n=== Real World: Price Calculation ===");
            
            var products = new List<ProductPrice>();
            for (int i = 0; i < 50; i++)
            {
                products.Add(new ProductPrice { BasePrice = i * 10.0m });
            }
            
            var calculatedPrices = products
                .AsParallel()
                .AsOrdered()
                .Select(p => new
                {
                    Index = p.BasePrice,
                    FinalPrice = CalculatePrice(p.BasePrice)
                })
                .Take(10)
                .ToList();
            
            Console.WriteLine("Calculated prices (first 10):");
            foreach (var price in calculatedPrices)
            {
                Console.WriteLine($"  Index {price.Index}: ${price.FinalPrice:F2}");
            }
        }

        static int CountWords(string text)
        {
            return text.Split(' ').Length;
        }

        static decimal CalculatePrice(decimal basePrice)
        {
            // Simulate computation
            var tax = basePrice * 0.1m;
            var shipping = basePrice > 100 ? 0 : 10m;
            return basePrice + tax + shipping;
        }
    }

    public class Document
    {
        public int Id { get; set; }
        public string Content { get; set; }
    }

    public class ProductPrice
    {
        public decimal BasePrice { get; set; }
    }
}