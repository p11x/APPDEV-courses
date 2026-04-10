/*
 * ============================================================
 * TOPIC     : Miscellaneous
 * SUBTOPIC  : Real-World Miscellaneous
 * FILE      : 03_Miscellaneous_RealWorld.cs
 * PURPOSE   : Real-world miscellaneous examples
 * ============================================================
 */
using System; // needed for Console, basic types
using System.Collections.Generic; // needed for List<T>

namespace CSharp_MasterGuide._24_Miscellaneous._03_RealWorld
{
    public class MiscellaneousRealWorldDemo
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Miscellaneous Real-World ===\n");
            
            Console.WriteLine("1. Extension Methods:");
            var text = "hello";
            Console.WriteLine($"   {text.ToTitleCase()}");
            
            Console.WriteLine("\n2. Yield Return:");
            var nums = GetNumbers();
            foreach (var n in nums) Console.WriteLine($"   {n}");
            
            Console.WriteLine("\n=== Miscellaneous Real-World Complete ===");
        }
        
        static IEnumerable<int> GetNumbers()
        {
            yield return 1;
            yield return 2;
            yield return 3;
        }
    }

    public static class StringExtensions
    {
        public static string ToTitleCase(this string s) => 
            char.ToUpper(s[0]) + s.Substring(1);
    }
}