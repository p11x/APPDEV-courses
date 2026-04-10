/*
 * ============================================================
 * TOPIC     : Exception Handling
 * SUBTOPIC  : Try-Catch Basics
 * FILE      : TryCatchBasics.cs
 * PURPOSE   : Learn try-catch fundamentals, throwing exceptions,
 *            and basic exception handling patterns
 * ============================================================
 */

using System;

namespace CSharp_MasterGuide._05_ExceptionHandling._01_TryCatchFinally
{
    class TryCatchBasics
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Try-Catch Basics in C# ===\n");

            // ═══════════════════════════════════════════════════════════
            // SECTION 1: Basic Try-Catch Structure
            // ═══════════════════════════════════════════════════════════

            // The try block contains code that might throw an exception
            // The catch block handles the exception if one occurs

            // ── EXAMPLE 1: Simple Try-Catch ───────────────────────────────
            try
            {
                int[] numbers = { 1, 2, 3 };
                int value = numbers[10]; // This will throw IndexOutOfRangeException
                Console.WriteLine($"Value: {value}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  Caught exception: {ex.GetType().Name}");
                Console.WriteLine($"  Message: {ex.Message}");
            }
            // Output: Caught exception: IndexOutOfRangeException
            // Output: Message: Index was outside the bounds of the array.

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: Try-Catch with Division
            // ═══════════════════════════════════════════════════════════

            // Division by zero throws DivideByZeroException

            // ── EXAMPLE 1: Division by Zero ──────────────────────────────
            try
            {
                int result = 10 / 0; // DivideByZeroException
                Console.WriteLine($"Result: {result}");
            }
            catch (DivideByZeroException ex)
            {
                Console.WriteLine($"\n  DivideByZeroException caught!");
                Console.WriteLine($"  Message: {ex.Message}");
            }
            // Output: DivideByZeroException caught!
            // Output: Message: Attempted to divide by zero.

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: Throwing Exceptions
            // ═══════════════════════════════════════════════════════════

            // Use the throw keyword to explicitly throw an exception

            // ── EXAMPLE 1: throw Keyword ────────────────────────────────
            int age = -5;
            try
            {
                if (age < 0)
                {
                    throw new ArgumentException("Age cannot be negative", "age");
                }
                Console.WriteLine($"\n  Age is valid: {age}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"\n  ArgumentException caught!");
                Console.WriteLine($"  Parameter: {ex.ParamName}");
                Console.WriteLine($"  Message: {ex.Message}");
            }
            // Output: ArgumentException caught!
            // Output: Parameter: age
            // Output: Message: Age cannot be negative

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: Throwing Custom Exceptions
            // ═══════════════════════════════════════════════════════════

            // You can throw any exception type, including custom ones

            // ── EXAMPLE 1: Throwing InvalidOperationException ───────────
            try
            {
                ValidateUserAccess(null);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"\n  InvalidOperationException: {ex.Message}");
            }
            // Output: InvalidOperationException: User cannot be null

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: Try-Catch with Multiple Statements
            // ═══════════════════════════════════════════════════════════

            // The catch block executes if ANY statement in the try block throws

            // ── EXAMPLE 1: Multiple Statements in Try ───────────────────
            try
            {
                string str = null;
                int length = str.Length; // First exception
                int num = int.Parse("abc"); // Won't reach here
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine($"\n  First catch - NullReferenceException: {ex.Message}");
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"\n  Second catch - FormatException: {ex.Message}");
            }
            // Output: First catch - NullReferenceException: Object reference not set to an instance of an object.

            // ═══════════════════════════════════════════════════════════
            // SECTION 6: Real-World: User Registration Validation
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Validate User Registration ────────────────────
            var register = new UserRegistration();
            
            bool result1 = register.RegisterUser("John", "john@email.com", 25);
            Console.WriteLine($"\n  Registration (valid): {(result1 ? "Success" : "Failed")}");
            // Output: Registration (valid): Success

            bool result2 = register.RegisterUser("", "john@email.com", 25);
            Console.WriteLine($"  Registration (empty name): {(result2 ? "Success" : "Failed")}");
            // Output: Registration (empty name): Failed
            // Output: ArgumentException: Name cannot be empty

            bool result3 = register.RegisterUser("John", "invalid-email", 25);
            Console.WriteLine($"  Registration (bad email): {(result3 ? "Success" : "Failed")}");
            // Output: Registration (bad email): Failed
            // Output: FormatException: The format of the email address is invalid.

            Console.WriteLine("\n=== Try-Catch Basics Complete ===");
        }

        // ── REAL-WORLD EXAMPLE: User Access Validation ───────────────────────
        static void ValidateUserAccess(User user)
        {
            if (user == null)
            {
                throw new InvalidOperationException("User cannot be null");
            }
            // Additional validation logic...
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Real-World: User Registration Class
    // ═══════════════════════════════════════════════════════════

    class UserRegistration
    {
        public bool RegisterUser(string name, string email, int age)
        {
            try
            {
                ValidateName(name);
                ValidateEmail(email);
                ValidateAge(age);
                
                Console.WriteLine($"  Registered: {name}, {email}, {age}");
                return true;
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"  {ex.GetType().Name}: {ex.Message}");
                return false;
            }
        }

        private void ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name cannot be empty", "name");
            }
        }

        private void ValidateEmail(string email)
        {
            if (!email.Contains("@") || !email.Contains("."))
            {
                throw new FormatException("The format of the email address is invalid.");
            }
        }

        private void ValidateAge(int age)
        {
            if (age < 0 || age > 150)
            {
                throw new ArgumentOutOfRangeException("age", "Age must be between 0 and 150");
            }
        }
    }

    // Simple User class for demonstration
    class User
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
}