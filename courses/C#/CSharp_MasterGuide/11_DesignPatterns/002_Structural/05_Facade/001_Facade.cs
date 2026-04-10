/*
 * ============================================================
 * TOPIC     : Design Patterns
 * SUBTOPIC  : Structural - Facade Pattern
 * FILE      : 01_Facade.cs
 * PURPOSE   : Demonstrates Facade design pattern in C#
 * ============================================================
 */
using System;

namespace CSharp_MasterGuide._11_DesignPatterns._02_Structural._05_Facade
{
    /// <summary>
    /// Demonstrates Facade pattern - simple interface to complex system
    /// </summary>
    public class FacadePattern
    {
        /// <summary>
        /// Entry point for Facade examples
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs to console
            // Output: === Facade Pattern Demo ===
            Console.WriteLine("=== Facade Pattern Demo ===\n");

            // ── CONCEPT: Facade Pattern ─────────────────────────────────────────
            // Provide unified interface to a set of interfaces

            // Example 1: Home Theater Facade
            // Output: 1. Home Theater:
            Console.WriteLine("1. Home Theater:");
            
            var homeTheater = new HomeTheaterFacade();
            homeTheater.WatchMovie("Inception");
            
            // Output: Lights dimmed, Projector on, Sound system configured

            // Example 2: Computer Startup
            // Output: 2. Computer Startup:
            Console.WriteLine("\n2. Computer Startup:");
            
            var computer = new ComputerFacade();
            computer.Start();
            
            // Output: BIOS check, Boot loader, OS loaded

            // ── REAL-WORLD EXAMPLE: Order Processing ───────────────────────────
            // Output: --- Real-World: Order Processing ---
            Console.WriteLine("\n--- Real-World: Order Processing ---");
            
            var orderSystem = new OrderProcessingFacade();
            orderSystem.ProcessOrder("ORDER-001");
            
            // Output: Order ORDER-001 processed successfully

            Console.WriteLine("\n=== Facade Pattern Complete ===");
        }
    }

    /// <summary>
    /// Subsystem - Amplifier
    /// </summary>
    public class Amplifier
    {
        public void On() => Console.WriteLine("   Amplifier on");
        public void SetVolume(int level) => Console.WriteLine($"   Volume set to {level}");
    }

    /// <summary>
    /// Subsystem - Projector
    /// </summary>
    public class Projector
    {
        public void On() => Console.WriteLine("   Projector on");
        public void SetInput(string input) => Console.WriteLine($"   Input: {input}");
    }

    /// <summary>
    /// Subsystem - Lights
    /// </summary>
    public class Lights
    {
        public void Dim(int level) => Console.WriteLine($"   Lights dimmed to {level}%");
    }

    /// <summary>
    /// Home Theater Facade
    /// </summary>
    public class HomeTheaterFacade
    {
        private readonly Amplifier _amp;
        private readonly Projector _projector;
        private readonly Lights _lights;
        
        public HomeTheaterFacade()
        {
            _amp = new Amplifier();
            _projector = new Projector();
            _lights = new Lights();
        }
        
        /// <summary>
        /// Watch movie - simple interface to complex subsystem
        /// </summary>
        public void WatchMovie(string movie)
        {
            _lights.Dim(20);
            _projector.On();
            _projector.SetInput("DVD");
            _amp.On();
            _amp.SetVolume(10);
            Console.WriteLine($"   Playing: {movie}");
        }
    }

    /// <summary>
    /// Subsystem - BIOS
    /// </summary>
    public class Bios
    {
        public void Post() => Console.WriteLine("   BIOS POST check");
    }

    /// <summary>
    /// Subsystem - OS
    /// </summary>
    public class OperatingSystem
    {
        public void Boot() => Console.WriteLine("   OS loaded");
    }

    /// <summary>
    /// Computer Facade
    /// </summary>
    public class ComputerFacade
    {
        private readonly Bios _bios;
        private readonly OperatingSystem _os;
        
        public ComputerFacade()
        {
            _bios = new Bios();
            _os = new OperatingSystem();
        }
        
        /// <summary>
        /// Start computer - simplified interface
        /// </summary>
        public void Start()
        {
            _bios.Post();
            _os.Boot();
        }
    }

    /// <summary>
    /// Order Processing Facade
    /// </summary>
    public class OrderProcessingFacade
    {
        /// <summary>
        /// Process order - hides all complexity
        /// </summary>
        public void ProcessOrder(string orderId)
        {
            ValidateOrder(orderId);
            CheckInventory(orderId);
            ProcessPayment(orderId);
            ShipOrder(orderId);
            
            Console.WriteLine($"   Order {orderId} processed successfully");
        }
        
        private void ValidateOrder(string id) { }
        private void CheckInventory(string id) { }
        private void ProcessPayment(string id) { }
        private void ShipOrder(string id) { }
    }
}