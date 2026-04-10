/*
 * ============================================================
 * TOPIC     : Advanced OOP
 * SUBTOPIC  : Generics - Generic Methods Part 2
 * FILE      : GenericMethods_Part2.cs
 * PURPOSE   : Teaches advanced constraints in methods, generic delegates,
 *            Func and Action delegates, and practical applications
 * ============================================================
 */

using System;
using System.Collections.Generic;

namespace CSharp_MasterGuide._03_Advanced_OOP._01_Generics
{
    class GenericMethods_Part2
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Generic Methods Part 2 ===\n");

            // ═══════════════════════════════════════════════════════════
            // SECTION 1: Generic Method Constraints Deep Dive
            // ═══════════════════════════════════════════════════════════

            // Using class constraint - must be reference type
            var container1 = new Container<string>();
            container1.SetValue("Hello");
            Console.WriteLine($"Value: {container1.GetValue()}");
            // Output: Value: Hello

            // Using struct constraint - must be value type
            int result = CalculateSquare(5);
            Console.WriteLine($"Square: {result}");
            // Output: Square: 25

            // Using new() constraint - must have parameterless constructor
            var factory = new ObjectFactory();
            var product = factory.Create<Product>();
            Console.WriteLine($"Created: {product.GetType().Name}");
            // Output: Created: Product

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: Base Class and Interface Constraints
            // ═══════════════════════════════════════════════════════════

            // Using base class constraint
            List<Dog2> dogs = new List<Dog2> { new Dog2("Rex"), new Dog2("Max") };
            MakeDogsSpeak(dogs);
            // Output: Rex says: Woof!
            // Output: Max says: Woof!

            // Using interface constraint
            List<Rectangle2> rectangles = new List<Rectangle2>
            {
                new Rectangle2(5, 3),
                new Rectangle2(4, 4)
            };
            var totalArea = CalculateTotalArea(rectangles);
            Console.WriteLine($"Total Area: {totalArea}");
            // Output: Total Area: 31

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: Generic Delegates
            // ═══════════════════════════════════════════════════════════

            // Using Func delegate - returns a value
            Func<int, int, int> add = (a, b) => a + b;
            Console.WriteLine($"Add result: {add(5, 3)}");
            // Output: Add result: 8

            Func<string, string> reverse = s => {
                char[] chars = s.ToCharArray();
                Array.Reverse(chars);
                return new string(chars);
            };
            Console.WriteLine($"Reversed: {reverse("Hello")}");
            // Output: Reversed: olleH

            // Using Action delegate - returns void
            Action<string> print = msg => Console.WriteLine($"Message: {msg}");
            print("Testing Action delegate");
            // Output: Message: Testing Action delegate

            Action<int, int> debug = (x, y) => Console.WriteLine($"Debug: {x}, {y}");
            debug(10, 20);
            // Output: Debug: 10, 20

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: Custom Generic Delegates
            // ═══════════════════════════════════════════════════════════

            // Custom delegate that takes two parameters and returns bool
            ComparisonHandler<int> intComparison = (a, b) => a > b;
            Console.WriteLine($"Is 10 > 5: {intComparison(10, 5)}");
            // Output: Is 10 > 5: True

            ComparisonHandler<string> stringComparison = (a, b) => a.Length > b.Length;
            Console.WriteLine($"Is 'Hello' > 'Hi': {stringComparison("Hello", "Hi")}");
            // Output: Is 'Hello' > 'Hi': True

            // Processor delegate
            TransformationHandler<int> doubleValue = x => x * 2;
            Console.WriteLine($"Doubled: {doubleValue(7)}");
            // Output: Doubled: 14

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: Real-World - Generic Validation
            // ═══════════════════════════════════════════════════════════

            var validator = new Validator<int>();
            Console.WriteLine($"Is 10 valid: {validator.Validate(10, x => x > 0 && x < 100)}");
            // Output: Is 10 valid: True

            Console.WriteLine($"Is 150 valid: {validator.Validate(150, x => x > 0 && x < 100)}");
            // Output: Is 150 valid: False

            var stringValidator = new Validator<string>();
            Console.WriteLine($"Is 'Hello' valid: {stringValidator.Validate(\"Hello\", s => !string.IsNullOrEmpty(s))}");
            // Output: Is 'Hello' valid: True

            // ═══════════════════════════════════════════════════════════
            // SECTION 6: Real-World - Generic Pipeline
            // ═══════════════════════════════════════════════════════════

            var pipeline = new Pipeline<string>();
            pipeline.AddStep(s => s.ToUpper());
            pipeline.AddStep(s => s.Replace(" ", "_"));
            pipeline.AddStep(s => s + "_PROCESSED");

            string input = "hello world";
            string output = pipeline.Execute(input);
            Console.WriteLine($"Pipeline result: {output}");
            // Output: Pipeline result: HELLO_WORLD_PROCESSED

            var intPipeline = new Pipeline<int>();
            intPipeline.AddStep(x => x + 1);
            intPipeline.AddStep(x => x * 2);
            intPipeline.AddStep(x => x - 5);

            int intInput = 10;
            int intOutput = intPipeline.Execute(intInput);
            Console.WriteLine($"Int pipeline: {intInput} -> {intOutput}");
            // Output: Int pipeline: 10 -> 21

            Console.WriteLine("\n=== Generic Methods Part 2 Complete ===");
        }

        // ═══════════════════════════════════════════════════════════
        // Generic Method with class Constraint
        // ═══════════════════════════════════════════════════════════

        class Container<T> where T : class
        {
            private T _value;

            public void SetValue(T value) => _value = value;
            public T GetValue() => _value;
        }

        // ═══════════════════════════════════════════════════════════
        // Generic Method with struct Constraint
        // ═══════════════════════════════════════════════════════════

        // Generic method with struct constraint for value types
        static T CalculateSquare<T>(T number) where T : struct
        {
            dynamic d = number;
            return (T)(d * d);
        }

        // ═══════════════════════════════════════════════════════════
        // Generic Method with new() Constraint
        // ═══════════════════════════════════════════════════════════

        // Factory that creates instances using parameterless constructor
        class ObjectFactory
        {
            public T Create<T>() where T : new()
            {
                return new T();
            }
        }

        class Product
        {
            public string Name { get; set; } = "Default Product";
        }

        // ═══════════════════════════════════════════════════════════
        // Generic Method with Base Class Constraint
        // ═══════════════════════════════════════════════════════════

        abstract class Animal2
        {
            public string Name { get; set; }
            public abstract void Speak();
        }

        class Dog2 : Animal2
        {
            public Dog2(string name) => Name = name;
            public override void Speak() => Console.WriteLine($"{Name} says: Woof!");
        }

        // Method that works only with Animal or derived types
        static void MakeDogsSpeak<T>(List<T> dogs) where T : Animal2
        {
            foreach (var dog in dogs)
            {
                dog.Speak();
            }
        }

        // ═══════════════════════════════════════════════════════════
        // Generic Method with Interface Constraint
        // ═══════════════════════════════════════════════════════════

        interface IShape2
        {
            double Area { get; }
        }

        class Rectangle2 : IShape2
        {
            public double Width { get; }
            public double Height { get; }

            public Rectangle2(double width, double height)
            {
                Width = width;
                Height = height;
            }

            public double Area => Width * Height;
        }

        // Calculate total area of shapes
        static double CalculateTotalArea<T>(List<T> shapes) where T : IShape2
        {
            double total = 0;
            foreach (var shape in shapes)
            {
                total += shape.Area;
            }
            return total;
        }

        // ═══════════════════════════════════════════════════════════
        // Custom Generic Delegates
        // ═══════════════════════════════════════════════════════════

        // Generic delegate for comparison operations
        delegate bool ComparisonHandler<T>(T first, T second);

        // Generic delegate for transformation operations
        delegate TOutput TransformationHandler<TInput, TOutput>(TInput input);

        // ═══════════════════════════════════════════════════════════
        // Real-World: Generic Validator
        // ═══════════════════════════════════════════════════════════

        class Validator<T>
        {
            public bool Validate(T value, Func<T, bool> predicate)
            {
                return predicate(value);
            }
        }

        // ═══════════════════════════════════════════════════════════
        // Real-World: Generic Pipeline
        // ═══════════════════════════════════════════════════════════

        class Pipeline<T>
        {
            private List<Func<T, T>> _steps = new List<Func<T, T>>();

            public void AddStep(Func<T, T> step)
            {
                _steps.Add(step);
            }

            public T Execute(T input)
            {
                T result = input;
                foreach (var step in _steps)
                {
                    result = step(result);
                }
                return result;
            }
        }
    }
}