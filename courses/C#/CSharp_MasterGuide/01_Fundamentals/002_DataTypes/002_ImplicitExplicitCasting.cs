/*
 * ============================================================
 * TOPIC     : C# Fundamentals
 * SUBTOPIC  : Data Types - Implicit and Explicit Casting
 * FILE      : ImplicitExplicitCasting.cs
 * PURPOSE   : This file demonstrates the differences between implicit and explicit casting,
 *             when each is used, and how to safely perform conversions.
 * ============================================================
 */

// --- SECTION: Understanding Implicit vs Explicit Casting ---
// Implicit casting (widening): Happens automatically when no data loss is possible
// Explicit casting (narrowing): Requires manual cast when data loss might occur

using System;

namespace CSharp_MasterGuide._01_Fundamentals._02_DataTypes
{
    class ImplicitExplicitCasting
    {
        static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════════
            // SECTION: Implicit Casting (Automatic - No Data Loss)
            // ═══════════════════════════════════════════════════════════════
            
            // ── byte to short ───────────────────────────────────────────────
            // byte (0-255) can fit in short (-32768 to 32767)
            byte byteValue = 100;
            short shortValue = byteValue; // Automatic widening
            Console.WriteLine($"byte→short: {shortValue}"); // Output: byte→short: 100
            
            // ── short to int ─────────────────────────────────────────────────
            // short (-32768 to 32767) fits in int
            short short2 = 1000;
            int intValue = short2; // Automatic
            Console.WriteLine($"short→int: {intValue}"); // Output: short→int: 1000
            
            // ── int to long ──────────────────────────────────────────────────
            // int fits in long
            int int2 = 50000;
            long longValue = int2;
            Console.WriteLine($"int→long: {longValue}"); // Output: int→long: 50000
            
            // ── char to int/uint/long ────────────────────────────────────────
            // char (0-65535) converts to numeric
            char letter = 'A';
            int ascii = letter; // Unicode 65
            Console.WriteLine($"char→int: {ascii}"); // Output: char→int: 65
            
            // ── int to float/double ──────────────────────────────────────────
            // Integral types convert to floating point
            int num = 42;
            float flt = num;
            double dbl = num;
            Console.WriteLine($"int→float: {flt}"); // Output: int→float: 42
            Console.WriteLine($"int→double: {dbl}"); // Output: int→double: 42
            
            // ── float to double ─────────────────────────────────────────────
            // float (32-bit) converts to double (64-bit)
            float f = 3.14f;
            double d = f;
            Console.WriteLine($"float→double: {d}"); // Output: float→double: 3.140000104904175

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Explicit Casting (Manual - Potential Data Loss)
            // ═══════════════════════════════════════════════════════════════
            
            // ── double/float to int ─────────────────────────────────────────
            // Decimal/truncate portion
            double pi = 3.99;
            int trunc = (int)pi;
            Console.WriteLine($"(int)3.99 = {trunc}"); // Output: (int)3.99 = 3
            
            // ── double to float ─────────────────────────────────────────────
            // Potential precision loss
            double bigD = 3.141592653589793;
            float smallF = (float)bigD;
            Console.WriteLine($"(float)double = {smallF}"); // Output: (float)double = 3.1415927
            
            // ── long to int ─────────────────────────────────────────────────
            // Potential overflow
            long bigLong = 2000;
            int smallInt = (int)bigLong;
            Console.WriteLine($"(int)2000 = {smallInt}"); // Output: (int)2000 = 2000
            
            // Overflow example
            long overflowLong = 3000000000;
            int overflowInt = (int)overflowLong;
            Console.WriteLine($"(int)3000000000 = {overflowInt}"); // Output: (int)3000000000 = -1294967296 (wrap around!)
            
            // ── decimal to double/float ────────────────────────────────────
            decimal dec = 123456789.123456789m;
            double fromDec = (double)dec;
            Console.WriteLine($"(double)decimal = {fromDec}"); // Output: (double)decimal = 123456789.12345679
            
            // ═══════════════════════════════════════════════════════════════
            // SECTION: Safe Casting with 'as' Operator
            // ═══════════════════════════════════════════════════════════════
            
            // ── Using 'as' for reference types ─────────────────────────────
            // Returns null instead of throwing exception
            object obj = "Hello";
            string str1 = obj as string;
            if (str1 != null)
            {
                Console.WriteLine($"as string: {str1}"); // Output: as string: Hello
            }
            
            object notString = 123;
            string? str2 = notString as string; // Returns null, no exception
            Console.WriteLine($"as string (not string): {str2 ?? "null"}"); // Output: as string (not string): null
            
            // ── 'as' can only be used with reference types ─────────────────
            // int? nullable = notString as int?; // Not allowed - value type
            
            // ═══════════════════════════════════════════════════════════════
            // SECTION: Safe Casting with Pattern Matching
            // ═══════════════════════════════════════════════════════════════
            
            // ── is with pattern ────────────────────────────────────────────
            object mixed = 42;
            if (mixed is int matchedInt)
            {
                Console.WriteLine($"Pattern matched: {matchedInt}"); // Output: Pattern matched: 42
            }
            
            // ── is with var pattern ────────────────────────────────────────
            object unknown = "test";
            if (unknown is var something)
            {
                Console.WriteLine($"Var pattern: {something} (type: {something.GetType().Name})");
                // Output: Var pattern: test (type: String)
            }
            
            // ── Switch expression pattern matching ────────────────────────
            object value = 3.14;
            string description = value switch
            {
                int i => $"Integer: {i}",
                double d => $"Double: {d}",
                string s => $"String: {s}",
                _ => $"Unknown type: {value.GetType().Name}"
            };
            Console.WriteLine($"Switch pattern: {description}"); // Output: Switch pattern: Double: 3.14

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Casting with checked/unchecked
            // ═══════════════════════════════════════════════════════════════
            
            // ── unchecked (default) - overflow wraps silently ───────────────
            int wrapped = unchecked(int.MaxValue + 1);
            Console.WriteLine($"unchecked overflow: {wrapped}"); // Output: unchecked overflow: -2147483648
            
            // ── checked - throws OverflowException ─────────────────────────
            try
            {
                int overflowChecked = checked(int.MaxValue + 1);
            }
            catch (OverflowException)
            {
                Console.WriteLine("checked context throws OverflowException"); 
                // Output: checked context throws OverflowException
            }
            
            // ── checked block for expressions ─────────────────────────────
            checked
            {
                try
                {
                    int a = int.MaxValue;
                    int b = 1;
                    // int c = a + b; // Would throw
                }
                catch (OverflowException)
                {
                    Console.WriteLine("Addition overflow detected");
                }
            }

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Real-World Casting Scenarios
            // ═══════════════════════════════════════════════════════════════
            
            // ── User input validation and casting ─────────────────────────
            Console.WriteLine("\n=== Real-World: User Input ===");
            string input = "42";
            if (int.TryParse(input, out int userAge))
            {
                Console.WriteLine($"Valid age: {userAge}");
            }
            
            // ── Database value conversion ─────────────────────────────────
            // Simulating database values
            object dbValue = "150.50"; // Could be returned as string from some DBs
            decimal price = dbValue is string priceStr && decimal.TryParse(priceStr, out price) 
                ? price 
                : 0;
            Console.WriteLine($"Parsed price: {price}"); // Output: Parsed price: 150.50
            
            // ── API response handling ─────────────────────────────────────
            object apiResponse = GetApiResponse();
            if (apiResponse is int statusCode)
            {
                Console.WriteLine($"API status: {statusCode}");
            }
            else if (apiResponse is string message)
            {
                Console.WriteLine($"API message: {message}");
            }
            else
            {
                Console.WriteLine($"Unknown response type: {apiResponse?.GetType().Name}");
            }
        }
        
        static object GetApiResponse() => 200; // Returns int boxed in object
    }
}
