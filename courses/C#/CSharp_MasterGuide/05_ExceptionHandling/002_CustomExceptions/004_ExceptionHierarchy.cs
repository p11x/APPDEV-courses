/*
 * ============================================================
 * TOPIC     : Exception Handling
 * SUBTOPIC  : Exception Hierarchy
 * FILE      : ExceptionHierarchy.cs
 * PURPOSE   : Understand the .NET exception hierarchy, when to use
 *            ApplicationException vs SystemException, and proper
 *            exception inheritance
 * ============================================================
 */

using System;

namespace CSharp_MasterGuide._05_ExceptionHandling._02_CustomExceptions
{
    class ExceptionHierarchy
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Exception Hierarchy in C# ===\n");

            // ═══════════════════════════════════════════════════════════
            // SECTION 1: Understanding the Exception Hierarchy
            // ═══════════════════════════════════════════════════════════

            // Base Exception (System.Exception)
            // ├── SystemException (system-level exceptions)
            // └── ApplicationException (application-level exceptions)

            // ── EXAMPLE 1: System Exception Types ────────────────────────
            try { int.Parse("not a number"); }
            catch (FormatException ex)
            {
                Console.WriteLine($"  SystemException: {ex.GetType().Name}");
                Console.WriteLine($"  Base type: {ex.GetType().BaseType.Name}");
            }
            // Output: SystemException: FormatException
            // Output: Base type: SystemException

            try { int[] arr = { 1 }; var x = arr[10]; }
            catch (IndexOutOfRangeException ex)
            {
                Console.WriteLine($"\n  SystemException: {ex.GetType().Name}");
                Console.WriteLine($"  Base type: {ex.GetType().BaseType.Name}");
            }
            // Output: SystemException: IndexOutOfRangeException
            // Output: Base type: SystemException

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: ApplicationException (Legacy)
            // ═══════════════════════════════════════════════════════════

            // NOTE: ApplicationException is largely deprecated in modern .NET
            // It was originally meant for application-defined exceptions
            // Now recommended to just inherit directly from Exception

            // ── EXAMPLE 1: Legacy ApplicationException ──────────────────────
            try
            {
                throw new LegacyAppException("Legacy application error");
            }
            catch (ApplicationException ex)
            {
                Console.WriteLine($"\n  ApplicationException caught:");
                Console.WriteLine($"  Type: {ex.GetType().Name}");
                Console.WriteLine($"  Base: {ex.GetType().BaseType.Name}");
            }
            // Output: ApplicationException caught:
            // Output: Type: LegacyAppException
            // Output: Base: ApplicationException

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: SystemException Types
            // ═══════════════════════════════════════════════════════════

            // SystemException = exceptions thrown by the CLR/system
            // Examples: NullReferenceException, ArgumentException, etc.

            // ── EXAMPLE 1: ArgumentException Types ─────────────────────
            try
            {
                int[] arr = { 1, 2, 3 };
                Array.IndexOf(arr, 1, 10); // position out of range
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($"\n  ArgumentOutOfRangeException:");
                Console.WriteLine($"  ParamName: {ex.ParamName}");
            }
            // Output: ArgumentOutOfRangeException:
            // Output: ParamName: position

            // ── EXAMPLE 2: InvalidOperationException ────────────────────
            try
            {
                var list = new System.Collections.Generic.List<string>();
                list.Sort();
                var item = list[0];
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"\n  InvalidOperationException:");
                Console.WriteLine($"  Message: {ex.Message}");
            }
            // Output: InvalidOperationException:
            // Output: Message: Index was out of range.

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: When to Use Each Exception Type
            // ═══════════════════════════════════════════════════════════

            // System exceptions = bugs in code (NullReference, IndexOutOfRange)
            // Application exceptions = expected business logic errors

            // ── EXAMPLE 1: Logic for Choosing Exception Type ───────────────
            Console.WriteLine($"\n  Choosing Exception Type:");

            // Use ArgumentException for invalid method arguments
            try { ValidateInput(""); }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"  Input validation: {ex.GetType().Name}");
            }
            // Output: Input validation: ArgumentException

            // Use InvalidOperationException for wrong state
            try { ProcessAfterClose(); }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"  State validation: {ex.GetType().Name}");
            }
            // Output: State validation: InvalidOperationException

            // Use NotImplementedException for missing features
            try { throw new NotImplementedException(); }
            catch (NotImplementedException ex)
            {
                Console.WriteLine($"  Missing feature: {ex.GetType().Name}");
            }
            // Output: Missing feature: NotImplementedException

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: Catching Base Exception
            // ═══════════════���═══════════════════════════════════════════

            // Catching Exception (base class) catches ALL exceptions

            // ── EXAMPLE 1: Catch Exception Base ────────────────────────
            try { throw new FormatException("test"); }
            catch (Exception ex)
            {
                Console.WriteLine($"\n  Caught with Exception base:");
                Console.WriteLine($"  Type: {ex.GetType().Name}");
                Console.WriteLine($"  Is SystemException: {ex is SystemException}");
            }
            // Output: Caught with Exception base:
            // Output: Type: FormatException
            // Output: Is SystemException: True

            try { throw new CustomAppException("test"); }
            catch (Exception ex)
            {
                Console.WriteLine($"\n  Caught with Exception base:");
                Console.WriteLine($"  Type: {ex.GetType().Name}");
                Console.WriteLine($"  Is SystemException: {ex is SystemException}");
            }
            // Output: Caught with Exception base:
            // Output: Type: CustomAppException
            // Output: Is SystemException: False

            // ═══════════════════════════════════════════════════════════
            // SECTION 6: Real-World: Exception Handling Strategy
            // ═══════════════════════════════════════════════════════════

            // Best practice: catch specific exceptions first
            // Then catch Exception as fallback for logging

            // ── EXAMPLE 1: Multi-Level Catch ────────────────────────────
            var service = new PaymentService();

            var result1 = service.ProcessPayment(100, "valid_card");
            Console.WriteLine($"\n  Payment (valid): {result1}");
            // Output: Payment (valid): Success

            var result2 = service.ProcessPayment(0, "valid_card");
            Console.WriteLine($"  Payment (zero): {result2}");
            // Output: Payment (zero): Failed
            // Output: ArgumentException: Amount must be positive

            var result3 = service.ProcessPayment(100, "");
            Console.WriteLine($"  Payment (no card): {result3}");
            // Output: Payment (no card): Failed
            // Output: ArgumentException: Card number required

            var result4 = service.ProcessPayment(100, "declined");
            Console.WriteLine($"  Payment (declined): {result4}");
            // Output: Payment (declined): Failed
            // Output: PaymentDeclinedException: Card declined

            Console.WriteLine("\n=== Exception Hierarchy Complete ===");
        }

        static void ValidateInput(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentException("Input cannot be empty", "input");
            }
        }

        static void ProcessAfterClose()
        {
            throw new InvalidOperationException("Cannot process after close");
        }
    }

    // ═══════════════════════════════════════════════════════════
    // LEGACY APPLICATIONEXCEPTION (DEPRECATED)
    // ═════════════════════════════════════════════��═��═══════════

    // NOTE: This class exists to show historical usage
    // In modern code, just inherit from Exception directly
    [Serializable]
    public class LegacyAppException : ApplicationException
    {
        public LegacyAppException() : base() { }
        public LegacyAppException(string message) : base(message) { }
        public LegacyAppException(string message, Exception inner) : base(message, inner) { }
    }

    // ═══════════════════════════════════════════════════════════
    // CUSTOM APPLICATION EXCEPTION (MODERN APPROACH)
    // ═══════════════════════════════════════════════════════════

    // Modern approach: just inherit from Exception
    // No need to use ApplicationException
    [Serializable]
    public class CustomAppException : Exception
    {
        public CustomAppException() : base() { }
        public CustomAppException(string message) : base(message) { }
        public CustomAppException(string message, Exception inner) : base(message, inner) { }
    }

    // ═══════════════════════════════════════════════════════════
    // REAL-WORLD: Payment Service
    // ═══════════════════════════════════════════════════════════

    class PaymentDeclinedException : Exception
    {
        public string DeclineCode { get; }

        public PaymentDeclinedException() : base() { }
        public PaymentDeclinedException(string message) : base(message) { }
        public PaymentDeclinedException(string message, Exception inner) : base(message, inner) { }
        public PaymentDeclinedException(string message, string declineCode) : base(message)
        {
            DeclineCode = declineCode;
        }
    }

    class PaymentService
    {
        public string ProcessPayment(decimal amount, string cardNumber)
        {
            try
            {
                // Validate amount
                if (amount <= 0)
                {
                    throw new ArgumentException("Amount must be positive", "amount");
                }

                // Validate card
                if (string.IsNullOrEmpty(cardNumber))
                {
                    throw new ArgumentException("Card number required", "cardNumber");
                }

                // Simulate payment processing
                if (cardNumber == "declined")
                {
                    throw new PaymentDeclinedException("Card declined", "DECLINED");
                }

                return "Success";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  {ex.GetType().Name}: {ex.Message}");
                return "Failed";
            }
        }
    }
}