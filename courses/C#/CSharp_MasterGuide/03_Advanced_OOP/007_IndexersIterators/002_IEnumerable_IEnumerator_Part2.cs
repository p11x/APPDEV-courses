/*
 * TOPIC: Indexers and Iterators
 * SUBTOPIC: IEnumerable and IEnumerator Part 2
 * FILE: IEnumerable_IEnumerator_Part2.cs
 * PURPOSE: Demonstrate implementing custom enumerables and advanced enumeration patterns
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace CSharp_MasterGuide._03_Advanced_OOP._07_IndexersIterators
{
    // Custom enumerable with nested iterator class
    public class ReversedList<T> : IEnumerable<T>
    {
        private List<T> _items = new List<T>();

        public void Add(T item) => _items.Add(item);
        public int Count => _items.Count;

        public IEnumerator<T> GetEnumerator()
        {
            return new ReverseEnumerator<T>(_items);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class ReverseEnumerator<T> : IEnumerator<T>
    {
        private List<T> _items;
        private int _position;

        public ReverseEnumerator(List<T> items)
        {
            _items = items;
            _position = items.Count;
        }

        public T Current
        {
            get
            {
                if (_position >= 0 && _position < _items.Count)
                    return _items[_position];
                return default(T);
            }
        }

        public void Dispose() { }

        public bool MoveNext()
        {
            _position--;
            return _position >= 0;
        }

        public void Reset()
        {
            _position = _items.Count;
        }

        object IEnumerator.Current => Current;
    }

    // Circular iterator - loops through collection infinitely
    public class CircularCollection<T> : IEnumerable<T>
    {
        private List<T> _items = new List<T>();

        public void Add(T item) => _items.Add(item);
        public int Count => _items.Count;

        public IEnumerator<T> GetEnumerator()
        {
            return new CircularEnumerator<T>(_items);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class CircularEnumerator<T> : IEnumerator<T>
    {
        private List<T> _items;
        private int _position = -1;
        private int _passes = 0;
        private const int MaxPasses = 3; // Limit iterations

        public CircularEnumerator(List<T> items)
        {
            _items = items;
        }

        public T Current
        {
            get
            {
                if (_items.Count == 0)
                    return default(T);
                return _items[_position % _items.Count];
            }
        }

        public void Dispose() { }

        public bool MoveNext()
        {
            _position++;
            if (_position / _items.Count >= MaxPasses)
                return false;
            return true;
        }

        public void Reset()
        {
            _position = -1;
            _passes = 0;
        }

        object IEnumerator.Current => Current;
    }

    // Filtered enumerable with predicate
    public class FilteredCollection<T> : IEnumerable<T>
    {
        private List<T> _items = new List<T>();
        private Func<T, bool> _filter;

        public FilteredCollection(Func<T, bool> filter)
        {
            _filter = filter;
        }

        public void Add(T item) => _items.Add(item);

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var item in _items)
            {
                if (_filter(item))
                    yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    // Range implementation with custom enumerator
    public class NumericRange : IEnumerable<int>
    {
        private int _start;
        private int _end;
        private int _step;

        public NumericRange(int start, int end, int step = 1)
        {
            _start = start;
            _end = end;
            _step = step;
        }

        public IEnumerator<int> GetEnumerator()
        {
            for (int i = _start; i <= _end; i += _step)
            {
                yield return i;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    // Real-world: Product catalog with filtering
    public class Product
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public bool InStock { get; set; }

        public Product(string name, string category, decimal price, bool inStock)
        {
            Name = name;
            Category = category;
            Price = price;
            InStock = inStock;
        }

        public override string ToString() => $"{Name} - ${Price:N2} ({Category})";
    }

    public class ProductCatalog : IEnumerable<Product>
    {
        private List<Product> _products = new List<Product>();

        public void Add(Product product) => _products.Add(product);

        public IEnumerator<Product> GetEnumerator()
        {
            return _products.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerable<Product> ByCategory(string category)
        {
            foreach (var p in _products)
            {
                if (p.Category == category)
                    yield return p;
            }
        }

        public IEnumerable<Product> InPriceRange(decimal min, decimal max)
        {
            foreach (var p in _products)
            {
                if (p.Price >= min && p.Price <= max)
                    yield return p;
            }
        }

        public IEnumerable<Product> Available()
        {
            foreach (var p in _products)
            {
                if (p.InStock)
                    yield return p;
            }
        }
    }

    // Real-world: Cached enumerable with lazy evaluation
    public class LazyCachedEnumerable<T> : IEnumerable<T>
    {
        private Func<IEnumerable<T>> _factory;
        private List<T> _cache;
        private bool _cached = false;

        public LazyCachedEnumerable(Func<IEnumerable<T>> factory)
        {
            _factory = factory;
            _cache = new List<T>();
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (!_cached)
            {
                foreach (var item in _factory())
                {
                    _cache.Add(item);
                }
                _cached = true;
            }

            return _cache.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void ClearCache()
        {
            _cache.Clear();
            _cached = false;
        }
    }

    // Real-world: Composite collection (multiple collections as one)
    public class CompositeCollection<T> : IEnumerable<T>
    {
        private List<IEnumerable<T>> _collections = new List<IEnumerable<T>>();

        public void AddCollection(IEnumerable<T> collection)
        {
            _collections.Add(collection);
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var collection in _collections)
            {
                foreach (var item in collection)
                {
                    yield return item;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class IEnumerableIEnumeratorPart2
    {
        public static void Main()
        {
            Console.WriteLine("=== IEnumerable/IEnumerator Part 2 Demo ===\n");

            // Example 1: Reversed enumeration
            Console.WriteLine("--- Reversed List ---");
            var reversed = new ReversedList<string>();
            reversed.Add("First");
            reversed.Add("Second");
            reversed.Add("Third");

            Console.WriteLine("Normal order:");
            foreach (var item in reversed)
                Console.WriteLine($"  {item}");
            // Output: First, Second, Third

            Console.WriteLine("Reversed order:");
            // Note: ReversedList already iterates in reverse
            foreach (var item in reversed)
                Console.WriteLine($"  {item}");
            // Output: Third, Second, First
            Console.WriteLine();

            // Example 2: Circular collection
            Console.WriteLine("--- Circular Collection ---");
            var circular = new CircularCollection<string>();
            circular.Add("A");
            circular.Add("B");
            circular.Add("C");

            int count = 0;
            foreach (var item in circular)
            {
                Console.WriteLine($"  {item}");
                count++;
                if (count >= 9) break; // Limit output
            }
            // Output: A, B, C, A, B, C, A, B, C (3 passes)
            Console.WriteLine();

            // Example 3: Filtered collection
            Console.WriteLine("--- Filtered Collection ---");
            var numbers = new FilteredCollection<int>(n => n % 2 == 0);
            numbers.Add(1);
            numbers.Add(2);
            numbers.Add(3);
            numbers.Add(4);
            numbers.Add(5);
            numbers.Add(6);

            Console.WriteLine("Even numbers only:");
            foreach (var n in numbers)
                Console.WriteLine($"  {n}");
            // Output: 2, 4, 6
            Console.WriteLine();

            // Example 4: Numeric range
            Console.WriteLine("--- Numeric Range ---");
            var range = new NumericRange(1, 10, 2);
            Console.Write("Range 1-10 step 2: ");
            foreach (var n in range)
                Console.Write($"{n} ");
            Console.WriteLine();
            // Output: 1 3 5 7 9
            Console.WriteLine();

            // Example 5: Real-world - Product catalog
            Console.WriteLine("--- Real-World: Product Catalog ---");
            var catalog = new ProductCatalog();
            catalog.Add(new Product("Laptop", "Electronics", 999.99m, true));
            catalog.Add(new Product("Phone", "Electronics", 599.99m, true));
            catalog.Add(new Product("Desk", "Furniture", 299.99m, false));
            catalog.Add(new Product("Chair", "Furniture", 199.99m, true));
            catalog.Add(new Product("Tablet", "Electronics", 399.99m, false));

            Console.WriteLine("Electronics:");
            foreach (var p in catalog.ByCategory("Electronics"))
                Console.WriteLine($"  {p}");
            // Output: Laptop, Phone, Tablet

            Console.WriteLine("\nIn stock ($200-$600):");
            foreach (var p in catalog.InPriceRange(200, 600))
                Console.WriteLine($"  {p}");
            // Output: Phone, Chair

            Console.WriteLine("\nAvailable items:");
            foreach (var p in catalog.Available())
                Console.WriteLine($"  {p}");
            // Output: Laptop, Phone, Chair
            Console.WriteLine();

            // Example 6: Lazy cached enumerable
            Console.WriteLine("--- Lazy Cached Enumerable ---");
            int factoryCalls = 0;
            var lazyCached = new LazyCachedEnumerable<int>(() =>
            {
                factoryCalls++;
                Console.WriteLine($"  Factory called (call #{factoryCalls})");
                return new[] { 1, 2, 3 };
            });

            Console.WriteLine("First iteration:");
            foreach (var n in lazyCached)
                Console.WriteLine($"  {n}");

            Console.WriteLine("Second iteration (from cache):");
            foreach (var n in lazyCached)
                Console.WriteLine($"  {n}");
            // Output: Factory called once, then cached
            Console.WriteLine();

            // Example 7: Composite collection
            Console.WriteLine("--- Composite Collection ---");
            var list1 = new List<string> { "A1", "A2" };
            var list2 = new List<string> { "B1", "B2" };
            var list3 = new List<string> { "C1", "C2" };

            var composite = new CompositeCollection<string>();
            composite.AddCollection(list1);
            composite.AddCollection(list2);
            composite.AddCollection(list3);

            Console.WriteLine("Combined collections:");
            foreach (var item in composite)
                Console.WriteLine($"  {item}");
            // Output: A1, A2, B1, B2, C1, C2
        }
    }
}
