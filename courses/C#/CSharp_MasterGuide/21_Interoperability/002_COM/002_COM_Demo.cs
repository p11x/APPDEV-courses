/*
 * ============================================================
 * TOPIC     : Interoperability
 * SUBTOPIC  : COM Interop
 * FILE      : COM_Demo.cs
 * PURPOSE   : Using COM components from C#
 * ============================================================
 */
using System; // Core System namespace
using System.Runtime.InteropServices; // COM interop

namespace CSharp_MasterGuide._21_Interoperability._01_COM
{
    /// <summary>
    /// COM interop demonstration
    /// </summary>
    public class COMDemo
    {
        /// <summary>
        /// Entry point
        /// </summary>
        public static void Main(string[] args)
        {
            Console.WriteLine("=== COM Interop ===\n");

            // Output: --- Early Binding ---
            Console.WriteLine("--- Early Binding ---");

            var excel = new ExcelApplication();
            excel.Visible = true;
            Console.WriteLine("   Excel visible");
            // Output: Excel visible

            // Output: --- Late Binding ---
            Console.WriteLine("\n--- Late Binding ---");

            var workbook = Activator.CreateInstance(
                Type.GetTypeFromProgID("Excel.Application"));
            Console.WriteLine("   Excel created");
            // Output: Excel created

            // Output: --- COM Interop Attributes ---
            Console.WriteLine("\n--- Interop Attributes ---");

            Console.WriteLine("   [ComImport]");
            Console.WriteLine("   [Guid()]");
            Console.WriteLine("   [InterfaceType()]");
            // Output: COM attributes

            // Output: --- Type Mismatch ---
            Console.WriteLine("\n--- Type Mismatch ---");

            var convert = new VariantConverter();
            var variant = convert.ToVariant("string");
            Console.WriteLine($"   Variant: {variant}");
            // Output: Variant: string

            Console.WriteLine("\n=== COM Interop Complete ===");
        }
    }

    /// <summary>
    /// Excel application interface
    /// </summary>
    [ComImport]
    [Guid("00024500-0000-0000-C000-000000000046")]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface ExcelApplication
    {
        bool Visible { get; set; } // property: visible
    }

    /// <summary>
    /// Variant converter
    /// </summary>
    public class VariantConverter
    {
        public object ToVariant(string value) => value;
    }
}