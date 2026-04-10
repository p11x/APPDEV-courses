/*
 * ============================================================
 * TOPIC     : Fundamentals - Strings
 * SUBTOPIC  : Real-World String Applications - Part 2
 * FILE      : Strings_RealWorld_Part2.cs
 * PURPOSE   : More advanced real-world string applications including
 *            text analysis, generation, transformation, and utilities
 * ============================================================
 */

using System; // Core System namespace
using System.Text; // For StringBuilder
using System.Text.RegularExpressions; // For regex
using System.Collections.Generic; // For generic collections

namespace CSharp_MasterGuide._01_Fundamentals._07_Strings
{
    class Strings_RealWorld_Part2
    {
        static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════
            // APPLICATION 1: Text Analyzer and Statistics
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("=== Text Analyzer and Statistics ===\n");

            string sampleText = @"C# is a modern, general-purpose programming language. 
It was developed by Microsoft as part of its .NET initiative. 
C# is a strongly typed language that supports object-oriented programming concepts 
like encapsulation, inheritance, and polymorphism. The language is designed for 
building a variety of applications that run on the .NET Framework.";

            // Analyze text
            var stats = AnalyzeText(sampleText);

            Console.WriteLine("Text Statistics:");
            Console.WriteLine($"  Characters (with spaces): {stats.CharsWithSpaces}");
            Console.WriteLine($"  Characters (without spaces): {stats.CharsWithoutSpaces}");
            Console.WriteLine($"  Words: {stats.WordCount}");
            Console.WriteLine($"  Lines: {stats.LineCount}");
            Console.WriteLine($"  Paragraphs: {stats.ParagraphCount}");
            Console.WriteLine($"  Sentences: {stats.SentenceCount}");
            Console.WriteLine($"  Average word length: {stats.AvgWordLength:F2}");
            Console.WriteLine($"  Unique words: {stats.UniqueWordCount}");

            Console.WriteLine("\nMost Common Words:");
            foreach (var (word, count) in stats.TopWords.Take(5))
            {
                Console.WriteLine($"  '{word}': {count}");
            }

            // ═══════════════════════════════════════════════════════════
            // APPLICATION 2: String Template Engine
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n=== String Template Engine ===\n");

            // Define templates
            var templates = new Dictionary<string, string>
            {
                ["welcome"] = "Welcome, {{name}}! You have {{count}} new messages.",
                ["order_confirm"] = "Order #{{order_id}} confirmed. Total: ${{total}}. Delivery by {{date}}.",
                ["alert"] = "[{{level}}] {{timestamp}}: {{message}}",
                ["email_header"] = "From: {{sender}}\nTo: {{receiver}}\nSubject: {{subject}}\nDate: {{date}}"
            };

            // Template data
            var data = new Dictionary<string, string>
            {
                ["name"] = "John Doe",
                ["count"] = "5",
                ["order_id"] = "12345",
                ["total"] = "99.99",
                ["date"] = "2024-01-15",
                ["level"] = "ERROR",
                ["timestamp"] = "2024-01-15 14:30:45",
                ["message"] = "Connection timeout",
                ["sender"] = "system@company.com",
                ["receiver"] = "user@company.com",
                ["subject"] = "Notification"
            };

            // Render templates
            foreach (var (key, template) in templates)
            {
                string result = RenderTemplate(template, data);
                Console.WriteLine($"{key}:");
                Console.WriteLine(result);
                Console.WriteLine();
            }

            // ═══════════════════════════════════════════════════════════
            // APPLICATION 3: Password Generator
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("=== Password Generator ===\n");

            // Generate various password types
            var passwords = new[]
            {
                GeneratePassword(12, true, true, true, true),
                GeneratePassword(16, true, true, false, true),
                GeneratePassword(8, false, true, true, false),
                GeneratePassword(20, true, true, true, true)
            };

            Console.WriteLine("Generated Passwords:");
            foreach (var pw in passwords)
            {
                Console.WriteLine($"  {pw} (Length: {pw.Length})");
            }

            // Validate generated passwords
            Console.WriteLine("\nPassword Strength Validation:");
            foreach (var pw in passwords)
            {
                Console.WriteLine($"  {pw}: {ValidatePassword(pw)}");
            }

            // ═══════════════════════════════════════════════════════════
            // APPLICATION 4: Search and Highlight Engine
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n=== Search and Highlight Engine ===\n");

            string document = @"The quick brown fox jumps over the lazy dog. 
A quick brown fox is often used in typography examples. 
The dog is not amused by the fox's jumping.";

            string searchTerm = "brown fox";

            // Find and highlight matches
            var highlights = FindAndHighlight(document, searchTerm);

            Console.WriteLine($"Search term: '{searchTerm}'");
            Console.WriteLine($"Matches found: {highlights.Count}");
            Console.WriteLine("\nHighlighted text:");
            foreach (var h in highlights)
            {
                Console.WriteLine($"  Index {h.Index}: '{h.Match}'");
            }

            // Show context around matches
            Console.WriteLine("\nContext:");
            foreach (var h in highlights)
            {
                Console.WriteLine($"  ...{GetContext(document, h.Index, 20)}...");
            }

            // ═══════════════════════════════════════════════════════════
            // APPLICATION 5: Slug Generator
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n=== Slug Generator ===\n");

            string[] titles = {
                "How to Learn C# Programming",
                "Understanding OOP Concepts: Encapsulation, Inheritance & Polymorphism",
                "   Spaces   and   Multiple   Spaces   ",
                "Special!@#$%Characters^&*()",
                "Numbers 123 and 456 in Title",
                "UPPERCASE TITLE TO LOWERCASE",
                "Mixed Case With Some CAPS",
                "Stop Words: a, an, the, is, are, was, were"
            };

            Console.WriteLine("Original -> Slug:");
            foreach (string title in titles)
            {
                string slug = GenerateSlug(title);
                Console.WriteLine($"  '{title}' -> '{slug}'");
            }

            // ═══════════════════════════════════════════════════════════
            // APPLICATION 6: Diff/Comparison Tool
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n=== String Diff Tool ===\n");

            string original = "The quick brown fox jumps over the lazy dog.";
            string modified = "The quick red fox jumps over the sleepy dog.";

            Console.WriteLine($"Original:  {original}");
            Console.WriteLine($"Modified:  {modified}");
            Console.WriteLine();

            var diffs = ComputeDiff(original, modified);

            Console.WriteLine("Differences:");
            foreach (var diff in diffs)
            {
                Console.WriteLine($"  {diff.Type}: '{diff.Text}'");
            }

            // ═══════════════════════════════════════════════════════════
            // APPLICATION 7: Word Wrap Formatter
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n=== Word Wrap Formatter ===\n");

            string longText = "This is a very long line of text that needs to be wrapped at a specific column width to fit within a container or terminal window. The algorithm should handle this gracefully without breaking words in the middle.";

            int[] widths = { 40, 60, 80 };

            foreach (int width in widths)
            {
                Console.WriteLine($"Width: {width}");
                string[] wrapped = WrapText(longText, width);
                foreach (string line in wrapped)
                {
                    Console.WriteLine($"  |{line}|");
                }
                Console.WriteLine();
            }

            // ═══════════════════════════════════════════════════════════
            // APPLICATION 8: Morse Code Converter
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("=== Morse Code Converter ===\n");

            string[] messages = { "HELLO WORLD", "SOS", "C SHARP", "123" };

            Console.WriteLine("Text -> Morse:");
            foreach (string msg in messages)
            {
                string morse = TextToMorse(msg);
                Console.WriteLine($"  {msg} -> {morse}");
            }

            Console.WriteLine("\nMorse -> Text:");
            string[] morseCodes = { ".... . .-.. .-.. --- / .-- --- .-. .-.. -..", "... --- ...", "-.-. / ... .... .- .-. .--" };
            foreach (string mc in morseCodes)
            {
                string text = MorseToText(mc);
                Console.WriteLine($"  {mc} -> {text}");
            }

            // ═══════════════════════════════════════════════════════════
            // APPLICATION 9: Text Transformation Pipeline
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n=== Text Transformation Pipeline ===\n");

            // Define a transformation pipeline
            Func<string, string>[] pipeline = {
                s => s.ToLower(),
                s => Regex.Replace(s, @"[^\w\s]", ""), // Remove punctuation
                s => Regex.Replace(s, @"\s+", " "),   // Normalize spaces
                s => s.Trim(),
                s => s.Replace(" ", "-")              // Convert spaces to hyphens
            };

            string input = "  HELLO World!!! This is a TEST...  ";

            Console.WriteLine($"Input: '{input}'");
            Console.WriteLine("\nPipeline steps:");

            string current = input;
            string[] stepNames = { "Lowercase", "Remove punctuation", "Normalize spaces", "Trim", "Spaces to hyphens" };

            for (int i = 0; i < pipeline.Length; i++)
            {
                current = pipeline[i](current);
                Console.WriteLine($"  {i + 1}. {stepNames[i]}: '{current}'");
            }

            Console.WriteLine($"\nFinal output: '{current}'");

            // ═══════════════════════════════════════════════════════════
            // APPLICATION 10: Custom String Encryption
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n=== Simple String Encryption (ROT13) ===\n");

            string[] toEncrypt = { "Hello", "Secret Message", "ABC123XYZ" };

            Console.WriteLine("ROT13 Encryption:");
            foreach (string s in toEncrypt)
            {
                string encrypted = Rot13(s);
                string decrypted = Rot13(encrypted);
                Console.WriteLine($"  '{s}' -> '{encrypted}' -> '{decrypted}'");
            }

            // Caesar cipher with custom shift
            Console.WriteLine("\nCaesar Cipher (shift 3):");
            foreach (string s in toEncrypt)
            {
                string encrypted = CaesarCipher(s, 3);
                string decrypted = CaesarCipher(encrypted, -3);
                Console.WriteLine($"  '{s}' -> '{encrypted}' -> '{decrypted}'");
            }

            Console.WriteLine("\n=== Strings Real-World Part 2 Complete ===");
        }

        // ═══════════════════════════════════════════════════════════
        // HELPER METHODS AND CLASSES
        // ═══════════════════════════════════════════════════════════

        // Application 1: Text Analyzer
        class TextStats
        {
            public int CharsWithSpaces { get; set; }
            public int CharsWithoutSpaces { get; set; }
            public int WordCount { get; set; }
            public int LineCount { get; set; }
            public int ParagraphCount { get; set; }
            public int SentenceCount { get; set; }
            public double AvgWordLength { get; set; }
            public int UniqueWordCount { get; set; }
            public List<(string Word, int Count)> TopWords { get; set; } = new();
        }

        static TextStats AnalyzeText(string text)
        {
            var stats = new TextStats();

            stats.CharsWithSpaces = text.Length;
            stats.CharsWithoutSpaces = text.Replace(" ", "").Length;

            // Count words
            var words = Regex.Matches(text, @"\b\w+\b").Cast<Match>().Select(m => m.Value.ToLower()).ToList();
            stats.WordCount = words.Count;

            // Count lines
            stats.LineCount = text.Split('\n').Length;

            // Count paragraphs
            stats.ParagraphCount = Regex.Split(text, @"\n\s*\n").Where(p => !string.IsNullOrWhiteSpace(p)).Count();

            // Count sentences
            stats.SentenceCount = Regex.Matches(text, @"[.!?]+").Count;

            // Average word length
            if (words.Count > 0)
                stats.AvgWordLength = words.Average(w => w.Length);

            // Unique words
            stats.UniqueWordCount = words.Distinct().Count();

            // Top words
            stats.TopWords = words
                .GroupBy(w => w)
                .OrderByDescending(g => g.Count())
                .Take(10)
                .Select(g => (g.Key, g.Count()))
                .ToList();

            return stats;
        }

        // Application 2: Template Engine
        static string RenderTemplate(string template, Dictionary<string, string> data)
        {
            string result = template;

            foreach (var (key, value) in data)
            {
                result = result.Replace("{{" + key + "}}", value);
            }

            return result;
        }

        // Application 3: Password Generator
        static string GeneratePassword(int length, bool useUpper, bool useLower, bool useDigits, bool useSpecial)
        {
            const string upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lower = "abcdefghijklmnopqrstuvwxyz";
            const string digits = "0123456789";
            const string special = "!@#$%^&*()_+-=[]{}|;:,.<>?";

            string chars = "";
            if (useUpper) chars += upper;
            if (useLower) chars += lower;
            if (useDigits) chars += digits;
            if (useSpecial) chars += special;

            if (string.IsNullOrEmpty(chars))
                chars = lower + digits; // Fallback

            var random = new Random();
            var password = new char[length];

            // Ensure at least one from each required type
            int pos = 0;
            if (useUpper) password[pos++] = upper[random.Next(upper.Length)];
            if (useLower) password[pos++] = lower[random.Next(lower.Length)];
            if (useDigits) password[pos++] = digits[random.Next(digits.Length)];
            if (useSpecial) password[pos++] = special[random.Next(special.Length)];

            // Fill remaining
            for (int i = pos; i < length; i++)
            {
                password[i] = chars[random.Next(chars.Length)];
            }

            // Shuffle
            for (int i = password.Length - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                (password[i], password[j]) = (password[j], password[i]);
            }

            return new string(password);
        }

        static string ValidatePassword(string password)
        {
            bool hasLower = Regex.IsMatch(password, @"[a-z]");
            bool hasUpper = Regex.IsMatch(password, @"[A-Z]");
            bool hasDigit = Regex.IsMatch(password, @"[0-9]");
            bool hasSpecial = Regex.IsMatch(password, @"[!@#$%^&*()_+\-=\[\]{}|;:,.<>?]");

            int strength = (hasLower ? 1 : 0) + (hasUpper ? 1 : 0) + (hasDigit ? 1 : 0) + (hasSpecial ? 1 : 0);

            return strength switch
            {
                4 => "Strong",
                3 => "Medium",
                2 => "Weak",
                _ => "Very Weak"
            };
        }

        // Application 4: Search and Highlight
        class HighlightResult
        {
            public int Index { get; set; }
            public string Match { get; set; } = "";
        }

        static List<HighlightResult> FindAndHighlight(string text, string searchTerm)
        {
            var results = new List<HighlightResult>();
            var regex = new Regex(Regex.Escape(searchTerm), RegexOptions.IgnoreCase);

            foreach (Match m in regex.Matches(text))
            {
                results.Add(new HighlightResult
                {
                    Index = m.Index,
                    Match = m.Value
                });
            }

            return results;
        }

        static string GetContext(string text, int index, int charsAround)
        {
            int start = Math.Max(0, index - charsAround);
            int length = Math.Min(charsAround * 2 + 1, text.Length - start);
            return text.Substring(start, length);
        }

        // Application 5: Slug Generator
        static string GenerateSlug(string title)
        {
            // Convert to lowercase
            string slug = title.ToLower();

            // Remove special characters except spaces and hyphens
            slug = Regex.Replace(slug, @"[^\w\s-]", "");

            // Replace multiple spaces with single space
            slug = Regex.Replace(slug, @"\s+", " ");

            // Trim
            slug = slug.Trim();

            // Replace spaces with hyphens
            slug = slug.Replace(' ', '-');

            // Remove duplicate hyphens
            slug = Regex.Replace(slug, @"-+", "-");

            // Remove stop words (optional, simple implementation)
            string[] stopWords = { "a", "an", "the", "is", "are", "was", "were" };
            foreach (string word in stopWords)
            {
                slug = Regex.Replace(slug, $@"\b{word}\b-", "");
                slug = Regex.Replace(slug, $@"-{word}\b", "");
            }

            // Clean up leading/trailing hyphens
            slug = slug.Trim('-');

            return slug;
        }

        // Application 6: Diff Tool
        class DiffResult
        {
            public string Type { get; set; } = ""; // Added, Removed, Unchanged
            public string Text { get; set; } = "";
        }

        static List<DiffResult> ComputeDiff(string original, string modified)
        {
            var diffs = new List<DiffResult>();

            // Simple word-by-word comparison
            string[] origWords = original.Split(' ');
            string[] modWords = modified.Split(' ');

            int i = 0, j = 0;
            while (i < origWords.Length || j < modWords.Length)
            {
                if (i >= origWords.Length)
                {
                    diffs.Add(new DiffResult { Type = "Added", Text = modWords[j] });
                    j++;
                }
                else if (j >= modWords.Length)
                {
                    diffs.Add(new DiffResult { Type = "Removed", Text = origWords[i] });
                    i++;
                }
                else if (origWords[i] == modWords[j])
                {
                    diffs.Add(new DiffResult { Type = "Unchanged", Text = origWords[i] });
                    i++;
                    j++;
                }
                else
                {
                    // Look ahead to see if it's a replacement or insert/delete
                    diffs.Add(new DiffResult { Type = "Removed", Text = origWords[i] });
                    diffs.Add(new DiffResult { Type = "Added", Text = modWords[j] });
                    i++;
                    j++;
                }
            }

            return diffs;
        }

        // Application 7: Word Wrap
        static string[] WrapText(string text, int width)
        {
            var lines = new List<string>();
            string[] words = text.Split(' ');
            string currentLine = "";

            foreach (string word in words)
            {
                if (currentLine.Length + word.Length + 1 > width)
                {
                    if (!string.IsNullOrEmpty(currentLine))
                        lines.Add(currentLine);
                    currentLine = word;
                }
                else
                {
                    if (string.IsNullOrEmpty(currentLine))
                        currentLine = word;
                    else
                        currentLine += " " + word;
                }
            }

            if (!string.IsNullOrEmpty(currentLine))
                lines.Add(currentLine);

            return lines.ToArray();
        }

        // Application 8: Morse Code
        static Dictionary<char, string> MorseAlphabet = new()
        {
            {'A', ".-"}, {'B', "-..."}, {'C', "-.-."}, {'D', "-.."}, {'E', "."},
            {'F', "..-."}, {'G', "--."}, {'H', "...."}, {'I', ".."}, {'J', ".---"},
            {'K', "-.-"}, {'L', ".-.."}, {'M', "--"}, {'N', "-."}, {'O', "---"},
            {'P', ".--."}, {'Q', "--.-"}, {'R', ".-."}, {'S', "..."}, {'T', "-"},
            {'U', "..-"}, {'V', "...-"}, {'W', ".--"}, {'X', "-..-"}, {'Y', "-.--"},
            {'Z', "--.."}, {'0', "-----"}, {'1', ".----"}, {'2', "..---"}, {'3', "...--"},
            {'4', "....-"}, {'5', "....."}, {'6', "-...."}, {'7', "--..."}, {'8', "---.."}, {'9', "----."},
            {' ', "/"}
        };

        static string TextToMorse(string text)
        {
            var morse = new StringBuilder();
            foreach (char c in text.ToUpper())
            {
                if (MorseAlphabet.TryGetValue(c, out string code))
                {
                    if (morse.Length > 0 && c != ' ')
                        morse.Append(' ');
                    morse.Append(code);
                }
            }
            return morse.ToString();
        }

        static string MorseToText(string morse)
        {
            // Reverse the dictionary
            var reverseMorse = MorseAlphabet.ToLookup(x => x.Value, x => x.Key)
                .ToDictionary(g => g.Key, g => g.First());

            var text = new StringBuilder();
            string[] codes = morse.Split(' ');

            foreach (string code in codes)
            {
                if (reverseMorse.TryGetValue(code, out char c))
                    text.Append(c);
            }

            return text.ToString();
        }

        // Application 10: ROT13 and Caesar Cipher
        static string Rot13(string input)
        {
            var result = new StringBuilder();
            foreach (char c in input)
            {
                if (char.IsLetter(c))
                {
                    char baseChar = char.IsUpper(c) ? 'A' : 'a';
                    char rotated = (char)(((c - baseChar + 13) % 26) + baseChar);
                    result.Append(rotated);
                }
                else
                {
                    result.Append(c);
                }
            }
            return result.ToString();
        }

        static string CaesarCipher(string input, int shift)
        {
            var result = new StringBuilder();
            shift = ((shift % 26) + 26) % 26; // Normalize shift

            foreach (char c in input)
            {
                if (char.IsLetter(c))
                {
                    char baseChar = char.IsUpper(c) ? 'A' : 'a';
                    char shifted = (char)(((c - baseChar + shift) % 26) + baseChar);
                    result.Append(shifted);
                }
                else
                {
                    result.Append(c);
                }
            }
            return result.ToString();
        }
    }
}