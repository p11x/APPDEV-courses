/*
 * ============================================================
 * TOPIC     : C# Fundamentals
 * SUBTOPIC  : Operators - Null-Coalescing Operators
 * FILE      : NullCoalescing_Operators.cs
 * PURPOSE   : This file covers null-coalescing operators in C#: ?? and ??=.
 *             These operators provide safe null handling with default values.
 * ============================================================
 */

// --- SECTION: Null-Coalescing Operators ---
// Null-coalescing operators provide elegant null handling
// ?? returns left operand if not null, otherwise right operand
// ??= assigns right operand if left is null (C# 8.0+)

using System;

namespace CSharp_MasterGuide._01_Fundamentals._04_Operators
{
    class NullCoalescing_Operators
    {
        static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════════
            // SECTION: Null-Coalescing Operator (??)
            // ═══════════════════════════════════════════════════════════════
            
            // ── Basic usage ───────────────────────────────────────────────
            // Returns left operand if not null, otherwise returns right
            
            string? nullable = null;
            string result = nullable ?? "default value";
            Console.WriteLine($"null ?? \"default\": {result}"); // Output: default value
            
            nullable = "hello";
            result = nullable ?? "default value";
            Console.WriteLine($"\"hello\" ?? \"default\": {result}"); // Output: hello
            
            // ── With nullable value types ───────────────────────────────────
            int? maybeInt = null;
            int definite = maybeInt ?? 0;
            Console.WriteLine($"int? null ?? 0: {definite}"); // Output: 0
            
            maybeInt = 42;
            definite = maybeInt ?? 0;
            Console.WriteLine($"int? 42 ?? 0: {definite}"); // Output: 42
            
            // ── Chaining null-coalescing ───────────────────────────────────
            string? first = null;
            string? second = null;
            string? third = "value";
            
            string chained = first ?? second ?? third ?? "fallback";
            Console.WriteLine($"Chained: {chained}"); // Output: value
            
            first = "first";
            chained = first ?? second ?? third ?? "fallback";
            Console.WriteLine($"Chained with first: {chained}"); // Output: first

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Null-Coalescing Assignment (??=)
            // ═══════════════════════════════════════════════════════════════
            
            // C# 8.0+ - assigns right operand only if left is null
            
            string? optional = null;
            optional ??= "assigned"; // Assign because null
            Console.WriteLine($"After ??=: {optional}"); // Output: assigned
            
            optional = "already set";
            optional ??= "would not assign"; // No change
            Console.WriteLine($"Second ??=: {optional}"); // Output: already set
            
            // ── Practical use with configuration ───────────────────────────
            string? configTimeout = null;
            configTimeout ??= GetDefaultTimeout();
            Console.WriteLine($"Config timeout: {configTimeout}"); // Output: 30 (from method)
            
            // ═══════════════════════════════════════════════════════════════
            // SECTION: Real-World Examples
            // ═══════════════════════════════════════════════════════════════
            
            // ── Database values with null handling ─────────────────────────
            // Simulating database row where some fields might be NULL
            var user = new 
            {
                Name = "John",
                Phone = (string?)null, // Phone not provided
                Age = (int?)null       // Age not provided
            };
            
            // Use ?? for null-safe access
            string phone = user.Phone ?? "Not provided";
            int age = user.Age ?? 0;
            
            Console.WriteLine($"Phone: {phone}"); // Output: Phone: Not provided
            Console.WriteLine($"Age: {age}"); // Output: Age: 0
            
            // ── API optional parameters ─────────────────────────────────────
            // Many API parameters are optional - use null-coalescing for defaults
            var apiRequest = new 
            {
                Page = (int?)null,
                PageSize = (int?)null,
                SortBy = (string?)null
            };
            
            int page = apiRequest.Page ?? 1;
            int pageSize = apiRequest.PageSize ?? 10;
            string sortBy = apiRequest.SortBy ?? "created_at";
            
            Console.WriteLine($"Page: {page}, Size: {pageSize}, Sort: {sortBy}");
            // Output: Page: 1, Size: 10, Sort: created_at
            
            // ── Dictionary lookup ──────────────────────────────────────────
            var settings = new Dictionary<string, string?>
            {
                ["theme"] = "dark",
                ["language"] = null,
                ["timezone"] = "UTC"
            };
            
            string theme = settings["theme"] ?? "light";
            string language = settings["language"] ?? "en";
            string timezone = settings["timezone"] ?? "local";
            
            Console.WriteLine($"Theme: {theme}, Language: {language}, Timezone: {timezone}");
            // Output: Theme: dark, Language: en, Timezone: UTC
            
            // ═══════════════════════════════════════════════════════════════
            // SECTION: Null-Conditional with Null-Coalescing
            // ═══════════════════════════════════════════════════════════════
            
            // ── Null-conditional operator (?.) with ?? ───────────────────
            string? nullString = null;
            int? length = nullString?.Length; // Returns null (not exception)
            int definiteLength = length ?? -1;
            Console.WriteLine($"Length: {definiteLength}"); // Output: Length: -1
            
            nullString = "hello";
            length = nullString?.Length; // Returns 5
            definiteLength = length ?? -1;
            Console.WriteLine($"Length: {definiteLength}"); // Output: Length: 5
            
            // ── Chaining null checks ───────────────────────────────────────
            var nested = new 
            {
                User = new 
                {
                    Address = (Address?)null
                }
            };
            
            // Get city with null-safe access
            string city = nested?.User?.Address?.City ?? "Unknown";
            Console.WriteLine($"City: {city}"); // Output: City: Unknown
            
            // With actual address
            nested = new 
            {
                User = new 
                {
                    Address = new Address { City = "NYC" }
                }
            };
            city = nested?.User?.Address?.City ?? "Unknown";
            Console.WriteLine($"City: {city}"); // Output: City: NYC

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Expression with Null-Coalescing
            // ═══════════════════════════════════════════════════════════════
            
            // In method arguments
            PrintName(null); // Output: Guest
            PrintName("Alice"); // Output: Alice
            
            // In LINQ expressions (with Select)
            var names = new List<string?> { "Bob", null, "Charlie", null };
            var processedNames = names.Select(n => n ?? "(empty)").ToList();
            Console.WriteLine($"Processed: {string.Join(", ", processedNames)}");
            // Output: Processed: Bob, (empty), Charlie, (empty)
            
            // In conditional expression
            bool? permission = null;
            string access = permission == true ? "granted" : 
                           permission == false ? "denied" : 
                           "pending";
            Console.WriteLine($"Access: {access}"); // Output: pending
        }
        
        static string GetDefaultTimeout() => "30";
        
        static void PrintName(string? name)
        {
            Console.WriteLine($"Name: {name ?? "Guest"}");
        }
    }
    
    // Helper class for nested example
    class Address
    {
        public string City { get; set; } = "";
    }
}
