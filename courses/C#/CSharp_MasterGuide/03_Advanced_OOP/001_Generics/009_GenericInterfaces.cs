/*
 * ============================================================
 * TOPIC     : Advanced OOP
 * SUBTOPIC  : Generics - Generic Interfaces
 * FILE      : GenericInterfaces.cs
 * PURPOSE   : Teaches generic interface definition, implementation,
 *            and interface inheritance with generics
 * ============================================================
 */

using System;
using System.Collections.Generic;

namespace CSharp_MasterGuide._03_Advanced_OOP._01_Generics
{
    class GenericInterfaces
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Generic Interfaces in C# ===\n");

            // ═══════════════════════════════════════════════════════════
            // SECTION 1: Defining Generic Interfaces
            // ═══════════════════════════════════════════════════════════

            // Using IContainer<T> generic interface
            IContainer<int> intContainer = new IntContainer(42);
            Console.WriteLine($"Container Value: {intContainer.GetValue()}");
            // Output: Container Value: 42

            IContainer<string> stringContainer = new StringContainer("Hello");
            Console.WriteLine($"String Container: {stringContainer.GetValue()}");
            // Output: String Container: Hello

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: Implementing Generic Interfaces
            // ═══════════════════════════════════════════════════════════

            // Using IRepository<T> interface
            var productRepo = new ProductRepository();
            productRepo.Add(new Product2 { Id = 1, Name = "Laptop" });
            productRepo.Add(new Product2 { Id = 2, Name = "Phone" });
            
            Console.WriteLine($"Products count: {productRepo.GetAll().Count}");
            // Output: Products count: 2

            var product = productRepo.GetById(1);
            Console.WriteLine($"Found: {product?.Name}");
            // Output: Found: Laptop

            // User repository
            var userRepo = new UserRepository();
            userRepo.Add(new User2 { Id = 1, Name = "Alice" });
            
            Console.WriteLine($"Users count: {userRepo.GetAll().Count}");
            // Output: Users count: 1

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: Interface with Multiple Type Parameters
            // ═══════════════════════════════════════════════════════════

            // Using IKeyValueStore<TKey, TValue>
            var cache = new MemoryCache<string, string>();
            cache.Set("session1", "user_data");
            cache.Set("session2", "cart_data");
            
            Console.WriteLine($"Session 1: {cache.Get("session1")}");
            // Output: Session 1: user_data

            Console.WriteLine($"Contains key: {cache.Contains("session1")}");
            // Output: Contains key: True

            // Different type combination
            var userCache = new MemoryCache<int, User2>();
            userCache.Set(1, new User2 { Id = 1, Name = "Bob" });
            Console.WriteLine($"User 1: {userCache.Get(1).Name}");
            // Output: User 1: Bob

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: Real-World - Service Layer Interfaces
            // ═══════════════════════════════════════════════════════════

            // Using IOutputService<T>
            var consoleService = new ConsoleOutputService<string>();
            consoleService.Output("Hello to Console");
            // Output: [CONSOLE] Hello to Console

            var fileService = new FileOutputService<string>();
            fileService.Output("Log entry");
            // Output: [FILE] Log entry

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: Interface Inheritance with Generics
            // ═══════════════════════════════════════════════════════════

            // Using IReadOnlyRepository<T> which extends IRepository<T>
            var readOnlyProductRepo = new ReadOnlyProductRepository();
            readOnlyProductRepo.Add(new Product2 { Id = 1, Name = "Book" });
            
            var allProducts = readOnlyProductRepo.GetAll();
            Console.WriteLine($"All products: {allProducts.Count}");
            // Output: All products: 1

            // ═══════════════════════════════════════════════════════════
            // SECTION 6: Generic Interface with Constraints
            // ═══════════════════════════════════════════════════════════

            // Using ISortable<T> with IComparable constraint
            var sorter = new Sorter<int>();
            int[] numbers = { 5, 2, 8, 1, 9 };
            sorter.Sort(numbers);
            Console.WriteLine($"Sorted: {string.Join(", ", numbers)}");
            // Output: Sorted: 1, 2, 5, 8, 9

            var stringSorter = new Sorter<string>();
            string[] words = { "banana", "apple", "cherry" };
            stringSorter.Sort(words);
            Console.WriteLine($"Sorted words: {string.Join(", ", words)}");
            // Output: Sorted words: apple, banana, cherry

            Console.WriteLine("\n=== Generic Interfaces Complete ===");
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Basic Generic Interface
    // ═══════════════════════════════════════════════════════════

    // Generic interface with single type parameter
    interface IContainer<T>
    {
        T GetValue();
        void SetValue(T value);
    }

    // Implementation for int type
    class IntContainer : IContainer<int>
    {
        private int _value;

        public IntContainer(int value)
        {
            _value = value;
        }

        public int GetValue() => _value;
        public void SetValue(int value) => _value = value;
    }

    // Implementation for string type
    class StringContainer : IContainer<string>
    {
        private string _value;

        public StringContainer(string value)
        {
            _value = value;
        }

        public string GetValue() => _value;
        public void SetValue(string value) => _value = value;
    }

    // ═══════════════════════════════════════════════════════════
    // Generic Interface - Repository Pattern
    // ═══════════════════════════════════════════════════════════

    // Generic repository interface for CRUD operations
    interface IRepository<T>
    {
        void Add(T item);
        void Update(T item);
        void Delete(T item);
        T GetById(int id);
        List<T> GetAll();
    }

    // Product repository implementation
    class ProductRepository : IRepository<Product2>
    {
        private List<Product2> _products = new List<Product2>();
        private int _nextId = 1;

        public void Add(Product2 item)
        {
            item.Id = _nextId++;
            _products.Add(item);
        }

        public void Update(Product2 item)
        {
            var existing = _products.Find(p => p.Id == item.Id);
            if (existing != null)
            {
                existing.Name = item.Name;
            }
        }

        public void Delete(Product2 item)
        {
            _products.RemoveAll(p => p.Id == item.Id);
        }

        public Product2 GetById(int id)
        {
            return _products.Find(p => p.Id == id);
        }

        public List<Product2> GetAll()
        {
            return new List<Product2>(_products);
        }
    }

    // User repository implementation
    class UserRepository : IRepository<User2>
    {
        private List<User2> _users = new List<User2>();
        private int _nextId = 1;

        public void Add(User2 item)
        {
            item.Id = _nextId++;
            _users.Add(item);
        }

        public void Update(User2 item)
        {
            var existing = _users.Find(u => u.Id == item.Id);
            if (existing != null)
            {
                existing.Name = item.Name;
            }
        }

        public void Delete(User2 item)
        {
            _users.RemoveAll(u => u.Id == item.Id);
        }

        public User2 GetById(int id)
        {
            return _users.Find(u => u.Id == id);
        }

        public List<User2> GetAll()
        {
            return new List<User2>(_users);
        }
    }

    // Domain classes
    class Product2
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    class User2
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    // ═══════════════════════════════════════════════════════════
    // Generic Interface with Multiple Type Parameters
    // ═══════════════════════════════════════════════════════════

    // Key-value store interface with two type parameters
    interface IKeyValueStore<TKey, TValue> where TKey : notnull
    {
        void Set(TKey key, TValue value);
        TValue Get(TKey key);
        bool Contains(TKey key);
        void Remove(TKey key);
    }

    // Implementation of key-value store
    class MemoryCache<TKey, TValue> : IKeyValueStore<TKey, TValue>
    {
        private Dictionary<TKey, TValue> _cache = new Dictionary<TKey, TValue>();

        public void Set(TKey key, TValue value)
        {
            _cache[key] = value;
        }

        public TValue Get(TKey key)
        {
            return _cache.ContainsKey(key) ? _cache[key] : default(TValue);
        }

        public bool Contains(TKey key)
        {
            return _cache.ContainsKey(key);
        }

        public void Remove(TKey key)
        {
            _cache.Remove(key);
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Real-World: Service Interface
    // ═══════════════════════════════════════════════════════════

    // Output service interface
    interface IOutputService<T>
    {
        void Output(T data);
    }

    // Console implementation
    class ConsoleOutputService<T> : IOutputService<T>
    {
        public void Output(T data)
        {
            Console.WriteLine($"[CONSOLE] {data}");
        }
    }

    // File implementation
    class FileOutputService<T> : IOutputService<T>
    {
        public void Output(T data)
        {
            // Simulating file output
            Console.WriteLine($"[FILE] {data}");
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Interface Inheritance with Generics
    // ═══════════════════════════════════════════════════════════

    // Base repository interface
    interface IReadOnlyRepository<T>
    {
        T GetById(int id);
        List<T> GetAll();
    }

    // Extended repository with write operations
    interface IWriteRepository<T> : IReadOnlyRepository<T>
    {
        void Add(T item);
        void Update(T item);
        void Delete(T item);
    }

    // Implementation of read-only repository
    class ReadOnlyProductRepository : IReadOnlyRepository<Product2>
    {
        private List<Product2> _products = new List<Product2>();

        public ReadOnlyProductRepository()
        {
            _products.Add(new Product2 { Id = 1, Name = "Initial Product" });
        }

        public Product2 GetById(int id)
        {
            return _products.Find(p => p.Id == id);
        }

        public List<Product2> GetAll()
        {
            return new List<Product2>(_products);
        }

        // Not implementing Add - it's a read-only implementation
    }

    // ═══════════════════════════════════════════════════════════
    // Generic Interface with Constraints
    // ═══════════════════════════════════════════════════════════

    // Sortable interface with IComparable constraint
    interface ISortable<T> where T : IComparable<T>
    {
        void Sort(T[] items);
    }

    // Generic sorter implementation
    class Sorter<T> : ISortable<T> where T : IComparable<T>
    {
        public void Sort(T[] items)
        {
            Array.Sort(items);
        }
    }
}