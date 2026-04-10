/*
 * ============================================================
 * TOPIC     : Memory Management
 * SUBTOPIC  : Unsafe Code - Pointers
 * FILE      : 01_UnsafePointers.cs
 * PURPOSE   : Teaches unsafe keyword, pointer basics,
 *            and direct memory manipulation
 * ============================================================
 */

using System; // System namespace for Console, basic types

namespace CSharp_MasterGuide._08_MemoryManagement._03_UnsafeCode
{
    /// <summary>
    /// Demonstrates unsafe code and pointer basics.
    /// Unsafe code allows direct memory manipulation
    /// using pointers, similar to C/C++.
    /// </summary>
    class UnsafePointers
    {
        static void Main(string[] args)
        {
#if unsafe
            // ═══════════════════════════════════════════════════════════
            // CONCEPT: Unsafe Code ────────────────────────────────────
            // ═══════════════════════════════════════════════════════════
            // Unsafe code bypasses .NET type safety:
            // - Direct memory access via pointers
            // - Faster operations (no bounds checking)
            // - Interop with native code
            // - Required for some performance-critical code
            //
            // Compiler flag /unsafe required to compile.
            // Use with caution - can cause memory corruption.
            
            Console.WriteLine("=== Unsafe Pointers Demo ===\n");

            // ── EXAMPLE 1: Unsafe Block ─────────────────────────────
            // Keyword 'unsafe' marks pointer-allowed code.

            Console.WriteLine("1. Basic unsafe block:");

            unsafe // unsafe = enable pointer operations
            {
                int value = 42; // int = value type on stack
                int* pointer = &value; // int* = pointer to value
                
                Console.WriteLine($"   Value: {value}"); // Output: Value: 42
                Console.WriteLine($"   Pointer value: {*pointer}"); // Output: Pointer value: 42
            } // Pointer scope limited

            // ── EXAMPLE 2: Pointer Types ───────────────────────────
            // C# supports several pointer types.

            Console.WriteLine("\n2. Pointer type declarations:");

            unsafe // unsafe = enable pointers
            {
                int integer = 100; // int = 32-bit signed
                double dbl = 3.14; // double = 64-bit float
                char ch = 'A'; // char = 16-bit unicode
                bool flag = true; // bool = true/false
                byte b = 255; // byte = 8-bit unsigned

                // Different pointer types
                int* intPtr = &integer; // int* = pointer to int
                double* dblPtr = &dbl; // double* = pointer to double
                char* charPtr = &ch; // char* = pointer to char
                bool* boolPtr = &flag; // bool* = pointer to bool
                byte* bytePtr = &b; // byte* = pointer to byte

                // Dereference to get values
                Console.WriteLine($"   int*: {*intPtr}"); // Output: int*: 100
                Console.WriteLine($"   double*: {*dblPtr}"); // Output: double*: 3.14
                Console.WriteLine($"   char*: {*charPtr}"); // Output: char*: A
                Console.WriteLine($"   bool*: {*boolPtr}"); // Output: bool*: True
                Console.WriteLine($"   byte*: {*bytePtr}"); // Output: byte*: 255
            }

            // ── EXAMPLE 3: Pointer Arithmetic ──────────────────────
            // Pointers support arithmetic operations.

            Console.WriteLine("\n3. Pointer arithmetic:");

            unsafe // unsafe = enable pointers
            {
                int[] numbers = { 10, 20, 30, 40, 50 }; // int[] = array on heap
                
                // Fixed pins array in memory (prevents GC moves)
                fixed (int* ptr = numbers) // fixed = pin array
                {
                    int* p = ptr; // int* = start of array
                    
                    Console.WriteLine($"   Initial: *p = {*p}"); // Output: Initial: *p = 10
                    
                    p++; // Increment pointer
                    Console.WriteLine($"   After p++: *p = {*p}"); // Output: After p++: *p = 20
                    
                    p += 2; // Add offset
                    Console.WriteLine($"   After p+=2: *p = {*p}"); // Output: After p+=2: *p = 40
                    
                    p--; // Decrement pointer
                    Console.WriteLine($"   After p--: *p = {*p}"); // Output: After p--: *p = 30
                }
            }

            // ── EXAMPLE 4: Stack Allocation ─────────────────────────
            // Can allocate arrays on stack (faster than heap).

            Console.WriteLine("\n4. Stack allocation:");

            unsafe // unsafe = enable pointers
            {
                // stackalloc allocates on stack, not heap
                int* stackArray = stackalloc int[5]; // int* = stack-allocated array
                
                // Initialize values
                for (int i = 0; i < 5; i++) // int = loop counter
                {
                    stackArray[i] = (i + 1) * 10; // Set value
                }
                
                // Read values
                Console.WriteLine($"   Values: {stackArray[0]}, {stackArray[1]}, {stackArray[2]}, {stackArray[3]}, {stackArray[4]}"); // Output: Values: 10, 20, 30, 40, 50
                
                // Pointer iteration
                for (int* p = stackArray; p < stackArray + 5; p++) // int* = loop pointer
                {
                    Console.WriteLine($"   *p = {*p}"); // Output each value
                }
            }

            // ── EXAMPLE 5: Null Pointer ────────────────────────────────
            // Pointers can be null.

            Console.WriteLine("\n5. Null pointer:");

            unsafe // unsafe = enable pointers
            {
                int* nullPtr = null; // null = null pointer
                
                // Check before dereference
                if (nullPtr == null) // Check for null
                {
                    Console.WriteLine("   Pointer is null"); // Output: Pointer is null
                }
                
                int value = 99; // int = regular value
                int* validPtr = &value; // int* = valid pointer
                
                if (validPtr != null) // Check not null
                {
                    Console.WriteLine($"   Pointer value: {*validPtr}"); // Output: Pointer value: 99
                }
            }

            // ── EXAMPLE 6: sizeof Operator ────────────────────────────
            // Get size of types in bytes.

            Console.WriteLine("\n6. sizeof operator:");

            unsafe // unsafe = enable pointers
            {
                Console.WriteLine($"   sizeof(int): {sizeof(int)}"); // Output: sizeof(int): 4
                Console.WriteLine($"   sizeof(double): {sizeof(double)}"); // Output: sizeof(double): 8
                Console.WriteLine($"   sizeof(char): {sizeof(char)}"); // Output: sizeof(char): 2
                Console.WriteLine($"   sizeof(long): {sizeof(long)}"); // Output: sizeof(long): 8
                Console.WriteLine($"   sizeof(byte): {sizeof(byte)}"); // Output: sizeof(byte): 1
            }

            // ── REAL-WORLD EXAMPLE: Fast Array Copy ───────────────────────
            Console.WriteLine("\n7. Real-world: Fast array copy:");

            unsafe // unsafe = enable pointers
            {
                int[] source = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }; // int[] = source array
                int[] dest = new int[10]; // int[] = destination array
                
                fixed (int* srcPtr = source) // fixed = pin source array
                fixed (int* dstPtr = dest) // fixed = pin destination array
                {
                    // Fast copy using pointers
                    for (int i = 0; i < source.Length; i++) // int = loop
                    {
                        dstPtr[i] = srcPtr[i]; // Copy element
                    }
                }
                
                Console.WriteLine($"   Copied: {string.Join(", ", dest)}"); // Output: Copied: 1, 2, 3, 4, 5, 6, 7, 8, 9, 10
            }

            Console.WriteLine("\n=== Unsafe Pointers Demo Complete ===");
#else
            Console.WriteLine("=== Unsafe Code Demo ===");
            Console.WriteLine("NOTE: Unsafe code is disabled in this build.");
            Console.WriteLine("Compile with /unsafe flag to enable.");
            Console.WriteLine("");
            Console.WriteLine("Key Concepts:");
            Console.WriteLine("- 'unsafe' keyword enables pointer operations");
            Console.WriteLine("- Pointers: int* (int), double*, byte*, etc.");
            Console.WriteLine("- & (address-of) gets pointer to variable");
            Console.WriteLine("- * (dereference) gets value at pointer");
            Console.WriteLine("- stackalloc allocates on stack");
            Console.WriteLine("- fixed pins arrays in memory");
#endif
        }
    }
}