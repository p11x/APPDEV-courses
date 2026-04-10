/*
 * ============================================================
 * TOPIC     : Exception Handling
 * SUBTOPIC  : Try-Catch Basics (Part 2)
 * FILE      : TryCatchBasics_Part2.cs
 * PURPOSE   : Learn more try-catch patterns, catch order rules,
 *            and rethrowing exceptions
 * ============================================================
 */

using System;

namespace CSharp_MasterGuide._05_ExceptionHandling._01_TryCatchFinally
{
    class TryCatchBasics_Part2
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Try-Catch Basics Part 2 ===\n");

            // ═══════════════════════════════════════════════════════════
            // SECTION 1: Catch Block Order Matters
            // ═══════════════════════════════════════════════════════════

            // More specific exceptions MUST come before more general ones
            // Exception is the base class for all exceptions

            // ── EXAMPLE 1: Correct Catch Order ────────────────────────────
            object obj = "Hello";
            try
            {
                int num = (int)obj; // This throws InvalidCastException
            }
            catch (InvalidCastException ex)
            {
                Console.WriteLine($"  InvalidCastException: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  General Exception: {ex.Message}");
            }
            // Output: InvalidCastException: Unable to cast object of type 'System.String' to type 'System.Int32'.

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: Wrong Catch Order (Shows Why Order Matters)
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Demonstrating Catch Order ────────────────────
            DemonstrateCatchOrder();
            // If Exception catch came first, it would catch everything
            // and InvalidCastException would never be handled specifically

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: Rethrowing Exceptions
            // ═══════════════════════════════════════════════════════════

            // Rethrow an exception to let it propagate to the caller

            // ── EXAMPLE 1: Rethrow Exception ────────────────────────────
            try
            {
                ProcessData("invalid");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"\n  Outer catch - ArgumentException: {ex.Message}");
                Console.WriteLine($"  Stack trace: {ex.StackTrace}");
            }
            // Output: Outer catch - ArgumentException: Data is invalid
            // Output: (shows full stack trace from ProcessData and ProcessInner)

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: Catch Without Variable
            // ═══════════════════════════════════════════════════════════

            // You can catch without storing the exception

            // ── EXAMPLE 1: Catch Without Variable ────────────────────────
            try
            {
                int x = 5 / 0;
            }
            catch (DivideByZeroException)
            {
                Console.WriteLine($"\n  Caught DivideByZeroException (no variable)");
            }
            // Output: Caught DivideByZeroException (no variable)

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: Nested Try-Catch
            // ═══════════════════════════════════════════════════════════

            // Try blocks can be nested

            // ── EXAMPLE 1: Nested Try-Catch ────────────────────────────
            try
            {
                try
                {
                    int[] arr = { 1, 2, 3 };
                    Console.WriteLine(arr[5]);
                }
                catch (IndexOutOfRangeException)
                {
                    Console.WriteLine($"\n  Inner catch: Index out of range");
                    throw new ArgumentException("Processed array error");
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"  Outer catch: {ex.Message}");
            }
            // Output: Inner catch: Index out of range
            // Output: Outer catch: Processed array error

            // ═══════════════════════════════════════════════════════════
            // SECTION 6: Real-World: Order Processing System
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Process Customer Order ──────────────────────
            var orderProcessor = new OrderProcessor();
            
            var validOrder = new Order 
            { 
                CustomerId = 1, 
                ProductId = 100, 
                Quantity = 5 
            };
            
            var invalidOrder = new Order 
            { 
                CustomerId = -1, // Invalid
                ProductId = 100, 
                Quantity = 5 
            };
            
            Console.WriteLine($"\n  Processing valid order...");
            orderProcessor.ProcessOrder(validOrder);
            // Output: Processing valid order...
            // Output: Order processed for Customer 1

            Console.WriteLine($"  Processing invalid order...");
            orderProcessor.ProcessOrder(invalidOrder);
            // Output: Processing invalid order...
            // Output: ArgumentException: CustomerId must be positive

            Console.WriteLine("\n=== Try-Catch Basics Part 2 Complete ===");
        }

        // ── DEMONSTRATION: Why Catch Order Matters ─────────────────────
        static void DemonstrateCatchOrder()
        {
            object data = 42;
            
            // This demonstrates catching from most specific to least specific
            try
            {
                string s = (string)data;
            }
            catch (Exception ex)
            {
                // This catches all exceptions since it's the most general
                Console.WriteLine($"  Caught: {ex.GetType().Name}");
            }
        }

        // ── INNER METHOD: Demonstrates Rethrowing ──────────────────
        static void ProcessData(string input)
        {
            try
            {
                ProcessInner(input);
            }
            catch (ArgumentException)
            {
                throw; // Rethrow the original exception
            }
        }

        static void ProcessInner(string input)
        {
            if (input == "invalid")
            {
                throw new ArgumentException("Data is invalid");
            }
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Real-World: Order Processor
    // ═══════════════════════════════════════════════════════════

    class OrderProcessor
    {
        public void ProcessOrder(Order order)
        {
            try
            {
                ValidateOrder(order);
                
                // Process the order
                Console.WriteLine($"  Order processed for Customer {order.CustomerId}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"  {ex.GetType().Name}: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"  {ex.GetType().Name}: {ex.Message}");
            }
        }

        private void ValidateOrder(Order order)
        {
            // Validate customer ID - throws ArgumentException
            if (order.CustomerId <= 0)
            {
                throw new ArgumentException("CustomerId must be positive", "CustomerId");
            }
            
            // Validate product ID - throws ArgumentException
            if (order.ProductId <= 0)
            {
                throw new ArgumentException("ProductId must be positive", "ProductId");
            }
            
            // Validate quantity - throws InvalidOperationException
            if (order.Quantity <= 0 || order.Quantity > 100)
            {
                throw new InvalidOperationException("Quantity must be between 1 and 100");
            }
        }
    }

    // Order class for demonstration
    class Order
    {
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}