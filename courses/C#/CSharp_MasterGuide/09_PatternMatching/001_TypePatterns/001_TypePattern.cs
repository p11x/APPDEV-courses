/*
 * ============================================================
 * TOPIC     : Pattern Matching
 * SUBTOPIC  : Type Patterns
 * FILE      : 01_TypePattern.cs
 * PURPOSE   : Demonstrates type pattern matching with 'is' and 'as' operators,
 *            type checking, and pattern-based type conversion
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._09_PatternMatching._01_TypePatterns
{
    /// <summary>
    /// Demonstrates type pattern matching in C# for type-safe operations
    /// </summary>
    public class TypePattern
    {
        /// <summary>
        /// Entry point demonstrating type pattern matching
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs message to console
            // Output: === Type Pattern Matching Demo ===
            Console.WriteLine("=== Type Pattern Matching Demo ===\n");

            // ── CONCEPT: Type Pattern with 'is' Operator ─────────────────────
            // The 'is' operator checks if an object is of a specific type
            // Can also perform pattern matching by capturing the result

            // Example 1: Basic type checking with is
            // Output: 1. Basic Type Checking:
            Console.WriteLine("1. Basic Type Checking:");
            
            // object = base type that can hold any reference type
            object obj1 = "Hello, World!";
            object obj2 = 42;
            object obj3 = 3.14;
            object obj4 = true;

            // Check if obj1 is a string using 'is' operator
            // bool isString = result of type check
            bool isString = obj1 is string;
            // Output: obj1 is string: True
            Console.WriteLine($"   obj1 is string: {isString}");
            
            // Check obj2 is int (boxed as object)
            // int = 32-bit signed integer
            // Output: obj2 is int: True (boxed value)
            Console.WriteLine($"   obj2 is int: {obj2 is int}");

            // ── CONCEPT: Type Pattern with Variable Capture ──────────────────
            // 'is' can capture the matched value in a new variable
            // This is useful for further operations on the matched type

            // Example 2: Type pattern with variable capture
            // Output: 2. Type Pattern with Capture:
            Console.WriteLine("\n2. Type Pattern with Capture:");
            
            // object sample = test object containing string
            object sample = "Captured string";
            
            // if (sample is string s) = checks type AND captures to variable s
            // This pattern matching extracts the value if type matches
            if (sample is string s)
            {
                // s = the captured string value
                // string.Length = number of characters in string
                // Output: Found string: Captured string, Length: 16
                Console.WriteLine($"   Found string: {s}, Length: {s.Length}");
            }

            // Example 3: Type pattern with different types
            // object[] = array that can hold any type of object
            object[] mixed = new object[] { "first", 42, 3.14, 'c', true };
            
            // foreach = iterate through each element in array
            foreach (object item in mixed)
            {
                // Pattern matching with is to identify types
                // Each is-check captures the value if matched
                if (item is string strItem)
                {
                    // Output: String: [value]
                    Console.WriteLine($"   String: {strItem}");
                }
                else if (item is int intItem)
                {
                    // Output: Integer: [value]
                    Console.WriteLine($"   Integer: {intItem}");
                }
                else if (item is double dblItem)
                {
                    // Output: Double: [value]
                    Console.WriteLine($"   Double: {dblItem}");
                }
                else if (item is char charItem)
                {
                    // Output: Character: [value]
                    Console.WriteLine($"   Character: {charItem}");
                }
                else if (item is bool boolItem)
                {
                    // Output: Boolean: [value]
                    Console.WriteLine($"   Boolean: {boolItem}");
                }
            }

            // ── CONCEPT: Negated Type Pattern ────────────────────────────────
            // 'is not' checks that object is NOT of a specified type

            // Example 4: Negated type pattern
            // Output: 3. Negated Type Pattern:
            Console.WriteLine("\n3. Negated Type Pattern:");
            
            // object notString = integer value (not string)
            object notString = 100;
            
            // is not string = true if object is NOT a string
            // Output: Is not a string: True
            Console.WriteLine($"   Is not a string: {notString is not string}");

            // ── CONCEPT: Null Check with Type Pattern ─────────────────────────
            // Type pattern automatically handles null (null fails type check)

            // Example 5: Null-safe type pattern
            // Output: 4. Null-Safe Type Pattern:
            Console.WriteLine("\n4. Null-Safe Type Pattern:");
            
            // string? nullable = nullable string (can be null)
            string? nullable = null;
            
            // null check combined with type check
            // is string = false when value is null
            // Output: null is string: False
            Console.WriteLine($"   null is string: {nullable is string}");
            
            // Non-null string passes the check
            // string? nonNull = string that has value
            string? nonNull = "Hello";
            // Output: "Hello" is string: True
            Console.WriteLine($"   \"Hello\" is string: {nonNull is string}");

            // ── CONCEPT: Switch Expression Type Patterns ─────────────────────
            // Type patterns work in switch expressions for clean type handling

            // Example 6: Switch expression with type patterns
            // Output: 5. Switch Expression Type Patterns:
            Console.WriteLine("\n5. Switch Expression Type Patterns:");
            
            // Process each object through switch expression
            // DescribeObject returns string description based on runtime type
            Console.WriteLine($"   42 -> {DescribeObject(42)}");
            Console.WriteLine($"   \"test\" -> {DescribeObject("test")}");
            Console.WriteLine($"   3.14 -> {DescribeObject(3.14)}");
            Console.WriteLine($"   true -> {DescribeObject(true)}");

            // ── REAL-WORLD EXAMPLE: Object Processor ─────────────────────────
            // Output: --- Real-World Scenario: Payment Processor ---
            Console.WriteLine("\n--- Real-World Scenario: Payment Processor ---");
            
            // Process different payment types using pattern matching
            // IPayment = interface for payment methods (simulated)
            ProcessPayment(new CreditCardPayment("Alice", 150.00m));
            ProcessPayment(new CashPayment(50.00m));
            ProcessPayment(new BankTransferPayment("Bob", 200.00m, "ACC123"));
            ProcessPayment(new CheckPayment(75.00m, "CHECK001"));

            Console.WriteLine("\n=== Type Pattern Complete ===");
        }

        /// <summary>
        /// Describes an object based on its runtime type using switch expression
        /// </summary>
        /// <param name="obj">Object to describe</param>
        /// <returns>String description of the object's type and value</returns>
        public static string DescribeObject(object obj)
        {
            // switch expression with type patterns
            // Each case checks type and returns appropriate description
            return obj switch
            {
                // int n = captures integer value
                // $"" = string interpolation
                int n => $"Integer with value {n}",
                
                // string s = captures string value
                string s => $"String with {s.Length} characters",
                
                // double d = captures double value
                double d => $"Double with value {d:F2}",
                
                // bool b = captures boolean value
                bool b => $"Boolean with value {b}",
                
                // _ = discard pattern (matches anything)
                // Returns description for unknown types
                _ => $"Unknown type: {obj?.GetType().Name ?? "null"}"
            };
        }

        /// <summary>
        /// Real-world: Processes different payment types polymorphically
        /// Demonstrates type pattern matching in business logic
        /// </summary>
        /// <param name="payment">Payment object to process</param>
        public static void ProcessPayment(object payment)
        {
            // Type pattern matching to handle different payment methods
            // Each case extracts specific properties for processing
            
            if (payment is CreditCardPayment credit)
            {
                // CreditCardPayment has CardholderName and Amount properties
                // decimal Amount = payment amount
                // Output: [Payment] Credit Card: Alice pays $150.00
                Console.WriteLine($"   [Payment] Credit Card: {credit.CardholderName} pays ${credit.Amount:F2}");
            }
            else if (payment is CashPayment cash)
            {
                // Output: [Payment] Cash payment of $50.00
                Console.WriteLine($"   [Payment] Cash payment of ${cash.Amount:F2}");
            }
            else if (payment is BankTransferPayment transfer)
            {
                // BankTransferPayment has AccountNumber property
                // Output: [Payment] Bank Transfer: Bob pays $200.00 (ACC123)
                Console.WriteLine($"   [Payment] Bank Transfer: {transfer.AccountName} pays ${transfer.Amount:F2} ({transfer.AccountNumber})");
            }
            else if (payment is CheckPayment check)
            {
                // CheckPayment has CheckNumber property
                // Output: [Payment] Check #CHECK001 for $75.00
                Console.WriteLine($"   [Payment] Check #{check.CheckNumber} for ${check.Amount:F2}");
            }
            else
            {
                // Unknown payment type
                // Output: [Payment] Unknown payment type
                Console.WriteLine($"   [Payment] Unknown payment type");
            }
        }
    }

    // ── REAL-WORLD EXAMPLE: Payment Classes ────────────────────────────────
    /// <summary>
    /// Credit card payment implementation
    /// </summary>
    public class CreditCardPayment
    {
        // string = reference type for cardholder name
        public string CardholderName { get; }
        
        // decimal = precise type for monetary amounts
        public decimal Amount { get; }

        // Constructor takes name and amount
        public CreditCardPayment(string name, decimal amount)
        {
            CardholderName = name;
            Amount = amount;
        }
    }

    /// <summary>
    /// Cash payment implementation
    /// </summary>
    public class CashPayment
    {
        public decimal Amount { get; }

        public CashPayment(decimal amount)
        {
            Amount = amount;
        }
    }

    /// <summary>
    /// Bank transfer payment implementation
    /// </summary>
    public class BankTransferPayment
    {
        public string AccountName { get; }
        public decimal Amount { get; }
        public string AccountNumber { get; }

        public BankTransferPayment(string name, decimal amount, string account)
        {
            AccountName = name;
            Amount = amount;
            AccountNumber = account;
        }
    }

    /// <summary>
    /// Check payment implementation
    /// </summary>
    public class CheckPayment
    {
        public decimal Amount { get; }
        public string CheckNumber { get; }

        public CheckPayment(decimal amount, string checkNumber)
        {
            Amount = amount;
            CheckNumber = checkNumber;
        }
    }
}
