/*
 * ============================================================
 * TOPIC     : Memory Management
 * SUBTOPIC  : Garbage Collection - Generations
 * FILE      : 03_GCGenerations.cs
 * PURPOSE   : Teaches GC generations, how objects are promoted,
 *            and why generational collection improves performance
 * ============================================================
 */

using System; // System namespace for Console, basic types

namespace CSharp_MasterGuide._08_MemoryManagement._01_GarbageCollection
{
    /// <summary>
    /// Demonstrates GC generations and object promotion in .NET.
    /// Generational GC improves performance by collecting short-lived objects frequently.
    /// </summary>
    class GCGenerations
    {
        static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════
            // CONCEPT: What are GC Generations?
            // ═══════════════════════════════════════════════════════════
            // GC divides the heap into 3 generations:
            // - Generation 0 (Gen 0): New objects, collected most frequently
            // - Generation 1 (Gen 1): Objects that survived Gen 0 collection
            // - Generation 2 (Gen 2): Long-lived objects, collected least often
            // 
            // Theory: Most objects are short-lived (scope-based, temporary data).
            // By collecting Gen 0 frequently, we can quickly reclaim memory
            // without scanning the entire heap.

            Console.WriteLine("=== GC Generations Demo ===\n");

            // ── EXAMPLE 1: Check Object Generation ────────────────────────
            // When you create a new object, it starts in Generation 0.

            var shortLived = new Person { Name = "Temporary" }; // New object on heap
            int initialGen = GC.GetGeneration(shortLived); // GetGeneration returns 0 for new objects
            Console.WriteLine($"New object in generation: {initialGen}"); // Output: New object in generation: 0

            // ── EXAMPLE 2: Force GC to Promote Objects ───────────────────
            // By forcing GC, we can observe object promotion between generations.

            // Create multiple objects to increase memory pressure
            var objects = new System.Collections.Generic.List<object>();
            for (int i = 0; i < 1000; i++) // int = loop counter, 1000 = enough to trigger GC
            {
                objects.Add(new object()); // Create temporary objects
            }

            // Clear the list to make objects eligible for GC
            objects.Clear();
            objects = null; // Remove all references

            // Force GC collection
            GC.Collect(0, GCCollectionMode.Forced); // Collect Gen 0
            GC.WaitForPendingFinalizers(); // Wait for finalizers
            GC.Collect(0, GCCollectionMode.Forced); // Collect again

            // Check generation again - object would have been promoted if still alive
            int genAfterGC = GC.GetGeneration(shortLived); // Returns 1 if promoted, 0 if collected
            Console.WriteLine($"Object generation after GC: {genAfterGC}");
            // Output: Object generation after GC: [varies - likely 1 if survived]

            // Keep reference alive until here
            GC.KeepAlive(shortLived); // Prevents optimization from collecting shortLived

            // ── CONCEPT: GC.GetTotalMemory ────────────────────────────────
            // GetTotalMemory returns approximate memory used by managed heap.

            long memoryBefore = GC.GetTotalMemory(forceFullCollection: false); 
            // forceFullCollection: false = approximate, true = exact but slower
            Console.WriteLine($"\nMemory before: {memoryBefore:N0} bytes"); 
            // Output: Memory before: [varies]

            // Allocate large objects
            var largeObjects = new System.Collections.Generic.List<byte[]>();
            for (int i = 0; i < 100; i++) // 100 = number of arrays to create
            {
                largeObjects.Add(new byte[10000]); // 10KB each = 1MB total
            }

            long memoryAfter = GC.GetTotalMemory(forceFullCollection: false);
            Console.WriteLine($"Memory after allocations: {memoryAfter:N0} bytes");
            // Output: Memory after allocations: [higher than before]

            largeObjects = null; // Clear references

            // ── CONCEPT: GC.CollectionCount ───────────────────────────────
            // Returns how many times a specific generation has been collected.

            int gen0Collections = GC.CollectionCount(0); // Count of Gen 0 collections
            int gen1Collections = GC.CollectionCount(1); // Count of Gen 1 collections
            int gen2Collections = GC.CollectionCount(2); // Count of Gen 2 collections

            Console.WriteLine($"\nGC Collection Counts:");
            Console.WriteLine($"  Gen 0: {gen0Collections}"); // Output: Gen 0: [varies]
            Console.WriteLine($"  Gen 1: {gen1Collections}"); // Output: Gen 1: [varies]
            Console.WriteLine($"  Gen 2: {gen2Collections}"); // Output: Gen 2: [varies]

            // ── CONCEPT: Large Object Heap (LOH) ────────────────────────
            // Objects >= 85KB are allocated on the Large Object Heap (LOH).
            // LOH is only collected during Gen 2 (full) collections.

            Console.WriteLine("\n--- Large Object Heap ---");

            // Create objects of different sizes
            var smallObj = new byte[1000]; // 1KB - regular heap
            var largeObj = new byte[90000]; // ~90KB - large object heap threshold is 85KB

            Console.WriteLine($"Small object (1KB): on {(smallObj.Length < 85000 ? "regular heap" : "LOH")}");
            // Output: Small object (1KB): on regular heap
            Console.WriteLine($"Large object (90KB): on {(largeObj.Length >= 85000 ? "LOH" : "regular heap")}");
            // Output: Large object (90KB): on LOH

            // ── REAL-WORLD EXAMPLE: Object Lifetime Patterns ────────────
            Console.WriteLine("\n--- Real-World: Understanding Object Lifetime ---");

            // In real applications, objects have different lifetimes:
            // - Ephemeral (short-lived): loop variables, temporary calculations
            // - Scoped (method lifetime): method parameters, local variables
            // - Application (long-lived): caches, singletons, configuration

            var processor = new DataProcessor();

            // Simulate processing with various object lifetimes
            processor.ProcessEphemeralData(); // Short-lived objects
            processor.ProcessScopedData(); // Method-scoped objects
            processor.ProcessLongLivedData(); // Long-lived cached data

            // ── CONCEPT: GC.AddMemoryPressure / RemoveMemoryPressure ─────
            // Use these for objects that hold unmanaged memory.

            Console.WriteLine("\n--- Memory Pressure ---");

            var image = new FakeBitmap(width: 1000, height: 1000); // Simulated bitmap
            GC.AddMemoryPressure(image.GetApproximateSize()); // Report unmanaged memory
            Console.WriteLine($"Memory pressure added: {image.GetApproximateSize()} bytes");
            // Output: Memory pressure added: 4000000 bytes

            // When done with object
            image.Dispose(); // Clean up
            GC.RemoveMemoryPressure(image.GetApproximateSize()); // Remove pressure
            Console.WriteLine("Memory pressure removed");
            // Output: Memory pressure removed

            // ── CONCEPT: GC.GetGCMemoryInfo ─────────────────────────────
            // Provides detailed information about GC memory.

            var gcInfo = GC.GetGCMemoryInfo(); // Returns GC memory information
            Console.WriteLine($"\nGC Memory Info:");
            Console.WriteLine($"  Heap Size: {gcInfo.HeapSizeBytes:N0} bytes");
            Console.WriteLine($"  High Memory Threshold: {gcInfo.HighMemoryLoadThresholdBytes:N0} bytes");
            Console.WriteLine($"  Total Available: {gcInfo.TotalAvailableMemoryBytes:N0} bytes");
            // Output: [varies based on system]

            // ── CONCEPT: Optimizing for Generations ─────────────────────
            // Tips for working with generational GC:
            // 1. Avoid keeping references to short-lived objects unnecessarily
            // 2. Use pooling for frequently allocated objects
            // 3. Keep large objects pooled or cached efficiently
            // 4. Avoid unexpected object promotions

            // ── REAL-WORLD EXAMPLE: Performance Optimization ────────────
            Console.WriteLine("\n--- Real-World: Optimizing Data Processing ---");

            var batchProcessor = new BatchDataProcessor();
            
            // Process data efficiently with batches
            batchProcessor.ProcessInBatches(1000, batchSize: 100);
            // Output:
            // Processing batch 0...
            // Processing batch 1...
            // ...
            // Processing batch 9...

            Console.WriteLine("\n=== GC Generations Complete ===");
        }
    }

    /// <summary>
    /// Simple person class demonstrating object lifecycle.
    /// </summary>
    class Person
    {
        public string Name { get; set; } // string = reference type, immutable
        public int Age { get; set; } // int = 32-bit signed integer
    }

    /// <summary>
    /// Simulated bitmap class demonstrating memory pressure.
    /// </summary>
    class FakeBitmap : IDisposable
    {
        private int Width { get; } // int = 32-bit
        private int Height { get; } // int = 32-bit
        private bool disposed = false; // bool = true/false flag

        public FakeBitmap(int width, int height) // Constructor taking dimensions
        {
            Width = width; // Set width
            Height = height; // Set height
        }

        public long GetApproximateSize() // Returns approximate size in bytes
        {
            // Each pixel = 4 bytes (BGRA)
            return (long)Width * Height * 4; // Cast to long to avoid overflow
        }

        public void Dispose() // IDisposable implementation
        {
            if (!disposed) // Check if already disposed
            {
                disposed = true; // Mark as disposed
                // In real code, would release unmanaged resources here
            }
        }
    }

    /// <summary>
    /// Data processor demonstrating various object lifetime patterns.
    /// </summary>
    class DataProcessor
    {
        // Cache is long-lived (application lifetime)
        private readonly System.Collections.Generic.Dictionary<string, string> _cache 
            = new System.Collections.Generic.Dictionary<string, string>();

        public DataProcessor() // Constructor
        {
            // Initialize cache with some data
            _cache["config"] = "loaded"; // string = reference type
        }

        public void ProcessEphemeralData() // Ephemeral objects
        {
            // Loop-scoped objects - short-lived
            for (int i = 0; i < 10; i++) // int = loop counter
            {
                var temp = new object(); // New object each iteration - Gen 0
                // temp is eligible for GC immediately after this iteration
            }
            Console.WriteLine("Ephemeral data processed");
            // Output: Ephemeral data processed
        }

        public void ProcessScopedData() // Method-scoped objects
        {
            // Method-scoped - survives for method duration
            var data = new System.Collections.Generic.List<int>(); // List for method scope
            for (int i = 0; i < 100; i++) // int = loop counter
            {
                data.Add(i); // Add items to list
            }
            ProcessList(data); // Pass to another method
            Console.WriteLine("Scoped data processed");
            // Output: Scoped data processed
        }

        public void ProcessLongLivedData() // Long-lived objects
        {
            // Access cached data - survives many GC cycles
            if (_cache.ContainsKey("config")) // Check if key exists
            {
                Console.WriteLine($"Config: {_cache["config"]}");
                // Output: Config: loaded
            }
        }

        private void ProcessList(System.Collections.Generic.List<int> list) // Process the list
        {
            // Process list items
            int sum = 0; // int = sum accumulator
            foreach (var item in list) // foreach = iterate collection
            {
                sum += item; // Add each item
            }
        }
    }

    /// <summary>
    /// Batch processor demonstrating efficient memory use with generations.
    /// </summary>
    class BatchDataProcessor
    {
        public void ProcessInBatches(int totalItems, int batchSize) // Process items in batches
        {
            // Divide work into manageable batches
            int batchCount = (totalItems + batchSize - 1) / batchSize; // Calculate batches
            
            for (int batch = 0; batch < batchCount; batch++) // Iterate each batch
            {
                // Create batch-specific data (short-lived)
                var batchData = new System.Collections.Generic.List<int>(batchSize); // Pre-allocate
                
                int start = batch * batchSize; // Calculate start index
                int count = Math.Min(batchSize, totalItems - start); // Items in this batch
                
                for (int i = 0; i < count; i++) // Populate batch
                {
                    batchData.Add(start + i); // Add item
                }

                // Process the batch
                ProcessBatch(batchData, batch); // Process
                
                // batchData goes out of scope and is collected in Gen 0
            }
        }

        private void ProcessBatch(System.Collections.Generic.List<int> batch, int batchNumber) // Process single batch
        {
            Console.WriteLine($"Processing batch {batchNumber}...");
            // Output: Processing batch [n]...
        }
    }
}