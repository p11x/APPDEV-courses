/*
 * ============================================================
 * TOPIC     : Diagnostics & Logging
 * SUBTOPIC  : NLog - Logging Framework
 * FILE      : NLog_Demo.cs
 * PURPOSE   : Using NLog for logging
 * ============================================================
 */
using System; // Core System namespace

namespace CSharp_MasterGuide._22_Diagnostics_Logging._01_Logging
{
    /// <summary>
    /// NLog demonstration
    /// </summary>
    public class NLogDemo
    {
        /// <summary>
        /// Entry point
        /// </summary>
        public static void Main(string[] args)
        {
            Console.WriteLine("=== NLog Demo ===\n");

            // Output: --- Logger ---
            Console.WriteLine("--- Logger ---");

            var logger = NLog.LogManager.GetCurrentClassLogger();
            logger.Info("Application started");
            logger.Warn("Warning message");
            logger.Error("Error occurred");
            // Output: [Info] Application started
            // Output: [Warn] Warning message
            // Output: [Error] Error occurred

            // Output: --- Configuration ---
            Console.WriteLine("\n--- Configuration ---");

            Console.WriteLine("   Targets: File, Console, Database");
            Console.WriteLine("   Layout: ${longdate}|${level}|${logger}|${message}");
            // Output: Target configuration

            // Output: --- Layout Renderers ---
            Console.WriteLine("\n--- Layout Renderers ---");

            logger.Info("User: ${identity}");
            logger.Error("Exception: ${exception}");
            // Output: [Info] User: ${identity}

            Console.WriteLine("\n=== NLog Complete ===");
        }
    }

    /// <summary>
    /// NLog manager
    /// </summary>
    public static class NLog
    {
        public static LogManager LogManager { get; } = new();
    }

    /// <summary>
    /// Log manager
    /// </summary>
    public class LogManager
    {
        public Logger GetCurrentClassLogger() => new Logger();
    }

    /// <summary>
    /// Logger
    /// </summary>
    public class Logger
    {
        public void Info(string msg) => Console.WriteLine($"   [Info] {msg}");
        public void Warn(string msg) => Console.WriteLine($"   [Warn] {msg}");
        public void Error(string msg) => Console.WriteLine($"   [Error] {msg}");
    }
}