/*
 * ============================================================
 * TOPIC     : C# Fundamentals
 * SUBTOPIC  : Loops - Foreach Loop (Part 1)
 * FILE      : ForeachLoop.cs
 * PURPOSE   : This file covers the foreach loop in C#, a cleaner way to iterate over collections
 *             and arrays without worrying about indices.
 * ============================================================
 */

// --- SECTION: Foreach Loops ---
// Foreach provides a cleaner way to iterate over collections
// Automatically handles enumeration - no index management needed

using System;
using System.Collections.Generic;

namespace CSharp_MasterGuide._01_Fundamentals._06_Loops
{
    class ForeachLoop
    {
        static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════════
            // SECTION: Basic Foreach with Arrays
            // ═══════════════════════════════════════════════════════════════
            
            // Basic foreach - iterate over array
            string[] colors = { "Red", "Green", "Blue" };
            
            foreach (string color in colors)
            {
                Console.WriteLine($"Color: {color}");
            }
            // Output: Color: Red, Color: Green, Color: Blue
            
            // With var (type inferred)
            foreach (var color in colors)
            {
                Console.WriteLine(color.ToUpper());
            }
            // Output: RED, GREEN, BLUE
            
            // Integer array
            int[] numbers = { 1, 2, 3, 4, 5 };
            
            int sum = 0;
            foreach (int num in numbers)
            {
                sum += num;
            }
            Console.WriteLine($"Sum: {sum}"); // Output: Sum: 15

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Foreach with Collections
            // ═══════════════════════════════════════════════════════════════
            
            // List<T>
            var fruits = new List<string> { "Apple", "Banana", "Cherry" };
            
            foreach (string fruit in fruits)
            {
                Console.WriteLine($"Fruit: {fruit}");
            }
            
            // Dictionary
            var capitals = new Dictionary<string, string>
            {
                ["USA"] = "Washington D.C.",
                ["UK"] = "London",
                ["France"] = "Paris"
            };
            
            foreach (KeyValuePair<string, string> pair in capitals)
            {
                Console.WriteLine($"{pair.Key} capital: {pair.Value}");
            }
            
            // HashSet
            var uniqueNumbers = new HashSet<int> { 1, 2, 3, 2, 1 };
            
            foreach (int num in uniqueNumbers)
            {
                Console.WriteLine($"Unique: {num}");
            }
            // Output: 1, 2, 3 (duplicates removed)

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Foreach with Custom Collections
            // ═══════════════════════════════════════════════════════════════
            
            // Queue
            var taskQueue = new Queue<string>();
            taskQueue.Enqueue("Task 1");
            taskQueue.Enqueue("Task 2");
            taskQueue.Enqueue("Task 3");
            
            Console.WriteLine("\n=== Processing Tasks ===");
            
            foreach (string task in taskQueue)
            {
                Console.WriteLine($"Executing: {task}");
            }
            
            // Stack
            var stack = new Stack<string>();
            stack.Push("First");
            stack.Push("Second");
            stack.Push("Third");
            
            Console.WriteLine("\n=== Stack Pop Order ===");
            
            foreach (string item in stack)
            {
                Console.WriteLine(item);
            }
            // Output: Third, Second, First (LIFO order)

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Foreach with LINQ
            // ═══════════════════════════════════════════════════════════════
            
            var data = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            
            // Filter with foreach (manual LINQ)
            Console.WriteLine("\n=== Manual Filter ===");
            
            foreach (int num in data)
            {
                if (num % 2 == 0)
                {
                    Console.WriteLine($"Even: {num}");
                }
            }
            
            // Transform with foreach (manual Select)
            Console.WriteLine("\n=== Manual Transform ===");
            
            foreach (int num in data)
            {
                Console.WriteLine($"{num} squared = {num * num}");
            }
            
            // Group with foreach (manual GroupBy)
            var grouped = new Dictionary<string, List<int>>();
            
            foreach (int num in data)
            {
                string key = num % 2 == 0 ? "Even" : "Odd";
                
                if (!grouped.ContainsKey(key))
                    grouped[key] = new List<int>();
                
                grouped[key].Add(num);
            }
            
            Console.WriteLine("\n=== Grouped ===");
            
            foreach (var group in grouped)
            {
                Console.WriteLine($"{group.Key}: {string.Join(", ", group.Value)}");
            }

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Foreach with Index (When Needed)
            // ═══════════════════════════════════════════════════════════════
            
            // If you need index, use for loop OR use Select with index
            var names = new[] { "Alice", "Bob", "Charlie" };
            
            // Option 1: for loop
            for (int i = 0; i < names.Length; i++)
            {
                Console.WriteLine($"{i}: {names[i]}");
            }
            
            // Option 2: LINQ Select with index
            foreach (var (name, index) in names.Select((n, i) => (n, i)))
            {
                Console.WriteLine($"Index {index}: {name}");
            }

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Foreach with Break/Continue
            // ═══════════════════════════════════════════════════════════════
            
            var numbers2 = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            
            // Break - exit early
            Console.WriteLine("\n=== Break Example ===");
            
            foreach (int num in numbers2)
            {
                if (num > 5)
                {
                    Console.WriteLine("Breaking at 6");
                    break;
                }
                Console.WriteLine(num);
            }
            // Output: 1,2,3,4,5, Breaking at 6
            
            // Continue - skip iteration
            Console.WriteLine("\n=== Continue Example ===");
            
            foreach (int num in numbers2)
            {
                if (num % 2 == 0)
                    continue; // Skip even numbers
                
                Console.WriteLine($"Odd: {num}");
            }
            // Output: 1, 3, 5, 7, 9

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Real-World Examples
            // ═══════════════════════════════════════════════════════════════
            
            // ── Configuration processing ───────────────────────────────────
            var config = new Dictionary<string, string>
            {
                ["Host"] = "localhost",
                ["Port"] = "8080",
                ["Timeout"] = "30"
            };
            
            Console.WriteLine("\n=== Configuration ===");
            
            foreach (var setting in config)
            {
                Console.WriteLine($"{setting.Key} = {setting.Value}");
            }
            
            // ── File list processing ───────────────────────────────────────
            var files = new[] { "file1.txt", "file2.txt", "file3.doc", "file4.pdf" };
            
            Console.WriteLine("\n=== File Types ===");
            
            foreach (var file in files)
            {
                if (file.EndsWith(".txt"))
                    Console.WriteLine($"Text file: {file}");
                else if (file.EndsWith(".pdf"))
                    Console.WriteLine($"PDF: {file}");
                else
                    Console.WriteLine($"Other: {file}");
            }
            
            // ── Tree traversal simulation ─────────────────────────────────
            var tree = new TreeNode<string>("Root")
            {
                Children = new List<TreeNode<string>>
                {
                    new TreeNode<string>("Child 1"),
                    new TreeNode<string>("Child 2")
                    {
                        Children = new List<TreeNode<string>>
                        {
                            new TreeNode<string>("Grandchild")
                        }
                    }
                }
            };
            
            Console.WriteLine("\n=== Tree Traversal ===");
            
            foreach (var node in tree.Flatten())
            {
                Console.WriteLine(node);
            }
        }
    }
    
    // ═══════════════════════════════════════════════════════════════════════
    // TreeNode for tree traversal example
    // ═══════════════════════════════════════════════════════════════════════
    
    class TreeNode<T>
    {
        public T Value { get; }
        public List<TreeNode<T>> Children { get; set; } = new List<TreeNode<T>>();
        
        public TreeNode(T value)
        {
            Value = value;
        }
        
        // Flatten tree for foreach
        public IEnumerable<T> Flatten()
        {
            yield return Value;
            
            foreach (var child in Children)
            {
                foreach (var descendant in child.Flatten())
                {
                    yield return descendant;
                }
            }
        }
    }
}
