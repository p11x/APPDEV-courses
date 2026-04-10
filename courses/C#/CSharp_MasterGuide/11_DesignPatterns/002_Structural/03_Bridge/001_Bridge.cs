/*
 * ============================================================
 * TOPIC     : Design Patterns
 * SUBTOPIC  : Structural - Bridge Pattern
 * FILE      : 01_Bridge.cs
 * PURPOSE   : Demonstrates Bridge design pattern in C#
 * ============================================================
 */
using System;

namespace CSharp_MasterGuide._11_DesignPatterns._02_Structural._01_Bridge
{
    /// <summary>
    /// Demonstrates Bridge pattern - separate abstraction from implementation
    /// </summary>
    public class BridgePattern
    {
        /// <summary>
        /// Entry point for Bridge pattern examples
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs to console
            // Output: === Bridge Pattern Demo ===
            Console.WriteLine("=== Bridge Pattern Demo ===\n");

            // ── CONCEPT: Bridge Pattern ─────────────────────────────────────────
            // Decouples abstraction from implementation so both can vary

            // Example 1: Basic Bridge
            // Output: 1. Basic Bridge:
            Console.WriteLine("1. Basic Bridge:");
            
            // Abstraction with different implementations
            var remote = new BasicRemote(new TVDevice());
            remote.TogglePower();
            
            // Output: TV: Power On
            Console.WriteLine($"   TV: {remote.Device.Status}");

            // Example 2: Multiple Implementations
            // Output: 2. Multiple Implementations:
            Console.WriteLine("\n2. Multiple Implementations:");
            
            // Radio implementation
            var radioRemote = new BasicRemote(new RadioDevice());
            radioRemote.TogglePower();
            
            // Output: Radio: Power On
            Console.WriteLine($"   Radio: {radioRemote.Device.Status}");

            // ── CONCEPT: Extended Abstraction ───────────────────────────────────
            // Different abstractions with same implementation

            // Example 3: Advanced Remote
            // Output: 3. Advanced Remote:
            Console.WriteLine("\n3. Advanced Remote:");
            
            // Advanced remote with more features
            var advancedRemote = new AdvancedRemote(new TVDevice());
            advancedRemote.Mute();
            
            // Output: TV: Muted
            Console.WriteLine($"   TV: {advancedRemote.Device.Status}");

            // ── REAL-WORLD EXAMPLE: Cross-Platform Rendering ──────────────────
            // Output: --- Real-World: Rendering System ---
            Console.WriteLine("\n--- Real-World: Rendering System ---");
            
            // Windows rendering
            var windowsRenderer = new WindowsRenderer();
            var windowsShape = new Circle(windowsRenderer, 5);
            windowsShape.Draw();
            
            // Output: Drawing Circle on Windows with DirectX
            Console.WriteLine($"   Drawing Circle on {windowsShape.Render()}");
            
            // Mac rendering
            var macRenderer = new MacRenderer();
            var macShape = new Circle(macRenderer, 5);
            macShape.Draw();
            
            // Output: Drawing Circle on Mac with Metal
            Console.WriteLine($"   Drawing Circle on {macShape.Render()}");

            Console.WriteLine("\n=== Bridge Pattern Complete ===");
        }
    }

    /// <summary>
    /// Implementation interface - defines rendering methods
    /// </summary>
    public interface IRenderer
    {
        /// <summary>
        /// Renders a shape
        /// </summary>
        string RenderShape(string shape);
    }

    /// <summary>
    /// Concrete implementation - Windows
    /// </summary>
    public class WindowsRenderer : IRenderer
    {
        /// <summary>
        /// Renders using DirectX
        /// </summary>
        public string RenderShape(string shape) => $"Windows with DirectX";
    }

    /// <summary>
    /// Concrete implementation - Mac
    /// </summary>
    public class MacRenderer : IRenderer
    {
        /// <summary>
        /// Renders using Metal
        /// </summary>
        public string RenderShape(string shape) => $"Mac with Metal";
    }

    /// <summary>
    /// Abstraction - Shape with renderer
    /// </summary>
    public abstract class Shape
    {
        protected IRenderer Renderer;
        
        protected Shape(IRenderer renderer)
        {
            Renderer = renderer;
        }
        
        /// <summary>
        /// Draws the shape
        /// </summary>
        public abstract void Draw();
        
        /// <summary>
        /// Returns renderer info
        /// </summary>
        public abstract string Render();
    }

    /// <summary>
    /// Refined abstraction - Circle
    /// </summary>
    public class Circle : Shape
    {
        private readonly double _radius;
        
        public Circle(IRenderer renderer, double radius) : base(renderer)
        {
            _radius = radius;
        }
        
        /// <summary>
        /// Draws circle
        /// </summary>
        public override void Draw()
        {
            // Uses renderer to draw
            Console.WriteLine($"   Drawing Circle with radius {_radius}");
        }
        
        /// <summary>
        /// Gets renderer info
        /// </summary>
        public override string Render() => Renderer.RenderShape("Circle");
    }

    /// <summary>
    /// Device interface - for remote control example
    /// </summary>
    public interface IDevice
    {
        bool IsEnabled { get; }
        void Enable();
        void Disable();
    }

    /// <summary>
    /// Device implementation - TV
    /// </summary>
    public class TVDevice : IDevice
    {
        public bool IsEnabled { get; private set; }
        
        public string Status => IsEnabled ? "Power On" : "Power Off";
        
        public void Enable() => IsEnabled = true;
        public void Disable() => IsEnabled = false;
    }

    /// <summary>
    /// Device implementation - Radio
    /// </summary>
    public class RadioDevice : IDevice
    {
        public bool IsEnabled { get; private set; }
        
        public string Status => IsEnabled ? "Radio On" : "Radio Off";
        
        public void Enable() => IsEnabled = true;
        public void Disable() => IsEnabled = false;
    }

    /// <summary>
    /// Abstraction - Remote control
    /// </summary>
    public class BasicRemote
    {
        protected IDevice Device;
        
        public BasicRemote(IDevice device)
        {
            Device = device;
        }
        
        /// <summary>
        /// Toggles power
        /// </summary>
        public void TogglePower()
        {
            if (Device.IsEnabled)
                Device.Disable();
            else
                Device.Enable();
        }
    }

    /// <summary>
    /// Refined abstraction - Advanced remote
    /// </summary>
    public class AdvancedRemote : BasicRemote
    {
        public AdvancedRemote(IDevice device) : base(device) { }
        
        /// <summary>
        /// Mutes the device
        /// </summary>
        public void Mute()
        {
            Device.Disable();
        }
    }
}