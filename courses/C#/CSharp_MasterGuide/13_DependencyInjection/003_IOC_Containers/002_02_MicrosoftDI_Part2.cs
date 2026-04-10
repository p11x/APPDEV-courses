/*
 * ============================================================
 * TOPIC     : Dependency Injection
 * SUBTOPIC  : IOC Containers - Part 2
 * FILE      : 02_MicrosoftDI_Part2.cs
 * PURPOSE   : Microsoft DI advanced features
 * ============================================================
 */
using System;

namespace CSharp_MasterGuide._13_DependencyInjection._03_IOC_Containers
{
    /// <summary>
    /// Microsoft DI advanced
    /// </summary>
    public class MicrosoftDIPart2
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Microsoft DI Part 2 ===\n");

            Console.WriteLine("1. Options Pattern:");
            Console.WriteLine("   IOptions<T> for configuration");
            
            Console.WriteLine("\n2. Named Services:");
            Console.WriteLine("   Multiple implementations of same interface");
            
            Console.WriteLine("\n3. Decorator Pattern:");
            Console.WriteLine("   Wrap services with additional behavior");

            Console.WriteLine("\n=== Microsoft DI Part 2 Complete ===");
        }
    }
}