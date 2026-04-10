/*
 * TOPIC: Language Integrated Query (LINQ)
 * SUBTOPIC: LINQ Partitioning Operations
 * FILE: LINQ_Partitioning.cs
 * PURPOSE: Take, Skip, TakeWhile, SkipWhile partitioning
 */
using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharp_MasterGuide._03_Advanced_OOP._04_LINQ
{
    public class LINQ_Partitioning
    {
        public static void Main()
        {
            // Take - take first N elements
            Console.WriteLine("=== Take ===");
            
            var numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            
            var firstFive = numbers.Take(5);
            
            Console.WriteLine("First 5 elements:");
            foreach (var n in firstFive)
            {
                Console.WriteLine(n); // Output: 1, 2, 3, 4, 5
            }
            
            // Take with predicate (.NET 6+)
            Console.WriteLine("\nTake while condition:");
            var takenWhile = numbers.Take(n => n <= 5);
            
            foreach (var n in takenWhile)
            {
                Console.WriteLine(n); // Output: 1, 2, 3, 4, 5
            }

            // Skip - skip first N elements
            Console.WriteLine("\n=== Skip ===");
            
            var skipFive = numbers.Skip(5);
            
            Console.WriteLine("Skip first 5:");
            foreach (var n in skipFive)
            {
                Console.WriteLine(n); // Output: 6, 7, 8, 9, 10
            }
            
            // Skip with predicate (.NET 6+)
            Console.WriteLine("\nSkip while condition:");
            var skippedWhile = numbers.Skip(n => n <= 5);
            
            foreach (var n in skippedWhile)
            {
                Console.WriteLine(n); // Output: 6, 7, 8, 9, 10
            }

            // TakeWhile - take elements while condition is true
            Console.WriteLine("\n=== TakeWhile ===");
            
            var seq = new List<int> { 1, 2, 3, 4, 5, 1, 2, 3 };
            
            var taken = seq.TakeWhile(n => n <= 3);
            
            Console.WriteLine("Take while <= 3:");
            foreach (var n in taken)
            {
                Console.WriteLine(n); // Output: 1, 2, 3
            }
            
            // TakeWhile with index
            Console.WriteLine("\nTakeWhile with index:");
            var withIndex = seq.TakeWhile((n, i) => i < 4);
            
            foreach (var n in withIndex)
            {
                Console.WriteLine(n); // Output: 1, 2, 3, 4
            }

            // SkipWhile - skip elements while condition is true
            Console.WriteLine("\n=== SkipWhile ===");
            
            var skipped = seq.SkipWhile(n => n <= 3);
            
            Console.WriteLine("Skip while <= 3:");
            foreach (var n in skipped)
            {
                Console.WriteLine(n); // Output: 4, 5, 1, 2, 3
            }
            
            // SkipWhile with index
            Console.WriteLine("\nSkipWhile with index:");
            var skipIndex = seq.SkipWhile((n, i) => i < 4);
            
            foreach (var n in skipIndex)
            {
                Console.WriteLine(n); // Output: 5, 1, 2, 3
            }

            // Chunk - split into arrays of size N (.NET 6+)
            Console.WriteLine("\n=== Chunk ===");
            
            var chunked = numbers.Chunk(3);
            
            foreach (var chunk in chunked)
            {
                Console.WriteLine($"Chunk: {string.Join(", ", chunk)}");
                // Output: 1, 2, 3 | 4, 5, 6 | 7, 8, 9 | 10
            }

            // REAL WORLD EXAMPLE: Pagination
            Console.WriteLine("\n=== Real World: Pagination ===");
            
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Laptop", Price = 999.99m },
                new Product { Id = 2, Name = "Mouse", Price = 29.99m },
                new Product { Id = 3, Name = "Keyboard", Price = 79.99m },
                new Product { Id = 4, Name = "Monitor", Price = 399.99m },
                new Product { Id = 5, Name = "Chair", Price = 149.99m },
                new Product { Id = 6, Name = "Desk", Price = 299.99m },
                new Product { Id = 7, Name = "Headset", Price = 99.99m },
                new Product { Id = 8, Name = "Webcam", Price = 79.99m }
            };
            
            int pageSize = 3;
            int pageNumber = 2;
            
            var pagedProducts = products
                .OrderBy(p => p.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);
            
            Console.WriteLine($"Page {pageNumber} (size {pageSize}):");
            foreach (var p in pagedProducts)
            {
                Console.WriteLine($"  {p.Name}: ${p.Price}");
                // Output: Monitor $399.99, Chair $149.99, Desk $299.99
            }

            // REAL WORLD EXAMPLE: Top N performers
            Console.WriteLine("\n=== Real World: Top Performers ===");
            
            var employees = new List<Employee>
            {
                new Employee { Name = "John", Sales = 50000 },
                new Employee { Name = "Jane", Sales = 75000 },
                new Employee { Name = "Bob", Sales = 45000 },
                new Employee { Name = "Alice", Sales = 80000 },
                new Employee { Name = "Charlie", Sales = 60000 },
                new Employee { Name = "Diana", Sales = 55000 }
            };
            
            // Top 3 by sales
            var topThree = employees
                .OrderByDescending(e => e.Sales)
                .Take(3);
            
            Console.WriteLine("Top 3 Sales Representatives:");
            int rank = 1;
            foreach (var emp in topThree)
            {
                Console.WriteLine($"  #{rank++} {emp.Name}: ${emp.Sales:N0}");
                // Output: #1 Alice: $80000, #2 Jane: $75000, #3 Charlie: $60000
            }

            // REAL WORLD EXAMPLE: Process until condition met
            Console.WriteLine("\n=== Real World: Process Until Condition ===");
            
            var transactions = new List<Transaction>
            {
                new Transaction { Id = 1, Amount = 100.00m, Status = "Pending" },
                new Transaction { Id = 2, Amount = 200.00m, Status = "Completed" },
                new Transaction { Id = 3, Amount = 150.00m, Status = "Completed" },
                new Transaction { Id = 4, Amount = 300.00m, Status = "Pending" },
                new Transaction { Id = 5, Amount = 75.00m, Status = "Completed" }
            };
            
            // Process pending transactions in order
            var pendingToProcess = transactions
                .OrderBy(t => t.Id)
                .TakeWhile(t => t.Status == "Pending");
            
            Console.WriteLine("Transactions to process:");
            decimal runningTotal = 0;
            foreach (var t in pendingToProcess)
            {
                runningTotal += t.Amount;
                Console.WriteLine($"  #{t.Id}: ${t.Amount}");
            }
            Console.WriteLine($"Running total: ${runningTotal}");
            // Output: #1: $100 (stops because #2 is Completed)
        }
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }

    public class Employee
    {
        public string Name { get; set; }
        public decimal Sales { get; set; }
    }

    public class Transaction
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
    }
}