/*
 * ============================================================
 * TOPIC     : C# Fundamentals
 * SUBTOPIC  : Operators - Real World Applications
 * FILE      : Operators_RealWorld.cs
 * PURPOSE   : This file demonstrates practical, real-world uses of all operators in production applications.
 * ============================================================
 */

// --- SECTION: Real-World Operator Applications ---
// This file demonstrates operators in real production scenarios

using System;

namespace CSharp_MasterGuide._01_Fundamentals._04_Operators
{
    class Operators_RealWorld
    {
        static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════════
            // SECTION: Financial Calculations
            // ═══════════════════════════════════════════════════════════════
            
            Console.WriteLine("=== Financial Calculations ===");
            
            // ── Order total calculation ─────────────────────────────────────
            decimal itemPrice = 99.99m;
            int quantity = 5;
            decimal discountPercent = 0.15m;
            decimal taxRate = 0.08m;
            
            // Using compound operators
            decimal subtotal = itemPrice * quantity;
            decimal discount = subtotal * discountPercent;
            decimal afterDiscount = subtotal - discount;
            decimal tax = afterDiscount * taxRate;
            decimal total = afterDiscount + tax;
            
            Console.WriteLine($"Subtotal: {subtotal:C2}");
            Console.WriteLine($"Discount: -{discount:C2}");
            Console.WriteLine($"Tax: +{tax:C2}");
            Console.WriteLine($"Total: {total:C2}");
            
            // ── Percentage calculations ───────────────────────────────────
            decimal revenue = 150000m;
            decimal expenses = 85000m;
            decimal profit = revenue - expenses;
            decimal profitMargin = profit / revenue * 100;
            
            Console.WriteLine($"Profit margin: {profitMargin:F1}%");

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Validation and Logic
            // ═══════════════════════════════════════════════════════════════
            
            Console.WriteLine("\n=== Validation ===");
            
            // ── User registration validation ───────────────────────────────
            string? username = "john";
            string? email = "john@example.com";
            string? password = "secure123";
            
            // Using logical operators
            bool isValid = !string.IsNullOrWhiteSpace(username) &&
                           !string.IsNullOrWhiteSpace(email) &&
                           !string.IsNullOrWhiteSpace(password) &&
                           password.Length >= 8;
            
            Console.WriteLine($"Registration valid: {isValid}");
            
            // ── Access control ───────────────────────────────────────────
            bool isAuthenticated = true;
            bool isAuthorized = false;
            bool isAdmin = true;
            
            bool canAccess = isAuthenticated && (isAuthorized || isAdmin);
            Console.WriteLine($"Can access: {canAccess}");

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Bit Flags for Permissions
            // ═══════════════════════════════════════════════════════════════
            
            Console.WriteLine("\n=== Permission Flags ===");
            
            // Using bitwise operators for flags
            const int READ = 0b0001;
            const int WRITE = 0b0010;
            const int DELETE = 0b0100;
            const int ADMIN = 0b1000;
            
            // Grant permissions
            int userPermissions = READ | WRITE;
            Console.WriteLine($"User permissions: {userPermissions} (binary: {Convert.ToString(userPermissions, 2)})");
            
            // Check permissions
            bool canRead = (userPermissions & READ) != 0;
            bool canWrite = (userPermissions & WRITE) != 0;
            bool canDelete = (userPermissions & DELETE) != 0;
            
            Console.WriteLine($"Can: Read={canRead}, Write={canWrite}, Delete={canDelete}");
            
            // Add permission
            userPermissions |= DELETE;
            Console.WriteLine($"After adding DELETE: {userPermissions}");
            
            // Remove permission
            userPermissions &= ~WRITE;
            Console.WriteLine($"After removing WRITE: {userPermissions}");

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Data Processing
            // ═══════════════════════════════════════════════════════════════
            
            Console.WriteLine("\n=== Data Processing ===");
            
            // ── Pagination calculation ───────────────────────────────────
            int totalRecords = 143;
            int pageSize = 10;
            int currentPage = 5;
            
            // Calculate offset for database query
            int offset = (currentPage - 1) * pageSize;
            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            
            Console.WriteLine($"Page {currentPage} of {totalPages}");
            Console.WriteLine($"Records {offset + 1}-{Math.Min(offset + pageSize, totalRecords)}");
            
            // ── Even/Odd detection using bitwise ────────────────────────
            int[] numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            
            foreach (int num in numbers)
            {
                bool isEven = (num & 1) == 0;
                Console.WriteLine($"{num}: {(isEven ? "Even" : "Odd")}");
            }
            
            // ── Finding power of 2 ───────────────────────────────────────
            // Using bitwise for fast operations
            int value = 256;
            bool isPowerOf2 = (value & (value - 1)) == 0;
            Console.WriteLine($"{value} is power of 2: {isPowerOf2}");
            
            // Next power of 2
            int nextPow2 = 1;
            while (nextPow2 < value)
            {
                nextPow2 <<= 1;
            }
            Console.WriteLine($"Next power of 2: {nextPow2}");

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Null Handling
            // ═══════════════════════════════════════════════════════════════
            
            Console.WriteLine("\n=== Null Handling ===");
            
            // ── Configuration with defaults ───────────────────────────────
            string? dbHost = null;
            int? dbPort = null;
            
            string connectionString = $"{dbHost ?? "localhost"}:{dbPort ?? 5432}";
            Console.WriteLine($"Connection: {connectionString}");
            
            // ── Null-coalescing with objects ──────────────────────────────
            var user = new { Name = (string?)null, Score = (int?)null };
            
            string displayName = user.Name ?? "Anonymous";
            int score = user.Score ?? 0;
            
            Console.WriteLine($"Display: {displayName}, Score: {score}");

            // ═══════════════════════════════════════════════════════════════
            // SECTION: String Building
            // ═══════════════════════════════════════════════════════════════
            
            Console.WriteLine("\n=== String Building ===");
            
            // Using += for string concatenation
            string path = "";
            string[] parts = { "home", "users", "john", "documents" };
            
            foreach (string part in parts)
            {
                path += "/" + part;
            }
            Console.WriteLine($"Path: {path}");
            
            // Using arithmetic with string interpolation
            string template = "User {0} has {1} points";
            string message = string.Format(template, "Alice", 1500);
            Console.WriteLine($"Message: {message}");

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Date and Time Calculations
            // ═══════════════════════════════════════════════════════════════
            
            Console.WriteLine("\n=== Date Calculations ===");
            
            DateTime startDate = DateTime.Now;
            DateTime endDate = startDate.AddDays(30);
            
            // Days until deadline
            TimeSpan remaining = endDate - startDate;
            int daysLeft = remaining.Days;
            Console.WriteLine($"Days until deadline: {daysLeft}");
            
            // Unix timestamp
            DateTime unixEpoch = new DateTime(1970, 1, 1);
            long timestamp = (long)(DateTime.Now - unixEpoch).TotalSeconds;
            Console.WriteLine($"Unix timestamp: {timestamp}");

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Comparison in Sorting
            // ═══════════════════════════════════════════════════════════════
            
            Console.WriteLine("\n=== Sorting ===");
            
            int[] unsorted = { 5, 2, 8, 1, 9, 3 };
            
            // Simple bubble sort (educational - use Array.Sort in production)
            int[] sorted = (int[])unsorted.Clone();
            for (int i = 0; i < sorted.Length - 1; i++)
            {
                for (int j = 0; j < sorted.Length - i - 1; j++)
                {
                    if (sorted[j] > sorted[j + 1]) // Comparison
                    {
                        // Swap
                        int temp = sorted[j];
                        sorted[j] = sorted[j + 1];
                        sorted[j + 1] = temp;
                    }
                }
            }
            
            Console.WriteLine($"Sorted: {string.Join(", ", sorted)}");
        }
    }
}
