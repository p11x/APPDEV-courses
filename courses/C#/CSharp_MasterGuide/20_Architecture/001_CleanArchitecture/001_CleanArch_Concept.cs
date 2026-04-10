/*
 * ============================================================
 * TOPIC     : Architecture
 * SUBTOPIC  : Clean Architecture - Concept
 * FILE      : 01_CleanArch_Concept.cs
 * PURPOSE   : Clean Architecture overview
 * ============================================================
 */
using System;

namespace CSharp_MasterGuide._20_Architecture._01_CleanArchitecture
{
    /// <summary>
    /// Clean Architecture concept
    /// </summary>
    public class CleanArchConcept
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Clean Architecture ===\n");

            Console.WriteLine("1. Domain Layer (Core):");
            Console.WriteLine("   Entities, Value Objects, Domain Events");
            
            Console.WriteLine("\n2. Application Layer:");
            Console.WriteLine("   Use Cases, Commands, Queries, DTOs");
            
            Console.WriteLine("\n3. Infrastructure Layer:");
            Console.WriteLine("   Database, External Services, Repositories");
            
            Console.WriteLine("\n4. Presentation Layer:");
            Console.WriteLine("   API Controllers, UI Components");

            Console.WriteLine("\n=== Clean Architecture Complete ===");
        }
    }
}