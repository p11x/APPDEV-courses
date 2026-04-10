/*
TOPIC: C# File I/O Operations
SUBTOPIC: Compression
FILE: 02_ZipFile.cs
PURPOSE: ZipFile class for creating and extracting ZIP archives
*/

using System;
using System.IO;
using System.IO.Compression;

namespace CSharp_MasterGuide._06_FileIO._04_Compression
{
    public class NN_02_ZipFile
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== ZipFile Class Demo ===");
            Console.WriteLine();

            BasicZipCreation();
            Console.WriteLine();

            ZipExtraction();
            Console.WriteLine();

            ZipEntriesManipulation();
            Console.WriteLine();

            CompressionOptions();
            Console.WriteLine();

            RealWorldExample_BackupCreator();
            
            CleanupDemoFiles();
        }

        private static void BasicZipCreation()
        {
            Console.WriteLine("--- Basic ZIP Creation ---");
            
            string sourceDir = "NN_source_files";
            string zipPath = "NN_basic_archive.zip";
            
            Directory.CreateDirectory(sourceDir);
            File.WriteAllText(Path.Combine(sourceDir, "file1.txt"), "Content of file 1");
            File.WriteAllText(Path.Combine(sourceDir, "file2.txt"), "Content of file 2");
            File.WriteAllText(Path.Combine(sourceDir, "file3.txt"), "Content of file 3");
            
            ZipFile.CreateFromDirectory(sourceDir, zipPath, CompressionLevel.Optimal, false);
            
            long zipSize = new FileInfo(zipPath).Length;
            long sourceSize = GetDirectorySize(sourceDir);
            Console.WriteLine($"Created: {zipPath}");
            Console.WriteLine($"Source size: {sourceSize} bytes");
            Console.WriteLine($"ZIP size: {zipSize} bytes");
            Console.WriteLine($"Compression: {(1 - (double)zipSize / sourceSize) * 100:F1}%");
            
            using (ZipArchive archive = ZipFile.OpenRead(zipPath))
            {
                Console.WriteLine("ZIP contents:");
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    Console.WriteLine($"  {entry.Name} ({entry.Length} bytes)");
                }
            }
            
            Directory.Delete(sourceDir, true);
            File.Delete(zipPath);
            Console.WriteLine("// Output: ZIP archive created from directory");
        }

        private static long GetDirectorySize(string path)
        {
            long size = 0;
            foreach (string file in Directory.GetFiles(path))
            {
                size += new FileInfo(file).Length;
            }
            return size;
        }

        private static void ZipExtraction()
        {
            Console.WriteLine("--- ZIP Extraction ---");
            
            string zipPath = "NN_extract_test.zip";
            string extractDir = "NN_extracted";
            
            using (ZipArchive archive = ZipFile.Open(zipPath, ZipArchiveMode.Create))
            {
                archive.CreateEntryFromFile("NN_02_ZipFile.cs", "source_code.cs");
                
                var entry = archive.CreateEntry("generated_data.txt");
                using (var writer = new StreamWriter(entry.Open()))
                {
                    writer.WriteLine("Generated content line 1");
                    writer.WriteLine("Generated content line 2");
                    writer.WriteLine("Generated content line 3");
                }
            }
            
            Console.WriteLine("Created ZIP with entries");
            
            ZipFile.ExtractToDirectory(zipPath, extractDir, true);
            
            Console.WriteLine("Extracted files:");
            foreach (string file in Directory.GetFiles(extractDir, "*", SearchOption.AllDirectories))
            {
                Console.WriteLine($"  {file}");
            }
            
            Directory.Delete(extractDir, true);
            File.Delete(zipPath);
            Console.WriteLine("// Output: ZIP extracted to directory");
        }

        private static void ZipEntriesManipulation()
        {
            Console.WriteLine("--- ZIP Entries Manipulation ---");
            
            string zipPath = "NN_modify.zip";
            
            using (ZipArchive archive = ZipFile.Open(zipPath, ZipArchiveMode.Update))
            {
                ZipArchiveEntry entry1 = archive.CreateEntry("entry1.txt");
                using (var writer = new StreamWriter(entry1.Open()))
                {
                    writer.WriteLine("First entry content");
                }
                
                ZipArchiveEntry? entry2 = archive.GetEntry("nonexistent.txt");
                Console.WriteLine($"GetEntry returns null for missing: {entry2 == null}");
                
                ZipArchiveEntry entry3 = archive.CreateEntry("entry3.txt");
                entry3.LastWriteTime = DateTime.Now.AddDays(-7);
                
                Console.WriteLine("Entries in archive:");
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    Console.WriteLine($"  {entry.Name} - {entry.Length} bytes - {entry.LastWriteTime:yyyy-MM-dd}");
                }
                
                ZipArchiveEntry? toDelete = archive.GetEntry("entry1.txt");
                if (toDelete != null)
                {
                    toDelete.Delete();
                    Console.WriteLine("Deleted: entry1.txt");
                }
            }
            
            using (ZipArchive archive = ZipFile.OpenRead(zipPath))
            {
                Console.WriteLine("Final archive contents:");
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    Console.WriteLine($"  {entry.Name}");
                }
            }
            
            File.Delete(zipPath);
            Console.WriteLine("// Output: ZIP entries added, modified, and deleted");
        }

        private static void CompressionOptions()
        {
            Console.WriteLine("--- Compression Level Options ---");
            
            string sourceFile = "NN_compress_test.txt";
            File.WriteAllText(sourceFile, new string('A', 10000));
            
            foreach (CompressionLevel level in new[] { CompressionLevel.NoCompression, CompressionLevel.Fastest, CompressionLevel.Optimal, CompressionLevel.SmallestSize })
            {
                string zipPath = $"NN_test_{level}.zip";
                
                ZipFile.CreateFromDirectory(".", zipPath, level, false, new[] { sourceFile });
                
                long size = new FileInfo(zipPath).Length;
                Console.WriteLine($"  {level}: {size} bytes");
                
                File.Delete(zipPath);
            }
            
            File.Delete(sourceFile);
            Console.WriteLine("// Output: Different compression levels compared");
        }

        private static void RealWorldExample_BackupCreator()
        {
            Console.WriteLine();
            Console.WriteLine("=== REAL-WORLD EXAMPLE: Backup Creator ===");
            
            string backupDir = "NN_backup_source";
            string backupZip = $"NN_backup_{DateTime.Now:yyyyMMdd_HHmmss}.zip";
            
            Directory.CreateDirectory(Path.Combine(backupDir, "documents"));
            Directory.CreateDirectory(Path.Combine(backupDir, "images"));
            Directory.CreateDirectory(Path.Combine(backupDir, "data"));
            
            File.WriteAllText(Path.Combine(backupDir, "documents", "readme.txt"), "Backup files");
            File.WriteAllText(Path.Combine(backupDir, "documents", "notes.txt"), "Important notes");
            File.WriteAllText(Path.Combine(backupDir, "images", "photo1.jpg"), "fake_image_data");
            File.WriteAllText(Path.Combine(backupDir, "images", "photo2.jpg"), "fake_image_data");
            File.WriteAllText(Path.Combine(backupDir, "data", "config.json"), "{\"setting\": true}");
            File.WriteAllText(Path.Combine(backupDir, "data", "cache.dat"), "cache_content");
            
            long sourceSize = GetDirectorySize(backupDir);
            
            ZipFile.CreateFromDirectory(backupDir, backupZip, CompressionLevel.Optimal, false);
            
            long zipSize = new FileInfo(backupZip).Length;
            Console.WriteLine($"Backup created: {backupZip}");
            Console.WriteLine($"Source size: {sourceSize:N0} bytes");
            Console.WriteLine($"ZIP size: {zipSize:N0} bytes");
            Console.WriteLine($"Compression: {(1 - (double)zipSize / sourceSize) * 100:F1}%");
            
            string restoreDir = "NN_restored_backup";
            ZipFile.ExtractToDirectory(backupZip, restoreDir, true);
            
            Console.WriteLine("Restored directory structure:");
            foreach (string dir in Directory.GetDirectories(restoreDir, "*", SearchOption.AllDirectories))
            {
                Console.WriteLine($"  [Dir] {dir}");
            }
            foreach (string file in Directory.GetFiles(restoreDir, "*", SearchOption.AllDirectories))
            {
                Console.WriteLine($"  [File] {file}");
            }
            
            Directory.Delete(backupDir, true);
            Directory.Delete(restoreDir, true);
            File.Delete(backupZip);
            Console.WriteLine("// Output: Backup created from multiple directories");
        }

        private static void CleanupDemoFiles()
        {
            string[] files = { "NN_basic_archive.zip", "NN_extract_test.zip", "NN_modify.zip" };
            string[] dirs = { "NN_source_files", "NN_extracted", "NN_backup_source", "NN_restored_backup" };
            
            foreach (string f in files)
            {
                if (File.Exists(f)) File.Delete(f);
            }
            
            foreach (string d in dirs)
            {
                if (Directory.Exists(d)) Directory.Delete(d, true);
            }
            
            foreach (CompressionLevel level in Enum.GetValues(typeof(CompressionLevel)))
            {
                string zipFile = $"NN_test_{level}.zip";
                if (File.Exists(zipFile)) File.Delete(zipFile);
            }
            
            Console.WriteLine("[Cleanup] Demo files removed");
        }
    }
}