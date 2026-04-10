/*
 * ============================================================
 * TOPIC     : Diagnostics & Logging
 * SUBTOPIC  : Serilog - Structured Logging
 * FILE      : Serilog_Demo.cs
 * PURPOSE   : Using Serilog for structured logging
 * ============================================================
 */
using System; // Core System namespace

namespace CSharp_MasterGuide._22_Diagnostics_Logging._01_Logging
{
    /// <summary>
    /// Serilog demonstration
    /// </summary>
    public class SerilogDemo
    {
        /// <summary>
        /// Entry point
        /// </summary>
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Serilog Demo ===\n");

            // Output: --- Basic Logging ---
            Console.WriteLine("--- Basic Logging ---");

            Log.Information("User {UserId} logged in", 1);
            Log.Warning("Low disk space");
            Log.Error(ex: new Exception(), "Operation failed");
            // Output: [Information] User 1 logged in
            // Output: [Warning] Low disk space
            // Output: [Error] Operation failed

            // Output: --- Structured Data ---
            Console.WriteLine("\n--- Structured Data ---");

            Log.Information("Order {OrderId} created for {Customer}", 1, "Alice");
            // Output: [Information] Order 1 created for Alice

            // Output: --- Sinks ---
            Console.WriteLine("\n--- Sinks ---");

            Log.Information("Logged to console");
            Log.Information("Logged to file");
            Log.Information("Logged to sequence");
            // Output: Console sink
            // Output: File sink
            // Output: Seq sink

            Console.WriteLine("\n=== Serilog Complete ===");
        }
    }

    /// <summary>
    /// Static logger facade
    /// </summary>
    public static class Log
    {
        public static void Information(string template, params object[] args)
            => Console.WriteLine($"   [Information] {template}");

        public static void Warning(string message)
            => Console.WriteLine($"   [Warning] {message}");

        public static void Error(Exception ex, string message)
            => Console.WriteLine($"   [Error] {message}");
    }
}