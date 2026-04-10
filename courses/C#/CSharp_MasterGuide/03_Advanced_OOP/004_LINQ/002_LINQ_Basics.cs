/*
 * TOPIC: Language Integrated Query (LINQ)
 * SUBTOPIC: LINQ Basics - Introduction
 * FILE: LINQ_Basics.cs
 * PURPOSE: Introduction to LINQ, what it is, and why to use it
 */
using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharp_MasterGuide._03_Advanced_OOP._04_LINQ
{
    public class LINQ_Basics
    {
        // What is LINQ?
        // LINQ is a set of C# language extensions that allows writing queries
        // against various data sources (collections, XML, databases, etc.)
        // using a unified query syntax integrated into C#.

        public static void Main()
        {
            // Basic LINQ query on a collection
            // Traditional approach without LINQ
            Console.WriteLine("=== Without LINQ ===");
            var numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            
            var evensWithoutLinq = new List<int>();
            foreach (var num in numbers)
            {
                if (num % 2 == 0)
                {
                    evensWithoutLinq.Add(num);
                }
            }
            
            foreach (var n in evensWithoutLinq)
            {
                Console.WriteLine(n); // Output: 2, 4, 6, 8, 10
            }

            // Same result using LINQ - more concise and readable
            Console.WriteLine("\n=== With LINQ ===");
            var evensWithLinq = numbers.Where(n => n % 2 == 0).ToList();
            
            foreach (var n in evensWithLinq)
            {
                Console.WriteLine(n); // Output: 2, 4, 6, 8, 10
            }

            // Why Use LINQ?
            // 1. Readable and expressive
            // 2. Type-safe (compile-time checking)
            // 3. Works with any IEnumerable<T>
            // 4. Supports both Query Syntax and Method Syntax
            // 5. Can be extended to work with custom data sources

            // REAL WORLD EXAMPLE: Filter products by category
            Console.WriteLine("\n=== Real World Example ===");
            
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Laptop", Category = "Electronics", Price = 999.99m },
                new Product { Id = 2, Name = "Keyboard", Category = "Electronics", Price = 79.99m },
                new Product { Id = 3, Name = "Chair", Category = "Furniture", Price = 249.99m },
                new Product { Id = 4, Name = "Monitor", Category = "Electronics", Price = 399.99m },
                new Product { Id = 5, Name = "Desk", Category = "Furniture", Price = 199.99m }
            };

            // Filter electronics under $500
            var affordableElectronics = products
                .Where(p => p.Category == "Electronics" && p.Price < 500)
                .Select(p => p.Name);

            Console.WriteLine("Affordable Electronics:");
            foreach (var name in affordableElectronics)
            {
                Console.WriteLine($"  - {name}"); 
                // Output: Keyboard, Monitor
            }

            // REAL WORLD EXAMPLE: Search students by name
            Console.WriteLine("\n=== Student Search Example ===");
            
            var students = new List<Student>
            {
                new Student { Id = 1, Name = "Alice Johnson", Grade = "A" },
                new Student { Id = 2, Name = "Bob Smith", Grade = "B" },
                new Student { Id = 3, Name = "Charlie Brown", Grade = "A" },
                new Student { Id = 4, Name = "Diana Prince", Grade = "C" }
            };

            // Find students with grade A
            var topStudents = students
                .Where(s => s.Grade == "A")
                .Select(s => s.Name);

            Console.WriteLine("Top Students (Grade A):");
            foreach (var name in topStudents)
            {
                Console.WriteLine($"  - {name}"); 
                // Output: Alice Johnson, Charlie Brown
            }
        }
    }

    // Helper classes for examples
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
    }

    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Grade { get; set; }
    }
}