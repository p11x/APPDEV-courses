/*
 * ============================================================
 * TOPIC     : Collections
 * SUBTOPIC  : LinkedList<T> - Advanced Operations
 * FILE      : LinkedListBasics_Part2.cs
 * PURPOSE   : Teaches advanced LinkedList operations - Find, FindLast,
 *             traversal, node properties, and custom implementations
 * ============================================================
 */

using System;
using System.Collections.Generic;

namespace CSharp_MasterGuide._04_Collections._05_LinkedList
{
    class LinkedListBasics_Part2
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== LinkedList<T> Advanced Operations ===\n");

            // ═══════════════════════════════════════════════════════════
            // SECTION 1: Find - Locate a Node
            // ═══════════════════════════════════════════════════════════

            var numbers = new LinkedList<int> { 10, 20, 30, 40, 50 };

            // Find - returns first node containing the value
            var node30 = numbers.Find(30);

            Console.WriteLine($"Found node: {node30?.Value}");
            // Output: 30
            Console.WriteLine($"Is node null: {node30 == null}");
            // Output: False

            // Find non-existent value
            var node99 = numbers.Find(99);
            Console.WriteLine($"Find 99 result: {node99?.Value}");
            // Output: (nothing/null)

            if (node30 != null)
            {
                Console.WriteLine($"Found! Value is {node30.Value}");
            }

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: FindLast - Find Last Occurrence
            // ═══════════════════════════════════════════════════════════

            var items = new LinkedList<string> { "A", "B", "C", "B", "D" };

            // Find returns first occurrence
            var firstB = items.Find("B");
            Console.WriteLine($"\nFind 'B': {firstB?.Value}");
            // Output: B (first one)

            // FindLast returns last occurrence
            var lastB = items.FindLast("B");
            Console.WriteLine($"FindLast 'B': {lastB?.Value}");
            // Output: B (second one)

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: Node Properties - Next and Previous
            // ═══════════════════════════════════════════════════════════

            var letters = new LinkedList<string> { "A", "B", "C", "D" };

            // Get first node
            var firstNode = letters.First;
            var lastNode = letters.Last;

            Console.WriteLine($"\nFirst: {firstNode?.Value}");
            // Output: A
            Console.WriteLine($"Last: {lastNode?.Value}");
            // Output: D

            // Traverse using Next
            Console.WriteLine("Traverse forward using Next:");
            var current = letters.First;
            while (current != null)
            {
                Console.WriteLine($"  {current.Value}");
                current = current.Next;
            }
            // Output: A, B, C, D

            // Traverse using Previous (backward)
            Console.WriteLine("Traverse backward using Previous:");
            current = letters.Last;
            while (current != null)
            {
                Console.WriteLine($"  {current.Value}");
                current = current.Previous;
            }
            // Output: D, C, B, A

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: LinkedListNode Properties
            // ═══════════════════════════════════════════════════════════

            var colors = new LinkedList<string> { "Red", "Green", "Blue" };

            var greenNode = colors.Find("Green");

            // Value property
            Console.WriteLine($"\nGreen node value: {greenNode.Value}");
            // Output: Green

            // List property - reference to parent list
            Console.WriteLine($"Green node's list count: {greenNode.List?.Count}");
            // Output: 3

            // Next property
            Console.WriteLine($"Green.Next value: {greenNode.Next?.Value}");
            // Output: Blue

            // Previous property
            Console.WriteLine($"Green.Previous value: {greenNode.Previous?.Value}");
            // Output: Red

            // IsSingleNode and IsFirst/IsLast
            Console.WriteLine($"IsGreen First: {greenNode == colors.First}");
            // Output: False
            Console.WriteLine($"IsGreen Last: {greenNode == colors.Last}");
            // Output: False

            // First and Last node checks
            Console.WriteLine($"Red is first: {colors.First.Value == "Red"}");
            // Output: True
            Console.WriteLine($"Blue is last: {colors.Last.Value == "Blue"}");
            // Output: True

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: Manual Traversal Techniques
            // ═══════════════════════════════════════════════════════════

            var digits = new LinkedList<int> { 1, 2, 3, 4, 5 };

            // Using foreach - most common approach
            Console.WriteLine($"\nUsing foreach:");
            foreach (var digit in digits)
            {
                Console.WriteLine($"  {digit}");
            }
            // Output: 1, 2, 3, 4, 5

            // Manual forward traversal
            Console.WriteLine("Manual forward traversal:");
            var node = digits.First;
            int index = 0;
            while (node != null)
            {
                Console.WriteLine($"  [{index}] = {node.Value}");
                node = node.Next;
                index++;
            }
            // Output: [0]=1, [1]=2, [2]=3, [3]=4, [4]=5

            // Find specific position
            node = digits.First;
            int targetPosition = 2;
            for (int i = 0; i < targetPosition && node != null; i++)
            {
                node = node.Next;
            }
            Console.WriteLine($"Value at position {targetPosition}: {node?.Value}");
            // Output: 3

            // ═══════════════════════════════════════════════════════════
            // SECTION 6: Insertion and Deletion at Positions
            // ═══════════════════════════════════════════════════════════

            var list = new LinkedList<string>();

            // Add initial elements
            list.AddLast("Item1");
            list.AddLast("Item2");
            list.AddLast("Item5");

            // Insert at beginning
            list.AddFirst("Item0");

            // Insert at end
            list.AddLast("Item6");

            // Insert in middle using Find and AddBefore/AddAfter
            var item2 = list.Find("Item2");
            list.AddAfter(item2, "Item3");
            list.AddAfter(item2.Next, "Item4"); // Item3

            Console.WriteLine($"\nAfter insertions:");
            Console.WriteLine(string.Join(", ", list));
            // Output: Item0, Item1, Item2, Item3, Item4, Item5, Item6

            // Delete specific node
            var itemToDelete = list.Find("Item3");
            list.Remove(itemToDelete);

            Console.WriteLine($"After removing Item3:");
            Console.WriteLine(string.Join(", ", list));
            // Output: Item0, Item1, Item2, Item4, Item5, Item6

            // Delete all even-indexed items (0-based)
            node = list.First;
            bool first = true;
            while (node != null)
            {
                var nextNode = node.Next;
                if (!first)
                {
                    list.Remove(node);
                }
                first = false;
                node = nextNode;
            }

            Console.WriteLine($"After removing odd positions:");
            Console.WriteLine(string.Join(", ", list));
            // Output: Item0, Item2, Item4, Item6

            // ═══════════════════════════════════════════════════════════
            // SECTION 7: Count and Accessing Properties
            // ═══════════════════════════════════════════════════════════

            var sampleList = new LinkedList<int> { 100, 200, 300 };

            Console.WriteLine($"\nList properties:");
            Console.WriteLine($"Count: {sampleList.Count}");
            // Output: 3
            Console.WriteLine($"First: {sampleList.First?.Value}");
            // Output: 100
            Console.WriteLine($"Last: {sampleList.Last?.Value}");
            // Output: 300

            // Check if empty
            Console.WriteLine($"IsEmpty: {sampleList.Count == 0}");
            // Output: False

            // ═══════════════════════════════════════════════════════════
            // SECTION 8: Real-World Examples
            // ═══════════════════════════════════════════════════════════

            // Example 1: Priority queue with immediate front insertion
            var priorityQueue = new LinkedList<string>();

            void Enqueue(string task)
            {
                if (priorityQueue.Count == 0 || string.Compare(task, priorityQueue.First.Value) < 0)
                {
                    priorityQueue.AddFirst(task);
                }
                else
                {
                    var current = priorityQueue.First;
                    while (current.Next != null && 
                           string.Compare(task, current.Next.Value) > 0)
                    {
                        current = current.Next;
                    }
                    priorityQueue.AddAfter(current, task);
                }
            }

            Enqueue("Update documentation");
            Enqueue("Fix critical bug");
            Enqueue("Code review");
            Enqueue("Deploy to production");

            Console.WriteLine($"\n--- Priority Queue ---");
            foreach (var task in priorityQueue)
            {
                Console.WriteLine($"  {task}");
            }
            // Output: Fix critical bug, Deploy to production, Code review, Update documentation

            // Example 2: Circular buffer traversal
            var buffer = new LinkedList<int> { 1, 2, 3, 4, 5 };

            int readPosition = 0;
            int itemsToRead = 3;

            Console.WriteLine($"\n--- Circular Buffer Read ---");
            var node2 = buffer.First;
            for (int i = 0; i < itemsToRead; i++)
            {
                Console.WriteLine($"Read: {node2.Value}");
                node2 = node2.Next ?? buffer.First;
            }

            // Example 3: LRU Cache with linked list
            var lruCache = new LinkedList<string>();
            var cacheSet = new HashSet<string>();
            int maxCacheSize = 3;

            void AccessCacheItem(string item)
            {
                if (cacheSet.Contains(item))
                {
                    var existing = lruCache.Find(item);
                    lruCache.Remove(existing);
                    lruCache.AddLast(item);
                }
                else
                {
                    if (lruCache.Count >= maxCacheSize)
                    {
                        var removed = lruCache.First;
                        lruCache.RemoveFirst();
                        cacheSet.Remove(removed.Value);
                    }
                    lruCache.AddLast(item);
                    cacheSet.Add(item);
                }
            }

            AccessCacheItem("Page1");
            AccessCacheItem("Page2");
            AccessCacheItem("Page3");
            AccessCacheItem("Page1"); // Already there - move to end
            AccessCacheItem("Page4"); // Evicts Page2

            Console.WriteLine($"\n--- LRU Cache ---");
            Console.WriteLine($"Cache order: {string.Join(", ", lruCache)}");
            // Output: Page1, Page3, Page4 (Page2 was evicted)

            // Example 4: Building a linked list from array
            int[] sourceArray = { 1, 2, 3, 4, 5 };
            var fromArray = new LinkedList<int>(sourceArray);

            Console.WriteLine($"\n--- From Array ---");
            Console.WriteLine(string.Join(", ", fromArray));
            // Output: 1, 2, 3, 4, 5

            // Example 5: Finding middle element
            var middleList = new LinkedList<int> { 10, 20, 30, 40, 50 };
            int middleIndex = middleList.Count / 2;
            var midNode = middleList.First;
            for (int i = 0; i < middleIndex; i++)
            {
                midNode = midNode.Next;
            }

            Console.WriteLine($"\n--- Middle Element ---");
            Console.WriteLine($"Middle: {midNode.Value}");
            // Output: 30

            Console.WriteLine("\n=== LinkedList Advanced Operations Complete ===");
        }
    }
}