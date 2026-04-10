/*
 * ============================================================
 * TOPIC     : C# Fundamentals
 * SUBTOPIC  : Data Types - Real World Applications
 * FILE      : DataTypes_RealWorld.cs
 * PURPOSE   : This file demonstrates practical, real-world uses of data types in production applications,
 *             including financial calculations, scientific computing, and data processing.
 * ============================================================
 */

// --- SECTION: Real-World Data Type Applications ---
// This file demonstrates how different data types are used in real-world scenarios

using System;
using System.IO;

namespace CSharp_MasterGuide._01_Fundamentals._02_DataTypes
{
    class DataTypes_RealWorld
    {
        static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════════
            // SECTION: Financial/Currency Applications
            // ═══════════════════════════════════════════════════════════════
            
            Console.WriteLine("=== Financial Calculations ===");
            
            // ── Why decimal for money ───────────────────────────────────────
            // Floating-point can't precisely represent many decimal values
            // This is critical for financial calculations
            
            // PROBLEM: Using double for money causes errors
            double doubleTotal = 0.0;
            for (int i = 0; i < 10; i++) doubleTotal += 0.1;
            Console.WriteLine($"Double 0.1*10: {doubleTotal}"); // Output: Double 0.1*10: 1 (not exactly!)
            
            // SOLUTION: Use decimal for exact representation
            decimal decimalTotal = 0.0m;
            for (int i = 0; i < 10; i++) decimalTotal += 0.1m;
            Console.WriteLine($"Decimal 0.1*10: {decimalTotal}"); // Output: Decimal 0.1*10: 1 (exact!)
            
            // ── Real-world financial calculation ───────────────────────────
            decimal itemPrice = 29.99m;
            int quantity = 100;
            decimal discountPercent = 0.15m; // 15% discount
            decimal taxRate = 0.08m; // 8% tax
            
            decimal subtotal = itemPrice * quantity;
            decimal discount = subtotal * discountPercent;
            decimal afterDiscount = subtotal - discount;
            decimal tax = afterDiscount * taxRate;
            decimal total = afterDiscount + tax;
            
            Console.WriteLine($"Item: ${itemPrice:F2} x {quantity}");
            Console.WriteLine($"Subtotal: ${subtotal:F2}");
            Console.WriteLine($"Discount (15%): -${discount:F2}");
            Console.WriteLine($"Tax (8%): +${tax:F2}");
            Console.WriteLine($"TOTAL: ${total:F2}");
            // Output shows precise decimal calculations

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Scientific/Engineering Applications
            // ═══════════════════════════════════════════════════════════════
            
            Console.WriteLine("\n=== Scientific Calculations ===");
            
            // ── Double for scientific calculations ─────────────────────────
            // Requires both large range AND reasonable precision
            double mass = 5.972e24; // Earth mass in kg
            double radius = 6.371e6; // Earth radius in meters
            double gravity = 6.674e-11; // Gravitational constant
            
            // Calculate gravitational acceleration at Earth's surface
            // g = GM/r²
            double g = (gravity * mass) / (radius * radius);
            Console.WriteLine($"Earth gravity: {g:F2} m/s²"); // Output: Earth gravity: 9.82 m/s²
            
            // ── Double for physics simulations ─────────────────────────────
            double initialVelocity = 100.0; // m/s
            double angle = 45.0; // degrees
            double angleRad = angle * Math.PI / 180.0; // Convert to radians
            double g2 = 9.81; // m/s²
            
            // Projectile range: R = v²sin(2θ)/g
            double range = Math.Pow(initialVelocity, 2) * Math.Sin(2 * angleRad) / g2;
            Console.WriteLine($"Projectile range at 45°: {range:F2} m"); // Output: Projectile range: 1020.41 m
            
            // ── When to use float vs double ───────────────────────────────
            // Float: graphics/games (slight precision loss acceptable)
            // Double: most scientific work (needs precision)
            // Decimal: financial/monetary (exact decimal representation)

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Database Integration
            // ═══════════════════════════════════════════════════════════════
            
            Console.WriteLine("\n=== Database Value Mapping ===");
            
            // Database types map to C# types
            // SQL int → int, SQL bigint → long, SQL decimal → decimal, SQL varchar → string
            
            // Simulating database record
            int? dbInt = null; // SQL int NULL
            long? dbBigInt = 123456789012345; // SQL bigint
            decimal? dbDecimal = 1234.56m; // SQL decimal
            bool? dbBit = null; // SQL bit NULL
            DateTime? dbDateTime = DateTime.Now; // SQL datetime
            
            // Null-coalescing for safe defaults
            int id = dbInt ?? 0;
            decimal price = dbDecimal ?? 0.0m;
            bool isActive = dbBit ?? false;
            
            Console.WriteLine($"DB Int: {id}");
            Console.WriteLine($"DB BigInt: {dbBigInt}");
            Console.WriteLine($"DB Decimal: {price:C}");
            Console.WriteLine($"DB Bit: {isActive}");
            Console.WriteLine($"DB DateTime: {dbDateTime:yyyy-MM-dd HH:mm:ss}");

            // ═══════════════════════════════════════════════════════════════
            // SECTION: File Size and Memory Calculations
            // ═══════════════════════════════════════════════════════════════
            
            Console.WriteLine("\n=== File Size Calculations ===");
            
            // Use appropriate types for file/memory sizes
            // byte for raw bytes, long for large files, double for display
            
            long fileSizeBytes = 1_500_000_000L; // 1.5 GB in bytes
            double fileSizeMB = fileSizeBytes / (1024.0 * 1024.0);
            double fileSizeGB = fileSizeBytes / (1024.0 * 1024.0 * 1024.0);
            
            Console.WriteLine($"Size in bytes: {fileSizeBytes:N0}");
            Console.WriteLine($"Size in MB: {fileSizeMB:F2}");
            Console.WriteLine($"Size in GB: {fileSizeGB:F2}");
            // Output shows formatted large numbers

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Bit Flags and Enums
            // ═══════════════════════════════════════════════════════════════
            
            Console.WriteLine("\n=== File Attributes (Flags) ===");
            
            // Combine enum flags with bitwise operations
            FileAttributes attrs = FileAttributes.Hidden | FileAttributes.ReadOnly | FileAttributes.Archive;
            Console.WriteLine($"Attributes: {attrs}");
            // Output: Attributes: Hidden, ReadOnly, Archive
            
            // Check individual flags
            bool isHidden = (attrs & FileAttributes.Hidden) != 0;
            bool isReadOnly = (attrs & FileAttributes.ReadOnly) != 0;
            bool isSystem = (attrs & FileAttributes.System) != 0;
            
            Console.WriteLine($"Is Hidden: {isHidden}");
            Console.WriteLine($"Is ReadOnly: {isReadOnly}");
            Console.WriteLine($"Is System: {isSystem}");
            // Output: True, True, False

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Unicode and Character Processing
            // ═══════════════════════════════════════════════════════════════
            
            Console.WriteLine("\n=== Unicode Processing ===");
            
            // Char handles full Unicode (not just ASCII)
            string greeting = "Hello 🌍"; // Emoji is valid in C# strings
            foreach (char c in greeting)
            {
                Console.WriteLine($"'{c}' = U+{(int)c:X4}");
            }
            // Output shows each character's Unicode code point
            
            // String operations handle Unicode properly
            string cyrillic = "Привет мир"; // Russian
            string chinese = "你好世界";     // Chinese
            string arabic = "مرحبا بالعالم"; // Arabic
            
            Console.WriteLine($"Russian: {cyrillic}");
            Console.WriteLine($"Chinese: {chinese}");
            Console.WriteLine($"Arabic: {arabic}");
            
            // Length - number of characters (code points), not bytes
            Console.WriteLine($"Byte length: {greeting.Length} chars, {System.Text.Encoding.UTF8.GetByteCount(greeting)} bytes");
            // Output: Byte length: 7 chars (including emoji), 13 bytes

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Boolean Logic in Real Applications
            // ═══════════════════════════════════════════════════════════════
            
            Console.WriteLine("\n=== Application Logic ===");
            
            // Complex boolean conditions for business logic
            bool isLoggedIn = true;
            bool hasSubscription = false;
            bool isAdmin = true;
            DateTime? subscriptionExpires = null;
            
            // Multi-condition authorization
            bool canAccessPremium = isLoggedIn && hasSubscription;
            bool canAccessAdmin = isLoggedIn && isAdmin;
            bool canRenew = isLoggedIn && subscriptionExpires < DateTime.Now;
            
            Console.WriteLine($"Can access premium: {canAccessPremium}");
            Console.WriteLine($"Can access admin: {canAccessAdmin}");
            Console.WriteLine($"Can renew: {canRenew}");
            // Output: False, True, False

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Performance-Critical Code
            // ═══════════════════════════════════════════════════════════════
            
            Console.WriteLine("\n=== Performance Considerations ===");
            
            // Use smallest type that fits to reduce memory and improve cache
            byte[] buffer = new byte[1024]; // 1KB buffer - byte is smallest
            
            // For large arrays, type choice affects memory significantly
            int[] smallNumbers = new int[1000]; // 4KB
            short[] smallerNumbers = new short[1000]; // 2KB (if values fit)
            
            Console.WriteLine($"int array: {smallNumbers.Length * 4} bytes");
            Console.WriteLine($"short array: {smallerNumbers.Length * 2} bytes");
            
            // Span<T> for high-performance processing
            // Demonstrates value type stack usage
            Span<int> stackSpan = stackalloc int[100]; // Allocated on stack
            for (int i = 0; i < stackSpan.Length; i++)
            {
                stackSpan[i] = i * 2;
            }
            Console.WriteLine($"Span sample: {stackSpan[50]}"); // Output: Span sample: 100
        }
    }
}
