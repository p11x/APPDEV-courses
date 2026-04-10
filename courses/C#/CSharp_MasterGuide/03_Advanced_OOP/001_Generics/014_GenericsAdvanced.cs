/*
 * ============================================================
 * TOPIC     : Advanced OOP
 * SUBTOPIC  : Generics - Advanced Generics
 * FILE      : GenericsAdvanced.cs
 * PURPOSE   : Advanced generic patterns including default values,
 *            covariance/contravariance, generic interfaces, and
 *            practical applications
 * ============================================================
 */

using System; // Core System namespace
using System.Collections.Generic; // Collections namespace

namespace CSharp_MasterGuide._03_Advanced_OOP._01_Generics
{
    class GenericsAdvanced
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Advanced Generics in C# ===\n");

            // ═══════════════════════════════════════════════════════════
            // SECTION 1: Default Value in Generics
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: default(T) Keyword ──────────────────────────
            var defaultInt = GetDefault<int>();
            var defaultString = GetDefault<string>();
            var defaultDouble = GetDefault<double>();
            
            Console.WriteLine($"Default int: {defaultInt}");
            Console.WriteLine($"Default string: '{defaultString}'");
            Console.WriteLine($"Default double: {defaultDouble}");

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: Generic Interfaces
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Generic Repository Interface ────────────────
            var userRepo = new UserRepository();
            userRepo.Add(new User { Name = "Alice" });
            var user = userRepo.GetById(1);
            
            var productRepo = new ProductRepository();
            productRepo.Add(new Product { Name = "Laptop" });
            var product = productRepo.GetById(1);

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: Covariance and Contravariance
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: IEnumerable<T> is Covariant ─────────────────
            List<DogGeneric> dogs = new List<DogGeneric>
            {
                new DogGeneric(),
                new DogGeneric()
            };
            
            // IEnumerable<out T> is covariant - can assign to base type
            IEnumerable<AnimalGeneric> animals = dogs;
            Console.WriteLine($"\nCovariance - converted List<Dog> to IEnumerable<Animal>");

            // ── EXAMPLE 2: IComparer<T> is Contravariant ──────────────
            IComparer<AnimalGeneric> animalComparer = new AnimalComparer();
            IComparer<DogGeneric> dogComparer = animalComparer; // Contravariance

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: Generic Delegates
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Func and Action Delegates ───────────────────
            Func<int, int, int> add = (a, b) => a + b;
            Console.WriteLine($"\nFunc result: {add(5, 3)}");
            
            Func<string, string> upper = s => s.ToUpper();
            Console.WriteLine($"Func string: {upper("hello")}");
            
            Action<string> print = s => Console.WriteLine($"  Action: {s}");
            print("Message");

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: Generic Constraints Deep Dive
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Multiple Constraints ────────────────────────
            var manager = new ManagerGeneric();
            var emp = new EmployeeGeneric { Name = "Alice" };
            manager.Assign(emp);
            
            // ── EXAMPLE 2: Interface Constraint ────────────────────────
            var list = new List<int> { 1, 2, 3, 4, 5 };
            var sum = SumList(list);
            Console.WriteLine($"\nSum: {sum}");

            // ═══════════════════════════════════════════════════════════
            // SECTION 6: Real-World: Caching Generic
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Generic Cache ───────────────────────────────
            var cache = new Cache<string, UserCache>();
            cache.Set("user1", new UserCache { Name = "Alice", Email = "alice@email.com" });
            var cached = cache.Get("user1");
            Console.WriteLine($"\nCached user: {cached?.Name}");

            // ═══════════════════════════════════════════════════════════
            // SECTION 7: Generic Event Handler
            // ═══════════════════════════════════════════════════════════

            // ── EXAMPLE 1: Event with Generic ───────────────────────────
            var publisher = new EventPublisher();
            publisher.OnEvent += (sender, args) => 
                Console.WriteLine($"  Event received: {args.Message}");
            
            publisher.RaiseEvent("Hello Generics!");

            Console.WriteLine("\n=== Advanced Generics Complete ===");
        }

        // Generic method with default value
        static T GetDefault<T>()
        {
            return default(T);
        }

        // Interface constraint
        static T SumList<T>(List<T> list) where T : INumber<T>
        {
            T sum = T.Zero;
            foreach (var item in list)
            {
                sum += item;
            }
            return sum;
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Generic Interface
    // ═══════════════════════════════════════════════════════════

    interface IRepository<T>
    {
        void Add(T item);
        T GetById(int id);
        List<T> GetAll();
    }

    class User { public string Name { get; set; } }
    class Product { public string Name { get; set; } }

    class UserRepository : IRepository<User>
    {
        private List<User> _users = new List<User>();
        
        public void Add(User item) => _users.Add(item);
        public User GetById(int id) => id < _users.Count ? _users[id] : null;
        public List<User> GetAll() => _users;
    }

    class ProductRepository : IRepository<Product>
    {
        private List<Product> _products = new List<Product>();
        
        public void Add(Product item) => _products.Add(item);
        public Product GetById(int id) => id < _products.Count ? _products[id] : null;
        public List<Product> GetAll() => _products;
    }

    // ═══════════════════════════════════════════════════════════
    // Contravariance
    // ═══════════════════════════════════════════════════════════

    class AnimalComparer : IComparer<AnimalGeneric>
    {
        public int Compare(AnimalGeneric x, AnimalGeneric y)
        {
            return 0; // Simple comparison
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Multiple Constraints
    // ═══════════════════════════════════════════════════════════

    class EmployeeGeneric
    {
        public string Name { get; set; }
    }

    interface IManager<T> where T : EmployeeGeneric
    {
        void Assign(T employee);
    }

    class ManagerGeneric : IManager<EmployeeGeneric>
    {
        public void Assign(EmployeeGeneric employee)
        {
            Console.WriteLine($"  Assigned: {employee.Name}");
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Generic Cache
    // ═══════════════════════════════════════════════════════════

    class UserCache
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }

    class Cache<TKey, TValue> where TValue : class
    {
        private Dictionary<TKey, TValue> _cache = new Dictionary<TKey, TValue>();
        
        public void Set(TKey key, TValue value)
        {
            _cache[key] = value;
        }
        
        public TValue Get(TKey key)
        {
            return _cache.TryGetValue(key, out var value) ? value : null;
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Generic Event Handler
    // ═══════════════════════════════════════════════════════════

    class EventArgsGeneric : EventArgs
    {
        public string Message { get; set; }
    }

    class EventPublisher
    {
        public event EventHandler<EventArgsGeneric> OnEvent;
        
        public void RaiseEvent(string message)
        {
            OnEvent?.Invoke(this, new EventArgsGeneric { Message = message });
        }
    }
}