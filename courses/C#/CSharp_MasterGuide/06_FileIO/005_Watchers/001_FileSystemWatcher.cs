/*
TOPIC: C# File I/O Operations
SUBTOPIC: FileSystemWatcher
FILE: 01_FileSystemWatcher.cs
PURPOSE: FileSystemWatcher basics - monitoring file system changes
*/

using System;
using System.IO;

namespace CSharp_MasterGuide._06_FileIO._05_Watchers
{
    public class NN_01_FileSystemWatcher
    {
        private static int _eventCount = 0;
        
        public static void Main(string[] args)
        {
            Console.WriteLine("=== FileSystemWatcher Basics Demo ===");
            Console.WriteLine();

            BasicWatcherDemo();
            Console.WriteLine();

            EventTypesDemo();
            Console.WriteLine();

            WatchSubdirectoryDemo();
            Console.WriteLine();

            RealWorldExample_MonitorDownloads();
            
            CleanupDemoFiles();
        }

        private static void BasicWatcherDemo()
        {
            Console.WriteLine("--- Basic FileSystemWatcher Demo ---");
            
            string watchDir = "NN_watch_basic";
            Directory.CreateDirectory(watchDir);
            
            using (FileSystemWatcher watcher = new FileSystemWatcher(watchDir))
            {
                watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite;
                
                watcher.Created += OnFileCreated;
                watcher.Changed += OnFileChanged;
                watcher.Deleted += OnFileDeleted;
                watcher.Renamed += OnFileRenamed;
                
                watcher.EnableRaisingEvents = true;
                
                Console.WriteLine("Watching directory: " + watchDir);
                Console.WriteLine("Events enabled. Creating some files...");
                
                File.WriteAllText(Path.Combine(watchDir, "file1.txt"), "Content 1");
                System.Threading.Thread.Sleep(100);
                
                File.WriteAllText(Path.Combine(watchDir, "file2.txt"), "Content 2");
                System.Threading.Thread.Sleep(100);
                
                File.Delete(Path.Combine(watchDir, "file1.txt"));
                System.Threading.Thread.Sleep(100);
                
                File.Move(Path.Combine(watchDir, "file2.txt"), Path.Combine(watchDir, "renamed_file.txt"));
                System.Threading.Thread.Sleep(100);
                
                Console.WriteLine($"Total events received: {_eventCount}");
            }
            
            Console.WriteLine("// Output: FileSystemWatcher detects basic file operations");
            
            Directory.Delete(watchDir, true);
            _eventCount = 0;
        }

        private static void EventTypesDemo()
        {
            Console.WriteLine("--- Different Event Types Demo ---");
            
            string watchDir = "NN_watch_events";
            Directory.CreateDirectory(watchDir);
            
            using (FileSystemWatcher watcher = new FileSystemWatcher(watchDir))
            {
                watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName | 
                                       NotifyFilters.LastWrite | NotifyFilters.Size;
                
                watcher.Created += (s, e) => Console.WriteLine($"  CREATED: {e.Name}");
                watcher.Changed += (s, e) => Console.WriteLine($"  CHANGED: {e.Name} ({e.ChangeType})");
                watcher.Deleted += (s, e) => Console.WriteLine($"  DELETED: {e.Name}");
                watcher.Renamed += (s, e) => Console.WriteLine($"  RENAMED: {e.OldName} -> {e.Name}");
                watcher.Error += (s, e) => Console.WriteLine($"  ERROR: {e.GetException().Message}");
                
                watcher.EnableRaisingEvents = true;
                
                Console.WriteLine("Creating files:");
                File.WriteAllText(Path.Combine(watchDir, "new.txt"), "new");
                System.Threading.Thread.Sleep(50);
                
                Console.WriteLine("Modifying file:");
                File.AppendAllText(Path.Combine(watchDir, "new.txt"), " modified");
                System.Threading.Thread.Sleep(50);
                
                Console.WriteLine("Creating directory:");
                Directory.CreateDirectory(Path.Combine(watchDir, "newdir"));
                System.Threading.Thread.Sleep(50);
                
                Console.WriteLine("Deleting file:");
                File.Delete(Path.Combine(watchDir, "new.txt"));
                System.Threading.Thread.Sleep(50);
            }
            
            Console.WriteLine("// Output: Different event types fired for various operations");
            
            Directory.Delete(watchDir, true);
        }

        private static void WatchSubdirectoryDemo()
        {
            Console.WriteLine("--- Watch Subdirectories Demo ---");
            
            string watchDir = "NN_watch_tree";
            string subDir1 = Path.Combine(watchDir, "sub1");
            string subDir2 = Path.Combine(watchDir, "sub1", "sub2");
            
            Directory.CreateDirectory(subDir2);
            
            using (FileSystemWatcher watcher = new FileSystemWatcher(watchDir))
            {
                watcher.IncludeSubdirectories = true;
                watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName;
                
                int events = 0;
                watcher.Created += (s, e) => 
                {
                    Console.WriteLine($"  CREATED: {e.FullPath}");
                    events++;
                };
                
                watcher.EnableRaisingEvents = true;
                
                Console.WriteLine("Creating files in subdirectories:");
                File.WriteAllText(Path.Combine(watchDir, "root.txt"), "root");
                System.Threading.Thread.Sleep(50);
                
                File.WriteAllText(Path.Combine(subDir1, "sub1.txt"), "sub1");
                System.Threading.Thread.Sleep(50);
                
                File.WriteAllText(Path.Combine(subDir2, "sub2.txt"), "sub2");
                System.Threading.Thread.Sleep(50);
                
                Console.WriteLine($"Total subdirectory events: {events}");
            }
            
            Console.WriteLine("// Output: IncludeSubdirectories monitors entire tree");
            
            Directory.Delete(watchDir, true);
        }

        private static void RealWorldExample_MonitorDownloads()
        {
            Console.WriteLine();
            Console.WriteLine("=== REAL-WORLD EXAMPLE: Download Monitor ===");
            
            string downloadDir = "NN_downloads";
            string processedDir = "NN_processed";
            
            Directory.CreateDirectory(downloadDir);
            Directory.CreateDirectory(processedDir);
            
            Console.WriteLine("Monitoring downloads folder...");
            
            using (FileSystemWatcher watcher = new FileSystemWatcher(downloadDir))
            {
                watcher.Filter = "*.*";
                watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.CreationTime;
                
                int newFiles = 0;
                int completedFiles = 0;
                
                watcher.Created += (s, e) =>
                {
                    newFiles++;
                    Console.WriteLine($"[NEW] Download detected: {e.Name}");
                };
                
                watcher.Changed += (s, e) =>
                {
                    if (e.ChangeType == WatcherChangeTypes.Created || e.ChangeType == WatcherChangeTypes.Changed)
                    {
                        Console.WriteLine($"[CHANGE] File changed: {e.Name}");
                    }
                };
                
                watcher.EnableRaisingEvents = true;
                
                Console.WriteLine("Simulating downloads:");
                
                string file1 = Path.Combine(downloadDir, "document.pdf");
                File.WriteAllText(file1, "PDF content");
                System.Threading.Thread.Sleep(100);
                
                string file2 = Path.Combine(downloadDir, "image.jpg");
                File.WriteAllText(file2, "Image content");
                System.Threading.Thread.Sleep(100);
                
                string file3 = Path.Combine(downloadDir, "archive.zip");
                File.WriteAllText(file3, "Archive content");
                System.Threading.Thread.Sleep(100);
                
                Console.WriteLine("Processing completed downloads:");
                string[] downloads = Directory.GetFiles(downloadDir);
                foreach (string file in downloads)
                {
                    string fileName = Path.GetFileName(file);
                    string dest = Path.Combine(processedDir, fileName);
                    File.Move(file, dest);
                    completedFiles++;
                    Console.WriteLine($"  Processed: {fileName}");
                }
                
                Console.WriteLine($"Summary: {newFiles} new files, {completedFiles} processed");
            }
            
            Directory.Delete(downloadDir, true);
            Directory.Delete(processedDir, true);
            Console.WriteLine("// Output: Download folder monitored and processed");
        }

        private static void OnFileCreated(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"  Created: {e.Name}");
            _eventCount++;
        }

        private static void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"  Changed: {e.Name}");
            _eventCount++;
        }

        private static void OnFileDeleted(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"  Deleted: {e.Name}");
            _eventCount++;
        }

        private static void OnFileRenamed(object sender, RenamedEventArgs e)
        {
            Console.WriteLine($"  Renamed: {e.OldName} -> {e.Name}");
            _eventCount++;
        }

        private static void CleanupDemoFiles()
        {
            string[] dirs = { "NN_watch_basic", "NN_watch_events", "NN_watch_tree", "NN_downloads", "NN_processed" };
            foreach (string d in dirs)
            {
                if (Directory.Exists(d)) Directory.Delete(d, true);
            }
            Console.WriteLine("[Cleanup] Demo directories removed");
        }
    }
}