/*
 * ============================================================
 * TOPIC     : Design Patterns
 * SUBTOPIC  : Creational - Singleton Pattern (Part 2)
 * FILE      : 03_Singleton_Part2.cs
 * PURPOSE   : Continues singleton patterns with lazy initialization,
 *             thread-safe implementations, and advanced variations
 * ============================================================
 */

using System; // Core System namespace for Console
using System.Threading; // Threading namespace for locks

namespace CSharp_MasterGuide._11_DesignPatterns._01_Creational._01_Singleton
{
    /// <summary>
    /// Demonstrates advanced singleton patterns and thread-safety
    /// </summary>
    public class SingletonAdvancedDemo
    {
        /// <summary>
        /// Entry point for singleton advanced examples
        /// </summary>
        public static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════
            // SECTION 1: Lazy Initialization
            // ═══════════════════════════════════════════════════════════
            // Lazy<T> provides thread-safe lazy initialization
            // The instance is created only when first accessed

            Console.WriteLine("=== Singleton Advanced (Part 2) ===\n");

            // Output: --- Lazy Initialization ---
            Console.WriteLine("--- Lazy Initialization ---");

            // LazySingleton uses Lazy<T> for thread-safe lazy init
            var lazyInstance = LazySingleton.Instance;
            // Output: LazySingleton instance created
            Console.WriteLine($"  Instance created: {lazyInstance.GetHashCode()}");

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: Thread-Safe Double-Check Locking
            // ═══════════════════════════════════════════════════════════
            // Double-check locking reduces lock overhead
            // Check for null before acquiring lock (optimization)

            // Output: --- Double-Check Locking ---
            Console.WriteLine("\n--- Double-Check Locking ---");

            var dcl1 = DoubleCheckLock.Instance;
            var dcl2 = DoubleCheckLock.Instance;
            // Output: DoubleCheckLock instance created
            Console.WriteLine($"  Same instance: {dcl1 == dcl2}");
            // Output: Same instance: True

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: Static Initialization (Eager)
            // ═══════════════════════════════════════════════════════════
            // Static constructor runs before any static member access
            // .NET guarantees thread-safe static initialization

            // Output: --- Static Initialization ---
            Console.WriteLine("\n--- Static Initialization ---");

            var static1 = StaticSingleton.Instance;
            var static2 = StaticSingleton.Instance;
            // Output: StaticSingleton created at: [time]
            Console.WriteLine($"  Same instance: {static1 == static2}");
            // Output: Same instance: True

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: Thread-Safe with Interlocked
            // ═══════════════════════════════════════════════════════════
            // Interlocked provides atomic operations for simple cases
            // Faster than locks for simple read/write operations

            // Output: --- Interlocked Operations ---
            Console.WriteLine("\n--- Interlocked Operations ---");

            var inter1 = InterlockedSingleton.Instance;
            var inter2 = InterlockedSingleton.Instance;
            // Output: InterlockedSingleton created
            Console.WriteLine($"  Same instance: {inter1 == inter2}");
            // Output: Same instance: True

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: Generic Singleton
            // ═══════════════════════════════════════════════════════════
            // Generic singleton can be reused for any type
            // Uses Lazy<T> internally for thread-safety

            // Output: --- Generic Singleton ---
            Console.WriteLine("\n--- Generic Singleton ---");

            var generic1 = Singleton<DatabaseConnection>.Instance;
            var generic2 = Singleton<DatabaseConnection>.Instance;
            // Output: DatabaseConnection singleton created
            Console.WriteLine($"  Same instance: {generic1 == generic2}");
            // Output: Same instance: True

            var generic3 = Singleton<CacheManager>.Instance;
            var generic4 = Singleton<CacheManager>.Instance;
            // Output: CacheManager singleton created
            Console.WriteLine($"  Same instance: {generic3 == generic4}");
            // Output: Same instance: True

            // ═══════════════════════════════════════════════════════════
            // SECTION 6: Singleton with Parameters
            // ═══════════════════════════════════════════════════════════
            // Sometimes singleton needs configuration
            // Use a factory method or builder pattern

            // Output: --- Singleton with Parameters ---
            Console.WriteLine("\n--- Singleton with Parameters ---");

            var configured = ConfigurableSingleton.Instance("Production");
            // Output: Configured for: Production
            Console.WriteLine($"  Mode: {configured.Mode}");
            // Output: Mode: Production

            // ═══════════════════════════════════════════════════════════
            // SECTION 7: Breaking Singleton (Testing)
            // ═══════════════════════════════════════════════════════════
            // For testing, you may need to reset singleton
            // Use reflection or internal setter

            // Output: --- Testing Singleton ---
            Console.WriteLine("\n--- Testing Singleton ---");

            var testInstance = TestableSingleton.Instance;
            // Output: TestableSingleton created
            TestableSingleton.ResetForTesting();
            // Output: Singleton reset for testing
            var newInstance = TestableSingleton.Instance;
            // Output: TestableSingleton created (new instance)
            TestableSingleton.ResetForTesting();
            // Output: Singleton reset for testing

            // ═══════════════════════════════════════════════════════════
            // SECTION 8: Real-World: Configuration Manager
            // ═══════════════════════════════════════════════════════════
            // Common real-world use: app settings/configuration

            // Output: --- Real-World: Config Manager ---
            Console.WriteLine("\n--- Real-World: Config Manager ---");

            var config = AppConfig.Instance;
            // Output: Loading configuration...
            Console.WriteLine($"  Database: {config.Get("Database")}");
            // Output: Database: mydb
            Console.WriteLine($"  MaxConnections: {config.Get("MaxConnections")}");
            // Output: MaxConnections: 100

            var config2 = AppConfig.Instance;
            Console.WriteLine($"  Same config: {config == config2}");
            // Output: Same config: True

            Console.WriteLine("\n=== Singleton Advanced Complete ===");
        }
    }

    /// <summary>
    /// Lazy singleton using Lazy<T>
    /// Thread-safe by design with LazyThreadSafetyMode.ExecutionAndPublication
    /// </summary>
    public class LazySingleton
    {
        // Lazy<T> defers creation until first access
        // LazyThreadSafetyMode.ExecutionAndPublication ensures thread-safe creation
        private static readonly Lazy<LazySingleton> _instance = new Lazy<LazySingleton>(
            () => new LazySingleton(), 
            LazyThreadSafetyMode.ExecutionAndPublication
        );

        // Private constructor prevents external instantiation
        private LazySingleton()
        {
            Console.WriteLine("   LazySingleton instance created");
        }

        // Public static property to access instance
        public static LazySingleton Instance => _instance.Value;
    }

    /// <summary>
    /// Double-check locking pattern for thread-safety
    /// More performant than full lock on every access
    /// </summary>
    public class DoubleCheckLock
    {
        // Volatile ensures proper ordering of reads/writes
        private static volatile DoubleCheckLock _instance;
        private static readonly object _lock = new object();

        private DoubleCheckLock()
        {
            Console.WriteLine("   DoubleCheckLock instance created");
        }

        public static DoubleCheckLock Instance
        {
            // First check (no lock - fast path)
            if (_instance == null)
            {
                // Second check (with lock - ensures single instance)
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new DoubleCheckLock();
                    }
                }
            }
            return _instance;
        }
    }

    /// <summary>
    /// Static initialization - eager loading
    /// .NET static constructors are thread-safe
    /// </summary>
    public class StaticSingleton
    {
        // Static constructor runs once, before any static member access
        private static readonly StaticSingleton _instance = new StaticSingleton();

        public static StaticSingleton Instance => _instance;

        private StaticSingleton()
        {
            Console.WriteLine($"   StaticSingleton created at: {DateTime.Now.TimeOfDay}");
        }
    }

    /// <summary>
    /// Using Interlocked for atomic operations
    /// Simpler than locks for basic operations
    /// </summary>
    public class InterlockedSingleton
    {
        private static InterlockedSingleton _instance;

        public static InterlockedSingleton Instance
        {
            get
            {
                // CompareExchange returns current value if not null
                // If null, sets to new instance and returns null
                // If not null, returns existing instance
                var instance = _instance;
                if (instance == null)
                {
                    instance = new InterlockedSingleton();
                    Interlocked.CompareExchange(ref _instance, instance, null);
                }
                return _instance;
            }
        }

        private InterlockedSingleton()
        {
            Console.WriteLine("   InterlockedSingleton created");
        }
    }

    /// <summary>
    /// Generic singleton pattern - reusable for any type
    /// </summary>
    /// <typeparam name="T">Type to make singleton</typeparam>
    public class Singleton<T> where T : class
    {
        // Generic lazy initialization
        private static readonly Lazy<T> _instance = new Lazy<T>(() => 
        {
            // Activator creates instance using private constructor
            return (T)Activator.CreateInstance(typeof(T), true);
        });

        public static T Instance => _instance.Value;
    }

    /// <summary>
    /// Concrete classes for generic singleton demo
    /// </summary>
    public class DatabaseConnection
    {
        public DatabaseConnection()
        {
            Console.WriteLine("   DatabaseConnection singleton created");
        }
    }

    /// <summary>
    /// Another concrete class for generic singleton demo
    /// </summary>
    public class CacheManager
    {
        public CacheManager()
        {
            Console.WriteLine("   CacheManager singleton created");
        }
    }

    /// <summary>
    /// Singleton that can accept parameters
    /// Uses nested class for initialization control
    /// </summary>
    public class ConfigurableSingleton
    {
        private static ConfigurableSingleton _instance;
        private static readonly object _lock = new object();
        
        public string Mode { get; private set; }

        private ConfigurableSingleton(string mode)
        {
            Mode = mode;
            Console.WriteLine($"   Configured for: {mode}");
        }

        public static ConfigurableSingleton Instance(string mode)
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new ConfigurableSingleton(mode);
                    }
                }
            }
            return _instance;
        }
    }

    /// <summary>
    /// Testable singleton with reset capability
    /// Useful for unit testing
    /// </summary>
    public class TestableSingleton
    {
        private static TestableSingleton _instance;
        private static readonly object _lock = new object();

        public static void ResetForTesting()
        {
            lock (_lock)
            {
                _instance = null;
                Console.WriteLine("   Singleton reset for testing");
            }
        }

        public static TestableSingleton Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new TestableSingleton();
                            Console.WriteLine("   TestableSingleton created (new instance)");
                        }
                    }
                }
                return _instance;
            }
        }

        private TestableSingleton()
        {
            Console.WriteLine("   TestableSingleton created");
        }
    }

    /// <summary>
    /// Real-world: Application configuration manager
    /// Singleton ensures single source of truth for settings
    /// </summary>
    public class AppConfig
    {
        private static readonly Lazy<AppConfig> _instance = new Lazy<AppConfig>(
            () => new AppConfig()
        );

        private readonly Dictionary<string, string> _settings;

        public static AppConfig Instance => _instance.Value;

        private AppConfig()
        {
            _settings = new Dictionary<string, string>();
            LoadConfiguration();
        }

        private void LoadConfiguration()
        {
            Console.WriteLine("   Loading configuration...");
            // Simulated config loading
            _settings["Database"] = "mydb";
            _settings["MaxConnections"] = "100";
            _settings["Timeout"] = "30";
        }

        public string Get(string key)
        {
            return _settings.ContainsKey(key) ? _settings[key] : null;
        }
    }
}
