/*
 * ============================================================
 * TOPIC     : Miscellaneous
 * SUBTOPIC  : Preprocessor Directives
 * FILE      : 01_PreprocessorDirectives.cs
 * PURPOSE   : C# preprocessor directives
 * ============================================================
 */
using System;

namespace CSharp_MasterGuide._24_Miscellaneous._01_Preprocessor_Directives
{
    /// <summary>
    /// Preprocessor directives
    /// </summary>
    public class PreprocessorDirectives
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Preprocessor Directives ===\n");

#if DEBUG
            Console.WriteLine("   DEBUG mode: Detailed logging enabled");
#else
            Console.WriteLine("   RELEASE mode: Optimized");
#endif

            Console.WriteLine("\n=== Preprocessor Directives Complete ===");
        }
    }
}