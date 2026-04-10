/*
 * ============================================================
 * TOPIC     : Collections
 * SUBTOPIC  : Dictionary<TKey,TValue> - Core Operations
 * FILE      : Dictionary.cs
 * PURPOSE   : Demonstrates Dictionary<TKey,TValue> fundamentals
 *            including Add, TryGetValue, ContainsKey, and iteration
 * ============================================================
 */

using System;
using System.Collections.Generic;

namespace CSharp_MasterGuide._04_Collections._02_Dictionary_Hashtable
{
    class DictionaryBasics
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Dictionary<TKey,TValue> Basics ===\n");

            // ═══════════════════════════════════════════════════════════
            // SECTION 1: Creating Dictionary
            // ═══════════════════════════════════════════════════════════

            // Empty dictionary with string key and string value
            var emptyDict = new Dictionary<string, int>();
            Console.WriteLine($"Empty dict count: {emptyDict.Count}");
            // Output: Empty dict count: 0

            // Initialize with collection initializer syntax
            var capitals = new Dictionary<string, string>
            {
                ["USA"] = "Washington D.C.",
                ["UK"] = "London",
                ["France"] = "Paris"
            };
            Console.WriteLine($"Capitals count: {capitals.Count}");
            // Output: Capitals count: 3

            // Alternative initialization using Add method
            var colors = new Dictionary<string, string>
            {
                { "Red", "#FF0000" },
                { "Green", "#00FF00" },
                { "Blue", "#0000FF" }
            };
            Console.WriteLine($"Colors count: {colors.Count}");
            // Output: Colors count: 3

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: Adding Elements
            // ═══════════════════════════════════════════════════════════

            var scores = new Dictionary<string, int>();

            // Add key-value pair (throws exception if key exists)
            scores.Add("Alice", 95);
            scores.Add("Bob", 87);
            scores.Add("Charlie", 92);
            Console.WriteLine($"\nAfter Add: {scores.Count} entries");
            // Output: After Add: 3 entries

            // Using indexer to add (overwrites if key exists)
            scores["David"] = 78;
            scores["Eve"] = 88;
            Console.WriteLine($"After indexer add: {scores.Count} entries");
            // Output: After indexer add: 5 entries

            // Overwriting existing value using indexer
            scores["Bob"] = 91; // Updates Bob's score
            Console.WriteLine($"Bob's updated score: {scores["Bob"]}");
            // Output: Bob's updated score: 91

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: Accessing Values
            // ═══════════════════════════════════════════════════════════

            // Direct access via indexer (throws KeyNotFoundException if key missing)
            int aliceScore = scores["Alice"];
            Console.WriteLine($"\nAlice's score: {aliceScore}");
            // Output: Alice's score: 95

            // Using TryGetValue (safe access - returns false if key missing)
            bool found = scores.TryGetValue("Charlie", out int charlieScore);
            Console.WriteLine($"TryGetValue for Charlie: {charlieScore}");
            // Output: TryGetValue for Charlie: 92

            // TryGetValue with non-existent key
            bool notFound = scores.TryGetValue("Zack", out int zackScore);
            Console.WriteLine($"TryGetValue for Zack (found: {notFound}), score: {zackScore}");
            // Output: TryGetValue for Zack (found: False), score: 0

            // Check if key exists
            bool hasAlice = scores.ContainsKey("Alice");
            bool hasZack = scores.ContainsKey("Zack");
            Console.WriteLine($"ContainsKey 'Alice': {hasAlice}, 'Zack': {hasZack}");
            // Output: ContainsKey 'Alice': True, 'Zack': False

            // Check if value exists
            bool hasScore95 = scores.ContainsValue(95);
            Console.WriteLine($"ContainsValue 95: {hasScore95}");
            // Output: ContainsValue 95: True

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: Iterating Dictionary
            // ═══════════════════════════════════════════════════════════

            // Iterate over key-value pairs
            Console.WriteLine("\nAll scores:");
            foreach (KeyValuePair<string, int> entry in scores)
            {
                Console.WriteLine($"  {entry.Key}: {entry.Value}");
            }
            // Output:
            //   Alice: 95
            //   Bob: 91
            //   Charlie: 92
            //   David: 78
            //   Eve: 88

            // Iterate over keys only
            Console.WriteLine("\nAll names:");
            foreach (string name in scores.Keys)
            {
                Console.WriteLine($"  {name}");
            }
            // Output:
            //   Alice
            //   Bob
            //   Charlie
            //   David
            //   Eve

            // Iterate over values only
            Console.WriteLine("\nAll score values:");
            foreach (int score in scores.Values)
            {
                Console.WriteLine($"  Score: {score}");
            }
            // Output:
            //   Score: 95
            //   Score: 91
            //   Score: 92
            //   Score: 78
            //   Score: 88

            // Using LINQ to find keys with score > 90
            var highScorers = scores.Where(s => s.Value > 90).Select(s => s.Key);
            Console.WriteLine($"\nHigh scorers (>90): {string.Join(", ", highScorers)}");
            // Output: High scorers (>90): Alice, Bob, Charlie

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: Real-World Example - Employee Directory
            // ═══════════════════════════════════════════════════════════

            var employeeDirectory = new Dictionary<int, Employee>
            {
                { 101, new Employee { Id = 101, Name = "John Smith", Department = "Engineering" } },
                { 102, new Employee { Id = 102, Name = "Jane Doe", Department = "Marketing" } },
                { 103, new Employee { Id = 103, Name = "Mike Johnson", Department = "Engineering" } }
            };

            Console.WriteLine("\n=== Employee Directory ===");

            // Lookup employee by ID
            if (employeeDirectory.TryGetValue(102, out Employee emp))
            {
                Console.WriteLine($"Found: {emp.Name} - {emp.Department}");
                // Output: Found: Jane Doe - Marketing
            }

            // Add new employee
            employeeDirectory[104] = new Employee { Id = 104, Name = "Sarah Williams", Department = "HR" };
            Console.WriteLine($"Total employees: {employeeDirectory.Count}");
            // Output: Total employees: 4

            // List all engineers
            var engineers = employeeDirectory.Values.Where(e => e.Department == "Engineering");
            Console.WriteLine("Engineers:");
            foreach (var e in engineers)
            {
                Console.WriteLine($"  {e.Name}");
            }
            // Output:
            //   John Smith
            //   Mike Johnson

            Console.WriteLine("\n=== Dictionary<TKey,TValue> Basics Complete ===");
        }
    }

    class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
    }
}
