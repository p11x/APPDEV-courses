/*
 * ============================================================
 * TOPIC     : SOLID Principles
 * SUBTOPIC  : Single Responsibility Principle
 * FILE      : 01_SingleResponsibility.cs
 * PURPOSE   : Demonstrates SRP - one reason to change
 * ============================================================
 */
using System; // needed for Console, basic types
using System.Collections.Generic; // needed for List<T>

namespace CSharp_MasterGuide._12_SOLID_Principles._01_SingleResponsibility
{
    /// <summary>
    /// Demonstrates Single Responsibility Principle
    /// </summary>
    public class SingleResponsibilityDemo
    {
        /// <summary>
        /// Entry point for SRP examples
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs to console
            // Output: === Single Responsibility Principle ===
            Console.WriteLine("=== Single Responsibility Principle ===\n");

            // ── CONCEPT: What is SRP? ────────────────────────────────────────
            // A class should have only one reason to change

            // Example 1: Violating SRP
            // Output: 1. Violating SRP:
            Console.WriteLine("1. Violating SRP:");
            
            // This class has multiple responsibilities
            var bad = new BadInvoice();
            bad.CreateInvoice(); // creates invoice
            bad.PrintInvoice(); // prints invoice
            bad.SaveToDatabase(); // saves to database
            bad.SendEmail(); // sends email
            // Output: Created invoice
            // Output: Printed invoice
            // Output: Saved to database
            // Output: Sent email
            
            // Problem: Any change to printing affects invoice creation

            // Example 2: Following SRP
            // Output: 2. Following SRP:
            Console.WriteLine("\n2. Following SRP:");
            
            // Each class has single responsibility
            var invoiceCreator = new InvoiceCreator();
            var invoicePrinter = new InvoicePrinter();
            var invoiceRepository = new InvoiceRepository();
            var invoiceEmailer = new InvoiceEmailer();
            
            // Create
            var invoice = invoiceCreator.Create();
            // Output: Created invoice
            
            // Print
            invoicePrinter.Print(invoice);
            // Output: Printed invoice
            
            // Save
            invoiceRepository.Save(invoice);
            // Output: Saved to database
            
            // Email
            invoiceEmailer.Send(invoice);
            // Output: Sent email

            // ── CONCEPT: Benefits ────────────────────────────────────────────
            // Easier to maintain, test, understand

            // Example 3: Benefits
            // Output: 3. Benefits:
            Console.WriteLine("\n3. Benefits:");
            
            // Test each responsibility separately
            var testCreator = new InvoiceCreator();
            var testInvoice = testCreator.Create();
            // Output: Invoice created with ID: 123
            
            // Coupling reduced - changes isolated
            // Output: Printing changes don't affect data layer

            // ── REAL-WORLD EXAMPLE: User Management ─────────────────────────
            // Output: --- Real-World: User Management ---
            Console.WriteLine("\n--- Real-World: User Management ---");
            
            // Separate concerns
            var userValidator = new UserValidator();
            var userRepository = new UserRepository();
            var userAuthenticator = new UserAuthenticator();
            
            // Validate
            var isValid = userValidator.Validate("john@email.com");
            // Output: Valid email format
            
            // Save
            userRepository.Save("john", "john@email.com");
            // Output: User saved to database
            
            // Authenticate
            var authenticated = userAuthenticator.Authenticate("john", "pass123");
            // Output: User authenticated

            Console.WriteLine("\n=== SRP Complete ===");
        }
    }

    /// <summary>
    /// BAD: Violates SRP - multiple responsibilities
    /// </summary>
    public class BadInvoice
    {
        public void CreateInvoice() => Console.WriteLine("   Created invoice");
        public void PrintInvoice() => Console.WriteLine("   Printed invoice");
        public void SaveToDatabase() => Console.WriteLine("   Saved to database");
        public void SendEmail() => Console.WriteLine("   Sent email");
    }

    /// <summary>
    /// Invoice - just data
    /// </summary>
    public class Invoice
    {
        public int Id { get; set; } // property: invoice ID
        public decimal Amount { get; set; } // property: invoice amount
        public string Customer { get; set; } // property: customer name
    }

    /// <summary>
    /// GOOD: Creates invoices - single responsibility
    /// </summary>
    public class InvoiceCreator
    {
        public Invoice Create()
        {
            var invoice = new Invoice { Id = 123, Amount = 100.00m, Customer = "Acme" };
            Console.WriteLine("   Created invoice");
            return invoice;
        }
    }

    /// <summary>
    /// GOOD: Prints invoices - single responsibility
    /// </summary>
    public class InvoicePrinter
    {
        public void Print(Invoice invoice)
        {
            Console.WriteLine("   Printed invoice");
        }
    }

    /// <summary>
    /// GOOD: Saves invoices - single responsibility
    /// </summary>
    public class InvoiceRepository
    {
        public void Save(Invoice invoice)
        {
            Console.WriteLine("   Saved to database");
        }
    }

    /// <summary>
    /// GOOD: Emails invoices - single responsibility
    /// </summary>
    public class InvoiceEmailer
    {
        public void Send(Invoice invoice)
        {
            Console.WriteLine("   Sent email");
        }
    }

    /// <summary>
    /// User validator - single responsibility
    /// </summary>
    public class UserValidator
    {
        public bool Validate(string email)
        {
            Console.WriteLine("   Valid email format");
            return email.Contains("@");
        }
    }

    /// <summary>
    /// User repository - single responsibility
    /// </summary>
    public class UserRepository
    {
        public void Save(string username, string email)
        {
            Console.WriteLine("   User saved to database");
        }
    }

    /// <summary>
    /// User authenticator - single responsibility
    /// </summary>
    public class UserAuthenticator
    {
        public bool Authenticate(string username, string password)
        {
            Console.WriteLine("   User authenticated");
            return true;
        }
    }
}