/*
 * TOPIC: Span<T> and Memory<T>
 * SUBTOPIC: Memory<T> and ReadOnlyMemory<T> - For Async, Can Escape Stack
 * FILE: MemoryBasics.cs
 * PURPOSE: Demonstrate Memory<T> and ReadOnlyMemory<T> which can be stored
 *          in classes and used for async operations, unlike Span<T>
 */
using System;
using System.Buffers;
using System.Threading.Tasks;

namespace CSharp_MasterGuide._04_Collections._08_Span_Memory
{
    public class MemoryBasics
    {
        public static void Main()
        {
            Console.WriteLine("=== Memory<T> and ReadOnlyMemory<T> Basics ===\n");

            CreatingMemory();
            MemoryFromArray();
            MemorySlicing();
            MemoryVsSpan();
            MemoryInClasses();
            AsyncWithMemory();
            RealWorldExample();
        }

        static void CreatingMemory()
        {
            Console.WriteLine("--- 1. Creating Memory<T> ---");
            Console.WriteLine();

            // Memory<T> from array
            int[] numbers = { 1, 2, 3, 4, 5 };
            Memory<int> memoryFromArray = numbers;
            Console.WriteLine($"  Memory from array: [{string.Join(", ", memoryFromArray.ToArray())}]");
            // Output: Memory from array: [1, 2, 3, 4, 5]

            // Memory<T> from portion of array
            Memory<int> partialMemory = new Memory<int>(numbers, 1, 3);
            Console.WriteLine($"  Partial memory: [{string.Join(", ", partialMemory.ToArray())}]");
            // Output: Partial memory: [2, 3, 4]

            // Create empty Memory<T>
            Memory<int> emptyMemory = Memory<int>.Empty;
            Console.WriteLine($"  IsEmpty: {emptyMemory.IsEmpty}");
            // Output: IsEmpty: True

            // Memory<T> from stack (via Span) - needs array intermediate
            Span<int> stackSpan = stackalloc int[3] { 10, 20, 30 };
            int[] heapArray = stackSpan.ToArray();
            Memory<int> fromStack = heapArray;
            Console.WriteLine($"  From stack: [{string.Join(", ", fromStack.ToArray())}]");
            // Output: From stack: [10, 20, 30]

            // Memory<T> length
            Console.WriteLine($"  Length: {memoryFromArray.Length}");
            // Output: Length: 5
            Console.WriteLine();
        }

        static void MemoryFromArray()
        {
            Console.WriteLine("--- 2. Memory<T> from Arrays ---");
            Console.WriteLine();

            string[] words = { "Apple", "Banana", "Cherry", "Date" };
            Memory<string> wordMemory = words;

            // Access via Span
            Span<string> wordSpan = wordMemory.Span;
            Console.WriteLine($"  Words: [{string.Join(", ", wordSpan.ToArray())}]");
            // Output: Words: [Apple, Banana, Cherry, Date]

            // Direct access
            Console.WriteLine($"  First: {wordMemory.Span[0]}");
            // Output: First: Apple

            // Indexer
            Console.WriteLine($"  wordMemory[2]: {wordMemory.Span[2]}");
            // Output: wordMemory[2]: Cherry

            // ReadOnlyMemory - created from Memory or directly
            ReadOnlyMemory<string> readOnlyMemory = wordMemory;
            ReadOnlySpan<string> readOnlySpan = readOnlyMemory.Span;
            Console.WriteLine($"  ReadOnly: [{string.Join(", ", readOnlySpan.ToArray())}]");
            // Output: ReadOnly: [Apple, Banana, Cherry, Date]
            Console.WriteLine();
        }

        static void MemorySlicing()
        {
            Console.WriteLine("--- 3. Memory<T> Slicing ---");
            Console.WriteLine();

            int[] data = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            Memory<int> fullMemory = data;

            // Slice from start
            Memory<int> firstHalf = fullMemory.Slice(0, 5);
            Console.WriteLine($"  First half: [{string.Join(", ", firstHalf.ToArray())}]");
            // Output: First half: [0, 1, 2, 3, 4]

            // Slice from position
            Memory<int> secondHalf = fullMemory.Slice(5);
            Console.WriteLine($"  Second half: [{string.Join(", ", secondHalf.ToArray())}]");
            // Output: Second half: [5, 6, 7, 8, 9]

            // Range operator (C# 8+)
            Memory<int> middle = fullMemory[3..7];
            Console.WriteLine($"  Middle [3..7]: [{string.Join(", ", middle.ToArray())}]");
            // Output: Middle [3..7]: [3, 4, 5, 6]

            // Negative index (C# 8+)
            Memory<int> lastThree = fullMemory[^3..];
            Console.WriteLine($"  Last three [^3..]: [{string.Join(", ", lastThree.ToArray())}]");
            // Output: Last three [^3..]: [7, 8, 9]

            // String slicing with Memory<char>
            string message = "Hello, Memory!";
            ReadOnlyMemory<char> messageMemory = message.ToArray();

            ReadOnlyMemory<char> hello = messageMemory[..5];
            Console.WriteLine($"  Hello: '{hello.ToString()}'");
            // Output: Hello: 'Hello'

            ReadOnlyMemory<char> memory = messageMemory[7..];
            Console.WriteLine($"  Memory: '{memory.ToString()}'");
            // Output: Memory: 'Memory!'
            Console.WriteLine();
        }

        static void MemoryVsSpan()
        {
            Console.WriteLine("--- 4. Memory<T> vs Span<T> ---");
            Console.WriteLine();

            // Key differences:
            // 1. Span<T> is ref struct (stack-only), Memory<T> can be stored in classes
            // 2. Memory<T> is heap-allocated, Span<T> is stack-allocated

            // Span<T> CANNOT be a class field
            // Memory<T> CAN be a class field

            int[] array = { 1, 2, 3 };

            // Span<T> - stack only
            Span<int> span = array;
            Console.WriteLine($"  Span: [{string.Join(", ", span.ToArray())}]");
            // Output: Span: [1, 2, 3]

            // Memory<T> - can escape the stack
            Memory<int> memory = array;
            Console.WriteLine($"  Memory: [{string.Join(", ", memory.ToArray())}]");
            // Output: Memory: [1, 2, 3]

            // Get underlying Span for both
            Span<int> spanFromMemory = memory.Span;
            Console.WriteLine($"  Memory.Span: [{string.Join(", ", spanFromMemory.ToArray())}]");
            // Output: Memory.Span: [1, 2, 3]

            // Memory<T> can be used in async operations
            Console.WriteLine("  Memory<T> can be stored in classes/async methods");
            Console.WriteLine("  Span<T> cannot escape the stack");
            // Output: Memory<T> can be stored in classes/async methods
            // Output: Span<T> cannot escape the stack
            Console.WriteLine();
        }

        static void MemoryInClasses()
        {
            Console.WriteLine("--- 5. Memory<T> in Classes ---");
            Console.WriteLine();

            // Create buffer processor that stores Memory in a class
            var processor = new DataBufferProcessor();

            int[] buffer = { 10, 20, 30, 40, 50 };
            processor.SetBuffer(buffer);

            processor.ProcessBuffer();
            processor.AppendMore(60);

            Console.WriteLine($"  Final processed: [{string.Join(", ", processor.GetData())}]");
            // Output: Final processed: [10, 20, 30, 40, 50, 60]

            // Can use Memory<T> as method parameter
            ProcessMemory(buffer);
            Console.WriteLine($"  After passing Memory: [{string.Join(", ", buffer)}]");
            // Output: After passing Memory: [10, 20, 30, 40, 50]

            // Memory<T> can be returned from methods
            Memory<int> returned = CreateAndProcess();
            Console.WriteLine($"  Returned Memory: [{string.Join(", ", returned.ToArray())}]");
            // Output: Returned Memory: [100, 200, 300]
            Console.WriteLine();
        }

        static void ProcessMemory(Memory<int> data)
        {
            Span<int> span = data.Span;
            for (int i = 0; i < span.Length; i++)
            {
                span[i] *= 2;
            }
        }

        static Memory<int> CreateAndProcess()
        {
            int[] array = { 50, 100, 150 };
            Memory<int> memory = array;

            // Modify via Span
            memory.Span[0] = 100;

            return memory;
        }

        static void AsyncWithMemory()
        {
            Console.WriteLine("--- 6. Async with Memory<T> ---");
            Console.WriteLine();

            // Simulate async processing with Memory<T>
            ProcessDataAsync().GetAwaiter().GetResult();

            // ArrayPool for efficient memory reuse
            int[] rentedArray = ArrayPool<int>.Shared.Rent(10);
            Console.WriteLine($"  Rented array length: {rentedArray.Length}");
            // Output: Rented array length: 10

            // Use the array
            for (int i = 0; i < 5; i++)
            {
                rentedArray[i] = i * 10;
            }

            // Return to pool
            ArrayPool<int>.Shared.Return(rentedArray);
            Console.WriteLine("  Array returned to pool");
            // Output: Array returned to pool
            Console.WriteLine();
        }

        static async Task ProcessDataAsync()
        {
            // Memory<T> can be used in async methods - unlike Span<T>
            int[] data = { 1, 2, 3, 4, 5 };
            Memory<int> memory = data;

            // Simulate async work
            await Task.Delay(10);

            // Process memory
            var result = SumMemoryAsync(memory);
            Console.WriteLine($"  Sum of Memory: {result}");
            // Output: Sum of Memory: 15
        }

        static int SumMemoryAsync(Memory<int> memory)
        {
            int sum = 0;
            Span<int> span = memory.Span;
            for (int i = 0; i < span.Length; i++)
            {
                sum += span[i];
            }
            return sum;
        }

        static void RealWorldExample()
        {
            Console.WriteLine("--- Real-World: Stream Processing with Memory ---");
            Console.WriteLine();

            var streamProcessor = new StreamDataProcessor();
            
            string sampleData = "FirstChunk,SecondChunk,ThirdChunk";
            byte[] dataBytes = System.Text.Encoding.UTF8.GetBytes(sampleData);

            streamProcessor.ProcessData(dataBytes);
            Console.WriteLine($"  All processed: {streamProcessor.GetAllResults()}");
            // Output: All processed: FirstChunk|SecondChunk|ThirdChunk

            // Memory<T> allows efficient buffer management in pipeline
            var bufferManager = new BufferManager();
            Memory<byte> buffer = bufferManager.GetBuffer(1024);

            // Fill buffer
            var span = buffer.Span;
            for (int i = 0; i < 100; i++)
            {
                span[i] = (byte)(i % 256);
            }

            Console.WriteLine($"  Buffer first 10 bytes: {string.Join(", ", span.Slice(0, 10).ToArray())}");
            // Output: Buffer first 10 bytes: 0, 1, 2, 3, 4, 5, 6, 7, 8, 9
            Console.WriteLine();
        }
    }

    class DataBufferProcessor
    {
        private Memory<int> _buffer;

        public void SetBuffer(int[] data)
        {
            _buffer = data;
        }

        public void ProcessBuffer()
        {
            Span<int> span = _buffer.Span;
            for (int i = 0; i < span.Length; i++)
            {
                span[i] += 1;
            }
        }

        public void AppendMore(int value)
        {
            // Create new array (in real code, consider ArrayPool)
            int[] current = _buffer.ToArray();
            int[] expanded = new int[current.Length + 1];
            Array.Copy(current, expanded, current.Length);
            expanded[current.Length] = value;

            _buffer = expanded;
        }

        public int[] GetData()
        {
            return _buffer.ToArray();
        }
    }

    class StreamDataProcessor
    {
        private string _allResults = "";

        public void ProcessData(byte[] data)
        {
            // Process in chunks
            string fullData = System.Text.Encoding.UTF8.GetString(data);
            string[] chunks = fullData.Split(',');

            foreach (var chunk in chunks)
            {
                ProcessChunk(chunk);
            }
        }

        private void ProcessChunk(string chunk)
        {
            // In real code: process chunk with Memory<T>
            ReadOnlyMemory<byte> chunkMemory = System.Text.Encoding.UTF8.GetBytes(chunk);

            // Process the chunk
            _allResults += chunk + "|";
        }

        public string GetAllResults()
        {
            return _allResults.TrimEnd('|');
        }
    }

    class BufferManager
    {
        private byte[] _pooledBuffer;

        public Memory<byte> GetBuffer(int size)
        {
            // In real code, use ArrayPool<byte>.Shared.Rent(size)
            _pooledBuffer = new byte[size];
            return _pooledBuffer;
        }

        public void ReturnBuffer()
        {
            // In real code: ArrayPool<byte>.Shared.Return(_pooledBuffer);
            _pooledBuffer = null;
        }
    }
}
