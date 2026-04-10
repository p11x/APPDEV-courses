/*
TOPIC: C# File I/O Operations
SUBTOPIC: StreamReader and StreamWriter
FILE: 02_StreamReader_Part2.cs
PURPOSE: More StreamReader features - encoding, async operations, Peek, Lines
*/

using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CSharp_MasterGuide._06_FileIO._02_StreamReaderWriter
{
    public class NN_02_StreamReader_Part2
    {
        public static async Task Main(string[] args)
        {
            string testFilePath = CreateTestFile();
            
            Console.WriteLine("=== StreamReader Advanced Features Demo ===");
            Console.WriteLine();

            AsyncReadMethods(testFilePath);
            Console.WriteLine();

            PeekAndLookahead(testFilePath);
            Console.WriteLine();

            EncodingHandling(testFilePath);
            Console.WriteLine();

            UsingLinesProperty(filePath: testFilePath);
            Console.WriteLine();

            RealWorldExample_AsyncLogProcessor();
            
            CleanupTestFile(testFilePath);
        }

        private static string CreateTestFile()
        {
            string path = "NN_test_advanced.txt";
            string content = @"Header: Application Log
Timestamp: 2024-01-15T10:30:00Z
Level: INFO
Message: System initialized successfully

Timestamp: 2024-01-15T10:30:01Z
Level: DEBUG
Message: Loading user preferences

Timestamp: 2024-01-15T10:30:02Z
Level: ERROR
Message: Connection timeout - retrying

Timestamp: 2024-01-15T10:30:05Z
Level: INFO
Message: Connection restored

Footer: End of log";

            File.WriteAllText(path, content, Encoding.UTF8);
            Console.WriteLine($"[Setup] Created test file: {path}");
            return path;
        }

        private static async void AsyncReadMethods(string filePath)
        {
            Console.WriteLine("--- Async Reading Methods ---");
            
            using (StreamReader reader = new StreamReader(filePath))
            {
                string content = await reader.ReadToEndAsync();
                Console.WriteLine($"Async ReadToEndAsync completed: {content.Length} chars");
                Console.WriteLine($"Content preview: {content.Substring(0, Math.Min(60, content.Length))}...");
            }
            
            using (StreamReader reader = new StreamReader(filePath))
            {
                char[] buffer = new char[20];
                int count = await reader.ReadAsync(buffer, 0, buffer.Length);
                string result = new string(buffer, 0, count);
                Console.WriteLine($"Async ReadAsync: {count} chars - \"{result}\"");
            }
            
            using (StreamReader reader = new StreamReader(filePath))
            {
                string? line = await reader.ReadLineAsync();
                Console.WriteLine($"Async ReadLineAsync: \"{line}\"");
            }
            
            Console.WriteLine("// Output: Async operations complete without blocking");
        }

        private static void PeekAndLookahead(string filePath)
        {
            Console.WriteLine("--- Peek() Method Demo ---");
            
            using (StreamReader reader = new StreamReader(filePath))
            {
                Console.WriteLine("Checking first character without consuming:");
                
                int firstChar = reader.Peek();
                if (firstChar != -1)
                {
                    char first = (char)firstChar;
                    Console.WriteLine($"  Peek() returns: '{(char)firstChar}' (int: {firstChar})");
                }
                
                string firstLine = reader.ReadLine();
                Console.WriteLine($"  ReadLine() returns: \"{firstLine}\"");
                
                int secondPeek = reader.Peek();
                Console.WriteLine($"  Peek() after read: '{(char)secondPeek}' (int: {secondPeek})");
            }
            
            Console.WriteLine();
            Console.WriteLine("--- Reading Specific Positions ---");
            
            using (StreamReader reader = new StreamReader(filePath))
            {
                reader.BaseStream.Seek(10, SeekOrigin.Begin);
                char[] buffer = new char[15];
                int read = reader.Read(buffer, 0, buffer.Length);
                string section = new string(buffer, 0, read);
                Console.WriteLine($"  Seek to position 10, read 15 chars: \"{section}\"");
            }
            
            Console.WriteLine("// Output: Peek shows next char without consuming");
        }

        private static void EncodingHandling(string filePath)
        {
            Console.WriteLine("--- Encoding Handling Demo ---");
            
            string utf8File = "NN_test_utf8.txt";
            string utf16File = "NN_test_utf16.txt";
            
            File.WriteAllText(utf8File, "Hello UTF-8 World! 日本語", new UTF8Encoding(false));
            File.WriteAllText(utf16File, "Hello UTF-16 World! 日本語", new UnicodeEncoding(false, true));
            
            Console.WriteLine("Reading UTF-8 file:");
            using (StreamReader reader = new StreamReader(utf8File, Encoding.UTF8, false, 1024, true))
            {
                string content = reader.ReadToEnd();
                Console.WriteLine($"  Encoding: {reader.CurrentEncoding.EncodingName}");
                Console.WriteLine($"  Content: {content}");
            }
            
            Console.WriteLine("Reading UTF-16 file:");
            using (StreamReader reader = new StreamReader(utf16File, Encoding.Unicode, false, 1024, true))
            {
                string content = reader.ReadToEnd();
                Console.WriteLine($"  Encoding: {reader.CurrentEncoding.EncodingName}");
                Console.WriteLine($"  Content: {content}");
            }
            
            File.Delete(utf8File);
            File.Delete(utf16File);
            
            Console.WriteLine("// Output: Different encodings handled correctly");
        }

        private static void UsingLinesProperty(string filePath)
        {
            Console.WriteLine("--- Using StreamReader.Lines Property (C# 8+) ---");
            
            using (StreamReader reader = new StreamReader(filePath))
            {
                int lineCount = 0;
                foreach (var line in reader.Lines)
                {
                    lineCount++;
                    if (lineCount <= 3)
                        Console.WriteLine($"  Line {lineCount}: {line}");
                }
                Console.WriteLine($"  Total lines: {lineCount}");
            }
            
            Console.WriteLine("// Output: Lines property enables foreach without explicit reading");
        }

        private static async Task RealWorldExample_AsyncLogProcessor()
        {
            Console.WriteLine();
            Console.WriteLine("=== REAL-WORLD EXAMPLE: Async Large File Processor ===");
            
            string largeLogFile = "NN_large_log.txt";
            StringBuilder sb = new StringBuilder();
            
            for (int i = 0; i < 1000; i++)
            {
                sb.AppendLine($"[{DateTime.Now.AddSeconds(i):yyyy-MM-dd HH:mm:ss}] INFO - Processing item {i:D4}");
            }
            await File.WriteAllTextAsync(largeLogFile, sb.ToString());
            
            Console.WriteLine($"[Setup] Created large log file: {largeLogFile} ({new FileInfo(largeLogFile).Length} bytes)");
            
            int errorCount = 0;
            int totalLines = 0;
            
            using (StreamReader reader = new StreamReader(largeLogFile))
            {
                await foreach (var line in reader.ReadLinesAsync())
                {
                    totalLines++;
                    if (line.Contains("ERROR"))
                        errorCount++;
                }
            }
            
            Console.WriteLine($"  Processed {totalLines} lines asynchronously");
            Console.WriteLine($"  Errors found: {errorCount}");
            
            File.Delete(largeLogFile);
            Console.WriteLine($"[Cleanup] Deleted: {largeLogFile}");
            Console.WriteLine("// Output: Large file processed efficiently with async/await");
        }

        private static void CleanupTestFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                Console.WriteLine($"[Cleanup] Deleted: {filePath}");
            }
        }
    }

    public static class StreamReaderExtensions
    {
        public static IAsyncEnumerable<string> ReadLinesAsync(this StreamReader reader)
        {
            return ReadLinesAsyncIterator(reader);
        }

        private static async IAsyncEnumerable<string> ReadLinesAsyncIterator(StreamReader reader)
        {
            string? line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                yield return line;
            }
        }
    }
}