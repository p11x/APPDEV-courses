/*
 * ============================================================
 * TOPIC     : C# Fundamentals
 * SUBTOPIC  : Strings - String Basics (Part 1)
 * FILE      : StringBasics.cs
 * PURPOSE   : This file covers string fundamentals in C#, including declaration,
 *             initialization, immutability, and basic properties.
 * ============================================================
 */

// --- SECTION: String Basics ---
// Strings in C# are immutable sequences of characters (Unicode)
// Once created, a string cannot be modified - all operations create new strings

using System;

namespace CSharp_MasterGuide._01_Fundamentals._07_Strings
{
    class StringBasics
    {
        static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════════
            // SECTION: String Declaration and Initialization
            // ═══════════════════════════════════════════════════════════════
            
            // ── String literals ─────────────────────────────────────────────
            string greeting = "Hello, World!"; // Most common way
            Console.WriteLine(greeting); // Output: Hello, World!
            
            // ── Empty string ───────────────────────────────────────────────
            string empty = ""; // Empty string (length 0)
            string alsoEmpty = string.Empty; // Same as ""
            Console.WriteLine($"Empty string length: {empty.Length}"); // Output: 0
            
            // ── Null string ──────────────────────────────────────────────────
            string? nullString = null; // Null - no value
            // Console.WriteLine(nullString.Length); // NullReferenceException!
            
            // ── String with special characters ──────────────────────────────
            string withQuote = "She said \"Hello\""; // Escape quotes
            string withNewline = "Line 1\nLine 2"; // Newline
            string withTab = "Col1\tCol2"; // Tab
            string path = "C:\\Users\\John\\Documents"; // Backslash escape
            string rawPath = @"C:\Users\John\Documents"; // Raw string (verbatim)
            
            Console.WriteLine(withQuote); // Output: She said "Hello"
            Console.WriteLine(withNewline); // Output: Line 1 (newline) Line 2
            Console.WriteLine(path); // Output: C:\Users\John\Documents
            
            // ═══════════════════════════════════════════════════════════════
            // SECTION: String Immutability
            // ═══════════════════════════════════════════════════════════════
            
            // Strings are immutable - operations create new strings
            string original = "Hello";
            string modified = original + " World"; // Creates new string
            modified = modified.ToUpper(); // Also creates new string
            
            Console.WriteLine($"Original: {original}"); // Output: Hello (unchanged!)
            Console.WriteLine($"Modified: {modified}"); // Output: HELLO WORLD
            
            // String intern pool - identical literals share memory
            string a = "hello";
            string b = "hello";
            bool sameReference = ReferenceEquals(a, b); // True - interned
            Console.WriteLine($"Same reference: {sameReference}"); // Output: True
            
            // New string from char array - different reference
            char[] chars = { 'h', 'e', 'l', 'l', 'o' };
            string fromChars = new string(chars);
            bool alsoInterned = ReferenceEquals(a, fromChars); // May be true
            Console.WriteLine($"Interned from array: {alsoInterned}"); // Varies by JIT

            // ═══════════════════════════════════════════════════════════════
            // SECTION: String Properties
            // ═══════════════════════════════════════════════════════════════
            
            string sample = "Hello, World!";
            
            // Length property
            Console.WriteLine($"Length: {sample.Length}"); // Output: 13
            
            // Indexer - access characters by position (0-based)
            char firstChar = sample[0]; // 'H'
            char lastChar = sample[sample.Length - 1]; // '!'
            Console.WriteLine($"First char: {firstChar}, Last char: {lastChar}"); // H, !
            
            // IsNullOrEmpty and IsNullOrWhiteSpace
            string? nullOrEmpty = null;
            bool isNullOrEmpty = string.IsNullOrEmpty(nullOrEmpty); // True
            bool isNullOrWhite = string.IsNullOrWhiteSpace("  "); // True
            Console.WriteLine($"Is null or empty: {isNullOrEmpty}"); // Output: True

            // ═══════════════════════════════════════════════════════════════
            // SECTION: String Concatenation
            // ═══════════════════════════════════════════════════════════════
            
            // Using + operator
            string first = "Hello";
            string second = "World";
            string combined = first + " " + second;
            Console.WriteLine(combined); // Output: Hello World
            
            // String.Concat
            string concatResult = string.Concat(first, " ", second);
            Console.WriteLine(concatResult); // Output: Hello World
            
            // String.Join
            string[] words = { "Apple", "Banana", "Cherry" };
            string joined = string.Join(", ", words);
            Console.WriteLine(joined); // Output: Apple, Banana, Cherry
            
            // StringBuilder is more efficient for many concatenations
            // (see StringBuilderClass.cs)
            
            // Format
            string formatted = string.Format("Hello {0}! Your score is {1}.", "Player", 1000);
            Console.WriteLine(formatted); // Output: Hello Player! Your score is 1000.

            // ═══════════════════════════════════════════════════════════════
            // SECTION: String Comparison
            // ═══════════════════════════════════════════════════════════════
            
            string s1 = "hello";
            string s2 = "HELLO";
            string s3 = "hello";
            
            // == is case-sensitive for strings
            bool equal = s1 == s3; // True
            bool equalCase = s1 == s2; // False (case-sensitive)
            Console.WriteLine($"s1 == s3: {equal}"); // Output: True
            Console.WriteLine($"s1 == s2: {equalCase}"); // Output: False
            
            // String.Compare
            int cmpResult = string.Compare(s1, s2, true); // Ignore case
            Console.WriteLine($"Compare (ignore case): {cmpResult}"); // Output: 0 (equal)
            
            cmpResult = string.Compare(s1, s2, false); // Case-sensitive
            Console.WriteLine($"Compare (case): {cmpResult}"); // Output: -1 (s1 < s2)
            
            // Equals with StringComparison
            bool caseInsensitive = s1.Equals(s2, StringComparison.OrdinalIgnoreCase);
            bool caseSensitive = s1.Equals(s2, StringComparison.Ordinal);
            Console.WriteLine($"Equals ignore case: {caseInsensitive}"); // Output: True
            Console.WriteLine($"Equals case: {caseSensitive}"); // Output: False

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Real-World Examples
            // ═══════════════════════════════════════════════════════════════
            
            // ── User input sanitization ────────────────────────────────────
            string? userInput = "  John  ";
            string trimmed = userInput.Trim(); // Remove leading/trailing whitespace
            string sanitized = trimmed.Replace(" ", "_"); // Replace spaces
            Console.WriteLine($"Sanitized: {sanitized}"); // Output: John_
            
            // ── Building URLs ──────────────────────────────────────────────
            string baseUrl = "https://api.example.com";
            string endpoint = "users";
            string id = "123";
            
            string url = $"{baseUrl}/{endpoint}/{id}";
            Console.WriteLine($"URL: {url}");
            
            // ── Logging ───────────────────────────────────────────────────
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string logMessage = $"[{timestamp}] Application started";
            Console.WriteLine(logMessage);
        }
    }
}
