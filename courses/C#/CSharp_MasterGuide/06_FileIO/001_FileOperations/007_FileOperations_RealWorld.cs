/*
    TOPIC: C# File I/O Operations
    SUBTOPIC: Real-World File Operations Examples
    FILE: 07_FileOperations_RealWorld.cs
    PURPOSE: Demonstrates practical, real-world file operations
             including configuration, logging, and data processing
*/

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace CSharp_MasterGuide._06_FileIO._01_FileOperations
{
    public class FileOperationsRealWorldDemo
    {
        private static readonly string BasePath = Path.Combine(Path.GetTempPath(), "RealWorld_Demo");
        
        public static void Main(string[] args)
        {
            SetupDirectory();
            
            ApplicationConfigurationDemo();
            LogFileProcessingDemo();
            DataExportImportDemo();
            TempFileManagementDemo();
            FileWatcherDemo();
            
            Console.WriteLine("\n=== Real-world demos completed ===");
        }
        
        private static void SetupDirectory()
        {
            if (Directory.Exists(BasePath))
            {
                Directory.Delete(BasePath, true);
            }
            Directory.CreateDirectory(BasePath);
            Console.WriteLine($"Working directory: {BasePath}");
        }
        
        private static void ApplicationConfigurationDemo()
        {
            Console.WriteLine("\n=== Application Configuration System ===");
            
            string configDir = Path.Combine(BasePath, "config");
            Directory.CreateDirectory(configDir);
            
            var config = new Dictionary<string, string>
            {
                { "AppName", "MyApplication" },
                { "Version", "1.0.0" },
                { "LogLevel", "Information" },
                { "MaxRetries", "3" },
                { "DatabasePath", "./data/db.sqlite" },
                { "EnableCache", "true" }
            };
            
            string configPath = Path.Combine(configDir, "app.config");
            SaveConfiguration(configPath, config);
            
            Console.WriteLine("Configuration saved:");
            foreach (var kvp in config)
            {
                Console.WriteLine($"  {kvp.Key} = {kvp.Value}");
                // Output: AppName = MyApplication
                // Output: Version = 1.0.0
                // etc.
            }
            
            var loadedConfig = LoadConfiguration(configPath);
            Console.WriteLine("\nConfiguration loaded:");
            Console.WriteLine($"  App: {loadedConfig["AppName"]}");
            // Output: App: MyApplication
            Console.WriteLine($"  Version: {loadedConfig["Version"]}");
            // Output: Version: 1.0.0
            
            // Update configuration
            loadedConfig["LogLevel"] = "Debug";
            SaveConfiguration(configPath, loadedConfig);
            Console.WriteLine("\nLog level updated to Debug");
            
            // Load and verify
            var updated = LoadConfiguration(configPath);
            Console.WriteLine($"Updated LogLevel: {updated["LogLevel"]}");
            // Output: Updated LogLevel: Debug
        }
        
        private static void SaveConfiguration(string path, Dictionary<string, string> config)
        {
            string[] lines = config.Select(kvp => $"{kvp.Key}={kvp.Value}").ToArray();
            File.WriteAllLines(path, lines);
        }
        
        private static Dictionary<string, string> LoadConfiguration(string path)
        {
            var config = new Dictionary<string, string>();
            
            if (!File.Exists(path))
            {
                return config;
            }
            
            string[] lines = File.ReadAllLines(path);
            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line) || line.Contains("#"))
                {
                    continue;
                }
                
                int separator = line.IndexOf('=');
                if (separator > 0)
                {
                    string key = line.Substring(0, separator).Trim();
                    string value = line.Substring(separator + 1).Trim();
                    config[key] = value;
                }
            }
            
            return config;
        }
        
        private static void LogFileProcessingDemo()
        {
            Console.WriteLine("\n=== Log File Processing ===");
            
            string logDir = Path.Combine(BasePath, "logs");
            Directory.CreateDirectory(logDir);
            
            string logFile = Path.Combine(logDir, $"app_{DateTime.Now:yyyyMMdd}.log");
            
            // Application logging
            LogMessage(logFile, "INFO", "Application started");
            LogMessage(logFile, "INFO", "Configuration loaded successfully");
            LogMessage(logFile, "DEBUG", "Initializing database connection");
            LogMessage(logFile, "INFO", "Database connected");
            LogMessage(logFile, "WARNING", "Cache not available, using direct database");
            LogMessage(logFile, "ERROR", "Failed to load user preferences: timeout");
            LogMessage(logFile, "INFO", "Using default preferences");
            LogMessage(logFile, "INFO", "Application ready for requests");
            
            Console.WriteLine("Log entries written to file");
            // Output: Log entries written to file
            
            // Process and analyze logs
            var logAnalysis = AnalyzeLogFile(logFile);
            Console.WriteLine("\nLog Analysis:");
            Console.WriteLine($"  Total entries: {logAnalysis.TotalEntries}");
            // Output: Total entries: 8
            Console.WriteLine($"  Errors: {logAnalysis.ErrorCount}");
            // Output: Errors: 1
            Console.WriteLine($"  Warnings: {logAnalysis.WarningCount}");
            // Output: Warnings: 1
            Console.WriteLine($"  Info: {logAnalysis.InfoCount}");
            // Output: Info: 6
            
            Console.WriteLine("\nError entries:");
            foreach (string error in logAnalysis.Errors)
            {
                Console.WriteLine($"  {error}");
                // Output: [timestamp] ERROR: Failed to load user preferences: timeout
            }
            
            // Search logs for specific patterns
            var dbLogs = SearchLog(logFile, "database", "DATABASE", "DB");
            Console.WriteLine("\nDatabase-related logs:");
            foreach (string entry in dbLogs)
            {
                Console.WriteLine($"  {entry}");
            }
        }
        
        private static void LogMessage(string logFile, string level, string message)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string logLine = $"[{timestamp}] [{level}] {message}";
            File.AppendAllText(logFile, logLine + Environment.NewLine);
        }
        
        private static LogAnalysisResult AnalyzeLogFile(string logFile)
        {
            var result = new LogAnalysisResult();
            
            if (!File.Exists(logFile))
            {
                return result;
            }
            
            string[] lines = File.ReadAllLines(logFile);
            result.TotalEntries = lines.Length;
            
            foreach (string line in lines)
            {
                if (line.Contains("[ERROR]"))
                {
                    result.ErrorCount++;
                    result.Errors.Add(line);
                }
                else if (line.Contains("[WARNING]"))
                {
                    result.WarningCount++;
                }
                else if (line.Contains("[INFO]"))
                {
                    result.InfoCount++;
                }
                else if (line.Contains("[DEBUG]"))
                {
                    result.DebugCount++;
                }
            }
            
            return result;
        }
        
        private static List<string> SearchLog(string logFile, params string[] keywords)
        {
            var matches = new List<string>();
            
            if (!File.Exists(logFile))
            {
                return matches;
            }
            
            string[] lines = File.ReadAllLines(logFile);
            foreach (string line in lines)
            {
                foreach (string keyword in keywords)
                {
                    if (line.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                    {
                        matches.Add(line);
                        break;
                    }
                }
            }
            
            return matches;
        }
        
        private static void DataExportImportDemo()
        {
            Console.WriteLine("\n=== Data Export/Import ===");
            
            string dataDir = Path.Combine(BasePath, "data");
            Directory.CreateDirectory(dataDir);
            
            // Sample data
            var users = new List<UserRecord>
            {
                new UserRecord { Id = 1, Name = "John Doe", Email = "john@example.com", Created = DateTime.Now.AddDays(-30) },
                new UserRecord { Id = 2, Name = "Jane Smith", Email = "jane@example.com", Created = DateTime.Now.AddDays(-15) },
                new UserRecord { Id = 3, Name = "Bob Johnson", Email = "bob@example.com", Created = DateTime.Now.AddDays(-5) }
            };
            
            // Export to CSV
            string csvPath = Path.Combine(dataDir, "users.csv");
            ExportToCsv(csvPath, users);
            Console.WriteLine($"CSV exported: {csvPath}");
            
            // Export to JSON
            string jsonPath = Path.Combine(dataDir, "users.json");
            ExportToJson(jsonPath, users);
            Console.WriteLine($"JSON exported: {jsonPath}");
            
            // Export to TSV
            string tsvPath = Path.Combine(dataDir, "users.tsv");
            ExportToTsv(tsvPath, users);
            Console.WriteLine($"TSV exported: {tsvPath}");
            
            // Import from CSV
            var importedFromCsv = ImportFromCsv(csvPath);
            Console.WriteLine($"\nImported from CSV: {importedFromCsv.Count} users");
            foreach (var user in importedFromCsv)
            {
                Console.WriteLine($"  {user.Name} ({user.Email})");
                // Output: John Doe (john@example.com)
                // etc.
            }
            
            // Import from JSON
            var importedFromJson = ImportFromJson(jsonPath);
            Console.WriteLine($"\nImported from JSON: {importedFromJson.Count} users");
        }
        
        private static void ExportToCsv(string path, List<UserRecord> users)
        {
            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.WriteLine("Id,Name,Email,Created");
                foreach (var user in users)
                {
                    writer.WriteLine($"{user.Id},{EscapeCsv(user.Name)},{EscapeCsv(user.Email)},{user.Created:yyyy-MM-dd}");
                }
            }
        }
        
        private static string EscapeCsv(string value)
        {
            if (value.Contains(',') || value.Contains('"') || value.Contains('\n'))
            {
                return $"\"{value.Replace("\"", "\"\"")}\"";
            }
            return value;
        }
        
        private static List<UserRecord> ImportFromCsv(string path)
        {
            var users = new List<UserRecord>();
            string[] lines = File.ReadAllLines(path);
            
            for (int i = 1; i < lines.Length; i++)  // Skip header
            {
                string[] parts = lines[i].Split(',');
                users.Add(new UserRecord
                {
                    Id = int.Parse(parts[0]),
                    Name = parts[1],
                    Email = parts[2],
                    Created = DateTime.Parse(parts[3])
                });
            }
            
            return users;
        }
        
        private static void ExportToJson(string path, List<UserRecord> users)
        {
            var json = System.Text.Json.JsonSerializer.Serialize(users, new System.Text.Json.JsonSerializerOptions 
            { 
                WriteIndented = true 
            });
            File.WriteAllText(path, json);
        }
        
        private static List<UserRecord> ImportFromJson(string path)
        {
            string json = File.ReadAllText(path);
            return System.Text.Json.JsonSerializer.Deserialize<List<UserRecord>>(json);
        }
        
        private static void ExportToTsv(string path, List<UserRecord> users)
        {
            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.WriteLine("Id\tName\tEmail\tCreated");
                foreach (var user in users)
                {
                    writer.WriteLine($"{user.Id}\t{user.Name}\t{user.Email}\t{user.Created:yyyy-MM-dd}");
                }
            }
        }
        
        private static void TempFileManagementDemo()
        {
            Console.WriteLine("\n=== Temp File Management ===");
            
            string tempDir = Path.Combine(BasePath, "temp");
            
            // Create temp file safely
            string tempFile = Path.Combine(tempDir, Path.GetRandomFileName());
            File.WriteAllText(tempFile, "Temporary content");
            Console.WriteLine($"Temp file created: {Path.GetFileName(tempFile)}");
            
            // Use Path.GetTempFileName (creates actual file)
            string systemTempFile = Path.GetTempFileName();
            File.WriteAllText(systemTempFile, "System temp file");
            Console.WriteLine($"System temp file: {systemTempFile}");
            // Output: C:\Users\...\AppData\Local\Temp\tmpXXXX.tmp
            
            // Use Path.GetTempPath
            Console.WriteLine($"System temp path: {Path.GetTempPath()}");
            // Output: C:\Users\...\AppData\Local\Temp\
            
            // Clean up temp files
            string[] tempFiles = Directory.GetFiles(tempDir, "*.*");
            foreach (string f in tempFiles)
            {
                try
                {
                    File.Delete(f);
                    Console.WriteLine($"Deleted: {Path.GetFileName(f)}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to delete {f}: {ex.Message}");
                }
            }
            
            // Clean up system temp file
            if (File.Exists(systemTempFile))
            {
                File.Delete(systemTempFile);
                Console.WriteLine($"System temp file cleaned up");
            }
        }
        
        private static void FileWatcherDemo()
        {
            Console.WriteLine("\n=== FileSystemWatcher Demo ===");
            
            string watchDir = Path.Combine(BasePath, "watched");
            Directory.CreateDirectory(watchDir);
            
            using (var watcher = new FileSystemWatcher(watchDir))
            {
                watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.Size;
                watcher.Filter = "*.txt";
                
                watcher.Created += (s, e) => Console.WriteLine($"Created: {e.Name}");
                watcher.Changed += (s, e) => Console.WriteLine($"Changed: {e.Name}");
                watcher.Deleted += (s, e) => Console.WriteLine($"Deleted: {e.Name}");
                watcher.Renamed += (s, e) => Console.WriteLine($"Renamed: {e.Name} -> {e.FullPath}");
                
                watcher.EnableRaisingEvents = true;
                
                // Trigger some events
                File.WriteAllText(Path.Combine(watchDir, "test1.txt"), "Test 1");
                File.WriteAllText(Path.Combine(watchDir, "test2.txt"), "Test 2");
                File.Move(Path.Combine(watchDir, "test1.txt"), Path.Combine(watchDir, "renamed1.txt"));
                File.Delete(Path.Combine(watchDir, "test2.txt"));
                
                // Wait a bit for events to process
                System.Threading.Thread.Sleep(500);
                
                Console.WriteLine("FileSystemWatcher events processed");
                // Output shows Created, Changed, Renamed, Deleted events
            }
        }
        
        private class LogAnalysisResult
        {
            public int TotalEntries { get; set; }
            public int ErrorCount { get; set; }
            public int WarningCount { get; set; }
            public int InfoCount { get; set; }
            public int DebugCount { get; set; }
            public List<string> Errors { get; set; } = new List<string>();
        }
        
        private class UserRecord
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public DateTime Created { get; set; }
        }
    }
}