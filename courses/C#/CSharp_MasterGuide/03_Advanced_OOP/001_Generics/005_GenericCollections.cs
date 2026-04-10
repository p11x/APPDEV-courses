/*
 * ============================================================
 * TOPIC     : Advanced OOP
 * SUBTOPIC  : Generics - Generic Collections
 * FILE      : GenericCollections.cs
 * PURPOSE   : Teaches List<T>, Dictionary<TKey,TValue> basics
 *            and common collection operations
 * ============================================================
 */

using System;
using System.Collections.Generic;

namespace CSharp_MasterGuide._03_Advanced_OOP._01_Generics
{
    class GenericCollections
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Generic Collections in C# ===\n");

            // ═══════════════════════════════════════════════════════════
            // SECTION 1: List<T> Basics
            // ═══════════════════════════════════════════════════════════

            // Creating a generic list
            List<string> fruits = new List<string>();
            fruits.Add("Apple");
            fruits.Add("Banana");
            fruits.Add("Orange");

            Console.WriteLine($"First fruit: {fruits[0]}");
            // Output: First fruit: Apple

            Console.WriteLine($"Count: {fruits.Count}");
            // Output: Count: 3

            // Using initializer syntax
            List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };
            Console.WriteLine($"Numbers: {string.Join(", ", numbers)}");
            // Output: Numbers: 1, 2, 3, 4, 5

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: List<T> Common Operations
            // ═══════════════════════════════════════════════════════════

            // Add range - add multiple items
            List<string> moreFruits = new List<string> { "Mango", "Grape" };
            fruits.AddRange(moreFruits);
            Console.WriteLine($"After AddRange: {string.Join(", ", fruits)}");
            // Output: After AddRange: Apple, Banana, Orange, Mango, Grape

            // Insert - insert at specific index
            fruits.Insert(1, "Peach");
            Console.WriteLine($"After Insert: {string.Join(", ", fruits)}");
            // Output: After Insert: Apple, Peach, Banana, Orange, Mango, Grape

            // Remove - remove by value
            fruits.Remove("Banana");
            Console.WriteLine($"After Remove: {string.Join(", ", fruits)}");
            // Output: After Remove: Apple, Peach, Orange, Mango, Grape

            // RemoveAt - remove by index
            fruits.RemoveAt(2);
            Console.WriteLine($"After RemoveAt: {string.Join(", ", fruits)}");
            // Output: After RemoveAt: Apple, Peach, Mango, Grape

            // Contains - check if item exists
            bool hasApple = fruits.Contains("Apple");
            Console.WriteLine($"Contains 'Apple': {hasApple}");
            // Output: Contains 'Apple': True

            // IndexOf - find index of item
            int idx = fruits.IndexOf("Mango");
            Console.WriteLine($"Index of 'Mango': {idx}");
            // Output: Index of 'Mango': 2

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: Dictionary<TKey, TValue> Basics
            // ═══════════════════════════════════════════════════════════

            // Creating a dictionary
            Dictionary<string, int> ages = new Dictionary<string, int>();
            ages["Alice"] = 30;
            ages["Bob"] = 25;
            ages["Charlie"] = 35;

            Console.WriteLine($"Alice's age: {ages["Alice"]}");
            // Output: Alice's age: 30

            // Using initializer syntax
            Dictionary<string, string> capitals = new Dictionary<string, string>
            {
                ["USA"] = "Washington D.C.",
                ["UK"] = "London",
                ["France"] = "Paris"
            };

            Console.WriteLine($"France capital: {capitals["France"]}");
            // Output: France capital: Paris

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: Dictionary Operations
            // ═══════════════════════════════════════════════════════════

            // TryGetValue - safely get value
            if (ages.TryGetValue("Bob", out int bobAge))
            {
                Console.WriteLine($"Bob's age via TryGetValue: {bobAge}");
                // Output: Bob's age via TryGetValue: 25
            }

            // ContainsKey - check if key exists
            bool hasDavid = ages.ContainsKey("David");
            Console.WriteLine($"Has 'David': {hasDavid}");
            // Output: Has 'David': False

            // ContainsValue - check if value exists
            bool hasAge30 = ages.ContainsValue(30);
            Console.WriteLine($"Contains value 30: {hasAge30}");
            // Output: Contains value 30: True

            // Remove - remove key-value pair
            ages.Remove("Bob");
            Console.WriteLine($"After remove, count: {ages.Count}");
            // Output: After remove, count: 2

            // Loop through dictionary
            Console.WriteLine("All entries:");
            foreach (var kvp in ages)
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
            }
            // Output:
            //   Alice: 30
            //   Charlie: 35

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: Real-World Example - Product Catalog
            // ═══════════════════════════════════════════════════════════

            // Product catalog using Dictionary
            Dictionary<int, Product5> catalog = new Dictionary<int, Product5>
            {
                [1] = new Product5 { Id = 1, Name = "Laptop", Price = 999.99m },
                [2] = new Product5 { Id = 2, Name = "Phone", Price = 699.99m },
                [3] = new Product5 { Id = 3, Name = "Tablet", Price = 499.99m }
            };

            // Find product by ID
            if (catalog.TryGetValue(2, out Product5 phone))
            {
                Console.WriteLine($"Found: {phone.Name} - ${phone.Price}");
                // Output: Found: Phone - $699.99
            }

            // Update product
            catalog[1] = new Product5 { Id = 1, Name = "Laptop Pro", Price = 1299.99m };
            Console.WriteLine($"Updated: {catalog[1].Name}");
            // Output: Updated: Laptop Pro

            Console.WriteLine("\n=== Generic Collections Complete ===");
        }
    }

    // Product class for demonstration
    class Product5
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}