/*
 * ============================================================
 * TOPIC     : SOLID Principles
 * SUBTOPIC  : Liskov Substitution Principle - Part 2
 * FILE      : 02_LSP_Part2.cs
 * PURPOSE   : Advanced LSP examples with real-world patterns
 * ============================================================
 */
using System; // Core System namespace for Console

namespace CSharp_MasterGuide._12_SOLID_Principles._03_LiskovSubstitution._02_LSP_Part2
{
    /// <summary>
    /// Demonstrates LSP advanced examples
    /// </summary>
    public class LSPPart2Demo
    {
        /// <summary>
        /// Entry point for LSP Part 2 examples
        /// </summary>
        public static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════
            // SECTION 1: Function Parameters
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("=== LSP Part 2 ===\n");

            // Output: --- Function Parameters ---
            Console.WriteLine("--- Function Parameters ---");

            // Use abstract base for parameters
            ProcessEmployees(new List<IEmployee>
            {
                new Developer { Name = "Alice", Salary = 1000 },
                new Manager { Name = "Bob", Salary = 1500 }
            });
            // Output: Developer: Alice earns $1,000
            // Output: Manager: Bob earns $1,500

            // ═══════════════════════════════════════════════════════════════════
            // SECTION 2: Return Types
            // ═══════════════════════════════════════════════════════════
            
            // Output: --- Return Types ---
            Console.WriteLine("\n--- Return Types ---");

            // Factory returns base type - caller doesn't know concrete
            var factory = new EmployeeFactory();
            IEmployee emp = factory.Create("developer");
            Console.WriteLine($"   Created: {emp.GetType().Name}");
            // Output: Created: Developer

            emp = factory.Create("manager");
            Console.WriteLine($"   Created: {emp.GetType().Name}");
            // Output: Created: Manager

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: Collection Covariance
            // ═══════════════════════════════════════════════════════════
            
            // Output: --- Collection Covariance ---
            Console.WriteLine("\n--- Collection Covariance ---");

            // IEnumerable<T> is covariant in T
            IReadOnlyList<Manager> managers = new List<Manager>
            {
                new Manager { Name = "Bob" },
                new Manager { Name = "Carol" }
            };

            // Can assign to IReadOnlyList of base type
            IReadOnlyList<IEmployee> employees = managers;
            Console.WriteLine($"   Count: {employees.Count}");
            // Output: Count: 2

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: Null Object Pattern
            // ═══════════════════════════════════════════════════════════
            
            // Output: --- Null Object Pattern ---
            Console.WriteLine("\n--- Null Object Pattern ---");

            // Null object acts like real object
            IEmployee nullEmployee = new NullEmployee();
            nullEmployee.Work(); // no crash!
            // Output: (does nothing)
            Console.WriteLine("   Null employee handled gracefully");
            // Output: Null employee handled gracefully

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: Template Method
            // ═══════════════════════════════════════════════════════════
            
            // Output: --- Template Method ---
            Console.WriteLine("\n--- Template Method ---");

            // Base class defines algorithm skeleton
            var webProcess = new WebDataProcessor();
            webProcess.ProcessOnline();
            // Output: Connecting
            // Output: Requesting data
            // Output: Web: Got data
            // Output: Disconnecting

            var fileProcess = new FileDataProcessor();
            fileProcess.ProcessOnline();
            // Output: Opening file
            // Output: Reading data
            // Output: File: Got data
            // Output: Closing file

            Console.WriteLine("\n=== LSP Part 2 Complete ===");
        }
    }

    /// <summary>
    /// Employee abstraction for LSP
    /// </summary>
    public interface IEmployee
    {
        string Name { get; set; } // property: name
        decimal Salary { get; set; } // property: salary

        void Work(); // method: does work
        void GetPaid(); // method: gets paid
    }

    /// <summary>
    /// Developer implementation
    /// </summary>
    public class Developer : IEmployee
    {
        public string Name { get; set; } // property: name
        public decimal Salary { get; set; } // property: salary

        public void Work() => Console.WriteLine($"   Developer: {Name} is coding");
        public void GetPaid() => Console.WriteLine($"   Developer: {Name} earns {Salary:C}");
    }

    /// <summary>
    /// Manager implementation
    /// </summary>
    public class Manager : IEmployee
    {
        public string Name { get; set; } // property: name
        public decimal Salary { get; set; } // property: salary

        public void Work() => Console.WriteLine($"   Manager: {Name} is managing");
        public void GetPaid() => Console.WriteLine($"   Manager: {Name} earns {Salary:C}");
    }

    /// <summary>
    /// Process employees - works with any IEmployee
    /// </summary>
    public static void ProcessEmployees(System.Collections.Generic.List<IEmployee> employees)
    {
        foreach (var emp in employees)
        {
            emp.GetPaid();
        }
    }

    /// <summary>
    /// Employee factory - returns base type
    /// </summary>
    public class EmployeeFactory
    {
        public IEmployee Create(string type)
        {
            return type switch
            {
                "developer" => new Developer { Name = "New Dev" },
                "manager" => new Manager { Name = "New Manager" },
                _ => new NullEmployee()
            };
        }
    }

    /// <summary>
    /// Null employee - follows LSP
    /// </summary>
    public class NullEmployee : IEmployee
    {
        public string Name { get; set; } = "None"; // property: name
        public decimal Salary { get; set; } = 0; // property: salary

        public void Work() { } // does nothing - graceful
        public void GetPaid() { } // does nothing - graceful
    }

    /// <summary>
    /// Data processor abstraction
    /// </summary>
    public abstract class DataProcessor
    {
        // Template method - defines algorithm
        public void ProcessOnline()
        {
            Connect();
            Request();
            DisplayData();
            Disconnect();
        }

        protected abstract void Connect(); // method: connect
        protected abstract void Request(); // method: request data
        protected abstract void DisplayData(); // method: display data

        protected void Disconnect() => Console.WriteLine("   Disconnecting"); // method: disconnect
    }

    /// <summary>
    /// Web processor - can substitute DataProcessor
    /// </summary>
    public class WebDataProcessor : DataProcessor
    {
        protected override void Connect() => Console.WriteLine("   Connecting");
        protected override void Request() => Console.WriteLine("   Requesting data");
        protected override void DisplayData() => Console.WriteLine("   Web: Got data");
    }

    /// <summary>
    /// File processor - can substitute DataProcessor
    /// </summary>
    public class FileDataProcessor : DataProcessor
    {
        protected override void Connect() => Console.WriteLine("   Opening file");
        protected override void Request() => Console.WriteLine("   Reading data");
        protected override void DisplayData() => Console.WriteLine("   File: Got data");
    }
}