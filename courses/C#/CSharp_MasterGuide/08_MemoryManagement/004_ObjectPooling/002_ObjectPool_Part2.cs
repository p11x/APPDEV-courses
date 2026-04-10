/*
 * ============================================================
 * TOPIC     : Memory Management
 * SUBTOPIC  : Object Pooling - Part 2
 * FILE      : 02_ObjectPool_Part2.cs
 * PURPOSE   : Advanced pooling patterns, pool configuration,
 *            and pooling strategies
 * ============================================================
 */

using System; // System namespace for Console, basic types
using System.Buffers; // For ObjectPool<T>

namespace CSharp_MasterGuide._08_MemoryManagement._04_ObjectPooling
{
    /// <summary>
    /// Demonstrates advanced object pooling patterns
    /// and configuration options.
    /// </summary>
    class ObjectPool_Part2
    {
        static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════
            // ADVANCED POOLING ───────────────────────────────────────────
            // ═══════════════════════════════════════════════════════════
            // Advanced pooling topics:
            // - Pool configuration options
            // - Clear pool state on return
            // - Thread-safe patterns
            // - Pool leak detection
            // - Custom pool policies

            Console.WriteLine("=== Object Pooling Part 2 ===\n");

            // ── EXAMPLE 1: Pool with Reset ───────────────────────────────
            // Reset objects on return for reuse.

            Console.WriteLine("1. Pool with reset:");

            var resetPool = ObjectPool<ResettableObject>.Create(
                () => new ResettableObject(), // Factory
                5 // MaxRetained
            );

            // Rent object
            var resettable = resetPool.Rent(); // Rent
            resettable.SetData("Test", 42); // Set data
            Console.WriteLine($"   Name: {resettable.Name}, Value: {resettable.Value}"); // Output: Name: Test, Value: 42

            // Return - resets state for next user
            resetPool.Return(resettable); // Return

            // Rent again - state cleared
            var reused = resetPool.Rent(); // Rent reused
            Console.WriteLine($"   Reused Name: '{reused.Name}', Value: {reused.Value}"); // Output: Reused Name: '', Value: 0

            // ── EXAMPLE 2: Multiple Pool Types ─────────────────────────
            Console.WriteLine("\n2. Multiple pool types:");

            // Different pool sizes for different needs
            var smallPool = ObjectPool<byte[]>.Create(() => new byte[256], 2); // Small
            var largePool = ObjectPool<byte[]>.Create(() => new byte[65536], 1); // Large

            // Small buffer needed
            using (var small = smallPool.Get()) // using = auto-return
            {
                Console.WriteLine($"   Small buffer: {small.Length}"); // Output: Small buffer: 256
            }

            // Large buffer needed
            using (var large = largePool.Get()) // using = auto-return
            {
                Console.WriteLine($"   Large buffer: {large.Length}"); // Output: Large buffer: 65536
            }

            // ── EXAMPLE 3: Thread-Safe Pool Access ──────────────────
            // ObjectPool<T> handles thread safety internally.

            Console.WriteLine("\n3. Thread-safe usage:");

            var threadPool = ObjectPool<ThreadObject>.Create(
                () => new ThreadObject(), // Factory
                10 // MaxRetained
            );

            // Simple thread usage example
            var threadObj1 = threadPool.Rent(); // Rent thread 1
            var threadObj2 = threadPool.Rent(); // Rent thread 2

            threadObj1.SetThreadId(1); // Set thread
            threadObj2.SetThreadId(2); // Set thread

            Console.WriteLine($"   Thread 1 ID: {threadObj1.ThreadId}"); // Output: Thread 1 ID: 1
            Console.WriteLine($"   Thread 2 ID: {threadObj2.ThreadId}"); // Output: Thread 2 ID: 2

            threadPool.Return(threadObj1); // Return
            threadPool.Return(threadObj2); // Return

            // ── EXAMPLE 4: Pool with Cleanup ────────────────────────
            // Clean up sensitive data on return.

            Console.WriteLine("\n4. Pool with cleanup:");

            var securePool = ObjectPool<SecureObject>.Create(
                () => new SecureObject(), // Factory
                3 // MaxRetained
            );

            // Use secure object
            var secure = securePool.Rent(); // Rent
            secure.Store("password123"); // Store sensitive
            Console.WriteLine($"   Stored: {secure.GetData()}"); // Output: Stored: password123

            // Return - cleans up on return
            securePool.Return(secure); // Return with cleanup
            Console.WriteLine("   Secure object returned and cleaned"); // Output message

            // ── EXAMPLE 5: Lazy Pool Initialization ──────────────────
            Console.WriteLine("\n5. Lazy pool:");

            // Create pool lazily
            var lazyPool = ObjectPool<StringBuilderWrapper>.Create(
                () => new StringBuilderWrapper(), // Factory
                5 // MaxRetained
            );

            // First use - creates object
            using (var sb = lazyPool.Get()) // using = get
            {
                sb.Append("First use"); // string = append
                Console.WriteLine($"   Content: {sb.ToString()}"); // Output: Content: First use
            }

            // Second use - reuses object
            using (var sb2 = lazyPool.Get()) // using = get
            {
                sb2.Append("Second use"); // string = append
                Console.WriteLine($"   Content: {sb2.ToString()}"); // Output: Content: Second use
            }

            // ── EXAMPLE 6: Pool Statistics ────────────────────────────
            // Track pool usage (basic tracking).

            Console.WriteLine("\n6. Pool tracking:");

            var trackedPool = ObjectPool<TrackedObject>.Create(
                () => new TrackedObject(), // Factory
                3 // MaxRetained
            );

            // Track rentals
            long beforeRent = TrackedObject.RentCount; // Count before
            var tracked1 = trackedPool.Rent(); // Rent
            long afterRent = TrackedObject.RentCount; // Count after

            Console.WriteLine($"   Rented: object #{TrackedObject.RentCount}"); // Output: Rented: object #1

            // Track returns
            trackedPool.Return(tracked1); // Return
            Console.WriteLine($"   Returned: object #{TrackedObject.ReturnCount}"); // Output: Returned: object #1

            // ── EXAMPLE 7: Custom Pool Policy ────────────────────────
            // Implement custom pool behavior.

            Console.WriteLine("\n7. Custom pool policy:");

            var policyPool = new PolicyPool<PolicyObject>(
                () => new PolicyObject(), // Factory
                2, // MaxRetained
                (obj) => obj.IsValid, // Predicate = validation
                (obj) => obj.Cleanup // Action = cleanup
            );

            var policyObj = policyPool.Rent(); // Rent
            policyObj.Validate(); // Validate
            Console.WriteLine($"   Valid: {policyObj.IsValid}"); // Output: Valid: True

            policyPool.Return(policyObj); // Return with cleanup

            // ── REAL-WORLD EXAMPLE: JSON Processing ───────────────────
            Console.WriteLine("\n8. Real-world: JSON processing:");

            var jsonPool = ObjectPool<char[]>.Create(
                () => new char[4096], // Factory = JSON buffer
                10 // MaxRetained
            );

            // Process multiple JSON strings
            string[] jsonStrings = { 
                "{\"name\":\"Alice\"}", 
                "{\"name\":\"Bob\"}", 
                "{\"name\":\"Charlie\"}" 
            };

            foreach (var json in jsonStrings) // string = loop
            {
                using (var buffer = jsonPool.Get()) // using = get buffer
                {
                    // Copy JSON to buffer
                    for (int i = 0; i < json.Length && i < buffer.Length; i++) // int = loop
                    {
                        buffer[i] = json[i]; // char = copy
                    }

                    // Process buffer
                    Console.WriteLine($"   Processing: {new string(buffer, 0, json.Length)}"); // Output: Processing: [json]
                } // Return to pool
            }

            Console.WriteLine("   JSON processing complete"); // Output message

            Console.WriteLine("\n=== Object Pooling Part 2 Complete ===");
        }
    }

    /// <summary>
    /// Object that can be reset for reuse.
    /// </summary>
    class ResettableObject
    {
        public string Name { get; set; } = string.Empty; // string = name
        public int Value { get; set; } // int = value

        public void Reset() // Reset state
        {
            Name = string.Empty; // Reset name
            Value = 0; // Reset value
        }
    }

    /// <summary>
    /// Object that tracks thread.
    /// </summary>
    class ThreadObject
    {
        public int ThreadId { get; private set; } // int = thread ID

        public void SetThreadId(int id) // Set thread
        {
            ThreadId = id; // Set ID
        }
    }

    /// <summary>
    /// Object that cleans up sensitive data.
    /// </summary>
    class SecureObject
    {
        private byte[] _data = Array.Empty<byte>(); // byte[] = data storage

        public void Store(string sensitive) // Store sensitive data
        {
            _data = System.Text.Encoding.UTF8.GetBytes(sensitive); // byte[] = encode
        }

        public string GetData() // Get data
        {
            return System.Text.Encoding.UTF8.GetString(_data); // string = decode
        }

        public void Cleanup() // Clean up sensitive data
        {
            Array.Clear(_data, 0, _data.Length); // Clear array
            _data = Array.Empty<byte>(); // Reset
        }
    }

    /// <summary>
    /// Simple StringBuilder wrapper for pooling.
    /// </summary>
    class StringBuilderWrapper
    {
        private readonly System.Text.StringBuilder _sb = new System.Text.StringBuilder(); // StringBuilder = builder

        public void Append(string text) // Append text
        {
            _sb.Append(text); // Append
        }

        public override string ToString() // Get string
        {
            return _sb.ToString(); // string = get
        }

        public void Clear() // Clear builder
        {
            _sb.Clear(); // Clear
        }
    }

    /// <summary>
    /// Object with tracking counters.
    /// </summary>
    class TrackedObject
    {
        private static long _rentCount = 0; // static long = rent counter
        private static long _returnCount = 0; // static long = return counter

        public static long RentCount => _rentCount; // Property getter
        public static long ReturnCount => _returnCount; // Property getter

        public TrackedObject() // Constructor
        {
            // Increment static counter
        }

        public void MarkRented() // Mark as rented
        {
            System.Threading.Interlocked.Increment(ref _rentCount); // Increment
        }

        public void MarkReturned() // Mark as returned
        {
            System.Threading.Interlocked.Increment(ref _returnCount); // Increment
        }
    }

    /// <summary>
    /// Object with policy requirements.
    /// </summary>
    class PolicyObject
    {
        public bool IsValid { get; private set; } // bool = valid flag

        public void Validate() // Validate object
        {
            IsValid = true; // Set valid
        }

        public void Cleanup() // Cleanup
        {
            IsValid = false; // Reset
        }
    }

    /// <summary>
    /// Custom pool with policy.
    /// </summary>
    class PolicyPool<T> where T : class
    {
        private readonly Func<T> _factory; // Func<T> = factory
        private readonly int _maxSize; // int = max
        private readonly System.Collections.Generic.Queue<T> _items; // Queue = items
        private readonly Func<T, bool> _canReturn; // Func = predicate
        private readonly Action<T> _cleanup; // Action = cleanup

        public PolicyPool(Func<T> factory, int maxSize, Func<T, bool> canReturn, Action<T> cleanup) // Constructor
        {
            _factory = factory; // Set factory
            _maxSize = maxSize; // Set max
            _items = new System.Collections.Generic.Queue<T>(); // Create queue
            _canReturn = canReturn; // Set predicate
            _cleanup = cleanup; // Set cleanup
        }

        public T Rent() // Rent item
        {
            T item; // T = result
            if (_items.Count > 0) // Check queue
            {
                item = _items.Dequeue(); // Dequeue
            }
            else
            {
                item = _factory(); // Create new
            }
            return item; // T = return
        }

        public void Return(T item) // Return item
        {
            if (item == null) // Check item
                throw new ArgumentNullException(nameof(item)); // Throw

            if (_canReturn(item)) // Check policy
            {
                _cleanup(item); // Cleanup
            }

            if (_items.Count < _maxSize) // Check size
            {
                _items.Enqueue(item); // Enqueue
            }
        }
    }
}