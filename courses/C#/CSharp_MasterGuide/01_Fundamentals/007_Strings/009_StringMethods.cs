/*
 * ============================================================
 * TOPIC     : Fundamentals - Strings
 * SUBTOPIC  : String Methods and Operations
 * FILE      : StringMethods.cs
 * PURPOSE   : Teaches essential string methods including searching, 
 *            case conversion, trimming, and substring operations
 * ============================================================
 */

using System; // Core System namespace for Console and basic types

namespace CSharp_MasterGuide._01_Fundamentals._07_Strings
{
    class StringMethods
    {
        static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════
            // SECTION 1: Search and Find Methods
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: IndexOf - Finding position of substring ────
            // IndexOf returns the zero-based index of the first occurrence
            // Returns -1 if substring not found (important for validation)

            string text = "Hello, World! Welcome to C# programming.";
            int index = text.IndexOf("World"); // Search for "World" in the string
            Console.WriteLine($"IndexOf 'World': {index}"); // Output: IndexOf 'World': 7

            // IndexOf with start position - useful for finding multiple occurrences
            int secondIndex = text.IndexOf("o", index + 1); // Start after first 'o'
            Console.WriteLine($"Second 'o' after index {index}: {secondIndex}");
            // Output: Second 'o' after index 7: 8

            // ── EXAMPLE 2: LastIndexOf - Finding last occurrence ──────
            // LastIndexOf searches from end of string, useful for file paths
            string filePath = "C:\\Users\\John\\Documents\\Documents\\report.pdf";
            int lastSlash = filePath.LastIndexOf('\\'); // Find last backslash
            string fileName = filePath.Substring(lastSlash + 1); // Extract filename
            Console.WriteLine($"Filename: {fileName}"); // Output: Filename: report.pdf

            // ── REAL-WORLD EXAMPLE: Search in user input validation ─────
            // Validate that a specific keyword exists in user input
            string userMessage = "I want to order a large pizza with extra cheese";
            string spamKeyword = "winner"; // Common spam indicator
            if (userMessage.IndexOf(spamKeyword, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                Console.WriteLine("Message flagged as potential spam");
            }
            else
            {
                Console.WriteLine("Message accepted"); // Output: Message accepted
            }

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: Contains, StartsWith, EndsWith
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Contains - Check if substring exists ─────────
            // Returns boolean - cleaner than IndexOf >= 0 pattern
            string email = "user@example.com";
            bool hasAt = email.Contains("@"); // Simple and readable
            bool hasDot = email.Contains("."); // Check for domain
            Console.WriteLine($"Has @: {hasAt}, Has .: {hasDot}"); 
            // Output: Has @: True, Has .: True

            // Contains with StringComparison for case-insensitive search
            string password = "MySecurePassword123";
            bool hasUpper = password.Contains("MYSECURE", StringComparison.OrdinalIgnoreCase);
            Console.WriteLine($"Contains 'MYSECURE' (case-insensitive): {hasUpper}");
            // Output: Contains 'MYSECURE' (case-insensitive): True

            // ── EXAMPLE 2: StartsWith and EndsWith ────────────────────
            // Essential for file type and URL validation
            string url = "https://api.example.com/users";
            bool isHttps = url.StartsWith("https"); // Security check
            bool isApi = url.StartsWith("/api"); // API endpoint check
            Console.WriteLine($"Is HTTPS: {isHttps}, Is API: {isApi}"); 
            // Output: Is HTTPS: True, Is API: False

            // File extension validation
            string filename = "document.pdf";
            bool isPdf = filename.EndsWith(".pdf"); // Check file type
            bool isImage = filename.EndsWith(".jpg") || filename.EndsWith(".png"); // Multiple checks
            Console.WriteLine($"Is PDF: {isPdf}, Is Image: {isImage}"); 
            // Output: Is PDF: True, Is Image: False

            // ── REAL-WORLD EXAMPLE: Email format validation ────────────
            string userEmail = "john.doe@company.co.uk";
            bool validFormat = userEmail.Contains("@") && 
                           userEmail.Contains(".") &&
                           !userEmail.StartsWith("@") &&
                           !userEmail.EndsWith(".");
            Console.WriteLine($"Email format valid: {validFormat}"); // Output: Email format valid: True

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: Case Conversion Methods
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: ToUpper and ToLower ─────────────────────
            // ToUpper - converting to uppercase
            string lowercase = "hello world";
            string uppercase = lowercase.ToUpper(); // Convert entire string
            Console.WriteLine(uppercase); // Output: HELLO WORLD

            // ToLower - converting to lowercase (useful for comparison)
            string mixedCase = "HeLLo WoRLd";
            string normalized = mixedCase.ToLower();
            Console.WriteLine(normalized); // Output: hello world

            // ── EXAMPLE 2: ToUpperInvariant and ToLowerInvariant ────
            // Invariant culture - ignores current culture settings
            // Use for case-insensitive comparisons that must be culture-independent
            string turkishI = "I"; // Turkish has special uppercase 'I'
            string lowerInvariant = turkishI.ToLowerInvariant();
            Console.WriteLine($"ToLowerInvariant: {lowerInvariant}"); // Output: ToLowerInvariant: i

            // ── REAL-WORLD EXAMPLE: Case-insensitive search ───────────
            string searchTerm = "C# PROGRAMMING";
            string[] titles = { "c# programming", "Python Basics", "C# Advanced" };
            
            foreach (string title in titles)
            {
                // Normalize both for comparison
                if (title.ToLower().Contains(searchTerm.ToLower()))
                {
                    Console.WriteLine($"Found: {title}"); // Output: Found: c# programming
                    // Output: Found: C# Advanced
                }
            }

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: Trim and Whitespace Methods
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Trim - Remove leading/trailing whitespace ──
            // Essential for user input that may have accidental spaces
            string dirtyInput = "   John Doe   "; // User accidentally added spaces
            string cleanInput = dirtyInput.Trim(); // Remove spaces
            Console.WriteLine($"Clean input: '{cleanInput}'"); // Output: Clean input: 'John Doe'

            // TrimStart and TrimEnd - specific direction Trim
            string padded = "   |text|   ";
            string leftTrimmed = padded.TrimStart(); // Only leading
            string rightTrimmed = padded.TrimEnd();   // Only trailing
            Console.WriteLine($"Left: '{leftTrimmed}', Right: '{rightTrimmed}'");
            // Output: Left: '|text|   ', Right: '   |text|'

            // ── EXAMPLE 2: trimming specific characters ──────────────
            // Trim can remove specific characters from ends
            string path = "/path/to/file/";
            string trimmedPath = path.Trim('/'); // Remove leading/trailing slashes
            Console.WriteLine($"Trimmed path: '{trimmedPath}'"); // Output: Trimmed path: 'path/to/file'

            string quotes = "\"Hello World\"";
            string unquoted = quotes.Trim('"'); // Remove quotes
            Console.WriteLine($"Unquoted: {unquoted}"); // Output: Unquoted: Hello World

            // ── REAL-WORLD EXAMPLE: CSV data cleaning ────────────────
            string csvLine = "  John  ,  30  ,  Engineer  ";
            string[] fields = csvLine.Split(','); // Split by comma
            
            for (int i = 0; i < fields.Length; i++)
            {
                fields[i] = fields[i].Trim(); // Clean each field
            }
            
            Console.WriteLine($"Name: {fields[0]}, Age: {fields[1]}, Job: {fields[2]}");
            // Output: Name: John, Age: 30, Job: Engineer

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: Substring Methods
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Substring - Extract part of string ──────────
            // Substring(startIndex) - from startIndex to end
            string fullText = "Hello, World!";
            string world = fullText.Substring(7); // Start at index 7
            Console.WriteLine(world); // Output: World!

            // Substring(startIndex, length) - specific length
            string numbers = "1234567890";
            string firstThree = numbers.Substring(0, 3); // First 3 characters
            string midThree = numbers.Substring(3, 3);   // Characters at index 3-5
            Console.WriteLine($"First: {firstThree}, Mid: {midThree}");
            // Output: First: 123, Mid: 456

            // ── EXAMPLE 2: Using Substring with other methods ────────
            // Extract domain from email
            string emailAddress = "user@subdomain.example.com";
            int atIndex = emailAddress.IndexOf("@");
            string domain = emailAddress.Substring(atIndex + 1); // Everything after @
            Console.WriteLine($"Domain: {domain}"); // Output: Domain: subdomain.example.com

            // Extract filename from path
            string filePath2 = "C:\\Users\\Documents\\project\\app.cs";
            int lastBackslash = filePath2.LastIndexOf("\\");
            string file = filePath2.Substring(lastBackslash + 1);
            Console.WriteLine($"File: {file}"); // Output: File: app.cs

            // ── REAL-WORLD EXAMPLE: Parse formatted data ──────────────
            // Parse phone number in format "123-456-7890"
            string phone = "555-123-4567";
            string areaCode = phone.Substring(0, 3);
            string exchange = phone.Substring(4, 3);
            string number = phone.Substring(8, 4);
            Console.WriteLine($"({areaCode}) {exchange}-{number}");
            // Output: (555) 123-4567

            // ═══════════════════════════════════════════════════════════
            // SECTION 6: Replace Methods
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Replace - Simple substring replacement ────
            string sentence = "I like cats and dogs";
            string modified = sentence.Replace("dogs", "cats"); // Replace dogs with cats
            Console.WriteLine(modified); // Output: I like cats and cats

            // Replace all occurrences automatically
            string numbers2 = "1,2,3,1,2,3";
            string semicolons = numbers2.Replace(",", ";"); // Change delimiter
            Console.WriteLine(semicolons); // Output: 1;2;3;1;2;3

            // ── EXAMPLE 2: Replace with StringComparison ───────────
            // Case-insensitive replace
            string greeting = "Hello World";
            string replaced = greeting.Replace("hello", "Goodbye", 
                                               StringComparison.OrdinalIgnoreCase);
            Console.WriteLine(replaced); // Output: Goodbye World

            // Remove substring by replacing with empty string
            string withTags = "<p>Hello</p>";
            string withoutTags = withTags.Replace("<p>", "").Replace("</p>", "");
            Console.WriteLine(withoutTags); // Output: Hello

            // ── REAL-WORLD EXAMPLE: Sanitize user input ───────────
            // Remove potentially dangerous characters
            string userInput = "<script>alert('xss')</script>";
            string sanitized = userInput.Replace("<", "&lt;")
                                     .Replace(">", "&gt;")
                                     .Replace("\"", "&quot;")
                                     .Replace("'", "&#x27;");
            Console.WriteLine($"Sanitized: {sanitized}");
            // Output: Sanitized: &lt;script&gt;alert(&#x27;xss&#x27;)&lt;/script&gt;

            Console.WriteLine("\n=== String Methods Examples Complete ===");
        }
    }
}