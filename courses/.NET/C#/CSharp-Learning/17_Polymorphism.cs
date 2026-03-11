/*
================================================================================
TOPIC 17: POLYMORPHISM
================================================================================

Polymorphism allows objects of different types to be treated uniformly.

TABLE OF CONTENTS:
1. What is Polymorphism?
2. Method Overloading
3. Method Overriding
4. Runtime Polymorphism
================================================================================
*/

namespace PolymorphismExamples
{
    // Base class
    class Shape
    {
        public virtual void Draw()
        {
            Console.WriteLine("Drawing a shape");
        }
        
        public virtual double GetArea() => 0;
    }
    
    // Derived classes
    class Circle : Shape
    {
        public double Radius { get; set; }
        
        public Circle(double radius) { Radius = radius; }
        
        public override void Draw()
        {
            Console.WriteLine($"Drawing a circle with radius {Radius}");
        }
        
        public override double GetArea() => Math.PI * Radius * Radius;
    }
    
    class Rectangle : Shape
    {
        public double Width { get; set; }
        public double Height { get; set; }
        
        public Rectangle(double w, double h) { Width = w; Height = h; }
        
        public override void Draw()
        {
            Console.WriteLine($"Drawing rectangle {Width}x{Height}");
        }
        
        public override double GetArea() => Width * Height;
    }
    
    class Program
    {
        static void Main()
        {
            // Polymorphism in action
            Shape[] shapes = {
                new Circle(5),
                new Rectangle(4, 6),
                new Circle(3)
            };
            
            foreach (Shape s in shapes)
            {
                s.Draw();
                Console.WriteLine($"Area: {s.GetArea():F2}");
            }
        }
    }
}

/*
POLYMORPHISM TYPES:
-------------------
1. Compile-time (Overloading): Same method name, different parameters
2. Runtime (Overriding): Same method, different implementation
*/

// ================================================================================
// NEXT STEPS
// =============================================================================

/*
NEXT: Topic 18 covers Abstraction.
*/
