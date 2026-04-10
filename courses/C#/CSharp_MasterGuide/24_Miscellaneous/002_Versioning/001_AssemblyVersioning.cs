/*
 * ============================================================
 * TOPIC     : Miscellaneous
 * SUBTOPIC  : Versioning - Assembly Versioning
 * FILE      : 01_AssemblyVersioning.cs
 * PURPOSE   : Assembly versioning in C#
 * ============================================================
 */
using System;
using System.Reflection;

namespace CSharp_MasterGuide._24_Miscellaneous._02_Versioning
{
    /// <summary>
    /// Assembly versioning
    /// </summary>
    public class AssemblyVersioning
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Assembly Versioning ===\n");

            var assembly = Assembly.GetExecutingAssembly();
            var version = assembly.GetName().Version;
            Console.WriteLine($"   Version: {version}");
            
            var infoVersion = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
            Console.WriteLine($"   Info Version: {infoVersion?.InformationalVersion}");

            Console.WriteLine("\n=== Versioning Complete ===");
        }
    }
}