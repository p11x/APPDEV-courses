/*
 * ============================================================
 * TOPIC     : C# Fundamentals
 * SUBTOPIC  : Variables and Constants - Var Keyword
 * FILE      : VarKeyword.cs
 * PURPOSE   : This file explains the var keyword in C#, when to use it, and best practices.
 *             Var provides implicit type inference at compile time.
 * ============================================================
 */

// --- SECTION: Var Keyword ---
// 'var' tells the compiler to infer the type from the right-hand side expression
// It's not dynamic typing - types are resolved at compile time
// The variable still has a strong, static type

using System;
using System.Collections.Generic;

namespace CSharp_MasterGuide._01_Fundamentals._03_Variables_Constants
{
    class VarKeyword
    {
        static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════════
            // SECTION: Basic var Usage
            // ═══════════════════════════════════════════════════════════════
            
            // Compiler infers type from right side
            var name = "Alice"; // Compiler sees string literal → type is string
            var age = 30; // Compiler sees int literal → type is int
            var price = 19.99; // Compiler sees double → type is double
            var isActive = true; // Compiler sees bool → type is bool
            
            Console.WriteLine($"name: {name} ({name.GetType().Name})"); // Output: name: Alice (String)
            Console.WriteLine($"age: {age} ({age.GetType().Name})"); // Output: age: 30 (Int32)
            Console.WriteLine($"price: {price} ({price.GetType().Name})"); // Output: price: 19.99 (Double)
            Console.WriteLine($"isActive: {isActive} ({isActive.GetType().Name})"); // Output: isActive: True (Boolean)
            
            // ── All var statements are equivalent to explicit types ─────────
            // var x = "hello"; is exactly the same as:
            string x = "hello"; // Explicit equivalent
            
            // var is NOT dynamic - type is fixed at compile time
            // name = 42; // ERROR: Cannot convert int to string!
            // age = "thirty"; // ERROR: Cannot convert string to int!

            // ═══════════════════════════════════════════════════════════════
            // SECTION: When to Use var
            // ═══════════════════════════════════════════════════════════════
            
            // ── Long/complex type names ─────────────────────────────────────
            // Without var (verbose)
            Dictionary<string, List<int>> dict1 = new Dictionary<string, List<int>>();
            
            // With var (cleaner)
            var dict2 = new Dictionary<string, List<int>>();
            
            // ── LINQ queries ───────────────────────────────────────────────
            var numbers = new List<int> { 1, 2, 3, 4, 5 };
            var evenNumbers = numbers.Where(n => n % 2 == 0); // Type: IEnumerable<int>
            // Without var: IEnumerable<int> evenNumbers = numbers.Where(n => n % 2 == 0);
            
            // ── Anonymous types ─────────────────────────────────────────────
            // MUST use var - no explicit type name available
            var person = new { Name = "John", Age = 30 };
            Console.WriteLine($"Anonymous: {person.Name}"); // Output: Anonymous: John
            
            // ── Object/collection initializers ───────────────────────────
            var customer = new Customer { Name = "Bob", Email = "bob@email.com" };
            var orders = new List<Order>
            {
                new Order { Id = 1, Amount = 100 },
                new Order { Id = 2, Amount = 200 }
            };

            // ═══════════════════════════════════════════════════════════════
            // SECTION: When NOT to Use var
            // ═══════════════════════════════════════════════════════════════
            
            // ── Type not obvious from right side ───────────────────────────
            // var x = GetValue(); // What type is returned?
            // Better: int x = GetValue(); // Clear from method signature
            
            // ── Numeric literals with decimals ─────────────────────────────
            // var pi = 3.14; // infers double - may want decimal!
            decimal pi = 3.14m; // Explicit: we want decimal
            
            // ── For code clarity/documentation ───────────────────────────
            // Public API surfaces - explicit types help documentation
            // int CalculateTotal(List<Product> products) { } // Clear
            // CalculateTotal(var products) { } // Confusing

            // ═══════════════════════════════════════════════════════════════
            // SECTION: var with Different Types
            // ═══════════════════════════════════════════════════════════════
            
            // ── Arrays ─────────────────────────────────────────────────────
            var array = new int[] { 1, 2, 3 }; // Type: int[]
            Console.WriteLine($"Array type: {array.GetType().Name}"); // Output: Array type: Int32[]
            
            // ── Null ───────────────────────────────────────────────────────
            // var nullVal = null; // ERROR: Cannot infer type from null
            string? nullVal = null; // Must specify type
            
            // ── Lambda/Anonymous Methods ───────────────────────────────────
            // For lambdas, you typically need Func/Action or explicit type
            Func<int, int> square = x => x * x;
            var squareResult = square(5);
            Console.WriteLine($"Lambda result: {squareResult}"); // Output: Lambda result: 25
            
            // ── String with var ─────────────────────────────────────────────
            var greeting = "Hello"; // string
            var emptyString = string.Empty; // string (static property)
            var interpolated = $"{greeting}, World!"; // string

            // ═══════════════════════════════════════════════════════════════
            // SECTION: var in Different Contexts
            // ═══════════════════════════════════════════════════════════════
            
            // ── For loop ───────────────────────────────────────────────────
            var list = new List<string> { "a", "b", "c" };
            for (var i = 0; i < list.Count; i++) // var as loop counter
            {
                Console.WriteLine($"Index {i}: {list[i]}");
            }
            
            // ── Using statement ────────────────────────────────────────────
            // using var file = new StreamReader("file.txt"); // C# 8.0+
            // var is useful with IDisposable
            
            // ── Foreach ───────────────────────────────────────────────────
            foreach (var item in list)
            {
                Console.WriteLine($"Item: {item}");
            }
            
            // ── Linq Select ────────────────────────────────────────────────
            var names = new List<string> { "Alice", "Bob", "Charlie" };
            var upperNames = names.Select(n => n.ToUpper()); // IEnumerable<string>
            Console.WriteLine($"Upper names: {string.Join(", ", upperNames)}");
            // Output: Upper names: ALICE, BOB, CHARLIE

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Real-World var Examples
            // ═══════════════════════════════════════════════════════════════
            
            // ── API response parsing ───────────────────────────────────────
            // var jsonResponse = DeserializeObject(jsonString); // Clear from method
            // var user = jsonResponse["user"]; // May need explicit type
            
            // ── Database queries ────────────────────────────────────────────
            // var users = context.Users.Where(u => u.IsActive); // IEnumerable<User>
            // Clear what we're getting from LINQ context
            
            // ── File operations ─────────────────────────────────────────────
            // var lines = File.ReadAllLines("file.txt"); // string[]
            // var directory = new DirectoryInfo("path"); // DirectoryInfo
            
            // ── Complex initialization ─────────────────────────────────────
            var settings = new Dictionary<string, object>
            {
                ["Timeout"] = 30,
                ["RetryCount"] = 3,
                ["Endpoint"] = "https://api.example.com"
            };
            Console.WriteLine($"Settings count: {settings.Count}"); // Output: Settings count: 3

            // ═══════════════════════════════════════════════════════════════
            // SECTION: var vs explicit type performance
            // ═══════════════════════════════════════════════════════════════
            
            // There is NO performance difference - both compile to identical IL
            // var is syntactic sugar - compiler generates the same bytecode
            
            // This:
            var s1 = "hello";
            
            // Compiles to exactly the same as:
            string s2 = "hello";
            
            // JIT sees identical types, identical code execution
            // Choice is purely about readability and maintainability
        }
    }
    
    // Supporting classes
    class Customer
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
    
    class Order
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
    }
    
    // Method demonstrating when explicit type might be better
    class SomeClass
    {
        // Clear return type - explicit is better
        public List<Customer> GetCustomers() => new List<Customer>();
        
        // Ambiguous return - explicit might be better
        // public object GetValue() => "something"; // var would be object
        
        // Generic return - explicit is clearer
        // public T Get<T>() => default(T);
    }
}
