/*
 * ============================================================
 * TOPIC     : Advanced OOP
 * SUBTOPIC  : Generics - Generic Classes Part 2
 * FILE      : GenericClasses_Part2.cs
 * PURPOSE   : Teaches multiple type parameters, generic inheritance,
 *            generic class hierarchies, and nested generics
 * ============================================================
 */

using System;
using System.Collections.Generic;

namespace CSharp_MasterGuide._03_Advanced_OOP._01_Generics
{
    class GenericClasses_Part2
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Generic Classes Part 2 ===\n");

            // ═══════════════════════════════════════════════════════════
            // SECTION 1: Multiple Type Parameters Deep Dive
            // ═══════════════════════════════════════════════════════════

            // Using Triple generic class with three type parameters
            var triple = new Triple<int, string, double>(1, "One", 1.5);
            Console.WriteLine($"First: {triple.First}, Second: {triple.Second}, Third: {triple.Third}");
            // Output: First: 1, Second: One, Third: 1.5

            // Using with different types
            var personTriple = new Triple<string, Person, Address>(
                "Employee",
                new Person("Alice"),
                new Address("123 Main St")
            );
            Console.WriteLine($"Role: {personTriple.First}, Name: {personTriple.Second.Name}");
            // Output: Role: Employee, Name: Alice

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: Generic Inheritance
            // ═══════════════════════════════════════════════════════════

            // Creating instances of generic derived classes
            var intRepository = new IntRepository();
            intRepository.Add(100);
            intRepository.Add(200);
            Console.WriteLine($"Int Repository Count: {intRepository.Count()}");
            // Output: Int Repository Count: 2

            var stringRepository = new StringRepository();
            stringRepository.Add("Hello");
            Console.WriteLine($"String Repository Count: {stringRepository.Count()}");
            // Output: String Repository Count: 1

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: Generic Base Class with Type Parameter
            // ═══════════════════════════════════════════════════════════

            // Using specialized service
            var orderService = new OrderService();
            orderService.Process(new Order(1, "Order #001"));
            Console.WriteLine($"Order Processed: {orderService.GetProcessedCount()}");
            // Output: Order Processed: 1

            var userService = new UserService();
            userService.Process(new User("Bob"));
            Console.WriteLine($"User Processed: {userService.GetProcessedCount()}");
            // Output: User Processed: 1

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: Nested Generic Classes
            // ═══════════════════════════════════════════════════════════

            // Using nested generic structures
            var matrix = new Matrix<int>(2, 2);
            matrix.Set(0, 0, 1);
            matrix.Set(0, 1, 2);
            matrix.Set(1, 0, 3);
            matrix.Set(1, 1, 4);
            
            Console.WriteLine($"Matrix[0,0]: {matrix.Get(0, 0)}, [1,1]: {matrix.Get(1, 1)}");
            // Output: Matrix[0,0]: 1, [1,1]: 4

            // Using nested generic with different types
            var stringMatrix = new Matrix<string>(2, 1);
            stringMatrix.Set(0, 0, "A");
            stringMatrix.Set(1, 0, "B");
            Console.WriteLine($"String Matrix[0,0]: {stringMatrix.Get(0, 0)}");
            // Output: String Matrix[0,0]: A

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: Real-World Example - Generic Queue System
            // ═══════════════════════════════════════════════════════════

            var messageQueue = new MessageQueue<string>();
            messageQueue.Enqueue("Message 1");
            messageQueue.Enqueue("Message 2");
            
            Console.WriteLine($"Dequeued: {messageQueue.Dequeue()}");
            Console.WriteLine($"Peek: {messageQueue.Peek()}");
            // Output: Dequeued: Message 1
            // Output: Peek: Message 2

            var intQueue = new MessageQueue<int>();
            intQueue.Enqueue(10);
            intQueue.Enqueue(20);
            Console.WriteLine($"Int Dequeued: {intQueue.Dequeue()}");
            // Output: Int Dequeued: 10

            // ═══════════════════════════════════════════════════════════
            // SECTION 6: Generic Class with Static Members
            // ═══════════════════════════════════════════════════════════

            // Static members are shared per type parameter
            var counter1 = new Counter<int>();
            var counter2 = new Counter<int>();
            var counter3 = new Counter<string>();

            Console.WriteLine($"Int Counter 1: {Counter<int>.InstanceCount}");
            Console.WriteLine($"Int Counter 2: {Counter<int>.InstanceCount}");
            Console.WriteLine($"String Counter: {Counter<string>.InstanceCount}");
            // Output: Int Counter 1: 2
            // Output: Int Counter 2: 2
            // Output: String Counter: 1

            Console.WriteLine("\n=== Generic Classes Part 2 Complete ===");
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Multiple Type Parameters - Three Type Parameters
    // ═══════════════════════════════════════════════════════════

    // Triple<TFirst, TSecond, TThird> holds three values of different types
    class Triple<TFirst, TSecond, TThird>
    {
        public TFirst First { get; set; }
        public TSecond Second { get; set; }
        public TThird Third { get; set; }

        public Triple(TFirst first, TSecond second, TThird third)
        {
            First = first;
            Second = second;
            Third = third;
        }
    }

    // Helper classes for demonstration
    class Address
    {
        public string Street { get; set; }
        public Address(string street) => Street = street;
    }

    // ═══════════════════════════════════════════════════════════
    // Generic Inheritance - Non-Generic Base
    // ═══════════════════════════════════════════════════════════

    // Non-generic base class
    abstract class BaseRepository
    {
        protected List<object> _items = new List<object>();

        public abstract void Add(object item);
        public abstract int Count();
    }

    // Generic derived class from non-generic base
    class IntRepository : BaseRepository
    {
        private List<int> _intItems = new List<int>();

        public override void Add(object item)
        {
            if (item is int i)
                _intItems.Add(i);
        }

        public void Add(int item)
        {
            _intItems.Add(item);
        }

        public override int Count()
        {
            return _intItems.Count;
        }

        public List<int> GetAll()
        {
            return new List<int>(_intItems);
        }
    }

    // Another generic derived class
    class StringRepository : BaseRepository
    {
        private List<string> _stringItems = new List<string>();

        public override void Add(object item)
        {
            if (item is string s)
                _stringItems.Add(s);
        }

        public void Add(string item)
        {
            _stringItems.Add(item);
        }

        public override int Count()
        {
            return _stringItems.Count;
        }

        public List<string> GetAll()
        {
            return new List<string>(_stringItems);
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Generic Inheritance - Generic Base Class
    // ═══════════════════════════════════════════════════════════

    // Generic base class with type parameter
    abstract class BaseService<T>
    {
        protected List<T> _processedItems = new List<T>();

        public abstract void Process(T item);

        public List<T> GetProcessedItems()
        {
            return new List<T>(_processedItems);
        }

        public int GetProcessedCount()
        {
            return _processedItems.Count;
        }
    }

    // Generic derived class that specializes the base
    class OrderService : BaseService<Order>
    {
        public override void Process(Order order)
        {
            _processedItems.Add(order);
            Console.WriteLine($"  Processed order: {order.OrderNumber}");
        }
    }

    // Another specialization
    class UserService : BaseService<User>
    {
        public override void Process(User user)
        {
            _processedItems.Add(user);
            Console.WriteLine($"  Processed user: {user.Name}");
        }
    }

    // Domain classes
    class Order
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; }

        public Order(int id, string orderNumber)
        {
            Id = id;
            OrderNumber = orderNumber;
        }
    }

    class User
    {
        public string Name { get; set; }
        public User(string name) => Name = name;
    }

    // ═══════════════════════════════════════════════════════════
    // Nested Generic Classes
    // ═══════════════════════════════════════════════════════════

    // Matrix<T> uses row and column indices, stores T elements
    class Matrix<T>
    {
        private T[,] _data;
        private int _rows;
        private int _columns;

        public Matrix(int rows, int columns)
        {
            _rows = rows;
            _columns = columns;
            _data = new T[rows, columns];
        }

        public void Set(int row, int column, T value)
        {
            if (row >= 0 && row < _rows && column >= 0 && column < _columns)
            {
                _data[row, column] = value;
            }
        }

        public T Get(int row, int column)
        {
            if (row >= 0 && row < _rows && column >= 0 && column < _columns)
            {
                return _data[row, column];
            }
            return default(T);
        }

        public int RowCount => _rows;
        public int ColumnCount => _columns;
    }

    // ═══════════════════════════════════════════════════════════
    // Real-World: Generic Message Queue
    // ═══════════════════════════════════════════════════════════

    // Generic queue for storing messages of any type
    class MessageQueue<T>
    {
        private Queue<T> _queue = new Queue<T>();

        public void Enqueue(T item)
        {
            _queue.Enqueue(item);
        }

        public T Dequeue()
        {
            return _queue.Count > 0 ? _queue.Dequeue() : default(T);
        }

        public T Peek()
        {
            return _queue.Count > 0 ? _queue.Peek() : default(T);
        }

        public int Count => _queue.Count;

        public bool IsEmpty => _queue.Count == 0;

        public void Clear()
        {
            _queue.Clear();
        }
    }

    // ═══════════════════════════════════════════════════════════
    // Generic Class with Static Members per Type
    // ═══════════════════════════════════════════════════════════

    // Counter<T> has a static count that is separate for each T
    class Counter<T>
    {
        // Static field - one per type parameter value
        private static int _instanceCount = 0;

        public static int InstanceCount
        {
            get { return _instanceCount; }
        }

        public Counter()
        {
            // Increment count for this type parameter
            _instanceCount++;
        }

        // Instance method to reset count for this type
        public static void ResetCount()
        {
            _instanceCount = 0;
        }
    }
}