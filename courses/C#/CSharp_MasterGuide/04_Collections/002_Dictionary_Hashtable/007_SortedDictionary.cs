/*
 * ============================================================
 * TOPIC     : Collections
 * SUBTOPIC  : SortedDictionary<TKey,TValue> - Tree-Based Sorted Collection
 * FILE      : SortedDictionary.cs
 * PURPOSE   : Demonstrates SortedDictionary<TKey,TValue> which maintains
 *            entries sorted by key using a binary search tree
 * ============================================================
 */

using System;
using System.Collections.Generic;

namespace CSharp_MasterGuide._04_Collections._02_Dictionary_Hashtable
{
    class SortedDictionaryDemo
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== SortedDictionary<TKey,TValue> ===\n");

            // ═══════════════════════════════════════════════════════════
            // SECTION 1: Creating SortedDictionary
            // ═══════════════════════════════════════════════════════════

            // Empty sorted dictionary
            var emptySorted = new SortedDictionary<string, int>();
            Console.WriteLine($"Empty sorted dict count: {emptySorted.Count}");
            // Output: Empty sorted dict count: 0

            // Initialize with values (unsorted input will be sorted on output)
            var sortedCapitals = new SortedDictionary<string, string>
            {
                { "France", "Paris" },
                { "USA", "Washington D.C." },
                { "UK", "London" },
                { "Germany", "Berlin" },
                { "Japan", "Tokyo" }
            };

            Console.WriteLine($"\nSorted capitals (alphabetical by key):");
            foreach (var kvp in sortedCapitals)
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
            }
            // Output:
            //   France: Paris
            //   Germany: Berlin
            //   Japan: Tokyo
            //   UK: London
            //   USA: Washington D.C.

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: Basic Operations (Same as Dictionary)
            // ═══════════════════════════════════════════════════════════

            var scores = new SortedDictionary<string, int>();

            // Add elements (they will be automatically sorted)
            scores.Add("Zoe", 95);
            scores.Add("Alice", 87);
            scores.Add("Bob", 92);
            scores.Add("Charlie", 78);
            scores.Add("Diana", 88);

            Console.WriteLine($"\nScores (sorted by name):");
            foreach (var kvp in scores)
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
            }
            // Output:
            //   Alice: 87
            //   Bob: 92
            //   Charlie: 78
            //   Diana: 88
            //   Zoe: 95

            // TryGetValue
            bool found = scores.TryGetValue("Bob", out int bobScore);
            Console.WriteLine($"\nBob's score: {bobScore}");
            // Output: Bob's score: 92

            // ContainsKey
            Console.WriteLine($"ContainsKey 'Alice': {scores.ContainsKey("Alice")}");
            // Output: ContainsKey 'Alice': True

            // Remove
            scores.Remove("Charlie");
            Console.WriteLine($"After Remove, count: {scores.Count}");
            // Output: After Remove, count: 4

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: SortedDictionary vs Dictionary
            // ═══════════════════════════════════════════════════════════

            var regularDict = new Dictionary<string, int>
            {
                { "Zoe", 95 },
                { "Alice", 87 },
                { "Bob", 92 }
            };

            var sortedDict = new SortedDictionary<string, int>
            {
                { "Zoe", 95 },
                { "Alice", 87 },
                { "Bob", 92 }
            };

            Console.WriteLine("\n=== Regular Dictionary (unordered) ===");
            foreach (var kvp in regularDict)
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
            }
            // Order is unpredictable

            Console.WriteLine("\n=== SortedDictionary (ordered by key) ===");
            foreach (var kvp in sortedDict)
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
            }
            // Output: Always Alice, Bob, Zoe (alphabetical)

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: Using Custom Comparer
            // ═══════════════════════════════════════════════════════════

            // Case-insensitive sorting
            var caseInsensitive = new SortedDictionary<string, string>(
                StringComparer.OrdinalIgnoreCase)
            {
                { "Apple", "Red" },
                { "BANANA", "Yellow" },
                { "cherry", "Red" },
                { "Date", "Brown" }
            };

            Console.WriteLine("\n=== Case-Insensitive SortedDictionary ===");
            foreach (var kvp in caseInsensitive)
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
            }
            // Output: Apple, BANANA, cherry, Date (sorted ignoring case)
            // Actually sorted as: Apple, BANANA, cherry, Date

            // Reverse order using LINQ
            Console.WriteLine("\n=== Reversed Order ===");
            foreach (var kvp in caseInsensitive.Reverse())
            {
                Console.WriteLine($"  {kvp.Key}");
            }

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: Numeric Keys
            // ═══════════════════════════════════════════════════════════

            var salesByQuarter = new SortedDictionary<int, decimal>
            {
                { 4, 45000m },
                { 1, 32000m },
                { 3, 38000m },
                { 2, 29000m }
            };

            Console.WriteLine("\n=== Sales by Quarter (sorted numerically) ===");
            foreach (var kvp in salesByQuarter)
            {
                Console.WriteLine($"  Q{kvp.Key}: {kvp.Value:C}");
            }
            // Output:
            //   Q1: $32,000.00
            //   Q2: $29,000.00
            //   Q3: $38,000.00
            //   Q4: $45,000.00

            // ═══════════════════════════════════════════════════════════
            // SECTION 6: Performance Considerations
            // ═══════════════════════════════════════════════════════════
            
            // SortedDictionary:
            // - O(log n) for Add, Remove, TryGetValue, ContainsKey
            // - Maintains sorted order automatically
            // - Uses binary search tree internally
            // - Higher memory overhead than Dictionary
            // - Ideal when sorted iteration is needed
            
            var perfDict = new SortedDictionary<int, string>();
            
            // Add items
            for (int i = 1000; i >= 1; i--)
            {
                perfDict[i] = $"Item{i}";
            }
            
            // Items are automatically sorted
            Console.WriteLine($"\nFirst 5 keys (should be 1-5):");
            int count = 0;
            foreach (var key in perfDict.Keys)
            {
                Console.Write($"{key} ");
                if (++count >= 5) break;
            }
            // Output: 1 2 3 4 5

            // ═══════════════════════════════════════════════════════════
            // SECTION 7: Real-World Example - Leaderboard
            // ═══════════════════════════════════════════════════════════

            var leaderboard = new SortedDictionary<int, string>(Comparer<int>.Create((a, b) => b.CompareTo(a)))
            {
                { 1500, "Player1" },
                { 2300, "Player2" },
                { 1800, "Player3" },
                { 2100, "Player4" },
                { 900, "Player5" }
            };

            Console.WriteLine("\n=== Leaderboard (sorted by score descending) ===");
            int rank = 1;
            foreach (var kvp in leaderboard)
            {
                Console.WriteLine($"  #{rank}: {kvp.Value} - {kvp.Key} points");
                rank++;
            }
            // Output:
            //   #1: Player2 - 2300 points
            //   #2: Player4 - 2100 points
            //   #3: Player3 - 1800 points
            //   #4: Player1 - 1500 points
            //   #5: Player5 - 900 points

            Console.WriteLine("\n=== SortedDictionary Complete ===");
        }
    }
}
