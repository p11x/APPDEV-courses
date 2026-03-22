/*
 * SUB TOPIC: Exception Handling in Java
 * 
 * DEFINITION:
 * Exception handling is a mechanism to handle runtime errors gracefully. An exception is an 
 * event that disrupts the normal flow of program execution. Java provides try-catch blocks 
 * to catch and handle exceptions, preventing the program from crashing.
 * 
 * FUNCTIONALITIES:
 * 1. try-catch - Catch and handle exceptions
 * 2. Multiple catch blocks - Handle different exception types
 * 3. finally block - Code that always executes
 * 4. throw - Manually throw an exception
 * 5. throws - Declare exceptions in method signature
 * 6. Custom exceptions - User-defined exception classes
 */

public class Example34 {
    public static void main(String[] args) {
        
        // Topic Explanation: Basic Exception Handling
        
        // try-catch: Wrap risky code in try, handle in catch
        System.out.println("=== Basic Try-Catch ===");
        try {
            int result = 10 / 2; // This works fine
            System.out.println("Result: " + result);
        } catch (ArithmeticException e) {
            System.out.println("Error: " + e.getMessage());
        }
        
        // Try-catch with exception
        try {
            int result = 10 / 0; // Division by zero throws exception
            System.out.println("Result: " + result); // This won't execute
        } catch (ArithmeticException e) {
            System.out.println("Caught ArithmeticException: Division by zero not allowed");
        }
        
        // Multiple Catch Blocks: Handle different exception types
        System.out.println("\n=== Multiple Catch Blocks ===");
        int[] numbers = {1, 2, 3};
        
        try {
            System.out.println(numbers[10]); // ArrayIndexOutOfBoundsException
        } catch (ArithmeticException e) {
            System.out.println("Arithmetic error: " + e.getMessage());
        } catch (ArrayIndexOutOfBoundsException e) {
            System.out.println("Array error: Index out of bounds");
        } catch (Exception e) {
            System.out.println("General error: " + e.getMessage());
        }
        
        // Finally Block: Always executes regardless of exception
        System.out.println("\n=== Finally Block ===");
        try {
            int result = 10 / 2;
            System.out.println("Calculation successful: " + result);
        } catch (Exception e) {
            System.out.println("Error occurred: " + e.getMessage());
        } finally {
            System.out.println("Finally block: This always executes!");
        }
        
        // throw keyword: Manually throw an exception
        System.out.println("\n=== Throw Keyword ===");
        int age = -5;
        try {
            if (age < 0) {
                throw new IllegalArgumentException("Age cannot be negative!");
            }
            System.out.println("Age is valid: " + age);
        } catch (IllegalArgumentException e) {
            System.out.println("Caught: " + e.getMessage());
        }
        
        // Real-time Example 1: Age validation for registration
        System.out.println("\n=== Example 1: User Registration Validation ===");
        int userAge = 17;
        try {
            if (userAge < 18) {
                throw new Exception("User must be 18 or older");
            }
            System.out.println("Registration successful for age: " + userAge);
        } catch (Exception e) {
            System.out.println("Registration failed: " + e.getMessage());
        }
        
        // Real-time Example 2: Bank withdrawal with insufficient balance
        System.out.println("\n=== Example 2: Bank Withdrawal ===");
        double balance = 100.0;
        double withdrawAmount = 150.0;
        
        try {
            if (withdrawAmount > balance) {
                throw new Exception("Insufficient funds. Balance: " + balance);
            }
            balance = balance - withdrawAmount;
            System.out.println("Withdrawal successful. New balance: " + balance);
        } catch (Exception e) {
            System.out.println("Transaction failed: " + e.getMessage());
        } finally {
            System.out.println("Transaction completed");
        }
        
        // Real-time Example 3: File processing simulation
        System.out.println("\n=== Example 3: File Processing ===");
        String filename = "data.txt";
        
        try {
            if (!filename.endsWith(".txt")) {
                throw new Exception("Invalid file format. Only .txt allowed");
            }
            System.out.println("Processing file: " + filename);
        } catch (Exception e) {
            System.out.println("Error: " + e.getMessage());
        } finally {
            System.out.println("File handler closed");
        }
        
        // Real-time Example 4: Array element access with bounds checking
        System.out.println("\n=== Example 4: Safe Array Access ===");
        int[] arr = {10, 20, 30, 40, 50};
        int index = 5;
        
        try {
            if (index < 0 || index >= arr.length) {
                throw new ArrayIndexOutOfBoundsException("Index must be between 0 and " + (arr.length - 1));
            }
            System.out.println("Element at index " + index + ": " + arr[index]);
        } catch (ArrayIndexOutOfBoundsException e) {
            System.out.println("Error: " + e.getMessage());
        }
        
        // Real-time Example 5: Null pointer handling
        System.out.println("\n=== Example 5: Null Check ===");
        String userName = null;
        
        try {
            if (userName == null) {
                throw new NullPointerException("Username cannot be null");
            }
            System.out.println("Welcome, " + userName);
        } catch (NullPointerException e) {
            System.out.println("Error: " + e.getMessage());
        }
        
        // Real-time Example 6: Number format validation
        System.out.println("\n=== Example 6: Number Parsing ===");
        String input = "12345";
        
        try {
            int parsed = Integer.parseInt(input);
            System.out.println("Parsed number: " + parsed);
        } catch (NumberFormatException e) {
            System.out.println("Error: Invalid number format");
        }
        
        // Custom Exception Usage
        System.out.println("\n=== Custom Exception ===");
        try {
            validateScore(95);
            System.out.println("Score is valid");
        } catch (InvalidScoreException e) {
            System.out.println("Score error: " + e.getMessage());
        }
    }
    
    // Method that throws custom exception
    static void validateScore(int score) throws InvalidScoreException {
        if (score < 0 || score > 100) {
            throw new InvalidScoreException("Score must be between 0 and 100");
        }
    }
}

// Custom Exception Class
class InvalidScoreException extends Exception {
    public InvalidScoreException(String message) {
        super(message);
    }
}
