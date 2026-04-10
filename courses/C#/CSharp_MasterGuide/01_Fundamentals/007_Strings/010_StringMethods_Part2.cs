/*
 * ============================================================
 * TOPIC     : Fundamentals - Strings
 * SUBTOPIC  : String Methods - Advanced Operations
 * FILE      : StringMethods_Part2.cs
 * PURPOSE   : Continues string methods covering Split, Join, Pad, 
 *            and utility methods for string manipulation
 * ============================================================
 */

using System; // Core System namespace

namespace CSharp_MasterGuide._01_Fundamentals._07_Strings
{
    class StringMethods_Part2
    {
        static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════
            // SECTION 1: Split and Join Methods
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Split - Breaking string into array ──────────
            // Split by character - most common use case
            string csv = "apple,banana,cherry,date";
            string[] fruits = csv.Split(','); // Split on comma
            Console.WriteLine($"Fruits count: {fruits.Length}"); // Output: Fruits count: 4
            
            foreach (string fruit in fruits)
            {
                Console.WriteLine($"  - {fruit}"); 
                // Output:   - apple,   - banana,   - cherry,   - date
            }

            // Split by string (multi-character delimiter)
            string text = "Hello###World###Test";
            string[] parts = text.Split("###"); // Split on "###"
            Console.WriteLine(string.Join(" | ", parts)); // Output: Hello | World | Test

            // Split with options - RemoveEmptyEntries to skip empty strings
            string spaced = "a,,b,,c"; // Empty between commas
            string[] validParts = spaced.Split(',', StringSplitOptions.RemoveEmptyEntries);
            Console.WriteLine($"Valid parts: {validParts.Length}"); // Output: Valid parts: 3

            // Split with StringSplitOptions.IncludeEmptyEntries to keep empties
            string[] withEmpties = spaced.Split(',', StringSplitOptions.IncludeEmptyEntries);
            Console.WriteLine($"With empties: {withEmpties.Length}"); // Output: With empties: 5

            // ── EXAMPLE 2: Split by char array ────────────────────────
            // Split by multiple delimiters simultaneously
            string mixed = "apple,banana;cherry|date";
            char[] delimiters = { ',', ';', '|' };
            string[] mixedFruits = mixed.Split(delimiters);
            Console.WriteLine($"Mixed split: {string.Join(", ", mixedFruits)}");
            // Output: Mixed split: apple, banana, cherry, date

            // Split with max count - limit number of resulting elements
            string longText = "one,two,three,four,five";
            string[] limited = longText.Split(',', 3); // Max 3 elements
            Console.WriteLine(string.Join(" | ", limited));
            // Output: one | two | three,four,five

            // ── EXAMPLE 3: Join - Combining array into string ─────────
            // Basic Join - opposite of Split
            string[] colors = { "Red", "Green", "Blue" };
            string joined = string.Join(", ", colors);
            Console.WriteLine(joined); // Output: Red, Green, Blue

            // Join with empty separator - concatenate without delimiter
            string concatenated = string.Join("", colors);
            Console.WriteLine(concatenated); // Output: RedGreenBlue

            // Join with newline - create multi-line text
            string[] lines = { "Line 1", "Line 2", "Line 3" };
            string multiline = string.Join(Environment.NewLine, lines);
            Console.WriteLine(multiline);
            // Output: Line 1
            //         Line 2
            //         Line 3

            // ── REAL-WORLD EXAMPLE: Parse log file ───────────────────
            string logEntry = "2024-01-15 10:30:45 ERROR: Connection failed";
            string[] logParts = logEntry.Split(' ', 4); // Split into 4 parts max
            
            if (logParts.Length >= 4)
            {
                Console.WriteLine($"Timestamp: {logParts[0]} {logParts[1]}");
                Console.WriteLine($"Level: {logParts[2].TrimEnd(':')}"); // Remove trailing colon
                Console.WriteLine($"Message: {logParts[3]}");
                // Output: Timestamp: 2024-01-15 10:30:45
                // Output: Level: ERROR
                // Output: Message: Connection failed
            }

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: Padding Methods
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: PadLeft - Left padding ────────────────────
            // PadLeft(totalWidth) - adds spaces to the left
            string number = "42";
            string padded = number.PadLeft(5); // Make it 5 characters wide
            Console.WriteLine($"Padded: '{padded}'"); // Output: Padded: '   42'

            // PadLeft with custom character
            string hex = "FF";
            string zeroPadded = hex.PadLeft(8, '0'); // 8-char zero-padded hex
            Console.WriteLine($"Hex: {zeroPadded}"); // Output: Hex: 000000FF

            // Useful for tabular output
            string[] names = { "Alice", "Bob", "Charlie" };
            foreach (string name in names)
            {
                // Right-align names in 15-char column
                Console.WriteLine($"{name.PadRight(15)} | Age: 25");
                // Output: Alice           | Age: 25
                // Output: Bob              | Age: 25
                // Output: Charlie          | Age: 25
            }

            // ── EXAMPLE 2: PadRight - Right padding ─────────────────
            // PadRight(totalWidth) - adds spaces to the right
            string shortText = "Hi";
            string rightPadded = shortText.PadRight(10);
            Console.WriteLine($"'{rightPadded}'"); // Output: 'Hi        '

            // PadRight with character - useful for fixed-width file formats
            string code = "A1";
            string fixedCode = code.PadRight(5, '*'); // Pad with asterisks
            Console.WriteLine(fixedCode); // Output: A1***

            // ── REAL-WORLD EXAMPLE: Format table output ──────────────
            // Format price list with proper alignment
            string[] products = { "Laptop", "Mouse", "Keyboard" };
            decimal[] prices = { 999.99m, 25.50m, 79.99m };
            
            Console.WriteLine("Product".PadRight(15) + "Price");
            Console.WriteLine("-".PadRight(15, '-') + "----");
            
            for (int i = 0; i < products.Length; i++)
            {
                string priceStr = $"${prices[i]:F2}"; // Format to 2 decimals
                Console.WriteLine(products[i].PadRight(15) + priceStr);
                // Output: Laptop         $999.99
                // Output: Mouse           $25.50
                // Output: Keyboard        $79.99
            }

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: Comparison Methods
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Compare and CompareTo ─────────────────────
            // Compare - static method that returns integer (-1, 0, 1)
            int result1 = string.Compare("apple", "banana"); // a < b
            Console.WriteLine($"Compare apple vs banana: {result1}"); // Output: -1

            int result2 = string.Compare("apple", "apple"); // equal
            Console.WriteLine($"Compare apple vs apple: {result2}"); // Output: 0

            int result3 = string.Compare("zebra", "apple"); // z > a
            Console.WriteLine($"Compare zebra vs apple: {result3}"); // Output: 1

            // Compare with StringComparison for culture/case control
            int caseInsensitive = string.Compare("Apple", "apple", StringComparison.OrdinalIgnoreCase);
            Console.WriteLine($"Case-insensitive: {caseInsensitive}"); // Output: 0

            int cultureAware = string.Compare("Apple", "apple", StringComparison.CurrentCulture);
            Console.WriteLine($"Culture-aware: {cultureAware}"); // Output: 0 (depends on culture)

            // CompareTo instance method - returns same values
            string s1 = "hello";
            int compareResult = s1.CompareTo("world");
            Console.WriteLine($"CompareTo result: {compareResult}"); // Output: -1

            // ── EXAMPLE 2: Equals - Check string equality ───────────
            // Basic Equals - returns boolean
            string a = "test";
            string b = "test";
            bool areEqual = a.Equals(b); // True for same content
            Console.WriteLine($"Equals: {areEqual}"); // Output: True

            // Equals with StringComparison
            string upper = "TEST";
            bool equalsIgnoreCase = a.Equals(upper, StringComparison.OrdinalIgnoreCase);
            Console.WriteLine($"Ignore case: {equalsIgnoreCase}"); // Output: True

            // String literal comparison - == operator uses Equals internally
            // But explicit Equals allows StringComparison parameter

            // ── REAL-WORLD EXAMPLE: Password comparison ─────────────
            string enteredPassword = "MyPassword123";
            string storedHash = "mypassword123"; // Simulated stored (lowercase)
            
            // Case-insensitive comparison for user convenience
            bool passwordValid = string.Equals(
                enteredPassword, 
                storedHash, 
                StringComparison.OrdinalIgnoreCase
            );
            Console.WriteLine($"Password valid: {passwordValid}"); // Output: True

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: Format and Utility Methods
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: IsNullOrEmpty and IsNullOrWhiteSpace ──────
            // IsNullOrEmpty - checks for null OR empty string
            string nullString = null;
            string emptyString = "";
            string whiteString = "   ";
            
            Console.WriteLine($"Null empty: {string.IsNullOrEmpty(nullString)}"); // True
            Console.WriteLine($"Empty empty: {string.IsNullOrEmpty(emptyString)}"); // True
            Console.WriteLine($"White empty: {string.IsNullOrEmpty(whiteString)}"); // False

            // IsNullOrWhiteSpace - also checks for whitespace-only
            Console.WriteLine($"Null whitespace: {string.IsNullOrWhiteSpace(nullString)}"); // True
            Console.WriteLine($"Empty whitespace: {string.IsNullOrWhiteSpace(emptyString)}"); // True
            Console.WriteLine($"White whitespace: {string.IsNullOrWhiteSpace(whiteString)}"); // True

            // ── EXAMPLE 2: String.Empty vs "" ─────────────────────────
            // String.Empty is more explicit and slightly more efficient
            string empty1 = "";
            string empty2 = string.Empty;
            
            // Both are semantically identical, but String.Empty is clearer
            // Use String.Empty in production code for readability
            Console.WriteLine($"Empty check: {empty1 == string.Empty}"); // True
            Console.WriteLine($"Null coalesce: {null ?? string.Empty}"); // Output: (empty string)

            // ── EXAMPLE 3: Clone and Copy ─────────────────────────────
            string original = "Hello";
            string cloned = original.Clone() as string; // Returns object, cast to string
            Console.WriteLine($"Cloned: {cloned}"); // Output: Hello

            // Copy - creates independent copy
            string copied = string.Copy(original);
            copied = "World"; // Modify copy, original stays unchanged
            Console.WriteLine($"Original: {original}, Copied: {copied}");
            // Output: Original: Hello, Copied: World

            // ── REAL-WORLD EXAMPLE: Input validation ──────────────────
            string userName = "  "; // User entered only spaces
            string userEmail = null; // User didn't provide email
            
            // Comprehensive validation
            if (string.IsNullOrWhiteSpace(userName))
            {
                Console.WriteLine("Username is required");
            }
            else
            {
                Console.WriteLine($"Welcome, {userName.Trim()}!");
            }
            
            if (string.IsNullOrWhiteSpace(userEmail))
            {
                Console.WriteLine("Email is required"); // Output: Email is required
            }

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: Insert, Remove, and Other Transformations
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Insert - Add string at position ────────────
            string insertExample = "Hello World";
            string inserted = insertExample.Insert(6, "C# "); // Insert at index 6
            Console.WriteLine(inserted); // Output: Hello C# World

            // Insert at beginning
            string prepended = inserted.Insert(0, ">> ");
            Console.WriteLine(prepended); // Output: >> Hello C# World

            // ── EXAMPLE 2: Remove - Delete portion of string ─────────
            string removeExample = "Hello, Beautiful World!";
            string removed = removeExample.Remove(5, 12); // Remove ", Beautiful"
            Console.WriteLine(removed); // Output: Hello World!

            // Remove from position to end
            string truncate = "Hello, World, Goodbye!";
            string trimmed = truncate.Remove(truncate.IndexOf(", Goodbye"));
            Console.WriteLine(trimmed); // Output: Hello, World

            // ── EXAMPLE 3: String.CopyTo - Copy to char array ─────────
            // Copies portion of string to character array
            string source = "Hello";
            char[] destination = new char[5];
            source.CopyTo(0, destination, 0, 5); // Copy all 5 chars
            Console.WriteLine(new string(destination)); // Output: Hello

            // Copy specific portion
            string text2 = "Programming";
            char[] buffer = new char[4];
            text2.CopyTo(3, buffer, 0, 4); // Start at index 3, copy 4 chars = "gram"
            Console.WriteLine(new string(buffer)); // Output: gram

            // ── REAL-WORLD EXAMPLE: Format phone number ─────────────
            // Insert formatting into raw phone digits
            string rawPhone = "5551234567";
            string formatted = rawPhone.Insert(0, "(")
                                      .Insert(4, ")")
                                      .Insert(8, "-");
            Console.WriteLine($"Formatted: {formatted}"); // Output: Formatted: (555) 123-4567

            // Remove area code for display without it
            string withCode = "(555) 123-4567";
            string withoutCode = withCode.Remove(0, 1) // Remove (
                                   .Remove(3, 1)    // Remove )
                                   .Remove(7, 1);  // Remove -
            Console.WriteLine($"Without formatting: {withoutCode}"); // Output: 5551234567

            Console.WriteLine("\n=== String Methods Part 2 Complete ===");
        }
    }
}