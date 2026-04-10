/*
 * ============================================================
 * TOPIC     : Design Patterns
 * SUBTOPIC  : Creational - Singleton Variations
 * FILE      : 02_SingletonVariations.cs
 * PURPOSE   : Demonstrates different Singleton implementation approaches
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._11_DesignPatterns._01_Creational._01_Singleton
{
    /// <summary>
    /// Demonstrates Singleton variations and alternatives
    /// </summary>
    public class SingletonVariations
    {
        /// <summary>
        /// Entry point for Singleton variations
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs to console
            // Output: === Singleton Variations ===
            Console.WriteLine("=== Singleton Variations ===\n");

            // ── CONCEPT: Static Field Singleton ───────────────────────────────
            // Simplest thread-safe approach

            // Example 1: Static Field
            // Output: 1. Static Field Singleton:
            Console.WriteLine("1. Static Field Singleton:");
            
            // Static constructor called once
            var static1 = StaticSingleton.Instance;
            var static2 = StaticSingleton.Instance;
            // Output: Same instance: True
            Console.WriteLine($"   Same instance: {ReferenceEquals(static1, static2)}");

            // ── CONCEPT: Nested Class Singleton ───────────────────────────────
            // Lazy initialization without lock

            // Example 2: Nested Class
            // Output: 2. Nested Class Singleton:
            Console.WriteLine("\n2. Nested Class Singleton:");
            
            var nested1 = NestedSingleton.Instance;
            var nested2 = NestedSingleton.Instance;
            // Output: Nested same: True
            Console.WriteLine($"   Nested same: {ReferenceEquals(nested1, nested2)}");

            // ── CONCEPT: Generic Singleton ───────────────────────────────────
            // Reusable singleton for any type

            // Example 3: Generic Singleton
            // Output: 3. Generic Singleton:
            Console.WriteLine("\n3. Generic Singleton:");
            
            // Create singleton for specific type
            var generic1 = SingletonProvider<DatabaseService>.Instance;
            var generic2 = SingletonProvider<DatabaseService>.Instance;
            // Output: Generic same: True
            Console.WriteLine($"   Generic same: {ReferenceEquals(generic1, generic2)}");
            
            // Different type = different instance
            var different = SingletonProvider<CacheService>.Instance;
            // Output: Different type: False
            Console.WriteLine($"   Different type: {ReferenceEquals(generic1, different)}");

            // ── CONCEPT: Dependency Injection Alternative ────────────────────
            // DI containers can provide singleton behavior

            // Example 4: DI Container Approach
            // Output: 4. DI Container Approach:
            Console.WriteLine("\n4. DI Container Approach:");
            
            // Simulate DI container
            var container = new SimpleDIContainer();
            
            // Register as singleton
            container.RegisterSingleton<IService, RealService>();
            
            // Resolve same instance
            var service1 = container.Resolve<IService>();
            var service2 = container.Resolve<IService>();
            // Output: DI same: True
            Console.WriteLine($"   DI same: {ReferenceEquals(service1, service2)}");

            // ── REAL-WORLD EXAMPLE: Service Locator ────────────────────────────
            // Output: --- Real-World: Service Locator ---
            Console.WriteLine("\n--- Real-World: Service Locator ---");
            
            // Centralized service access
            ServiceLocator.Register<ILogger>(new FileLogger());
            
            // Get logger from anywhere
            var logger = ServiceLocator.Get<ILogger>();
            logger.Log("Application started");
            // Output: Logged: Application started
            Console.WriteLine($"   Logged: Application started");

            Console.WriteLine("\n=== Singleton Variations Complete ===");
        }
    }

    /// <summary>
    /// Static field singleton - thread-safe by .NET guarantee
    /// </summary>
    public class StaticSingleton
    {
        // Static field initialized at type load
        public static readonly StaticSingleton Instance = new StaticSingleton();
        
        private StaticSingleton()
        {
            // Initialize on construction
            Console.WriteLine("   StaticSingleton created");
        }
    }

    /// <summary>
    /// Nested class for lazy initialization
    /// </summary>
    public class NestedSingleton
    {
        /// <summary>
        /// Nested class triggers initialization
        /// </summary>
        public static NestedSingleton Instance 
        { 
            get { return Nested.Instance; } 
        }
        
        private NestedSingleton() { }
        
        /// <summary>
        /// Inner class with static initializer
        /// </summary>
        private static class Nested
        {
            // Static initializer runs once, thread-safe
            internal static readonly NestedSingleton Instance = new NestedSingleton();
            
            static Nested()
            {
                Console.WriteLine("   NestedSingleton created");
            }
        }
    }

    /// <summary>
    /// Generic singleton provider
    /// </summary>
    /// <typeparam name="T">Type to singleton-ize</typeparam>
    public class SingletonProvider<T> where T : class
    {
        private static readonly Lazy<T> _instance = new Lazy<T>(
            () => CreateInstance());
        
        private static T CreateInstance()
        {
            // Use reflection to create instance
            return Activator.CreateInstance<T>();
        }
        
        /// <summary>
        /// Gets singleton instance of type T
        /// </summary>
        public static T Instance => _instance.Value;
    }

    /// <summary>
    /// Sample service classes for generic singleton
    /// </summary>
    public class DatabaseService { }
    public class CacheService { }

    /// <summary>
    /// Simple DI container
    /// </summary>
    public class SimpleDIContainer
    {
        // Stores registered types
        private readonly Dictionary<Type, object> _singletons = 
            new Dictionary<Type, object>();
        
        /// <summary>
        /// Registers singleton service
        /// </summary>
        public void RegisterSingleton<TInterface, TImplementation>() 
            where TImplementation : class, TInterface, new()
        {
            // Create single instance
            _singletons[typeof(TInterface)] = new TImplementation();
        }
        
        /// <summary>
        /// Resolves registered service
        /// </summary>
        public TInterface Resolve<TInterface>()
        {
            return (TInterface)_singletons[typeof(TInterface)];
        }
    }

    /// <summary>
    /// Service interface
    /// </summary>
    public interface IService { }

    /// <summary>
    /// Service implementation
    /// </summary>
    public class RealService : IService { }

    /// <summary>
    /// Simple service locator
    /// </summary>
    public static class ServiceLocator
    {
        private static readonly Dictionary<Type, object> _services = 
            new Dictionary<Type, object>();
        
        /// <summary>
        /// Registers a service
        /// </summary>
        public static void Register<T>(T service) where T : class
        {
            _services[typeof(T)] = service;
        }
        
        /// <summary>
        /// Gets registered service
        /// </summary>
        public static T Get<T>() where T : class
        {
            return (T)_services[typeof(T)];
        }
    }

    /// <summary>
    /// Logger interface
    /// </summary>
    public interface ILogger
    {
        void Log(string message);
    }

    /// <summary>
    /// File logger implementation
    /// </summary>
    public class FileLogger : ILogger
    {
        public void Log(string message)
        {
            // Log to file (simulated)
            Console.WriteLine($"   [FILE] {message}");
        }
    }
}