/*
TOPIC: C# File I/O Operations
SUBTOPIC: FileSystemWatcher
FILE: 03_Watchers_RealWorld.cs
PURPOSE: Real-world FileSystemWatcher examples - file monitoring systems
*/

using System;
using System.IO;
using System.Collections.Generic;
using System.IO.Compression;
using System.Threading;

namespace CSharp_MasterGuide._06_FileIO._05_Watchers
{
    public class NN_03_Watchers_RealWorld
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Real-World FileSystemWatcher Examples ===");
            Console.WriteLine();

            RealWorldExample_FileUploadMonitor();
            Console.WriteLine();

            RealWorldExample_ConfigReload();
            Console.WriteLine();

            RealWorldExample_WatchdogProcess();
            Console.WriteLine();

            RealWorldExample_AutoBackup();
            
            CleanupDemoFiles();
        }

        private static void RealWorldExample_FileUploadMonitor()
        {
            Console.WriteLine("=== REAL-WORLD: File Upload Monitor ===");
            
            string uploadDir = "NN_uploads";
            string processedDir = "NN_processed";
            string quarantineDir = "NN_quarantine";
            
            Directory.CreateDirectory(uploadDir);
            Directory.CreateDirectory(processedDir);
            Directory.CreateDirectory(quarantineDir);
            
            var uploadTracker = new UploadTracker();
            
            using (FileSystemWatcher watcher = new FileSystemWatcher(uploadDir))
            {
                watcher.Filter = "*.*";
                watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.CreationTime | NotifyFilters.LastWrite;
                
                watcher.Created += (s, e) =>
                {
                    uploadTracker.AddUpload(e.Name, File.GetCreationTime(e.FullPath));
                    Console.WriteLine($"[UPLOAD] New file: {e.Name}");
                };
                
                watcher.Changed += (s, e) =>
                {
                    uploadTracker.UpdateSize(e.Name, new FileInfo(e.FullPath).Length);
                    Console.WriteLine($"[UPDATE] {e.Name} ({uploadTracker.GetSize(e.Name)} bytes)");
                };
                
                watcher.Deleted += (s, e) =>
                {
                    var upload = uploadTracker.RemoveUpload(e.Name);
                    if (upload != null)
                    {
                        Console.WriteLine($"[DELETE] Upload removed: {e.Name}");
                    }
                };
                
                watcher.Renamed += (s, e) =>
                {
                    Console.WriteLine($"[RENAME] {e.OldName} -> {e.Name}");
                };
                
                watcher.EnableRaisingEvents = true;
                
                Console.WriteLine("Monitoring uploads...");
                
                string file1 = Path.Combine(uploadDir, "report_2024.xlsx");
                File.WriteAllText(file1, "Excel content");
                Thread.Sleep(50);
                
                string file2 = Path.Combine(uploadDir, "image_upload.png");
                File.WriteAllText(file2, "PNG content");
                Thread.Sleep(50);
                
                string file3 = Path.Combine(uploadDir, "document.pdf");
                File.WriteAllText(file3, "PDF content");
                Thread.Sleep(50);
                
                string file4 = Path.Combine(uploadDir, "unknown.xyz");
                File.WriteAllText(file4, "Suspicious");
                Thread.Sleep(50);
                
                Console.WriteLine();
                Console.WriteLine("Processing uploads:");
                string[] files = Directory.GetFiles(uploadDir);
                foreach (string f in files)
                {
                    string name = Path.GetFileName(f);
                    string ext = Path.GetExtension(f).ToLower();
                    
                    string destDir = (ext == ".xyz") ? quarantineDir : processedDir;
                    string dest = Path.Combine(destDir, name);
                    
                    File.Move(f, dest);
                    Console.WriteLine($"  Moved: {name} -> {destDir}");
                }
                
                Console.WriteLine();
                Console.WriteLine("Upload Statistics:");
                Console.WriteLine($"  Total uploads: {uploadTracker.Count}");
            }
            
            Directory.Delete(uploadDir, true);
            Directory.Delete(processedDir, true);
            Directory.Delete(quarantineDir, true);
            Console.WriteLine("// Output: File upload system monitored and processed");
        }

        private static void RealWorldExample_ConfigReload()
        {
            Console.WriteLine("=== REAL-WORLD: Configuration File Reload ===");
            
            string configDir = "NN_app_config";
            string configFile = Path.Combine(configDir, "settings.json");
            
            Directory.CreateDirectory(configDir);
            
            var appSettings = new AppSettings { LogLevel = "INFO", MaxUsers = 100, Theme = "light" };
            SaveConfig(configFile, appSettings);
            
            int reloadCount = 0;
            var lastReload = DateTime.Now;
            
            using (FileSystemWatcher watcher = new FileSystemWatcher(configDir))
            {
                watcher.Filter = "settings.json";
                watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size;
                
                watcher.Changed += (s, e) =>
                {
                    Thread.Sleep(100);
                    
                    try
                    {
                        var newSettings = LoadConfig(configFile);
                        reloadCount++;
                        Console.WriteLine($"[RELOAD] Config changed - LogLevel: {newSettings.LogLevel}, MaxUsers: {newSettings.MaxUsers}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[ERROR] Config reload failed: {ex.Message}");
                    }
                };
                
                watcher.EnableRaisingEvents = true;
                
                Console.WriteLine("Monitoring config file...");
                
                Thread.Sleep(100);
                appSettings.LogLevel = "DEBUG";
                SaveConfig(configFile, appSettings);
                Thread.Sleep(100);
                
                Thread.Sleep(100);
                appSettings.MaxUsers = 250;
                SaveConfig(configFile, appSettings);
                Thread.Sleep(100);
                
                Thread.Sleep(100);
                appSettings.Theme = "dark";
                SaveConfig(configFile, appSettings);
                Thread.Sleep(100);
                
                Console.WriteLine($"Config reload count: {reloadCount}");
            }
            
            Directory.Delete(configDir, true);
            Console.WriteLine("// Output: Config file changes detected and reloaded");
        }

        private static void RealWorldExample_WatchdogProcess()
        {
            Console.WriteLine("=== REAL-WORLD: Watchdog Process ===");
            
            string monitoredDir = "NN_watchdog_target";
            string lockFile = Path.Combine(monitoredDir, "app.lock");
            string statusFile = Path.Combine(monitoredDir, "status.txt");
            string logFile = "NN_watchdog.log";
            
            Directory.CreateDirectory(monitoredDir);
            
            int eventsDetected = 0;
            int restartCount = 0;
            
            using (FileSystemWatcher watcher = new FileSystemWatcher(monitoredDir))
            {
                watcher.Filter = "*.lock";
                watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.CreationTime | NotifyFilters.Deletion;
                
                watcher.Created += (s, e) =>
                {
                    eventsDetected++;
                    Console.WriteLine($"[WATCHDOG] Application started: {e.Name} at {DateTime.Now:HH:mm:ss}");
                    File.WriteAllText(statusFile, "RUNNING");
                };
                
                watcher.Deleted += (s, e) =>
                {
                    eventsDetected++;
                    restartCount++;
                    Console.WriteLine($"[WATCHDOG] Application crashed! Restarting...");
                    File.WriteAllText(statusFile, "RESTARTING");
                    
                    Thread.Sleep(500);
                    File.WriteAllText(lockFile, $"PID: {Environment.ProcessId}");
                    Console.WriteLine($"[WATCHDOG] Application restarted at {DateTime.Now:HH:mm:ss}");
                    File.WriteAllText(statusFile, "RUNNING");
                };
                
                watcher.EnableRaisingEvents = true;
                
                Console.WriteLine("Watchdog monitoring...");
                
                File.WriteAllText(lockFile, $"PID: {Environment.ProcessId}");
                Thread.Sleep(100);
                
                Console.WriteLine("Simulating application crash...");
                File.Delete(lockFile);
                Thread.Sleep(200);
                
                Console.WriteLine("Application running normally...");
                Thread.Sleep(100);
                
                Console.WriteLine("Simulating another crash...");
                File.Delete(lockFile);
                Thread.Sleep(200);
                
                Console.WriteLine();
                Console.WriteLine($"Watchdog Events: {eventsDetected}");
                Console.WriteLine($"Restarts: {restartCount}");
            }
            
            if (File.Exists(lockFile)) File.Delete(lockFile);
            if (File.Exists(statusFile)) File.Delete(statusFile);
            Directory.Delete(monitoredDir, true);
            if (File.Exists(logFile)) File.Delete(logFile);
            Console.WriteLine("// Output: Watchdog detects and recovers from crashes");
        }

        private static void RealWorldExample_AutoBackup()
        {
            Console.WriteLine();
            Console.WriteLine("=== REAL-WORLD: Auto Backup System ===");
            
            string sourceDir = "NN_autobackup_source";
            string backupDir = "NN_autobackup_archive";
            string manifestFile = "NN_backup_manifest.json";
            
            Directory.CreateDirectory(sourceDir);
            Directory.CreateDirectory(backupDir);
            
            File.WriteAllText(Path.Combine(sourceDir, "document1.txt"), "Important document 1");
            File.WriteAllText(Path.Combine(sourceDir, "document2.txt"), "Important document 2");
            File.WriteAllText(Path.Combine(sourceDir, "data.json"), "{\"key\": \"value\"}");
            
            var backupManifest = new BackupManifest();
            backupManifest.CreatedAt = DateTime.Now;
            backupManifest.Backups = new List<BackupEntry>();
            
            int backupCount = 0;
            
            using (FileSystemWatcher watcher = new FileSystemWatcher(sourceDir))
            {
                watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
                watcher.IncludeSubdirectories = true;
                
                watcher.Created += (s, e) =>
                {
                    Console.WriteLine($"[BACKUP] New file detected: {e.Name}");
                    backupCount++;
                };
                
                watcher.Changed += (s, e) =>
                {
                    Console.WriteLine($"[BACKUP] Modified: {e.Name}");
                    backupCount++;
                };
                
                watcher.Deleted += (s, e) =>
                {
                    Console.WriteLine($"[BACKUP] Deleted (archived): {e.Name}");
                    backupCount++;
                };
                
                watcher.EnableRaisingEvents = true;
                
                Console.WriteLine("File changes detected:");
                
                File.AppendAllText(Path.Combine(sourceDir, "document1.txt"), "\nUpdated content");
                Thread.Sleep(50);
                
                File.WriteAllText(Path.Combine(sourceDir, "document3.txt"), "New document");
                Thread.Sleep(50);
                
                File.Delete(Path.Combine(sourceDir, "data.json"));
                Thread.Sleep(50);
                
                File.WriteAllText(Path.Combine(sourceDir, "new_data.xml"), "<data>XML content</data>");
                Thread.Sleep(50);
                
                backupManifest.Backups.Add(new BackupEntry
                {
                    Timestamp = DateTime.Now,
                    SourceFiles = Directory.GetFiles(sourceDir, "*", SearchOption.AllDirectories).Length,
                    Description = "Auto backup"
                });
                
                string archivePath = Path.Combine(backupDir, $"backup_{DateTime.Now:HHmmss}.zip");
                ZipFile.CreateFromDirectory(sourceDir, archivePath, CompressionLevel.Optimal, false);
                
                Console.WriteLine();
                Console.WriteLine($"Backup created: {archivePath}");
                Console.WriteLine($"Change events: {backupCount}");
                Console.WriteLine($"Archive size: {new FileInfo(archivePath).Length} bytes");
                
                Console.WriteLine("Archive contents:");
                using (ZipArchive archive = ZipFile.OpenRead(archivePath))
                {
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        Console.WriteLine($"  {entry.Name} ({entry.Length} bytes)");
                    }
                }
            }
            
            string manifestJson = System.Text.Json.JsonSerializer.Serialize(backupManifest, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(manifestFile, manifestJson);
            Console.WriteLine($"Manifest saved: {manifestFile}");
            
            Directory.Delete(sourceDir, true);
            Directory.Delete(backupDir, true);
            File.Delete(manifestFile);
            Console.WriteLine("// Output: Automatic backup on file changes");
        }

        private static void SaveConfig(string path, AppSettings settings)
        {
            string json = System.Text.Json.JsonSerializer.Serialize(settings, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json);
        }

        private static AppSettings LoadConfig(string path)
        {
            string json = File.ReadAllText(path);
            return System.Text.Json.JsonSerializer.Deserialize<AppSettings>(json)!;
        }

        private static void CleanupDemoFiles()
        {
            string[] dirs = { "NN_uploads", "NN_processed", "NN_quarantine", "NN_app_config", "NN_watchdog_target", "NN_autobackup_source", "NN_autobackup_archive" };
            string[] files = { "NN_watchdog.log", "NN_backup_manifest.json" };
            
            foreach (string d in dirs)
            {
                if (Directory.Exists(d)) Directory.Delete(d, true);
            }
            
            foreach (string f in files)
            {
                if (File.Exists(f)) File.Delete(f);
            }
            
            Console.WriteLine("[Cleanup] Demo directories and files removed");
        }
    }

    public class UploadTracker
    {
        private Dictionary<string, UploadInfo> _uploads = new Dictionary<string, UploadInfo>();
        
        public void AddUpload(string fileName, DateTime startTime)
        {
            _uploads[fileName] = new UploadInfo { FileName = fileName, StartTime = startTime };
        }
        
        public void UpdateSize(string fileName, long size)
        {
            if (_uploads.ContainsKey(fileName))
                _uploads[fileName].Size = size;
        }
        
        public UploadInfo? RemoveUpload(string fileName)
        {
            if (_uploads.TryGetValue(fileName, out var info))
            {
                _uploads.Remove(fileName);
                return info;
            }
            return null;
        }
        
        public long GetSize(string fileName)
        {
            return _uploads.TryGetValue(fileName, out var info) ? info.Size : 0;
        }
        
        public int Count => _uploads.Count;
    }

    public class UploadInfo
    {
        public string FileName { get; set; } = "";
        public DateTime StartTime { get; set; }
        public long Size { get; set; }
    }

    public class AppSettings
    {
        public string LogLevel { get; set; } = "";
        public int MaxUsers { get; set; }
        public string Theme { get; set; } = "";
    }

    public class BackupManifest
    {
        public DateTime CreatedAt { get; set; }
        public List<BackupEntry> Backups { get; set; } = new List<BackupEntry>();
    }

    public class BackupEntry
    {
        public DateTime Timestamp { get; set; }
        public int SourceFiles { get; set; }
        public string Description { get; set; } = "";
    }
}