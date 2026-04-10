/*
 * ============================================================
 * TOPIC     : Miscellaneous
 * SUBTOPIC  : Globalization Real-World
 * FILE      : Globalization_RealWorld.cs
 * PURPOSE   : Real-world globalization examples
 * ============================================================
 */
using System; // Core System namespace
using System.Globalization; // Culture namespace

namespace CSharp_MasterGuide._24_Miscellaneous._03_Globalization
{
    /// <summary>
    /// Real-world globalization
    /// </summary>
    public class GlobalizationRealWorldDemo
    {
        /// <summary>
        /// Entry point
        /// </summary>
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Globalization Real-World ===\n");

            // Output: --- Currency Formatting ---
            Console.WriteLine("--- Currency Formatting ---");

            var us = new CultureInfo("en-US");
            var eur = new CultureInfo("de-DE");
            var jpy = new CultureInfo("ja-JP");

            Console.WriteLine($"   US: {(100m).ToString("C", us)}");
            // Output: US: $100.00
            Console.WriteLine($"   Euro: {(100m).ToString("C", eur)}");
            // Output: Euro: 100,00 €
            Console.WriteLine($"   Yen: {(100m).ToString("C", jpy)}");
            // Output: Yen: ¥100

            // Output: --- Date Formatting ---
            Console.WriteLine("\n--- Date Formatting ---");

            var date = new DateTime(2024, 12, 25);
            Console.WriteLine($"   US: {date.ToString("d", us)}");
            // Output: US: 12/25/2024
            Console.WriteLine($"   UK: {date.ToString("d", new CultureInfo("en-GB"))}");
            // Output: UK: 25/12/2024

            // Output: --- Sorting ---
            Console.WriteLine("\n--- Sorting ---");

            Console.WriteLine("   Swedish: Å, Ä, Ö");
            Console.WriteLine("   German: Ö, Ä, A");
            // Output: Different sorting

            Console.WriteLine("\n=== Real-World Complete ===");
        }
    }
}