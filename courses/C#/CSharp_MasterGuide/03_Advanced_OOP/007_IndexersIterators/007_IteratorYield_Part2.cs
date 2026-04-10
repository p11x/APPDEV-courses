/*
 * TOPIC: Indexers and Iterators
 * SUBTOPIC: Iterator with Yield Part 2
 * FILE: IteratorYield_Part2.cs
 * PURPOSE: Demonstrate more yield patterns, filtering, and transformation with iterators
 */

using System;
using System.Collections.Generic;

namespace CSharp_MasterGuide._03_Advanced_OOP._07_IndexersIterators
{
    // Multiple yield statements with different conditions
    public class GradeProcessor
    {
        private List<int> _grades = new List<int>();

        public void AddGrade(int grade) => _grades.Add(grade);

        // Yield passing grades (>= 60)
        public IEnumerable<int> GetPassingGrades()
        {
            foreach (var grade in _grades)
            {
                if (grade >= 60)
                    yield return grade;
            }
        }

        // Yield failing grades (< 60)
        public IEnumerable<int> GetFailingGrades()
        {
            foreach (var grade in _grades)
            {
                if (grade < 60)
                    yield return grade;
            }
        }

        // Yield grades in range
        public IEnumerable<int> GetGradesInRange(int min, int max)
        {
            foreach (var grade in _grades)
            {
                if (grade >= min && grade <= max)
                    yield return grade;
            }
        }
    }

    // Iterator transformation patterns
    public class NumberTransformer
    {
        private List<int> _numbers = new List<int>();

        public void Add(int num) => _numbers.Add(num);

        // Transform: Square each number
        public IEnumerable<int> SquareAll()
        {
            foreach (var num in _numbers)
            {
                yield return num * num;
            }
        }

        // Transform: Double each number
        public IEnumerable<int> DoubleAll()
        {
            foreach (var num in _numbers)
            {
                yield return num * 2;
            }
        }

        // Transform with index
        public IEnumerable<(int index, int value)> WithIndex()
        {
            for (int i = 0; i < _numbers.Count; i++)
            {
                yield return (i, _numbers[i]);
            }
        }
    }

    // Chained iterator operations
    public class DataProcessor
    {
        private List<string> _data = new List<string>();

        public void Add(string item) => _data.Add(item);

        // Filter and transform chain
        public IEnumerable<string> FilterAndTransform()
        {
            foreach (var item in _data)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    yield return item.ToUpper();
                }
            }
        }

        // Multiple filtering stages
        public IEnumerable<string> ProcessPipeline(
            Func<string, bool> filter = null,
            Func<string, string> transform = null)
        {
            foreach (var item in _data)
            {
                if (filter != null && !filter(item))
                    continue;

                string processed = transform != null ? transform(item) : item;
                yield return processed;
            }
        }
    }

    // Real-world: Pagination iterator
    public class PagedResult<T>
    {
        private IList<T> _items;
        private int _pageSize;

        public PagedResult(IList<T> items, int pageSize)
        {
            _items = items;
            _pageSize = pageSize;
        }

        // Lazy pagination - yields pages on demand
        public IEnumerable<Page<T>> GetPages()
        {
            for (int i = 0; i < _items.Count; i += _pageSize)
            {
                int pageNumber = (i / _pageSize) + 1;
                var pageItems = new List<T>();

                for (int j = i; j < i + _pageSize && j < _items.Count; j++)
                {
                    pageItems.Add(_items[j]);
                }

                yield return new Page<T>(pageNumber, pageItems, _items.Count);
            }
        }
    }

    public class Page<T>
    {
        public int PageNumber { get; }
        public IList<T> Items { get; }
        public int TotalItems { get; }
        public int TotalPages { get; }

        public Page(int pageNumber, IList<T> items, int totalItems)
        {
            PageNumber = pageNumber;
            Items = items;
            TotalItems = totalItems;
            TotalPages = (int)Math.Ceiling((double)totalItems / items.Count);
        }
    }

    // Real-world: Event log iterator with filtering
    public class EventLogEntry
    {
        public DateTime Timestamp { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }

        public override string ToString() => $"[{Timestamp:HH:mm:ss}] [{Level}] {Message}";
    }

    public class EventLogIterator
    {
        private List<EventLogEntry> _entries = new List<EventLogEntry>();

        public void Add(EventLogEntry entry) => _entries.Add(entry);

        // Filter by severity level
        public IEnumerable<EventLogEntry> GetByLevel(string level)
        {
            foreach (var entry in _entries)
            {
                if (entry.Level == level)
                    yield return entry;
            }
        }

        // Filter by date range
        public IEnumerable<EventLogEntry> GetByDateRange(DateTime start, DateTime end)
        {
            foreach (var entry in _entries)
            {
                if (entry.Timestamp >= start && entry.Timestamp <= end)
                    yield return entry;
            }
        }

        // Get errors only
        public IEnumerable<EventLogEntry> GetErrors() => GetByLevel("ERROR");

        // Get warnings and above
        public IEnumerable<EventLogEntry> GetWarningsAndAbove()
        {
            string[] levels = { "ERROR", "WARNING", "CRITICAL" };
            foreach (var entry in _entries)
            {
                if (Array.Exists(levels, l => l == entry.Level))
                    yield return entry;
            }
        }
    }

    // Real-world: Tree traversal iterator
    public class TreeNode<T>
    {
        public T Value { get; set; }
        public List<TreeNode<T>> Children = new List<TreeNode<T>>();

        public TreeNode(T value)
        {
            Value = value;
        }

        public void AddChild(TreeNode<T> child) => Children.Add(child);
    }

    public class TreeIterator<T>
    {
        private TreeNode<T> _root;

        public TreeIterator(TreeNode<T> root)
        {
            _root = root;
        }

        // Depth-first traversal
        public IEnumerable<TreeNode<T>> DepthFirst()
        {
            yield return _root;

            foreach (var child in _root.Children)
            {
                var iterator = new TreeIterator<T>(child);
                foreach (var node in iterator.DepthFirst())
                {
                    yield return node;
                }
            }
        }

        // Get all values depth-first
        public IEnumerable<T> GetAllValuesDepthFirst()
        {
            foreach (var node in DepthFirst())
            {
                yield return node.Value;
            }
        }

        // Breadth-first traversal
        public IEnumerable<TreeNode<T>> BreadthFirst()
        {
            var queue = new Queue<TreeNode<T>>();
            queue.Enqueue(_root);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                yield return current;

                foreach (var child in current.Children)
                {
                    queue.Enqueue(child);
                }
            }
        }
    }

    public class IteratorYieldPart2
    {
        public static void Main()
        {
            Console.WriteLine("=== Iterator Yield Part 2 Demo ===\n");

            // Example 1: Grade processor with filtering
            Console.WriteLine("--- Grade Processing ---");
            var grades = new GradeProcessor();
            grades.AddGrade(85);
            grades.AddGrade(45);
            grades.AddGrade(72);
            grades.AddGrade(55);
            grades.AddGrade(90);

            Console.WriteLine("Passing grades:");
            foreach (var grade in grades.GetPassingGrades())
                Console.WriteLine($"  {grade}"); // Output: 85, 72, 90

            Console.WriteLine("Failing grades:");
            foreach (var grade in grades.GetFailingGrades())
                Console.WriteLine($"  {grade}"); // Output: 45, 55

            Console.WriteLine("Grades 60-80:");
            foreach (var grade in grades.GetGradesInRange(60, 80))
                Console.WriteLine($"  {grade}"); // Output: 72
            Console.WriteLine();

            // Example 2: Number transformation
            Console.WriteLine("--- Number Transformation ---");
            var transformer = new NumberTransformer();
            transformer.Add(1);
            transformer.Add(2);
            transformer.Add(3);
            transformer.Add(4);

            Console.WriteLine("Squared:");
            foreach (var n in transformer.SquareAll())
                Console.Write($"{n} "); // Output: 1 4 9 16
            Console.WriteLine();

            Console.WriteLine("Doubled:");
            foreach (var n in transformer.DoubleAll())
                Console.Write($"{n} "); // Output: 2 4 6 8
            Console.WriteLine();

            Console.WriteLine("With index:");
            foreach (var (index, value) in transformer.WithIndex())
                Console.WriteLine($"  [{index}]: {value}");
            // Output:
            //   [0]: 1
            //   [1]: 2
            //   [2]: 3
            //   [3]: 4
            Console.WriteLine();

            // Example 3: Chained operations
            Console.WriteLine("--- Chained Operations ---");
            var processor = new DataProcessor();
            processor.Add("hello");
            processor.Add("");
            processor.Add("world");
            processor.Add("csharp");

            Console.WriteLine("Filter and transform:");
            foreach (var item in processor.FilterAndTransform())
                Console.WriteLine($"  {item}"); // Output: HELLO, WORLD, CSHARP
            Console.WriteLine();

            // Example 4: Real-world - Pagination
            Console.WriteLine("--- Real-World: Pagination ---");
            var items = new List<string> { "A", "B", "C", "D", "E", "F", "G", "H" };
            var paged = new PagedResult<string>(items, 3);

            foreach (var page in paged.GetPages())
            {
                Console.WriteLine($"Page {page.PageNumber}/{page.TotalPages}:");
                foreach (var item in page.Items)
                    Console.WriteLine($"  {item}");
            }
            // Output:
            // Page 1/3: A, B, C
            // Page 2/3: D, E, F
            // Page 3/3: G, H
            Console.WriteLine();

            // Example 5: Real-world - Event log
            Console.WriteLine("--- Real-World: Event Log ---");
            var log = new EventLogIterator();
            log.Add(new EventLogEntry { Timestamp = DateTime.Now.AddHours(-2), Level = "INFO", Message = "Application started" });
            log.Add(new EventLogEntry { Timestamp = DateTime.Now.AddHours(-1), Level = "WARNING", Message = "Low memory" });
            log.Add(new EventLogEntry { Timestamp = DateTime.Now.AddMinutes(-30), Level = "ERROR", Message = "Connection failed" });
            log.Add(new EventLogEntry { Timestamp = DateTime.Now.AddMinutes(-15), Level = "CRITICAL", Message = "Service down" });

            Console.WriteLine("Errors:");
            foreach (var entry in log.GetErrors())
                Console.WriteLine($"  {entry}");
            // Output: [ERROR] Connection failed

            Console.WriteLine("Warnings and above:");
            foreach (var entry in log.GetWarningsAndAbove())
                Console.WriteLine($"  {entry}");
            // Output: [WARNING] Low memory, [ERROR] Connection failed, [CRITICAL] Service down
            Console.WriteLine();

            // Example 6: Real-world - Tree traversal
            Console.WriteLine("--- Real-World: Tree Traversal ---");
            var root = new TreeNode<string>("Root");
            var child1 = new TreeNode<string>("Child1");
            var child2 = new TreeNode<string>("Child2");
            var grandchild1 = new TreeNode<string>("Grandchild1");
            var grandchild2 = new TreeNode<string>("Grandchild2");

            child1.AddChild(grandchild1);
            child1.AddChild(grandchild2);
            root.AddChild(child1);
            root.AddChild(child2);

            var treeIter = new TreeIterator<string>(root);

            Console.WriteLine("Depth-first:");
            foreach (var node in treeIter.DepthFirst())
                Console.WriteLine($"  {node.Value}");
            // Output: Root, Child1, Grandchild1, Grandchild2, Child2

            Console.WriteLine("Breadth-first:");
            foreach (var node in treeIter.BreadthFirst())
                Console.WriteLine($"  {node.Value}");
            // Output: Root, Child1, Child2, Grandchild1, Grandchild2
        }
    }
}
