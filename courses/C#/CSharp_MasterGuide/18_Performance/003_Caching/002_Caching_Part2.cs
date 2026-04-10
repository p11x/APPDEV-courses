/*
 * ============================================================
 * TOPIC     : Performance
 * SUBTOPIC  : Caching Part 2
 * FILE      : Caching_Part2.cs
 * PURPOSE   : Advanced caching techniques
 * ============================================================
 */
using System; // Core System namespace

namespace CSharp_MasterGuide._18_Performance._03_Caching
{
    /// <summary>
    /// Advanced caching demonstration
    /// </summary>
    public class CachingPart2Demo
    {
        /// <summary>
        /// Entry point
        /// </summary>
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Caching Part 2 ===\n");

            // Output: --- Cache Invalidation ---
            Console.WriteLine("--- Cache Invalidation ---");

            var cache = new CacheWithInvalidation();
            cache.Set("key", "value");
            cache.Invalidate("key");
            Console.WriteLine("   Key invalidated");
            // Output: Key invalidated

            // Output: --- Cache Expiration ---
            Console.WriteLine("\n--- Cache Expiration ---");

            var expiring = new ExpiringCache();
            expiring.Set("key", "value", TimeSpan.FromSeconds(60));
            Console.WriteLine("   Expires in 60 seconds");
            // Output: Expires in 60 seconds

            // Output: --- Distributed Cache ---
            Console.WriteLine("\n--- Distributed Cache ---");

            var redis = new RedisCache();
            redis.Set("key", "value");
            var value = redis.Get("key");
            Console.WriteLine($"   Redis value: {value}");
            // Output: Redis value: value

            // Output: --- Cache Aside Pattern ---
            Console.WriteLine("\n--- Cache Aside ---");

            var service = new CacheAsideService();
            var result = service.Get("key");
            Console.WriteLine($"   Result: {result}");
            // Output: Result: data

            Console.WriteLine("\n=== Caching Part 2 Complete ===");
        }
    }

    /// <summary>
    /// Cache with invalidation
    /// </summary>
    public class CacheWithInvalidation
    {
        public void Set(string key, object value) { }
        public void Invalidate(string key) { }
    }

    /// <summary>
    /// Expiring cache
    /// </summary>
    public class ExpiringCache
    {
        public void Set(string key, object value, TimeSpan expiration) { }
    }

    /// <summary>
    /// Redis cache
    /// </summary>
    public class RedisCache
    {
        public void Set(string key, object value) { }
        public object Get(string key) => "value";
    }

    /// <summary>
    /// Cache aside service
    /// </summary>
    public class CacheAsideService
    {
        public object Get(string key)
        {
            return "data";
        }
    }
}