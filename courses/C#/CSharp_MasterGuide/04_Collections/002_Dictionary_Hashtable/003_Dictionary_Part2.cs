/*
 * ============================================================
 * TOPIC     : Collections
 * SUBTOPIC  : Dictionary<TKey,TValue> - Advanced Operations
 * FILE      : Dictionary_Part2.cs
 * PURPOSE   : Demonstrates more Dictionary operations including
 *            Remove, Clear, Keys/Values collections, and performance
 * ============================================================
 */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CSharp_MasterGuide._04_Collections._02_Dictionary_Hashtable
{
    class DictionaryAdvanced
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Dictionary<TKey,TValue> Advanced Operations ===\n");

            // ═══════════════════════════════════════════════════════════
            // SECTION 1: Removing Elements
            // ═══════════════════════════════════════════════════════════

            var scores = new Dictionary<string, int>
            {
                { "Alice", 95 },
                { "Bob", 87 },
                { "Charlie", 92 },
                { "David", 78 },
                { "Eve", 88 }
            };

            // Remove by key (returns bool indicating success)
            bool removed = scores.Remove("David");
            Console.WriteLine($"Remove 'David': {removed}, Count: {scores.Count}");
            // Output: Remove 'David': True, Count: 4

            // Remove using pattern: check then remove
            if (scores.ContainsKey("Eve"))
            {
                int eveScore = scores["Eve"];
                scores.Remove("Eve");
                Console.WriteLine($"Removed 'Eve', Score: {eveScore}");
            }
            // Output: Removed 'Eve', Score: 88

            Console.WriteLine($"\nRemaining entries: {scores.Count}");
            // Output: Remaining entries: 3

            // Clear all entries
            scores.Clear();
            Console.WriteLine($"After Clear, Count: {scores.Count}");
            // Output: After Clear, Count: 0

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: Keys and Values Collections
            // ═══════════════════════════════════════════════════════════

            var products = new Dictionary<string, decimal>
            {
                { "Laptop", 999.99m },
                { "Mouse", 29.99m },
                { "Keyboard", 79.99m },
                { "Monitor", 299.99m }
            };

            // Get Keys collection (supports LINQ)
            var productNames = products.Keys;
            Console.WriteLine($"\nProduct names: {string.Join(", ", productNames)}");
            // Output: Product names: Laptop, Mouse, Keyboard, Monitor

            // Get Values collection
            var prices = products.Values;
            Console.WriteLine($"Price range: {prices.Min()} - {prices.Max()}");
            // Output: Price range: 29.99 - 999.99

            // Read-only wrapper
            ReadOnlyDictionary<string, decimal> readOnlyProducts = 
                new ReadOnlyDictionary<string, decimal>(products);
            Console.WriteLine($"Is read-only: True (via ReadOnlyDictionary)");
            // Output: Is read-only: True (via ReadOnlyDictionary)

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: Performance Characteristics
            // ═══════════════════════════════════════════════════════════

            // Dictionary provides O(1) average time complexity for:
            // - Add, TryGetValue, ContainsKey, Remove
            
            var performanceTest = new Dictionary<int, int>();
            
            // Initial capacity can reduce rehashing for known sizes
            var largeDict = new Dictionary<int, string>(10000);
            
            // Add 10000 items
            for (int i = 0; i < 10000; i++)
            {
                largeDict[i] = $"Item{i}";
            }
            
            // ContainsKey is O(1)
            bool hasKey = largeDict.ContainsKey(5000);
            Console.WriteLine($"\nContainsKey 5000: {hasKey}");
            // Output: ContainsKey 5000: True

            // TryGetValue is O(1)
            bool found = largeDict.TryGetValue(5000, out string value);
            Console.WriteLine($"TryGetValue 5000: {value}");
            // Output: TryGetValue 5000: Item5000

            // Count is O(1)
            Console.WriteLine($"Count: {largeDict.Count}");
            // Output: Count: 10000

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: Dictionary with Complex Keys
            // ═══════════════════════════════════════════════════════════

            var salesByMonth = new Dictionary<(int Year, int Month), decimal>
            {
                { (2024, 1), 15000m },
                { (2024, 2), 18500m },
                { (2024, 3), 21000m }
            };

            // Access using tuple key
            decimal januarySales = salesByMonth[(2024, 1)];
            Console.WriteLine($"\nJanuary 2024 sales: {januarySales:C}");
            // Output: January 2024 sales: $15,000.00

            // Check if tuple key exists
            bool hasFeb = salesByMonth.TryGetValue((2024, 2), out decimal febSales);
            Console.WriteLine($"February 2024 sales: {febSales:C}");
            // Output: February 2024 sales: $18,500.00

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: Dictionary with Custom Objects as Keys
            // ═══════════════════════════════════════════════════════════

            var inventory = new Dictionary<Product, int>
            {
                { new Product { Id = 1, Name = "Laptop" }, 50 },
                { new Product { Id = 2, Name = "Mouse" }, 200 },
                { new Product { Id = 3, Name = "Keyboard" }, 150 }
            };

            // Must implement GetHashCode and Equals for custom keys
            var lookupProduct = new Product { Id = 2, Name = "Mouse" };
            if (inventory.TryGetValue(lookupProduct, out int quantity))
            {
                Console.WriteLine($"\nMouse stock: {quantity}");
                // Output: Mouse stock: 200
            }

            // ═══════════════════════════════════════════════════════════
            // SECTION 6: Real-World Example - Configuration Manager
            // ═══════════════════════════════════════════════════════════

            var config = new Dictionary<string, string>
            {
                { "Server", "localhost" },
                { "Port", "8080" },
                { "Database", "mydb" },
                { "Username", "admin" },
                { "MaxConnections", "100" }
            };

            Console.WriteLine("\n=== Configuration Settings ===");

            // Get value with default fallback
            string timeout = config.GetValueOrDefault("Timeout", "30");
            Console.WriteLine($"Timeout: {timeout}");
            // Output: Timeout: 30

            // Get connection string components
            string server = config.GetValueOrDefault("Server", "unknown");
            string port = config.GetValueOrDefault("Port", "80");
            Console.WriteLine($"Connecting to {server}:{port}");
            // Output: Connecting to localhost:8080

            // Update configuration
            config["MaxConnections"] = "200";
            Console.WriteLine($"Updated MaxConnections: {config["MaxConnections"]}");
            // Output: Updated MaxConnections: 200

            // Remove setting
            config.Remove("Username");
            Console.WriteLine($"Config entries after removal: {config.Count}");
            // Output: Config entries after removal: 4

            // List all settings
            Console.WriteLine("\nAll settings:");
            foreach (var kvp in config)
            {
                Console.WriteLine($"  {kvp.Key} = {kvp.Value}");
            }

            Console.WriteLine("\n=== Dictionary Advanced Operations Complete ===");
        }
    }

    class Product : IEquatable<Product>
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public bool Equals(Product? other)
        {
            if (other is null) return false;
            return Id == other.Id && Name == other.Name;
        }

        public override bool Equals(object? obj) => Equals(obj as Product);

        public override int GetHashCode() => HashCode.Combine(Id, Name);
    }

    static class DictionaryExtensions
    {
        public static TValue GetValueOrDefault<TKey, TValue>(
            this Dictionary<TKey, TValue> dict, 
            TKey key, 
            TValue defaultValue) where TKey : notnull
        {
            return dict.TryGetValue(key, out TValue? value) ? value! : defaultValue;
        }
    }
}
