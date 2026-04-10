/*
    TOPIC: C# File I/O Operations
    SUBTOPIC: File Read and Write Basics
    FILE: 01_FileReadWrite.cs
    PURPOSE: Demonstrates basic file operations using File class methods
             including ReadAllText, WriteAllText, and ReadAllLines
*/

using System;
using System.IO;
using System.Linq;

namespace CSharp_MasterGuide._06_FileIO._01_FileOperations
{
    public class FileReadWriteDemo
    {
        private static readonly string BasePath = Path.Combine(Path.GetTempPath(), "FileIO_Demo");
        
        public static void Main(string[] args)
        {
            SetupDirectory();
            
            DemonstrateReadAllText();
            DemonstrateWriteAllText();
            DemonstrateReadAllLines();
            DemonstrateRealWorldExamples();
            
            Console.WriteLine("\n=== All demonstrations completed ===");
        }
        
        private static void SetupDirectory()
        {
            if (!Directory.Exists(BasePath))
            {
                Directory.CreateDirectory(BasePath);
            }
            Console.WriteLine($"Working directory: {BasePath}");
        }
        
        private static void DemonstrateReadAllText()
        {
            Console.WriteLine("\n=== File.ReadAllText Demo ===");
            
            string filePath = Path.Combine(BasePath, "sample_text.txt");
            string content = "Hello, this is a sample text file.\nIt contains multiple lines.\nFile I/O in C# is powerful!";
            File.WriteAllText(filePath, content);
            
            // Read entire file as single string
            string readContent = File.ReadAllText(filePath);
            Console.WriteLine("File content read:");
            // Output: File content read:
            Console.WriteLine(readContent);
            // Output: Hello, this is a sample text file.
            // Output: It contains multiple lines.
            // Output: File I/O in C# is powerful!
            
            // ReadAllText with encoding
            string utf8Content = File.ReadAllText(filePath, System.Text.Encoding.UTF8);
            Console.WriteLine($"Content length: {utf8Content.Length}");
            // Output: Content length: 84
        }
        
        private static void DemonstrateWriteAllText()
        {
            Console.WriteLine("\n=== File.WriteAllText Demo ===");
            
            string filePath = Path.Combine(BasePath, "write_demo.txt");
            
            // Write simple text
            string simpleText = "This is a simple text written using WriteAllText.";
            File.WriteAllText(filePath, simpleText);
            string result = File.ReadAllText(filePath);
            Console.WriteLine($"Written and read: {result}");
            // Output: Written and read: This is a simple text written using WriteAllText.
            
            // Write with UTF-8 encoding
            string unicodeText = "Unicode: Hello World - こんにちは - مرحبا";
            File.WriteAllText(Path.Combine(BasePath, "unicode_demo.txt"), unicodeText, System.Text.Encoding.UTF8);
            Console.WriteLine("Unicode text written successfully");
            // Output: Unicode text written successfully
            
            // Overwrite existing file
            string newContent = "Updated content - This overwrites the previous content!";
            File.WriteAllText(filePath, newContent);
            Console.WriteLine($"After overwrite: {File.ReadAllText(filePath)}");
            // Output: After overwrite: Updated content - This overwrites the previous content!
        }
        
        private static void DemonstrateReadAllLines()
        {
            Console.WriteLine("\n=== File.ReadAllLines Demo ===");
            
            string filePath = Path.Combine(BasePath, "multi_line.txt");
            string[] lines = 
            {
                "Line 1: This is the first line",
                "Line 2: Here's the second line",
                "Line 3: And the third line",
                "Line 4: Fourth line for variety",
                "Line 5: Finally the fifth line"
            };
            File.WriteAllLines(filePath, lines);
            
            // ReadAllLines returns string array
            string[] readLines = File.ReadAllLines(filePath);
            Console.WriteLine("Reading all lines:");
            foreach (string line in readLines)
            {
                Console.WriteLine($"  {line}");
                // Output: Line 1: This is the first line
                // Output: Line 2: Here's the second line
                // ...etc
            }
            
            // LINQ operations on file lines
            string[] filteredLines = readLines.Where(l => l.Contains("Line 1") || l.Contains("Line 3")).ToArray();
            Console.WriteLine("\nFiltered lines containing 'Line 1' or 'Line 3':");
            foreach (string line in filteredLines)
            {
                Console.WriteLine($"  {line}");
                // Output: Line 1: This is the first line
                // Output: Line 3: And the third line
            }
            
            // Count lines
            Console.WriteLine($"\nTotal lines: {readLines.Length}");
            // Output: Total lines: 5
        }
        
        private static void DemonstrateRealWorldExamples()
        {
            Console.WriteLine("\n=== Real-World Examples ===");
            
            // Example 1: Config file processing
            ProcessConfigFile();
            
            // Example 2: Log file reader
            ProcessLogFile();
            
            // Example 3: CSV data processing
            ProcessCsvData();
        }
        
        private static void ProcessConfigFile()
        {
            Console.WriteLine("\n--- Config File Processing ---");
            
            string configPath = Path.Combine(BasePath, "app.config");
            string configContent = @"Database=localhost
Port=5432
Username=admin
DebugMode=true
MaxConnections=100";
            File.WriteAllText(configPath, configContent);
            
            string[] configLines = File.ReadAllLines(configPath);
            var config = new System.Collections.Generic.Dictionary<string, string>();
            
            foreach (string line in configLines)
            {
                string[] parts = line.Split('=');
                if (parts.Length == 2)
                {
                    config[parts[0].Trim()] = parts[1].Trim();
                }
            }
            
            Console.WriteLine("Parsed config values:");
            foreach (var kvp in config)
            {
                Console.WriteLine($"  {kvp.Key} = {kvp.Value}");
                // Output: Database = localhost
                // Output: Port = 5432
                // Output: Username = admin
                // Output: DebugMode = true
                // Output: MaxConnections = 100
            }
        }
        
        private static void ProcessLogFile()
        {
            Console.WriteLine("\n--- Log File Processing ---");
            
            string logPath = Path.Combine(BasePath, "application.log");
            string[] logEntries = 
            {
                "[2024-01-15 10:30:15] INFO: Application started",
                "[2024-01-15 10:30:16] DEBUG: Loading configuration",
                "[2024-01-15 10:30:17] INFO: Database connected",
                "[2024-01-15 10:30:18] ERROR: Failed to load plugin: FileNotFound",
                "[2024-01-15 10:30:19] WARNING: Using default settings",
                "[2024-01-15 10:30:20] INFO: Application ready"
            };
            File.WriteAllLines(logPath, logEntries);
            
            string[] allLogs = File.ReadAllLines(logPath);
            
            var errorLogs = allLogs.Where(l => l.Contains("ERROR")).ToArray();
            var warningLogs = allLogs.Where(l => l.Contains("WARNING")).ToArray();
            var infoLogs = allLogs.Where(l => l.Contains("INFO")).ToArray();
            
            Console.WriteLine($"Total log entries: {allLogs.Length}");
            // Output: Total log entries: 6
            Console.WriteLine($"Errors: {errorLogs.Length}, Warnings: {warningLogs.Length}, Info: {infoLogs.Length}");
            // Output: Errors: 1, Warnings: 1, Info: 4
            
            Console.WriteLine("\nError entries:");
            foreach (string error in errorLogs)
            {
                Console.WriteLine($"  {error}");
                // Output: [2024-01-15 10:30:18] ERROR: Failed to load plugin: FileNotFound
            }
        }
        
        private static void ProcessCsvData()
        {
            Console.WriteLine("\n--- CSV Data Processing ---");
            
            string csvPath = Path.Combine(BasePath, "users.csv");
            string csvContent = @"Name,Age,City,Email
John Doe,30,New York,john@example.com
Jane Smith,25,Los Angeles,jane@example.com
Bob Johnson,35,Chicago,bob@example.com
Alice Brown,28,Houston,alice@example.com";
            File.WriteAllText(csvPath, csvContent);
            
            string[] csvLines = File.ReadAllLines(csvPath);
            string[] headers = csvLines[0].Split(',');
            
            Console.WriteLine("CSV Headers: " + string.Join(", ", headers));
            // Output: CSV Headers: Name, Age, City, Email
            
            var users = new System.Collections.Generic.List<string[]>();
            for (int i = 1; i < csvLines.Length; i++)
            {
                users.Add(csvLines[i].Split(','));
            }
            
            Console.WriteLine("\nParsed users:");
            foreach (string[] user in users)
            {
                Console.WriteLine($"  Name: {user[0]}, Age: {user[1]}, City: {user[2]}, Email: {user[3]}");
                // Output: Name: John Doe, Age: 30, City: New York, Email: john@example.com
                // ... etc
            }
        }
    }
}