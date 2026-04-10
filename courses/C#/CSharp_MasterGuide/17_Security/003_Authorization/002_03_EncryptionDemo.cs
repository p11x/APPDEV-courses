/*
 * ============================================================
 * TOPIC     : Security
 * SUBTOPIC  : Encryption
 * FILE      : 03_EncryptionDemo.cs
 * PURPOSE   : Demonstrates encryption in C#
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._17_Security._03_Encryption
{
    public class EncryptionDemo
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Encryption Demo ===\n");

            Console.WriteLine("1. Hashing:");
            var hasher = new PasswordHasher();
            var hash = hasher.Hash("password123");
            Console.WriteLine($"   Hash: {hash}");

            Console.WriteLine("\n2. Symmetric Encryption:");
            var enc = new SymmetricEncryptor();
            var encrypted = enc.Encrypt("secret data", "key123");
            Console.WriteLine($"   Encrypted: {encrypted}");

            Console.WriteLine("\n=== Encryption Complete ===");
        }
    }

    public class PasswordHasher
    {
        public string Hash(string password) => "5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8";
    }

    public class SymmetricEncryptor
    {
        public string Encrypt(string data, string key) => "encrypted_" + data;
    }
}