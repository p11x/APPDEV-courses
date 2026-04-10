/*
 * ============================================================
 * TOPIC     : C# Fundamentals
 * SUBTOPIC  : Data Types - Reference Types (Part 2)
 * FILE      : ReferenceTypes_Part2.cs
 * PURPOSE   : This file covers interface, delegate, and delegate types as reference types.
 *             Also covers the 'var' keyword and anonymous types.
 * ============================================================
 */

// --- SECTION: Interface and Delegate Types as References ---
// Interfaces and delegates are also reference types in C#
// They define contracts that can be implemented by classes or delegate targets

using System;

namespace CSharp_MasterGuide._01_Fundamentals._02_DataTypes
{
    // ── INTERFACE: Reference Type Definition ─────────────────────────────────
    // Interfaces define contracts (method signatures) that implementing classes must provide
    // Interface itself is a reference type - it defines a reference to implementers
    
    // IPrintable interface - contract for types that can print themselves
    interface IPrintable
    {
        // Method signature - implementing classes must provide implementation
        void Print();
        
        // Property signature - read-only property implementing classes must provide
        string PrintLayout { get; }
    }
    
    // IComparable<T> - generic interface for comparison
    interface IComparable<T>
    {
        // Returns: < 0 if this < obj, 0 if equal, > 0 if this > obj
        int CompareTo(T obj);
    }
    
    // ── IMPLEMENTING CLASSES ─────────────────────────────────────────────────
    
    // Document class implements IPrintable
    class Document : IPrintable
    {
        public string Title { get; set; } // Auto-property
        public string Content { get; set; } // Auto-property
        
        public Document(string title, string content)
        {
            Title = title;
            Content = content;
        }
        
        // Implementation of IPrintable.Print()
        public void Print()
        {
            Console.WriteLine($"Printing: {Title}");
            Console.WriteLine(Content);
        }
        
        // Implementation of IPrintable.PrintLayout
        public string PrintLayout => "A4 Portrait";
    }
    
    // Book class also implements IPrintable
    class Book : IPrintable
    {
        public string BookTitle { get; set; }
        public int PageCount { get; set; }
        
        public Book(string title, int pages)
        {
            BookTitle = title;
            PageCount = pages;
        }
        
        public void Print() // Implementation
        {
            Console.WriteLine($"Printing Book: {BookTitle} ({PageCount} pages)");
        }
        
        public string PrintLayout => "A4 Landscape"; // Different layout
    }

    // ═══════════════════════════════════════════════════════════════════════
    // SECTION: Delegate Types (Reference to Methods)
    // ═══════════════════════════════════════════════════════════════════════
    // Delegates are reference types that hold references to methods
    // They enable callback patterns, event handling, and functional programming
    
    // ── Delegate declaration ────────────────────────────────────────────────
    // Define a delegate type that matches a specific method signature
    // This delegate can reference methods returning int and taking two int parameters
    delegate int Calculate(int a, int b);
    
    // Delegate for void methods with no parameters
    delegate void MessageHandler(string message);
    
    // Generic delegate types (built-in in C#)
    // Func<T, TResult> - delegate returning TResult
    // Action<T> - delegate returning void
    // Predicate<T> - delegate returning bool
    
    // ── Interface and Delegate Reference Demo ─────────────────────────────
    
    class ReferenceTypes_Part2
    {
        static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════════
            // SECTION: Interface as Reference Type
            // ═══════════════════════════════════════════════════════════════
            
            // ── Storing implementers in interface reference ──────────────────
            // Interface variable can hold reference to any implementing class
            IPrintable printable; // Reference to interface, no object yet
            
            printable = new Document("Report", "This is the report content."); // Store Document
            printable.Print(); // Output: Printing: Report
            Console.WriteLine($"Layout: {printable.PrintLayout}"); // Output: Layout: A4 Portrait
            
            printable = new Book("The C# Mastery", 500); // Switch to Book reference
            printable.Print(); // Output: Printing Book: The C# Mastery (500 pages)
            Console.WriteLine($"Layout: {printable.PrintLayout}"); // Output: Layout: A4 Landscape
            
            // ── Interface array ──────────────────────────────────────────────
            // Array of interface references holds different implementations
            IPrintable[] documents = new IPrintable[3];
            documents[0] = new Document("Memo", "Important memo content.");
            documents[1] = new Book("Programming Guide", 350);
            documents[2] = new Document("Invoice", "Invoice #12345");
            
            // Polymorphic iteration - each object's Print is called
            Console.WriteLine("=== Printing All Documents ===");
            foreach (IPrintable doc in documents)
            {
                doc.Print(); // Calls appropriate implementation
                Console.WriteLine($"Layout: {doc.PrintLayout}");
                Console.WriteLine();
            }
            // Output shows different implementations being called polymorphically

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Delegate as Reference Type
            // ═══════════════════════════════════════════════════════════════
            
            // ── Creating delegate instances ───────────────────────────────────
            // Point delegate to different methods with matching signature
            Calculate add = AddNumbers; // Reference to AddNumbers method
            int result = add(10, 5);    // Call through delegate
            Console.WriteLine($"Add result: {result}"); // Output: Add result: 15
            
            Calculate multiply = MultiplyNumbers; // Different method reference
            result = multiply(10, 5);
            Console.WriteLine($"Multiply result: {result}"); // Output: Multiply result: 50
            
            // ── Delegate as method parameter ────────────────────────────────
            // Delegates enable passing behavior as parameters
            Console.WriteLine("=== Delegate as Parameter ===");
            ProcessNumbers(20, 4, AddNumbers); // Pass add behavior
            // Output: Result: 24
            
            ProcessNumbers(20, 4, MultiplyNumbers); // Pass multiply behavior
            // Output: Result: 80
            
            // ── Built-in delegate types (Func, Action, Predicate) ───────────
            // Func<T, TResult> - returns a value
            Func<int, int, int> divide = DivideNumbers; // Returns int
            result = divide(20, 4);
            Console.WriteLine($"Divide result: {result}"); // Output: Divide result: 5
            
            // Action<T> - returns void
            Action<string> logger = LogMessage; // Returns void
            logger("Application started"); // Output: LOG: Application started
            
            // Predicate<T> - returns bool
            Predicate<int> isEven = IsEvenNumber;
            bool isEvenResult = isEven(42);
            Console.WriteLine($"Is 42 even: {isEvenResult}"); // Output: Is 42 even: True

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Anonymous Types
            // ═══════════════════════════════════════════════════════════════
            // Anonymous types allow creating objects without defining a class first
            // Compiler generates a read-only class with properties
            
            // ── Creating anonymous types ─────────────────────────────────────
            // compiler generates class with Name and Age properties
            var anonymousPerson = new { Name = "Alice", Age = 30 };
            Console.WriteLine($"Anonymous: {anonymousPerson.Name}, {anonymousPerson.Age}");
            // Output: Anonymous: Alice, 30
            
            // Multiple anonymous objects with same structure share the same type
            var anotherPerson = new { Name = "Bob", Age = 25 };
            Console.WriteLine($"Another: {anotherPerson.Name}, {anotherPerson.Age}");
            // Output: Another: Bob, 25
            
            // Anonymous type with multiple properties
            var complex = new { 
                Id = 1, 
                FullName = "John Doe", 
                Email = "john@example.com",
                IsActive = true 
            };
            Console.WriteLine($"Complex: {complex.FullName} ({complex.Email})");
            // Output: Complex: John Doe (john@example.com)
            
            // ── Anonymous types in collections ─────────────────────────────
            // Useful for temporary data shaping in LINQ queries
            var people = new[] 
            {
                new { Name = "Alice", Department = "Engineering" },
                new { Name = "Bob", Department = "Marketing" },
                new { Name = "Charlie", Department = "Engineering" }
            };
            
            foreach (var person in people)
            {
                Console.WriteLine($"{person.Name} works in {person.Department}");
            }
            // Output:
            // Alice works in Engineering
            // Bob works in Marketing
            // Charlie works in Engineering

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Real-World Reference Type Scenarios
            // ═══════════════════════════════════════════════════════════════
            
            // ── Repository pattern with interfaces ─────────────────────────
            // Common pattern in data access layers
            IRepository<User> userRepo = new SqlUserRepository(); // In production, inject via DI
            var users = userRepo.GetAll(); // Work with interface
            
            Console.WriteLine("=== User Repository Demo ===");
            foreach (var user in users)
            {
                Console.WriteLine($"User: {user.Name}");
            }
            
            // Swap to different implementation without changing calling code
            userRepo = new InMemoryUserRepository();
            users = userRepo.GetAll();
            // Same interface, different implementation!
        }
        
        // ═══════════════════════════════════════════════════════════════════
        // Methods used in delegate examples
        // ═════════════════════════════════════════════════════════════════════
        
        static int AddNumbers(int a, int b) => a + b;
        static int MultiplyNumbers(int a, int b) => a * b;
        static int DivideNumbers(int a, int b) => a / b;
        static bool IsEvenNumber(int number) => number % 2 == 0;
        
        // Method that accepts delegate as parameter (strategy pattern)
        static void ProcessNumbers(int a, int b, Calculate operation)
        {
            int result = operation(a, b);
            Console.WriteLine($"Result: {result}");
        }
        
        static void LogMessage(string message)
        {
            Console.WriteLine($"LOG: {message}");
        }
    }
    
    // ═══════════════════════════════════════════════════════════════════════
    // SECTION: Real-World Example Classes
    // ═══════════════════════════════════════════════════════════════════════
    
    // User entity for repository example
    class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
    
    // Repository interface - defines contract for data access
    interface IRepository<T>
    {
        IEnumerable<T> GetAll(); // Get all entities
        T GetById(int id);      // Get single entity
        void Add(T entity);     // Add new entity
        void Update(T entity);  // Update existing
        void Delete(int id);    // Delete by ID
    }
    
    // SQL implementation of repository
    class SqlUserRepository : IRepository<User>
    {
        public IEnumerable<User> GetAll() 
        {
            // In real code, this would query SQL database
            yield return new User { Id = 1, Name = "Alice", Email = "alice@sql.com" };
            yield return new User { Id = 2, Name = "Bob", Email = "bob@sql.com" };
        }
        
        public User GetById(int id) => new User { Id = id, Name = "From SQL", Email = "sql@test.com" };
        public void Add(User entity) => Console.WriteLine($"Adding {entity.Name} to SQL");
        public void Update(User entity) => Console.WriteLine($"Updating {entity.Name} in SQL");
        public void Delete(int id) => Console.WriteLine($"Deleting {id} from SQL");
    }
    
    // In-memory implementation of repository (for testing)
    class InMemoryUserRepository : IRepository<User>
    {
        private List<User> _users = new List<User>
        {
            new User { Id = 1, Name = "Charlie", Email = "charlie@mem.com" },
            new User { Id = 2, Name = "Diana", Email = "diana@mem.com" }
        };
        
        public IEnumerable<User> GetAll() => _users;
        public User GetById(int id) => _users.FirstOrDefault(u => u.Id == id);
        public void Add(User entity) => _users.Add(entity);
        public void Update(User entity) 
        {
            var existing = _users.FirstOrDefault(u => u.Id == entity.Id);
            if (existing != null) _users.Remove(existing);
            _users.Add(entity);
        }
        public void Delete(int id) => _users.RemoveAll(u => u.Id == id);
    }
}
