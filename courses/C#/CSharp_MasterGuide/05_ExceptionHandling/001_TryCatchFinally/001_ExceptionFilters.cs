/*
 * ============================================================
 * TOPIC     : Exception Handling
 * SUBTOPIC  : Exception Filters
 * FILE      : ExceptionFilters.cs
 * PURPOSE   : Learn to use 'when' clause in catch blocks
 *            for conditional exception handling
 * ============================================================
 */

using System;

namespace CSharp_MasterGuide._05_ExceptionHandling._01_TryCatchFinally
{
    class ExceptionFilters
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Exception Filters in C# ===\n");

            // ═══════════════════════════════════════════════════════════
            // SECTION 1: Basic Exception Filters
            // ═══════════════════════════════════════════════════════════

            // Use 'when' clause to filter exceptions by condition
            // The catch block only executes if the condition is true

            // ── EXAMPLE 1: Filter by Exception Property ────────────────
            try
            {
                throw new ArgumentException("Value must be positive", "amount");
            }
            catch (ArgumentException ex) when (ex.ParamName == "amount")
            {
                Console.WriteLine($"  Caught amount exception: {ex.Message}");
            }
            // Output: Caught amount exception: Value must be positive

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: Multiple Filters
            // ═══════════════════════════════════════════════════════════

            // Different catch blocks with different conditions

            // ── EXAMPLE 1: Multiple When Conditions ───────────────────
            try
            {
                throw new ArgumentException("Invalid age", "age");
            }
            catch (ArgumentException ex) when (ex.ParamName == "age")
            {
                Console.WriteLine($"\n  Caught age exception: {ex.Message}");
            }
            catch (ArgumentException ex) when (ex.ParamName == "amount")
            {
                Console.WriteLine($"\n  Caught amount exception: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"\n  Caught other argument exception: {ex.Message}");
            }
            // Output: Caught age exception: Invalid age

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: Filter Based on Exception Message
            // ═══════════════════════════════════════════════════════════

            // Check exception message content

            // ── EXAMPLE 1: Filter by Message Content ────────────────
            try
            {
                throw new InvalidOperationException("Database connection failed");
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("Database"))
            {
                Console.WriteLine($"\n  Database error: {ex.Message}");
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("network"))
            {
                Console.WriteLine($"\n  Network error: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"\n  General operation error: {ex.Message}");
            }
            // Output: Database error: Database connection failed

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: Filter with Custom Logic
            // ═══════════════════════════════════════════════════════════

            // Complex filter conditions using external methods

            // ── EXAMPLE 1: Custom Filter Condition ─────────────────────
            var data = new ExceptionData();
            
            try
            {
                throw new CustomException("Test error", data);
            }
            catch (CustomException ex) when (ex.Data.ShouldHandle)
            {
                Console.WriteLine($"\n  Handling critical exception: {ex.Message}");
            }
            catch (CustomException ex)
            {
                Console.WriteLine($"\n  Skipping non-critical exception: {ex.Message}");
            }
            // Output: Handling critical exception: Test error

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: Filter Without Matching (None Execute)
            // ═══════════════════════════════════════════════════════════

            // If no filter matches, exception propagates

            // ── EXAMPLE 1: No Matching Filter ──────────────────────────
            try
            {
                throw new ArgumentException("Bad value", "other");
            }
            catch (ArgumentException ex) when (ex.ParamName == "amount")
            {
                Console.WriteLine($"\n  Amount error: {ex.Message}");
            }
            catch (ArgumentException ex) when (ex.ParamName == "age")
            {
                Console.WriteLine($"\n  Age error: {ex.Message}");
            }
            // No catch matched - exception will propagate
            // But here we'll handle it for demonstration
            catch (Exception ex)
            {
                Console.WriteLine($"\n  No filter matched, caught general: {ex.Message}");
            }
            // Output: No filter matched, caught general: Bad value

            // ═══════════════════════════════════════════════════════════
            // SECTION 6: Real-World: Environment-Based Handling
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Environment-Specific Error Handling ─────
            var handler = new ErrorHandler();
            
            Console.WriteLine($"\n  Testing Development environment...");
            handler.HandleError("Database error", EnvironmentType.Development);
            // Output: Testing Development environment...
            // Output: DEV: Database error (full details shown)

            Console.WriteLine($"  Testing Production environment...");
            handler.HandleError("Database error", EnvironmentType.Production);
            // Output: Testing Production environment...
            // Output: PROD: An error occurred (details hidden for security)

            Console.WriteLine("\n=== Exception Filters Complete ===");
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Custom Exception with Data
    // ═══════════════════════════════════════════════════════════

    class CustomException : Exception
    {
        public ExceptionData Data { get; }
        
        public CustomException(string message, ExceptionData data) : base(message)
        {
            this.Data = data;
        }
    }

    class ExceptionData
    {
        public bool ShouldHandle { get; set; } = true;
    }

    // ═══════════════════════════════════════════════════════════
    // Environment Types
    // ═══════════════════════════════════════════════════════════

    enum EnvironmentType
    {
        Development,
        Staging,
        Production
    }

    // ═══════════════════════════════════════════════════════════
    // Real-World: Environment-Based Error Handler
    // ═══════════════════════════════════════════════════════════

    class ErrorHandler
    {
        public void HandleError(string errorMessage, EnvironmentType environment)
        {
            try
            {
                throw new InvalidOperationException(errorMessage);
            }
            catch (InvalidOperationException ex) when (environment == EnvironmentType.Development)
            {
                Console.WriteLine($"  DEV: {ex.Message} (full details shown)");
            }
            catch (InvalidOperationException ex) when (environment == EnvironmentType.Staging)
            {
                Console.WriteLine($"  STAGING: {ex.Message} (some details hidden)");
            }
            catch (InvalidOperationException ex) when (environment == EnvironmentType.Production)
            {
                Console.WriteLine($"  PROD: An error occurred (details hidden for security)");
                // Log error internally without exposing to user
            }
        }
    }
}