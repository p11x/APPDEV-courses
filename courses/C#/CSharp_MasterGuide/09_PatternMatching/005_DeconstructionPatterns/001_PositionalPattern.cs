/*
 * ============================================================
 * TOPIC     : Pattern Matching
 * SUBTOPIC  : Deconstruction Patterns
 * FILE      : 01_PositionalPattern.cs
 * PURPOSE   : Demonstrates positional patterns using Deconstruct methods
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._09_PatternMatching._05_DeconstructionPatterns
{
    /// <summary>
    /// Demonstrates positional pattern matching with Deconstruct
    /// </summary>
    public class PositionalPattern
    {
        /// <summary>
        /// Entry point for positional pattern examples
        /// </summary>
        public static void Main(string[] args)
        {
            // Output: === Positional Pattern Matching Demo ===
            Console.WriteLine("=== Positional Pattern Matching Demo ===\n");

            // ── CONCEPT: Deconstruct Methods ────────────────────────────────
            // Deconstruct allows tuple-like extraction from objects

            // Example 1: Basic positional pattern
            // Output: 1. Basic Positional Patterns:
            Console.WriteLine("1. Basic Positional Patterns:");
            
            // Point has Deconstruct that returns (X, Y)
            var point1 = new Point(10, 20);
            var point2 = new Point(0, 0);
            var point3 = new Point(-5, 15);
            
            // DescribePoint returns description
            Console.WriteLine($"   (10, 20): {DescribePoint(point1)}");
            Console.WriteLine($"   (0, 0): {DescribePoint(point2)}");
            Console.WriteLine($"   (-5, 15): {DescribePoint(point3)}");

            // ── CONCEPT: Positional Pattern with Type ───────────────────────
            // Combine with type checking

            // Example 2: Positional with type pattern
            // Output: 2. Positional with Type Pattern:
            Console.WriteLine("\n2. Positional with Type Pattern:");
            
            // Handle different shapes with Deconstruct
            var circle = new Circle(5);
            var rectangle = new Rectangle(10, 20);
            var line = new Line(0, 0, 10, 10);
            
            // GetShapeDescription returns description
            Console.WriteLine($"   Circle(r=5): {GetShapeDescription(circle)}");
            Console.WriteLine($"   Rectangle(10x20): {GetShapeDescription(rectangle)}");
            Console.WriteLine($"   Line(0,0 to 10,10): {GetShapeDescription(line)}");

            // ── CONCEPT: Record Positional Patterns ──────────────────────────
            // Records automatically have positional Deconstruct

            // Example 3: Record positional patterns
            // Output: 3. Record Positional Patterns:
            Console.WriteLine("\n3. Record Positional Patterns:");
            
            // PersonRecord has positional constructor
            var person1 = new PersonRecord("Alice", 30, "Engineer");
            var person2 = new PersonRecord("Bob", 25, "Sales");
            var person3 = new PersonRecord("Charlie", 45, "Manager");
            
            // GetPersonRole returns role info
            Console.WriteLine($"   {person1.Name}: {GetPersonRole(person1)}");
            Console.WriteLine($"   {person2.Name}: {GetPersonRole(person2)}");
            Console.WriteLine($"   {person3.Name}: {GetPersonRole(person3)}");

            // ── CONCEPT: Tuple Positional Patterns ──────────────────────────
            // Works with tuples directly

            // Example 4: Tuple positional patterns
            // Output: 4. Tuple Positional Patterns:
            Console.WriteLine("\n4. Tuple Positional Patterns:");
            
            // (int, int) = tuple coordinates
            var coord1 = (10, 20);
            var coord2 = (0, 0);
            var coord3 = (50, 50);
            
            // GetCoordType returns type
            Console.WriteLine($"   {coord1}: {GetCoordType(coord1)}");
            Console.WriteLine($"   {coord2}: {GetCoordType(coord2)}");
            Console.WriteLine($"   {coord3}: {GetCoordType(coord3)}");

            // ── CONCEPT: Nested Positional Patterns ─────────────────────────
            // Deconstruct within Deconstruct

            // Example 5: Nested positional patterns
            // Output: 5. Nested Positional Patterns:
            Console.WriteLine("\n5. Nested Positional Patterns:");
            
            // Rectangle with center point
            var rect1 = new RectangleWithCenter(0, 0, 10, 10);  // center at (5, 5)
            var rect2 = new RectangleWithCenter(10, 10, 20, 20); // center at (15, 15)
            
            // GetRectangleRegion returns region
            Console.WriteLine($"   Rect(0,0,10,10): {GetRectangleRegion(rect1)}");
            Console.WriteLine($"   Rect(10,10,20,20): {GetRectangleRegion(rect2)}");

            Console.WriteLine("\n=== Positional Pattern Complete ===");
        }

        /// <summary>
        /// Describes point using positional pattern
        /// </summary>
        public static string DescribePoint(Point point)
        {
            // Positional pattern: match (X, Y) from Deconstruct
            return point switch
            {
                // (0, 0) = origin
                (0, 0) => "Origin",
                
                // X = 0 = on Y axis
                (0, var y) => $"On Y-axis at Y={y}",
                
                // Y = 0 = on X axis
                (var x, 0) => $"On X-axis at X={x}",
                
                // Positive quadrant
                (var x, var y) when x > 0 && y > 0 => $"Quadrant I ({x}, {y})",
                
                // Default
                (var x, var y) => $"Point ({x}, {y})"
            };
        }

        /// <summary>
        /// Gets shape description using positional pattern
        /// </summary>
        public static string GetShapeDescription(object shape)
        {
            // Type pattern with positional matching
            return shape switch
            {
                // Circle has Deconstruct returning (Radius)
                Circle(var radius) => $"Circle with radius {radius}",
                
                // Rectangle returns (Width, Height)
                Rectangle(var width, var height) => $"Rectangle {width}x{height}",
                
                // Line returns (X1, Y1, X2, Y2)
                Line(var x1, var y1, var x2, var y2) => $"Line from ({x1},{y1}) to ({x2},{y2})",
                
                // Default
                _ => "Unknown shape"
            };
        }

        /// <summary>
        /// Gets person role using record positional pattern
        /// </summary>
        public static string GetPersonRole(PersonRecord person)
        {
            // Record automatically has Deconstruct for positional params
            return person switch
            {
                // Manager with high salary = executive
                ("Alice", 30, "Engineer") => "Senior Engineer",
                
                // Sales person = sales team
                (_, _, "Sales") => "Sales Team",
                
                // Manager = management
                (_, _, "Manager") => "Management",
                
                // Default = staff
                (var name, var age, var role) => $"{role} at age {age}"
            };
        }

        /// <summary>
        /// Gets coordinate type using tuple positional pattern
        /// </summary>
        public static string GetCoordType((int X, int Y) coord)
        {
            // Tuple already has positional structure
            return coord switch
            {
                // Origin
                (0, 0) => "Origin",
                
                // On axes
                (0, _) => "Y-axis",
                (_, 0) => "X-axis",
                
                // Quadrants
                (> 0, > 0) => "Quadrant I",
                (< 0, > 0) => "Quadrant II",
                (< 0, < 0) => "Quadrant III",
                (> 0, < 0) => "Quadrant IV"
            };
        }

        /// <summary>
        /// Gets rectangle region using nested positional pattern
        /// </summary>
        public static string GetRectangleRegion(RectangleWithCenter rect)
        {
            // Deconstruct returns (X1, Y1, X2, Y2), compute center internally
            return rect switch
            {
                // Center in first quadrant (positive coordinates)
                RectangleWithCenter(var x1, var y1, var x2, var y2) 
                    when (x1 + x2) / 2 > 0 && (y1 + y2) / 2 > 0 => "First Quadrant",
                
                // Center in other area
                RectangleWithCenter(var x1, var y1, var x2, var y2) => 
                    $"Center at ({(x1+x2)/2}, {(y1+y2)/2})"
            };
        }
    }

    // ── EXAMPLE CLASSES WITH DECONSTRUCT ──────────────────────────────────
    /// <summary>
    /// Point with Deconstruct method for positional pattern
    /// </summary>
    public class Point
    {
        public int X { get; }
        public int Y { get; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        // Deconstruct method enables positional pattern matching
        public void Deconstruct(out int x, out int y)
        {
            x = X;
            y = Y;
        }
    }

    /// <summary>
    /// Circle with Deconstruct for radius
    /// </summary>
    public class Circle
    {
        public double Radius { get; }

        public Circle(double radius) => Radius = radius;

        public void Deconstruct(out double radius)
        {
            radius = Radius;
        }
    }

    /// <summary>
    /// Rectangle with Deconstruct for dimensions
    /// </summary>
    public class Rectangle
    {
        public double Width { get; }
        public double Height { get; }

        public Rectangle(double width, double height)
        {
            Width = width;
            Height = height;
        }

        public void Deconstruct(out double width, out double height)
        {
            width = Width;
            height = Height;
        }
    }

    /// <summary>
    /// Line with Deconstruct for endpoints
    /// </summary>
    public class Line
    {
        public int X1, Y1, X2, Y2;

        public Line(int x1, int y1, int x2, int y2)
        {
            X1 = x1; Y1 = y1; X2 = x2; Y2 = y2;
        }

        public void Deconstruct(out int x1, out int y1, out int x2, out int y2)
        {
            x1 = X1; y1 = Y1; x2 = X2; y2 = Y2;
        }
    }

    /// <summary>
    /// Record with automatic positional Deconstruct
    /// </summary>
    public record PersonRecord(string Name, int Age, string Role);

    /// <summary>
    /// Rectangle with center calculation
    /// </summary>
    public class RectangleWithCenter
    {
        public int X1, Y1, X2, Y2;

        public RectangleWithCenter(int x1, int y1, int x2, int y2)
        {
            X1 = x1; Y1 = y1; X2 = x2; Y2 = y2;
        }

        public void Deconstruct(out int x1, out int y1, out int x2, out int y2)
        {
            x1 = X1; y1 = Y1; x2 = X2; y2 = Y2;
        }
    }
}
