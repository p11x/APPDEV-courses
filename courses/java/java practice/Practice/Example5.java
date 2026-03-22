/*
 * SUB TOPIC: While Loop
 * 
 * DEFINITION:
 * The while loop in Java is a control flow statement that allows code to be executed repeatedly
 * based on a given boolean condition. The loop checks the condition before executing the loop body.
 * 
 * FUNCTIONALITIES:
 * 1. Executes code as long as condition is true
 * 2. Pre-checks condition before iteration
 * 3. Can be used for indefinite loops
 * 4. Requires careful handling to avoid infinite loops
 */

import java.util.Scanner;

public class Example5 {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        
        // Topic Explanation with Code: While Loop - Sum of Numbers
        System.out.println("=== While Loop: Sum of Numbers ===");
        System.out.println("Enter the number: ");
        int n = scanner.nextInt(); // Read the limit number
        
        int i = 1; // Initialize counter
        int sum = 0; // Initialize sum to 0
        
        while (i <= n) { // While i is less than or equal to n
            sum += i; // Add i to sum
            i++; // Increment counter
        }
        System.out.println("The sum of numbers from 1 to " + n + " is: " + sum);
        
        // Real-time Example 1: Calculate factorial using while loop
        System.out.println("\n=== Factorial Calculator ===");
        System.out.print("Enter a number to find factorial: ");
        int num = scanner.nextInt(); // Read number for factorial
        int fact = 1; // Initialize factorial as 1
        int counter = 1; // Initialize counter
        
        while (counter <= num) { // Loop until counter reaches num
            fact *= counter; // Multiply fact by counter
            counter++; // Increment counter
        }
        System.out.println("Factorial of " + num + " is: " + fact);
        
        // Real-time Example 2: Count digits in a number
        System.out.println("\n=== Count Digits ===");
        System.out.print("Enter a number: ");
        int number = scanner.nextInt(); // Read number
        int count = 0; // Initialize digit count
        int temp = number; // Store original number
        
        while (temp > 0) { // While temp is greater than 0
            temp /= 10; // Remove last digit
            count++; // Increment count
        }
        System.out.println("Number of digits in " + number + " is: " + count);
        
        // Real-time Example 3: Reverse a number
        System.out.println("\n=== Reverse Number ===");
        System.out.print("Enter a number to reverse: ");
        int revNum = scanner.nextInt(); // Read number to reverse
        int reversed = 0; // Initialize reversed number
        
        while (revNum > 0) { // While number is greater than 0
            int digit = revNum % 10; // Get last digit
            reversed = reversed * 10 + digit; // Build reversed number
            revNum /= 10; // Remove last digit
        }
        System.out.println("Reversed number is: " + reversed);
        
        // Real-time Example 4: Print multiplication table
        System.out.println("\n=== Multiplication Table ===");
        System.out.print("Enter a number: ");
        int tableNum = scanner.nextInt(); // Read number for table
        int mul = 1; // Initialize multiplier
        
        while (mul <= 10) { // While multiplier is 1 to 10
            System.out.println(tableNum + " x " + mul + " = " + (tableNum * mul));
            mul++; // Increment multiplier
        }
        
        // Real-time Example 5: Find Fibonacci series
        System.out.println("\n=== Fibonacci Series ===");
        System.out.print("Enter number of terms: ");
        int fibTerms = scanner.nextInt(); // Read number of terms
        int fib1 = 0; // First Fibonacci number
        int fib2 = 1; // Second Fibonacci number
        int fib3; // Third Fibonacci number
        int fibCount = 2; // Counter starting from 2
        
        System.out.print(fib1 + " " + fib2 + " "); // Print first two terms
        
        while (fibCount < fibTerms) { // While count is less than terms
            fib3 = fib1 + fib2; // Calculate next term
            System.out.print(fib3 + " "); // Print term
            fib1 = fib2; // Update first
            fib2 = fib3; // Update second
            fibCount++; // Increment counter
        }
        
        scanner.close();
    }
}
