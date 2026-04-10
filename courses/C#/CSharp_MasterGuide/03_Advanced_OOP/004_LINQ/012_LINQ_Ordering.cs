/*
 * TOPIC: Language Integrated Query (LINQ)
 * SUBTOPIC: LINQ Ordering Operations
 * FILE: LINQ_Ordering.cs
 * PURPOSE: OrderBy, OrderByDescending, ThenBy ordering
 */
using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharp_MasterGuide._03_Advanced_OOP._04_LINQ
{
    public class LINQ_Ordering
    {
        public static void Main()
        {
            // OrderBy - ascending sort
            Console.WriteLine("=== OrderBy - Ascending ===");
            
            var numbers = new List<int> { 5, 2, 8, 1, 9, 3, 7, 4, 6 };
            
            var ascending = numbers.OrderBy(n => n);
            
            Console.WriteLine("Ascending:");
            foreach (var n in ascending)
            {
                Console.WriteLine(n); // Output: 1, 2, 3, 4, 5, 6, 7, 8, 9
            }

            // OrderByDescending - descending sort
            Console.WriteLine("\n=== OrderByDescending - Descending ===");
            
            var descending = numbers.OrderByDescending(n => n);
            
            Console.WriteLine("Descending:");
            foreach (var n in descending)
            {
                Console.WriteLine(n); // Output: 9, 8, 7, 6, 5, 4, 3, 2, 1
            }

            // ThenBy - secondary ascending sort
            Console.WriteLine("\n=== ThenBy - Secondary Sort ===");
            
            var people = new List<Person>
            {
                new Person { Name = "Alice", Age = 30, City = "New York" },
                new Person { Name = "Bob", Age = 25, City = "Los Angeles" },
                new Person { Name = "Charlie", Age = 30, City = "Chicago" },
                new Person { Name = "Diana", Age = 25, City = "New York" }
            };
            
            // Sort by Age, then by Name
            var sortedByAgeThenName = people
                .OrderBy(p => p.Age)
                .ThenBy(p => p.Name);
            
            Console.WriteLine("Sorted by Age then Name:");
            foreach (var p in sortedByAgeThenName)
            {
                Console.WriteLine($"  {p.Name}, Age: {p.Age}, City: {p.City}");
                // Output: Bob (25), Diana (25), Alice (30), Charlie (30)
            }

            // ThenByDescending - secondary descending sort
            Console.WriteLine("\n=== ThenByDescending ===");
            
            var sortedDesc = people
                .OrderBy(p => p.Age)
                .ThenByDescending(p => p.Name);
            
            Console.WriteLine("Sorted by Age asc, Name desc:");
            foreach (var p in sortedDesc)
            {
                Console.WriteLine($"  {p.Name}, Age: {p.Age}");
                // Output: Diana (25), Bob (25), Charlie (30), Alice (30)
            }

            // Multiple ThenBy - multiple sort levels
            Console.WriteLine("\n=== Multiple ThenBy ===");
            
            var products = new List<Product>
            {
                new Product { Name = "Laptop", Category = "Electronics", Price = 999.99m },
                new Product { Name = "Mouse", Category = "Electronics", Price = 29.99m },
                new Product { Name = "Chair", Category = "Furniture", Price = 149.99m },
                new Product { Name = "Keyboard", Category = "Electronics", Price = 79.99m },
                new Product { Name = "Desk", Category = "Furniture", Price = 299.99m }
            };
            
            // Sort by Category, then by Price descending, then by Name
            var multiSorted = products
                .OrderBy(p => p.Category)
                .ThenByDescending(p => p.Price)
                .ThenBy(p => p.Name);
            
            Console.WriteLine("Sorted by Category, Price (desc), Name:");
            foreach (var p in multiSorted)
            {
                Console.WriteLine($"  {p.Category} - {p.Name}: ${p.Price}");
                // Output: Electronics - Laptop $999.99, Keyboard $79.99, Mouse $29.99
                // Output: Furniture - Desk $299.99, Chair $149.99
            }

            // Reverse - reverse the collection order
            Console.WriteLine("\n=== Reverse ===");
            
            var reversed = numbers.Reverse();
            
            Console.WriteLine("Reversed:");
            foreach (var n in reversed)
            {
                Console.WriteLine(n); // Output: 6, 4, 7, 3, 9, 1, 8, 2, 5
            }

            // REAL WORLD EXAMPLE: Employee performance ranking
            Console.WriteLine("\n=== Real World: Employee Ranking ===");
            
            var employees = new List<Employee>
            {
                new Employee { Name = "John", Department = "Sales", Sales = 50000 },
                new Employee { Name = "Jane", Department = "IT", Sales = 45000 },
                new Employee { Name = "Bob", Department = "Sales", Sales = 60000 },
                new Employee { Name = "Alice", Department = "HR", Sales = 35000 },
                new Employee { Name = "Charlie", Department = "IT", Sales = 55000 }
            };
            
            // Rank by sales within department
            var ranked = employees
                .OrderBy(e => e.Department)
                .ThenByDescending(e => e.Sales)
                .Select((e, index) => new
                {
                    Rank = index + 1,
                    Department = e.Department,
                    Name = e.Name,
                    Sales = e.Sales
                });
            
            Console.WriteLine("Employee Rankings:");
            foreach (var emp in ranked)
            {
                Console.WriteLine($"  #{emp.Rank} - {emp.Name} ({emp.Department}): ${emp.Sales:N0}");
                // Output: #1 - Bob (Sales): $60000, #2 - John (Sales): $50000, etc.
            }

            // REAL WORLD EXAMPLE: Customer orders by value
            Console.WriteLine("\n=== Real World: Customer Orders ===");
            
            var orders = new List<Order>
            {
                new Order { Customer = "Alice", Date = new DateTime(2024, 1, 15), Total = 150.00m },
                new Order { Customer = "Bob", Date = new DateTime(2024, 2, 10), Total = 200.00m },
                new Order { Customer = "Alice", Date = new DateTime(2024, 3, 5), Total = 75.00m },
                new Order { Customer = "Charlie", Date = new DateTime(2024, 1, 20), Total = 300.00m },
                new Order { Customer = "Bob", Date = new DateTime(2024, 4, 1), Total = 50.00m }
            };
            
            // Sort by Customer, then by Date
            var sortedOrders = orders
                .OrderBy(o => o.Customer)
                .ThenByDescending(o => o.Date);
            
            Console.WriteLine("Orders sorted by Customer, Date (desc):");
            foreach (var o in sortedOrders)
            {
                Console.WriteLine($"  {o.Customer}: ${o.Total} on {o.Date:MMM dd}");
                // Output: Alice: $150 on Jan 15, $75 on Mar 05, etc.
            }
        }
    }

    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string City { get; set; }
    }

    public class Product
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
    }

    public class Employee
    {
        public string Name { get; set; }
        public string Department { get; set; }
        public decimal Sales { get; set; }
    }

    public class Order
    {
        public string Customer { get; set; }
        public DateTime Date { get; set; }
        public decimal Total { get; set; }
    }
}