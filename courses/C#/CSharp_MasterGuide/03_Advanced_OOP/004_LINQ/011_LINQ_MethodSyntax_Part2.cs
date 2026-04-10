/*
 * TOPIC: Language Integrated Query (LINQ)
 * SUBTOPIC: LINQ Method Syntax - Part 2
 * FILE: LINQ_MethodSyntax_Part2.cs
 * PURPOSE: More methods, chaining operations
 */
using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharp_MasterGuide._03_Advanced_OOP._04_LINQ
{
    public class LINQ_MethodSyntax_Part2
    {
        public static void Main()
        {
            // More Method Syntax Operations
            
            // SelectMany - flatten nested collections
            Console.WriteLine("=== SelectMany ===");
            
            var departments = new List<Department>
            {
                new Department { 
                    Name = "IT", 
                    Employees = new List<string> { "Alice", "Bob", "Charlie" } 
                },
                new Department { 
                    Name = "HR", 
                    Employees = new List<string> { "Diana", "Eve" } 
                }
            };
            
            // Flatten all employees
            var allEmployees = departments.SelectMany(d => d.Employees);
            
            Console.WriteLine("All Employees:");
            foreach (var emp in allEmployees)
            {
                Console.WriteLine($"  {emp}"); 
                // Output: Alice, Bob, Charlie, Diana, Eve
            }
            
            // SelectMany with index
            Console.WriteLine("\nSelectMany with index:");
            var indexed = departments.SelectMany((d, index) => 
                d.Employees.Select(e => $"{index}: {e}"));
            
            foreach (var item in indexed)
            {
                Console.WriteLine($"  {item}"); 
                // Output: 0: Alice, 0: Bob, 0: Charlie, 1: Diana, 1: Eve
            }

            // Distinct - remove duplicates
            Console.WriteLine("\n=== Distinct ===");
            
            var duplicates = new List<int> { 1, 2, 2, 3, 3, 3, 4, 4, 4, 4 };
            var unique = duplicates.Distinct();
            
            Console.WriteLine("Unique values:");
            foreach (var n in unique)
            {
                Console.WriteLine(n); // Output: 1, 2, 3, 4
            }
            
            // Distinct with custom comparer
            var words = new List<string> { "hello", "HELLO", "World", "WORLD" };
            var uniqueWords = words.Distinct(StringComparer.OrdinalIgnoreCase);
            
            Console.WriteLine("\nUnique (case-insensitive):");
            foreach (var w in uniqueWords)
            {
                Console.WriteLine(w); // Output: hello, World
            }

            // OfType - filter by type
            Console.WriteLine("\n=== OfType ===");
            
            var mixed = new List<object> { 1, "hello", 2.5, 3, "world", 4.5m };
            
            var strings = mixed.OfType<string>();
            Console.WriteLine("Strings:");
            foreach (var s in strings)
            {
                Console.WriteLine(s); // Output: hello, world
            }
            
            var numbers = mixed.OfType<int>();
            Console.WriteLine("\nIntegers:");
            foreach (var n in numbers)
            {
                Console.WriteLine(n); // Output: 1, 3
            }

            // Cast - convert elements (throws if conversion fails)
            Console.WriteLine("\n=== Cast ===");
            
            var boxed = new List<object> { 1, 2, 3, 4, 5 };
            var ints = boxed.Cast<int>().ToList();
            
            Console.WriteLine("Casted to int:");
            foreach (var n in ints)
            {
                Console.WriteLine(n); // Output: 1, 2, 3, 4, 5
            }

            // Chaining multiple operations
            Console.WriteLine("\n=== Chaining Operations ===");
            
            var data = new List<int> { 5, 2, 8, 1, 9, 3, 7, 4, 6 };
            
            var result = data
                .Where(n => n > 2)           // Filter: 5,8,1,9,3,7,4,6
                .OrderBy(n => n)             // Sort: 1,3,4,5,6,7,8,9
                .Distinct()                 // Dedupe: 1,3,4,5,6,7,8,9
                .Take(5)                    // Take first 5: 1,3,4,5,6
                .Select(n => n * 10);       // Transform: 10,30,40,50,60
            
            Console.WriteLine("Chained result:");
            foreach (var n in result)
            {
                Console.WriteLine(n); // Output: 10, 30, 40, 50, 60
            }

            // REAL WORLD EXAMPLE: Order processing
            Console.WriteLine("\n=== Real World: Order Processing ===");
            
            var orders = new List<Order>
            {
                new Order { Id = 1, Items = new List<string> { "Laptop", "Mouse" }, Total = 1029.99m },
                new Order { Id = 2, Items = new List<string> { "Keyboard" }, Total = 79.99m },
                new Order { Id = 3, Items = new List<string> { "Monitor", "Webcam" }, Total = 499.99m },
                new Order { Id = 4, Items = new List<string> { "Laptop" }, Total = 999.99m }
            };
            
            // Get all items from orders over $500
            var expensiveItems = orders
                .Where(o => o.Total > 500)
                .SelectMany(o => o.Items)
                .Distinct();
            
            Console.WriteLine("Items in orders over $500:");
            foreach (var item in expensiveItems)
            {
                Console.WriteLine($"  {item}"); 
                // Output: Laptop, Mouse, Monitor, Webcam
            }

            // REAL WORLD EXAMPLE: Student grade analysis
            Console.WriteLine("\n=== Real World: Grade Analysis ===");
            
            var students = new List<Student>
            {
                new Student { Name = "Alice", Grades = new List<int> { 85, 90, 88 } },
                new Student { Name = "Bob", Grades = new List<int> { 92, 95, 98 } },
                new Student { Name = "Charlie", Grades = new List<int> { 70, 75, 72 } }
            };
            
            // Find students with average > 80
            var topStudents = students
                .Select(s => new { 
                    Name = s.Name, 
                    AvgGrade = s.Grades.Average() 
                })
                .Where(s => s.AvgGrade > 80)
                .OrderByDescending(s => s.AvgGrade);
            
            Console.WriteLine("Students with avg > 80:");
            foreach (var s in topStudents)
            {
                Console.WriteLine($"  {s.Name}: {s.AvgGrade:F1}"); 
                // Output: Bob: 95.0, Alice: 87.7
            }
        }
    }

    public class Department
    {
        public string Name { get; set; }
        public List<string> Employees { get; set; }
    }

    public class Order
    {
        public int Id { get; set; }
        public List<string> Items { get; set; }
        public decimal Total { get; set; }
    }

    public class Student
    {
        public string Name { get; set; }
        public List<int> Grades { get; set; }
    }
}