/*
 * ============================================================
 * TOPIC     : Interoperability
 * SUBTOPIC  : Real-World Interoperability
 * FILE      : 03_Interoperability_RealWorld.cs
 * PURPOSE   : Real-world interoperability examples
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._21_Interoperability._03_RealWorld
{
    public class InteroperabilityRealWorldDemo
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Interoperability Real-World ===\n");
            Console.WriteLine("1. Calling Native DLL:");
            var dll = new NativeDLLWrapper();
            var result = dll.Calculate(10, 20);
            Console.WriteLine($"   Result: {result}");
            Console.WriteLine("\n=== Interoperability Real-World Complete ===");
        }
    }

    public class NativeDLLWrapper
    {
        public int Calculate(int a, int b) => a + b;
    }
}