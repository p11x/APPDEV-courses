/*
 * ============================================================
 * TOPIC     : Design Patterns
 * SUBTOPIC  : Behavioral - Visitor Pattern
 * FILE      : 01_Visitor.cs
 * PURPOSE   : Demonstrates Visitor design pattern in C#
 * ============================================================
 */
using System;

namespace CSharp_MasterGuide._11_DesignPatterns._03_Behavioral._06_Visitor
{
    /// <summary>
    /// Demonstrates Visitor pattern - operations on object structure
    /// </summary>
    public class VisitorPattern
    {
        /// <summary>
        /// Entry point for Visitor examples
        /// </summary>
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Visitor Pattern Demo ===\n");

            // Example: Shape Visitor
            Console.WriteLine("1. Shape Operations:");
            var shapes = new ShapeElement[] 
            { 
                new Circle(5), 
                new Rectangle(4, 6) 
            };
            
            var areaVisitor = new AreaVisitor();
            foreach (var shape in shapes)
            {
                shape.Accept(areaVisitor);
            }
            
            // Output: Circle area: 78.54, Rectangle area: 24

            Console.WriteLine("\n=== Visitor Complete ===");
        }
    }

    /// <summary>
    /// Element interface - accepts visitor
    /// </summary>
    public interface IShapeElement
    {
        void Accept(IShapeVisitor visitor);
    }

    /// <summary>
    /// Visitor interface - declares visit methods
    /// </summary>
    public interface IShapeVisitor
    {
        void VisitCircle(Circle circle);
        void VisitRectangle(Rectangle rectangle);
    }

    /// <summary>
    /// Circle element
    /// </summary>
    public class Circle : IShapeElement
    {
        public double Radius { get; }
        
        public Circle(double radius) => Radius = radius;
        
        public void Accept(IShapeVisitor visitor) => visitor.VisitCircle(this);
    }

    /// <summary>
    /// Rectangle element
    /// </summary>
    public class Rectangle : IShapeElement
    {
        public double Width { get; }
        public double Height { get; }
        
        public Rectangle(double width, double height)
        {
            Width = width;
            Height = height;
        }
        
        public void Accept(IShapeVisitor visitor) => visitor.VisitRectangle(this);
    }

    /// <summary>
    /// Area calculation visitor
    /// </summary>
    public class AreaVisitor : IShapeVisitor
    {
        public void VisitCircle(Circle circle)
        {
            var area = Math.PI * circle.Radius * circle.Radius;
            Console.WriteLine($"   Circle area: {area:F2}");
        }
        
        public void VisitRectangle(Rectangle rectangle)
        {
            var area = rectangle.Width * rectangle.Height;
            Console.WriteLine($"   Rectangle area: {area}");
        }
    }
}