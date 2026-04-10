/*
 * ============================================================
 * TOPIC     : Diagnostics & Logging
 * SUBTOPIC  : Diagnostics
 * FILE      : 01_DiagnosticsDemo.cs
 * PURPOSE   : Demonstrates diagnostics in C#
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._22_Diagnostics_Logging._01_Diagnostics
{
    public class DiagnosticsDemo
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Diagnostics Demo ===\n");
            Console.WriteLine("1. Debug Output:");
            System.Diagnostics.Debug.WriteLine("   Debug message");
            Console.WriteLine("   Trace message");
            Console.WriteLine("\n=== Diagnostics Complete ===");
        }
    }
}