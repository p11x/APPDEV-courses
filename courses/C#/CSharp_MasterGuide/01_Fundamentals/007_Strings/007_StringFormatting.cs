/*
 * ============================================================
 * TOPIC     : Fundamentals - Strings
 * SUBTOPIC  : String Formatting
 * FILE      : StringFormatting.cs
 * PURPOSE   : Teaches various string formatting techniques including
 *            composite formatting, format providers, and custom formats
 * ============================================================
 */

using System; // Core System namespace
using System.Globalization; // For CultureInfo

namespace CSharp_MasterGuide._01_Fundamentals._07_Strings
{
    class StringFormatting
    {
        static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════
            // SECTION 1: Composite Formatting (string.Format)
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Basic positional formatting ─────────────────
            // {index} - positional placeholder
            string name = "Alice";
            int age = 30;
            decimal salary = 75000.50m;
            
            string result = string.Format("Name: {0}, Age: {1}, Salary: {2:C}", name, age, salary);
            Console.WriteLine(result);
            // Output: Name: Alice, Age: 30, Salary: $75,000.50

            // Reusing positions - use same index multiple times
            string repeated = string.Format("{0} {0} {0}", "Echo");
            Console.WriteLine(repeated); // Output: Echo Echo Echo

            // Out of order - can reference positions in any order
            string unordered = string.Format("Third={2}, First={0}, Second={1}", "A", "B", "C");
            Console.WriteLine(unordered); // Output: Third=C, First=A, Second=B

            // ── EXAMPLE 2: Alignment in format ─────────────────────────
            // {index,width} - width specifies minimum character count
            string[] headers = { "Item", "Price", "Qty" };
            string[] items = { "Apple", "0.99", "10" };
            string[] items2 = { "Laptop", "999.99", "1" };
            
            // Right-align numbers, left-align strings
            Console.WriteLine(string.Format("{0,-10} {1,10} {2,5}", headers[0], headers[1], headers[2]));
            Console.WriteLine(new string('-', 28));
            Console.WriteLine(string.Format("{0,-10} {1,10:C} {2,5}", items[0], decimal.Parse(items[1]), int.Parse(items[2])));
            Console.WriteLine(string.Format("{0,-10} {1,10:C} {2,5}", items2[0], decimal.Parse(items2[1]), int.Parse(items2[2])));
            
            // Output: Item              Price  Qty
            // Output: ----------------------------
            // Output: Apple          $0.99   10
            // Output: Laptop       $999.99    1

            // ── EXAMPLE 3: Format specifiers ───────────────────────────
            // Numeric formats
            double pi = 3.14159265;
            Console.WriteLine(string.Format("Default: {0}", pi)); // 3.14159265
            Console.WriteLine(string.Format("F2: {0:F2}", pi)); // 3.14
            Console.WriteLine(string.Format("G3: {0:G3}", pi)); // 3.14
            Console.WriteLine(string.Format("E2: {0:E2}", pi)); // 3.14E+000
            Console.WriteLine(string.Format("P0: {0:P0}", pi)); // 314%

            // DateTime formats
            DateTime date = new DateTime(2024, 1, 15, 14, 30, 0);
            Console.WriteLine(string.Format("d: {0:d}", date)); // 1/15/2024
            Console.WriteLine(string.Format("D: {0:D}", date)); // Monday, January 15, 2024
            Console.WriteLine(string.Format("t: {0:t}", date)); // 2:30 PM
            Console.WriteLine(string.Format("T: {0:T}", date)); // 2:30:00 PM
            Console.WriteLine(string.Format("yyyy-MM-dd: {0:yyyy-MM-dd}", date)); // 2024-01-15

            // ── REAL-WORLD EXAMPLE: Invoice generation ────────────────
            string invoice = string.Format(@"
=====================================
            INVOICE
=====================================
Invoice #: INV-{0}
Date:      {1:yyyy-MM-dd}
Customer:  {2}
-------------------------------------
Item                    Qty    Price
-------------------------------------
{3,-20} {4,4} {5,8:C}
{6,-20} {7,4} {8,8:C}
-------------------------------------
Total:                            {9,8:C}
=====================================",
                12345,
                DateTime.Now,
                "Acme Corp",
                "Widget A", 10, 9.99m,
                "Widget B", 5, 24.99m,
                10 * 9.99m + 5 * 24.99m
            );
            
            Console.WriteLine(invoice);

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: Format Providers and Culture
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Culture-specific formatting ─────────────────
            // Different cultures use different decimal/thousand separators
            decimal amount = 1234567.89m;
            
            // US English
            Console.WriteLine(string.Format(new CultureInfo("en-US"), "US: {0:C}", amount));
            // Output: US: $1,234,567.89

            // German (uses . as thousands separator, , as decimal)
            Console.WriteLine(string.Format(new CultureInfo("de-DE"), "DE: {0:C}", amount));
            // Output: DE: 1.234.567,89 €

            // Japanese
            Console.WriteLine(string.Format(new CultureInfo("ja-JP"), "JP: {0:C}", amount));
            // Output: JP: ¥1,234,568 (rounded)

            // ── EXAMPLE 2: Invariant culture ───────────────────────────
            // For consistent output regardless of system culture
            string invariant = string.Format(CultureInfo.InvariantCulture, 
                "Invariant: {0:F2} | {1:yyyy-MM-dd}", 
                1234.567, 
                DateTime.Now);
            Console.WriteLine(invariant);
            // Output: Invariant: 1234.57 | 2024-01-15

            // Use InvariantCulture for:
            // - Log files
            // - Network protocols
            // - File formats that need consistent parsing
            // - Machine-readable output

            // ── EXAMPLE 3: Custom CultureInfo for formatting ──────────
            // Create custom format provider
            var customCulture = new CultureInfo("en-GB"); // British
            customCulture.NumberFormat.CurrencySymbol = "£";
            customCulture.NumberFormat.CurrencyDecimalPlaces = 0;
            
            string customFormatted = string.Format(customCulture, "Price: {0:C}", 99.99m);
            Console.WriteLine(customFormatted); // Output: Price: £100

            // ── REAL-WORLD EXAMPLE: Multi-region application ───────────
            // Format same data differently for different users
            decimal[] prices = { 99.99m, 150.00m, 2499.99m };
            string[] regions = { "en-US", "de-DE", "ja-JP" };
            
            foreach (string region in regions)
            {
                var culture = new CultureInfo(region);
                string priceList = string.Format(culture, 
                    "Prices: {0:C}, {1:C}, {2:C}", 
                    prices[0], prices[1], prices[2]);
                Console.WriteLine($"{region}: {priceList}");
                // Output: en-US: Prices: $99.99, $150.00, $2,499.99
                // Output: de-DE: Prices: 99,99 €, 150,00 €, 2.499,99 €
                // Output: ja-JP: Prices: ¥100, ¥150, ¥2,500
            }

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: Custom Numeric Format Strings
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Zero placeholder ───────────────────────────
            // 0 - replaces with digit or 0 if no digit
            double num1 = 12.345;
            Console.WriteLine(string.Format("000.00: {0:000.00}", num1)); // 012.35
            Console.WriteLine(string.Format("0000.0: {0:0000.0}", num1)); // 0012.3
            
            double small = 0.123;
            Console.WriteLine(string.Format("0.000: {0:0.000}", small)); // 0.123
            Console.WriteLine(string.Format("0.00: {0:0.00}", small)); // 0.12

            // ── EXAMPLE 2: Digit placeholder ─────────────────────────
            // # - replaces with digit or nothing if no digit
            Console.WriteLine(string.Format("###.##: {0:###.##}", 12.3)); // 12.3
            Console.WriteLine(string.Format("###.##: {0:###.##}", 12.345)); // 12.35
            Console.WriteLine(string.Format("###.##: {0:###.##}", 12)); // 12
            Console.WriteLine(string.Format("###.##: {0:###.##}", 0.12)); // .12

            // Combined: 0 and #
            double combined = 1234.567;
            Console.WriteLine(string.Format("#,##0.00: {0:#,##0.00}", combined)); // 1,234.57
            Console.WriteLine(string.Format("000,000.0: {0:000,000.0}", combined)); // 001,234.6

            // ── EXAMPLE 3: Decimal and percentage ────────────────────
            double value = 0.7567;
            
            Console.WriteLine(string.Format("Percent: {0:P1}", value)); // 75.7%
            Console.WriteLine(string.Format("Percent: {0:P2}", value)); // 75.67%
            
            double currency = 1234.56;
            Console.WriteLine(string.Format("Currency: {0:C0}", currency)); // $1,235 (rounded)
            Console.WriteLine(string.Format("Currency: {0:C}", currency)); // $1,234.56

            // ── EXAMPLE 4: Scientific notation ───────────────────────
            double big = 123456789.0;
            double small2 = 0.000012345;
            
            Console.WriteLine(string.Format("E: {0:E}", big)); // 1.234568E+008
            Console.WriteLine(string.Format("E2: {0:E2}", big)); // 1.23E+008
            Console.WriteLine(string.Format("e: {0:e}", small2)); // 1.2345e-005

            // ── REAL-WORLD EXAMPLE: Scientific data display ───────────
            // Display sensor readings with consistent precision
            double[] readings = { 0.001234, 123.456, 12345.6789, 123456789.0 };
            
            foreach (double reading in readings)
            {
                // Use scientific for very small or large numbers
                string format = reading < 0.01 || reading > 100000 
                    ? "E2" 
                    : "F2";
                Console.WriteLine(string.Format("Reading: {0:" + format + "}", reading));
                // Output: Reading: 1.23E-03
                // Output: Reading: 123.46
                // Output: Reading: 12345.68
                // Output: Reading: 1.23E+008
            }

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: DateTime Custom Formats
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Date components ─────────────────────────────
            DateTime dt = new DateTime(2024, 1, 15, 14, 30, 45);
            
            // Year
            Console.WriteLine(string.Format("yyyy: {0:yyyy}", dt)); // 2024
            Console.WriteLine(string.Format("yy: {0:yy}", dt)); // 24
            Console.WriteLine(string.Format("y: {0:y}", dt)); // January 2024

            // Month
            Console.WriteLine(string.Format("MM: {0:MM}", dt)); // 01
            Console.WriteLine(string.Format("MMM: {0:MMM}", dt)); // Jan
            Console.WriteLine(string.Format("MMMM: {0:MMMM}", dt)); // January

            // Day
            Console.WriteLine(string.Format("dd: {0:dd}", dt)); // 15
            Console.WriteLine(string.Format("ddd: {0:ddd}", dt)); // Mon
            Console.WriteLine(string.Format("dddd: {0:dddd}", dt)); // Monday

            // ── EXAMPLE 2: Time components ─────────────────────────────
            // Hour
            Console.WriteLine(string.Format("HH: {0:HH}", dt)); // 14 (24-hour)
            Console.WriteLine(string.Format("hh: {0:hh}", dt)); // 02 (12-hour)
            Console.WriteLine(string.Format("h: {0:h}", dt)); // 2 (12-hour)

            // Minute/Second
            Console.WriteLine(string.Format("mm: {0:mm}", dt)); // 30
            Console.WriteLine(string.Format("ss: {0:ss}", dt)); // 45

            // AM/PM
            Console.WriteLine(string.Format("tt: {0:tt}", dt)); // PM
            Console.WriteLine(string.Format("t: {0:t}", dt)); // P

            // ── EXAMPLE 3: Combined custom formats ───────────────────
            Console.WriteLine(string.Format("Custom1: {0:yyyy-MM-dd}", dt)); // 2024-01-15
            Console.WriteLine(string.Format("Custom2: {0:dd/MM/yy}", dt)); // 15/01/24
            Console.WriteLine(string.Format("Custom3: {0:MMM d, yyyy}", dt)); // Jan 15, 2024
            Console.WriteLine(string.Format("Custom4: {0:HH:mm:ss}", dt)); // 14:30:45
            Console.WriteLine(string.Format("Custom5: {0:yyyy-MM-dd HH:mm:ss}", dt)); // 2024-01-15 14:30:45

            // ── REAL-WORLD EXAMPLE: Log timestamp formatting ───────────
            DateTime[] logTimes = {
                DateTime.Now,
                DateTime.Now.AddDays(-1),
                DateTime.Now.AddMonths(-1)
            };
            
            foreach (DateTime logTime in logTimes)
            {
                string fileSafe = string.Format("{0:yyyyMMdd_HHmmss}", logTime);
                string human = string.Format("{0:yyyy-MM-dd HH:mm:ss.fff}", logTime);
                string iso = string.Format("{0:o}", logTime); // ISO 8601
                
                Console.WriteLine($"File: {fileSafe}");
                Console.WriteLine($"Human: {human}");
                Console.WriteLine($"ISO: {iso}");
                Console.WriteLine();
            }

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: IFormatProvider Implementation
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Custom format provider ─────────────────────
            // Implement IFormatProvider for custom formatting behavior
            var custom = new CustomFormatter();
            
            object[] values = { 42, "test", 3.14159 };
            foreach (object value in values)
            {
                string formatted = string.Format(custom, "Value: {0:CUSTOM}", value);
                Console.WriteLine(formatted);
            }

            // ── EXAMPLE 2: ICustomFormatter ───────────────────────────
            // Custom formatter for specific types
            var hexFormatter = new HexFormatter();
            
            int num = 255;
            byte b = 128;
            long l = 65535;
            
            Console.WriteLine(string.Format(hexFormatter, "int: {0:HEX}", num)); // int: FF
            Console.WriteLine(string.Format(hexFormatter, "byte: {0:HEX}", b)); // byte: 80
            Console.WriteLine(string.Format(hexFormatter, "long: {0:HEX}", l)); // long: FFFF

            // ── REAL-WORLD EXAMPLE: Custom logging formatter ──────────
            var logFormatter = new LogFormatter();
            
            string logMsg = string.Format(logFormatter, 
                "[{0:LEVEL}] {1:MESSAGE}", 
                "INFO", 
                "Application started successfully");
            Console.WriteLine(logMsg);

            Console.WriteLine("\n=== String Formatting Complete ===");
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Custom Format Provider Examples
    // ═══════════════════════════════════════════════════════════

    class CustomFormatter : IFormatProvider
    {
        public object GetFormat(Type formatType)
        {
            if (formatType == typeof(ICustomFormatter))
                return new Custom ICustomFormatter();
            return null;
        }
    }

    class CustomICustomFormatter : ICustomFormatter
    {
        public string Format(string format, object arg, IFormatProvider provider)
        {
            if (format == "CUSTOM")
            {
                if (arg is int i) return $"[INT:{i}]";
                if (arg is string s) return $"[STR:{s}]";
                if (arg is double d) return $"[DBL:{d:F2}]";
            }
            // Fallback to default
            if (arg is IFormattable f)
                return f.ToString(format, CultureInfo.CurrentCulture);
            return arg?.ToString() ?? "";
        }
    }

    class HexFormatter : IFormatProvider, ICustomFormatter
    {
        public object GetFormat(Type formatType)
        {
            return formatType == typeof(ICustomFormatter) ? this : null;
        }

        public string Format(string format, object arg, IFormatProvider provider)
        {
            if (format?.ToUpper() == "HEX")
            {
                if (arg is int i) return Convert.ToString(i, 16).ToUpper();
                if (arg is byte b) return Convert.ToString(b, 16).ToUpper().PadLeft(2, '0');
                if (arg is long l) return Convert.ToString(l, 16).ToUpper();
            }
            if (arg is IFormattable f)
                return f.ToString(format, CultureInfo.CurrentCulture);
            return arg?.ToString() ?? "";
        }
    }

    class LogFormatter : IFormatProvider, ICustomFormatter
    {
        public object GetFormat(Type formatType)
        {
            return formatType == typeof(ICustomFormatter) ? this : null;
        }

        public string Format(string format, object arg, IFormatProvider provider)
        {
            if (format?.ToUpper() == "LEVEL")
                return $"[{arg}]".ToUpper();
            if (format?.ToUpper() == "MESSAGE")
                return arg?.ToString()?.ToUpper() ?? "";
            
            return arg?.ToString() ?? "";
        }
    }
}