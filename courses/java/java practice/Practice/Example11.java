/*
 * SUB TOPIC: Control Statements (break, continue)
 * 
 * DEFINITION:
 * Control statements like break and continue are used to control the flow of loops. Break terminates
 * the loop entirely, while continue skips the current iteration and moves to the next one.
 * These are essential for implementing complex loop logic.
 * 
 * FUNCTIONALITIES:
 * 1. break - exits the loop immediately
 * 2. continue - skips current iteration
 * 3. Labeled break/continue - controls nested loops
 * 4. Loop control in real applications
 */

import java.util.Scanner;

public class Example11 {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        
        // Topic Explanation with Code: While loop with continue
        System.out.println("=== While Loop with Continue ===");
        System.out.println("Enter how many positive numbers you want to add: ");
        int n = scanner.nextInt(); // Read count
        int sum = 0; // Initialize sum
        int i = 1; // Initialize counter
        
        while (i <= n) { // Loop while i <= n
            System.out.println("Enter a positive number: ");
            int num = scanner.nextInt(); // Read number
            
            if (num < 0) { // Check if negative
                System.out.println("Negative number not allowed! Try again.");
                continue; // Skip rest of loop, continue to next iteration
            }
            
            sum = sum + num; // Add to sum
            i++; // Increment counter
        }
        
        System.out.println("Sum of all positive numbers: " + sum);
        
        // Real-time Example 1: Break statement - Find first prime
        System.out.println("\n=== Find First Prime Number ===");
        System.out.print("Enter start number: ");
        int start = scanner.nextInt(); // Read start
        System.out.print("Enter end number: ");
        int end = scanner.nextInt(); // Read end
        
        for (int num = start; num <= end; num++) { // Loop through range
            boolean isPrime = true; // Assume prime
            
            for (int j = 2; j <= num / 2; j++) { // Check divisibility
                if (num % j == 0) { // If divisible
                    isPrime = false; // Not prime
                    break; // Exit inner loop
                }
            }
            
            if (isPrime) { // If prime found
                System.out.println("First prime found: " + num);
                break; // Exit outer loop
            }
        }
        
        // Real-time Example 2: Continue - Skip even numbers
        System.out.println("\n=== Skip Even Numbers ===");
        
        for (int j = 1; j <= 10; j++) { // Loop 1 to 10
            if (j % 2 == 0) { // If even
                continue; // Skip this iteration
            }
            System.out.println("Odd number: " + j); // Print only odd
        }
        
        // Real-time Example 3: Break - Exit on specific condition
        System.out.println("\n=== Find Number ===");
        int[] arr = {5, 10, 15, 20, 25, 30, 35, 40}; // Sample array
        System.out.print("Array: ");
        for (int num : arr) {
            System.out.print(num + " ");
        }
        System.out.println();
        
        System.out.print("Enter number to find: ");
        int search = scanner.nextInt(); // Number to search
        
        for (int k = 0; k < arr.length; k++) { // Loop through array
            if (arr[k] == search) { // If found
                System.out.println("Number " + search + " found at index " + k);
                break; // Exit loop
            }
        }
        
        // Real-time Example 4: Continue - Filter specific values
        System.out.println("\n=== Filter Numbers ===");
        int[] numbers = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10}; // Sample array
        
        System.out.print("Numbers divisible by 3: ");
        for (int num : numbers) { // Loop through array
            if (num % 3 != 0) { // If not divisible by 3
                continue; // Skip
            }
            System.out.print(num + " "); // Print divisible numbers
        }
        
        // Real-time Example 5: Break in nested loops
        System.out.println("\n\n=== Break in Nested Loops ===");
        
        outerLoop:
        for (int p = 1; p <= 3; p++) { // Outer loop
            for (int q = 1; q <= 3; q++) { // Inner loop
                if (p == 2 && q == 2) { // Specific condition
                    System.out.println("Breaking at p=" + p + ", q=" + q);
                    break outerLoop; // Break outer loop
                }
                System.out.println("p=" + p + ", q=" + q);
            }
        }
        
        // Real-time Example 6: Continue in nested loops
        System.out.println("\n=== Continue in Nested Loops ===");
        
        for (int x = 1; x <= 3; x++) { // Outer loop
            for (int y = 1; y <= 3; y++) { // Inner loop
                if (y == 2) { // Skip when y is 2
                    continue; // Continue to next iteration of inner loop
                }
                System.out.println("x=" + x + ", y=" + y);
            }
        }
        
        scanner.close();
    }
}
