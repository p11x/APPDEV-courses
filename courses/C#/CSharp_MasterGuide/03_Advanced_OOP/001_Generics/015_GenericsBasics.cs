/*
 * ============================================================
 * TOPIC     : Advanced OOP
 * SUBTOPIC  : Generics - Generic Basics
 * FILE      : GenericsBasics.cs
 * PURPOSE   : Teaches generic programming fundamentals in C#,
 *            type parameters, generic classes, methods, and
 *            constraints
 * ============================================================
 */

using System; // Core System namespace

namespace CSharp_MasterGuide._03_Advanced_OOP._01_Generics
{
    class GenericsBasics
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Generics Basics in C# ===\n");

            // ═══════════════════════════════════════════════════════════
            // SECTION 1: What are Generics?
            // ═══════════════════════════════════════════════════════════

            // Generics allow you to write reusable, type-safe code
            // Instead of writing separate code for each type, you write
            // code that works with any type
            
            // ── EXAMPLE 1: Non-Generic vs Generic ───────────────────────
            // Before generics - had to use object or create separate classes
            var stringBox = new BoxString("Hello");
            var intBox = new BoxInt(42);
            
            Console.WriteLine($"StringBox: {stringBox.Value}");
            Console.WriteLine($"IntBox: {intBox.Value}");
            
            // With generics - one class works for all types
            var genericStringBox = new Box<string>("World");
            var genericIntBox = new Box<int>(100);
            var genericDoubleBox = new Box<double>(3.14);
            
            Console.WriteLine($"\nGeneric String: {genericStringBox.Value}");
            Console.WriteLine($"Generic Int: {genericIntBox.Value}");
            Console.WriteLine($"Generic Double: {genericDoubleBox.Value}");

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: Generic Class
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Generic Container ───────────────────────────
            var stack = new Stack<string>(3);
            stack.Push("First");
            stack.Push("Second");
            stack.Push("Third");
            
            Console.WriteLine($"\nStack Pop: {stack.Pop()}");
            Console.WriteLine($"Stack Pop: {stack.Pop()}");
            Console.WriteLine($"Stack Peek: {stack.Peek()}");

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: Generic Methods
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Generic Method ───────────────────────────────
            int a = 5, b = 10;
            Swap(ref a, ref b);
            Console.WriteLine($"\nAfter swap: a={a}, b={b}");
            
            string s1 = "Hello", s2 = "World";
            Swap(ref s1, ref s2);
            Console.WriteLine($"After swap: s1={s1}, s2={s2}");

            // ── EXAMPLE 2: Generic Return Type ─────────────────────────
            var array = new int[] { 1, 2, 3, 4, 5 };
            var first = GetFirst(array);
            Console.WriteLine($"\nFirst element: {first}");

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: Generic Constraints
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Class Constraint ─────────────────────────────
            var calculator = new NumericCalculator();
            Console.WriteLine($"\nSum: {calculator.Add(5, 3)}");
            Console.WriteLine($"Multiply: {calculator.Multiply(4, 7)}");
            
            // ── EXAMPLE 2: New Constraint ──────────────────────────────
            var factory = new Factory<Product>();
            var product = factory.Create();
            Console.WriteLine($"Created: {product.GetType().Name}");

            // ── EXAMPLE 3: Base Class Constraint ───────────────────────
            var animals = new List<AnimalGeneric> { new DogGeneric(), new CatGeneric() };
            ProcessAnimals(animals);

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: Multiple Type Parameters
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Dictionary with Two Types ────────────────────
            var dict = new Dictionary<string, int>();
            dict["One"] = 1;
            dict["Two"] = 2;
            
            Console.WriteLine($"\nDictionary: {dict["One"]}, {dict["Two"]}");

            // ── EXAMPLE 1: Generic Pair ─────────────────────────────────
            var pair = new Pair<string, int>("Age", 25);
            Console.WriteLine($"Pair: {pair.Key} = {pair.Value}");

            Console.WriteLine("\n=== Generics Basics Complete ===");
        }

        // Generic method
        static void Swap<T>(ref T a, ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }

        // Generic method returning first element
        static T GetFirst<T>(T[] array)
        {
            return array.Length > 0 ? array[0] : default(T);
        }

        // Generic method with constraint
        static void ProcessAnimals<T>(List<T> animals) where T : AnimalGeneric
        {
            foreach (var animal in animals)
            {
                animal.Speak();
            }
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Non-Generic Classes (Old Way)
    // ═══════════════════════════════════════════════════════════

    class BoxString
    {
        public string Value { get; set; }
        public BoxString(string value) => Value = value;
    }

    class BoxInt
    {
        public int Value { get; set; }
        public BoxInt(int value) => Value = value;
    }

    // ═══════════════════════════════════════════════════════════
    // Generic Class
    // ═══════════════════════════════════════════════════════════

    class Box<T>
    {
        public T Value { get; set; }
        
        public Box(T value)
        {
            Value = value;
        }
    }

    // Generic Stack
    class Stack<T>
    {
        private T[] _items;
        private int _top;
        
        public Stack(int capacity)
        {
            _items = new T[capacity];
            _top = -1;
        }
        
        public void Push(T item)
        {
            if (_top < _items.Length - 1)
            {
                _items[++_top] = item;
            }
        }
        
        public T Pop()
        {
            return _top >= 0 ? _items[_top--] : default(T);
        }
        
        public T Peek()
        {
            return _top >= 0 ? _items[_top] : default(T);
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Generic with Constraints
    // ═══════════════════════════════════════════════════════════

    // where T : struct - must be value type
    // where T : class - must be reference type
    // where T : new() - must have parameterless constructor
    // where T : BaseClass - must inherit from BaseClass

    class NumericCalculator<T> where T : struct, INumber<T>
    {
        public T Add(T a, T b) => a + b;
        public T Multiply(T a, T b) => a * b;
    }

    // Use class constraint to create instance
    class Factory<T> where T : new()
    {
        public T Create()
        {
            return new T();
        }
    }

    class Product
    {
        public string Name { get; set; } = "Product";
    }

    // Base class for constraint example
    class AnimalGeneric
    {
        public virtual void Speak() => Console.WriteLine("  Animal speaks");
    }

    class DogGeneric : AnimalGeneric
    {
        public override void Speak() => Console.WriteLine("  Dog says: Woof!");
    }

    class CatGeneric : AnimalGeneric
    {
        public override void Speak() => Console.WriteLine("  Cat says: Meow!");
    }

    // ═══════════════════════════════════════════════════════════
    // Multiple Type Parameters
    // ═══════════════════════════════════════════════════════════

    class Pair<TKey, TValue>
    {
        public TKey Key { get; set; }
        public TValue Value { get; set; }
        
        public Pair(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }
}