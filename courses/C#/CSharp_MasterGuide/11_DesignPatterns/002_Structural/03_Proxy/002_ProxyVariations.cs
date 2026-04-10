/*
 * ============================================================
 * TOPIC     : Design Patterns
 * SUBTOPIC  : Structural - Proxy Variations
 * FILE      : 02_ProxyVariations.cs
 * PURPOSE   : Demonstrates different Proxy pattern approaches
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._11_DesignPatterns._02_Structural._03_Proxy
{
    /// <summary>
    /// Demonstrates Proxy variations
    /// </summary>
    public class ProxyVariations
    {
        /// <summary>
        /// Entry point for Proxy variations
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs to console
            // Output: === Proxy Variations ===
            Console.WriteLine("=== Proxy Variations ===\n");

            // ── CONCEPT: Remote Proxy ─────────────────────────────────────────
            // Represents object in different address space

            // Example 1: Remote Proxy
            // Output: 1. Remote Proxy:
            Console.WriteLine("1. Remote Proxy:");
            
            // Client thinks it's local but proxy handles remote communication
            var remoteService = new RemoteServiceProxy();
            var result = remoteService.GetData();
            // Output: Remote call executed, Response: data

            // ── CONCEPT: Smart Reference ─────────────────────────────────────
            // Adds actions when object is accessed

            // Example 2: Smart Reference
            // Output: 2. Smart Reference:
            Console.WriteLine("\n2. Smart Reference:");
            
            // Proxy tracks object usage
            var smartObject = new SmartObjectProxy();
            smartObject.DoSomething();
            smartObject.DoSomething();
            smartObject.DoSomething();
            // Output: Object used 3 times

            // ── CONCEPT: Synchronization Proxy ───────────────────────────────
            // Ensures thread-safe access

            // Example 3: Synchronization Proxy
            // Output: 3. Synchronization Proxy:
            Console.WriteLine("\n3. Synchronization Proxy:");
            
            // Thread-safe access to shared resource
            var threadSafe = new ThreadSafeServiceProxy();
            // Would synchronize access in multi-threaded scenario
            threadSafe.Process("task1");
            // Output: Thread-safe: task1

            // ── REAL-WORLD EXAMPLE: Lazy Loading ────────────────────────────
            // Output: --- Real-World: Lazy Loading ---
            Console.WriteLine("\n--- Real-World: Lazy Loading ---");
            
            // Large collection not loaded until accessed
            var largeData = new LargeDataSetProxy();
            
            // First access triggers loading
            var item = largeData.GetItem(0);
            // Output: Loading dataset... (expensive)
            // Output: Item[0]: Data
            
            // Subsequent access uses cached
            var item2 = largeData.GetItem(5);
            // Output: Item[5]: Data

            Console.WriteLine("\n=== Proxy Variations Complete ===");
        }
    }

    /// <summary>
    /// Remote service interface
    /// </summary>
    public interface IRemoteService
    {
        string GetData(); // method: gets remote data
    }

    /// <summary>
    /// Remote service proxy
    /// </summary>
    public class RemoteServiceProxy : IRemoteService
    {
        public string GetData()
        {
            // Simulate network call
            Console.WriteLine("   Remote call executed");
            return "Response: data";
        }
    }

    /// <summary>
    /// Smart object interface
    /// </summary>
    public interface ISmartObject
    {
        void DoSomething(); // method: performs action
    }

    /// <summary>
    /// Smart reference proxy
    /// </summary>
    public class SmartObjectProxy : ISmartObject
    {
        private RealObject _realObject;
        private int _accessCount;
        
        public void DoSomething()
        {
            // Track access
            _accessCount++;
            
            if (_realObject == null)
            {
                _realObject = new RealObject();
            }
            
            _realObject.DoSomething();
            Console.WriteLine($"   Object used {_accessCount} times");
        }
    }

    /// <summary>
    /// Real object
    /// </summary>
    public class RealObject : ISmartObject
    {
        public void DoSomething()
        {
            Console.WriteLine("   Doing something...");
        }
    }

    /// <summary>
    /// Thread-safe service interface
    /// </summary>
    public interface IThreadSafeService
    {
        void Process(string task); // method: processes task
    }

    /// <summary>
    /// Thread-safe service proxy
    /// </summary>
    public class ThreadSafeServiceProxy : IThreadSafeService
    {
        private readonly object _lock = new object();
        private RealThreadSafeService _realService;
        
        public void Process(string task)
        {
            lock (_lock) // ensure thread safety
            {
                if (_realService == null)
                {
                    _realService = new RealThreadSafeService();
                }
                Console.WriteLine($"   Thread-safe: {task}");
                _realService.Process(task);
            }
        }
    }

    /// <summary>
    /// Real thread-safe service
    /// </summary>
    public class RealThreadSafeService : IThreadSafeService
    {
        public void Process(string task)
        {
            // Actual processing
        }
    }

    /// <summary>
    /// Large dataset interface
    /// </summary>
    public interface ILargeDataSet
    {
        object GetItem(int index); // method: gets item by index
    }

    /// <summary>
    /// Large dataset proxy with lazy loading
    /// </summary>
    public class LargeDataSetProxy : ILargeDataSet
    {
        private RealLargeDataSet _realDataSet;
        private bool _isLoaded;
        
        public object GetItem(int index)
        {
            // Lazy load on first access
            if (!_isLoaded)
            {
                Console.WriteLine("   Loading dataset... (expensive)");
                _realDataSet = new RealLargeDataSet();
                _isLoaded = true;
            }
            
            return _realDataSet.GetItem(index);
        }
    }

    /// <summary>
    /// Real large dataset
    /// </summary>
    public class RealLargeDataSet : ILargeDataSet
    {
        public object GetItem(int index)
        {
            return $"Item[{index}]: Data";
        }
    }
}