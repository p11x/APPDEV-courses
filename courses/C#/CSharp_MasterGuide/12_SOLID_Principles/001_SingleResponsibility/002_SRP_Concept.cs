/*
 * ============================================================
 * TOPIC     : SOLID Principles
 * SUBTOPIC  : Single Responsibility Principle - Concept
 * FILE      : 01_SRP_Concept.cs
 * PURPOSE   : Demonstrates SRP - a class should have only one reason to change
 * ============================================================
 */
using System; // Core System namespace for Console

namespace CSharp_MasterGuide._12_SOLID_Principles._01_SingleResponsibility._01_SRP_Concept
{
    /// <summary>
    /// Demonstrates Single Responsibility Principle concept
    /// </summary>
    public class SRPConceptDemo
    {
        /// <summary>
        /// Entry point for SRP concept examples
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs to console
            // Output: === Single Responsibility Principle ===
            Console.WriteLine("=== Single Responsibility Principle ===\n");

            // ── CONCEPT: What is SRP? ─────────────────────────────────────
            // A class should have only one reason to change
            // Each class should do one thing and do it well

            // Example 1: Violating SRP
            // Output: 1. Violating SRP:
            Console.WriteLine("1. Violating SRP:");
            
            // A class doing too many things - bad design
            var violateSRP = new ViolatingSRP();
            violateSRP.Process();
            // Output: Processing order
            // Output: Saving to database
            // Output: Sending email
            // Output: Generating report
            // Output: Logging

            // Example 2: Following SRP - separation of concerns
            // Output: 2. Following SRP:
            Console.WriteLine("\n2. Following SRP:");
            
            // Each class has one responsibility
            var order = new OrderProcessor();
            // Processes order only
            order.ProcessOrder("Product", 1);
            // Output: Processing order: Product x1
            
            var saver = new DataSaver();
            // Saves data only
            saver.Save("Order data");
            // Output: Saving to database
            
            var notifier = new EmailNotifier();
            // Sends notifications only
            notifier.SendEmail("order@email.com", "Order confirmed");
            // Output: Sending email to: order@email.com
            
            var reporter = new ReportGenerator();
            // Generates reports only
            reporter.GenerateReport();
            // Output: Generating report
            
            var logger = new Logger();
            // Logs only
            logger.Log("Order processed");
            // Output: Logging: Order processed

            // ── CONCEPT: Benefits of SRP ──────────────────────────────────
            // Easier to understand, maintain, and test
            // Changes are isolated to specific classes

            // Example 3: Benefits
            // Output: 3. Benefits of SRP:
            Console.WriteLine("\n3. Benefits of SRP:");
            
            // Each component can be tested independently
            var result = order.ProcessOrder("Test", 1);
            // Output: Processed: Test x1
            Console.WriteLine($"   Result: {result}");
            // Output: Result: Order Processed
            
            // Can easily change email implementation
            notifier.SendEmail("new@email.com", "Updated order");
            // Output: Sending email to: new@email.com

            // ── CONCEPT: When to apply SRP ────────────────────────────────
            // When a class has multiple responsibilities
            // When changes to one aspect affect the class

            // Example 4: Real-world sign
            // Output: 4. Signs you need SRP:
            Console.WriteLine("\n4. Signs you need SRP:");
            
            // Class name contains "And" or "Or"
            Console.WriteLine("   - Class name has 'And', 'Or', 'With'");
            // Output: - Class name has 'And', 'Or', 'With'
            
            // Multiple reasons to change
            Console.WriteLine("   - Multiple reasons to change");
            // Output: - Multiple reasons to change
            
            // Hard to test in isolation
            Console.WriteLine("   - Hard to test in isolation");
            // Output: - Hard to test in isolation

            Console.WriteLine("\n=== SRP Concept Complete ===");
        }
    }

    /// <summary>
    /// BAD: Violates SRP - has multiple responsibilities
    /// </summary>
    public class ViolatingSRP
    {
        public void Process()
        {
            // Does too many things
            Console.WriteLine("   Processing order");
            Console.WriteLine("   Saving to database");
            Console.WriteLine("   Sending email");
            Console.WriteLine("   Generating report");
            Console.WriteLine("   Logging");
        }
    }

    /// <summary>
    /// GOOD: Processes orders - single responsibility
    /// </summary>
    public class OrderProcessor
    {
        public string ProcessOrder(string product, int quantity)
        {
            // Only processes orders
            Console.WriteLine($"   Processing order: {product} x{quantity}");
            return "Order Processed";
        }
    }

    /// <summary>
    /// GOOD: Saves data - single responsibility
    /// </summary>
    public class DataSaver
    {
        public void Save(string data)
        {
            // Only saves data
            Console.WriteLine("   Saving to database");
        }
    }

    /// <summary>
    /// GOOD: Sends emails - single responsibility
    /// </summary>
    public class EmailNotifier
    {
        public void SendEmail(string to, string message)
        {
            // Only sends emails
            Console.WriteLine($"   Sending email to: {to}");
        }
    }

    /// <summary>
    /// GOOD: Generates reports - single responsibility
    /// </summary>
    public class ReportGenerator
    {
        public void GenerateReport()
        {
            // Only generates reports
            Console.WriteLine("   Generating report");
        }
    }

    /// <summary>
    /// GOOD: Logs - single responsibility
    /// </summary>
    public class Logger
    {
        public void Log(string message)
        {
            // Only logs
            Console.WriteLine($"   Logging: {message}");
        }
    }
}