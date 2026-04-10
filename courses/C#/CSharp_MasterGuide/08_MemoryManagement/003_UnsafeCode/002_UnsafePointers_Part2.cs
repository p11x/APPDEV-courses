/*
 * ============================================================
 * TOPIC     : Memory Management
 * SUBTOPIC  : Unsafe Code - More Pointer Operations
 * FILE      : 02_UnsafePointers_Part2.cs
 * PURPOSE   : Advanced pointer operations, pointer to void,
 *            function pointers, and pointer arrays
 * ============================================================
 */

using System; // System namespace for Console, basic types

namespace CSharp_MasterGuide._08_MemoryManagement._03_UnsafeCode
{
    /// <summary>
    /// Demonstrates advanced pointer operations
    /// including void pointers, function pointers,
    /// and pointer arrays.
    /// </summary>
    class UnsafePointers_Part2
    {
        static void Main(string[] args)
        {
#if unsafe
            // ═══════════════════════════════════════════════════════════
            // ADVANCED POINTER TOPICS ─────────────────────────────────────
            // ═══════════════════════════════════════════════════════════
            // - void*: Generic pointer type
            // - function pointers: Pass method references
            // - pointer to pointers: int**
            // - const pointers: const int*
            // - readonly pointers: int* const

            Console.WriteLine("=== Unsafe Pointers Part 2 ===\n");

            // ── EXAMPLE 1: Void Pointer (void*) ───────────────────────
            // Void pointer can point to any type.

            Console.WriteLine("1. Void pointer (void*):");

            unsafe // unsafe = enable pointers
            {
                int intVal = 42; // int = integer value
                double dblVal = 3.14; // double = double value
                string strVal = "hello"; // string = string value
                
                // void* can point to any type
                void* voidPtr = &intVal; // void* = points to int
                Console.WriteLine($"   void* to int: {*(int*)voidPtr}"); // Output: void* to int: 42
                
                voidPtr = &dblVal; // void* = now points to double
                Console.WriteLine($"   void* to double: {*(double*)voidPtr}"); // Output: void* to double: 3.14
                
                // Can cast to any type
                voidPtr = &intVal; // void* = set to int address
                int result = *(int*)voidPtr + 8; // Cast and use
                Console.WriteLine($"   Cast and compute: {result}"); // Output: Cast and compute: 50
            }

            // ── EXAMPLE 2: Pointer to Pointer ────────────────────────
            // Can have pointers that point to other pointers.

            Console.WriteLine("\n2. Pointer to pointer:");

            unsafe // unsafe = enable pointers
            {
                int value = 100; // int = original value
                int* ptr = &value; // int* = first level pointer
                int** ptr2 = &ptr; // int** = pointer to pointer
                
                Console.WriteLine($"   value: {value}"); // Output: value: 100
                Console.WriteLine($"   *ptr: {*ptr}"); // Output: *ptr: 100
                Console.WriteLine($"   **ptr2: {**ptr2}"); // Output: **ptr2: 100
                
                // Modify through pointer to pointer
                **ptr2 = 200; // Modify value through ptr2
                Console.WriteLine($"   After **ptr2 = 200: {value}"); // Output: After **ptr2 = 200: 200
            }

            // ── EXAMPLE 3: Constant Pointers ─────────────────────────
            // const prevents modification of pointed-to value.

            Console.WriteLine("\n3. Constant pointers:");

            unsafe // unsafe = enable pointers
            {
                int value = 50; // int = value
                
                // const int* = cannot modify through this pointer
                const int* cptr = &value; // const int* = const pointer
                // *cptr = 10; // ERROR: Cannot modify
                
                Console.WriteLine($"   const value: {*cptr}"); // Output: const value: 50
                
                // int* const = pointer itself is const (cannot change)
                int* const ptrConst = &value; // int* const = const pointer variable
                *ptrConst = 60; // Can modify value
                // ptrConst = &anotherValue; // ERROR: Cannot change pointer
                
                Console.WriteLine($"   After *ptrConst = 60: {value}"); // Output: After *ptrConst = 60: 60
            }

            // ── EXAMPLE 4: Pointer Arithmetic with Different Types ──
            // sizeof determines pointer arithmetic stride.

            Console.WriteLine("\n4. Pointer arithmetic strides:");

            unsafe // unsafe = enable pointers
            {
                double[] dblArray = { 1.0, 2.0, 3.0, 4.0 }; // double[] = array
                char[] charArray = { 'A', 'B', 'C', 'D' }; // char[] = array
                
                fixed (double* d = dblArray) // fixed = pin arrays
                fixed (char* c = charArray) // fixed = pin arrays
                {
                    double* dp = d; // double* = start
                    char* cp = c; // char* = start
                    
                    Console.WriteLine($"   sizeof(double): {sizeof(double)}"); // Output: sizeof(double): 8
                    Console.WriteLine($"   sizeof(char): {sizeof(char)}"); // Output: sizeof(char): 2
                    
                    // After increment, pointer moves by sizeof(type)
                    Console.WriteLine($"   d[0]: {*dp}, d[1]: {*(dp + 1)}"); // Output: d[0]: 1, d[1]: 2
                    Console.WriteLine($"   c[0]: {*cp}, c[2]: {*(cp + 2)}"); // Output: c[0]: A, c[2]: C
                }
            }

            // ── EXAMPLE 5: Comparing Pointers ─────────────────────────
            // Can compare pointers for equality.

            Console.WriteLine("\n5. Pointer comparison:");

            unsafe // unsafe = enable pointers
            {
                int value1 = 10; // int = first value
                int value2 = 20; // int = second value
                
                int* ptr1 = &value1; // int* = first pointer
                int* ptr2 = &value2; // int* = second pointer
                
                // Compare pointers (addresses)
                bool sameAddress = (ptr1 == ptr2); // bool = addresses equal
                Console.WriteLine($"   Same address: {sameAddress}"); // Output: Same address: False
                
                // Compare values through pointers
                bool sameValue = (*ptr1 == *ptr2); // bool = values equal
                Console.WriteLine($"   Same value: {sameValue}"); // Output: Same value: False
                
                int* ptr3 = ptr1; // int* = copy pointer
                bool identical = (ptr1 == ptr3); // bool = same pointer
                Console.WriteLine($"   Identical: {identical}"); // Output: Identical: True
            }

            // ── EXAMPLE 6: Array Access via Pointers ────────────────
            // Can access arrays using pointer arithmetic.

            Console.WriteLine("\n6. Array access via pointers:");

            unsafe // unsafe = enable pointers
            {
                int[] numbers = { 5, 10, 15, 20, 25 }; // int[] = array
                
                fixed (int* ptr = numbers) // fixed = pin array
                {
                    // Pointer with index
                    Console.WriteLine($"   ptr[0] = {ptr[0]}"); // Output: ptr[0] = 5
                    Console.WriteLine($"   ptr[2] = {ptr[2]}"); // Output: ptr[2] = 15
                    Console.WriteLine($"   ptr[4] = {ptr[4]}"); // Output: ptr[4] = 25
                    
                    // Using pointer offset
                    int* p = ptr + 2; // int* = offset by 2
                    Console.WriteLine($"   *(ptr+2) = {*p}"); // Output: *(ptr+2) = 15
                    
                    // Loop through array
                    Console.Write("   All values: "); // Output prefix
                    for (int i = 0; i < 5; i++) // int = loop
                    {
                        Console.Write($"{ptr[i]} "); // Output each value
                    }
                    Console.WriteLine(); // New line
                }
            }

            // ── EXAMPLE 7: Function Pointers (C# 9+) ───────────────────
            // Function pointers for unmanaged code interop.

            Console.WriteLine("\n7. Function pointers:");

            unsafe // unsafe = enable function pointers
            {
                // delegate* unmanaged<Cdecl> for C-style calls
                // This is a simple example
                delegate*<int, int, int> addFunc = &AddNumbers; // delegate* = function pointer
                
                int result = addFunc(10, 20); // Call via pointer
                Console.WriteLine($"   Function result: {result}"); // Output: Function result: 30
            }

            // ── REAL-WORLD EXAMPLE: Memory Operations ───────────────
            Console.WriteLine("\n8. Real-world: Memory compare:");

            unsafe // unsafe = enable pointers
            {
                byte[] buffer1 = { 0x01, 0x02, 0x03, 0x04 }; // byte[] = first buffer
                byte[] buffer2 = { 0x01, 0x02, 0x03, 0x05 }; // byte[] = second buffer
                
                fixed (byte* b1 = buffer1) // fixed = pin first buffer
                fixed (byte* b2 = buffer2) // fixed = pin second buffer
                {
                    bool identical = CompareMemory(b1, b2, 4); // Compare memory
                    Console.WriteLine($"   Buffers identical: {identical}"); // Output: Buffers identical: False
                    
                    // Make them identical
                    buffer2[3] = 0x04; // byte = fix buffer
                    identical = CompareMemory(b1, b2, 4); // Compare again
                    Console.WriteLine($"   After fix identical: {identical}"); // Output: After fix identical: True
                }
            }

            Console.WriteLine("\n=== Unsafe Pointers Part 2 Complete ===");
#else
            Console.WriteLine("=== Unsafe Pointers Part 2 ===");
            Console.WriteLine("NOTE: Unsafe code is disabled in this build.");
            Console.WriteLine("Compile with /unsafe flag to enable.");
            Console.WriteLine("");
            Console.WriteLine("Advanced Topics:");
            Console.WriteLine("- void*: Generic/any pointer type");
            Console.WriteLine("- int**: Pointer to pointer");
            Console.WriteLine("- const int*: Cannot modify value");
            Console.WriteLine("- int* const: Cannot change address");
            Console.WriteLine("- Pointer arithmetic by sizeof(type)");
            Console.WriteLine("- Function pointers for interop");
#endif
        }

#if unsafe
        // Helper function for function pointer example
        static int AddNumbers(int a, int b) // int = add two numbers
        {
            return a + b; // int = return sum
        }

        // Memory compare function
        static bool CompareMemory(byte* ptr1, byte* ptr2, int size) // bool = compare bytes
        {
            for (int i = 0; i < size; i++) // int = loop
            {
                if (ptr1[i] != ptr2[i]) // Check bytes
                {
                    return false; // bool = differ
                }
            }
            return true; // bool = identical
        }
#endif
    }
}