/*
 * ============================================================
 * TOPIC     : Memory Management
 * SUBTOPIC  : Unsafe Code - Real-World Examples
 * FILE      : 05_Unsafe_RealWorld.cs
 * PURPOSE   : Practical real-world examples of unsafe code
 *            in performance-critical scenarios
 * ============================================================
 */

using System; // System namespace for Console, basic types

namespace CSharp_MasterGuide._08_MemoryManagement._03_UnsafeCode
{
    /// <summary>
    /// Real-world examples of unsafe code usage
    /// in performance-critical applications.
    /// </summary>
    class Unsafe_RealWorld
    {
        static void Main(string[] args)
        {
#if unsafe
            // ═══════════════════════════════════════════════════════════
            // REAL-WORLD USE CASES ──────────────────────────────────────
            // ═══════════════════════════════════════════════════
            // Unsafe code is used when:
            // - Interoperating with native libraries
            // - High-performance data processing
            // - Working with binary protocols
            // - Low-level system programming
            // - Memory-mapped files
            //
            // Best practices:
            // - Minimize unsafe code scope
            // - Use Span<T> for safety
            // - Validate all pointers
            // - Handle exceptions

            Console.WriteLine("=== Unsafe Real-World Examples ===\n");

            // ═══════════════════════════════════════════════════════════
            // SCENARIO 1: High-Performance JSON Parsing ─────────────────
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("=== Scenario 1: JSON Parsing (High Perf) ===\n");

            Console.WriteLine("1.1. Parse JSON buffer:");

            unsafe // unsafe = enable pointers
            {
                // JSON string as bytes
                byte[] json = System.Text.Encoding.UTF8.GetBytes("{\"name\":\"John\",\"age\":30}"); // byte[] = JSON bytes
                
                fixed (byte* ptr = json) // fixed = pin array
                {
                    // Find property name
                    int nameStart = FindString(ptr, json.Length, "name"); // int = search
                    Console.WriteLine($"   'name' at index: {nameStart}"); // Output: 'name' at index
                    
                    // Find value
                    int valueStart = FindString(ptr, json.Length, "John"); // int = search
                    Console.WriteLine($"   'John' at index: {valueStart}"); // Output: 'John' at index
                }
            }

            // ═══════════════════════════════════════════════════════════
            // SCENARIO 2: Image Processing ────────────────────────────
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n=== Scenario 2: Image Processing ===\n");

            Console.WriteLine("2.1. Grayscale conversion:");

            unsafe // unsafe = enable pointers
            {
                // Simulate 4x4 pixel image (RGBA)
                byte[] pixels = new byte[4 * 4 * 4]; // byte[] = pixel data
                
                // Fill with test pattern (red-ish)
                for (int i = 0; i < pixels.Length; i += 4) // int = pixel loop
                {
                    pixels[i] = 200; // R = red
                    pixels[i + 1] = 50; // G = green
                    pixels[i + 2] = 50; // B = blue
                    pixels[i + 3] = 255; // A = alpha
                }
                
                fixed (byte* ptr = pixels) // fixed = pin image
                {
                    // Convert to grayscale
                    for (int i = 0; i < pixels.Length; i += 4) // int = pixel loop
                    {
                        byte r = ptr[i]; // byte = red
                        byte g = ptr[i + 1]; // byte = green
                        byte b = ptr[i + 2]; // byte = blue
                        
                        // Luminance formula
                        byte gray = (byte)((r * 0.299) + (g * 0.587) + (b * 0.114)); // byte = gray value
                        
                        ptr[i] = gray; // Set gray
                        ptr[i + 1] = gray; // Set gray
                        ptr[i + 2] = gray; // Set gray
                    }
                }
                
                Console.WriteLine($"   First pixel R: {pixels[0]}"); // Output: First pixel R: [gray value]
                Console.WriteLine($"   First pixel G: {pixels[1]}"); // Output: First pixel G: [gray value]
            }

            // ═══════════════════════════════════════════════════════════
            // SCENARIO 3: Binary Protocol Parsing ─────────────────────
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n=== Scenario 3: Binary Protocol ===\n");

            Console.WriteLine("3.1. Parse binary message:");

            unsafe // unsafe = enable pointers
            {
                // Binary message: [length:2][type:1][data:length-3]
                byte[] message = new byte[10]; // byte[] = message
                message[0] = 0x00; // byte = length high
                message[1] = 0x07; // byte = length (7 bytes)
                message[2] = 0x01; // byte = type (request)
                message[3] = (byte)'H'; // byte = data
                message[4] = (byte)'e'; // byte = data
                message[5] = (byte)'l'; // byte = data
                message[6] = (byte)'l'; // byte = data
                message[7] = (byte)'o'; // byte = data
                message[8] = (byte)0x00; // padding
                message[9] = (byte)0x00; // padding
                
                fixed (byte* ptr = message) // fixed = pin message
                {
                    // Parse header
                    ushort length = (ushort)((ptr[0] << 8) | ptr[1]); // ushort = message length
                    byte type = ptr[2]; // byte = message type
                    
                    Console.WriteLine($"   Length: {length}"); // Output: Length: 7
                    Console.WriteLine($"   Type: {type}"); // Output: Type: 1
                    
                    // Parse data
                    Console.Write("   Data: "); // Output prefix
                    for (int i = 3; i < 3 + (length - 3); i++) // int = data loop
                    {
                        Console.Write((char)ptr[i]); // Output char
                    }
                    Console.WriteLine(); // New line
                }
            }

            // ═══════════════════════════════════════════════════════════
            // SCENARIO 4: CRC Calculation ─────────────────────────────
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n=== Scenario 4: CRC Calculation ===\n");

            Console.WriteLine("4.1. Calculate CRC32:");

            unsafe // unsafe = enable pointers
            {
                byte[] data = System.Text.Encoding.UTF8.GetBytes("Hello World"); // byte[] = data
                
                fixed (byte* ptr = data) // fixed = pin data
                {
                    uint crc = CalculateCRC(ptr, data.Length); // uint = CRC result
                    Console.WriteLine($"   CRC32: 0x{crc:X8}"); // Output: CRC32: 0x[value]
                }
            }

            // ═══════════════════════════════════════════════════════════
            // SCENARIO 5: Memory Comparison ──────────────────────────
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n=== Scenario 5: Diff Detection ===\n");

            Console.WriteLine("5.1. Compare memory blocks:");

            unsafe // unsafe = enable pointers
            {
                byte[] block1 = new byte[256]; // byte[] = first block
                byte[] block2 = new byte[256]; // byte[] = second block
                
                // Fill blocks
                for (int i = 0; i < 256; i++) // int = loop
                {
                    block1[i] = (byte)i; // byte = fill first
                    block2[i] = (byte)((i == 100) ? 200 : i); // byte = second with diff
                }
                
                fixed (byte* b1 = block1, b2 = block2) // fixed = pin both
                {
                    int diffIndex = FindDifference(b1, b2, 256); // int = find diff
                    Console.WriteLine($"   First difference at: {diffIndex}"); // Output: First difference at: 100
                }
            }

            // ═══════════════════════════════════════════════════════════
            // SCENARIO 6: Byte Swapping ────────────────────────────
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n=== Scenario 6: Byte Swapping ===\n");

            Console.WriteLine("6.1. Swap endianness:");

            unsafe // unsafe = enable pointers
            {
                uint value = 0x12345678; // uint = value to swap
                uint swapped = ByteSwap(value); // uint = swapped result
                Console.WriteLine($"   Original: 0x{value:X8}"); // Output: Original: 0x12345678
                Console.WriteLine($"   Swapped: 0x{swapped:X8}"); // Output: Swapped: 0x78563412
            }

            // ═══════════════════════════════════════════════════════════
            // SCENARIO 7: Hash Calculation ───────────────────────────
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n=== Scenario 7: FNV Hash ===\n");

            Console.WriteLine("7.1. Calculate FNV hash:");

            unsafe // unsafe = enable pointers
            {
                string input = "Hello"; // string = input string
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(input); // byte[] = bytes
                
                fixed (byte* ptr = bytes) // fixed = pin bytes
                {
                    ulong hash = CalculateFNV(ptr, bytes.Length); // ulong = FNV hash
                    Console.WriteLine($"   FNV1a: 0x{hash:X16}"); // Output: FNV1a: 0x[value]
                }
            }

            // ═══════════════════════════════════════════════════════════
            // SCENARIO 8: Quick Sort ─────────────────────────────────
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("\n=== Scenario 8: Quick Sort ===\n");

            Console.WriteLine("8.1. Quick sort with pointers:");

            unsafe // unsafe = enable pointers
            {
                int[] array = { 5, 2, 8, 1, 9, 3, 7, 4, 6 }; // int[] = unsorted
                
                Console.WriteLine($"   Before: {array[0]}, {array[1]}, {array[2]}, {array[3]}, {array[4]}, {array[5]}, {array[6]}, {array[7]}, {array[8]}"); // Output before
                
                fixed (int* ptr = array) // fixed = pin array
                {
                    QuickSort(ptr, 0, array.Length - 1); // Sort
                }
                
                Console.WriteLine($"   After: {array[0]}, {array[1]}, {array[2]}, {array[3]}, {array[4]}, {array[5]}, {array[6]}, {array[7]}, {array[8]}"); // Output after
            }

            Console.WriteLine("\n=== All Real-World Examples Complete ===");
#else
            Console.WriteLine("=== Unsafe Real-World Examples ===");
            Console.WriteLine("NOTE: Unsafe code is disabled in this build.");
            Console.WriteLine("Compile with /unsafe flag to enable.");
            Console.WriteLine("");
            Console.WriteLine("Real-World Use Cases:");
            Console.WriteLine("- JSON/binary parsing");
            Console.WriteLine("- Image processing");
            Console.WriteLine("- Network protocol parsing");
            Console.WriteLine("- CRC/hash calculations");
            Console.WriteLine("- Data comparison");
            Console.WriteLine("- Byte manipulation");
            Console.WriteLine("- High-perf sorting");
#endif
        }

#if unsafe
        // Find string in buffer
        static int FindString(byte* ptr, int length, string search) // int = search result
        {
            byte[] searchBytes = System.Text.Encoding.UTF8.GetBytes(search); // byte[] = search bytes
            int searchLen = searchBytes.Length; // int = search length
            
            for (int i = 0; i <= length - searchLen; i++) // int = loop
            {
                bool found = true; // bool = found flag
                for (int j = 0; j < searchLen && found; j++) // int = inner loop
                {
                    if (ptr[i + j] != searchBytes[j]) // Check bytes
                    {
                        found = false; // Set not found
                    }
                }
                if (found) // Found
                {
                    return i; // Return index
                }
            }
            return -1; // Not found
        }

        // CRC32 calculation
        static uint CalculateCRC(byte* ptr, int length) // uint = CRC value
        {
            uint crc = 0xFFFFFFFF; // uint = initial
            for (int i = 0; i < length; i++) // int = loop
            {
                crc ^= ptr[i]; // byte = xor
                for (int j = 0; j < 8; j++) // int = bit loop
                {
                    if ((crc & 1) != 0) // Check bit
                    {
                        crc = (crc >> 1) ^ 0xEDB88320; // uint = polynomial
                    }
                    else
                    {
                        crc >>= 1; // Shift
                    }
                }
            }
            return ~crc; // uint = invert
        }

        // Find first difference
        static int FindDifference(byte* b1, byte* b2, int length) // int = index
        {
            for (int i = 0; i < length; i++) // int = loop
            {
                if (b1[i] != b2[i]) // Compare
                {
                    return i; // Return index
                }
            }
            return -1; // No difference
        }

        // Byte swap for uint
        static uint ByteSwap(uint value) // uint = swapped value
        {
            unsafe // unsafe needed for bit operations
            {
                uint result; // uint = result
                byte* src = (byte*)&value; // byte* = source
                byte* dst = (byte*)&result; // byte* = destination
                
                dst[0] = src[3]; // Swap bytes
                dst[1] = src[2]; // Swap bytes
                dst[2] = src[1]; // Swap bytes
                dst[3] = src[0]; // Swap bytes
                
                return result; // uint = return swapped
            }
        }

        // FNV-1a hash
        static ulong CalculateFNV(byte* ptr, int length) // ulong = hash
        {
            ulong hash = 14695981039346656037UL; // ulong = FNV offset
            ulong prime = 1099511628211UL; // ulong = FNV prime
            
            for (int i = 0; i < length; i++) // int = loop
            {
                hash ^= ptr[i]; // byte = xor
                hash *= prime; // Multiply
            }
            return hash; // ulong = return hash
        }

        // QuickSort implementation
        static void QuickSort(int* arr, int low, int high) // void = sort
        {
            if (low < high) // Check if valid
            {
                int pivot = Partition(arr, low, high); // int = partition
                QuickSort(arr, low, pivot - 1); // Sort left
                QuickSort(arr, pivot + 1, high); // Sort right
            }
        }

        static int Partition(int* arr, int low, int high) // int = partition point
        {
            int pivot = arr[high]; // int = pivot value
            int i = low - 1; // int = index
            
            for (int j = low; j < high; j++) // int = loop
            {
                if (arr[j] <= pivot) // Compare
                {
                    i++; // Increment
                    Swap(arr, i, j); // Swap
                }
            }
            Swap(arr, i + 1, high); // Swap pivot
            
            return i + 1; // int = return point
        }

        static void Swap(int* arr, int i, int j) // void = swap
        {
            int temp = arr[i]; // int = temp
            arr[i] = arr[j]; // Swap
            arr[j] = temp; // Complete swap
        }
#endif
    }
}