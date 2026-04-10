/*
    TOPIC: C# File I/O Operations
    SUBTOPIC: File Read and Write - Binary and Appending
    FILE: 02_FileReadWrite_Part2.cs
    PURPOSE: Demonstrates binary file operations (ReadAllBytes, WriteAllBytes)
             and file appending operations
*/

using System;
using System.IO;
using System.Text;

namespace CSharp_MasterGuide._06_FileIO._01_FileOperations
{
    public class FileReadWritePart2Demo
    {
        private static readonly string BasePath = Path.Combine(Path.GetTempPath(), "FileIO_Demo_Part2");
        
        public static void Main(string[] args)
        {
            SetupDirectory();
            
            DemonstrateReadAllBytes();
            DemonstrateWriteAllBytes();
            DemonstrateAppendOperations();
            DemonstrateRealWorldExamples();
            
            Console.WriteLine("\n=== All Part 2 demonstrations completed ===");
        }
        
        private static void SetupDirectory()
        {
            if (!Directory.Exists(BasePath))
            {
                Directory.CreateDirectory(BasePath);
            }
            Console.WriteLine($"Working directory: {BasePath}");
        }
        
        private static void DemonstrateReadAllBytes()
        {
            Console.WriteLine("\n=== File.ReadAllBytes Demo ===");
            
            string filePath = Path.Combine(BasePath, "binary_data.bin");
            
            // Create sample binary data
            byte[] originalData = new byte[] { 0x48, 0x65, 0x6C, 0x6C, 0x6F, 0x20, 0x57, 0x6F, 0x72, 0x6C, 0x64 };
            File.WriteAllBytes(filePath, originalData);
            
            // Read all bytes from file
            byte[] readData = File.ReadAllBytes(filePath);
            
            Console.WriteLine("Original bytes: " + BitConverter.ToString(originalData));
            // Output: Original bytes: 48-65-6C-6C-6F-20-57-6F-72-6C-64
            
            Console.WriteLine("Read bytes: " + BitConverter.ToString(readData));
            // Output: Read bytes: 48-65-6C-6C-6F-20-57-6F-72-6C-64
            
            // Convert bytes to string
            string textContent = Encoding.UTF8.GetString(readData);
            Console.WriteLine($"As string: {textContent}");
            // Output: As string: Hello World
            
            Console.WriteLine($"Byte array length: {readData.Length}");
            // Output: Byte array length: 11
        }
        
        private static void DemonstrateWriteAllBytes()
        {
            Console.WriteLine("\n=== File.WriteAllBytes Demo ===");
            
            // Write simple byte array
            string textFile = Path.Combine(BasePath, "text_as_bytes.txt");
            byte[] textBytes = Encoding.UTF8.GetBytes("Hello from WriteAllBytes!");
            File.WriteAllBytes(textFile, textBytes);
            
            byte[] verifyBytes = File.ReadAllBytes(textFile);
            Console.WriteLine($"Written and verified: {Encoding.UTF8.GetString(verifyBytes)}");
            // Output: Written and verified: Hello from WriteAllBytes!
            
            // Write numeric data as bytes
            string numbersFile = Path.Combine(BasePath, "numbers.bin");
            int number = 12345;
            byte[] intBytes = BitConverter.GetBytes(number);
            File.WriteAllBytes(numbersFile, intBytes);
            
            byte[] readIntBytes = File.ReadAllBytes(numbersFile);
            int restoredNumber = BitConverter.ToInt32(readIntBytes, 0);
            Console.WriteLine($"Integer written: {number}, restored: {restoredNumber}");
            // Output: Integer written: 12345, restored: 12345
            
            // Write multiple data types
            string mixedFile = Path.Combine(BasePath, "mixed_data.bin");
            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(BitConverter.GetBytes((short)100), 0, 2);
                ms.Write(BitConverter.GetBytes(50000), 0, 4);
                ms.Write(BitConverter.GetBytes(3.14159f), 0, 4);
                ms.Write(BitConverter.GetBytes(true), 0, 1);
                File.WriteAllBytes(mixedFile, ms.ToArray());
            }
            
            byte[] mixedData = File.ReadAllBytes(mixedFile);
            Console.WriteLine($"Mixed data file size: {mixedData.Length} bytes");
            // Output: Mixed data file size: 11 bytes
        }
        
        private static void DemonstrateAppendOperations()
        {
            Console.WriteLine("\n=== File Append Operations Demo ===");
            
            string appendFile = Path.Combine(BasePath, "append_demo.txt");
            
            // Ensure clean start
            if (File.Exists(appendFile))
            {
                File.Delete(appendFile);
            }
            
            // WriteAllText - overwrites file
            File.WriteAllText(appendFile, "First write.\n");
            Console.WriteLine("After first WriteAllText:");
            Console.WriteLine(File.ReadAllText(appendFile));
            // Output: First write.
            
            // AppendAllText - adds to end
            File.AppendAllText(appendFile, "Second line appended.\n");
            Console.WriteLine("After first AppendAllText:");
            Console.WriteLine(File.ReadAllText(appendFile));
            // Output: First write.
            // Output: Second line appended.
            
            // AppendAllText again
            File.AppendAllText(appendFile, "Third line appended.\n");
            Console.WriteLine("After second AppendAllText:");
            Console.WriteLine(File.ReadAllText(appendFile));
            // Output: First write.
            // Output: Second line appended.
            // Output: Third line appended.
            
            // AppendAllLines - array of lines
            string[] moreLines = { "Line A", "Line B", "Line C" };
            File.AppendAllLines(appendFile, moreLines);
            
            Console.WriteLine("After AppendAllLines:");
            string[] finalContent = File.ReadAllLines(appendFile);
            foreach (string line in finalContent)
            {
                Console.WriteLine($"  {line}");
                // Output: First write.
                // Output: Second line appended.
                // Output: Third line appended.
                // Output: Line A
                // Output: Line B
                // Output: Line C
            }
            
            // Using FileStream for append with FileMode.Append
            using (FileStream fs = new FileStream(appendFile, FileMode.Append, FileAccess.Write))
            {
                byte[] newContent = Encoding.UTF8.GetBytes("Added via FileStream append\n");
                fs.Write(newContent, 0, newContent.Length);
            }
            
            Console.WriteLine("After FileStream append:");
            Console.WriteLine(File.ReadAllText(appendFile));
            // Output includes: Added via FileStream append
        }
        
        private static void DemonstrateRealWorldExamples()
        {
            Console.WriteLine("\n=== Real-World Examples ===");
            
            // Example 1: Image file copy
            CopyImageFile();
            
            // Example 2: Session data persistence
            PersistSessionData();
            
            // Example 3: Append-only log system
            AppendOnlyLogSystem();
        }
        
        private static void CopyImageFile()
        {
            Console.WriteLine("\n--- Image File Copy (Binary) ---");
            
            string sourceImage = Path.Combine(BasePath, "source_image.png");
            string destImage = Path.Combine(BasePath, "dest_image.png");
            
            // Create a simple "image" (11x11 white pixels as raw bytes)
            byte[] fakeImage = new byte[121];
            for (int i = 0; i < 121; i++) fakeImage[i] = 0xFF;
            File.WriteAllBytes(sourceImage, fakeImage);
            
            // Copy using ReadAllBytes/WriteAllBytes
            byte[] imageData = File.ReadAllBytes(sourceImage);
            File.WriteAllBytes(destImage, imageData);
            
            Console.WriteLine($"Source file size: {imageData.Length} bytes");
            // Output: Source file size: 121 bytes
            Console.WriteLine($"Destination file size: {File.ReadAllBytes(destImage).Length} bytes");
            // Output: Destination file size: 121 bytes
            Console.WriteLine($"Files identical: {imageData.SequenceEqual(File.ReadAllBytes(destImage))}");
            // Output: Files identical: True
        }
        
        private static void PersistSessionData()
        {
            Console.WriteLine("\n--- Session Data Persistence ---");
            
            string sessionFile = Path.Combine(BasePath, "session.dat");
            
            // Simulate session data
            var sessionData = new System.Collections.Generic.Dictionary<string, string>
            {
                { "UserId", "USER12345" },
                { "Username", "john_doe" },
                { "LoginTime", DateTime.Now.AddHours(-2).ToString("o") },
                { "LastActivity", DateTime.Now.ToString("o") },
                { "Theme", "dark" }
            };
            
            // Serialize session to bytes
            using (MemoryStream ms = new MemoryStream())
            using (StreamWriter sw = new StreamWriter(ms))
            {
                foreach (var kvp in sessionData)
                {
                    sw.WriteLine($"{kvp.Key}={kvp.Value}");
                }
                File.WriteAllBytes(sessionFile, ms.ToArray());
            }
            
            // Read and deserialize session
            byte[] storedData = File.ReadAllBytes(sessionFile);
            string sessionString = Encoding.UTF8.GetString(storedData);
            var restoredSession = new System.Collections.Generic.Dictionary<string, string>();
            
            foreach (string line in sessionString.Split('\n', StringSplitOptions.RemoveEmptyEntries))
            {
                string[] parts = line.Split('=');
                if (parts.Length == 2)
                {
                    restoredSession[parts[0]] = parts[1];
                }
            }
            
            Console.WriteLine("Restored session data:");
            foreach (var kvp in restoredSession)
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
                // Output: UserId: USER12345
                // Output: Username: john_doe
                // Output: LoginTime: (timestamp)
                // Output: LastActivity: (timestamp)
                // Output: Theme: dark
            }
        }
        
        private static void AppendOnlyLogSystem()
        {
            Console.WriteLine("\n--- Append-Only Log System ---");
            
            string logFile = Path.Combine(BasePath, "audit_log.txt");
            
            // Clear previous demo
            if (File.Exists(logFile)) File.Delete(logFile);
            
            // Log entries with timestamps
            LogEntry(logFile, "INFO", "Application started");
            LogEntry(logFile, "INFO", "User session initialized");
            LogEntry(logFile, "DEBUG", "Loading user preferences");
            LogEntry(logFile, "WARNING", "Cache miss for key: user_settings");
            LogEntry(logFile, "ERROR", "Database connection timeout");
            LogEntry(logFile, "INFO", "Retrying database connection");
            LogEntry(logFile, "INFO", "Connection restored");
            
            // Read and display all logs
            Console.WriteLine("Complete log file:");
            string[] allLogs = File.ReadAllLines(logFile);
            foreach (string log in allLogs)
            {
                Console.WriteLine($"  {log}");
                // Output: [timestamp] [LEVEL] message
            }
            
            // Count by level
            var levelCounts = new System.Collections.Generic.Dictionary<string, int>();
            foreach (string log in allLogs)
            {
                string level = log.Split('[')[2].TrimEnd(']');
                if (!levelCounts.ContainsKey(level)) levelCounts[level] = 0;
                levelCounts[level]++;
            }
            
            Console.WriteLine("\nLog summary:");
            foreach (var kvp in levelCounts)
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
                // Output: INFO: 4
                // Output: DEBUG: 1
                // Output: WARNING: 1
                // Output: ERROR: 1
            }
        }
        
        private static void LogEntry(string logFile, string level, string message)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string logLine = $"[{timestamp}] [{level}] {message}";
            File.AppendAllText(logFile, logLine + Environment.NewLine);
        }
    }
}