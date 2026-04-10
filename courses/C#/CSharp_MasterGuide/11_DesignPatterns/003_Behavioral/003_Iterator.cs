/*
 * ============================================================
 * TOPIC     : Design Patterns
 * SUBTOPIC  : Behavioral - Iterator Pattern
 * FILE      : 05_Iterator.cs
 * PURPOSE   : Demonstrates Iterator design pattern in C#
 * ============================================================
 */
using System; // needed for Console, basic types
using System.Collections; // needed for IEnumerator

namespace CSharp_MasterGuide._11_DesignPatterns._03_Behavioral
{
    /// <summary>
    /// Demonstrates Iterator pattern
    /// </summary>
    public class IteratorDemo
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Iterator Pattern ===\n");

            Console.WriteLine("1. Custom Iterator:");
            var collection = new StringCollection();
            collection.Add("First");
            collection.Add("Second");
            collection.Add("Third");
            
            foreach (var item in collection)
            {
                Console.WriteLine($"   {item}");
            }
            // Output: First, Second, Third

            Console.WriteLine("\n=== Iterator Complete ===");
        }
    }

    public class StringCollection : IEnumerable
    {
        private List<string> _items = new();
        
        public void Add(string item) => _items.Add(item);
        
        public IEnumerator GetEnumerator() => new StringIterator(_items);
    }

    public class StringIterator : IEnumerator
    {
        private List<string> _items;
        private int _position = -1;
        
        public StringIterator(List<string> items) => _items = items;
        
        public object Current => _items[_position];
        
        public bool MoveNext() => ++_position < _items.Count;
        
        public void Reset() => _position = -1;
    }
}