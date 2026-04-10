/*
 * ============================================================
 * TOPIC     : Collections
 * SUBTOPIC  : ConcurrentDictionary - Thread-Safe Dictionary
 * FILE      : ConcurrentDictionary.cs
 * PURPOSE   : Demonstrates ConcurrentDictionary<TKey,TValue> which
 *            provides thread-safe operations for multi-threaded access
 * ============================================================
 */

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CSharp_MasterGuide._04_Collections._02_Dictionary_Hashtable
{
    class ConcurrentDictionaryDemo
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== ConcurrentDictionary<TKey,TValue> ===\n");

            // ═══════════════════════════════════════════════════════════
            // SECTION 1: Creating ConcurrentDictionary
            // ═══════════════════════════════════════════════════════════

            // Empty concurrent dictionary
            var emptyConcurrent = new ConcurrentDictionary<string, int>();
            Console.WriteLine($"Empty concurrent dict count: {emptyConcurrent.Count}");
            // Output: Empty concurrent dict count: 0

            // Initialize with values
            var capitals = new ConcurrentDictionary<string, string>(
                new Dictionary<string, string>
                {
                    { "USA", "Washington D.C." },
                    { "UK", "London" },
                    { "France", "Paris" }
                });
            Console.WriteLine($"Capitals count: {capitals.Count}");
            // Output: Capitals count: 3

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: Thread-Safe Operations
            // ═══════════════════════════════════════════════════════════

            var scores = new ConcurrentDictionary<string, int>();

            // TryAdd - adds only if key doesn't exist (atomic)
            bool added = scores.TryAdd("Alice", 95);
            Console.WriteLine($"\nTryAdd 'Alice': {added}");
            // Output: TryAdd 'Alice': True

            added = scores.TryAdd("Alice", 100); // Won't overwrite
            Console.WriteLine($"TryAdd 'Alice' again: {added}");
            // Output: TryAdd 'Alice' again: False

            // AddOrUpdate - adds or updates atomically
            scores.AddOrUpdate("Bob", 87, (key, oldValue) => oldValue + 1);
            scores.AddOrUpdate("Bob", 87, (key, oldValue) => oldValue + 1);
            Console.WriteLine($"Bob's score: {scores["Bob"]}");
            // Output: Bob's score: 89

            // GetOrDownload - retrieves or computes value (for lazy initialization)
            int result = scores.GetOrAdd("Charlie", key =>
            {
                // Simulate expensive computation
                Thread.Sleep(10);
                return 92;
            });
            Console.WriteLine($"Charlie's score: {result}");
            // Output: Charlie's score: 92

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: Updating Values Safely
            // ═══════════════════════════════════════════════════════════

            var counter = new ConcurrentDictionary<string, int>();

            // Increment counter thread-safely
            counter.TryAdd("Visits", 0);
            
            // Using Interlocked pattern with AddOrUpdate
            counter.AddOrUpdate("Visits", 1, (k, v) => v + 1);
            Console.WriteLine($"\nVisits: {counter["Visits"]}");
            // Output: Visits: 1

            // Atomic update using AddOrUpdate
            counter.AddOrUpdate("Visits", 1, (k, v) => v + 1);
            Console.WriteLine($"Visits after increment: {counter["Visits"]}");
            // Output: Visits after increment: 2

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: Concurrent Updates from Multiple Threads
            // ═══════════════════════════════════════════════════════════

            var parallelScores = new ConcurrentDictionary<int, int>();

            // Simulate multiple threads updating same key
            Parallel.For(0, 1000, i =>
            {
                parallelScores.AddOrUpdate(i, 1, (key, count) => count + 1);
            });

            Console.WriteLine($"\nParallel counter test:");
            Console.WriteLine($"Unique keys: {parallelScores.Count}");
            // Output: Unique keys: 1000

            // Each key was added once, updated 999 times
            Console.WriteLine($"Key 0 value: {parallelScores[0]}");
            // Output: Key 0 value: 1000 (all threads hit key 0? No, each key has unique value 1)
            // Actually: Each i gets value 1 (first AddOrUpdate sets value, subsequent updates don't trigger for new keys)
            
            // Let's test with a single shared counter
            var sharedCounter = new ConcurrentDictionary<string, int>();
            sharedCounter["Total"] = 0;
            
            Parallel.For(0, 1000, i =>
            {
                sharedCounter.AddOrUpdate("Total", 1, (k, v) => v + 1);
            });
            
            Console.WriteLine($"Shared counter after 1000 increments: {sharedCounter["Total"]}");
            // Output: Shared counter after 1000 increments: 1000

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: Safe Removal
            // ═══════════════════════════════════════════════════════════

            var toRemove = new ConcurrentDictionary<string, int>
            {
                ["A"] = 1,
                ["B"] = 2,
                ["C"] = 3
            };

            // TryRemove - atomic remove
            bool removed = toRemove.TryRemove("B", out int removedValue);
            Console.WriteLine($"\nTryRemove 'B': {removed}, Value: {removedValue}");
            // Output: TryRemove 'B': True, Value: 2

            // TryRemove on non-existent key
            removed = toRemove.TryRemove("Z", out int notFound);
            Console.WriteLine($"TryRemove 'Z': {removed}");
            // Output: TryRemove 'Z': False

            Console.WriteLine($"Remaining count: {toRemove.Count}");
            // Output: Remaining count: 2

            // ═══════════════════════════════════════════════════════════
            // SECTION 6: Comparison with Regular Dictionary
            // ═══════════════════════════════════════════════════════════

            var regularDict = new Dictionary<string, int>();
            var concurrentDict = new ConcurrentDictionary<string, int>();

            // Regular Dictionary - needs manual locking for thread safety
            lock (regularDict)
            {
                regularDict["Key"] = 1;
            }

            // ConcurrentDictionary - safe by default
            concurrentDict["Key"] = 1;

            Console.WriteLine("\n=== Thread-Safe Operations ===");
            Console.WriteLine("ConcurrentDictionary provides lock-free reads");
            Console.WriteLine("and atomic writes for common operations");

            // ═══════════════════════════════════════════════════════════
            // SECTION 7: Real-World Example - Caching
            // ═══════════════════════════════════════════════════════════

            var cache = new ConcurrentDictionary<string, User>();

            // Simulate cached data
            cache.TryAdd("user_1", new User { Id = 1, Name = "Alice" });
            cache.TryAdd("user_2", new User { Id = 2, Name = "Bob" });

            Console.WriteLine("\n=== User Cache ===");

            // Get or create (lazy load)
            User? user1 = cache.GetOrAdd("user_1", key => new User { Id = 99, Name = "New User" });
            Console.WriteLine($"Got user_1: {user1?.Name}");
            // Output: Got user_1: Alice

            // Non-existent key - creates new
            User user3 = cache.GetOrAdd("user_3", key => new User { Id = 3, Name = "Charlie" });
            Console.WriteLine($"Created user_3: {user3.Name}");
            // Output: Created user_3: Charlie

            // Invalidate cache
            cache.TryRemove("user_2", out User? removedUser);
            Console.WriteLine($"Removed user_2: {removedUser?.Name}");
            // Output: Removed user_2: Bob

            // Clear cache
            cache.Clear();
            Console.WriteLine($"Cache cleared, count: {cache.Count}");
            // Output: Cache cleared, count: 0

            Console.WriteLine("\n=== ConcurrentDictionary Complete ===");
        }
    }

    class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
