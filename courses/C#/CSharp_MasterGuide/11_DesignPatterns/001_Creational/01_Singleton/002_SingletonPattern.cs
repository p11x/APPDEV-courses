/*
 * ============================================================
 * TOPIC     : Design Patterns
 * SUBTOPIC  : Creational - Singleton Pattern
 * FILE      : 01_SingletonPattern.cs
 * PURPOSE   : Demonstrates the Singleton design pattern in C#
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._11_DesignPatterns._01_Creational._01_Singleton
{
    /// <summary>
    /// Demonstrates Singleton pattern - single instance guaranteed
    /// </summary>
    public class SingletonPattern
    {
        /// <summary>
        /// Entry point for Singleton pattern examples
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs to console
            // Output: === Singleton Pattern Demo ===
            Console.WriteLine("=== Singleton Pattern Demo ===\n");

            // ── CONCEPT: What is Singleton? ───────────────────────────────────
            // Ensures only one instance exists

            // Example 1: Basic Singleton
            // Output: 1. Basic Singleton:
            Console.WriteLine("1. Basic Singleton:");
            
            // GetInstance returns the single instance
            var config1 = ConfigurationManager.GetInstance();
            // Same instance returned
            var config2 = ConfigurationManager.GetInstance();
            
            // ReferenceEquals checks if same object
            // Output: Same instance: True
            Console.WriteLine($"   Same instance: {ReferenceEquals(config1, config2)}");

            // ── CONCEPT: Thread-Safe Singleton ───────────────────────────────
            // Safe for multi-threaded applications

            // Example 2: Thread-Safe Singleton
            // Output: 2. Thread-Safe Singleton:
            Console.WriteLine("\n2. Thread-Safe Singleton:");
            
            // Use lock for thread safety
            var threadSafe1 = ThreadSafeDatabase.GetInstance();
            var threadSafe2 = ThreadSafeDatabase.GetInstance();
            
            // Output: Thread-safe instance: True
            Console.WriteLine($"   Thread-safe instance: {ReferenceEquals(threadSafe1, threadSafe2)}");
            
            // Both reference same connection pool
            // Output: Connection pool: Shared
            Console.WriteLine($"   Connection pool: Shared");

            // ── CONCEPT: Lazy Initialization ──────────────────────────────────
            // Instance created only when needed

            // Example 3: Lazy Singleton
            // Output: 3. Lazy Initialization:
            Console.WriteLine("\n3. Lazy Initialization:");
            
            // Lazy<T> defers creation until first access
            var lazy1 = LazyLogger.Instance;
            var lazy2 = LazyLogger.Instance;
            
            // Output: Lazy instance: True
            Console.WriteLine($"   Lazy instance: {ReferenceEquals(lazy1, lazy2)}");
            
            // Logs are centralized
            // Output: Logged to single file
            Console.WriteLine($"   Logged to single file");

            // ── REAL-WORLD EXAMPLE: Configuration Manager ───────────────────
            // Output: --- Real-World: Configuration Manager ---
            Console.WriteLine("\n--- Real-World: Configuration Manager ---");
            
            // Centralized configuration access
            var config = AppConfiguration.GetInstance();
            
            // Set configuration values
            config.Set("AppName", "MyApp"); // key-value pair
            config.Set("MaxConnections", "100"); // connection limit
            
            // Get from anywhere - same instance
            var sameConfig = AppConfiguration.GetInstance();
            // Output: AppName: MyApp, MaxConnections: 100
            Console.WriteLine($"   AppName: {sameConfig.Get("AppName")}, MaxConnections: {sameConfig.Get("MaxConnections")}");

            Console.WriteLine("\n=== Singleton Pattern Complete ===");
        }
    }

    /// <summary>
    /// Basic Singleton implementation
    /// Not thread-safe, suitable for single-threaded scenarios
    /// </summary>
    public class ConfigurationManager
    {
        // Static instance - created once
        private static ConfigurationManager _instance;
        
        // Private constructor prevents external creation
        private ConfigurationManager() { }
        
        /// <summary>
        /// Gets the singleton instance
        /// </summary>
        /// <returns>ConfigurationManager instance</returns>
        public static ConfigurationManager GetInstance()
        {
            // Create instance if not exists (not thread-safe)
            if (_instance == null)
            {
                _instance = new ConfigurationManager();
            }
            return _instance;
        }
    }

    /// <summary>
    /// Thread-safe Singleton using double-check locking
    /// </summary>
    public class ThreadSafeDatabase
    {
        // Static instance
        private static ThreadSafeDatabase _instance;
        // Lock object for synchronization
        private static readonly object _lock = new object();
        
        // Private constructor
        private ThreadSafeDatabase()
        {
            // Initialize connection pool
            Console.WriteLine("   Database connection pool initialized");
        }
        
        /// <summary>
        /// Gets thread-safe singleton instance
        /// </summary>
        /// <returns>ThreadSafeDatabase instance</returns>
        public static ThreadSafeDatabase GetInstance()
        {
            // Double-check locking pattern
            if (_instance == null) // first check (no lock)
            {
                lock (_lock) // acquire lock
                {
                    if (_instance == null) // second check (with lock)
                    {
                        _instance = new ThreadSafeDatabase();
                    }
                }
            }
            return _instance;
        }
    }

    /// <summary>
    /// Lazy-initialized Singleton using Lazy<T>
    /// Thread-safe by default
    /// </summary>
    public class LazyLogger
    {
        // Lazy<T> creates instance on first access
        private static readonly Lazy<LazyLogger> _instance = 
            new Lazy<LazyLogger>(() => new LazyLogger());
        
        private LazyLogger()
        {
            // Initialize logging system
            Console.WriteLine("   Logging system initialized");
        }
        
        /// <summary>
        /// Gets the singleton instance (lazy)
        /// </summary>
        /// <returns>LazyLogger instance</returns>
        public static LazyLogger Instance => _instance.Value;
        
        /// <summary>
        /// Logs a message
        /// </summary>
        /// <param name="message">Message to log</param>
        public void Log(string message)
        {
            // Log to centralized location
            Console.WriteLine($"   [LOG] {message}");
        }
    }

    /// <summary>
    /// Real-world configuration singleton
    /// </summary>
    public class AppConfiguration
    {
        // Thread-safe lazy initialization
        private static readonly Lazy<AppConfiguration> _instance = 
            new Lazy<AppConfiguration>(() => new AppConfiguration());
        
        // Dictionary stores key-value pairs
        private readonly Dictionary<string, string> _settings = 
            new Dictionary<string, string>();
        
        private AppConfiguration() { }
        
        /// <summary>
        /// Gets the configuration instance
        /// </summary>
        /// <returns>AppConfiguration instance</returns>
        public static AppConfiguration GetInstance() => _instance.Value;
        
        /// <summary>
        /// Sets a configuration value
        /// </summary>
        /// <param name="key">Configuration key</param>
        /// <param name="value">Configuration value</param>
        public void Set(string key, string value)
        {
            _settings[key] = value; // store key-value pair
        }
        
        /// <summary>
        /// Gets a configuration value
        /// </summary>
        /// <param name="key">Configuration key</param>
        /// <returns>Configuration value</returns>
        public string Get(string key)
        {
            return _settings.TryGetValue(key, out var value) ? value : null;
        }
    }
}