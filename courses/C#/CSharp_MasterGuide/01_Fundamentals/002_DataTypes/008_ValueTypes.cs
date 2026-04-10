/*
 * ============================================================
 * TOPIC     : C# Fundamentals
 * SUBTOPIC  : Data Types - Value Types (Part 1)
 * FILE      : ValueTypes.cs
 * PURPOSE   : This file covers C# value types including integral, floating-point, decimal, boolean, and char types.
 *             Value types are stored on the stack and hold their data directly.
 * ============================================================
 */

// --- SECTION: Introduction to Value Types ---
// Value types are primitive data types that store their value directly in memory.
// When assigned to another variable, a copy of the value is made.
// Value types include: integral, floating-point, decimal, boolean, char, and structs.
// They are stored on the stack (for local variables) or as part of other objects.

using System;

namespace CSharp_MasterGuide._01_Fundamentals._02_DataTypes
{
    class ValueTypes
    {
        static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════════
            // SECTION: Integral Types (Whole Numbers)
            // ═══════════════════════════════════════════════════════════════
            // Integral types store whole numbers (positive, negative, or zero)
            // They differ in range and memory size

            // ── byte: 8-bit unsigned integer (0 to 255) ──────────────────────
            // Used for: pixel values, file bytes, flags, network protocols
            byte pixelValue = 255; // Maximum value for byte - stores raw color data
            byte minByte = 0;      // Minimum value for byte
            Console.WriteLine($"byte: {pixelValue} to {minByte}"); // Output: byte: 255 to 0

            // ── sbyte: 8-bit signed integer (-128 to 127) ───────────────────
            // Used for: small signed numbers, legacy file formats
            sbyte signedByte = 127;  // Maximum positive value - fits in 7 bits + sign
            sbyte negativeByte = -128; // Minimum negative value
            Console.WriteLine($"sbyte: {signedByte} to {negativeByte}"); // Output: sbyte: 127 to -128

            // ── short: 16-bit signed integer (-32,768 to 32,767) ────────────
            // Used for: small integers, coordinate values, array indices
            short shortValue = 32767; // Maximum value for short (Int16)
            short shortMin = -32768;  // Minimum value for short
            Console.WriteLine($"short: {shortValue} to {shortMin}"); // Output: short: 32767 to -32768

            // ── ushort: 16-bit unsigned integer (0 to 65,535) ───────────────
            // Used for: port numbers, flags, version numbers
            ushort unsignedShort = 65535; // Maximum value for ushort (UInt16)
            Console.WriteLine($"ushort: 0 to {unsignedShort}"); // Output: ushort: 0 to 65535

            // ── int: 32-bit signed integer (-2,147,483,648 to 2,147,483,647) ─
            // Used for: most common integer type, loop counters, array lengths
            int intValue = 2147483647; // Maximum value for int (Int32) - ~2.1 billion
            int intMin = -2147483648;  // Minimum value for int
            Console.WriteLine($"int: {intValue} to {intMin}"); // Output: int: 2147483647 to -2147483648

            // ── uint: 32-bit unsigned integer (0 to 4,294,967,295) ───────────
            // Used for: memory sizes, file sizes, bit flags
            uint unsignedInt = 4294967295U; // The U suffix denotes unsigned int literal
            Console.WriteLine($"uint: 0 to {unsignedInt}"); // Output: uint: 0 to 4294967295

            // ── long: 64-bit signed integer ───────────────────────────────────
            // Used for: large numbers, file sizes, timestamps (Unix epoch)
            long longValue = 9223372036854775807L; // Maximum value for long (Int64)
            long longMin = -9223372036854775808L;  // Minimum value for long
            Console.WriteLine($"long: {longValue} to {longMin}"); // Output: long: 9223372036854775807 to -9223372036854775808

            // ── ulong: 64-bit unsigned integer ───────────────────────────────
            // Used for: very large positive numbers, hash values
            ulong unsignedLong = 18446744073709551615UL; // Maximum value for ulong (UInt64)
            Console.WriteLine($"ulong: 0 to {unsignedLong}"); // Output: ulong: 0 to 18446744073709551615

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Floating-Point Types (Decimal Numbers)
            // ═══════════════════════════════════════════════════════════════
            // Floating-point types represent numbers with fractional parts
            // They follow IEEE 754 standard for floating-point arithmetic

            // ── float: 32-bit single-precision floating-point ────────────────
            // Precision: ~6-7 significant digits
            // Used for: graphics, physics, games, when memory is constrained
            float floatValue = 3.14159f; // The f suffix denotes float literal (required!)
            float floatNegative = -273.15f; // Can store negative values (temperature in Celsius)
            float floatScientific = 1.5e-10f; // Scientific notation: 1.5 × 10^-10
            Console.WriteLine($"float: {floatValue}, {floatNegative}, {floatScientific}");
            // Output: float: 3.14159, -273.15, 1.5E-10

            // ── double: 64-bit double-precision floating-point ──────────────
            // Precision: ~15-16 significant digits
            // Used for: scientific calculations, most general-purpose decimal math
            double doubleValue = 3.141592653589793; // Default for decimal literals in C#
            double doubleNegative = -1.7976931348623157E+308; // Maximum positive value
            double doubleSmall = 4.9406564584124654E-324; // Minimum positive value (subnormal)
            Console.WriteLine($"double: {doubleValue}");
            Console.WriteLine($"double range: {doubleSmall} to {doubleNegative}");
            // Output:
            // double: 3.141592653589793
            // double range: 4.9406564584124654E-324 to 1.7976931348623157E+308

            // ── float vs double precision demonstration ──────────────────────
            float floatPi = 3.141592653589793238f; // Truncated to float precision
            double doublePi = 3.141592653589793238; // Full double precision
            Console.WriteLine($"float Pi: {floatPi}"); // Output: float Pi: 3.141593 (rounded)
            Console.WriteLine($"double Pi: {doublePi}"); // Output: double Pi: 3.141592653589793

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Decimal Type (High Precision)
            // ═══════════════════════════════════════════════════════════════
            // Decimal type provides higher precision than floating-point
            // Ideal for financial and monetary calculations

            // ── decimal: 128-bit high-precision decimal ───────────────────────
            // Precision: 28-29 significant digits
            // Used for: financial calculations, currency, accounting
            decimal decimalValue = 79228162514264337593543950335m; // Maximum value (Decimal.MaxValue)
            decimal decimalMin = -79228162514264337593543950335m;  // Minimum value (Decimal.MinValue)
            decimal currency = 1234.56m; // Money values should use decimal, not float/double
            decimal smallDecimal = 0.0000001m; // Can represent very small fractions precisely
            
            Console.WriteLine($"decimal max: {decimalValue}"); // Output: decimal max: 79228162514264337593543950335
            Console.WriteLine($"currency: {currency:C}"); // Output: currency: $1234.56 (C = Currency format)
            Console.WriteLine($"small decimal: {smallDecimal}"); // Output: small decimal: 0.0000001

            // ── Why not use float/double for money? ─────────────────────────
            // Floating-point cannot precisely represent certain decimal values
            // This leads to rounding errors in financial calculations
            
            // PROBLEM: float/double produces rounding errors
            float floatMoney = 0.1f; // Cannot represent exactly 0.1
            Console.WriteLine($"float 0.1: {floatMoney}"); // Output: float 0.1: 0.1 (but internally 0.10000000149011612)
            
            double doubleMoney = 0.1d; // Same issue with double
            Console.WriteLine($"double 0.1: {doubleMoney}"); // Output: double 0.1: 0.1 (but internally 0.10000000000000001)
            
            // SOLUTION: Use decimal for exact representation
            decimal decimalMoney = 0.1m; // Can represent exactly 0.1
            Console.WriteLine($"decimal 0.1: {decimalMoney}"); // Output: decimal 0.1: 0.1

            // Practical example: adding money with float vs decimal
            float floatTotal = 0.0f;
            for (int i = 0; i < 10; i++) floatTotal += 0.1f; // Add 0.1 ten times
            Console.WriteLine($"float 0.1 * 10: {floatTotal}"); // Output: float 0.1 * 10: 1 (approximately!)
            
            decimal decimalTotal = 0.0m;
            for (int i = 0; i < 10; i++) decimalTotal += 0.1m; // Add 0.1 ten times
            Console.WriteLine($"decimal 0.1 * 10: {decimalTotal}"); // Output: decimal 0.1 * 10: 1 (exact!)
        }
    }
}
