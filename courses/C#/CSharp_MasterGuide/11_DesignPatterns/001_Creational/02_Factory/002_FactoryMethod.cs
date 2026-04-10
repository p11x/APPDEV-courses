/*
 * ============================================================
 * TOPIC     : Design Patterns
 * SUBTOPIC  : Creational - Factory Method Pattern
 * FILE      : 01_FactoryMethod.cs
 * PURPOSE   : Demonstrates Factory Method design pattern in C#
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._11_DesignPatterns._01_Creational._02_Factory
{
    /// <summary>
    /// Demonstrates Factory Method pattern
    /// </summary>
    public class FactoryMethod
    {
        /// <summary>
        /// Entry point for Factory Method examples
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs to console
            // Output: === Factory Method Pattern ===
            Console.WriteLine("=== Factory Method Pattern ===\n");

            // ── CONCEPT: What is Factory Method? ──────────────────────────────
            // Defines interface for creating objects, lets subclasses decide

            // Example 1: Basic Factory Method
            // Output: 1. Basic Factory Method:
            Console.WriteLine("1. Basic Factory Method:");
            
            // Use factory to create products
            var creator = new ConcreteCreator();
            
            // Factory method returns Product instance
            var product = creator.FactoryMethod();
            // Output: Created: ConcreteProduct
            Console.WriteLine($"   Created: {product.GetType().Name}");

            // ── CONCEPT: Parameterized Factory ────────────────────────────────
            // Factory accepts parameters to determine object type

            // Example 2: Parameterized Factory
            // Output: 2. Parameterized Factory:
            Console.WriteLine("\n2. Parameterized Factory:");
            
            // Create different types based on parameter
            var pdfDoc = DocumentFactory.CreateDocument("PDF");
            var wordDoc = DocumentFactory.CreateDocument("Word");
            var excelDoc = DocumentFactory.CreateDocument("Excel");
            
            // Output: PDF Document created
            // Output: Word Document created
            // Output: Excel Document created
            Console.WriteLine($"   {pdfDoc.GetType().Name} created");
            Console.WriteLine($"   {wordDoc.GetType().Name} created");
            Console.WriteLine($"   {excelDoc.GetType().Name} created");

            // ── CONCEPT: Generic Factory ─────────────────────────────────────
            // Type-safe factory using generics

            // Example 3: Generic Factory
            // Output: 3. Generic Factory:
            Console.WriteLine("\n3. Generic Factory:");
            
            // Create instances using generic type
            var logger = GenericFactory.Create<ConsoleLogger>();
            var reader = GenericFactory.Create<FileReader>();
            
            // Output: ConsoleLogger created
            // Output: FileReader created
            Console.WriteLine($"   {logger.GetType().Name} created");
            Console.WriteLine($"   {reader.GetType().Name} created");

            // ── REAL-WORLD EXAMPLE: Payment Processor ────────────────────────
            // Output: --- Real-World: Payment Processor ---
            Console.WriteLine("\n--- Real-World: Payment Processor ---");
            
            // Create payment method based on type
            var creditPayment = PaymentFactory.CreatePayment("CreditCard");
            var paypalPayment = PaymentFactory.CreatePayment("PayPal");
            var cryptoPayment = PaymentFactory.CreatePayment("Crypto");
            
            // Process payments
            // Output: Processing CreditCard payment of $100
            creditPayment.Process(100);
            // Output: Processing PayPal payment of $100
            paypalPayment.Process(100);
            // Output: Processing Crypto payment of $100
            cryptoPayment.Process(100);

            Console.WriteLine("\n=== Factory Method Complete ===");
        }
    }

    /// <summary>
    /// Product interface
    /// </summary>
    public interface IProduct
    {
        string Operation(); // method: returns product info
    }

    /// <summary>
    /// Concrete product implementation
    /// </summary>
    public class ConcreteProduct : IProduct
    {
        public string Operation()
        {
            return "ConcreteProduct Operation";
        }
    }

    /// <summary>
    /// Creator abstract class
    /// </summary>
    public abstract class Creator
    {
        /// <summary>
        /// Factory method - overridden by subclasses
        /// </summary>
        public abstract IProduct FactoryMethod();
    }

    /// <summary>
    /// Concrete creator
    /// </summary>
    public class ConcreteCreator : Creator
    {
        /// <summary>
        /// Returns concrete product instance
        /// </summary>
        public override IProduct FactoryMethod()
        {
            return new ConcreteProduct();
        }
    }

    /// <summary>
    /// Document interface for parameterized factory
    /// </summary>
    public interface IDocument
    {
        void Open(); // method: opens document
        void Save(); // method: saves document
    }

    /// <summary>
    /// PDF document implementation
    /// </summary>
    public class PDFDocument : IDocument
    {
        public void Open() => Console.WriteLine("   Opening PDF");
        public void Save() => Console.WriteLine("   Saving PDF");
    }

    /// <summary>
    /// Word document implementation
    /// </summary>
    public class WordDocument : IDocument
    {
        public void Open() => Console.WriteLine("   Opening Word");
        public void Save() => Console.WriteLine("   Saving Word");
    }

    /// <summary>
    /// Excel document implementation
    /// </summary>
    public class ExcelDocument : IDocument
    {
        public void Open() => Console.WriteLine("   Opening Excel");
        public void Save() => Console.WriteLine("   Saving Excel");
    }

    /// <summary>
    /// Static factory for documents
    /// </summary>
    public static class DocumentFactory
    {
        /// <summary>
        /// Creates document by type
        /// </summary>
        public static IDocument CreateDocument(string type)
        {
            return type switch
            {
                "PDF" => new PDFDocument(),
                "Word" => new WordDocument(),
                "Excel" => new ExcelDocument(),
                _ => throw new ArgumentException($"Unknown type: {type}")
            };
        }
    }

    /// <summary>
    /// Generic factory using reflection
    /// </summary>
    public static class GenericFactory
    {
        /// <summary>
        /// Creates instance of specified type
        /// </summary>
        public static T Create<T>() where T : class, new()
        {
            return new T(); // create using parameterless constructor
        }
    }

    /// <summary>
    /// Console logger implementation
    /// </summary>
    public class ConsoleLogger { }

    /// <summary>
    /// File reader implementation
    /// </summary>
    public class FileReader { }

    /// <summary>
    /// Payment method interface
    /// </summary>
    public interface IPayment
    {
        void Process(decimal amount); // method: processes payment
    }

    /// <summary>
    /// Credit card payment
    /// </summary>
    public class CreditCardPayment : IPayment
    {
        public void Process(decimal amount)
        {
            // Output: Processing CreditCard payment of $amount
            Console.WriteLine($"   Processing CreditCard payment of ${amount}");
        }
    }

    /// <summary>
    /// PayPal payment
    /// </summary>
    public class PayPalPayment : IPayment
    {
        public void Process(decimal amount)
        {
            // Output: Processing PayPal payment of $amount
            Console.WriteLine($"   Processing PayPal payment of ${amount}");
        }
    }

    /// <summary>
    /// Crypto payment
    /// </summary>
    public class CryptoPayment : IPayment
    {
        public void Process(decimal amount)
        {
            // Output: Processing Crypto payment of $amount
            Console.WriteLine($"   Processing Crypto payment of ${amount}");
        }
    }

    /// <summary>
    /// Payment factory
    /// </summary>
    public static class PaymentFactory
    {
        /// <summary>
        /// Creates payment method
        /// </summary>
        public static IPayment CreatePayment(string type)
        {
            return type switch
            {
                "CreditCard" => new CreditCardPayment(),
                "PayPal" => new PayPalPayment(),
                "Crypto" => new CryptoPayment(),
                _ => throw new ArgumentException($"Unknown payment: {type}")
            };
        }
    }
}