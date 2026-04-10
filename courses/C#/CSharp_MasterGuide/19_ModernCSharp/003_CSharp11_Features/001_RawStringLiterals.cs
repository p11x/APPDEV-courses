/*
 * ============================================================
 * TOPIC     : Modern C#
 * SUBTOPIC  : C# 11 Features - Raw String Literals
 * FILE      : 01_RawStringLiterals.cs
 * PURPOSE   : Raw string literals in C# 11
 * ============================================================
 */
using System;

namespace CSharp_MasterGuide._19_ModernCSharp._03_CSharp11_Features
{
    /// <summary>
    /// C# 11 Raw String Literals
    /// </summary>
    public class RawStringLiteralsDemo
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Raw String Literals ===\n");

            // Multi-line string
            var json = """
                {
                    "name": "John",
                    "age": 30
                }
                """;
            Console.WriteLine($"   JSON: {json.Trim()}");
            
            // Interpolated raw string
            var name = "John";
            var greeting = $$"""Hello, {{name}}!""";
            Console.WriteLine($"   Greeting: {greeting}");

            Console.WriteLine("\n=== Raw String Literals Complete ===");
        }
    }
}