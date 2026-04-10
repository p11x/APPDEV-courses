/*
 * ============================================================
 * TOPIC     : SOLID Principles
 * SUBTOPIC  : Interface Segregation Principle - Concept
 * FILE      : 01_ISP_Concept.cs
 * PURPOSE   : Demonstrates ISP - prefer many small interfaces over one large
 * ============================================================
 */
using System; // Core System namespace for Console

namespace CSharp_MasterGuide._12_SOLID_Principles._04_InterfaceSegregation._01_ISP_Concept
{
    /// <summary>
    /// Demonstrates Interface Segregation Principle concept
    /// </summary>
    public class ISPConceptDemo
    {
        /// <summary>
        /// Entry point for ISP concept examples
        /// </summary>
        public static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════
            // SECTION 1: Concept Introduction
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("=== Interface Segregation Principle ===\n");

            // Output: --- Concept: What is ISP? ---
            Console.WriteLine("--- Concept: What is ISP? ---");

            // Clients should not be forced to depend on methods they don't use
            // Prefer small, focused interfaces over fat ones

            Console.WriteLine("   Many small interfaces over one large");
            // Output: Many small interfaces over one large

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: Violating ISP
            // ═══════════════════════════════════════════════════════════════════
            
            // Output: --- Violating ISP ---
            Console.WriteLine("\n--- Violating ISP ---");

            // Fat interface forces implementation of unused methods
            var printer = new MultiFunctionPrinter();
            printer.Print();
            // Output: Printing document
            printer.Scan();
            // Output: Scanning document
            printer.Fax();
            // Output: Sending fax

            // Even when we only need print!
            // Output: But what if we only want to print?
            Console.WriteLine("   But what if we only want to print?");

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: Following ISP
            // ═══════════════════════════════════════════════════════════
            
            // Output: --- Following ISP ---
            Console.WriteLine("\n--- Following ISP ---");

            // Use small, focused interfaces
            IPrinter simplePrinter = new SimplePrinter();
            simplePrinter.Print();
            // Output: Printing document

            IScanner simpleScanner = new SimpleScanner();
            simpleScanner.Scan();
            // Output: Scanning document

            IFax simpleFax = new SimpleFax();
            simpleFax.Fax();
            // Output: Sending fax

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: Combining Interfaces
            // ═══════════════════════════════════════════════════════════
            
            // Output: --- Combining Interfaces ---
            Console.WriteLine("\n--- Combining Interfaces ---");

            // Use role interfaces together
            var allInOne = new AllInOneDevice();
            allInOne.Print();
            allInOne.Scan();
            allInOne.Fax();
            // Output: Printing document
            // Output: Scanning document
            // Output: Sending fax

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: Dependency Injection
            // ═══════════════════════════════════════════════════════════
            
            // Output: --- Dependency Injection ---
            Console.WriteLine("\n--- Dependency Injection ---");

            // Inject only what we need
            var printJob = new PrintJobService(simplePrinter);
            printJob.Execute("Document");
            // Output: Starting job
            // Output: Printing Document
            // Output: Job complete

            Console.WriteLine("\n=== ISP Concept Complete ===");
        }
    }

    /// <summary>
    /// BAD: Fat interface with unrelated methods
    /// </summary>
    public interface IMultiFunctionInterface
    {
        void Print(); // method: print
        void Scan(); // method: scan  
        void Fax(); // method: fax
        void Copy(); // method: copy
    }

    /// <summary>
    /// BAD: Implements unused methods
    /// </summary>
    public class MultiFunctionPrinter : IMultiFunctionInterface
    {
        public void Print() => Console.WriteLine("   Printing document");
        public void Scan() => Console.WriteLine("   Scanning document");
        public void Fax() => Console.WriteLine("   Sending fax");
        public void Copy() => Console.WriteLine("   Copying document");
    }

    /// <summary>
    /// GOOD: Small focused interfaces
    /// </summary>
    public interface IPrinter
    {
        void Print(); // method: print only
    }

    /// <summary>
    /// GOOD: Single responsibility interface
    /// </summary>
    public interface IScanner
    {
        void Scan(); // method: scan only
    }

    /// <summary>
    /// GOOD: Single responsibility interface
    /// </summary>
    public interface IFax
    {
        void Fax(); // method: fax only
    }

    /// <summary>
    /// GOOD: Implements only what it needs
    /// </summary>
    public class SimplePrinter : IPrinter
    {
        public void Print() => Console.WriteLine("   Printing document");
    }

    /// <summary>
    /// GOOD: Implements only what it needs
    /// </summary>
    public class SimpleScanner : IScanner
    {
        public void Scan() => Console.WriteLine("   Scanning document");
    }

    /// <summary>
    /// GOOD: Implements only what it needs
    /// </summary>
    public class SimpleFax : IFax
    {
        public void Fax() => Console.WriteLine("   Sending fax");
    }

    /// <summary>
    /// GOOD: Combines multiple small interfaces
    /// </summary>
    public class AllInOneDevice : IPrinter, IScanner, IFax
    {
        public void Print() => Console.WriteLine("   Printing document");
        public void Scan() => Console.WriteLine("   Scanning document");
        public void Fax() => Console.WriteLine("   Sending fax");
    }

    /// <summary>
    /// Service depending on small interface
    /// </summary>
    public class PrintJobService
    {
        private readonly IPrinter _printer; // field: printer

        public PrintJobService(IPrinter printer)
        {
            _printer = printer;
        }

        public void Execute(string document)
        {
            Console.WriteLine("   Starting job");
            _printer.Print();
            Console.WriteLine("   Job complete");
        }
    }
}