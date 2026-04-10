/*
 * ============================================================
 * TOPIC     : Interoperability
 * SUBTOPIC  : COM Interop
 * FILE      : 01_COMInteropDemo.cs
 * PURPOSE   : Demonstrates COM interoperability in C#
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._21_Interoperability._01_COM
{
    public class COMInteropDemo
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== COM Interop Demo ===\n");
            Console.WriteLine("1. COM Component Access:");
            var com = new COMObject();
            com.InvokeMethod();
            Console.WriteLine("\n=== COM Interop Complete ===");
        }
    }

    public class COMObject
    {
        public void InvokeMethod() => Console.WriteLine("   COM method invoked");
    }
}