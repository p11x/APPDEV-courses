/*
 * ============================================================
 * TOPIC     : Memory Management
 * SUBTOPIC  : Array Pooling
 * FILE      : 03_ArrayPool.cs
 * PURPOSE   : Teaches ArrayPool<T> for reusing arrays,
 *            ideal for buffer management
 * ============================================================
 */

using System; // System namespace for Console, basic types
using System.Buffers; // For ArrayPool<T>

namespace CSharp_MasterGuide._08_MemoryManagement._04_ObjectPooling
{
    /// <summary>
    /// Demonstrates ArrayPool<T> for array reuse.
    /// ArrayPool<T> is optimized for byte[],
    /// commonly used in network I/O scenarios.
    /// </summary>
    class ArrayPool
    {
        static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════════════
            // CONCEPT: ArrayPool<T> ───────────────────────────────────────────────
            // ═════════════════════════════════════════════════════════════
            // ArrayPool<T> is specialized for array reuse:
            // - Optimized for byte[], int[], char[], etc.
            // - Shared default pool per type
            // - Thread-safe rentals/returns
            // - Minimum memory fragmentation
            //
            // Unlike ObjectPool<T>:
            // - Uses shared static instance
            // - Returns arrays to shared pool
            // - Multiple buffer sizes available
            //
            // Use cases:
            // - Network read/write buffers
            // - Stream operations
            // - Binary serialization

            Console.WriteLine("=== ArrayPool Demo ===\n");

            // ── EXAMPLE 1: Shared ArrayPool ───────────────────────────
            // Use shared ArrayPool<T>.Shared.

            Console.WriteLine("1. Shared ArrayPool:");

            // Rent array from shared pool
            byte[] buffer1 = ArrayPool<byte>.Shared.Rent(1024); // byte[] = rented buffer
            Console.WriteLine($"   Rented buffer length: {buffer1.Length}"); // Output: Rented buffer length: 1024

            // Fill with data
            for (int i = 0; i < 100; i++) // int = loop
            {
                buffer1[i] = (byte)i; // byte = fill
            }

            // Return to pool (don't forget!)
            ArrayPool<byte>.Shared.Return(buffer1, true); // true = clear array
            Console.WriteLine("   Buffer returned"); // Output: Buffer returned

            // ── EXAMPLE 2: Rent Specific Sizes ────────────────────────────
            // Request appropriate size buffer.

            Console.WriteLine("\n2. Various buffer sizes:");

            // Small buffer (gets at least requested)
            byte[] small = ArrayPool<byte>.Shared.Rent(64); // byte[] = small
            Console.WriteLine($"   Small: {small.Length}"); // Output: Small: 64 (or larger)

            // Medium buffer
            byte[] medium = ArrayPool<byte>.Shared.Rent(1024); // byte[] = medium
            Console.WriteLine($"   Medium: {medium.Length}"); // Output: Medium: 1024

            // Large buffer
            byte[] large = ArrayPool<byte>.Shared.Rent(65536); // byte[] = large
            Console.WriteLine($"   Large: {large.Length}"); // Output: Large: 65536

            // Return all
            ArrayPool<byte>.Shared.Return(small, false); // Return small
            ArrayPool<byte>.Shared.Return(medium, false); // Return medium
            ArrayPool<byte>.Shared.Return(large, false); // Return large

            Console.WriteLine("   All returned"); // Output: All returned

            // ── EXAMPLE 3: Int Array Pool ────────────────────────────────
            // Can pool any array type.

            Console.WriteLine("\n3. Int array pool:");

            int[] intArray = ArrayPool<int>.Shared.Rent(256); // int[] = array
            Console.WriteLine($"   Int array length: {intArray.Length}"); // Output: Int array length: 256

            // Fill with values
            for (int i = 0; i < 100; i++) // int = loop
            {
                intArray[i] = i * i; // int = square
            }

            // Process
            long sum = 0; // long = sum
            for (int i = 0; i < 100; i++) // int = loop
            {
                sum += intArray[i]; // int = add
            }
            Console.WriteLine($"   Sum: {sum}"); // Output: Sum: [n]

            // Return
            ArrayPool<int>.Shared.Return(intArray, true); // Clear on return

            // ── EXAMPLE 4: Char Array for String Operations ─────────────────
            Console.WriteLine("\n4. Char array for strings:");

            char[] chars = ArrayPool<char>.Shared.Rent(256); // char[] = chars
            string input = "Hello, ArrayPool!"; // string = input

            // Copy string to char array
            for (int i = 0; i < input.Length; i++) // int = loop
            {
                chars[i] = input[i]; // char = copy
            }

            // Process as string
            string result = new string(chars, 0, input.Length); // string = new string
            Console.WriteLine($"   Result: {result}"); // Output: Result: Hello, ArrayPool!

            // Return
            ArrayPool<char>.Shared.Return(chars, true); // Clear and return

            // ── EXAMPLE 5: Network Read Pattern ────────────────────
            Console.WriteLine("\n5. Network read pattern:");

            // Simulate network data
            byte[] networkData = System.Text.Encoding.UTF8.GetBytes("Network data received"); // byte[] = network

            // Rent buffer
            byte[] buffer = ArrayPool<byte>.Shared.Rent(1024); // byte[] = buffer

            // Copy to pooled buffer
            Buffer.BlockCopy(networkData, 0, buffer, 0, networkData.Length); // Copy data
            Console.WriteLine($"   Copied: {networkData.Length} bytes"); // Output: Copied: [n] bytes

            // Process (simulated)
            Console.WriteLine($"   Data: {System.Text.Encoding.UTF8.GetString(buffer, 0, networkData.Length)}"); // Output: Data: Network data received

            // Return buffer
            ArrayPool<byte>.Shared.Return(buffer, true); // Clear and return
            Console.WriteLine("   Buffer returned"); // Output: Buffer returned

            // ── EXAMPLE 6: Dual Buffer Pattern ────────────────────────
            // Use two buffers for duplex I/O.

            Console.WriteLine("\n6. Dual buffer pattern:");

            // Rent separate buffers
            byte[] sendBuffer = ArrayPool<byte>.Shared.Rent(512); // byte[] = send
            byte[] recvBuffer = ArrayPool<byte>.Shared.Rent(512); // byte[] = receive

            // Prepare send data
            string sendData = "Request data"; // string = send
            byte[] sendBytes = System.Text.Encoding.UTF8.GetBytes(sendData); // byte[] = bytes
            Buffer.BlockCopy(sendBytes, 0, sendBuffer, 0, sendBytes.Length); // Copy

            // Simulate send/receive
            Console.WriteLine($"   Sending: {sendData}"); // Output: Sending: Request data
            Console.WriteLine($"   Received: (simulated response)"); // Output: Received: (simulated response)

            // Return both buffers
            ArrayPool<byte>.Shared.Return(sendBuffer, true); // Return send
            ArrayPool<byte>.Shared.Return(recvBuffer, true); // Return recv

            Console.WriteLine("   Both buffers returned"); // Output: Both buffers returned

            // ── EXAMPLE 7: Memory Overlap ────────────────────────────
            // Check size rounding (pool may return larger).

            Console.WriteLine("\n7. Buffer size behavior:");

            // Request various sizes
            int[] requested = { 100, 128, 256, 512, 1024 }; // int[] = sizes
            foreach (int size in requested) // int = loop
            {
                byte[] buf = ArrayPool<byte>.Shared.Rent(size); // byte[] = rented
                Console.WriteLine($"   Requested: {size}, Got: {buf.Length}"); // Output sizes
                ArrayPool<byte>.Shared.Return(buf, false); // Return
            }

            // ── REAL-WORLD EXAMPLE: File Copy ─────────────────────
            Console.WriteLine("\n8. Real-world: File operations:");

            // Simulate file processing
            string[] lines = { "Line 1", "Line 2", "Line 3" }; // string[] = lines

            // Process each line with pooled buffer
            foreach (var line in lines) // string = loop
            {
                // Rent buffer for line
                char[] lineBuffer = ArrayPool<char>.Shared.Rent(256); // char[] = buffer

                // Copy line to buffer
                for (int i = 0; i < line.Length; i++) // int = loop
                {
                    lineBuffer[i] = line[i]; // char = copy
                }

                // Process line
                string processed = new string(lineBuffer, 0, line.Length); // string = processed
                Console.WriteLine($"   Processed: '{processed}'"); // Output: Processed: 'Line n'

                // Return buffer
                ArrayPool<char>.Shared.Return(lineBuffer, true); // Clear and return
            }

            Console.WriteLine("   File processing complete"); // Output message

            // ── REAL-WORLD EXAMPLE: Binary Encoder ─────────────────
            Console.WriteLine("\n9. Real-world: Binary encoding:");

            // Encode multiple values
            byte[] encodeBuffer = ArrayPool<byte>.Shared.Rent(16); // byte[] = buffer

            // Encode int value
            int value = 12345; // int = value to encode
            unsafe // Enable for BitConverter (may need unsafe)
            {
                // Write value as bytes
                byte[] valueBytes = BitConverter.GetBytes(value); // byte[] = bytes
                for (int i = 0; i < valueBytes.Length; i++) // int = loop
                {
                    encodeBuffer[i] = valueBytes[i]; // byte = copy
                }
            }

            Console.WriteLine($"   Encoded {value} as bytes"); // Output: Encoded 12345 as bytes

            // Decode
            int decoded = BitConverter.ToInt32(encodeBuffer, 0); // int = decoded
            Console.WriteLine($"   Decoded: {decoded}"); // Output: Decoded: 12345

            // Return
            ArrayPool<byte>.Shared.Return(encodeBuffer, true); // Clear and return

            Console.WriteLine("\n=== ArrayPool Demo Complete ===");
        }
    }
}