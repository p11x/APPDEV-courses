/*
 * TOPIC: Language Integrated Query (LINQ)
 * SUBTOPIC: LINQ Projection Operations
 * FILE: LINQ_Projection.cs
 * PURPOSE: Select and SelectMany for transforming data
 */
using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharp_MasterGuide._03_Advanced_OOP._04_LINQ
{
    public class LINQ_Projection
    {
        public static void Main()
        {
            // Select - transform each element
            Console.WriteLine("=== Select - Basic Transformation ===");
            
            var numbers = new List<int> { 1, 2, 3, 4, 5 };
            
            // Square each number
            var squares = numbers.Select(n => n * n);
            
            Console.WriteLine("Squares:");
            foreach (var n in squares)
            {
                Console.WriteLine(n); // Output: 1, 4, 9, 16, 25
            }
            
            // Transform to different type
            Console.WriteLine("\nTransform to string:");
            var asStrings = numbers.Select(n => $"Number: {n}");
            
            foreach (var s in asStrings)
            {
                Console.WriteLine(s); // Output: Number: 1, Number: 2, etc.
            }
            
            // Select with index
            Console.WriteLine("\nSelect with index:");
            var withIndex = numbers.Select((n, i) => $"Position {i}: {n}");
            
            foreach (var s in withIndex)
            {
                Console.WriteLine(s); // Output: Position 0: 1, Position 1: 2, etc.
            }

            // Select into anonymous types
            Console.WriteLine("\n=== Select to Anonymous Types ===");
            
            var products = new List<Product>
            {
                new Product { Name = "Laptop", Price = 999.99m },
                new Product { Name = "Mouse", Price = 29.99m },
                new Product { Name = "Keyboard", Price = 79.99m }
            };
            
            var projected = products.Select(p => new
            {
                ProductName = p.Name,
                PriceWithTax = p.Price * 1.1m, // Add 10% tax
                IsExpensive = p.Price > 100
            });
            
            foreach (var item in projected)
            {
                Console.WriteLine($"{item.ProductName}: ${item.PriceWithTax:F2}, Expensive: {item.IsExpensive}");
                // Output: Laptop: $1099.99, Expensive: True
                // Output: Mouse: $32.99, Expensive: False
                // Output: Keyboard: $87.99, Expensive: False
            }

            // SelectMany - flatten nested collections
            Console.WriteLine("\n=== SelectMany - Flatten Collections ===");
            
            var departments = new List<Department>
            {
                new Department 
                { 
                    Name = "Engineering",
                    Employees = new List<string> { "Alice", "Bob", "Charlie" }
                },
                new Department 
                { 
                    Name = "Marketing", 
                    Employees = new List<string> { "Diana", "Eve" }
                }
            };
            
            // Flatten all employees
            var allEmployees = departments.SelectMany(d => d.Employees);
            
            Console.WriteLine("All Employees:");
            foreach (var emp in allEmployees)
            {
                Console.WriteLine($"  {emp}"); // Output: Alice, Bob, Charlie, Diana, Eve
            }
            
            // SelectMany with result selector
            Console.WriteLine("\nSelectMany with result:");
            var withDept = departments.SelectMany(
                d => d.Employees,
                (d, emp) => $"{emp} ({d.Name})"
            );
            
            foreach (var item in withDept)
            {
                Console.WriteLine($"  {item}"); 
                // Output: Alice (Engineering), Bob (Engineering), etc.
            }

            // SelectMany with index
            Console.WriteLine("\nSelectMany with index:");
            var teams = new List<string[]>
            {
                new string[] { "A1", "A2", "A3" },
                new string[] { "B1", "B2" },
                new string[] { "C1", "C2", "C3", "C4" }
            };
            
            var indexed = teams.SelectMany((team, i) => 
                team.Select(member => $"Team {i}: {member}")
            );
            
            foreach (var item in indexed)
            {
                Console.WriteLine($"  {item}"); 
                // Output: Team 0: A1, Team 0: A2, Team 0: A3, Team 1: B1, etc.
            }

            // REAL WORLD EXAMPLE: Project customer order data
            Console.WriteLine("\n=== Real World: Order Projection ===");
            
            var orders = new List<Order>
            {
                new Order 
                { 
                    Id = 1, 
                    Customer = "John", 
                    Items = new List<OrderItem> 
                    { 
                        new OrderItem { Product = "Laptop", Qty = 1, Price = 999.99m },
                        new OrderItem { Product = "Mouse", Qty = 2, Price = 29.99m }
                    }
                },
                new Order 
                { 
                    Id = 2, 
                    Customer = "Jane", 
                    Items = new List<OrderItem> 
                    { 
                        new OrderItem { Product = "Keyboard", Qty = 1, Price = 79.99m }
                    }
                }
            };
            
            // Flatten all order items
            var allItems = orders.SelectMany(
                o => o.Items,
                (o, item) => new
                {
                    OrderId = o.Id,
                    Customer = o.Customer,
                    Product = item.Product,
                    Total = item.Qty * item.Price
                }
            );
            
            Console.WriteLine("All Order Items:");
            foreach (var item in allItems)
            {
                Console.WriteLine($"  Order #{item.OrderId} ({item.Customer}): {item.Product} - ${item.Total:F2}"); 
                // Output: Order #1 (John): Laptop - $999.99
                // Output: Order #1 (John): Mouse - $59.98
                // Output: Order #2 (Jane): Keyboard - $79.99
            }

            // REAL WORLD EXAMPLE: Project student grades
            Console.WriteLine("\n=== Real World: Student Grade Projection ===");
            
            var students = new List<Student>
            {
                new Student 
                { 
                    Name = "Alice", 
                    Subjects = new List<SubjectGrade>
                    {
                        new SubjectGrade { Subject = "Math", Grade = 95 },
                        new SubjectGrade { Subject = "Science", Grade = 88 }
                    }
                },
                new Student 
                { 
                    Name = "Bob", 
                    Subjects = new List<SubjectGrade>
                    {
                        new SubjectGrade { Subject = "Math", Grade = 78 },
                        new SubjectGrade { Subject = "Science", Grade = 82 }
                    }
                }
            };
            
            // Get all subject grades as flat list
            var allGrades = students.SelectMany(
                s => s.Subjects,
                (s, sg) => new
                {
                    Student = s.Name,
                    Subject = sg.Subject,
                    Grade = sg.Grade
                }
            );
            
            Console.WriteLine("All Grades:");
            foreach (var g in allGrades)
            {
                Console.WriteLine($"  {g.Student} - {g.Subject}: {g.Grade}"); 
                // Output: Alice - Math: 95, Alice - Science: 88, Bob - Math: 78, Bob - Science: 82
            }
        }
    }

    public class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
    }

    public class Department
    {
        public string Name { get; set; }
        public List<string> Employees { get; set; }
    }

    public class Order
    {
        public int Id { get; set; }
        public string Customer { get; set; }
        public List<OrderItem> Items { get; set; }
    }

    public class OrderItem
    {
        public string Product { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }
    }

    public class Student
    {
        public string Name { get; set; }
        public List<SubjectGrade> Subjects { get; set; }
    }

    public class SubjectGrade
    {
        public string Subject { get; set; }
        public int Grade { get; set; }
    }
}