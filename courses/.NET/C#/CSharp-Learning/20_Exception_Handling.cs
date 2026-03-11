/*
================================================================================
TOPIC 20: EXCEPTION HANDLING
================================================================================

Exception handling manages runtime errors gracefully.

TABLE OF CONTENTS:
1. What are Exceptions?
2. Try-Catch-Finally
3. Custom Exceptions
4. Throwing Exceptions
================================================================================
*/

namespace ExceptionHandlingExamples
{
    class Program
    {
        static void Main()
        {
            // Basic try-catch
            Console.WriteLine("=== Basic Exception Handling ===");
            
            try
            {
                int[] numbers = { 1, 2, 3 };
                Console.WriteLine(numbers[5]);  // Index out of range!
            }
            catch (IndexOutOfRangeException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            
            // Multiple catch blocks
            Console.WriteLine("\n=== Multiple Catch Blocks ===");
            
            try
            {
                Console.Write("Enter a number: ");
                string input = Console.ReadLine();
                int num = int.Parse(input);
                int result = 10 / num;
                Console.WriteLine($"Result: {result}");
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Format error: {ex.Message}");
            }
            catch (DivideByZeroException ex)
            {
                Console.WriteLine($"Divide by zero: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General error: {ex.Message}");
            }
            
            // Finally block
            Console.WriteLine("\n=== Finally Block ===");
            
            try
            {
                int result = 10 / 2;
                Console.WriteLine($"Result: {result}");
            }
            catch
            {
                Console.WriteLine("Error occurred");
            }
            finally
            {
                Console.WriteLine("This always executes!");
            }
            
            // Throw exceptions
            Console.WriteLine("\n=== Throwing Exceptions ===");
            
            try
            {
                ValidateAge(15);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Validation error: {ex.Message}");
            }
        }
        
        static void ValidateAge(int age)
        {
            if (age < 18)
            {
                throw new ArgumentException("Age must be 18 or older");
            }
            Console.WriteLine("Age is valid");
        }
    }
}

/*
KEY EXCEPTION CLASSES:
-----------------------
Exception           - Base class for all
ArgumentException   - Invalid argument
FormatException     - Wrong format
NullReferenceException - Null object access
IndexOutOfRangeException - Invalid index
DivideByZeroException - Division by zero
FileNotFoundException - File doesn't exist

BEST PRACTICES:
---------------
- Catch specific exceptions first
- Use finally for cleanup
- Don't catch everything blindly
- Log exceptions
- Throw meaningful exceptions
*/

// ================================================================================
// NEXT STEPS
// =============================================================================

/*
NEXT: Topic 21 covers File Handling.
*/
