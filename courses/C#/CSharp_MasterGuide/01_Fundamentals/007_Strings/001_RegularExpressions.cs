/*
 * ============================================================
 * TOPIC     : Fundamentals - Strings
 * SUBTOPIC  : Regular Expressions
 * FILE      : RegularExpressions.cs
 * PURPOSE   : Teaches regex basics including patterns, matching,
 *            groups, and common regex operations
 * ============================================================
 */

using System; // Core System namespace
using System.Text.RegularExpressions; // Required for regex operations

namespace CSharp_MasterGuide._01_Fundamentals._07_Strings
{
    class RegularExpressions
    {
        static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════
            // SECTION 1: Regex Basics - Match and IsMatch
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: IsMatch - Simple boolean check ─────────────
            // Check if pattern exists in string
            string pattern1 = @"\d+"; // One or more digits
            string text1 = "abc123def";
            
            bool hasDigits = Regex.IsMatch(text1, pattern1);
            Console.WriteLine($"Has digits: {hasDigits}"); // True

            // Check for specific word
            bool hasHello = Regex.IsMatch("Hello World", @"\bHello\b");
            Console.WriteLine($"Has 'Hello': {hasHello}"); // True

            // Check email format simplified
            bool isEmail = Regex.IsMatch("user@example.com", @"^[\w\.-]+@[\w\.-]+\.\w+$");
            Console.WriteLine($"Is email: {isEmail}"); // True

            // ── EXAMPLE 2: Match - Get match details ──────────────────
            // Get first match with position and value
            string text2 = "Order #12345 confirmed";
            Match match = Regex.Match(text2, @"\d+"); // Find first digit sequence
            
            Console.WriteLine($"Found: '{match.Value}' at index {match.Index}");
            // Output: Found: '12345' at index 8

            // Match with Groups - capture portions
            string text3 = "Price: $99.99";
            Match priceMatch = Regex.Match(text3, @"(\$[\d.]+)");
            
            Console.WriteLine($"Price: {priceMatch.Groups[1].Value}"); // $99.99
            Console.WriteLine($"Full match: {priceMatch.Groups[0].Value}"); // $99.99

            // ── EXAMPLE 3: Matches - Find all occurrences ────────────
            // Find all matches in string
            string numbers = "1a2b3c4d5";
            MatchCollection allMatches = Regex.Matches(numbers, @"\d");
            
            Console.WriteLine($"Found {allMatches.Count} digits:");
            foreach (Match m in allMatches)
            {
                Console.WriteLine($"  {m.Value} at {m.Index}");
            }
            // Output: Found 5 digits: 1,2,3,4,5

            // Find all words
            string sentence = "The quick brown fox jumps";
            MatchCollection words = Regex.Matches(sentence, @"\w+");
            
            foreach (Match w in words)
            {
                Console.WriteLine($"Word: {w.Value}");
            }

            // ── REAL-WORLD EXAMPLE: Input validation ──────────────────
            // Validate US phone number format
            string[] phoneNumbers = { 
                "123-456-7890", 
                "(123) 456-7890", 
                "1234567890",
                "123-45-6789" 
            };
            
            string phonePattern = @"^\(?([0-9]{3})\)?[-.\s]?([0-9]{3})[-.\s]?([0-9]{4})$";
            
            foreach (string phone in phoneNumbers)
            {
                bool valid = Regex.IsMatch(phone, phonePattern);
                Console.WriteLine($"{phone}: {(valid ? "Valid" : "Invalid")}");
                // Output: 123-456-7890: Valid
                // Output: (123) 456-7890: Valid
                // Output: 1234567890: Valid
                // Output: 123-45-6789: Invalid
            }

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: Regex Patterns and Metacharacters
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Character classes ──────────────────────────
            // \d - digit [0-9]
            Console.WriteLine(Regex.IsMatch("5", @"\d")); // True
            Console.WriteLine(Regex.IsMatch("a", @"\d")); // False

            // \D - non-digit
            Console.WriteLine(Regex.IsMatch("a", @"\D")); // True

            // \w - word character [a-zA-Z0-9_]
            Console.WriteLine(Regex.IsMatch("abc_123", @"\w+")); // True
            Console.WriteLine(Regex.IsMatch("hello!", @"\w+")); // False (exclamation)

            // \s - whitespace
            string withSpaces = "Hello World";
            Console.WriteLine(Regex.IsMatch(withSpaces, @"\s")); // True
            Console.WriteLine(Regex.IsMatch("NoSpaces", @"\s")); // False

            // [abc] - any of a, b, or c
            Console.WriteLine(Regex.IsMatch("bat", @"[aeiou]")); // False (a is vowel, but b is not)
            Console.WriteLine(Regex.IsMatch("bat", @"[bcr]at")); // True (starts with b, c, or r)

            // [a-z] - range
            Console.WriteLine(Regex.IsMatch("abc", @"^[a-z]+$")); // True (lowercase only)
            Console.WriteLine(Regex.IsMatch("ABC", @"^[a-z]+$")); // False

            // ── EXAMPLE 2: Quantifiers ────────────────────────────────
            // * - zero or more
            Console.WriteLine(Regex.IsMatch("abc", @"ab*c")); // True (b repeated)
            Console.WriteLine(Regex.IsMatch("ac", @"ab*c")); // True (b zero times)

            // + - one or more
            Console.WriteLine(Regex.IsMatch("abbbc", @"ab+c")); // True
            Console.WriteLine(Regex.IsMatch("ac", @"ab+c")); // False (must have at least one b)

            // ? - zero or one (optional)
            Console.WriteLine(Regex.IsMatch("color", @"colou?r")); // True
            Console.WriteLine(Regex.IsMatch("colour", @"colou?r")); // True

            // {n} - exactly n times
            Console.WriteLine(Regex.IsMatch("aaa", @"a{3}")); // True
            Console.WriteLine(Regex.IsMatch("aa", @"a{3}")); // False

            // {n,} - n or more times
            Console.WriteLine(Regex.IsMatch("aaa", @"a{2,}")); // True
            Console.WriteLine(Regex.IsMatch("a", @"a{2,}")); // False

            // {n,m} - between n and m times
            Console.WriteLine(Regex.IsMatch("aaa", @"a{2,4}")); // True
            Console.WriteLine(Regex.IsMatch("aaaaa", @"a{2,4}")); // False

            // ── EXAMPLE 3: Anchors ────────────────────────────────────
            // ^ - start of string (or line in multiline mode)
            Console.WriteLine(Regex.IsMatch("hello world", @"^hello")); // True
            Console.WriteLine(Regex.IsMatch("world hello", @"^hello")); // False

            // $ - end of string (or line)
            Console.WriteLine(Regex.IsMatch("hello world", @"world$")); // True
            Console.WriteLine(Regex.IsMatch("hello world!", @"world$")); // False

            // \b - word boundary
            Console.WriteLine(Regex.IsMatch("cat catalog", @"\bcat\b")); // True (cat as word)
            Console.WriteLine(Regex.IsMatch("category", @"\bcat\b")); // False (cat inside word)

            // ── REAL-WORLD EXAMPLE: Password validation ───────────────
            // Requirements: 8+ chars, at least 1 uppercase, 1 lowercase, 1 digit
            string[] passwords = { 
                "Pass1234", 
                "password", 
                "PASSWORD123", 
                "Pass123" 
            };
            
            // Complex pattern for password validation
            string pwPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$";
            
            foreach (string pw in passwords)
            {
                bool valid = Regex.IsMatch(pw, pwPattern);
                Console.WriteLine($"'{pw}': {(valid ? "Valid" : "Invalid")}");
                // Output: 'Pass1234': Valid
                // Output: 'password': Invalid (no uppercase)
                // Output: 'PASSWORD123': Invalid (no lowercase)
                // Output: 'Pass123': Invalid (less than 8 chars)
            }

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: Groups and Capturing
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Named groups ───────────────────────────────
            // (?<name>pattern) - capture with name
            string logEntry = "2024-01-15 14:30:45 ERROR Connection failed";
            
            Regex dateTimePattern = new Regex(@"(?<date>\d{4}-\d{2}-\d{2}) (?<time>\d{2}:\d{2}:\d{2}) (?<level>\w+) (?<message>.+)");
            Match logMatch = dateTimePattern.Match(logEntry);
            
            Console.WriteLine($"Date: {logMatch.Groups["date"].Value}");
            Console.WriteLine($"Time: {logMatch.Groups["time"].Value}");
            Console.WriteLine($"Level: {logMatch.Groups["level"].Value}");
            Console.WriteLine($"Message: {logMatch.Groups["message"].Value}");
            // Output: Date: 2024-01-15
            // Output: Time: 14:30:45
            // Output: Level: ERROR
            // Output: Message: Connection failed

            // ── EXAMPLE 2: Multiple groups ─────────────────────────────
            // Extract parts from phone number
            string phone = "(555) 123-4567";
            Regex phonePattern = new Regex(@"(?<area>\d{3})[-.\s]?(?<exchange>\d{3})[-.\s]?(?<number>\d{4})");
            Match phoneMatch = phonePattern.Match(phone);
            
            Console.WriteLine($"Area: {phoneMatch.Groups["area"]}");
            Console.WriteLine($"Exchange: {phoneMatch.Groups["exchange"]}");
            Console.WriteLine($"Number: {phoneMatch.Groups["number"]}");

            // ── EXAMPLE 3: Non-capturing groups ───────────────────────
            // (?:pattern) - group without capturing
            string colorText = "red blue green";
            Regex colorPattern = new Regex(@"(?:red|blue|green)\s+(\w+)");
            Match colorMatch = colorPattern.Match(colorText);
            
            Console.WriteLine($"Value: {colorMatch.Value}"); // "red blue"
            Console.WriteLine($"Captured: {colorMatch.Groups[1].Value}"); // "blue"
            // Groups[0] is full match, Groups[1] is first capturing group

            // ── REAL-WORLD EXAMPLE: Parse structured data ──────────────
            string dataLine = "John Doe,30,Engineer,2024-01-15";
            
            // Named groups for CSV-like data
            Regex csvPattern = new Regex(@"^(?<name>[^,]+),(?<age>\d+),(?<role>[^,]+),(?<date>\d{4}-\d{2}-\d{2})$");
            Match dataMatch = csvPattern.Match(dataLine);
            
            if (dataMatch.Success)
            {
                var person = new {
                    Name = dataMatch.Groups["name"].Value,
                    Age = int.Parse(dataMatch.Groups["age"].Value),
                    Role = dataMatch.Groups["role"].Value,
                    StartDate = dataMatch.Groups["date"].Value
                };
                
                Console.WriteLine($"Parsed: {person.Name}, {person.Age}, {person.Role}, {person.StartDate}");
            }

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: Replace and Substitution
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Simple replace ─────────────────────────────
            string messy = "hello   world";
            string cleaned = Regex.Replace(messy, @"\s+", " "); // Multiple spaces to one
            Console.WriteLine($"Cleaned: '{cleaned}'"); // "hello world"

            // Replace digits with #
            string withNumbers = "Item1 $10 Item2 $20";
            string hashed = Regex.Replace(withNumbers, @"\d+", "#");
            Console.WriteLine(hashed); // "Item# $# Item# $#"

            // ── EXAMPLE 2: Replace with groups ────────────────────────
            // $1, $2 refer to captured groups
            string nameFormat = "Doe, John";
            string fixedName = Regex.Replace(nameFormat, @"(\w+),\s*(\w+)", "$2 $1");
            Console.WriteLine(fixedName); // "John Doe"

            // Swap date format from YYYY-MM-DD to MM/DD/YYYY
            string date1 = "2024-01-15";
            string americanDate = Regex.Replace(date1, @"(\d{4})-(\d{2})-(\d{2})", "$2/$3/$1");
            Console.WriteLine(americanDate); // "01/15/2024"

            // ── EXAMPLE 3: Named group substitution ───────────────────
            string name = "John Doe";
            string template = "Dear {name},";
            
            // Replace placeholder with actual name
            string personalized = Regex.Replace(template, @"\{name\}", name);
            Console.WriteLine(personalized); // "Dear John Doe,"

            // ── REAL-WORLD EXAMPLE: Sanitize HTML ─────────────────────
            string htmlInput = "<script>alert('XSS')</script><p>Hello</p>";
            
            // Remove HTML tags
            string plainText = Regex.Replace(htmlInput, @"<[^>]+>", "");
            Console.WriteLine($"Plain text: '{plainText}'"); // "alert('XSS')Hello"
            
            // Convert URLs to links
            string textWithUrls = "Visit http://example.com for more info";
            string linked = Regex.Replace(textWithUrls, 
                @"(http[s]?://[^\s]+)", 
                "<a href=\"$1\">$1</a>");
            Console.WriteLine(linked); // "Visit <a href="http://example.com">http://example.com</a> for more info"

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: Split with Regex
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Split by pattern ───────────────────────────
            string csvData = "apple,banana;cherry:date";
            
            // Split by any of comma, semicolon, or colon
            string[] fruits = Regex.Split(csvData, @"[,;:]+");
            
            foreach (string f in fruits)
            {
                Console.WriteLine($"Fruit: {f}");
            }
            // Output: apple, banana, cherry, date

            // Split by whitespace (multiple)
            string multiSpace = "Hello    World    !";
            string[] words2 = Regex.Split(multiSpace, @"\s+");
            Console.WriteLine($"Words: {string.Join("|", words2)}"); // Hello|World|!

            // ── EXAMPLE 2: Split with capture ─────────────────────────
            // Keep delimiters in result
            string delimited = "a-b-c-d";
            string[] parts = Regex.Split(delimited, @"(-)");
            // Result: "a", "-", "b", "-", "c", "-", "d"
            
            Console.WriteLine($"Split count: {parts.Length}");
            foreach (string p in parts)
            {
                Console.WriteLine($"  '{p}'");
            }

            // ── REAL-WORLD EXAMPLE: Parse log file ────────────────────
            string logFile = @"2024-01-15 10:30:45 ERROR Connection failed
2024-01-15 10:30:46 INFO Retry attempt 1
2024-01-15 10:30:47 DEBUG Packet received";
            
            string[] logLines = Regex.Split(logFile, @"\r?\n");
            
            Regex logPattern = new Regex(@"^(?<timestamp>\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}) (?<level>\w+) (?<message>.+)$");
            
            foreach (string line in logLines)
            {
                Match m = logPattern.Match(line);
                if (m.Success)
                {
                    Console.WriteLine($"[{m.Groups["level"]}] {m.Groups["message"]}");
                }
            }

            Console.WriteLine("\n=== Regular Expressions Complete ===");
        }
    }
}