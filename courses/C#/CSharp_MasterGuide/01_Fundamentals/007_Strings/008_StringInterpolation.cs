/*
 * ============================================================
 * TOPIC     : Fundamentals - Strings
 * SUBTOPIC  : String Interpolation
 * FILE      : StringInterpolation.cs
 * PURPOSE   : Teaches string interpolation syntax, formatting options,
 *            and advanced interpolation features in C#
 * ============================================================
 */

using System; // Core System namespace

namespace CSharp_MasterGuide._01_Fundamentals._07_Strings
{
    class StringInterpolation
    {
        static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════
            // SECTION 1: Basic String Interpolation
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: $"" - String interpolation prefix ──────────
            // $ enables interpolation - {} contains expressions/variables
            string name = "Alice";
            int age = 30;
            
            string greeting = $"Hello, {name}! You are {age} years old.";
            Console.WriteLine(greeting); 
            // Output: Hello, Alice! You are 30 years old.

            // Works with any expressions inside {}
            string message = $"2 + 2 = {2 + 2}"; // Expression evaluation
            Console.WriteLine(message); // Output: 2 + 2 = 4

            // ── EXAMPLE 2: Interpolating multiple types ──────────────
            // Works with any type - ToString() is called automatically
            decimal price = 99.99m;
            bool inStock = true;
            DateTime date = new DateTime(2024, 1, 15);
            
            string productInfo = $"Price: ${price}, In Stock: {inStock}, Date: {date:yyyy-MM-dd}";
            Console.WriteLine(productInfo); 
            // Output: Price: $99.99, In Stock: True, Date: 2024-01-15

            // Array interpolation
            int[] numbers = { 1, 2, 3 };
            string arrayInfo = $"First: {numbers[0]}, Length: {numbers.Length}";
            Console.WriteLine(arrayInfo); // Output: First: 1, Length: 3

            // ── REAL-WORLD EXAMPLE: User greeting message ────────────
            string firstName = "John";
            string lastName = "Doe";
            string fullName = $"{firstName} {lastName}";
            int loginCount = 42;
            DateTime lastLogin = DateTime.Now.AddDays(-1);
            
            string welcome = $"Welcome back, {fullName}! " +
                           $"Login #{loginCount} - Last login: {lastLogin:MMM dd, yyyy}";
            Console.WriteLine(welcome); 
            // Output: Welcome back, John Doe! Login #42 - Last login: Jan 14, 2024

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: Format Specifiers
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Numeric format specifiers ──────────────────
            // Format: {index,alignment:format}
            double pi = 3.14159265;
            
            // F2 = Fixed-point, 2 decimals
            string piFixed = $"Pi: {pi:F2}"; // "3.14"
            Console.WriteLine(piFixed); // Output: Pi: 3.14

            // C2 = Currency, 2 decimals
            decimal amount = 1234.56m;
            string currency = $"Amount: {amount:C2}"; // "$1,234.56"
            Console.WriteLine(currency); // Output: Amount: $1,234.56

            // N0 = Number with thousands separator, 0 decimals
            long population = 7800000000;
            string number = $"World population: {population:N0}";
            Console.WriteLine(number); // Output: World population: 7,800,000,000

            // P0 = Percentage, 0 decimals
            double successRate = 0.856;
            string percentage = $"Success: {successRate:P0}";
            Console.WriteLine(percentage); // Output: Success: 86%

            // X = Hexadecimal
            int hexValue = 255;
            string hex = $"Hex: {hexValue:X}"; // "FF"
            Console.WriteLine(hex); // Output: Hex: FF

            // ── EXAMPLE 2: DateTime format specifiers ──────────────────
            DateTime now = new DateTime(2024, 1, 15, 10, 30, 45);
            
            string shortDate = $"Short: {now:d}"; // 1/15/2024
            string longDate = $"Long: {now:D}"; // Monday, January 15, 2024
            string shortTime = $"Time: {now:t}"; // 10:30 AM
            string fullDateTime = $"Full: {now:f}"; // Monday, January 15, 2024 10:30 AM
            
            Console.WriteLine(shortDate); // Output: Short: 1/15/2024
            Console.WriteLine(longDate); // Output: Long: Monday, January 15, 2024
            Console.WriteLine(shortTime); // Output: Time: 10:30 AM
            Console.WriteLine(fullDateTime); // Output: Full: Monday, January 15, 2024 10:30 AM

            // Custom patterns
            string custom = $"Custom: {now:yyyy-MM-dd HH:mm:ss}";
            Console.WriteLine(custom); // Output: Custom: 2024-01-15 10:30:45

            // ── EXAMPLE 3: Alignment and padding ─────────────────────
            // {index, width} - right-align, negative for left-align
            string[] products = { "Apple", "Banana", "Strawberry" };
            decimal[] prices = { 0.99m, 0.50m, 3.99m };
            
            Console.WriteLine("Product".PadRight(15) + "Price");
            Console.WriteLine("-".PadRight(15, '-') + "-----");
            
            for (int i = 0; i < products.Length; i++)
            {
                // {0,-15} = left-align in 15-char width
                // {1,8:C2} = right-align currency in 8-char width
                string line = $"{products[i],-15} {prices[i],8:C2}";
                Console.WriteLine(line);
                // Output: Apple             $0.99
                // Output: Banana            $0.50
                // Output: Strawberry        $3.99
            }

            // ── REAL-WORLD EXAMPLE: Financial report ──────────────────
            decimal revenue = 1250000.50m;
            decimal expenses = 875000.25m;
            decimal profit = revenue - expenses;
            
            string report = $@"
=====================================
          FINANCIAL REPORT
=====================================
Revenue:    {revenue,15:C2}
Expenses:   {expenses,15:C2}
--------------------------------------
Profit:     {profit,15:C2}
Margin:     {(profit/revenue):P1}
=====================================";
            
            Console.WriteLine(report);
            // Output shows formatted financial data with alignment

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: Expressions in Interpolation
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Complex expressions inside {} ──────────────
            int a = 10, b = 3;
            
            // Arithmetic
            string math = $"10 + 3 = {a + b}, 10 - 3 = {a - b}, 10 * 3 = {a * b}";
            Console.WriteLine(math); // Output: 10 + 3 = 13, 10 - 3 = 7, 10 * 3 = 30

            // Ternary operator
            int score = 75;
            string grade = $"Score: {score} = {(score >= 60 ? "Pass" : "Fail")}";
            Console.WriteLine(grade); // Output: Score: 75 = Pass

            // Method calls
            string text = "  Hello World  ";
            string processed = $"Trimmed: '{text.Trim().ToUpper()}'";
            Console.WriteLine(processed); // Output: Trimmed: 'HELLO WORLD'

            // ── EXAMPLE 2: Null-conditional in interpolation ────────
            string maybeNull = null;
            
            // Without null-coalescing - displays empty
            string safe = $"Value: {maybeNull ?? "(none)"}";
            Console.WriteLine(safe); // Output: Value: (none)

            // Using ?.
            string display = $"Length: {(maybeNull?.Length ?? 0)}";
            Console.WriteLine(display); // Output: Length: 0

            // ── EXAMPLE 3: Collection and object access ───────────────
            var person = new { Name = "Alice", Age = 30 };
            Console.WriteLine($"Object: {person.Name} is {person.Age}");

            string[] cities = { "NYC", "LA", "Chicago" };
            string cityList = $"Cities: {string.Join(", ", cities)}";
            Console.WriteLine(cityList); // Output: Cities: NYC, LA, Chicago

            // Dictionary access
            var dict = new System.Collections.Generic.Dictionary<string, int>
            {
                {"Apple", 1}, {"Banana", 2}
            };
            string dictValue = $"Banana = {dict["Banana"]}";
            Console.WriteLine(dictValue); // Output: Banana = 2

            // ── REAL-WORLD EXAMPLE: Dynamic message construction ────
            var order = new 
            {
                Id = 12345,
                Customer = "John Smith",
                Total = 150.00m,
                Items = 3,
                Status = "Processing"
            };
            
            string orderMsg = $"Order #{order.Id} for {order.Customer}: " +
                             $"{order.Items} items totaling {order.Total:C} " +
                             $"status: {(order.Status == "Processing" ? "📦" : "✅")}";
            
            Console.WriteLine(orderMsg); 
            // Output: Order #12345 for John Smith: 3 items totaling $150.00 status: 📦

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: Raw String Literals (C# 11+)
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Raw string literals ─────────────────────────
            // """ allows multi-line without escape sequences
            string json = """
            {
                "name": "John",
                "age": 30,
                "city": "NYC"
            }
            """;
            Console.WriteLine(json);
            // Output: {"name": "John", "age": 30, "city": "NYC"}

            // Can use quotes directly
            string sql = """
            SELECT * FROM Users 
            WHERE Name = "John"
            """;
            Console.WriteLine(sql);

            // ── EXAMPLE 2: Interpolating raw strings ──────────────────
            // Combine $""" with interpolation
            string name = "Alice";
            int age = 30;
            
            string rawInterpolated = $"""
            {name} is {age} years old.
            In 5 years, {name} will be {age + 5}.
            """;
            Console.WriteLine(rawInterpolated);

            // ── REAL-WORLD EXAMPLE: HTML generation ────────────────────
            string pageTitle = "My Page";
            string userName = "John";
            
            string html = $"""
            <!DOCTYPE html>
            <html>
            <head>
                <title>{pageTitle}</title>
            </head>
            <body>
                <h1>Welcome, {userName}!</h1>
                <p>Current time: {DateTime.Now:yyyy-MM-dd HH:mm}</p>
            </body>
            </html>
            """;
            Console.WriteLine(html);

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: InterpolatedStringHandler (Advanced)
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Conditional formatting with custom handler ─
            // The $"" syntax actually uses InterpolatedStringHandler
            
            // Logging with conditional severity
            bool isDebug = true;
            string debugMessage = $"[DEBUG] Processing {name}"; // Compiled efficiently
            Console.WriteLine(debugMessage);

            // ── EXAMPLE 2: Performance consideration ──────────────────
            // Interpolation creates FormattedString, then ToString()
            // For heavy concatenation, use StringBuilder instead
            
            var builder = new System.Text.StringBuilder();
            for (int i = 0; i < 100; i++)
            {
                // Using string interpolation in loop - less efficient
                // For high performance, use StringBuilder.Append directly
                builder.Append($"{i:D3} "); // Pad to 3 digits
            }
            Console.WriteLine(builder.ToString());
            // Output: 000 001 002 ... 099

            // ═══════════════════════════════════════════════════════════
            // SECTION 6: Escape Sequences in Interpolation
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Escaping braces and special chars ──────────
            // {{ escaped to } in output
            string braces = $"Use {{ braces }} like this";
            Console.WriteLine(braces); // Output: Use { braces } like this

            // Backslash escaping
            string path = $@"C:\Users\{name}\Documents"; // @ keeps raw string
            Console.WriteLine(path); // Output: C:\Users\Alice\Documents

            // Newlines in interpolation
            string multiLine = $"Line 1\nLine 2";
            Console.WriteLine(multiLine);
            // Output: Line 1
            //         Line 2

            // ── EXAMPLE 2: Verbatim strings with interpolation ──────
            string folder = "docs";
            string file = "report";
            
            // Combine $ and @ for both features
            string fullPath = $@"C:\Users\{folder}\{file}.txt";
            Console.WriteLine(fullPath); // Output: C:\Users\docs\report.txt

            // ── REAL-WORLD EXAMPLE: File path construction ───────────
            string basePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string appFolder = "MyApp";
            string fileName = "data.json";
            
            string fullFilePath = $@"{basePath}\{appFolder}\{fileName}";
            Console.WriteLine(fullFilePath); // Output: C:\Users\...\Documents\MyApp\data.json

            Console.WriteLine("\n=== String Interpolation Complete ===");
        }
    }
}