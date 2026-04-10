/*
 * ============================================================
 * TOPIC     : Interoperability
 * SUBTOPIC  : P/Invoke - Part 2
 * FILE      : 02_PInvoke_Part2.cs
 * PURPOSE   : Advanced P/Invoke features
 * ============================================================
 */
using System;

namespace CSharp_MasterGuide._21_Interoperability._01_PInvoke
{
    /// <summary>
    /// Advanced P/Invoke
    /// </summary>
    public class PInvokePart2
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== P/Invoke Part 2 ===\n");

            Console.WriteLine("1. Struct Marshaling:");
            Console.WriteLine("   Passing structures to native code");
            
            Console.WriteLine("\n2. Callback Functions:");
            Console.WriteLine("   Native callbacks to C#");
            
            Console.WriteLine("\n3. Memory Management:");
            Console.WriteLine("   SafeHandle for resources");

            Console.WriteLine("\n=== P/Invoke Part 2 Complete ===");
        }
    }
}