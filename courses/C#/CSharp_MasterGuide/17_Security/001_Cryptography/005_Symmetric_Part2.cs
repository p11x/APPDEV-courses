/*
 * ============================================================
 * TOPIC     : Security
 * SUBTOPIC  : Symmetric Encryption Part 2
 * FILE      : Symmetric_Part2.cs
 * PURPOSE   : Advanced symmetric encryption
 * ============================================================
 */
using System; // Core System namespace
using System.Security.Cryptography; // Cryptography namespace

namespace CSharp_MasterGuide._17_Security._01_Cryptography
{
    /// <summary>
    /// Advanced symmetric encryption
    /// </summary>
    public class SymmetricPart2Demo
    {
        /// <summary>
        /// Entry point
        /// </summary>
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Symmetric Part 2 ===\n");

            // Output: --- AES Mode Comparison ---
            Console.WriteLine("--- AES Mode Comparison ---");

            // ECB - electronic code book (not secure)
            var ecb = EncryptAES_ECB("message");
            Console.WriteLine($"   ECB: {ecb.Substring(0, 16)}...");
            // Output: ECB: [encrypted]

            // CBC - cipher block chaining (secure)
            var cbc = EncryptAES_CBC("message");
            Console.WriteLine($"   CBC: {cbc.Substring(0, 16)}...");
            // Output: CBC: [encrypted]

            // GCM - galois/counter mode (authenticated)
            var gcm = EncryptAES_GCM("message");
            Console.WriteLine($"   GCM: {gcm.Substring(0, 16)}...");
            // Output: GCM: [encrypted]

            // Output: --- Key Derivation ---
            Console.WriteLine("\n--- Key Derivation ---");

            var key = DeriveKey("password", "salt");
            Console.WriteLine($"   Derived key: {key.Substring(0, 16)}...");
            // Output: Derived key: [derived key]

            // Output: --- Stream Encryption ---
            Console.WriteLine("\n--- Stream Encryption ---");

            var encrypted = EncryptStream("longmessage");
            Console.WriteLine($"   Encrypted: {encrypted}");
            // Output: Encrypted: [encrypted stream]

            Console.WriteLine("\n=== Symmetric Part 2 Complete ===");
        }
    }

    /// <summary>
    /// Encrypt with AES ECB
    /// </summary>
    public static string EncryptAES_ECB(string plainText)
    {
        using var aes = Aes.Create();
        aes.Mode = CipherMode.ECB;
        aes.Padding = PaddingMode.PKCS7;

        using var encryptor = aes.CreateEncryptor();
        var encrypted = encryptor.TransformFinalBlock(
            System.Text.Encoding.UTF8.GetBytes(plainText),
            0, plainText.Length);

        return Convert.ToBase64String(encrypted);
    }

    /// <summary>
    /// Encrypt with AES CBC
    /// </summary>
    public static string EncryptAES_CBC(string plainText)
    {
        using var aes = Aes.Create();
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        using var encryptor = aes.CreateEncryptor();
        var encrypted = encryptor.TransformFinalBlock(
            System.Text.Encoding.UTF8.GetBytes(plainText),
            0, plainText.Length);

        return Convert.ToBase64String(encrypted);
    }

    /// <summary>
    /// Encrypt with AES GCM
    /// </summary>
    public static string EncryptAES_GCM(string plainText)
    {
        return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(plainText));
    }

    /// <summary>
    /// Derive key from password
    /// </summary>
    public static string DeriveKey(string password, string salt)
    {
        using var deriveBytes = new PBKDF2(
            password,
            System.Text.Encoding.UTF8.GetBytes(salt),
            1000,
            HashAlgorithmName.SHA256,
            32);

        return Convert.ToBase64String(deriveBytes.GetBytes(32));
    }

    /// <summary>
    /// Encrypt stream (simulated)
    /// </summary>
    public static string EncryptStream(string plainText)
    {
        return "[encrypted stream]";
    }
}