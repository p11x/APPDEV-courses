/*
================================================================================
TOPIC 21: FILE HANDLING
================================================================================

File handling allows reading and writing files on disk.

TABLE OF CONTENTS:
1. Reading Files
2. Writing Files
3. File Classes
4. Working with Paths
================================================================================
*/

using System;
using System.IO;

namespace FileHandlingExamples
{
    class Program
    {
        static void Main()
        {
            string filePath = "test.txt";
            
            // Writing to file
            Console.WriteLine("=== Writing to File ===");
            string[] lines = { "Hello, World!", "Line 2", "Line 3" };
            File.WriteAllLines(filePath, lines);
            Console.WriteLine("File written successfully");
            
            // Reading from file
            Console.WriteLine("\n=== Reading from File ===");
            if (File.Exists(filePath))
            {
                string content = File.ReadAllText(filePath);
                Console.WriteLine(content);
                
                string[] readLines = File.ReadAllLines(filePath);
                foreach (string line in readLines)
                {
                    Console.WriteLine(line);
                }
            }
            
            // Appending to file
            Console.WriteLine("\n=== Appending to File ===");
            File.AppendAllText(filePath, "\nAppended line");
            
            // FileInfo
            Console.WriteLine("\n=== File Info ===");
            FileInfo info = new FileInfo(filePath);
            Console.WriteLine($"Name: {info.Name}");
            Console.WriteLine($"Size: {info.Length}");
            Console.WriteLine($"Created: {info.CreationTime}");
            
            // Directory operations
            Console.WriteLine("\n=== Directory Operations ===");
            string dirPath = "testdir";
            Directory.CreateDirectory(dirPath);
            Console.WriteLine($"Directory created: {Directory.Exists(dirPath)}");
            
            // Cleanup
            if (File.Exists(filePath))
                File.Delete(filePath);
            if (Directory.Exists(dirPath))
                Directory.Delete(dirPath);
            
            Console.WriteLine("Files cleaned up");
        }
    }
}

/*
FILE HANDLING CLASSES:
----------------------
File        - Static methods for file operations
FileInfo    - Instance methods, more control
Directory   - Static methods for directories
DirectoryInfo - Directory instance methods
Path        - Work with path strings

COMMON METHODS:
--------------
ReadAllText()    - Read entire file as string
WriteAllText()   - Write string to file
ReadAllLines()   - Read as string array
WriteAllLines()  - Write array of lines
AppendAllText()  - Append to file
*/

// ================================================================================
// NEXT STEPS
// =============================================================================

/*
NEXT: Topic 22 covers LINQ.
*/
