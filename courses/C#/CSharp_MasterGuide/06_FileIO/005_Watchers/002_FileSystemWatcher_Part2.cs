/*
TOPIC: C# File I/O Operations
SUBTOPIC: FileSystemWatcher
FILE: 02_FileSystemWatcher_Part2.cs
PURPOSE: More FileSystemWatcher - filters, event buffering, properties
*/

using System;
using System.IO;
using System.Threading;

namespace CSharp_MasterGuide._06_FileIO._05_Watchers
{
    public class NN_02_FileSystemWatcher_Part2
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== FileSystemWatcher Advanced Demo ===");
            Console.WriteLine();

            FilterPatternsDemo();
            Console.WriteLine();

            NotifyFilterDemo();
            Console.WriteLine();

            BufferOverflowHandling();
            Console.WriteLine();

            WaitForChangedDemo();
            Console.WriteLine();

            RealWorldExample_FolderSynchronizer();
            
            CleanupDemoFiles();
        }

        private static void FilterPatternsDemo()
        {
            Console.WriteLine("--- Filter Patterns Demo ---");
            
            string watchDir = "NN_watch_filter";
            Directory.CreateDirectory(watchDir);
            
            Console.WriteLine("Watching for *.txt files only:");
            using (FileSystemWatcher watcher = new FileSystemWatcher(watchDir))
            {
                watcher.Filter = "*.txt";
                watcher.Created += (s, e) => Console.WriteLine($"  TXT created: {e.Name}");
                
                watcher.EnableRaisingEvents = true;
                
                File.WriteAllText(Path.Combine(watchDir, "document.txt"), "text");
                Thread.Sleep(50);
                
                File.WriteAllText(Path.Combine(watchDir, "image.jpg"), "image");
                Thread.Sleep(50);
                
                File.WriteAllText(Path.Combine(watchDir, "data.json"), "json");
                Thread.Sleep(50);
                
                File.WriteAllText(Path.Combine(watchDir, "readme.txt"), "readme");
                Thread.Sleep(50);
            }
            
            Console.WriteLine();
            Console.WriteLine("Using Filter with specific file:");
            using (FileSystemWatcher watcher = new FileSystemWatcher(watchDir))
            {
                watcher.Filter = "specific_file.txt";
                int specificEvents = 0;
                watcher.Created += (s, e) => { specificEvents++; Console.WriteLine($"  Specific file: {e.Name}"); };
                
                watcher.EnableRaisingEvents = true;
                
                File.WriteAllText(Path.Combine(watchDir, "specific_file.txt"), "target");
                Thread.Sleep(50);
                
                File.WriteAllText(Path.Combine(watchDir, "other.txt"), "other");
                Thread.Sleep(50);
                
                Console.WriteLine($"Specific file events: {specificEvents}");
            }
            
            Directory.Delete(watchDir, true);
            Console.WriteLine("// Output: Filter property limits watched files");
        }

        private static void NotifyFilterDemo()
        {
            Console.WriteLine("--- NotifyFilter Options Demo ---");
            
            string watchDir = "NN_watch_notify";
            Directory.CreateDirectory(watchDir);
            
            string testFile = Path.Combine(watchDir, "test.txt");
            
            Console.WriteLine("Testing different NotifyFilter options:");
            
            using (FileSystemWatcher watcher = new FileSystemWatcher(watchDir))
            {
                watcher.Filter = "test.txt";
                watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size;
                
                int changeCount = 0;
                watcher.Changed += (s, e) =>
                {
                    changeCount++;
                    Console.WriteLine($"  Changed event #{changeCount}: {e.ChangeType}");
                };
                
                watcher.EnableRaisingEvents = true;
                
                File.WriteAllText(testFile, "version 1");
                Thread.Sleep(50);
                
                File.WriteAllText(testFile, "version 2");
                Thread.Sleep(50);
                
                File.WriteAllText(testFile, "version 3");
                Thread.Sleep(50);
                
                Console.WriteLine($"Total change events: {changeCount}");
            }
            
            Console.WriteLine();
            Console.WriteLine("All NotifyFilter options:");
            Array filters = Enum.GetValues(typeof(NotifyFilters));
            foreach (NotifyFilters filter in filters)
            {
                Console.WriteLine($"  {filter}");
            }
            
            Directory.Delete(watchDir, true);
            Console.WriteLine("// Output: NotifyFilter controls what changes are detected");
        }

        private static void BufferOverflowHandling()
        {
            Console.WriteLine("--- Buffer Overflow Handling ---");
            
            string watchDir = "NN_watch_buffer";
            Directory.CreateDirectory(watchDir);
            
            using (FileSystemWatcher watcher = new FileSystemWatcher(watchDir))
            {
                watcher.NotifyFilter = NotifyFilters.FileName;
                watcher.InternalBufferSize = 4096;
                
                Console.WriteLine($"Buffer size: {watcher.InternalBufferSize} bytes");
                
                watcher.Created += (s, e) => Console.WriteLine($"  Created: {e.Name}");
                watcher.Error += (s, e) =>
                {
                    Console.WriteLine($"  BUFFER OVERFLOW: {e.GetException().Message}");
                    
                    watcher.EnableRaisingEvents = false;
                    
                    string[] existingFiles = Directory.GetFiles(watchDir);
                    foreach (string f in existingFiles)
                    {
                        Console.WriteLine($"  Missed file detected: {Path.GetFileName(f)}");
                    }
                    
                    watcher.EnableRaisingEvents = true;
                };
                
                watcher.EnableRaisingEvents = true;
                
                Console.WriteLine("Creating many files quickly:");
                for (int i = 0; i < 100; i++)
                {
                    File.WriteAllText(Path.Combine(watchDir, $"file_{i:D3}.txt"), $"content {i}");
                }
                Thread.Sleep(100);
                
                int createdCount = Directory.GetFiles(watchDir).Length;
                Console.WriteLine($"Files created: {createdCount}/100");
            }
            
            Directory.Delete(watchDir, true);
            Console.WriteLine("// Output: Large buffer prevents overflow, error handler recovers");
        }

        private static void WaitForChangedDemo()
        {
            Console.WriteLine("--- WaitForChanged Method Demo ---");
            
            string watchDir = "NN_watch_wait";
            Directory.CreateDirectory(watchDir);
            
            using (FileSystemWatcher watcher = new FileSystemWatcher(watchDir))
            {
                watcher.Filter = "*.txt";
                watcher.EnableRaisingEvents = true;
                
                Console.WriteLine("Testing WaitForChanged (synchronous):");
                
                File.WriteAllText(Path.Combine(watchDir, "async.txt"), "async content");
                var result1 = watcher.WaitForChanged(WatcherChangeTypes.Created, 1000);
                Console.WriteLine($"  Result: {result1.ChangeType}, Name: {result1.Name}");
                
                string toDelete = Path.Combine(watchDir, "delete_me.txt");
                File.WriteAllText(toDelete, "will be deleted");
                var result2 = watcher.WaitForChanged(WatcherChangeTypes.Deleted, 1000);
                Console.WriteLine($"  Result: {result2.ChangeType}, Name: {result2.Name}");
                
                string renamed = Path.Combine(watchDir, "rename_source.txt");
                File.WriteAllText(renamed, "rename");
                string renamedDest = Path.Combine(watchDir, "rename_target.txt");
                File.Move(renamed, renamedDest);
                var result3 = watcher.WaitForChanged(WatcherChangeTypes.Renamed, 1000);
                Console.WriteLine($"  Result: {result3.ChangeType}, OldName: {result3.OldName}, Name: {result3.Name}");
            }
            
            Directory.Delete(watchDir, true);
            Console.WriteLine("// Output: WaitForChanged blocks until event occurs or timeout");
        }

        private static void RealWorldExample_FolderSynchronizer()
        {
            Console.WriteLine();
            Console.WriteLine("=== REAL-WORLD EXAMPLE: Folder Synchronizer ===");
            
            string sourceDir = "NN_sync_source";
            string destDir = "NN_sync_destination";
            
            Directory.CreateDirectory(sourceDir);
            Directory.CreateDirectory(destDir);
            
            Console.WriteLine("Starting two-way folder sync...");
            
            int syncCount = 0;
            
            using (FileSystemWatcher sourceWatcher = new FileSystemWatcher(sourceDir))
            using (FileSystemWatcher destWatcher = new FileSystemWatcher(destDir))
            {
                sourceWatcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite;
                destWatcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite;
                
                sourceWatcher.Created += (s, e) =>
                {
                    string destPath = Path.Combine(destDir, e.Name);
                    if (!File.Exists(destPath))
                    {
                        File.Copy(e.FullPath, destPath);
                        syncCount++;
                        Console.WriteLine($"  [SYNC->] {e.Name}");
                    }
                };
                
                sourceWatcher.Changed += (s, e) =>
                {
                    string destPath = Path.Combine(destDir, e.Name);
                    File.Copy(e.FullPath, destPath, true);
                    syncCount++;
                    Console.WriteLine($"  [SYNC->] {e.Name} (updated)");
                };
                
                sourceWatcher.Deleted += (s, e) =>
                {
                    string destPath = Path.Combine(destDir, e.Name);
                    if (File.Exists(destPath))
                    {
                        File.Delete(destPath);
                        syncCount++;
                        Console.WriteLine($"  [DEL->] {e.Name}");
                    }
                };
                
                sourceWatcher.EnableRaisingEvents = true;
                destWatcher.EnableRaisingEvents = true;
                
                Console.WriteLine("Source changes:");
                File.WriteAllText(Path.Combine(sourceDir, "file1.txt"), "content 1");
                Thread.Sleep(50);
                
                File.WriteAllText(Path.Combine(sourceDir, "file2.txt"), "content 2");
                Thread.Sleep(50);
                
                File.WriteAllText(Path.Combine(sourceDir, "file1.txt"), "updated content 1");
                Thread.Sleep(50);
                
                File.Delete(Path.Combine(sourceDir, "file2.txt"));
                Thread.Sleep(50);
                
                File.WriteAllText(Path.Combine(sourceDir, "file3.txt"), "content 3");
                Thread.Sleep(50);
                
                Console.WriteLine("Destination changes (reverse sync):");
                File.WriteAllText(Path.Combine(destDir, "file4.txt"), "from dest");
                Thread.Sleep(50);
                
                Console.WriteLine($"Total sync operations: {syncCount}");
                Console.WriteLine("Source files:");
                foreach (string f in Directory.GetFiles(sourceDir))
                    Console.WriteLine($"  {Path.GetFileName(f)}");
                Console.WriteLine("Destination files:");
                foreach (string f in Directory.GetFiles(destDir))
                    Console.WriteLine($"  {Path.GetFileName(f)}");
            }
            
            Directory.Delete(sourceDir, true);
            Directory.Delete(destDir, true);
            Console.WriteLine("// Output: Bidirectional folder synchronization");
        }

        private static void CleanupDemoFiles()
        {
            string[] dirs = { "NN_watch_filter", "NN_watch_notify", "NN_watch_buffer", "NN_watch_wait", "NN_sync_source", "NN_sync_destination" };
            foreach (string d in dirs)
            {
                if (Directory.Exists(d)) Directory.Delete(d, true);
            }
            Console.WriteLine("[Cleanup] Demo directories removed");
        }
    }
}