/*
 * ============================================================
 * TOPIC     : Dependency Injection
 * SUBTOPIC  : DI Basics - Concept Part 2
 * FILE      : 02_DI_Concept_Part2.cs
 * PURPOSE   : Advanced DI concepts and patterns
 * ============================================================
 */
using System;

namespace CSharp_MasterGuide._13_DependencyInjection._01_DI_Basics
{
    /// <summary>
    /// Advanced DI concepts
    /// </summary>
    public class DIConceptPart2
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== DI Concept Part 2 ===\n");

            // Service registration
            Console.WriteLine("1. Service Registration:");
            Console.WriteLine("   Singleton, Scoped, Transient");
            
            // Resolution
            Console.WriteLine("\n2. Service Resolution:");
            Console.WriteLine("   Constructor, Property, Method");
            
            // Lifetime management
            Console.WriteLine("\n3. Lifetime Management:");
            Console.WriteLine("   Container manages object lifecycle");

            Console.WriteLine("\n=== DI Concept Part 2 Complete ===");
        }
    }
}