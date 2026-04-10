/*
 * ============================================================
 * TOPIC     : Design Patterns
 * SUBTOPIC  : Creational - Factory Method Pattern
 * FILE      : 01_Factory.cs
 * PURPOSE   : Demonstrates Factory Method design pattern in C#
 * ============================================================
 */
using System;

namespace CSharp_MasterGuide._11_DesignPatterns._01_Creational._02_Factory
{
    /// <summary>
    /// Demonstrates Factory Method pattern - creation via inheritance
    /// </summary>
    public class FactoryMethodPattern
    {
        /// <summary>
        /// Entry point for Factory Method examples
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs to console
            // Output: === Factory Method Pattern Demo ===
            Console.WriteLine("=== Factory Method Pattern Demo ===\n");

            // ── CONCEPT: Factory Method Basics ───────────────────────────────────
            // Define interface for creating objects, let subclasses decide

            // Example 1: Basic Factory Method
            // Output: 1. Basic Factory Method:
            Console.WriteLine("1. Basic Factory Method:");
            
            // Create products via factory
            var documentFactory = new ReportDocumentFactory();
            var report = documentFactory.CreateDocument();
            
            // Output: Created: Report Document
            Console.WriteLine($"   Created: {report.Name}");
            
            // Render method defined in product
            // Output: Rendered as: PDF Format
            Console.WriteLine($"   Rendered as: {report.Render()}");

            // Example 2: Different Product Types
            // Output: 2. Different Product Types:
            Console.WriteLine("\n2. Different Product Types:");
            
            // Invoice factory creates invoices
            var invoiceFactory = new InvoiceDocumentFactory();
            var invoice = invoiceFactory.CreateDocument();
            
            // Output: Created: Invoice Document
            Console.WriteLine($"   Created: {invoice.Name}");
            
            // Output: Rendered as: Excel Format
            Console.WriteLine($"   Rendered as: {invoice.Render()}");

            // ── CONCEPT: Factory with Parameters ──────────────────────────────────
            // Pass parameters to customize product creation

            // Example 3: Parameterized Factory
            // Output: 3. Parameterized Factory:
            Console.WriteLine("\n3. Parameterized Factory:");
            
            // Create different document types via same factory
            var genericFactory = new DocumentFactory();
            
            var pdf = genericFactory.CreateDocument("PDF");
            var word = genericFactory.CreateDocument("Word");
            var excel = genericFactory.CreateDocument("Excel");
            
            // Output: PDF: PDF Document, Word: Word Document, Excel: Excel Document
            Console.WriteLine($"   PDF: {pdf.Name}, Word: {word.Name}, Excel: {excel.Name}");

            // ── REAL-WORLD EXAMPLE: Payment Processor ───────────────────────────
            // Output: --- Real-World: Payment Processor ---
            Console.WriteLine("\n--- Real-World: Payment Processor ---");
            
            // Process different payment types
            var paymentFactory = new PaymentProcessorFactory();
            
            // Credit card payment
            var creditCard = paymentFactory.CreatePayment("CreditCard");
            // Process method returns status
            // Output: Credit Card: Processed - $100.00
            Console.WriteLine($"   Credit Card: {creditCard.Process(100.00m)}");
            
            // PayPal payment
            var paypal = paymentFactory.CreatePayment("PayPal");
            // Output: PayPal: Processed - $50.00
            Console.WriteLine($"   PayPal: {paypal.Process(50.00m)}");
            
            // Crypto payment
            var crypto = paymentFactory.CreatePayment("Crypto");
            // Output: Crypto: Processed - 0.005 BTC
            Console.WriteLine($"   Crypto: {crypto.Process(0.005m)}");

            Console.WriteLine("\n=== Factory Method Pattern Complete ===");
        }
    }

    /// <summary>
    /// Product interface - defines common operations
    /// </summary>
    public interface IDocument
    {
        // Product name property
        string Name { get; }
        
        /// <summary>
        /// Renders document to specific format
        /// </summary>
        /// <returns>Format string</returns>
        string Render();
    }

    /// <summary>
    /// Concrete Product - Report Document
    /// </summary>
    public class ReportDocument : IDocument
    {
        // Product name
        public string Name => "Report Document";
        
        /// <summary>
        /// Renders as PDF
        /// </summary>
        /// <returns>Format name</returns>
        public string Render() => "PDF Format";
    }

    /// <summary>
    /// Concrete Product - Invoice Document
    /// </summary>
    public class InvoiceDocument : IDocument
    {
        // Product name
        public string Name => "Invoice Document";
        
        /// <summary>
        /// Renders as Excel
        /// </summary>
        /// <returns>Format name</returns>
        public string Render() => "Excel Format";
    }

    /// <summary>
    /// Creator abstract class - declares factory method
    /// </summary>
    public abstract class DocumentFactory
    {
        /// <summary>
        /// Factory method - subclasses override to create products
        /// </summary>
        /// <returns>IDocument product</returns>
        public abstract IDocument CreateDocument();
    }

    /// <summary>
    /// Concrete Creator - creates Report documents
    /// </summary>
    public class ReportDocumentFactory : DocumentFactory
    {
        /// <summary>
        /// Factory method implementation - returns Report document
        /// </summary>
        /// <returns>ReportDocument instance</returns>
        public override IDocument CreateDocument() => new ReportDocument();
    }

    /// <summary>
    /// Concrete Creator - creates Invoice documents
    /// </summary>
    public class InvoiceDocumentFactory : DocumentFactory
    {
        /// <summary>
        /// Factory method implementation - returns Invoice document
        /// </summary>
        /// <returns>InvoiceDocument instance</returns>
        public override IDocument CreateDocument() => new InvoiceDocument();
    }

    /// <summary>
    /// Generic factory with parameter support
    /// </summary>
    public class DocumentFactory
    {
        /// <summary>
        /// Creates document based on type parameter
        /// </summary>
        /// <param name="documentType">Type: PDF, Word, Excel</param>
        /// <returns>IDocument instance</returns>
        public IDocument CreateDocument(string documentType)
        {
            // Switch on type to return appropriate product
            return documentType switch
            {
                "PDF" => new PdfDocument(),
                "Word" => new WordDocument(),
                "Excel" => new ExcelDocument(),
                _ => throw new ArgumentException($"Unknown type: {documentType}")
            };
        }
    }

    /// <summary>
    /// PDF Document product
    /// </summary>
    public class PdfDocument : IDocument
    {
        public string Name => "PDF Document";
        public string Render() => "PDF Format";
    }

    /// <summary>
    /// Word Document product
    /// </summary>
    public class WordDocument : IDocument
    {
        public string Name => "Word Document";
        public string Render() => "Word Format";
    }

    /// <summary>
    /// Excel Document product
    /// </summary>
    public class ExcelDocument : IDocument
    {
        public string Name => "Excel Document";
        public string Render() => "Excel Format";
    }

    /// <summary>
    /// Payment product interface
    /// </summary>
    public interface IPayment
    {
        /// <summary>
        /// Processes payment with amount
        /// </summary>
        /// <param name="amount">Payment amount</param>
        /// <returns>Status message</returns>
        string Process(decimal amount);
    }

    /// <summary>
    /// Credit Card payment
    /// </summary>
    public class CreditCardPayment : IPayment
    {
        /// <summary>
        /// Processes credit card payment
        /// </summary>
        /// <param name="amount">Payment amount</param>
        /// <returns>Processed status</returns>
        public string Process(decimal amount) => $"Processed - ${amount:F2}";
    }

    /// <summary>
    /// PayPal payment
    /// </summary>
    public class PayPalPayment : IPayment
    {
        /// <summary>
        /// Processes PayPal payment
        /// </summary>
        /// <param name="amount">Payment amount</param>
        /// <returns>Processed status</returns>
        public string Process(decimal amount) => $"Processed - ${amount:F2}";
    }

    /// <summary>
    /// Crypto payment
    /// </summary>
    public class CryptoPayment : IPayment
    {
        /// <summary>
        /// Processes cryptocurrency payment
        /// </summary>
        /// <param name="amount">Payment amount in BTC</param>
        /// <returns>Processed status</returns>
        public string Process(decimal amount) => $"Processed - {amount:F6} BTC";
    }

    /// <summary>
    /// Payment processor factory
    /// </summary>
    public class PaymentProcessorFactory
    {
        /// <summary>
        /// Creates payment processor by type
        /// </summary>
        /// <param name="paymentType">Type: CreditCard, PayPal, Crypto</param>
        /// <returns>IPayment instance</returns>
        public IPayment CreatePayment(string paymentType)
        {
            return paymentType switch
            {
                "CreditCard" => new CreditCardPayment(),
                "PayPal" => new PayPalPayment(),
                "Crypto" => new CryptoPayment(),
                _ => throw new ArgumentException($"Unknown payment: {paymentType}")
            };
        }
    }
}