/*
    TOPIC: C# File I/O Operations
    SUBTOPIC: Path Class Operations
    FILE: 06_PathOperations.cs
    PURPOSE: Demonstrates Path class methods for file path manipulation
             including GetFileName, GetExtension, Combine, and more
*/

using System;
using System.IO;

namespace CSharp_MasterGuide._06_FileIO._01_FileOperations
{
    public class PathOperationsDemo
    {
        private static readonly string BasePath = Path.Combine(Path.GetTempPath(), "Path_Demo");
        
        public static void Main(string[] args)
        {
            SetupDirectory();
            
            DemonstrateGetFileName();
            DemonstrateGetExtension();
            DemonstrateGetDirectoryName();
            DemonstrateCombine();
            DemonstratePathSeparators();
            DemonstratePathManipulation();
            DemonstrateRealWorldExamples();
            
            Console.WriteLine("\n=== Path operations demos completed ===");
        }
        
        private static void SetupDirectory()
        {
            if (!Directory.Exists(BasePath))
            {
                Directory.CreateDirectory(BasePath);
            }
            Console.WriteLine($"Working directory: {BasePath}");
        }
        
        private static void DemonstrateGetFileName()
        {
            Console.WriteLine("\n=== Path.GetFileName Demo ===");
            
            string[] paths = 
            {
                @"C:\Users\John\Documents\file.txt",
                "/home/user/documents/image.png",
                "simple_filename.cs",
                @"C:\temp\",
                "noextension",
                @"C:\Users\Documents\..\file.txt"
            };
            
            foreach (string path in paths)
            {
                string fileName = Path.GetFileName(path);
                Console.WriteLine($"Input: {path}");
                Console.WriteLine($"  GetFileName: {fileName}");
                // Output examples:
                // Input: C:\Users\John\Documents\file.txt
                //   GetFileName: file.txt
                // Input: simple_filename.cs
                //   GetFileName: simple_filename.cs
                // Input: C:\temp\
                //   GetFileName: (empty string)
            }
            
            // GetFileNameWithoutExtension
            Console.WriteLine("\n--- Without Extension ---");
            string fileWithExt = @"C:\data\document.pdf";
            string nameWithoutExt = Path.GetFileNameWithoutExtension(fileWithExt);
            Console.WriteLine($"Original: {fileWithExt}");
            Console.WriteLine($"Without extension: {nameWithoutExt}");
            // Output: Without extension: document
        }
        
        private static void DemonstrateGetExtension()
        {
            Console.WriteLine("\n=== Path.GetExtension Demo ===");
            
            string[] paths = 
            {
                @"C:\files\document.txt",
                "/usr/local/image.png",
                "script.js",
                "archive.tar.gz",
                "noextensionatall",
                @"C:\path\to\file."
            };
            
            foreach (string path in paths)
            {
                string extension = Path.GetExtension(path);
                Console.WriteLine($"Path: {path}");
                Console.WriteLine($"  Extension: {extension}");
                // Output examples:
                // Path: C:\files\document.txt
                //   Extension: .txt
                // Path: archive.tar.gz
                //   Extension: .gz
            }
            
            // Check if has extension
            string fileWithExt = "file.txt";
            string fileNoExt = "README";
            Console.WriteLine($"\n'{fileWithExt}' has extension: {Path.HasExtension(fileWithExt)}");
            // Output: 'file.txt' has extension: True
            Console.WriteLine($"'{fileNoExt}' has extension: {Path.HasExtension(fileNoExt)}");
            // Output: 'README' has extension: False
        }
        
        private static void DemonstrateGetDirectoryName()
        {
            Console.WriteLine("\n=== Path.GetDirectoryName Demo ===");
            
            string[] paths = 
            {
                @"C:\Users\John\Documents\file.txt",
                "/home/user/project/source.cs",
                "filename.txt",
                @"C:\root\",
                @"C:\"
            };
            
            foreach (string path in paths)
            {
                string directory = Path.GetDirectoryName(path);
                Console.WriteLine($"Path: {path}");
                Console.WriteLine($"  Directory: {directory}");
                // Output examples:
                // Path: C:\Users\John\Documents\file.txt
                //   Directory: C:\Users\John\Documents
            }
            
            // GetPathRoot
            Console.WriteLine("\n--- GetPathRoot ---");
            string[] roots = 
            {
                @"C:\Users\file.txt",
                "/home/user/file.txt",
                "relative/file.txt"
            };
            
            foreach (string path in roots)
            {
                string root = Path.GetPathRoot(path);
                Console.WriteLine($"Path: {path}");
                Console.WriteLine($"  Root: {root}");
                // Output:
                // Path: C:\Users\file.txt
                //   Root: C:\
            }
        }
        
        private static void DemonstrateCombine()
        {
            Console.WriteLine("\n=== Path.Combine Demo ===");
            
            // Two-path combine
            string part1 = @"C:\Users\John";
            string part2 = "Documents";
            string combined2 = Path.Combine(part1, part2);
            Console.WriteLine($"Combine(\"{part1}\", \"{part2}\") = {combined2}");
            // Output: C:\Users\John\Documents
            
            // Three-path combine
            string part3 = "file.txt";
            string combined3 = Path.Combine(part1, part2, part3);
            Console.WriteLine($"Combine 3 paths = {combined3}");
            // Output: C:\Users\John\Documents\file.txt
            
            // Array combine
            string[] paths = { @"C:\", "Users", "John", "Documents", "file.txt" };
            string combinedArray = Path.Combine(paths);
            Console.WriteLine($"Combine array = {combinedArray}");
            // Output: C:\Users\John\Documents\file.txt
            
            // Handling trailing separators
            string withSlash = @"C:\Users\John\";
            string noSlash = @"C:\Users\John";
            Console.WriteLine($"Trail slash: {Path.Combine(withSlash, "Docs")}");
            // Output: C:\Users\John\Docs
            Console.WriteLine($"No trail: {Path.Combine(noSlash, "Docs")}");
            // Output: C:\Users\John\Docs
            
            // Relative path combine
            string baseDir = "/home/user";
            string relativeDir = "../other/project";
            string combinedRelative = Path.Combine(baseDir, relativeDir);
            Console.WriteLine($"Relative combine: {combinedRelative}");
            // Output: /home/user/../other/project (not normalized)
        }
        
        private static void DemonstratePathSeparators()
        {
            Console.WriteLine("\n=== Path Separators ===");
            
            // DirectorySeparatorChar
            char dirSep = Path.DirectorySeparatorChar;
            Console.WriteLine($"Directory separator: '{dirSep}' ({(int)dirSep})");
            // Output: Directory separator: '\' (92)
            
            // AltDirectorySeparatorChar
            char altSep = Path.AltDirectorySeparatorChar;
            Console.WriteLine($"Alt separator: '{altSep}' ({(int)altSep})");
            // Output: Alt separator: '/' (47)
            
            // PathSeparator
            char pathSep = Path.PathSeparator;
            Console.WriteLine($"Path separator: '{pathSep}' ({(int)pathSep})");
            // Output: Path separator: ';' (59)
            
            // Build path using separator
            string builtPath = "Folder1" + Path.DirectorySeparatorChar + "Folder2" + 
                              Path.DirectorySeparatorChar + "file.txt";
            Console.WriteLine($"Built path: {builtPath}");
            // Output: Built path: Folder1\Folder2\file.txt
            
            // In Linux, DirectorySeparatorChar is '/'
            // Console.WriteLine($"On Linux: {Path.DirectorySeparatorChar}");
        }
        
        private static void DemonstratePathManipulation()
        {
            Console.WriteLine("\n=== Path Manipulation Methods ===");
            
            // ChangeExtension
            string original = @"C:\files\document.txt";
            string changedExt = Path.ChangeExtension(original, ".md");
            Console.WriteLine($"ChangeExtension: {original} -> {changedExt}");
            // Output: C:\files\document.md
            
            string noExt = @"C:\files\readme";
            string addedExt = Path.ChangeExtension(noExt, ".txt");
            Console.WriteLine($"Add extension: {noExt} -> {addedExt}");
            // Output: C:\files\readme.txt
            
            // Join (similar to Combine but modern)
            string joined = Path.Join(@"C:\Users", "John", "Documents", "file.txt");
            Console.WriteLine($"Path.Join: {joined}");
            // Output: C:\Users\John\Documents\file.txt
            
            // TrimEndingDirectorySeparator
            string withEndSep = @"C:\Users\John\";
            string trimmed = Path.TrimEndingDirectorySeparator(withEndSep);
            Console.WriteLine($"Trimmed: '{withEndSep}' -> '{trimmed}'");
            // Output: Trimmed: 'C:\Users\John\' -> 'C:\Users\John'
            
            // Ensure leading/trailing separator
            string normalized = Path.TrimEndingDirectorySeparator(
                Path.DirectorySeparatorChar.ToString() + "Users" + 
                Path.DirectorySeparatorChar + "John" + Path.DirectorySeparatorChar);
            Console.WriteLine($"Normalized: {normalized}");
        }
        
        private static void DemonstrateRealWorldExamples()
        {
            Console.WriteLine("\n=== Real-World Examples ===");
            
            // Example 1: File extension filter
            FilterByExtension();
            
            // Example 2: Build paths safely
            SafePathBuilder();
            
            // Example 3: Unique filename generator
            UniqueFilenameGenerator();
            
            // Example 4: Path normalization
            PathNormalization();
            
            // Example 5: Relative path calculation
            RelativePathCalculation();
        }
        
        private static void FilterByExtension()
        {
            Console.WriteLine("\n--- Extension Filter ---");
            
            // Create test files
            string[] testFiles = { "doc.txt", "image.png", "data.json", "report.pdf", "script.cs" };
            string testDir = Path.Combine(BasePath, "extensions");
            Directory.CreateDirectory(testDir);
            
            foreach (string file in testFiles)
            {
                File.WriteAllText(Path.Combine(testDir, file), "test");
            }
            
            // Filter by single extension
            string[] txtFiles = Directory.GetFiles(testDir, "*.txt");
            Console.WriteLine("Text files:");
            foreach (string f in txtFiles)
            {
                Console.WriteLine($"  {Path.GetFileName(f)}");
                // Output: doc.txt
            }
            
            // Filter by multiple extensions
            string[] imageAndJson = Directory.GetFiles(testDir)
                .Where(f => 
                {
                    string ext = Path.GetExtension(f).ToLower();
                    return ext == ".png" || ext == ".json";
                })
                .ToArray();
            
            Console.WriteLine("\nImage and JSON files:");
            foreach (string f in imageAndJson)
            {
                Console.WriteLine($"  {Path.GetFileName(f)}");
                // Output: image.png, data.json
            }
            
            // Group by extension
            var grouped = testFiles.GroupBy(f => Path.GetExtension(f));
            Console.WriteLine("\nFiles grouped by extension:");
            foreach (var group in grouped)
            {
                Console.WriteLine($"  {group.Key}: {string.Join(", ", group)}");
                // Output: .txt: doc.txt
                // Output: .png: image.png
                // etc.
            }
        }
        
        private static void SafePathBuilder()
        {
            Console.WriteLine("\n--- Safe Path Builder ---");
            
            string baseDir = @"C:\App\Data";
            string userInput = "..\..\windows\system32";  // Could be malicious
            
            // Always use Path.Combine, never string concatenation
            string combined = Path.Combine(baseDir, userInput);
            Console.WriteLine($"Combined: {combined}");
            // Output: C:\App\Data\..\..\windows\system32
            
            // GetFullPath normalizes the path
            string fullPath = Path.GetFullPath(combined);
            Console.WriteLine($"FullPath: {fullPath}");
            // Output: C:\windows\system32
            
            // Better: Validate the path stays within base
            if (!fullPath.StartsWith(baseDir) && !fullPath.StartsWith(Path.GetFullPath(baseDir)))
            {
                Console.WriteLine("WARNING: Path escapes base directory!");
            }
            else
            {
                Console.WriteLine("Path is within base directory");
            }
            
            // Safe file creation
            string fileName = Path.GetFileName("document.txt");  // Sanitizes input
            string safePath = Path.Combine(baseDir, fileName);
            Console.WriteLine($"Safe file path: {safePath}");
            // Output: C:\App\Data\document.txt
        }
        
        private static void UniqueFilenameGenerator()
        {
            Console.WriteLine("\n--- Unique Filename Generator ---");
            
            string targetDir = Path.Combine(BasePath, "unique_test");
            Directory.CreateDirectory(targetDir);
            
            string baseName = "document";
            string extension = ".txt";
            
            // Generate unique filename
            string uniqueName = GetUniqueFileName(targetDir, baseName, extension);
            Console.WriteLine($"Generated unique name: {uniqueName}");
            // Output: document.txt
            
            // Create it
            File.WriteAllText(Path.Combine(targetDir, uniqueName), "first");
            
            // Generate another
            uniqueName = GetUniqueFileName(targetDir, baseName, extension);
            Console.WriteLine($"Next unique name: {uniqueName}");
            // Output: document_1.txt
            
            // Create with timestamp
            string timestampName = GenerateTimestampFilename("report", ".pdf");
            Console.WriteLine($"Timestamp name: {timestampName}");
            // Output: report_2024-01-15_10-30-45.pdf
        }
        
        private static string GetUniqueFileName(string directory, string baseName, string extension)
        {
            string path = Path.Combine(directory, baseName + extension);
            if (!File.Exists(path))
            {
                return baseName + extension;
            }
            
            int counter = 1;
            while (File.Exists(Path.Combine(directory, $"{baseName}_{counter}{extension}")))
            {
                counter++;
            }
            
            return $"{baseName}_{counter}{extension}";
        }
        
        private static string GenerateTimestampFilename(string baseName, string extension)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            return $"{baseName}_{timestamp}{extension}";
        }
        
        private static void PathNormalization()
        {
            Console.WriteLine("\n--- Path Normalization ---");
            
            // Mixed separators
            string mixed = "C:/Users\\John/Documents\\file.txt";
            string normalized = Path.GetFullPath(mixed);
            Console.WriteLine($"Mixed: {mixed}");
            // Output: Mixed: C:/Users\John/Documents\file.txt
            Console.WriteLine($"Normalized: {normalized}");
            // Output: Normalized: C:\Users\John\Documents\file.txt
            
            // Relative paths
            string withDots = @"C:\Users\John\..\Jane\Documents\file.txt";
            string resolved = Path.GetFullPath(withDots);
            Console.WriteLine($"With dots: {withDots}");
            Console.WriteLine($"Resolved: {resolved}");
            // Output: Resolved: C:\Users\Jane\Documents\file.txt
            
            // Trim unnecessary separators
            string extraSlashes = "C:\\\\Users\\\\John\\\\Documents";
            Console.WriteLine($"Extra slashes: {extraSlashes}");
            
            // Check if path is rooted
            string[] testPaths = { @"C:\file.txt", "/home/user/file", "relative/file.txt" };
            Console.WriteLine("\nPath rooted check:");
            foreach (string p in testPaths)
            {
                bool isRooted = Path.IsPathRooted(p);
                Console.WriteLine($"  '{p}' -> IsRooted: {isRooted}");
                // Output: 'C:\file.txt' -> IsRooted: True
                // Output: '/home/user/file' -> IsRooted: True
                // Output: 'relative/file.txt' -> IsRooted: False
            }
            
            // Get relative path (when possible)
            string fromPath = @"C:\Users\John";
            string toPath = @"C:\Users\John\Documents\file.txt";
            try
            {
                string relative = Path.GetRelativePath(fromPath, toPath);
                Console.WriteLine($"Relative path: {relative}");
                // Output: Documents\file.txt
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        
        private static void RelativePathCalculation()
        {
            Console.WriteLine("\n--- Relative Path Calculation ---");
            
            string baseDir = @"C:\Projects\MyApp";
            string[] targetPaths = 
            {
                @"C:\Projects\MyApp\src\Program.cs",
                @"C:\Projects\MyApp\docs\readme.md",
                @"C:\Other\file.txt"
            };
            
            foreach (string target in targetPaths)
            {
                try
                {
                    string relative = Path.GetRelativePath(baseDir, target);
                    Console.WriteLine($"From '{baseDir}'");
                    Console.WriteLine($"  To '{target}'");
                    Console.WriteLine($"  Relative: {relative}");
                    // Output examples:
                    // Relative: src\Program.cs
                    // Relative: ..\..\Other\file.txt
                }
                catch (ArgumentException)
                {
                    Console.WriteLine($"  Cannot compute relative path");
                }
            }
            
            // Common use case: make paths relative to current working directory
            string cwd = Directory.GetCurrentDirectory();
            Console.WriteLine($"\nCurrent directory: {cwd}");
            
            // Get files relative to CWD
            string fullFilePath = Path.Combine(cwd, "data", "config.json");
            if (File.Exists(fullFilePath))
            {
                string relativeToCwd = Path.GetRelativePath(cwd, fullFilePath);
                Console.WriteLine($"File relative to CWD: {relativeToCwd}");
            }
        }
    }
}