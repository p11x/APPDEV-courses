/*
TOPIC: C# File I/O Operations
SUBTOPIC: StreamReader and StreamWriter
FILE: 03_StreamWriter.cs
PURPOSE: Demonstrates StreamWriter basics - Write, WriteLine methods
*/

using System;
using System.IO;
using System.Text;

namespace CSharp_MasterGuide._06_FileIO._02_StreamReaderWriter
{
    public class NN_03_StreamWriter
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== StreamWriter Basics Demo ===");
            Console.WriteLine();

            BasicWriteOperations();
            Console.WriteLine();

            WriteLineMethods();
            Console.WriteLine();

            EncodingAndAppendMode();
            Console.WriteLine();

            AutoFlushAndBuffering();
            Console.WriteLine();

            RealWorldExample_GenerateReport();
            
            CleanupDemoFiles();
        }

        private static void BasicWriteOperations()
        {
            Console.WriteLine("--- Basic Write Operations ---");
            
            string filePath = "NN_test_write_basic.txt";
            
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.Write("Hello ");
                writer.Write("World");
                writer.Write("!");
                writer.Write(123);
                writer.Write('A');
                writer.Write(true);
            }
            
            string content = File.ReadAllText(filePath);
            Console.WriteLine($"Written content: {content}");
            Console.WriteLine("// Output: Multiple Write calls append to file");
            
            File.Delete(filePath);
        }

        private static void WriteLineMethods()
        {
            Console.WriteLine("--- WriteLine Methods ---");
            
            string filePath = "NN_test_write_line.txt";
            
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("First line");
                writer.WriteLine("Second line");
                writer.WriteLine("Third line");
                
                writer.Write("No newline here");
                writer.WriteLine(" - but this adds one");
                
                writer.WriteLine();
                writer.WriteLine("Empty line above");
                
                writer.WriteLine("Formatted: {0} + {1} = {2}", 5, 3, 8);
            }
            
            string[] lines = File.ReadAllLines(filePath);
            Console.WriteLine("Written lines:");
            for (int i = 0; i < lines.Length; i++)
            {
                Console.WriteLine($"  {i + 1}: {lines[i]}");
            }
            
            File.Delete(filePath);
            Console.WriteLine("// Output: Each WriteLine adds a newline");
        }

        private static void EncodingAndAppendMode()
        {
            Console.WriteLine("--- Encoding and Append Mode ---");
            
            string utf8File = "NN_test_utf8_write.txt";
            string appendFile = "NN_test_append.txt";
            
            using (StreamWriter writer = new StreamWriter(utf8File, false, Encoding.UTF8))
            {
                writer.WriteLine("UTF-8 encoded content");
                writer.WriteLine("Special chars: àéïõü 中文");
            }
            
            Console.WriteLine($"UTF-8 file encoding: {File.ReadAllText(utf8File, Encoding.UTF8)}");
            
            File.Delete(utf8File);
            
            using (StreamWriter writer = new StreamWriter(appendFile))
            {
                writer.WriteLine("First write");
            }
            
            using (StreamWriter writer = new StreamWriter(appendFile, true))
            {
                writer.WriteLine("Second write (appended)");
                writer.WriteLine("Third write (appended)");
            }
            
            Console.WriteLine("Appended content:");
            Console.WriteLine(File.ReadAllText(appendFile));
            
            File.Delete(appendFile);
            Console.WriteLine("// Output: Append mode adds to existing file");
        }

        private static void AutoFlushAndBuffering()
        {
            Console.WriteLine("--- AutoFlush and Buffering ---");
            
            string filePath = "NN_test_buffer.txt";
            
            using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8, 1024))
            {
                writer.AutoFlush = true;
                Console.WriteLine($"AutoFlush enabled: {writer.AutoFlush}");
                Console.WriteLine($"Buffer size: {writer.BufferSize} bytes");
                
                writer.WriteLine("Immediate write (AutoFlush=true)");
            }
            
            Console.WriteLine($"File written immediately: {File.ReadAllText(filePath)}");
            
            using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8, 1024))
            {
                writer.AutoFlush = false;
                writer.WriteLine("Buffered write");
                Console.WriteLine("(Buffer not flushed yet)");
            }
            
            Console.WriteLine("(After dispose, buffer flushed automatically)");
            Console.WriteLine($"File content: {File.ReadAllText(filePath)}");
            
            File.Delete(filePath);
            Console.WriteLine("// Output: AutoFlush controls when data is written to disk");
        }

        private static void RealWorldExample_GenerateReport()
        {
            Console.WriteLine();
            Console.WriteLine("=== REAL-WORLD EXAMPLE: Generate Sales Report ===");
            
            string reportPath = "NN_sales_report.txt";
            
            using (StreamWriter writer = new StreamWriter(reportPath, false, Encoding.UTF8))
            {
                writer.WriteLine("╔════════════════════════════════════════════════════════╗");
                writer.WriteLine("║           MONTHLY SALES REPORT - January 2024          ║");
                writer.WriteLine("╚════════════════════════════════════════════════════════╝");
                writer.WriteLine();
                
                writer.WriteLine($"Report Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                writer.WriteLine(new string('-', 50));
                writer.WriteLine();
                
                string[] products = { "Widget A", "Widget B", "Gadget X", "Gadget Y", "Tool Z" };
                int[] quantities = { 150, 200, 75, 120, 90 };
                decimal[] prices = { 19.99m, 29.99m, 49.99m, 39.99m, 24.99m };
                
                writer.WriteLine("Product Name          | Qty  | Price   | Total   ");
                writer.WriteLine(new string('-', 50));
                
                decimal grandTotal = 0;
                for (int i = 0; i < products.Length; i++)
                {
                    decimal total = quantities[i] * prices[i];
                    grandTotal += total;
                    writer.WriteLine($"{products[i],-20}| {quantities[i],4} | ${prices[i],6:F2} | ${total,7:F2}");
                }
                
                writer.WriteLine(new string('-', 50));
                writer.WriteLine($"{"GRAND TOTAL",-20}     |      |         | ${grandTotal,7:F2}");
                writer.WriteLine();
                writer.WriteLine(new string('=', 50));
                writer.WriteLine("END OF REPORT");
            }
            
            Console.WriteLine("Generated Report:");
            Console.WriteLine(File.ReadAllText(reportPath));
            
            File.Delete(reportPath);
            Console.WriteLine("// Output: Formatted report file created");
        }

        private static void CleanupDemoFiles()
        {
            string[] filesToClean = new[]
            {
                "NN_test_write_basic.txt", "NN_test_write_line.txt",
                "NN_test_utf8_write.txt", "NN_test_append.txt",
                "NN_test_buffer.txt", "NN_sales_report.txt"
            };
            
            foreach (string file in filesToClean)
            {
                if (File.Exists(file))
                    File.Delete(file);
            }
            Console.WriteLine("[Cleanup] All demo files removed");
        }
    }
}