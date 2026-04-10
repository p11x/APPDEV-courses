/*
 * ============================================================
 * TOPIC     : Database
 * SUBTOPIC  : LINQ to SQL
 * FILE      : 03_LINQDemo.cs
 * PURPOSE   : Demonstrates LINQ for database queries in C#
 * ============================================================
 */
using System; // needed for Console, basic types
using System.Linq; // needed for LINQ
using System.Collections.Generic; // needed for List<T>

namespace CSharp_MasterGuide._16_Database._03_LINQ
{
    /// <summary>
    /// Demonstrates LINQ
    /// </summary>
    public class LINQDemo
    {
        /// <summary>
        /// Entry point for LINQ examples
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs to console
            // Output: === LINQ Demo ===
            Console.WriteLine("=== LINQ Demo ===\n");

            // ── CONCEPT: Query Syntax ─────────────────────────────────────────
            // SQL-like query syntax

            // Example 1: Query Syntax
            // Output: 1. Query Syntax:
            Console.WriteLine("1. Query Syntax:");
            
            var users = new List<User>
            {
                new User { Id = 1, Name = "John", Age = 30 },
                new User { Id = 2, Name = "Jane", Age = 25 },
                new User { Id = 3, Name = "Bob", Age = 35 }
            };
            
            var adults = from u in users where u.Age >= 30 orderby u.Name select u;
            foreach (var user in adults)
            {
                Console.WriteLine($"   {user.Name}, Age: {user.Age}");
            }
            // Output: John, Age: 30
            // Output: Bob, Age: 35

            // Example 2: Method Syntax
            // Output: 2. Method Syntax:
            Console.WriteLine("\n2. Method Syntax:");
            
            var names = users.Select(u => u.Name);
            // Output: John, Jane, Bob
            Console.WriteLine($"   Names: {string.Join(", ", names)}");

            Console.WriteLine("\n=== LINQ Complete ===");
        }
    }

    public class User
    {
        public int Id { get; set; } // property: user ID
        public string Name { get; set; } // property: user name
        public int Age { get; set; } // property: user age
    }
}