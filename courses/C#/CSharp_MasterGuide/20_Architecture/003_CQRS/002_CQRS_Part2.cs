/*
 * ============================================================
 * TOPIC     : Architecture
 * SUBTOPIC  : CQRS - Part 2
 * FILE      : 02_CQRS_Part2.cs
 * PURPOSE   : CQRS implementation details
 * ============================================================
 */
using System;

namespace CSharp_MasterGuide._20_Architecture._02_CQRS_MediatR
{
    /// <summary>
    /// CQRS implementation
    /// </summary>
    public class CQRSPart2
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== CQRS Part 2 ===\n");

            Console.WriteLine("1. Commands:");
            Console.WriteLine("   Create, Update, Delete operations");
            
            Console.WriteLine("\n2. Queries:");
            Console.WriteLine("   Read operations with DTOs");
            
            Console.WriteLine("\n3. MediatR:");
            Console.WriteLine("   Mediator pattern for decoupling");

            Console.WriteLine("\n=== CQRS Part 2 Complete ===");
        }
    }
}