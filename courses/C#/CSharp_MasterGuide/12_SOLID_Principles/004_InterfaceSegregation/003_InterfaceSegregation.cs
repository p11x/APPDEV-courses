/*
 * ============================================================
 * TOPIC     : SOLID Principles
 * SUBTOPIC  : Interface Segregation Principle
 * FILE      : 04_InterfaceSegregation.cs
 * PURPOSE   : Demonstrates ISP - prefer small, focused interfaces
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._12_SOLID_Principles._04_InterfaceSegregation
{
    /// <summary>
    /// Demonstrates Interface Segregation Principle
    /// </summary>
    public class InterfaceSegregationDemo
    {
        /// <summary>
        /// Entry point for ISP examples
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs to console
            // Output: === Interface Segregation Principle ===
            Console.WriteLine("=== Interface Segregation Principle ===\n");

            // ── CONCEPT: What is ISP? ────────────────────────────────────────
            // Prefer many small interfaces over one large one

            // Example 1: Violating ISP
            // Output: 1. Violating ISP:
            Console.WriteLine("1. Violating ISP:");
            
            // Fat interface forces implementing all methods
            var printer = new BadMultiFunctionPrinter();
            printer.Print();
            printer.Scan();
            printer.Fax();
            // Output: Printing
            // Output: Scanning
            // Output: Faxing
            
            // A simple printer still has to implement Scan and Fax
            var simplePrinter = new BadSimplePrinter();
            simplePrinter.Print();
            // Output: Printing
            // But still implements unused Scan and Fax

            // Example 2: Following ISP
            // Output: 2. Following ISP:
            Console.WriteLine("\n2. Following ISP:");
            
            // Small, focused interfaces
            var advancedPrinter = new GoodMultiFunctionPrinter();
            advancedPrinter.Print();
            advancedPrinter.Scan();
            // Output: Printing
            // Output: Scanning
            
            var simple = new GoodSimplePrinter();
            simple.Print();
            // Output: Printing

            // ── CONCEPT: Role-Based Interfaces ───────────────────────────────
            // Interfaces represent roles, not entity types

            // Example 3: Role-Based
            // Output: 3. Role-Based Interfaces:
            Console.WriteLine("\n3. Role-Based Interfaces:");
            
            // Robot can print and scan but not fax
            var robot = new RobotPrinter();
            robot.Print();
            robot.Scan();
            // Output: Robot printing
            // Output: Robot scanning

            Console.WriteLine("\n=== ISP Complete ===");
        }
    }

    /// <summary>
    /// BAD: Fat interface
    /// </summary>
    public interface IMultiFunctionDevice
    {
        void Print(); // method: prints document
        void Scan(); // method: scans document
        void Fax(); // method: sends fax
    }

    /// <summary>
    /// BAD: Multi-function printer implements all
    /// </summary>
    public class BadMultiFunctionPrinter : IMultiFunctionDevice
    {
        public void Print() => Console.WriteLine("   Printing");
        public void Scan() => Console.WriteLine("   Scanning");
        public void Fax() => Console.WriteLine("   Faxing");
    }

    /// <summary>
    /// BAD: Simple printer forced to implement unused methods
    /// </summary>
    public class BadSimplePrinter : IMultiFunctionDevice
    {
        public void Print() => Console.WriteLine("   Printing");
        public void Scan() => Console.WriteLine("   (unused) Scanning");
        public void Fax() => Console.WriteLine("   (unused) Faxing");
    }

    /// <summary>
    /// GOOD: Small interface - print
    /// </summary>
    public interface IPrinter
    {
        void Print(); // method: prints document
    }

    /// <summary>
    /// GOOD: Small interface - scan
    /// </summary>
    public interface IScanner
    {
        void Scan(); // method: scans document
    }

    /// <summary>
    /// GOOD: Small interface - fax
    /// </summary>
    public interface IFax
    {
        void Fax(); // method: sends fax
    }

    /// <summary>
    /// GOOD: Multi-function implements multiple interfaces
    /// </summary>
    public class GoodMultiFunctionPrinter : IPrinter, IScanner, IFax
    {
        public void Print() => Console.WriteLine("   Printing");
        public void Scan() => Console.WriteLine("   Scanning");
        public void Fax() => Console.WriteLine("   Faxing");
    }

    /// <summary>
    /// GOOD: Simple printer only implements needed interface
    /// </summary>
    public class GoodSimplePrinter : IPrinter
    {
        public void Print() => Console.WriteLine("   Printing");
    }

    /// <summary>
    /// Robot - implements only what it needs
    /// </summary>
    public class RobotPrinter : IPrinter, IScanner
    {
        public void Print() => Console.WriteLine("   Robot printing");
        public void Scan() => Console.WriteLine("   Robot scanning");
    }
}