/*
 * TOPIC: Span<T> and Memory<T>
 * SUBTOPIC: ReadOnlySpan<T>, Parsing, Substring Without Allocation
 * FILE: SpanBasics_Part2.cs
 * PURPOSE: Demonstrate ReadOnlySpan<T>, parsing operations,
 *          and substring extraction without heap allocation
 */
using System;
using System.Text;

namespace CSharp_MasterGuide._04_Collections._08_Span_Memory
{
    public class SpanBasics_Part2
    {
        public static void Main()
        {
            Console.WriteLine("=== Span<T> Part 2 - ReadOnlySpan & Parsing ===\n");

            ReadOnlySpanBasics();
            StringParsingWithSpan();
            SubstringWithoutAllocation();
            ParsingNumbers();
            SpanExtensions();
            RealWorldParsingExample();
        }

        static void ReadOnlySpanBasics()
        {
            Console.WriteLine("--- 1. ReadOnlySpan<T> Basics ---");
            Console.WriteLine();

            // ReadOnlySpan<T> - read-only view into memory
            // Can be created from arrays, strings, or spans

            int[] numbers = { 10, 20, 30, 40, 50 };
            ReadOnlySpan<int> readOnlyNumbers = numbers;

            Console.WriteLine($"  ReadOnlySpan: [{string.Join(", ", readOnlyNumbers.ToArray())}]");
            // Output: ReadOnlySpan: [10, 20, 30, 40, 50]

            // All read-only operations work
            Console.WriteLine($"  Length: {readOnlyNumbers.Length}");
            // Output: Length: 5

            Console.WriteLine($"  First [0]: {readOnlyNumbers[0]}");
            // Output: First [0]: 10

            Console.WriteLine($"  Last [^1]: {readOnlyNumbers[^1]}");
            // Output: Last [^1]: 50

            // Contains and IndexOf
            bool has30 = readOnlyNumbers.Contains(30);
            Console.WriteLine($"  Contains 30: {has30}");
            // Output: Contains 30: True

            int index = readOnlyNumbers.IndexOf(40);
            Console.WriteLine($"  IndexOf 40: {index}");
            // Output: IndexOf 40: 3

            // Slice - returns ReadOnlySpan
            ReadOnlySpan<int> middle = readOnlyNumbers.Slice(1, 3);
            Console.WriteLine($"  Sliced [1..3]: [{string.Join(", ", middle.ToArray())}]");
            // Output: Sliced [1..3]: [20, 30, 40]

            // ReadOnlySpan from string (char array underneath)
            string text = "Hello, World!";
            ReadOnlySpan<char> textSpan = text;
            Console.WriteLine($"  Text span: '{textSpan.ToString()}'");
            // Output: Text span: 'Hello, World!'

            // Access characters
            Console.WriteLine($"  textSpan[0]: '{textSpan[0]}'");
            // Output: textSpan[0]: 'H'

            // Use ^ operator
            Console.WriteLine($"  textSpan[^1]: '{textSpan[^1]}'");
            // Output: textSpan[^1]: '!'

            // Slice for substring without allocation
            ReadOnlySpan<char> hello = textSpan[..5];
            Console.WriteLine($"  hello: '{hello.ToString()}'");
            // Output: hello: 'Hello'

            ReadOnlySpan<char> world = textSpan[7..12];
            Console.WriteLine($"  world: '{world.ToString()}'");
            // Output: world: 'World'
            Console.WriteLine();
        }

        static void StringParsingWithSpan()
        {
            Console.WriteLine("--- 2. String Parsing with Span<T> ---");
            Console.WriteLine();

            // Parse integers without creating substring
            string numberString = "12345";
            ReadOnlySpan<char> numberSpan = numberString;

            int parsedInt = int.Parse(numberSpan);
            Console.WriteLine($"  Parsed int: {parsedInt}");
            // Output: Parsed int: 12345

            // TryParse for safe parsing
            string invalidNumber = "abc";
            ReadOnlySpan<char> invalidSpan = invalidNumber;

            if (int.TryParse(invalidSpan, out int result))
            {
                Console.WriteLine($"  Parsed: {result}");
            }
            else
            {
                Console.WriteLine($"  '{invalidSpan.ToString()}' is not a valid number");
                // Output: 'abc' is not a valid number
            }

            // Parse different number types
            long parsedLong = long.Parse("9876543210");
            Console.WriteLine($"  Parsed long: {parsedLong}");
            // Output: Parsed long: 9876543210

            double parsedDouble = double.Parse("3.14159");
            Console.WriteLine($"  Parsed double: {parsedDouble}");
            // Output: Parsed double: 3.14159

            decimal parsedDecimal = decimal.Parse("999.99");
            Console.WriteLine($"  Parsed decimal: {parsedDecimal}");
            // Output: Parsed decimal: 999.99

            // Parse from byte array
            byte[] byteData = { (byte)'1', (byte)'2', (byte)'3' };
            ReadOnlySpan<byte> byteSpan = byteData;
            // Output: Parsed from bytes: 123

            // Use Encoding for byte to string conversion
            string byteString = Encoding.ASCII.GetString(byteData);
            int fromBytes = int.Parse(byteString);
            Console.WriteLine($"  Parsed from bytes: {fromBytes}");
            Console.WriteLine();
        }

        static void SubstringWithoutAllocation()
        {
            Console.WriteLine("--- 3. Substring Without Allocation ---");
            Console.WriteLine();

            // Traditional approach - creates new string (allocation)
            string original = "The quick brown fox jumps over the lazy dog";
            string traditional = original.Substring(4, 15);
            Console.WriteLine($"  Traditional Substring: '{traditional}'");
            // Output: Traditional Substring: 'quick brown fox'

            // Span approach - no allocation, view into original
            ReadOnlySpan<char> span = original;
            ReadOnlySpan<char> noAllocation = span.Slice(4, 15);
            Console.WriteLine($"  Span slice (no allocation): '{noAllocation.ToString()}'");
            // Output: Span slice (no allocation): 'quick brown fox'

            // Memory usage comparison
            // Traditional: allocates new string ~15 chars + overhead
            // Span: zero allocation, just references original memory

            // Using range operator (C# 8+)
            ReadOnlySpan<char> fox = original[16..19];
            Console.WriteLine($"  Range [16..19]: '{fox.ToString()}'");
            // Output: Range [16..19]: 'fox'

            // Extract from end
            ReadOnlySpan<char> dog = original[^12..];
            Console.WriteLine($"  From end [^12..]: '{dog.ToString()}'");
            // Output: From end [^12..]: 'lazy dog'

            // Find position then slice
            int dogIndex = span.IndexOf("dog");
            if (dogIndex >= 0)
            {
                ReadOnlySpan<char> found = span.Slice(dogIndex, 3);
                Console.WriteLine($"  Found 'dog' at {dogIndex}: '{found.ToString()}'");
                // Output: Found 'dog' at 40: 'dog'
            }

            // Multiple slices without allocation
            string data = "Name:John|Age:30|Email:john@test.com";
            ReadOnlySpan<char> dataSpan = data;

            int pipe1 = dataSpan.IndexOf('|');
            int pipe2 = dataSpan.Slice(pipe1 + 1).IndexOf('|');
            pipe2 += pipe1 + 1;

            ReadOnlySpan<char> name = dataSpan.Slice(5, pipe1 - 5);
            ReadOnlySpan<char> age = dataSpan.Slice(pipe1 + 4, pipe2 - pipe1 - 4);
            ReadOnlySpan<char> email = dataSpan.Slice(pipe2 + 6);

            Console.WriteLine($"  Name: '{name.ToString()}'");
            // Output: Name: 'John'
            Console.WriteLine($"  Age: '{age.ToString()}'");
            // Output: Age: '30'
            Console.WriteLine($"  Email: '{email.ToString()}'");
            // Output: Email: 'john@test.com'
            Console.WriteLine();
        }

        static void ParsingNumbers()
        {
            Console.WriteLine("--- 4. Number Parsing with Span ---");
            Console.WriteLine();

            // Parse hex string to integer
            string hexValue = "FF";
            int hexParsed = Convert.ToInt32(hexValue, 16);
            Console.WriteLine($"  Hex 'FF' to int: {hexParsed}");
            // Output: Hex 'FF' to int: 255

            // Parse binary string
            string binaryValue = "1010";
            int binaryParsed = Convert.ToInt32(binaryValue, 2);
            Console.WriteLine($"  Binary '1010' to int: {binaryParsed}");
            // Output: Binary '1010' to int: 10

            // Custom parsing without allocation - parse delimited numbers
            string numberList = "1,2,3,4,5,6,7,8,9,10";
            ReadOnlySpan<char> listSpan = numberList;

            var numbers = new System.Collections.Generic.List<int>();
            int start = 0;

            for (int i = 0; i <= listSpan.Length; i++)
            {
                if (i == listSpan.Length || listSpan[i] == ',')
                {
                    if (i > start)
                    {
                        ReadOnlySpan<char> numSlice = listSpan.Slice(start, i - start);
                        if (int.TryParse(numSlice, out int num))
                        {
                            numbers.Add(num);
                        }
                    }
                    start = i + 1;
                }
            }

            Console.WriteLine($"  Parsed numbers: [{string.Join(", ", numbers)}]");
            // Output: Parsed numbers: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]

            // Sum without allocation
            int sum = 0;
            foreach (var n in numbers)
            {
                sum += n;
            }
            Console.WriteLine($"  Sum: {sum}");
            // Output: Sum: 55

            // Parse floats from string
            string floatList = "1.1,2.2,3.3";
            ReadOnlySpan<char> floatSpan = floatList;

            var floats = new System.Collections.Generic.List<float>();
            start = 0;

            for (int i = 0; i <= floatSpan.Length; i++)
            {
                if (i == floatSpan.Length || floatSpan[i] == ',')
                {
                    if (i > start)
                    {
                        ReadOnlySpan<char> floatSlice = floatSpan.Slice(start, i - start);
                        if (float.TryParse(floatSlice, out float f))
                        {
                            floats.Add(f);
                        }
                    }
                    start = i + 1;
                }
            }

            Console.WriteLine($"  Parsed floats: [{string.Join(", ", floats)}]");
            // Output: Parsed floats: [1.1, 2.2, 3.3]
            Console.WriteLine();
        }

        static void SpanExtensions()
        {
            Console.WriteLine("--- 5. Span Extension Methods ---");
            Console.WriteLine();

            // Create span from stack memory
            Span<char> buffer = stackalloc char[100];
            int written = 0;

            // Format using span (without allocation)
            var item = ("Product", 99.99, 5);
            written += FormatToSpan(buffer, item.Item1, item.Item2, item.Item3);

            string result = buffer.Slice(0, written).ToString();
            Console.WriteLine($"  Formatted: '{result}'");
            // Output: Formatted: 'Product - $99.99 x 5 = $499.95'

            // Using BinaryPrimitives for endian-aware parsing
            byte[] littleEndian = BitConverter.GetBytes(123456);
            if (BitConverter.IsLittleEndian)
            {
                int parsed = System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(littleEndian);
                Console.WriteLine($"  Little endian read: {parsed}");
                // Output: Little endian read: 123456
            }

            // Write with binary primitives
            byte[] writeBuffer = new byte[4];
            System.Buffers.Binary.BinaryPrimitives.WriteInt32BigEndian(writeBuffer, 123456);
            Console.WriteLine($"  Big endian written: {BitConverter.ToString(writeBuffer)}");
            // Output: Big endian written: 00-01-E2-40
            Console.WriteLine();
        }

        static int FormatToSpan(Span<char> buffer, string name, double price, int quantity)
        {
            int pos = 0;

            // Copy name
            foreach (char c in name)
            {
                buffer[pos++] = c;
            }

            buffer[pos++] = ' ';
            buffer[pos++] = '-';
            buffer[pos++] = ' ';

            buffer[pos++] = '$';

            // Format price (simple implementation)
            var priceStr = price.ToString("0.00");
            foreach (char c in priceStr)
            {
                buffer[pos++] = c;
            }

            buffer[pos++] = ' ';
            buffer[pos++] = 'x';
            buffer[pos++] = ' ';

            // Format quantity
            var qtyStr = quantity.ToString();
            foreach (char c in qtyStr)
            {
                buffer[pos++] = c;
            }

            buffer[pos++] = ' ';
            buffer[pos++] = '=';
            buffer[pos++] = ' ';

            buffer[pos++] = '$';

            double total = price * quantity;
            var totalStr = total.ToString("0.00");
            foreach (char c in totalStr)
            {
                buffer[pos++] = c;
            }

            return pos;
        }

        static void RealWorldParsingExample()
        {
            Console.WriteLine("--- Real-World: Log File Parsing ---");
            Console.WriteLine();

            // Simulate log entries
            string logData = @"2024-01-15 10:30:45 INFO User login successful
2024-01-15 10:31:22 ERROR Database connection failed
2024-01-15 10:32:05 WARN Retry attempt 1 for database
2024-01-15 10:32:30 INFO User logout";

            ReadOnlySpan<char> logSpan = logData;
            int lineStart = 0;

            Console.WriteLine("  Parsing log entries:");
            for (int i = 0; i <= logSpan.Length; i++)
            {
                if (i == logSpan.Length || (i > 0 && logSpan[i - 1] == '\n'))
                {
                    if (i > lineStart)
                    {
                        ReadOnlySpan<char> line = logSpan.Slice(lineStart, i - lineStart);
                        ParseLogEntry(line);
                    }
                    lineStart = i + 1;
                }
            }
            Console.WriteLine();
        }

        static void ParseLogEntry(ReadOnlySpan<char> line)
        {
            // Format: 2024-01-15 10:30:45 INFO message
            // Find timestamp end (first space after date)
            int spaceIndex = line.IndexOf(' ');

            if (spaceIndex > 0)
            {
                ReadOnlySpan<char> timestamp = line.Slice(0, spaceIndex);
                ReadOnlySpan<char> rest = line.Slice(spaceIndex + 1);

                int secondSpace = rest.IndexOf(' ');
                ReadOnlySpan<char> level = rest.Slice(0, secondSpace);
                ReadOnlySpan<char> message = rest.Slice(secondSpace + 1);

                Console.WriteLine($"    [{timestamp.ToString()}] {level.ToString()}: {message.ToString()}");
                // Output: [2024-01-15 10:30:45] INFO: User login successful
                // Output: [2024-01-15 10:31:22] ERROR: Database connection failed
                // Output: [2024-01-15 10:32:05] WARN: Retry attempt 1 for database
                // Output: [2024-01-15 10:32:30] INFO: User logout
            }
        }
    }
}
