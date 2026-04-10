/*
TOPIC: C# File I/O Operations
SUBTOPIC: StreamReader and StreamWriter
FILE: 05_Streams_RealWorld.cs
PURPOSE: Real-world examples - log file processing, data export/import
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace CSharp_MasterGuide._06_FileIO._02_StreamReaderWriter
{
    public class NN_05_Streams_RealWorld
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Real-World Stream Examples ===");
            Console.WriteLine();

            RealWorldExample_ApplicationLogger();
            Console.WriteLine();

            RealWorldExample_DataExportCSV();
            Console.WriteLine();

            RealWorldExample_DataImportCSV();
            Console.WriteLine();

            RealWorldExample_FileMerger();
            Console.WriteLine();

            RealWorldExample_LogParserStatistics();
            
            CleanupDemoFiles();
        }

        private static void RealWorldExample_ApplicationLogger()
        {
            Console.WriteLine("=== REAL-WORLD: Application Logger ===");
            
            string logFile = "NN_app.log";
            DateTime startTime = DateTime.Now;
            
            using (StreamWriter logger = new StreamWriter(logFile, true, Encoding.UTF8))
            {
                logger.AutoFlush = true;
                
                logger.WriteLine($"[INFO] {DateTime.Now:HH:mm:ss.fff} - Application starting");
                logger.WriteLine($"[DEBUG] {DateTime.Now:HH:mm:ss.fff} - Loading configuration from config.json");
                logger.WriteLine($"[INFO] {DateTime.Now:HH:mm:ss.fff} - Database connection established");
                logger.WriteLine($"[WARN] {DateTime.Now:HH:mm:ss.fff} - Cache miss for key: user_preferences");
                logger.WriteLine($"[INFO] {DateTime.Now:HH:mm:ss.fff} - User 'john_doe' logged in");
                logger.WriteLine($"[ERROR] {DateTime.Now:HH:mm:ss.fff} - Failed to send email: SMTP timeout");
                logger.WriteLine($"[INFO] {DateTime.Now:HH:mm:ss.fff} - Background job 'cleanup' completed");
                logger.WriteLine($"[DEBUG] {DateTime.Now:HH:mm:ss.fff} - Memory usage: 125MB");
            }
            
            Console.WriteLine("Log file created with entries:");
            string[] logLines = File.ReadAllLines(logFile);
            foreach (string line in logLines)
            {
                Console.WriteLine($"  {line}");
            }
            
            Console.WriteLine($"Log file size: {new FileInfo(logFile).Length} bytes");
            Console.WriteLine("// Output: Application logging to file with timestamps");
            
            File.Delete(logFile);
        }

        private static void RealWorldExample_DataExportCSV()
        {
            Console.WriteLine("=== REAL-WORLD: Export Data to CSV ===");
            
            string csvFile = "NN_employees.csv";
            
            List<Employee> employees = new List<Employee>
            {
                new Employee { Id = 1, Name = "Alice Johnson", Department = "Engineering", Salary = 75000, HireDate = new DateTime(2020, 3, 15) },
                new Employee { Id = 2, Name = "Bob Smith", Department = "Marketing", Salary = 65000, HireDate = new DateTime(2019, 7, 22) },
                new Employee { Id = 3, Name = "Carol Williams", Department = "Engineering", Salary = 80000, HireDate = new DateTime(2018, 11, 5) },
                new Employee { Id = 4, Name = "David Brown", Department = "Sales", Salary = 55000, HireDate = new DateTime(2021, 1, 10) },
                new Employee { Id = 5, Name = "Eva Martinez", Department = "HR", Salary = 60000, HireDate = new DateTime(2020, 9, 1) }
            };
            
            using (StreamWriter writer = new StreamWriter(csvFile, false, Encoding.UTF8))
            {
                writer.WriteLine("ID,Name,Department,Salary,HireDate");
                
                foreach (Employee emp in employees)
                {
                    writer.WriteLine($"{emp.Id},{EscapeCsv(emp.Name)},{EscapeCsv(emp.Department)},{emp.Salary},{emp.HireDate:yyyy-MM-dd}");
                }
            }
            
            Console.WriteLine("Exported CSV content:");
            string[] csvLines = File.ReadAllLines(csvFile);
            foreach (string line in csvLines)
            {
                Console.WriteLine($"  {line}");
            }
            
            decimal totalSalary = employees.Sum(e => e.Salary);
            double avgSalary = employees.Average(e => e.Salary);
            Console.WriteLine($"  Total employees: {employees.Count}");
            Console.WriteLine($"  Total salary: ${totalSalary:N0}");
            Console.WriteLine($"  Average salary: ${avgSalary:N2}");
            Console.WriteLine("// Output: Employee data exported to CSV format");
            
            File.Delete(csvFile);
        }

        private static void RealWorldExample_DataImportCSV()
        {
            Console.WriteLine("=== REAL-WORLD: Import Data from CSV ===");
            
            string csvFile = "NN_products.csv";
            
            string csvContent = @"ProductID,Name,Category,Price,Stock,Supplier
1001,Wireless Mouse,Electronics,29.99,150,TechSupply Co
1002,Office Chair,Furniture,199.99,25,FurniturePlus
1003,USB Cable,Electronics,9.99,500,TechSupply Co
1004,Desk Lamp,Furniture,45.00,40,LightingWorld
1005,Notebook Pack,Office Supplies,12.50,200,PaperMart";
            
            File.WriteAllText(csvFile, csvContent);
            
            List<Product> products = new List<Product>();
            
            using (StreamReader reader = new StreamReader(csvFile))
            {
                string? headerLine = reader.ReadLine();
                Console.WriteLine($"Header: {headerLine}");
                
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] fields = ParseCsvLine(line);
                    if (fields.Length >= 6)
                    {
                        products.Add(new Product
                        {
                            ProductId = int.Parse(fields[0]),
                            Name = fields[1],
                            Category = fields[2],
                            Price = decimal.Parse(fields[3]),
                            Stock = int.Parse(fields[4]),
                            Supplier = fields[5]
                        });
                    }
                }
            }
            
            Console.WriteLine("Imported Products:");
            foreach (Product p in products)
            {
                Console.WriteLine($"  {p.ProductId}: {p.Name} ({p.Category}) - ${p.Price:F2}, Stock: {p.Stock}");
            }
            
            var categoryGroups = products.GroupBy(p => p.Category);
            Console.WriteLine("Products by Category:");
            foreach (var group in categoryGroups)
            {
                Console.WriteLine($"  {group.Key}: {group.Count()} items, ${group.Sum(p => p.Price * p.Stock):N2} value");
            }
            Console.WriteLine("// Output: CSV data parsed into structured objects");
            
            File.Delete(csvFile);
        }

        private static void RealWorldExample_FileMerger()
        {
            Console.WriteLine("=== REAL-WORLD: Merge Multiple Files ===");
            
            string[] partFiles = new[] { "NN_part1.txt", "NN_part2.txt", "NN_part3.txt" };
            string mergedFile = "NN_merged.txt";
            
            string[] partContents = new[]
            {
                "=== Part 1: Introduction ===\nThis is the first section of the document.\nIt contains introductory material.",
                "=== Part 2: Main Content ===\nHere we present the main arguments.\nSupporting evidence follows.",
                "=== Part 3: Conclusion ===\nIn conclusion, we've covered all topics.\nThank you for reading."
            };
            
            for (int i = 0; i < partFiles.Length; i++)
            {
                File.WriteAllText(partFiles[i], partContents[i]);
            }
            
            using (StreamWriter writer = new StreamWriter(mergedFile, false, Encoding.UTF8))
            {
                writer.WriteLine($"Merged Document - Created: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                writer.WriteLine(new string('=', 50));
                writer.WriteLine();
                
                foreach (string partFile in partFiles)
                {
                    using (StreamReader reader = new StreamReader(partFile))
                    {
                        string? line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            writer.WriteLine(line);
                        }
                    }
                    writer.WriteLine();
                }
                
                writer.WriteLine(new string('=', 50));
                writer.WriteLine("End of merged document");
            }
            
            Console.WriteLine("Merged file content:");
            Console.WriteLine(File.ReadAllText(mergedFile));
            
            foreach (string partFile in partFiles)
            {
                File.Delete(partFile);
            }
            File.Delete(mergedFile);
            Console.WriteLine("// Output: Multiple files combined into one");
        }

        private static void RealWorldExample_LogParserStatistics()
        {
            Console.WriteLine("=== REAL-WORLD: Log Parser with Statistics ===");
            
            string logFile = "NN_server.log";
            
            string[] logEntries = new[]
            {
                "2024-01-15 08:00:01 INFO Server started on port 8080",
                "2024-01-15 08:00:05 DEBUG Loading configuration from config.xml",
                "2024-01-15 08:00:10 INFO Database connection pool initialized (10 connections)",
                "2024-01-15 08:01:23 WARN High memory usage detected: 85%",
                "2024-01-15 08:02:45 ERROR Failed to process request: timeout",
                "2024-01-15 08:03:12 INFO Request processed: /api/users (200ms)",
                "2024-01-15 08:03:15 DEBUG Cache hit for key: user_session_123",
                "2024-01-15 08:04:30 WARN Database query slow: 5000ms",
                "2024-01-15 08:05:00 ERROR Connection lost to cache server",
                "2024-01-15 08:05:05 INFO Cache server reconnected",
                "2024-01-15 08:06:22 INFO Request processed: /api/data (150ms)",
                "2024-01-15 08:07:45 DEBUG User authentication successful: admin",
                "2024-01-15 08:08:00 INFO Server health check: OK"
            };
            
            File.WriteAllLines(logFile, logEntries);
            
            int infoCount = 0, warnCount = 0, errorCount = 0, debugCount = 0;
            var errorMessages = new List<string>();
            
            using (StreamReader reader = new StreamReader(logFile))
            {
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains("INFO")) infoCount++;
                    else if (line.Contains("WARN")) warnCount++;
                    else if (line.Contains("ERROR"))
                    {
                        errorCount++;
                        errorMessages.Add(line);
                    }
                    else if (line.Contains("DEBUG")) debugCount++;
                }
            }
            
            Console.WriteLine("Log Statistics:");
            Console.WriteLine($"  INFO messages:  {infoCount}");
            Console.WriteLine($"  DEBUG messages: {debugCount}");
            Console.WriteLine($"  WARN messages:  {warnCount}");
            Console.WriteLine($"  ERROR messages: {errorCount}");
            
            if (errorMessages.Count > 0)
            {
                Console.WriteLine();
                Console.WriteLine("Error Details:");
                foreach (string err in errorMessages)
                {
                    Console.WriteLine($"  - {err}");
                }
            }
            
            Console.WriteLine();
            Console.WriteLine($"Total log entries: {infoCount + warnCount + errorCount + debugCount}");
            Console.WriteLine($"Error rate: {(double)errorCount / (infoCount + warnCount + errorCount + debugCount) * 100:F1}%");
            Console.WriteLine("// Output: Log file analyzed, statistics displayed");
            
            File.Delete(logFile);
        }

        private static string EscapeCsv(string value)
        {
            if (value.Contains(',') || value.Contains('"') || value.Contains('\n'))
            {
                return $"\"{value.Replace("\"", "\"\"")}\"";
            }
            return value;
        }

        private static string[] ParseCsvLine(string line)
        {
            List<string> fields = new List<string>();
            bool inQuotes = false;
            StringBuilder field = new StringBuilder();
            
            foreach (char c in line)
            {
                if (c == '"')
                {
                    inQuotes = !inQuotes;
                }
                else if (c == ',' && !inQuotes)
                {
                    fields.Add(field.ToString());
                    field.Clear();
                }
                else
                {
                    field.Append(c);
                }
            }
            fields.Add(field.ToString());
            
            return fields.ToArray();
        }

        private static void CleanupDemoFiles()
        {
            string[] filesToClean = Directory.GetFiles(Directory.GetCurrentDirectory(), "NN_*.*")
                .Where(f => f.EndsWith(".log") || f.EndsWith(".csv") || f.EndsWith(".txt"))
                .ToArray();
            
            foreach (string file in filesToClean)
            {
                try { File.Delete(file); } catch { }
            }
            Console.WriteLine("[Cleanup] All demo files removed");
        }
    }

    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Department { get; set; } = "";
        public decimal Salary { get; set; }
        public DateTime HireDate { get; set; }
    }

    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = "";
        public string Category { get; set; } = "";
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string Supplier { get; set; } = "";
    }
}