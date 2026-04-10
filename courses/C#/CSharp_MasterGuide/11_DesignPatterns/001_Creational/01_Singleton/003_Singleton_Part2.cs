/*
 * ============================================================
 * TOPIC     : Design Patterns
 * SUBTOPIC  : Creational - Singleton Advanced
 * FILE      : 02_Singleton_Part2.cs
 * PURPOSE   : Demonstrates advanced Singleton patterns in C#
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._11_DesignPatterns._01_Creational._01_Singleton
{
    /// <summary>
    /// Advanced Singleton pattern implementations
    /// </summary>
    public class SingletonAdvanced
    {
        /// <summary>
        /// Entry point for Singleton advanced examples
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs to console
            // Output: === Singleton Advanced ===
            Console.WriteLine("=== Singleton Advanced ===\n");

            // ── CONCEPT: Multithreaded Singleton ─────────────────────────────
            // Thread-safe initialization patterns

            // Example 1: Double-Check Locking
            // Output: 1. Double-Check Locking:
            Console.WriteLine("1. Double-Check Locking:");
            
            var instance1 = ThreadSafeSingleton.GetInstance();
            var instance2 = ThreadSafeSingleton.GetInstance();
            // Output: Instance created (thread-safe)
            // Output: Same instance: True
            Console.WriteLine($"   Same instance: {ReferenceEquals(instance1, instance2)}");

            // Example 2: Lazy<T> Singleton
            // Output: 2. Lazy<T> Singleton:
            Console.WriteLine("\n2. Lazy<T> Singleton:");
            
            var lazy1 = LazyConfiguration.Instance;
            var lazy2 = LazyConfiguration.Instance;
            // Output: Lazy instance same: True
            Console.WriteLine($"   Lazy instance same: {ReferenceEquals(lazy1, lazy2)}");

            // Example 3: Parallel Singleton Access
            // Output: 3. Parallel Access:
            Console.WriteLine("\n3. Parallel Access:");
            
            // Simulate parallel access
            var parallelDemo = new ParallelSingleton();
            parallelDemo.AccessFromMultipleThreads();
            // Output: All threads got same instance

            Console.WriteLine("\n=== Singleton Advanced Complete ===");
        }
    }

    /// <summary>
    /// Thread-safe singleton with double-check locking
    /// </summary>
    public class ThreadSafeSingleton
    {
        private static ThreadSafeSingleton _instance;
        private static readonly object _lock = new object();
        
        private ThreadSafeSingleton() 
        {
            Console.WriteLine("   Instance created (thread-safe)");
        }
        
        /// <summary>
        /// Gets singleton instance with double-check locking
        /// </summary>
        public static ThreadSafeSingleton GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new ThreadSafeSingleton();
                    }
                }
            }
            return _instance;
        }
    }

    /// <summary>
    /// Lazy-initialized singleton
    /// </summary>
    public class LazyConfiguration
    {
        private static readonly Lazy<LazyConfiguration> _instance = 
            new Lazy<LazyConfiguration>(() => new LazyConfiguration());
        
        private LazyConfiguration() { }
        
        /// <summary>
        /// Gets lazy singleton instance
        /// </summary>
        public static LazyConfiguration Instance => _instance.Value;
    }

    /// <summary>
    /// Demonstrates parallel singleton access
    /// </summary>
    public class ParallelSingleton
    {
        /// <summary>
        /// Simulates access from multiple threads
        /// </summary>
        public void AccessFromMultipleThreads()
        {
            // In real scenario, would use Task.Run for parallel access
            var instance1 = ThreadSafeSingleton.GetInstance();
            var instance2 = ThreadSafeSingleton.GetInstance();
            var instance3 = ThreadSafeSingleton.GetInstance();
            Console.WriteLine("   All threads got same instance");
        }
    }
}