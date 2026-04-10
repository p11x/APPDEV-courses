/*
 * ============================================================
 * TOPIC     : C# Fundamentals
 * SUBTOPIC  : Loops - Foreach Loop (Part 2)
 * FILE      : ForeachLoop_Part2.cs
 * PURPOSE   : This file covers advanced foreach topics including IEnumerable,
 *             deconstruction, and performance considerations.
 * ============================================================
 */

// --- SECTION: Advanced Foreach Topics ---
// Covers advanced patterns and considerations with foreach

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CSharp_MasterGuide._01_Fundamentals._06_Loops
{
    // Custom enumerable for demonstration
    class CountEnumerable : IEnumerable<int>
    {
        private int _start;
        private int _count;
        
        public CountEnumerable(int start, int count)
        {
            _start = start;
            _count = count;
        }
        
        public IEnumerator<int> GetEnumerator()
        {
            for (int i = 0; i < _count; i++)
            {
                yield return _start + i;
            }
        }
        
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    
    // Enumerable with state
    class FibonacciEnumerable : IEnumerable<long>
    {
        private int _count;
        
        public FibonacciEnumerable(int count)
        {
            _count = count;
        }
        
        public IEnumerator<long> GetEnumerator()
        {
            long n1 = 0, n2 = 1;
            
            for (int i = 0; i < _count; i++)
            {
                yield return n1;
                long next = n1 + n2;
                n1 = n2;
                n2 = next;
            }
        }
        
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    class ForeachLoop_Part2
    {
        static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════════
            // SECTION: Foreach with Custom Enumerables
            // ═══════════════════════════════════════════════════════════════
            
            // Using custom enumerable
            var count = new CountEnumerable(5, 10);
            
            foreach (int num in count)
            {
                Console.WriteLine($"Count: {num}");
            }
            // Output: 5,6,7,8,9,10,11,12,13,14
            
            // Fibonacci sequence
            Console.WriteLine("\n=== Fibonacci ===");
            
            var fib = new FibonacciEnumerable(10);
            
            foreach (long num in fib)
            {
                Console.WriteLine(num);
            }
            // Output: 0,1,1,2,3,5,8,13,21,34

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Deconstruction in Foreach
            // ═══════════════════════════════════════════════════════════════
            
            // Tuple deconstruction (C# 8.0+)
            var pairs = new[] { (1, "One"), (2, "Two"), (3, "Three") };
            
            foreach (var (num, name) in pairs)
            {
                Console.WriteLine($"Number: {num}, Name: {name}");
            }
            
            // With index using LINQ
            var items = new[] { "A", "B", "C" };
            
            foreach (var (item, index) in items.Select((x, i) => (x, i)))
            {
                Console.WriteLine($"[{index}] {item}");
            }

            // ═══════════════════════════════════════════════════════════════
            // SECTION: IAsyncEnumerable (C# 8.0+)
            // ═══════════════════════════════════════════════════════════════
            
            // Note: IAsyncEnumerable requires async foreach
            // This is just to show the concept
            Console.WriteLine("\n=== Async Enumerable (concept) ===");
            
            // Real async foreach:
            // await foreach (var item in GetAsyncStream())
            // {
            //     Console.WriteLine(item);
            // }

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Performance Considerations
            // ═══════════════════════════════════════════════════════════════
            
            // Foreach vs for - when to use which
            
            // Use for when:
            // - Need index for access or modification
            // - Working with arrays (slightly faster)
            // - Need to skip elements
            // - Need to iterate backwards
            
            // Use foreach when:
            // - Just reading all elements
            // - Working with any collection
            // - Cleaner code is priority
            // - Collection might change during iteration
            
            // Performance demo
            int[] arr = Enumerable.Range(1, 10000).ToArray();
            
            // for loop
            long sum1 = 0;
            var sw = System.Diagnostics.Stopwatch.StartNew();
            
            for (int i = 0; i < arr.Length; i++)
            {
                sum1 += arr[i];
            }
            
            sw.Stop();
            Console.WriteLine($"For loop: {sw.ElapsedTicks} ticks");
            
            // foreach loop
            long sum2 = 0;
            sw.Restart();
            
            foreach (int val in arr)
            {
                sum2 += val;
            }
            
            sw.Stop();
            Console.WriteLine($"Foreach: {sw.ElapsedTicks} ticks");

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Modifying Collection During Iteration
            // ═══════════════════════════════════════════════════════════════
            
            // This will throw exception!
            // var list = new List<int> { 1, 2, 3 };
            // foreach (int x in list)
            // {
            //     list.Add(x * 2); // InvalidOperationException
            // }
            
            // Safe approach: create a copy
            var safeList = new List<int> { 1, 2, 3 };
            var copy = safeList.ToList();
            
            foreach (int x in copy)
            {
                safeList.Add(x * 2);
            }
            
            Console.WriteLine("\n=== Safe Modification ===");
            
            foreach (int x in safeList)
            {
                Console.WriteLine(x);
            }
            // Output: 1,2,3,2,4,6 (doubled values added)

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Foreach with Null Collections
            // ═══════════════════════════════════════════════════════════════
            
            List<int>? nullableList = null;
            
            // Null check before foreach
            if (nullableList != null)
            {
                foreach (int x in nullableList)
                {
                    Console.WriteLine(x);
                }
            }
            else
            {
                Console.WriteLine("List is null");
            }
            
            // Using null-coalescing with default
            var items2 = nullableList ?? new List<int>();
            
            foreach (int x in items2)
            {
                Console.WriteLine(x);
            }
            // Output: List is null (then empty)

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Real-World Examples
            // ═══════════════════════════════════════════════════════════════
            
            // ── Event handler processing ───────────────────────────────────
            var events = new List<string> { "Login", "Click", "Submit", "Error" };
            
            Console.WriteLine("\n=== Event Processing ===");
            
            foreach (var evt in events)
            {
                if (evt == "Error")
                {
                    Console.WriteLine("ALERT: Error event received!");
                    // In real code: log alert, send notification
                }
                else
                {
                    Console.WriteLine($"Processed event: {evt}");
                }
            }
            
            // ── Hierarchy traversal ───────────────────────────────────────
            var org = new Department("CEO")
            {
                SubDepartments = new List<Department>
                {
                    new Department("Engineering"),
                    new Department("Sales")
                    {
                        SubDepartments = new List<Department>
                        {
                            new Department("North America"),
                            new Department("Europe")
                        }
                    }
                }
            };
            
            Console.WriteLine("\n=== Organization ===");
            
            foreach (var dept in org.Flatten())
            {
                Console.WriteLine(dept);
            }
            
            // ── Graph traversal ─────────────────────────────────────────────
            var graph = new Dictionary<string, string[]>
            {
                ["A"] = new[] { "B", "C" },
                ["B"] = new[] { "D" },
                ["C"] = new[] { "D" },
                ["D"] = new string[] { }
            };
            
            Console.WriteLine("\n=== Graph Edges ===");
            
            foreach (var node in graph)
            {
                Console.WriteLine($"{node.Key} connects to: {string.Join(", ", node.Value)}");
            }
        }
    }
    
    // ═══════════════════════════════════════════════════════════════════════
    // Department class for hierarchy example
    // ═══════════════════════════════════════════════════════════════════════
    
    class Department
    {
        public string Name { get; }
        public List<Department> SubDepartments { get; set; } = new();
        
        public Department(string name) => Name = name;
        
        public IEnumerable<string> Flatten()
        {
            yield return Name;
            
            foreach (var sub in SubDepartments)
            {
                foreach (var descendant in sub.Flatten())
                {
                    yield return descendant;
                }
            }
        }
        
        public override string ToString() => Name;
    }
}
