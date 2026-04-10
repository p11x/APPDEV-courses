/*
 * ============================================================
 * TOPIC     : Collections
 * SUBTOPIC  : LinkedList<T> - Doubly Linked List Basics
 * FILE      : LinkedListBasics.cs
 * PURPOSE   : Teaches LinkedList<T> fundamentals - adding,
 *             removing, and basic node operations
 * ============================================================
 */

using System;
using System.Collections.Generic;

namespace CSharp_MasterGuide._04_Collections._05_LinkedList
{
    class LinkedListBasics
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== LinkedList<T> Fundamentals ===\n");

            // ═══════════════════════════════════════════════════════════
            // SECTION 1: Creating LinkedList<T>
            // ═══════════════════════════════════════════════════════════

            // Empty LinkedList - no elements initially
            var emptyList = new LinkedList<int>();
            Console.WriteLine($"Empty LinkedList count: {emptyList.Count}");
            // Output: 0

            // LinkedList with collection initializer syntax
            var colors = new LinkedList<string> { "Red", "Green", "Blue" };
            Console.WriteLine($"Colors count: {colors.Count}");
            // Output: 3

            // Display elements
            Console.WriteLine($"Colors: {string.Join(", ", colors)}");
            // Output: Red, Green, Blue

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: AddFirst - Add to Beginning
            // ═══════════════════════════════════════════════════════════

            var numbers = new LinkedList<int>();

            // AddFirst - adds at the beginning of the list
            numbers.AddFirst(10);
            numbers.AddFirst(5);
            numbers.AddFirst(1);

            Console.WriteLine($"\nAfter AddFirst(10), AddFirst(5), AddFirst(1):");
            Console.WriteLine(string.Join(", ", numbers));
            // Output: 1, 5, 10

            // AddFirst with a LinkedListNode
            var node = new LinkedListNode<int>(0);
            numbers.AddFirst(node);

            Console.WriteLine($"After AddFirst(node with 0):");
            Console.WriteLine(string.Join(", ", numbers));
            // Output: 0, 1, 5, 10

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: AddLast - Add to End
            // ═══════════════════════════════════════════════════════════

            var fruits = new LinkedList<string>();

            // AddLast - adds at the end of the list
            fruits.AddLast("Apple");
            fruits.AddLast("Banana");
            fruits.AddLast("Cherry");

            Console.WriteLine($"\nAfter AddLast:");
            Console.WriteLine(string.Join(", ", fruits));
            // Output: Apple, Banana, Cherry

            // AddLast with node reference
            var lastNode = new LinkedListNode<string>("Date");
            fruits.AddLast(lastNode);

            Console.WriteLine($"After AddLast(Date):");
            Console.WriteLine(string.Join(", ", fruits));
            // Output: Apple, Banana, Cherry, Date

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: AddAfter - Add After a Specific Node
            // ═══════════════════════════════════════════════════════════

            var weekdays = new LinkedList<string>();

            weekdays.AddLast("Monday");
            weekdays.AddLast("Wednesday");
            weekdays.AddLast("Friday");

            // Find the node to insert after
            var mondayNode = weekdays.Find("Monday");

            // AddAfter - inserts new node after the specified node
            weekdays.AddAfter(mondayNode, "Tuesday");

            Console.WriteLine($"\nAfter AddAfter Monday with Tuesday:");
            Console.WriteLine(string.Join(", ", weekdays));
            // Output: Monday, Tuesday, Wednesday, Friday

            // AddAfter with another existing node
            var wedNode = weekdays.Find("Wednesday");
            weekdays.AddAfter(wedNode, "Thursday");

            Console.WriteLine($"After AddAfter Wednesday with Thursday:");
            Console.WriteLine(string.Join(", ", weekdays));
            // Output: Monday, Tuesday, Wednesday, Thursday, Friday

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: AddBefore - Add Before a Specific Node
            // ═══════════════════════════════════════════════════════════

            var list = new LinkedList<int>();

            list.AddLast(10);
            list.AddLast(30);
            list.AddLast(50);

            var node30 = list.Find(30);

            // AddBefore - inserts new node before the specified node
            list.AddBefore(node30, 20);

            Console.WriteLine($"\nAfter AddBefore 30 with 20:");
            Console.WriteLine(string.Join(", ", list));
            // Output: 10, 20, 30, 50

            // AddBefore with first node (at beginning)
            var node10 = list.Find(10);
            list.AddBefore(node10, 5);

            Console.WriteLine($"After AddBefore 10 with 5:");
            Console.WriteLine(string.Join(", ", list));
            // Output: 5, 10, 20, 30, 50

            // ═══════════════════════════════════════════════════════════
            // SECTION 6: Remove - Delete Nodes
            // ═══════════════════════════════════════════════════════════

            var animals = new LinkedList<string> { "Dog", "Cat", "Bird", "Fish", "Rabbit" };

            // RemoveFirst - removes the first node
            animals.RemoveFirst();

            Console.WriteLine($"\nAfter RemoveFirst:");
            Console.WriteLine(string.Join(", ", animals));
            // Output: Cat, Bird, Fish, Rabbit

            // RemoveLast - removes the last node
            animals.RemoveLast();

            Console.WriteLine($"After RemoveLast:");
            Console.WriteLine(string.Join(", ", animals));
            // Output: Cat, Bird, Fish

            // Remove - removes first occurrence of value
            bool removed = animals.Remove("Bird");

            Console.WriteLine($"Remove 'Bird': {removed}");
            // Output: True
            Console.WriteLine($"After Remove 'Bird':");
            Console.WriteLine(string.Join(", ", animals));
            // Output: Cat, Fish

            // Remove with node reference
            var fishNode = animals.Find("Fish");
            animals.Remove(fishNode);

            Console.WriteLine($"After Remove(Fish node):");
            Console.WriteLine(string.Join(", ", animals));
            // Output: Cat

            // Remove non-existent returns false
            bool notRemoved = animals.Remove("Bear");
            Console.WriteLine($"Remove 'Bear': {notRemoved}");
            // Output: False

            // ═══════════════════════════════════════════════════════════
            // SECTION 7: Clear - Remove All Elements
            // ═══════════════════════════════════════════════════════════

            var primes = new LinkedList<int> { 2, 3, 5, 7, 11 };

            Console.WriteLine($"\nBefore Clear count: {primes.Count}");
            // Output: 5

            primes.Clear();

            Console.WriteLine($"After Clear count: {primes.Count}");
            // Output: 0

            // ═══════════════════════════════════════════════════════════
            // SECTION 8: Contains - Check for Existence
            // ═══════════════════════════════════════════════════════════

            var letters = new LinkedList<char> { 'A', 'B', 'C', 'D', 'E' };

            bool hasC = letters.Contains('C');
            bool hasZ = letters.Contains('Z');

            Console.WriteLine($"\nContains 'C': {hasC}");
            // Output: True
            Console.WriteLine($"Contains 'Z': {hasZ}");
            // Output: False

            // ═══════════════════════════════════════════════════════════
            // SECTION 9: Real-World Examples
            // ═══════════════════════════════════════════════════════════

            // Example 1: Task list - adding and managing tasks
            var taskList = new LinkedList<string>();

            taskList.AddLast("Review code");
            taskList.AddLast("Write tests");
            var codeReviewNode = taskList.Find("Review code");
            taskList.AddAfter(codeReviewNode, "Run tests");

            Console.WriteLine($"\n--- Task List ---");
            Console.WriteLine($"Tasks: {string.Join(" -> ", taskList)}");
            // Output: Review code -> Run tests -> Write tests

            // Mark first task complete
            taskList.RemoveFirst();
            Console.WriteLine($"After completing first task: {string.Join(" -> ", taskList)}");

            // Example 2: Building a list in order
            var sortedNumbers = new LinkedList<int>();

            void AddInOrder(int value)
            {
                if (sortedNumbers.Count == 0 || value <= sortedNumbers.First.Value)
                {
                    sortedNumbers.AddFirst(value);
                }
                else if (value >= sortedNumbers.Last.Value)
                {
                    sortedNumbers.AddLast(value);
                }
                else
                {
                    var current = sortedNumbers.First;
                    while (current != null && current.Value < value)
                    {
                        current = current.Next;
                    }
                    if (current != null)
                    {
                        sortedNumbers.AddBefore(current, value);
                    }
                }
            }

            AddInOrder(50);
            AddInOrder(30);
            AddInOrder(40);
            AddInOrder(10);
            AddInOrder(60);

            Console.WriteLine($"\n--- Sorted Insert ---");
            Console.WriteLine($"Numbers: {string.Join(", ", sortedNumbers)}");
            // Output: 10, 30, 40, 50, 60

            // Example 3: Navigation breadcrumbs
            var breadcrumbs = new LinkedList<string>();
            breadcrumbs.AddLast("Home");
            breadcrumbs.AddLast("Products");
            breadcrumbs.AddLast("Electronics");
            breadcrumbs.AddLast("Phones");

            Console.WriteLine($"\n--- Breadcrumb Navigation ---");
            Console.WriteLine($"Path: {string.Join(" > ", breadcrumbs)}");
            // Output: Home > Products > Electronics > Phones
            Console.WriteLine($"Current page: {breadcrumbs.Last.Value}");
            // Output: Phones
            Console.WriteLine($"Can go back: {breadcrumbs.Count > 1}");
            // Output: True

            // User goes back
            breadcrumbs.RemoveLast();
            Console.WriteLine($"After going back: {breadcrumbs.Last.Value}");
            // Output: Electronics

            Console.WriteLine("\n=== LinkedList Basics Complete ===");
        }
    }
}