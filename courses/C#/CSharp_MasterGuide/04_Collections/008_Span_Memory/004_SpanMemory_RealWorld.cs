/*
 * TOPIC: Span<T> and Memory<T>
 * SUBTOPIC: Real-World Examples - Parsing Without Allocation, High Performance
 * FILE: SpanMemory_RealWorld.cs
 * PURPOSE: Demonstrate practical high-performance scenarios using Span<T>
 *          and Memory<T>: CSV parsing, binary protocol handling, 
 *          string manipulation, and memory-efficient data processing
 */
using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Runtime.InteropServices;
using System.Text;

namespace CSharp_MasterGuide._04_Collections._08_Span_Memory
{
    public class SpanMemory_RealWorld
    {
        public static void Main()
        {
            Console.WriteLine("=== Span<T> & Memory<T> Real-World Examples ===\n");

            CsvParsingExample();
            BinaryProtocolExample();
            StringBuildingExample();
            MessageParsingExample();
            ProtocolBufferExample();
            HighPerformanceSearch();
            StructuredLoggingExample();
        }

        static void CsvParsingExample()
        {
            Console.WriteLine("--- 1. High-Performance CSV Parsing ---");
            Console.WriteLine();

            // Parse large CSV without allocations using Span<T>
            string csvData = "ID,Name,Age,Salary,Department\n" +
                            "1,John Doe,30,75000,Engineering\n" +
                            "2,Jane Smith,28,65000,Marketing\n" +
                            "3,Bob Johnson,35,85000,Engineering\n" +
                            "4,Alice Williams,32,70000,Sales\n" +
                            "5,Charlie Brown,29,60000,Marketing";

            ReadOnlySpan<char> csvSpan = csvData;
            
            // Parse header
            int lineEnd = csvSpan.IndexOf('\n');
            ReadOnlySpan<char> header = csvSpan[..lineEnd];
            Console.WriteLine($"  Header: '{header.ToString().Trim()}'");
            // Output: Header: 'ID,Name,Age,Salary,Department'

            // Parse data rows without allocation
            var employees = ParseCsv(csvSpan.Slice(lineEnd + 1));

            Console.WriteLine($"  Parsed {employees.Count} employees:");
            foreach (var emp in employees)
            {
                Console.WriteLine($"    {emp}");
                // Output: 1 - John Doe (Age: 30, Salary: $75000, Dept: Engineering)
                // Output: 2 - Jane Smith (Age: 28, Salary: $65000, Dept: Marketing)
                // Output: 3 - Bob Johnson (Age: 35, Salary: $85000, Dept: Engineering)
                // Output: 4 - Alice Williams (Age: 32, Salary: $70000, Dept: Sales)
                // Output: 5 - Charlie Brown (Age: 29, Salary: $60000, Dept: Marketing)
            }
            Console.WriteLine();
        }

        static System.Collections.Generic.List<string> ParseCsv(ReadOnlySpan<char> data)
        {
            var results = new System.Collections.Generic.List<string>();
            int lineStart = 0;

            for (int i = 0; i <= data.Length; i++)
            {
                if (i == data.Length || data[i] == '\n')
                {
                    if (i > lineStart)
                    {
                        ReadOnlySpan<char> line = data.Slice(lineStart, i - lineStart);
                        var fields = SplitLine(line);
                        if (fields.Count >= 5)
                        {
                            results.Add($"{fields[0]} - {fields[1]} (Age: {fields[2]}, Salary: ${fields[3]}, Dept: {fields[4]})");
                        }
                    }
                    lineStart = i + 1;
                }
            }

            return results;
        }

        static System.Collections.Generic.List<string> SplitLine(ReadOnlySpan<char> line)
        {
            var fields = new System.Collections.Generic.List<string>();
            int start = 0;

            for (int i = 0; i <= line.Length; i++)
            {
                if (i == line.Length || line[i] == ',')
                {
                    fields.Add(line.Slice(start, i - start).ToString());
                    start = i + 1;
                }
            }

            return fields;
        }

        static void BinaryProtocolExample()
        {
            Console.WriteLine("--- 2. Binary Protocol Handling ---");
            Console.WriteLine();

            // Simulate parsing binary protocol (e.g., network packet)
            byte[] packet = new byte[16];

            // Packet format: 
            // [0-3]: Message Type (int32)
            // [4-7]: Sequence Number (int32)
            // [8-15]: Payload (8 bytes)

            Span<byte> packetSpan = packet;
            BinaryPrimitives.WriteInt32BigEndian(packetSpan, 1001);  // Message type
            BinaryPrimitives.WriteInt32BigEndian(packetSpan.Slice(4), 42);  // Sequence
            Encoding.ASCII.GetBytes("payload", 0, 7, packet, 8);  // Payload

            Console.WriteLine($"  Raw packet (hex): {BitConverter.ToString(packet)}");
            // Output: Raw packet (hex): 00-00-03-E9-00-00-00-2A-70-61-79-6C-6F-61-64-00

            // Parse using BinaryPrimitives
            ReadOnlySpan<byte> readOnlyPacket = packet;
            int messageType = BinaryPrimitives.ReadInt32BigEndian(readOnlyPacket);
            int seqNum = BinaryPrimitives.ReadInt32BigEndian(readOnlyPacket.Slice(4));
            ReadOnlySpan<byte> payload = readOnlyPacket.Slice(8, 8);

            Console.WriteLine($"  Message Type: {messageType}");
            // Output: Message Type: 1001
            Console.WriteLine($"  Sequence: {seqNum}");
            // Output: Sequence: 42
            Console.WriteLine($"  Payload: '{Encoding.ASCII.GetString(payload).TrimEnd('\0')}'");
            // Output: Payload: 'payload'

            // Write response
            byte[] response = new byte[16];
            Span<byte> responseSpan = response;
            BinaryPrimitives.WriteInt32BigEndian(responseSpan, 2001);  // Response type
            BinaryPrimitives.WriteInt32BigEndian(responseSpan.Slice(4), 42);  // Same sequence
            Encoding.ASCII.GetBytes("OK", 0, 2, response, 8);

            Console.WriteLine($"  Response: {BitConverter.ToString(response)}");
            // Output: Response: 00-00-07-D1-00-00-00-2A-4F-4B-00-00-00-00-00-00
            Console.WriteLine();
        }

        static void StringBuildingExample()
        {
            Console.WriteLine("--- 3. Zero-Allocation String Building ---");
            Console.WriteLine();

            // Using Span<T> to build strings without multiple allocations
            Span<char> buffer = stackalloc char[256];

            // Build formatted output
            int written = FormatEmployeeRecord(buffer, "John Smith", 35, 85000, "Engineering");
            string result = buffer.Slice(0, written).ToString();
            Console.WriteLine($"  Formatted: '{result}'");
            // Output: Formatted: 'Employee: John Smith | Age: 35 | Salary: $85,000 | Dept: Engineering'

            // Build multiple records
            var employees = new[]
            {
                ("Alice", 28, 65000, "Marketing"),
                ("Bob", 42, 95000, "Sales"),
                ("Charlie", 31, 72000, "Engineering")
            };

            Span<char> largeBuffer = stackalloc char[500];
            int totalWritten = 0;

            foreach (var (name, age, salary, dept) in employees)
            {
                int written2 = FormatEmployeeRecord(largeBuffer.Slice(totalWritten), name, age, salary, dept);
                totalWritten += written2;
                largeBuffer[totalWritten++] = '\n';
            }

            Console.WriteLine($"  Multiple records:\n{largeBuffer.Slice(0, totalWritten - 1).ToString()}");
            // Output: Multiple records:
            // Employee: Alice | Age: 28 | Salary: $65,000 | Dept: Marketing
            // Employee: Bob | Age: 42 | Salary: $95,000 | Dept: Sales
            // Employee: Charlie | Age: 31 | Salary: $72,000 | Dept: Engineering
            Console.WriteLine();
        }

        static int FormatEmployeeRecord(Span<char> buffer, string name, int age, int salary, string dept)
        {
            int pos = 0;

            // "Employee: "
            "Employee: ".AsSpan().CopyTo(buffer);
            pos += 9;

            // Name
            name.AsSpan().CopyTo(buffer.Slice(pos));
            pos += name.Length;

            buffer[pos++] = ' ';
            buffer[pos++] = '|';
            buffer[pos++] = ' ';

            // "Age: "
            "Age: ".AsSpan().CopyTo(buffer.Slice(pos));
            pos += 4;

            // Age as string
            var ageStr = age.ToString();
            ageStr.AsSpan().CopyTo(buffer.Slice(pos));
            pos += ageStr.Length;

            buffer[pos++] = ' ';
            buffer[pos++] = '|';
            buffer[pos++] = ' ';

            // "Salary: $"
            "Salary: $".AsSpan().CopyTo(buffer.Slice(pos));
            pos += 9;

            // Salary formatted
            var salaryStr = salary.ToString("N0");
            salaryStr.AsSpan().CopyTo(buffer.Slice(pos));
            pos += salaryStr.Length;

            buffer[pos++] = ' ';
            buffer[pos++] = '|';
            buffer[pos++] = ' ';

            // "Dept: "
            "Dept: ".AsSpan().CopyTo(buffer.Slice(pos));
            pos += 5;

            // Department
            dept.AsSpan().CopyTo(buffer.Slice(pos));
            pos += dept.Length;

            return pos;
        }

        static void MessageParsingExample()
        {
            Console.WriteLine("--- 4. Network Message Parsing ---");
            Console.WriteLine();

            // Simulate HTTP-like request parsing
            string httpRequest = 
                "GET /api/users/123 HTTP/1.1\r\n" +
                "Host: api.example.com\r\n" +
                "Content-Type: application/json\r\n" +
                "Authorization: Bearer token123\r\n" +
                "\r\n" +
                "{\"name\":\"test\"}";

            ParseHttpRequest(httpRequest);
            Console.WriteLine();
        }

        static void ParseHttpRequest(string request)
        {
            ReadOnlySpan<char> requestSpan = request;

            // Find request line
            int lineEnd = requestSpan.IndexOf("\r\n");
            ReadOnlySpan<char> requestLine = requestSpan[..lineEnd];
            
            // Parse method, path, version
            int firstSpace = requestLine.IndexOf(' ');
            ReadOnlySpan<char> afterFirst = requestLine.Slice(firstSpace + 1);
            int secondSpace = afterFirst.IndexOf(' ');
            secondSpace += firstSpace + 1;

            ReadOnlySpan<char> method = requestLine[..firstSpace];
            ReadOnlySpan<char> path = requestLine.Slice(firstSpace + 1, secondSpace - firstSpace - 1);
            ReadOnlySpan<char> version = requestLine.Slice(secondSpace + 1);

            Console.WriteLine($"  Method: {method.ToString()}");
            // Output: Method: GET
            Console.WriteLine($"  Path: {path.ToString()}");
            // Output: Path: /api/users/123
            Console.WriteLine($"  Version: {version.ToString()}");
            // Output: Version: HTTP/1.1

            // Parse headers
            ReadOnlySpan<char> headers = requestSpan.Slice(lineEnd + 2);
            int headerEnd = headers.IndexOf("\r\n\r\n");
            ReadOnlySpan<char> headerSection = headers[..(headerEnd + 2)];

            int headerLineStart = 0;
            Console.WriteLine("  Headers:");
            for (int i = 0; i <= headerSection.Length; i++)
            {
                if (i == headerSection.Length || (i > 0 && headerSection[i - 1] == '\n')
                    || (i > 1 && i < headerSection.Length && headerSection[i] == '\r' && headerSection[i - 1] != '\n'))
                {
                    if (headerSection[i - 2] == '\r' && headerSection[i - 1] == '\n')
                    {
                        ReadOnlySpan<char> headerLine = headerSection.Slice(headerLineStart, i - headerLineStart - 2);
                        int colon = headerLine.IndexOf(':');
                        if (colon > 0)
                        {
                            ReadOnlySpan<char> name = headerLine[..colon];
                            ReadOnlySpan<char> value = headerLine.Slice(colon + 2);
                            Console.WriteLine($"    {name.ToString()}: {value.ToString()}");
                            // Output: Host: api.example.com
                            // Output: Content-Type: application/json
                            // Output: Authorization: Bearer token123
                        }
                        headerLineStart = i + 1;
                    }
                }
            }
        }

        static void ProtocolBufferExample()
        {
            Console.WriteLine("--- 5. Protocol Buffer Style Parsing ---");
            Console.WriteLine();

            // Simulate reading protobuf-like data
            // Format: [tag][length][value] repeating
            byte[] protoData = new byte[]
            {
                0x08, 0x01,        // Tag 1: int32 = 1
                0x12, 0x05, 0x48, 0x65, 0x6C, 0x6C, 0x6F,  // Tag 2: string "Hello"
                0x1A, 0x04, 0x57, 0x6F, 0x72, 0x6C,       // Tag 3: string "World"
                0x20, 0x64,        // Tag 4: int32 = 100
            };

            ReadOnlySpan<byte> data = protoData;
            int pos = 0;

            while (pos < data.Length)
            {
                int tag = data[pos++];
                int fieldNumber = tag >> 3;
                int wireType = tag & 0x7;

                switch (wireType)
                {
                    case 0: // Varint
                        long varintValue = ReadVarint(data, ref pos);
                        Console.WriteLine($"  Field {fieldNumber}: varint = {varintValue}");
                        // Output: Field 1: varint = 1
                        // Output: Field 4: varint = 100
                        break;

                    case 2: // Length-delimited
                        int length = (int)ReadVarint(data, ref pos);
                        ReadOnlySpan<byte> stringValue = data.Slice(pos, length);
                        Console.WriteLine($"  Field {fieldNumber}: string = '{Encoding.UTF8.GetString(stringValue)}'");
                        // Output: Field 2: string = Hello
                        // Output: Field 3: string = World
                        pos += length;
                        break;
                }
            }
            Console.WriteLine();
        }

        static long ReadVarint(ReadOnlySpan<byte> data, ref int pos)
        {
            long result = 0;
            int shift = 0;

            while (pos < data.Length)
            {
                byte b = data[pos++];
                result |= (b & 0x7F) << shift;
                if ((b & 0x80) == 0) break;
                shift += 7;
            }

            return result;
        }

        static void HighPerformanceSearch()
        {
            Console.WriteLine("--- 6. High-Performance Search ---");
            Console.WriteLine();

            // Search in large data without allocation
            string largeText = new string('A', 10000) + "FINDME" + new string('A', 10000);
            ReadOnlySpan<char> text = largeText;

            // Find substring
            int index = text.IndexOf("FINDME");
            Console.WriteLine($"  Found 'FINDME' at index: {index}");
            // Output: Found 'FINDME' at index: 10000

            // Find multiple occurrences
            string repeatedPattern = "ABCABCABCABCABC";
            ReadOnlySpan<char> pattern = repeatedPattern;

            int count = 0;
            int searchPos = 0;
            while (true)
            {
                int found = pattern.Slice(searchPos).IndexOf("ABC");
                if (found < 0) break;
                count++;
                searchPos += found + 3;
            }

            Console.WriteLine($"  'ABC' appears {count} times");
            // Output: 'ABC' appears 5 times

            // Binary search
            int[] sorted = { 1, 3, 5, 7, 9, 11, 13, 15, 17, 19 };
            Span<int> sortedSpan = sorted;

            int binaryIndex = sortedSpan.BinarySearch(11);
            Console.WriteLine($"  Binary search for 11: index {binaryIndex}");
            // Output: Binary search for 11: index 5

            // Custom search with span
            int[] toFind = { 7, 9, 11 };
            int customIndex = sortedSpan.IndexOf(toFind);
            Console.WriteLine($"  Index of [7,9,11]: {customIndex}");
            // Output: Index of [7,9,11]: 3
            Console.WriteLine();
        }

        static void StructuredLoggingExample()
        {
            Console.WriteLine("--- 7. Structured Logging with Span ---");
            Console.WriteLine();

            // Build structured log entries efficiently
            var logBuilder = new SpanLogBuilder(256);
            
            logBuilder.AppendLog("INFO", "UserLogin", "user_id", 12345);
            logBuilder.AppendLog("ERROR", "PaymentFailed", "error_code", "INSUFFICIENT_FUNDS");
            logBuilder.AppendLog("WARN", "RateLimit", "remaining", 5);

            Console.WriteLine($"  Log entries:\n{logBuilder.GetLog()}");
            // Output: Log entries:
            // 2024-01-15 10:30:45.123 [INFO] UserLogin - user_id:12345
            // 2024-01-15 10:30:45.124 [ERROR] PaymentFailed - error_code:INSUFFICIENT_FUNDS
            // 2024-01-15 10:30:45.125 [WARN] RateLimit - remaining:5

            Console.WriteLine();
        }
    }

    class SpanLogBuilder
    {
        private readonly char[] _buffer;
        private int _position;

        public SpanLogBuilder(int size)
        {
            _buffer = new char[size];
            _position = 0;
        }

        public void AppendLog(string level, string eventName, string key, object value)
        {
            var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            Span<char> span = _buffer;

            // Timestamp
            timestamp.AsSpan().CopyTo(span.Slice(_position));
            _position += timestamp.Length;

            span[_position++] = ' ';
            span[_position++] = '[';

            // Level
            level.AsSpan().CopyTo(span.Slice(_position));
            _position += level.Length;

            span[_position++] = ']';
            span[_position++] = ' ';

            // Event name
            eventName.AsSpan().CopyTo(span.Slice(_position));
            _position += eventName.Length;

            span[_position++] = ' ';
            span[_position++] = '-';
            span[_position++] = ' ';

            // Key
            key.AsSpan().CopyTo(span.Slice(_position));
            _position += key.Length;

            span[_position++] = ':';

            // Value
            var valueStr = value.ToString();
            valueStr.AsSpan().CopyTo(span.Slice(_position));
            _position += valueStr.Length;

            span[_position++] = '\n';
        }

        public string GetLog()
        {
            return new string(_buffer, 0, _position);
        }
    }
}
