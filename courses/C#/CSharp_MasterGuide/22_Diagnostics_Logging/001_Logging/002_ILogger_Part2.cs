/*
 * ============================================================
 * TOPIC     : Diagnostics & Logging
 * SUBTOPIC  : Logging - ILogger Part 2
 * FILE      : 02_ILogger_Part2.cs
 * PURPOSE   : Advanced logging features
 * ============================================================
 */
using System;

namespace CSharp_MasterGuide._22_Diagnostics_Logging._01_Logging
{
    /// <summary>
    /// Advanced logging
    /// </summary>
    public class ILoggerPart2
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== ILogger Part 2 ===\n");

            Console.WriteLine("1. Structured Logging:");
            Console.WriteLine("   Log with properties: {UserId}");
            
            Console.WriteLine("\n2. Log Levels:");
            Console.WriteLine("   Trace, Debug, Info, Warning, Error");
            
            Console.WriteLine("\n3. Scopes:");
            Console.WriteLine("   Group related log entries");

            Console.WriteLine("\n=== ILogger Part 2 Complete ===");
        }
    }
}