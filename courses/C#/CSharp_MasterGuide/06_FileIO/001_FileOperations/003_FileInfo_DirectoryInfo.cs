/*
    TOPIC: C# File I/O Operations
    SUBTOPIC: FileInfo and DirectoryInfo Classes
    FILE: 03_FileInfo_DirectoryInfo.cs
    PURPOSE: Demonstrates FileInfo and DirectoryInfo for file/directory
             manipulation with object-oriented approach
*/

using System;
using System.IO;
using System.Linq;

namespace CSharp_MasterGuide._06_FileIO._01_FileOperations
{
    public class FileInfoDemo
    {
        private static readonly string BasePath = Path.Combine(Path.GetTempPath(), "FileInfo_Demo");
        
        public static void Main(string[] args)
        {
            SetupDirectory();
            
            DemonstrateFileInfo();
            DemonstrateDirectoryInfo();
            DemonstrateFileInfoProperties();
            DemonstrateDirectoryInfoOperations();
            DemonstrateRealWorldExamples();
            
            Console.WriteLine("\n=== FileInfo and DirectoryInfo demos completed ===");
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
        
        private static void DemonstrateFileInfo()
        {
            Console.WriteLine("\n=== FileInfo Basic Usage ===");
            
            string filePath = Path.Combine(BasePath, "test_file.txt");
            string content = "This is a test file for FileInfo demonstration.";
            File.WriteAllText(filePath, content);
            
            // Create FileInfo object
            FileInfo fileInfo = new FileInfo(filePath);
            
            // Display basic info
            Console.WriteLine($"File Name: {fileInfo.Name}");
            // Output: File Name: test_file.txt
            Console.WriteLine($"Full Path: {fileInfo.FullName}");
            // Output: Full Path: C:\Users\...\AppData\Local\Temp\FileInfo_Demo\test_file.txt
            Console.WriteLine($"Extension: {fileInfo.Extension}");
            // Output: Extension: .txt
            Console.WriteLine($"Directory: {fileInfo.DirectoryName}");
            // Output: Directory: C:\Users\...\AppData\Local\Temp\FileInfo_Demo
        }
        
        private static void DemonstrateDirectoryInfo()
        {
            Console.WriteLine("\n=== DirectoryInfo Basic Usage ===");
            
            string subDirPath = Path.Combine(BasePath, "sub_directory");
            Directory.CreateDirectory(subDirPath);
            
            // Create DirectoryInfo object
            DirectoryInfo dirInfo = new DirectoryInfo(subDirPath);
            
            // Display basic info
            Console.WriteLine($"Directory Name: {dirInfo.Name}");
            // Output: Directory Name: sub_directory
            Console.WriteLine($"Full Path: {dirInfo.FullName}");
            // Output: Full Path: C:\Users\...\AppData\Local\Temp\FileInfo_Demo\sub_directory
            Console.WriteLine($"Parent: {dirInfo.Parent?.Name}");
            // Output: Parent: FileInfo_Demo
            Console.WriteLine($"Root: {dirInfo.Root?.Name}");
            // Output: Root: C:\
            
            // Create nested DirectoryInfo
            DirectoryInfo nestedInfo = new DirectoryInfo(Path.Combine(BasePath, "level1", "level2", "level3"));
            nestedInfo.Create();
            Console.WriteLine($"Created nested: {nestedInfo.FullName}");
            // Output: Created nested: ...\level1\level2\level3
        }
        
        private static void DemonstrateFileInfoProperties()
        {
            Console.WriteLine("\n=== FileInfo Properties ===");
            
            string testFile = Path.Combine(BasePath, "properties_demo.txt");
            
            // Write some content with specific timing
            string content = "Sample content for property demonstration";
            File.WriteAllText(testFile, content);
            
            FileInfo fi = new FileInfo(testFile);
            
            // Existence check
            Console.WriteLine($"Exists: {fi.Exists}");
            // Output: Exists: True
            
            // Length (in bytes)
            Console.WriteLine($"Length: {fi.Length} bytes");
            // Output: Length: 44 bytes
            
            // Time properties
            Console.WriteLine($"Creation Time: {fi.CreationTime}");
            // Output: Creation Time: (date time)
            Console.WriteLine($"Last Access Time: {fi.LastAccessTime}");
            // Output: Last Access Time: (date time)
            Console.WriteLine($"Last Write Time: {fi.LastWriteTime}");
            // Output: Last Write Time: (date time)
            
            // Read-only and hidden attributes
            Console.WriteLine($"IsReadOnly: {fi.IsReadOnly}");
            // Output: IsReadOnly: False
            Console.WriteLine($"Attributes: {fi.Attributes}");
            // Output: Attributes: Normal
            
            // Check a non-existent file
            FileInfo nonExistent = new FileInfo(Path.Combine(BasePath, "nonexistent.txt"));
            Console.WriteLine($"\nNon-existent file:");
            Console.WriteLine($"Exists: {nonExistent.Exists}");
            // Output: Exists: False
            Console.WriteLine($"Length: {nonExistent.Length}");
            // Output: Length: 0
        }
        
        private static void DemonstrateDirectoryInfoOperations()
        {
            Console.WriteLine("\n=== DirectoryInfo Operations ===");
            
            DirectoryInfo di = new DirectoryInfo(BasePath);
            
            // Create files in the directory
            string[] files = { "file1.txt", "file2.txt", "file3.txt" };
            foreach (string fname in files)
            {
                File.WriteAllText(Path.Combine(BasePath, fname), $"Content of {fname}");
            }
            
            // Create subdirectories
            Directory.CreateDirectory(Path.Combine(BasePath, "images"));
            Directory.CreateDirectory(Path.Combine(BasePath, "documents"));
            
            // GetFiles - returns FileInfo array
            FileInfo[] fileInfos = di.GetFiles();
            Console.WriteLine("Files in directory:");
            foreach (FileInfo f in fileInfos)
            {
                Console.WriteLine($"  {f.Name} ({f.Length} bytes)");
                // Output: file1.txt (17 bytes), etc.
            }
            
            // GetDirectories - returns DirectoryInfo array
            DirectoryInfo[] dirInfos = di.GetDirectories();
            Console.WriteLine("\nSubdirectories:");
            foreach (DirectoryInfo d in dirInfos)
            {
                Console.WriteLine($"  {d.Name}");
                // Output: sub_directory, images, documents, level1
            }
            
            // GetFileSystemInfos - both files and directories
            FileSystemInfo[] allItems = di.GetFileSystemInfos();
            Console.WriteLine($"\nTotal items: {allItems.Length}");
            // Output: Total items: 7 (3 files + 4 directories)
            
            // EnumerateFileSystemInfos (lazy enumeration)
            Console.WriteLine("\nLazy enumeration:");
            foreach (FileSystemInfo fsi in di.EnumerateFileSystemInfos())
            {
                string type = fsi is DirectoryInfo ? "DIR" : "FILE";
                Console.WriteLine($"  [{type}] {fsi.Name}");
            }
        }
        
        private static void DemonstrateRealWorldExamples()
        {
            Console.WriteLine("\n=== Real-World Examples ===");
            
            // Example 1: File search with filtering
            SearchFilesWithFilter();
            
            // Example 2: Directory size calculator
            CalculateDirectorySize();
            
            // Example 3: Recent files finder
            FindRecentFiles();
            
            // Example 4: Directory tree display
            DisplayDirectoryTree();
        }
        
        private static void SearchFilesWithFilter()
        {
            Console.WriteLine("\n--- File Search with Filtering ---");
            
            // Create test files with different extensions
            string[] testFiles = { "data.csv", "report.txt", "image.png", "backup.bak", "config.json" };
            foreach (string f in testFiles)
            {
                File.WriteAllText(Path.Combine(BasePath, f), "test");
            }
            
            DirectoryInfo di = new DirectoryInfo(BasePath);
            
            // Search for text files
            FileInfo[] textFiles = di.GetFiles("*.txt");
            Console.WriteLine("Text files (*.txt):");
            foreach (FileInfo f in textFiles)
            {
                Console.WriteLine($"  {f.Name}");
                // Output: report.txt
            }
            
            // Search for multiple patterns
            FileInfo[] dataFiles = di.GetFiles("*.csv;*.json".Split(';'));
            Console.WriteLine("\nData files (*.csv, *.json):");
            foreach (FileInfo f in dataFiles)
            {
                Console.WriteLine($"  {f.Name}");
                // Output: data.csv, config.json
            }
            
            // Case-insensitive search
            FileInfo[] allFiles = di.GetFiles("*.*", SearchOption.TopDirectoryOnly);
            Console.WriteLine($"\nAll files (case-insensitive): {allFiles.Length}");
            // Output: All files: 9 (includes previous demo files)
        }
        
        private static void CalculateDirectorySize()
        {
            Console.WriteLine("\n--- Directory Size Calculator ---");
            
            // Create nested directory structure with files
            string testDir = Path.Combine(BasePath, "size_test");
            if (Directory.Exists(testDir)) Directory.Delete(testDir, true);
            
            Directory.CreateDirectory(Path.Combine(testDir, "sub1"));
            Directory.CreateDirectory(Path.Combine(testDir, "sub2"));
            
            // Create files of known sizes
            string file1 = Path.Combine(testDir, "file1.txt");
            string file2 = Path.Combine(testDir, "sub1", "file2.txt");
            string file3 = Path.Combine(testDir, "sub2", "file3.txt");
            
            File.WriteAllText(file1, new string('A', 1000));  // ~1KB
            File.WriteAllText(file2, new string('B', 2048));  // ~2KB
            File.WriteAllText(file3, new string('C', 512));   // ~0.5KB
            
            long totalSize = CalculateSize(new DirectoryInfo(testDir));
            Console.WriteLine($"Total size of '{testDir}': {totalSize} bytes");
            // Output: Total size: 3560 bytes (including newlines)
            
            Console.WriteLine($"Formatted: {totalSize / 1024.0:F2} KB");
            // Output: Formatted: 3.48 KB
        }
        
        private static long CalculateSize(DirectoryInfo dirInfo)
        {
            long size = 0;
            try
            {
                foreach (FileInfo file in dirInfo.GetFiles())
                {
                    size += file.Length;
                }
                foreach (DirectoryInfo subDir in dirInfo.GetDirectories())
                {
                    size += CalculateSize(subDir);
                }
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine($"Access denied to: {dirInfo.FullName}");
            }
            return size;
        }
        
        private static void FindRecentFiles()
        {
            Console.WriteLine("\n--- Recent Files Finder ---");
            
            // Create some files with different timestamps
            string recentFile = Path.Combine(BasePath, "recent.txt");
            string oldFile = Path.Combine(BasePath, "old.txt");
            string veryOldFile = Path.Combine(BasePath, "veryold.txt");
            
            File.WriteAllText(recentFile, "recent content");
            File.WriteAllText(oldFile, "old content");
            File.WriteAllText(veryOldFile, "very old content");
            
            // Set different access times
            File.SetLastAccessTime(oldFile, DateTime.Now.AddDays(-30));
            File.SetLastAccessTime(veryOldFile, DateTime.Now.AddDays(-90));
            
            DirectoryInfo di = new DirectoryInfo(BasePath);
            TimeSpan recentThreshold = TimeSpan.FromDays(7);
            
            FileInfo[] recentFiles = di.GetFiles()
                .Where(f => (DateTime.Now - f.LastAccessTime) < recentThreshold)
                .OrderByDescending(f => f.LastAccessTime)
                .ToArray();
            
            Console.WriteLine($"Files accessed in last 7 days:");
            foreach (FileInfo f in recentFiles)
            {
                Console.WriteLine($"  {f.Name} - Last accessed: {f.LastAccessTime:g}");
            }
            
            // Old files (more than 30 days)
            FileInfo[] oldFiles = di.GetFiles()
                .Where(f => (DateTime.Now - f.LastAccessTime).TotalDays > 30)
                .ToArray();
            
            Console.WriteLine($"\nFiles not accessed in 30+ days: {oldFiles.Length}");
            foreach (FileInfo f in oldFiles)
            {
                Console.WriteLine($"  {f.Name}");
            }
        }
        
        private static void DisplayDirectoryTree()
        {
            Console.WriteLine("\n--- Directory Tree Display ---");
            
            string treeRoot = Path.Combine(BasePath, "tree_root");
            if (Directory.Exists(treeRoot)) Directory.Delete(treeRoot, true);
            
            // Create structure
            Directory.CreateDirectory(Path.Combine(treeRoot, "src"));
            Directory.CreateDirectory(Path.Combine(treeRoot, "src", "models"));
            Directory.CreateDirectory(Path.Combine(treeRoot, "src", "views"));
            Directory.CreateDirectory(Path.Combine(treeRoot, "tests"));
            Directory.CreateDirectory(Path.Combine(treeRoot, "docs"));
            
            File.WriteAllText(Path.Combine(treeRoot, "README.md"), "# Project");
            File.WriteAllText(Path.Combine(treeRoot, "src", "Program.cs"), "class Program {}");
            File.WriteAllText(Path.Combine(treeRoot, "src", "models", "User.cs"), "class User {}");
            File.WriteAllText(Path.Combine(treeRoot, "docs", "guide.md"), "# Guide");
            
            PrintTree(new DirectoryInfo(treeRoot), "", true);
        }
        
        private static void PrintTree(DirectoryInfo dir, string indent, bool isLast)
        {
            string branch = isLast ? "└── " : "├── ";
            Console.WriteLine($"{indent}{branch}{dir.Name}/");
            
            string childIndent = indent + (isLast ? "    " : "│   ");
            
            FileInfo[] files = dir.GetFiles();
            DirectoryInfo[] dirs = dir.GetDirectories();
            
            int totalItems = files.Length + dirs.Length;
            int current = 0;
            
            foreach (FileInfo file in files)
            {
                current++;
                string fileBranch = current == totalItems ? "└── " : "├── ";
                Console.WriteLine($"{childIndent}{fileBranch}{file.Name}");
            }
            
            foreach (DirectoryInfo subDir in dirs)
            {
                current++;
                PrintTree(subDir, childIndent, current == totalItems);
            }
        }
    }
}