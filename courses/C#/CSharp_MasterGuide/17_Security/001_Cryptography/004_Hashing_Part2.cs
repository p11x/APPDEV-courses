/*
 * ============================================================
 * TOPIC     : Security
 * SUBTOPIC  : Hashing Algorithms Part 2
 * FILE      : Hashing_Part2.cs
 * PURPOSE   : Advanced hashing techniques
 * ============================================================
 */
using System; // Core System namespace
using System.Security.Cryptography; // Cryptography namespace

namespace CSharp_MasterGuide._17_Security._01_Cryptography
{
    /// <summary>
    /// Advanced hashing demonstration
    /// </summary>
    public class HashingPart2Demo
    {
        /// <summary>
        /// Entry point
        /// </summary>
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Hashing Part 2 ===\n");

            // Output: --- PBKDF2 ---
            Console.WriteLine("--- PBKDF2 ---");

            var hash = HashPasswordPBKDF2("password");
            Console.WriteLine($"   PBKDF2 hash: {hash.Substring(0, 20)}...");
            // Output: PBKDF2 hash: [hashed value]

            // Output: --- BCrypt ---
            Console.WriteLine("\n--- BCrypt ---");

            var bcryptHash = HashPasswordBCrypt("password");
            Console.WriteLine($"   BCrypt hash: {bcryptHash.Substring(0, 20)}...");
            // Output: BCrypt hash: [hashed value]

            // Output: --- Argon2 ---
            Console.WriteLine("\n--- Argon2 ---");

            var argonHash = HashPasswordArgon2("password");
            Console.WriteLine($"   Argon2 hash: {argonHash.Substring(0, 20)}...");
            // Output: Argon2 hash: [hashed value]

            // Output: --- HMAC ---
            Console.WriteLine("\n--- HMAC ---");

            var hmac = ComputeHMAC("message", "key");
            Console.WriteLine($"   HMAC: {hmac}");
            // Output: HMAC: [computed value]

            Console.WriteLine("\n=== Hashing Part 2 Complete ===");
        }
    }

    /// <summary>
    /// Hash using PBKDF2
    /// </summary>
    public static string HashPasswordPBKDF2(string password)
    {
        var salt = new byte[16];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(salt);

        var hash = new byte[32];
        using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000);
        pbkdf2.GetBytes(hash);

        return Convert.ToBase64String(hash);
    }

    /// <summary>
    /// Hash using BCrypt (simulated)
    /// </summary>
    public static string HashPasswordBCrypt(string password)
    {
        return "$2a$10$" + "hashedvalue12345678901234567890123456789012";
    }

    /// <summary>
    /// Hash using Argon2 (simulated)
    /// </summary>
    public static string HashPasswordArgon2(string password)
    {
        return "$argon2i$" + "hashedvalue123456789012345678901234567890";
    }

    /// <summary>
    /// Compute HMAC
    /// </summary>
    public static string ComputeHMAC(string message, string key)
    {
        using var hmac = new HMACSHA256();
        var hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(message));
        return Convert.ToBase64String(hash);
    }
}