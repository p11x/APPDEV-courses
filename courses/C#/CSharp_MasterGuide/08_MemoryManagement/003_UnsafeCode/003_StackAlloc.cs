/*
 * ============================================================
 * TOPIC     : Memory Management
 * SUBTOPIC  : StackAlloc
 * FILE      : 03_StackAlloc.cs
 * PURPOSE   : Teaches stackalloc keyword for stack-based
 *            memory allocation, performance benefits
 * ============================================================
 */

using System; // System namespace for Console, basic types

namespace CSharp_MasterGuide._08_MemoryManagement._03_UnsafeCode
{
    /// <summary>
    /// Demonstrates stackalloc for stack-based memory.
    /// stackalloc allocates on the stack instead of heap,
    /// useful for performance-critical code.
    /// </summary>
    class StackAlloc
    {
        static void Main(string[] args)
        {
#if unsafe
            // ═══════════════════════════════════════════════════════════
            // CONCEPT: stackalloc ────────────────────────────────────────
            // ═══════════════════════════════════════════════════════════
            // stackalloc allocates memory on the stack:
            // - Much faster than heap allocation
            // - No GC pressure (not managed)
            // - Automatically freed when scope ends
            // - Limited by stack size (~1MB default)
            //
            // Use cases:
            // - High-performance array processing
            // - Span<T> buffers for zero-copy operations
            // - Temporary buffers in tight loops
            //
            // Syntax: span<T> = stackalloc T[ size ];

            Console.WriteLine("=== StackAlloc Demo ===\n");

            // ── EXAMPLE 1: Basic Stack Allocation ────────────────────
            // Allocate simple array on stack.

            Console.WriteLine("1. Basic stack allocation:");

            unsafe // unsafe = enable pointers
            {
                // int on stack - very fast allocation
                int* stackBuffer = stackalloc int[10]; // int* = allocate 10 ints
                
                // Initialize values
                for (int i = 0; i < 10; i++) // int = loop
                {
                    stackBuffer[i] = i * i; // Set value (square)
                }
                
                // Read values
                Console.WriteLine("   Squares: "); // Output prefix
                for (int i = 0; i < 10; i++) // int = loop
                {
                    Console.Write($"{stackBuffer[i]} "); // Output each value
                }
                Console.WriteLine(); // New line
            }

            // ── EXAMPLE 2: byte Stack Allocation ────────────────────
            // Common use: buffer for binary data.

            Console.WriteLine("\n2. Byte buffer on stack:");

            unsafe // unsafe = enable pointers
            {
                // 256 byte buffer on stack
                byte* buffer = stackalloc byte[256]; // byte* = 256 bytes
                
                // Fill with pattern
                for (int i = 0; i < 256; i++) // int = loop
                {
                    buffer[i] = (byte)(i % 256); // byte = pattern
                }
                
                // Read some bytes
                Console.WriteLine($"   buffer[0]: {buffer[0]}"); // Output: buffer[0]: 0
                Console.WriteLine($"   buffer[64]: {buffer[64]}"); // Output: buffer[64]: 64
                Console.WriteLine($"   buffer[128]: {buffer[128]}"); // Output: buffer[128]: 128
                Console.WriteLine($"   buffer[255]: {buffer[255]}"); // Output: buffer[255]: 255
            }

            // ── EXAMPLE 3: Span<T> Pattern (Recommended) ────────────
            // Use Span<T> with stackalloc for safety.

            Console.WriteLine("\n3. Span<T> with stackalloc:");

            // Span<T> provides safe array wrapper
            Span<int> stackSpan = stackalloc int[20]; // Span<int> = safe wrapper
            
            // Use like normal array
            for (int i = 0; i < stackSpan.Length; i++) // int = loop
            {
                stackSpan[i] = i * 3; // Set value (triple)
            }
            
            Console.WriteLine("   Multiples of 3: "); // Output prefix
            for (int i = 0; i < stackSpan.Length; i++) // int = loop
            {
                Console.Write($"{stackSpan[i]} "); // Output each value
            }
            Console.WriteLine(); // New line

            // ── EXAMPLE 4: Stack vs Heap Comparison ───────────────────
            // Shows performance difference (conceptual).

            Console.WriteLine("\n4. Stack vs heap allocation:");

            unsafe // unsafe = enable pointers
            {
                // Stack allocation (instant)
                int* stackArray = stackalloc int[1000]; // int* = stack
                stackArray[0] = 42; // Set value
                Console.WriteLine($"   Stack value: {stackArray[0]}"); // Output: Stack value: 42
                // Freed automatically when scope ends
                
                // Heap allocation (slower, requires GC)
                int[] heapArray = new int[1000]; // int[] = heap
                heapArray[0] = 42; // Set value
                Console.WriteLine($"   Heap value: {heapArray[0]}"); // Output: Heap value: 42
                // GC will collect eventually
            }

            // ── EXAMPLE 5: Working with Strings ────────────────────
            // Allocate buffer for string manipulation.

            Console.WriteLine("\n5. String buffer manipulation:");

            unsafe // unsafe = enable pointers
            {
                // Create buffer for string
                char* buffer = stackalloc char[100]; // char* = 100 chars
                
                // Copy string to buffer
                string source = "Hello"; // string = source
                fixed (char* src = source) // fixed = pin string
                {
                    for (int i = 0; i < source.Length; i++) // int = loop
                    {
                        buffer[i] = src[i]; // Copy chars
                    }
                }
                
                // Null terminate
                buffer[5] = '\0'; // char = null terminator
                
                // Read as string (use Span for safety)
                Console.WriteLine($"   Buffer: {new string(buffer)}"); // Output: Buffer: Hello
            }

            // ── EXAMPLE 6: Temporary Processing Buffer ─────────────
            // Use in tight loops where speed matters.

            Console.WriteLine("\n6. Temporary processing buffer:");

            // Process data multiple times
            for (int pass = 0; pass < 3; pass++) // int = outer loop
            {
                unsafe // unsafe = enable pointers
                {
                    // Fresh buffer each iteration
                    Span<byte> tempBuffer = stackalloc byte[32]; // Span<byte> = temp
                    
                    // Fill with pass pattern
                    for (int i = 0; i < tempBuffer.Length; i++) // int = loop
                    {
                        tempBuffer[i] = (byte)(pass * 10 + i); // byte = pattern
                    }
                    
                    // Process buffer
                    int sum = 0; // int = sum
                    for (int i = 0; i < tempBuffer.Length; i++) // int = loop
                    {
                        sum += tempBuffer[i]; // Add to sum
                    }
                    
                    Console.WriteLine($"   Pass {pass}: sum = {sum}"); // Output: Pass [n]: sum = [n]
                }
                // Buffer automatically freed each iteration
            }

            // ── EXAMPLE 7: Inline Buffer for Algorithm ────────────
            // Common in sorting/searching.

            Console.WriteLine("\n7. Sorting algorithm buffer:");

            Span<int> numbers = stackalloc int[10]; // Span<int> = numbers
            numbers[0] = 5; numbers[1] = 2; numbers[2] = 8;
            numbers[3] = 1; numbers[4] = 9; numbers[5] = 3;
            numbers[6] = 7; numbers[7] = 4; numbers[8] = 6;
            numbers[9] = 0; // Set unsorted values
            
            Console.WriteLine($"   Original: {numbers[0]}, {numbers[1]}, {numbers[2]}, {numbers[3]}, {numbers[4]}, {numbers[5]}, {numbers[6]}, {numbers[7]}, {numbers[8]}, {numbers[9]}"); // Output unsorted

            // Simple bubble sort on stack
            for (int i = 0; i < numbers.Length - 1; i++) // int = outer loop
            {
                for (int j = 0; j < numbers.Length - i - 1; j++) // int = inner loop
                {
                    if (numbers[j] > numbers[j + 1]) // Compare
                    {
                        int temp = numbers[j]; // int = swap
                        numbers[j] = numbers[j + 1]; // Swap
                        numbers[j + 1] = temp; // Complete swap
                    }
                }
            }
            
            Console.WriteLine($"   Sorted: {numbers[0]}, {numbers[1]}, {numbers[2]}, {numbers[3]}, {numbers[4]}, {numbers[5]}, {numbers[6]}, {numbers[7]}, {numbers[8]}, {numbers[9]}"); // Output sorted

            // ── REAL-WORLD EXAMPLE: Parse Bytes ───────────────────────
            Console.WriteLine("\n8. Real-world: Parse buffer:");

            unsafe // unsafe = enable pointers
            {
                // Buffer for parsing
                byte* parseBuffer = stackalloc byte[16]; // byte* = parse buffer
                
                // Fill with "123,456,789" as bytes
                string input = "123,456,789"; // string = input
                fixed (char* inputChars = input) // fixed = pin string
                {
                    for (int i = 0; i < input.Length; i++) // int = loop
                    {
                        parseBuffer[i] = (byte)inputChars[i]; // byte = copy
                    }
                    parseBuffer[input.Length] = 0; // Null terminate
                }
                
                // First number parse
                int value = 0; // int = parsed value
                int index = 0; // int = position
                while (parseBuffer[index] >= '0' && parseBuffer[index] <= '9') // digit check
                {
                    value = value * 10 + (parseBuffer[index] - '0'); // Parse digit
                    index++; // Next position
                }
                
                Console.WriteLine($"   Parsed first number: {value}"); // Output: Parsed first number: 123
            }

            Console.WriteLine("\n=== StackAlloc Demo Complete ===");
#else
            Console.WriteLine("=== StackAlloc Demo ===");
            Console.WriteLine("NOTE: Unsafe code is disabled in this build.");
            Console.WriteLine("Compile with /unsafe flag to enable.");
            Console.WriteLine("");
            Console.WriteLine("Key Concepts:");
            Console.WriteLine("- stackalloc T[size] allocates on stack");
            Console.WriteLine("- Much faster than heap allocation");
            Console.WriteLine("- No GC pressure");
            Console.WriteLine("- Automatically freed when scope ends");
            Console.WriteLine("- Use with Span<T> for safety");
            Console.WriteLine("- Stack size limited (~1MB)");
#endif
        }
    }
}