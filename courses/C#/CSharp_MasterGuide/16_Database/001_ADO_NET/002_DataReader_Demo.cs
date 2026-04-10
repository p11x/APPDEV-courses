/*
 * ============================================================
 * TOPIC     : Database
 * SUBTOPIC  : DataReader - Reading Data
 * FILE      : DataReader_Demo.cs
 * PURPOSE   : Using DataReader for fast forward-only reading
 * ============================================================
 */
using System; // Core System namespace

namespace CSharp_MasterGuide._16_Database._01_ADO_NET
{
    /// <summary>
    /// DataReader demonstration
    /// </summary>
    public class DataReaderDemo
    {
        /// <summary>
        /// Entry point
        /// </summary>
        public static void Main(string[] args)
        {
            Console.WriteLine("=== DataReader Demo ===\n");

            // Output: --- Fast Forward-Only ---
            Console.WriteLine("--- Fast Forward-Only ---");

            // DataReader: connected, forward-only reader
            // Fast, low memory - streams data

            var reader = new MockDataReader();
            while (reader.Read())
            {
                var name = reader.GetString(0);
                var age = reader.GetInt32(1);
                Console.WriteLine($"   {name}, {age}");
            }
            // Output: Alice, 30
            // Output: Bob, 25

            // Output: --- Sequential Access ---
            Console.WriteLine("\n--- Sequential Access ---");

            var seqReader = new MockDataReader();
            while (seqReader.Read())
            {
                Console.WriteLine($"   Reading: {seqReader[0]}");
            }
            // Output: Reading: Alice
            // Output: Reading: Bob

            // Output: --- Typed Access ---
            Console.WriteLine("\n--- Typed Access ---");

            var typedReader = new MockDataReader();
            if (typedReader.Read())
            {
                var name = typedReader.GetString("Name");
                var age = typedReader.GetInt32("Age");
                Console.WriteLine($"   {name} is {age}");
            }
            // Output: Alice is 30

            // Output: --- Multiple Results ---
            Console.WriteLine("\n--- Multiple Results ---");

            var multiReader = new MockDataReader();
            while (multiReader.Read())
            {
                Console.WriteLine($"   Row: {multiReader[0]}");
            }
            // Output: Row: Alice

            Console.WriteLine("\n=== DataReader Complete ===");
        }
    }

    /// <summary>
    /// Mock DataReader
    /// </summary>
    public class MockDataReader
    {
        private readonly string[,] _data = {
            { "Alice", "30" },
            { "Bob", "25" }
        };
        private int _position;

        public bool Read() => _position < 2;

        public object this[int ordinal] => _data[_position, ordinal];
        public object this[string name] => _data[_position, 0];

        public string GetString(int ordinal) => _data[_position, ordinal];
        public int GetInt32(int ordinal) => int.Parse(_data[_position, ordinal]);

        public string GetString(string name) => _data[_position, 0];
        public int GetInt32(string name) => int.Parse(_data[_position, 1]);
    }
}