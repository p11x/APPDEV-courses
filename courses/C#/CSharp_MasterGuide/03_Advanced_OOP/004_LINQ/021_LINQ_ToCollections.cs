/*
 * TOPIC: Language Integrated Query (LINQ)
 * SUBTOPIC: LINQ to Collections
 * FILE: LINQ_ToCollections.cs
 * PURPOSE: ToList, ToArray, ToDictionary, ToHashSet conversions
 */
using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharp_MasterGuide._03_Advanced_OOP._04_LINQ
{
    public class LINQ_ToCollections
    {
        public static void Main()
        {
            // ToList - convert to List<T>
            Console.WriteLine("=== ToList ===");
            
            var numbers = new[] { 1, 2, 3, 4, 5 };
            
            // ToList converts IEnumerable to List
            var list = numbers.Where(n => n > 2).ToList();
            
            list.Add(6); // List is mutable
            Console.WriteLine($"List count: {list.Count}"); // Output: 4
            Console.WriteLine($"List contents: {string.Join(", ", list)}"); // Output: 3, 4, 5, 6

            // ToArray - convert to T[]
            Console.WriteLine("\n=== ToArray ===");
            
            var array = numbers.Where(n => n % 2 == 0).ToArray();
            
            Console.WriteLine($"Array length: {array.Length}"); // Output: 2
            Console.WriteLine($"Array contents: {string.Join(", ", array)}"); // Output: 2, 4

            // ToDictionary - convert to Dictionary<TKey, TValue>
            Console.WriteLine("\n=== ToDictionary ===");
            
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Laptop", Price = 999.99m },
                new Product { Id = 2, Name = "Mouse", Price = 29.99m },
                new Product { Id = 3, Name = "Keyboard", Price = 79.99m }
            };
            
            // ToDictionary with key selector
            var productDict = products.ToDictionary(p => p.Id);
            
            Console.WriteLine("Product by ID:");
            foreach (var kvp in productDict)
            {
                Console.WriteLine($"  ID {kvp.Key}: {kvp.Value.Name}");
                // Output: ID 1: Laptop, ID 2: Mouse, ID 3: Keyboard
            }
            
            // ToDictionary with key and element selector
            var namePriceDict = products.ToDictionary(p => p.Name, p => p.Price);
            
            Console.WriteLine("\nName to Price:");
            foreach (var kvp in namePriceDict)
            {
                Console.WriteLine($"  {kvp.Key}: ${kvp.Value}");
                // Output: Laptop: $999.99, Mouse: $29.99, Keyboard: $79.99
            }

            // ToHashSet - convert to HashSet<T>
            Console.WriteLine("\n=== ToHashSet ===");
            
            var withDuplicates = new List<int> { 1, 2, 2, 3, 3, 3, 4, 4 };
            
            var hashSet = withDuplicates.ToHashSet();
            
            Console.WriteLine($"HashSet count: {hashSet.Count}"); // Output: 4
            Console.WriteLine($"HashSet contents: {string.Join(", ", hashSet)}"); // Output: 1, 2, 3, 4
            
            // ToHashSet with custom equality comparer
            var words = new List<string> { "apple", "APPLE", "banana" };
            var caseInsensitive = words.ToHashSet(StringComparer.OrdinalIgnoreCase);
            
            Console.WriteLine("\nCase-insensitive HashSet:");
            Console.WriteLine($"Count: {caseInsensitive.Count}"); // Output: 2

            // ToLookup - create ILookup (grouped collection)
            Console.WriteLine("\n=== ToLookup ===");
            
            var employees = new List<Employee>
            {
                new Employee { Name = "Alice", Dept = "IT" },
                new Employee { Name = "Bob", Dept = "HR" },
                new Employee { Name = "Charlie", Dept = "IT" },
                new Employee { Name = "Diana", Dept = "HR" }
            };
            
            var byDept = employees.ToLookup(e => e.Dept);
            
            Console.WriteLine("Employees by Department:");
            foreach (var group in byDept)
            {
                Console.WriteLine($"  {group.Key}: {string.Join(", ", group.Select(e => e.Name))}");
                // Output: IT: Alice, Charlie; HR: Bob, Diana
            }
            
            // Access specific group
            var itEmployees = byDept["IT"];
            Console.WriteLine("\nIT Department:");
            foreach (var emp in itEmployees)
            {
                Console.WriteLine($"  {emp.Name}"); // Output: Alice, Charlie
            }

            // REAL WORLD EXAMPLE: Convert query results to different collections
            Console.WriteLine("\n=== Real World: Collection Conversion ===");
            
            var orders = new List<Order>
            {
                new Order { Id = 1, CustomerId = 101, Total = 150.00m },
                new Order { Id = 2, CustomerId = 102, Total = 200.00m },
                new Order { Id = 3, CustomerId = 101, Total = 75.00m }
            };
            
            // Convert to Dictionary by order ID
            var orderDict = orders.ToDictionary(o => o.Id);
            
            // Get specific order
            var order1 = orderDict[1];
            Console.WriteLine($"Order 1 total: ${order1.Total}"); // Output: $150.00
            
            // Group orders by customer and convert to lookup
            var ordersByCustomer = orders.ToLookup(o => o.CustomerId);
            
            Console.WriteLine("\nOrders by Customer:");
            foreach (var group in ordersByCustomer)
            {
                decimal total = group.Sum(o => o.Total);
                Console.WriteLine($"  Customer {group.Key}: {group.Count()} orders, ${total}");
                // Output: Customer 101: 2 orders, $225
                // Output: Customer 102: 1 order, $200
            }

            // REAL WORLD EXAMPLE: Product catalog conversion
            Console.WriteLine("\n=== Real World: Product Catalog ===");
            
            var catalog = new List<Product>
            {
                new Product { Code = "LAP", Name = "Laptop", Price = 999.99m },
                new Product { Code = "MOU", Name = "Mouse", Price = 29.99m },
                new Product { Code = "KEY", Name = "Keyboard", Price = 79.99m }
            };
            
            // Create price lookup
            var priceLookup = catalog.ToDictionary(p => p.Code, p => p.Price);
            
            // Fast O(1) lookup
            Console.WriteLine($"Laptop price: ${priceLookup["LAP"]}"); // Output: $999.99
            Console.WriteLine($"Mouse price: ${priceLookup["MOU"]}"); // Output: $29.99
            
            // Create category-based lookup
            var categories = new List<CategoryProduct>
            {
                new CategoryProduct { Name = "Laptop", Category = "Electronics" },
                new CategoryProduct { Name = "Mouse", Category = "Electronics" },
                new CategoryProduct { Name = "Chair", Category = "Furniture" }
            };
            
            var categoryLookup = categories.ToLookup(c => c.Category);
            
            Console.WriteLine("\nProducts by Category:");
            foreach (var group in categoryLookup)
            {
                Console.WriteLine($"  {group.Key}: {string.Join(", ", group.Select(p => p.Name))}");
                // Output: Electronics: Laptop, Mouse; Furniture: Chair
            }
        }
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Code { get; set; }
    }

    public class Employee
    {
        public string Name { get; set; }
        public string Dept { get; set; }
    }

    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public decimal Total { get; set; }
    }

    public class CategoryProduct
    {
        public string Name { get; set; }
        public string Category { get; set; }
    }
}