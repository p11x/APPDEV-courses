/*
 * ============================================================
 * TOPIC     : Memory Management
 * SUBTOPIC  : IDisposable Pattern - Part 2
 * FILE      : 02_IDisposable_Pattern_Part2.cs
 * PURPOSE   : Advanced dispose patterns including subclassing,
 *            sealed classes, and custom scenarios
 * ============================================================
 */

using System; // System namespace for Console, basic types
using System.IO; // For stream types

namespace CSharp_MasterGuide._08_MemoryManagement._02_IDisposable
{
    /// <summary>
    /// Demonstrates advanced IDisposable patterns including
    /// subclassing hierarchies and sealed class implementations.
    /// </summary>
    class IDisposable_Pattern_Part2
    {
        static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════
            // CONCEPT: Subclassing with IDisposable ─────────────────
            // ═══════════════════════════════════════════════════════════
            // When base class implements IDisposable and derived
            // classes need their own resources, override Dispose(bool).
            //
            // Key points:
            // - Base class must have protected virtual Dispose(bool)
            // - Override Dispose(bool) in derived classes
            // - Call base.Dispose(disposing) at end

            Console.WriteLine("=== IDisposable Pattern Part 2 ===\n");

            // ── EXAMPLE 1: Subclassing IDisposable ────────────────────
            // Base class with virtual dispose, derived class overrides.

            Console.WriteLine("1. Subclassing IDisposable:");

            // Create derived class instance
            using (var derived = new DerivedFileHandler("data.txt")) // using = auto-dispose
            {
                derived.Read(); // Call derived method
                Console.WriteLine($"   Read completed"); // Output: Read completed
            } // Dispose called, triggers both base and derived cleanup

            // ── EXAMPLE 2: Sealed Class Implementation ──────────────
            // Sealed classes can't be subclassed, use simpler pattern.

            Console.WriteLine("\n2. Sealed class implementation:");

            // SealedDatabaseConnection is sealed, uses simpler pattern
            using (var db = new SealedDatabaseConnection("Server=localhost")) // using = auto-dispose
            {
                db.Connect(); // Connect to database
                Console.WriteLine($"   Connected: {db.IsConnected}"); // Output: Connected: True
            } // Dispose called

            // ── EXAMPLE 3: Multiple Inheritance Levels ────────────────
            // Three-level hierarchy shows proper chaining.

            Console.WriteLine("\n3. Multiple inheritance levels:");

            // Grandchild class has resources at all levels
            using (var grandchild = new SpecializedProcessor("input.bin")) // using = auto-dispose
            {
                grandchild.Process(); // Process data
                Console.WriteLine($"   Processed successfully"); // Output: Processed successfully
            } // Dispose chains: grandchild -> child -> base

            // ── EXAMPLE 4: Composition over Inheritance ───────────────────
            // Class wraps other IDisposable resources.

            Console.WriteLine("\n4. Composition pattern:");

            // CompositeManager wraps multiple IDisposable objects
            using (var manager = new CompositeManager()) // using = auto-dispose
            {
                manager.Initialize(); // Initialize all resources
                Console.WriteLine($"   Manager initialized"); // Output: Manager initialized
            } // All wrapped resources disposed automatically

            // ── EXAMPLE 5: Interface with IDisposable ──────────────
            // Class implements both IDisposable and another interface.

            Console.WriteLine("\n5. Interface + IDisposable:");

            // DataRepository implements IRepository and IDisposable
            using (var repo = new DataRepository("connection")) // using = auto-dispose
            {
                var data = repo.GetData(); // Get data via interface
                Console.WriteLine($"   Got {data.Length} items"); // Output: Got [n] items
            } // Dispose called

            // ── REAL-WORLD EXAMPLE: Base class for Data Access ────────
            Console.WriteLine("\n6. Data access hierarchy:");

            // BaseSqlDataAccess -> DerivedCustomerDataAccess
            using (var dal = new CustomerDataAccess("Server=prod")) // using = auto-dispose
            {
                var customers = dal.GetAll(); // Get all customers
                Console.WriteLine($"   Loaded {customers.Count} customers"); // Output: Loaded [n] customers
            } // Dispose chains properly

            Console.WriteLine("\n=== IDisposable Pattern Part 2 Complete ===");
        }
    }

    /// <summary>
    /// Base class for file handling with IDisposable.
    /// Provides virtual Dispose(bool) for derived classes.
    /// </summary>
    class BaseFileHandler : IDisposable
    {
        // Protected so derived classes can access
        protected FileStream _fileStream = null; // FileStream = file handle
        protected string _filePath = string.Empty; // string = file path
        protected bool _disposed = false; // bool = disposal flag

        public BaseFileHandler(string filePath) // Constructor
        {
            _filePath = filePath; // Set file path
            // Base opens file in constructor
            _fileStream = new FileStream(filePath, FileMode.Create); // Create file
            Console.WriteLine($"   Base: Opened file {_filePath}"); // Output: Base: Opened file [path]
        }

        /// <summary>
        /// Public Dispose method - implements IDisposable.
        /// </summary>
        public void Dispose() // IDisposable implementation
        {
            Dispose(disposing: true); // Call overload
            GC.SuppressFinalize(this); // Prevent finalization
        }

        /// <summary>
        /// Protected virtual Dispose(bool) for overridden cleanup.
        /// </summary>
        /// <param name="disposing">true if from Dispose(), false from finalizer</param>
        protected virtual void Dispose(bool disposing) // Virtual for override
        {
            if (!_disposed) // Check if already disposed
            {
                if (disposing) // Check if from Dispose()
                {
                    // Release managed resources
                    if (_fileStream != null) // Check if stream exists
                    {
                        _fileStream.Dispose(); // Dispose stream
                        _fileStream = null; // Clear reference
                        Console.WriteLine("   Base: FileStream disposed"); // Output: Base: FileStream disposed
                    }
                }

                // Release unmanaged resources (none here)

                _disposed = true; // Mark disposed
            }
        }

        /// <summary>
        /// Finalizer as safety net.
        /// </summary>
        ~BaseFileHandler() // Finalizer
        {
            Dispose(disposing: false); // Cleanup from finalizer
        }
    }

    /// <summary>
    /// Derived class that adds its own resources.
    /// Must override Dispose(bool) to clean its own resources.
    /// </summary>
    class DerivedFileHandler : BaseFileHandler
    {
        // Derived adds its own resource
        private MemoryStream _buffer = null; // MemoryStream = additional buffer
        private bool _derivedDisposed = false; // bool = derived disposal flag

        public DerivedFileHandler(string filePath) : base(filePath) // Call base
        {
            // Initialize derived resources
            _buffer = new MemoryStream(); // Create buffer
            Console.WriteLine("   Derived: Buffer created"); // Output: Derived: Buffer created
        }

        public void Read() // Read from file
        {
            if (_disposed || _derivedDisposed) // Check if disposed
                throw new ObjectDisposedException(nameof(DerivedFileHandler)); // Throw if disposed

            // Read file content into buffer
            if (_fileStream != null && _buffer != null) // Check resources exist
            {
                _fileStream.CopyTo(_buffer); // Copy to buffer
                Console.WriteLine($"   Derived: Read {_buffer.Length} bytes"); // Output: Derived: Read [n] bytes
            }
        }

        /// <summary>
        /// Override Dispose(bool) to clean derived resources.
        /// MUST call base.Dispose(disposing) at the end.
        /// </summary>
        /// <param name="disposing">true if from Dispose(), false from finalizer</param>
        protected override void Dispose(bool disposing) // Override for derived cleanup
        {
            if (!_derivedDisposed) // Check if derived already disposed
            {
                if (disposing) // Check if from Dispose()
                {
                    // Clean derived resources FIRST (LIFO order optional)
                    if (_buffer != null) // Check if buffer exists
                    {
                        _buffer.Dispose(); // Dispose buffer
                        _buffer = null; // Clear reference
                        Console.WriteLine("   Derived: Buffer disposed"); // Output: Derived: Buffer disposed
                    }
                }

                _derivedDisposed = true; // Mark derived disposed
            }

            // ALWAYS call base last to ensure proper cleanup order
            base.Dispose(disposing); // Call base Dispose
        }

        /// <summary>
        /// Finalizer.
        /// </summary>
        ~DerivedFileHandler() // Finalizer
        {
            Dispose(disposing: false); // Cleanup from finalizer
        }
    }

    /// <summary>
    /// Sealed class with simpler dispose pattern.
    /// Since sealed can't be inherited, no virtual Dispose needed.
    /// </summary>
    sealed class SealedDatabaseConnection : IDisposable
    {
        private readonly string _connectionString; // string = DB connection string
        private bool _isConnected = false; // bool = connection state
        private bool _disposed = false; // bool = disposal flag

        public bool IsConnected => _isConnected; // Property getter

        public SealedDatabaseConnection(string connectionString) // Constructor
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            // ?? throw = null coalescing check
        }

        public void Connect() // Connect to database
        {
            if (_disposed) // Check if disposed
                throw new ObjectDisposedException(nameof(SealedDatabaseConnection)); // Throw if disposed

            // Simulate connection (in real code: actual DB connection)
            _isConnected = true; // Set connected flag
            Console.WriteLine($"   Sealed: Connected to {_connectionString}"); // Output: Sealed: Connected to [connection]
        }

        /// <summary>
        /// Simple Dispose implementation (no virtual needed).
        /// </summary>
        public void Dispose() // IDisposable implementation
        {
            if (!_disposed) // Check if already disposed
            {
                if (_isConnected) // Check if connected
                {
                    // Release resources
                    Disconnect(); // Disconnect
                }

                _disposed = true; // Mark disposed
            }

            GC.SuppressFinalize(this); // Prevent finalization
        }

        private void Disconnect() // Disconnect from database
        {
            _isConnected = false; // Set disconnected
            Console.WriteLine("   Sealed: Disconnected"); // Output: Sealed: Disconnected
        }

        // Note: No finalizer needed for sealed class in most cases
    }

    /// <summary>
    /// Three-level inheritance example: Base -> Child -> Specialized.
    /// </summary>
    class BaseResource : IDisposable
    {
        protected bool _disposed = false; // bool = disposal flag

        public BaseResource() // Constructor
        {
            Console.WriteLine("   Base: Initialize base resources"); // Output: Base: Initialize base resources
        }

        public void Dispose() // IDisposable
        {
            Dispose(disposing: true); // Call overload
            GC.SuppressFinalize(this); // Prevent finalization
        }

        protected virtual void Dispose(bool disposing) // Virtual for override
        {
            if (!_disposed) // Check if disposed
            {
                if (disposing) // From Dispose()
                {
                    // Base cleanup
                    Console.WriteLine("   Base: Cleanup base resources"); // Output: Base: Cleanup base resources
                }

                _disposed = true; // Mark disposed
            }
        }

        ~BaseResource() // Finalizer
        {
            Dispose(disposing: false); // Cleanup from finalizer
        }
    }

    class ChildResource : BaseResource
    {
        private bool _childDisposed = false; // bool = child disposal flag

        public ChildResource() // Constructor
        {
            Console.WriteLine("   Child: Initialize child resources"); // Output: Child: Initialize child resources
        }

        protected override void Dispose(bool disposing) // Override
        {
            if (!_childDisposed) // Check child disposed
            {
                if (disposing) // From Dispose()
                {
                    // Child cleanup
                    Console.WriteLine("   Child: Cleanup child resources"); // Output: Child: Cleanup child resources
                }

                _childDisposed = true; // Mark child disposed
            }

            base.Dispose(disposing); // Call base
        }

        ~ChildResource() // Finalizer
        {
            Dispose(disposing: false); // Cleanup from finalizer
        }
    }

    class SpecializedProcessor : ChildResource
    {
        private FileStream _file = null; // FileStream = specific resource
        private bool _specializedDisposed = false; // bool = specialized disposal

        public SpecializedProcessor(string path) // Constructor
        {
            _file = new FileStream(path, FileMode.Create); // Create file
            Console.WriteLine("   Specialized: Initialize file resource"); // Output: Specialized: Initialize file resource
        }

        public void Process() // Process method
        {
            if (_disposed || _specializedDisposed) // Check disposed
                throw new ObjectDisposedException(nameof(SpecializedProcessor)); // Throw if disposed

            // Process data
            Console.WriteLine("   Specialized: Processing data"); // Output: Specialized: Processing data
        }

        protected override void Dispose(bool disposing) // Override
        {
            if (!_specializedDisposed) // Check specialized disposed
            {
                if (disposing) // From Dispose()
                {
                    // Specialized cleanup
                    if (_file != null) // Check file exists
                    {
                        _file.Dispose(); // Dispose file
                        _file = null; // Clear reference
                        Console.WriteLine("   Specialized: File disposed"); // Output: Specialized: File disposed
                    }
                }

                _specializedDisposed = true; // Mark specialized disposed
            }

            base.Dispose(disposing); // Call base -> child -> base
        }

        ~SpecializedProcessor() // Finalizer
        {
            Dispose(disposing: false); // Cleanup from finalizer
        }
    }

    /// <summary>
    /// Composite pattern: Class wraps multiple IDisposable objects.
    /// </summary>
    class CompositeManager : IDisposable
    {
        // Composition: owns multiple IDisposable
        private DatabaseConnection _dbConnection = null; // DatabaseConnection = DB
        private FileProcessor _fileProcessor = null; // FileProcessor = file handler
        private CacheManager _cacheManager = null; // CacheManager = cache
        private bool _disposed = false; // bool = disposal flag

        public void Initialize() // Initialize all resources
        {
            _dbConnection = new DatabaseConnection("Server=localhost"); // Create DB
            _dbConnection.Open(); // Open connection

            _fileProcessor = new FileProcessor("temp.txt"); // Create processor
            _fileProcessor.Open(); // Open file

            _cacheManager = new CacheManager(); // Create cache

            Console.WriteLine("   Composite: All resources initialized"); // Output: Composite: All resources initialized
        }

        public void Dispose() // IDisposable implementation
        {
            if (!_disposed) // Check if disposed
            {
                // Dispose in reverse order of initialization (LIFO)
                if (_cacheManager != null) // Check if exists
                {
                    _cacheManager.Dispose(); // Dispose cache
                    _cacheManager = null; // Clear reference
                }

                if (_fileProcessor != null) // Check if exists
                {
                    _fileProcessor.Dispose(); // Dispose processor
                    _fileProcessor = null; // Clear reference
                }

                if (_dbConnection != null) // Check if exists
                {
                    _dbConnection.Dispose(); // Dispose connection
                    _dbConnection = null; // Clear reference
                }

                _disposed = true; // Mark disposed
            }

            GC.SuppressFinalize(this); // Prevent finalization
        }

        ~CompositeManager() // Finalizer
        {
            Dispose(disposing: false); // Cleanup from finalizer
        }
    }

    // Supporting classes for CompositeManager
    class DatabaseConnection : IDisposable
    {
        private readonly string _connString; // string = connection string
        private bool _isOpen = false; // bool = open flag

        public DatabaseConnection(string connString) // Constructor
        {
            _connString = connString; // Set connection string
        }

        public void Open() // Open connection
        {
            _isOpen = true; // Set open
            Console.WriteLine("   Composite: DB connected"); // Output: Composite: DB connected
        }

        public void Dispose() // IDisposable
        {
            if (_isOpen) // Check if open
            {
                _isOpen = false; // Set closed
                Console.WriteLine("   Composite: DB disconnected"); // Output: Composite: DB disconnected
            }

            GC.SuppressFinalize(this); // Prevent finalization
        }

        ~DatabaseConnection() // Finalizer
        {
            Dispose(); // Cleanup
        }
    }

    class FileProcessor : IDisposable
    {
        private FileStream _stream = null; // FileStream = file handle

        public FileProcessor(string path) // Constructor
        {
            _stream = new FileStream(path, FileMode.Create); // Create file
        }

        public void Open() // Open file
        {
            Console.WriteLine("   Composite: File opened"); // Output: Composite: File opened
        }

        public void Dispose() // IDisposable
        {
            if (_stream != null) // Check if exists
            {
                _stream.Dispose(); // Dispose stream
                _stream = null; // Clear reference
                Console.WriteLine("   Composite: File closed"); // Output: Composite: File closed
            }

            GC.SuppressFinalize(this); // Prevent finalization
        }

        ~FileProcessor() // Finalizer
        {
            Dispose(); // Cleanup
        }
    }

    class CacheManager : IDisposable
    {
        public CacheManager() // Constructor
        {
            Console.WriteLine("   Composite: Cache initialized"); // Output: Composite: Cache initialized
        }

        public void Dispose() // IDisposable
        {
            Console.WriteLine("   Composite: Cache cleared"); // Output: Composite: Cache cleared
            GC.SuppressFinalize(this); // Prevent finalization
        }

        ~CacheManager() // Finalizer
        {
            Dispose(); // Cleanup
        }
    }

    /// <summary>
    /// Interface example with IDisposable.
    /// </summary>
    interface IRepository
    {
        string[] GetData(); // Get data method
    }

    /// <summary>
    /// Class implements both IRepository and IDisposable.
    /// </summary>
    class DataRepository : IRepository, IDisposable
    {
        private readonly string _connectionString; // string = connection string
        private bool _disposed = false; // bool = disposal flag

        public DataRepository(string connectionString) // Constructor
        {
            _connectionString = connectionString; // Set connection string
        }

        public string[] GetData() // IRepository implementation
        {
            if (_disposed) // Check if disposed
                throw new ObjectDisposedException(nameof(DataRepository)); // Throw if disposed

            // Return sample data
            return new string[] { "Item1", "Item2", "Item3" }; // string[] = sample data
        }

        public void Dispose() // IDisposable implementation
        {
            if (!_disposed) // Check if disposed
            {
                // Clean up resources
                Console.WriteLine("   Repository: Connection closed"); // Output: Repository: Connection closed
                _disposed = true; // Mark disposed
            }

            GC.SuppressFinalize(this); // Prevent finalization
        }

        ~DataRepository() // Finalizer
        {
            Dispose(); // Cleanup
        }
    }

    /// <summary>
    /// Real-world: Base data access class.
    /// </summary>
    class BaseSqlDataAccess : IDisposable
    {
        protected readonly string _connectionString; // string = connection string
        protected bool _disposed = false; // bool = disposal flag
        protected SqlConnection _sqlConnection = null; // SqlConnection = wrapped connection

        public BaseSqlDataAccess(string connectionString) // Constructor
        {
            _connectionString = connectionString; // Set connection string
            _sqlConnection = new SqlConnection(connectionString); // Create connection
            _sqlConnection.Open(); // Open connection
            Console.WriteLine("   BaseSql: Connection opened"); // Output: BaseSql: Connection opened
        }

        protected virtual void Dispose(bool disposing) // Virtual for override
        {
            if (!_disposed) // Check if disposed
            {
                if (disposing) // From Dispose()
                {
                    if (_sqlConnection != null) // Check if exists
                    {
                        _sqlConnection.Close(); // Close connection
                        _sqlConnection = null; // Clear reference
                        Console.WriteLine("   BaseSql: Connection closed"); // Output: BaseSql: Connection closed
                    }
                }

                _disposed = true; // Mark disposed
            }
        }

        public void Dispose() // IDisposable
        {
            Dispose(disposing: true); // Call overload
            GC.SuppressFinalize(this); // Prevent finalization
        }

        ~BaseSqlDataAccess() // Finalizer
        {
            Dispose(disposing: false); // Cleanup
        }
    }

    /// <summary>
    /// Real-world: Derived customer data access.
    /// </summary>
    class CustomerDataAccess : BaseSqlDataAccess
    {
        private List<string> _customers = null; // List<string> = customer cache
        private bool _derivedDisposed = false; // bool = derived disposal

        public CustomerDataAccess(string connectionString) : base(connectionString) // Base call
        {
            _customers = new List<string>(); // Initialize list
        }

        public List<string> GetAll() // Get all customers
        {
            if (_disposed || _derivedDisposed) // Check disposed
                throw new ObjectDisposedException(nameof(CustomerDataAccess)); // Throw if disposed

            // Simulate loading customers
            _customers.Add("Customer1"); // Add customer
            _customers.Add("Customer2"); // Add customer
            _customers.Add("Customer3"); // Add customer
            Console.WriteLine("   CustomerDal: Customers loaded"); // Output: CustomerDal: Customers loaded

            return _customers; // Return customers
        }

        protected override void Dispose(bool disposing) // Override
        {
            if (!_derivedDisposed) // Check derived disposed
            {
                if (disposing) // From Dispose()
                {
                    if (_customers != null) // Check if exists
                    {
                        _customers.Clear(); // Clear list
                        _customers = null; // Clear reference
                        Console.WriteLine("   CustomerDal: Cache cleared"); // Output: CustomerDal: Cache cleared
                    }
                }

                _derivedDisposed = true; // Mark derived disposed
            }

            base.Dispose(disposing); // Call base
        }

        ~CustomerDataAccess() // Finalizer
        {
            Dispose(disposing: false); // Cleanup
        }
    }

    // Simple wrapper for SQL connection simulation
    class SqlConnection
    {
        private readonly string _connString; // string = connection string

        public SqlConnection(string connString) // Constructor
        {
            _connString = connString; // Set connection string
        }

        public void Open() // Open connection
        {
            // Simulate open
        }

        public void Close() // Close connection
        {
            // Simulate close
        }
    }

    // Simple list for demonstration
    class List<T>
    {
        private T[] _items = new T[0]; // T[] = internal array

        public void Add(T item) // Add item
        {
            // Simple add (not production code)
        }

        public int Count => _items.Length; // Property getter
    }
}