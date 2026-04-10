/*
 * ============================================================
 * TOPIC     : C# Fundamentals
 * SUBTOPIC  : Control Flow - Switch Expression
 * FILE      : SwitchExpression.cs
 * PURPOSE   : This file covers switch expressions in C# (C# 8.0+), a modern concise alternative to switch statements.
 *             Switch expressions return a value and use => syntax.
 * ============================================================
 */

// --- SECTION: Switch Expressions ---
// Switch expressions are a modern C# 8.0+ feature that provides a more concise way
// to return values based on pattern matching. They are expressions that return a single value.

using System;
using System.Collections.Generic;

namespace CSharp_MasterGuide._01_Fundamentals._05_ControlFlow
{
    class SwitchExpression
    {
        static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════════
            // SECTION: Basic Switch Expression
            // ═══════════════════════════════════════════════════════════════
            
            // Switch expression - returns a value
            int dayNumber = 3;
            string dayName = dayNumber switch
            {
                1 => "Monday",
                2 => "Tuesday",
                3 => "Wednesday",
                4 => "Thursday",
                5 => "Friday",
                6 => "Saturday",
                7 => "Sunday",
                _ => "Invalid"
            };
            
            Console.WriteLine($"Day {dayNumber} is {dayName}"); // Output: Day 3 is Wednesday
            
            // Note: Uses _ instead of default for discard pattern

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Multiple Values with Switch Expression
            // ═══════════════════════════════════════════════════════════════
            
            // Multiple values in single case using comma
            char grade = 'B';
            string description = grade switch
            {
                'A' or 'a' => "Excellent work",
                'B' or 'b' => "Good job",
                'C' or 'c' => "Satisfactory",
                'D' or 'd' => "Needs improvement",
                'F' or 'f' => "Failed",
                _ => "Invalid grade"
            };
            
            Console.WriteLine($"Grade {grade}: {description}"); // Output: Grade B: Good job

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Pattern Matching in Switch Expression
            // ═══════════════════════════════════════════════════════════════
            
            // Type pattern matching
            object shape = new Circle(5);
            double area = shape switch
            {
                Circle c => Math.PI * c.Radius * c.Radius,
                Rectangle r => r.Width * r.Height,
                Triangle t => 0.5 * t.Base * t.Height,
                _ => 0
            };
            
            Console.WriteLine($"Area: {area:F2}"); // Output: Area: 78.54
            
            // Property pattern (C# 9.0+)
            object person = new { Name = "John", Age = 30 };
            string category = person switch
            {
                { Age: < 13 } => "Child",
                { Age: < 20 } => "Teenager",
                { Age: < 35 } => "Young adult",
                { Age: < 60 } => "Adult",
                { Age: _ } => "Senior"
            };
            
            Console.WriteLine($"Category: {category}"); // Output: Category: Young adult
            
            // Var pattern - captures value
            object value = 42;
            string typeDescription = value switch
            {
                int i => $"Integer: {i}",
                double d => $"Double: {d}",
                string s => $"String: {s}",
                bool b => $"Boolean: {b}",
                var v => $"Unknown: {v}"
            };
            
            Console.WriteLine(typeDescription); // Output: Integer: 42

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Switch Expression with When Clause
            // ═══════════════════════════════════════════════════════════════
            
            // Guard clauses with when
            int score = 95;
            string gradeResult = score switch
            {
                >= 90 => "A - Excellent",
                >= 80 => "B - Good",
                >= 70 => "C - Average",
                >= 60 => "D - Passing",
                _ when score >= 0 => "F - Failing",
                _ => "Invalid score"
            };
            
            Console.WriteLine($"Score {score}: {gradeResult}"); // Output: Score 95: A - Excellent
            
            // With relational patterns (C# 9.0+)
            int age = 25;
            string ageGroup = age switch
            {
                < 0 or > 120 => "Invalid age",
                >= 0 and < 13 => "Child",
                >= 13 and < 20 => "Teenager",
                >= 20 and < 35 => "Young adult",
                >= 35 and < 55 => "Middle-aged",
                >= 55 and < 75 => "Senior",
                _ => "Elderly"
            };
            
            Console.WriteLine($"Age {age}: {ageGroup}"); // Output: Age 25: Young adult

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Switch Expression with Tuples
            // ═══════════════════════════════════════════════════════════════
            
            // Tuple pattern matching
            (int x, int y) point = (5, 3);
            string quadrant = point switch
            {
                ( > 0, > 0) => "First quadrant",
                ( < 0, > 0) => "Second quadrant",
                ( < 0, < 0) => "Third quadrant",
                ( > 0, < 0) => "Fourth quadrant",
                (0, 0) => "Origin",
                (0, _) => "On Y-axis",
                (_, 0) => "On X-axis",
                _ => "Invalid point"
            };
            
            Console.WriteLine($"Point (5,3): {quadrant}"); // Output: First quadrant
            
            // Color mixing with tuple
            (string, string) colors = ("red", "blue");
            string result = colors switch
            {
                ("red", "blue") or ("blue", "red") => "Purple",
                ("red", "yellow") or ("yellow", "red") => "Orange",
                ("blue", "yellow") or ("yellow", "blue") => "Green",
                ("red", "red") => "Red",
                ("blue", "blue") => "Blue",
                ("yellow", "yellow") => "Yellow",
                _ => "Unknown combination"
            };
            
            Console.WriteLine($"Red + Blue = {result}"); // Output: Red + Blue = Purple

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Switch Expression as Expression Body
            // ═══════════════════════════════════════════════════════════════
            
            // Can use in expression-bodied members
            // See method below for examples

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Real-World Examples
            // ═══════════════════════════════════════════════════════════════
            
            // ── Order status ───────────────────────────────────────────────
            string status = "Shipped";
            string statusMessage = status.ToLower() switch
            {
                "pending" => "Your order is being processed",
                "processing" => "Your order is being prepared",
                "shipped" => "Your order is on its way",
                "delivered" => "Your order has arrived",
                "cancelled" => "Your order was cancelled",
                _ => "Unknown status"
            };
            
            Console.WriteLine($"Status: {statusMessage}");
            
            // ── API response handling ──────────────────────────────────────
            var apiResponse = new { Code = 200, Message = "Success" };
            string responseAction = apiResponse.Code switch
            {
                200 or 201 or 202 => "Process success response",
                400 => "Handle bad request",
                401 or 403 => "Redirect to login",
                404 => "Show not found page",
                500 or 502 or 503 => "Show error page",
                _ => "Unknown response"
            };
            
            Console.WriteLine($"Response action: {responseAction}");
            
            // ── Shipping calculator ────────────────────────────────────────
            string shippingMethod = "express";
            string shippingEstimate = shippingMethod.ToLower() switch
            {
                "standard" => "5-7 business days",
                "express" => "2-3 business days",
                "overnight" => "Next business day",
                "pickup" => "Ready for pickup today",
                _ => "Contact customer service"
            };
            
            Console.WriteLine($"Shipping: {shippingEstimate}");
            
            // ── Null handling with switch ─────────────────────────────────
            string? input = null;
            string output = input switch
            {
                null => "No input provided",
                string s when string.IsNullOrWhiteSpace(s) => "Empty input",
                _ => $"Input: {input}"
            };
            
            Console.WriteLine($"Output: {output}"); // Output: No input provided

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Using with Method Bodies
            // ═══════════════════════════════════════════════════════════════
            
            // Method returning switch expression result
            Console.WriteLine($"Day type: {GetDayType(DayOfWeek.Friday)}"); // weekday
            Console.WriteLine($"Day type: {GetDayType(DayOfWeek.Saturday)}"); // weekend
            
            // Method with tuple switch
            Console.WriteLine($"Navigation: {GetDirection((0, 1))}"); // North
            Console.WriteLine($"Navigation: {GetDirection((-1, 0))}"); // West
        }
        
        // ═══════════════════════════════════════════════════════════════
        // Methods using switch expressions
        // ═══════════════════════════════════════════════════════════════
        
        // Expression-bodied method using switch
        static string GetDayType(DayOfWeek day) => day switch
        {
            DayOfWeek.Saturday or DayOfWeek.Sunday => "weekend",
            _ => "weekday"
        };
        
        // Tuple pattern in switch expression
        static string GetDirection((int x, int y) vector) => vector switch
        {
            (0, 1) => "North",
            (1, 0) => "East",
            (0, -1) => "South",
            (-1, 0) => "West",
            ( > 0, > 0) => "Northeast",
            ( > 0, < 0) => "Southeast",
            ( < 0, > 0) => "Northwest",
            ( < 0, < 0) => "Southwest",
            _ => "Unknown direction"
        };
    }
    
    // ═══════════════════════════════════════════════════════════════
    // Shape classes for examples
    // ═══════════════════════════════════════════════════════════════
    
    class Circle
    {
        public double Radius { get; }
        public Circle(double radius) => Radius = radius;
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
}
