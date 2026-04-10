/*
 * ============================================================
 * TOPIC     : Advanced OOP
 * SUBTOPIC  : Generics - Generic Methods
 * FILE      : GenericMethods.cs
 * PURPOSE   : Teaches generic method syntax, type inference,
 *            method overloading with generics, and return types
 * ============================================================
 */

using System;
using System.Collections.Generic;

namespace CSharp_MasterGuide._03_Advanced_OOP._01_Generics
{
    class GenericMethods
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Generic Methods in C# ===\n");

            // ═══════════════════════════════════════════════════════════
            // SECTION 1: Basic Generic Methods
            // ═══════════════════════════════════════════════════════════

            // Generic method with single type parameter
            int num1 = 10, num2 = 20;
            Swap(ref num1, ref num2);
            Console.WriteLine($"After swap: num1 = {num1}, num2 = {num2}");
            // Output: After swap: num1 = 20, num2 = 10

            string str1 = "First", str2 = "Second";
            Swap(ref str1, ref str2);
            Console.WriteLine($"After swap: str1 = {str1}, str2 = {str2}");
            // Output: After swap: str1 = Second, str2 = First

            // ═══════════════════════════════════════════════════════════
            // SECTION 2: Generic Methods with Return Types
            // ═══════════════════════════════════════════════════════════

            // Generic method returning T
            int[] numbers = { 1, 2, 3, 4, 5 };
            var firstNumber = GetFirst(numbers);
            Console.WriteLine($"First number: {firstNumber}");
            // Output: First number: 1

            string[] words = { "apple", "banana", "cherry" };
            var firstWord = GetFirst(words);
            Console.WriteLine($"First word: {firstWord}");
            // Output: First word: apple

            // Empty array returns default value
            int[] emptyNumbers = {};
            var emptyResult = GetFirst(emptyNumbers);
            Console.WriteLine($"Empty array result: {emptyResult}");
            // Output: Empty array result: 0

            // ═══════════════════════════════════════════════════════════
            // SECTION 3: Generic Methods with Multiple Parameters
            // ═══════════════════════════════════════════════════════════

            // Method with two type parameters
            var tuple = CreateTuple("Key", 100);
            Console.WriteLine($"Tuple: {tuple.Item1}, {tuple.Item2}");
            // Output: Tuple: Key, 100

            // Method returning generic pair
            var pair = CreatePair(42, "Answer");
            Console.WriteLine($"Pair: {pair.First}, {pair.Second}");
            // Output: Pair: 42, Answer

            // ═══════════════════════════════════════════════════════════
            // SECTION 4: Real-World Example - Generic Utilities
            // ═══════════════════════════════════════════════════════════

            // Generic array utilities
            int[] intArray = { 5, 2, 8, 1, 9 };
            Console.WriteLine($"Array before sort: {string.Join(", ", intArray)}");
            // Output: Array before sort: 5, 2, 8, 1, 9

            Sort(intArray);
            Console.WriteLine($"Array after sort: {string.Join(", ", intArray)}");
            // Output: Array after sort: 1, 2, 5, 8, 9

            string[] stringArray = { "zebra", "apple", "banana" };
            Sort(stringArray);
            Console.WriteLine($"String array sorted: {string.Join(", ", stringArray)}");
            // Output: String array sorted: apple, banana, zebra

            // Binary search example
            int index = BinarySearch(intArray, 5);
            Console.WriteLine($"Found 5 at index: {index}");
            // Output: Found 5 at index: 2

            // ═══════════════════════════════════════════════════════════
            // SECTION 5: Generic Method with Type Inference
            // ═══════════════════════════════════════════════════════════

            // Type inference - compiler determines type from arguments
            var result1 = FindMax(5, 10);
            Console.WriteLine($"Max of 5, 10: {result1}");
            // Output: Max of 5, 10: 10

            var result2 = FindMax("apple", "banana");
            Console.WriteLine($"Max string: {result2}");
            // Output: Max string: banana

            var result3 = FindMax(3.14, 2.71);
            Console.WriteLine($"Max double: {result3}");
            // Output: Max double: 3.14

            // ═══════════════════════════════════════════════════════════
            // SECTION 6: Generic Method with Generic Collections
            // ═══════════════════════════════════════════════════════════

            List<int> numbersList = new List<int> { 1, 2, 3, 4, 5 };
            var doubled = Transform(numbersList, x => x * 2);
            Console.WriteLine($"Doubled: {string.Join(", ", doubled)}");
            // Output: Doubled: 2, 4, 6, 8, 10

            List<string> wordsList = new List<string> { "Hello", "World" };
            var uppercased = Transform(wordsList, s => s.ToUpper());
            Console.WriteLine($"Uppercased: {string.Join(", ", uppercased)}");
            // Output: Uppercased: HELLO, WORLD

            // Filter example
            var evens = Filter(numbersList, x => x % 2 == 0);
            Console.WriteLine($"Evens: {string.Join(", ", evens)}");
            // Output: Evens: 2, 4

            Console.WriteLine("\n=== Generic Methods Complete ===");
        }

        // ═══════════════════════════════════════════════════════════
        // Basic Generic Method - Swap Two Values
        // ═══════════════════════════════════════════════════════════

        // Generic method that swaps two values of any type
        // The type parameter T is inferred from the arguments
        static void Swap<T>(ref T first, ref T second)
        {
            T temp = first;
            first = second;
            second = temp;
        }

        // ═══════════════════════════════════════════════════════════
        // Generic Method with Return Type - Get First Element
        // ═══════════════════════════════════════════════════════════

        // Returns the first element of an array, or default(T) if empty
        static T GetFirst<T>(T[] array)
        {
            if (array == null || array.Length == 0)
            {
                return default(T);
            }
            return array[0];
        }

        // ═══════════════════════════════════════════════════════════
        // Generic Method with Multiple Type Parameters
        // ═══════════════════════════════════════════════════════════

        // Creates a tuple from two values of different types
        static (T1, T2) CreateTuple<T1, T2>(T1 first, T2 second)
        {
            return (first, second);
        }

        // Simple pair class for demonstration
        class Pair<TFirst, TSecond>
        {
            public TFirst First { get; set; }
            public TSecond Second { get; set; }
        }

        // Creates a pair from two values
        static Pair<TFirst, TSecond> CreatePair<TFirst, TSecond>(TFirst first, TSecond second)
        {
            return new Pair<TFirst, TSecond> { First = first, Second = second };
        }

        // ═══════════════════════════════════════════════════════════
        // Real-World: Generic Sort Method
        // ═══════════════════════════════════════════════════════════

        // Sorts an array using bubble sort algorithm
        // Works with any type that implements IComparable<T>
        static void Sort<T>(T[] array) where T : IComparable<T>
        {
            if (array == null || array.Length <= 1)
                return;

            for (int i = 0; i < array.Length - 1; i++)
            {
                for (int j = 0; j < array.Length - i - 1; j++)
                {
                    if (array[j].CompareTo(array[j + 1]) > 0)
                    {
                        Swap(ref array[j], ref array[j + 1]);
                    }
                }
            }
        }

        // Binary search on a sorted array
        static int BinarySearch<T>(T[] array, T target) where T : IComparable<T>
        {
            if (array == null || array.Length == 0)
                return -1;

            int left = 0;
            int right = array.Length - 1;

            while (left <= right)
            {
                int mid = left + (right - left) / 2;
                int comparison = array[mid].CompareTo(target);

                if (comparison == 0)
                    return mid;
                else if (comparison < 0)
                    left = mid + 1;
                else
                    right = mid - 1;
            }

            return -1;
        }

        // ═══════════════════════════════════════════════════════════
        // Generic Method with IComparable Constraint - Find Max
        // ═══════════════════════════════════════════════════════════

        // Returns the maximum of two values
        static T FindMax<T>(T a, T b) where T : IComparable<T>
        {
            return a.CompareTo(b) > 0 ? a : b;
        }

        // ═══════════════════════════════════════════════════════════
        // Real-World: Generic Transform and Filter
        // ═══════════════════════════════════════════════════════════

        // Transforms each element using a transformation function
        static List<TOutput> Transform<TInput, TOutput>(
            List<TInput> input,
            Func<TInput, TOutput> transformer)
        {
            List<TOutput> result = new List<TOutput>();
            foreach (var item in input)
            {
                result.Add(transformer(item));
            }
            return result;
        }

        // Filters elements based on a predicate
        static List<T> Filter<T>(List<T> input, Func<T, bool> predicate)
        {
            List<T> result = new List<T>();
            foreach (var item in input)
            {
                if (predicate(item))
                {
                    result.Add(item);
                }
            }
            return result;
        }
    }
}