/*
 * ============================================================
 * TOPIC     : Security
 * SUBTOPIC  : Cryptography - Symmetric Encryption
 * FILE      : 03_SymmetricEncryption.cs
 * PURPOSE   : AES symmetric encryption
 * ============================================================
 */
using System;

namespace CSharp_MasterGuide._17_Security._01_Cryptography
{
    /// <summary>
    /// Symmetric encryption
    /// </summary>
    public class SymmetricEncryption
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Symmetric Encryption ===\n");

            Console.WriteLine("1. AES Algorithm:");
            Console.WriteLine("   Advanced Encryption Standard");
            
            Console.WriteLine("\n2. Key Management:");
            Console.WriteLine("   Secure key storage required");
            
            Console.WriteLine("\n3. Use Cases:");
            Console.WriteLine("   File encryption, database fields");

            Console.WriteLine("\n=== Symmetric Encryption Complete ===");
        }
    }
}