/*
 * ============================================================
 * TOPIC     : Exception Handling
 * SUBTOPIC  : Custom Exceptions - Basics
 * FILE      : CustomExceptionBasics.cs
 * PURPOSE   : Learn how to create custom exception classes by 
 *            inheriting from Exception, defining constructors,
 *            and using custom exceptions in code
 * ============================================================
 */

using System;

namespace CSharp_MasterGuide._05_ExceptionHandling._02_CustomExceptions
{
    class CustomExceptionBasics
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Custom Exception Basics in C# ===\n");

            // ═══════════════════════════════════════════════════════════
            // SECTION 1: Creating a Basic Custom Exception
            // ═══════════════════════════════════════════════════════════

            // Custom exceptions should inherit from System.Exception
            // The simplest custom exception just needs constructors

            // ── EXAMPLE 1: Throwing Custom Exception ────────────────────────
            try
            {
                throw new InvalidAgeException("Age must be between 0 and 120");
            }
            catch (InvalidAgeException ex)
            {
                Console.WriteLine($"  Caught: {ex.GetType().Name}");
                Console.WriteLine($"  Message: {ex.Message}");
            }
            // Output: Caught: InvalidAgeException
            // Output: Message: Age must be between 0 and 120

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: Custom Exception with Inner Exception
            // ═══════════════════════════════════════════════════════════

            // The inner exception constructor wraps the original exception
            // This preserves the full exception chain for debugging

            // ── EXAMPLE 1: Wrapping Original Exception ───────────────────
            try
            {
                try
                {
                    int num = int.Parse("not a number");
                }
                catch (FormatException innerEx)
                {
                    throw new CalculationException("Calculation failed", innerEx);
                }
            }
            catch (CalculationException ex)
            {
                Console.WriteLine($"\n  Outer exception: {ex.GetType().Name}");
                Console.WriteLine($"  Message: {ex.Message}");
                Console.WriteLine($"  Inner exception: {ex.InnerException?.GetType().Name}");
            }
            // Output: Outer exception: CalculationException
            // Output: Message: Calculation failed
            // Output: Inner exception: FormatException

            // ═════════════════════════════════════════════════════���═════
            // SECTION 3: Custom Exception with Custom Property
            // ═══════════════════════════════════════════════════════════

            // Custom properties provide additional context about the error
            // These help with debugging and error handling

            // ── EXAMPLE 1: Using Custom Properties ────────────────────────
            try
            {
                throw new InsufficientFundsException(500.00, 1000.00);
            }
            catch (InsufficientFundsException ex)
            {
                Console.WriteLine($"\n  Exception: {ex.GetType().Name}");
                Console.WriteLine($"  Message: {ex.Message}");
                Console.WriteLine($"  Balance: ${ex.Balance:F2}");
                Console.WriteLine($"  Amount Requested: ${ex.AmountRequested:F2}");
            }
            // Output: Exception: InsufficientFundsException
            // Output: Message: Insufficient funds. Available: 500.00, Requested: 1000.00
            // Output: Balance: $500.00
            // Output: Amount Requested: $1000.00

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: Throwing Custom Exceptions in Methods
            // ═══════════════════════════════════════════════════════════

            // Methods should throw meaningful exceptions for business logic errors

            // ── EXAMPLE 1: Validating Input with Custom Exception ───────────────
            var calculator = new BankAccount();
            
            try
            {
                calculator.Withdraw(500.00);
            }
            catch (InsufficientFundsException ex)
            {
                Console.WriteLine($"\n  Withdrawal failed: {ex.Message}");
            }
            // Output: Withdrawal failed: Insufficient funds. Available: 100.00, Requested: 500.00

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: Re-Throwing Custom Exceptions
            // ═══════════════════════════════════════════════════════════

            // You can catch, log, and re-throw exceptions to let callers handle them

            // ── EXAMPLE 1: Catch and Re-Throw ───────────────────────────────
            try
            {
                ProcessUserAge(-5);
            }
            catch (InvalidAgeException ex)
            {
                Console.WriteLine($"\n  Logged error: {ex.Message}");
                throw; // Re-throws the original exception
            }
            // Output: Logged error: Age must be between 0 and 120

            // ═══════════════════════════════════════════════════════════
            // SECTION 6: Real-World: Order Processing
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Process Order ────────────────────────────���──────────
            var orderProcessor = new OrderProcessor();
            bool success = orderProcessor.ProcessOrder(null);
            Console.WriteLine($"\n  Order (null items): {(success ? "Processed" : "Failed")}");
            // Output: Order (null items): Failed
            // Output: OrderValidationException: Order must contain at least one item

            var items = new OrderItem[] { new OrderItem("Widget", 5) };
            success = orderProcessor.ProcessOrder(items);
            Console.WriteLine($"  Order (valid): {(success ? "Processed" : "Failed")}");
            // Output: Order (valid): Processed

            Console.WriteLine("\n=== Custom Exception Basics Complete ===");
        }

        static void ProcessUserAge(int age)
        {
            if (age < 0 || age > 120)
            {
                throw new InvalidAgeException("Age must be between 0 and 120");
            }
        }
    }

    // ═══════════════════════════════════════════════════════════════════
    // BASIC CUSTOM EXCEPTIONS
    // ═══════════════════════════════════════════════════════════

    // A minimal custom exception needs at least the base constructors
    // Each constructor should call the base version to properly set properties

    class InvalidAgeException : Exception
    {
        // Default constructor - initializes with empty message
        public InvalidAgeException() : base() { }
        
        // Message constructor - allows custom message
        public InvalidAgeException(string message) : base(message) { }
        
        // Message and inner exception constructor
        public InvalidAgeException(string message, Exception inner) : base(message, inner) { }
    }

    // ═══════════════════════════════════════════════════════════
    // Custom Exception with Inner Exception Support
    // ═══════════════════════════════════════════════════════════

    class CalculationException : Exception
    {
        public CalculationException() : base() { }
        public CalculationException(string message) : base(message) { }
        public CalculationException(string message, Exception inner) : base(message, inner) { }
    }

    // ═══════════════════════════════════════════════════════════
    // Custom Exception with Custom Properties
    // ═══════════════════════════════════════════════════════════

    class InsufficientFundsException : Exception
    {
        // Custom property to store available balance
        public double Balance { get; set; }
        
        // Custom property to store requested amount
        public double AmountRequested { get; set; }

        public InsufficientFundsException() : base() { }

        public InsufficientFundsException(double balance, double amountRequested) 
            : base($"Insufficient funds. Available: {balance:F2}, Requested: {amountRequested:F2}")
        {
            Balance = balance;
            AmountRequested = amountRequested;
        }

        public InsufficientFundsException(double balance, double amountRequested, Exception inner)
            : base($"Insufficient funds. Available: {balance:F2}, Requested: {amountRequested:F2}", inner)
        {
            Balance = balance;
            AmountRequested = amountRequested;
        }
    }

    // ═══════════════════════════════════════════════════════════
    // REAL-WORLD: Bank Account Class
    // ═══════════════════════════════════════════════════════════

    class BankAccount
    {
        private double _balance = 100.00;

        public void Withdraw(double amount)
        {
            if (amount > _balance)
            {
                throw new InsufficientFundsException(_balance, amount);
            }
            _balance -= amount;
            Console.WriteLine($"  Withdrew: ${amount:F2}, Remaining: ${_balance:F2}");
        }
    }

    // ═══════════════════════════════════════════════════════════
    // REAL-WORLD: Order Processing
    // ═══════════════════════════════════════════════════════════

    class OrderValidationException : Exception
    {
        public OrderValidationException() : base() { }
        public OrderValidationException(string message) : base(message) { }
        public OrderValidationException(string message, Exception inner) : base(message, inner) { }
    }

    class OrderProcessor
    {
        public bool ProcessOrder(OrderItem[] items)
        {
            try
            {
                ValidateOrder(items);
                Console.WriteLine($"  Processing order with {items.Length} items...");
                return true;
            }
            catch (OrderValidationException ex)
            {
                Console.WriteLine($"  {ex.GetType().Name}: {ex.Message}");
                return false;
            }
        }

        private void ValidateOrder(OrderItem[] items)
        {
            if (items == null || items.Length == 0)
            {
                throw new OrderValidationException("Order must contain at least one item");
            }
        }
    }

    class OrderItem
    {
        public string Name { get; set; }
        public int Quantity { get; set; }

        public OrderItem(string name, int quantity)
        {
            Name = name;
            Quantity = quantity;
        }
    }
}