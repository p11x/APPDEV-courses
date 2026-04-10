/*
 * ============================================================
 * TOPIC     : Dependency Injection
 * SUBTOPIC  : Service Lifetimes - Part 2
 * FILE      : 02_Singleton_Scoped_Transient_Part2.cs
 * PURPOSE   : Deep dive into service lifetimes
 * ============================================================
 */
using System;

namespace CSharp_MasterGuide._13_DependencyInjection._02_ServiceLifetimes
{
    /// <summary>
    /// Service lifetimes deep dive
    /// </summary>
    public class LifetimesPart2
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Service Lifetimes Part 2 ===\n");

            Console.WriteLine("1. Singleton:");
            Console.WriteLine("   One instance for entire application");
            
            Console.WriteLine("\n2. Scoped:");
            Console.WriteLine("   One instance per request/scope");
            
            Console.WriteLine("\n3. Transient:");
            Console.WriteLine("   New instance each time requested");

            Console.WriteLine("\n=== Lifetimes Part 2 Complete ===");
        }
    }
}