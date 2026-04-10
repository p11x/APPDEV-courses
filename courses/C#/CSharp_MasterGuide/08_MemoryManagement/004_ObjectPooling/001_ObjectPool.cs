/*
 * ============================================================
 * TOPIC     : Memory Management
 * SUBTOPIC  : Object Pooling
 * FILE      : 01_ObjectPool.cs
 * PURPOSE   : Teaches ObjectPool<T> for reusing objects,
 *            reducing allocation overhead
 * ============================================================
 */

using System; // System namespace for Console, basic types
using System.Buffers; // For ObjectPool<T>

namespace CSharp_MasterGuide._08_MemoryManagement._04_ObjectPooling
{
    /// <summary>
    /// Demonstrates object pooling for performance.
    /// ObjectPool<T> reuses objects instead of
    /// creating new ones, reducing GC pressure.
    /// </summary>
    class ObjectPool
    {
        static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════
            // CONCEPT: Object Pooling ──────────────────────────────────────
            // ═══════════════════════════════════════════════════════════
            // Object pooling reuses expensive objects:
            // - Reduces GC allocations
            // - Lowers latency (no allocation cost)
            // - Controls resource usage
            // - Good for connection-like objects
            //
            // .NET provides ObjectPool<T> in System.Buffers
            // - Automatic rent/return management
            // - Configurable pool size
            // - Thread-safe
            //
            // Use cases:
            // - Database connections
            // - StringBuilders
            // - Buffers
            // - Large objects

            Console.WriteLine("=== Object Pooling Demo ===\n");

            // ── EXAMPLE 1: Basic ObjectPool Usage ────────────────────
            // Rent and return objects from pool.

            Console.WriteLine("1. Basic ObjectPool:");

            // Create pool with factory
            var pool = ObjectPool<ExpensiveObject>.Create(
                () => new ExpensiveObject(), // Factory = create new
                10 // MaxRetained = max objects
            );

            // Rent object from pool
            var obj1 = pool.Rent(); // Rent from pool
            obj1.Initialize("Object1"); // Initialize
            Console.WriteLine($"   Rented: {obj1.Name}"); // Output: Rented: Object1

            // Return to pool when done
            pool.Return(obj1); // Return to pool
            Console.WriteLine("   Returned to pool"); // Output: Returned to pool

            // Rent another - may get same object
            var obj2 = pool.Rent(); // Rent from pool
            Console.WriteLine($"   Rented: {obj2.Name}"); // Output: Rented: Object1 (reused!)

            // ── EXAMPLE 2: Pool with Multiple Objects ────────────────────
            Console.WriteLine("\n2. Multiple objects from pool:");

            var bufferPool = ObjectPool<byte[]>.Create(
                () => new byte[1024], // Factory = 1KB buffer
                5 // MaxRetained = 5 max
            );

            // Rent multiple buffers
            var buf1 = bufferPool.Rent(); // Rent first
            var buf2 = bufferPool.Rent(); // Rent second
            var buf3 = bufferPool.Rent(); // Rent third

            Console.WriteLine($"   Buf1 length: {buf1.Length}"); // Output: Buf1 length: 1024
            Console.WriteLine($"   Buf2 length: {buf2.Length}"); // Output: Buf2 length: 1024
            Console.WriteLine($"   Buf3 length: {buf3.Length}"); // Output: Buf3 length: 1024

            // Return all to pool
            bufferPool.Return(buf1); // Return first
            bufferPool.Return(buf2); // Return second
            bufferPool.Return(buf3); // Return third

            Console.WriteLine("   All buffers returned"); // Output: All buffers returned

            // ── EXAMPLE 3: Automatic Return with using ────────────────────────
            // Pool provides Get method that returns on dispose.

            Console.WriteLine("\n3. Get (automatic return):");

            // Get returns IDisposable wrapper
            using (var wrapper = bufferPool.Get()) // using = auto-return
            {
                var buffer = wrapper; // byte[] = get pooled item
                Console.WriteLine($"   Buffer acquired: {buffer.Length}"); // Output: Buffer acquired: 1024
            } // Automatically returned to pool

            Console.WriteLine("   Buffer auto-returned"); // Output: Buffer auto-returned

            // ── EXAMPLE 4: Pool Statistics ────────────────────────────────
            Console.WriteLine("\n4. Pool statistics:");

            var statPool = ObjectPool<StatObject>.Create(
                () => new StatObject(), // Factory
                5 // MaxRetained
            );

            // Rent and return multiple
            var item1 = statPool.Rent(); // Rent
            var item2 = statPool.Rent(); // Rent
            var item3 = statPool.Rent(); // Rent
            statPool.Return(item1); // Return
            statPool.Return(item2); // Return
            statPool.Return(item3); // Return

            Console.WriteLine("   Statistics:"); // Output header
            Console.WriteLine("   (Pool handles management)"); // Output: Pool handles management

            // ── EXAMPLE 5: Custom Pool Implementation ────────────────────
            Console.WriteLine("\n5. Custom pool implementation:");

            var customPool = new SimpleObjectPool<MyObject>(
                () => new MyObject(), // Factory
                3 // Max
            );

            // Use custom pool
            var myObj = customPool.Rent(); // Rent
            myObj.DoWork(); // Use object
            customPool.Return(myObj); // Return

            Console.WriteLine("   Custom pool used"); // Output: Custom pool used

            // ── EXAMPLE 6: Pool for Connection-Like Objects ───────
            Console.WriteLine("\n6. Connection pool:");

            var connectionPool = new ConnectionPool(); // ConnectionPool = wrapper

            var conn1 = connectionPool.Acquire(); // Connection = acquire
            conn1.Connect(); // Connect
            Console.WriteLine($"   Connected: {conn1.IsConnected}"); // Output: Connected: True

            var conn2 = connectionPool.Acquire(); // Connection = acquire another
            Console.WriteLine($"   Connection pool size: 2"); // Output: Connection pool size

            connectionPool.Release(conn1); // Release
            connectionPool.Release(conn2); // Release

            Console.WriteLine("   Connections released"); // Output: Connections released

            // ── REAL-WORLD EXAMPLE: Buffer Processing ────────────────
            Console.WriteLine("\n7. Real-world: Buffer processing:");

            var processPool = ObjectPool<byte[]>.Create(
                () => new byte[4096], // Factory = 4KB
                10 // MaxRetained
            );

            // Process multiple buffers
            for (int i = 0; i < 5; i++) // int = loop
            {
                using (var buffer = processPool.Get()) // using = auto-return
                {
                    // Fill buffer
                    var buf = buffer; // byte[] = reference
                    for (int j = 0; j < buf.Length; j++) // int = fill loop
                    {
                        buf[j] = (byte)(i * 100 + j); // byte = fill
                    }

                    // Process buffer
                    long sum = 0; // long = checksum
                    for (int j = 0; j < 100; j++) // int = process loop
                    {
                        sum += buf[j]; // byte = add
                    }

                    Console.WriteLine($"   Batch {i}: sum = {sum}"); // Output: Batch [n]: sum = [n]
                } // Returned to pool
            }

            Console.WriteLine("   All buffers processed"); // Output: All buffers processed

            Console.WriteLine("\n=== Object Pooling Demo Complete ===");
        }
    }

    /// <summary>
    /// Expensive object to pool - simulates heavy resource.
    /// </summary>
    class ExpensiveObject
    {
        public string Name { get; set; } = string.Empty; // string = name property
        private bool _isInitialized = false; // bool = init flag

        public void Initialize(string name) // Initialize object
        {
            Name = name; // Set name
            _isInitialized = true; // Set initialized
        }

        public bool IsInitialized => _isInitialized; // Property
    }

    /// <summary>
    /// Object for statistics tracking.
    /// </summary>
    class StatObject
    {
        public long Count { get; set; } // long = counter
        public DateTime LastUsed { get; set; } // DateTime = timestamp
    }

    /// <summary>
    /// Simple pooled object with work method.
    /// </summary>
    class MyObject
    {
        public void DoWork() // Do some work
        {
            Console.WriteLine("   MyObject working"); // Output: MyObject working
        }
    }

    /// <summary>
    /// Custom simple object pool implementation.
    /// </summary>
    class SimpleObjectPool<T> where T : class
    {
        private readonly Func<T> _factory; // Func<T> = factory
        private readonly int _maxSize; // int = max size
        private readonly System.Collections.Generic.Queue<T> _queue; // Queue = queue

        public SimpleObjectPool(Func<T> factory, int maxSize) // Constructor
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory)); // Check factory
            _maxSize = maxSize; // Set max
            _queue = new System.Collections.Generic.Queue<T>(); // Create queue
        }

        public T Rent() // Rent from pool
        {
            T item; // T = result
            if (_queue.Count > 0) // Check queue
            {
                item = _queue.Dequeue(); // Get from queue
            }
            else
            {
                item = _factory(); // Create new
            }
            return item; // T = return item
        }

        public void Return(T item) // Return to pool
        {
            if (item == null) // Check item
                throw new ArgumentNullException(nameof(item)); // Throw if null

            if (_queue.Count < _maxSize) // Check size
            {
                _queue.Enqueue(item); // Return to queue
            }
        }
    }

    /// <summary>
    /// Simple connection pool for demonstration.
    /// </summary>
    class ConnectionPool
    {
        private readonly System.Collections.Generic.Queue<PooledConnection> _connections; // Queue = connections
        private const int MaxConnections = 5; // int = max size

        public ConnectionPool() // Constructor
        {
            _connections = new System.Collections.Generic.Queue<PooledConnection>(); // Create queue
        }

        public PooledConnection Acquire() // Acquire connection
        {
            PooledConnection conn; // PooledConnection = result
            if (_connections.Count > 0) // Check available
            {
                conn = _connections.Dequeue(); // Reuse
                conn.Reset(); // Reset state
            }
            else
            {
                conn = new PooledConnection(); // Create new
            }
            return conn; // PooledConnection = return
        }

        public void Release(PooledConnection connection) // Release connection
        {
            if (connection == null) // Check connection
                throw new ArgumentNullException(nameof(connection)); // Throw

            if (_connections.Count < MaxConnections) // Check size
            {
                _connections.Enqueue(connection); // Return to pool
            }
        }
    }

    /// <summary>
    /// Pooled connection.
    /// </summary>
    class PooledConnection
    {
        private bool _isConnected; // bool = state

        public bool IsConnected => _isConnected; // Property

        public void Connect() // Connect
        {
            _isConnected = true; // Set connected
        }

        public void Reset() // Reset for reuse
        {
            _isconnected = false; // Reset state
            // Would close any underlying connection
        }
    }
}