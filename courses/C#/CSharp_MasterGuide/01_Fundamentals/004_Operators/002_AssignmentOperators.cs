/*
 * ============================================================
 * TOPIC     : C# Fundamentals
 * SUBTOPIC  : Operators - Assignment Operators
 * FILE      : AssignmentOperators.cs
 * PURPOSE   : This file covers assignment operators in C#: =, +=, -=, *=, /=, %=, &=, |=, ^=, <<=, >>=.
 *             These operators assign values to variables with various operations.
 * ============================================================
 */

// --- SECTION: Assignment Operators ---
// Assignment operators assign values to variables
// Compound operators combine assignment with arithmetic/bitwise operations

using System;

namespace CSharp_MasterGuide._01_Fundamentals._04_Operators
{
    class AssignmentOperators
    {
        static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════════
            // SECTION: Simple Assignment (=)
            // ═══════════════════════════════════════════════════════════════
            
            // Basic assignment
            int x = 10;
            Console.WriteLine($"x = {x}"); // Output: x = 10
            
            // Assign from expression
            int y = x + 5;
            Console.WriteLine($"y = x + 5: {y}"); // Output: y = x + 5: 15
            
            // Chained assignment (right to left)
            int a, b, c;
            a = b = c = 5; // All become 5
            Console.WriteLine($"a={a}, b={b}, c={c}"); // Output: a=5, b=5, c=5
            
            // Assign to different types with casting
            double pi = 3.14159;
            int truncated = (int)pi;
            Console.WriteLine($"int from double: {truncated}"); // Output: int from double: 3

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Compound Assignment Operators
            // ═══════════════════════════════════════════════════════════════
            
            // ── += (Add and assign) ────────────────────────────────────────
            int num = 10;
            num += 5; // Same as: num = num + 5
            Console.WriteLine($"num += 5: {num}"); // Output: num += 5: 15
            
            // String concatenation with +=
            string text = "Hello";
            text += " World";
            Console.WriteLine($"text += \" World\": \"{text}\""); // Output: Hello World
            
            // ── -= (Subtract and assign) ───────────────────────────────────
            num = 10;
            num -= 3; // Same as: num = num - 3
            Console.WriteLine($"num -= 3: {num}"); // Output: num -= 3: 7
            
            // ── *= (Multiply and assign) ──────────────────────────────────
            num = 10;
            num *= 2; // Same as: num = num * 2
            Console.WriteLine($"num *= 2: {num}"); // Output: num *= 2: 20
            
            // ── /= (Divide and assign) ─────────────────────────────────────
            num = 10;
            num /= 4; // Same as: num = num / 4 (integer division)
            Console.WriteLine($"num /= 4: {num}"); // Output: num /= 4: 2
            
            // With floating-point
            double d = 10.0;
            d /= 4;
            Console.WriteLine($"d /= 4: {d}"); // Output: d /= 4: 2.5
            
            // ── %= (Modulus and assign) ────────────────────────────────────
            num = 10;
            num %= 3; // Same as: num = num % 3
            Console.WriteLine($"num %= 3: {num}"); // Output: num %= 3: 1

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Bitwise Compound Assignments
            // ═══════════════════════════════════════════════════════════════
            
            // ── &= (Bitwise AND and assign) ────────────────────────────────
            int flags = 0b1100;
            int mask = 0b1010;
            flags &= mask; // Same as: flags = flags & mask
            Console.WriteLine($"flags &= mask: {flags}"); // Output: flags &= mask: 8
            
            // ── |= (Bitwise OR and assign) ────────────────────────────────
            flags = 0b0100;
            flags |= 0b0010; // Set bit
            Console.WriteLine($"flags |= 0b0010: {flags}"); // Output: flags |= 0b0010: 6
            
            // ── ^= (Bitwise XOR and assign) ───────────────────────────────
            flags = 0b1111;
            flags ^= 0b0101; // Toggle bits
            Console.WriteLine($"flags ^= 0b0101: {flags}"); // Output: flags ^= 0b0101: 10
            
            // ── <<= (Left shift and assign) ────────────────────────────────
            int shift = 1;
            shift <<= 3; // Same as: shift = shift << 3
            Console.WriteLine($"shift <<= 3: {shift}"); // Output: shift <<= 3: 8
            
            // ── >>= (Right shift and assign) ───────────────────────────────
            shift = 16;
            shift >>= 2;
            Console.WriteLine($"shift >>= 2: {shift}"); // Output: shift >>= 2: 4

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Practical Examples
            // ═══════════════════════════════════════════════════════════════
            
            // ── Accumulator pattern ─────────────────────────────────────────
            int sum = 0;
            int[] values = { 10, 20, 30, 40 };
            
            foreach (int v in values)
            {
                sum += v; // Accumulate sum
            }
            Console.WriteLine($"Sum: {sum}"); // Output: Sum: 100
            
            // ── Building string ───────────────────────────────────────────
            var parts = new[] { "A", "B", "C" };
            string result = "";
            
            foreach (var part in parts)
            {
                result += part + ",";
            }
            Console.WriteLine($"Joined: {result.TrimEnd(',')}"); // Output: Joined: A,B,C
            
            // ── Toggle flag ───────────────────────────────────────────────
            bool flag = false;
            flag = !flag; // Toggle
            
            // With bitwise
            int options = 0;
            options |= 0x01; // Enable option 1
            options |= 0x02; // Enable option 2
            // Toggle option 1
            options ^= 0x01;
            Console.WriteLine($"Options: {options}"); // Output: Options: 2

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Multiple Assignment with Tuple
            // ═══════════════════════════════════════════════════════════════
            
            // C# 7.0+ tuple assignment
            int m = 10, n = 20;
            (m, n) = (n, m); // Swap using tuple
            Console.WriteLine($"After swap: m={m}, n={n}"); // Output: m=20, n=10
            
            // Multiple values from method
            (int max, int min) = GetRange(new[] { 1, 5, 3, 9, 2 });
            Console.WriteLine($"Max: {max}, Min: {min}"); // Output: Max: 9, Min: 1

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Ref Assignment (C# 7.0+)
            // ═══════════════════════════════════════════════════════════════
            
            int[] arr = { 1, 2, 3 };
            ref int refToFirst = ref arr[0]; // Get reference to element
            refToFirst = 100; // Modify through reference
            Console.WriteLine($"arr[0] after ref modify: {arr[0]}"); // Output: arr[0] after ref modify: 100
            
            // Swap using ref
            ref int swapRef = ref GetElement(arr, 0);
            swapRef = 1; // Reset for demonstration
            Console.WriteLine($"arr[0] reset: {arr[0]}"); // Output: arr[0] reset: 1

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Conditional Assignment (ternary operator)
            // ═══════════════════════════════════════════════════════════════
            
            // Not technically assignment operator, but related
            int age = 25;
            string category = age >= 18 ? "Adult" : "Minor";
            Console.WriteLine($"Category: {category}"); // Output: Category: Adult
            
            // Null-coalescing assignment (C# 8.0+)
            string? value = null;
            value ??= "default"; // Assign if null
            Console.WriteLine($"Value: {value}"); // Output: Value: default
            
            value = "provided";
            value ??= "default"; // No change
            Console.WriteLine($"Value after: {value}"); // Output: Value after: provided
        }
        
        // Helper method for range calculation
        static (int max, int min) GetRange(int[] numbers)
        {
            int max = numbers[0];
            int min = numbers[0];
            
            foreach (int n in numbers)
            {
                if (n > max) max = n;
                if (n < min) min = n;
            }
            
            return (max, min);
        }
        
        // Helper method to get ref to array element
        static ref int GetElement(int[] array, int index)
        {
            return ref array[index];
        }
    }
}
