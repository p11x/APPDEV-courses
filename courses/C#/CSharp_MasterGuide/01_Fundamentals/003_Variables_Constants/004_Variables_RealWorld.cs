/*
 * ============================================================
 * TOPIC     : C# Fundamentals
 * SUBTOPIC  : Variables and Constants - Variables Real World
 * FILE      : Variables_RealWorld.cs
 * PURPOSE   : This file demonstrates practical, real-world uses of variables in production applications,
 *             including configuration, state management, and data processing.
 * ============================================================
 */

// --- SECTION: Real-World Variable Applications ---
// This file demonstrates how variables are used in real production scenarios

using System;
using System.IO;

namespace CSharp_MasterGuide._01_Fundamentals._03_Variables_Constants
{
    class Variables_RealWorld
    {
        static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════════
            // SECTION: Configuration Variables
            // ═══════════════════════════════════════════════════════════════
            
            Console.WriteLine("=== Configuration Variables ===");
            
            // ── Application settings as variables ──────────────────────────
            string appName = "InventorySystem"; // Application identifier
            int maxRetries = 3; // Retry logic attempts
            int connectionTimeout = 30; // Seconds
            bool enableLogging = true;
            string logLevel = "Warning"; // Debug, Info, Warning, Error
            
            Console.WriteLine($"App: {appName}, Timeout: {connectionTimeout}s");
            
            // ── Environment-based configuration ───────────────────────────
            // Variables adjust based on environment
            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") 
                ?? "Development";
            
            // Configuration based on environment
            if (environment == "Production")
            {
                connectionTimeout = 60;
                enableLogging = true;
                logLevel = "Error";
            }
            
            Console.WriteLine($"Environment: {environment}");
            Console.WriteLine($"Configured timeout: {connectionTimeout}");

            // ═══════════════════════════════════════════════════════════════
            // SECTION: State Management
            // ═══════════════════════════════════════════════════════════════
            
            Console.WriteLine("\n=== State Management ===");
            
            // ── User session state ─────────────────────────────────────────
            bool isLoggedIn = false; // Initial state
            string? currentUser = null;
            DateTime? loginTime = null;
            
            // Simulate login
            isLoggedIn = true;
            currentUser = "john@example.com";
            loginTime = DateTime.Now;
            
            Console.WriteLine($"Logged in: {isLoggedIn}");
            Console.WriteLine($"User: {currentUser}");
            Console.WriteLine($"Login time: {loginTime:yyyy-MM-dd HH:mm:ss}");
            
            // ── Application state (singleton pattern) ──────────────────────
            var appState = ApplicationState.Instance;
            appState.RequestCount++;
            appState.StartTime = DateTime.Now;
            
            Console.WriteLine($"Requests: {appState.RequestCount}");

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Data Processing Variables
            // ═══════════════════════════════════════════════════════════════
            
            Console.WriteLine("\n=== Data Processing ===");
            
            // ── Input validation state ─────────────────────────────────────
            bool isValid = true;
            string errorMessage = "";
            
            string? username = "john"; // Simulated input
            string? email = "invalid-email"; // Invalid email
            
            // Validate username
            if (string.IsNullOrWhiteSpace(username))
            {
                isValid = false;
                errorMessage = "Username is required";
            }
            else if (username.Length < 3)
            {
                isValid = false;
                errorMessage = "Username must be at least 3 characters";
            }
            
            // Validate email (simple check)
            if (!email?.Contains("@") ?? true)
            {
                isValid = false;
                errorMessage = "Invalid email format";
            }
            
            Console.WriteLine($"Valid: {isValid}");
            if (!isValid) Console.WriteLine($"Error: {errorMessage}");

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Calculation Variables
            // ═══════════════════════════════════════════════════════════════
            
            Console.WriteLine("\n=== Calculations ===");
            
            // ── Financial calculations ─────────────────────────────────────
            decimal[] prices = { 100.00m, 50.00m, 75.00m, 25.00m };
            decimal subtotal = 0;
            
            foreach (var price in prices)
            {
                subtotal += price;
            }
            
            decimal taxRate = 0.08m;
            decimal discountPercent = 0.10m;
            decimal discount = subtotal * discountPercent;
            decimal afterDiscount = subtotal - discount;
            decimal tax = afterDiscount * taxRate;
            decimal total = afterDiscount + tax;
            
            Console.WriteLine($"Subtotal: {subtotal:C2}");
            Console.WriteLine($"Discount (10%): -{discount:C2}");
            Console.WriteLine($"Tax (8%): +{tax:C2}");
            Console.WriteLine($"Total: {total:C2}");

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Loop Variables
            // ═══════════════════════════════════════════════════════════════
            
            Console.WriteLine("\n=== Loop Processing ===");
            
            // ── Processing items with index ────────────────────────────────
            string[] items = { "Apple", "Banana", "Cherry", "Date" };
            
            for (int i = 0; i < items.Length; i++)
            {
                string item = items[i]; // Current item
                bool isFirst = (i == 0); // First item flag
                bool isLast = (i == items.Length - 1); // Last item flag
                
                Console.WriteLine($"[{i}] {item} (First: {isFirst}, Last: {isLast})");
            }
            
            // ── Aggregation with variables ─────────────────────────────────
            int count = 0;
            decimal sum = 0;
            decimal min = decimal.MaxValue;
            decimal max = decimal.MinValue;
            
            decimal[] sales = { 100m, 250m, 75m, 300m, 50m };
            
            foreach (var sale in sales)
            {
                count++;
                sum += sale;
                if (sale < min) min = sale;
                if (sale > max) max = sale;
            }
            
            decimal average = sum / count;
            
            Console.WriteLine($"Count: {count}");
            Console.WriteLine($"Sum: {sum:C2}");
            Console.WriteLine($"Average: {average:C2}");
            Console.WriteLine($"Min: {min:C2}");
            Console.WriteLine($"Max: {max:C2}");

            // ═══════════════════════════════════════════════════════════════
            // SECTION: File Processing Variables
            // ═══════════════════════════════════════════════════════════════
            
            Console.WriteLine("\n=== File Processing ===");
            
            // Variables for file operations
            string? filePath = "sample.txt"; // Nullable for optional file
            
            if (File.Exists(filePath))
            {
                // Read file into memory
                string content = File.ReadAllText(filePath);
                
                // Calculate statistics
                int charCount = content.Length;
                int wordCount = content.Split(' ').Length;
                int lineCount = content.Split('\n').Length;
                
                Console.WriteLine($"File: {filePath}");
                Console.WriteLine($"Characters: {charCount}");
                Console.WriteLine($"Words: {wordCount}");
                Console.WriteLine($"Lines: {lineCount}");
            }
            
            // ── Working with paths ─────────────────────────────────────────
            string fullPath = @"C:\Users\John\Documents\report.pdf";
            string? directory = Path.GetDirectoryName(fullPath);
            string fileName = Path.GetFileName(fullPath);
            string extension = Path.GetExtension(fullPath);
            string nameWithoutExt = Path.GetFileNameWithoutExtension(fullPath);
            
            Console.WriteLine($"Directory: {directory}");
            Console.WriteLine($"Filename: {fileName}");
            Console.WriteLine($"Extension: {extension}");
            Console.WriteLine($"Name: {nameWithoutExt}");

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Enum and Flag Variables
            // ═══════════════════════════════════════════════════════════════
            
            Console.WriteLine("\n=== Enums and Flags ===");
            
            // ── Enum for state management ───────────────────────────────────
            OrderStatus status = OrderStatus.Processing;
            
            switch (status)
            {
                case OrderStatus.Pending:
                    Console.WriteLine("Order is pending");
                    break;
                case OrderStatus.Processing:
                    Console.WriteLine("Order is being processed"); // Output
                    break;
                case OrderStatus.Shipped:
                    Console.WriteLine("Order has been shipped");
                    break;
                case OrderStatus.Delivered:
                    Console.WriteLine("Order delivered");
                    break;
            }
            
            // ── Flags for permissions ──────────────────────────────────────
            UserPermissions permissions = UserPermissions.Read;
            permissions |= UserPermissions.Write; // Add write
            permissions |= UserPermissions.Delete; // Add delete
            
            bool canRead = (permissions & UserPermissions.Read) != 0;
            bool canWrite = (permissions & UserPermissions.Write) != 0;
            bool canDelete = (permissions & UserPermissions.Delete) != 0;
            
            Console.WriteLine($"Can Read: {canRead}, Write: {canWrite}, Delete: {canDelete}");
            // Output: Can Read: True, Write: True, Delete: True

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Caching and Memoization Variables
            // ═══════════════════════════════════════════════════════════════
            
            Console.WriteLine("\n=== Caching ===");
            
            // Simple cache using dictionary
            var cache = new Dictionary<string, object>();
            
            // Check cache before expensive operation
            string cacheKey = "user_123";
            
            if (cache.TryGetValue(cacheKey, out object? cachedValue))
            {
                Console.WriteLine($"Cache hit: {cachedValue}");
            }
            else
            {
                // Simulate expensive operation
                object result = new { Name = "John", Age = 30 };
                cache[cacheKey] = result;
                Console.WriteLine($"Cache miss - stored: {result}");
            }
        }
    }
    
    // ═══════════════════════════════════════════════════════════════════════
    // Supporting classes and enums
    // ═══════════════════════════════════════════════════════════════════════
    
    // Singleton application state
    class ApplicationState
    {
        private static ApplicationState? _instance;
        public static ApplicationState Instance => _instance ??= new ApplicationState();
        
        private ApplicationState() { }
        
        public int RequestCount { get; set; }
        public DateTime StartTime { get; set; }
        public string? CurrentUser { get; set; }
    }
    
    // Order status enum
    enum OrderStatus
    {
        Pending,
        Processing,
        Shipped,
        Delivered,
        Cancelled
    }
    
    // Flag enum for permissions
    [Flags]
    enum UserPermissions
    {
        None = 0,
        Read = 1,
        Write = 2,
        Delete = 4,
        Admin = 8
    }
}
