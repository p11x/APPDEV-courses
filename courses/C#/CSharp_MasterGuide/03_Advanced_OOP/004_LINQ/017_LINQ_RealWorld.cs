/*
 * TOPIC: Language Integrated Query (LINQ)
 * SUBTOPIC: LINQ Real-World Examples - Part 1
 * FILE: LINQ_RealWorld.cs
 * PURPOSE: Database-style queries, filtering real-world data
 */
using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharp_MasterGuide._03_Advanced_OOP._04_LINQ
{
    public class LINQ_RealWorld
    {
        public static void Main()
        {
            // Real-world scenario: Employee database query
            Console.WriteLine("=== Real World: Employee Database ===");
            
            var employees = new List<Employee>
            {
                new Employee { Id = 1, Name = "John Smith", Department = "Engineering", 
                    Salary = 85000, StartDate = new DateTime(2020, 3, 15), Active = true },
                new Employee { Id = 2, Name = "Jane Doe", Department = "Marketing", 
                    Salary = 65000, StartDate = new DateTime(2019, 7, 1), Active = true },
                new Employee { Id = 3, Name = "Bob Johnson", Department = "Engineering", 
                    Salary = 90000, StartDate = new DateTime(2018, 1, 10), Active = true },
                new Employee { Id = 4, Name = "Alice Brown", Department = "HR", 
                    Salary = 55000, StartDate = new DateTime(2021, 6, 1), Active = false },
                new Employee { Id = 5, Name = "Charlie Wilson", Department = "Engineering", 
                    Salary = 75000, StartDate = new DateTime(2020, 9, 1), Active = true }
            };
            
            // Filter active employees in Engineering
            var activeEngineers = employees
                .Where(e => e.Active && e.Department == "Engineering")
                .OrderByDescending(e => e.Salary);
            
            Console.WriteLine("Active Engineers (by salary desc):");
            foreach (var emp in activeEngineers)
            {
                Console.WriteLine($"  {emp.Name}: ${emp.Salary:N0}");
                // Output: Bob Johnson: $90,000, John Smith: $85,000, Charlie Wilson: $75,000
            }

            // Real-world scenario: Product inventory management
            Console.WriteLine("\n=== Real World: Inventory Management ===");
            
            var products = new List<Product>
            {
                new Product { SKU = "LAP001", Name = "Gaming Laptop", Category = "Electronics", 
                    Price = 1299.99m, Stock = 15, ReorderLevel = 10 },
                new Product { SKU = "MOU001", Name = "Wireless Mouse", Category = "Electronics", 
                    Price = 29.99m, Stock = 5, ReorderLevel = 20 },
                new Product { SKU = "KEY001", Name = "Mechanical Keyboard", Category = "Electronics", 
                    Price = 149.99m, Stock = 45, ReorderLevel = 15 },
                new Product { SKU = "CHR001", Name = "Office Chair", Category = "Furniture", 
                    Price = 299.99m, Stock = 8, ReorderLevel = 10 },
                new Product { SKU = "DSK001", Name = "Standing Desk", Category = "Furniture", 
                    Price = 549.99m, Stock = 3, ReorderLevel = 5 },
                new Product { SKU = "MON001", Name = "4K Monitor", Category = "Electronics", 
                    Price = 449.99m, Stock = 25, ReorderLevel = 10 }
            };
            
            // Find products needing reorder
            var reorderNeeded = products
                .Where(p => p.Stock <= p.ReorderLevel)
                .OrderBy(p => p.Stock)
                .Select(p => new
                {
                    p.SKU,
                    p.Name,
                    p.Category,
                    p.Stock,
                    ReorderQty = p.ReorderLevel * 2 - p.Stock
                });
            
            Console.WriteLine("Products needing reorder:");
            foreach (var p in reorderNeeded)
            {
                Console.WriteLine($"  {p.SKU} - {p.Name}: Stock={p.Stock}, Reorder Qty={p.ReorderQty}");
                // Output: MOU001, DSK001, CHR001
            }

            // Real-world scenario: Customer order filtering
            Console.WriteLine("\n=== Real World: Order Filtering ===");
            
            var orders = new List<Order>
            {
                new Order { OrderId = 1001, CustomerId = 101, OrderDate = new DateTime(2024, 1, 15), 
                    Status = "Delivered", Total = 250.00m, Items = 3 },
                new Order { OrderId = 1002, CustomerId = 102, OrderDate = new DateTime(2024, 1, 20), 
                    Status = "Shipped", Total = 75.00m, Items = 1 },
                new Order { OrderId = 1003, CustomerId = 101, OrderDate = new DateTime(2024, 2, 5), 
                    Status = "Pending", Total = 500.00m, Items = 5 },
                new Order { OrderId = 1004, CustomerId = 103, OrderDate = new DateTime(2024, 2, 10), 
                    Status = "Delivered", Total = 150.00m, Items = 2 },
                new Order { OrderId = 1005, CustomerId = 102, OrderDate = new DateTime(2024, 2, 15), 
                    Status = "Processing", Total = 1000.00m, Items = 8 }
            };
            
            // Find high-value pending orders
            var highValuePending = orders
                .Where(o => o.Status == "Pending" || o.Status == "Processing")
                .Where(o => o.Total > 200)
                .OrderByDescending(o => o.Total);
            
            Console.WriteLine("High-value pending orders:");
            foreach (var o in highValuePending)
            {
                Console.WriteLine($"  Order #{o.OrderId}: ${o.Total} ({o.Status})");
                // Output: Order #1003: $500 (Pending), Order #1005: $1000 (Processing)
            }

            // Real-world scenario: Transaction log analysis
            Console.WriteLine("\n=== Real World: Transaction Analysis ===");
            
            var transactions = new List<Transaction>
            {
                new Transaction { Id = 1, Type = "Credit", Amount = 500.00m, 
                    Account = "ACC001", Timestamp = new DateTime(2024, 1, 1, 10, 0, 0) },
                new Transaction { Id = 2, Type = "Debit", Amount = 150.00m, 
                    Account = "ACC001", Timestamp = new DateTime(2024, 1, 1, 14, 30, 0) },
                new Transaction { Id = 3, Type = "Credit", Amount = 1000.00m, 
                    Account = "ACC002", Timestamp = new DateTime(2024, 1, 2, 9, 0, 0) },
                new Transaction { Id = 4, Type = "Debit", Amount = 200.00m, 
                    Account = "ACC001", Timestamp = new DateTime(2024, 1, 2, 11, 0, 0) },
                new Transaction { Id = 5, Type = "Credit", Amount = 750.00m, 
                    Account = "ACC003", Timestamp = new DateTime(2024, 1, 3, 15, 0, 0) }
            };
            
            // Filter credit transactions over $500
            var largeCredits = transactions
                .Where(t => t.Type == "Credit" && t.Amount > 500)
                .OrderByDescending(t => t.Amount);
            
            Console.WriteLine("Large credit transactions:");
            foreach (var t in largeCredits)
            {
                Console.WriteLine($"  {t.Account}: +${t.Amount} at {t.Timestamp:HH:mm}");
                // Output: ACC002: +$1000, ACC003: +$750
            }

            // Real-world scenario: Student grade filtering
            Console.WriteLine("\n=== Real World: Student Performance ===");
            
            var students = new List<Student>
            {
                new Student { Id = 1, Name = "Emily", Math = 85, Science = 92, English = 78 },
                new Student { Id = 2, Name = "Michael", Math = 95, Science = 88, English = 90 },
                new Student { Id = 3, Name = "Sarah", Math = 72, Science = 75, English = 80 },
                new Student { Id = 4, Name = "David", Math = 88, Science = 85, English = 87 },
                new Student { Id = 5, Name = "Lisa", Math = 91, Science = 94, English = 89 }
            };
            
            // Find students with all subjects above 80
            var topPerformers = students
                .Where(s => s.Math > 80 && s.Science > 80 && s.English > 80)
                .Select(s => new
                {
                    s.Name,
                    Average = (s.Math + s.Science + s.English) / 3.0,
                    Lowest = Math.Min(s.Math, Math.Min(s.Science, s.English))
                })
                .OrderByDescending(s => s.Average);
            
            Console.WriteLine("Students with all scores > 80:");
            foreach (var s in topPerformers)
            {
                Console.WriteLine($"  {s.Name}: Avg={s.Average:F1}, Lowest={s.Lowest}");
                // Output: Michael, David, Lisa
            }
        }
    }

    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public decimal Salary { get; set; }
        public DateTime StartDate { get; set; }
        public bool Active { get; set; }
    }

    public class Product
    {
        public string SKU { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int ReorderLevel { get; set; }
    }

    public class Order
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public decimal Total { get; set; }
        public int Items { get; set; }
    }

    public class Transaction
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }
        public string Account { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Math { get; set; }
        public int Science { get; set; }
        public int English { get; set; }
    }
}