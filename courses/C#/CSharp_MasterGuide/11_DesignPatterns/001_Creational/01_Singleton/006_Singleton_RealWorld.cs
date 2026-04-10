/*
 * ============================================================
 * TOPIC     : Design Patterns
 * SUBTOPIC  : Creational - Singleton Real-World
 * FILE      : 03_Singleton_RealWorld.cs
 * PURPOSE   : Real-world Singleton pattern applications
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._11_DesignPatterns._01_Creational._01_Singleton
{
    /// <summary>
    /// Real-world Singleton pattern examples
    /// </summary>
    public class SingletonRealWorld
    {
        /// <summary>
        /// Entry point for real-world examples
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs to console
            // Output: === Singleton Real-World ===
            Console.WriteLine("=== Singleton Real-World ===\n");

            // ── REAL-WORLD 1: Cache Manager ───────────────────────────────────
            // Centralized caching system

            // Example 1: Cache Manager
            // Output: 1. Cache Manager:
            Console.WriteLine("1. Cache Manager:");
            
            // Get cache instance
            var cache1 = CacheManager.Instance;
            var cache2 = CacheManager.Instance;
            
            // Add items to cache
            cache1.Set("user:1", "John"); // key-value pair
            cache1.Set("user:2", "Jane"); // key-value pair
            
            // Retrieve from cache
            var user1 = cache2.Get("user:1");
            // Output: Cached user: John
            Console.WriteLine($"   Cached user: {user1}");
            
            // Output: Cache hits: 1
            Console.WriteLine($"   Cache hits: {cache1.HitCount}");

            // ── REAL-WORLD 2: Connection Pool ────────────────────────────────
            // Database connection management

            // Example 2: Connection Pool
            // Output: 2. Connection Pool:
            Console.WriteLine("\n2. Connection Pool:");
            
            var pool = ConnectionPool.Instance;
            
            // Get connection from pool
            var conn1 = pool.AcquireConnection();
            // Output: Acquired connection: Connection-1
            Console.WriteLine($"   Acquired connection: {conn1}");
            
            // Release back to pool
            pool.ReleaseConnection(conn1);
            // Output: Connection released
            Console.WriteLine($"   Connection released");
            
            // Pool maintains single set of connections
            // Output: Active connections: 0
            Console.WriteLine($"   Active connections: {pool.ActiveCount}");

            // ── REAL-WORLD 3: Event Aggregator ────────────────────────────────
            // Cross-component communication

            // Example 3: Event Aggregator
            // Output: 3. Event Aggregator:
            Console.WriteLine("\n3. Event Aggregator:");
            
            var aggregator = EventAggregator.Instance;
            
            // Subscribe to events
            aggregator.Subscribe<OrderPlacedEvent>(e => 
                Console.WriteLine($"   Order event: {e.OrderId}"));
            
            // Publish event
            aggregator.Publish(new OrderPlacedEvent { OrderId = 123 });
            // Output: Order event: 123

            // ── REAL-WORLD 4: Environment Configuration ─────────────────────
            // Application-wide settings

            // Example 4: Environment Config
            // Output: 4. Environment Config:
            Console.WriteLine("\n4. Environment Config:");
            
            var env = EnvironmentConfig.Instance;
            
            // Set environment
            env.SetEnvironment("Development");
            env.SetDebugMode(true);
            
            // Access anywhere
            // Output: Environment: Development, Debug: True
            Console.WriteLine($"   Environment: {env.GetEnvironment()}, Debug: {env.IsDebugMode()}");

            // ── REAL-WORLD 5: State Machine ───────────────────────────────────
            // Application state management

            // Example 5: Application State
            // Output: 5. Application State:
            Console.WriteLine("\n5. Application State:");
            
            var state = ApplicationState.Instance;
            
            // Transition states
            state.SetState(ApplicationState.StateEnum.Started);
            // Output: State: Started
            Console.WriteLine($"   State: {state.GetState()}");
            
            state.SetState(ApplicationState.StateEnum.Running);
            // Output: State: Running
            Console.WriteLine($"   State: {state.GetState()}");

            Console.WriteLine("\n=== Singleton Real-World Complete ===");
        }
    }

    /// <summary>
    /// Centralized cache manager
    /// </summary>
    public class CacheManager
    {
        private static readonly Lazy<CacheManager> _instance = 
            new Lazy<CacheManager>(() => new CacheManager());
        
        // In-memory cache storage
        private readonly Dictionary<string, object> _cache = 
            new Dictionary<string, object>();
        
        private int _hitCount; // tracks cache hits
        
        private CacheManager() { }
        
        /// <summary>
        /// Gets cache instance
        /// </summary>
        public static CacheManager Instance => _instance.Value;
        
        /// <summary>
        /// Gets cached value
        /// </summary>
        public string Get(string key)
        {
            if (_cache.TryGetValue(key, out var value))
            {
                _hitCount++; // increment hit count
                return value.ToString(); // return cached value
            }
            return null; // not found
        }
        
        /// <summary>
        /// Sets cache value
        /// </summary>
        public void Set(string key, string value)
        {
            _cache[key] = value; // store in cache
        }
        
        /// <summary>
        /// Number of cache hits
        /// </summary>
        public int HitCount => _hitCount;
    }

    /// <summary>
    /// Database connection pool
    /// </summary>
    public class ConnectionPool
    {
        private static readonly Lazy<ConnectionPool> _instance = 
            new Lazy<ConnectionPool>(() => new ConnectionPool());
        
        private readonly List<string> _available = new List<string>(); // available connections
        private readonly List<string> _inUse = new List<string>(); // in-use connections
        
        private ConnectionPool()
        {
            // Pre-create connections
            for (int i = 0; i < 10; i++)
            {
                _available.Add($"Connection-{i}");
            }
        }
        
        /// <summary>
        /// Gets pool instance
        /// </summary>
        public static ConnectionPool Instance => _instance.Value;
        
        /// <summary>
        /// Acquires a connection
        /// </summary>
        public string AcquireConnection()
        {
            if (_available.Count > 0) // if available connection exists
            {
                var conn = _available[0]; // get first available
                _available.RemoveAt(0); // remove from available
                _inUse.Add(conn); // add to in-use
                return conn; // return connection
            }
            return null; // no available connections
        }
        
        /// <summary>
        /// Releases connection back to pool
        /// </summary>
        public void ReleaseConnection(string connection)
        {
            _inUse.Remove(connection); // remove from in-use
            _available.Add(connection); // add back to available
        }
        
        /// <summary>
        /// Active connection count
        /// </summary>
        public int ActiveCount => _inUse.Count;
    }

    /// <summary>
    /// Event for order placement
    /// </summary>
    public class OrderPlacedEvent
    {
        public int OrderId { get; set; } // property: order identifier
    }

    /// <summary>
    /// Event aggregator for pub/sub
    /// </summary>
    public class EventAggregator
    {
        private static readonly Lazy<EventAggregator> _instance = 
            new Lazy<EventAggregator>(() => new EventAggregator());
        
        private readonly Dictionary<Type, List<Delegate>> _handlers = 
            new Dictionary<Type, List<Delegate>>();
        
        private EventAggregator() { }
        
        /// <summary>
        /// Gets aggregator instance
        /// </summary>
        public static EventAggregator Instance => _instance.Value;
        
        /// <summary>
        /// Subscribes to event type
        /// </summary>
        public void Subscribe<TEvent>(Action<TEvent> handler) where TEvent : class
        {
            var type = typeof(TEvent);
            if (!_handlers.ContainsKey(type))
            {
                _handlers[type] = new List<Delegate>();
            }
            _handlers[type].Add(handler);
        }
        
        /// <summary>
        /// Publishes event
        /// </summary>
        public void Publish<TEvent>(TEvent eventToPublish) where TEvent : class
        {
            var type = typeof(TEvent);
            if (_handlers.ContainsKey(type))
            {
                foreach (var handler in _handlers[type])
                {
                    ((Action<TEvent>)handler)(eventToPublish);
                }
            }
        }
    }

    /// <summary>
    /// Environment configuration singleton
    /// </summary>
    public class EnvironmentConfig
    {
        private static readonly Lazy<EnvironmentConfig> _instance = 
            new Lazy<EnvironmentConfig>(() => new EnvironmentConfig());
        
        private string _environment; // stores environment name
        private bool _debugMode; // stores debug flag
        
        private EnvironmentConfig() { }
        
        /// <summary>
        /// Gets config instance
        /// </summary>
        public static EnvironmentConfig Instance => _instance.Value;
        
        /// <summary>
        /// Sets environment
        /// </summary>
        public void SetEnvironment(string env) { _environment = env; }
        
        /// <summary>
        /// Gets environment
        /// </summary>
        public string GetEnvironment() => _environment;
        
        /// <summary>
        /// Sets debug mode
        /// </summary>
        public void SetDebugMode(bool mode) { _debugMode = mode; }
        
        /// <summary>
        /// Gets debug mode
        /// </summary>
        public bool IsDebugMode() => _debugMode;
    }

    /// <summary>
    /// Application state manager
    /// </summary>
    public class ApplicationState
    {
        private static readonly Lazy<ApplicationState> _instance = 
            new Lazy<ApplicationState>(() => new ApplicationState());
        
        public enum StateEnum { Started, Running, Paused, Stopped }
        
        private StateEnum _currentState = StateEnum.Stopped;
        
        private ApplicationState() { }
        
        /// <summary>
        /// Gets state instance
        /// </summary>
        public static ApplicationState Instance => _instance.Value;
        
        /// <summary>
        /// Sets current state
        /// </summary>
        public void SetState(StateEnum state) { _currentState = state; }
        
        /// <summary>
        /// Gets current state
        /// </summary>
        public StateEnum GetState() => _currentState;
    }
}