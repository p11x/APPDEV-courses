/*
 * ============================================================
 * TOPIC     : Miscellaneous
 * SUBTOPIC  : Localization
 * FILE      : 03_Localization.cs
 * PURPOSE   : Resource-based localization
 * ============================================================
 */
using System;
using System.Globalization;

namespace CSharp_MasterGuide._24_Miscellaneous._03_Globalization
{
    /// <summary>
    /// Localization
    /// </summary>
    public class LocalizationDemo
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Localization ===\n");

            Console.WriteLine("1. Resource Files:");
            Console.WriteLine("   .resx files for translations");
            
            Console.WriteLine("\n2. ResourceManager:");
            Console.WriteLine("   Load localized strings");
            
            Console.WriteLine("\n3. Culture Switching:");
            Console.WriteLine("   Thread.CurrentThread.CurrentUICulture");

            Console.WriteLine("\n=== Localization Complete ===");
        }
    }
}