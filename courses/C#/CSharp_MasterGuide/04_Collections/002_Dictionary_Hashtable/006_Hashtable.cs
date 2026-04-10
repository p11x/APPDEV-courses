/*
 * ============================================================
 * TOPIC     : Collections
 * SUBTOPIC  : Hashtable - Non-Generic Legacy Collection
 * FILE      : Hashtable.cs
 * PURPOSE   : Demonstrates Hashtable (non-generic) which stores
 *            key-value pairs using hashing. Legacy but still useful
 *            for interoperability with older code.
 * ============================================================
 */

using System;
using System.Collections;

namespace CSharp_MasterGuide._04_Collections._02_Dictionary_Hashtable
{
    class HashtableDemo
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Hashtable (Non-Generic) ===\n");

            // ═══════════════════════════════════════════════════════════
            // SECTION 1: Creating Hashtable
            // ═══════════════════════════════════════════════════════════

            // Empty hashtable (non-generic, stores object references)
            var emptyHash = new Hashtable();
            Console.WriteLine($"Empty hashtable count: {emptyHash.Count}");
            // Output: Empty hashtable count: 0

            // Initialize with collection initializer (using Add method)
            var capitals = new Hashtable
            {
                { "USA", "Washington D.C." },
                { "UK", "London" },
                { "France", "Paris" }
            };
            Console.WriteLine($"Capitals count: {capitals.Count}");
            // Output: Capitals count: 3

            // Initialize using indexer syntax
            var colors = new Hashtable
            {
                ["Red"] = "#FF0000",
                ["Green"] = "#00FF00",
                ["Blue"] = "#0000FF"
            };
            Console.WriteLine($"Colors count: {colors.Count}");
            // Output: Colors count: 3

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: Adding and Accessing Elements
            // ═══════════════════════════════════════════════════════════

            var scores = new Hashtable();

            // Add key-value pairs (requires casting due to non-generic)
            scores["Alice"] = 95;
            scores["Bob"] = 87;
            scores["Charlie"] = 92;
            Console.WriteLine($"\nAdded {scores.Count} entries");

            // Access value using indexer (returns object, needs casting)
            int aliceScore = (int)scores["Alice"];
            Console.WriteLine($"Alice's score: {aliceScore}");
            // Output: Alice's score: 95

            // TryGetValue returns DictionaryEntry
            object charlieObj = scores["Charlie"];
            if (charlieObj != null)
            {
                int charlieScore = (int)charlieObj;
                Console.WriteLine($"Charlie's score: {charlieScore}");
                // Output: Charlie's score: 92
            }

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: Checking Keys and Values
            // ═══════════════════════════════════════════════════════════

            // ContainsKey (similar to Dictionary)
            bool hasAlice = scores.ContainsKey("Alice");
            bool hasZack = scores.ContainsKey("Zack");
            Console.WriteLine($"\nContainsKey 'Alice': {hasAlice}, 'Zack': {hasZack}");
            // Output: ContainsKey 'Alice': True, 'Zack': False

            // Contains (checks for key)
            bool contains = scores.Contains("Bob");
            Console.WriteLine($"Contains 'Bob': {contains}");
            // Output: Contains 'Bob': True

            // ContainsValue
            bool hasScore92 = scores.ContainsValue(92);
            Console.WriteLine($"ContainsValue 92: {hasScore92}");
            // Output: ContainsValue 92: True

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: Iterating Hashtable
            // ═══════════════════════════════════════════════════════════

            // Iterate over DictionaryEntry objects
            Console.WriteLine("\nAll entries:");
            foreach (DictionaryEntry entry in scores)
            {
                Console.WriteLine($"  {entry.Key}: {entry.Value}");
            }
            // Output:
            //   Alice: 95
            //   Bob: 87
            //   Charlie: 92

            // Iterate over keys only
            Console.WriteLine("\nAll keys:");
            foreach (string key in scores.Keys)
            {
                Console.WriteLine($"  {key}");
            }

            // Iterate over values only
            Console.WriteLine("\nAll values:");
            foreach (int value in scores.Values)
            {
                Console.WriteLine($"  Score: {value}");
            }

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: Removing and Clearing
            // ═══════════════════════════════════════════════════════════

            var config = new Hashtable
            {
                { "Server", "localhost" },
                { "Port", 8080 },
                { "Timeout", 30 }
            };

            // Remove by key
            config.Remove("Timeout");
            Console.WriteLine($"\nRemove 'Timeout': True");
            // Output: Remove 'Timeout': True

            // Clear all
            config.Clear();
            Console.WriteLine($"After Clear, count: {config.Count}");
            // Output: After Clear, count: 0

            // ═══════════════════════════════════════════════════════════
            // SECTION 6: Heterogeneous Data Storage
            // ═══════════════════════════════════════════════════════════

            // Hashtable can store different types (boxed as object)
            var mixed = new Hashtable
            {
                { "Name", "John" },           // string
                { "Age", 30 },                // int
                { "Salary", 5000.50 },        // double
                { "IsActive", true },         // bool
                { "Count", 100L }             // long
            };

            Console.WriteLine("\n=== Mixed Type Hashtable ===");
            foreach (DictionaryEntry entry in mixed)
            {
                Console.WriteLine($"  {entry.Key}: {entry.Value} ({entry.Value.GetType().Name})");
            }
            // Output:
            //   Name: John (String)
            //   Age: 30 (Int32)
            //   Salary: 5000.5 (Double)
            //   IsActive: Boolean (True)
            //   Count: 100 (Int64)

            // ═══════════════════════════════════════════════════════════
            // SECTION 7: Hashtable vs Dictionary
            // ═══════════════════════════════════════════════════════════

            // Hashtable:
            // - Non-generic, stores object references
            // - Supports thread-safe synchronized wrapper
            // - Allows null key (one) and null values (multiple)
            // - Slower due to boxing/unboxing
            // - Use for legacy code interop or dynamic keys

            // Get synchronized (thread-safe) wrapper
            var syncHash = Hashtable.Synchronized(new Hashtable
            {
                { "A", 1 },
                { "B", 2 }
            });

            Console.WriteLine($"\nIsSynchronized: {syncHash.IsSynchronized}");
            // Output: IsSynchronized: True

            // ═══════════════════════════════════════════════════════════
            // SECTION 8: Real-World Example - Dynamic Configuration
            // ═══════════════════════════════════════════════════════════

            var appConfig = new Hashtable
            {
                { "AppName", "MyApplication" },
                { "Version", "1.0.0" },
                { "MaxUsers", 1000 },
                { "EnableLogging", true },
                { "LogPath", @"C:\Logs" }
            };

            Console.WriteLine("\n=== Application Configuration ===");

            // Get values with type checking
            string appName = appConfig["AppName"]?.ToString() ?? "Unknown";
            Console.WriteLine($"App Name: {appName}");
            // Output: App Name: MyApplication

            int maxUsers = Convert.ToInt32(appConfig["MaxUsers"]);
            Console.WriteLine($"Max Users: {maxUsers}");
            // Output: Max Users: 1000

            bool loggingEnabled = Convert.ToBoolean(appConfig["EnableLogging"]);
            Console.WriteLine($"Logging Enabled: {loggingEnabled}");
            // Output: Logging Enabled: True

            // List all config entries
            Console.WriteLine("\nAll Configuration:");
            foreach (DictionaryEntry entry in appConfig)
            {
                Console.WriteLine($"  {entry.Key} = {entry.Value}");
            }

            Console.WriteLine("\n=== Hashtable Complete ===");
        }
    }
}
