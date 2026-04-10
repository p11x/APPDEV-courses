/*
 * ============================================================
 * TOPIC     : Collections
 * SUBTOPIC  : HashSet<T> - Advanced Operations
 * FILE      : HashSet_Part2.cs
 * PURPOSE   : Teaches advanced HashSet operations - subset/superset
 *             tests, symmetric difference, and set comparisons
 * ============================================================
 */

using System;
using System.Collections.Generic;

namespace CSharp_MasterGuide._04_Collections._04_HashSet_SortedSet
{
    class HashSet_Part2
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== HashSet<T> Advanced Operations ===\n");

            // ═══════════════════════════════════════════════════════════
            // SECTION 1: IsSubsetOf - Check if All Elements in Another Set
            // ═══════════════════════════════════════════════════════════

            var subset = new HashSet<int> { 1, 2, 3 };
            var superset = new HashSet<int> { 1, 2, 3, 4, 5 };

            bool isSubset = subset.IsSubsetOf(superset);
            Console.WriteLine($"Is {{1,2,3}} subset of {{1,2,3,4,5}}? {isSubset}");
            // Output: True

            // Empty set is always a subset
            var emptySet = new HashSet<int>();
            bool emptyIsSubset = emptySet.IsSubsetOf(superset);
            Console.WriteLine($"Empty set is subset: {emptyIsSubset}");
            // Output: True

            // Non-subset example
            var notSubset = new HashSet<int> { 1, 2, 6 };
            bool isNotSubset = notSubset.IsSubsetOf(superset);
            Console.WriteLine($"Is {{1,2,6}} subset? {isNotSubset}");
            // Output: False

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: IsSupersetOf - Check if Contains All Elements
            // ═══════════════════════════════════════════════════════════

            var smallSet = new HashSet<int> { 1, 2 };
            var largeSet = new HashSet<int> { 1, 2, 3, 4, 5 };

            bool isSuperset = largeSet.IsSupersetOf(smallSet);
            Console.WriteLine($"\nIs {{1,2,3,4,5}} superset of {{1,2}}? {isSuperset}");
            // Output: True

            bool reverseSuperset = smallSet.IsSupersetOf(largeSet);
            Console.WriteLine($"Is {{1,2}} superset of {{1,2,3,4,5}}? {reverseSuperset}");
            // Output: False

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: Proper Subset/Superset (Strict)
            // ═══════════════════════════════════════════════════════════

            var properSubset = new HashSet<int> { 1, 2, 3 };
            var properSuperset = new HashSet<int> { 1, 2, 3, 4, 5 };

            // IsProperSubsetOf - true only if subset AND sets are not equal
            bool properSub = properSubset.IsProperSubsetOf(properSuperset);
            Console.WriteLine($"\nIs proper subset: {properSub}");
            // Output: True

            var equalSets = new HashSet<int> { 1, 2, 3, 4, 5 };
            bool equalSubset = properSubset.IsProperSubsetOf(equalSets);
            Console.WriteLine($"Equal sets - proper subset: {equalSubset}");
            // Output: False

            // IsProperSupersetOf
            bool properSuper = properSuperset.IsProperSupersetOf(properSubset);
            Console.WriteLine($"Is proper superset: {properSuper}");
            // Output: True

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: Overlaps - Check for Common Elements
            // ═══════════════════════════════════════════════════════════

            var setAlpha = new HashSet<int> { 1, 2, 3 };
            var setBeta = new HashSet<int> { 3, 4, 5 };
            var setGamma = new HashSet<int> { 6, 7, 8 };

            bool hasOverlap = setAlpha.Overlaps(setBeta);
            bool noOverlap = setAlpha.Overlaps(setGamma);

            Console.WriteLine($"\n{{1,2,3}} overlaps {{3,4,5}}? {hasOverlap}");
            // Output: True
            Console.WriteLine($"{{1,2,3}} overlaps {{6,7,8}}? {noOverlap}");
            // Output: False

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: SetEquals - Check for Identical Elements
            // ═══════════════════════════════════════════════════════════

            var set1 = new HashSet<int> { 1, 2, 3 };
            var set2 = new HashSet<int> { 3, 2, 1 }; // Same elements, different order
            var set3 = new HashSet<int> { 1, 2, 4 };

            bool equal1 = set1.SetEquals(set2);
            bool equal2 = set1.SetEquals(set3);

            Console.WriteLine($"\n{{1,2,3}} equals {{3,2,1}}? {equal1}");
            // Output: True (order doesn't matter)
            Console.WriteLine($"{{1,2,3}} equals {{1,2,4}}? {equal2}");
            // Output: False

            // ═══════════════════════════════════════════════════════════
            // SECTION 6: ExceptWith - Remove Elements Present in Another Set
            // ═══════════════════════════════════════════════════════════

            var numbers = new HashSet<int> { 1, 2, 3, 4, 5 };
            var toRemove = new HashSet<int> { 3, 4, 6 };

            // Removes all elements in toRemove from numbers
            numbers.ExceptWith(toRemove);
            Console.WriteLine($"\nExceptWith - Remove {{3,4,6}} from {{1,2,3,4,5}}:");
            Console.WriteLine(string.Join(", ", numbers));
            // Output: 1, 2, 5 (3 and 4 removed, 6 wasn't there)

            // ═══════════════════════════════════════════════════════════
            // SECTION 7: SymmetricExceptWith - Elements in One Set Only
            // ═══════════════════════════════════════════════════════════

            var leftSet = new HashSet<int> { 1, 2, 3, 4 };
            var rightSet = new HashSet<int> { 3, 4, 5, 6 };

            // Keeps elements that are in either set but not in both
            leftSet.SymmetricExceptWith(rightSet);
            Console.WriteLine($"\nSymmetricExceptWith on {{1,2,3,4}} and {{3,4,5,6}}:");
            Console.WriteLine(string.Join(", ", leftSet));
            // Output: 1, 2, 5, 6 (3 and 4 removed as they're in both)

            // ═══════════════════════════════════════════════════════════
            // SECTION 8: CopyTo - Copy to Array
            // ═══════════════════════════════════════════════════════════

            var collection = new HashSet<string> { "Alpha", "Beta", "Gamma" };
            string[] array = new string[collection.Count];

            collection.CopyTo(array);
            Console.WriteLine($"\nCopied to array:");
            Console.WriteLine(string.Join(", ", array));
            // Output: Alpha, Beta, Gamma

            // Copy to array with specific index
            string[] largerArray = new string[5];
            collection.CopyTo(largerArray, 1); // Start at index 1
            Console.WriteLine("Copied to array starting at index 1:");
            Console.WriteLine(string.Join(", ", largerArray));
            // Output: , Alpha, Beta, Gamma (first element empty)

            // ═══════════════════════════════════════════════════════════
            // SECTION 9: Real-World Examples
            // ═══════════════════════════════════════════════════════════

            // Example 1: User permissions - check if all required permissions granted
            var userPermissions = new HashSet<string> { "read", "write", "delete" };
            var requiredPermissions = new HashSet<string> { "read", "write", "execute" };

            bool hasAccess = userPermissions.IsSupersetOf(requiredPermissions);
            Console.WriteLine($"\n--- Permission Check ---");
            Console.WriteLine($"User has all required permissions: {hasAccess}");
            // Output: False (missing "execute")

            // Example 2: Find unique items between two lists
            var listA = new List<string> { "Apple", "Banana", "Cherry" };
            var listB = new List<string> { "Banana", "Cherry", "Date" };

            var uniqueToA = new HashSet<string>(listA);
            var uniqueToB = new HashSet<string>(listB);

            uniqueToA.ExceptWith(listB);
            uniqueToB.ExceptWith(listA);

            Console.WriteLine($"\n--- Unique Items Comparison ---");
            Console.WriteLine($"Only in List A: {string.Join(", ", uniqueToA)}");
            // Output: Apple
            Console.WriteLine($"Only in List B: {string.Join(", ", uniqueToB)}");
            // Output: Date

            // Example 3: Tag filtering - remove excluded tags
            var allowedTags = new HashSet<string> { "tech", "programming", "csharp" };
            var excludedTags = new HashSet<string> { "programming", "deprecated" };

            allowedTags.ExceptWith(excludedTags);
            Console.WriteLine($"\n--- Filtered Tags ---");
            Console.WriteLine($"Allowed tags: {string.Join(", ", allowedTags)}");
            // Output: tech, csharp

            Console.WriteLine("\n=== HashSet Advanced Operations Complete ===");
        }
    }
}
