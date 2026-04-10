/*
 * ============================================================
 * TOPIC     : Design Patterns
 * SUBTOPIC  : Behavioral - Template Method Pattern
 * FILE      : 01_TemplateMethod.cs
 * PURPOSE   : Demonstrates Template Method design pattern in C#
 * ============================================================
 */
using System;

namespace CSharp_MasterGuide._11_DesignPatterns._03_Behavioral._05_TemplateMethod
{
    /// <summary>
    /// Demonstrates Template Method pattern - algorithm skeleton
    /// </summary>
    public class TemplateMethodPattern
    {
        /// <summary>
        /// Entry point for Template Method examples
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs to console
            // Output: === Template Method Pattern Demo ===
            Console.WriteLine("=== Template Method Pattern Demo ===\n");

            // Example: Data Processing
            Console.WriteLine("1. Data Processing:");
            var csvProcessor = new CsvDataProcessor();
            csvProcessor.Process();
            
            // Output: Read CSV, Process data, Save results

            Console.WriteLine("\n2. JSON Processing:");
            var jsonProcessor = new JsonDataProcessor();
            jsonProcessor.Process();
            
            // Output: Read JSON, Process data, Save results

            Console.WriteLine("\n=== Template Method Complete ===");
        }
    }

    /// <summary>
    /// Abstract template - defines algorithm skeleton
    /// </summary>
    public abstract class DataProcessor
    {
        /// <summary>
        /// Template method - defines algorithm structure
        /// </summary>
        public void Process()
        {
            var data = ReadData();
            var processed = ProcessData(data);
            SaveResults(processed);
        }
        
        /// <summary>
        /// Step 1: Read data (override in subclasses)
        /// </summary>
        protected abstract string ReadData();
        
        /// <summary>
        /// Step 2: Process data (override in subclasses)
        /// </summary>
        protected abstract string ProcessData(string data);
        
        /// <summary>
        /// Step 3: Save results (override in subclasses)
        /// </summary>
        protected abstract void SaveResults(string data);
    }

    /// <summary>
    /// CSV processor - concrete implementation
    /// </summary>
    public class CsvDataProcessor : DataProcessor
    {
        protected override string ReadData() => "   Read CSV data";
        
        protected override string ProcessData(string data) => "   Process CSV data";
        
        protected override void SaveResults(string data) => Console.WriteLine("   Save CSV results");
    }

    /// <summary>
    /// JSON processor - concrete implementation
    /// </summary>
    public class JsonDataProcessor : DataProcessor
    {
        protected override string ReadData() => "   Read JSON data";
        
        protected override string ProcessData(string data) => "   Process JSON data";
        
        protected override void SaveResults(string data) => Console.WriteLine("   Save JSON results");
    }
}