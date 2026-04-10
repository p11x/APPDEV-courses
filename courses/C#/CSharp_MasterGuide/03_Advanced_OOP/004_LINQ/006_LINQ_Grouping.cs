/*
 * TOPIC: Language Integrated Query (LINQ)
 * SUBTOPIC: LINQ Grouping Operations
 * FILE: LINQ_Grouping.cs
 * PURPOSE: GroupBy - grouping data by keys
 */
using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharp_MasterGuide._03_Advanced_OOP._04_LINQ
{
    public class LINQ_Grouping
    {
        public static void Main()
        {
            // GroupBy - group elements by a key
            Console.WriteLine("=== GroupBy - Basic Grouping ===");
            
            var products = new List<Product>
            {
                new Product { Name = "Laptop", Category = "Electronics", Price = 999.99m },
                new Product { Name = "Mouse", Category = "Electronics", Price = 29.99m },
                new Product { Name = "Chair", Category = "Furniture", Price = 149.99m },
                new Product { Name = "Keyboard", Category = "Electronics", Price = 79.99m },
                new Product { Name = "Desk", Category = "Furniture", Price = 299.99m }
            };
            
            // Group by category
            var grouped = products.GroupBy(p => p.Category);
            
            foreach (var group in grouped)
            {
                Console.WriteLine($"\nCategory: {group.Key}");
                foreach (var product in group)
                {
                    Console.WriteLine($"  - {product.Name}: ${product.Price}");
                    // Output: Electronics: Laptop, Mouse, Keyboard
                    // Output: Furniture: Chair, Desk
                }
            }

            // GroupBy with multiple keys (anonymous type)
            Console.WriteLine("\n=== GroupBy with Multiple Keys ===");
            
            var sales = new List<Sale>
            {
                new Sale { Product = "Laptop", Region = "North", Amount = 5000 },
                new Sale { Product = "Mouse", Region = "South", Amount = 1500 },
                new Sale { Product = "Laptop", Region = "South", Amount = 3000 },
                new Sale { Product = "Keyboard", Region = "North", Amount = 2000 },
                new Sale { Product = "Mouse", Region = "North", Amount = 1000 }
            };
            
            // Group by product AND region
            var groupedByProductRegion = sales.GroupBy(s => new { s.Product, s.Region });
            
            foreach (var group in groupedByProductRegion)
            {
                Console.WriteLine($"{group.Key.Product} - {group.Key.Region}: ${group.Sum(s => s.Amount)}");
                // Output: Laptop - North: $5000, Mouse - South: $1500, etc.
            }

            // GroupBy with element selector
            Console.WriteLine("\n=== GroupBy with Element Selector ===");
            
            var groupedNames = products.GroupBy(
                p => p.Category,           // Key selector
                p => p.Name                // Element selector
            );
            
            foreach (var group in groupedNames)
            {
                Console.WriteLine($"{group.Key}: {string.Join(", ", group.ToList())}");
                // Output: Electronics: Laptop, Mouse, Keyboard
                // Output: Furniture: Chair, Desk
            }

            // GroupBy with result selector
            Console.WriteLine("\n=== GroupBy with Result Selector ===");
            
            var stats = products.GroupBy(
                p => p.Category,
                (category, items) => new
                {
                    Category = category,
                    Count = items.Count(),
                    TotalValue = items.Sum(p => p.Price),
                    AvgPrice = items.Average(p => p.Price)
                }
            );
            
            foreach (var stat in stats)
            {
                Console.WriteLine($"{stat.Category}: {stat.Count} items, " +
                    $"Total: ${stat.TotalValue:F2}, Avg: ${stat.AvgPrice:F2}");
                // Output: Electronics: 3 items, Total: $1109.97, Avg: $369.99
                // Output: Furniture: 2 items, Total: $449.98, Avg: $224.99
            }

            // REAL WORLD EXAMPLE: Employee by department
            Console.WriteLine("\n=== Real World: Employee by Department ===");
            
            var employees = new List<Employee>
            {
                new Employee { Name = "John", Department = "IT", Salary = 75000 },
                new Employee { Name = "Jane", Department = "HR", Salary = 65000 },
                new Employee { Name = "Bob", Department = "IT", Salary = 80000 },
                new Employee { Name = "Alice", Department = "Finance", Salary = 70000 },
                new Employee { Name = "Charlie", Department = "HR", Salary = 60000 }
            };
            
            var byDept = employees.GroupBy(e => e.Department);
            
            foreach (var dept in byDept)
            {
                Console.WriteLine($"\n{dept.Key} Department:");
                foreach (var emp in dept)
                {
                    Console.WriteLine($"  {emp.Name}: ${emp.Salary}");
                    // Output: IT: John $75000, Bob $80000
                    // Output: HR: Jane $65000, Charlie $60000
                }
            }

            // REAL WORLD EXAMPLE: Order statistics by status
            Console.WriteLine("\n=== Real World: Orders by Status ===");
            
            var orders = new List<Order>
            {
                new Order { Id = 1, Status = "Pending", Total = 150.00m },
                new Order { Id = 2, Status = "Shipped", Total = 200.00m },
                new Order { Id = 3, Status = "Pending", Total = 75.00m },
                new Order { Id = 4, Status = "Delivered", Total = 300.00m },
                new Order { Id = 5, Status = "Shipped", Total = 450.00m },
                new Order { Id = 6, Status = "Pending", Total = 100.00m }
            };
            
            var orderStats = orders.GroupBy(o => o.Status)
                .Select(g => new
                {
                    Status = g.Key,
                    Count = g.Count(),
                    TotalValue = g.Sum(o => o.Total)
                });
            
            foreach (var stat in orderStats)
            {
                Console.WriteLine($"{stat.Status}: {stat.Count} orders, ${stat.TotalValue}");
                // Output: Pending: 3 orders, $325
                // Output: Shipped: 2 orders, $650
                // Output: Delivered: 1 order, $300
            }
        }
    }

    public class Product
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
    }

    public class Sale
    {
        public string Product { get; set; }
        public string Region { get; set; }
        public decimal Amount { get; set; }
    }

    public class Employee
    {
        public string Name { get; set; }
        public string Department { get; set; }
        public decimal Salary { get; set; }
    }

    public class Order
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public decimal Total { get; set; }
    }
}