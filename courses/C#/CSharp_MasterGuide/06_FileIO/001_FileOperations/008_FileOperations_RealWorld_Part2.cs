/*
    TOPIC: C# File I/O Operations
    SUBTOPIC: Real-World File Operations - Part 2
    FILE: 08_FileOperations_RealWorld_Part2.cs
    PURPOSE: Demonstrates more advanced real-world file operations
             including backup, file comparison, encryption, and streaming
*/

using System;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharp_MasterGuide._06_FileIO._01_FileOperations
{
    public class FileOperationsRealWorldPart2Demo
    {
        private static readonly string BasePath = Path.Combine(Path.GetTempPath(), "RealWorld_Part2_Demo");
        
        public static void Main(string[] args)
        {
            SetupDirectory();
            
            BackupAndRestoreDemo();
            FileComparisonDemo();
            SecureFileStorageDemo();
            LargeFileProcessingDemo();
            ArchiveManagementDemo();
            
            Console.WriteLine("\n=== Real-world Part 2 demos completed ===");
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
        
        private static void BackupAndRestoreDemo()
        {
            Console.WriteLine("\n=== Backup and Restore System ===");
            
            string sourceDir = Path.Combine(BasePath, "backup_source");
            string backupDir = Path.Combine(BasePath, "backups");
            Directory.CreateDirectory(sourceDir);
            Directory.CreateDirectory(backupDir);
            
            // Create source files
            File.WriteAllText(Path.Combine(sourceDir, "document1.txt"), "Important document content");
            File.WriteAllText(Path.Combine(sourceDir, "document2.txt"), "Another important document");
            File.WriteAllText(Path.Combine(sourceDir, "data.json"), "{\"key\": \"value\"}");
            
            // Create subdirectory with files
            string subDir = Path.Combine(sourceDir, "subfolder");
            Directory.CreateDirectory(subDir);
            File.WriteAllText(Path.Combine(subDir, "nested.txt"), "Nested file content");
            
            Console.WriteLine("Source files created:");
            Console.WriteLine($"  document1.txt");
            Console.WriteLine($"  document2.txt");
            Console.WriteLine($"  data.json");
            Console.WriteLine($"  subfolder/nested.txt");
            
            // Create backup
            string backupName = $"backup_{DateTime.Now:yyyyMMdd_HHmmss}";
            string backupPath = Path.Combine(backupDir, backupName);
            
            CreateBackup(sourceDir, backupPath);
            Console.WriteLine($"\nBackup created: {backupName}");
            
            // Verify backup contents
            string[] backedUpFiles = Directory.GetFiles(backupPath, "*.*", SearchOption.AllDirectories);
            Console.WriteLine($"Files in backup: {backedUpFiles.Length}");
            foreach (string f in backedUpFiles)
            {
                string relativePath = Path.GetRelativePath(backupPath, f);
                Console.WriteLine($"  {relativePath}");
                // Output: document1.txt, document2.txt, data.json, subfolder/nested.txt
            }
            
            // Restore from backup
            string restoreDir = Path.Combine(BasePath, "restored");
            RestoreBackup(backupPath, restoreDir);
            Console.WriteLine($"\nBackup restored to: {restoreDir}");
            
            // Verify restored files
            string restoredFile = Path.Combine(restoreDir, "document1.txt");
            Console.WriteLine($"Restored content: {File.ReadAllText(restoredFile)}");
            // Output: Restored content: Important document content
        }
        
        private static void CreateBackup(string sourceDir, string backupDir)
        {
            Directory.CreateDirectory(backupDir);
            
            string[] files = Directory.GetFiles(sourceDir, "*.*", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                string relativePath = Path.GetRelativePath(sourceDir, file);
                string destPath = Path.Combine(backupDir, relativePath);
                
                string destDir = Path.GetDirectoryName(destPath);
                if (!Directory.Exists(destDir))
                {
                    Directory.CreateDirectory(destDir);
                }
                
                File.Copy(file, destPath, true);
            }
            
            // Create backup metadata
            var metadata = new Dictionary<string, string>
            {
                { "BackupDate", DateTime.Now.ToString("o") },
                { "SourceDir", sourceDir },
                { "FileCount", files.Length.ToString() }
            };
            
            string metaPath = Path.Combine(backupDir, "backup_metadata.txt");
            File.WriteAllLines(metaPath, metadata.Select(kvp => $"{kvp.Key}={kvp.Value}"));
        }
        
        private static void RestoreBackup(string backupDir, string restoreDir)
        {
            if (Directory.Exists(restoreDir))
            {
                Directory.Delete(restoreDir, true);
            }
            
            Directory.CreateDirectory(restoreDir);
            
            string[] files = Directory.GetFiles(backupDir, "*.*", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                if (Path.GetFileName(file) == "backup_metadata.txt")
                {
                    continue;
                }
                
                string relativePath = Path.GetRelativePath(backupDir, file);
                string destPath = Path.Combine(restoreDir, relativePath);
                
                string destDir = Path.GetDirectoryName(destPath);
                if (!Directory.Exists(destDir))
                {
                    Directory.CreateDirectory(destDir);
                }
                
                File.Copy(file, destPath, true);
            }
        }
        
        private static void FileComparisonDemo()
        {
            Console.WriteLine("\n=== File Comparison System ===");
            
            string compareDir = Path.Combine(BasePath, "comparison");
            Directory.CreateDirectory(compareDir);
            
            // Create original file
            string original = Path.Combine(compareDir, "original.txt");
            string content1 = "Line 1\nLine 2\nLine 3\nLine 4\nLine 5";
            File.WriteAllText(original, content1);
            
            // Create identical copy
            string identical = Path.Combine(compareDir, "identical.txt");
            File.WriteAllText(identical, content1);
            
            // Create modified copy
            string modified = Path.Combine(compareDir, "modified.txt");
            string content2 = "Line 1\nLine 2 MODIFIED\nLine 3\nLine 4\nLine 5";
            File.WriteAllText(modified, content2);
            
            // Create slightly different
            string different = Path.Combine(compareDir, "different.txt");
            string content3 = "Line 1\nLine 2\nLine 3 Changed";
            File.WriteAllText(different, content3);
            
            // Compare files
            Console.WriteLine("Comparing files:");
            
            Console.WriteLine($"\nOriginal vs Identical:");
            bool identicalResult = CompareFiles(original, identical);
            Console.WriteLine($"  Result: {identicalResult}");
            // Output: Result: True
            
            Console.WriteLine($"\nOriginal vs Modified:");
            bool modifiedResult = CompareFiles(original, modified);
            Console.WriteLine($"  Result: {modifiedResult}");
            // Output: Result: False
            
            Console.WriteLine($"\nOriginal vs Different:");
            bool differentResult = CompareFiles(original, different);
            Console.WriteLine($"  Result: {differentResult}");
            // Output: Result: False
            
            // Detailed comparison
            var diffResult = DetailedCompare(original, modified);
            Console.WriteLine($"\nDetailed comparison (Original vs Modified):");
            Console.WriteLine($"  AreEqual: {diffResult.AreEqual}");
            // Output: AreEqual: False
            Console.WriteLine($"  Differences: {diffResult.Differences.Count}");
            // Output: Differences: 2
            
            foreach (var diff in diffResult.Differences)
            {
                Console.WriteLine($"    {diff}");
                // Output shows line-by-line differences
            }
        }
        
        private static bool CompareFiles(string file1, string file2)
        {
            if (!File.Exists(file1) || !File.Exists(file2))
            {
                return false;
            }
            
            FileInfo fi1 = new FileInfo(file1);
            FileInfo fi2 = new FileInfo(file2);
            
            if (fi1.Length != fi2.Length)
            {
                return false;
            }
            
            byte[] content1 = File.ReadAllBytes(file1);
            byte[] content2 = File.ReadAllBytes(file2);
            
            return content1.SequenceEqual(content2);
        }
        
        private static ComparisonResult DetailedCompare(string file1, string file2)
        {
            var result = new ComparisonResult();
            
            string[] lines1 = File.ReadAllLines(file1);
            string[] lines2 = File.ReadAllLines(file2);
            
            int maxLines = Math.Max(lines1.Length, lines2.Length);
            
            for (int i = 0; i < maxLines; i++)
            {
                string line1 = i < lines1.Length ? lines1[i] : "[EOF]";
                string line2 = i < lines2.Length ? lines2[i] : "[EOF]";
                
                if (line1 != line2)
                {
                    result.Differences.Add($"Line {i + 1}: '{line1}' vs '{line2}'");
                }
            }
            
            result.AreEqual = result.Differences.Count == 0;
            return result;
        }
        
        private static void SecureFileStorageDemo()
        {
            Console.WriteLine("\n=== Secure File Storage ===");
            
            string secureDir = Path.Combine(BasePath, "secure");
            Directory.CreateDirectory(secureDir);
            
            string plainFile = Path.Combine(secureDir, "secret.txt");
            string plainContent = "This is sensitive data that needs encryption!";
            File.WriteAllText(plainFile, plainContent);
            
            // Encrypt file
            string encryptedFile = Path.Combine(secureDir, "secret.enc");
            string password = "MySecurePassword123";
            
            EncryptFile(plainFile, encryptedFile, password);
            Console.WriteLine("File encrypted successfully");
            
            // Verify encrypted file is not readable as plain text
            string encryptedContent = File.ReadAllText(encryptedFile);
            bool containsPlainText = encryptedContent.Contains("sensitive");
            Console.WriteLine($"Encrypted file contains plain text: {containsPlainText}");
            // Output: Encrypted file contains plain text: False
            
            // Decrypt file
            string decryptedFile = Path.Combine(secureDir, "decrypted.txt");
            DecryptFile(encryptedFile, decryptedFile, password);
            
            string decryptedContent = File.ReadAllText(decryptedFile);
            Console.WriteLine($"Decrypted content: {decryptedContent.Substring(0, 30)}...");
            // Output: Decrypted content: This is sensitive data that...
            
            Console.WriteLine($"Content matches: {decryptedContent == plainContent}");
            // Output: Content matches: True
            
            // Try with wrong password
            try
            {
                DecryptFile(encryptedFile, Path.Combine(secureDir, "wrong_decrypt.txt"), "WrongPassword");
            }
            catch (CryptographicException ex)
            {
                Console.WriteLine($"Wrong password correctly rejected: {ex.Message}");
            }
        }
        
        private static void EncryptFile(string inputFile, string outputFile, string password)
        {
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            
            using (var aes = Aes.Create())
            {
                var key = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256);
                aes.Key = key.GetBytes(32);
                aes.IV = key.GetBytes(16);
                
                using (FileStream fsInput = new FileStream(inputFile, FileMode.Open, FileAccess.Read))
                using (FileStream fsOutput = new FileStream(outputFile, FileMode.Create, FileAccess.Write))
                {
                    fsOutput.Write(salt, 0, salt.Length);
                    
                    using (CryptoStream cs = new CryptoStream(fsOutput, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        fsInput.CopyTo(cs);
                    }
                }
            }
        }
        
        private static void DecryptFile(string inputFile, string outputFile, string password)
        {
            using (FileStream fsInput = new FileStream(inputFile, FileMode.Open, FileAccess.Read))
            {
                byte[] salt = new byte[16];
                fsInput.Read(salt, 0, 16);
                
                using (var aes = Aes.Create())
                {
                    var key = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256);
                    aes.Key = key.GetBytes(32);
                    aes.IV = key.GetBytes(16);
                    
                    using (FileStream fsOutput = new FileStream(outputFile, FileMode.Create, FileAccess.Write))
                    using (CryptoStream cs = new CryptoStream(fsOutput, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        fsInput.CopyTo(cs);
                    }
                }
            }
        }
        
        private static void LargeFileProcessingDemo()
        {
            Console.WriteLine("\n=== Large File Processing ===");
            
            string largeFileDir = Path.Combine(BasePath, "large_files");
            Directory.CreateDirectory(largeFileDir);
            
            // Create a "large" file (simulated)
            string largeFile = Path.Combine(largeFileDir, "large_data.dat");
            const int targetSize = 10 * 1024 * 1024;  // 10MB
            const int chunkSize = 1024 * 1024;  // 1MB chunks
            
            Console.WriteLine($"Creating {targetSize / (1024 * 1024)}MB test file...");
            
            using (FileStream fs = new FileStream(largeFile, FileMode.Create, FileAccess.Write))
            {
                byte[] chunk = new byte[chunkSize];
                for (int i = 0; i < chunkSize; i++)
                {
                    chunk[i] = (byte)(i % 256);
                }
                
                for (int chunkNum = 0; chunkNum < targetSize / chunkSize; chunkNum++)
                {
                    fs.Write(chunk, 0, chunkSize);
                    
                    if (chunkNum % 5 == 0)
                    {
                        Console.WriteLine($"  Progress: {(chunkNum * 100) / (targetSize / chunkSize)}%");
                    }
                }
            }
            
            long fileSize = new FileInfo(largeFile).Length;
            Console.WriteLine($"File created: {fileSize / (1024 * 1024)} MB");
            // Output: File created: 10 MB
            
            // Process large file in chunks
            Console.WriteLine("\nProcessing file in chunks...");
            long totalSum = 0;
            int chunksProcessed = 0;
            
            using (FileStream fs = new FileStream(largeFile, FileMode.Open, FileAccess.Read))
            {
                byte[] buffer = new byte[chunkSize];
                int bytesRead;
                
                while ((bytesRead = fs.Read(buffer, 0, buffer.Length)) > 0)
                {
                    for (int i = 0; i < bytesRead; i++)
                    {
                        totalSum += buffer[i];
                    }
                    chunksProcessed++;
                }
            }
            
            Console.WriteLine($"Chunks processed: {chunksProcessed}");
            // Output: Chunks processed: 10
            Console.WriteLine($"Total byte sum: {totalSum}");
            
            // Find specific patterns in large file
            Console.WriteLine("\nSearching for pattern in large file...");
            long patternPosition = FindPatternInLargeFile(largeFile, new byte[] { 0, 1, 2, 3 });
            Console.WriteLine($"Pattern found at position: {patternPosition}");
            // Output: Pattern found at position: 0 (or position of first match)
        }
        
        private static long FindPatternInLargeFile(string filePath, byte[] pattern)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096))
            {
                byte[] buffer = new byte[4096 * 4];
                int patternIndex = 0;
                long position = 0;
                
                int bytesRead;
                while ((bytesRead = fs.Read(buffer, 0, buffer.Length)) > 0)
                {
                    for (int i = 0; i < bytesRead; i++)
                    {
                        if (buffer[i] == pattern[patternIndex])
                        {
                            patternIndex++;
                            if (patternIndex == pattern.Length)
                            {
                                return position + i - pattern.Length + 1;
                            }
                        }
                        else
                        {
                            patternIndex = 0;
                        }
                    }
                    position += bytesRead;
                }
            }
            
            return -1;
        }
        
        private static void ArchiveManagementDemo()
        {
            Console.WriteLine("\n=== Archive Management ===");
            
            string archiveDir = Path.Combine(BasePath, "archives");
            string sourceDir = Path.Combine(BasePath, "archive_source");
            Directory.CreateDirectory(archiveDir);
            Directory.CreateDirectory(sourceDir);
            
            // Create files to archive
            File.WriteAllText(Path.Combine(sourceDir, "file1.txt"), "Content of file 1");
            File.WriteAllText(Path.Combine(sourceDir, "file2.txt"), "Content of file 2");
            File.WriteAllText(Path.Combine(sourceDir, "file3.txt"), "Content of file 3");
            
            string subDir = Path.Combine(sourceDir, "subdir");
            Directory.CreateDirectory(subDir);
            File.WriteAllText(Path.Combine(subDir, "nested.txt"), "Nested content");
            
            // Create ZIP archive
            string zipPath = Path.Combine(archiveDir, "archive.zip");
            CreateZipArchive(sourceDir, zipPath);
            Console.WriteLine($"ZIP archive created: {zipPath}");
            
            long zipSize = new FileInfo(zipPath).Length;
            long sourceSize = Directory.GetFiles(sourceDir, "*.*", SearchOption.AllDirectories)
                .Sum(f => new FileInfo(f).Length);
            
            Console.WriteLine($"Original size: {sourceSize} bytes");
            // Output: Original size: ~70 bytes
            Console.WriteLine($"Compressed size: {zipSize} bytes");
            // Output: Compressed size: ~800 bytes (smaller due to compression)
            Console.WriteLine($"Compression ratio: {(1 - (double)zipSize / sourceSize) * 100:F1}%");
            
            // Extract archive
            string extractDir = Path.Combine(archiveDir, "extracted");
            ExtractZipArchive(zipPath, extractDir);
            Console.WriteLine($"Archive extracted to: {extractDir}");
            
            // Verify extracted files
            string[] extractedFiles = Directory.GetFiles(extractDir, "*.*", SearchOption.AllDirectories);
            Console.WriteLine($"Extracted {extractedFiles.Length} files:");
            foreach (string f in extractedFiles)
            {
                Console.WriteLine($"  {Path.GetRelativePath(extractDir, f)}");
                // Output: file1.txt, file2.txt, file3.txt, subdir/nested.txt
            }
            
            // List archive contents
            Console.WriteLine("\nArchive contents:");
            using (ZipArchive archive = ZipFile.OpenRead(zipPath))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    Console.WriteLine($"  {entry.Name} ({entry.Length} bytes)");
                }
            }
        }
        
        private static void CreateZipArchive(string sourceDir, string zipPath)
        {
            if (File.Exists(zipPath))
            {
                File.Delete(zipPath);
            }
            
            ZipFile.CreateFromDirectory(sourceDir, zipPath, CompressionLevel.Optimal, false);
        }
        
        private static void ExtractZipArchive(string zipPath, string extractDir)
        {
            if (Directory.Exists(extractDir))
            {
                Directory.Delete(extractDir, true);
            }
            
            ZipFile.ExtractToDirectory(zipPath, extractDir);
        }
        
        private class ComparisonResult
        {
            public bool AreEqual { get; set; }
            public List<string> Differences { get; set; } = new List<string>();
        }
    }
}