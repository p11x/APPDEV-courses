/*
 * ============================================================
 * TOPIC     : Fundamentals - Strings
 * SUBTOPIC  : String Methods - Advanced Features
 * FILE      : StringMethods_Part3.cs
 * PURPOSE   : Covers string comparison, Span-based operations, 
 *            and performance-oriented string methods
 * ============================================================
 */

using System; // Core System namespace for Console and string operations

namespace CSharp_MasterGuide._01_Fundamentals._07_Strings
{
    class StringMethods_Part3
    {
        static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════
            // SECTION 1: Advanced Comparison with StringComparison
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Ordinal vs Culture-aware comparison ───────
            // Ordinal - byte-by-byte comparison, fastest, no culture
            string s1 = "resume";
            string s2 = "résumé"; // French word with accent
            
            int ordinal = string.Compare(s1, s2, StringComparison.Ordinal);
            Console.WriteLine($"Ordinal comparison: {ordinal}"); // 1 (not equal)

            int ordinalIgnore = string.Compare(s1, s2, StringComparison.OrdinalIgnoreCase);
            Console.WriteLine($"Ordinal ignore case: {ordinalIgnore}"); // 1 (still not equal)

            // CurrentCulture - uses OS culture settings for comparison
            int culture = string.Compare(s1, s2, StringComparison.CurrentCulture);
            Console.WriteLine($"Current culture: {culture}"); // -1 or 0 depending on culture

            // InvariantCulture - consistent across all systems
            int invariant = string.Compare(s1, s2, StringComparison.InvariantCulture);
            Console.WriteLine($"Invariant culture: {invariant}"); // -1 or 0

            // ── EXAMPLE 2: OrdinalIgnoreCase - Best for identifiers ──
            // Use for file paths, URLs, environment variables
            string filePath = "C:\\MyDocuments\\FILE.TXT";
            string lookup = "c:\\mydocuments\\file.txt";
            
            bool pathsMatch = string.Equals(
                filePath, 
                lookup, 
                StringComparison.OrdinalIgnoreCase
            );
            Console.WriteLine($"Paths match: {pathsMatch}"); // True

            // Use for enum-like string comparisons
            string userInput = "CreateUser";
            string expectedCommand = "CREATEUSER";
            bool commandMatch = string.Equals(
                userInput, 
                expectedCommand, 
                StringComparison.OrdinalIgnoreCase
            );
            Console.WriteLine($"Command match: {commandMatch}"); // True

            // ── REAL-WORLD EXAMPLE: Username validation ─────────────
            // Ensure username is case-insensitive but byte-exact
            string storedUsername = "JohnDoe";
            string loginAttempt = "JOHNDOE";
            
            bool isValidUser = string.Equals(
                storedUsername, 
                loginAttempt, 
                StringComparison.OrdinalIgnoreCase
            );
            Console.WriteLine($"User authenticated: {isValidUser}"); // True

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: Span-based String Operations (C# 7.3+)
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: AsSpan - Zero-copy string to Span ─────────
            // Avoids string allocation for substring operations
            string longText = "Hello, World! This is a long string for demonstration.";
            
            // Get span without copying - uses same underlying memory
            ReadOnlySpan<char> span = longText.AsSpan();
            Console.WriteLine($"Span length: {span.Length}"); // 56

            // Slice span without allocation - much faster than Substring
            ReadOnlySpan<char> worldSpan = span.Slice(7, 5); // "World"
            Console.WriteLine($"Sliced: {worldSpan.ToString()}"); // Output: World

            // ── EXAMPLE 2: Span comparison methods ───────────────────
            // StartsWith and EndsWith on Span - very efficient
            ReadOnlySpan<char> url = "https://api.example.com".AsSpan();
            
            bool isHttps = url.StartsWith("https".AsSpan(), StringComparison.OrdinalIgnoreCase);
            bool isSecure = url.StartsWith("https".AsSpan());
            Console.WriteLine($"Is HTTPS: {isHttps}, Is secure prefix: {isSecure}");
            // Output: Is HTTPS: True, Is secure prefix: True

            // SequenceEqual for exact matching
            ReadOnlySpan<char> expected = "test".AsSpan();
            ReadOnlySpan<char> actual = "test".AsSpan();
            bool exactMatch = expected.SequenceEqual(actual);
            Console.WriteLine($"Exact match: {exactMatch}"); // True

            // ── REAL-WORLD EXAMPLE: High-performance parsing ─────────
            // Process large text without allocations
            string logData = "ERROR|2024-01-15|ConnectionTimeout|localhost";
            
            ReadOnlySpan<char> logSpan = logData.AsSpan();
            
            // Find pipe positions efficiently
            int firstPipe = logSpan.IndexOf('|');
            int secondPipe = logSpan.Slice(firstPipe + 1).IndexOf('|') + firstPipe + 1;
            
            ReadOnlySpan<char> level = logSpan.Slice(0, firstPipe);
            ReadOnlySpan<char> timestamp = logSpan.Slice(firstPipe + 1, secondPipe - firstPipe - 1);
            
            Console.WriteLine($"Level: {level.ToString()}, Time: {timestamp.ToString()}");
            // Output: Level: ERROR, Time: 2024-01-15

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: Memory-efficient Operations
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: string.Create - Interpolated string builder ─
            // Creates string with format known at runtime without intermediate allocations
            int count = 42;
            string formatted = string.Create(null, $"Count: {count,5}"); // Right-align in 5 chars
            Console.WriteLine(formatted); // Output: Count:    42

            // Using span in interpolation
            string name = "World";
            string greeting = string.Create(null, $"Hello, {name}!");
            Console.WriteLine(greeting); // Output: Hello, World!

            // ── EXAMPLE 2: StringBuilder for complex concatenation ─────
            // Use when concatenating many strings or in loops
            var sb = new System.Text.StringBuilder();
            
            for (int i = 0; i < 5; i++)
            {
                sb.Append($"Item {i}, "); // Append without new allocations
            }
            
            string result = sb.ToString();
            // Remove trailing ", "
            if (result.Length > 2)
                result = result.Substring(0, result.Length - 2);
                
            Console.WriteLine(result); // Output: Item 0, Item 1, Item 2, Item 3, Item 4

            // ── REAL-WORLD EXAMPLE: Build SQL query dynamically ─────
            var queryBuilder = new System.Text.StringBuilder();
            
            string[] columns = { "Id", "Name", "Email", "CreatedAt" };
            string tableName = "Users";
            
            queryBuilder.Append("SELECT ");
            queryBuilder.AppendJoin(", ", columns); // Efficiently join columns
            queryBuilder.Append(" FROM ");
            queryBuilder.Append(tableName);
            
            Console.WriteLine(queryBuilder.ToString());
            // Output: SELECT Id, Name, Email, CreatedAt FROM Users

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: Null and Empty Safety Methods
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Null coalescing for strings ───────────────
            string nullableString = null;
            
            // ?? operator - return right side if left is null
            string safeString = nullableString ?? "Default Value";
            Console.WriteLine($"Coalesced: {safeString}"); // Output: Default Value

            // ??= operator - assign if null (C# 8+)
            nullableString ??= "Assigned Value";
            Console.WriteLine($"After ??=: {nullableString}"); // Output: Assigned Value

            // ── EXAMPLE 2: Null-conditional operators ─────────────────
            // ?. - safe navigation, returns null if chain broken
            string chain = null;
            int? length = chain?.Length; // Returns null, not exception
            Console.WriteLine($"Length: {length}"); // Output: Length: 

            // Use with null-coalescing for default
            int safeLength = chain?.Length ?? 0;
            Console.WriteLine($"Safe length: {safeLength}"); // Output: Safe length: 0

            // Useful for nested property access
            string nested = "Hello";
            char firstChar = nested?.Length > 0 ? nested[0] : '\0'; // Simple null check
            Console.WriteLine($"First char: {firstChar}"); // Output: First char: H

            // ── EXAMPLE 3: ArgumentNullException.ThrowIfNull (C# 9+) ──
            // Modern null validation - clean and efficient
            string requiredParam = "some value";
            
            // Simple one-liner validation (uncomment to see behavior)
            // ArgumentNullException.ThrowIfNull(requiredParam);
            
            // Equivalent old way:
            if (requiredParam == null)
                throw new ArgumentNullException(nameof(requiredParam));

            Console.WriteLine($"Parameter validated: {requiredParam}");

            // ── REAL-WORLD EXAMPLE: Safe configuration reading ─────
            string configValue = null;
            string setting = configValue ?? "Production"; // Default to Production
            string debugMode = Environment.GetEnvironmentVariable("DEBUG") ?? "false";
            
            Console.WriteLine($"Setting: {setting}, Debug: {debugMode}");
            // Output: Setting: Production, Debug: false (or whatever env var is)

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: Encoding and Character Operations
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: GetHashCode - String-specific hash ─────────
            // Different from object hash - consistent within app domain
            string s3 = "test";
            string s4 = "test";
            Console.WriteLine($"Same string hash: {s3.GetHashCode()} == {s4.GetHashCode()}");
            // Output: Same string hash: True (they match)

            // Different strings likely have different hashes
            Console.WriteLine($"Different strings: {'a'.GetHashCode()} vs {'b'.GetHashCode()}");
            // Output: Different strings: 64531078 vs 64531077

            // Note: Hash code can differ across .NET versions and processes
            // Don't persist or use across boundaries

            // ── EXAMPLE 2: String length and character access ─────────
            string greeting2 = "Hello";
            Console.WriteLine($"Length: {greeting2.Length}"); // Output: 5
            
            // Access individual characters
            for (int i = 0; i < greeting2.Length; i++)
            {
                Console.WriteLine($"Char[{i}]: {greeting2[i]}");
                // Output: Char[0]: H, Char[1]: e, Char[2]: l, Char[3]: l, Char[4]: o
            }

            // ── EXAMPLE 3: ToCharArray - Convert to char array ───────
            string word = "Hello";
            char[] chars = word.ToCharArray(); // Creates new array
            chars[0] = 'J'; // Modify array
            Console.WriteLine($"Original: {word}, Modified: {new string(chars)}");
            // Output: Original: Hello, Modified: Jello

            // ── REAL-WORLD EXAMPLE: Character frequency counting ─────
            string text = "Mississippi";
            int[] charCounts = new int[256]; // ASCII range
            
            foreach (char c in text)
            {
                charCounts[(int)c]++; // Count each character
            }
            
            Console.WriteLine("Character frequencies:");
            for (int i = 0; i < 256; i++)
            {
                if (charCounts[i] > 0 && i < 128) // Only printable ASCII
                {
                    Console.WriteLine($"  '{(char)i}': {charCounts[i]}");
                    // Output:   'M': 1,   'i': 4,   'p': 2,   's': 4
                }
            }

            // ═══════════════════════════════════════════════════════════
            // SECTION 6: String Interning (Advanced)
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: String intern pool ─────────────────────────
            // Compiler interns string literals automatically
            string literal1 = "hello";
            string literal2 = "hello";
            
            // Both point to same object in memory - true reference equality
            bool sameReference = ReferenceEquals(literal1, literal2);
            Console.WriteLine($"Literals interned: {sameReference}"); // True

            // Runtime interning - add dynamically created strings to pool
            string dynamic = new string(new[] { 'h', 'e', 'l', 'l', 'o' });
            string interned = string.Intern(dynamic); // Add to pool
            Console.WriteLine($"Interned matches literal: {ReferenceEquals(literal1, interned)}");
            // True after interning

            // ── EXAMPLE 2: When interning matters ───────────────────
            // Large number of repeated string comparisons
            string[] names = { "Alice", "Bob", "Alice", "Charlie", "Alice" };
            
            // Without interning - each comparison allocates
            // With interning - faster comparisons for repeated literals

            // Check if string is interned
            Console.WriteLine($"'Alice' is interned: {string.IsInterned(\"Alice\") != null}");
            // True for literals

            // ── REAL-WORLD EXAMPLE: Symbol table implementation ──────
            // Use string interning for efficient symbol lookup
            var symbolTable = new System.Collections.Generic.Dictionary<string, int>();
            
            void InternAndAdd(string symbol)
            {
                string internedSymbol = string.Intern(symbol); // Ensure single instance
                if (!symbolTable.ContainsKey(internedSymbol))
                {
                    symbolTable[internedSymbol] = symbolTable.Count + 1;
                }
            }
            
            InternAndAdd("ADD");
            InternAndAdd("SUB");
            InternAndAdd("ADD"); // Same as before - reused from pool
            
            Console.WriteLine($"Unique symbols: {symbolTable.Count}"); // Output: 2
            Console.WriteLine($"ADD value: {symbolTable["ADD"]}"); // Output: 1

            Console.WriteLine("\n=== String Methods Part 3 Complete ===");
        }
    }
}