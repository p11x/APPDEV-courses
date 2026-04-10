/*
 * TOPIC: Immutable Collections
 * SUBTOPIC: ImmutableDictionary<TKey, TValue> - Key-Value Collections
 * FILE: ImmutableDictionary.cs
 * PURPOSE: Demonstrate ImmutableDictionary creation, modification operations,
 *          and lookup patterns
 */
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace CSharp_MasterGuide._04_Collections._07_ImmutableCollections
{
    public class ImmutableDictionaryDemo
    {
        public static void Main()
        {
            Console.WriteLine("=== ImmutableDictionary<TKey, TValue> Basics ===\n");

            CreatingImmutableDictionary();
            AddingAndUpdating();
            RemovingElements();
            QueryingAndSearching();
            EnumerationOperations();
            RealWorldConfiguration();
        }

        static void CreatingImmutableDictionary()
        {
            Console.WriteLine("--- 1. Creating ImmutableDictionary ---");
            Console.WriteLine();

            // Create empty dictionary
            var empty = ImmutableDictionary<string, int>.Empty;
            Console.WriteLine($"  Empty dict count: {empty.Count}");
            // Output: Empty dict count: 0

            // Create with initial key-value pairs
            var settings = ImmutableDictionary<string, string>.Empty
                .Add("Theme", "Dark")
                .Add("Language", "English")
                .Add("FontSize", "14");

            Console.WriteLine($"  Settings: {settings.Count} entries");
            // Output: Settings: 3 entries

            foreach (var kvp in settings)
            {
                Console.WriteLine($"    {kvp.Key} = {kvp.Value}");
            }
            // Output:
            //   Theme = Dark
            //   Language = English
            //   FontSize = 14

            // Create from existing dictionary
            var existing = new Dictionary<string, decimal>
            {
                { "Price", 99.99m },
                { "Tax", 7.50m },
                { "Shipping", 5.00m }
            };

            var fromExisting = ImmutableDictionary.CreateRange(existing);
            Console.WriteLine($"  From existing: {string.Join(", ", fromExisting.Select(kv => $"{kv.Key}={kv.Value}"))}");
            // Output: From existing: Price=99.99, Tax=7.5, Shipping=5
            Console.WriteLine();
        }

        static void AddingAndUpdating()
        {
            Console.WriteLine("--- 2. Adding and Updating Entries ---");
            Console.WriteLine();

            var scores = ImmutableDictionary<string, int>.Empty
                .Add("Alice", 95)
                .Add("Bob", 87)
                .Add("Charlie", 92);

            Console.WriteLine($"  Initial: {string.Join(", ", scores.Select(kv => $"{kv.Key}={kv.Value}"))}");
            // Output: Initial: Alice=95, Bob=87, Charlie=92

            // Add new entry - returns new dictionary
            var withDave = scores.Add("Dave", 78);
            Console.WriteLine($"  After Add('Dave', 78): {string.Join(", ", withDave.Select(kv => $"{kv.Key}={kv.Value}"))}");
            // Output: After Add('Dave', 78): Alice=95, Bob=87, Charlie=92, Dave=78

            // TryAdd - safe add (doesn't throw if key exists)
            var tryAddResult = scores.TryGetValue("Alice", out _);
            var safeAdd = withDave.TryAdd("Alice", 100); // Won't change existing
            Console.WriteLine($"  TryAdd('Alice', 100) succeeded: {safeAdd}");
            // Output: TryAdd('Alice', 100) succeeded: False

            // SetItem - add or update existing key
            var updated = withDave.SetItem("Bob", 95);
            Console.WriteLine($"  After SetItem('Bob', 95): {string.Join(", ", updated.Select(kv => $"{kv.Key}={kv.Value}"))}");
            // Output: After SetItem('Bob', 95): Alice=95, Bob=95, Charlie=92, Dave=78

            // SetItems - batch update/insert
            var multiUpdate = scores.SetItems(new Dictionary<string, int>
            {
                { "Dave", 85 },
                { "Eve", 90 }
            });
            Console.WriteLine($"  After SetItems: {string.Join(", ", multiUpdate.Select(kv => $"{kv.Key}={kv.Value}"))}");
            // Output: After SetItems: Alice=95, Bob=87, Charlie=92, Dave=85, Eve=90

            // Original unchanged
            Console.WriteLine($"  Original unchanged: {string.Join(", ", scores.Select(kv => $"{kv.Key}={kv.Value}"))}");
            // Output: Original unchanged: Alice=95, Bob=87, Charlie=92
            Console.WriteLine();
        }

        static void RemovingElements()
        {
            Console.WriteLine("--- 3. Removing Elements ---");
            Console.WriteLine();

            var products = ImmutableDictionary<string, decimal>.Empty
                .Add("Laptop", 999.99m)
                .Add("Mouse", 29.99m)
                .Add("Keyboard", 79.99m)
                .Add("Monitor", 299.99m)
                .Add("Headphones", 149.99m);

            Console.WriteLine($"  Original: {string.Join(", ", products.Select(kv => $"{kv.Key}={kv.Value:C}"))}");
            // Output: Original: Laptop=$999.99, Mouse=$29.99, Keyboard=$79.99, Monitor=$299.99, Headphones=$149.99

            // Remove by key
            var withoutMouse = products.Remove("Mouse");
            Console.WriteLine($"  After Remove('Mouse'): {string.Join(", ", withoutMouse.Select(kv => $"{kv.Key}={kv.Value:C}"))}");
            // Output: After Remove('Mouse'): Laptop=$999.99, Keyboard=$79.99, Monitor=$299.99, Headphones=$149.99

            // Remove multiple keys
            var withoutFew = products.Remove(new[] { "Mouse", "Keyboard" });
            Console.WriteLine($"  After Remove(['Mouse', 'Keyboard']): {string.Join(", ", withoutFew.Select(kv => $"{kv.Key}={kv.Value:C}"))}");
            // Output: After Remove(['Mouse', 'Keyboard']): Laptop=$999.99, Monitor=$299.99, Headphones=$149.99

            // Clear all
            var cleared = products.Clear();
            Console.WriteLine($"  After Clear: Count = {cleared.Count}");
            // Output: After Clear: Count = 0
            Console.WriteLine();
        }

        static void QueryingAndSearching()
        {
            Console.WriteLine("--- 4. Querying and Searching ---");
            Console.WriteLine();

            var inventory = ImmutableDictionary<string, int>.Empty
                .Add("Apples", 150)
                .Add("Oranges", 75)
                .Add("Bananas", 200)
                .Add("Grapes", 50)
                .Add("Mangoes", 30);

            Console.WriteLine($"  Inventory: {string.Join(", ", inventory.Select(kv => $"{kv.Key}={kv.Value}"))}");
            // Output: Inventory: Apples=150, Oranges=75, Bananas=200, Grapes=50, Mangoes=30

            // TryGetValue - safe lookup
            bool found = inventory.TryGetValue("Bananas", out int bananaCount);
            Console.WriteLine($"  TryGetValue('Bananas'): Found={found}, Value={bananaCount}");
            // Output: TryGetValue('Bananas'): Found=True, Value=200

            // ContainsKey
            bool hasMango = inventory.ContainsKey("Mangoes");
            Console.WriteLine($"  ContainsKey('Mangoes'): {hasMango}");
            // Output: ContainsKey('Mangoes'): True

            // Contains - check for key-value pair
            bool hasApples100 = inventory.Contains(new KeyValuePair<string, int>("Apples", 100));
            bool hasApples150 = inventory.Contains(new KeyValuePair<string, int>("Apples", 150));
            Console.WriteLine($"  Contains(Apples=100): {hasApples100}, Contains(Apples=150): {hasApples150}");
            // Output: Contains(Apples=100): False, Contains(Apples=150): True

            // GetValueOrDefault
            int unknownCount = inventory.GetValueOrDefault("Unknown", 0);
            int mangoCount = inventory.GetValueOrDefault("Mangoes", 0);
            Console.WriteLine($"  GetValueOrDefault('Unknown', 0): {unknownCount}");
            Console.WriteLine($"  GetValueOrDefault('Mangoes', 0): {mangoCount}");
            // Output: GetValueOrDefault('Unknown', 0): 0
            // Output: GetValueOrDefault('Mangoes', 0): 30
            Console.WriteLine();
        }

        static void EnumerationOperations()
        {
            Console.WriteLine("--- 5. Enumeration Operations ---");
            Console.WriteLine();

            var capitals = ImmutableDictionary<string, string>.Empty
                .Add("USA", "Washington D.C.")
                .Add("UK", "London")
                .Add("France", "Paris")
                .Add("Germany", "Berlin")
                .Add("Japan", "Tokyo");

            // Keys
            Console.WriteLine("  Keys: " + string.Join(", ", capitals.Keys));
            // Output: Keys: USA, UK, France, Germany, Japan

            // Values
            Console.WriteLine("  Values: " + string.Join(", ", capitals.Values));
            // Output: Values: Washington D.C., London, Paris, Berlin, Tokyo

            // Iterate with LINQ
            var sortedByValue = capitals.OrderBy(kv => kv.Value).ToImmutableDictionary(kv => kv.Key, kv => kv.Value);
            Console.WriteLine("  Sorted by value:");
            foreach (var kv in sortedByValue)
            {
                Console.WriteLine($"    {kv.Key} -> {kv.Value}");
            }
            // Output:
            //   Germany -> Berlin
            //   Japan -> Tokyo
            //   UK -> London
            //   France -> Paris
            //   USA -> Washington D.C.

            // Filter and create new dictionary
            var shortNames = capitals.Where(kv => kv.Key.Length <= 3).ToImmutableDictionary(kv => kv.Key, kv => kv.Value);
            Console.WriteLine($"  Keys with length <= 3: {string.Join(", ", shortNames.Keys)}");
            // Output: Keys with length <= 3: USA, UK
            Console.WriteLine();
        }

        static void RealWorldConfiguration()
        {
            Console.WriteLine("--- Real-World: Application Configuration ---");
            Console.WriteLine();

            // Configuration is a perfect use case for immutability
            var baseConfig = ImmutableDictionary<string, object>.Empty
                .Add("Database", "localhost:5432")
                .Add("MaxConnections", 100)
                .Add("EnableLogging", true)
                .Add("Timeout", 30);

            Console.WriteLine("  Base Configuration:");
            foreach (var kv in baseConfig)
            {
                Console.WriteLine($"    {kv.Key}: {kv.Value}");
            }
            // Output:
            //   Database: localhost:5432
            //   MaxConnections: 100
            //   EnableLogging: True
            //   Timeout: 30

            // Environment-specific overrides (development)
            var devConfig = baseConfig
                .SetItem("Database", "dev.local:5432")
                .SetItem("EnableLogging", true);

            Console.WriteLine("\n  Development Configuration:");
            foreach (var kv in devConfig)
            {
                Console.WriteLine($"    {kv.Key}: {kv.Value}");
            }
            // Output:
            //   Database: dev.local:5432
            //   MaxConnections: 100
            //   EnableLogging: True
            //   Timeout: 30

            // Environment-specific overrides (production)
            var prodConfig = baseConfig
                .SetItem("Database", "prod.db.com:5432")
                .SetItem("MaxConnections", 500)
                .SetItem("EnableLogging", false);

            Console.WriteLine("\n  Production Configuration:");
            foreach (var kv in prodConfig)
            {
                Console.WriteLine($"    {kv.Key}: {kv.Value}");
            }
            // Output:
            //   Database: prod.db.com:5432
            //   MaxConnections: 500
            //   EnableLogging: False
            //   Timeout: 30

            // Original config unchanged - can be safely shared
            Console.WriteLine("\n  Original base config still available:");
            Console.WriteLine($"    Database: {baseConfig["Database"]}");
            // Output: Database: localhost:5432
            Console.WriteLine();
        }
    }
}