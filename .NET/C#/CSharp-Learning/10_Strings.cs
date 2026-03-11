/*
================================================================================
TOPIC 10: STRINGS
================================================================================

Strings are sequences of characters and are one of the most used data types
in C#. This topic covers everything you need to work with text effectively.

TABLE OF CONTENTS:
1. Introduction to Strings
2. String Creation
3. String Properties
4. String Methods
5. String Manipulation
6. String Formatting
7. String Interpolation
8. StringBuilder
9. Common Mistakes
================================================================================
*/

// ================================================================================
// SECTION 1: INTRODUCTION TO STRINGS
// ================================================================================

/*
WHAT IS A STRING?
-----------------
A string is an immutable sequence of characters used to represent text.

REAL-WORLD ANALOGY:
-------------------
Think of a string like a bead necklace:
- Each bead is a character
- The entire necklace is a string
- Once made, you can't change individual beads (immutable)

IMMUTABLE MEANS:
----------------
- Once created, cannot be changed
- Any "modification" creates a new string
- Original string remains unchanged
- This makes strings thread-safe and memory-efficient

WHY STRINGS MATTER:
-------------------
- User input/output
- File handling
- Data processing
- Web development
- Database operations

String is so important that C# has special syntax for it!
*/


// ================================================================================
// SECTION 2: STRING CREATION
// ================================================================================

namespace StringCreation
{
    class Program
    {
        static void Main(string[] args)
        {
            // ====================================================================
            // WAYS TO CREATE STRINGS
            // ====================================================================
            
            // Method 1: String literals (most common)
            string greeting = "Hello, World!";
            
            // Method 2: Using new keyword
            string message = new string("Hello");
            
            // Method 3: Character array
            char[] letters = { 'H', 'e', 'l', 'l', 'o' };
            string fromChars = new string(letters);
            
            // Method 4: String with repeat
            string repeated = new string('*', 5);  // "*****"
            
            // ====================================================================
            // ESCAPE SEQUENCES
            // ====================================================================
            
            Console.WriteLine("=== Escape Sequences ===");
            
            string newLine = "Line 1\nLine 2";
            Console.WriteLine(newLine);
            
            string tabbed = "Col1\tCol2\tCol3";
            Console.WriteLine(tabbed);
            
            string quote = "He said \"Hello\"";
            Console.WriteLine(quote);
            
            string backslash = "C:\\Users\\John";
            Console.WriteLine(backslash);
            
            // Verbatim string (raw - no escaping)
            string raw = @"C:\Users\John\Documents";
            Console.WriteLine(raw);
            
            // Multi-line strings
            string multi = @"This is
a multi-line
string";
            Console.WriteLine(multi);
        }
    }
}

/*
ESCAPE SEQUENCES:
-----------------
\n  - New line
\t  - Tab
\"  - Double quote
\'  - Single quote
\\  - Backslash
\r  - Carriage return
@   - Verbatim (no escape parsing)
*/


// ================================================================================
// SECTION 3: STRING PROPERTIES
// ================================================================================

namespace StringProperties
{
    class Program
    {
        static void Main(string[] args)
        {
            string text = "Hello, World!";
            
            Console.WriteLine("=== String Properties ===");
            
            // Length - number of characters
            Console.WriteLine($"Length: {text.Length}");  // 13
            
            // Indexer - access individual characters
            Console.WriteLine($"First char: {text[0]}");  // H
            Console.WriteLine($"Last char: {text[text.Length - 1]}");  // !
            
            // Characters are immutable
            // text[0] = 'J';  // ERROR - can't modify!
            
            // Empty vs Null vs Whitespace
            string empty = "";
            string nullString = null;
            string whiteSpace = "   ";
            string blank = string.Empty;
            
            Console.WriteLine($"\n=== Checking Strings ===");
            Console.WriteLine($"empty.IsNullOrEmpty: {string.IsNullOrEmpty(empty)}");  // true
            Console.WriteLine($"nullString.IsNullOrEmpty: {string.IsNullOrEmpty(nullString)}");  // true
            Console.WriteLine($"whiteSpace.IsNullOrEmpty: {string.IsNullOrEmpty(whiteSpace)}");  // false
            Console.WriteLine($"whiteSpace.IsNullOrWhiteSpace: {string.IsNullOrWhiteSpace(whiteSpace)}");  // true
        }
    }
}

/*
STRING PROPERTIES:
------------------
.Length         - Number of characters
[index]         - Get character at position
IsNullOrEmpty() - Check for null or ""
IsNullOrWhiteSpace() - Check for null, "", or whitespace
*/


// ================================================================================
// SECTION 4: STRING METHODS
// ================================================================================

namespace StringMethods
{
    class Program
    {
        static void Main(string[] args)
        {
            string text = "Hello, World!";
            
            // ====================================================================
            // CASE CONVERSION
            // ====================================================================
            
            Console.WriteLine("=== Case Conversion ===");
            Console.WriteLine($"Original: {text}");
            Console.WriteLine($"Upper: {text.ToUpper()}");
            Console.WriteLine($"Lower: {text.ToLower()}");
            
            // ====================================================================
            // SEARCHING
            // ====================================================================
            
            Console.WriteLine("\n=== Searching ===");
            Console.WriteLine($"Contains 'World': {text.Contains("World")}");  // true
            Console.WriteLine($"Contains 'world': {text.Contains("world")}");  // false (case-sensitive)
            Console.WriteLine($"Starts with 'Hello': {text.StartsWith("Hello")}");  // true
            Console.WriteLine($"Ends with '!': {text.EndsWith("!")}");  // true
            Console.WriteLine($"Index of 'o': {text.IndexOf('o')}");  // 4
            Console.WriteLine($"Last Index of 'o': {text.LastIndexOf('o')}");  // 8
            
            // ====================================================================
            // EXTRACTING
            // ====================================================================
            
            Console.WriteLine("\n=== Extracting ===");
            string sample = "Hello, World!";
            
            Console.WriteLine($"Substring(0,5): {sample.Substring(0, 5)}");  // Hello
            Console.WriteLine($"Substring(7): {sample.Substring(7)}");  // World!
            Console.WriteLine($"Remove(5): {sample.Remove(5)}");  // Hello
            Console.WriteLine($"Remove(5, 7): {sample.Remove(5, 7)}");  // Hello!
            
            // ====================================================================
            // TRIMMING
            // ====================================================================
            
            Console.WriteLine("\n=== Trimming ===");
            string padded = "   Hello World   ";
            Console.WriteLine($"Original: '{padded}'");
            Console.WriteLine($"Trim: '{padded.Trim()}'");
            Console.WriteLine($"TrimStart: '{padded.TrimStart()}'");
            Console.WriteLine($"TrimEnd: '{padded.TrimEnd()}'");
            
            // ====================================================================
            // REPLACING
            // ====================================================================
            
            Console.WriteLine("\n=== Replacing ===");
            string replaced = "Hello, World!";
            Console.WriteLine($"Replace 'World' with 'C#': {replaced.Replace("World", "C#")}");
            Console.WriteLine($"Replace 'o' with '0': {replaced.Replace("o", "0")}");
            
            // ====================================================================
            // SPLITTING
            // ====================================================================
            
            Console.WriteLine("\n=== Splitting ===");
            string csv = "apple,banana,cherry";
            string[] fruits = csv.Split(',');
            
            foreach (string fruit in fruits)
            {
                Console.WriteLine(fruit);
            }
        }
    }
}

/*
COMMON STRING METHODS:
---------------------
Case:      ToUpper(), ToLower(), ToTitleCase()
Search:    Contains(), StartsWith(), EndsWith(), IndexOf()
Extract:   Substring(), Remove(), Take/Skip
Trim:      Trim(), TrimStart(), TrimEnd()
Replace:   Replace(), Remove()
Split:     Split()
Join:      Join()
Format:    Format()
*/


// ================================================================================
// SECTION 5: STRING MANIPULATION
// ================================================================================

namespace StringManipulation
{
    class Program
    {
        static void Main(string[] args)
        {
            // ====================================================================
            // STRING COMPARISON
            // ====================================================================
            
            Console.WriteLine("=== Comparison ===");
            
            string a = "hello";
            string b = "hello";
            string c = "Hello";
            
            Console.WriteLine($"a == b: {a == b}");        // true
            Console.WriteLine($"a == c: {a == c}");        // false (case)
            
            // Case-insensitive
            Console.WriteLine($"Equals (ignore case): {a.Equals(c, StringComparison.OrdinalIgnoreCase)}");  // true
            
            // Compare methods
            Console.WriteLine($"Compare: {string.Compare(a, c, StringComparison.OrdinalIgnoreCase)}");  // 0 (equal)
            
            // ====================================================================
            // STRING CONCATENATION
            // ====================================================================
            
            Console.WriteLine("\n=== Concatenation ===");
            
            string first = "Hello";
            string second = "World";
            
            // Method 1: +
            Console.WriteLine(first + " " + second);
            
            // Method 2: Concat
            Console.WriteLine(string.Concat(first, " ", second));
            
            // Method 3: Join
            string[] words = { "Hello", "World", "!" };
            Console.WriteLine(string.Join(" ", words));
            
            // ====================================================================
            // PADDING
            // ====================================================================
            
            Console.WriteLine("\n=== Padding ===");
            
            Console.WriteLine("|" + "Hello".PadLeft(10) + "|");
            Console.WriteLine("|" + "Hello".PadRight(10) + "|");
            
            // ====================================================================
            // PRACTICAL EXAMPLES
            // ====================================================================
            
            // Check if email is valid
            string email = "user@example.com";
            bool isValid = email.Contains("@") && email.Contains(".");
            Console.WriteLine($"\nEmail valid: {isValid}");
            
            // Parse name parts
            string fullName = "John Doe";
            string[] parts = fullName.Split(' ');
            Console.WriteLine($"First: {parts[0]}, Last: {parts[1]}");
        }
    }
}

/*
STRING OPERATIONS:
------------------
Comparison: ==, Equals(), Compare()
Concatenation: +, Concat(), Join()
Padding: PadLeft(), PadRight()
Alignment: Align text in fixed-width output
*/


// ================================================================================
// SECTION 6: STRING FORMATTING
// ================================================================================

namespace StringFormatting
{
    class Program
    {
        static void Main(string[] args)
        {
            // ====================================================================
            // STRING FORMAT
            // ====================================================================
            
            // Format with placeholders {0}, {1}, etc.
            string name = "John";
            int age = 30;
            
            Console.WriteLine("=== Format ===");
            Console.WriteLine(string.Format("Name: {0}, Age: {1}", name, age));
            
            // Format with numbers
            double price = 19.99;
            Console.WriteLine(string.Format("Price: {0:C}", price));  // Currency
            Console.WriteLine(string.Format("Price: {0:N2}", price));  // Number
            Console.WriteLine(string.Format("Percent: {0:P}", 0.75));  // Percent
            
            // Date formatting
            DateTime date = new DateTime(2024, 3, 15);
            Console.WriteLine(string.Format("Date: {0:d}", date));  // Short
            Console.WriteLine(string.Format("Date: {0:D}", date));  // Long
            Console.WriteLine(string.Format("Date: {0:yyyy-MM-dd}", date));
            
            // ====================================================================
            // STRING INTERPOLATION (Modern way!)
            // ====================================================================
            
            Console.WriteLine("\n=== String Interpolation ===");
            
            string message = $"Name: {name}, Age: {age}";
            Console.WriteLine(message);
            
            // Expressions in interpolation
            int a = 10, b = 20;
            Console.WriteLine($"{a} + {b} = {a + b}");
            
            // Format within interpolation
            Console.WriteLine($"Price: {price:C2}");
            Console.WriteLine($"Date: {date:yyyy-MM-dd}");
            
            // Multiple lines
            Console.WriteLine($@"
Name: {name}
Age: {age}
Price: {price:C}
");
        }
    }
}

/*
FORMATTING OPTIONS:
-------------------
C   - Currency
N   - Number
P   - Percentage
D   - Decimal
X   - Hexadecimal
d   - Short date
D   - Long date
*/


// ================================================================================
// SECTION 7: STRINGBUILDER
// ================================================================================

namespace StringBuilderExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // ====================================================================
            // WHY STRINGBUILDER?
            // ====================================================================
            
            // Strings are immutable - each concatenation creates new string!
            // StringBuilder is mutable - better for many concatenations
            
            // BAD: Multiple concatenations (creates 5 strings in memory)
            Console.WriteLine("=== String Concatenation (slow) ===");
            string result = "";
            for (int i = 1; i <= 5; i++)
            {
                result += i + " ";
            }
            Console.WriteLine(result);
            
            // GOOD: StringBuilder (modifies in place)
            Console.WriteLine("\n=== StringBuilder (fast) ===");
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            
            for (int i = 1; i <= 5; i++)
            {
                sb.Append(i);
                sb.Append(" ");
            }
            Console.WriteLine(sb.ToString());
            
            // ====================================================================
            // STRINGBUILDER METHODS
            // ====================================================================
            
            Console.WriteLine("\n=== StringBuilder Methods ===");
            
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            
            builder.Append("Hello");        // Add to end
            builder.AppendLine(" World");   // Add with newline
            builder.AppendFormat("Value: {0}", 42);  // Formatted
            builder.Insert(5, ", C#");     // Insert at position
            builder.Replace("World", "C#"); // Replace text
            builder.Remove(5, 2);           // Remove characters
            
            Console.WriteLine(builder.ToString());
            
            // Capacity
            System.Text.StringBuilder sb2 = new System.Text.StringBuilder(100);
            Console.WriteLine($"\nCapacity: {sb2.Capacity}");
        }
    }
}

/*
WHEN TO USE STRINGBUILDER:
--------------------------
- Building strings in loops
- Many concatenations (3+)
- When performance matters

WHEN TO USE REGULAR STRINGS:
----------------------------
- Simple concatenations
- When readability matters
- Small number of operations
*/


// ================================================================================
// SECTION 8: COMMON MISTAKES
// ================================================================================

/*
MISTAKE 1: Confusing string with number
---------------------------------------
int num = 42;
string text = "42" + num;  // "4242" not 42+42!


MISTAKE 2: Case sensitivity
----------------------------
"hello".Contains("HELLO")  // false!


MISTAKE 3: Not handling null
-----------------------------
string s = null;
Console.WriteLine(s.Length);  // NullReferenceException!


MISTAKE 4: Using string methods on null
----------------------------------------
// Same as above!


MISTAKE 5: Off-by-one in substring
-----------------------------------
"Hello".Substring(0, 5)  // "Hello" - OK
"Hello".Substring(0, 6)  // ERROR - out of bounds!


MISTAKE 6: Mutable string attempt
----------------------------------
string s = "hello";
s[0] = 'j';  // ERROR - strings are immutable!


MISTAKE 7: String comparison with ==
-------------------------------------
Use Equals() or Compare() for proper comparison!
*/


// ================================================================================
// SECTION 9: PRACTICE EXERCISES
// ================================================================================

/*
EXERCISE 1: Palindrome Checker
------------------------------
Input: "racecar"
Output: true
Input: "hello"
Output: false

EXERCISE 2: Word Counter
------------------------
Input: "Hello World Hello"
Output: Word count: 3

EXERCISE 3: Email Validator
---------------------------
Check if email contains @ and .
Check if @ comes before .

EXERCISE 4: Reverse Words
--------------------------
Input: "Hello World"
Output: "World Hello"

EXERCISE 5: Password Strength
-----------------------------
Check if password has:
- At least 8 characters
- At least one uppercase
- At least one lowercase
- At least one number
*/


// ================================================================================
// SECTION 10: INTERVIEW QUESTIONS
// ================================================================================

/*
Q1: Are strings mutable or immutable in C#?
A: Strings are immutable. Any modification creates a new string.

Q2: What is StringBuilder and when should you use it?
A: StringBuilder is a mutable string class. Use it when performing
   many concatenations to avoid performance issues.

Q3: What is the difference between == and Equals() for strings?
A: == compares references by default. Equals() compares values.
   For strings, use Equals() or == which is overridden to compare values.

Q4: How do you handle null strings safely?
A: Use string.IsNullOrEmpty() or string.IsNullOrWhiteSpace() checks
   before accessing string methods.

Q5: What is string interpolation?
A: A modern C# feature using $ to embed variables directly in strings:
   $"Hello {name}"; instead of string.Format("Hello {0}", name);
*/


// ================================================================================
// NEXT STEPS
// =============================================================================

/*
EXCELLENT! You've completed the beginner topics! You now know:
- Variables and data types
- Type casting
- Operators
- Control statements
- Loops
- Methods
- Arrays
- Strings

WHAT'S NEXT:
In Topic 11, we'll explore Collections - more powerful data structures
that can grow and shrink dynamically.
*/
