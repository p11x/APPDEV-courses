/*
 * TOPIC: Indexers and Iterators
 * SUBTOPIC: Indexer Basics Part 2
 * FILE: IndexerBasics_Part2.cs
 * PURPOSE: Demonstrate multi-parameter indexers, read-only indexers, and indexer overloading
 */

using System;

namespace CSharp_MasterGuide._03_Advanced_OOP._07_IndexersIterators
{
    // Multi-parameter indexer example
    public class Matrix
    {
        private double[,] _data;
        private int _rows;
        private int _cols;

        public Matrix(int rows, int cols)
        {
            _rows = rows;
            _cols = cols;
            _data = new double[rows, cols];
        }

        // Multi-parameter indexer using comma-separated parameters
        public double this[int row, int col]
        {
            get
            {
                ValidateIndices(row, col);
                return _data[row, col];
            }
            set
            {
                ValidateIndices(row, col);
                _data[row, col] = value;
            }
        }

        private void ValidateIndices(int row, int col)
        {
            if (row < 0 || row >= _rows || col < 0 || col >= _cols)
                throw new IndexOutOfRangeException($"Invalid position ({row}, {col})");
        }

        public int Rows => _rows;
        public int Cols => _cols;
    }

    // Read-only indexer example
    public class ReadOnlyDataStore
    {
        private string[] _data = { "Alpha", "Beta", "Gamma", "Delta", "Epsilon" };

        // Read-only indexer - only has get accessor
        public string this[int index]
        {
            get
            {
                if (index < 0 || index >= _data.Length)
                    throw new IndexOutOfRangeException("Index out of range");
                return _data[index];
            }
        }

        public int Length => _data.Length;
    }

    // Indexer overloading example - same class with different parameter types
    public class FlexibleContainer
    {
        private Dictionary<string, string> _stringData = new Dictionary<string, string>();
        private Dictionary<int, string> _intData = new Dictionary<int, string>();

        // String indexer overload
        public string this[string key]
        {
            get
            {
                return _stringData.ContainsKey(key) ? _stringData[key] : null;
            }
            set
            {
                _stringData[key] = value;
            }
        }

        // Integer indexer overload
        public string this[int index]
        {
            get
            {
                return _intData.ContainsKey(index) ? _intData[index] : null;
            }
            set
            {
                _intData[index] = value;
            }
        }

        // Two-parameter indexer overload
        public string this[string category, string key]
        {
            get
            {
                return $"{category}:{key}";
            }
        }
    }

    // Real-world example: Spreadsheet cell access
    public class Spreadsheet
    {
        private string[,] _cells;
        private int _rows;
        private int _cols;

        public Spreadsheet(int rows, int cols)
        {
            _rows = rows;
            _cols = cols;
            _cells = new string[rows, cols];
        }

        // Multi-parameter indexer for cell reference (A1, B2, etc.)
        public string this[string cellReference]
        {
            get
            {
                var (row, col) = ParseCellReference(cellReference);
                return _cells[row, col];
            }
            set
            {
                var (row, col) = ParseCellReference(cellReference);
                _cells[row, col] = value;
            }
        }

        // Overloaded: Access by row and column numbers
        public string this[int row, int col]
        {
            get
            {
                ValidatePosition(row, col);
                return _cells[row, col];
            }
            set
            {
                ValidatePosition(row, col);
                _cells[row, col] = value;
            }
        }

        private (int row, int col) ParseCellReference(string reference)
        {
            // Parse format "A1" where letter is column and number is row
            int col = reference[0] - 'A';
            int row = int.Parse(reference.Substring(1)) - 1;
            ValidatePosition(row, col);
            return (row, col);
        }

        private void ValidatePosition(int row, int col)
        {
            if (row < 0 || row >= _rows || col < 0 || col >= _cols)
                throw new IndexOutOfRangeException($"Invalid cell position");
        }

        public int RowCount => _rows;
        public int ColCount => _cols;
    }

    // Real-world example: Configuration settings with defaults
    public class ConfigurationManager
    {
        private Dictionary<string, object> _settings = new Dictionary<string, object>();
        private Dictionary<string, object> _defaults = new Dictionary<string, object>()
        {
            { "timeout", 30 },
            { "maxRetries", 3 },
            { "debug", false },
            { "serverUrl", "http://localhost" }
        };

        // Generic read-only indexer with default values
        public T Get<T>(string key)
        {
            if (_settings.ContainsKey(key))
                return (T)_settings[key];
            if (_defaults.ContainsKey(key))
                return (T)_defaults[key];
            return default(T);
        }

        // Set configuration value
        public void Set<T>(string key, T value)
        {
            _settings[key] = value;
        }

        // Read-only indexer for easy access
        public object this[string key] => Get<object>(key);
    }

    public class IndexerBasicsPart2
    {
        public static void Main()
        {
            Console.WriteLine("=== Indexer Basics Part 2 Demo ===\n");

            // Example 1: Multi-parameter indexer (Matrix)
            Console.WriteLine("--- Matrix Multi-Parameter Indexer ---");
            var matrix = new Matrix(3, 3);
            matrix[0, 0] = 1.0;
            matrix[0, 1] = 2.0;
            matrix[1, 0] = 3.0;
            matrix[1, 1] = 4.0;

            Console.WriteLine($"Matrix[0,0]: {matrix[0, 0]}"); // Output: Matrix[0,0]: 1
            Console.WriteLine($"Matrix[0,1]: {matrix[0, 1]}"); // Output: Matrix[0,1]: 2
            Console.WriteLine($"Matrix[1,1]: {matrix[1, 1]}"); // Output: Matrix[1,1]: 4
            Console.WriteLine();

            // Example 2: Read-only indexer
            Console.WriteLine("--- Read-Only Indexer ---");
            var readOnlyStore = new ReadOnlyDataStore();
            Console.WriteLine($"Item at index 0: {readOnlyStore[0]}"); // Output: Item at index 0: Alpha
            Console.WriteLine($"Item at index 2: {readOnlyStore[2]}"); // Output: Item at index 2: Gamma

            // Cannot assign: readOnlyStore[0] = "Test"; // Would cause compile error
            Console.WriteLine();

            // Example 3: Indexer overloading
            Console.WriteLine("--- Indexer Overloading ---");
            var container = new FlexibleContainer();
            container["name"] = "Application";
            container[0] = "First";
            container["category", "key"] = "Value";

            Console.WriteLine($"String key: {container["name"]}");     // Output: String key: Application
            Console.WriteLine($"Int key: {container[0]}");            // Output: Int key: First
            Console.WriteLine($"Two params: {container["category", "key"]}"); // Output: Two params: category:key
            Console.WriteLine();

            // Example 4: Real-world - Spreadsheet
            Console.WriteLine("--- Real-World: Spreadsheet ---");
            var spreadsheet = new Spreadsheet(5, 5);
            spreadsheet["A1"] = "Name";
            spreadsheet["B1"] = "Age";
            spreadsheet["A2"] = "John";
            spreadsheet["B2"] = "30";

            Console.WriteLine($"Cell A1: {spreadsheet["A1"]}");   // Output: Cell A1: Name
            Console.WriteLine($"Cell B1: {spreadsheet["B1"]}");   // Output: Cell B1: Age
            Console.WriteLine($"Cell A2: {spreadsheet["A2"]}");   // Output: Cell A2: John
            Console.WriteLine($"Cell B2: {spreadsheet["B2"]}");   // Output: Cell B2: 30

            // Using numeric indices
            spreadsheet[0, 0] = "Header1";
            Console.WriteLine($"Cell [0,0]: {spreadsheet[0, 0]}"); // Output: Cell [0,0]: Header1
            Console.WriteLine();

            // Example 5: Real-world - Configuration Manager
            Console.WriteLine("--- Real-World: Configuration Manager ---");
            var config = new ConfigurationManager();
            config.Set("timeout", 60);
            config.Set("debug", true);

            Console.WriteLine($"Timeout: {config.Get<int>("timeout")}");      // Output: Timeout: 60
            Console.WriteLine($"Debug: {config.Get<bool>("debug")}");         // Output: Debug: True
            Console.WriteLine($"MaxRetries (default): {config.Get<int>("maxRetries")}"); // Output: MaxRetries (default): 3
            Console.WriteLine($"ServerUrl (default): {config.Get<string>("serverUrl")}"); // Output: ServerUrl (default): http://localhost
        }
    }
}
