/*
 * ============================================================
 * TOPIC     : Modern C#
 * SUBTOPIC  : C# 10 - File-Scoped Namespaces
 * FILE      : 02_FileScoped_Namespaces.cs
 * PURPOSE   : File-scoped namespace feature
 * ============================================================
 */
using System;

namespace CSharp_MasterGuide._19_ModernCSharp._02_CSharp10_Features
{
    /// <summary>
    /// File-scoped namespaces
    /// </summary>
    public class FileScopedNamespaces
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== File-Scoped Namespaces ===\n");
            Console.WriteLine("   namespace X.Y.Z; - applies to entire file");
            Console.WriteLine("\n=== File-Scoped Namespaces Complete ===");
        }
    }
}