/*
 * ============================================================
 * TOPIC     : Miscellaneous
 * SUBTOPIC  : Globalization - CultureInfo
 * FILE      : 01_CultureInfo.cs
 * PURPOSE   : CultureInfo for internationalization
 * ============================================================
 */
using System;
using System.Globalization;

namespace CSharp_MasterGuide._24_Miscellaneous._03_Globalization
{
    /// <summary>
    /// CultureInfo examples
    /// </summary>
    public class CultureInfoDemo
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== CultureInfo ===\n");

            // Current culture
            var current = CultureInfo.CurrentCulture;
            Console.WriteLine($"   Current: {current.Name}");
            
            // Specific culture
            var us = new CultureInfo("en-US");
            var de = new CultureInfo("de-DE");
            
            // Format numbers
            Console.WriteLine($"   US Format: {1234.56.ToString("C", us)}");
            Console.WriteLine($"   DE Format: {1234.56.ToString("C", de)}");
            
            // Format dates
            var date = new DateTime(2024, 12, 25);
            Console.WriteLine($"   US Date: {date.ToString("d", us)}");
            Console.WriteLine($"   DE Date: {date.ToString("d", de)}");

            Console.WriteLine("\n=== CultureInfo Complete ===");
        }
    }
}