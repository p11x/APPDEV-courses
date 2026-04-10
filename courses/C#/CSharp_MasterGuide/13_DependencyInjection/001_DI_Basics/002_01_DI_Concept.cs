/*
 * ============================================================
 * TOPIC     : Dependency Injection
 * SUBTOPIC  : DI Basics - Concept
 * FILE      : 01_DI_Concept.cs
 * PURPOSE   : Dependency Injection core concept
 * ============================================================
 */
using System;

namespace CSharp_MasterGuide._13_DependencyInjection._01_DI_Basics
{
    /// <summary>
    /// Dependency Injection concept
    /// </summary>
    public class DIConcept
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== DI Concept ===\n");

            // What is DI?
            Console.WriteLine("1. What is DI?");
            Console.WriteLine("   Dependency Injection - passing dependencies to objects");
            
            // Why use DI?
            Console.WriteLine("\n2. Why DI?");
            Console.WriteLine("   Loose coupling, testability, flexibility");
            
            // DI Container
            Console.WriteLine("\n3. DI Container");
            Console.WriteLine("   Manages object creation and lifetime");

            Console.WriteLine("\n=== DI Concept Complete ===");
        }
    }
}