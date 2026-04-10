/*
 * ============================================================
 * TOPIC     : Modern C#
 * SUBTOPIC  : C# 10 Features - Global Usings
 * FILE      : 01_GlobalUsings.cs
 * PURPOSE   : Global using directives in C# 10
 * ============================================================
 */
using System;

namespace CSharp_MasterGuide._19_ModernCSharp._02_CSharp10_Features
{
    /// <summary>
    /// C# 10 Global Usings
    /// </summary>
    public class GlobalUsingsDemo
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Global Usings ===\n");

            Console.WriteLine("1. Traditional Using:");
            Console.WriteLine("   using System; at top of each file");
            
            Console.WriteLine("\n2. Global Using:");
            Console.WriteLine("   global using System; in single file");
            
            Console.WriteLine("\n3. Implicit Usings:");
            Console.WriteLine("   <ImplicitUsings>enable</ImplicitUsings> in csproj");

            Console.WriteLine("\n=== Global Usings Complete ===");
        }
    }
}