/*
 * ============================================================
 * TOPIC     : Miscellaneous
 * SUBTOPIC  : Globalization - CultureInfo Part 2
 * FILE      : 02_CultureInfo_Part2.cs
 * PURPOSE   : Advanced CultureInfo features
 * ============================================================
 */
using System;
using System.Globalization;

namespace CSharp_MasterGuide._24_Miscellaneous._03_Globalization
{
    /// <summary>
    /// Advanced CultureInfo
    /// </summary>
    public class CultureInfoPart2
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== CultureInfo Part 2 ===\n");

            var us = new CultureInfo("en-US");
            var de = new CultureInfo("de-DE");
            
            Console.WriteLine($"   US Date: {DateTime.Now.ToString("D", us)}");
            Console.WriteLine($"   DE Date: {DateTime.Now.ToString("D", de)}");
            
            Console.WriteLine($"   US Number: {(1234.56).ToString("N", us)}");
            Console.WriteLine($"   DE Number: {(1234.56).ToString("N", de)}");

            Console.WriteLine("\n=== CultureInfo Part 2 Complete ===");
        }
    }
}