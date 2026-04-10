/*
 * ============================================================
 * TOPIC     : Interoperability
 * SUBTOPIC  : P/Invoke Basics
 * FILE      : 01_PInvoke_Basics.cs
 * PURPOSE   : Platform Invoke for native code
 * ============================================================
 */
using System;
using System.Runtime.InteropServices;

namespace CSharp_MasterGuide._21_Interoperability._01_PInvoke
{
    /// <summary>
    /// P/Invoke basics
    /// </summary>
    public class PInvokeBasics
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== P/Invoke Basics ===\n");

            // Call native function
            var result = GetSystemTime();
            Console.WriteLine($"   System time: {result}");
            
            // File operations
            var fileResult = CreateFile("test.txt");
            Console.WriteLine($"   File handle: {fileResult}");

            Console.WriteLine("\n=== P/Invoke Complete ===");
        }

        /// <summary>
        /// Import kernel32 GetSystemTime
        /// </summary>
        [DllImport("kernel32.dll")]
        static extern uint GetSystemTime();

        /// <summary>
        /// Import kernel32 CreateFile
        /// </summary>
        [DllImport("kernel32.dll")]
        static extern IntPtr CreateFile(string lpFileName);
    }
}