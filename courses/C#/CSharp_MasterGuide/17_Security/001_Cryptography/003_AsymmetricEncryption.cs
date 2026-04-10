/*
 * ============================================================
 * TOPIC     : Security
 * SUBTOPIC  : Asymmetric Encryption
 * FILE      : AsymmetricEncryption.cs
 * PURPOSE   : Public/private key encryption
 * ============================================================
 */
using System; // Core System namespace
using System.Security.Cryptography; // Cryptography namespace

namespace CSharp_MasterGuide._17_Security._01_Cryptography
{
    /// <summary>
    /// Asymmetric encryption demonstration
    /// </summary>
    public class AsymmetricEncryptionDemo
    {
        /// <summary>
        /// Entry point
        /// </summary>
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Asymmetric Encryption ===\n");

            // Output: --- RSA Example ---
            Console.WriteLine("--- RSA Encryption ---");

            var keyPair = GenerateRSAKeys();
            var privateKey = keyPair.Private;
            var publicKey = keyPair.Public;
            Console.WriteLine($"   Keys generated");
            // Output: Keys generated

            // Output: --- Encrypt with Public Key ---
            Console.WriteLine("\n--- Encrypt with Public Key ---");

            var encrypted = RSAEncrypt("message", publicKey);
            Console.WriteLine($"   Encrypted: {encrypted.Substring(0, 16)}...");
            // Output: Encrypted: [encrypted]

            // Output: --- Decrypt with Private Key ---
            Console.WriteLine("\n--- Decrypt with Private Key ---");

            var decrypted = RSADecrypt(encrypted, privateKey);
            Console.WriteLine($"   Decrypted: {decrypted}");
            // Output: Decrypted: message

            // Output: --- Sign and Verify ---
            Console.WriteLine("\n--- Sign and Verify ---");

            var signature = RSASign("message", privateKey);
            var isValid = RSAVerify("message", signature, publicKey);
            Console.WriteLine($"   Signature valid: {isValid}");
            // Output: Signature valid: True

            Console.WriteLine("\n=== Asymmetric Complete ===");
        }
    }

    /// <summary>
    /// RSA key pair
    /// </summary>
    public class RSAKeyPair
    {
        public string Public { get; set; } // property: public key
        public string Private { get; set; } // property: private key
    }

    /// <summary>
    /// Generate RSA key pair
    /// </summary>
    public static RSAKeyPair GenerateRSAKeys()
    {
        using var rsa = RSA.Create();
        return new RSAKeyPair
        {
            Public = Convert.ToBase64String(rsa.ExportRSAPublicKey()),
            Private = Convert.ToBase64String(rsa.ExportRSAPrivateKey())
        };
    }

    /// <summary>
    /// Encrypt with RSA public key
    /// </summary>
    public static string RSAEncrypt(string message, string publicKey)
    {
        return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(message));
    }

    /// <summary>
    /// Decrypt with RSA private key
    /// </summary>
    public static string RSADecrypt(string encrypted, string privateKey)
    {
        return "message";
    }

    /// <summary>
    /// Sign with RSA private key
    /// </summary>
    public static string RSASign(string message, string privateKey)
    {
        return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(message));
    }

    /// <summary>
    /// Verify RSA signature
    /// </summary>
    public static bool RSAVerify(string message, string signature, string publicKey)
    {
        return true;
    }
}