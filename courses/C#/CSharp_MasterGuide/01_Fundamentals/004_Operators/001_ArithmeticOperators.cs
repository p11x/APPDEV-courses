/*
 * ============================================================
 * TOPIC     : C# Fundamentals
 * SUBTOPIC  : Operators - Arithmetic Operators
 * FILE      : ArithmeticOperators.cs
 * PURPOSE   : This file covers arithmetic operators in C#: +, -, *, /, % and their compound assignments.
 *             These operators perform mathematical calculations on numeric types.
 * ============================================================
 */

// --- SECTION: Arithmetic Operators ---
// Arithmetic operators perform mathematical operations on numeric operands
// They work with integral types, floating-point types, and decimal

using System;

namespace CSharp_MasterGuide._01_Fundamentals._04_Operators
{
    class ArithmeticOperators
    {
        static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════════
            // SECTION: Basic Arithmetic Operators
            // ═══════════════════════════════════════════════════════════════
            
            // ── Addition (+) ───────────────────────────────────────────────
            int a = 10;
            int b = 5;
            int sum = a + b;
            Console.WriteLine($"{a} + {b} = {sum}"); // Output: 10 + 5 = 15
            
            // String concatenation (overloaded +)
            string firstName = "John";
            string lastName = "Doe";
            string fullName = firstName + " " + lastName;
            Console.WriteLine($"Name: {fullName}"); // Output: Name: John Doe
            
            // ── Subtraction (-) ───────────────────────────────────────────
            int difference = a - b;
            Console.WriteLine($"{a} - {b} = {difference}"); // Output: 10 - 5 = 5
            
            // Negative numbers
            int negative = -10;
            int negated = -negative;
            Console.WriteLine($"Negative of {negative} = {negated}"); // Output: Negative of -10 = 10
            
            // ── Multiplication (*) ────────────────────────────────────────
            int product = a * b;
            Console.WriteLine($"{a} * {b} = {product}"); // Output: 10 * 5 = 50
            
            // ── Division (/) ──────────────────────────────────────────────
            int quotient = a / b;
            Console.WriteLine($"{a} / {b} = {quotient}"); // Output: 10 / 5 = 2
            
            // Important: Integer division truncates decimal
            int division = 7 / 2;
            Console.WriteLine($"7 / 2 (int) = {division}"); // Output: 7 / 2 (int) = 3 (not 3.5!)
            
            // Use double/decimal for exact division
            double divisionDouble = 7.0 / 2;
            Console.WriteLine($"7.0 / 2 (double) = {divisionDouble}"); // Output: 7.0 / 2 (double) = 3.5
            
            // ── Modulus (%) ─────────────────────────────────────────────────
            // Returns remainder of division
            int remainder = 7 % 2;
            Console.WriteLine($"7 % 2 = {remainder}"); // Output: 7 % 2 = 1
            
            int evenCheck = 10 % 2;
            Console.WriteLine($"10 % 2 = {evenCheck} (even: {evenCheck == 0})"); // Output: 10 % 2 = 0 (even: True)
            
            int oddCheck = 11 % 2;
            Console.WriteLine($"11 % 2 = {oddCheck} (odd: {oddCheck != 0})"); // Output: 11 % 2 = 1 (odd: True)

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Compound Assignment Operators
            // ═══════════════════════════════════════════════════════════════
            
            // ── += (Add and assign) ────────────────────────────────────────
            int value = 10;
            value += 5; // Same as: value = value + 5;
            Console.WriteLine($"value += 5: {value}"); // Output: value += 5: 15
            
            // String concatenation with +=
            string message = "Hello";
            message += " World";
            Console.WriteLine($"message: {message}"); // Output: message: Hello World
            
            // ── -= (Subtract and assign) ───────────────────────────────────
            value = 10;
            value -= 3;
            Console.WriteLine($"value -= 3: {value}"); // Output: value -= 3: 7
            
            // ── *= (Multiply and assign) ───────────────────────────────────
            value = 10;
            value *= 2;
            Console.WriteLine($"value *= 2: {value}"); // Output: value *= 2: 20
            
            // ── /= (Divide and assign) ─────────────────────────────────────
            value = 10;
            value /= 4;
            Console.WriteLine($"value /= 4: {value}"); // Output: value /= 4: 2 (integer division)
            
            // ── %= (Modulus and assign) ───────────────────────────────────
            value = 10;
            value %= 3;
            Console.WriteLine($"value %= 3: {value}"); // Output: value %= 3: 1

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Unary Increment/Decrement
            // ═══════════════════════════════════════════════════════════════
            
            // ── Prefix (++x): Increment then use ───────────────────────────
            int counter = 5;
            int prefixResult = ++counter; // Increment first, then assign
            Console.WriteLine($"++counter: result={prefixResult}, counter={counter}");
            // Output: ++counter: result=6, counter=6
            
            // ── Postfix (x++): Use then increment ──────────────────────────
            counter = 5;
            int postfixResult = counter++; // Use first, then increment
            Console.WriteLine($"counter++: result={postfixResult}, counter={counter}");
            // Output: counter++: result=5, counter=6
            
            // Practical difference
            Console.WriteLine("--- Practical Examples ---");
            
            int i = 0;
            Console.WriteLine(++i); // Output: 1 (prints incremented value)
            
            i = 0;
            Console.WriteLine(i++); // Output: 0 (prints original, then increments)
            Console.WriteLine(i); // Output: 1
            
            // ── Decrement (--x, x--) ───────────────────────────────────────
            int j = 5;
            Console.WriteLine($"--j: {--j}"); // Output: --j: 4
            j = 5;
            Console.WriteLine($"j--: {j--}"); // Output: j--: 5
            Console.WriteLine($"After j--: {j}"); // Output: After j--: 4

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Operator Precedence
            // ═══════════════════════════════════════════════════════════════
            
            // Order: ++, -- (prefix) → * / % → + -
            int result = 2 + 3 * 4;
            Console.WriteLine($"2 + 3 * 4 = {result}"); // Output: 2 + 3 * 4 = 14 (not 20!)
            
            // Use parentheses to override precedence
            result = (2 + 3) * 4;
            Console.WriteLine($"(2 + 3) * 4 = {result}"); // Output: (2 + 3) * 4 = 20
            
            // Complex expression
            result = 2 + 3 * 4 / 2 - 1;
            Console.WriteLine($"2 + 3 * 4 / 2 - 1 = {result}"); // Output: 2 + 3 * 4 / 2 - 1 = 7

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Real-World Examples
            // ═══════════════════════════════════════════════════════════════
            
            // ── Calculating circle area ────────────────────────────────────
            double radius = 5.0;
            double area = Math.PI * radius * radius;
            Console.WriteLine($"Circle area (r=5): {area:F2}"); // Output: Circle area (r=5): 78.54
            
            // ── Percentage calculation ─────────────────────────────────────
            int total = 250;
            int completed = 75;
            double percentage = (double)completed / total * 100;
            Console.WriteLine($"Progress: {percentage:F1}%"); // Output: Progress: 30.0%
            
            // ── Average calculation ────────────────────────────────────────
            int[] scores = { 85, 90, 78, 92, 88 };
            int totalScore = 0;
            foreach (int score in scores)
            {
                totalScore += score;
            }
            double average = (double)totalScore / scores.Length;
            Console.WriteLine($"Average score: {average:F1}"); // Output: Average score: 86.6
        }
    }
}
