/*
TOPIC: C# File I/O Operations
SUBTOPIC: Compression
FILE: 01_GZipCompression.cs
PURPOSE: GZipStream for compressing and decompressing data
*/

using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace CSharp_MasterGuide._06_FileIO._04_Compression
{
    public class NN_01_GZipCompression
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== GZip Compression Demo ===");
            Console.WriteLine();

            BasicStringCompression();
            Console.WriteLine();

            FileCompression();
            Console.WriteLine();

            StreamChaining();
            Console.WriteLine();

            CompressionLevelDemo();
            Console.WriteLine();

            RealWorldExample_LogArchival();
            
            CleanupDemoFiles();
        }

        private static void BasicStringCompression()
        {
            Console.WriteLine("--- Basic String Compression ---");
            
            string original = "This is a sample text that we will compress using GZip. " +
                "Compression reduces file size by replacing repeated patterns with shorter representations. " +
                "GZip is widely used for web content and file archives.";
            
            Console.WriteLine($"Original length: {original.Length} characters");
            
            byte[] originalBytes = Encoding.UTF8.GetBytes(original);
            Console.WriteLine($"Original bytes: {originalBytes.Length}");
            
            using (MemoryStream compressedStream = new MemoryStream())
            {
                using (GZipStream gzip = new GZipStream(compressedStream, CompressionMode.Compress, true))
                {
                    gzip.Write(originalBytes, 0, originalBytes.Length);
                }
                
                byte[] compressedBytes = compressedStream.ToArray();
                Console.WriteLine($"Compressed bytes: {compressedBytes.Length}");
                Console.WriteLine($"Compression ratio: {(1 - (double)compressedBytes.Length / originalBytes.Length) * 100:F1}%");
                
                using (MemoryStream decompressStream = new MemoryStream(compressedBytes))
                using (GZipStream gzip = new GZipStream(decompressStream, CompressionMode.Decompress))
                using (MemoryStream decompressed = new MemoryStream())
                {
                    gzip.CopyTo(decompressed);
                    byte[] decompressedBytes = decompressed.ToArray();
                    string restored = Encoding.UTF8.GetString(decompressedBytes);
                    
                    Console.WriteLine($"Decompressed length: {decompressedBytes.Length}");
                    Console.WriteLine($"Restored text matches: {restored == original}");
                }
            }
            
            Console.WriteLine("// Output: String compressed and decompressed with GZip");
        }

        private static void FileCompression()
        {
            Console.WriteLine("--- File Compression ---");
            
            string sourceFile = "NN_source.txt";
            string compressedFile = "NN_compressed.gz";
            string decompressedFile = "NN_restored.txt";
            
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 1000; i++)
            {
                sb.AppendLine($"Line {i:D4}: This is sample text content for compression testing. " +
                    "Lorem ipsum dolor sit amet, consectetur adipiscing elit. " +
                    "Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.");
            }
            File.WriteAllText(sourceFile, sb.ToString());
            
            long originalSize = new FileInfo(sourceFile).Length;
            Console.WriteLine($"Original file size: {originalSize} bytes");
            
            CompressFile(sourceFile, compressedFile);
            long compressedSize = new FileInfo(compressedFile).Length;
            Console.WriteLine($"Compressed file size: {compressedSize} bytes");
            Console.WriteLine($"Compression ratio: {(1 - (double)compressedSize / originalSize) * 100:F1}%");
            
            DecompressFile(compressedFile, decompressedFile);
            long decompressedSize = new FileInfo(decompressedFile).Length;
            Console.WriteLine($"Decompressed file size: {decompressedSize} bytes");
            
            bool filesMatch = File.ReadAllText(sourceFile) == File.ReadAllText(decompressedFile);
            Console.WriteLine($"Files match: {filesMatch}");
            
            File.Delete(sourceFile);
            File.Delete(compressedFile);
            File.Delete(decompressedFile);
            Console.WriteLine("// Output: File compressed to .gz and decompressed back");
        }

        private static void CompressFile(string source, string destination)
        {
            using (FileStream sourceStream = File.OpenRead(source))
            using (FileStream destStream = File.Create(destination))
            using (GZipStream gzip = new GZipStream(destStream, CompressionMode.Compress, true))
            {
                sourceStream.CopyTo(gzip);
            }
        }

        private static void DecompressFile(string source, string destination)
        {
            using (FileStream sourceStream = File.OpenRead(source))
            using (GZipStream gzip = new GZipStream(sourceStream, CompressionMode.Decompress))
            using (FileStream destStream = File.Create(destination))
            {
                gzip.CopyTo(destStream);
            }
        }

        private static void StreamChaining()
        {
            Console.WriteLine("--- Stream Chaining with GZip ---");
            
            string originalFile = "NN_original_data.txt";
            string processedFile = "NN_processed.gz";
            
            string data = "Raw data content for processing. " + new string('x', 10000);
            File.WriteAllText(originalFile, data);
            
            using (FileStream fs = File.OpenRead(originalFile))
            using (GZipStream gzip = new GZipStream(File.Create(processedFile), CompressionMode.Compress, true))
            using (BufferedStream buffered = new BufferedStream(gzip, 8192))
            {
                byte[] buffer = new byte[4096];
                int bytesRead;
                while ((bytesRead = fs.Read(buffer, 0, buffer.Length)) > 0)
                {
                    buffered.Write(buffer, 0, bytesRead);
                }
            }
            
            Console.WriteLine($"Original: {new FileInfo(originalFile).Length} bytes");
            Console.WriteLine($"Compressed: {new FileInfo(processedFile).Length} bytes");
            
            File.Delete(originalFile);
            File.Delete(processedFile);
            Console.WriteLine("// Output: Buffered stream for optimized compression");
        }

        private static void CompressionLevelDemo()
        {
            Console.WriteLine("--- Compression Level Comparison ---");
            
            string sampleData = new string('A', 10000) + new string('B', 10000) + new string('C', 10000);
            
            byte[] data = Encoding.UTF8.GetBytes(sampleData);
            
            Console.WriteLine($"Sample data size: {data.Length} bytes");
            
            foreach (CompressionLevel level in Enum.GetValues(typeof(CompressionLevel)))
            {
                using (MemoryStream compressed = new MemoryStream())
                {
                    using (GZipStream gzip = new GZipStream(compressed, level, true))
                    {
                        gzip.Write(data, 0, data.Length);
                    }
                    
                    byte[] result = compressed.ToArray();
                    Console.WriteLine($"  {level}: {result.Length} bytes (ratio: {(1 - (double)result.Length / data.Length) * 100:F1}%)");
                }
            }
            
            Console.WriteLine("// Output: Different compression levels yield different results");
        }

        private static void RealWorldExample_LogArchival()
        {
            Console.WriteLine();
            Console.WriteLine("=== REAL-WORLD EXAMPLE: Log File Archival ===");
            
            string logDir = "NN_logs";
            string archivePath = "NN_logs_archive.gz";
            string extractDir = "NN_extracted_logs";
            
            Directory.CreateDirectory(logDir);
            
            string[] logFiles = new[]
            {
                "app_2024-01-01.log",
                "app_2024-01-02.log",
                "app_2024-01-03.log"
            };
            
            long totalOriginal = 0;
            foreach (string logFile in logFiles)
            {
                string[] entries = new string[100];
                for (int i = 0; i < entries.Length; i++)
                {
                    entries[i] = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] INFO - Log entry {i} with some additional text content";
                }
                string path = Path.Combine(logDir, logFile);
                File.WriteAllLines(path, entries);
                totalOriginal += new FileInfo(path).Length;
            }
            
            Console.WriteLine($"Created {logFiles.Length} log files, total: {totalOriginal} bytes");
            
            using (FileStream archive = File.Create(archivePath))
            using (GZipStream gzip = new GZipStream(archive, CompressionMode.Compress, true))
            {
                foreach (string logFile in logFiles)
                {
                    string filePath = Path.Combine(logDir, logFile);
                    string relativePath = Path.GetFileName(filePath);
                    
                    byte[] fileNameBytes = Encoding.UTF8.GetBytes(relativePath);
                    gzip.Write(BitConverter.GetBytes(fileNameBytes.Length), 0, 4);
                    gzip.Write(fileNameBytes, 0, fileNameBytes.Length);
                    
                    using (FileStream fs = File.OpenRead(filePath))
                    {
                        fs.CopyTo(gzip);
                    }
                }
            }
            
            long archiveSize = new FileInfo(archivePath).Length;
            Console.WriteLine($"Archive created: {archiveSize} bytes (ratio: {(1 - (double)archiveSize / totalOriginal) * 100:F1}%)");
            
            Directory.CreateDirectory(extractDir);
            using (FileStream archive = File.OpenRead(archivePath))
            using (GZipStream gzip = new GZipStream(archive, CompressionMode.Decompress))
            using (MemoryStream decompressed = new MemoryStream())
            {
                gzip.CopyTo(decompressed);
                decompressed.Position = 0;
                
                while (decompressed.Position < decompressed.Length)
                {
                    byte[] nameLenBytes = new byte[4];
                    decompressed.Read(nameLenBytes, 0, 4);
                    int nameLen = BitConverter.ToInt32(nameLenBytes, 0);
                    
                    byte[] nameBytes = new byte[nameLen];
                    decompressed.Read(nameBytes, 0, nameLen);
                    string fileName = Encoding.UTF8.GetString(nameBytes);
                    
                    string outputPath = Path.Combine(extractDir, fileName);
                    using (FileStream outFs = File.Create(outputPath))
                    {
                        decompressed.CopyTo(outFs);
                    }
                }
            }
            
            int extractedFiles = Directory.GetFiles(extractDir).Length;
            Console.WriteLine($"Extracted {extractedFiles} log files to {extractDir}");
            
            Directory.Delete(logDir, true);
            Directory.Delete(extractDir, true);
            File.Delete(archivePath);
            Console.WriteLine("// Output: Multiple log files archived together");
        }

        private static void CleanupDemoFiles()
        {
            string[] files = { "NN_source.txt", "NN_compressed.gz", "NN_restored.txt", "NN_original_data.txt", "NN_processed.gz" };
            foreach (string f in files)
            {
                if (File.Exists(f)) File.Delete(f);
            }
            
            if (Directory.Exists("NN_logs")) Directory.Delete("NN_logs", true);
            if (Directory.Exists("NN_extracted_logs")) Directory.Delete("NN_extracted_logs", true);
            if (File.Exists("NN_logs_archive.gz")) File.Delete("NN_logs_archive.gz");
            
            Console.WriteLine("[Cleanup] Demo files removed");
        }
    }
}