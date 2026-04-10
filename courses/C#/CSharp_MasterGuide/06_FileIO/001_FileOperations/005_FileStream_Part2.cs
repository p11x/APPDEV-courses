/*
    TOPIC: C# File I/O Operations
    SUBTOPIC: FileStream Advanced - Buffering and Async
    FILE: 05_FileStream_Part2.cs
    PURPOSE: Demonstrates advanced FileStream features including
             buffering, async operations, and FileShare modes
*/

using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CSharp_MasterGuide._06_FileIO._01_FileOperations
{
    public class FileStreamPart2Demo
    {
        private static readonly string BasePath = Path.Combine(Path.GetTempPath(), "FileStream_Part2_Demo");
        
        public static void Main(string[] args)
        {
            SetupDirectory();
            
            DemonstrateBufferedStream();
            DemonstrateAsyncFileStream();
            DemonstrateFileShareModes();
            DemonstrateFileStreamOptions();
            DemonstrateRealWorldExamples();
            
            Console.WriteLine("\n=== FileStream Part 2 demos completed ===");
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
        
        private static void DemonstrateBufferedStream()
        {
            Console.WriteLine("\n=== BufferedStream Demo ===");
            
            string filePath = Path.Combine(BasePath, "buffered_demo.dat");
            
            // Create buffered stream on top of FileStream
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            using (BufferedStream bufferedStream = new BufferedStream(fileStream, 8192))
            {
                // Write many small writes - buffered internally
                for (int i = 0; i < 100; i++)
                {
                    string line = $"Line {i}: This is some text data for buffering demonstration.\n";
                    byte[] data = Encoding.UTF8.GetBytes(line);
                    bufferedStream.Write(data, 0, data.Length);
                }
                
                Console.WriteLine("Data written to buffered stream");
                // Output: Data written to buffered stream
            }
            
            // Verify data was written
            long fileSize = new FileInfo(filePath).Length;
            Console.WriteLine($"File size after buffered writes: {fileSize} bytes");
            // Output: File size: ~3600 bytes
            
            // Reading with BufferedStream
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            using (BufferedStream bufferedStream = new BufferedStream(fileStream))
            using (MemoryStream ms = new MemoryStream())
            {
                bufferedStream.CopyTo(ms);
                string content = Encoding.UTF8.GetString(ms.ToArray());
                string[] lines = content.Split('\n');
                Console.WriteLine($"Lines read: {lines.Length}");
                // Output: Lines read: 100
            }
            
            // Performance comparison - unbuffered vs buffered
            string unbufferedFile = Path.Combine(BasePath, "unbuffered.dat");
            string bufferedFile = Path.Combine(BasePath, "buffered_perf.dat");
            
            // Unbuffered writes
            var sw = System.Diagnostics.Stopwatch.StartNew();
            using (FileStream fs = new FileStream(unbufferedFile, FileMode.Create, FileAccess.Write))
            {
                for (int i = 0; i < 1000; i++)
                {
                    fs.WriteByte((byte)(i % 256));
                }
            }
            sw.Stop();
            Console.WriteLine($"Unbuffered time: {sw.ElapsedTicks} ticks");
            
            // Buffered writes
            sw.Restart();
            using (FileStream fs = new FileStream(bufferedFile, FileMode.Create, FileAccess.Write))
            using (BufferedStream bs = new BufferedStream(fs, 4096))
            {
                for (int i = 0; i < 1000; i++)
                {
                    bs.WriteByte((byte)(i % 256));
                }
            }
            sw.Stop();
            Console.WriteLine($"Buffered time: {sw.ElapsedTicks} ticks");
            // Output shows buffered is faster for many small writes
        }
        
        private static async Task DemonstrateAsyncFileStream()
        {
            Console.WriteLine("\n=== Async FileStream Demo ===");
            
            string asyncFile = Path.Combine(BasePath, "async_demo.txt");
            
            // Async write operation
            string content = "This is async content written to file.\nMultiple lines.\nMore data.";
            byte[] data = Encoding.UTF8.GetBytes(content);
            
            using (FileStream fs = new FileStream(asyncFile, FileMode.Create, FileAccess.Write, FileShare.None, 4096, FileOptions.Asynchronous))
            {
                await fs.WriteAsync(data, 0, data.Length);
                Console.WriteLine("Async write completed");
                // Output: Async write completed
            }
            
            // Async read operation
            using (FileStream fs = new FileStream(asyncFile, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, FileOptions.Asynchronous))
            {
                byte[] buffer = new byte[data.Length];
                int bytesRead = await fs.ReadAsync(buffer, 0, buffer.Length);
                string readContent = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Async read: {readContent.Split('\n')[0]}");
                // Output: Async read: This is async content written to file.
            }
            
            // Multiple async operations
            string multiFile = Path.Combine(BasePath, "multi_async.txt");
            using (FileStream fs = new FileStream(multiFile, FileMode.Create, FileAccess.ReadWrite, FileShare.None, 4096, FileOptions.Asynchronous))
            {
                var tasks = new Task[5];
                for (int i = 0; i < 5; i++)
                {
                    int taskNum = i;
                    tasks[i] = Task.Run(async () =>
                    {
                        byte[] chunk = Encoding.UTF8.GetBytes($"Task {taskNum} data\n");
                        await fs.WriteAsync(chunk, 0, chunk.Length);
                    });
                }
                
                await Task.WhenAll(tasks);
                Console.WriteLine("All async tasks completed");
                // Output: All async tasks completed
            }
            
            // Read back
            fs.Position = 0;
            using (MemoryStream ms = new MemoryStream())
            {
                await fs.CopyToAsync(ms);
                Console.WriteLine($"Total content length: {ms.Length}");
                // Output: Total content length: 65
            }
            
            // Using async with cancellation
            await AsyncWithCancellation();
        }
        
        private static async Task AsyncWithCancellation()
        {
            Console.WriteLine("\n--- Async with Cancellation ---");
            
            string cancelFile = Path.Combine(BasePath, "cancel_demo.dat");
            using (FileStream fs = new FileStream(cancelFile, FileMode.Create, FileAccess.Write, FileShare.None, 4096, FileOptions.Asynchronous))
            {
                var cts = new System.Threading.CancellationTokenSource();
                
                try
                {
                    byte[] largeData = new byte[100000];
                    await fs.WriteAsync(largeData, 0, largeData.Length, cts.Token);
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("Operation was cancelled");
                }
            }
        }
        
        private static void DemonstrateFileShareModes()
        {
            Console.WriteLine("\n=== FileShare Modes Demo ===");
            
            string sharedFile = Path.Combine(BasePath, "shared_file.txt");
            
            // Default: exclusive access
            using (FileStream fs1 = new FileStream(sharedFile, FileMode.Create, FileAccess.Write))
            {
                fs1.Write(Encoding.UTF8.GetBytes("Exclusive write"), 0, 15);
                Console.WriteLine("Exclusive write done");
                // Output: Exclusive write done
                
                // This would fail:
                // using (FileStream fs2 = new FileStream(sharedFile, FileMode.Open, FileAccess.Read)) { }
            }
            
            // FileShare.Read - allow other readers
            File.WriteAllText(sharedFile, "Initial content");
            
            using (FileStream fs1 = new FileStream(sharedFile, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                Console.WriteLine($"Reader 1: {new StreamReader(fs1).ReadToEnd()}");
                // Output: Reader 1: Initial content
                
                // Now another process can also read
                using (FileStream fs2 = new FileStream(sharedFile, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    Console.WriteLine($"Reader 2: {new StreamReader(fs2).ReadToEnd()}");
                    // Output: Reader 2: Initial content
                }
            }
            
            // FileShare.ReadWrite - allow read and write
            using (FileStream fs1 = new FileStream(sharedFile, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                fs1.Position = 0;
                Console.WriteLine($"Read: {new StreamReader(fs1).ReadToEnd()}");
                
                fs1.Position = 0;
                fs1.Write(Encoding.UTF8.GetBytes("Updated"), 0, 7);
                fs1.Flush();
            }
            
            // Read updated content
            Console.WriteLine($"After update: {File.ReadAllText(sharedFile)}");
            // Output: After update: Updated
            
            // FileShare.Delete - allow file deletion while open
            using (FileStream fs = new FileStream(sharedFile, FileMode.Open, FileAccess.Read, FileShare.Read | FileShare.Delete))
            {
                // File can be deleted while we're reading
                Console.WriteLine("File opened with delete sharing");
                // Output: File opened with delete sharing
            }
            
            // Write with read sharing (common for log files)
            WriteToLogFile();
            ReadFromLogFile();
        }
        
        private static void WriteToLogFile()
        {
            string logFile = Path.Combine(BasePath, "shared_log.txt");
            using (FileStream fs = new FileStream(logFile, FileMode.Append, FileAccess.Write, FileShare.Read))
            {
                string[] logEntries = { "Log: App started", "Log: User logged in", "Log: Data loaded" };
                foreach (string entry in logEntries)
                {
                    byte[] data = Encoding.UTF8.GetBytes(entry + Environment.NewLine);
                    fs.Write(data, 0, data.Length);
                }
            }
        }
        
        private static void ReadFromLogFile()
        {
            string logFile = Path.Combine(BasePath, "shared_log.txt");
            using (FileStream fs = new FileStream(logFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                string content = new StreamReader(fs).ReadToEnd();
                Console.WriteLine("Log content:");
                Console.WriteLine(content);
                // Output: Log: App started
                // Output: Log: User logged in
                // Output: Log: Data loaded
            }
        }
        
        private static void DemonstrateFileStreamOptions()
        {
            Console.WriteLine("\n=== FileStreamOptions Demo ===");
            
            string filePath = Path.Combine(BasePath, "options_demo.txt");
            
            // Using FileStreamOptions (C# 10+)
            var options = new FileStreamOptions
            {
                Mode = FileMode.Create,
                Access = FileAccess.ReadWrite,
                Share = FileShare.Read,
                BufferSize = 4096,
                Options = FileOptions.Asynchronous | FileOptions.SequentialScan
            };
            
            using (FileStream fs = new FileStream(filePath, options))
            {
                Console.WriteLine($"Stream created with options: Async={fs.IsAsync}, BufferSize={options.BufferSize}");
                // Output: Stream created with options: Async=True, BufferSize=4096
            }
            
            // Pre-allocate file space
            string preallocFile = Path.Combine(BasePath, "prealloc.dat");
            using (FileStream fs = new FileStream(preallocFile, FileMode.Create, FileAccess.Write))
            {
                fs.SetLength(1024 * 1024);  // 1MB
            }
            
            long length = new FileInfo(preallocFile).Length;
            Console.WriteLine($"Pre-allocated file size: {length} bytes");
            // Output: Pre-allocated file size: 1048576 bytes
            
            // Random access hints
            string randomAccessFile = Path.Combine(BasePath, "random_access.dat");
            using (FileStream fs = new FileStream(randomAccessFile, FileMode.Create, FileAccess.Write, FileShare.None, 4096, FileOptions.RandomAccess))
            {
                for (int i = 0; i < 100; i++)
                {
                    fs.WriteByte((byte)i);
                }
            }
            Console.WriteLine("Random access file created");
            // Output: Random access file created
        }
        
        private static void DemonstrateRealWorldExamples()
        {
            Console.WriteLine("\n=== Real-World Examples ===");
            
            // Example 1: Concurrent file processing
            ConcurrentFileProcessing();
            
            // Example 2: Large file async upload simulation
            AsyncLargeFileProcessing();
            
            // Example 3: Producer-consumer with buffering
            BufferedProducerConsumer();
        }
        
        private static void ConcurrentFileProcessing()
        {
            Console.WriteLine("\n--- Concurrent File Processing ---");
            
            string sourceFile = Path.Combine(BasePath, "concurrent_source.dat");
            string[] destFiles = new string[4];
            for (int i = 0; i < 4; i++)
            {
                destFiles[i] = Path.Combine(BasePath, $"concurrent_part_{i}.dat");
            }
            
            // Create source file with sequential data
            using (FileStream fs = new FileStream(sourceFile, FileMode.Create, FileAccess.Write))
            {
                for (int i = 0; i < 400; i++)
                {
                    fs.WriteByte((byte)i);
                }
            }
            
            // Split into 4 parts concurrently
            Parallel.For(0, 4, partIndex =>
            {
                int startByte = partIndex * 100;
                int bytesToRead = 100;
                
                using (FileStream source = new FileStream(sourceFile, FileMode.Open, FileAccess.Read, FileShare.Read))
                using (FileStream dest = new FileStream(destFiles[partIndex], FileMode.Create, FileAccess.Write))
                {
                    source.Seek(startByte, SeekOrigin.Begin);
                    byte[] buffer = new byte[bytesToRead];
                    source.Read(buffer, 0, bytesToRead);
                    dest.Write(buffer, 0, bytesToRead);
                }
            });
            
            Console.WriteLine("Concurrent processing completed");
            // Output: Concurrent processing completed
            
            // Verify each part
            for (int i = 0; i < 4; i++)
            {
                long size = new FileInfo(destFiles[i]).Length;
                Console.WriteLine($"Part {i}: {size} bytes");
                // Output: Part 0: 100 bytes, Part 1: 100 bytes, etc.
            }
        }
        
        private static async Task AsyncLargeFileProcessing()
        {
            Console.WriteLine("\n--- Async Large File Processing ---");
            
            string largeFile = Path.Combine(BasePath, "large_async.dat");
            
            // Simulate large file with progress
            long totalSize = 50000;
            using (FileStream fs = new FileStream(largeFile, FileMode.Create, FileAccess.Write, FileShare.None, 4096, FileOptions.Asynchronous))
            {
                byte[] chunk = new byte[1000];
                for (int i = 0; i < 50; i++)
                {
                    for (int j = 0; j < 1000; j++)
                    {
                        chunk[j] = (byte)(i + j);
                    }
                    await fs.WriteAsync(chunk, 0, chunk.Length);
                    
                    if (i % 10 == 0)
                    {
                        Console.WriteLine($"Progress: {(i * 1000) * 100 / totalSize}%");
                    }
                }
            }
            
            Console.WriteLine("Large file written asynchronously");
            // Output: Large file written asynchronously
            
            // Async read with progress
            using (FileStream fs = new FileStream(largeFile, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, FileOptions.Asynchronous))
            {
                byte[] buffer = new byte[1024];
                long totalRead = 0;
                int read;
                
                while ((read = await fs.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    totalRead += read;
                }
                
                Console.WriteLine($"Async read complete: {totalRead} bytes");
                // Output: Async read complete: 50000 bytes
            }
        }
        
        private static void BufferedProducerConsumer()
        {
            Console.WriteLine("\n--- Buffered Producer-Consumer ---");
            
            string bufferFile = Path.Combine(BasePath, "buffered_queue.dat");
            
            // Clear previous
            if (File.Exists(bufferFile)) File.Delete(bufferFile);
            
            // Producer - writes data periodically
            Task.Run(() =>
            {
                using (FileStream fs = new FileStream(bufferFile, FileMode.Append, FileAccess.Write, FileShare.Read))
                {
                    for (int i = 0; i < 10; i++)
                    {
                        string message = $"Message {i}\n";
                        byte[] data = Encoding.UTF8.GetBytes(message);
                        fs.Write(data, 0, data.Length);
                        Thread.Sleep(50);
                    }
                }
            });
            
            // Consumer - reads data as it becomes available
            Thread.Sleep(100);  // Let producer start
            
            using (FileStream fs = new FileStream(bufferFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                string allContent = "";
                int previousLength = 0;
                
                for (int check = 0; check < 20; check++)
                {
                    if (fs.Length > previousLength)
                    {
                        fs.Seek(previousLength, SeekOrigin.Begin);
                        byte[] newData = new byte[fs.Length - previousLength];
                        fs.Read(newData, 0, newData.Length);
                        allContent += Encoding.UTF8.GetString(newData);
                        previousLength = (int)fs.Length;
                    }
                    Thread.Sleep(30);
                }
                
                Console.WriteLine($"Consumer collected {allContent.Split('\n').Length} messages");
                // Output: Consumer collected 11 messages (10 + empty)
            }
        }
    }
}