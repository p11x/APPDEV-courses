/*
 * SUB TOPIC: Java Bitwise Operations
 * 
 * DEFINITION:
 * Bitwise operators work on individual bits of numbers. They include AND (&), OR (|), XOR (^), NOT (~),
 * left shift (<<), and right shift (>>).
 * 
 * FUNCTIONALITIES:
 * 1. Bitwise AND (&)
 * 2. Bitwise OR (|)
 * 3. Bitwise XOR (^)
 * 4. Bitwise NOT (~)
 * 5. Left shift (<<)
 * 6. Right shift (>>)
 */

public class Example64 {
    public static void main(String[] args) {
        
        // Bitwise AND
        System.out.println("=== Bitwise AND ===");
        int a = 5;  // 0101
        int b = 3;  // 0011
        System.out.println("5 & 3 = " + (a & b)); // 0001 = 1
        
        // Bitwise OR
        System.out.println("\n=== Bitwise OR ===");
        System.out.println("5 | 3 = " + (a | b)); // 0111 = 7
        
        // Bitwise XOR
        System.out.println("\n=== Bitwise XOR ===");
        System.out.println("5 ^ 3 = " + (a ^ b)); // 0110 = 6
        
        // Bitwise NOT
        System.out.println("\n=== Bitwise NOT ===");
        System.out.println("~5 = " + (~a)); // -6
        
        // Left shift
        System.out.println("\n=== Left Shift ===");
        System.out.println("5 << 1 = " + (a << 1)); // 10
        System.out.println("5 << 2 = " + (a << 2)); // 20
        
        // Right shift
        System.out.println("\n=== Right Shift ===");
        System.out.println("10 >> 1 = " + (10 >> 1)); // 5
        System.out.println("10 >> 2 = " + (10 >> 2)); // 2
        
        // Real-time Example 1: Check even/odd
        System.out.println("\n=== Example 1: Even/Odd ===");
        
        int num = 7;
        if ((num & 1) == 0) {
            System.out.println(num + " is even");
        } else {
            System.out.println(num + " is odd");
        }
        
        // Real-time Example 2: Power of 2
        System.out.println("\n=== Example 2: Power of 2 ===");
        
        int n = 16;
        if ((n & (n - 1)) == 0) {
            System.out.println(n + " is power of 2");
        } else {
            System.out.println(n + " is not power of 2");
        }
        
        // Real-time Example 3: Swap without temp
        System.out.println("\n=== Example 3: Swap ===");
        
        int x = 5, y = 10;
        System.out.println("Before: x=" + x + ", y=" + y);
        x = x ^ y;
        y = x ^ y;
        x = x ^ y;
        System.out.println("After: x=" + x + ", y=" + y);
        
        // Real-time Example 4: Mask bits
        System.out.println("\n=== Example 4: Mask ===");
        
        int flags = 0b1010; // 10
        int mask = 0b0010; // 2
        if ((flags & mask) != 0) {
            System.out.println("Bit is set");
        } else {
            System.out.println("Bit is not set");
        }
        
        // Real-time Example 5: Division by 2
        System.out.println("\n=== Example 5: Fast Division ===");
        
        int value = 100;
        System.out.println(value + " / 2 = " + (value >> 1));
        System.out.println(value + " / 4 = " + (value >> 2));
        
        // Real-time Example 6: Multiply by 2
        System.out.println("\n=== Example 6: Fast Multiply ===");
        
        int val = 7;
        System.out.println(val + " * 2 = " + (val << 1));
        System.out.println(val + " * 4 = " + (val << 2));
    }
}
