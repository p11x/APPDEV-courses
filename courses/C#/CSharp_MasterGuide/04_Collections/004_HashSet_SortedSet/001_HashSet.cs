/*
 * ============================================================
 * TOPIC     : Collections
 * SUBTOPIC  : HashSet<T> - Unique Collection Basics
 * FILE      : HashSet.cs
 * PURPOSE   : Teaches HashSet<T> fundamentals - adding, removing,
 *             and basic set operations (Union, Intersect)
 * ============================================================
 */

using System;
using System.Collections.Generic;

namespace CSharp_MasterGuide._04_Collections._04_HashSet_SortedSet
{
    class HashSetDemo
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== HashSet<T> Fundamentals ===\n");

            // ═══════════════════════════════════════════════════════════
            // SECTION 1: Creating HashSet<T>
            // ═══════════════════════════════════════════════════════════

            // Empty HashSet - initialized with default capacity
            var emptySet = new HashSet<int>();
            Console.WriteLine($"Empty HashSet count: {emptySet.Count}");
            // Output: 0

            // HashSet initialized with collection initializer syntax
            var colors = new HashSet<string> { "Red", "Green", "Blue" };
            Console.WriteLine($"Colors count: {colors.Count}");
            // Output: 3

            // HashSet with custom equality comparer (case-insensitive)
            var caseInsensitive = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            caseInsensitive.Add("Hello");
            caseInsensitive.Add("HELLO"); // Treated as duplicate
            Console.WriteLine($"Case-insensitive count: {caseInsensitive.Count}");
            // Output: 1

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: Adding Elements
            // ═══════════════════════════════════════════════════════════

            var numbers = new HashSet<int>();

            // Add returns true if element was successfully added
            bool added1 = numbers.Add(10);
            bool added2 = numbers.Add(20);
            bool added3 = numbers.Add(30);

            Console.WriteLine($"\nAfter adding 10, 20, 30:");
            Console.WriteLine($"Add 10: {added1}, Add 20: {added2}, Add 30: {added3}");
            // Output: True, True, True
            Console.WriteLine($"Count: {numbers.Count}");
            // Output: 3

            // Adding duplicate - returns false, collection unchanged
            bool duplicateAdded = numbers.Add(20);
            Console.WriteLine($"Add 20 again: {duplicateAdded}, Count: {numbers.Count}");
            // Output: False, 3

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: Removing Elements
            // ═══════════════════════════════════════════════════════════

            var fruits = new HashSet<string> { "Apple", "Banana", "Cherry", "Date" };

            // Remove returns true if element was found and removed
            bool removed = fruits.Remove("Banana");
            Console.WriteLine($"\nRemove 'Banana': {removed}");
            // Output: True
            Console.WriteLine($"Remaining: {string.Join(", ", fruits)}");
            // Output: Apple, Cherry, Date

            // Remove non-existent element returns false
            bool notFound = fruits.Remove("Grape");
            Console.WriteLine($"Remove 'Grape': {notFound}");
            // Output: False

            // Clear removes all elements
            fruits.Clear();
            Console.WriteLine($"After Clear count: {fruits.Count}");
            // Output: 0

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: UnionWith - Combine Two Sets
            // ═══════════════════════════════════════════════════════════

            var setA = new HashSet<int> { 1, 2, 3, 4 };
            var setB = new HashSet<int> { 3, 4, 5, 6 };

            // UnionWith modifies setA to contain all unique elements from both
            setA.UnionWith(setB);
            Console.WriteLine($"\nUnion of {{1,2,3,4}} and {{3,4,5,6}}:");
            Console.WriteLine(string.Join(", ", setA));
            // Output: 1, 2, 3, 4, 5, 6

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: IntersectWith - Common Elements
            // ═══════════════════════════════════════════════════════════

            var group1 = new HashSet<string> { "Dog", "Cat", "Bird", "Fish" };
            var group2 = new HashSet<string> { "Cat", "Fish", "Rabbit", "Mouse" };

            // IntersectWith keeps only elements present in both sets
            group1.IntersectWith(group2);
            Console.WriteLine($"\nIntersect of pet groups:");
            Console.WriteLine(string.Join(", ", group1));
            // Output: Cat, Fish

            // ═══════════════════════════════════════════════════════════
            // SECTION 6: Contains - Membership Test
            // ═══════════════════════════════════════════════════════════

            var cities = new HashSet<string> { "New York", "London", "Tokyo", "Paris" };

            // Contains checks if element exists in the set
            bool hasLondon = cities.Contains("London");
            bool hasBerlin = cities.Contains("Berlin");

            Console.WriteLine($"\nContains 'London': {hasLondon}");
            // Output: True
            Console.WriteLine($"Contains 'Berlin': {hasBerlin}");
            // Output: False

            // ═══════════════════════════════════════════════════════════
            // SECTION 7: Real-World Examples
            // ═══════════════════════════════════════════════════════════

            // Example 1: Tracking unique visitors
            var uniqueVisitors = new HashSet<string>();

            uniqueVisitors.Add("user1@email.com");
            uniqueVisitors.Add("user2@email.com");
            uniqueVisitors.Add("user1@email.com"); // Duplicate - ignored

            Console.WriteLine($"\n--- Unique Visitors ---");
            Console.WriteLine($"Total unique: {uniqueVisitors.Count}");
            // Output: 2
            Console.WriteLine(string.Join(", ", uniqueVisitors));

            // Example 2: Building unique tag list
            var articleTags = new HashSet<string>();
            string[] inputTags = { "C#", "Programming", "C#", "Tutorial", "Programming", ".NET" };

            foreach (var tag in inputTags)
            {
                articleTags.Add(tag);
            }

            Console.WriteLine($"\n--- Unique Tags ---");
            Console.WriteLine(string.Join(", ", articleTags));
            // Output: C#, Programming, Tutorial, .NET

            // Example 3: Checking for duplicate entries before insert
            var existingUsers = new HashSet<string> { "alice", "bob", "charlie" };
            string newUser = "dave";

            if (existingUsers.Add(newUser))
            {
                Console.WriteLine($"\n--- User Registration ---");
                Console.WriteLine($"User '{newUser}' registered successfully");
                // Output: User 'dave' registered successfully
            }
            else
            {
                Console.WriteLine($"User '{newUser}' already exists");
            }

            Console.WriteLine("\n=== HashSet Fundamentals Complete ===");
        }
    }
}
