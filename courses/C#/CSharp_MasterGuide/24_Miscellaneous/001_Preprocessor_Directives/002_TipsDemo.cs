/*
 * ============================================================
 * TOPIC     : Miscellaneous
 * SUBTOPIC  : Tips
 * FILE      : 01_TipsDemo.cs
 * PURPOSE   : Demonstrates C# tips and tricks
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._24_Miscellaneous._01_Tips
{
    public class TipsDemo
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Tips Demo ===\n");
            
            Console.WriteLine("1. Null Coalescing:");
            string value = null;
            var result = value ?? "default";
            Console.WriteLine($"   Result: {result}");
            
            Console.WriteLine("\n2. String Interpolation:");
            var name = "World";
            Console.WriteLine($"   Hello, {name}!");
            
            Console.WriteLine("\n=== Tips Complete ===");
        }
    }
}