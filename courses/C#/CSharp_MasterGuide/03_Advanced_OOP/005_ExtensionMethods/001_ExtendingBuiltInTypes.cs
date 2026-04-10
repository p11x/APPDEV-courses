/*
 * TOPIC: Extension Methods
 * SUBTOPIC: Extending Built-in Types
 * FILE: ExtendingBuiltInTypes.cs
 * PURPOSE: Demonstrates extending common built-in types like string, int, List, and IEnumerable
 *          with practical extension methods for common operations.
 */

using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharp_MasterGuide._03_Advanced_OOP._05_ExtensionMethods
{
    public static class StringBuiltInExtensions
    {
        // Check if string is a valid email
        public static bool IsValidEmail(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;

            try
            {
                var addr = new System.Net.Mail.MailAddress(input);
                return addr.Address == input;
            }
            catch
            {
                return false;
            }
        }

        // Check if string contains only digits
        public static bool IsNumeric(this string input)
        {
            return !string.IsNullOrEmpty(input) && input.All(char.IsDigit);
        }

        // Remove all whitespace from string
        public static string RemoveWhitespace(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            return new string(input.Where(c => !char.IsWhiteSpace(c)).ToArray());
        }

        // Repeat string n times
        public static string Repeat(this string input, int count)
        {
            if (string.IsNullOrEmpty(input) || count <= 0)
                return string.Empty;

            return new string(Enumerable.Repeat(input, count).SelectMany(s => s).ToArray());
        }

        // Pad string to specified length with custom character
        public static string PadBoth(this string input, int totalLength, char paddingChar = ' ')
        {
            if (string.IsNullOrEmpty(input))
                return new string(paddingChar, totalLength);

            int diff = totalLength - input.Length;
            if (diff <= 0)
                return input;

            int leftPad = diff / 2;
            int rightPad = diff - leftPad;
            return new string(paddingChar, leftPad) + input + new string(paddingChar, rightPad);
        }
    }

    public static class IntBuiltInExtensions
    {
        // Check if number is prime
        public static bool IsPrime(this int number)
        {
            if (number <= 1)
                return false;
            if (number <= 3)
                return true;
            if (number % 2 == 0 || number % 3 == 0)
                return false;

            for (int i = 5; i * i <= number; i += 6)
            {
                if (number % i == 0 || number % (i + 2) == 0)
                    return false;
            }
            return true;
        }

        // Check if number is even
        public static bool IsEven(this int number)
        {
            return number % 2 == 0;
        }

        // Check if number is within range
        public static bool IsInRange(this int number, int min, int max)
        {
            return number >= min && number <= max;
        }

        // Format number as currency
        public static string ToCurrency(this int amount, string symbol = "$")
        {
            return $"{symbol}{amount:N0}";
        }

        // Get factorial (with overflow check)
        public static int Factorial(this int number)
        {
            if (number < 0)
                throw new ArgumentException("Factorial is not defined for negative numbers");
            if (number > 12)
                throw new ArgumentException("Factorial would overflow for numbers greater than 12");

            int result = 1;
            for (int i = 2; i <= number; i++)
                result *= i;
            return result;
        }
    }

    public static class ListBuiltInExtensions
    {
        // Add multiple items to list at once
        public static void AddRange<T>(this List<T> list, params T[] items)
        {
            foreach (var item in items)
            {
                list.Add(item);
            }
        }

        // Check if list is null or empty
        public static bool IsNullOrEmpty<T>(this List<T> list)
        {
            return list == null || list.Count == 0;
        }

        // Swap elements at two indices
        public static void Swap<T>(this List<T> list, int index1, int index2)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));
            if (index1 < 0 || index1 >= list.Count)
                throw new ArgumentOutOfRangeException(nameof(index1));
            if (index2 < 0 || index2 >= list.Count)
                throw new ArgumentOutOfRangeException(nameof(index2));

            T temp = list[index1];
            list[index1] = list[index2];
            list[index2] = temp;
        }

        // Get random element from list
        public static T RandomElement<T>(this List<T> list)
        {
            if (list.IsNullOrEmpty())
                throw new InvalidOperationException("List is empty or null");

            var random = new Random();
            return list[random.Next(list.Count)];
        }

        // Shuffle the list in place
        public static void Shuffle<T>(this List<T> list)
        {
            if (list.IsNullOrEmpty())
                return;

            var random = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }

    public static class IEnumerableBuiltInExtensions
    {
        // Check if IEnumerable is null or empty
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            return source == null || !source.Any();
        }

        // Get first element or default value
        public static T FirstOrDefault<T>(this IEnumerable<T> source, T defaultValue)
        {
            if (source == null)
                return defaultValue;

            foreach (var item in source)
                return item;

            return defaultValue;
        }

        // ForEach on IEnumerable (commonly used in LINQ-like scenarios)
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            foreach (var item in source)
            {
                action(item);
            }
        }

        // Batch elements into chunks
        public static IEnumerable<List<T>> Batch<T>(this IEnumerable<T> source, int batchSize)
        {
            var batch = new List<T>(batchSize);

            foreach (var item in source)
            {
                batch.Add(item);
                if (batch.Count == batchSize)
                {
                    yield return batch;
                    batch = new List<T>(batchSize);
                }
            }

            if (batch.Count > 0)
                yield return batch;
        }

        // Distinct by specific property
        public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();
            foreach (var item in source)
            {
                var key = keySelector(item);
                if (seenKeys.Add(key))
                    yield return item;
            }
        }
    }

    class ExtendingBuiltInTypes
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== STRING EXTENSIONS ===");
            
            string email = "user@example.com";
            Console.WriteLine($"Is '{email}' valid email: {email.IsValidEmail()}");
            // Output: Is 'user@example.com' valid email: True

            string invalidEmail = "not-an-email";
            Console.WriteLine($"Is '{invalidEmail}' valid email: {invalidEmail.IsValidEmail()}");
            // Output: Is 'not-an-email' valid email: False

            string numericStr = "12345";
            Console.WriteLine($"Is '{numericStr}' numeric: {numericStr.IsNumeric()}");
            // Output: Is '12345' numeric: True

            string withSpaces = "  Hello   World  ";
            Console.WriteLine($"Without whitespace: '{withSpaces.RemoveWhitespace()}'");
            // Output: Without whitespace: 'HelloWorld'

            Console.WriteLine($"Repeat 'abc' 3 times: {'abc'.Repeat(3)}");
            // Output: Repeat 'abc' 3 times: abcabcabc

            Console.WriteLine($"Padded: '{"test".PadBoth(10, '*')}'");
            // Output: Padded: '**test***'

            Console.WriteLine("\n=== INT EXTENSIONS ===");

            int number = 17;
            Console.WriteLine($"Is {number} prime: {number.IsPrime()}");
            // Output: Is 17 prime: True

            int evenNumber = 42;
            Console.WriteLine($"Is {evenNumber} even: {evenNumber.IsEven()}");
            // Output: Is 42 even: True

            int testRange = 5;
            Console.WriteLine($"Is {testRange} in range [1,10]: {testRange.IsInRange(1, 10)}");
            // Output: Is 5 in range [1,10]: True

            Console.WriteLine($"Currency format: {1000.ToCurrency("€")}");
            // Output: Currency format: €1,000

            Console.WriteLine($"5! = {5.Factorial()}");
            // Output: 5! = 120

            Console.WriteLine("\n=== LIST EXTENSIONS ===");

            var names = new List<string> { "Alice", "Bob" };
            names.AddRange("Charlie", "David", "Eve");
            Console.WriteLine($"List after AddRange: {string.Join(", ", names)}");
            // Output: List after AddRange: Alice, Bob, Charlie, David, Eve

            var emptyList = new List<int>();
            Console.WriteLine($"Is empty list null or empty: {emptyList.IsNullOrEmpty()}");
            // Output: Is empty list null or empty: True

            var numbers = new List<int> { 1, 2, 3, 4, 5 };
            numbers.Swap(0, 4);
            Console.WriteLine($"After swap: {string.Join(", ", numbers)}");
            // Output: After swap: 5, 2, 3, 4, 1

            var randomList = new List<string> { "A", "B", "C", "D" };
            randomList.Shuffle();
            Console.WriteLine($"After shuffle: {string.Join(", ", randomList)}");
            // Output: After shuffle: (random order)

            Console.WriteLine("\n=== IEnumerable EXTENSIONS ===");

            var namesList = new List<string> { "Alice", "Bob", "Charlie" };
            namesList.ForEach(name => Console.WriteLine($"Hello, {name}!"));
            // Output: Hello, Alice!
            // Output: Hello, Bob!
            // Output: Hello, Charlie!

            var items = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var batches = items.Batch(3).ToList();
            Console.WriteLine($"Batches of 3: {batches.Count}");
            // Output: Batches of 3: 3

            var people = new List<Person1> 
            { 
                new Person1 { Name = "Alice", Age = 30 },
                new Person1 { Name = "Bob", Age = 25 },
                new Person1 { Name = "Alice", Age = 35 }
            };
            var distinct = people.DistinctBy(p => p.Name).ToList();
            Console.WriteLine($"Distinct by Name: {string.Join(", ", distinct.Select(p => p.Name))}");
            // Output: Distinct by Name: Alice, Bob
        }
    }

    public class Person1
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
