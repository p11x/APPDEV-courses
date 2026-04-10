/*
 * TOPIC: Indexers and Iterators
 * SUBTOPIC: IEnumerable and IEnumerator Interfaces
 * FILE: IEnumerable_IEnumerator.cs
 * PURPOSE: Demonstrate IEnumerable and IEnumerator interfaces for custom collection enumeration
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace CSharp_MasterGuide._03_Advanced_OOP._07_IndexersIterators
{
    // Custom collection implementing IEnumerable and IEnumerator
    public class SimpleList : IEnumerable, IEnumerator
    {
        private object[] _items = new object[10];
        private int _count = 0;
        private int _position = -1;

        public void Add(object item)
        {
            if (_count < 10)
                _items[_count++] = item;
        }

        // IEnumerable.GetEnumerator - returns enumerator
        public IEnumerator GetEnumerator()
        {
            return this;
        }

        // IEnumerator: Move to next element
        public bool MoveNext()
        {
            _position++;
            return _position < _count;
        }

        // IEnumerator: Reset position to before first element
        public void Reset()
        {
            _position = -1;
        }

        // IEnumerator: Current element
        public object Current => _position >= 0 && _position < _count 
            ? _items[_position] 
            : null;

        public int Count => _count;
    }

    // Generic version with proper type safety
    public class GenericList<T> : IEnumerable<T>, IEnumerator<T>
    {
        private T[] _items = new T[10];
        private int _count = 0;
        private int _position = -1;

        public void Add(T item)
        {
            if (_count < 10)
                _items[_count++] = item;
        }

        // IEnumerable<T>.GetEnumerator
        public IEnumerator<T> GetEnumerator()
        {
            Reset();
            return this;
        }

        // IEnumerable.GetEnumerator (explicit implementation)
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        // IEnumerator<T>: Current property
        public T Current
        {
            get
            {
                if (_position >= 0 && _position < _count)
                    return _items[_position];
                return default(T);
            }
        }

        // IEnumerator: IDisposable implementation
        public void Dispose()
        {
            // Cleanup if needed
        }

        // IEnumerator: MoveNext
        public bool MoveNext()
        {
            _position++;
            return _position < _count;
        }

        // IEnumerator: Reset
        public void Reset()
        {
            _position = -1;
        }

        // Non-typed Current for IEnumerator
        object IEnumerator.Current => Current;

        public int Count => _count;
    }

    // Real-world: Temperature readings collection
    public class TemperatureReadings : IEnumerable<TemperatureReading>
    {
        private List<TemperatureReading> _readings = new List<TemperatureReading>();

        public void Add(TemperatureReading reading)
        {
            _readings.Add(reading);
        }

        public IEnumerator<TemperatureReading> GetEnumerator()
        {
            return new TemperatureEnumerator(_readings);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class TemperatureReading
    {
        public DateTime Timestamp { get; set; }
        public double Celsius { get; set; }

        public TemperatureReading(DateTime timestamp, double celsius)
        {
            Timestamp = timestamp;
            Celsius = celsius;
        }

        public override string ToString() => $"{Timestamp:HH:mm:ss}: {Celsius:F1}°C";
    }

    public class TemperatureEnumerator : IEnumerator<TemperatureReading>
    {
        private List<TemperatureReading> _readings;
        private int _position = -1;

        public TemperatureEnumerator(List<TemperatureReading> readings)
        {
            _readings = readings;
        }

        public TemperatureReading Current
        {
            get
            {
                if (_position >= 0 && _position < _readings.Count)
                    return _readings[_position];
                return null;
            }
        }

        public void Dispose() { }

        public bool MoveNext()
        {
            _position++;
            return _position < _readings.Count;
        }

        public void Reset()
        {
            _position = -1;
        }

        object IEnumerator.Current => Current;
    }

    // Real-world: Employee directory with custom enumeration
    public class Employee
    {
        public string Name { get; set; }
        public string Department { get; set; }
        public decimal Salary { get; set; }

        public Employee(string name, string department, decimal salary)
        {
            Name = name;
            Department = department;
            Salary = salary;
        }

        public override string ToString() => $"{Name} ({Department}): ${Salary:N0}";
    }

    public class EmployeeCollection : IEnumerable<Employee>
    {
        private List<Employee> _employees = new List<Employee>();

        public void Add(Employee employee)
        {
            _employees.Add(employee);
        }

        public void Add(string name, string department, decimal salary)
        {
            _employees.Add(new Employee(name, department, salary));
        }

        public IEnumerator<Employee> GetEnumerator()
        {
            return _employees.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        // Custom method returning filtered enumerator
        public IEnumerable<Employee> GetByDepartment(string department)
        {
            foreach (var emp in _employees)
            {
                if (emp.Department == department)
                    yield return emp;
            }
        }

        public IEnumerable<Employee> GetHighEarners(decimal threshold)
        {
            foreach (var emp in _employees)
            {
                if (emp.Salary >= threshold)
                    yield return emp;
            }
        }
    }

    public class IEnumerableIEnumerator
    {
        public static void Main()
        {
            Console.WriteLine("=== IEnumerable and IEnumerator Demo ===\n");

            // Example 1: Non-generic IEnumerable/IEnumerator
            Console.WriteLine("--- Non-Generic SimpleList ---");
            var list = new SimpleList();
            list.Add("Apple");
            list.Add("Banana");
            list.Add("Cherry");

            foreach (var item in list)
            {
                Console.WriteLine($"Item: {item}");
            }
            // Output:
            // Item: Apple
            // Item: Banana
            // Item: Cherry
            Console.WriteLine();

            // Example 2: Generic IEnumerable<T>/IEnumerator<T>
            Console.WriteLine("--- Generic List<T> ---");
            var genericList = new GenericList<string>();
            genericList.Add("Red");
            genericList.Add("Green");
            genericList.Add("Blue");

            foreach (var item in genericList)
            {
                Console.WriteLine($"Color: {item}");
            }
            // Output:
            // Color: Red
            // Color: Green
            // Color: Blue
            Console.WriteLine();

            // Example 3: Manual IEnumerator implementation
            Console.WriteLine("--- Manual IEnumerator Implementation ---");
            var enumerator = genericList.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Console.WriteLine($"Current: {enumerator.Current}");
            }
            enumerator.Reset();
            Console.WriteLine("After reset:");
            while (enumerator.MoveNext())
            {
                Console.WriteLine($"Current: {enumerator.Current}");
            }
            // Output:
            // Current: Red
            // Current: Green
            // Current: Blue
            // After reset:
            // Current: Red
            // Current: Green
            // Current: Blue
            Console.WriteLine();

            // Example 4: Real-world - Temperature readings
            Console.WriteLine("--- Real-World: Temperature Readable ---");
            var readings = new TemperatureReadings();
            readings.Add(new TemperatureReading(DateTime.Now.AddHours(-3), 15.5));
            readings.Add(new TemperatureReading(DateTime.Now.AddHours(-2), 18.2));
            readings.Add(new TemperatureReading(DateTime.Now.AddHours(-1), 20.1));
            readings.Add(new TemperatureReading(DateTime.Now, 19.8));

            foreach (var reading in readings)
            {
                Console.WriteLine(reading);
            }
            // Output:
            // 07:00:00: 15.5°C
            // 08:00:00: 18.2°C
            // 09:00:00: 20.1°C
            // 10:00:00: 19.8°C
            Console.WriteLine();

            // Example 5: Real-world - Employee collection
            Console.WriteLine("--- Real-World: Employee Collection ---");
            var employees = new EmployeeCollection();
            employees.Add("Alice", "Engineering", 85000);
            employees.Add("Bob", "Marketing", 65000);
            employees.Add("Charlie", "Engineering", 90000);
            employees.Add("Diana", "Sales", 70000);

            Console.WriteLine("All employees:");
            foreach (var emp in employees)
            {
                Console.WriteLine($"  {emp}");
            }
            // Output:
            //   Alice (Engineering): $85,000
            //   Bob (Marketing): $65,000
            //   Charlie (Engineering): $90,000
            //   Diana (Sales): $70,000

            Console.WriteLine("\nEngineering department:");
            foreach (var emp in employees.GetByDepartment("Engineering"))
            {
                Console.WriteLine($"  {emp}");
            }
            // Output:
            //   Alice (Engineering): $85,000
            //   Charlie (Engineering): $90,000

            Console.WriteLine("\nHigh earners (>$75,000):");
            foreach (var emp in employees.GetHighEarners(75000))
            {
                Console.WriteLine($"  {emp}");
            }
            // Output:
            //   Alice (Engineering): $85,000
            //   Charlie (Engineering): $90,000
        }
    }
}
