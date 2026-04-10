/*
 * ============================================================
 * TOPIC     : Advanced OOP
 * SUBTOPIC  : Generics - Generic Classes
 * FILE      : GenericClasses.cs
 * PURPOSE   : Teaches generic class syntax, type parameters,
 *            instance creation, and class-level generics in C#
 * ============================================================
 */

using System;
using System.Collections.Generic;

namespace CSharp_MasterGuide._03_Advanced_OOP._01_Generics
{
    class GenericClasses
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Generic Classes in C# ===\n");

            // ═══════════════════════════════════════════════════════════
            // SECTION 1: Basic Generic Class
            // ═══════════════════════════════════════════════════════════

            // Generic class with single type parameter
            // The type T is specified when creating an instance
            var intContainer = new Container<int>(42);
            Console.WriteLine($"Integer Container: {intContainer.GetValue()}");
            // Output: Integer Container: 42

            var stringContainer = new Container<string>("Hello Generics");
            Console.WriteLine($"String Container: {stringContainer.GetValue()}");
            // Output: String Container: Hello Generics

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: Generic Class with Multiple Type Parameters
            // ═══════════════════════════════════════════════════════════

            // Using a generic class with two type parameters
            var keyValuePair = new KeyValuePair<string, int>("Score", 100);
            Console.WriteLine($"Key: {keyValuePair.Key}, Value: {keyValuePair.Value}");
            // Output: Key: Score, Value: 100

            // Different type combinations
            var personRecord = new KeyValuePair<string, Person>("CEO", new Person("Alice"));
            Console.WriteLine($"Person Role: {personRecord.Key}, Name: {personRecord.Value.Name}");
            // Output: Person Role: CEO, Name: Alice

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: Real-World Example - Cache System
            // ═══════════════════════════════════════════════════════════

            // Creating a simple cache for different types
            var intCache = new Cache<int, string>();
            intCache.Set(1, "First Item");
            intCache.Set(2, "Second Item");
            Console.WriteLine($"Cache[1]: {intCache.Get(1)}");
            // Output: Cache[1]: First Item

            var userCache = new Cache<string, UserProfile>();
            userCache.Set("john", new UserProfile("John", "john@email.com"));
            var user = userCache.Get("john");
            Console.WriteLine($"User: {user.Name}, Email: {user.Email}");
            // Output: User: John, Email: john@email.com

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: Generic Class with Methods
            // ═══════════════════════════════════════════════════════════

            var resultHolder = new ResultHolder<double>();
            resultHolder.SetValue(95.5);
            resultHolder.SetSuccess(true);
            
            Console.WriteLine($"Value: {resultHolder.GetValue()}, Success: {resultHolder.IsSuccess()}");
            // Output: Value: 95.5, Success: True

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: Generic Class with Constraints
            // ═══════════════════════════════════════════════════════════

            // Using a generic class with struct constraint
            var numberBox = new NumberBox<int>(10);
            numberBox.Double();
            Console.WriteLine($"Doubled value: {numberBox.GetValue()}");
            // Output: Doubled value: 20

            // Using a generic class with class constraint
            var referenceBox = new ReferenceBox<string>("test");
            referenceBox.Append("ing");
            Console.WriteLine($"Appended value: {referenceBox.GetValue()}");
            // Output: Appended value: testing

            // ═══════════════════════════════════════════════════════════
            // SECTION 6: Real-World Example - Event Log System
            // ═══════════════════════════════════════════════════════════

            var errorLog = new EventLog<string>();
            errorLog.Log("Application started");
            errorLog.Log("User logged in");
            errorLog.Log("Error occurred: Database connection failed");
            
            Console.WriteLine("Event Log Entries:");
            foreach (var entry in errorLog.GetAllEntries())
            {
                Console.WriteLine($"  - {entry}");
            }
            // Output:
            //   - Application started
            //   - User logged in
            //   - Error occurred: Database connection failed

            Console.WriteLine("\n=== Generic Classes Complete ===");
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Basic Generic Class with Single Type Parameter
    // ═══════════════════════════════════════════════════════════

    // Container<T> is a generic class that can hold any type
    // The type parameter T is specified when instantiating
    class Container<T>
    {
        // Private field of type T
        private T _value;

        // Constructor accepts a value of type T
        public Container(T value)
        {
            _value = value;
        }

        // Method returns the stored value
        public T GetValue()
        {
            return _value;
        }

        // Method to set a new value
        public void SetValue(T value)
        {
            _value = value;
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Generic Class with Multiple Type Parameters
    // ═══════════════════════════════════════════════════════════

    // KeyValuePair<TKey, TValue> uses two type parameters
    // TKey for the key type, TValue for the value type
    class KeyValuePair<TKey, TValue>
    {
        public TKey Key { get; set; }
        public TValue Value { get; set; }

        public KeyValuePair(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }

    // Simple Person class for demonstration
    class Person
    {
        public string Name { get; set; }
        public Person(string name) => Name = name;
    }

    // ═══════════════════════════════════════════════════════════
    // Real-World: Generic Cache Class
    // ═══════════════════════════════════════════════════════════

    // A generic cache that stores key-value pairs
    // TKey: type for the cache key
    // TValue: type for the cached value
    class Cache<TKey, TValue> where TKey : notnull
    {
        // Internal dictionary to store cached items
        private Dictionary<TKey, TValue> _storage = new Dictionary<TKey, TValue>();

        // Add or update an item in the cache
        public void Set(TKey key, TValue value)
        {
            _storage[key] = value;
        }

        // Retrieve an item from the cache
        public TValue Get(TKey key)
        {
            if (_storage.ContainsKey(key))
            {
                return _storage[key];
            }
            return default(TValue);
        }

        // Check if a key exists in the cache
        public bool Contains(TKey key)
        {
            return _storage.ContainsKey(key);
        }

        // Clear all cached items
        public void Clear()
        {
            _storage.Clear();
        }
    }

    // UserProfile class for cache demonstration
    class UserProfile
    {
        public string Name { get; set; }
        public string Email { get; set; }

        public UserProfile(string name, string email)
        {
            Name = name;
            Email = email;
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Generic Class with Multiple Methods
    // ═══════════════════════════════════════════════════════════

    // ResultHolder<T> stores a result and its success status
    class ResultHolder<T>
    {
        private T _value;
        private bool _success;

        public void SetValue(T value)
        {
            _value = value;
        }

        public T GetValue()
        {
            return _value;
        }

        public void SetSuccess(bool success)
        {
            _success = success;
        }

        public bool IsSuccess()
        {
            return _success;
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Generic Class with struct Constraint
    // ═══════════════════════════════════════════════════════════

    // NumberBox<T> requires T to be a value type (struct)
    // This allows us to use arithmetic operations
    class NumberBox<T> where T : struct
    {
        private T _value;

        public NumberBox(T value)
        {
            _value = value;
        }

        public T GetValue()
        {
            return _value;
        }

        public void SetValue(T value)
        {
            _value = value;
        }

        // Method that works specifically with numeric types
        // Note: In real scenarios, you'd use INumber<T> interface
        public void Double()
        {
            // For demonstration, we'll use dynamic
            dynamic d = _value;
            _value = (T)(d * 2);
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Generic Class with class Constraint
    // ═══════════════════════════════════════════════════════════

    // ReferenceBox<T> requires T to be a reference type (class)
    class ReferenceBox<T> where T : class
    {
        private T _value;

        public ReferenceBox(T value)
        {
            _value = value;
        }

        public T GetValue()
        {
            return _value;
        }

        // Method to manipulate string (demonstrates reference type operations)
        public void Append(string suffix)
        {
            // Using dynamic to allow string-specific operations
            dynamic d = _value;
            _value = (T)(d + suffix);
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Real-World: Event Log System
    // ═══════════════════════════════════════════════════════════

    // GenericEventLog<T> stores events of type T
    class EventLog<T>
    {
        private List<T> _entries = new List<T>();

        // Log a new event
        public void Log(T entry)
        {
            _entries.Add(entry);
        }

        // Get all logged entries
        public List<T> GetAllEntries()
        {
            return new List<T>(_entries);
        }

        // Get the count of entries
        public int Count()
        {
            return _entries.Count;
        }

        // Clear all entries
        public void Clear()
        {
            _entries.Clear();
        }
    }
}