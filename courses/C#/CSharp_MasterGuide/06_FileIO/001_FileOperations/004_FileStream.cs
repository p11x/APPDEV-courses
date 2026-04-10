/*
    TOPIC: C# File I/O Operations
    SUBTOPIC: FileStream Basics
    FILE: 04_FileStream.cs
    PURPOSE: Demonstrates FileStream for low-level file operations
             including opening, reading, writing, and seeking
*/

using System;
using System.IO;
using System.Text;

namespace CSharp_MasterGuide._06_FileIO._01_FileOperations
{
    public class FileStreamDemo
    {
        private static readonly string BasePath = Path.Combine(Path.GetTempPath(), "FileStream_Demo");
        
        public static void Main(string[] args)
        {
            SetupDirectory();
            
            DemonstrateFileStreamCreation();
            DemonstrateFileStreamWrite();
            DemonstrateFileStreamRead();
            DemonstrateFileStreamSeek();
            DemonstrateRealWorldExamples();
            
            Console.WriteLine("\n=== FileStream demos completed ===");
        }
        
        private static void SetupDirectory()
        {
            if (!Directory.Exists(BasePath))
            {
                Directory.CreateDirectory(BasePath);
            }
            Console.WriteLine($"Working directory: {BasePath}");
        }
        
        private static void DemonstrateFileStreamCreation()
        {
            Console.WriteLine("\n=== FileStream Creation ===");
            
            string filePath = Path.Combine(BasePath, "basic_stream.txt");
            
            // FileMode options explained:
            // CreateNew - Creates new, fails if exists
            // Create - Creates or overwrites
            // Open - Opens existing, fails if doesn't exist
            // OpenOrCreate - Opens or creates
            // Append - Opens or creates, seeks to end
            // Truncate - Opens and truncates to zero length
            
            // Create new file stream with FileMode.Create
            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                Console.WriteLine($"File created: {filePath}");
                // Output: File created: ...\basic_stream.txt
                Console.WriteLine($"Stream length: {fs.Length}");
                // Output: Stream length: 0
            }
            
            // Open existing file
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                Console.WriteLine($"Opened existing file, length: {fs.Length}");
                // Output: Opened existing file, length: 0
            }
            
            // OpenOrCreate - creates if doesn't exist
            string newFilePath = Path.Combine(BasePath, "or_create.txt");
            using (FileStream fs = new FileStream(newFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                Console.WriteLine($"OpenOrCreate result - length: {fs.Length}");
                // Output: OpenOrCreate result - length: 0
            }
        }
        
        private static void DemonstrateFileStreamWrite()
        {
            Console.WriteLine("\n=== FileStream Write Operations ===");
            
            string filePath = Path.Combine(BasePath, "write_demo.fs");
            
            // Write using FileStream
            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                string message = "Hello, FileStream!";
                byte[] data = Encoding.UTF8.GetBytes(message);
                
                // Write entire byte array
                fs.Write(data, 0, data.Length);
                Console.WriteLine($"Written {data.Length} bytes");
                // Output: Written 17 bytes
                
                Console.WriteLine($"Stream position after write: {fs.Position}");
                // Output: Stream position after write: 17
            }
            
            // Append using FileMode.Append
            string appendPath = Path.Combine(BasePath, "append_demo.fs");
            using (FileStream fs = new FileStream(appendPath, FileMode.Create, FileAccess.Write))
            {
                fs.Write(Encoding.UTF8.GetBytes("First "), 0, 6);
            }
            
            using (FileStream fs = new FileStream(appendPath, FileMode.Append, FileAccess.Write))
            {
                fs.Write(Encoding.UTF8.GetBytes("Second"), 0, 6);
            }
            
            // Read back to verify
            byte[] readBack = File.ReadAllBytes(appendPath);
            Console.WriteLine($"Appended content: {Encoding.UTF8.GetString(readBack)}");
            // Output: Appended content: First Second
            
            // Write multiple segments
            string multiPath = Path.Combine(BasePath, "multi_write.fs");
            using (FileStream fs = new FileStream(multiPath, FileMode.Create, FileAccess.Write))
            {
                byte[] header = Encoding.UTF8.GetBytes("HEADER:");
                byte[] body = Encoding.UTF8.GetBytes("Content here");
                byte[] footer = Encoding.UTF8.GetBytes(":FOOTER");
                
                fs.Write(header, 0, header.Length);
                fs.Write(body, 0, body.Length);
                fs.Write(footer, 0, footer.Length);
                
                Console.WriteLine($"Total written: {fs.Position} bytes");
                // Output: Total written: 25 bytes
            }
            
            // Verify
            Console.WriteLine($"File content: {File.ReadAllText(multiPath)}");
            // Output: File content: HEADER:Content here:FOOTER
        }
        
        private static void DemonstrateFileStreamRead()
        {
            Console.WriteLine("\n=== FileStream Read Operations ===");
            
            string filePath = Path.Combine(BasePath, "read_demo.fs");
            string content = "0123456789ABCDEF";
            File.WriteAllBytes(filePath, Encoding.UTF8.GetBytes(content));
            
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                // Read single byte
                int singleByte = fs.ReadByte();
                Console.WriteLine($"First byte (as char): {(char)singleByte}");
                // Output: First byte (as char): 0
                Console.WriteLine($"Position after ReadByte: {fs.Position}");
                // Output: Position after ReadByte: 1
                
                // Reset position
                fs.Position = 0;
                
                // Read multiple bytes
                byte[] buffer = new byte[5];
                int bytesRead = fs.Read(buffer, 0, 5);
                Console.WriteLine($"Bytes read: {bytesRead}");
                // Output: Bytes read: 5
                Console.WriteLine($"Buffer content: {Encoding.UTF8.GetString(buffer)}");
                // Output: Buffer content: 01234
                Console.WriteLine($"Position after Read: {fs.Position}");
                // Output: Position after Read: 5
                
                // Read remaining
                fs.Position = 0;
                using (MemoryStream ms = new MemoryStream())
                {
                    fs.CopyTo(ms);
                    Console.WriteLine($"Remaining bytes read: {ms.Length}");
                    // Output: Remaining bytes read: 16
                }
            }
            
            // Read large file in chunks
            string largeFile = Path.Combine(BasePath, "large_file.fs");
            byte[] largeData = new byte[1000];
            for (int i = 0; i < 1000; i++) largeData[i] = (byte)(i % 256);
            File.WriteAllBytes(largeFile, largeData);
            
            using (FileStream fs = new FileStream(largeFile, FileMode.Open, FileAccess.Read))
            {
                byte[] chunk = new byte[100];
                int totalRead = 0;
                
                while (totalRead < fs.Length)
                {
                    int read = fs.Read(chunk, 0, chunk.Length);
                    if (read == 0) break;
                    totalRead += read;
                }
                
                Console.WriteLine($"Large file - total bytes read: {totalRead}");
                // Output: Large file - total bytes read: 1000
            }
        }
        
        private static void DemonstrateFileStreamSeek()
        {
            Console.WriteLine("\n=== FileStream Seek Operations ===");
            
            string filePath = Path.Combine(BasePath, "seek_demo.fs");
            string content = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            File.WriteAllBytes(filePath, Encoding.UTF8.GetBytes(content));
            
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite))
            {
                // Seek from beginning
                fs.Seek(0, SeekOrigin.Begin);
                Console.WriteLine($"Position at start: {fs.Position}");
                // Output: Position at start: 0
                
                // Seek to specific position
                fs.Seek(5, SeekOrigin.Begin);
                Console.WriteLine($"Position after seek(5): {fs.Position}");
                // Output: Position after seek(5): 5
                
                byte[] buffer = new byte[5];
                fs.Read(buffer, 0, 5);
                Console.WriteLine($"Read from position 5: {Encoding.UTF8.GetString(buffer)}");
                // Output: Read from position 5: FGHIJ
                
                // Seek from current position
                fs.Seek(3, SeekOrigin.Current);
                Console.WriteLine($"Position after seek(3, current): {fs.Position}");
                // Output: Position after seek(3, current): 13
                
                // Seek from end
                fs.Seek(-5, SeekOrigin.End);
                Console.WriteLine($"Position from end: {fs.Position}");
                // Output: Position from end: 21
                
                buffer = new byte[5];
                fs.Read(buffer, 0, 5);
                Console.WriteLine($"Read last 5 chars: {Encoding.UTF8.GetString(buffer)}");
                // Output: Read last 5 chars: VWXYZ
                
                // Write at specific position (overwrite)
                fs.Seek(0, SeekOrigin.Begin);
                fs.Write(Encoding.UTF8.GetBytes("XXXX"), 0, 4);
                
                // Verify
                fs.Position = 0;
                byte[] all = new byte[26];
                fs.Read(all, 0, 26);
                Console.WriteLine($"After overwrite: {Encoding.UTF8.GetString(all)}");
                // Output: After overwrite: XXXDEFGHIJKLMNOPQRSTUVWXYZ
            }
        }
        
        private static void DemonstrateRealWorldExamples()
        {
            Console.WriteLine("\n=== Real-World Examples ===");
            
            // Example 1: Random access file
            RandomAccessDemo();
            
            // Example 2: Binary file read/write
            BinaryRecordDemo();
            
            // Example 3: File segment processing
            SegmentProcessingDemo();
        }
        
        private static void RandomAccessDemo()
        {
            Console.WriteLine("\n--- Random Access File Demo ---");
            
            string dataFile = Path.Combine(BasePath, "random_access.dat");
            
            // Create file with fixed-size records
            const int RecordSize = 50;
            using (FileStream fs = new FileStream(dataFile, FileMode.Create, FileAccess.Write))
            {
                string[] records = { "Record1", "Record2", "Record3", "Record4", "Record5" };
                foreach (string record in records)
                {
                    byte[] recordBytes = new byte[RecordSize];
                    byte[] content = Encoding.UTF8.GetBytes(record);
                    Array.Copy(content, recordBytes, content.Length);
                    fs.Write(recordBytes, 0, RecordSize);
                }
            }
            
            // Read specific record (e.g., record #3 - index 2)
            using (FileStream fs = new FileStream(dataFile, FileMode.Open, FileAccess.Read))
            {
                int recordIndex = 2;
                long offset = recordIndex * RecordSize;
                
                fs.Seek(offset, SeekOrigin.Begin);
                byte[] recordData = new byte[RecordSize];
                fs.Read(recordData, 0, RecordSize);
                
                string record = Encoding.UTF8.GetString(recordData).Trim('\0');
                Console.WriteLine($"Record {recordIndex + 1}: {record}");
                // Output: Record 3: Record3
            }
            
            // Update specific record
            using (FileStream fs = new FileStream(dataFile, FileMode.Open, FileAccess.ReadWrite))
            {
                int recordIndex = 1;
                long offset = recordIndex * RecordSize;
                
                fs.Seek(offset, SeekOrigin.Begin);
                byte[] newRecord = Encoding.UTF8.GetBytes("UPDATED");
                fs.Write(newRecord, 0, newRecord.Length);
            }
            
            // Verify update
            using (FileStream fs = new FileStream(dataFile, FileMode.Open, FileAccess.Read))
            {
                byte[] recordData = new byte[RecordSize];
                fs.Seek(RecordSize, SeekOrigin.Begin);
                fs.Read(recordData, 0, RecordSize);
                Console.WriteLine($"Updated record 2: {Encoding.UTF8.GetString(recordData).Trim('\0')}");
                // Output: Updated record 2: UPDATED
            }
        }
        
        private static void BinaryRecordDemo()
        {
            Console.WriteLine("\n--- Binary Record Demo ---");
            
            string binaryFile = Path.Combine(BasePath, "binary_records.bin");
            
            // Write structured binary data
            using (FileStream fs = new FileStream(binaryFile, FileMode.Create, FileAccess.Write))
            {
                // Record structure: int Id (4 bytes) + string Name (30 bytes) + double Salary (8 bytes)
                WriteRecord(fs, 1, "John Doe", 75000.50);
                WriteRecord(fs, 2, "Jane Smith", 82000.00);
                WriteRecord(fs, 3, "Bob Johnson", 65000.75);
            }
            
            // Read all records
            using (FileStream fs = new FileStream(binaryFile, FileMode.Open, FileAccess.Read))
            {
                while (fs.Position < fs.Length)
                {
                    var record = ReadRecord(fs);
                    Console.WriteLine($"ID: {record.Id}, Name: {record.Name}, Salary: {record.Salary:F2}");
                    // Output: ID: 1, Name: John Doe, Salary: 75000.50
                    // etc.
                }
            }
        }
        
        private static void WriteRecord(FileStream fs, int id, string name, double salary)
        {
            fs.Write(BitConverter.GetBytes(id), 0, 4);
            
            byte[] nameBytes = new byte[30];
            byte[] actualName = Encoding.UTF8.GetBytes(name);
            Array.Copy(actualName, nameBytes, Math.Min(actualName.Length, 30));
            fs.Write(nameBytes, 0, 30);
            
            fs.Write(BitConverter.GetBytes(salary), 0, 8);
        }
        
        private static (int Id, string Name, double Salary) ReadRecord(FileStream fs)
        {
            byte[] idBytes = new byte[4];
            fs.Read(idBytes, 0, 4);
            int id = BitConverter.ToInt32(idBytes, 0);
            
            byte[] nameBytes = new byte[30];
            fs.Read(nameBytes, 0, 30);
            string name = Encoding.UTF8.GetString(nameBytes).Trim('\0');
            
            byte[] salaryBytes = new byte[8];
            fs.Read(salaryBytes, 0, 8);
            double salary = BitConverter.ToDouble(salaryBytes, 0);
            
            return (id, name, salary);
        }
        
        private static void SegmentProcessingDemo()
        {
            Console.WriteLine("\n--- File Segment Processing ---");
            
            // Create a large "file" (simulated with in-memory)
            string segmentFile = Path.Combine(BasePath, "segments.dat");
            byte[] largeData = new byte[1000];
            for (int i = 0; i < 1000; i++) largeData[i] = (byte)(i % 256);
            File.WriteAllBytes(segmentFile, largeData);
            
            const int SegmentSize = 100;
            int segmentNumber = 0;
            
            using (FileStream fs = new FileStream(segmentFile, FileMode.Open, FileAccess.Read))
            {
                byte[] segment = new byte[SegmentSize];
                int bytesRead;
                
                while ((bytesRead = fs.Read(segment, 0, SegmentSize)) > 0)
                {
                    // Process each segment
                    int sum = 0;
                    for (int i = 0; i < bytesRead; i++)
                    {
                        sum += segment[i];
                    }
                    
                    Console.WriteLine($"Segment {segmentNumber}: {bytesRead} bytes, checksum: {sum}");
                    // Output: Segment 0: 100 bytes, checksum: 4950
                    // ... (10 segments total)
                    
                    segmentNumber++;
                    
                    // For demo, only process first 3 segments
                    if (segmentNumber >= 3) break;
                }
            }
        }
    }
}