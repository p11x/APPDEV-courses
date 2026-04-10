/*
 * ============================================================
 * TOPIC     : Exception Handling
 * SUBTOPIC  : Finally Block
 * FILE      : FinallyBlock.cs
 * PURPOSE   : Learn the finally block, when it executes,
 *            and cleanup patterns
 * ============================================================
 */

using System;

namespace CSharp_MasterGuide._05_ExceptionHandling._01_TryCatchFinally
{
    class FinallyBlock
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Finally Block in C# ===\n");

            // ═══════════════════════════════════════════════════════════
            // SECTION 1: Finally Always Executes
            // ═══════════════════════════════════════════════════════════

            // The finally block runs whether or not an exception occurs
            // It's guaranteed to execute even if there's a return statement

            // ── EXAMPLE 1: Finally After Successful Try ────────────────
            try
            {
                Console.WriteLine("  Try block: Starting operation");
                int result = 10 + 5;
                Console.WriteLine($"  Try block: Result = {result}");
            }
            finally
            {
                Console.WriteLine("  Finally block: Cleanup - This ALWAYS runs");
            }
            // Output: Try block: Starting operation
            // Output: Try block: Result = 15
            // Output: Finally block: Cleanup - This ALWAYS runs

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: Finally After Exception
            // ═══════════════════════════════════════════════════════════

            // Finally runs even when an exception is thrown

            // ── EXAMPLE 1: Finally After Exception ────────────────────
            try
            {
                Console.WriteLine($"\n  Try block: About to divide by zero");
                int x = 10 / 0;
            }
            catch (DivideByZeroException)
            {
                Console.WriteLine("  Catch block: Division by zero caught");
            }
            finally
            {
                Console.WriteLine("  Finally block: Cleanup - Still runs!");
            }
            // Output: Try block: About to divide by zero
            // Output: Catch block: Division by zero caught
            // Output: Finally block: Cleanup - Still runs!

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: Finally Without Catch
            // ═══════════════════════════════════════════════════════════

            // Finally can be used without catch (try-finally)

            // ── EXAMPLE 1: Try-Finally Without Catch ──────────────────
            try
            {
                Console.WriteLine($"\n  Try: Operation started");
                int[] arr = { 1, 2, 3 };
            }
            finally
            {
                Console.WriteLine("  Finally: Resource cleanup");
            }
            // Output: Try: Operation started
            // Output: Finally: Resource cleanup

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: Return in Try with Finally
            // ═══════════════════════════════════════════════════════════

            // Finally runs even when returning from try

            // ── EXAMPLE 1: Return Inside Try ──────────────────────────
            string result1 = GetMessage(true);
            Console.WriteLine($"\n  Return with true: {result1}");
            // Output: Try block: Returning value
            // Output: Finally: Cleanup runs before return

            string result2 = GetMessage(false);
            Console.WriteLine($"  Return with false: {result2}");
            // Output: Try block: Returning value
            // Output: Finally: Cleanup runs before return

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: Using Finally for Resource Cleanup
            // ═══════════════════════════════════════════════════════════

            // Common pattern: open resource in try, close in finally

            // ── EXAMPLE 1: Simulated Resource Cleanup ───────────────────
            var db = new DatabaseConnection();
            
            try
            {
                db.Connect();
                Console.WriteLine("\n  Connected to database");
                db.ExecuteQuery("SELECT * FROM Users");
            }
            finally
            {
                db.Disconnect();
                Console.WriteLine("  Database connection closed");
            }
            // Output: Connected to database
            // Output: Executing query: SELECT * FROM Users
            // Output: Database connection closed

            // ═══════════════════════════════════════════════════════════
            // SECTION 6: Real-World: File Processing
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Process File with Cleanup ───────────────────
            var fileHandler = new FileHandler();
            
            string content = fileHandler.ReadFile("data.txt");
            Console.WriteLine($"\n  File content: {content}");
            // Output: File content: Sample data

            Console.WriteLine("\n=== Finally Block Complete ===");
        }

        // ── DEMONSTRATION: Return with Finally ────────────────────────
        static string GetMessage(bool test)
        {
            try
            {
                Console.WriteLine("  Try block: Returning value");
                return test ? "Success" : "Failure";
            }
            finally
            {
                Console.WriteLine("  Finally: Cleanup runs before return");
            }
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Real-World: Database Connection Simulation
    // ═══════════════════════════════════════════════════════════

    class DatabaseConnection
    {
        private bool _isConnected = false;

        public void Connect()
        {
            Console.WriteLine("  Opening database connection...");
            _isConnected = true;
        }

        public void ExecuteQuery(string query)
        {
            if (!_isConnected)
            {
                throw new InvalidOperationException("Not connected to database");
            }
            Console.WriteLine($"  Executing query: {query}");
        }

        public void Disconnect()
        {
            if (_isConnected)
            {
                Console.WriteLine("  Closing database connection...");
                _isConnected = false;
            }
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Real-World: File Handler with Cleanup
    // ═══════════════════════════════════════════════════════════

    class FileHandler
    {
        private FileStream _stream = null;

        public string ReadFile(string fileName)
        {
            string content = null;
            
            try
            {
                // Simulate opening file
                Console.WriteLine($"  Opening file: {fileName}");
                _stream = new FileStream();
                
                // Simulate reading
                content = _stream.ReadAll();
                
                return content;
            }
            finally
            {
                // Always close the file
                if (_stream != null)
                {
                    Console.WriteLine($"  Closing file: {fileName}");
                    _stream.Close();
                }
            }
        }
    }

    // Simple FileStream simulation
    class FileStream
    {
        private bool _isOpen = false;

        public void Close()
        {
            _isOpen = false;
        }

        public string ReadAll()
        {
            _isOpen = true;
            return "Sample data";
        }
    }
}