/*
 * TOPIC: Language Integrated Query (LINQ)
 * SUBTOPIC: LINQ Basics - More Basics
 * FILE: LINQ_Basics_Part2.cs
 * PURPOSE: Deferred vs Immediate execution, understanding query execution models
 */
using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharp_MasterGuide._03_Advanced_OOP._04_LINQ
{
    public class LINQ_Basics_Part2
    {
        public static void Main()
        {
            // Understanding Execution Models
            
            // DEFERRED EXECUTION (Lazy Evaluation)
            // The query is NOT executed when defined - only when enumerated
            // Each time you enumerate, the query runs fresh with current data
            
            Console.WriteLine("=== Deferred Execution ===");
            
            var numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            
            // Query is defined but NOT executed yet
            var deferredQuery = numbers.Where(n => n > 5);
            
            // Modify original collection
            numbers.Add(11);
            numbers.Add(12);
            
            // Now execute the query - gets updated data
            Console.WriteLine("After adding more numbers:");
            foreach (var n in deferredQuery)
            {
                Console.WriteLine(n); // Output: 6, 7, 8, 9, 10, 11, 12
            }

            // IMMEDIATE EXECUTION (Eager Evaluation)
            // Query is executed immediately and results are stored
            
            Console.WriteLine("\n=== Immediate Execution ===");
            
            var numbers2 = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            
            // ToList() executes immediately
            var immediateQuery = numbers2.Where(n => n > 5).ToList();
            
            // Modify original collection
            numbers2.Add(11);
            numbers2.Add(12);
            
            // Query result doesn't change
            Console.WriteLine("After adding more numbers:");
            foreach (var n in immediateQuery)
            {
                Console.WriteLine(n); // Output: 6, 7, 8, 9, 10
            }

            // Methods that cause IMMEDIATE execution:
            // ToList(), ToArray(), ToDictionary(), ToHashSet()
            // First(), Last(), Single(), Count(), Sum(), Average(), etc.
            
            Console.WriteLine("\n=== More Immediate Execution Examples ===");
            
            var data = new List<int> { 10, 20, 30, 40, 50 };
            
            // Count() executes immediately
            int count = data.Count(n => n > 25);
            Console.WriteLine($"Count > 25: {count}"); // Output: Count > 25: 3
            
            // First() executes immediately
            int first = data.First(n => n > 25);
            Console.WriteLine($"First > 25: {first}"); // Output: First > 25: 30
            
            // REAL WORLD EXAMPLE: Track order changes
            Console.WriteLine("\n=== Real World: Order Tracking ===");
            
            var orders = new List<Order>
            {
                new Order { Id = 1, Customer = "John", Total = 150.00m, Status = "Pending" },
                new Order { Id = 2, Customer = "Jane", Total = 200.00m, Status = "Shipped" },
                new Order { Id = 3, Customer = "Bob", Total = 75.00m, Status = "Pending" }
            };
            
            // Deferred query - will reflect new orders
            var pendingOrders = orders.Where(o => o.Status == "Pending");
            
            // Add new pending order
            orders.Add(new Order { Id = 4, Customer = "Alice", Total = 300.00m, Status = "Pending" });
            
            // Query reflects the new order
            Console.WriteLine("Pending Orders (includes new):");
            foreach (var order in pendingOrders)
            {
                Console.WriteLine($"  Order #{order.Id} - {order.Customer}: ${order.Total}"); 
                // Output: John: $150, Bob: $75, Alice: $300
            }
            
            // REAL WORLD EXAMPLE: Cache query results
            Console.WriteLine("\n=== Real World: Caching Results ===");
            
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Laptop", Price = 999.99m },
                new Product { Id = 2, Name = "Mouse", Price = 29.99m },
                new Product { Id = 3, Name = "Keyboard", Price = 79.99m }
            };
            
            // Immediate execution with ToList for caching
            var cachedProducts = products
                .Where(p => p.Price < 500)
                .OrderBy(p => p.Price)
                .ToList(); // Execute now, cache results
            
            // Even if products list changes, cachedProducts stays the same
            products.Add(new Product { Id = 4, Name = "Monitor", Price = 399.99m });
            
            Console.WriteLine("Cached Products (unchanged):");
            foreach (var p in cachedProducts)
            {
                Console.WriteLine($"  {p.Name}: ${p.Price}"); 
                // Output: Mouse: $29.99, Keyboard: $79.99
            }
        }
    }

    public class Order
    {
        public int Id { get; set; }
        public string Customer { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; }
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}