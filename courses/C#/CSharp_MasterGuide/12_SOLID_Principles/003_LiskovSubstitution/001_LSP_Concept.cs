/*
 * ============================================================
 * TOPIC     : SOLID Principles
 * SUBTOPIC  : Liskov Substitution Principle - Concept
 * FILE      : 01_LSP_Concept.cs
 * PURPOSE   : Demonstrates LSP - derived classes must be substitutable for base
 * ============================================================
 */
using System; // Core System namespace for Console

namespace CSharp_MasterGuide._12_SOLID_Principles._03_LiskovSubstitution._01_LSP_Concept
{
    /// <summary>
    /// Demonstrates Liskov Substitution Principle concept
    /// </summary>
    public class LSPConceptDemo
    {
        /// <summary>
        /// Entry point for LSP concept examples
        /// </summary>
        public static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════
            // SECTION 1: Concept Introduction
            // ═══════════════════════════════════════════════════════════

            Console.WriteLine("=== Liskov Substitution Principle ===\n");

            // Output: --- Concept: What is LSP? ---
            Console.WriteLine("--- Concept: What is LSP? ---");

            // Objects of a superclass should be replaceable
            // with objects of a subclass without breaking the application

            Console.WriteLine("   Subclasses must be substitutable for base class");
            // Output: Subclasses must be substitutable for base class

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: Violating LSP
            // ═══════════════════════════════════════════════════════════
            
            // Output: --- Violating LSP ---
            Console.WriteLine("\n--- Violating LSP ---");

            // Square IS-A Rectangle but breaks Liskov
            var rectangle = new BadRectangle { Width = 5, Height = 4 };
            Console.WriteLine($"   Bad rectangle: {rectangle.Width}x{rectangle.Height} = {rectangle.Area}");
            // Output: Bad rectangle: 5x4 = 20

            var square = new BadSquare { Width = 5, Height = 4 }; // breaks!
            Console.WriteLine($"   Bad square: {square.Width}x{square.Height} = {square.Area}");
            // Output: Bad square: 5x5 = 25 (unexpected - height ignored!)

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: Following LSP
            // ═══════════════════════════════════════════════════════════
            
            // Output: --- Following LSP ---
            Console.WriteLine("\n--- Following LSP ---");

            // Use different abstraction for square
            IShape rectangle2 = new GoodRectangle { Width = 5, Height = 4 };
            IShape square2 = new GoodSquare { Side = 5 };

            var calculator = new AreaCalculator();
            Console.WriteLine($"   Rectangle area: {calculator.CalculateArea(rectangle2)}");
            // Output: Rectangle area: 20
            Console.WriteLine($"   Square area: {calculator.CalculateArea(square2)}");
            // Output: Square area: 25

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: Behavior Substitutability
            // ═══════════════════════════════════════════════════════════
            
            // Output: --- Behavior Substitutability ---
            Console.WriteLine("\n--- Behavior Substitutability ---");

            // Subclass maintains expected behavior
            var shapes = new List<IShape>
            {
                new GoodRectangle { Width = 3, Height = 4 },
                new GoodSquare { Side = 5 }
            };

            foreach (var shape in shapes)
            {
                var area = calculator.CalculateArea(shape);
                Console.WriteLine($"   {shape.GetType().Name}: {area}");
            }
            // Output: GoodRectangle: 12
            // Output: GoodSquare: 25

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: Interface Contracts
            // ═══════════════════════════════════════════════════════════
            
            // Output: --- Interface Contracts ---
            Console.WriteLine("\n--- Interface Contracts ---");

            // Read-Only collection - can't add items
            IReadOnlyCollection<string> list = new CustomReadOnlyList();
            Console.WriteLine($"   Count: {list.Count}");
            // Output: Count: 3
            Console.WriteLine($"   First: {list.GetType().Name}");
            // Output: First: CustomReadOnlyList

            Console.WriteLine("\n=== LSP Concept Complete ===");
        }
    }

    /// <summary>
    /// BAD: Square "extends" Rectangle but modifies behavior
    /// </summary>
    public class BadRectangle
    {
        public virtual int Width { get; set; } // property: width
        public virtual int Height { get; set; } // property: height

        public virtual int Area => Width * Height; // property: area
    }

    /// <summary>
    /// BAD: Square inherits Rectangle but breaks substitution
    /// </summary>
    public class BadSquare : BadRectangle
    {
        public override int Width
        {
            get => base.Width;
            set { base.Width = value; base.Height = value; }
        }

        public override int Height
        {
            get => base.Width;
            set { base.Width = value; base.Height = value; }
        }
    }

    /// <summary>
    /// Shape abstraction - properly designed for LSP
    /// </summary>
    public interface IShape
    {
        int Area { get; } // property: area calculation
    }

    /// <summary>
    /// GOOD: Rectangle implements IShape
    /// </summary>
    public class GoodRectangle : IShape
    {
        public int Width { get; set; } // property: width
        public int Height { get; set; } // property: height

        public int Area => Width * Height; // property: area
    }

    /// <summary>
    /// GOOD: Square implements IShape independently
    /// </summary>
    public class GoodSquare : IShape
    {
        public int Side { get; set; } // property: side

        public int Area => Side * Side; // property: area
    }

    /// <summary>
    /// Area calculator - works with any IShape
    /// </summary>
    public class AreaCalculator
    {
        public int CalculateArea(IShape shape)
        {
            return shape.Area;
        }
    }

    /// <summary>
    /// Read-only collection for LSP
    /// </summary>
    public class CustomReadOnlyList : IReadOnlyCollection<string>
    {
        private readonly List<string> _items = new() { "A", "B", "C" }; // list: internal items

        public int Count => _items.Count; // property: count

        public IEnumerator<string> GetEnumerator() => _items.GetEnumerator(); // method: enumerator

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            => _items.GetEnumerator(); // explicit implementation

        public bool Contains(string item) => _items.Contains(item); // method: contains

        public void CopyTo(string[] array, int arrayIndex)
            => _items.CopyTo(array, arrayIndex); // method: copy to
    }
}