/*
 * ============================================================
 * TOPIC     : C# Fundamentals
 * SUBTOPIC  : Operators - Comparison Operators
 * FILE      : ComparisonOperators.cs
 * PURPOSE   : This file covers comparison (relational) operators in C#: ==, !=, <, >, <=, >=.
 *             These operators compare two values and return boolean results.
 * ============================================================
 */

// --- SECTION: Comparison Operators ---
// Comparison operators compare two values and return true or false
// They are fundamental to conditional logic and decision making

using System;

namespace CSharp_MasterGuide._01_Fundamentals._04_Operators
{
    class ComparisonOperators
    {
        static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════════
            // SECTION: Equality Operators (==, !=)
            // ═══════════════════════════════════════════════════════════════
            
            // ── Equality (==) ───────────────────────────────────────────────
            int a = 10;
            int b = 20;
            bool equal = (a == b);
            Console.WriteLine($"{a} == {b} = {equal}"); // Output: 10 == 20 = False
            
            a = 10;
            b = 10;
            equal = (a == b);
            Console.WriteLine($"{a} == {b} = {equal}"); // Output: 10 == 10 = True
            
            // String equality (case-sensitive by default)
            string str1 = "hello";
            string str2 = "hello";
            bool stringsEqual = str1 == str2;
            Console.WriteLine($"\"hello\" == \"hello\" = {stringsEqual}"); // Output: True
            
            string str3 = "Hello";
            bool caseInsensitive = str1 == str3; // False - case-sensitive
            Console.WriteLine($"\"hello\" == \"Hello\" = {caseInsensitive}"); // Output: False
            
            // ── Inequality (!=) ──────────────────────────────────────────────
            a = 10;
            b = 20;
            bool notEqual = (a != b);
            Console.WriteLine($"{a} != {b} = {notEqual}"); // Output: True
            
            // Strings and inequality
            string name1 = "John";
            string name2 = "Jane";
            bool namesDifferent = name1 != name2;
            Console.WriteLine($"\"{name1}\" != \"{name2}\" = {namesDifferent}"); // Output: True

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Relational Operators (<, >, <=, >=)
            // ═══════════════════════════════════════════════════════════════
            
            // ── Less than (<) ───────────────────────────────────────────────
            int x = 5;
            int y = 10;
            bool lessThan = (x < y);
            Console.WriteLine($"{x} < {y} = {lessThan}"); // Output: True
            
            // ── Greater than (>) ─────────────────────────────────────────────
            bool greaterThan = (x > y);
            Console.WriteLine($"{x} > {y} = {greaterThan}"); // Output: False
            
            // ── Less than or equal (<=) ────────────────────────────────────
            bool lessEqual = (x <= y);
            Console.WriteLine($"{x} <= {y} = {lessEqual}"); // Output: True
            
            x = 10;
            lessEqual = (x <= y);
            Console.WriteLine($"{x} <= {y} = {lessEqual}"); // Output: True (equal)
            
            // ── Greater than or equal (>=) ────────────────────────────────
            x = 15;
            bool greaterEqual = (x >= y);
            Console.WriteLine($"{x} >= {y} = {greaterEqual}"); // Output: True
            
            x = 10;
            greaterEqual = (x >= y);
            Console.WriteLine($"{x} >= {y} = {greaterEqual}"); // Output: True (equal)

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Comparing Different Types
            // ═══════════════════════════════════════════════════════════════
            
            // ── Numeric type comparison ───────────────────────────────────
            int intVal = 10;
            double doubleVal = 10.0;
            bool intDoubleEqual = (intVal == doubleVal);
            Console.WriteLine($"int 10 == double 10.0 = {intDoubleEqual}"); // Output: True
            
            // ── Decimal comparison ─────────────────────────────────────────
            decimal decVal = 10.0m;
            bool decEqual = (intVal == decVal);
            Console.WriteLine($"int 10 == decimal 10.0 = {decEqual}"); // Output: True
            
            // Floating-point precision issues
            double d1 = 0.1 + 0.2;
            double d2 = 0.3;
            bool approxEqual = (d1 == d2); // May be false due to floating-point
            Console.WriteLine($"0.1 + 0.2 == 0.3: {approxEqual}"); // Output: False (precision!)
            
            // Better: use epsilon for comparison
            double epsilon = 0.0001;
            bool approxEqualFixed = Math.Abs(d1 - d2) < epsilon;
            Console.WriteLine($"Approximate equal: {approxEqualFixed}"); // Output: True

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Reference Type Comparison
            // ═══════════════════════════════════════════════════════════════
            
            // For reference types: == compares references by default
            object obj1 = new object();
            object obj2 = new object();
            bool sameReference = (obj1 == obj2);
            Console.WriteLine($"Different objects == : {sameReference}"); // Output: False
            
            // Same reference
            object obj3 = obj1;
            bool sameReference2 = (obj1 == obj3);
            Console.WriteLine($"Same objects == : {sameReference2}"); // Output: True
            
            // String is special: == compares content, not references
            string s1 = new string("test");
            string s2 = new string("test");
            bool stringEqual = (s1 == s2);
            Console.WriteLine($"New strings == : {stringEqual}"); // Output: True (content!)
            
            // ── ReferenceEquals for strict reference comparison ───────────
            bool strictRef = ReferenceEquals(s1, s2);
            Console.WriteLine($"ReferenceEquals(new, new): {strictRef}"); // Output: False

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Real-World Examples
            // ═══════════════════════════════════════════════════════════════
            
            // ── Age verification ───────────────────────────────────────────
            int age = 25;
            bool isAdult = age >= 18;
            bool isSenior = age >= 65;
            bool canVote = isAdult;
            Console.WriteLine($"Age {age}: Adult={isAdult}, Senior={isSenior}, CanVote={canVote}");
            // Output: Adult=True, Senior=False, CanVote=True
            
            // ── Grade calculation ─────────────────────────────────────────
            int score = 85;
            char grade = score switch
            {
                >= 90 => 'A',
                >= 80 => 'B',
                >= 70 => 'C',
                >= 60 => 'D',
                _ => 'F'
            };
            Console.WriteLine($"Score {score} = Grade {grade}"); // Output: Score 85 = Grade B
            
            // ── Range validation ───────────────────────────────────────────
            int value = 75;
            bool inRange = value >= 0 && value <= 100;
            Console.WriteLine($"{value} in [0,100]: {inRange}"); // Output: True
            
            // ── Null-safe comparison ──────────────────────────────────────
            string? nullableStr = null;
            bool isEmptyOrNull = string.IsNullOrEmpty(nullableStr);
            Console.WriteLine($"Null or empty: {isEmptyOrNull}"); // Output: True
            
            nullableStr = "";
            isEmptyOrNull = string.IsNullOrEmpty(nullableStr);
            Console.WriteLine($"Empty string: {isEmptyOrNull}"); // Output: True
            
            nullableStr = "hello";
            isEmptyOrNull = string.IsNullOrEmpty(nullableStr);
            Console.WriteLine($"With value: {isEmptyOrNull}"); // Output: False
            
            // ── Date comparison ───────────────────────────────────────────
            DateTime deadline = new DateTime(2024, 12, 31);
            DateTime today = DateTime.Now;
            bool pastDue = today > deadline;
            bool onTime = today <= deadline;
            Console.WriteLine($"Today: {today:yyyy-MM-dd}, Deadline: {deadline:yyyy-MM-dd}");
            Console.WriteLine($"Past due: {pastDue}, On time: {onTime}");
        }
    }
}
