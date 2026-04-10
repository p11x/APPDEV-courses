/*
 * ============================================================
 * TOPIC     : Interoperability
 * SUBTOPIC  : Native Memory
 * FILE      : NativeMemory_Demo.cs
 * PURPOSE   : Using native memory allocation
 * ============================================================
 */
using System; // Core System namespace
using System.Runtime.InteropServices; // Memory allocation

namespace CSharp_MasterGuide._21_Interoperability._02_Unmanaged
{
    /// <summary>
    /// Native memory demonstration
    /// </summary>
    public class NativeMemoryDemo
    {
        /// <summary>
        /// Entry point
        /// </summary>
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Native Memory ===\n");

            // Output: --- Alloc ---
            Console.WriteLine("--- Alloc ---");

            var ptr = Marshal.AllocHGlobal(100);
            Console.WriteLine($"   Allocated: {ptr}");
            // Output: Allocated: [pointer]

            // Output: --- Free ---
            Console.WriteLine("\n--- Free ---");

            Marshal.FreeHGlobal(ptr);
            Console.WriteLine("   Memory freed");
            // Output: Memory freed

            // Output: --- Struct to Ptr ---
            Console.WriteLine("\n--- Struct to Ptr ---");

            var person = new Person3 { Name = "Alice" };
            var size = Marshal.SizeOf(person);
            var structPtr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(person, structPtr, false);
            Console.WriteLine("   Struct marshaled");
            // Output: Struct marshaled

            // Output: --- Ptr to Struct ---
            Console.WriteLine("\n--- Ptr to Struct ---");

            var unmarshaled = Marshal.PtrToStructure<Person3>(structPtr);
            Console.WriteLine($"   Name: {unmarshaled.Name}");
            // Output: Name: Alice

            Marshal.FreeHGlobal(structPtr);

            Console.WriteLine("\n=== Native Memory Complete ===");
        }
    }

    /// <summary>
    /// Person struct
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Person3
    {
        public string Name; // field: name
    }
}