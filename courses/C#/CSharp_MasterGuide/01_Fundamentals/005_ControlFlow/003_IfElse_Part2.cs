/*
 * ============================================================
 * TOPIC     : C# Fundamentals
 * SUBTOPIC  : Control Flow - If-Else Statements (Part 2)
 * FILE      : IfElse_Part2.cs
 * PURPOSE   : This file covers advanced if-else patterns including switch expressions,
 *             pattern matching, and real-world use cases.
 * ============================================================
 */

// --- SECTION: Advanced If-Else Patterns ---
// This file covers advanced patterns and modern C# features related to conditional logic

using System;

namespace CSharp_MasterGuide._01_Fundamentals._05_ControlFlow
{
    // ═══════════════════════════════════════════════════════════════════════
    // Supporting classes for pattern matching examples
    // ═══════════════════════════════════════════════════════════════════════
    
    class Animal { }
    class Dog : Animal { public string Breed { get; set; } = "Unknown"; }
    class Cat : Animal { public bool IsIndoor { get; set; } }
    class Bird : Animal { public bool CanFly { get; set; } }
    
    class Vehicle { }
    class Car : Vehicle { public int Doors { get; set; } }
    class Truck : Vehicle { public double BedLength { get; set; } }
    class Motorcycle : Vehicle { public bool HasSidecar { get; set; } }

    class IfElse_Part2
    {
        static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════════
            // SECTION: Pattern Matching with If
            // ═══════════════════════════════════════════════════════════════
            
            // Type pattern matching
            object shape = new Circle(5);
            
            if (shape is Circle c)
            {
                double area = Math.PI * c.Radius * c.Radius;
                Console.WriteLine($"Circle area: {area:F2}");
            }
            else if (shape is Rectangle r)
            {
                double area = r.Width * r.Height;
                Console.WriteLine($"Rectangle area: {area:F2}");
            }
            
            // Declaration pattern (introduces variable)
            object value = 42;
            
            if (value is int intValue && intValue > 0)
            {
                Console.WriteLine($"Positive integer: {intValue}");
            }
            
            // Null pattern
            object? nullValue = null;
            if (nullValue is null)
            {
                Console.WriteLine("Value is null");
            }
            
            // Relational patterns (C# 9.0+)
            int grade = 85;
            
            if (grade >= 90)
                Console.WriteLine("A - Excellent");
            else if (grade >= 80)
                Console.WriteLine("B - Good");
            else if (grade >= 70)
                Console.WriteLine("C - Satisfactory");
            else if (grade >= 60)
                Console.WriteLine("D - Passing");
            else
                Console.WriteLine("F - Failing");

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Switch Expression (Modern Alternative)
            // ═══════════════════════════════════════════════════════════════
            
            // Switch expression - cleaner syntax (C# 8.0+)
            string dayType = GetDayType(DayOfWeek.Monday);
            Console.WriteLine($"Monday is: {dayType}"); // Output: weekday
            
            dayType = GetDayType(DayOfWeek.Saturday);
            Console.WriteLine($"Saturday is: {dayType}"); // Output: weekend
            
            // String matching in switch
            string status = "Active";
            string description = status switch
            {
                "Active" => "Currently in use",
                "Inactive" => "Not currently used",
                "Pending" => "Awaiting activation",
                "Cancelled" => "No longer active",
                _ => "Unknown status"
            };
            Console.WriteLine($"Status: {description}");

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Advanced Conditional Patterns
            // ═══════════════════════════════════════════════════════════════
            
            // Combining patterns with 'or' and 'and' (C# 9.0+)
            int number = 15;
            
            if (number is >= 0 and <= 10)
                Console.WriteLine("In range 0-10");
            else if (number is > 10 and <= 20)
                Console.WriteLine("In range 11-20");
            else if (number is > 20)
                Console.WriteLine("Greater than 20");
            else
                Console.WriteLine("Negative");
            
            // Negation pattern
            object obj = "hello";
            if (obj is not string)
                Console.WriteLine("Not a string");
            else
                Console.WriteLine("Is a string");
            
            // Parenthesized pattern
            object num = 5;
            if (num is (int)i && i > 0)
            {
                Console.WriteLine($"Positive int: {i}");
            }

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Real-World Examples
            // ═══════════════════════════════════════════════════════════════
            
            // ── Order processing ───────────────────────────────────────────
            ProcessOrder("Pending");
            ProcessOrder("Processing");
            ProcessOrder("Shipped");
            ProcessOrder("Delivered");
            ProcessOrder("Cancelled");
            
            // ── Temperature-based recommendations ───────────────────────
            int temperature = 72;
            GetActivityRecommendation(temperature);
            
            temperature = 45;
            GetActivityRecommendation(temperature);
            
            temperature = 95;
            GetActivityRecommendation(temperature);
            
            // ── User authorization levels ─────────────────────────────────
            CheckUserAccess("admin", "read");
            CheckUserAccess("admin", "delete");
            CheckUserAccess("user", "delete");
            CheckUserAccess("guest", "read");
            
            // ── Age group categorization ───────────────────────────────────
            CategorizeAge(5);
            CategorizeAge(15);
            CategorizeAge(25);
            CategorizeAge(45);
            CategorizeAge(70);
            CategorizeAge(90);
        }
        
        // ── Switch expression method ─────────────────────────────────────
        static string GetDayType(DayOfWeek day) => day switch
        {
            DayOfWeek.Saturday or DayOfWeek.Sunday => "weekend",
            _ => "weekday"
        };
        
        // ── Process order method ─────────────────────────────────────────
        static void ProcessOrder(string status)
        {
            if (status == "Pending")
            {
                Console.WriteLine("Order is being reviewed");
            }
            else if (status == "Processing")
            {
                Console.WriteLine("Order is being prepared");
            }
            else if (status == "Shipped")
            {
                Console.WriteLine("Order has been shipped");
            }
            else if (status == "Delivered")
            {
                Console.WriteLine("Order delivered successfully");
            }
            else if (status == "Cancelled")
            {
                Console.WriteLine("Order was cancelled");
            }
            else
            {
                Console.WriteLine("Unknown order status");
            }
        }
        
        // ── Activity recommendation ──────────────────────────────────────
        static void GetActivityRecommendation(int tempF)
        {
            if (tempF < 32)
            {
                Console.WriteLine($"Temperature {tempF}°F: Stay indoors - freezing!");
            }
            else if (tempF < 50)
            {
                Console.WriteLine($"Temperature {tempF}°F: Wear warm clothes");
            }
            else if (tempF < 70)
            {
                Console.WriteLine($"Temperature {tempF}°F: Nice weather for walking");
            }
            else if (tempF < 85)
            {
                Console.WriteLine($"Temperature {tempF}°F: Great outdoor weather");
            }
            else
            {
                Console.WriteLine($"Temperature {tempF}°F: Stay cool indoors!");
            }
        }
        
        // ── Authorization check ──────────────────────────────────────────
        static void CheckUserAccess(string role, string action)
        {
            bool hasAccess = false;
            
            if (role == "admin")
            {
                hasAccess = true; // Admin can do anything
            }
            else if (role == "moderator")
            {
                if (action == "read" || action == "write" || action == "delete")
                {
                    hasAccess = true;
                }
            }
            else if (role == "user")
            {
                if (action == "read" || action == "write")
                {
                    hasAccess = true;
                }
            }
            else if (role == "guest")
            {
                if (action == "read")
                {
                    hasAccess = true;
                }
            }
            
            Console.WriteLine($"Role={role}, Action={action}: Access={hasAccess}");
        }
        
        // ── Age categorization ────────────────────────────────────────────
        static void CategorizeAge(int age)
        {
            string category;
            
            if (age < 0 || age > 150)
            {
                category = "Invalid age";
            }
            else if (age < 13)
            {
                category = "Child";
            }
            else if (age < 20)
            {
                category = "Teenager";
            }
            else if (age < 35)
            {
                category = "Young adult";
            }
            else if (age < 55)
            {
                category = "Middle-aged";
            }
            else if (age < 75)
            {
                category = "Senior";
            }
            else
            {
                category = "Elderly";
            }
            
            Console.WriteLine($"Age {age}: {category}");
        }
    }
    
    // ═══════════════════════════════════════════════════════════════════════
    // Shape classes for pattern matching examples
    // ═══════════════════════════════════════════════════════════════════════
    
    class Circle
    {
        public double Radius { get; set; }
        public Circle(double radius) { Radius = radius; }
    }
    
    class Rectangle
    {
        public double Width { get; set; }
        public double Height { get; set; }
        public Rectangle(double w, double h) { Width = w; Height = h; }
    }
}
