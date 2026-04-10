/*
 * ============================================================
 * TOPIC     : File I/O Operations
 * SUBTOPIC  : StreamReader and StreamWriter
 * FILE      : 01_StreamReader.cs
 * PURPOSE   : Demonstrates StreamReader basics - Read, ReadLine, ReadToEnd methods
 * ============================================================
 */
using System; // needed for Console.WriteLine and basic types
using System.IO; // needed for StreamReader, File, FileInfo
using System.Text; // needed for Encoding

namespace CSharp_MasterGuide._06_FileIO._02_StreamReaderWriter
{
    /// <summary>
    /// Demonstrates StreamReader methods for reading text files character by character, line by line, or entire content
    /// </summary>
    public class NN_01_StreamReader
    {
        /// <summary>
        /// Main entry point demonstrating StreamReader operations
        /// </summary>
        /// <param name="args">Command line arguments (not used)</param>
        public static void Main(string[] args)
        {
            // Create test file for demonstration
            // string testFilePath = path to temporary test file
            string testFilePath = CreateTestFile();
            
            // Console.WriteLine = outputs message to console
            // Output: === StreamReader Basics Demo ===
            Console.WriteLine("=== StreamReader Basics Demo ===");
            // Output: Test file: [path]
            Console.WriteLine($"Test file: {testFilePath}");
            Console.WriteLine();

            // Call method to demonstrate Read() method
            ReadUsingReadMethod(testFilePath);
            Console.WriteLine();

            // Call method to demonstrate ReadLine() method
            ReadUsingReadLineMethod(testFilePath);
            Console.WriteLine();

            // Call method to demonstrate ReadToEnd() method
            ReadUsingReadToEndMethod(testFilePath);
            Console.WriteLine();

            // Call method to demonstrate reading with specific encoding
            ReadWithSpecificEncoding(testFilePath);
            Console.WriteLine();

            // Call real-world example method
            RealWorldExample_ProcessLogFile();
            
            // Clean up test file
            CleanupTestFile(testFilePath);
        }

        /// <summary>
        /// Creates a test file with sample content for reading demonstrations
        /// </summary>
        /// <returns>Path to created test file</returns>
        private static string CreateTestFile()
        {
            // string path = file name for test file
            string path = "NN_test_streamreader.txt";
            
            // string[] lines = array of lines to write to file
            // new[] = implicitly typed array initialization
            string[] lines = new[]
            {
                "Line 1: This is the first line of text.",
                "Line 2: Here we have some numbers: 12345",
                "Line 3: Special characters: àéïõü 中文",
                "Line 4: Empty line follows",
                "", // Empty line
                "Line 6: Final line with timestamp",
                "---END OF FILE---"
            };

            // File.WriteAllLines = writes all lines to file at once
            // Encoding.UTF8 = UTF-8 encoding for international characters
            File.WriteAllLines(path, lines, Encoding.UTF8);
            
            // Output: [Setup] Created test file: [path]
            Console.WriteLine($"[Setup] Created test file: {path}");
            return path;
        }

        /// <summary>
        /// Demonstrates StreamReader.Read() - reading character by character or in chunks
        /// </summary>
        /// <param name="filePath">Path to file to read</param>
        private static void ReadUsingReadMethod(string filePath)
        {
            // Output: --- Read() Method Demo ---
            Console.WriteLine("--- Read() Method Demo ---");
            
            // using statement = ensures StreamReader is disposed properly
            // new StreamReader(path) = opens file for reading
            using (StreamReader reader = new StreamReader(filePath))
            {
                // char[] buffer = array to store read characters
                // new char[10] = buffer size of 10 characters
                char[] buffer = new char[10];
                
                // int charsRead = number of characters actually read
                // int totalChars = cumulative count of characters read
                int charsRead;
                int totalChars = 0;
                
                // Output: Reading 10 characters at a time:
                Console.WriteLine("Reading 10 characters at a time:");
                
                // while loop = read until end of file (returns 0)
                // reader.Read(buffer, offset, count) = reads into buffer at offset
                while ((charsRead = reader.Read(buffer, 0, buffer.Length)) > 0)
                {
                    // new string(buffer, offset, count) = creates string from char array
                    string chunk = new string(buffer, 0, charsRead);
                    
                    // Output: Chunk [n]: "[content]"
                    Console.WriteLine($"  Chunk {totalChars / 10 + 1}: \"{chunk}\"");
                    totalChars += charsRead;
                    
                    // Break after 50 characters for demo purposes
                    if (totalChars > 50) break;
                }
                
                // Output: Total characters read: [count]
                Console.WriteLine($"  Total characters read: {totalChars}");
            }
        }

        /// <summary>
        /// Demonstrates StreamReader.ReadLine() - reading line by line
        /// </summary>
        /// <param name="filePath">Path to file to read</param>
        private static void ReadUsingReadLineMethod(string filePath)
        {
            // Output: --- ReadLine() Method Demo ---
            Console.WriteLine("--- ReadLine() Method Demo ---");
            
            // using statement = ensures StreamReader is disposed
            using (StreamReader reader = new StreamReader(filePath))
            {
                // int lineNumber = counter for current line number
                int lineNumber = 0;
                
                // string? = nullable string (can be null at end of file)
                string? line;
                
                // Output: Reading line by line:
                Console.WriteLine("Reading line by line:");
                
                // while loop = read each line until null (end of file)
                // reader.ReadLine() = returns next line or null
                while ((line = reader.ReadLine()) != null)
                {
                    lineNumber++; // Increment line counter
                    
                    // string.IsNullOrEmpty = checks for null or empty string
                    // Ternary operator = "(condition) ? (true) : (false)"
                    string displayLine = string.IsNullOrEmpty(line) ? "(empty)" : line;
                    
                    // Output: Line [n]: [content]
                    Console.WriteLine($"  Line {lineNumber}: {displayLine}");
                    
                    // Stop after 5 lines for demo
                    if (lineNumber >= 5) break;
                }
            }
            
            // NOTE: ReadLine reads until newline character, not including the newline
            // Output: Each line is displayed individually
            Console.WriteLine("// Output: Each line is displayed individually");
        }

        /// <summary>
        /// Demonstrates StreamReader.ReadToEnd() - reading entire remaining content
        /// </summary>
        /// <param name="filePath">Path to file to read</param>
        private static void ReadUsingReadToEndMethod(string filePath)
        {
            // Output: --- ReadToEnd() Method Demo ---
            Console.WriteLine("--- ReadToEnd() Method Demo ---");
            
            // using statement = ensures StreamReader is disposed
            using (StreamReader reader = new StreamReader(filePath))
            {
                // reader.ReadToEnd() = reads from current position to end of file
                // string content = stores entire file content
                string content = reader.ReadToEnd();
                
                // string.Split with StringSplitOptions = splits content into lines
                // new[] {"\r\n", "\r", "\n"} = split on all common newline types
                // StringSplitOptions.None = include empty entries from consecutive newlines
                string[] allLines = content.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                
                // Output: Total content length: [n] characters
                Console.WriteLine($"Total content length: {content.Length} characters");
                // Output: Total lines: [n]
                Console.WriteLine($"Total lines: {allLines.Length}");
                Console.WriteLine();
                
                // Output: Full content:
                Console.WriteLine("Full content:");
                // Output: [entire file content]
                Console.WriteLine(content);
            }
            
            // CAUTION: ReadToEnd loads entire file into memory - not suitable for large files
            // Output: Entire file content displayed at once
            Console.WriteLine("// Output: Entire file content displayed at once");
        }

        /// <summary>
        /// Demonstrates reading with specific encoding detection
        /// </summary>
        /// <param name="filePath">Path to file to read</param>
        private static void ReadWithSpecificEncoding(string filePath)
        {
            // Output: --- Reading with Specific Encoding ---
            Console.WriteLine("--- Reading with Specific Encoding ---");
            
            // new StreamReader(path, encoding, detectEncoding) = constructor with encoding
            // detectEncoding = true means auto-detect byte order mark
            using (StreamReader reader = new StreamReader(filePath, Encoding.UTF8, true))
            {
                // string content = read all content
                string content = reader.ReadToEnd();
                
                // reader.CurrentEncoding = encoding being used for reading
                // Encoding.EncodingName = human-readable encoding name
                // Output: Encoding detected: [name]
                Console.WriteLine($"Encoding detected: {reader.CurrentEncoding.EncodingName}");
                
                // string.Substring = extract portion of string
                // Math.Min(50, content.Length) = use smaller of 50 or actual length
                // Output: Content sample: [first 50 chars]...
                Console.WriteLine($"Content sample: {content.Substring(0, Math.Min(50, content.Length))}...");
            }
            
            // Output: Content read with UTF-8 encoding
            Console.WriteLine("// Output: Content read with UTF-8 encoding");
        }

        // ── REAL-WORLD EXAMPLE: Log File Processor ──────────────────────────
        /// <summary>
        /// Real-world example: Processing application log files for analysis
        /// Demonstrates practical use of StreamReader in production applications
        /// </summary>
        private static void RealWorldExample_ProcessLogFile()
        {
            // Output: (blank line)
            Console.WriteLine();
            // Output: === REAL-WORLD EXAMPLE: Log File Processor ===
            Console.WriteLine("=== REAL-WORLD EXAMPLE: Log File Processor ===");
            
            // string logFilePath = path for log file to create
            string logFilePath = "NN_application.log";
            
            // string[] logEntries = sample log entries mimicking real application logs
            string[] logEntries = new[]
            {
                "[2024-01-15 08:30:15] INFO  - Application started",
                "[2024-01-15 08:30:16] DEBUG - Loading configuration from config.json",
                "[2024-01-15 08:30:17] INFO  - Database connection established",
                "[2024-01-15 08:30:18] WARN  - Cache size exceeded, evicting old entries",
                "[2024-01-15 08:30:19] ERROR - Failed to connect to external API: timeout",
                "[2024-01-15 08:30:20] INFO  - Retrying external API connection",
                "[2024-01-15 08:30:25] INFO  - External API connected successfully",
                "[2024-01-15 08:30:30] INFO  - Processing batch job: 150 items",
                "[2024-01-15 08:35:00] INFO  - Batch job completed: 150/150 processed",
                "[2024-01-15 08:35:01] INFO  - Application shutting down"
            };

            // File.WriteAllLines = write array of lines to file
            File.WriteAllLines(logFilePath, logEntries);
            // Output: [Setup] Created log file: [path]
            Console.WriteLine($"[Setup] Created log file: {logFilePath}");

            // using statement = ensures StreamReader is properly disposed
            using (StreamReader reader = new StreamReader(logFilePath))
            {
                // int errorCount, warningCount, infoCount = counters for log levels
                int errorCount = 0;
                int warningCount = 0;
                int infoCount = 0;
                
                // string? line = current line being processed
                string? line;
                
                // while loop = read each line until end of file
                while ((line = reader.ReadLine()) != null)
                {
                    // string.Contains = case-sensitive substring search
                    // Check for each log level and increment respective counter
                    if (line.Contains("ERROR"))
                        errorCount++;
                    else if (line.Contains("WARN"))
                        warningCount++;
                    else if (line.Contains("INFO"))
                        infoCount++;
                }
                
                Console.WriteLine();
                // Output: Log Analysis Results:
                Console.WriteLine("Log Analysis Results:");
                // Output: Total INFO messages:  [n]
                Console.WriteLine($"  Total INFO messages:  {infoCount}");
                // Output: Total WARN messages: [n]
                Console.WriteLine($"  Total WARN messages: {warningCount}");
                // Output: Total ERROR messages: [n]
                Console.WriteLine($"  Total ERROR messages: {errorCount}");
                
                // Conditional logic based on error count
                if (errorCount > 0)
                    // Output: Status: ISSUES DETECTED - Review required!
                    Console.WriteLine("  Status: ISSUES DETECTED - Review required!");
                else
                    // Output: Status: All clear
                    Console.WriteLine("  Status: All clear");
            }
            
            // File.Delete = remove file from filesystem
            File.Delete(logFilePath);
            // Output: [Cleanup] Deleted: [path]
            Console.WriteLine($"[Cleanup] Deleted: {logFilePath}");
            // Output: Log file processed, statistics displayed
            Console.WriteLine("// Output: Log file processed, statistics displayed");
        }

        /// <summary>
        /// Cleans up test file if it exists
        /// </summary>
        /// <param name="filePath">Path to file to delete</param>
        private static void CleanupTestFile(string filePath)
        {
            // File.Exists = checks if file exists on filesystem
            if (File.Exists(filePath))
            {
                // File.Delete = removes file from filesystem
                File.Delete(filePath);
                // Output: [Cleanup] Deleted: [path]
                Console.WriteLine($"[Cleanup] Deleted: {filePath}");
            }
        }
    }
}
