/*
 * ============================================================
 * TOPIC     : Design Patterns
 * SUBTOPIC  : Behavioral - Real-World Part 3
 * FILE      : 19_Behavioral_RealWorld_Part3.cs
 * PURPOSE   : Additional real-world Behavioral pattern examples
 * ============================================================
 */
using System;

namespace CSharp_MasterGuide._11_DesignPatterns._03_Behavioral._08_RealWorld
{
    /// <summary>
    /// Additional real-world Behavioral patterns
    /// </summary>
    public class BehavioralRealWorldPart3
    {
        /// <summary>
        /// Entry point
        /// </summary>
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Behavioral Patterns Real-World Part 3 ===\n");

            // Example: Template Method - Data Export
            Console.WriteLine("1. Template Method - Data Export:");
            var csvExporter = new CsvExporter();
            csvExporter.Export();
            
            var jsonExporter = new JsonExporter();
            jsonExporter.Export();
            
            // Output: CSV exported, JSON exported

            // Example: Visitor - Report Generation
            Console.WriteLine("\n2. Visitor - Report Generation:");
            var reportVisitor = new ReportVisitor();
            var employee = new Employee("John", 5000);
            employee.Accept(reportVisitor);
            
            // Output: Employee: John, Salary: $5000

            // Example: Memento - Game Save
            Console.WriteLine("\n3. Memento - Game Save:");
            var game = new Game();
            game.Play("Level 1");
            var savePoint = game.Save();
            game.Play("Level 2");
            game.Load(savePoint);
            
            // Output: Loaded: Level 1

            Console.WriteLine("\n=== Behavioral Real-World Part 3 Complete ===");
        }
    }

    // Template Method - Export
    public abstract class DataExporter
    {
        public void Export()
        {
            var data = FetchData();
            var formatted = FormatData(data);
            WriteToFile(formatted);
        }
        
        protected abstract string FetchData();
        protected abstract string FormatData(string data);
        protected abstract void WriteToFile(string data);
    }

    public class CsvExporter : DataExporter
    {
        protected override string FetchData() => "Data";
        protected override string FormatData(string d) => "csv,data";
        protected override void WriteToFile(string d) => Console.WriteLine("   CSV exported");
    }

    public class JsonExporter : DataExporter
    {
        protected override string FetchData() => "Data";
        protected override string FormatData(string d) => "{\"data\"}";
        protected override void WriteToFile(string d) => Console.WriteLine("   JSON exported");
    }

    // Visitor - Report
    public interface IVisitor
    {
        void VisitEmployee(Employee employee);
    }

    public class Employee
    {
        public string Name { get; }
        public decimal Salary { get; }
        
        public Employee(string name, decimal salary)
        {
            Name = name;
            Salary = salary;
        }
        
        public void Accept(IVisitor visitor) => visitor.VisitEmployee(this);
    }

    public class ReportVisitor : IVisitor
    {
        public void VisitEmployee(Employee employee) => 
            Console.WriteLine($"   Employee: {employee.Name}, Salary: ${employee.Salary}");
    }

    // Memento - Game
    public class GameMemento
    {
        public string Level { get; }
        
        public GameMemento(string level) => Level = level;
    }

    public class Game
    {
        private string _currentLevel = "Start";
        
        public void Play(string level)
        {
            _currentLevel = level;
            Console.WriteLine($"   Playing: {level}");
        }
        
        public GameMemento Save() => new GameMemento(_currentLevel);
        
        public void Load(GameMemento memento)
        {
            _currentLevel = memento.Level;
            Console.WriteLine($"   Loaded: {_currentLevel}");
        }
    }
}