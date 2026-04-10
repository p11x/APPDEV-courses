/*
 * ============================================================
 * TOPIC     : Memory Management
 * SUBTOPIC  : Garbage Collection Basics
 * FILE      : 01_GCBasics.cs
 * PURPOSE   : Teaches fundamentals of garbage collection in .NET,
 *            how GC works, memory allocation, and managed heap
 * ============================================================
 */

using System; // System namespace for Console, basic types
using System.Runtime.InteropServices; // For GC native interop hints

namespace CSharp_MasterGuide._08_MemoryManagement._01_GarbageCollection
{
    /// <summary>
    /// Demonstrates the fundamentals of garbage collection in .NET.
    /// GC automatically manages memory for managed objects on the heap.
    /// </summary>
    class GCBasics
    {
        static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════
            // CONCEPT: What is Garbage Collection?
            // ═══════════════════════════════════════════════════════════
            // Garbage Collection (GC) is an automatic memory management feature
            // in .NET that reclaims memory from objects no longer in use.
            // It runs on the managed heap and identifies unreachable objects.

            // ── EXAMPLE 1: Simple Object Allocation ─────────────────────
            // Creating objects allocates memory on the managed heap.
            // When objects become unreachable, GC can reclaim that memory.

            var person = new Person { Name = "Alice", Age = 30 }; // person = reference to object on heap
            Console.WriteLine($"Created person: {person.Name}"); // Output: Created person: Alice
            
            // The person variable now references an unreachable object after this point
            // GC will eventually reclaim the memory (not immediate)
            person = null; // Setting to null makes object eligible for GC

            // ── EXAMPLE 2: GC.GetTotalMemory ───────────────────────────
            // GetTotalMemory returns approximate bytes allocated on heap.
            // Parameter 'forceFullGC' = true triggers full collection.

            long memoryBefore = GC.GetTotalMemory(forceFullCollection: false); // false = don't force GC
            Console.WriteLine($"Memory before allocations: {memoryBefore:N0} bytes"); 
            // Output: Memory before allocations: [varies based on runtime]

            // ── EXAMPLE 3: Memory Allocation with new objects ──────────
            // Each 'new' allocates memory on the managed heap.

            for (int i = 0; i < 1000; i++) // int = 32-bit signed integer, loop counter
            {
                var temp = new string('x', 10); // Creates 1000 string objects
            }

            long memoryAfter = GC.GetTotalMemory(forceFullCollection: false);
            Console.WriteLine($"Memory after allocations: {memoryAfter:N0} bytes");
            // Output: Memory after allocations: [higher than before]

            // ── CONCEPT: Managed vs Unmanaged Memory ─────────────────
            // Managed memory = objects tracked by GC (heap)
            // Unmanaged memory = raw memory not tracked by GC (file handles, connections)

            // ── EXAMPLE 4: GC Collection Modes ────────────────────────
            // GC.Collect() forces garbage collection (not recommended in production).
            
            GC.Collect(0, GCCollectionMode.Forced); // 0 = generation 0, Forced = immediate
            GC.WaitForPendingFinalizers(); // Wait for finalizers to run
            GC.Collect(0, GCCollectionMode.Forced); // Collect again after finalizers

            long memoryAfterGC = GC.GetTotalMemory(forceFullCollection: true);
            Console.WriteLine($"Memory after forced GC: {memoryAfterGC:N0} bytes");
            // Output: Memory after forced GC: [typically lower]

            // ── CONCEPT: GC Generation Theory ──────────────────────────
            // Objects are divided into generations (0, 1, 2).
            // New objects start in generation 0.
            // Surviving objects get promoted to higher generations.
            // Generation 2 = longest-lived objects (permanent data, caches).

            // ── EXAMPLE 5: Checking Generation ────────────────────────
            var shortLived = new object(); // New object starts in gen 0
            int gen = GC.GetGeneration(shortLived); // GetGeneration returns which gen
            Console.WriteLine($"New object generation: {gen}");
            // Output: New object generation: 0

            // Keep reference alive to prevent collection
            GC.KeepAlive(shortLived); // Keeps object alive until this point

            // ── REAL-WORLD EXAMPLE: Resource Management ──────────────
            // In production, rely on 'using' statements and IDisposable
            // rather than explicitly calling GC methods.

            Console.WriteLine("\n--- Real-World Scenario: Cache Simulation ---");
            
            // Simulate a simple cache that gets cleaned up
            var cache = new SimpleCache();
            
            // Add items to cache
            cache.Set("user:1", "Alice");
            cache.Set("user:2", "Bob");
            cache.Set("user:3", "Charlie");
            
            Console.WriteLine($"Cache size: {cache.Count}");
            // Output: Cache size: 3

            // Clear cache (simulates memory pressure)
            cache.Clear();
            
            // Force garbage collection to demonstrate
            GC.Collect(0, GCCollectionMode.Aggressive, blocking: true);
            
            Console.WriteLine($"Cache after clear: {cache.Count}");
            // Output: Cache after clear: 0

            // ── CONCEPT: When to call GC (rarely needed) ─────────────
            // Only call GC in specific scenarios:
            // 1. After releasing large objects (memory pressure)
            // 2. During application idle state
            // 3. For measurement/benchmarking

            // ── REAL-WORLD EXAMPLE: Large Data Processing ───────────
            Console.WriteLine("\n--- Real-World Scenario: Batch Processing ---");
            
            // Process data in batches to avoid memory issues
            var processor = new BatchProcessor();
            
            // Simulate processing
            for (int batch = 0; batch < 3; batch++) // 3 batches
            {
                processor.ProcessBatch(batch);
                Console.WriteLine($"Processed batch {batch}");
            }
            // Output:
            // Processed batch 0
            // Processed batch 1
            // Processed batch 2

            // ── EXAMPLE 6: GC RegisterForFullGCNotification ─────────
            // Advanced: Get notified when GC threshold approaches.
            // This is for high-performance scenarios.

            // NOTE: This requires .NET Framework or specific runtime
            // Console.WriteLine($"GC Heap size: {GC.GetTotalMemory(false)}");

            Console.WriteLine("\n=== GC Basics Complete ===");
        }
    }

    /// <summary>
    /// Simple person class for demonstrating object allocation.
    /// </summary>
    class Person
    {
        public string Name { get; set; } // string = reference type, immutable
        public int Age { get; set; } // int = 32-bit signed integer
    }

    /// <summary>
    /// Simple cache demonstrating memory management concepts.
    /// </summary>
    class SimpleCache
    {
        // Dictionary<string, string> = key-value pairs, reference type
        private readonly System.Collections.Generic.Dictionary<string, string> _cache 
            = new System.Collections.Generic.Dictionary<string, string>();

        public int Count => _cache.Count; // Property returning cache size

        public void Set(string key, string value)
        {
            _cache[key] = value; // Dictionary assignment
        }

        public void Clear()
        {
            _cache.Clear(); // Clear all items
        }
    }

    /// <summary>
    /// Batch processor demonstrating memory-conscious data handling.
    /// </summary>
    class BatchProcessor
    {
        public void ProcessBatch(int batchNumber)
        {
            // Create temporary data for this batch
            // In real scenarios, process and release immediately
            var batchData = new System.Collections.Generic.List<string>();
            
            for (int i = 0; i < 100; i++) // Simulate 100 items per batch
            {
                batchData.Add($"Item_{batchNumber}_{i}");
            }

            // Process the batch (in real code, this would do actual work)
            // batchData is eligible for GC after this method returns
            Console.WriteLine($"  Batch {batchNumber} has {batchData.Count} items");
        }
    }
}