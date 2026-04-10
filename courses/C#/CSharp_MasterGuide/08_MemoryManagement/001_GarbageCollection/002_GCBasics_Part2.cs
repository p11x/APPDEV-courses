/*
 * ============================================================
 * TOPIC     : Memory Management
 * SUBTOPIC  : Garbage Collection - GC Basics Part 2
 * FILE      : 02_GCBasics_Part2.cs
 * PURPOSE   : Explores advanced GC concepts: generations, collection 
 *            modes, latency modes, and GC notifications
 * ============================================================
 */

using System; // System namespace for Console, basic types
using System.Runtime; // For GCLatencyMode enum
using System.Runtime.CompilerServices; // For runtime hints

namespace CSharp_MasterGuide._08_MemoryManagement._01_GarbageCollection
{
    /// <summary>
    /// Demonstrates advanced garbage collection concepts including
    /// collection modes, latency modes, and GC notifications.
    /// </summary>
    class GCBasics_Part2
    {
        static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════
            // CONCEPT: GC Collection Modes
            // ═══════════════════════════════════════════════════════════
            // GCCollectionMode defines when and how collection occurs.
            // Options: Default, Forced, ForcedBlocking, Aggressive, Optimized

            // ── EXAMPLE 1: GCCollectionMode.Default ───────────────────
            // Default mode - GC decides optimal time to collect.
            
            long memoryStart = GC.GetTotalMemory(forceFullCollection: false);
            Console.WriteLine($"Initial memory: {memoryStart:N0} bytes");
            // Output: Initial memory: [varies based on runtime]

            // Allocate some objects
            for (int i = 0; i < 500; i++)
            {
                var temp = new byte[100]; // byte[] = managed array, heap allocation
            }

            long memoryAfterAlloc = GC.GetTotalMemory(forceFullCollection: false);
            Console.WriteLine($"Memory after alloc: {memoryAfterAlloc:N0} bytes");
            // Output: Memory after alloc: [higher]

            // ── EXAMPLE 2: GCCollectionMode.Forced ─────────────────────
            // Forces immediate garbage collection (not recommended in production).
            
            GC.Collect(0, GCCollectionMode.Forced); // generation 0, forced
            GC.WaitForPendingFinalizers(); // Wait for finalizers
            GC.Collect(0, GCCollectionMode.Forced); // Collect again post-finalizers

            long memoryAfterForced = GC.GetTotalMemory(forceFullCollection: true);
            Console.WriteLine($"After forced GC: {memoryAfterForced:N0} bytes");
            // Output: After forced GC: [typically lower]

            // ── EXAMPLE 3: GCCollectionMode.Aggressive ─────────────────
            // Aggressive collection - reclaims maximum memory quickly.
            // Use when under severe memory pressure.

            Console.WriteLine("\n--- Testing Aggressive Collection ---");
            
            // Allocate large objects
            var largeObjects = new System.Collections.Generic.List<byte[]>();
            for (int i = 0; i < 10; i++)
            {
                largeObjects.Add(new byte[10000]); // 10KB each
            }

            Console.WriteLine($"Allocated {largeObjects.Count} large objects");
            // Output: Allocated 10 large objects

            // Clear list - objects now eligible for GC
            largeObjects.Clear(); // Clear references
            largeObjects = null; // Make eligible for collection

            // Aggressive collection attempt
            GC.Collect(2, GCCollectionMode.Aggressive, blocking: true, compacting: true);
            
            long memoryAfterAggressive = GC.GetTotalMemory(forceFullCollection: true);
            Console.WriteLine($"After aggressive GC: {memoryAfterAggressive:N0} bytes");
            // Output: After aggressive GC: [significantly lower]

            // ═══════════════════════════════════════════════════════════
            // CONCEPT: GC Latency Modes
            // ═══════════════════════════════════════════════════════════
            // LatencyMode controls how intrusive GC is to application execution.
            // Options: Batch, Interactive, LowLatency, NoGCRegion

            // ── EXAMPLE 4: Setting Latency Mode ────────────────────────
            
            GCLatencyMode originalMode = GC.GetGCLatencyMode(); // Get current mode
            Console.WriteLine($"\nOriginal GC Latency Mode: {originalMode}");
            // Output: Original GC Latency Mode: Interactive (typically)

            try
            {
                // Set to Low Latency - less aggressive collection
                // Good for real-time or latency-sensitive applications
                GC.SetGCLatencyMode(GCLatencyMode.LowLatency);
                
                Console.WriteLine($"New GC Latency Mode: {GC.GetGCLatencyMode()}");
                // Output: New GC Latency Mode: LowLatency

                // Simulate latency-sensitive work
                SimulateLatencySensitiveWork();
            }
            finally
            {
                // Restore original mode
                GC.SetGCLatencyMode(originalMode);
                Console.WriteLine($"Restored GC Latency Mode: {GC.GetGCLatencyMode()}");
                // Output: Restored GC Latency Mode: [original]
            }

            // ═══════════════════════════════════════════════════════════
            // CONCEPT: GC Modes (Workstation vs Server)
            // ═══════════════════════════════════════════════════════════
            // Workstation: Low latency, faster but less thorough
            // Server: Higher throughput, more memory, more CPU intensive

            // ── EXAMPLE 5: Checking GC Mode ───────────────────────────
            
            bool isServerGC = GCSettings.IsServerGC; // IsServerGC property
            string gcMode = isServerGC ? "Server" : "Workstation";
            
            Console.WriteLine($"\nGC Mode: {gcMode}");
            // Output: GC Mode: [varies - typically Workstation for console]

            Console.WriteLine($"GC Latency Mode: {GCSettings.LatencyMode}");
            // Output: GC Latency Mode: Interactive (or other)

            // ═══════════════════════════════════════════════════════════
            // CONCEPT: Large Object Heap (LOH)
            // ═══════════════════════════════════════════════════════════
            // Objects >= 85KB are allocated on LOH (generation 2).
            // LOH is not compacted by default (can cause fragmentation).

            // ── EXAMPLE 6: Large Object Heap ───────────────────────────
            
            Console.WriteLine("\n--- Large Object Heap ---");
            
            // Allocate large objects (>85KB threshold)
            var largeArray = new byte[100000]; // ~100KB - goes to LOH
            Console.WriteLine($"Large array allocated on LOH (Gen {GC.GetGeneration(largeArray)})");
            // Output: Large array allocated on LOH (Gen 2)

            // Keep reference alive
            GC.KeepAlive(largeArray);

            // ═══════════════════════════════════════════════════════════
            // REAL-WORLD EXAMPLE: Game Development
            // ═══════════════════════════════════════════════════════════
            Console.WriteLine("\n--- Real-World: Game Frame ---");
            
            var gameFrame = new GameFrame();
            
            // Process game objects
            gameFrame.AddGameObject("Player", 100);
            gameFrame.AddGameObject("Enemy", 50);
            gameFrame.AddGameObject("Bullet", 200);
            
            Console.WriteLine($"Game objects: {gameFrame.ObjectCount}");
            // Output: Game objects: 3

            // End of frame - objects become eligible for GC
            gameFrame.Clear();
            
            // Minimal GC during gameplay - optimize for frame rate
            GC.Collect(0, GCCollectionMode.Optimized);

            // ═══════════════════════════════════════════════════════════
            // REAL-WORLD EXAMPLE: Web Server Request Handling
            // ═══════════════════════════════════════════════════════════
            Console.WriteLine("\n--- Real-World: Web Request ---");
            
            var requestHandler = new RequestHandler();
            
            // Simulate request processing
            for (int i = 0; i < 3; i++)
            {
                var request = new RequestData { Id = i, Data = new byte[1000] };
                requestHandler.ProcessRequest(request);
            }
            
            Console.WriteLine($"Requests processed: {requestHandler.ProcessedCount}");
            // Output: Requests processed: 3

            // After request completes, memory is reclaimed automatically
            // Low latency mode keeps GC from interrupting request handling

            Console.WriteLine("\n=== GC Basics Part 2 Complete ===");
        }

        /// <summary>
        /// Simulates latency-sensitive work where GC should be minimally intrusive.
        /// </summary>
        static void SimulateLatencySensitiveWork()
        {
            // Allocate some objects
            var data = new byte[5000]; // 5KB buffer
            
            // Process data with minimal GC interruption
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = (byte)(i % 256); // byte = 0-255
            }
            
            Console.WriteLine($"Processed {data.Length} bytes with LowLatency mode");
            // Output: Processed 5000 bytes with LowLatency mode

            GC.KeepAlive(data); // Keep alive until here
        }
    }

    /// <summary>
    /// Represents a single frame in a game, managing game objects.
    /// Demonstrates memory management in real-time applications.
    /// </summary>
    class GameFrame
    {
        // Dictionary<string, GameObject> = maps name to game object
        private System.Collections.Generic.Dictionary<string, GameObject> _objects 
            = new System.Collections.Generic.Dictionary<string, GameObject>();

        public int ObjectCount => _objects.Count; // Property for count

        /// <summary>
        /// Adds a game object to this frame.
        /// </summary>
        public void AddGameObject(string name, int size)
        {
            _objects[name] = new GameObject { Name = name, Size = size };
        }

        /// <summary>
        /// Clears all objects at end of frame.
        /// </summary>
        public void Clear()
        {
            _objects.Clear();
        }
    }

    /// <summary>
    /// Represents a game object with name and memory size.
    /// </summary>
    class GameObject
    {
        public string Name { get; set; } // string = reference type
        public int Size { get; set; } // int = 32-bit signed
    }

    /// <summary>
    /// Handles HTTP-like request processing with memory awareness.
    /// </summary>
    class RequestHandler
    {
        private int _processedCount = 0; // int counter

        public int ProcessedCount => _processedCount; // Property

        /// <summary>
        /// Processes a single request, handling memory efficiently.
        /// </summary>
        public void ProcessRequest(RequestData request)
        {
            // Process request data (in real scenario, would do actual work)
            
            // Simulate processing
            var buffer = new byte[request.Data.Length]; // byte array
            Array.Copy(request.Data, buffer, buffer.Length); // Array.Copy = bulk copy
            
            _processedCount++; // Increment counter
            
            Console.WriteLine($"Request {request.Id} processed, buffer size: {buffer.Length}");
            // Output: Request [id] processed, buffer size: [size]

            GC.KeepAlive(request); // Keep request alive during processing
        }
    }

    /// <summary>
    /// Represents incoming request data.
    /// </summary>
    class RequestData
    {
        public int Id { get; set; } // int = request identifier
        public byte[] Data { get; set; } // byte[] = request payload
    }
}