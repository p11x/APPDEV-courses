/*
 * ============================================================
 * TOPIC     : Diagnostics & Logging
 * SUBTOPIC  : Real-World Logging
 * FILE      : 03_Logging_RealWorld.cs
 * PURPOSE   : Real-world logging examples
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._22_Diagnostics_Logging._03_RealWorld
{
    public class LoggingRealWorldDemo
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Logging Real-World ===\n");
            Console.WriteLine("1. Structured Logging:");
            var logger = new StructuredLogger();
            logger.Log("OrderCreated", new { OrderId = 123, Amount = 99.99m });
            Console.WriteLine("\n=== Logging Real-World Complete ===");
        }
    }

    public class StructuredLogger
    {
        public void Log(string eventType, object data) => Console.WriteLine($"   Event: {eventType}, Data: {data}");
    }
}