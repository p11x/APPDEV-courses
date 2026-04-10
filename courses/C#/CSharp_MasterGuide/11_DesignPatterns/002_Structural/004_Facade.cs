/*
 * ============================================================
 * TOPIC     : Design Patterns
 * SUBTOPIC  : Structural - Facade Pattern
 * FILE      : 08_Facade.cs
 * PURPOSE   : Demonstrates Facade design pattern in C#
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._11_DesignPatterns._02_Structural
{
    /// <summary>
    /// Demonstrates Facade pattern
    /// </summary>
    public class FacadePattern
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Facade Pattern ===\n");

            Console.WriteLine("1. Facade - Simplified Interface:");
            var computer = new ComputerFacade();
            computer.Start();
            // Output: CPU started
            // Output: Memory loaded
            // Output: Disk initialized
            // Output: Display ready

            Console.WriteLine("\n=== Facade Complete ===");
        }
    }

    public class CPU { public void Start() => Console.WriteLine("   CPU started"); public void Execute() { } }
    public class Memory { public void Load() => Console.WriteLine("   Memory loaded"); }
    public class Disk { public void Initialize() => Console.WriteLine("   Disk initialized"); }
    public class Display { public void Ready() => Console.WriteLine("   Display ready"); }

    public class ComputerFacade
    {
        private CPU _cpu = new();
        private Memory _memory = new();
        private Disk _disk = new();
        private Display _display = new();
        
        public void Start()
        {
            _cpu.Start();
            _memory.Load();
            _disk.Initialize();
            _display.Ready();
        }
    }
}