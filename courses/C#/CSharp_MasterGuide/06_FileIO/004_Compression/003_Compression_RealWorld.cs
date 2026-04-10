/*
TOPIC: C# File I/O Operations
SUBTOPIC: Compression
FILE: 03_Compression_RealWorld.cs
PURPOSE: Real-world compression examples - backup, archive, data transfer
*/

using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Collections.Generic;

namespace CSharp_MasterGuide._06_FileIO._04_Compression
{
    public class NN_03_Compression_RealWorld
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Real-World Compression Examples ===");
            Console.WriteLine();

            RealWorldExample_DailyBackup();
            Console.WriteLine();

            RealWorldExample_IncrementalArchive();
            Console.WriteLine();

            RealWorldExample_DataTransfer();
            Console.WriteLine();

            RealWorldExample_LogRotation();
            
            CleanupDemoFiles();
        }

        private static void RealWorldExample_DailyBackup()
        {
            Console.WriteLine("=== REAL-WORLD: Daily Backup System ===");
            
            string projectDir = "NN_project_backup";
            string backupDir = "NN_backups";
            string todayBackup = Path.Combine(backupDir, $"backup_{DateTime.Now:yyyyMMdd}.zip");
            
            Directory.CreateDirectory(projectDir);
            Directory.CreateDirectory(backupDir);
            
            File.WriteAllText(Path.Combine(projectDir, "src", "Program.cs"), "class Program { static void Main() { } }");
            File.WriteAllText(Path.Combine(projectDir, "src", "Helper.cs"), "class Helper { }");
            File.WriteAllText(Path.Combine(projectDir, "config.json"), "{\"version\": \"1.0\"}");
            File.WriteAllText(Path.Combine(projectDir, "data.txt"), "Project data content");
            
            long sourceSize = GetDirectorySize(projectDir);
            
            if (File.Exists(todayBackup))
            {
                File.Delete(todayBackup);
                Console.WriteLine("Removed existing backup for today");
            }
            
            ZipFile.CreateFromDirectory(projectDir, todayBackup, CompressionLevel.Optimal, false);
            
            long backupSize = new FileInfo(todayBackup).Length;
            
            Console.WriteLine($"Project source: {sourceSize:N0} bytes");
            Console.WriteLine($"Backup file: {backupSize:N0} bytes");
            Console.WriteLine($"Compression: {(1 - (double)backupSize / sourceSize) * 100:F1}%");
            
            if (File.Exists(todayBackup))
            {
                using (ZipArchive archive = ZipFile.OpenRead(todayBackup))
                {
                    Console.WriteLine("Backup contents:");
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        Console.WriteLine($"  {entry.FullName} ({entry.Length} bytes)");
                    }
                }
            }
            
            Directory.Delete(projectDir, true);
            Directory.Delete(backupDir, true);
            Console.WriteLine("// Output: Daily project backup created");
        }

        private static void RealWorldExample_IncrementalArchive()
        {
            Console.WriteLine("=== REAL-WORLD: Incremental Archive System ===");
            
            string dataDir = "NN_incremental_data";
            string archiveBase = "NN_incremental_archive";
            
            Directory.CreateDirectory(dataDir);
            
            var snapshots = new List<SnapshotInfo>();
            
            for (int day = 1; day <= 5; day++)
            {
                string dayFile = Path.Combine(dataDir, $"data_day{day}.txt");
                StringBuilder content = new StringBuilder();
                for (int i = 0; i < 100; i++)
                {
                    content.AppendLine($"Day {day} - Record {i:D3}: Some data content here");
                }
                File.WriteAllText(dayFile, content.ToString());
                
                string archivePath = $"{archiveBase}_day{day}.zip";
                
                using (ZipArchive archive = ZipFile.Open(archivePath, ZipArchiveMode.Create))
                {
                    string[] files = Directory.GetFiles(dataDir);
                    foreach (string file in files)
                    {
                        string name = Path.GetFileName(file);
                        archive.CreateEntryFromFile(file, name, CompressionLevel.Optimal);
                    }
                }
                
                long size = new FileInfo(archivePath).Length;
                Console.WriteLine($"Day {day} archive: {size:N0} bytes");
                
                snapshots.Add(new SnapshotInfo { Day = day, ArchivePath = archivePath, Size = size });
            }
            
            long totalSize = 0;
            foreach (var s in snapshots)
            {
                totalSize += s.Size;
            }
            Console.WriteLine($"Total archive size: {totalSize:N0} bytes");
            
            Console.WriteLine("Extracting latest archive:");
            string extractDir = "NN_incremental_restore";
            ZipFile.ExtractToDirectory($"{archiveBase}_day5.zip", extractDir, true);
            
            int extractedFiles = Directory.GetFiles(extractDir).Length;
            Console.WriteLine($"Extracted {extractedFiles} files");
            
            Directory.Delete(dataDir, true);
            Directory.Delete(extractDir, true);
            foreach (var s in snapshots)
            {
                File.Delete(s.ArchivePath);
            }
            Console.WriteLine("// Output: Incremental archives created and extracted");
        }

        private static void RealWorldExample_DataTransfer()
        {
            Console.WriteLine("=== REAL-WORLD: Compressed Data Transfer ===");
            
            string originalDataFile = "NN_transfer_data.json";
            string transferFile = "NN_transfer_compressed.gz";
            string receivedFile = "NN_received_data.json";
            
            var transferData = new TransferPackage
            {
                PackageId = Guid.NewGuid().ToString(),
                Timestamp = DateTime.UtcNow,
                Sender = "server_A",
                Recipient = "server_B",
                Payload = new List<PayloadItem>
                {
                    new PayloadItem { Type = "user", Id = "U001", Data = "{\"name\":\"Alice\",\"email\":\"alice@test.com\"}" },
                    new PayloadItem { Type = "order", Id = "O001", Data = "{\"total\":150.00,\"items\":3}" },
                    new PayloadItem { Type = "product", Id = "P001", Data = "{\"name\":\"Widget\",\"price\":29.99}" }
                },
                Metadata = new Dictionary<string, string>
                {
                    { "priority", "normal" },
                    { "encoding", "json" }
                }
            };
            
            var jsonOptions = new System.Text.Json.JsonSerializerOptions { WriteIndented = true };
            string json = System.Text.Json.JsonSerializer.Serialize(transferData, jsonOptions);
            File.WriteAllText(originalDataFile, json);
            
            long originalSize = new FileInfo(originalDataFile).Length;
            Console.WriteLine($"Original data: {originalSize} bytes");
            
            byte[] dataBytes = Encoding.UTF8.GetBytes(json);
            using (FileStream fs = File.Create(transferFile))
            using (GZipStream gz = new GZipStream(fs, CompressionMode.Compress, true))
            {
                gz.Write(dataBytes, 0, dataBytes.Length);
            }
            
            long compressedSize = new FileInfo(transferFile).Length;
            Console.WriteLine($"Compressed: {compressedSize} bytes ({(1 - (double)compressedSize / originalSize) * 100:F1}% smaller)");
            
            using (FileStream fs = File.OpenRead(transferFile))
            using (GZipStream gz = new GZipStream(fs, CompressionMode.Decompress))
            using (MemoryStream decompressed = new MemoryStream())
            {
                gz.CopyTo(decompressed);
                byte[] decompressedBytes = decompressed.ToArray();
                File.WriteAllBytes(receivedFile, decompressedBytes);
            }
            
            string receivedJson = File.ReadAllText(receivedFile);
            var received = System.Text.Json.JsonSerializer.Deserialize<TransferPackage>(receivedJson);
            
            Console.WriteLine("Transfer verified:");
            Console.WriteLine($"  Package ID: {received?.PackageId}");
            Console.WriteLine($"  From: {received?.Sender}");
            Console.WriteLine($"  To: {received?.Recipient}");
            Console.WriteLine($"  Items: {received?.Payload?.Count}");
            Console.WriteLine($"  Data integrity: {(received?.PackageId == transferData.PackageId ? "OK" : "FAILED")}");
            
            File.Delete(originalDataFile);
            File.Delete(transferFile);
            File.Delete(receivedFile);
            Console.WriteLine("// Output: Compressed data transfer simulation");
        }

        private static void RealWorldExample_LogRotation()
        {
            Console.WriteLine("=== REAL-WORLD: Log Rotation with Compression ===");
            
            string logDir = "NN_application_logs";
            string archiveDir = "NN_log_archives";
            
            Directory.CreateDirectory(logDir);
            Directory.CreateDirectory(archiveDir);
            
            DateTime baseDate = new DateTime(2024, 1, 1);
            
            for (int day = 0; day < 30; day++)
            {
                string logFile = Path.Combine(logDir, $"app_{baseDate.AddDays(day):yyyyMMdd}.log");
                
                var entries = new List<string>();
                for (int hour = 0; hour < 24; hour++)
                {
                    for (int minute = 0; minute < 6; minute++)
                    {
                        string timestamp = baseDate.AddDays(day).AddHours(hour).AddMinutes(minute * 10).ToString("yyyy-MM-dd HH:mm:ss");
                        string level = (minute % 5 == 0) ? "ERROR" : (minute % 3 == 0) ? "WARN" : "INFO";
                        entries.Add($"[{timestamp}] {level} - Application log entry #{minute}");
                    }
                }
                File.WriteAllLines(logFile, entries);
            }
            
            long totalLogSize = GetDirectorySize(logDir);
            Console.WriteLine($"Created 30 daily log files, total: {totalLogSize:N0} bytes");
            
            var archiveFiles = new List<string>();
            for (int week = 0; week < 4; week++)
            {
                int startDay = week * 7 + 1;
                int endDay = startDay + 6;
                
                string weekDir = Path.Combine(logDir, $"week_{week + 1}");
                Directory.CreateDirectory(weekDir);
                
                for (int day = startDay; day <= endDay && day <= 30; day++)
                {
                    string sourceLog = Path.Combine(logDir, $"app_{baseDate.AddDays(day - 1):yyyyMMdd}.log");
                    if (File.Exists(sourceLog))
                    {
                        string destLog = Path.Combine(weekDir, $"app_{baseDate.AddDays(day - 1):yyyyMMdd}.log");
                        File.Copy(sourceLog, destLog);
                    }
                }
                
                string archivePath = Path.Combine(archiveDir, $"logs_week{week + 1}_{baseDate.AddDays(startDay - 1):MMdd}.zip");
                ZipFile.CreateFromDirectory(weekDir, archivePath, CompressionLevel.Optimal, false);
                
                archiveFiles.Add(archivePath);
                Console.WriteLine($"Week {week + 1} archived: {new FileInfo(archivePath).Length:N0} bytes");
                
                for (int day = startDay; day <= endDay && day <= 30; day++)
                {
                    string logFile = Path.Combine(logDir, $"app_{baseDate.AddDays(day - 1):yyyyMMdd}.log");
                    if (File.Exists(logFile)) File.Delete(logFile);
                }
                
                Directory.Delete(weekDir, true);
            }
            
            long totalArchiveSize = 0;
            foreach (string f in archiveFiles)
            {
                totalArchiveSize += new FileInfo(f).Length;
            }
            
            Console.WriteLine($"Original logs: {totalLogSize:N0} bytes");
            Console.WriteLine($"Archives: {totalArchiveSize:N0} bytes");
            Console.WriteLine($"Space saved: {totalLogSize - totalArchiveSize:N0} bytes ({(1 - (double)totalArchiveSize / totalLogSize) * 100:F1}%)");
            
            string latestArchive = archiveFiles[archiveFiles.Count - 1];
            string testExtractDir = "NN_log_test_extract";
            ZipFile.ExtractToDirectory(latestArchive, testExtractDir, true);
            
            int extractedCount = Directory.GetFiles(testExtractDir).Length;
            Console.WriteLine($"Sample extraction: {extractedCount} log files from week 4");
            
            Directory.Delete(logDir, true);
            Directory.Delete(archiveDir, true);
            Directory.Delete(testExtractDir, true);
            Console.WriteLine("// Output: Log rotation with weekly compression");
        }

        private static long GetDirectorySize(string path)
        {
            long size = 0;
            foreach (string file in Directory.GetFiles(path, "*", SearchOption.AllDirectories))
            {
                size += new FileInfo(file).Length;
            }
            return size;
        }

        private static void CleanupDemoFiles()
        {
            string[] dirs = { "NN_project_backup", "NN_backups", "NN_incremental_data", "NN_incremental_restore", "NN_transfer_data.json", "NN_application_logs", "NN_log_archives", "NN_log_test_extract" };
            string[] files = { "NN_transfer_compressed.gz", "NN_received_data.json" };
            
            foreach (string d in dirs)
            {
                if (Directory.Exists(d)) Directory.Delete(d, true);
            }
            
            foreach (string f in files)
            {
                if (File.Exists(f)) File.Delete(f);
            }
            
            Console.WriteLine("[Cleanup] Demo files removed");
        }
    }

    public class SnapshotInfo
    {
        public int Day { get; set; }
        public string ArchivePath { get; set; } = "";
        public long Size { get; set; }
    }

    public class TransferPackage
    {
        public string PackageId { get; set; } = "";
        public DateTime Timestamp { get; set; }
        public string Sender { get; set; } = "";
        public string Recipient { get; set; } = "";
        public List<PayloadItem>? Payload { get; set; }
        public Dictionary<string, string>? Metadata { get; set; }
    }

    public class PayloadItem
    {
        public string Type { get; set; } = "";
        public string Id { get; set; } = "";
        public string Data { get; set; } = "";
    }
}