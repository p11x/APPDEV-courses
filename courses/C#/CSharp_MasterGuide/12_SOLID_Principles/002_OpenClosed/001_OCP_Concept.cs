/*
 * ============================================================
 * TOPIC     : SOLID Principles
 * SUBTOPIC  : Open-Closed Principle - Concept
 * FILE      : 01_OCP_Concept.cs
 * PURPOSE   : Demonstrates OCP - open for extension, closed for modification
 * ============================================================
 */
using System; // Core System namespace for Console

namespace CSharp_MasterGuide._12_SOLID_Principles._02_OpenClosed._01_OCP_Concept
{
    /// <summary>
    /// Demonstrates Open-Closed Principle concept
    /// </summary>
    public class OCPConceptDemo
    {
        /// <summary>
        /// Entry point for OCP concept examples
        /// </summary>
        public static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════
            // SECTION 1: Concept Introduction
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("=== Open-Closed Principle ===\n");

            // Output: --- Concept: What is OCP? ---
            Console.WriteLine("--- Concept: What is OCP? ---");

            // Open for extension: add new features by inheritance
            // Closed for modification: don't change existing code

            Console.WriteLine("   Open for extension, closed for modification");
            // Output: Open for extension, closed for modification

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: Violating OCP
            // ═══════════════════════════════════════════════════════════
            
            // Output: --- Violating OCP ---
            Console.WriteLine("\n--- Violating OCP ---");

            // Adding new shape requires modifying existing code
            var badCalculator = new BadAreaCalculator();
            
            // Must modify calculator to add new shapes
            badCalculator.CalculateArea("Circle", 5);
            // Output: Circle area: 78.54
            badCalculator.CalculateArea("Rectangle", 4, 5);
            // Output: Rectangle area: 20
            badCalculator.CalculateArea("Triangle", 3, 4);
            // Output: Triangle area: 6

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: Following OCP
            // ═══════════════════════════════════════════════════════════
            
            // Output: --- Following OCP ---
            Console.WriteLine("\n--- Following OCP ---");

            // Use polymorphism - add new shapes without changing calculator
            var shapes = new List<Shape>
            {
                new Circle { Radius = 5 },
                new Rectangle { Width = 4, Height = 5 },
                new Triangle { Base = 3, Height = 4 }
            };

            var goodCalculator = new GoodAreaCalculator();
            foreach (var shape in shapes)
            {
                var area = goodCalculator.Calculate(shape);
                Console.WriteLine($"   {shape.GetType().Name}: {area:F2}");
            }
            // Output: Circle: 78.54
            // Output: Rectangle: 20.00
            // Output: Triangle: 6.00

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: Adding New Shape
            // ═══════════════════════════════════════════════════════════
            
            // Output: --- Adding New Shape ---
            Console.WriteLine("\n--- Adding New Shape ---");

            // Add new shape WITHOUT modifying calculator
            shapes.Add(new Square { Side = 3 });
            
            foreach (var shape in shapes)
            {
                var area = goodCalculator.Calculate(shape);
                Console.WriteLine($"   {shape.GetType().Name}: {area:F2}");
            }
            // Output: Circle: 78.54
            // Output: Rectangle: 20.00
            // Output: Triangle: 6.00
            // Output: Square: 9.00

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: Strategy Pattern
            // ═══════════════════════════════════════════════════════════
            
            // Output: --- Strategy Pattern ---
            Console.WriteLine("\n--- Strategy Pattern ---");

            // Strategy pattern - behavior can be extended
            var discount1 = new PercentageDiscount { Percentage = 10m };
            var discount2 = new FixedDiscount { Amount = 50m };
            var discount3 = new NoDiscount();

            var calculator = new PriceCalculator();
            
            Console.WriteLine($"   10% off $100: {calculator.Calculate(100m, discount1):C}");
            // Output: 10% off $100: $90.00
            Console.WriteLine($"   $50 off $100: {calculator.Calculate(100m, discount2):C}");
            // Output: $50 off $100: $50.00
            Console.WriteLine($"   No discount: {calculator.Calculate(100m, discount3):C}");
            // Output: No discount: $100.00

            Console.WriteLine("\n=== OCP Concept Complete ===");
        }
    }

    /// <summary>
    /// BAD: Must modify to add new shapes
    /// </summary>
    public class BadAreaCalculator
    {
        public void CalculateArea(string shapeType, double param1, double param2 = 0)
        {
            // Using if statements - violates OCP
            if (shapeType == "Circle")
            {
                var area = Math.PI * param1 * param1;
                Console.WriteLine($"   Circle area: {area:F2}");
            }
            else if (shapeType == "Rectangle")
            {
                var area = param1 * param2;
                Console.WriteLine($"   Rectangle area: {area:F2}");
            }
            else if (shapeType == "Triangle")
            {
                var area = 0.5 * param1 * param2;
                Console.WriteLine($"   Triangle area: {area:F2}");
            }
            // Adding new shape = modifying this code!
        }
    }

    /// <summary>
    /// Shape abstraction - open for extension
    /// </summary>
    public abstract class Shape
    {
        public abstract double CalculateArea();
    }

    /// <summary>
    /// Circle implementation
    /// </summary>
    public class Circle : Shape
    {
        public double Radius { get; set; } // property: radius

        public override double CalculateArea()
        {
            return Math.PI * Radius * Radius;
        }
    }

    /// <summary>
    /// Rectangle implementation
    /// </summary>
    public class Rectangle : Shape
    {
        public double Width { get; set; } // property: width
        public double Height { get; set; } // property: height

        public override double CalculateArea()
        {
            return Width * Height;
        }
    }

    /// <summary>
    /// Triangle implementation
    /// </summary>
    public class Triangle : Shape
    {
        public double Base { get; set; } // property: base
        public double Height { get; set; } // property: height

        public override double CalculateArea()
        {
            return 0.5 * Base * Height;
        }
    }

    /// <summary>
    /// Square implementation - added without modifying calculator
    /// </summary>
    public class Square : Shape
    {
        public double Side { get; set; } // property: side

        public override double CalculateArea()
        {
            return Side * Side;
        }
    }

    /// <summary>
    /// GOOD: Closed for modification
    /// </summary>
    public class GoodAreaCalculator
    {
        public double Calculate(Shape shape)
        {
            // Uses polymorphism - no if statements
            return shape.CalculateArea();
        }
    }

    /// <summary>
    /// Discount strategy abstraction
    /// </summary>
    public interface IDiscountStrategy
    {
        decimal Calculate(decimal price); // method: calculates discounted price
    }

    /// <summary>
    /// Percentage discount - can add new strategies
    /// </summary>
    public class PercentageDiscount : IDiscountStrategy
    {
        public decimal Percentage { get; set; } // property: discount percentage

        public decimal Calculate(decimal price)
        {
            return price * (1 - Percentage / 100m);
        }
    }

    /// <summary>
    /// Fixed discount - can add new strategies
    /// </summary>
    public class FixedDiscount : IDiscountStrategy
    {
        public decimal Amount { get; set; } // property: fixed amount

        public decimal Calculate(decimal price)
        {
            return Math.Max(0, price - Amount);
        }
    }

    /// <summary>
    /// No discount - default
    /// </summary>
    public class NoDiscount : IDiscountStrategy
    {
        public decimal Calculate(decimal price)
        {
            return price;
        }
    }

    /// <summary>
    /// Price calculator - closed for modification
    /// </summary>
    public class PriceCalculator
    {
        public decimal Calculate(decimal price, IDiscountStrategy discount)
        {
            return discount.Calculate(price);
        }
    }
}