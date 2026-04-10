/*
 * TOPIC: Language Integrated Query (LINQ)
 * SUBTOPIC: LINQ Query Syntax
 * FILE: LINQ_QuerySyntax.cs
 * PURPOSE: Query syntax (from, where, orderby, select)
 */
using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharp_MasterGuide._03_Advanced_OOP._04_LINQ
{
    public class LINQ_QuerySyntax
    {
        public static void Main()
        {
            // Basic Query Syntax Structure:
            // var result = from variable in collection
            //              where condition
            //              orderby expression
            //              select expression;

            Console.WriteLine("=== Basic Query Syntax ===");
            
            var numbers = new List<int> { 5, 2, 8, 1, 9, 3, 7, 4, 6 };
            
            // Simple query: from, where, select
            var doubledEvens = from n in numbers
                              where n % 2 == 0
                              select n * 2;
            
            Console.WriteLine("Doubled even numbers:");
            foreach (var n in doubledEvens)
            {
                Console.WriteLine(n); // Output: 16, 4, 12, 8
            }

            // OrderBy in Query Syntax
            Console.WriteLine("\n=== With OrderBy ===");
            
            var names = new List<string> { "Zoe", "Alice", "Bob", "Charlie", "Diana" };
            
            var sortedNames = from name in names
                             orderby name ascending
                             select name;
            
            Console.WriteLine("Sorted ascending:");
            foreach (var name in sortedNames)
            {
                Console.WriteLine(name); // Output: Alice, Bob, Charlie, Diana, Zoe
            }
            
            // OrderByDescending
            var sortedDesc = from name in names
                            orderby name descending
                            select name;
            
            Console.WriteLine("\nSorted descending:");
            foreach (var name in sortedDesc)
            {
                Console.WriteLine(name); // Output: Zoe, Diana, Charlie, Bob, Alice
            }

            // Multiple conditions in where
            Console.WriteLine("\n=== Multiple Where Conditions ===");
            
            var products = new List<Product>
            {
                new Product { Name = "Laptop", Price = 999.99m, Category = "Electronics" },
                new Product { Name = "Chair", Price = 149.99m, Category = "Furniture" },
                new Product { Name = "Mouse", Price = 29.99m, Category = "Electronics" },
                new Product { Name = "Desk", Price = 299.99m, Category = "Furniture" },
                new Product { Name = "Monitor", Price = 399.99m, Category = "Electronics" }
            };
            
            // Multiple conditions
            var affordableElectronics = from p in products
                                       where p.Category == "Electronics"
                                       where p.Price < 500
                                       select p.Name;
            
            Console.WriteLine("Affordable Electronics:");
            foreach (var name in affordableElectronics)
            {
                Console.WriteLine($"  {name}"); // Output: Mouse, Monitor
            }

            // Select with transformation
            Console.WriteLine("\n=== Select with Transformation ===");
            
            var numbers2 = new List<int> { 1, 2, 3, 4, 5 };
            
            var squares = from n in numbers2
                         select n * n;
            
            Console.WriteLine("Squares:");
            foreach (var n in squares)
            {
                Console.WriteLine(n); // Output: 1, 4, 9, 16, 25
            }
            
            // Select into anonymous type
            var anon = from n in numbers2
                       select new { Number = n, Square = n * n, Cube = n * n * n };
            
            Console.WriteLine("\nAnonymous type projection:");
            foreach (var item in anon)
            {
                Console.WriteLine($"  {item.Number}: Square={item.Square}, Cube={item.Cube}"); 
                // Output: 1: Square=1, Cube=1
                // Output: 2: Square=4, Cube=8
            }

            // REAL WORLD EXAMPLE: Employee filtering and sorting
            Console.WriteLine("\n=== Real World: Employee Report ===");
            
            var employees = new List<Employee>
            {
                new Employee { Name = "John Smith", Department = "IT", Salary = 75000 },
                new Employee { Name = "Jane Doe", Department = "HR", Salary = 65000 },
                new Employee { Name = "Bob Wilson", Department = "IT", Salary = 80000 },
                new Employee { Name = "Alice Brown", Department = "Finance", Salary = 70000 },
                new Employee { Name = "Charlie Davis", Department = "HR", Salary = 60000 }
            };
            
            // Query: IT department with salary > 70000, ordered by salary descending
            var topItEmployees = from e in employees
                                 where e.Department == "IT" && e.Salary > 70000
                                 orderby e.Salary descending
                                 select new { e.Name, e.Salary };
            
            Console.WriteLine("Top IT Employees:");
            foreach (var emp in topItEmployees)
            {
                Console.WriteLine($"  {emp.Name}: ${emp.Salary}"); 
                // Output: Bob Wilson: $80000, John Smith: $75000
            }

            // REAL WORLD EXAMPLE: Student grades analysis
            Console.WriteLine("\n=== Real World: Student Analysis ===");
            
            var students = new List<Student>
            {
                new Student { Name = "Alex", MathScore = 85, ScienceScore = 90 },
                new Student { Name = "Beth", MathScore = 92, ScienceScore = 88 },
                new Student { Name = "Carl", MathScore = 78, ScienceScore = 95 },
                new Student { Name = "Diana", MathScore = 95, ScienceScore = 92 }
            };
            
            // Find students with average score > 85
            var topStudents = from s in students
                              let average = (s.MathScore + s.ScienceScore) / 2.0
                              where average > 85
                              orderby average descending
                              select new { s.Name, Average = average };
            
            Console.WriteLine("Top Students (avg > 85):");
            foreach (var s in topStudents)
            {
                Console.WriteLine($"  {s.Name}: {s.Average:F1}"); 
                // Output: Diana: 93.5, Beth: 90.0, Alex: 87.5
            }
        }
    }

    public class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
    }

    public class Employee
    {
        public string Name { get; set; }
        public string Department { get; set; }
        public decimal Salary { get; set; }
    }

    public class Student
    {
        public string Name { get; set; }
        public int MathScore { get; set; }
        public int ScienceScore { get; set; }
    }
}