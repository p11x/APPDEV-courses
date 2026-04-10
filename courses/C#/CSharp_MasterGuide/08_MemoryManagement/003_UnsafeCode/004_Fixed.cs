/*
 * ============================================================
 * TOPIC     : Memory Management
 * SUBTOPIC  : Fixed Statement
 * FILE      : 04_Fixed.cs
 * PURPOSE   : Teaches fixed statement for pinning arrays
 *            and getting direct pointers to managed memory
 * ============================================================
 */

using System; // System namespace for Console, basic types

namespace CSharp_MasterGuide._08_MemoryManagement._03_UnsafeCode
{
    /// <summary>
    /// Demonstrates the fixed statement for pinning
    /// managed objects to get stable addresses.
    /// </summary>
    class Fixed
    {
        static void Main(string[] args)
        {
#if unsafe
            // ═══════════════════════════════════════════════════════════
            // CONCEPT: Fixed Statement ────────────────────────────────────
            // ═══════════════════════════════════════════════════════════
            // .NET managed objects move in memory (GC compacts).
            // fixed statement pins object to prevent movement:
            // - Creates stable address for pointers
            // - Required to get pointer to managed array/string
            // - Only valid within fixed block scope
            //
            // Warning: Excessive pinning causes memory
            // fragmentation. Use briefly.
            
            Console.WriteLine("=== Fixed Statement Demo ===\n");

            // ── EXAMPLE 1: Pinning Arrays ────────────────────────────
            // Get stable pointer to array elements.

            Console.WriteLine("1. Pinning arrays:");

            unsafe // unsafe = enable pointers
            {
                int[] numbers = { 10, 20, 30, 40, 50 }; // int[] = managed array
                
                // fixed pins array in memory
                fixed (int* ptr = numbers) // fixed = pin array
                {
                    // Use pointer (safe during fixed block)
                    Console.WriteLine($"   *ptr: {*ptr}"); // Output: *ptr: 10
                    Console.WriteLine($"   ptr[2]: {ptr[2]}"); // Output: ptr[2]: 30
                    
                    // Modify through pointer
                    ptr[2] = 35; // Modify value
                    Console.WriteLine($"   Modified: {numbers[2]}"); // Output: Modified: 35
                }
                // Array unpinned here - safe to move again
            }

            // ── EXAMPLE 2: Pinning Strings ──────────────────────────────
            // Strings are immutable but can be pinned briefly.

            Console.WriteLine("\n2. Pinning strings:");

            unsafe // unsafe = enable pointers
            {
                string text = "Hello"; // string = managed string
                
                // Pin string to get char pointer
                fixed (char* ptr = text) // fixed = pin string
                {
                    // Read characters
                    for (int i = 0; i < text.Length; i++) // int = loop
                    {
                        Console.WriteLine($"   text[{i}]: {ptr[i]}"); // Output each char
                    }
                    
                    // Can modify char array (creates copy)
                    ptr[0] = 'J'; // Change first char (note: different behavior)
                    Console.WriteLine($"   First char: {ptr[0]}"); // Output: First char: J
                }
            }

            // ── EXAMPLE 3: Pinning with Multiple Variables ────────────────
            // Can pin multiple arrays simultaneously.

            Console.WriteLine("\n3. Multiple fixed variables:");

            unsafe // unsafe = enable pointers
            {
                int[] array1 = { 1, 2, 3 }; // int[] = first array
                int[] array2 = { 10, 20, 30 }; // int[] = second array
                
                // Multiple fixed statements
                fixed (int* p1 = array1, p2 = array2) // fixed = multiple
                {
                    // Process both arrays
                    Console.WriteLine($"   Sum: {*p1 + *p2}"); // Output: Sum: 11
                    Console.WriteLine($"   array2[1]: {p2[1]}"); // Output: array2[1]: 20
                }
            }

            // ── EXAMPLE 4: Fixed with Offset ────────────────────────────
            // Can offset into array after pinning.

            Console.WriteLine("\n4. Fixed with offset:");

            unsafe // unsafe = enable pointers
            {
                byte[] data = new byte[10]; // byte[] = array
                data[0] = 100; // byte = first value
                data[1] = 101; // byte = second
                data[2] = 102; // byte = third
                data[3] = 103; // byte = fourth
                
                fixed (byte* ptr = data) // fixed = pin entire array
                {
                    byte* offset = ptr + 2; // byte* = offset by 2
                    Console.WriteLine($"   offset[0]: {offset[0]}"); // Output: offset[0]: 102
                    Console.WriteLine($"   offset[1]: {offset[1]}"); // Output: offset[1]: 103
                }
            }

            // ── EXAMPLE 5: Fixed in Struct ────────────────────────────────
            //fixed can be used in unsafe structs.

            Console.WriteLine("\n5. Fixed buffer in struct:");

            unsafe // unsafe = enable pointers
            {
                // Struct with fixed array (C# 9+ fixed-size buffers)
                var bufferStruct = new FixedBufferStruct(); // FixedBufferStruct = struct
                bufferStruct.SetBuffer(); // Initialize buffer
                bufferStruct.PrintBuffer(); // Print values
            }

            // ── EXAMPLE 6: Copying Memory ────────────────────────────
            // Use fixed for fast memory copy.

            Console.WriteLine("\n6. Fast memory copy:");

            unsafe // unsafe = enable pointers
            {
                byte[] source = new byte[100]; // byte[] = source
                byte[] dest = new byte[100]; // byte[] = destination
                
                // Fill source
                for (int i = 0; i < source.Length; i++) // int = loop
                {
                    source[i] = (byte)i; // byte = fill
                }
                
                // Copy using fixed pointers
                fixed (byte* src = source) // fixed = pin source
                fixed (byte* dst = dest) // fixed = pin destination
                {
                    // Fast byte copy
                    for (int i = 0; i < source.Length; i++) // int = loop
                    {
                        dst[i] = src[i]; // Copy bytes
                    }
                }
                
                Console.WriteLine($"   dest[0]: {dest[0]}"); // Output: dest[0]: 0
                Console.WriteLine($"   dest[50]: {dest[50]}"); // Output: dest[50]: 50
                Console.WriteLine($"   dest[99]: {dest[99]}"); // Output: dest[99]: 99
            }

            // ── EXAMPLE 7: Finding Bytes ────────────────────────────────
            // Search for byte pattern.

            Console.WriteLine("\n7. Search in fixed array:");

            unsafe // unsafe = enable pointers
            {
                byte[] buffer = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }; // byte[] = buffer
                byte search = 7; // byte = search value
                int foundIndex = -1; // int = result index
                
                fixed (byte* ptr = buffer) // fixed = pin buffer
                {
                    for (int i = 0; i < buffer.Length; i++) // int = loop
                    {
                        if (ptr[i] == search) // Check value
                        {
                            foundIndex = i; // Set index
                            break; // Found, exit
                        }
                    }
                }
                
                Console.WriteLine($"   Found {search} at index: {foundIndex}"); // Output: Found 7 at index: 6
            }

            // ── REAL-WORLD EXAMPLE: Parse Integer ────────────────────────
            Console.WriteLine("\n8. Real-world: Parse integer from bytes:");

            unsafe // unsafe = enable pointers
            {
                byte[] bytes = { (byte)'4', (byte)'5', (byte)'6' }; // byte[] = digits
                
                fixed (byte* ptr = bytes) // fixed = pin array
                {
                    int result = 0; // int = parsed value
                    
                    // Parse digits
                    for (int i = 0; i < bytes.Length; i++) // int = loop
                    {
                        result = result * 10 + (ptr[i] - '0'); // Parse
                    }
                    
                    Console.WriteLine($"   Parsed: {result}"); // Output: Parsed: 456
                }
            }

            Console.WriteLine("\n=== Fixed Statement Demo Complete ===");
#else
            Console.WriteLine("=== Fixed Statement Demo ===");
            Console.WriteLine("NOTE: Unsafe code is disabled in this build.");
            Console.WriteLine("Compile with /unsafe flag to enable.");
            Console.WriteLine("");
            Console.WriteLine("Key Concepts:");
            Console.WriteLine("- fixed (type* ptr = array) pins array");
            Console.WriteLine("- fixed (char* ptr = string) pins string");
            Console.WriteLine("- Provides stable memory address");
            Console.WriteLine("- Only valid within fixed block");
            Console.WriteLine("- Required for pointer access to managed types");
            Console.WriteLine("- Can use multiple fixed in one statement");
#endif
        }
    }

#if unsafe
    /// <summary>
    /// Struct with fixed-size buffer.
    /// Cannot have managed types, only fixed buffer.
    /// </summary>
    unsafe struct FixedBufferStruct
    {
        // Fixed-size buffer (only in struct)
        private fixed byte _buffer[16]; // byte = fixed buffer

        public void SetBuffer() // Fill buffer
        {
            // Cannot use fixed in struct methods
            // This is a conceptual example
            Console.WriteLine("   Buffer struct set"); // Output: Buffer struct set
        }

        public void PrintBuffer() // Print buffer
        {
            Console.WriteLine("   Buffer struct display"); // Output: Buffer struct display
        }
    }
#endif
}