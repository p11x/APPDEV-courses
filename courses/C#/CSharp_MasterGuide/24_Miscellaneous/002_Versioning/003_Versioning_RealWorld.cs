/*
 * ============================================================
 * TOPIC     : Miscellaneous
 * SUBTOPIC  : Versioning - Real-World
 * FILE      : 02_Versioning_RealWorld.cs
 * PURPOSE   : Versioning real-world examples
 * ============================================================
 */
using System;

namespace CSharp_MasterGuide._24_Miscellaneous._02_Versioning
{
    /// <summary>
    /// Versioning real-world
    /// </summary>
    public class VersioningRealWorld
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Versioning Real-World ===\n");

            Console.WriteLine("1. Semantic Versioning:");
            Console.WriteLine("   Major.Minor.Patch");
            
            Console.WriteLine("\n2. Assembly Version:");
            Console.WriteLine("   [assembly: AssemblyVersion]");
            
            Console.WriteLine("\n3. File Version:");
            Console.WriteLine("   Different from assembly version");

            Console.WriteLine("\n=== Versioning Real-World Complete ===");
        }
    }
}