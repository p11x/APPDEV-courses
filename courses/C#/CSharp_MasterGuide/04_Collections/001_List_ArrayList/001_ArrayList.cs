/*
 * ============================================================
 * TOPIC     : Collections
 * SUBTOPIC  : Non-Generic ArrayList
 * FILE      : ArrayList.cs
 * PURPOSE   : Demonstrates ArrayList (legacy non-generic collection)
 *            in C# - understanding its usage, boxing/unboxing,
 *            and when to prefer List<T> over ArrayList
 * ============================================================
 */

using System;
using System.Collections;

namespace CSharp_MasterGuide._04_Collections._01_List_ArrayList
{
    class ArrayListDemo
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== ArrayList - Non-Generic Collection ===\n");

            // ═══════════════════════════════════════════════════════════
            // SECTION 1: Creating ArrayList
            // ═══════════════════════════════════════════════════════════

            // Empty ArrayList (uses System.Collections namespace)
            var emptyArrayList = new ArrayList();
            Console.WriteLine($"Empty ArrayList count: {emptyArrayList.Count}");
            // Output: Empty ArrayList count: 0

            // ArrayList with initial capacity
            var sizedArrayList = new ArrayList(10);
            Console.WriteLine($"Sized ArrayList capacity: {sizedArrayList.Capacity}");
            // Output: Sized ArrayList capacity: 10

            // Initialize with collection initializer
            var initialized = new ArrayList { 1, "two", 3.0, true };
            Console.WriteLine($"Initialized count: {initialized.Count}");
            // Output: Initialized count: 4

            // Create from another collection
            var sourceList = new ArrayList { "a", "b", "c" };
            var copiedList = new ArrayList(sourceList);
            Console.WriteLine($"Copied count: {copiedList.Count}");
            // Output: Copied count: 3

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: Adding Elements (Boxing)
            // ═══════════════════════════════════════════════════════════

            var mixedList = new ArrayList();

            // Add various types - they get "boxed" to object
            mixedList.Add(42);                  // int boxed to object
            mixedList.Add("Hello World");       // string already reference type
            mixedList.Add(3.14159);             // double boxed to object
            mixedList.Add(true);                // bool boxed to object
            mixedList.Add(new Person { Name = "Alice" }); // custom object

            Console.WriteLine($"\n=== Mixed Types Demo ===");
            Console.WriteLine($"Count: {mixedList.Count}");
            // Output: Count: 5

            // Display all elements (need to cast back - unboxing)
            foreach (var item in mixedList)
            {
                Console.WriteLine($"  {item} (Type: {item.GetType().Name})");
            }
            // Output:
            //   42 (Type: Int32)
            //   Hello World (Type: String)
            //   3.14159 (Type: Double)
            //   True (Type: Boolean)
            //   Alice (Type: Person)

            // AddRange - add multiple elements at once
            var moreItems = new ArrayList { 100, 200, 300 };
            mixedList.AddRange(moreItems);
            Console.WriteLine($"After AddRange: {mixedList.Count}");
            // Output: After AddRange: 8

            // Insert at specific index
            mixedList.Insert(1, "Inserted");
            Console.WriteLine($"After Insert: {mixedList[1]}");
            // Output: After Insert: Inserted

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: Accessing Elements (Unboxing)
            // ═══════════════════════════════════════════════════════════

            var numbers = new ArrayList { 10, 20, 30, 40, 50 };

            // Access by index - returns object, must cast
            object firstItem = numbers[0];
            int firstNumber = (int)numbers[0];  // Must unbox to int
            Console.WriteLine($"\nFirst element: {firstNumber}");
            // Output: First element: 10

            // Contains - checks if element exists
            bool has20 = numbers.Contains(20);
            bool has99 = numbers.Contains(99);
            Console.WriteLine($"Contains 20: {has20}, Contains 99: {has99}");
            // Output: Contains 20: True, Contains 99: False

            // IndexOf - find index of element
            int idx20 = numbers.IndexOf(20);
            int idx99 = numbers.IndexOf(99);
            Console.WriteLine($"IndexOf 20: {idx20}, IndexOf 99: {idx99}");
            // Output: IndexOf 20: 1, IndexOf 99: -1

            // GetRange - get subset
            var subset = numbers.GetRange(1, 3);
            Console.WriteLine($"GetRange(1,3): {string.Join(", ", subset.ToArray())}");
            // Output: GetRange(1,3): 20, 30, 40

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: Removing Elements
            // ═══════════════════════════════════════════════════════════

            var fruits = new ArrayList { "Apple", "Banana", "Cherry", "Banana", "Date" };

            // Remove - removes first matching element (requires exact match)
            bool removed = fruits.Remove("Banana");
            Console.WriteLine($"\nAfter Remove('Banana'): {string.Join(", ", fruits.ToArray())}");
            Console.WriteLine($"Remove returned: {removed}");
            // Output: After Remove('Banana'): Apple, Cherry, Banana, Date
            // Output: Remove returned: True

            // RemoveAt - remove by index
            fruits.RemoveAt(0);
            Console.WriteLine($"After RemoveAt(0): {string.Join(", ", fruits.ToArray())}");
            // Output: After RemoveAt(0): Cherry, Banana, Date

            // RemoveRange - remove a range
            var moreFruits = new ArrayList { "One", "Two", "Three", "Four", "Five" };
            moreFruits.RemoveRange(1, 2); // Remove 2 elements starting at index 1
            Console.WriteLine($"After RemoveRange(1,2): {string.Join(", ", moreFruits.ToArray())}");
            // Output: After RemoveRange(1,2): One, Four, Five

            // RemoveAll - remove all matching elements (uses predicate)
            // Note: Requires .NET Framework 3.5+, in older versions use RemoveAt in loop
            var nums = new ArrayList { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            // In modern C#, we can't use RemoveAll with ArrayList directly in same way as List<T>
            // Instead, we iterate backwards or use different approach

            // Simulating RemoveAll by iterating backwards
            for (int i = nums.Count - 1; i >= 0; i--)
            {
                if ((int)nums[i] % 2 == 0)
                {
                    nums.RemoveAt(i);
                }
            }
            Console.WriteLine($"After removing evens: {string.Join(", ", nums.ToArray())}");
            // Output: After removing evens: 1, 3, 5, 7, 9

            // Clear all elements
            nums.Clear();
            Console.WriteLine($"After Clear, count: {nums.Count}");
            // Output: After Clear, count: 0

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: Sorting
            // ═══════════════════════════════════════════════════════════

            var unsorted = new ArrayList { 5, 2, 8, 1, 9, 3 };
            
            // Sort - uses default comparer
            unsorted.Sort();
            Console.WriteLine($"\nSorted: {string.Join(", ", unsorted.ToArray())}");
            // Output: Sorted: 1, 2, 3, 5, 8, 9

            // Sort with custom comparer (descending)
            unsorted.Sort(new ReverseComparer());
            Console.WriteLine($"Sorted descending: {string.Join(", ", unsorted.ToArray())}");
            // Output: Sorted descending: 9, 8, 5, 3, 2, 1

            // Sorting strings
            var strings = new ArrayList { "Zebra", "Apple", "Mango" };
            strings.Sort();
            Console.WriteLine($"Strings sorted: {string.Join(", ", strings.ToArray())}");
            // Output: Strings sorted: Apple, Mango, Zebra

            // Reverse
            strings.Reverse();
            Console.WriteLine($"Reversed: {string.Join(", ", strings.ToArray())}");
            // Output: Reversed: Zebra, Mango, Apple

            // ═══════════════════════════════════════════════════════════
            // SECTION 6: ToArray - Converting to Array
            // ═══════════════════════════════════════════════════════════

            var original = new ArrayList { "A", "B", "C" };

            // Convert to object array
            object[] objArray = (object[])original.ToArray(typeof(object));
            Console.WriteLine($"\nTo object array: {objArray[0]}");
            // Output: To object array: A

            // Convert to specific type array (requires all elements to be that type)
            var intArrayList = new ArrayList { 1, 2, 3, 4, 5 };
            int[] intArray = (int[])intArrayList.ToArray(typeof(int));
            Console.WriteLine($"To int array, first: {intArray[0]}");
            // Output: To int array, first: 1

            // Note: If ArrayList contains mixed types, this will throw exception
            var mixed = new ArrayList { 1, "two", 3 };
            // int[] will fail: int[] bad = (int[])mixed.ToArray(typeof(int)); // throws exception

            // ═══════════════════════════════════════════════════════════
            // SECTION 7: Why Prefer List<T> Over ArrayList
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine($"\n=== List<T> vs ArrayList Comparison ===");

            // ArrayList - stores as object ( boxing for value types)
            var arrayList = new ArrayList();
            arrayList.Add(42);  // Boxing: int -> object
            int val1 = (int)arrayList[0]; // Unboxing: object -> int

            // List<T> - type-safe, no boxing for T
            var genericList = new List<int>();
            genericList.Add(42); // No boxing - stored as int directly
            int val2 = genericList[0]; // No unboxing needed

            // Performance difference in tight loops
            var stopwatch = new System.Diagnostics.Stopwatch();

            // ArrayList boxing overhead
            stopwatch.Restart();
            var al = new ArrayList();
            for (int i = 0; i < 100000; i++)
            {
                al.Add(i);  // Boxing
                int x = (int)al[i]; // Unboxing
            }
            stopwatch.Stop();
            Console.WriteLine($"ArrayList time: {stopwatch.ElapsedMilliseconds}ms");
            // Output: ArrayList time: (varies, typically higher)

            // List<T> no boxing
            stopwatch.Restart();
            var gl = new List<int>();
            for (int i = 0; i < 100000; i++)
            {
                gl.Add(i); // No boxing
                int x = gl[i]; // No unboxing
            }
            stopwatch.Stop();
            Console.WriteLine($"List<T> time: {stopwatch.ElapsedMilliseconds}ms");
            // Output: List<T> time: (typically faster)

            // Type safety - ArrayList allows anything
            var anything = new ArrayList();
            anything.Add("string");
            anything.Add(123);
            anything.Add(new Button()); // Compiler doesn't catch type errors

            // List<T> enforces type at compile time
            // var stringsOnly = new List<string>();
            // stringsOnly.Add(123);  // COMPILE ERROR: cannot convert int to string
            // stringsOnly.Add(new Button()); // COMPILE ERROR

            Console.WriteLine("\nConclusion: Use List<T> instead of ArrayList for:");
            Console.WriteLine("  - Type safety at compile time");
            Console.WriteLine("  - Better performance (no boxing/unboxing)");
            Console.WriteLine("  - Cleaner code (no casting)");

            // ═══════════════════════════════════════════════════════════
            // SECTION 8: Real-World Example - Legacy System Integration
            // ═══════════════════════════════════════════════════════════

            // Sometimes you need ArrayList when working with legacy code
            // that expects ArrayList or non-generic collections

            // Simulating reading from legacy COM component
            var legacyData = GetLegacyData(); // Returns ArrayList

            Console.WriteLine($"\n=== Legacy System Integration ===");
            Console.WriteLine($"Received {legacyData.Count} items from legacy system");

            // Process the mixed-type data
            foreach (var item in legacyData)
            {
                if (item is int intVal)
                    Console.WriteLine($"  Integer: {intVal}");
                else if (item is string strVal)
                    Console.WriteLine($"  String: {strVal}");
                else if (item is double dblVal)
                    Console.WriteLine($"  Double: {dblVal}");
                else
                    Console.WriteLine($"  Unknown: {item}");
            }
            // Output: (varies based on GetLegacyData)

            // Convert to strongly-typed list for modern processing
            var typedList = new List<LegacyDataItem>();
            foreach (var item in legacyData)
            {
                typedList.Add(ConvertToTyped(item));
            }
            Console.WriteLine($"Converted to typed list: {typedList.Count} items");

            Console.WriteLine("\n=== ArrayList Demo Complete ===");
        }

        // Helper method simulating legacy data source
        static ArrayList GetLegacyData()
        {
            var data = new ArrayList();
            data.Add("Customer Name");
            data.Add(1001);
            data.Add(1500.50);
            data.Add("Order #12345");
            return data;
        }

        // Helper to convert mixed types to typed object
        static LegacyDataItem ConvertToTyped(object item)
        {
            if (item is int)
                return new LegacyDataItem { Type = "int", Value = item.ToString() };
            if (item is double)
                return new LegacyDataItem { Type = "double", Value = item.ToString() };
            return new LegacyDataItem { Type = "string", Value = item?.ToString() };
        }
    }

    class ReverseComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            // Reverse the comparison result
            return ((IComparable)y).CompareTo(x);
        }
    }

    class Person
    {
        public string Name { get; set; }
        public override string ToString() => Name;
    }

    class Button { } // Placeholder for UI control example

    class LegacyDataItem
    {
        public string Type { get; set; }
        public string Value { get; set; }
    }
}