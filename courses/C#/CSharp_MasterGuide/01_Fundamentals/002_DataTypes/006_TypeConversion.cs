/*
 * ============================================================
 * TOPIC     : C# Fundamentals
 * SUBTOPIC  : Data Types - Type Conversion (Part 1)
 * FILE      : TypeConversion.cs
 * PURPOSE   : This file covers type conversion in C# including implicit and explicit conversions.
 *             Explains when conversions happen automatically vs. requiring casting.
 * ============================================================
 */

// --- SECTION: Type Conversion Overview ---
// Type conversion transforms data from one type to another
// Implicit conversions happen automatically (no data loss)
// Explicit conversions (casting) require manual intervention (potential data loss)

using System;

namespace CSharp_MasterGuide._01_Fundamentals._02_DataTypes
{
    class TypeConversion
    {
        static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════════
            // SECTION: Implicit Conversions (Widening)
            // ═══════════════════════════════════════════════════════════════
            // Implicit conversions happen automatically when no data loss can occur
            // The CLR (Common Language Runtime) handles these safely
            
            // ── Integral to larger integral ────────────────────────────────
            // int to long - no data loss possible
            int smallNum = 100;
            long largeNum = smallNum; // Implicit conversion from int to long
            Console.WriteLine($"int to long: {largeNum}"); // Output: int to long: 100
            
            // int to double - always safe
            int num = 42;
            double dbl = num; // int converts to double
            Console.WriteLine($"int to double: {dbl}"); // Output: int to double: 42
            
            // ── Floating-point conversions ────────────────────────────────
            // float to double - more precision, no data loss
            float f = 3.14f;
            double df = f; // Implicit float to double
            Console.WriteLine($"float to double: {df}"); // Output: float to double: 3.140000104904175
            
            // decimal conversions - decimal is highest precision
            decimal dec = 100.50m;
            decimal fromInt = 42; // int to decimal
            decimal fromDouble = 3.14m; // double to decimal (explicit needed for 3.14d)
            Console.WriteLine($"int to decimal: {fromInt}"); // Output: int to decimal: 42
            Console.WriteLine($"double to decimal: {fromDouble}"); // Output: double to decimal: 3.14
            
            // ── Character to numeric ───────────────────────────────────────
            // char converts to integral types (Unicode code point)
            char letter = 'A';
            int ascii = letter; // char to int (Unicode value 65)
            long asciiLong = letter; // char to long
            Console.WriteLine($"char 'A' to int: {ascii}"); // Output: char 'A' to int: 65
            Console.WriteLine($"char 'A' to long: {asciiLong}"); // Output: char 'A' to long: 65

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Explicit Conversions (Narrowing/Casting)
            // ═══════════════════════════════════════════════════════════════
            // Explicit conversions are required when data loss might occur
            // Uses cast syntax: (targetType)value
            
            // ── double/float to integral ───────────────────────────────────
            // Decimal part is truncated (not rounded!)
            double pi = 3.99;
            int truncated = (int)pi; // Truncates to 3
            Console.WriteLine($"(int)3.99 = {truncated}"); // Output: (int)3.99 = 3
            
            // float to int - truncation
            float bigFloat = 99.9f;
            int fromFloat = (int)bigFloat; // Truncates to 99
            Console.WriteLine($"(int)99.9f = {fromFloat}"); // Output: (int)99.9f = 99
            
            // ── Larger integral to smaller ───────────────────────────────
            // Potential data loss if value exceeds target range
            long bigNum = 1000;
            int small = (int)bigNum; // Safe - fits in int range
            Console.WriteLine($"(int)1000 = {small}"); // Output: (int)1000 = 1000
            
            long overflowNum = 3000000000; // Exceeds int.MaxValue (2,147,483,647)
            int overflow = (int)overflowNum; // WRAP AROUND! - unexpected result
            Console.WriteLine($"(int)3000000000 = {overflow}"); // Output: (int)3000000000 = -1294967296 (wrapped!)
            
            // ── decimal to floating-point ─────────────────────────────────
            // Can lose precision but not magnitude
            decimal bigDecimal = 123456789.123456789m;
            double fromDecimal = (double)bigDecimal; // May lose precision
            float fromDecimalFloat = (float)bigDecimal;
            Console.WriteLine($"decimal to double: {fromDecimal}"); // Output: decimal to double: 123456789.12345679
            Console.WriteLine($"decimal to float: {fromDecimalFloat}"); // Output: decimal to float: 1.2345679E+08

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Numeric Type Conversion Methods
            // ═══════════════════════════════════════════════════════════════
            
            // ── Convert.ToXXX methods ───────────────────────────────────────
            // System.Convert class provides safe conversion methods
            // These handle edge cases better than casting
            
            // Convert.ToInt32 - handles various input types
            double toInt = 42.7;
            int converted = Convert.ToInt32(toInt); // ROUNDS to 43, not truncates!
            Console.WriteLine($"Convert.ToInt32(42.7) = {converted}"); // Output: Convert.ToInt32(42.7) = 43
            
            string numStr = "123";
            int fromString = Convert.ToInt32(numStr); // Parse string to int
            Console.WriteLine($"Convert.ToInt32(\"123\") = {fromString}"); // Output: Convert.ToInt32("123") = 123
            
            bool boolVal = true;
            int fromBool = Convert.ToInt32(boolVal); // true = 1, false = 0
            Console.WriteLine($"Convert.ToInt32(true) = {fromBool}"); // Output: Convert.ToInt32(true) = 1
            
            // Convert.ToDouble, ToDecimal, etc.
            string doubleStr = "3.14";
            double fromStr = Convert.ToDouble(doubleStr);
            Console.WriteLine($"Convert.ToDouble(\"3.14\") = {fromStr}"); // Output: Convert.ToDouble("3.14") = 3.14
            
            // ── Convert methods with null handling ─────────────────────────
            // Convert methods handle null gracefully
            object nullObj = null;
            int nullConvert = Convert.ToInt32(nullObj); // Returns 0, doesn't throw
            Console.WriteLine($"Convert.ToInt32(null) = {nullConvert}"); // Output: Convert.ToInt32(null) = 0
            
            // Convert.ToString - safe for any type
            object anyValue = 42;
            string strValue = Convert.ToString(anyValue);
            Console.WriteLine($"Convert.ToString(42) = \"{strValue}\""); // Output: Convert.ToString(42) = "42"

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Parse and TryParse
            // ═══════════════════════════════════════════════════════════════
            
            // ── Parse method ───────────────────────────────────────────────
            // Converts string to specified type - throws on failure
            string intStr = "42";
            int parsed = int.Parse(intStr); // Convert string "42" to int 42
            Console.WriteLine($"int.Parse(\"42\") = {parsed}"); // Output: int.Parse("42") = 42
            
            // Parse with different number styles
            string hexStr = "FF";
            int hexParsed = int.Parse(hexStr, System.Globalization.NumberStyles.HexNumber);
            Console.WriteLine($"int.Parse(\"FF\", HexNumber) = {hexParsed}"); // Output: int.Parse("FF", HexNumber) = 255
            
            // Parse double (culture-aware)
            string doubleStr2 = "3.14";
            double doubleParsed = double.Parse(doubleStr2);
            Console.WriteLine($"double.Parse(\"3.14\") = {doubleParsed}"); // Output: double.Parse("3.14") = 3.14
            
            // Parse failures - throws FormatException
            // int badParse = int.Parse("not a number"); // Would throw FormatException!
            
            // ── TryParse method ───────────────────────────────────────────
            // Safe parsing - returns bool, outputs result if successful
            string validStr = "123";
            if (int.TryParse(validStr, out int result1))
            {
                Console.WriteLine($"TryParse(\"123\") = {result1}"); // Output: TryParse("123") = 123
            }
            
            string invalidStr = "abc";
            if (int.TryParse(invalidStr, out int result2))
            {
                Console.WriteLine($"TryParse(\"abc\") = {result2}");
            }
            else
            {
                Console.WriteLine($"TryParse(\"abc\") failed - use default"); // Output: TryParse("abc") failed - use default
            }
            
            // TryParse with number styles
            string hexTry = "FF";
            if (int.TryParse(hexTry, System.Globalization.NumberStyles.HexNumber, null, out int hexResult))
            {
                Console.WriteLine($"TryParse with HexNumber: {hexResult}"); // Output: TryParse with HexNumber: 255
            }

            // ═══════════════════════════════════════════════════════════════
            // SECTION: String to Number Conversions
            // ═══════════════════════════════════════════════════════════════
            
            // ── Multiple parse/convert scenarios ─────────────────────────
            // Common in user input and file parsing
            
            // Currency strings (remove $ first)
            string currency = "$1,234.56";
            string cleanCurrency = currency.Replace("$", "").Replace(",", "");
            decimal money = decimal.Parse(cleanCurrency);
            Console.WriteLine($"Parsed currency: {money}"); // Output: Parsed currency: 1234.56
            
            // Percentage strings
            string percent = "85%";
            string cleanPercent = percent.Replace("%", "");
            int percentage = int.Parse(cleanPercent);
            Console.WriteLine($"Parsed percentage: {percentage}%"); // Output: Parsed percentage: 85%
            
            // Date strings (different formats)
            // DateTime is covered in DateTime utilities
            
            // Integer with leading zeros
            string withZeros = "007";
            int withLeadingZeros = int.Parse(withZeros); // Parses as 7
            Console.WriteLine($"Parsed with zeros: {withLeadingZeros}"); // Output: Parsed with zeros: 7
        }
    }
}
