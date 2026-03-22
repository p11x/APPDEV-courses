/*
 * SUB TOPIC: For Loop
 * 
 * DEFINITION:
 * The for loop in Java is a control flow statement that allows code to be executed repeatedly
 * based on initialization, condition, and increment/decrement. It is used when the number of
 * iterations is known beforehand.
 * 
 * FUNCTIONALITIES:
 * 1. Executes code for a fixed number of times
 * 2. Contains initialization, condition, and increment in one line
 * 3. Used for iterating arrays and collections
 * 4. More compact than while loop for known iterations
 */

public class Example7 {
    public static void main(String[] args) {
        
        // Topic Explanation with Code: For Loop - Natural Numbers
        System.out.println("=== For Loop: Natural Numbers ===");
        int n = 10; // Define limit
        int sum = 0; // Initialize sum
        
        System.out.print("The first " + n + " natural numbers are: ");
        
        for (int i = 1; i <= n; i++) { // Initialize i=1, condition i<=n, increment i++
            System.out.print(i + " "); // Print each number
            sum += i; // Add to sum
        }
        
        System.out.println("\nThe sum of the first " + n + " natural numbers is: " + sum);
        
        // Real-time Example 1: Print even numbers
        System.out.println("\n=== Even Numbers ===");
        System.out.print("Even numbers from 1 to 20: ");
        
        for (int i = 2; i <= 20; i += 2) { // Start from 2, increment by 2
            System.out.print(i + " "); // Print even number
        }
        
        // Real-time Example 2: Print odd numbers
        System.out.println("\n\n=== Odd Numbers ===");
        System.out.print("Odd numbers from 1 to 20: ");
        
        for (int i = 1; i <= 20; i += 2) { // Start from 1, increment by 2
            System.out.print(i + " "); // Print odd number
        }
        
        // Real-time Example 3: Calculate factorial
        System.out.println("\n\n=== Factorial ===");
        int number = 5; // Number to find factorial
        int factorial = 1; // Initialize factorial
        
        for (int i = 1; i <= number; i++) { // Loop from 1 to number
            factorial *= i; // Multiply each i
        }
        
        System.out.println("Factorial of " + number + " is: " + factorial);
        
        // Real-time Example 4: Print multiplication table
        System.out.println("\n=== Multiplication Table ===");
        int tableNum = 7; // Number for table
        
        for (int i = 1; i <= 10; i++) { // Loop from 1 to 10
            System.out.println(tableNum + " x " + i + " = " + (tableNum * i));
        }
        
        // Real-time Example 5: Calculate power
        System.out.println("\n=== Calculate Power ===");
        int base = 2; // Base number
        int exponent = 5; // Exponent
        int result = 1; // Result initialized
        
        for (int i = 1; i <= exponent; i++) { // Multiply base exponent times
            result *= base; // result = result * base
        }
        
        System.out.println(base + " raised to power " + exponent + " is: " + result);
        
        // Real-time Example 6: Sum of array elements
        System.out.println("\n=== Sum of Array Elements ===");
        int[] arr = {10, 20, 30, 40, 50}; // Sample array
        int arraySum = 0; // Initialize sum
        
        for (int i = 0; i < arr.length; i++) { // Loop through array indices
            arraySum += arr[i]; // Add each element
        }
        
        System.out.println("Array elements: ");
        for (int i = 0; i < arr.length; i++) {
            System.out.print(arr[i] + " ");
        }
        System.out.println("\nSum: " + arraySum);
    }
}
