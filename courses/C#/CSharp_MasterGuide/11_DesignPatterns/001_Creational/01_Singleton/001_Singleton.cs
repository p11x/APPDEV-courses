/*
 * ============================================================
 * TOPIC     : Design Patterns
 * SUBTOPIC  : Creational - Singleton Deep Dive
 * FILE      : 01_Singleton.cs
 * PURPOSE   : Deep dive into Singleton pattern variations and implementations
 * ============================================================
 */
using System;
using System.Threading;

namespace CSharp_MasterGuide._11_DesignPatterns._01_Creational._01_Singleton
{
    /// <summary>
    /// Demonstrates various Singleton implementations in C#
    /// </summary>
    public class SingletonDeepDive
    {
        /// <summary>
        /// Entry point for Singleton examples
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs to console
            // Output: === Singleton Deep Dive ===
            Console.WriteLine("=== Singleton Deep Dive ===\n");

            // ── CONCEPT: Eager vs Lazy Initialization ─────────────────────────────
            // Eager: instance created at class load
            // Lazy: instance created on first access

            // Example 1: Eager Initialization
            // Output: 1. Eager Initialization:
            Console.WriteLine("1. Eager Initialization:");
            
            // Instance already created when type is first accessed
            var eager1 = EagerSingleton.Instance;
            var eager2 = EagerSingleton.Instance;
            
            // ReferenceEquals checks object identity
            // Output: Same instance: True
            Console.WriteLine($"   Same instance: {ReferenceEquals(eager1, eager2)}");
            
            // Id property returns unique identifier
            // Output: Instance ID: 1
            Console.WriteLine($"   Instance ID: {eager1.Id}");

            // Example 2: Lazy Initialization
            // Output: 2. Lazy Initialization:
            Console.WriteLine("\n2. Lazy Initialization:");
            
            // Lazy<T> defers creation until .Value is accessed
            var lazy1 = LazySingleton.Instance;
            var lazy2 = LazySingleton.Instance;
            
            // Output: Same instance: True
            Console.WriteLine($"   Same instance: {ReferenceEquals(lazy1, lazy2)}");
            
            // Output: Lazy loaded: True
            Console.WriteLine($"   Lazy loaded: {LazySingleton.IsLoaded}");

            // ── CONCEPT: Thread-Safe Implementations ─────────────────────────────
            // Multiple ways to ensure thread safety

            // Example 3: Lock-based Singleton
            // Output: 3. Lock-based Singleton:
            Console.WriteLine("\n3. Lock-based Singleton:");
            
            // GetInstance uses lock for thread safety
            var lock1 = LockSingleton.GetInstance();
            var lock2 = LockSingleton.GetInstance();
            
            // Output: Thread-safe: True
            Console.WriteLine($"   Thread-safe: {ReferenceEquals(lock1, lock2)}");

            // Example 4: Double-Check Locking
            // Output: 4. Double-Check Locking:
            Console.WriteLine("\n4. Double-Check Locking:");
            
            // Checks instance twice - before and after lock
            var dcl1 = DoubleCheckSingleton.GetInstance();
            var dcl2 = DoubleCheckSingleton.GetInstance();
            
            // Output: DCL instance: True
            Console.WriteLine($"   DCL instance: {ReferenceEquals(dcl1, dcl2)}");

            // ── CONCEPT: Singleton with Parameter ───────────────────────────────
            // Sometimes singletons need configuration

            // Example 5: Parametrized Singleton
            // Output: 5. Parametrized Singleton:
            Console.WriteLine("\n5. Parametrized Singleton:");
            
            // Initialize with configuration
            var config = ConfigurableSingleton.GetInstance("Production");
            
            // Output: Environment: Production
            Console.WriteLine($"   Environment: {config.Environment}");
            
            // Same instance returned - second call ignored
            var sameConfig = ConfigurableSingleton.GetInstance("Development");
            
            // Output: Still Production: True (first config retained)
            Console.WriteLine($"   Still Production: {sameConfig.Environment == "Production"}");

            // ── REAL-WORLD EXAMPLE: Connection Pool ───────────────────────────
            // Output: --- Real-World: Database Connection Pool ---
            Console.WriteLine("\n--- Real-World: Database Connection Pool ---");
            
            // Get shared connection pool instance
            var pool = ConnectionPool.GetInstance();
            
            // Acquire connection from pool
            var conn1 = pool.AcquireConnection();
            var conn2 = pool.AcquireConnection();
            
            // Output: Active connections: 2
            Console.WriteLine($"   Active connections: {pool.ActiveCount}");
            
            // Release connections back to pool
            pool.ReleaseConnection(conn1);
            pool.ReleaseConnection(conn2);
            
            // Output: Released: 2
            Console.WriteLine($"   Released: {pool.ActiveCount}");

            Console.WriteLine("\n=== Singleton Deep Dive Complete ===");
        }
    }

    /// <summary>
    /// Eager initialization - instance created at class load
    /// Simple but may waste resources if never used
    /// </summary>
    public class EagerSingleton
    {
        // Static readonly field - initialized once at type load
        private static readonly EagerSingleton _instance = new EagerSingleton();
        
        // Instance counter
        public int Id { get; } = 1;
        
        // Private constructor prevents external instantiation
        private EagerSingleton()
        {
            // Initialize resources
            Console.WriteLine("   EagerSingleton initialized");
        }
        
        /// <summary>
        /// Gets the singleton instance (eager)
        /// </summary>
        /// <returns>EagerSingleton instance</returns>
        public static EagerSingleton Instance => _instance;
    }

    /// <summary>
    /// Lazy initialization using Lazy<T>
    /// Thread-safe by default, instance created on first access
    /// </summary>
    public class LazySingleton
    {
        // Lazy<T> defers creation until first .Value access
        private static readonly Lazy<LazySingleton> _instance = 
            new Lazy<LazySingleton>(() => new LazySingleton());
        
        // Tracks if instance has been created
        public static bool IsLoaded => _instance.IsValueCreated;
        
        private LazySingleton()
        {
            Console.WriteLine("   LazySingleton initialized");
        }
        
        /// <summary>
        /// Gets the lazy singleton instance
        /// </summary>
        /// <returns>LazySingleton instance</returns>
        public static LazySingleton Instance => _instance.Value;
    }

    /// <summary>
    /// Thread-safe using lock - simple but may have performance impact
    /// </summary>
    public class LockSingleton
    {
        // Lock object for synchronization
        private static readonly object _lock = new object();
        private static LockSingleton _instance;
        
        private LockSingleton()
        {
            Console.WriteLine("   LockSingleton initialized");
        }
        
        /// <summary>
        /// Gets instance with lock - thread-safe but slower
        /// </summary>
        /// <returns>LockSingleton instance</returns>
        public static LockSingleton GetInstance()
        {
            // lock statement ensures only one thread enters at a time
            lock (_lock)
            {
                // Create instance if not exists
                if (_instance == null)
                {
                    _instance = new LockSingleton();
                }
            }
            return _instance;
        }
    }

    /// <summary>
    /// Double-check locking - optimized thread safety
    /// Checks twice: before and after acquiring lock
    /// </summary>
    public class DoubleCheckSingleton
    {
        // Lock object (readonly for thread safety)
        private static readonly object _lock = new object();
        private static DoubleCheckSingleton _instance;
        
        private DoubleCheckSingleton()
        {
            Console.WriteLine("   DoubleCheckSingleton initialized");
        }
        
        /// <summary>
        /// Gets instance using double-check locking pattern
        /// </summary>
        /// <returns>DoubleCheckSingleton instance</returns>
        public static DoubleCheckSingleton GetInstance()
        {
            // First check without lock (fast path)
            if (_instance == null)
            {
                // Acquire lock only if needed
                lock (_lock)
                {
                    // Second check with lock (ensures thread safety)
                    if (_instance == null)
                    {
                        _instance = new DoubleCheckSingleton();
                    }
                }
            }
            return _instance;
        }
    }

    /// <summary>
    /// Singleton that accepts configuration on first call
    /// Configuration persists for application lifetime
    /// </summary>
    public class ConfigurableSingleton
    {
        // Lazy initialization for thread safety
        private static readonly Lazy<ConfigurableSingleton> _instance = 
            new Lazy<ConfigurableSingleton>(() => new ConfigurableSingleton());
        
        // Configuration stored in properties
        public string Environment { get; private set; }
        
        // Tracks if configuration has been set
        private bool _initialized;
        
        private ConfigurableSingleton()
        {
            Console.WriteLine("   ConfigurableSingleton initialized");
        }
        
        /// <summary>
        /// Gets instance - first call sets config, subsequent calls ignored
        /// </summary>
        /// <param name="environment">Environment name (first call only)</param>
        /// <returns>ConfigurableSingleton instance</returns>
        public static ConfigurableSingleton GetInstance(string environment = "Development")
        {
            var instance = _instance.Value;
            
            // Set configuration only on first call
            if (!instance._initialized)
            {
                instance.Environment = environment;
                instance._initialized = true;
            }
            
            return instance;
        }
    }

    /// <summary>
    /// Real-world connection pool singleton
    /// Manages database connections for the application
    /// </summary>
    public class ConnectionPool
    {
        // Thread-safe lazy singleton
        private static readonly Lazy<ConnectionPool> _instance = 
            new Lazy<ConnectionPool>(() => new ConnectionPool());
        
        // List of available connections
        private readonly List<string> _availableConnections = new List<string>();
        
        // List of active connections in use
        private readonly List<string> _activeConnections = new List<string>();
        
        // Lock for thread-safe access
        private readonly object _lock = new object();
        
        private ConnectionPool()
        {
            // Pre-create some connections
            for (int i = 0; i < 5; i++)
            {
                _availableConnections.Add($"Connection-{i}");
            }
            Console.WriteLine("   ConnectionPool initialized with 5 connections");
        }
        
        /// <summary>
        /// Gets the connection pool instance
        /// </summary>
        /// <returns>ConnectionPool instance</returns>
        public static ConnectionPool GetInstance() => _instance.Value;
        
        /// <summary>
        /// Acquires a connection from the pool
        /// </summary>
        /// <returns>Connection identifier</returns>
        public string AcquireConnection()
        {
            lock (_lock)
            {
                // Take from available pool
                if (_availableConnections.Count > 0)
                {
                    var conn = _availableConnections[0];
                    _availableConnections.RemoveAt(0);
                    _activeConnections.Add(conn);
                    return conn;
                }
                
                // Create new if pool exhausted
                var newConn = $"Connection-{_activeConnections.Count + _availableConnections.Count}";
                _activeConnections.Add(newConn);
                return newConn;
            }
        }
        
        /// <summary>
        /// Releases connection back to pool
        /// </summary>
        /// <param name="connection">Connection to release</param>
        public void ReleaseConnection(string connection)
        {
            lock (_lock)
            {
                if (_activeConnections.Remove(connection))
                {
                    _availableConnections.Add(connection);
                }
            }
        }
        
        /// <summary>
        /// Gets count of active connections
        /// </summary>
        public int ActiveCount => _activeConnections.Count;
    }
}