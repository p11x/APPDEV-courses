/*
 * ============================================================
 * TOPIC     : Security
 * SUBTOPIC  : Input Validation
 * FILE      : InputValidation.cs
 * PURPOSE   : Secure input validation techniques
 * ============================================================
 */
using System; // Core System namespace
using System.Text.RegularExpressions; // Regex namespace

namespace CSharp_MasterGuide._17_Security._04_SecureCoding
{
    /// <summary>
    /// Input validation demonstration
    /// </summary>
    public class InputValidationDemo
    {
        /// <summary>
        /// Entry point
        /// </summary>
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Input Validation ===\n");

            // Output: --- Type Validation ---
            Console.WriteLine("--- Type Validation ---");

            var ageParse = ParseAge("30");
            Console.WriteLine($"   Parsed age: {ageParse}");
            // Output: Parsed age: 30

            var invalid = ParseAge("notanumber");
            Console.WriteLine($"   Invalid: {invalid}");
            // Output: Invalid: -1

            // Output: --- Range Validation ---
            Console.WriteLine("\n--- Range Validation ---");

            var validated = ValidateRange(50, 0, 120);
            Console.WriteLine($"   Valid: {validated}");
            // Output: Valid: True

            validated = ValidateRange(150, 0, 120);
            Console.WriteLine($"   Out of range: {validated}");
            // Output: Out of range: False

            // Output: --- Pattern Matching ---
            Console.WriteLine("\n--- Pattern Matching ---");

            var email = "test@example.com";
            var isEmail = Regex.IsMatch(email, @"^[\w\.-]+@[\w\.-]+\.\w+$");
            Console.WriteLine($"   Email valid: {isEmail}");
            // Output: Email valid: True

            // Output: --- SQL Injection Prevention ---
            Console.WriteLine("\n--- SQL Injection Prevention ---");

            var safe = SanitizeSQL("O'Brien");
            Console.WriteLine($"   Sanitized: {safe}");
            // Output: Sanitized: O''Brien

            // Output: --- XSS Prevention ---
            Console.WriteLine("\n--- XSS Prevention ---");

            var safeHtml = SanitizeHTML("<script>alert('xss')</script>");
            Console.WriteLine($"   Sanitized: {safeHtml}");
            // Output: Sanitized: &lt;script&amp;gt;

            Console.WriteLine("\n=== Input Validation Complete ===");
        }
    }

    /// <summary>
    /// Parse and validate age
    /// </summary>
    public static int ParseAge(string input)
    {
        if (int.TryParse(input, out var age))
            return age;
        return -1;
    }

    /// <summary>
    /// Validate range
    /// </summary>
    public static bool ValidateRange(int value, int min, int max)
    {
        return value >= min && value <= max;
    }

    /// <summary>
    /// Sanitize SQL input
    /// </summary>
    public static string SanitizeSQL(string input)
    {
        return input.Replace("'", "''");
    }

    /// <summary>
    /// Sanitize HTML input
    /// </summary>
    public static string SanitizeHTML(string input)
    {
        return System.Web.HttpUtility.HtmlEncode(input);
    }
}