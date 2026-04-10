/*
 * ============================================================
 * TOPIC     : Design Patterns
 * SUBTOPIC  : Structural - Real-World Part 2
 * FILE      : 13_Structural_RealWorld_Part2.cs
 * PURPOSE   : More real-world Structural pattern examples
 * ============================================================
 */
using System;
using System.Collections.Generic;

namespace CSharp_MasterGuide._11_DesignPatterns._02_Structural._07_RealWorld
{
    /// <summary>
    /// More real-world Structural pattern examples
    /// </summary>
    public class StructuralRealWorldPart2
    {
        /// <summary>
        /// Entry point
        /// </summary>
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Structural Patterns Real-World Part 2 ===\n");

            // Example: Shape Rendering with Bridge
            Console.WriteLine("1. Shape Rendering (Bridge):");
            var pdfRenderer = new PdfRenderer();
            var vectorShape = new Rectangle(pdfRenderer);
            vectorShape.Draw();
            
            // Output: Rectangle rendered as PDF

            // Example: Tree Structure with Composite
            Console.WriteLine("\n2. Organization Structure (Composite):");
            var company = new OrganizationUnit("Tech Corp");
            var engineering = new OrganizationUnit("Engineering");
            engineering.Add(new Employee("Alice", "Developer"));
            engineering.Add(new Employee("Bob", "Developer"));
            company.Add(engineering);
            company.Add(new Employee("Charlie", "CEO"));
            
            company.Print("");
            
            // Output: Tech Corp -> Engineering -> Alice, Bob, Charlie

            Console.WriteLine("\n=== Structural Real-World Part 2 Complete ===");
        }
    }

    /// <summary>
    /// Renderer interface
    /// </summary>
    public interface IRenderEngine
    {
        string Render(string shape);
    }

    /// <summary>
    /// PDF renderer
    /// </summary>
    public class PdfRenderer : IRenderEngine
    {
        public string Render(string shape) => $"{shape} rendered as PDF";
    }

    /// <summary>
    /// SVG renderer
    /// </summary>
    public class SvgRenderer : IRenderEngine
    {
        public string Render(string shape) => $"{shape} rendered as SVG";
    }

    /// <summary>
    /// Shape abstraction
    /// </summary>
    public abstract class Shape
    {
        protected IRenderEngine Renderer;
        
        protected Shape(IRenderEngine renderer) => Renderer = renderer;
        
        public abstract void Draw();
    }

    /// <summary>
    /// Rectangle shape
    /// </summary>
    public class Rectangle : Shape
    {
        public Rectangle(IRenderEngine renderer) : base(renderer) { }
        
        public override void Draw() => Console.WriteLine($"   {Renderer.Render("Rectangle")}");
    }

    /// <summary>
    /// Organization unit interface
    /// </summary>
    public interface IOrganizationUnit
    {
        void Print(string indent);
    }

    /// <summary>
    /// Employee - leaf
    /// </summary>
    public class Employee : IOrganizationUnit
    {
        public string Name { get; }
        public string Role { get; }
        
        public Employee(string name, string role)
        {
            Name = name;
            Role = role;
        }
        
        public void Print(string indent) => 
            Console.WriteLine($"{indent}{Name} - {Role}");
    }

    /// <summary>
    /// Department - composite
    /// </summary>
    public class OrganizationUnit : IOrganizationUnit
    {
        public string Name { get; }
        private readonly List<IOrganizationUnit> _members = new();
        
        public OrganizationUnit(string name) => Name = name;
        
        public void Add(IOrganizationUnit unit) => _members.Add(unit);
        
        public void Print(string indent)
        {
            Console.WriteLine($"{indent}{Name}/");
            foreach (var member in _members)
                member.Print(indent + "  ");
        }
    }
}