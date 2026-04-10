/*
 * ============================================================
 * TOPIC     : SOLID Principles
 * SUBTOPIC  : SOLID Combined - Real-World Part 2
 * FILE      : 04_SOLID_RealWorld_Part2.cs
 * PURPOSE   : Extended real-world SOLID examples
 * ============================================================
 */
using System; // Core System namespace for Console

namespace CSharp_MasterGuide._12_SOLID_Principles._06_SOLID_Combined._04_SOLID_RealWorld_Part2
{
    /// <summary>
    /// Extended real-world SOLID demonstration
    /// </summary>
    public class SOLIDRealWorldPart2Demo
    {
        /// <summary>
        /// Entry point
        /// </summary>
        public static void Main(string[] args)
        {
            Console.WriteLine("=== SOLID Real-World Part 2 ===\n");

            // Output: --- Reporting System ---
            Console.WriteLine("--- Reporting System ---");

            var reportExporter = new ReportExporter(
                new PdfExportStrategy(),
                new FileSystemWriter());

            reportExporter.Export("Report", "data");
            // Output: PDF export: Report
            // Output: File written

            Console.WriteLine("\n=== Part 2 Complete ===");
        }
    }

    // SRP: Export strategy - single responsibility
    public interface IExportStrategy
    {
        byte[] Export(string data); // method: export data
    }

    public class PdfExportStrategy : IExportStrategy
    {
        public byte[] Export(string data)
        {
            Console.WriteLine($"   PDF export: {data}");
            return new byte[0];
        }
    }

    public class ExcelExportStrategy : IExportStrategy
    {
        public byte[] Export(string data)
        {
            Console.WriteLine($"   Excel export: {data}");
            return new byte[0];
        }
    }

    // SRP: Writer - single responsibility
    public interface IWriter
    {
        void Write(byte[] data); // method: write data
    }

    public class FileSystemWriter : IWriter
    {
        public void Write(byte[] data) => Console.WriteLine("   File written");
    }

    // DIP: Report exporter depends on abstractions
    public class ReportExporter
    {
        private readonly IExportStrategy _strategy; // field: strategy
        private readonly IWriter _writer; // field: writer

        public ReportExporter(IExportStrategy strategy, IWriter writer)
        {
            _strategy = strategy;
            _writer = writer;
        }

        public void Export(string reportName, string data)
        {
            var bytes = _strategy.Export(data);
            _writer.Write(bytes);
        }
    }
}