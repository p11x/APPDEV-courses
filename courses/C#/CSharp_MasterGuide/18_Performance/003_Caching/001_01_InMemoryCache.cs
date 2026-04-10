/*
 * ============================================================
 * TOPIC     : Performance
 * SUBTOPIC  : Caching - In-Memory Cache
 * FILE      : 01_InMemoryCache.cs
 * PURPOSE   : In-memory caching in C#
 * ============================================================
 */
using System;
using System.Collections.Generic;

namespace CSharp_MasterGuide._18_Performance._03_Caching
{
    /// <summary>
    /// In-memory caching
    /// </summary>
    public class InMemoryCache
    {
        private readonly Dictionary<string, object> _cache = new();
        
        public static void Main(string[] args)
        {
            Console.WriteLine("=== In-Memory Cache ===\n");

            var cache = new InMemoryCache();
            cache.Set("key1", "value1");
            var value = cache.Get("key1");
            Console.WriteLine($"   Retrieved: {value}");

            Console.WriteLine("\n=== In-Memory Cache Complete ===");
        }
        
        public void Set(string key, object value) => _cache[key] = value;
        public object Get(string key) => _cache.TryGetValue(key, out var v) ? v : null;
    }
}