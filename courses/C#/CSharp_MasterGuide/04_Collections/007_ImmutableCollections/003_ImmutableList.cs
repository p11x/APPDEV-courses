/*
 * TOPIC: Immutable Collections
 * SUBTOPIC: ImmutableList<T> - Creating, Adding, Removing
 * FILE: ImmutableList.cs
 * PURPOSE: Demonstrate ImmutableList<T> creation, modification operations
 *          (which return new instances), and common patterns
 */
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace CSharp_MasterGuide._04_Collections._07_ImmutableCollections
{
    public class ImmutableListDemo
    {
        public static void Main()
        {
            Console.WriteLine("=== ImmutableList<T> Basics ===\n");

            CreatingImmutableList();
            AddingElements();
            RemovingElements();
            SearchingAndQuerying();
            TransformationOperations();
            RealWorldExample();
        }

        static void CreatingImmutableList()
        {
            Console.WriteLine("--- 1. Creating ImmutableList<T> ---");
            Console.WriteLine();

            // Create empty ImmutableList using builder
            var emptyList = ImmutableList<int>.Empty;
            Console.WriteLine($"  Empty list count: {emptyList.Count}");
            // Output: Empty list count: 0

            // Create with initial values using ImmutableList.Create
            var numbers = ImmutableList.Create(1, 2, 3, 4, 5);
            Console.WriteLine($"  Created list: {string.Join(", ", numbers)}");
            // Output: Created list: 1, 2, 3, 4, 5

            // Create from existing collection
            var existing = new List<string> { "Apple", "Banana", "Cherry" };
            var immutableFromList = ImmutableList.CreateRange(existing);
            Console.WriteLine($"  From List<T>: {string.Join(", ", immutableFromList)}");
            // Output: From List<T>: Apple, Banana, Cherry

            // Using ImmutableList<T>.Empty with Add chain
            var builder = ImmutableList<string>.Empty.ToBuilder();
            builder.Add("First");
            builder.Add("Second");
            var builtList = builder.ToImmutable();
            Console.WriteLine($"  Built list: {string.Join(", ", builtList)}");
            // Output: Built list: First, Second
            Console.WriteLine();
        }

        static void AddingElements()
        {
            Console.WriteLine("--- 2. Adding Elements (Returns New Instance) ---");
            Console.WriteLine();

            var original = ImmutableList.Create("A", "B", "C");
            Console.WriteLine($"  Original: {string.Join(", ", original)}");
            // Output: Original: A, B, C

            // Add returns a NEW list - original unchanged
            var withD = original.Add("D");
            Console.WriteLine($"  After Add('D'): {string.Join(", ", withD)}");
            // Output: After Add('D'): A, B, C, D

            Console.WriteLine($"  Original unchanged: {string.Join(", ", original)}");
            // Output: Original unchanged: A, B, C

            // Insert at specific index
            var withX = original.Insert(1, "X");
            Console.WriteLine($"  After Insert(1, 'X'): {string.Join(", ", withX)}");
            // Output: After Insert(1, 'X'): A, X, B, C

            // AddRange - add multiple items
            var additional = new[] { "D", "E", "F" };
            var extended = original.AddRange(additional);
            Console.WriteLine($"  After AddRange: {string.Join(", ", extended)}");
            // Output: After AddRange: A, B, C, D, E, F

            // Chain multiple adds
            var chained = original.Add("D").Add("E").Add("F");
            Console.WriteLine($"  Chained adds: {string.Join(", ", chained)}");
            // Output: Chained adds: A, B, C, D, E, F
            Console.WriteLine();
        }

        static void RemovingElements()
        {
            Console.WriteLine("--- 3. Removing Elements (Returns New Instance) ---");
            Console.WriteLine();

            var fruits = ImmutableList.Create("Apple", "Banana", "Cherry", "Date", "Apple");
            Console.WriteLine($"  Original: {string.Join(", ", fruits)}");
            // Output: Original: Apple, Banana, Cherry, Date, Apple

            // Remove first occurrence - returns new list
            var afterRemove = fruits.Remove("Apple");
            Console.WriteLine($"  After Remove('Apple'): {string.Join(", ", afterRemove)}");
            // Output: After Remove('Apple'): Banana, Cherry, Date, Apple

            // RemoveAll - remove all matching elements
            var noApples = fruits.RemoveAll(f => f == "Apple");
            Console.WriteLine($"  After RemoveAll (no Apples): {string.Join(", ", noApples)}");
            // Output: After RemoveAll (no Apples): Banana, Cherry, Date

            // RemoveAt - remove by index
            var afterIndexRemove = fruits.RemoveAt(2);
            Console.WriteLine($"  After RemoveAt(2): {string.Join(", ", afterIndexRemove)}");
            // Output: After RemoveAt(2): Apple, Banana, Date, Apple

            // RemoveRange - remove by index and count
            var afterRangeRemove = fruits.RemoveRange(1, 2);
            Console.WriteLine($"  After RemoveRange(1, 2): {string.Join(", ", afterRangeRemove)}");
            // Output: After RemoveRange(1, 2): Apple, Date, Apple

            // Clear - remove all elements
            var cleared = fruits.Clear();
            Console.WriteLine($"  After Clear: Count = {cleared.Count}");
            // Output: After Clear: Count = 0
            Console.WriteLine();
        }

        static void SearchingAndQuerying()
        {
            Console.WriteLine("--- 4. Searching and Querying ---");
            Console.WriteLine();

            var colors = ImmutableList.Create("Red", "Green", "Blue", "Yellow", "Green");
            Console.WriteLine($"  Colors: {string.Join(", ", colors)}");
            // Output: Colors: Red, Green, Blue, Yellow, Green

            // Contains
            bool hasBlue = colors.Contains("Blue");
            Console.WriteLine($"  Contains('Blue'): {hasBlue}");
            // Output: Contains('Blue'): True

            // IndexOf
            int index = colors.IndexOf("Green");
            Console.WriteLine($"  IndexOf('Green'): {index}");
            // Output: IndexOf('Green'): 1

            // LastIndexOf
            int lastIndex = colors.LastIndexOf("Green");
            Console.WriteLine($"  LastIndexOf('Green'): {lastIndex}");
            // Output: LastIndexOf('Green'): 4

            // Find - returns first match
            var found = colors.Find(c => c.Length > 4);
            Console.WriteLine($"  Find (length > 4): {found}");
            // Output: Find (length > 4): Green

            // FindIndex
            int foundIndex = colors.FindIndex(c => c.StartsWith("Y"));
            Console.WriteLine($"  FindIndex (starts with Y): {foundIndex}");
            // Output: FindIndex (starts with Y): 3

            // FindLast
            var lastGreen = colors.FindLast(c => c == "Green");
            Console.WriteLine($"  FindLast ('Green'): {lastGreen}");
            // Output: FindLast ('Green'): Green

            // FindAll - returns new ImmutableList with matches
            var longNames = colors.FindAll(c => c.Length > 4);
            Console.WriteLine($"  FindAll (length > 4): {string.Join(", ", longNames)}");
            // Output: FindAll (length > 4): Green, Yellow, Green
            Console.WriteLine();
        }

        static void TransformationOperations()
        {
            Console.WriteLine("--- 5. Transformation Operations ---");
            Console.WriteLine();

            var numbers = ImmutableList.Create(1, 2, 3, 4, 5);
            Console.WriteLine($"  Original: {string.Join(", ", numbers)}");
            // Output: Original: 1, 2, 3, 4, 5

            // SetItem - replace element at index (returns new list)
            var replaced = numbers.SetItem(2, 99);
            Console.WriteLine($"  After SetItem(2, 99): {string.Join(", ", replaced)}");
            // Output: After SetItem(2, 99): 1, 2, 99, 4, 5

            // Sort - returns new sorted list
            var unsorted = ImmutableList.Create(5, 2, 8, 1, 9);
            var sorted = unsorted.Sort();
            Console.WriteLine($"  Sorted: {string.Join(", ", sorted)}");
            // Output: Sorted: 1, 2, 5, 8, 9

            // Reverse - returns new reversed list
            var reversed = unsorted.Reverse();
            Console.WriteLine($"  Reversed: {string.Join(", ", reversed)}");
            // Output: Reversed: 9, 1, 8, 2, 5

            // With - replace entire list (useful for builders)
            var names = ImmutableList.Create("Alice", "Bob");
            var updatedNames = names.With(new[] { "Charlie", "Diana", "Eve" });
            Console.WriteLine($"  With new items: {string.Join(", ", updatedNames)}");
            // Output: With new items: Charlie, Diana, Eve

            // GetRange - extract portion as new list
            var range = numbers.GetRange(1, 3);
            Console.WriteLine($"  GetRange(1, 3): {string.Join(", ", range)}");
            // Output: GetRange(1, 3): 2, 3, 4
            Console.WriteLine();
        }

        static void RealWorldExample()
        {
            Console.WriteLine("--- Real-World: Undo/Redo with Immutable List ---");
            Console.WriteLine();

            // State manager for undo functionality
            var history = ImmutableList<string>.Empty;

            // Add actions to history
            var action1 = history.Add("Open File");
            Console.WriteLine($"  Action: Open File");
            Console.WriteLine($"  History: {string.Join(" -> ", action1)}");
            // Output: History: Open File

            var action2 = action1.Add("Edit Document");
            Console.WriteLine($"  Action: Edit Document");
            Console.WriteLine($"  History: {string.Join(" -> ", action2)}");
            // Output: History: Open File -> Edit Document

            var action3 = action2.Add("Save File");
            Console.WriteLine($"  Action: Save File");
            Console.WriteLine($"  History: {string.Join(" -> ", action3)}");
            // Output: History: Open File -> Edit Document -> Save File

            // Undo - remove last action (get previous state)
            var afterUndo = action3.Remove(action3.Last());
            Console.WriteLine($"  Undo: Removed 'Save File'");
            Console.WriteLine($"  History: {string.Join(" -> ", afterUndo)}");
            // Output: History: Open File -> Edit Document

            // Can still access old states - immutable!
            Console.WriteLine($"  Previous state still accessible: {string.Join(" -> ", action3)}");
            // Output: Previous state still accessible: Open File -> Edit Document -> Save File
            Console.WriteLine();
        }
    }
}