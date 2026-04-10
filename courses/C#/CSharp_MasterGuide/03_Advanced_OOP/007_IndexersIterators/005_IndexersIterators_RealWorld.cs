/*
 * TOPIC: Indexers and Iterators
 * SUBTOPIC: Real-World Applications
 * FILE: IndexersIterators_RealWorld.cs
 * PURPOSE: Demonstrate practical real-world applications of indexers and iterators
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace CSharp_MasterGuide._03_Advanced_OOP._07_IndexersIterators
{
    // Real-world: Sparse matrix with indexer
    public class SparseMatrix
    {
        private Dictionary<(int row, int col), double> _elements = new Dictionary<(int, int), double>();
        private int _rows;
        private int _cols;

        public SparseMatrix(int rows, int cols)
        {
            _rows = rows;
            _cols = cols;
        }

        public double this[int row, int col]
        {
            get
            {
                ValidateIndices(row, col);
                return _elements.TryGetValue((row, col), out double value) ? value : 0.0;
            }
            set
            {
                ValidateIndices(row, col);
                if (value != 0)
                    _elements[(row, col)] = value;
                else
                    _elements.Remove((row, col));
            }
        }

        private void ValidateIndices(int row, int col)
        {
            if (row < 0 || row >= _rows || col < 0 || col >= _cols)
                throw new IndexOutOfRangeException($"Position ({row}, {col}) out of bounds");
        }

        public int NonZeroCount => _elements.Count;
        public (int rows, int cols) Dimensions => (_rows, _cols);

        public IEnumerable<(int row, int col, double value)> GetNonZeroElements()
        {
            foreach (var kvp in _elements)
            {
                yield return (kvp.Key.row, kvp.Key.col, kvp.Value);
            }
        }
    }

    // Real-world: Paginated data collection
    public class PaginatedCollection<T> : IEnumerable<T>
    {
        private IList<T> _allItems = new List<T>();
        private int _pageSize;

        public PaginatedCollection(int pageSize = 10)
        {
            _pageSize = pageSize;
        }

        public void Add(T item) => _allItems.Add(item);
        public void AddRange(IEnumerable<T> items)
        {
            foreach (var item in items)
                _allItems.Add(item);
        }

        public int TotalItems => _allItems.Count;
        public int TotalPages => (int)Math.Ceiling((double)_allItems.Count / _pageSize);

        public IEnumerable<T> GetPage(int pageNumber)
        {
            if (pageNumber < 1 || pageNumber > TotalPages)
                yield break;

            int startIndex = (pageNumber - 1) * _pageSize;
            int endIndex = Math.Min(startIndex + _pageSize, _allItems.Count);

            for (int i = startIndex; i < endIndex; i++)
            {
                yield return _allItems[i];
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _allItems.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    // Real-world: Custom observable collection
    public class ObservableCollection<T> : IEnumerable<T>
    {
        private List<T> _items = new List<T>();

        public event EventHandler<ItemEventArgs<T>> ItemAdded;
        public event EventHandler<ItemEventArgs<T>> ItemRemoved;
        public event EventHandler Cleared;

        public void Add(T item)
        {
            _items.Add(item);
            ItemAdded?.Invoke(this, new ItemEventArgs<T>(item, _items.Count - 1));
        }

        public bool Remove(T item)
        {
            int index = _items.IndexOf(item);
            if (index >= 0)
            {
                _items.RemoveAt(index);
                ItemRemoved?.Invoke(this, new ItemEventArgs<T>(item, index));
                return true;
            }
            return false;
        }

        public void Clear()
        {
            _items.Clear();
            Cleared?.Invoke(this, EventArgs.Empty);
        }

        public T this[int index] => _items[index];
        public int Count => _items.Count;

        public IEnumerator<T> GetEnumerator() => _items.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class ItemEventArgs<T> : EventArgs
    {
        public T Item { get; }
        public int Index { get; }

        public ItemEventArgs(T item, int index)
        {
            Item = item;
            Index = index;
        }
    }

    // Real-world: Time-series data with indexer
    public class TimeSeriesData
    {
        private SortedDictionary<DateTime, double> _data = new SortedDictionary<DateTime, double>();

        public double this[DateTime timestamp]
        {
            get
            {
                if (_data.TryGetValue(timestamp, out double value))
                    return value;
                throw new KeyNotFoundException($"No data at {timestamp}");
            }
            set
            {
                _data[timestamp] = value;
            }
        }

        public bool ContainsKey(DateTime timestamp) => _data.ContainsKey(timestamp);

        public void Add(DateTime timestamp, double value)
        {
            _data[timestamp] = value;
        }

        public IEnumerable<DateTime> GetTimestamps() => _data.Keys;
        public IEnumerable<double> GetValues() => _data.Values;

        public IEnumerable<(DateTime time, double value)> GetRange(DateTime start, DateTime end)
        {
            foreach (var kvp in _data)
            {
                if (kvp.Key >= start && kvp.Key <= end)
                    yield return (kvp.Key, kvp.Value);
            }
        }

        public double GetAverage()
        {
            if (_data.Count == 0) return 0;
            double sum = 0;
            foreach (var value in _data.Values)
                sum += value;
            return sum / _data.Count;
        }
    }

    // Real-world: Multi-dimensional warehouse inventory
    public class WarehouseInventory
    {
        private Dictionary<string, Dictionary<string, int>> _inventory = new Dictionary<string, Dictionary<string, int>>();

        // Three-level indexer: warehouse -> shelf -> product count
        public int this[string warehouse, string shelf, string product]
        {
            get
            {
                if (_inventory.TryGetValue(warehouse, out var shelves))
                {
                    if (shelves.TryGetValue($"{shelf}:{product}", out int count))
                        return count;
                }
                return 0;
            }
            set
            {
                if (!_inventory.ContainsKey(warehouse))
                    _inventory[warehouse] = new Dictionary<string, int>();
                _inventory[warehouse][$"{shelf}:{product}"] = value;
            }
        }

        public int GetProductCount(string warehouse, string shelf, string product)
        {
            return this[warehouse, shelf, product];
        }

        public IEnumerable<string> GetWarehouses()
        {
            return _inventory.Keys;
        }

        public int GetTotalItems(string warehouse)
        {
            if (!_inventory.TryGetValue(warehouse, out var shelves))
                return 0;

            int total = 0;
            foreach (var count in shelves.Values)
                total += count;
            return total;
        }
    }

    // Real-world: Student grade book with composite indexer
    public class GradeBook
    {
        private Dictionary<string, Dictionary<string, int>> _grades = new Dictionary<string, Dictionary<string, int>>();

        // Indexer: student name -> subject -> grade
        public int this[string student, string subject]
        {
            get
            {
                if (_grades.TryGetValue(student, out var subjects))
                {
                    if (subjects.TryGetValue(subject, out int grade))
                        return grade;
                }
                return -1;
            }
            set
            {
                if (!_grades.ContainsKey(student))
                    _grades[student] = new Dictionary<string, int>();
                _grades[student][subject] = value;
            }
        }

        public IEnumerable<string> GetStudents() => _grades.Keys;

        public double GetAverage(string student)
        {
            if (!_grades.TryGetValue(student, out var subjects))
                return 0;

            int sum = 0;
            foreach (var grade in subjects.Values)
                sum += grade;
            return subjects.Count > 0 ? (double)sum / subjects.Count : 0;
        }

        public string GetBestSubject(string student)
        {
            if (!_grades.TryGetValue(student, out var subjects))
                return null;

            string best = null;
            int highest = -1;
            foreach (var kvp in subjects)
            {
                if (kvp.Value > highest)
                {
                    highest = kvp.Value;
                    best = kvp.Key;
                }
            }
            return best;
        }
    }

    public class IndexersIteratorsRealWorld
    {
        public static void Main()
        {
            Console.WriteLine("=== Real-World Indexers and Iterators Demo ===\n");

            // Example 1: Sparse Matrix
            Console.WriteLine("--- Sparse Matrix ---");
            var sparse = new SparseMatrix(5, 5);
            sparse[0, 0] = 1.0;
            sparse[1, 2] = 2.5;
            sparse[3, 3] = 3.75;
            sparse[4, 4] = 4.0;

            Console.WriteLine($"Value at [0,0]: {sparse[0, 0]}");   // Output: 1
            Console.WriteLine($"Value at [1,2]: {sparse[1, 2]}");   // Output: 2.5
            Console.WriteLine($"Value at [2,1] (not set): {sparse[2, 1]}"); // Output: 0
            Console.WriteLine($"Non-zero elements: {sparse.NonZeroCount}"); // Output: 4

            Console.WriteLine("Non-zero positions:");
            foreach (var (row, col, value) in sparse.GetNonZeroElements())
            {
                Console.WriteLine($"  [{row},{col}] = {value}");
            }
            // Output: [0,0]=1, [1,2]=2.5, [3,3]=3.75, [4,4]=4
            Console.WriteLine();

            // Example 2: Paginated Collection
            Console.WriteLine("--- Paginated Collection ---");
            var pages = new PaginatedCollection<string>(3);
            for (int i = 1; i <= 10; i++)
                pages.Add($"Item {i}");

            Console.WriteLine($"Total items: {pages.TotalItems}");   // Output: 10
            Console.WriteLine($"Page size: 3, Total pages: {pages.TotalPages}"); // Output: 4

            for (int page = 1; page <= pages.TotalPages; page++)
            {
                Console.WriteLine($"\nPage {page}:");
                foreach (var item in pages.GetPage(page))
                    Console.WriteLine($"  {item}");
            }
            // Output: Page 1: Item 1-3, Page 2: Item 4-6, etc.
            Console.WriteLine();

            // Example 3: Observable Collection
            Console.WriteLine("--- Observable Collection ---");
            var observable = new ObservableCollection<string>();
            observable.ItemAdded += (s, e) => Console.WriteLine($"  Added: {e.Item} at index {e.Index}");
            observable.ItemRemoved += (s, e) => Console.WriteLine($"  Removed: {e.Item} from index {e.Index}");

            Console.WriteLine("Adding items:");
            observable.Add("First");
            observable.Add("Second");
            observable.Add("Third");

            Console.WriteLine("\nRemoving item:");
            observable.Remove("Second");
            Console.WriteLine();

            // Example 4: Time Series Data
            Console.WriteLine("--- Time Series Data ---");
            var timeSeries = new TimeSeriesData();
            var baseTime = new DateTime(2024, 1, 1, 0, 0, 0);

            timeSeries.Add(baseTime.AddHours(1), 25.5);
            timeSeries.Add(baseTime.AddHours(2), 26.0);
            timeSeries.Add(baseTime.AddHours(3), 27.2);
            timeSeries.Add(baseTime.AddHours(4), 26.8);

            Console.WriteLine("Data points:");
            foreach (var (time, value) in timeSeries.GetRange(baseTime.AddHours(1), baseTime.AddHours(3)))
            {
                Console.WriteLine($"  {time:HH:mm} = {value}°C");
            }
            // Output: 01:00=25.5, 02:00=26.0, 03:00=27.2

            Console.WriteLine($"\nAverage temperature: {timeSeries.GetAverage():F1}°C"); // Output: 26.4
            Console.WriteLine();

            // Example 5: Warehouse Inventory
            Console.WriteLine("--- Warehouse Inventory ---");
            var warehouse = new WarehouseInventory();

            warehouse["WarehouseA", "Shelf1", "Widget"] = 100;
            warehouse["WarehouseA", "Shelf1", "Gadget"] = 50;
            warehouse["WarehouseA", "Shelf2", "Widget"] = 75;
            warehouse["WarehouseB", "Shelf1", "Widget"] = 200;

            Console.WriteLine($"Widget in WarehouseA Shelf1: {warehouse.GetProductCount("WarehouseA", "Shelf1", "Widget")}"); // Output: 100
            Console.WriteLine($"Gadget in WarehouseA Shelf1: {warehouse.GetProductCount("WarehouseA", "Shelf1", "Gadget")}"); // Output: 50
            Console.WriteLine($"Total items in WarehouseA: {warehouse.GetTotalItems("WarehouseA")}"); // Output: 225
            Console.WriteLine();

            // Example 6: Grade Book
            Console.WriteLine("--- Grade Book ---");
            var grades = new GradeBook();

            grades["Alice", "Math"] = 95;
            grades["Alice", "Science"] = 88;
            grades["Alice", "History"] = 92;
            grades["Bob", "Math"] = 78;
            grades["Bob", "Science"] = 82;
            grades["Bob", "History"] = 85;

            Console.WriteLine("Alice's grades:");
            Console.WriteLine($"  Math: {grades["Alice", "Math"]}");        // Output: 95
            Console.WriteLine($"  Science: {grades["Alice", "Science"]}"); // Output: 88
            Console.WriteLine($"  History: {grades["Alice", "History"]}"); // Output: 92
            Console.WriteLine($"  Average: {grades.GetAverage("Alice"):F1}"); // Output: 91.7
            Console.WriteLine($"  Best subject: {grades.GetBestSubject("Alice")}"); // Output: Math

            Console.WriteLine("\nBob's grades:");
            Console.WriteLine($"  Average: {grades.GetAverage("Bob"):F1}"); // Output: 81.7
            Console.WriteLine($"  Best subject: {grades.GetBestSubject("Bob")}"); // Output: History
        }
    }
}
