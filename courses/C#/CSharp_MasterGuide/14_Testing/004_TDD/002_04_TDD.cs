/*
 * ============================================================
 * TOPIC     : Testing
 * SUBTOPIC  : Test-Driven Development (TDD)
 * FILE      : 04_TDD.cs
 * PURPOSE   : Demonstrates TDD methodology in C#
 * ============================================================
 */
using System; // needed for Console, basic types
using System.Collections.Generic; // needed for List<T>

namespace CSharp_MasterGuide._14_Testing._04_TDD
{
    /// <summary>
    /// Demonstrates Test-Driven Development
    /// </summary>
    public class TDDDemo
    {
        /// <summary>
        /// Entry point for TDD examples
        /// </summary>
        public static void Main(string[] args)
        {
            // Console.WriteLine = outputs to console
            // Output: === Test-Driven Development ===
            Console.WriteLine("=== Test-Driven Development ===\n");

            // ── CONCEPT: TDD Cycle ───────────────────────────────────────────
            // Red-Green-Refactor

            // Example 1: TDD Cycle
            // Output: 1. TDD Cycle:
            Console.WriteLine("1. TDD Cycle:");
            
            // Step 1: RED - Write failing test first
            // test.AddItem("Apple") should add item to cart
            
            // Step 2: GREEN - Write minimal code to pass
            var cart = new ShoppingCart();
            cart.AddItem("Apple");
            var count = cart.ItemCount;
            // Output: Item added, count: 1
            Console.WriteLine($"   Item added, count: {count}");
            
            // Step 3: REFACTOR - Improve code
            cart.AddItem("Banana");
            cart.AddItem("Orange");
            // Output: Total items: 3
            Console.WriteLine($"   Total items: {cart.ItemCount}");

            // Example 2: TDD with Calculator
            // Output: 2. TDD with Calculator:
            Console.WriteLine("\n2. TDD with Calculator:");
            
            // Test 1: Add
            var calc = new TDDCalculator();
            calc.Push(5);
            calc.Push(3);
            var sum = calc.Add();
            // Output: 5 + 3 = 8
            Console.WriteLine($"   5 + 3 = {sum}");
            
            // Test 2: Multiply
            calc.Push(4);
            calc.Push(2);
            var product = calc.Multiply();
            // Output: 4 * 2 = 8
            Console.WriteLine($"   4 * 2 = {product}");

            Console.WriteLine("\n=== TDD Complete ===");
        }
    }

    /// <summary>
    /// Shopping cart - developed via TDD
    /// </summary>
    public class ShoppingCart
    {
        private List<string> _items = new List<string>();
        
        public void AddItem(string item)
        {
            _items.Add(item);
            Console.WriteLine($"   Added: {item}");
        }
        
        public int ItemCount => _items.Count;
    }

    /// <summary>
    /// Calculator developed via TDD
    /// </summary>
    public class TDDCalculator
    {
        private Stack<int> _stack = new Stack<int>();
        
        public void Push(int value) => _stack.Push(value);
        
        public int Add()
        {
            var b = _stack.Pop();
            var a = _stack.Pop();
            return a + b;
        }
        
        public int Multiply()
        {
            var b = _stack.Pop();
            var a = _stack.Pop();
            return a * b;
        }
    }
}