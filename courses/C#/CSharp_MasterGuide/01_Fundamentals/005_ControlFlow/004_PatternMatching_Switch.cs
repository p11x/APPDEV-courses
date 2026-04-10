/*
 * ============================================================
 * TOPIC     : C# Fundamentals
 * SUBTOPIC  : Control Flow - Pattern Matching in Switch
 * FILE      : PatternMatching_Switch.cs
 * PURPOSE   : This file covers pattern matching with switch in C# (C# 7.0+).
 *             Pattern matching allows testing values against complex patterns.
 * ============================================================
 */

// --- SECTION: Pattern Matching in Switch ---
// Pattern matching in switch allows testing values against type patterns,
// property patterns, relational patterns, and more

using System;
using System.Collections.Generic;

namespace CSharp_MasterGuide._01_Fundamentals._05_ControlFlow
{
    // ═══════════════════════════════════════════════════════════════════════
    // Supporting classes for examples
    // ═══════════════════════════════════════════════════════════════════════
    
    class Animal { public string Name { get; set; } = ""; }
    class Dog : Animal { public string Breed { get; set; } = "Unknown"; }
    class Cat : Animal { public bool IsIndoor { get; set; } }
    class Bird : Animal { public bool CanFly { get; set; } }
    
    interface IVehicle { }
    class Car : IVehicle { public int Doors { get; set; } }
    class Truck : IVehicle { public double BedLength { get; set; } }
    class Motorcycle : IVehicle { public bool HasSidecar { get; set; } }

    class PatternMatching_Switch
    {
        static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════════
            // SECTION: Type Pattern Matching
            // ═══════════════════════════════════════════════════════════════
            
            // Basic type pattern
            object shape = GetShape();
            
            switch (shape)
            {
                case Circle c:
                    Console.WriteLine($"Circle with radius {c.Radius}");
                    break;
                case Rectangle r:
                    Console.WriteLine($"Rectangle {r.Width}x{r.Height}");
                    break;
                case Triangle t:
                    Console.WriteLine($"Triangle base={t.Base}, height={t.Height}");
                    break;
                case null:
                    Console.WriteLine("No shape");
                    break;
                default:
                    Console.WriteLine("Unknown shape type");
                    break;
            }
            
            // Type pattern with conditions
            object num = 42;
            switch (num)
            {
                case int i when i > 0:
                    Console.WriteLine($"Positive integer: {i}");
                    break;
                case int i when i < 0:
                    Console.WriteLine($"Negative integer: {i}");
                    break;
                case int i:
                    Console.WriteLine($"Zero: {i}");
                    break;
            }
            
            // Multiple types in one case
            object value = "hello";
            switch (value)
            {
                case string s:
                    Console.WriteLine($"String of length {s.Length}");
                    break;
                case int or long or double: // C# 9.0+ or pattern
                    Console.WriteLine("Numeric value");
                    break;
                default:
                    Console.WriteLine("Other type");
                    break;
            }

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Property Patterns (C# 8.0+)
            // ═══════════════════════════════════════════════════════════════
            
            // Property pattern - match on object properties
            var person = new { Name = "Alice", Age = 30, City = "NYC" };
            
            string location = person switch
            {
                { City: "NYC" } => "New Yorker",
                { City: "LA" } => "Angeleno",
                { Age: < 18 } => "Minor",
                _ => "Other"
            };
            
            Console.WriteLine($"Person: {location}"); // Output: New Yorker
            
            // Nested property patterns
            var order = new 
            { 
                Customer = new { Name = "John", Level = "Gold" },
                Total = 150.00
            };
            
            string discountCategory = order switch
            {
                { Customer: { Level: "Gold" }, Total: > 100 } => "Gold discount 20%",
                { Customer: { Level: "Gold" }, Total: <= 100 } => "Gold discount 10%",
                { Customer: { Level: "Silver" }, Total: > 50 } => "Silver discount 10%",
                _ => "No discount"
            };
            
            Console.WriteLine($"Discount: {discountCategory}");

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Relational Patterns (C# 9.0+)
            // ═══════════════════════════════════════════════════════════════
            
            // Relational patterns with <, >, <=, >=
            int temperature = 72;
            string weather = temperature switch
            {
                < 32 => "Freezing",
                < 50 => "Cold",
                < 70 => "Cool",
                < 85 => "Warm",
                _ => "Hot"
            };
            
            Console.WriteLine($"Weather: {weather}"); // Output: Warm
            
            // Combining with and/or/not (C# 9.0+)
            int age = 25;
            string category = age switch
            {
                >= 0 and < 13 => "Child",
                >= 13 and < 20 => "Teenager",
                >= 20 and < 35 => "Young adult",
                >= 35 and < 55 => "Middle aged",
                >= 55 => "Senior",
                _ => "Invalid"
            };
            
            Console.WriteLine($"Age category: {category}"); // Output: Young adult
            
            // Negation with not
            object test = "hello";
            string testResult = test switch
            {
                not string => "Not a string",
                not null => "Some string value",
                _ => "String"
            };
            
            Console.WriteLine($"Test result: {testResult}"); // Output: Some string value

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Tuple Patterns
            // ═══════════════════════════════════════════════════════════════
            
            // Match on tuples
            (int x, int y) point = (5, -3);
            
            string quadrant = point switch
            {
                ( > 0, > 0) => "First quadrant (positive, positive)",
                ( < 0, > 0) => "Second quadrant (negative, positive)",
                ( < 0, < 0) => "Third quadrant (negative, negative)",
                ( > 0, < 0) => "Fourth quadrant (positive, negative)",
                (0, 0) => "Origin",
                (0, _) => "On Y-axis",
                (_, 0) => "On X-axis"
            };
            
            Console.WriteLine($"Point {point} is in: {quadrant}");
            
            // Practical tuple pattern - time classification
            (int hour, bool isWeekend) time = (15, false);
            string timeType = time switch
            {
                ( >= 9 and < 17, false) => "Weekday work hours",
                ( >= 9 and < 17, true) => "Weekend day",
                ( >= 17 and < 22, _) => "Evening",
                ( >= 22 or < 6, _) => "Night",
                _ => "Early morning"
            };
            
            Console.WriteLine($"Time type: {timeType}");

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Var Patterns
            // ═══════════════════════════════════════════════════════════════
            
            // Var pattern captures the value
            object anything = 42;
            string varCapture = anything switch
            {
                var v when v is int i && i > 0 => $"Captured int: {i}",
                var v => $"Captured: {v}"
            };
            
            Console.WriteLine(varCapture); // Output: Captured int: 42
            
            // Discard pattern with _
            object[] data = { 1, "two", 3.0, '4' };
            
            foreach (var item in data)
            {
                string typeInfo = item switch
                {
                    int => "Integer",
                    string => "String",
                    double => "Double",
                    char => "Character",
                    _ => "Unknown"
                };
                
                Console.WriteLine($"{item}: {typeInfo}");
            }

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Positional Patterns
            // ═══════════════════════════════════════════════════════════════
            
            // Using Deconstruct to match on position
            var point2D = new Point2D(3, 4);
            
            string pointType = point2D switch
            {
                (0, 0) => "Origin",
                var (x, y) when x == y => $"Diagonal line (x=y={x})",
                var (x, y) when x == -y => $"Anti-diagonal (x=-y={x})",
                var (x, y) => $"Point ({x}, {y})"
            };
            
            Console.WriteLine(pointType); // Output: Diagonal line (x=y=3)

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Real-World Examples
            // ═══════════════════════════════════════════════════════════════
            
            // ── Shape calculator with polymorphism ───────────────────────
            object[] shapes = { new Circle(5), new Rectangle(4, 3), new Triangle(6, 4) };
            
            foreach (var shape in shapes)
            {
                double area = CalculateArea(shape);
                Console.WriteLine($"Shape area: {area:F2}");
            }
            
            // ── API response handling ────────────────────────────────────
            var responses = new object[] 
            { 
                new { Code = 200, Body = "Success" },
                new { Code = 404, Body = "Not Found" },
                new { Code = 500, Body = "Error" }
            };
            
            foreach (var resp in responses)
            {
                string handling = resp switch
                {
                    { Code: 200 or 201 } => "Success - process body",
                    { Code: 400 or 401 or 403 } => "Client error - check input",
                    { Code: >= 500 } => "Server error - log and alert",
                    _ => "Unknown response"
                };
                
                Console.WriteLine($"Code {((dynamic)resp).Code}: {handling}");
            }
            
            // ── Animal classification ─────────────────────────────────────
            Animal[] animals = { new Dog { Name = "Buddy" }, new Cat { IsIndoor = true }, new Bird { CanFly = true } };
            
            foreach (var animal in animals)
            {
                string description = animal switch
                {
                    Dog d => $"Dog named {d.Name}, breed {d.Breed}",
                    Cat c => $"Cat ({(c.IsIndoor ? "indoor" : "outdoor")})",
                    Bird b => $"Bird (can {(b.CanFly ? "fly" : "not fly")})",
                    _ => "Unknown animal"
                };
                
                Console.WriteLine(description);
            }
        }
        
        // Method returning shape for demo
        static object GetShape() => new Circle(5);
        
        // Method using pattern matching for area
        static double CalculateArea(object shape) => shape switch
        {
            Circle c => Math.PI * c.Radius * c.Radius,
            Rectangle r => r.Width * r.Height,
            Triangle t => 0.5 * t.Base * t.Height,
            _ => 0
        };
    }
    
    // ═══════════════════════════════════════════════════════════════════════
    // Supporting classes for examples
    // ═══════════════════════════════════════════════════════════════════════
    
    class Circle
    {
        public double Radius { get; }
        public Circle(double r) => Radius = r;
    }
    
    class Rectangle
    {
        public double Width { get; }
        public double Height { get; }
        public Rectangle(double w, double h) { Width = w; Height = h; }
    }
    
    class Triangle
    {
        public double Base { get; }
        public double Height { get; }
        public Triangle(double b, double h) { Base = b; Height = h; }
    }
    
    // Point with Deconstruct for positional pattern
    class Point2D
    {
        public int X { get; }
        public int Y { get; }
        
        public Point2D(int x, int y) { X = x; Y = y; }
        
        public void Deconstruct(out int x, out int y)
        {
            x = X;
            y = Y;
        }
    }
}
