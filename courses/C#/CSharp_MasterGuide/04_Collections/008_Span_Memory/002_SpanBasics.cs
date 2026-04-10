/*
 * TOPIC: Span<T> and Memory<T>
 * SUBTOPIC: Span<T> Basics - Stack-Only, Slicing, Pointer-Like but Safe
 * FILE: SpanBasics.cs
 * PURPOSE: Demonstrate Span<T> fundamentals - stack-only allocation,
 *          slicing arrays without copying, and pointer-like performance with safety
 */
using System;

namespace CSharp_MasterGuide._04_Collections._08_Span_Memory
{
    public class SpanBasics
    {
        public static void Main()
        {
            Console.WriteLine("=== Span<T> Basics ===\n");

            CreatingSpan();
            SpanFromArray();
            SpanSlicing();
            SpanStackOnly();
            SpanElementAccess();
            SpanOperations();
            SpanRealWorldExample();
        }

        static void CreatingSpan()
        {
            Console.WriteLine("--- 1. Creating Span<T> ---");
            Console.WriteLine();

            // Span<T> from stack-allocated array
            int[] stackArray = new int[] { 1, 2, 3, 4, 5 };
            Span<int> spanFromArray = stackArray;
            Console.WriteLine($"  Span from array: [{string.Join(", ", spanFromArray.ToArray())}]");
            // Output: Span from array: [1, 2, 3, 4, 5]

            // Span<T> from portion of array using span constructor
            Span<int> partialSpan = new Span<int>(stackArray, 1, 3);
            Console.WriteLine($"  Partial span (index 1, length 3): [{string.Join(", ", partialSpan.ToArray())}]");
            // Output: Partial span (index 1, length 3): [2, 3, 4]

            // Span<T> from stack memory (stackalloc)
            Span<byte> stackMemory = stackalloc byte[5] { 10, 20, 30, 40, 50 };
            Console.WriteLine($"  Stack memory: [{string.Join(", ", stackMemory.ToArray())}]");
            // Output: Stack memory: [10, 20, 30, 40, 50]

            // Create span from span (slicing)
            Span<int> firstThree = spanFromArray.Slice(0, 3);
            Console.WriteLine($"  Sliced span: [{string.Join(", ", firstThree.ToArray())}]");
            // Output: Sliced span: [1, 2, 3]
            Console.WriteLine();
        }

        static void SpanFromArray()
        {
            Console.WriteLine("--- 2. Span<T> from Arrays ---");
            Console.WriteLine();

            // Direct conversion from array to Span<T>
            string[] words = { "Hello", "World", "CSharp" };
            Span<string> wordSpan = words;
            Console.WriteLine($"  Words span: [{string.Join(", ", wordSpan.ToArray())}]");
            // Output: Words span: [Hello, World, CSharp]

            // Access span elements (same as array)
            Console.WriteLine($"  First element: {wordSpan[0]}");
            // Output: First element: Hello

            // Length property
            Console.WriteLine($"  Span length: {wordSpan.Length}");
            // Output: Span length: 3

            // IsEmpty property
            Span<int> emptySpan = Span<int>.Empty;
            Console.WriteLine($"  IsEmpty: {emptySpan.IsEmpty}");
            // Output: IsEmpty: True
            Console.WriteLine();
        }

        static void SpanSlicing()
        {
            Console.WriteLine("--- 3. Span<T> Slicing (No Allocation) ---");
            Console.WriteLine();

            int[] data = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            Span<int> fullSpan = data;

            // Slice from start
            Span<int> firstHalf = fullSpan.Slice(0, 5);
            Console.WriteLine($"  First half (0-5): [{string.Join(", ", firstHalf.ToArray())}]");
            // Output: First half (0-5): [0, 1, 2, 3, 4]

            // Slice from middle
            Span<int> secondHalf = fullSpan.Slice(5);
            Console.WriteLine($"  Second half (5+): [{string.Join(", ", secondHalf.ToArray())}]");
            // Output: Second half (5+): [5, 6, 7, 8, 9]

            // Slice with range (C# 8+)
            Span<int> middle = fullSpan[3..7];
            Console.WriteLine($"  Middle [3..7]: [{string.Join(", ", middle.ToArray())}]");
            // Output: Middle [3..7]: [3, 4, 5, 6]

            // Negative from end (C# 8+)
            Span<int> lastThree = fullSpan[^3..];
            Console.WriteLine($"  Last three [^3..]: [{string.Join(", ", lastThree.ToArray())}]");
            // Output: Last three [^3..]: [7, 8, 9]

            // Slicing character span for substrings
            string message = "Hello, World!";
            ReadOnlySpan<char> charSpan = message;
            ReadOnlySpan<char> hello = charSpan[..5];
            Console.WriteLine($"  Substring 'Hello': '{hello.ToString()}'");
            // Output: Substring 'Hello': 'Hello'

            ReadOnlySpan<char> world = charSpan[7..12];
            Console.WriteLine($"  Substring 'World': '{world.ToString()}'");
            // Output: Substring 'World': 'World'
            Console.WriteLine();
        }

        static void SpanStackOnly()
        {
            Console.WriteLine("--- 4. Span<T> is Stack-Only ---");
            Console.WriteLine();

            // Span<T> CANNOT be stored in a class field (stack-only)
            // This is by design for memory safety and performance

            // Demonstrating stack-only nature with stackalloc
            Span<int> stackSpan = stackalloc int[5] { 1, 2, 3, 4, 5 };
            Console.WriteLine($"  Stack-allocated span: [{string.Join(", ", stackSpan.ToArray())}]");
            // Output: Stack-allocated span: [1, 2, 3, 4, 5]

            // Passing span to methods - efficient, no copying
            ProcessSpan(stackSpan);
            Console.WriteLine($"  After processing: [{string.Join(", ", stackSpan.ToArray())}]");
            // Output: After processing: [2, 4, 6, 8, 10]

            // Span<T> works with ref structs - cannot be boxed
            // This ensures it stays on stack, preventing heap allocation
            Console.WriteLine("  Span<T> is a ref struct - always stack-allocated");
            // Output: Span<T> is a ref struct - always stack-allocated
            Console.WriteLine();
        }

        static void ProcessSpan(Span<int> data)
        {
            // Modify span in place - no allocation
            for (int i = 0; i < data.Length; i++)
            {
                data[i] *= 2;
            }
        }

        static void SpanElementAccess()
        {
            Console.WriteLine("--- 5. Span<T> Element Access ---");
            Console.WriteLine();

            Span<char> letters = stackalloc char[5] { 'A', 'B', 'C', 'D', 'E' };

            // Indexer access
            Console.WriteLine($"  letters[0]: {letters[0]}");
            // Output: letters[0]: A
            Console.WriteLine($"  letters[^1]: {letters[^1]}");
            // Output: letters[^1]: E

            // IndexOf - search within span
            Span<int> numbers = stackalloc int[] { 10, 20, 30, 40, 50 };
            int indexOf30 = numbers.IndexOf(30);
            Console.WriteLine($"  Index of 30: {indexOf30}");
            // Output: Index of 30: 2

            // Contains
            bool has40 = numbers.Contains(40);
            Console.WriteLine($"  Contains 40: {has40}");
            // Output: Contains 40: True

            // First element access via indexer
            Console.WriteLine($"  First [0]: {numbers[0]}");
            // Output: First [0]: 10
            Console.WriteLine($"  Last [^1]: {numbers[^1]}");
            // Output: Last [^1]: 50
            Console.WriteLine();
        }

        static void SpanOperations()
        {
            Console.WriteLine("--- 6. Span<T> Operations ---");
            Console.WriteLine();

            // Fill entire span with value
            Span<int> values = stackalloc int[5];
            values.Fill(7);
            Console.WriteLine($"  After Fill(7): [{string.Join(", ", values.ToArray())}]");
            // Output: After Fill(7): [7, 7, 7, 7, 7]

            // Copy span to another span
            Span<int> source = stackalloc int[] { 1, 2, 3 };
            Span<int> destination = stackalloc int[3];
            source.CopyTo(destination);
            Console.WriteLine($"  Copied: [{string.Join(", ", destination.ToArray())}]");
            // Output: Copied: [1, 2, 3]

            // TryCopy - returns false if destination too small
            Span<int> smallDest = stackalloc int[2];
            bool copied = source.TryCopyTo(smallDest);
            Console.WriteLine($"  TryCopy to small: {copied}");
            // Output: TryCopy to small: False

            // Clear span
            Span<int> toClear = stackalloc int[] { 1, 2, 3 };
            toClear.Clear();
            Console.WriteLine($"  After Clear: [{string.Join(", ", toClear.ToArray())}]");
            // Output: After Clear: [0, 0, 0]

            // ToArray - converts to heap-allocated array (use carefully)
            Span<int> span = stackalloc int[] { 1, 2, 3 };
            int[] array = span.ToArray();
            Console.WriteLine($"  ToArray: [{string.Join(", ", array)}]");
            // Output: ToArray: [1, 2, 3]
            Console.WriteLine();
        }

        static void SpanRealWorldExample()
        {
            Console.WriteLine("--- Real-World: Fast Data Processing ---");
            Console.WriteLine();

            // Parse CSV data without allocations using Span<T>
            string csvData = "John,25,Engineer Jane,30,Developer Bob,35,Manager";
            ReadOnlySpan<char> dataSpan = csvData;

            Console.WriteLine($"  CSV data: {csvData}");
            // Output: CSV data: John,25,Engineer Jane,30,Developer Bob,35,Manager

            // Split by comma using span slicing
            int start = 0;
            int fieldIndex = 0;
            string[] fields = new string[6];

            for (int i = 0; i <= dataSpan.Length; i++)
            {
                if (i == dataSpan.Length || dataSpan[i] == ',')
                {
                    fields[fieldIndex] = dataSpan.Slice(start, i - start).ToString();
                    fieldIndex++;
                    start = i + 1;
                }
            }

            Console.WriteLine($"  Parsed fields:");
            for (int i = 0; i < fields.Length; i += 3)
            {
                Console.WriteLine($"    Name: {fields[i]}, Age: {fields[i + 1]}, Role: {fields[i + 2]}");
                // Output: Name: John, Age: 25, Role: Engineer
                // Output: Name: Jane, Age: 30, Role: Developer
                // Output: Name: Bob, Age: 35, Role: Manager
            }

            // Binary parsing example - parse integers from byte array
            byte[] bytes = BitConverter.GetBytes(12345);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }

            Span<byte> byteSpan = bytes;
            int parsedValue = BitConverter.ToInt32(byteSpan);
            Console.WriteLine($"  Parsed int from bytes: {parsedValue}");
            // Output: Parsed int from bytes: 12345

            // Reverse bytes in place
            byteSpan.Reverse();
            int reversedValue = BitConverter.ToInt32(byteSpan);
            Console.WriteLine($"  After reverse: {reversedValue}");
            // Output: After reverse: -1241505792
            Console.WriteLine();
        }
    }
}
