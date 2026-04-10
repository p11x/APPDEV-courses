/*
 * ============================================================
 * TOPIC     : Diagnostics & Logging
 * SUBTOPIC  : Diagnostics - Activity Part 2
 * FILE      : 02_Activity_Part2.cs
 * PURPOSE   : Advanced activity tracing
 * ============================================================
 */
using System;

namespace CSharp_MasterGuide._22_Diagnostics_Logging._02_Diagnostics
{
    /// <summary>
    /// Advanced activity tracing
    /// </summary>
    public class ActivityPart2
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Activity Part 2 ===\n");

            Console.WriteLine("1. Baggage:");
            Console.WriteLine("   Propagate data across operations");
            
            Console.WriteLine("\n2. Links:");
            Console.WriteLine("   Connect related activities");
            
            Console.WriteLine("\n3. Tags:");
            Console.WriteLine("   Add metadata to activities");

            Console.WriteLine("\n=== Activity Part 2 Complete ===");
        }
    }
}