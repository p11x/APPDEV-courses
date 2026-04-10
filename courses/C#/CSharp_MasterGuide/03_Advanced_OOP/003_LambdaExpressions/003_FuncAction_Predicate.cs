/*
 * TOPIC: CSharp_MasterGuide/03_Advanced_OOP/03_LambdaExpressions
 * SUBTOPIC: Func<T>, Action<T>, Predicate<T>
 * FILE: FuncAction_Predicate.cs
 * PURPOSE: In-depth coverage of the three most common generic delegate types in C#
 */
using System;
using System.Collections.Generic;

namespace CSharp_MasterGuide._03_Advanced_OOP._03_LambdaExpressions
{
    // Sample class for examples
    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Department { get; set; }
        public decimal Salary { get; set; }
    }

    public class FuncActionPredicate
    {
        public static void RunMain(string[] args)
        {
            Console.WriteLine("=== Func<T>, Action<T>, Predicate<T> ===\n");

            // ============================================
            // Func<T, TResult> - RETURNS A VALUE
            // ============================================

            // Func<> always has at least one type parameter (the return type)
            // Last type parameter is always the return type

            // Example 1: Func<T1, TResult> - single parameter
            Func<int, int> square = x => x * x;
            Console.WriteLine($"Func<int, int> - Square of 7: {square(7)}"); // Output: 49

            // Example 2: Func<T1, T2, TResult> - two parameters
            Func<int, int, double> divide = (a, b) => (double)a / b;
            Console.WriteLine($"Func<int, int, double> - 10/4: {divide(10, 4)}"); // Output: 2.5

            // Example 3: Func<T1, T2, T3, TResult> - three parameters
            Func<int, int, int, int> average3 = (a, b, c) => (a + b + c) / 3;
            Console.WriteLine($"Func with 3 params - Avg(10, 20, 30): {average3(10, 20, 30)}"); // Output: 20

            // Example 4: Func<T1, T2, T3, T4, TResult> - four parameters
            Func<int, int, int, int, int> sum4 = (a, b, c, d) => a + b + c + d;
            Console.WriteLine($"Func with 4 params - Sum(1,2,3,4): {sum4(1, 2, 3, 4)}"); // Output: 10

            // Example 5: Func<TResult> - no parameters, returns value
            Func<string> getGreeting = () => "Hello, World!";
            Console.WriteLine($"Func<TResult> - No params: {getGreeting()}"); // Output: Hello, World!

            // ============================================
            // Action<T> - VOID RETURN (SIDE EFFECTS)
            // ============================================

            // Action<> returns void - used for side effects
            // Can have 0 to 16 type parameters

            // Example 6: Action - no parameters
            Action logMessage = () => Console.WriteLine("Logging: Operation started");
            logMessage(); // Output: Logging: Operation started

            // Example 7: Action<T> - single parameter
            Action<string> printLine = s => Console.WriteLine(s);
            printLine("Action with 1 param"); // Output: Action with 1 param

            // Example 8: Action<T1, T2> - two parameters
            Action<string, string> printFormatted = (format, arg) =>
                Console.WriteLine(string.Format(format, arg));
            printFormatted("Value is: {0}", 42); // Output: Value is: 42

            // Example 9: Action<T1, T2, T3> - three parameters
            Action<string, int, ConsoleColor> styledPrint = (msg, count, color) =>
            {
                var original = Console.ForegroundColor;
                Console.ForegroundColor = color;
                for (int i = 0; i < count; i++)
                    Console.WriteLine(msg);
                Console.ForegroundColor = original;
            };
            Console.WriteLine("\nStyled print:");
            // Note: ConsoleColor might not be visible in all terminals
            styledPrint("Important!", 2, ConsoleColor.Cyan);

            // Example 10: Real-world Action use - event handlers
            Action<object, EventArgs> buttonClick = (sender, e) =>
                Console.WriteLine($"Button clicked by {sender}");
            buttonClick("SubmitButton", EventArgs.Empty);

            // ============================================
            // Predicate<T> - RETURNS BOOLEAN
            // ============================================

            // Predicate<T> is essentially Func<T, bool>
            // Used for filtering, validation checks

            // Example 11: Basic Predicate
            Predicate<int> isEven = n => n % 2 == 0;
            Console.WriteLine($"\nPredicate<int> - Is 4 even? {isEven(4)}"); // Output: True
            Console.WriteLine($"Predicate<int> - Is 5 even? {isEven(5)}"); // Output: False

            // Example 12: Predicate with string
            Predicate<string> isNullOrEmpty = s => !string.IsNullOrEmpty(s);
            Console.WriteLine($"Predicate<string> - Is 'test' valid? {isNullOrEmpty("test")}"); // True
            Console.WriteLine($"Predicate<string> - Is '' valid? {isNullOrEmpty("")}"); // False

            // Example 13: Predicate with object
            Predicate<Person> isAdult = p => p.Age >= 18;
            var person = new Person { Name = "John", Age = 25 };
            Console.WriteLine($"Predicate<Person> - Is {person.Name} adult? {isAdult(person)}"); // True

            // ============================================
            // COMBINING AND CHAINING DELEGATES
            // ============================================

            // Example 14: Multicast delegates (combine Action)
            Action<string> delegate1 = s => Console.WriteLine($"1: {s}");
            Action<string> delegate2 = s => Console.WriteLine($"2: {s}");
            Action<string> combined = delegate1 + delegate2;
            Console.WriteLine("\nMulticast delegate:");
            combined("Hello!"); // Output: Two lines

            // Example 15: Removing delegates
            Action<string> removeResult = combined - delegate1;
            Console.WriteLine("\nAfter removing delegate1:");
            removeResult("Hello!"); // Output: Only 2:

            // ============================================
            // REAL-WORLD PATTERNS WITH DELEGATES
            // ============================================

            // Example 16: LINQ-style filtering with Predicate
            var numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var evenNumbers = FilterList(numbers, n => n % 2 == 0);
            Console.WriteLine($"\nEven numbers: {string.Join(", ", evenNumbers)}"); // Output: 2, 4, 6, 8, 10

            // Example 17: Validation with Predicate
            Predicate<Person> isValidEmployee = p =>
                p.Age >= 18 && !string.IsNullOrEmpty(p.Name) && p.Salary > 0;

            var employees = new List<Person>
            {
                new Person { Name = "Alice", Age = 25, Salary = 50000 },
                new Person { Name = "Bob", Age = 17, Salary = 30000 }, // Invalid
                new Person { Name = "", Age = 30, Salary = 60000 }    // Invalid
            };

            var validEmployees = FilterList(employees, isValidEmployee);
            Console.WriteLine($"Valid employees: {validEmployees.Count}"); // Output: 1

            // Example 18: Transformation with Func
            Func<Person, string> personToGreeting = p => $"Hello, {p.Name}!";
            var greetings = TransformList(employees, personToGreeting);
            Console.WriteLine($"\nGreetings:");
            foreach (var g in greetings)
            {
                Console.WriteLine(g);
            }

            // Example 19: Sorting with comparison Func
            var sortedByAge = SortList(employees, (p1, p2) => p1.Age.CompareTo(p2.Age));
            Console.WriteLine($"\nSorted by age:");
            foreach (var p in sortedByAge)
            {
                Console.WriteLine($"{p.Name}: {p.Age}");
            }
        }

        // Generic filter method using Predicate
        public static List<T> FilterList<T>(List<T> list, Predicate<T> predicate)
        {
            var result = new List<T>();
            foreach (var item in list)
            {
                if (predicate(item))
                {
                    result.Add(item);
                }
            }
            return result;
        }

        // Generic transform method using Func
        public static List<TResult> TransformList<T, TResult>(List<T> list, Func<T, TResult> selector)
        {
            var result = new List<TResult>();
            foreach (var item in list)
            {
                result.Add(selector(item));
            }
            return result;
        }

        // Generic sort method using comparison Func
        public static List<T> SortList<T>(List<T> list, Func<T, T, int> comparison)
        {
            var sorted = new List<T>(list);
            sorted.Sort((a, b) => comparison(a, b));
            return sorted;
        }
    }
}