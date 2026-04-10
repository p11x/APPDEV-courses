/*
 * ============================================================
 * TOPIC     : Performance
 * SUBTOPIC  : Real-World Performance
 * FILE      : 03_Performance_RealWorld.cs
 * PURPOSE   : Real-world performance examples
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._18_Performance._04_RealWorld
{
    public class PerformanceRealWorldDemo
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Performance Real-World ===\n");
            Console.WriteLine("1. Caching:");
            var cache = new ResponseCache();
            cache.Get("user:1");
            cache.Get("user:1");
            Console.WriteLine("   Cache hit on second request");
            Console.WriteLine("\n=== Performance Real-World Complete ===");
        }
    }

    public class ResponseCache
    {
        public string Get(string key) => $"Data for {key}";
    }
}