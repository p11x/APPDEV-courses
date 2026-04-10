/*
 * TOPIC: Indexers and Iterators
 * SUBTOPIC: Iterator with Yield
 * FILE: IteratorYield.cs
 * PURPOSE: Demonstrate yield return, yield break, and creating custom iterators
 */

using System;
using System.Collections.Generic;

namespace CSharp_MasterGuide._03_Advanced_OOP._07_IndexersIterators
{
    // Basic iterator using yield return
    public class NumberSequence
    {
        private int _start;
        private int _end;

        public NumberSequence(int start, int end)
        {
            _start = start;
            _end = end;
        }

        // Iterator method that yields each number in range
        public IEnumerator<int> GetEnumerator()
        {
            for (int i = _start; i <= _end; i++)
            {
                yield return i; // Yield each number one at a time
            }
        }
    }

    // Iterator with yield break example
    public class EvenNumbers
    {
        private int _max;

        public EvenNumbers(int max)
        {
            _max = max;
        }

        // Yield only even numbers, stop when exceeding max
        public IEnumerable<int> GetEvenNumbers()
        {
            for (int i = 2; i <= _max; i += 2)
            {
                if (i > 100) // Stop condition using yield break
                {
                    yield break; // Early termination
                }
                yield return i;
            }
        }
    }

    // Iterator that filters elements
    public class StringCollection
    {
        private List<string> _strings = new List<string>();

        public void Add(string s) => _strings.Add(s);

        // Yield only non-empty strings
        public IEnumerable<string> GetNonEmpty()
        {
            foreach (var s in _strings)
            {
                if (!string.IsNullOrEmpty(s))
                {
                    yield return s;
                }
            }
        }

        // Yield strings longer than specified length
        public IEnumerable<string> GetLongerThan(int minLength)
        {
            foreach (var s in _strings)
            {
                if (s.Length > minLength)
                {
                    yield return s;
                }
            }
        }
    }

    // Real-world example: Fibonacci sequence generator
    public class FibonacciGenerator
    {
        private int _maxCount;

        public FibonacciGenerator(int maxCount)
        {
            _maxCount = maxCount;
        }

        // Generate Fibonacci numbers lazily
        public IEnumerable<long> Generate()
        {
            long previous = 0;
            long current = 1;

            for (int i = 0; i < _maxCount; i++)
            {
                yield return previous;
                long next = previous + current;
                previous = current;
                current = next;
            }
        }

        // Infinite Fibonacci using yield break (when requested count reached)
        public IEnumerable<long> GenerateInfinite(int count)
        {
            long previous = 0;
            long current = 1;

            for (int i = 0; i < count; i++)
            {
                yield return previous;
                long next = previous + current;
                previous = current;
                current = next;
            }
        }
    }

    // Real-world example: File line reader with lazy loading
    public class LazyFileReader
    {
        private string _filePath;

        public LazyFileReader(string filePath)
        {
            _filePath = filePath;
        }

        // Lazily read lines only when enumerated
        public IEnumerable<string> ReadLines()
        {
            if (!File.Exists(_filePath))
                yield break;

            using (var reader = File.OpenText(_filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        }

        // Read non-empty lines only
        public IEnumerable<string> ReadNonEmptyLines()
        {
            if (!File.Exists(_filePath))
                yield break;

            using (var reader = File.OpenText(_filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                        yield return line;
                }
            }
        }
    }

    public class IteratorYield
    {
        public static void Main()
        {
            Console.WriteLine("=== Iterator with Yield Demo ===\n");

            // Example 1: Basic yield return
            Console.WriteLine("--- Basic Yield Return ---");
            var sequence = new NumberSequence(1, 5);
            foreach (var num in sequence)
            {
                Console.WriteLine($"Number: {num}");
            }
            // Output:
            // Number: 1
            // Number: 2
            // Number: 3
            // Number: 4
            // Number: 5
            Console.WriteLine();

            // Example 2: yield break to stop iteration
            Console.WriteLine("--- Yield Break Example ---");
            var evens = new EvenNumbers(20);
            foreach (var even in evens.GetEvenNumbers())
            {
                Console.WriteLine($"Even: {even}");
            }
            // Output: Even: 2, 4, 6, 8, 10, 12, 14, 16, 18, 20
            Console.WriteLine();

            // Example 3: String filtering with yield
            Console.WriteLine("--- String Filtering ---");
            var strings = new StringCollection();
            strings.Add("Hello");
            strings.Add("");
            strings.Add("World");
            strings.Add(" ");
            strings.Add("C#");

            Console.WriteLine("Non-empty strings:");
            foreach (var s in strings.GetNonEmpty())
            {
                Console.WriteLine($"  {s}");
            }
            // Output:
            //   Hello
            //   World
            //    (space)
            //   C#

            Console.WriteLine("Strings longer than 4:");
            foreach (var s in strings.GetLongerThan(4))
            {
                Console.WriteLine($"  {s}");
            }
            // Output:
            //   Hello
            //   World
            Console.WriteLine();

            // Example 4: Real-world - Fibonacci Generator
            Console.WriteLine("--- Real-World: Fibonacci Generator ---");
            var fib = new FibonacciGenerator(10);
            Console.WriteLine("First 10 Fibonacci numbers:");
            foreach (var num in fib.Generate())
            {
                Console.Write($"{num} "); // Output: 0 1 1 2 3 5 8 13 21 34 
            }
            Console.WriteLine();
            Console.WriteLine();

            // Example 5: Real-world - File reader simulation
            Console.WriteLine("--- Real-World: Lazy File Reader ---");
            // Create a temp file for demonstration
            string tempFile = Path.Combine(Path.GetTempPath(), "demo.txt");
            File.WriteAllText(tempFile, "Line 1\nLine 2\nLine 3\n\nLine 4");

            var reader = new LazyFileReader(tempFile);
            Console.WriteLine("All lines:");
            foreach (var line in reader.ReadLines())
            {
                Console.WriteLine($"  {line}");
            }
            // Output:
            //   Line 1
            //   Line 2
            //   Line 3
            //   Line 4

            Console.WriteLine("Non-empty lines:");
            foreach (var line in reader.ReadNonEmptyLines())
            {
                Console.WriteLine($"  {line}");
            }
            // Output:
            //   Line 1
            //   Line 2
            //   Line 3
            //   Line 4

            // Clean up
            File.Delete(tempFile);
        }
    }
}
