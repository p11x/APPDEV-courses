/*
 * ============================================================
 * TOPIC     : Dependency Injection
 * SUBTOPIC  : DI Basics - Property Injection
 * FILE      : 02_PropertyInjection.cs
 * PURPOSE   : Property injection pattern in C#
 * ============================================================
 */
using System;

namespace CSharp_MasterGuide._13_DependencyInjection._01_DI_Basics
{
    /// <summary>
    /// Demonstrates property injection
    /// </summary>
    public class PropertyInjection
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Property Injection ===\n");

            var service = new OrderService();
            service.Logger = new FileLogger();
            
            service.ProcessOrder("ORDER-001");
            // Output: Order processed, Logged to file

            Console.WriteLine("\n=== Property Injection Complete ===");
        }
    }

    public interface ILogger { void Log(string message); }
    public class FileLogger : ILogger { public void Log(string m) => Console.WriteLine($"   Logged to file: {m}"); }

    /// <summary>
    /// Service with property injection
    /// </summary>
    public class OrderService
    {
        // Property injection
        public ILogger Logger { get; set; }
        
        public void ProcessOrder(string orderId)
        {
            Console.WriteLine($"   Order processed: {orderId}");
            Logger?.Log($"Order processed: {orderId}");
        }
    }
}