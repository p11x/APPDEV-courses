/*
 * ============================================================
 * TOPIC     : Dependency Injection
 * SUBTOPIC  : DI Basics - Method Injection
 * FILE      : 03_MethodInjection.cs
 * PURPOSE   : Method injection pattern in C#
 * ============================================================
 */
using System;

namespace CSharp_MasterGuide._13_DependencyInjection._01_DI_Basics
{
    /// <summary>
    /// Demonstrates method injection
    /// </summary>
    public class MethodInjection
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Method Injection ===\n");

            var service = new ReportService();
            service.GenerateReport(new PdfGenerator());
            service.GenerateReport(new HtmlGenerator());

            Console.WriteLine("\n=== Method Injection Complete ===");
        }
    }

    public interface IReportGenerator { string Generate(); }
    public class PdfGenerator : IReportGenerator { public string Generate() => "PDF Report"; }
    public class HtmlGenerator : IReportGenerator { public string Generate() => "HTML Report"; }

    public class ReportService
    {
        // Method injection - dependency passed to method
        public void GenerateReport(IReportGenerator generator)
        {
            var report = generator.Generate();
            Console.WriteLine($"   {report}");
        }
    }
}