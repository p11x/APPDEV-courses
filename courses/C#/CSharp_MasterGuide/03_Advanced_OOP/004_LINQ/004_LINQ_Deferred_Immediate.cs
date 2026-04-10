/*
 * TOPIC: Language Integrated Query (LINQ)
 * SUBTOPIC: Deferred vs Immediate Execution
 * FILE: LINQ_Deferred_Immediate.cs
 * PURPOSE: Understanding execution models in LINQ
 */
using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharp_MasterGuide._03_Advanced_OOP._04_LINQ
{
    public class LINQ_Deferred_Immediate
    {
        public static void Main()
        {
            // DEFERRED EXECUTION
            // Query is NOT executed until the result is enumerated
            // The query is re-evaluated each time you iterate
            
            Console.WriteLine("=== Deferred Execution ===");
            
            var numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            
            // Query is defined but NOT executed
            var deferredQuery = numbers
                .Where(n => n > 5)
                .Select(n => n * 2);
            
            Console.WriteLine("Query defined (not executed yet)");
            
            // Modify source collection
            numbers.Add(11);
            numbers.Add(12);
            
            // NOW execute - gets updated data
            Console.WriteLine("After modifying source:");
            foreach (var n in deferredQuery)
            {
                Console.WriteLine(n); // Output: 12, 14, 16, 18, 20, 22, 24
            }
            
            // Re-enumerate - executes again
            Console.WriteLine("Re-enumerating (executes again):");
            foreach (var n in deferredQuery)
            {
                Console.WriteLine(n); // Output: Same as above
            }

            // IMMEDIATE EXECUTION
            // Query executes immediately and returns results
            
            Console.WriteLine("\n=== Immediate Execution ===");
            
            var numbers2 = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            
            // ToList forces immediate execution
            var immediateQuery = numbers2
                .Where(n => n > 5)
                .Select(n => n * 2)
                .ToList(); // Executes NOW
            
            Console.WriteLine("Query executed immediately");
            
            // Modify source
            numbers2.Add(11);
            numbers2.Add(12);
            
            // Result doesn't change - already executed
            Console.WriteLine("After modifying source:");
            foreach (var n in immediateQuery)
            {
                Console.WriteLine(n); // Output: 12, 14, 16, 18, 20
            }

            // Methods that cause IMMEDIATE execution
            Console.WriteLine("\n=== Immediate Execution Methods ===");
            
            var data = new List<int> { 10, 20, 30, 40, 50 };
            
            // ToList, ToArray
            var asList = data.Where(n => n > 25).ToList();
            var asArray = data.Where(n => n > 25).ToArray();
            
            // Element operators (return single value)
            int first = data.First();
            int last = data.Last();
            int single = data.Single(n => n == 30);
            int elementAt = data.ElementAt(2);
            
            Console.WriteLine($"First: {first}"); // Output: 10
            Console.WriteLine($"Last: {last}"); // Output: 50
            Console.WriteLine($"Single(30): {single}"); // Output: 30
            Console.WriteLine($"ElementAt(2): {elementAt}"); // Output: 30
            
            // Aggregation operators
            int count = data.Count();
            int sum = data.Sum();
            double avg = data.Average();
            int max = data.Max();
            int min = data.Min();
            
            Console.WriteLine($"\nCount: {count}"); // Output: 5
            Console.WriteLine($"Sum: {sum}"); // Output: 150
            Console.WriteLine($"Average: {avg}"); // Output: 30
            Console.WriteLine($"Max: {max}"); // Output: 50
            Console.WriteLine($"Min: {min}"); // Output: 10

            // REAL WORLD EXAMPLE: Dynamic data filtering
            Console.WriteLine("\n=== Real World: Dynamic Filtering ===");
            
            var inventory = new List<InventoryItem>
            {
                new InventoryItem { Id = 1, Name = "Laptop", Quantity = 10, Price = 999.99m },
                new InventoryItem { Id = 2, Name = "Mouse", Quantity = 5, Price = 29.99m },
                new InventoryItem { Id = 3, Name = "Keyboard", Quantity = 15, Price = 79.99m }
            };
            
            // Deferred - will reflect future changes
            var lowStock = inventory.Where(i => i.Quantity < 10);
            
            // Add new low stock item
            inventory.Add(new InventoryItem { Id = 4, Name = "Monitor", Quantity = 3, Price = 399.99m });
            
            Console.WriteLine("Low stock items (deferred):");
            foreach (var item in lowStock)
            {
                Console.WriteLine($"  {item.Name}: {item.Quantity}"); 
                // Output: Mouse: 5, Monitor: 3
            }

            // REAL WORLD EXAMPLE: Caching query results
            Console.WriteLine("\n=== Real World: Caching Results ===");
            
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Laptop", Category = "Electronics", Price = 999.99m },
                new Product { Id = 2, Name = "Chair", Category = "Furniture", Price = 149.99m }
            };
            
            // Immediate execution - cache results
            var cachedResults = products
                .Where(p => p.Price > 100)
                .OrderBy(p => p.Price)
                .ToList(); // Execute and cache
            
            // Even if products change, cachedResults stays same
            products.Add(new Product { Id = 3, Name = "Monitor", Category = "Electronics", Price = 399.99m });
            
            Console.WriteLine("Cached results (unchanged):");
            foreach (var p in cachedResults)
            {
                Console.WriteLine($"  {p.Name}: ${p.Price}"); 
                // Output: Chair: $149.99, Laptop: $999.99
            }

            // REAL WORLD EXAMPLE: Web API data fetching
            Console.WriteLine("\n=== Real World: API Data Handling ===");
            
            var apiResponse = new List<ApiRecord>
            {
                new ApiRecord { Id = 1, Value = "A" },
                new ApiRecord { Id = 2, Value = "B" },
                new ApiRecord { Id = 3, Value = "C" }
            };
            
            // Deferred - might re-fetch on enumeration
            var activeRecords = apiResponse.Where(r => r.Id > 1);
            
            // Simulate API refresh (data changed)
            apiResponse.Clear();
            apiResponse.Add(new ApiRecord { Id = 1, Value = "X" });
            apiResponse.Add(new ApiRecord { Id = 2, Value = "Y" });
            
            Console.WriteLine("After simulated API refresh:");
            foreach (var r in activeRecords)
            {
                Console.WriteLine($"  Id: {r.Id}, Value: {r.Value}"); 
                // Output: Id: 2, Value: Y
            }
        }
    }

    public class InventoryItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
    }

    public class ApiRecord
    {
        public int Id { get; set; }
        public string Value { get; set; }
    }
}