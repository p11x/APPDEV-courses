/*
 * ============================================================
 * TOPIC     : Collections
 * SUBTOPIC  : SortedSet<T> - Ordered Unique Collection
 * FILE      : SortedSet.cs
 * PURPOSE   : Teaches SortedSet<T> - maintains elements in sorted
 *             order with uniqueness, includes Min, Max, GetViewBetween
 * ============================================================
 */

using System;
using System.Collections.Generic;

namespace CSharp_MasterGuide._04_Collections._04_HashSet_SortedSet
{
    class SortedSet
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== SortedSet<T> Ordered Collection ===\n");

            // ═══════════════════════════════════════════════════════════
            // SECTION 1: Creating SortedSet<T>
            // ═══════════════════════════════════════════════════════════

            // Empty SortedSet - elements will be automatically sorted
            var emptySorted = new SortedSet<int>();
            Console.WriteLine($"Empty SortedSet count: {emptySorted.Count}");
            // Output: 0

            // Initialize with values - automatically sorted
            var numbers = new SortedSet<int> { 5, 2, 8, 1, 9, 3 };
            Console.WriteLine($"Numbers count: {numbers.Count}");
            // Output: 6

            Console.WriteLine("Numbers in sorted order:");
            foreach (var num in numbers)
            {
                Console.Write(num + " ");
            }
            Console.WriteLine();
            // Output: 1 2 3 5 8 9

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: Adding Elements
            // ═══════════════════════════════════════════════════════════

            var sortedColors = new SortedSet<string>();

            // Add returns true if element was added, false if duplicate
            bool added1 = sortedColors.Add("Red");
            bool added2 = sortedColors.Add("Blue");
            bool added3 = sortedColors.Add("Green");
            bool added4 = sortedColors.Add("Blue"); // Duplicate

            Console.WriteLine($"\nAdd results: Red={added1}, Blue={added2}, Green={added3}, Blue again={added4}");
            // Output: True, True, True, False
            Console.WriteLine($"Count: {sortedColors.Count}");
            // Output: 3

            Console.WriteLine("Sorted colors:");
            foreach (var color in sortedColors)
            {
                Console.Write(color + " ");
            }
            Console.WriteLine();
            // Output: Blue Green Red

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: Min and Max Properties
            // ═══════════════════════════════════════════════════════════

            var scores = new SortedSet<int> { 95, 87, 92, 78, 100, 88 };

            int minimum = scores.Min;
            int maximum = scores.Max;

            Console.WriteLine($"\nScores: {string.Join(", ", scores)}");
            Console.WriteLine($"Min: {minimum}");
            // Output: 78
            Console.WriteLine($"Max: {maximum}");
            // Output: 100

            // With strings - Min/Max use alphabetical order
            var words = new SortedSet<string> { "apple", "zebra", "mango", "banana" };
            Console.WriteLine($"\nWords: {string.Join(", ", words)}");
            Console.WriteLine($"First (Min): {words.Min}");
            // Output: apple
            Console.WriteLine($"Last (Max): {words.Max}");
            // Output: zebra

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: GetViewBetween - Range View
            // ═══════════════════════════════════════════════════════════

            var rangeSet = new SortedSet<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            // GetViewBetween returns a subset within the specified range (inclusive)
            var midRange = rangeSet.GetViewBetween(3, 7);

            Console.WriteLine($"\nOriginal: {string.Join(", ", rangeSet)}");
            Console.WriteLine($"View between 3 and 7: {string.Join(", ", midRange)}");
            // Output: 3, 4, 5, 6, 7

            // Modifying the view affects the original set
            midRange.Add(6); // Already exists
            Console.WriteLine($"After adding 6 to view, original count: {rangeSet.Count}");
            // Output: 10 (no change)

            midRange.Add(35); // Out of range - still added to view
            Console.WriteLine($"After adding 35 to view, original count: {rangeSet.Count}");
            // Output: 11

            Console.WriteLine($"View after adding 35: {string.Join(", ", midRange)}");
            // Output: 3, 4, 5, 6, 7, 35

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: Set Operations (Same as HashSet)
            // ═══════════════════════════════════════════════════════════

            var sortedA = new SortedSet<int> { 1, 2, 3, 4, 5 };
            var sortedB = new SortedSet<int> { 4, 5, 6, 7, 8 };

            // UnionWith
            var unionSet = new SortedSet<int> { 1, 2, 3, 4, 5 };
            unionSet.UnionWith(sortedB);
            Console.WriteLine($"\nUnion: {string.Join(", ", unionSet)}");
            // Output: 1, 2, 3, 4, 5, 6, 7, 8

            // IntersectWith
            var intersectSet = new SortedSet<int> { 1, 2, 3, 4, 5 };
            intersectSet.IntersectWith(sortedB);
            Console.WriteLine($"Intersect: {string.Join(", ", intersectSet)}");
            // Output: 4, 5

            // ExceptWith
            var exceptSet = new SortedSet<int> { 1, 2, 3, 4, 5 };
            exceptSet.ExceptWith(sortedB);
            Console.WriteLine($"Except: {string.Join(", ", exceptSet)}");
            // Output: 1, 2, 3

            // ═══════════════════════════════════════════════════════════
            // SECTION 6: Reverse Iteration
            // ═══════════════════════════════════════════════════════════

            var reverseDemo = new SortedSet<int> { 3, 1, 4, 1, 5, 9, 2, 6 };

            Console.WriteLine($"\nNormal iteration: {string.Join(", ", reverseDemo)}");
            // Output: 1, 2, 3, 4, 5, 6, 9

            // Using Reverse for descending order
            Console.Write("Reverse iteration: ");
            foreach (var item in reverseDemo.Reverse())
            {
                Console.Write(item + " ");
            }
            Console.WriteLine();
            // Output: 9 6 5 4 3 2 1

            // ═══════════════════════════════════════════════════════════
            // SECTION 7: Custom Comparer
            // ═══════════════════════════════════════════════════════════

            // Case-insensitive string comparison
            var caseInsensitive = new SortedSet<string>(StringComparer.OrdinalIgnoreCase);
            caseInsensitive.Add("Apple");
            caseInsensitive.Add("APPLE");
            caseInsensitive.Add("Banana");
            caseInsensitive.Add("banana");

            Console.WriteLine($"\nCase-insensitive: {string.Join(", ", caseInsensitive)}");
            // Output: Apple, Banana

            // Reverse alphabetical order with custom comparer
            var reverseOrder = new SortedSet<string>(new ReverseStringComparer());
            reverseOrder.Add("Zebra");
            reverseOrder.Add("Apple");
            reverseOrder.Add("Monkey");

            Console.WriteLine($"Reverse alphabetical: {string.Join(", ", reverseOrder)}");
            // Output: Zebra, Monkey, Apple

            // ═══════════════════════════════════════════════════════════
            // SECTION 8: Real-World Examples
            // ═══════════════════════════════════════════════════════════

            // Example 1: Sorted leaderboard with unique scores
            var leaderboard = new SortedSet<int>(Comparer<int>.Create((a, b) => b.CompareTo(a)));

            leaderboard.Add(1000);
            leaderboard.Add(2500);
            leaderboard.Add(500);
            leaderboard.Add(2500); // Duplicate
            leaderboard.Add(3000);

            Console.WriteLine($"\n--- Leaderboard (High to Low) ---");
            int rank = 1;
            foreach (var score in leaderboard)
            {
                Console.WriteLine($"Rank {rank++}: {score}");
            }
            // Output: Rank 1: 3000, Rank 2: 2500, Rank 3: 1000, Rank 4: 500

            // Example 2: Date-based event scheduling
            var events = new SortedSet<DateTime>();
            events.Add(new DateTime(2024, 6, 15));
            events.Add(new DateTime(2024, 1, 10));
            events.Add(new DateTime(2024, 12, 25));
            events.Add(new DateTime(2024, 6, 15)); // Duplicate

            Console.WriteLine($"\n--- Upcoming Events ---");
            foreach (var evt in events)
            {
                Console.WriteLine(evt.ToString("yyyy-MM-dd"));
            }
            // Output: 2024-01-10, 2024-06-15, 2024-12-25

            // Example 3: Get range of items efficiently
            var priceRanges = new SortedSet<decimal> { 10.00m, 25.50m, 50.00m, 75.25m, 100.00m, 250.00m };

            // Get items in a price range
            var budgetItems = priceRanges.GetViewBetween(25.00m, 100.00m);
            Console.WriteLine($"\n--- Budget Items ($25-$100) ---");
            Console.WriteLine(string.Join(", ", budgetItems));
            // Output: 25.5, 50, 75.25, 100

            Console.WriteLine("\n=== SortedSet Complete ===");
        }
    }

    // Custom comparer for reverse alphabetical order
    class ReverseStringComparer : IComparer<string>
    {
        public int Compare(string? x, string? y)
        {
            if (x == null && y == null) return 0;
            if (x == null) return 1;
            if (y == null) return -1;
            return y.CompareTo(x); // Reversed comparison
        }
    }
}
