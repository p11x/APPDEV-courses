/*
 * TOPIC: Indexers and Iterators
 * SUBTOPIC: Indexer Basics
 * FILE: IndexerBasics.cs
 * PURPOSE: Demonstrate basic indexer syntax, this[] accessor, and get/set implementation
 */

using System;
using System.Collections;

namespace CSharp_MasterGuide._03_Advanced_OOP._07_IndexersIterators
{
    // Basic indexer with string key
    public class StringDictionary
    {
        private string[] _keys = new string[100];
        private string[] _values = new string[100];
        private int _count = 0;

        // Indexer declaration using this[] - allows object[index] syntax
        public string this[string key]
        {
            get
            {
                // Find and return value for given key
                for (int i = 0; i < _count; i++)
                {
                    if (_keys[i] == key)
                        return _values[i];
                }
                return null; // Key not found
            }
            set
            {
                // Update existing key or add new key-value pair
                for (int i = 0; i < _count; i++)
                {
                    if (_keys[i] == key)
                    {
                        _values[i] = value;
                        return;
                    }
                }
                // Add new entry if space available
                if (_count < 100)
                {
                    _keys[_count] = key;
                    _values[_count] = value;
                    _count++;
                }
            }
        }

        public int Count => _count;
    }

    // Indexer with integer index
    public class NumberedStorage
    {
        private object[] _items = new object[10];

        // Integer indexer - enables array-like access
        public object this[int index]
        {
            get
            {
                if (index < 0 || index >= _items.Length)
                    throw new IndexOutOfRangeException("Index out of range");
                return _items[index];
            }
            set
            {
                if (index < 0 || index >= _items.Length)
                    throw new IndexOutOfRangeException("Index out of range");
                _items[index] = value;
            }
        }

        public int Length => _items.Length;
    }

    // Real-world example: Temperature sensor data storage
    public class TemperatureSensor
    {
        private double[] _temperatures = new double[24];
        private string[] _timestamps = new string[24];
        private int _hourIndex = 0;

        // Indexer to access temperature by hour (0-23)
        public double this[int hour]
        {
            get
            {
                if (hour < 0 || hour >= 24)
                    throw new IndexOutOfRangeException("Hour must be 0-23");
                return _temperatures[hour];
            }
            set
            {
                if (hour < 0 || hour >= 24)
                    throw new IndexOutOfRangeException("Hour must be 0-23");
                _temperatures[hour] = value;
                _timestamps[hour] = $"{hour}:00";
            }
        }

        public string GetTimestamp(int hour) => _timestamps[hour];

        public double GetAverageTemperature()
        {
            double sum = 0;
            int count = 0;
            for (int i = 0; i < 24; i++)
            {
                if (_temperatures[i] != 0)
                {
                    sum += _temperatures[i];
                    count++;
                }
            }
            return count > 0 ? sum / count : 0;
        }
    }

    public class IndexerBasics
    {
        public static void Main()
        {
            Console.WriteLine("=== Indexer Basics Demo ===\n");

            // Example 1: StringDictionary indexer
            Console.WriteLine("--- StringDictionary Example ---");
            var dict = new StringDictionary();
            dict["name"] = "John";
            dict["city"] = "New York";
            dict["age"] = "30";

            // Access using indexer syntax
            Console.WriteLine($"Name: {dict["name"]}"); // Output: Name: John
            Console.WriteLine($"City: {dict["city"]}"); // Output: City: New York
            Console.WriteLine($"Age: {dict["age"]}");   // Output: Age: 30

            // Update existing value
            dict["name"] = "Jane";
            Console.WriteLine($"Updated Name: {dict["name"]}"); // Output: Updated Name: Jane
            Console.WriteLine();

            // Example 2: NumberedStorage indexer
            Console.WriteLine("--- NumberedStorage Example ---");
            var storage = new NumberedStorage();
            storage[0] = "First item";
            storage[1] = "Second item";
            storage[2] = "Third item";

            Console.WriteLine($"Index 0: {storage[0]}"); // Output: Index 0: First item
            Console.WriteLine($"Index 1: {storage[1]}"); // Output: Index 1: Second item
            Console.WriteLine($"Index 2: {storage[2]}"); // Output: Index 2: Third item
            Console.WriteLine();

            // Example 3: Real-world - Temperature Sensor
            Console.WriteLine("--- Real-World: Temperature Sensor ---");
            var sensor = new TemperatureSensor();
            sensor[6] = 15.5;
            sensor[12] = 22.3;
            sensor[18] = 18.7;

            Console.WriteLine($"Temperature at 6 AM: {sensor[6]}");    // Output: Temperature at 6 AM: 15.5
            Console.WriteLine($"Temperature at 12 PM: {sensor[12]}");  // Output: Temperature at 12 PM: 22.3
            Console.WriteLine($"Temperature at 6 PM: {sensor[18]}");  // Output: Temperature at 6 PM: 18.7
            Console.WriteLine($"Average: {sensor.GetAverageTemperature():F1}"); // Output: Average: 18.8
        }
    }
}
