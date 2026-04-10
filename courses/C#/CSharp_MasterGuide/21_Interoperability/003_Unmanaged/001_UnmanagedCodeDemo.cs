/*
 * ============================================================
 * TOPIC     : Interoperability
 * SUBTOPIC  : Unmanaged Code
 * FILE      : 02_UnmanagedCodeDemo.cs
 * PURPOSE   : Demonstrates unmanaged code interop in C#
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._21_Interoperability._02_Unmanaged
{
    public class UnmanagedCodeDemo
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Unmanaged Code Demo ===\n");
            Console.WriteLine("1. P/Invoke:");
            var native = new NativeLib();
            native.CallExternalFunction();
            Console.WriteLine("\n=== Unmanaged Code Complete ===");
        }
    }

    public class NativeLib
    {
        public void CallExternalFunction() => Console.WriteLine("   Native function called");
    }
}