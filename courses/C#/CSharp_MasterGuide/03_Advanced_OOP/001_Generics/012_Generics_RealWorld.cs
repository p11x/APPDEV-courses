/*
 * ============================================================
 * TOPIC     : Advanced OOP
 * SUBTOPIC  : Generics - Real-World Examples
 * FILE      : Generics_RealWorld.cs
 * PURPOSE   : Teaches practical generic patterns: repository,
 *            cache, unit of work, and service locator
 * ============================================================
 */

using System;
using System.Collections.Generic;

namespace CSharp_MasterGuide._03_Advanced_OOP._01_Generics
{
    class Generics_RealWorld
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Generics Real-World Examples ===\n");

            // ═══════════════════════════════════════════════════════════
            // SECTION 1: Generic Repository Pattern
            // ═══════════════════════════════════════════════════════════

            // Using generic repository for different entity types
            var productRepo = new Repository<Product7, int>();
            productRepo.Add(new Product7 { Id = 1, Name = "Laptop", Price = 999.99m });
            productRepo.Add(new Product7 { Id = 2, Name = "Mouse", Price = 29.99m });
            
            var allProducts = productRepo.GetAll();
            Console.WriteLine($"Products count: {allProducts.Count}");
            // Output: Products count: 2

            var product = productRepo.GetById(1);
            Console.WriteLine($"Product: {product.Name}");
            // Output: Product: Laptop

            // Using with different entity
            var userRepo = new Repository<User7, int>();
            userRepo.Add(new User7 { Id = 1, Name = "Alice", Email = "alice@email.com" });
            Console.WriteLine($"Users count: {userRepo.GetAll().Count}");
            // Output: Users count: 1

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: Generic Cache System
            // ═══════════════════════════════════════════════════════════

            // Simple memory cache
            var cache = new SimpleCache<string, Product7>();
            
            // Add items to cache
            cache.Set("product-1", new Product7 { Id = 1, Name = "Laptop", Price = 999.99m });
            cache.Set("product-2", new Product7 { Id = 2, Name = "Phone", Price = 699.99m });
            
            // Retrieve from cache
            var cachedProduct = cache.Get("product-1");
            Console.WriteLine($"Cached product: {cachedProduct.Name}");
            // Output: Cached product: Laptop

            // Check if exists
            Console.WriteLine($"Has product-1: {cache.Contains("product-1")}");
            // Output: Has product-1: True

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: Generic Unit of Work
            // ═══════════════════════════════════════════════════════════

            // Unit of work managing multiple repositories
            var unitOfWork = new UnitOfWork();
            
            // Get repositories and perform operations
            var products = unitOfWork.Products;
            products.Add(new Product7 { Id = 1, Name = "Tablet", Price = 499.99m });
            
            var users = unitOfWork.Users;
            users.Add(new User7 { Id = 1, Name = "Bob", Email = "bob@email.com" });
            
            // Commit all changes
            unitOfWork.Commit();
            Console.WriteLine("Unit of work committed");
            // Output: Unit of work committed

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: Generic Service Locator
            // ═══════════════════════════════════════════════════════════

            // Simple service locator
            var serviceLocator = ServiceLocator.Instance;
            
            // Register services
            serviceLocator.Register<ILogger>(() => new FileLogger());
            serviceLocator.Register<IEmailService>(() => new EmailService());
            
            // Resolve services
            var logger = serviceLocator.Resolve<ILogger>();
            logger.Log("Application started");
            // Output: [LOG] Application started

            var emailService = serviceLocator.Resolve<IEmailService>();
            emailService.Send("user@example.com", "Test message");
            // Output: [EMAIL] To: user@example.com, Message: Test message

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: Generic Result Wrapper
            // ═══════════════════════════════════════════════════════════

            // Operation result wrapper
            var successResult = Result7<Product7>.Success(
                new Product7 { Id = 1, Name = "Laptop", Price = 999.99m }
            );
            Console.WriteLine($"Success: {successResult.IsSuccess}, Data: {successResult.Data?.Name}");
            // Output: Success: True, Data: Laptop

            var errorResult = Result7<Product7>.Failure("Product not found");
            Console.WriteLine($"Success: {errorResult.IsSuccess}, Error: {errorResult.ErrorMessage}");
            // Output: Success: False, Error: Product not found

            Console.WriteLine("\n=== Generics Real-World Complete ===");
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Generic Repository Pattern
    // ═══════════════════════════════════════════════════════════

    // Entity interface with ID
    interface IEntity<TId>
    {
        TId Id { get; set; }
    }

    // Generic repository interface
    interface IRepository<T, TId> where T : IEntity<TId>
    {
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        T GetById(TId id);
        List<T> GetAll();
    }

    // Generic repository implementation
    class Repository<T, TId> : IRepository<T, TId> where T : IEntity<TId>, new()
    {
        private List<T> _entities = new List<T>();

        public void Add(T entity)
        {
            _entities.Add(entity);
        }

        public void Update(T entity)
        {
            // Find and update
            int index = _entities.FindIndex(e => Equals(e.Id, entity.Id));
            if (index >= 0)
                _entities[index] = entity;
        }

        public void Delete(T entity)
        {
            _entities.RemoveAll(e => Equals(e.Id, entity.Id));
        }

        public T GetById(TId id)
        {
            return _entities.Find(e => Equals(e.Id, id);
        }

        public List<T> GetAll()
        {
            return new List<T>(_entities);
        }
    }

    // Domain entities
    class Product7 : IEntity<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }

    class User7 : IEntity<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }

    // ═══════════════════════════════════════════════════════════
    // Generic Cache System
    // ═══════════════════════════════════════════════════════════

    // Simple generic cache
    class SimpleCache<TKey, TValue> where TKey : notnull
    {
        private Dictionary<TKey, TValue> _cache = new Dictionary<TKey, TValue>();
        private Dictionary<TKey, DateTime> _expiry = new Dictionary<TKey, DateTime>();

        public void Set(TKey key, TValue value, TimeSpan? expiry = null)
        {
            _cache[key] = value;
            if (expiry.HasValue)
            {
                _expiry[key] = DateTime.Now.Add(expiry.Value);
            }
        }

        public TValue Get(TKey key)
        {
            if (_expiry.TryGetValue(key, out var expiry) && expiry < DateTime.Now)
            {
                _cache.Remove(key);
                _expiry.Remove(key);
                return default(TValue);
            }
            return _cache.ContainsKey(key) ? _cache[key] : default(TValue);
        }

        public bool Contains(TKey key)
        {
            return _cache.ContainsKey(key);
        }

        public void Remove(TKey key)
        {
            _cache.Remove(key);
            _expiry.Remove(key);
        }

        public void Clear()
        {
            _cache.Clear();
            _expiry.Clear();
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Generic Unit of Work
    // ═══════════════════════════════════════════════════════════

    class UnitOfWork
    {
        private Repository<Product7, int> _productRepo;
        private Repository<User7, int> _userRepo;
        private bool _committed = false;

        public Repository<Product7, int> Products => 
            _productRepo ??= new Repository<Product7, int>();
        
        public Repository<User7, int> Users => 
            _userRepo ??= new Repository<User7, int>();

        public void Commit()
        {
            // In real scenario, this would save to database
            _committed = true;
            Console.WriteLine("  Changes committed to database");
        }

        public void Rollback()
        {
            _committed = false;
            Console.WriteLine("  Changes rolled back");
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Generic Service Locator
    // ═══════════════════════════════════════════════════════════

    interface ILogger
    {
        void Log(string message);
    }

    interface IEmailService
    {
        void Send(string to, string message);
    }

    class FileLogger : ILogger
    {
        public void Log(string message) => Console.WriteLine($"[LOG] {message}");
    }

    class EmailService : IEmailService
    {
        public void Send(string to, string message) => 
            Console.WriteLine($"[EMAIL] To: {to}, Message: {message}");
    }

    class ServiceLocator
    {
        private static ServiceLocator _instance;
        private Dictionary<Type, Func<object>> _factories = new Dictionary<Type, Func<object>>();

        public static ServiceLocator Instance => _instance ??= new ServiceLocator();

        public void Register<T>(Func<T> factory)
        {
            _factories[typeof(T)] = () => factory();
        }

        public void Register<TInterface>(Func<TInterface> factory) where TInterface : class
        {
            _factories[typeof(TInterface)] = () => factory();
        }

        public T Resolve<T>()
        {
            if (_factories.TryGetValue(typeof(T), out var factory))
            {
                return (T)factory();
            }
            throw new InvalidOperationException($"Service {typeof(T).Name} not registered");
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Generic Result Wrapper
    // ═══════════════════════════════════════════════════════════

    class Result7<T>
    {
        public bool IsSuccess { get; private set; }
        public T Data { get; private set; }
        public string ErrorMessage { get; private set; }

        private Result7() { }

        public static Result7<T> Success(T data)
        {
            return new Result7<T> 
            { 
                IsSuccess = true, 
                Data = data 
            };
        }

        public static Result7<T> Failure(string errorMessage)
        {
            return new Result7<T> 
            { 
                IsSuccess = false, 
                ErrorMessage = errorMessage 
            };
        }
    }
}