/*
 * TOPIC: Extension Methods
 * SUBTOPIC: Extension Method Basics - Part 2
 * FILE: ExtensionMethodBasics_Part2.cs
 * PURPOSE: Demonstrates method overloading with extension methods, static vs instance method differences,
 *          and how to resolve conflicts between extension methods and instance methods.
 */

using System;
using System.Collections.Generic;

namespace CSharp_MasterGuide._03_Advanced_OOP._05_ExtensionMethods
{
    public static class OverloadExtensions
    {
        // Extension method with no additional parameters
        public static bool IsNullOrEmpty(this string input)
        {
            return string.IsNullOrEmpty(input);
        }

        // Overloaded version with custom null message
        public static bool IsNullOrEmpty(this string input, string customMessage)
        {
            bool result = string.IsNullOrEmpty(input);
            if (result)
                Console.WriteLine(customMessage);
            return result;
        }

        // Overloaded version with whitespace check
        public static bool IsNullOrEmpty(this string input, bool checkWhitespace)
        {
            if (checkWhitespace)
                return string.IsNullOrWhiteSpace(input);
            return string.IsNullOrEmpty(input);
        }

        // Multiple overloads with different parameter types
        public static int WordCount(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return 0;

            string[] words = input.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            return words.Length;
        }

        // Overload with custom delimiter
        public static int WordCount(this string input, char delimiter)
        {
            if (string.IsNullOrWhiteSpace(input))
                return 0;

            string[] words = input.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
            return words.Length;
        }

        // Overload with array of delimiters
        public static int WordCount(this string input, char[] delimiters)
        {
            if (string.IsNullOrWhiteSpace(input))
                return 0;

            string[] words = input.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            return words.Length;
        }
    }

    public static class ConflictResolutionExtensions
    {
        // Extension method with default parameter value
        public static string ReverseWords(this string input, bool preserveOrder = true)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            string[] words = input.Split(' ');
            
            if (preserveOrder)
            {
                // Reverse the order of words
                Array.Reverse(words);
                return string.Join(" ", words);
            }
            else
            {
                // Reverse each word individually
                for (int i = 0; i < words.Length; i++)
                {
                    char[] charArray = words[i].ToCharArray();
                    Array.Reverse(charArray);
                    words[i] = new string(charArray);
                }
                return string.Join(" ", words);
            }
        }
    }

    public static class StaticVsInstanceExtensions
    {
        // Static method - requires class name to call
        public static string StaticFormatDate(DateTime date)
        {
            return date.ToString("yyyy-MM-dd");
        }

        // Extension method (instance-like) - called on the object
        public static string ToIsoDate(this DateTime date)
        {
            return date.ToString("O"); // ISO 8601 format
        }

        // Extension method with additional options
        public static string ToIsoDate(this DateTime date, bool includeTime)
        {
            return includeTime ? date.ToString("O") : date.ToString("yyyy-MM-dd");
        }

        // Extension method for formatting currency
        public static string FormatCurrency(this decimal amount, string currencySymbol = "$")
        {
            return $"{currencySymbol}{amount:N2}";
        }
    }

    class ExtensionMethodBasics_Part2
    {
        static void Main(string[] args)
        {
            // Method overloading with extension methods
            string sampleText = "Hello World from Extension Methods";
            
            Console.WriteLine($"Default word count: {sampleText.WordCount()}");
            // Output: Default word count: 4

            Console.WriteLine($"Word count (comma delimiter): {sampleText.WordCount(',')}");
            // Output: Word count (comma delimiter): 1

            Console.WriteLine($"Word count (space/tab): {sampleText.WordCount(new[] { ' ', '\t' })}");
            // Output: Word count (space/tab): 4

            // NullOrEmpty overloads
            string empty = "";
            empty.IsNullOrEmpty();
            // Output: (no output - returns false)

            string nullStr = null;
            nullStr.IsNullOrEmpty("Custom message: String is null or empty!");
            // Output: Custom message: String is null or empty!

            string whitespace = "   ";
            Console.WriteLine($"Is NullOrEmpty (whitespace=false): {whitespace.IsNullOrEmpty(false)}");
            // Output: Is NullOrEmpty (whitespace=false): False

            Console.WriteLine($"Is NullOrEmpty (whitespace=true): {whitespace.IsNullOrEmpty(true)}");
            // Output: Is NullOrEmpty (whitespace=true): True

            // Static vs Instance comparison
            DateTime now = DateTime.Now;
            
            // Static method call - requires class name
            string staticFormatted = StaticVsInstanceExtensions.StaticFormatDate(now);
            Console.WriteLine($"Static method result: {staticFormatted}");
            // Output: Static method result: 2026-04-04 (or current date)

            // Extension method call - called on the object
            string isoDate = now.ToIsoDate();
            Console.WriteLine($"Extension method result: {isoDate}");
            // Output: Extension method result: 2026-04-04T09:26:20.0000000+05:30

            // Extension method with optional parameter
            string isoDateNoTime = now.ToIsoDate(false);
            Console.WriteLine($"ISO date without time: {isoDateNoTime}");
            // Output: ISO date without time: 2026-04-04

            // ReverseWords with different options
            string testPhrase = "The Quick Brown Fox";
            Console.WriteLine($"Original: {testPhrase}");
            // Output: Original: The Quick Brown Fox

            Console.WriteLine($"Reverse order: {testPhrase.ReverseWords()}");
            // Output: Reverse order: Fox Brown Quick The

            Console.WriteLine($"Reverse each word: {testPhrase.ReverseWords(false)}");
            // Output: Reverse each word: ehT kciuQ nworf xoF

            // FormatCurrency example
            decimal price = 1234.5678m;
            Console.WriteLine($"Default currency: {price.FormatCurrency()}");
            // Output: Default currency: $1,234.57

            Console.WriteLine($"Euro currency: {price.FormatCurrency("€")}");
            // Output: Euro currency: €1,234.57
        }
    }
}
