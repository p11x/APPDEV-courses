/*
 * ============================================================
 * TOPIC     : Exception Handling
 * SUBTOPIC  : Multiple Catch Blocks
 * FILE      : MultipleCatch.cs
 * PURPOSE   : Learn to handle different exception types
 *            with multiple catch blocks
 * ============================================================
 */

using System;

namespace CSharp_MasterGuide._05_ExceptionHandling._01_TryCatchFinally
{
    class MultipleCatch
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Multiple Catch Blocks in C# ===\n");

            // ═══════════════════════════════════════════════════════════
            // SECTION 1: Catching Different Exception Types
            // ═══════════════════════════════════════════════════════════

            // Use multiple catch blocks to handle different exceptions differently

            // ── EXAMPLE 1: Handle FormatException ────────────────────────
            try
            {
                string input = "not a number";
                int result = int.Parse(input);
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"  FormatException: '{input}' is not a valid number");
                Console.WriteLine($"  Message: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"  ArgumentException: {ex.Message}");
            }
            // Output: FormatException: 'not a number' is not a valid number
            // Output: Message: Input string was not in a correct format.

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: Multiple Specific Catches
            // ═══════════════════════════════════════════════════════════

            // Catch specific exceptions first, then general ones

            // ── EXAMPLE 1: Array Access Exceptions ────────────────────────
            try
            {
                int[] numbers = { 1, 2, 3 };
                int index = -1;
                int value = numbers[index];
            }
            catch (IndexOutOfRangeException ex)
            {
                Console.WriteLine($"\n  IndexOutOfRangeException: Index {ex.Message}");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($"\n  ArgumentOutOfRangeException: Index {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"\n  ArgumentException: {ex.Message}");
            }
            // Output: ArgumentOutOfRangeException: Index was outside the bounds of the array.

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: Argument Exceptions
            // ═════��═════════════════════════════════════════════════════

            // Handle various argument-related exceptions

            // ── EXAMPLE 1: ArgumentNullException ─────────────────────
            try
            {
                string name = null;
                Console.WriteLine(name.Length);
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"\n  ArgumentNullException: {ex.ParamName} is null");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"\n  ArgumentException: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n  General Exception: {ex.Message}");
            }
            // Output: ArgumentNullException: Value cannot be null.

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: InvalidOperationException
            // ═══════════════════════════════════════════════════════════

            // Handle state-related exceptions

            // ── EXAMPLE 1: InvalidOperationException ─────────────────
            try
            {
                var parser = new JsonParser();
                parser.Parse("invalid json");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"\n  InvalidOperationException: {ex.Message}");
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"\n  FormatException: {ex.Message}");
            }
            // Output: InvalidOperationException: Invalid JSON structure

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: Catch Order Demonstration
            // ═══════════════════════════════════════════════════════════

            // Shows why order matters

            // ── EXAMPLE 1: Inheritance Hierarchy ──────────────────────
            DemonstrateCatchOrder();
            // ArgumentException is base class for many specific exceptions
            // So it must come AFTER specific exception types

            // ═══════════════════════════════════════════════════════════
            // SECTION 6: Real-World: API Error Handling
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Handle Various API Errors ──────────────────
            var apiClient = new ApiClient();
            
            Console.WriteLine($"\n  Testing GET /users...");
            apiClient.MakeRequest("GET", "/users");
            // Output: Testing GET /users...
            // Output: Success: 200 OK
            
            Console.WriteLine($"  Testing GET /users/999...");
            apiClient.MakeRequest("GET", "/users/999");
            // Output: Testing GET /users/999...
            // Output: NotFoundException: User 999 not found
            
            Console.WriteLine($"  Testing POST /users with invalid data...");
            apiClient.MakeRequest("POST", "/users");
            // Output: Testing POST /users with invalid data...
            // Output: ArgumentException: Request data is invalid
            
            Console.WriteLine($"  Testing unauthorized...");
            apiClient.MakeRequest("DELETE", "/admin");
            // Output: Testing unauthorized...
            // Output: UnauthorizedAccessException: Access denied

            Console.WriteLine("\n=== Multiple Catch Blocks Complete ===");
        }

        // ── DEMONSTRATION: Catch Order with Inheritance ─────────────────
        static void DemonstrateCatchOrder()
        {
            try
            {
                throw new ArgumentNullException("test");
            }
            catch (Exception ex)
            {
                // This catches ALL exceptions (most general)
                Console.WriteLine($"\n  Caught by general catch: {ex.GetType().Name}");
            }
            // If we tried to catch ArgumentException first after catching Exception,
            // it would be unreachable code and cause compiler error
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Real-World: JSON Parser
    // ═══════════════════════════════════════════════════════════

    class JsonParser
    {
        public void Parse(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                throw new ArgumentException("JSON cannot be empty", "json");
            }
            
            if (!json.StartsWith("{") || !json.EndsWith("}"))
            {
                throw new InvalidOperationException("Invalid JSON structure");
            }
            
            // Normal parsing would happen here...
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Real-World: API Client
    // ═══════════════════════════════════════════════════════════

    class ApiClient
    {
        private int _statusCode = 200;

        public void MakeRequest(string method, string endpoint)
        {
            try
            {
                // Simulate request
                if (endpoint == "/users/999")
                {
                    throw new KeyNotFoundException("User 999 not found");
                }
                
                if (method == "POST" && endpoint == "/users")
                {
                    throw new ArgumentException("Request data is invalid", "data");
                }
                
                if (method == "DELETE" && endpoint == "/admin")
                {
                    throw new UnauthorizedAccessException("Access denied");
                }
                
                Console.WriteLine($"  Success: {_statusCode} OK");
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine($"  NotFoundException: {ex.Message}");
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"  UnauthorizedAccessException: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"  ArgumentException: {ex.Message}");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"  HttpRequestException: {ex.Message}");
            }
        }
    }

    // Additional exception types for demonstration
    class KeyNotFoundException : Exception
    {
        public KeyNotFoundException(string message) : base(message) { }
    }

    class HttpRequestException : Exception
    {
        public HttpRequestException(string message) : base(message) { }
    }
}