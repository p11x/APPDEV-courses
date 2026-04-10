/*
 * ============================================================
 * TOPIC     : SOLID Principles
 * SUBTOPIC  : Liskov Substitution Principle
 * FILE      : 03_LiskovSubstitution.cs
 * PURPOSE   : Demonstrates LSP - subtypes must be substitutable
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._12_SOLID_Principles._03_LiskovSubstitution
{
    /// <summary>
    /// Demonstrates Liskov Substitution Principle
    /// </summary>
    public class LiskovSubstitutionDemo
    {
        /// <summary>
        /// Entry point for LSP examples
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs to console
            // Output: === Liskov Substitution Principle ===
            Console.WriteLine("=== Liskov Substitution Principle ===\n");

            // ── CONCEPT: What is LSP? ────────────────────────────────────────
            // Objects of superclass should be replaceable with subclasses

            // Example 1: Violating LSP
            // Output: 1. Violating LSP:
            Console.WriteLine("1. Violating LSP:");
            
            // Square inherits from Rectangle but breaks expectations
            var rectangle = new BadRectangle(10, 5);
            var area = rectangle.GetArea();
            // Output: Rectangle area: 50
            Console.WriteLine($"   Rectangle area: {area}");
            
            // If we use a Square where Rectangle is expected
            var square = new BadSquare(10);
            // square.Width = 5 would break height assumption
            // Output: Square area: 100 (not 50!)
            Console.WriteLine($"   Square area: {square.GetArea()}");

            // Example 2: Following LSP
            // Output: 2. Following LSP:
            Console.WriteLine("\n2. Following LSP:");
            
            // Use interface - any shape can be substituted
            var shapes = new IShape[]
            {
                new Rectangle(10, 5),
                new Circle(5),
                new Triangle(10, 5)
            };
            
            foreach (var shape in shapes)
            {
                var area = shape.Area();
                // Output: Rectangle: 50, Circle: 78.54, Triangle: 25
                Console.WriteLine($"   {shape.GetType().Name}: {area:F2}");
            }

            // ── CONCEPT: Behavioral Subtyping ───────────────────────────────
            // Subclass must honor base class contract

            // Example 3: Behavioral Subtyping
            // Output: 3. Behavioral Subtyping:
            Console.WriteLine("\n3. Behavioral Subtyping:");
            
            var birds = new List<IBird>
            {
                new Sparrow(),
                new Penguin() // Cannot fly - but implements interface properly
            };
            
            foreach (var bird in birds)
            {
                bird.Fly(); // Penguins handle this gracefully
                bird.Eat();
            }
            // Output: Sparrow flying
            // Output: Sparrow eating
            // Output: Penguin cannot fly, but eats

            Console.WriteLine("\n=== LSP Complete ===");
        }
    }

    /// <summary>
    /// BAD: Square violates LSP
    /// </summary>
    public class BadRectangle
    {
        public virtual int Width { get; set; }
        public virtual int Height { get; set; }
        
        public BadRectangle(int width, int height)
        {
            Width = width;
            Height = height;
        }
        
        public int GetArea() => Width * Height;
    }

    /// <summary>
    /// BAD: Square breaks rectangle's contract
    /// </summary>
    public class BadSquare : BadRectangle
    {
        public BadSquare(int side) : base(side, side) { }
        
        public override int Width
        {
            get => base.Width;
            set { base.Width = value; base.Height = value; }
        }
        
        public override int Height
        {
            get => base.Height;
            set { base.Width = value; base.Height = value; }
        }
    }

    /// <summary>
    /// GOOD: Shape interface
    /// </summary>
    public interface IShape
    {
        double Area(); // method: calculates area
    }

    /// <summary>
    /// Rectangle - proper implementation
    /// </summary>
    public class Rectangle : IShape
    {
        public double Width { get; set; }
        public double Height { get; set; }
        
        public Rectangle(double width, double height)
        {
            Width = width;
            Height = height;
        }
        
        public double Area() => Width * Height;
    }

    /// <summary>
    /// Circle - proper implementation
    /// </summary>
    public class Circle : IShape
    {
        public double Radius { get; }
        
        public Circle(double radius) => Radius = radius;
        
        public double Area() => Math.PI * Radius * Radius;
    }

    /// <summary>
    /// Triangle - proper implementation
    /// </summary>
    public class Triangle : IShape
    {
        public double Base { get; }
        public double Height { get; }
        
        public Triangle(double @base, double height)
        {
            Base = @base;
            Height = height;
        }
        
        public double Area() => 0.5 * Base * Height;
    }

    /// <summary>
    /// Bird interface
    /// </summary>
    public interface IBird
    {
        void Fly(); // method: bird flies
        void Eat(); // method: bird eats
    }

    /// <summary>
    /// Sparrow - can fly
    /// </summary>
    public class Sparrow : IBird
    {
        public void Fly() => Console.WriteLine("   Sparrow flying");
        public void Eat() => Console.WriteLine("   Sparrow eating");
    }

    /// <summary>
    /// Penguin - cannot fly but honors contract
    /// </summary>
    public class Penguin : IBird
    {
        public void Fly() => Console.WriteLine("   Penguin cannot fly, but swims");
        public void Eat() => Console.WriteLine("   Penguin eating");
    }
}