/*
 * TOPIC: Language Integrated Query (LINQ)
 * SUBTOPIC: LINQ Method Syntax
 * FILE: LINQ_MethodSyntax.cs
 * PURPOSE: Method syntax (Where, Select, OrderBy)
 */
using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharp_MasterGuide._03_Advanced_OOP._04_LINQ
{
    public class LINQ_MethodSyntax
    {
        public static void Main()
        {
            // Method Syntax uses lambda expressions and extension methods
            // Syntax: collection.Method(lambda)
            
            Console.WriteLine("=== Basic Method Syntax ===");
            
            var numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            
            // Where - filter elements
            var evens = numbers.Where(n => n % 2 == 0);
            
            Console.WriteLine("Even numbers:");
            foreach (var n in evens)
            {
                Console.WriteLine(n); // Output: 2, 4, 6, 8, 10
            }
            
            // Select - transform elements
            Console.WriteLine("\n=== Select ===");
            
            var squared = numbers.Select(n => n * n);
            
            Console.WriteLine("Squared numbers:");
            foreach (var n in squared)
            {
                Console.WriteLine(n); // Output: 1, 4, 9, 16, 25, 36, 49, 64, 81, 100
            }

            // OrderBy / OrderByDescending
            Console.WriteLine("\n=== OrderBy ===");
            
            var names = new List<string> { "Zoe", "Alice", "Bob", "Charlie", "Diana" };
            
            var ascending = names.OrderBy(n => n);
            Console.WriteLine("Ascending:");
            foreach (var name in ascending)
            {
                Console.WriteLine(name); // Output: Alice, Bob, Charlie, Diana, Zoe
            }
            
            var descending = names.OrderByDescending(n => n);
            Console.WriteLine("\nDescending:");
            foreach (var name in descending)
            {
                Console.WriteLine(name); // Output: Zoe, Diana, Charlie, Bob, Alice
            }

            // Combined operations
            Console.WriteLine("\n=== Combined Operations ===");
            
            var result = numbers
                .Where(n => n > 3)          // Filter
                .OrderByDescending(n => n) // Sort
                .Select(n => n * 2);        // Transform
            
            Console.WriteLine("n > 3, desc, doubled:");
            foreach (var n in result)
            {
                Console.WriteLine(n); // Output: 20, 18, 16, 14, 12, 10, 8
            }

            // First, Last, Single
            Console.WriteLine("\n=== Element Operations ===");
            
            var nums = new List<int> { 5, 10, 15, 20, 25 };
            
            int first = nums.First();
            Console.WriteLine($"First: {first}"); // Output: 5
            
            int last = nums.Last();
            Console.WriteLine($"Last: {last}"); // Output: 25
            
            int firstEven = nums.First(n => n > 15);
            Console.WriteLine($"First > 15: {firstEven}"); // Output: 20
            
            // Single - returns only one element, throws if more than one
            var singleNum = nums.Single(n => n == 15);
            Console.WriteLine($"Single(15): {singleNum}"); // Output: 15

            // REAL WORLD EXAMPLE: Product filtering
            Console.WriteLine("\n=== Real World: Product Filter ===");
            
            var products = new List<Product>
            {
                new Product { Name = "Laptop", Price = 999.99m, Category = "Electronics", InStock = true },
                new Product { Name = "Mouse", Price = 29.99m, Category = "Electronics", InStock = true },
                new Product { Name = "Chair", Price = 149.99m, Category = "Furniture", InStock = false },
                new Product { Name = "Keyboard", Price = 79.99m, Category = "Electronics", InStock = true },
                new Product { Name = "Desk", Price = 299.99m, Category = "Furniture", InStock = true }
            };
            
            // Get in-stock electronics under $200
            var affordableAvailable = products
                .Where(p => p.Category == "Electronics" && p.InStock && p.Price < 200)
                .OrderBy(p => p.Price)
                .Select(p => p.Name);
            
            Console.WriteLine("In-stock electronics under $200:");
            foreach (var name in affordableAvailable)
            {
                Console.WriteLine($"  {name}"); // Output: Keyboard, Mouse
            }

            // REAL WORLD EXAMPLE: Customer order history
            Console.WriteLine("\n=== Real World: Customer Orders ===");
            
            var customers = new List<Customer>
            {
                new Customer { Name = "John", Orders = new List<decimal> { 150.00m, 200.00m, 75.00m } },
                new Customer { Name = "Jane", Orders = new List<decimal> { 300.00m } },
                new Customer { Name = "Bob", Orders = new List<decimal> { 50.00m, 100.00m, 150.00m, 200.00m } }
            };
            
            // Find customers with total order > 300, sorted by total
            var topCustomers = customers
                .Select(c => new { 
                    Name = c.Name, 
                    Total = c.Orders.Sum() 
                })
                .Where(c => c.Total > 300)
                .OrderByDescending(c => c.Total);
            
            Console.WriteLine("Customers with total > $300:");
            foreach (var c in topCustomers)
            {
                Console.WriteLine($"  {c.Name}: ${c.Total}"); 
                // Output: Bob: $500, John: $425
            }
        }
    }

    public class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public bool InStock { get; set; }
    }

    public class Customer
    {
        public string Name { get; set; }
        public List<decimal> Orders { get; set; }
    }
}