/*
 * ============================================================
 * TOPIC     : Dependency Injection
 * SUBTOPIC  : Service Lifetimes - Singleton/Scoped/Transient
 * FILE      : 01_Singleton_Scoped_Transient.cs
 * PURPOSE   : DI service lifetime patterns
 * ============================================================
 */
using System;

namespace CSharp_MasterGuide._13_DependencyInjection._02_ServiceLifetimes
{
    /// <summary>
    /// Demonstrates service lifetimes
    /// </summary>
    public class ServiceLifetimes
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Service Lifetimes ===\n");

            // Singleton - same instance everywhere
            Console.WriteLine("1. Singleton:");
            var singleton1 = SingletonService.Instance;
            var singleton2 = SingletonService.Instance;
            Console.WriteLine($"   Same: {singleton1 == singleton2}");

            // Transient - new instance each time
            Console.WriteLine("\n2. Transient:");
            var transient1 = new TransientService();
            var transient2 = new TransientService();
            Console.WriteLine($"   Same: {transient1 == transient2}");

            // Scoped - same instance within scope
            Console.WriteLine("\n3. Scoped:");
            using (var scope = new Scope())
            {
                var scoped1 = scope.Create<ScopedService>();
                var scoped2 = scope.Create<ScopedService>();
                Console.WriteLine($"   Same in scope: {scoped1 == scoped2}");
            }

            Console.WriteLine("\n=== Service Lifetimes Complete ===");
        }
    }

    public class SingletonService
    {
        private static readonly Lazy<SingletonService> _instance = new(() => new SingletonService());
        public static SingletonService Instance => _instance.Value;
    }

    public class TransientService { }

    public class ScopedService { }

    public class Scope : IDisposable
    {
        private readonly List<object> _services = new();
        public T Create<T>() where T : new()
        {
            var service = new T();
            _services.Add(service);
            return service;
        }
        public void Dispose() { }
    }
}