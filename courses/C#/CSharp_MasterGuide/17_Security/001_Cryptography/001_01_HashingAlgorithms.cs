/*
 * ============================================================
 * TOPIC     : Security
 * SUBTOPIC  : Cryptography - Hashing
 * FILE      : 01_HashingAlgorithms.cs
 * PURPOSE   : Password hashing in C#
 * ============================================================
 */
using System;
using System.Security.Cryptography;

namespace CSharp_MasterGuide._17_Security._01_Cryptography
{
    /// <summary>
    /// Hashing algorithms
    /// </summary>
    public class HashingAlgorithms
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Hashing Algorithms ===\n");

            var password = "MySecurePassword";
            
            // SHA256 hashing
            Console.WriteLine("1. SHA256:");
            var sha256 = SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(password));
            Console.WriteLine($"   Hash: {Convert.ToHexString(sha256)}");
            
            // MD5 (not for security)
            Console.WriteLine("\n2. MD5:");
            var md5 = MD5.HashData(System.Text.Encoding.UTF8.GetBytes(password));
            Console.WriteLine($"   Hash: {Convert.ToHexString(md5)}");
            
            // BCrypt (recommended)
            Console.WriteLine("\n3. BCrypt (recommended):");
            Console.WriteLine("   Using BCrypt for password storage");

            Console.WriteLine("\n=== Hashing Complete ===");
        }
    }
}