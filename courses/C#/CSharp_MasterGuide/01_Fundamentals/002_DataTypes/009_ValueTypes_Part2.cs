/*
 * ============================================================
 * TOPIC     : C# Fundamentals
 * SUBTOPIC  : Data Types - Value Types (Part 2)
 * FILE      : ValueTypes_Part2.cs
 * PURPOSE   : This file covers char, boolean, and enum types, plus nullable value types.
 *             These are essential value types not covered in the first part.
 * ============================================================
 */

// --- SECTION: Character and Boolean Types ---
// Char and boolean are fundamental value types with specific use cases

using System;

namespace CSharp_MasterGuide._01_Fundamentals._02_DataTypes
{
    class ValueTypes_Part2
    {
        static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════════
            // SECTION: Char Type (Single Character)
            // ═══════════════════════════════════════════════════════════════
            // Char represents a single 16-bit Unicode character
            // Unicode includes characters from all writing systems in the world

            // ── char: 16-bit Unicode character ─────────────────────────────
            // Used for: text processing, parsing, game characters, UI labels
            char letter = 'A'; // Single character in single quotes
            char digit = '5';   // Can represent any digit
            char special = '@'; // Special characters
            char unicode = '\u0041'; // Unicode escape sequence (A = U+0041)
            char newline = '\n'; // Control character for new line
            char tab = '\t';    // Control character for tab
            
            Console.WriteLine($"char letter: {letter}"); // Output: char letter: A
            Console.WriteLine($"char unicode: {unicode}"); // Output: char unicode: A
            Console.WriteLine($"char digit: {digit}"); // Output: char digit: 5

            // ── Char methods and properties ──────────────────────────────────
            // Char is actually a struct with useful methods
            char lowerChar = 'a';
            char upperChar = char.ToUpper(lowerChar); // Convert to uppercase
            Console.WriteLine($"ToUpper('a'): {upperChar}"); // Output: ToUpper('a'): A
            
            bool isDigit = char.IsDigit('5'); // Check if character is a digit
            bool isLetter = char.IsLetter('A'); // Check if character is a letter
            bool isLetterOrDigit = char.IsLetterOrDigit('A'); // Check if either
            Console.WriteLine($"IsDigit('5'): {isDigit}"); // Output: IsDigit('5'): True
            Console.WriteLine($"IsLetter('A'): {isLetter}"); // Output: IsLetter('A'): True
            
            bool isWhitespace = char.IsWhiteSpace(' '); // Check for whitespace
            bool isPunctuation = char.IsPunctuation('!'); // Check for punctuation
            Console.WriteLine($"IsWhiteSpace(' '): {isWhitespace}"); // Output: IsWhiteSpace(' '): True
            Console.WriteLine($"IsPunctuation('!'): {isPunctuation}"); // Output: IsPunctuation('!'): True

            // ── Char in real-world scenarios ───────────────────────────────
            // Parsing user input, validation, text processing
            string userInput = "John123"; // Example username input
            foreach (char c in userInput) // Loop through each character
            {
                bool valid = char.IsLetterOrDigit(c); // Allow only letters and digits
                Console.WriteLine($"Character '{c}': IsLetterOrDigit = {valid}");
            }
            // Output:
            // Character 'J': IsLetterOrDigit = True
            // Character 'o': IsLetterOrDigit = True
            // Character 'h': IsLetterOrDigit = True
            // Character '1': IsLetterOrDigit = True
            // Character '2': IsLetterOrDigit = True
            // Character '3': IsLetterOrDigit = True

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Boolean Type (True/False)
            // ═══════════════════════════════════════════════════════════════
            // Boolean represents logical true or false values
            // Fundamental for conditional logic and decision making

            // ── bool: Boolean value (true or false) ─────────────────────────
            // Used for: flags, conditions, switches, game states
            bool isActive = true; // True state - active, enabled, valid
            bool isComplete = false; // False state - inactive, disabled, invalid
            bool isNull = bool.Parse("true"); // Parse string to bool
            bool isTrue = bool.Parse("True"); // Parse is case-insensitive
            
            Console.WriteLine($"bool true: {isActive}"); // Output: bool true: True
            Console.WriteLine($"bool false: {isComplete}"); // Output: bool false: False

            // ── Boolean operators ───────────────────────────────────────────
            // Boolean values can be combined with logical operators
            bool a = true;
            bool b = false;
            
            bool andResult = a && b; // Logical AND - true only if both true
            bool orResult = a || b;  // Logical OR - true if at least one true
            bool notResult = !a;     // Logical NOT - inverts the value
            
            Console.WriteLine($"true && false: {andResult}"); // Output: true && false: False
            Console.WriteLine($"true || false: {orResult}"); // Output: true || false: True
            Console.WriteLine($"!true: {notResult}"); // Output: !true: False

            // ── Boolean in real-world scenarios ─────────────────────────────
            bool isLoggedIn = true; // User authentication status
            bool hasPermission = false; // Permission check
            bool isPremiumUser = true; // User subscription level
            
            // Multiple conditions combined
            bool canAccessPremium = isLoggedIn && hasPermission && isPremiumUser;
            Console.WriteLine($"Can access premium content: {canAccessPremium}");
            // Output: Can access premium content: False (because hasPermission is false)

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Enum Types (Named Constants)
            // ═══════════════════════════════════════════════════════════════
            // Enum creates a set of named constant values
            // Improves code readability and prevents invalid values

            // ── Enum declaration and usage ───────────────────────────────────
            // Define an enum to represent days of the week
            // By default, enum underlying type is int (0-indexed)
            Days today = Days.Wednesday; // Assign named constant to variable
            
            Console.WriteLine($"Today is: {today}"); // Output: Today is: Wednesday
            Console.WriteLine($"Today value: {(int)today}"); // Output: Today value: 3

            // ── Enum with custom values ─────────────────────────────────────
            // You can assign custom integer values to enum members
            HttpStatusCode status = HttpStatusCode.OK; // HTTP 200
            Console.WriteLine($"Status: {status} ({(int)status})"); // Output: Status: OK (200)
            
            status = HttpStatusCode.NotFound; // HTTP 404
            Console.WriteLine($"Status: {status} ({(int)status})"); // Output: Status: NotFound (404)

            // ── Enum methods ─────────────────────────────────────────────────
            // Enums have useful methods for working with enum values
            string[] names = Enum.GetNames(typeof(Days)); // Get all names as string array
            Console.WriteLine($"All days: {string.Join(", ", names)}");
            // Output: All days: Sunday, Monday, Tuesday, Wednesday, Thursday, Friday, Saturday
            
            int[] values = (int[])Enum.GetValues(typeof(Days)); // Get all values as int array
            Console.WriteLine($"Day values: {string.Join(", ", values)}");
            // Output: Day values: 0, 1, 2, 3, 4, 5, 6
            
            bool isValidDay = Enum.IsDefined(typeof(Days), 5); // Check if value exists
            Console.WriteLine($"Is 5 a valid day: {isValidDay}"); // Output: Is 5 a valid day: True
            
            Days parsedDay = Enum.Parse<Days>("Tuesday"); // Parse string to enum
            Console.WriteLine($"Parsed day: {parsedDay}"); // Output: Parsed day: Tuesday

            // ── Enum in real-world scenarios ─────────────────────────────────
            // Using enums for type-safe constants
            LogLevel currentLevel = LogLevel.Warning; // Set current logging level
            
            if (currentLevel >= LogLevel.Error) // Compare enum values
            {
                Console.WriteLine("Error or critical - send alert!"); // Take action
            }
            // Output: (nothing, because Warning < Error)

            // Switch on enum for clean code
            OrderStatus orderStatus = OrderStatus.Shipped;
            string statusMessage = orderStatus switch
            {
                OrderStatus.Pending => "Your order is being processed",
                OrderStatus.Processing => "Your order is being prepared",
                OrderStatus.Shipped => "Your order is on its way",
                OrderStatus.Delivered => "Your order has been delivered",
                OrderStatus.Cancelled => "Your order was cancelled",
                _ => "Unknown status"
            };
            Console.WriteLine($"Status message: {statusMessage}");
            // Output: Status message: Your order is on its way

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Nullable Value Types
            // ═══════════════════════════════════════════════════════════════
            // Value types cannot normally be null, but nullable types allow it
            // Useful for database fields, optional parameters, missing data

            // ── Nullable value types ─────────────────────────────────────────
            // Add ? after value type to make it nullable
            int? nullableInt = null; // Can store null or integer
            double? nullableDouble = null;
            bool? nullableBool = null;
            
            Console.WriteLine($"Nullable int: {nullableInt ?? "null"}"); // Output: Nullable int: null
            Console.WriteLine($"Nullable double: {nullableDouble ?? "null"}"); // Output: Nullable double: null
            Console.WriteLine($"Nullable bool: {nullableBool ?? "null"}"); // Output: Nullable bool: null

            // ── Checking nullable values ─────────────────────────────────────
            int? score = 85; // Assign a value
            bool hasValue = score.HasValue; // Check if has value
            int actualValue = score.Value; // Get the value (throws if null)
            
            Console.WriteLine($"Has value: {hasValue}"); // Output: Has value: True
            Console.WriteLine($"Value: {actualValue}"); // Output: Value: 85
            
            score = null; // Set to null
            hasValue = score.HasValue; // Check again
            Console.WriteLine($"Has value (null): {hasValue}"); // Output: Has value (null): False

            // ── Null-coalescing with nullable ───────────────────────────────
            // Provide default values when nullable is null
            int? optionalId = null;
            int id = optionalId ?? 0; // If null, use 0
            Console.WriteLine($"ID with default: {id}"); // Output: ID with default: 0
            
            optionalId = 42;
            id = optionalId ?? 0;
            Console.WriteLine($"ID with value: {id}"); // Output: ID with value: 42

            // ── Nullable in real-world scenarios ───────────────────────────
            // Database often has nullable columns
            // When reading from database, nullable types represent NULL values
            int? customerAge = null; // Age might not be provided
            int age = customerAge ?? 18; // Default to 18 if not provided
            Console.WriteLine($"Customer age (with default): {age}"); // Output: Customer age (with default): 18
        }
        
        // Enum definitions for the examples above
        
        // Days enum - simple enum with default int values
        enum Days { Sunday, Monday, Tuesday, Wednesday, Thursday, Friday, Saturday }
        
        // HttpStatusCode enum - HTTP response codes (simplified)
        enum HttpStatusCode 
        { 
            OK = 200,           // Success
            Created = 201,      // Resource created
            Accepted = 202,     // Request accepted
            BadRequest = 400,   // Client error
            Unauthorized = 401, // Authentication required
            Forbidden = 403,    // Access denied
            NotFound = 404,     // Resource not found
            InternalError = 500 // Server error
        }
        
        // LogLevel enum - for logging systems
        enum LogLevel
        {
            Debug = 0,     // Detailed debugging info
            Info = 1,      // General information
            Warning = 2,   // Warning conditions
            Error = 3,     // Error conditions
            Critical = 4   // Critical failures
        }
        
        // OrderStatus enum - for e-commerce order processing
        enum OrderStatus
        {
            Pending = 1,       // Order placed, not yet processed
            Processing = 2,    // Order being prepared
            Shipped = 3,       // Order sent to customer
            Delivered = 4,    // Order received by customer
            Cancelled = 5     // Order cancelled
        }
    }
}
