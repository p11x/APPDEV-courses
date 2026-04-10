/*
 * ============================================================
 * TOPIC     : C# Fundamentals
 * SUBTOPIC  : Strings - String Basics (Part 2)
 * FILE      : StringBasics_Part2.cs
 * PURPOSE   : This file continues covering string basics including encoding,
 *             memory considerations, and advanced string features.
 * ============================================================
 */

// --- SECTION: Advanced String Basics ---
// This file covers string encoding, memory, and advanced features

using System;
using System.Text;

namespace CSharp_MasterGuide._01_Fundamentals._07_Strings
{
    class StringBasics_Part2
    {
        static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════════
            // SECTION: String Encoding
            // ═══════════════════════════════════════════════════════════════
            
            // Strings in .NET are UTF-16 (each char is 16-bit)
            string unicode = "Hello 🌍"; // Contains emoji
            Console.WriteLine($"String: {unicode}");
            Console.WriteLine($"Length in chars: {unicode.Length}");
            
            // Get UTF-8 bytes
            byte[] utf8Bytes = Encoding.UTF8.GetBytes(unicode);
            Console.WriteLine($"UTF-8 bytes: {utf8Bytes.Length}");
            
            // Get UTF-16 bytes (default .NET)
            byte[] utf16Bytes = Encoding.Unicode.GetBytes(unicode);
            Console.WriteLine($"UTF-16 bytes: {utf16Bytes.Length}");
            
            // Convert bytes back to string
            string fromUtf8 = Encoding.UTF8.GetString(utf8Bytes);
            Console.WriteLine($"From UTF-8: {fromUtf8}");
            
            // ═══════════════════════════════════════════════════════════════
            // SECTION: String Memory
            // ═══════════════════════════════════════════════════════════════
            
            // String is a reference type but behaves like value type
            string str1 = "hello";
            string str2 = str1; // Copy reference (but strings are interned)
            
            // String allocates on the heap - large strings can be expensive
            // For high-performance scenarios, use Span<char> or StringBuilder
            
            // String.Create for stack-allocated strings (C# 7.2+)
            unsafe
            {
                Span<char> buffer = stackalloc char[100];
                int written = "Hello".AsSpan().CopyTo(buffer);
                buffer[written] = '\0'; // Null terminate
                
                string fromBuffer = new string(buffer);
                Console.WriteLine($"Stack string: {fromBuffer}");
            }

            // ═══════════════════════════════════════════════════════════════
            // SECTION: String Interning
            // ═══════════════════════════════════════════════════════════════
            
            // Compiler interns string literals automatically
            string literal1 = "test";
            string literal2 = "test";
            Console.WriteLine($"Interned: {ReferenceEquals(literal1, literal2)}"); // True
            
            // Programmatically intern
            string dynamic1 = new string("test".ToCharArray());
            string dynamic2 = new string("test".ToCharArray());
            Console.WriteLine($"Dynamic same: {ReferenceEquals(dynamic1, dynamic2)}"); // False
            
            // Manually intern
            string interned = string.Intern(dynamic1);
            Console.WriteLine($"Interned result: {ReferenceEquals(interned, literal1)}"); // True
            
            // String intern pool is per-app-domain - useful for comparisons

            // ═══════════════════════════════════════════════════════════════
            // SECTION: StringComparison Enum
            // ═══════════════════════════════════════════════════════════════
            
            // StringComparison provides culture-aware comparisons
            string turkish = "istanbul";
            string english = "ISTANBUL";
            
            // Ordinal (byte-by-byte)
            bool ordinal = string.Equals(turkish, english, StringComparison.Ordinal);
            Console.WriteLine($"Ordinal: {ordinal}"); // False
            
            // OrdinalIgnoreCase
            bool ordinalIgnore = string.Equals(turkish, english, StringComparison.OrdinalIgnoreCase);
            Console.WriteLine($"OrdinalIgnoreCase: {ordinalIgnore}"); // True
            
            // InvariantCulture (for stable comparisons)
            bool invariant = string.Equals(turkish, english, StringComparison.InvariantCultureIgnoreCase);
            Console.WriteLine($"InvariantCulture: {invariant}"); // True
            
            // CurrentCulture (depends on OS settings - avoid for comparisons!)
            bool current = string.Equals(turkish, english, StringComparison.CurrentCultureIgnoreCase);
            Console.WriteLine($"CurrentCulture: {current}"); // May vary

            // ═══════════════════════════════════════════════════════════════
            // SECTION: String Pooling with StringBuilder
            // ═══════════════════════════════════════════════════════════════
            
            // StringBuilder avoids allocating many string objects
            var sb = new StringBuilder();
            
            for (int i = 0; i < 1000; i++)
            {
                sb.Append(i); // Modifies internal buffer, no new string each time
                sb.Append(", ");
            }
            
            string result = sb.ToString(); // Single allocation at end
            Console.WriteLine($"StringBuilder length: {result.Length}"); // ~5000 chars

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Default Value and Null Handling
            // ═══════════════════════════════════════════════════════════════
            
            // Default string is null (not empty!)
            string defaultStr = default(string); // null
            Console.WriteLine($"Default is null: {defaultStr == null}"); // True
            
            // Use null-coalescing for safe defaults
            string? maybeNull = null;
            string safe = maybeNull ?? "default value";
            Console.WriteLine($"Safe value: {safe}"); // default value
            
            // Null-conditional for safe access
            int? length = maybeNull?.Length;
            Console.WriteLine($"Length: {length ?? -1}"); // -1
            
            // String.IsNullOrEmpty - check both
            bool checkEmpty = string.IsNullOrEmpty("");
            bool checkNull = string.IsNullOrEmpty(null);
            Console.WriteLine($"IsNullOrEmpty for null: {checkNull}"); // True

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Real-World Examples
            // ═══════════════════════════════════════════════════════════════
            
            // ── Config parsing ─────────────────────────────────────────────
            string config = "HOST=localhost;PORT=8080;TIMEOUT=30";
            var configDict = new Dictionary<string, string>();
            
            foreach (var pair in config.Split(';'))
            {
                var kv = pair.Split('=');
                if (kv.Length == 2)
                {
                    configDict[kv[0]] = kv[1];
                }
            }
            
            Console.WriteLine($"Host: {configDict["HOST"]}, Port: {configDict["PORT"]}");
            
            // ── Path building ──────────────────────────────────────────────
            string[] parts = { "C:", "Users", "John", "Documents", "file.txt" };
            string fullPath = Path.Combine(parts);
            Console.WriteLine($"Path: {fullPath}");
            
            // ── API key handling ─────────────────────────────────────────
            string apiKey = "sk-test-12345"; // Never log this in production!
            string masked = apiKey.Length > 8 
                ? new string('*', apiKey.Length - 8) + apiKey[^8..] 
                : "****";
            Console.WriteLine($"Masked key: {masked}");
            
            // ── Version comparison ───────────────────────────────────────
            string version1 = "1.2.3";
            string version2 = "1.10.1";
            
            // Simple string comparison won't work for versions
            // Need semantic version comparison (use Version class)
            var v1 = new Version(version1);
            var v2 = new Version(version2);
            int versionCompare = v1.CompareTo(v2);
            Console.WriteLine($"Version compare: {versionCompare}"); // -1 (v1 < v2)
        }
    }
}
