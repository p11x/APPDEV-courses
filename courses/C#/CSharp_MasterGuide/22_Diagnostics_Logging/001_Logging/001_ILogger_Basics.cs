/*
 * ============================================================
 * TOPIC     : Diagnostics & Logging
 * SUBTOPIC  : Logging Basics - ILogger
 * FILE      : 01_ILogger_Basics.cs
 * PURPOSE   : Microsoft.Extensions.Logging basics
 * ============================================================
 */
using System;

namespace CSharp_MasterGuide._22_Diagnostics_Logging._01_Logging
{
    /// <summary>
    /// ILogger basics
    /// </summary>
    public class ILoggerBasics
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== ILogger Basics ===\n");

            Console.WriteLine("1. Log Levels:");
            Console.WriteLine("   Trace, Debug, Information, Warning, Error, Critical");
            
            Console.WriteLine("\n2. Log Information:");
            Console.WriteLine("   [Information] Application started");
            
            Console.WriteLine("\n3. Log Warning:");
            Console.WriteLine("   [Warning] Low memory");
            
            Console.WriteLine("\n4. Log Error:");
            Console.WriteLine("   [Error] Database connection failed");

            Console.WriteLine("\n=== ILogger Complete ===");
        }
    }
}