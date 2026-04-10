/*
 * ============================================================
 * TOPIC     : Miscellaneous
 * SUBTOPIC  : Preprocessor Directives - Part 2
 * FILE      : 02_Preprocessor_Part2.cs
 * PURPOSE   : Advanced preprocessor directives
 * ============================================================
 */
using System;

namespace CSharp_MasterGuide._24_Miscellaneous._01_Preprocessor_Directives
{
    /// <summary>
    /// Advanced preprocessor directives
    /// </summary>
    public class PreprocessorPart2
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Preprocessor Part 2 ===\n");

#if DEBUG
            Console.WriteLine("   DEBUG: Full logging");
#elif TRACE
            Console.WriteLine("   TRACE: Some logging");
#else
            Console.WriteLine("   RELEASE: Minimal logging");
#endif

            Console.WriteLine("\n=== Preprocessor Part 2 Complete ===");
        }
    }
}