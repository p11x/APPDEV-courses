/*
 * ============================================================
 * TOPIC     : C# Fundamentals
 * SUBTOPIC  : Data Types - Type Conversion (Part 2)
 * FILE      : TypeConversion_Part2.cs
 * PURPOSE   : This file covers advanced type conversion topics including custom conversion operators,
 *             IConvertible interface, and converting between complex types.
 * ============================================================
 */

// --- SECTION: Advanced Type Conversion ---
// This file covers advanced conversion scenarios including custom conversion operators,
// IConvertible implementation, and complex type conversions

using System;
using System.Globalization;

namespace CSharp_MasterGuide._01_Fundamentals._02_DataTypes
{
    // Custom class demonstrating conversion operators
    class Money
    {
        public decimal Amount { get; } // Immutable amount
        public string Currency { get; } // Currency code
        
        public Money(decimal amount, string currency = "USD")
        {
            Amount = amount;
            Currency = currency;
        }
        
        // ── Implicit conversion from decimal ────────────────────────────────
        // Allows: Money m = 100m; (implicit conversion from decimal)
        public static implicit operator Money(decimal amount)
        {
            return new Money(amount, "USD");
        }
        
        // ── Explicit conversion to int ────────────────────────────────────
        // Requires cast: int dollars = (int)money;
        public static explicit operator int(Money m)
        {
            return (int)m.Amount; // Truncates decimal portion
        }
        
        // ── Conversion to double ─────────────────────────────────────────
        public static explicit operator double(Money m)
        {
            return (double)m.Amount;
        }
        
        // Override ToString for display
        public override string ToString() => $"{Currency} {Amount:N2}";
    }
    
    // Temperature class for conversion examples
    class Temperature
    {
        public double Celsius { get; } // Store temperature in Celsius
        
        public Temperature(double celsius)
        {
            Celsius = celsius;
        }
        
        // Implicit conversion from double (assumes Celsius)
        public static implicit operator Temperature(double celsius)
        {
            return new Temperature(celsius);
        }
        
        // Explicit conversion to Fahrenheit
        public static explicit operator double(Temperature t)
        {
            return t.Celsius * 9 / 5 + 32;
        }
        
        public override string ToString() => $"{Celsius:F1}°C";
    }
    
    // IConvertible implementation example
    class CustomNumber : IConvertible
    {
        private readonly int _value;
        
        public CustomNumber(int value)
        {
            _value = value;
        }
        
        // Required IConvertible implementations
        public TypeCode GetTypeCode() => TypeCode.Int32;
        
        public bool ToBoolean(IFormatProvider provider) => _value != 0;
        
        public byte ToByte(IFormatProvider provider) => (byte)_value;
        
        public char ToChar(IFormatProvider provider) => (char)_value;
        
        public DateTime ToDateTime(IFormatProvider provider) 
            => throw new InvalidCastException("Cannot convert to DateTime");
        
        public decimal ToDecimal(IFormatProvider provider) => _value;
        
        public double ToDouble(IFormatProvider provider) => _value;
        
        public short ToInt16(IFormatProvider provider) => (short)_value;
        
        public int ToInt32(IFormatProvider provider) => _value;
        
        public long ToInt64(IFormatProvider provider) => _value;
        
        public sbyte ToSByte(IFormatProvider provider) => (sbyte)_value;
        
        public float ToSingle(IFormatProvider provider) => _value;
        
        public string ToString(IFormatProvider provider) => _value.ToString();
        
        public object ToType(Type conversionType, IFormatProvider provider)
        {
            if (conversionType == typeof(int))
                return ToInt32(provider);
            if (conversionType == typeof(string))
                return ToString(provider);
            throw new InvalidCastException($"Cannot convert to {conversionType.Name}");
        }
        
        public ushort ToUInt16(IFormatProvider provider) => (ushort)_value;
        
        public uint ToUInt32(IFormatProvider provider) => (uint)_value;
        
        public ulong ToUInt64(IFormatProvider provider) => (ulong)_value;
    }

    class TypeConversion_Part2
    {
        static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════════
            // SECTION: Custom Conversion Operators
            // ═══════════════════════════════════════════════════════════════
            
            // ── Implicit conversion operators ───────────────────────────────
            // Allows natural assignment syntax
            Money salary = 5000m; // Implicit: decimal to Money
            Console.WriteLine($"Implicit from decimal: {salary}"); // Output: Implicit from decimal: USD 5,000.00
            
            Temperature temp1 = 25.0; // Implicit: double to Temperature (assumes Celsius)
            Console.WriteLine($"Implicit from double: {temp1}"); // Output: Implicit from double: 25.0°C
            
            // ── Explicit conversion operators ───────────────────────────────
            // Requires explicit cast
            Money price = 99.99m;
            int dollars = (int)price; // Explicit: Money to int (truncates)
            Console.WriteLine($"Explicit Money to int: {dollars}"); // Output: Explicit Money to int: 99
            
            double priceAsDouble = (double)price; // Explicit: Money to double
            Console.WriteLine($"Explicit Money to double: {priceAsDouble}"); // Output: Explicit Money to double: 99.99
            
            Temperature tempC = new Temperature(100);
            double tempF = (double)tempC; // Explicit: Temperature to Fahrenheit
            Console.WriteLine($"100°C in Fahrenheit: {tempF}"); // Output: 100°C in Fahrenheit: 212
            
            // ── Conversion operator considerations ──────────────────────────
            // Conversions should be: accurate, idempotent, lossless where possible
            Money discount = 50.25m;
            int discountedDollars = (int)discount; // Note: truncates - may not be desired!
            Console.WriteLine($"Discount truncated: {discountedDollars}"); // Output: Discount truncated: 50

            // ═══════════════════════════════════════════════════════════════
            // SECTION: IConvertible Implementation
            // ═══════════════════════════════════════════════════════════════
            
            // ── Using IConvertible ─────────────────────────────────────────
            // Classes implementing IConvertible can use Convert.ToXXX methods
            CustomNumber custom = new CustomNumber(42);
            
            // Convert class methods work with IConvertible
            int asInt = Convert.ToInt32(custom); // Uses IConvertible.ToInt32
            string asStr = Convert.ToString(custom); // Uses IConvertible.ToString
            bool asBool = Convert.ToBoolean(custom); // Uses IConvertible.ToBoolean
            
            Console.WriteLine($"CustomNumber to int: {asInt}"); // Output: CustomNumber to int: 42
            Console.WriteLine($"CustomNumber to string: {asStr}"); // Output: CustomNumber to string: 42
            Console.WriteLine($"CustomNumber to bool: {asBool}"); // Output: CustomNumber to bool: True

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Culture-Specific Conversions
            // ═══════════════════════════════════════════════════════════════
            
            // ── Culture-aware parsing ───────────────────────────────────────
            // Different cultures use different decimal separators
            // US: "3.14", German: "3,14"
            
            // Use invariant culture for consistent parsing
            string numberUS = "3.14";
            string numberDE = "3,14";
            
            double fromUS = double.Parse(numberUS, CultureInfo.InvariantCulture);
            double fromDE = double.Parse(numberDE, CultureInfo.GetCultureInfo("de-DE"));
            
            Console.WriteLine($"US format: {fromUS}"); // Output: US format: 3.14
            Console.WriteLine($"German format: {fromDE}"); // Output: German format: 3.14
            
            // ── InvariantCulture for formatting ───────────────────────────
            // Use InvariantCulture for consistent output format
            double value = 1234.56;
            string formattedUS = value.ToString(CultureInfo.InvariantCulture);
            Console.WriteLine($"Invariant format: {formattedUS}"); // Output: Invariant format: 1234.56
            
            // ── NumberStyles for parsing ───────────────────────────────────
            // Handle various number formats in input
            string withSpaces = "  1,234  ";
            string withHex = "0xFF";
            string currency = "$1,234.56";
            
            int parsedSpaces = int.Parse(withSpaces, NumberStyles.AllowLeadingSpaces | NumberStyles.AllowThousands);
            Console.WriteLine($"Parse with spaces: {parsedSpaces}"); // Output: Parse with spaces: 1234
            
            int parsedHex = int.Parse(withHex, NumberStyles.HexNumber);
            Console.WriteLine($"Parse hex: {parsedHex}"); // Output: Parse hex: 255
            
            decimal parsedCurrency = decimal.Parse(currency, NumberStyles.Currency);
            Console.WriteLine($"Parse currency: {parsedCurrency}"); // Output: Parse currency: 1234.56

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Reference Type Conversions
            // ═══════════════════════════════════════════════════════════════
            
            // ── Upcasting (safe) ───────────────────────────────────────────
            // Derived class to base class is always safe
            string str = "hello";
            object obj = str; // Upcast to object - safe, no data loss
            Console.WriteLine($"Upcast string to object: {obj}"); // Output: Upcast string to object: hello
            
            // ── Downcasting (requires validation) ───────────────────────────
            // Base class to derived class needs validation
            object unknown = "hello";
            // string s = (string)unknown; // Would work - we know it's string
            
            // Safer: use 'as' operator - returns null instead of throwing
            object maybeString = new object();
            string? asString = maybeString as string; // Returns null, not exception
            if (asString != null)
            {
                Console.WriteLine($"as operator success: {asString}");
            }
            else
            {
                Console.WriteLine("as operator failed - not a string"); // Output: as operator failed - not a string
            }
            
            // ── Pattern matching for type checks ───────────────────────────
            object value2 = 42;
            if (value2 is int intValue) // Pattern matching with type check
            {
                Console.WriteLine($"Pattern matched int: {intValue}"); // Output: Pattern matched int: 42
            }
            
            // ── is vs as operators ──────────────────────────────────────────
            // 'is' returns bool, 'as' returns converted reference or null
            object mixed = "test";
            
            bool isString = mixed is string; // is: returns bool
            Console.WriteLine($"is string: {isString}"); // Output: is string: True
            
            string? asCast = mixed as string; // as: returns reference or null
            Console.WriteLine($"as string: {asCast}"); // Output: as string: test

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Custom Type Converter
            // ═══════════════════════════════════════════════════════════════
            
            // ── Using TypeConverterAttribute ───────────────────────────────
            // Can define custom type converters for classes
            // [TypeConverter(typeof(MyCustomConverter))]
            
            // For demonstration, use Convert.ChangeType
            object dateObj = new DateTime(2024, 1, 15);
            string dateStr = Convert.ToString(dateObj);
            Console.WriteLine($"DateTime to string: {dateStr}"); // Output: DateTime to string: 1/15/2024 12:00:00 AM
            
            // ChangeType for custom conversions
            object numObj = 42;
            double changed = (double)Convert.ChangeType(numObj, typeof(double));
            Console.WriteLine($"ChangeType 42 to double: {changed}"); // Output: ChangeType 42 to double: 42

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Real-World Conversion Scenarios
            // ═══════════════════════════════════════════════════════════════
            
            // ── Configuration value parsing ───────────────────────────────
            string configValue = "timeout=30"; // Simulated config
            string[] parts = configValue.Split('=');
            if (parts.Length == 2 && int.TryParse(parts[1], out int timeout))
            {
                Console.WriteLine($"Config timeout: {timeout}"); // Output: Config timeout: 30
            }
            
            // ── Enum conversions ──────────────────────────────────────────
            // Parse string to enum
            string statusStr = "Active";
            // Enum status = (Enum)Enum.Parse(typeof(Enum), statusStr); // Old way
            
            if (Enum.TryParse<DayOfWeek>(statusStr, out DayOfWeek parsed))
            {
                Console.WriteLine($"Parsed enum: {parsed}"); // Output: Parsed enum: (nothing - 'Active' is not DayOfWeek)
            }
            
            // Standard enum parsing
            DayOfWeek day = DayOfWeek.Monday;
            string dayStr = day.ToString();
            Console.WriteLine($"Enum to string: {dayStr}"); // Output: Enum to string: Monday
            
            // ── Bitwise flags conversion ───────────────────────────────────
            // Converting enum flags to/from integer
            FileAttributes attributes = FileAttributes.Hidden | FileAttributes.ReadOnly;
            int flags = (int)attributes;
            Console.WriteLine($"FileAttributes to int: {flags}"); // Output: FileAttributes to int: 6
            
            FileAttributes restored = (FileAttributes)flags;
            Console.WriteLine($"int to FileAttributes: {restored}"); // Output: int to FileAttributes: Hidden, ReadOnly
        }
    }
}
