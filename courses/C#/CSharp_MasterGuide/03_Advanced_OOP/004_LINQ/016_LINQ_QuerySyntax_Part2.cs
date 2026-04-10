/*
 * TOPIC: Language Integrated Query (LINQ)
 * SUBTOPIC: LINQ Query Syntax - Part 2
 * FILE: LINQ_QuerySyntax_Part2.cs
 * PURPOSE: More query syntax - grouped queries, let clauses, into
 */
using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharp_MasterGuide._03_Advanced_OOP._04_LINQ
{
    public class LINQ_QuerySyntax_Part2
    {
        public static void Main()
        {
            // Group By in Query Syntax
            Console.WriteLine("=== Group By ===");
            
            var products = new List<Product>
            {
                new Product { Name = "Laptop", Category = "Electronics", Price = 999.99m },
                new Product { Name = "Mouse", Category = "Electronics", Price = 29.99m },
                new Product { Name = "Chair", Category = "Furniture", Price = 149.99m },
                new Product { Name = "Desk", Category = "Furniture", Price = 299.99m },
                new Product { Name = "Keyboard", Category = "Electronics", Price = 79.99m }
            };
            
            // Group by category
            var groupedProducts = from p in products
                                  group p by p.Category;
            
            foreach (var group in groupedProducts)
            {
                Console.WriteLine($"\nCategory: {group.Key}");
                foreach (var product in group)
                {
                    Console.WriteLine($"  - {product.Name}: ${product.Price}");
                    // Output: Electronics: Laptop $999.99, Mouse $29.99, Keyboard $79.99
                    // Output: Furniture: Chair $149.99, Desk $299.99
                }
            }

            // Let clause - intermediate variables
            Console.WriteLine("\n=== Let Clause ===");
            
            var numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            
            // Using let to store intermediate results
            var query = from n in numbers
                        let square = n * n
                        let isEven = n % 2 == 0
                        where isEven
                        select new { Number = n, Square = square };
            
            Console.WriteLine("Even numbers and their squares:");
            foreach (var item in query)
            {
                Console.WriteLine($"  {item.Number}^2 = {item.Square}"); 
                // Output: 2^2 = 4, 4^2 = 16, 6^2 = 36, 8^2 = 64, 10^2 = 100
            }

            // Into clause - continue querying after group or select
            Console.WriteLine("\n=== Into Clause ===");
            
            var data = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            
            // Use into to continue after group
            var result = from n in data
                         where n > 3
                         select n into filtered
                         where filtered < 8
                         select filtered;
            
            Console.WriteLine("Filtered (3 < n < 8):");
            foreach (var n in result)
            {
                Console.WriteLine(n); // Output: 4, 5, 6, 7
            }

            // Multiple from clauses (cross join)
            Console.WriteLine("\n=== Multiple From - Cross Join ===");
            
            var colors = new List<string> { "Red", "Blue" };
            var sizes = new List<string> { "S", "M", "L" };
            
            var combinations = from c in colors
                               from s in sizes
                               select $"{c} - {s}";
            
            Console.WriteLine("All combinations:");
            foreach (var combo in combinations)
            {
                Console.WriteLine($"  {combo}"); 
                // Output: Red-S, Red-M, Red-L, Blue-S, Blue-M, Blue-L
            }

            // REAL WORLD EXAMPLE: Sales by region
            Console.WriteLine("\n=== Real World: Sales by Region ===");
            
            var sales = new List<Sale>
            {
                new Sale { Product = "Laptop", Region = "North", Amount = 5000 },
                new Sale { Product = "Mouse", Region = "South", Amount = 1500 },
                new Sale { Product = "Laptop", Region = "South", Amount = 3000 },
                new Sale { Product = "Keyboard", Region = "North", Amount = 2000 },
                new Sale { Product = "Mouse", Region = "North", Amount = 1000 },
                new Sale { Product = "Keyboard", Region = "South", Amount = 2500 }
            };
            
            // Group by region
            var salesByRegion = from s in sales
                                group s by s.Region;
            
            foreach (var region in salesByRegion)
            {
                decimal total = region.Sum(s => s.Amount);
                Console.WriteLine($"{region.Key}: ${total}");
                // Output: North: $8000, South: $9000
            }

            // REAL WORLD EXAMPLE: Department salary analysis
            Console.WriteLine("\n=== Real World: Department Analysis ===");
            
            var employees = new List<Employee>
            {
                new Employee { Name = "John", Dept = "IT", Salary = 75000 },
                new Employee { Name = "Jane", Dept = "HR", Salary = 65000 },
                new Employee { Name = "Bob", Dept = "IT", Salary = 80000 },
                new Employee { Name = "Alice", Dept = "HR", Salary = 70000 },
                new Employee { Name = "Charlie", Dept = "Finance", Salary = 85000 }
            };
            
            // Group by department with statistics
            var deptStats = from e in employees
                            group e by e.Dept into deptGroup
                            select new
                            {
                                Department = deptGroup.Key,
                                Count = deptGroup.Count(),
                                TotalSalary = deptGroup.Sum(e => e.Salary),
                                AverageSalary = deptGroup.Average(e => e.Salary)
                            };
            
            Console.WriteLine("Department Statistics:");
            foreach (var dept in deptStats)
            {
                Console.WriteLine($"  {dept.Department}: {dept.Count} employees, " +
                    $"Avg: ${dept.AverageSalary:F0}");
                // Output: IT: 2, Avg: $77500, HR: 2, Avg: $67500, Finance: 1, Avg: $85000
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
        public string Dept { get; set; }
        public decimal Salary { get; set; }
    }
}