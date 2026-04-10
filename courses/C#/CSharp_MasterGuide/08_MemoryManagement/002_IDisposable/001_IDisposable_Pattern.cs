/*
 * ============================================================
 * TOPIC     : Memory Management
 * SUBTOPIC  : IDisposable Pattern
 * FILE      : 01_IDisposable_Pattern.cs
 * PURPOSE   : Teaches the IDisposable interface and standard
 *            dispose pattern for releasing unmanaged resources
 * ============================================================
 */

using System; // System namespace for Console, basic types
using System.IO; // For file stream example

namespace CSharp_MasterGuide._08_MemoryManagement._02_IDisposable
{
    /// <summary>
    /// Demonstrates the IDisposable pattern for resource management.
    /// IDisposable is used to release unmanaged resources like files,
    /// database connections, network sockets, etc.
    /// </summary>
    class IDisposable_Pattern
    {
        static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════
            // CONCEPT: Why IDisposable? ─────────────────────────────────
            // ═══════════════════════════════════════════════════════════
            // .NET's garbage collector manages managed memory automatically.
            // However, unmanaged resources (file handles, DB connections)
            // are NOT managed by GC and must be explicitly released.
            //
            // IDisposable provides a standard pattern for cleanup:
            // - Dispose() method for immediate cleanup
            // - Optional Finalizer as safety net

            Console.WriteLine("=== IDisposable Pattern Demo ===\n");

            // ── EXAMPLE 1: Basic Using Statement ───────────────────────
            // The 'using' statement ensures Dispose() is called.

            Console.WriteLine("1. Using statement with FileStream:");

            // FileStream implements IDisposable - automatically disposed
            using (var fs = new FileStream("test.txt", FileMode.Create)) // using = auto-dispose
            {
                var data = System.Text.Encoding.UTF8.GetBytes("Hello, World!"); // byte array
                fs.Write(data, 0, data.Length); // Write data to file
            } // Dispose() called automatically here
            
            Console.WriteLine("   File written and closed"); // Output: File written and closed
            // FileStream properly closed/disposed

            // ── EXAMPLE 2: Using with Declaration (C# 8+) ─────────────
            // Newer syntax that disposes at end of scope.

            Console.WriteLine("\n2. Using with declaration:");

            using var stream = new MemoryStream(); // using var = dispose at end of method
            stream.WriteByte(1); // Write single byte
            stream.WriteByte(2); // Write another byte
            Console.WriteLine("   Stream has data"); // Output: Stream has data
            // stream.Dispose() called when method ends

            // ── EXAMPLE 3: Multiple Resources ──────────────────────────
            // Can use multiple using statements (nested or sequential).

            Console.WriteLine("\n3. Multiple resources:");

            using (var r1 = new StringReader("input1")) // First resource
            using (var r2 = new StringReader("input2")) // Second resource
            {
                Console.WriteLine($"   Read: {r1.ReadLine()}"); // Output: Read: input1
                Console.WriteLine($"   Read: {r2.ReadLine()}}"); // Output: Read: input2
            } // Both disposed in reverse order

            // ── CONCEPT: Custom IDisposable Class ─────────────────────
            // Create your own classes for unmanaged resources.

            Console.WriteLine("\n4. Custom IDisposable:");

            var dbConnection = new DatabaseConnection("Server=localhost;Database=Test"); // string = connection string
            dbConnection.Open(); // Open connection
            Console.WriteLine($"   Connection opened: {dbConnection.IsOpen}"); // Output: Connection opened: True
            
            dbConnection.Execute("SELECT * FROM Users"); // Execute query
            Console.WriteLine("   Query executed"); // Output: Query executed
            
            dbConnection.Dispose(); // Explicitly call dispose
            Console.WriteLine($"   Connection disposed: {!dbConnection.IsOpen}"); // Output: Connection disposed: False

            // ── EXAMPLE 5: Dispose Pattern with Fields ─────────────────
            // Shows proper pattern when class has multiple resources.

            Console.WriteLine("\n5. Dispose pattern with multiple resources:");

            var processor = new DocumentProcessor(); // Create processor with multiple resources
            processor.LoadDocument("report.pdf"); // Load document
            processor.Process(); // Process document
            processor.Dispose(); // Clean up all resources

            // ── REAL-WORLD EXAMPLE: Resource Pool ───────────────────────
            Console.WriteLine("\n6. Resource pool pattern:");

            var pool = new ResourcePool(); // Create pool
            var resource1 = pool.Acquire(); // Acquire resource
            var resource2 = pool.Acquire(); // Acquire another
            
            Console.WriteLine($"   Pool active: {pool.ActiveCount}"); // Output: Pool active: 2
            
            pool.Release(resource1); // Return to pool
            pool.Release(resource2); // Return to pool
            
            Console.WriteLine($"   Pool available: {pool.AvailableCount}"); // Output: Pool available: 2

            // ── CONCEPT: When to Implement IDisposable ─────────────────
            // Implement IDisposable when class:
            // - Holds unmanaged resources directly
            // - Holds disposable managed objects
            // - Has a finalizer (for safety net)

            Console.WriteLine("\n=== IDisposable Pattern Complete ===");
        }
    }

    /// <summary>
    /// Simulated database connection demonstrating IDisposable.
    /// In real code, would wrap actual database connection.
    /// </summary>
    class DatabaseConnection : IDisposable
    {
        private readonly string _connectionString; // string = database connection details
        private bool _isOpen = false; // bool = connection state flag
        private bool _disposed = false; // bool = disposal flag

        public bool IsOpen => _isOpen; // Property to check if connection is open

        public DatabaseConnection(string connectionString) // Constructor
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            // ?? throw = null coalescing - throw if null
        }

        public void Open() // Open database connection
        {
            if (_disposed) // Check if already disposed
                throw new ObjectDisposedException(nameof(DatabaseConnection)); // Throw if disposed
            
            // In real code: actual database connection would be opened here
            _isOpen = true; // Set open flag
        }

        public void Execute(string query) // Execute SQL query
        {
            if (!_isOpen) // Check if connection is open
                throw new InvalidOperationException("Connection not open"); // Throw if not open
            
            // In real code: would execute actual SQL
            Console.WriteLine($"   Executing: {query}"); // Output the query
        }

        /// <summary>
        /// Implements IDisposable.Dispose() method.
        /// Releases all resources used by this object.
        /// </summary>
        public void Dispose() // IDisposable.Dispose implementation
        {
            Dispose(disposing: true); // Call overload with disposing = true
            GC.SuppressFinalize(this); // Prevent finalizer from running
        }

        /// <summary>
        /// Protected virtual dispose method.
        /// </summary>
        /// <param name="disposing">true if called from Dispose(), false from finalizer</param>
        protected virtual void Dispose(bool disposing) // Overload for disposal logic
        {
            if (!_disposed) // Check if already disposed
            {
                if (disposing) // Check if called from Dispose() (not finalizer)
                {
                    // Release managed resources
                    if (_isOpen) // Check if connection is open
                    {
                        Close(); // Close the connection
                    }
                }

                // Release unmanaged resources (if any)
                // In this example, there are no unmanaged resources
                
                _disposed = true; // Mark as disposed
            }
        }

        private void Close() // Close the connection
        {
            // In real code: actual connection.Close() would be called
            _isOpen = false; // Set closed flag
            Console.WriteLine("   Connection closed"); // Output: Connection closed
        }

        /// <summary>
        /// Finalizer - called by GC when object is collected.
        /// Provides safety net if Dispose() was never called.
        /// </summary>
        ~DatabaseConnection() // Finalizer - runs during garbage collection
        {
            Dispose(disposing: false); // Call dispose with disposing = false
        }
    }

    /// <summary>
    /// Document processor demonstrating multi-resource disposal.
    /// </summary>
    class DocumentProcessor : IDisposable
    {
        private FileStream _fileStream = null; // FileStream = file handle
        private MemoryStream _buffer = null; // MemoryStream = memory buffer
        private bool _disposed = false; // bool = disposal flag

        public void LoadDocument(string path) // Load document from file
        {
            // Open file for reading
            _fileStream = new FileStream(path, FileMode.Open, FileAccess.Read); // Open file
            _buffer = new MemoryStream(); // Create buffer for processing
            
            // Read file content into buffer
            _fileStream.CopyTo(_buffer); // Copy from file to buffer
            _buffer.Position = 0; // Reset position to start
            
            Console.WriteLine($"   Loaded document: {path}"); // Output: Loaded document: [path]
        }

        public void Process() // Process the document
        {
            if (_disposed) // Check if disposed
                throw new ObjectDisposedException(nameof(DocumentProcessor)); // Throw if disposed
            
            if (_buffer == null) // Check if buffer exists
                throw new InvalidOperationException("No document loaded"); // Throw if not loaded
            
            // Process document data
            var data = _buffer.ToArray(); // Get data as byte array
            Console.WriteLine($"   Processed {data.Length} bytes"); // Output: Processed [n] bytes
        }

        /// <summary>
        /// Implements IDisposable.Dispose().
        /// </summary>
        public void Dispose() // IDisposable implementation
        {
            Dispose(disposing: true); // Call overload
            GC.SuppressFinalize(this); // Prevent finalization
        }

        /// <summary>
        /// Protected virtual dispose method.
        /// </summary>
        /// <param name="disposing">true if called from Dispose(), false from finalizer</param>
        protected virtual void Dispose(bool disposing) // Overload for cleanup
        {
            if (!_disposed) // Check if already disposed
            {
                if (disposing) // Check if from Dispose()
                {
                    // Dispose managed resources
                    if (_buffer != null) // Check if buffer exists
                    {
                        _buffer.Dispose(); // Dispose buffer
                        _buffer = null; // Clear reference
                    }

                    if (_fileStream != null) // Check if file stream exists
                    {
                        _fileStream.Dispose(); // Dispose file stream
                        _fileStream = null; // Clear reference
                    }
                }

                // Dispose unmanaged resources (none in this example)
                
                _disposed = true; // Mark as disposed
            }
        }

        /// <summary>
        /// Finalizer for safety net.
        /// </summary>
        ~DocumentProcessor() // Finalizer
        {
            Dispose(disposing: false); // Call dispose from finalizer
        }
    }

    /// <summary>
    /// Resource pool for managing reusable resources.
    /// </summary>
    class ResourcePool : IDisposable
    {
        // Queue<PooledResource> = available resources waiting to be used
        private readonly Queue<PooledResource> _available = new Queue<PooledResource>();
        
        private int _activeCount = 0; // int = number of resources in use
        private bool _disposed = false; // bool = disposal flag

        public int ActiveCount => _activeCount; // Property for active count
        public int AvailableCount => _available.Count; // Property for available count

        public ResourcePool() // Constructor
        {
            // Pre-create some resources
            for (int i = 0; i < 5; i++) // int = loop counter
            {
                _available.Enqueue(new PooledResource()); // Create 5 resources
            }
        }

        public PooledResource Acquire() // Get resource from pool
        {
            if (_available.Count > 0) // Check if resources available
            {
                _activeCount++; // Increment active count
                return _available.Dequeue(); // Return available resource
            }
            
            // Create new if none available
            _activeCount++; // Increment active count
            return new PooledResource(); // Return new resource
        }

        public void Release(PooledResource resource) // Return resource to pool
        {
            if (resource == null) // Check if resource is valid
                throw new ArgumentNullException(nameof(resource)); // Throw if null
            
            resource.Reset(); // Reset resource state
            _available.Enqueue(resource); // Return to queue
            _activeCount--; // Decrement active count
        }

        public void Dispose() // IDisposable implementation
        {
            Dispose(disposing: true); // Call overload
            GC.SuppressFinalize(this); // Prevent finalization
        }

        protected virtual void Dispose(bool disposing) // Overload
        {
            if (!_disposed) // Check if disposed
            {
                if (disposing) // Check if from Dispose()
                {
                    // Clear all resources
                    while (_available.Count > 0) // While queue not empty
                    {
                        _available.Dequeue().Dispose(); // Dispose each resource
                    }
                }
                
                _disposed = true; // Mark as disposed
            }
        }

        ~ResourcePool() // Finalizer
        {
            Dispose(disposing: false); // Cleanup from finalizer
        }
    }

    /// <summary>
    /// Pooled resource that can be reused.
    /// </summary>
    class PooledResource : IDisposable
    {
        private bool _disposed = false; // bool = disposal flag

        public void Reset() // Reset state for reuse
        {
            // Reset all state to default values
        }

        public void Dispose() // IDisposable implementation
        {
            Dispose(disposing: true); // Call overload
            GC.SuppressFinalize(this); // Prevent finalization
        }

        protected virtual void Dispose(bool disposing) // Overload
        {
            if (!_disposed) // Check if disposed
            {
                if (disposing) // Check if from Dispose()
                {
                    // Release managed resources
                }
                
                // Release unmanaged resources
                
                _disposed = true; // Mark as disposed
            }
        }

        ~PooledResource() // Finalizer
        {
            Dispose(disposing: false); // Cleanup from finalizer
        }
    }
}