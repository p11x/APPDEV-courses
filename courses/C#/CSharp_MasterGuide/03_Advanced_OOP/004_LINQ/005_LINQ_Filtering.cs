/*
 * TOPIC: Language Integrated Query (LINQ)
 * SUBTOPIC: LINQ Filtering Operations
 * FILE: LINQ_Filtering.cs
 * PURPOSE: Where, OfType, Distinct filtering operations
 */
using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharp_MasterGuide._03_Advanced_OOP._04_LINQ
{
    public class LINQ_Filtering
    {
        public static void Main()
        {
            // Where - filter elements based on condition
            Console.WriteLine("=== Where - Basic Filtering ===");
            
            var numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            
            // Filter even numbers
            var evens = numbers.Where(n => n % 2 == 0);
            
            Console.WriteLine("Even numbers:");
            foreach (var n in evens)
            {
                Console.WriteLine(n); // Output: 2, 4, 6, 8, 10
            }
            
            // Filter with multiple conditions
            Console.WriteLine("\nMultiple conditions:");
            var result = numbers.Where(n => n > 3 && n < 8);
            
            foreach (var n in result)
            {
                Console.WriteLine(n); // Output: 4, 5, 6, 7
            }
            
            // Where with index (predicate receives index as second parameter)
            Console.WriteLine("\nWhere with index (even index positions):");
            var indexed = numbers.Where((n, i) => i % 2 == 0);
            
            foreach (var n in indexed)
            {
                Console.WriteLine(n); // Output: 1, 3, 5, 7, 9
            }

            // OfType - filter by type
            Console.WriteLine("\n=== OfType - Type Filtering ===");
            
            var mixedTypes = new List<object> 
            { 
                1, "hello", 2.5, 3, "world", 4.5m, 'c', 5.0f 
            };
            
            // Filter only strings
            var strings = mixedTypes.OfType<string>();
            Console.WriteLine("Strings only:");
            foreach (var s in strings)
            {
                Console.WriteLine(s); // Output: hello, world
            }
            
            // Filter only integers
            var ints = mixedTypes.OfType<int>();
            Console.WriteLine("\nIntegers only:");
            foreach (var n in ints)
            {
                Console.WriteLine(n); // Output: 1, 3
            }
            
            // Filter only doubles
            var doubles = mixedTypes.OfType<double>();
            Console.WriteLine("\nDoubles only:");
            foreach (var d in doubles)
            {
                Console.WriteLine(d); // Output: 2.5
            }
            
            // Filter decimals (both decimal and money)
            var decimals = mixedTypes.OfType<decimal>();
            Console.WriteLine("\nDecimals only:");
            foreach (var d in decimals)
            {
                Console.WriteLine(d); // Output: 4.5
            }

            // Distinct - remove duplicates
            Console.WriteLine("\n=== Distinct - Remove Duplicates ===");
            
            var withDuplicates = new List<int> { 1, 2, 2, 3, 3, 3, 4, 4, 5 };
            var unique = withDuplicates.Distinct();
            
            Console.WriteLine("Distinct values:");
            foreach (var n in unique)
            {
                Console.WriteLine(n); // Output: 1, 2, 3, 4, 5
            }
            
            // Distinct with strings (case-sensitive by default)
            var words = new List<string> { "Apple", "apple", "APPLE", "Banana", "banana" };
            var uniqueWords = words.Distinct();
            
            Console.WriteLine("\nDistinct strings (case-sensitive):");
            foreach (var w in uniqueWords)
            {
                Console.WriteLine(w); // Output: Apple, apple, APPLE, Banana, banana
            }
            
            // Distinct with case-insensitive
            var uniqueWordsIgnoreCase = words.Distinct(StringComparer.OrdinalIgnoreCase);
            Console.WriteLine("\nDistinct (case-insensitive):");
            foreach (var w in uniqueWordsIgnoreCase)
            {
                Console.WriteLine(w); // Output: Apple, Banana
            }

            // DistinctBy - .NET 6+ (distinct by specific key)
            Console.WriteLine("\n=== DistinctBy ===");
            
            var people = new List<Person>
            {
                new Person { Name = "John", Age = 30 },
                new Person { Name = "Jane", Age = 25 },
                new Person { Name = "John", Age = 35 }, // Duplicate name
                new Person { Name = "Alice", Age = 28 }
            };
            
            // Keep first occurrence by Name
            var uniqueByName = people.DistinctBy(p => p.Name);
            
            Console.WriteLine("Distinct by Name:");
            foreach (var p in uniqueByName)
            {
                Console.WriteLine($"  {p.Name}, Age: {p.Age}"); 
                // Output: John (30), Jane (25), Alice (28)
            }

            // REAL WORLD EXAMPLE: Filter products by criteria
            Console.WriteLine("\n=== Real World: Product Filter ===");
            
            var products = new List<Product>
            {
                new Product { Name = "Laptop", Price = 999.99m, Category = "Electronics", InStock = true },
                new Product { Name = "Mouse", Price = 29.99m, Category = "Electronics", InStock = true },
                new Product { Name = "Chair", Price = 149.99m, Category = "Furniture", InStock = false },
                new Product { Name = "Keyboard", Price = 79.99m, Category = "Electronics", InStock = true },
                new Product { Name = "Desk", Price = 299.99m, Category = "Furniture", InStock = true },
                new Product { Name = "Monitor", Price = 399.99m, Category = "Electronics", InStock = true }
            };
            
            // Filter: Electronics, in stock, price under $500
            var availableElectronics = products
                .Where(p => p.Category == "Electronics" && p.InStock && p.Price < 500)
                .OrderBy(p => p.Price);
            
            Console.WriteLine("Available Electronics under $500:");
            foreach (var p in availableElectronics)
            {
                Console.WriteLine($"  {p.Name}: ${p.Price}"); 
                // Output: Keyboard $79.99, Mouse $29.99, Monitor $399.99
            }

            // REAL WORLD EXAMPLE: Filter logs by severity
            Console.WriteLine("\n=== Real World: Log Filtering ===");
            
            var logs = new List<LogEntry>
            {
                new LogEntry { Timestamp = DateTime.Now, Level = "INFO", Message = "App started" },
                new LogEntry { Timestamp = DateTime.Now, Level = "ERROR", Message = "Connection failed" },
                new LogEntry { Timestamp = DateTime.Now, Level = "DEBUG", Message = "Debug info" },
                new LogEntry { Timestamp = DateTime.Now, Level = "ERROR", Message = "Null reference" },
                new LogEntry { Timestamp = DateTime.Now, Level = "WARN", Message = "Low memory" },
                new LogEntry { Timestamp = DateTime.Now, Level = "INFO", Message = "User logged in" }
            };
            
            // Get only errors
            var errors = logs.OfType<LogEntry>().Where(l => l.Level == "ERROR");
            
            Console.WriteLine("Error logs:");
            foreach (var log in errors)
            {
                Console.WriteLine($"  [{log.Level}] {log.Message}"); 
                // Output: Connection failed, Null reference
            }
            
            // Get distinct log levels
            var levels = logs.Select(l => l.Level).Distinct();
            Console.WriteLine("\nDistinct log levels:");
            foreach (var level in levels)
            {
                Console.WriteLine($"  {level}"); // Output: INFO, ERROR, DEBUG, WARN
            }
        }
    }

    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    public class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public bool InStock { get; set; }
    }

    public class LogEntry
    {
        public DateTime Timestamp { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }
    }
}