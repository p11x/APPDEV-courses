/*
 * ============================================================
 * TOPIC     : Advanced OOP
 * SUBTOPIC  : Generics - Generic Constraints
 * FILE      : GenericConstraints.cs
 * PURPOSE   : Teaches generic constraints: class, struct, new(),
 *            base class, and interface constraints in C#
 * ============================================================
 */

using System;
using System.Collections.Generic;

namespace CSharp_MasterGuide._03_Advanced_OOP._01_Generics
{
    class GenericConstraints
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Generic Constraints in C# ===\n");

            // ═══════════════════════════════════════════════════════════
            // SECTION 1: class Constraint (Reference Type)
            // ═══════════════════════════════════════════════════════════

            // where T : class - T must be a reference type
            var holder = new ObjectHolder<string>();
            holder.SetValue("Hello");
            Console.WriteLine($"Value: {holder.GetValue()}");
            // Output: Value: Hello

            // Works with any class type
            var personHolder = new ObjectHolder<Person3>();
            personHolder.SetValue(new Person3("Alice"));
            Console.WriteLine($"Person: {personHolder.GetValue().Name}");
            // Output: Person: Alice

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: struct Constraint (Value Type)
            // ═══════════════════════════════════════════════════════════

            // where T : struct - T must be a value type
            var numericOps = new NumericOperations<int>();
            int sum = numericOps.Add(10, 20);
            Console.WriteLine($"Sum: {sum}");
            // Output: Sum: 30

            var doubleOps = new NumericOperations<double>();
            double product = doubleOps.Multiply(2.5, 4.0);
            Console.WriteLine($"Product: {product}");
            // Output: Product: 10

            // Works with enums too
            var statusOps = new EnumHelper<Status>();
            Console.WriteLine($"Default status: {statusOps.GetDefault()}");
            // Output: Default status: Pending

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: new() Constraint (Parameterless Constructor)
            // ═══════════════════════════════════════════════════════════

            // where T : new() - T must have a parameterless constructor
            var factory = new Factory<string>();
            var instance = factory.Create();
            Console.WriteLine($"Created instance type: {instance.GetType().Name}");
            // Output: Created instance type: String

            // Works with custom classes that have parameterless constructors
            var productFactory = new Factory<Product3>();
            var newProduct = productFactory.Create();
            Console.WriteLine($"Created product: {newProduct.GetType().Name}");
            // Output: Created product: Product3

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: Base Class Constraint
            // ═══════════════════════════════════════════════════════════

            // where T : BaseClass - T must inherit from BaseClass
            var animals = new List<Animal3> 
            { 
                new Dog3("Rex"), 
                new Cat3("Whiskers") 
            };
            
            var processor = new AnimalProcessor();
            processor.ProcessAnimals(animals);
            // Output: Rex says: Woof!
            // Output: Whiskers says: Meow!

            // Works with derived classes
            var dogs = new List<Dog3> { new Dog3("Buddy") };
            processor.ProcessAnimals(dogs);
            // Output: Buddy says: Woof!

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: Interface Constraint
            // ═══════════════════════════════════════════════════════════

            // where T : IInterface - T must implement the interface
            var shapes = new List<Rectangle3> 
            { 
                new Rectangle3(5, 3), 
                new Rectangle3(4, 4) 
            };
            
            var calculator = new AreaCalculator();
            double totalArea = calculator.CalculateTotal(shapes);
            Console.WriteLine($"Total area: {totalArea}");
            // Output: Total area: 41

            // Works with different shape types
            var circles = new List<Circle3> 
            { 
                new Circle3(2), 
                new Circle3(3) 
            };
            double circleArea = calculator.CalculateTotal(circles);
            Console.WriteLine($"Circle total area: {circleArea}");
            // Output: Circle total area: 40.84

            // ═══════════════════════════════════════════════════════════
            // SECTION 6: Combining Multiple Constraints
            // ═══════════════════════════════════════════════════════════

            // Multiple constraints: struct and interface
            var dataOps = new DataOperations<int>();
            int[] data = { 5, 2, 8, 1, 9 };
            dataOps.Sort(data);
            Console.WriteLine($"Sorted: {string.Join(", ", data)}");
            // Output: Sorted: 1, 2, 5, 8, 9

            double[] doubleData = { 3.1, 1.5, 2.8, 0.9 };
            dataOps.Sort(doubleData);
            Console.WriteLine($"Sorted doubles: {string.Join(", ", doubleData)}");
            // Output: Sorted doubles: 0.9, 1.5, 2.8, 3.1

            Console.WriteLine("\n=== Generic Constraints Complete ===");
        }
    }

    // ═══════════════════════════════════════════════════════════
    // class Constraint - Reference Type Only
    // ═══════════════════════════════════════════════════════════

    // ObjectHolder accepts only reference types (classes)
    class ObjectHolder<T> where T : class
    {
        private T _value;

        public void SetValue(T value) => _value = value;
        public T GetValue() => _value;
    }

    // Person class for demonstration
    class Person3
    {
        public string Name { get; set; }
        public Person3(string name) => Name = name;
        public Person3() { } // Parameterless constructor for new() constraint
    }

    // ═══════════════════════════════════════════════════════════
    // struct Constraint - Value Type Only
    // ═══════════════════════════════════════════════════════════

    // NumericOperations works with value types (int, double, float, etc.)
    class NumericOperations<T> where T : struct
    {
        public T Add(T a, T b)
        {
            dynamic dA = a;
            dynamic dB = b;
            return (T)(dA + dB);
        }

        public T Multiply(T a, T b)
        {
            dynamic dA = a;
            dynamic dB = b;
            return (T)(dA * b);
        }

        public T Subtract(T a, T b)
        {
            dynamic dA = a;
            dynamic dB = b;
            return (T)(dA - dB);
        }
    }

    // Enum for demonstration
    enum Status { Pending, InProgress, Completed }

    // Helper for enum types (also struct constraint)
    class EnumHelper<T> where T : struct
    {
        public T GetDefault()
        {
            return default(T);
        }
    }

    // ═══════════════════════════════════════════════════════════
    // new() Constraint - Parameterless Constructor
    // ═══════════════════════════════════════════════════════════

    // Factory can create instances of any type with parameterless constructor
    class Factory<T> where T : new()
    {
        public T Create()
        {
            return new T();
        }
    }

    // Product class with parameterless constructor
    class Product3
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        
        // Parameterless constructor for new() constraint
        public Product3() { }
        
        public Product3(string name, decimal price)
        {
            Name = name;
            Price = price;
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Base Class Constraint
    // ═══════════════════════════════════════════════════════════

    // Base animal class
    abstract class Animal3
    {
        public string Name { get; set; }
        
        protected Animal3(string name)
        {
            Name = name;
        }

        public abstract void Speak();
    }

    // Derived class: Dog
    class Dog3 : Animal3
    {
        public Dog3(string name) : base(name) { }

        public override void Speak()
        {
            Console.WriteLine($"{Name} says: Woof!");
        }
    }

    // Derived class: Cat
    class Cat3 : Animal3
    {
        public Cat3(string name) : base(name) { }

        public override void Speak()
        {
            Console.WriteLine($"{Name} says: Meow!");
        }
    }

    // Processor that works with Animal types
    class AnimalProcessor
    {
        public void ProcessAnimals<T>(List<T> animals) where T : Animal3
        {
            foreach (var animal in animals)
            {
                animal.Speak();
            }
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Interface Constraint
    // ═══════════════════════════════════════════════════════════

    // IArea interface for shapes
    interface IArea
    {
        double CalculateArea();
    }

    // Rectangle implementation
    class Rectangle3 : IArea
    {
        public double Width { get; }
        public double Height { get; }

        public Rectangle3(double width, double height)
        {
            Width = width;
            Height = height;
        }

        public double CalculateArea() => Width * Height;
    }

    // Circle implementation
    class Circle3 : IArea
    {
        public double Radius { get; }

        public Circle3(double radius)
        {
            Radius = radius;
        }

        public double CalculateArea() => Math.PI * Radius * Radius;
    }

    // Calculator that works with IArea implementations
    class AreaCalculator
    {
        public double CalculateTotal<T>(List<T> shapes) where T : IArea
        {
            double total = 0;
            foreach (var shape in shapes)
            {
                total += shape.CalculateArea();
            }
            return total;
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Combining Multiple Constraints
    // ═══════════════════════════════════════════════════════════

    // T must be struct AND implement IComparable<T>
    class DataOperations<T> where T : struct, IComparable<T>
    {
        public void Sort(T[] items)
        {
            Array.Sort(items);
        }

        public T FindMin(T[] items)
        {
            if (items.Length == 0)
                return default(T);
            
            T min = items[0];
            for (int i = 1; i < items.Length; i++)
            {
                if (items[i].CompareTo(min) < 0)
                    min = items[i];
            }
            return min;
        }
    }
}