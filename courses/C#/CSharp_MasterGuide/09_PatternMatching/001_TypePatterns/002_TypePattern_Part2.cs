/*
 * ============================================================
 * TOPIC     : Pattern Matching
 * SUBTOPIC  : Type Patterns (Continued)
 * FILE      : 02_TypePattern_Part2.cs
 * PURPOSE   : Continues type pattern matching with nullable types, recursive patterns,
 *            and combining patterns with relational patterns
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._09_PatternMatching._01_TypePatterns
{
    /// <summary>
    /// Continues type pattern matching demonstrations with advanced scenarios
    /// </summary>
    public class TypePattern_Part2
    {
        /// <summary>
        /// Entry point for advanced type pattern examples
        /// </summary>
        public static void Main(string[] args)
        {
            // Output: === Type Pattern Matching Part 2 ===
            Console.WriteLine("=== Type Pattern Matching Part 2 ===\n");

            // ── CONCEPT: Nullable Type Patterns ────────────────────────────────
            // Pattern matching works with nullable value types

            // Example 1: Nullable<int> pattern matching
            // Output: 1. Nullable Type Patterns:
            Console.WriteLine("1. Nullable Type Patterns:");
            
            // int? = nullable int (Nullable<int>) - can hold null or int
            int? nullableInt = 42;
            int? nullInt = null;

            // is int value = checks if nullable has value AND is int
            // Output: 42 has value: True
            Console.WriteLine($"   42 has value: {nullableInt is int value}");
            
            // Output: null has value: False
            Console.WriteLine($"   null has value: {nullInt is int value}");

            // Example 2: Using captured nullable value
            // Output: 2. Capturing Nullable Values:
            Console.WriteLine("\n2. Capturing Nullable Values:");
            
            // Process nullable int if it has a value
            if (nullableInt is int positiveValue)
            {
                // positiveValue = the captured int value
                // Output: Processing value: 42
                Console.WriteLine($"   Processing value: {positiveValue}");
            }

            // ── CONCEPT: Var Pattern in Type Matching ────────────────────────
            // Var pattern captures any type including null

            // Example 3: Var pattern captures anything
            // Output: 3. Var Pattern:
            Console.WriteLine("\n3. Var Pattern:");
            
            // object[] = array of objects
            object[] items = { "hello", 42, null, 3.14, 'x' };
            
            foreach (object item in items)
            {
                // item is var v = always matches and captures value
                // This is useful for debugging or logging
                if (item is var v)
                {
                    // v = captured value (could be null)
                    // Output: Item: [type] = [value]
                    Console.WriteLine($"   Item: {v?.GetType().Name ?? "null"} = {v}");
                }
            }

            // ── CONCEPT: Combining Type and Constant Patterns ───────────────
            // Can combine type check with specific constant value check

            // Example 4: Type + Constant pattern
            // Output: 4. Combined Type and Constant Patterns:
            Console.WriteLine("\n4. Combined Type and Constant Patterns:");
            
            // object[] testValues = array of test objects
            object[] testValues = { 0, 1, 42, 100, "test", null };
            
            foreach (object val in testValues)
            {
                // DescribeValue combines type and constant matching
                // Output: [description]
                Console.WriteLine($"   {DescribeValue(val)}");
            }

            // ── CONCEPT: Switch Statement with Type Patterns ─────────────────
            // Type patterns work in traditional switch statements too

            // Example 5: Switch statement type patterns
            // Output: 5. Switch Statement Type Patterns:
            Console.WriteLine("\n5. Switch Statement Type Patterns:");
            
            // Test various objects through switch
            // SwitchStatementExample returns description
            Console.WriteLine($"   100: {SwitchStatementExample(100)}");
            Console.WriteLine($"   \"hello\": {SwitchStatementExample("hello")}");
            Console.WriteLine($"   3.14: {SwitchStatementExample(3.14)}");
            Console.WriteLine($"   true: {SwitchStatementExample(true)}");

            // ── CONCEPT: Pattern Matching with Generics ──────────────────────
            // Works with generic types

            // Example 6: Generic type pattern
            // Output: 6. Generic Type Patterns:
            Console.WriteLine("\n6. Generic Type Patterns:");
            
            // Process generic container
            // ProcessContainer demonstrates generic pattern matching
            ProcessContainer(new Box<string>("Test"));
            ProcessContainer(new Box<int>(123));
            ProcessContainer(new Box<DateTime>(DateTime.Now));

            // ── REAL-WORLD EXAMPLE: API Response Handler ─────────────────────
            // Output: --- Real-World Scenario: API Response Handler ---
            Console.WriteLine("\n--- Real-World Scenario: API Response Handler ---");
            
            // Handle different API response types
            HandleApiResponse(new SuccessResponse("Data retrieved", new { Id = 1, Name = "Test" }));
            HandleApiResponse(new ErrorResponse(404, "Not Found"));
            HandleApiResponse(new LoadingResponse());
            HandleApiResponse(new UnauthorizedResponse("Token expired"));

            Console.WriteLine("\n=== Type Pattern Part 2 Complete ===");
        }

        /// <summary>
        /// Demonstrates combined type and constant patterns
        /// </summary>
        public static string DescribeValue(object value)
        {
            // Combine type check with constant value check
            // This is like pattern matching with conditions
            
            // switch expression with combined patterns
            return value switch
            {
                // int n when n > 0 = type + relational pattern
                // when clause adds condition to pattern
                int n when n > 0 => $"Positive integer: {n}",
                
                // int n when n < 0 = negative integer
                int n when n < 0 => $"Negative integer: {n}",
                
                // int 0 = constant pattern (exact match)
                int 0 => "Zero",
                
                // string s when s.Length > 5 = string with length condition
                string s when s.Length > 5 => $"Long string ({s.Length} chars): {s}",
                
                // string s = any other string
                string s => $"String: {s}",
                
                // null = explicit null match
                null => "Null value",
                
                // _ = default case (matches anything)
                _ => $"Other type: {value?.GetType().Name}"
            };
        }

        /// <summary>
        /// Switch statement demonstration with type patterns
        /// </summary>
        public static string SwitchStatementExample(object value)
        {
            // Traditional switch with type patterns
            switch (value)
            {
                case int i:
                    return $"Integer: {i}";
                case string s:
                    return $"String: {s}";
                case double d:
                    return $"Double: {d:F2}";
                case bool b:
                    return $"Boolean: {b}";
                case null:
                    return "Null";
                default:
                    return $"Unknown: {value?.GetType().Name}";
            }
        }

        /// <summary>
        /// Generic container pattern matching
        /// </summary>
        public static void ProcessContainer(object box)
        {
            // Pattern match generic Box<T>
            if (box is Box<string> stringBox)
            {
                // Box<T>.Value returns the contained value
                // Output: String Box: [value]
                Console.WriteLine($"   String Box: {stringBox.Value}");
            }
            else if (box is Box<int> intBox)
            {
                // Output: Integer Box: [value]
                Console.WriteLine($"   Integer Box: {intBox.Value}");
            }
            else if (box is Box<DateTime> dateBox)
            {
                // DateTime.ToString("yyyy-MM-dd") = format date
                // Output: Date Box: [formatted date]
                Console.WriteLine($"   Date Box: {dateBox.Value:yyyy-MM-dd}");
            }
            else
            {
                // Output: Unknown Box type
                Console.WriteLine($"   Unknown Box type");
            }
        }

        /// <summary>
        /// Real-world: Handle different API response types
        /// </summary>
        public static void HandleApiResponse(object response)
        {
            // Type pattern matching for API responses
            // Each response type has different properties
            
            switch (response)
            {
                case SuccessResponse success:
                    // SuccessResponse has Data property
                    // Output: [Response] Success: [message] - Data: [data]
                    Console.WriteLine($"   [Response] Success: {success.Message} - Data: {success.Data}");
                    break;
                    
                case ErrorResponse error:
                    // ErrorResponse has Code and Message
                    // Output: [Response] Error [code]: [message]
                    Console.WriteLine($"   [Response] Error [{error.Code}]: {error.Message}");
                    break;
                    
                case LoadingResponse:
                    // LoadingResponse has no additional properties
                    // Output: [Response] Loading...
                    Console.WriteLine($"   [Response] Loading...");
                    break;
                    
                case UnauthorizedResponse unauthorized:
                    // UnauthorizedResponse has Reason property
                    // Output: [Response] Unauthorized: [reason]
                    Console.WriteLine($"   [Response] Unauthorized: {unauthorized.Reason}");
                    break;
                    
                default:
                    // Output: [Response] Unknown response type
                    Console.WriteLine($"   [Response] Unknown response type");
                    break;
            }
        }
    }

    /// <summary>
    /// Generic box container for type demonstration
    /// </summary>
    /// <typeparam name="T">Type of value stored in box</typeparam>
    public class Box<T>
    {
        // T Value = generic property storing the value
        public T Value { get; }

        // Constructor initializes value
        public Box(T value)
        {
            Value = value;
        }
    }

    // ── REAL-WORLD EXAMPLE: API Response Classes ────────────────────────────
    /// <summary>
    /// Success response with data payload
    /// </summary>
    public class SuccessResponse
    {
        public string Message { get; }
        public object Data { get; }

        public SuccessResponse(string message, object data)
        {
            Message = message;
            Data = data;
        }
    }

    /// <summary>
    /// Error response with error code and message
    /// </summary>
    public class ErrorResponse
    {
        public int Code { get; }
        public string Message { get; }

        public ErrorResponse(int code, string message)
        {
            Code = code;
            Message = message;
        }
    }

    /// <summary>
    /// Loading response for async operations
    /// </summary>
    public class LoadingResponse { }

    /// <summary>
    /// Unauthorized access response
    /// </summary>
    public class UnauthorizedResponse
    {
        public string Reason { get; }

        public UnauthorizedResponse(string reason)
        {
            Reason = reason;
        }
    }
}
