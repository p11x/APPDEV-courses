/*
 * SUB TOPIC: Java Operators - Complete Reference
 * 
 * DEFINITION:
 * Java operators are symbols that perform operations on operands. Categories include arithmetic, relational,
 * logical, bitwise, assignment, and ternary operators.
 * 
 * FUNCTIONALITIES:
 * 1. Arithmetic (+, -, *, /, %)
 * 2. Relational (==, !=, >, <, >=, <=)
 * 3. Logical (&&, ||, !)
 * 4. Assignment (=, +=, -=, etc.)
 * 5. Ternary (?:)
 * 6. instanceof
 */

public class Example65 {
    public static void main(String[] args) {
        
        // Arithmetic
        System.out.println("=== Arithmetic ===");
        System.out.println("10 + 5 = " + (10 + 5));
        System.out.println("10 - 5 = " + (10 - 5));
        System.out.println("10 * 5 = " + (10 * 5));
        System.out.println("10 / 5 = " + (10 / 5));
        System.out.println("10 % 3 = " + (10 % 3));
        
        // Relational
        System.out.println("\n=== Relational ===");
        System.out.println("5 == 5: " + (5 == 5));
        System.out.println("5 != 3: " + (5 != 3));
        System.out.println("5 > 3: " + (5 > 3));
        System.out.println("5 < 3: " + (5 < 3));
        
        // Logical
        System.out.println("\n=== Logical ===");
        boolean a = true, b = false;
        System.out.println("true && false: " + (a && b));
        System.out.println("true || false: " + (a || b));
        System.out.println("!true: " + (!a));
        
        // Assignment
        System.out.println("\n=== Assignment ===");
        int x = 10;
        x += 5;
        System.out.println("x += 5: " + x);
        
        x = 10;
        x -= 5;
        System.out.println("x -= 5: " + x);
        
        // Ternary
        System.out.println("\n=== Ternary ===");
        int age = 20;
        String result = age >= 18 ? "Adult" : "Minor";
        System.out.println(result);
        
        // Real-time Example 1: Grade
        System.out.println("\n=== Example 1: Grade ===");
        
        int score = 85;
        char grade = score >= 90 ? 'A' :
                     score >= 80 ? 'B' :
                     score >= 70 ? 'C' : 'F';
        System.out.println("Score: " + score + ", Grade: " + grade);
        
        // Real-time Example 2: Max
        System.out.println("\n=== Example 2: Max ===");
        
        int m1 = 10, m2 = 20;
        int max = m1 > m2 ? m1 : m2;
        System.out.println("Max: " + max);
        
        // Real-time Example 3: Check range
        System.out.println("\n=== Example 3: Range ===");
        
        int num = 15;
        boolean inRange = num >= 1 && num <= 10;
        System.out.println(num + " in 1-10: " + inRange);
        
        // Real-time Example 4: instanceof
        System.out.println("\n=== Example 4: instanceof ===");
        
        Object obj = "Hello";
        System.out.println("Is String: " + (obj instanceof String));
        
        // Real-time Example 5: Compound interest
        System.out.println("\n=== Example 5: Compound ===");
        
        double p = 1000, r = 5, t = 2;
        double amount = p * Math.pow(1 + r/100, t);
        System.out.println("Amount: $" + amount);
        
        // Real-time Example 6: Leap year
        System.out.println("\n=== Example 6: Leap Year ===");
        
        int year = 2024;
        boolean leap = (year % 4 == 0 && year % 100 != 0) || (year % 400 == 0);
        System.out.println(year + " is leap year: " + leap);
    }
}
