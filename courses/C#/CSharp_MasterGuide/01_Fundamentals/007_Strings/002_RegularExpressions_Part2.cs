/*
 * ============================================================
 * TOPIC     : Fundamentals - Strings
 * SUBTOPIC  : Regular Expressions - Advanced Features
 * FILE      : RegularExpressions_Part2.cs
 * PURPOSE   : Advanced regex features including lookahead/lookbehind,
 *            lazy quantifiers, options, and performance optimization
 * ============================================================
 */

using System; // Core System namespace
using System.Text.RegularExpressions; // Required for regex operations
using System.Diagnostics; // For Stopwatch for performance testing

namespace CSharp_MasterGuide._01_Fundamentals._07_Strings
{
    class RegularExpressions_Part2
    {
        static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════
            // SECTION 1: Lookahead and Lookbehind
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Positive lookahead ──────────────────────────
            // (?=pattern) - assert what follows matches pattern
            // Match password that is followed by "confirm"
            string text1 = "password123confirm password456";
            MatchCollection passMatches = Regex.Matches(text1, @"\w+(?=\s+confirm)");
            
            foreach (Match m in passMatches)
            {
                Console.WriteLine($"Password before confirm: {m.Value}");
            }
            // Output: Password before confirm: password123

            // Match word followed by specific character
            string text2 = "test tests testing tested";
            MatchCollection testMatches = Regex.Matches(text2, @"\w+(?=ing)");
            
            foreach (Match m in testMatches)
            {
                Console.WriteLine($"Word ending in 'ing': {m.Value}");
            }
            // Output: Word ending in 'ing': test

            // ── EXAMPLE 2: Negative lookahead ──────────────────────────
            // (?!pattern) - assert what follows does NOT match
            // Match words not followed by "ing"
            string words = "test running walking jumping";
            MatchCollection nonIng = Regex.Matches(words, @"\b\w+\b(?!ing)");
            
            foreach (Match m in nonIng)
            {
                Console.WriteLine($"Word not ending in 'ing': {m.Value}");
            }
            // Output: test, running, jumping (not walking)

            // Match file extensions except .exe
            string files = "document.txt image.jpg app.exe data.exe";
            MatchCollection notExe = Regex.Matches(files, @"\w+\.(?!exe)\w+");
            
            foreach (Match m in notExe)
            {
                Console.WriteLine($"Not .exe: {m.Value}");
            }
            // Output: document.txt, image.jpg

            // ── EXAMPLE 3: Positive lookbehind ────────────────────────
            // (?<=pattern) - assert what precedes matches pattern
            // Match price after $ symbol
            string prices = "Item1 $10 Item2 $20";
            MatchCollection dollarPrices = Regex.Matches(prices, @"(?<=\$)\d+");
            
            foreach (Match m in dollarPrices)
            {
                Console.WriteLine($"Price: {m.Value}");
            }
            // Output: 10, 20

            // Match word after "the"
            string sentence = "the cat the dog the bird";
            MatchCollection afterThe = Regex.Matches(sentence, @"(?<=the\s)\w+");
            
            foreach (Match m in afterThe)
            {
                Console.WriteLine($"Word after 'the': {m.Value}");
            }
            // Output: cat, dog, bird

            // ── EXAMPLE 4: Negative lookbehind ────────────────────────
            // (?<!pattern) - assert what precedes does NOT match
            // Match digit not preceded by $
            string mixed = "$10 20 $30 40";
            MatchCollection notDollar = Regex.Matches(mixed, @"(?<!\$)\d+");
            
            foreach (Match m in notDollar)
            {
                Console.WriteLine($"Not after $: {m.Value}");
            }
            // Output: 10, 30, 40 (not 20 because it's after space)

            // Match version numbers not preceded by "v"
            string versions = "v1.0 2.0 v3.0 4.0";
            MatchCollection noV = Regex.Matches(versions, @"(?<!v)\d+\.\d+");
            
            foreach (Match m in noV)
            {
                Console.WriteLine($"Version without v: {m.Value}");
            }

            // ── REAL-WORLD EXAMPLE: Complex validation ───────────────
            // Validate username: alphanumeric, 3-20 chars, no "admin"
            string[] usernames = { "john_doe", "admin123", "ab", "validuser123" };
            
            string userPattern = @"^(?![aA]dmin)\w{3,20}$";
            
            foreach (string user in usernames)
            {
                bool valid = Regex.IsMatch(user, userPattern);
                Console.WriteLine($"'{user}': {(valid ? "Valid" : "Invalid")}");
                // Output: 'john_doe': Valid
                // Output: 'admin123': Invalid (starts with admin)
                // Output: 'ab': Invalid (too short)
                // Output: 'validuser123': Valid
            }

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: Lazy Quantifiers and Backtracking
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Greedy vs Lazy quantifiers ──────────────────
            // Greedy: * + ? {n,} - matches as much as possible
            // Lazy: *? +? ?? {n,}? - matches as little as possible
            
            string html = "<div>content</div>";
            
            // Greedy .* - matches as much as possible
            string greedy = Regex.Match(html, @"<.*>").Value;
            Console.WriteLine($"Greedy: {greedy}"); // "<div>content</div>" (entire tag!)
            
            // Lazy .*? - matches as little as possible
            string lazy = Regex.Match(html, @"<.*?>").Value;
            Console.WriteLine($"Lazy: {lazy}"); // "<div>"
            
            // Practical example - extract first link
            string page = "<a href='url1'>Link1</a> <a href='url2'>Link2</a>";
            string firstLink = Regex.Match(page, @"href='([^']+)'").Groups[1].Value;
            Console.WriteLine($"First link: {firstLink}"); // "url1"

            // ── EXAMPLE 2: Lazy with alternation ───────────────────────
            // Use lazy to match minimal content
            string longText = "startimportantdataendjunkend";
            
            // Greedy - matches from first "start" to last "end"
            string greedyMatch = Regex.Match(longText, @"start.*end").Value;
            Console.WriteLine($"Greedy: {greedyMatch}"); // "startimportantdataendjunkend"
            
            // Lazy - matches from first "start" to first "end"
            string lazyMatch = Regex.Match(longText, @"start.*?end").Value;
            Console.WriteLine($"Lazy: {lazyMatch}"); // "startimportantdataend"

            // ── REAL-WORLD EXAMPLE: Parse quoted strings ─────────────
            // Extract content between quotes (first pair only)
            string quoted = "\"First quote\" then \"Second quote\" then \"Third\"";
            
            // Non-greedy match for content between quotes
            string firstQuoted = Regex.Match(quoted, "\"(.*?)\"").Groups[1].Value;
            Console.WriteLine($"First quote: {firstQuoted}"); // "First quote"
            
            // Get all quoted strings
            MatchCollection allQuotes = Regex.Matches(quoted, "\"(.*?)\"");
            Console.WriteLine("All quotes:");
            foreach (Match m in allQuotes)
            {
                Console.WriteLine($"  {m.Groups[1].Value}");
            }
            // Output:   First quote
            // Output:   Second quote
            // Output:   Third

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: Regex Options
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: IgnoreCase ─────────────────────────────────
            string mixedCase = "Hello HELLO heLLo";
            MatchCollection ignoreCase = Regex.Matches(mixedCase, "hello", RegexOptions.IgnoreCase);
            Console.WriteLine($"IgnoreCase matches: {ignoreCase.Count}"); // 3

            // ── EXAMPLE 2: Multiline ───────────────────────────────────
            string multiline = "first line\nsecond line\nthird line";
            
            // Default - ^ and $ match start/end of entire string
            Match defaultMatch = Regex.Match(multiline, @"^\w+");
            Console.WriteLine($"Default: {defaultMatch.Value}"); // "first"
            
            // Multiline - ^ and $ match start/end of each line
            MatchCollection multiMatches = Regex.Matches(multiline, @"^\w+", RegexOptions.Multiline);
            Console.WriteLine("Multiline matches:");
            foreach (Match m in multiMatches)
            {
                Console.WriteLine($"  {m.Value}");
            }
            // Output:   first, second, third

            // ── EXAMPLE 3: Singleline ( dot matches newline) ─────────
            string withNewline = "Hello\nWorld";
            
            // Default - . doesn't match newline
            bool defaultDot = Regex.IsMatch(withNewline, @"Hello.World"); // False
            
            // Singleline - . matches newline
            bool singleDot = Regex.IsMatch(withNewline, @"Hello.World", RegexOptions.Singleline);
            Console.WriteLine($"SingleLine dot: {singleDot}"); // True

            // ── EXAMPLE 4: Compiled regex for performance ─────────────
            // RegexOptions.Compile - compile to native code for speed
            string testString = "test123abc456def789";
            
            // Without Compile - interpreted
            var options1 = RegexOptions.None;
            string pattern1 = @"\d+";
            
            // With Compile - compiled to IL
            var options2 = RegexOptions.Compiled;
            
            // First call includes JIT overhead, subsequent faster
            Stopwatch sw = new Stopwatch();
            
            sw.Start();
            for (int i = 0; i < 10000; i++)
            {
                Regex.Match(testString, pattern1);
            }
            sw.Stop();
            Console.WriteLine($"Normal: {sw.ElapsedMilliseconds}ms");
            
            sw.Restart();
            for (int i = 0; i < 10000; i++)
            {
                Regex.Match(testString, pattern1, options2);
            }
            sw.Stop();
            Console.WriteLine($"Compiled: {sw.ElapsedMilliseconds}ms");

            // ── REAL-WORLD EXAMPLE: Complex multiline parsing ───────
            string codeBlock = @"
function foo() {
    // comment
    let x = 1;
    return x;
}
function bar() {
    return 2;
}";

            // Match all function names with multiline option
            Regex funcPattern = new Regex(@"function\s+(\w+)", RegexOptions.Multiline);
            MatchCollection functions = funcPattern.Matches(codeBlock);
            
            Console.WriteLine("Functions found:");
            foreach (Match f in functions)
            {
                Console.WriteLine($"  {f.Groups[1].Value}");
            }
            // Output:   foo, bar

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: Regex Timeout and Safety
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Setting timeout for complex regex ─────────
            // Prevent catastrophic backtracking
            string badInput = new string('a', 30) + "b"; // 30 a's then b
            
            try
            {
                // Timeout after 1 second - prevents hanging
                Match m = Regex.Match(badInput, @"^(a+)+b", RegexOptions.None, TimeSpan.FromSeconds(1));
                Console.WriteLine($"Match found: {m.Success}");
            }
            catch (RegexMatchTimeoutException ex)
            {
                Console.WriteLine($"Timeout: {ex.Message}");
            }

            // ── EXAMPLE 2: Safer patterns to avoid backtracking ──────
            // Instead of (a+)+b use a++b or atomic groups where possible
            string safePattern = @"a++b"; // Possessive quantifier (avoids backtracking)
            // Note: In .NET, possessive quantifiers aren't native but we can simulate
            
            // Better approach: avoid nested quantifiers
            string betterInput = "test data";
            bool result = Regex.IsMatch(betterInput, @"^[a-z]+\d+$"); // Simple pattern
            Console.WriteLine($"Safe pattern result: {result}"); // False

            // ── REAL-WORLD EXAMPLE: Safe user input regex ───────────
            // Always use timeout in production for user-provided patterns
            string userInput = "some input";
            
            bool SafeMatch(string input, string pattern, int timeoutMs = 1000)
            {
                try
                {
                    return Regex.IsMatch(input, pattern, RegexOptions.None, 
                                         TimeSpan.FromMilliseconds(timeoutMs));
                }
                catch (RegexMatchTimeoutException)
                {
                    return false; // Treat timeout as no match
                }
            }
            
            Console.WriteLine($"User input safe: {SafeMatch(userInput, @".*")}"); // True
            Console.WriteLine($"User input safe2: {SafeMatch(userInput, @"^[\w\s]+$")}"); // True

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: Balanced Groups and Recursion
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Balancing groups (limited support) ───────────
            // Not fully supported in .NET, but we can simulate
            
            // Match content inside quotes
            string quoted2 = "\"Hello World\" and 'Single quotes'";
            
            // Match double-quoted content
            string doubleQuoted = Regex.Match(quoted2, "\"[^\"]+\"").Value;
            Console.WriteLine($"Double quoted: {doubleQuoted}"); // "Hello World"
            
            // Match single-quoted content  
            string singleQuoted = Regex.Match(quoted2, "'[^']+'").Value;
            Console.WriteLine($"Single quoted: {singleQuoted}"); // 'Single quotes'

            // ── EXAMPLE 2: Match repeated pattern ─────────────────────
            // Match same word repeated
            string repeated = "test test test";
            Match repeatedMatch = Regex.Match(repeated, @"(\w+)(?:\s+\1)+");
            Console.WriteLine($"Repeated word: {repeatedMatch.Groups[1].Value}");
            // Output: test

            // Capture all repetitions
            string duplicates = "cat cat dog bird bird bird fish";
            MatchCollection dupMatches = Regex.Matches(duplicates, @"(\w+)(?:\s+\1)+");
            
            foreach (Match dm in dupMatches)
            {
                Console.WriteLine($"Duplicate: {dm.Groups[1].Value} ({dm.Groups[0].Value})");
            }
            // Output: cat (cat cat), bird (bird bird bird)

            // ── REAL-WORLD EXAMPLE: Extract numbers with signs ───────
            string math = "+5 -10 +20 -30";
            Regex signPattern = new Regex(@"([+-]?\d+)");
            MatchCollection numbers = signPattern.Matches(math);
            
            Console.WriteLine("Numbers with signs:");
            foreach (Match n in numbers)
            {
                Console.WriteLine($"  {n.Groups[1].Value}");
            }
            // Output: +5, -10, +20, -30

            // ═══════════════════════════════════════════════════════════
            // SECTION 6: Practical Advanced Patterns
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: IPv4 address validation ─────────────────────
            string[] ips = { "192.168.1.1", "256.1.1.1", "1.2.3.4", "192.168.1" };
            
            string ipPattern = @"^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$";
            
            foreach (string ip in ips)
            {
                bool valid = Regex.IsMatch(ip, ipPattern);
                Console.WriteLine($"{ip}: {(valid ? "Valid" : "Invalid")}");
                // Output: 192.168.1.1: Valid
                // Output: 256.1.1.1: Invalid
                // Output: 1.2.3.4: Valid
                // Output: 192.168.1: Invalid
            }

            // ── EXAMPLE 2: Credit card number mask ───────────────────
            string[] cards = { "4111111111111111", "5500000000000004" };
            
            string maskPattern = @"^(\d{4})\d{8}(\d{4})$";
            
            foreach (string card in cards)
            {
                string masked = Regex.Replace(card, maskPattern, "$1****$2");
                Console.WriteLine($"Card: {masked}");
                // Output: 4111****1111
                // Output: 5500****0004
            }

            // ── EXAMPLE 3: Extract and validate URL ──────────────────
            string urlText = "Visit https://example.com/page?id=123 or http://test.org";
            
            // Extract URLs
            Regex urlPattern = new Regex(@"(https?://[^\s]+)");
            MatchCollection urls = urlPattern.Matches(urlText);
            
            Console.WriteLine("URLs found:");
            foreach (Match u in urls)
            {
                string url = u.Groups[1].Value;
                Console.WriteLine($"  {url}");
                
                // Validate URL has valid scheme and domain
                bool validUrl = Regex.IsMatch(url, @"^https?://[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}/?.*$");
                Console.WriteLine($"    Valid: {validUrl}");
            }

            // ── EXAMPLE 4: Parse SQL-like where clause ───────────────
            string whereClause = "name = 'John' AND age > 25 AND status = 'active'";
            
            // Split into conditions
            string[] conditions = Regex.Split(whereClause, @"\s+AND\s+");
            
            Regex fieldPattern = new Regex(@"(\w+)\s*(=|>|<|>=|<=|!=)\s*('?[^']+'?)$");
            
            Console.WriteLine("Conditions parsed:");
            foreach (string cond in conditions)
            {
                Match cm = fieldPattern.Match(cond.Trim());
                if (cm.Success)
                {
                    Console.WriteLine($"  Field: {cm.Groups[1].Value}");
                    Console.WriteLine($"  Op: {cm.Groups[2].Value}");
                    Console.WriteLine($"  Value: {cm.Groups[3].Value}");
                }
            }

            // ═══════════════════════════════════════════════════════════
            // SECTION 7: Performance Best Practices
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Pre-compile regex for reuse ─────────────────
            // Create Regex instance once, reuse
            Regex precompiled = new Regex(@"\d+", RegexOptions.Compiled);
            
            string[] testStrings = { "abc123", "def456", "ghi789" };
            
            foreach (string s in testStrings)
            {
                Match m = precompiled.Match(s);
                Console.WriteLine($"{s}: {m.Value}");
            }

            // ── EXAMPLE 2: Use compiled with timeout for safety ────
            var safeCompiled = new Regex(@"\d+", 
                RegexOptions.Compiled, 
                TimeSpan.FromSeconds(1));
            
            Console.WriteLine($"Safe compiled match: {safeCompiled.Match("test123").Value}");

            // ── EXAMPLE 3: Cache small patterns when possible ───────
            // For very simple patterns, inline is fine
            // For patterns used repeatedly, create and cache Regex object
            
            var commonPatterns = new System.Collections.Generic.Dictionary<string, Regex>();
            
            string GetOrCreatePattern(string pattern)
            {
                if (!commonPatterns.ContainsKey(pattern))
                {
                    commonPatterns[pattern] = new Regex(pattern, RegexOptions.Compiled);
                }
                return commonPatterns[pattern].Match("").Value; // Just trigger creation
            }
            
            Console.WriteLine("Pattern caching ready for reuse");

            // ── REAL-WORLD EXAMPLE: High-performance parsing ───────────
            // Pre-compile common patterns
            Regex emailRegex = new Regex(@"^[\w\.-]+@[\w\.-]+\.\w+$", RegexOptions.Compiled);
            Regex phoneRegex = new Regex(@"^\(?([0-9]{3})\)?[-.\s]?([0-9]{3})[-.\s]?([0-9]{4})$", RegexOptions.Compiled);
            Regex urlRegex = new Regex(@"^https?://[\w\.-]+", RegexOptions.Compiled);
            
            string[] inputs = { 
                "test@example.com", 
                "555-123-4567", 
                "https://site.com",
                "invalid" 
            };
            
            foreach (string input in inputs)
            {
                if (emailRegex.IsMatch(input)) Console.WriteLine($"{input}: Email");
                else if (phoneRegex.IsMatch(input)) Console.WriteLine($"{input}: Phone");
                else if (urlRegex.IsMatch(input)) Console.WriteLine($"{input}: URL");
                else Console.WriteLine($"{input}: Unknown");
            }

            Console.WriteLine("\n=== Regular Expressions Part 2 Complete ===");
        }
    }
}