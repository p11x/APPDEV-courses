/*
 * ============================================================
 * TOPIC     : Advanced OOP
 * SUBTOPIC  : Generics - Generic Constraints Part 2
 * FILE      : GenericConstraints_Part2.cs
 * PURPOSE   : Teaches advanced constraint combinations, where T : U,
 *            and practical constraint patterns
 * ============================================================
 */

using System;
using System.Collections.Generic;

namespace CSharp_MasterGuide._03_Advanced_OOP._01_Generics
{
    class GenericConstraints_Part2
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Generic Constraints Part 2 ===\n");

            // ═══════════════════════════════════════════════════════════
            // SECTION 1: Combining Multiple Constraints
            // ═══════════════════════════════════════════════════════════

            // Constraint: class AND new() - reference type with parameterless constructor
            var refFactory = new ReferenceFactory<string>();
            var str = refFactory.CreateInstance();
            Console.WriteLine($"Created: '{str}'");
            // Output: Created: ''

            // Constraint: struct AND IComparable - value type that can be compared
            var comparer = new ComparableSorter<int>();
            int[] numbers = { 5, 2, 8, 1, 9 };
            comparer.Sort(numbers);
            Console.WriteLine($"Sorted numbers: {string.Join(", ", numbers)}");
            // Output: Sorted numbers: 1, 2, 5, 8, 9

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: Where T : U (Type Parameter Constraint)
            // ═══════════════════════════════════════════════════════════

            // T must be same as or derived from U
            var wrapper = new TypeWrapper<int, int>(100);
            Console.WriteLine($"Wrapped value: {wrapper.GetValue()}");
            // Output: Wrapped value: 100

            // Using base-derived relationship
            var baseWrapper = new TypeWrapper<Animal4, Animal4>(new Dog4("Rex"));
            Console.WriteLine($"Animal type: {baseWrapper.GetValue().GetType().Name}");
            // Output: Animal type: Dog4

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: notnull Constraint
            // ═══════════════════════════════════════════════════════════

            // where T : notnull - T cannot be null (C# 7.1+)
            var notNullHandler = new NotNullHandler<string>();
            notNullHandler.Process("Hello");
            // Output: Processing: Hello

            // Works with value types too
            var intHandler = new NotNullHandler<int>();
            intHandler.Process(42);
            // Output: Processing: 42

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: Unmanaged Constraint
            // ═══════════════════════════════════════════════════════════

            // where T : unmanaged - T must be unmanaged type (primitive-like)
            var byteOps = new UnmanagedOperations<byte>();
            byte sum = byteOps.Add(100, 50);
            Console.WriteLine($"Byte sum: {sum}");
            // Output: Byte sum: 150

            var intOps = new UnmanagedOperations<int>();
            int intSum = intOps.Add(1000, 2000);
            Console.WriteLine($"Int sum: {intSum}");
            // Output: Int sum: 3000

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: Practical Constraint Patterns
            // ═══════════════════════════════════════════════════════════

            // Pattern 1: Entity with ID
            var entityRepo = new EntityRepository<Product4>();
            var product = new Product4 { Name = "Laptop", Price = 999.99m };
            entityRepo.Save(product);
            Console.WriteLine($"Saved product ID: {product.Id}");
            // Output: Saved product ID: 1

            // Pattern 2: Cloneable objects
            var original = new CloneableObject { Value = "Original" };
            var cloned = original.Clone();
            cloned.Value = "Cloned";
            Console.WriteLine($"Original: {original.Value}, Cloned: {cloned.Value}");
            // Output: Original: Original, Cloned: Cloned

            // Pattern 3: Disposable resources
            var resourceManager = new ResourceManager<DatabaseConnection>();
            var connection = resourceManager.Acquire();
            Console.WriteLine($"Acquired: {connection.GetType().Name}");
            // Output: Acquired: DatabaseConnection

            Console.WriteLine("\n=== Generic Constraints Part 2 Complete ===");
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Combining class and new() Constraints
    // ═══════════════════════════════════════════════════════════

    // Reference type with parameterless constructor
    class ReferenceFactory<T> where T : class, new()
    {
        public T CreateInstance()
        {
            return new T();
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Combining struct and IComparable Constraints
    // ═══════════════════════════════════════════════════════════

    // Value type that can be compared
    class ComparableSorter<T> where T : struct, IComparable<T>
    {
        public void Sort(T[] items)
        {
            Array.Sort(items);
        }

        public T FindMaximum(T[] items)
        {
            if (items == null || items.Length == 0)
                return default(T);

            T max = items[0];
            for (int i = 1; i < items.Length; i++)
            {
                if (items[i].CompareTo(max) > 0)
                    max = items[i];
            }
            return max;
        }

        public T FindMinimum(T[] items)
        {
            if (items == null || items.Length == 0)
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

    // ═══════════════════════════════════════════════════════════
    // Where T : U - Type Parameter Constraint
    // ═══════════════════════════════════════════════════════════

    // TItem must be same as or derived from TValue
    class TypeWrapper<TItem, TValue> where TItem : TValue
    {
        private TItem _value;

        public TypeWrapper(TItem value)
        {
            _value = value;
        }

        public TItem GetValue() => _value;

        public void SetValue(TItem value)
        {
            _value = value;
        }
    }

    // Base class for type constraint demonstration
    class Animal4
    {
        public string Name { get; set; }
        public Animal4(string name) => Name = name;
    }

    class Dog4 : Animal4
    {
        public Dog4(string name) : base(name) { }
    }

    // ═══════════════════════════════════════════════════════════
    // notnull Constraint
    // ═══════════════════════════════════════════════════════════

    // T must not be null (works with nullable reference types)
    class NotNullHandler<T> where T : notnull
    {
        public void Process(T value)
        {
            Console.WriteLine($"Processing: {value}");
        }

        public T ValidateAndReturn(T value)
        {
            // No need to check for null due to notnull constraint
            return value;
        }
    }

    // ═══════════════════════════════════════════════════════════
    // unmanaged Constraint
    // ═══════════════════════════════════════════════════════════

    // T must be an unmanaged type (primitive, struct, enum, etc.)
    class UnmanagedOperations<T> where T : unmanaged
    {
        public T Add(T a, T b)
        {
            // Using pointer arithmetic for unmanaged types
            unsafe
            {
                T result;
                T* ptrA = &a;
                T* ptrB = &b;
                result = (T)((dynamic)ptrA[0] + (dynamic)ptrB[0]);
                return result;
            }
        }

        // Simpler version using dynamic
        public T AddSimple(T a, T b)
        {
            dynamic dA = a;
            dynamic dB = b;
            return (T)(dA + dB);
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Practical Pattern: Entity with ID
    // ═══════════════════════════════════════════════════════════

    // Base interface for entities with ID
    interface IEntity
    {
        int Id { get; set; }
    }

    // Repository for entities
    class EntityRepository<T> where T : class, IEntity, new()
    {
        private List<T> _entities = new List<T>();
        private int _nextId = 1;

        public void Save(T entity)
        {
            if (entity.Id == 0)
            {
                entity.Id = _nextId++;
            }
            _entities.Add(entity);
        }

        public T GetById(int id)
        {
            return _entities.Find(e => e.Id == id);
        }

        public List<T> GetAll()
        {
            return new List<T>(_entities);
        }
    }

    // Product implementing IEntity
    class Product4 : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }

    // ═══════════════════════════════════════════════════════════
    // Practical Pattern: Cloneable
    // ═══════════════════════════════════════════════════════════

    // Interface for cloneable objects
    interface ICloneable<T>
    {
        T Clone();
    }

    // Cloneable implementation
    class CloneableObject : ICloneable<CloneableObject>
    {
        public string Value { get; set; }

        public CloneableObject Clone()
        {
            // Create a new instance with the same values
            return new CloneableObject { Value = this.Value };
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Practical Pattern: Disposable Resources
    // ═══════════════════════════════════════════════════════════

    // Interface for disposable resources
    interface IResource
    {
        void Dispose();
    }

    // Resource manager for IDisposable types
    class ResourceManager<T> where T : class, IDisposable, new()
    {
        private T _resource;

        public T Acquire()
        {
            if (_resource == null)
            {
                _resource = new T();
            }
            return _resource;
        }

        public void Release()
        {
            _resource?.Dispose();
            _resource = null;
        }
    }

    // Sample database connection
    class DatabaseConnection : IDisposable
    {
        public void Dispose()
        {
            Console.WriteLine("Database connection disposed");
        }
    }
}