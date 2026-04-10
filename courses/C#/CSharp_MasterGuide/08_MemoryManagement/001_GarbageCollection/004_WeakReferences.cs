/*
 * ============================================================
 * TOPIC     : Memory Management
 * SUBTOPIC  : Garbage Collection - Weak References
 * FILE      : 04_WeakReferences.cs
 * PURPOSE   : Teaches WeakReference for caching, memory-efficient
 *            patterns, and GCKeepAlive usage
 * ============================================================
 */

using System; // System namespace for Console, basic types
using System.Collections.Generic; // Collections for cache examples

namespace CSharp_MasterGuide._08_MemoryManagement._01_GarbageCollection
{
    /// <summary>
    /// Demonstrates WeakReference for memory-efficient caching
    /// and patterns for holding references without preventing GC.
    /// </summary>
    class WeakReferences
    {
        static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════
            // CONCEPT: What is a WeakReference?
            // ═══════════════════════════════════════════════════════════
            // WeakReference allows holding a reference to an object while
            // still allowing the GC to collect that object. Useful for:
            // - Caches that shouldn't prevent garbage collection
            // - Observer patterns
            // - Memory-sensitive caching systems
            //
            // Unlike regular references (strong), weak references don't
            // prevent the GC from collecting the target object.

            Console.WriteLine("=== WeakReference Demo ===\n");

            // ── EXAMPLE 1: Basic WeakReference ─────────────────────────────
            // Create an object and a weak reference to it.

            var target = new Person { Name = "John", Age = 30 }; // Create object on heap
            var weakRef = new WeakReference(target); // Wrap in weak reference
            
            Console.WriteLine($"Target alive before GC: {weakRef.IsAlive}"); // Output: Target alive before GC: True
            
            // Remove strong reference
            target = null; // Remove strong reference
            
            // GC can now collect the object
            GC.Collect(0, GCCollectionMode.Forced); // Force collection
            GC.WaitForPendingFinalizers(); // Wait for finalizers
            GC.Collect(0, GCCollectionMode.Forced); // Collect again
            
            Console.WriteLine($"Target alive after GC: {weakRef.IsAlive}"); // Output: Target alive after GC: False

            // ── EXAMPLE 2: WeakReference with Target ────────────────────
            // Try to get the target from a weak reference.

            var obj = new object(); // Create object
            var weak = new WeakReference(obj); // Create weak reference
            
            object retrieved = weak.Target; // Try to get target (returns object or null)
            Console.WriteLine($"Retrieved object: {(retrieved != null ? "Yes" : "No")}"); 
            // Output: Retrieved object: Yes
            
            obj = null; // Remove strong reference
            
            GC.Collect(); // Trigger garbage collection
            GC.WaitForPendingFinalizers();
            
            retrieved = weak.Target; // Try again after GC
            Console.WriteLine($"Retrieved after GC: {(retrieved != null ? "Yes" : "No")}");
            // Output: Retrieved after GC: No

            // ── CONCEPT: WeakReference<T> (Generic) ────────────────────────
            // Generic version provides type-safe weak references.
            // Available in .NET Framework 4.5+ and .NET Core

            Console.WriteLine("\n--- Generic WeakReference<T> ---");

            var personCache = new WeakReference<Person>(new Person { Name = "Alice", Age = 25 });
            
            // TryGetTarget is convenient method to get value
            if (personCache.TryGetTarget(out Person cached)) // out parameter receives value
            {
                Console.WriteLine($"Cached person: {cached.Name}"); // Output: Cached person: Alice
            }

            // ── REAL-WORLD EXAMPLE: Simple Object Cache ────────────────
            Console.WriteLine("\n--- Real-World: Object Cache ---");

            var cache = new SimpleObjectCache(); // Create cache instance
            
            // Add items to cache
            cache.Set("user:1", new User { Id = 1, Name = "Alice" });
            cache.Set("user:2", new User { Id = 2, Name = "Bob" });
            cache.Set("user:3", new User { Id = 3, Name = "Charlie" });
            
            Console.WriteLine($"Cache count: {cache.Count}"); // Output: Cache count: 3
            
            // Try to get cached item
            var user = cache.Get("user:1"); // Try to get by key
            Console.WriteLine($"Retrieved: {user?.Name ?? "null"}"); // Output: Retrieved: Alice
            
            // Force garbage collection to demonstrate weak nature
            GC.Collect(); // Collect garbage
            GC.WaitForPendingFinalizers();
            
            // After GC, items might be collected if memory pressure exists
            var userAfterGC = cache.Get("user:1"); // Try again
            Console.WriteLine($"After GC: {userAfterGC?.Name ?? "null (collected)"}");
            // Output: After GC: [varies - might be null if collected]

            // ── CONCEPT: GC KeepAlive ───────────────────────────────────
            // KeepAlive prevents an object from being collected until
            // a specific point in code, even if no references exist.

            Console.WriteLine("\n--- GC.KeepAlive ---");

            var tempObj = new object(); // Create object
            var tempRef = tempObj; // Create reference
            tempObj = null; // Remove original reference
            
            // tempObj is eligible for GC here, but...
            GC.KeepAlive(tempRef); // ...tempRef keeps it alive until this point
            
            Console.WriteLine("Object kept alive until KeepAlive");
            // Output: Object kept alive until KeepAlive

            // ── CONCEPT: Conditional Weak Table ───────────────────────────
            // ConditionalWeakTable allows adding key-value pairs where
            // the keys are weakly referenced. Used for extension properties.

            Console.WriteLine("\n--- ConditionalWeakTable Example ---");

            var extensionData = new System.Runtime.CompilerServices.ConditionalWeakTable<string, string>();
            
            extensionData.Add(new object().ToString(), "Metadata"); // Add with weak key
            Console.WriteLine("Added data to ConditionalWeakTable");
            // Output: Added data to ConditionalWeakTable

            // ── REAL-WORLD EXAMPLE: Event Handler Management ───────────
            Console.WriteLine("\n--- Real-World: Event Handler Cleanup ---");

            var publisher = new EventPublisher(); // Create event publisher
            
            var handler = new EventHandler((s, e) => Console.WriteLine("Event fired!")); // Create handler
            publisher.Subscribe(handler); // Subscribe to event
            
            publisher.RaiseEvent(); // Raise event - handler should fire
            // Output: Event fired!
            
            // Remove handler reference
            handler = null; // Remove strong reference
            
            // After GC, the handler might be collected
            publisher.ClearSubscriptions(); // Clear all subscriptions

            // ── CONCEPT: PhantomReferences ────────────────────────────────
            // PhantomReference allows you to perform cleanup after an object
            // is finalized but before memory is reclaimed. Useful for special
            // cleanup scenarios (not commonly needed).

            Console.WriteLine("\n=== WeakReferences Complete ===");
        }
    }

    /// <summary>
    /// Simple person class for demonstration.
    /// </summary>
    class Person
    {
        public string Name { get; set; } // string = reference type
        public int Age { get; set; } // int = 32-bit signed integer
    }

    /// <summary>
    /// User class for cache example.
    /// </summary>
    class User
    {
        public int Id { get; set; } // int = identifier
        public string Name { get; set; } // string = name
    }

    /// <summary>
    /// Simple object cache using WeakReference for memory efficiency.
    /// Cache doesn't prevent garbage collection of cached items.
    /// </summary>
    class SimpleObjectCache
    {
        // Dictionary<string, WeakReference<object>> = cache storage
        private readonly Dictionary<string, WeakReference<object>> _cache 
            = new Dictionary<string, WeakReference<object>>();

        public int Count // Property to get cache size
        {
            get
            {
                // Clean up dead references first
                CleanupDeadReferences(); // Remove null targets
                return _cache.Count; // Return count
            }
        }

        public void Set(string key, object value) // Add item to cache
        {
            CleanupDeadReferences(); // Clean before adding
            
            // Remove existing key if present
            if (_cache.ContainsKey(key)) // Check if key exists
            {
                _cache.Remove(key); // Remove old entry
            }
            
            // Add new weak reference
            _cache[key] = new WeakReference<object>(value); // Wrap value weakly
        }

        public object Get(string key) // Retrieve item from cache
        {
            if (_cache.TryGetValue(key, out var weakRef)) // Try to get weak reference
            {
                if (weakRef.TryGetTarget(out var target)) // Try to get actual object
                {
                    return target; // Return the object if still alive
                }
                
                // Target was collected, remove dead entry
                _cache.Remove(key); // Clean up dead entry
            }
            
            return null; // Return null if not found or collected
        }

        private void CleanupDeadReferences() // Remove dead weak references
        {
            var deadKeys = new List<string>(); // List to track dead keys
            
            foreach (var kvp in _cache) // Iterate all entries
            {
                if (!kvp.Value.IsAlive) // Check if target is still alive
                {
                    deadKeys.Add(kvp.Key); // Add dead key to list
                }
            }
            
            foreach (var key in deadKeys) // Remove all dead entries
            {
                _cache.Remove(key); // Remove from cache
            }
        }
    }

    /// <summary>
    /// Event publisher demonstrating weak event pattern.
    /// </summary>
    class EventPublisher
    {
        // List<WeakReference<EventHandler>> = list of weak event handlers
        private readonly List<WeakReference<EventHandler>> _handlers 
            = new List<WeakReference<EventHandler>>();

        public void Subscribe(EventHandler handler) // Subscribe to events
        {
            _handlers.Add(new WeakReference<EventHandler>(handler)); // Add weakly
        }

        public void RaiseEvent() // Raise event to all subscribers
        {
            var deadRefs = new List<WeakReference<EventHandler>>(); // Track dead refs
            
            foreach (var weakRef in _handlers) // Iterate handlers
            {
                if (weakRef.TryGetTarget(out var handler)) // Try to get handler
                {
                    handler(this, EventArgs.Empty); // Invoke handler
                }
                else
                {
                    deadRefs.Add(weakRef); // Mark dead reference
                }
            }
            
            // Remove dead references
            foreach (var dead in deadRefs) // Clean up
            {
                _handlers.Remove(dead); // Remove from list
            }
        }

        public void ClearSubscriptions() // Clear all subscriptions
        {
            _handlers.Clear(); // Remove all handlers
            Console.WriteLine("Subscriptions cleared"); // Output: Subscriptions cleared
        }
    }
}