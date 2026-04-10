/*
 * ============================================================
 * TOPIC     : Architecture
 * SUBTOPIC  : Clean Architecture - Part 2
 * FILE      : 02_CleanArch_Part2.cs
 * PURPOSE   : Clean Architecture layers detail
 * ============================================================
 */
using System;

namespace CSharp_MasterGuide._20_Architecture._01_CleanArchitecture
{
    /// <summary>
    /// Clean Architecture layers
    /// </summary>
    public class CleanArchPart2
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Clean Architecture Part 2 ===\n");

            Console.WriteLine("1. Domain Layer:");
            Console.WriteLine("   Entities, Value Objects, Domain Services");
            
            Console.WriteLine("\n2. Application Layer:");
            Console.WriteLine("   Use Cases, Interfaces, DTOs");
            
            Console.WriteLine("\n3. Infrastructure:");
            Console.WriteLine("   Repositories, External Services");

            Console.WriteLine("\n=== Clean Architecture Part 2 Complete ===");
        }
    }
}