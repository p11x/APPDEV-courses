/*
================================================================================
TOPIC 30: BEST PRACTICES
================================================================================

Following best practices makes your code maintainable, efficient, and professional.

TABLE OF CONTENTS:
1. Naming Conventions
2. Code Organization
3. Error Handling
4. Performance Tips
5. Security Best Practices
================================================================================
*/

using System;

namespace BestPracticesExamples
{
    // ====================================================================
    // NAMING CONVENTIONS
    // ====================================================================
    
    // PascalCase for: Classes, Methods, Properties, Enums
    public class CustomerService
    {
        // camelCase for: local variables, parameters
        public int CalculateTotal(int quantity, decimal unitPrice)
        {
            int total = quantity * (int)unitPrice;
            return total;
        }
    }
    
    // UPPER_SNAKE_CASE for: Constants
    public class Constants
    {
        public const int MaxRetryCount = 3;
        public const string DefaultCurrency = "USD";
    }
    
    // Interfaces: Start with I
    interface IRepository<T>
    {
        T GetById(int id);
    }
    
    // ====================================================================
    // CODE ORGANIZATION
    // ====================================================================
    
    // Use meaningful names
    class BadCode
    {
        // Bad - unclear names
        int[] d = { 1, 2, 3 };
        void doIt() { /* code */ }
    }
    
    class GoodCode
    {
        // Good - clear names
        int[] dailyTemperatures = { 1, 2, 3 };
        void ProcessTemperatureData() { /* code */ }
    }
    
    // ====================================================================
    // ERROR HANDLING BEST PRACTICES
    // ====================================================================
    
    class ErrorHandling
    {
        // Do: Use specific exceptions
        void GoodPractice()
        {
            try
            {
                // Risky operation
            }
            catch (FileNotFoundException ex)
            {
                // Handle specific exception
            }
            catch (Exception ex)
            {
                // Handle general exception
            }
        }
        
        // Don't: Catch all without logging
        void BadPractice()
        {
            try
            {
                // Risky operation
            }
            catch
            {
                // Silent failure - bad!
            }
        }
    }
    
    // ====================================================================
    // PERFORMANCE TIPS
    // ====================================================================
    
    class PerformanceTips
    {
        // Use StringBuilder for many concatenations
        void StringBuilderExample()
        {
            // Good: StringBuilder for loops
            var sb = new System.Text.StringBuilder();
            for (int i = 0; i < 100; i++)
            {
                sb.Append(i);
            }
            
            // Bad: String concatenation in loop
            // string result = "";
            // for (int i = 0; i < 100; i++)
            //     result += i;
        }
        
        // Use appropriate collections
        void CollectionChoice()
        {
            // List for most cases
            var list = new System.Collections.Generic.List<int>();
            
            // Dictionary for lookups
            var dict = new System.Collections.Generic.Dictionary<string, int>();
            
            // HashSet for unique items
            var set = new System.Collections.Generic.HashSet<int>();
        }
        
        // Avoid boxing/unboxing
        void AvoidBoxing()
        {
            // Good: Use appropriate types
            int count = 5;
            
            // Avoid:
            // object o = count;  // boxing
            // int i = (int)o;   // unboxing
        }
    }
    
    // ====================================================================
    // SECURITY BEST PRACTICES
    // ====================================================================
    
    class SecurityTips
    {
        // Validate input
        void ValidateInput(string username)
        {
            // Good: Validate input
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Invalid username");
        }
        
        // Use parameterized queries (for SQL)
        // Good: cmd.Parameters.AddWithValue("@name", username);
        // Bad: cmd.ExecuteQuery("SELECT * FROM Users WHERE Name = '" + username + "'");
        
        // Don't expose sensitive data
        // Good: Don't log passwords
        // Bad: Console.WriteLine($"Password: {password}");
    }
    
    class Program
    {
        static void Main()
        {
            Console.WriteLine("=== Best Practices Summary ===");
            Console.WriteLine("1. Use meaningful names");
            Console.WriteLine("2. Follow naming conventions");
            Console.WriteLine("3. Handle exceptions properly");
            Console.WriteLine("4. Write clean, readable code");
            Console.WriteLine("5. Consider performance");
            Console.WriteLine("6. Validate input");
            Console.WriteLine("7. Keep methods small and focused");
            Console.WriteLine("8. Write comments for complex logic");
            Console.WriteLine("9. Test your code");
            Console.WriteLine("10. Use version control");
        }
    }
}

/*
SUMMARY OF BEST PRACTICES:
-------------------------
NAMING:        Clear, consistent, meaningful
ORGANIZATION:  Single responsibility, small methods
ERROR HANDLING: Specific catches, proper logging
PERFORMANCE:   StringBuilder, correct collections
SECURITY:     Validate input, parameterized queries
DOCUMENTATION: Comments where needed
TESTING:      Unit tests, integration tests
*/

// ================================================================================
// CONGRATULATIONS!
// =============================================================================

/*
YOU HAVE COMPLETED THE C# LEARNING COURSE!

WHAT YOU'VE LEARNED:
--------------------
✓ Topics 1-10:   Beginner - Variables, Types, Control Flow, Loops, Methods
✓ Topics 11-20:  Intermediate - Collections, OOP, Exception Handling
✓ Topics 21-30:  Advanced - LINQ, Delegates, Events, Async, DI, EF

NEXT STEPS:
----------
1. Build projects to practice
2. Learn ASP.NET Core for web development
3. Explore Xamarin/.NET MAUI for mobile
4. Learn Unity for game development
5. Study design patterns
6. Contribute to open source

GOOD LUCK ON YOUR C# JOURNEY!
*/
