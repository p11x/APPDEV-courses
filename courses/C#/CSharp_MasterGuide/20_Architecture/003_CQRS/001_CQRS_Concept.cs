/*
 * ============================================================
 * TOPIC     : Architecture
 * SUBTOPIC  : CQRS - Concept
 * FILE      : 01_CQRS_Concept.cs
 * PURPOSE   : Command Query Responsibility Segregation
 * ============================================================
 */
using System;

namespace CSharp_MasterGuide._20_Architecture._02_CQRS_MediatR
{
    /// <summary>
    /// CQRS concept
    /// </summary>
    public class CQRSConcept
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== CQRS Concept ===\n");

            Console.WriteLine("1. Commands (Write):");
            Console.WriteLine("   CreateUserCommand -> Execute -> Changes state");
            
            Console.WriteLine("\n2. Queries (Read):");
            Console.WriteLine("   GetUserQuery -> Execute -> Returns data");
            
            Console.WriteLine("\n3. Separate Models:");
            Console.WriteLine("   Command model != Query model");
            
            Console.WriteLine("\n4. Benefits:");
            Console.WriteLine("   Optimized read/write, Scalability");

            Console.WriteLine("\n=== CQRS Complete ===");
        }
    }
}