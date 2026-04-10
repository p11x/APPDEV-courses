/*
 * ============================================================
 * TOPIC     : Diagnostics & Logging
 * SUBTOPIC  : Logging
 * FILE      : 02_LoggingDemo.cs
 * PURPOSE   : Demonstrates logging in C#
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._22_Diagnostics_Logging._02_Logging
{
    public class LoggingDemo
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Logging Demo ===\n");
            Console.WriteLine("1. Simple Logging:");
            var logger = new SimpleLogger();
            logger.LogInfo("Application started");
            logger.LogWarning("Low memory");
            logger.LogError("Connection failed");
            Console.WriteLine("\n=== Logging Complete ===");
        }
    }

    public class SimpleLogger
    {
        public void LogInfo(string msg) => Console.WriteLine($"   [INFO] {msg}");
        public void LogWarning(string msg) => Console.WriteLine($"   [WARN] {msg}");
        public void LogError(string msg) => Console.WriteLine($"   [ERROR] {msg}");
    }
}