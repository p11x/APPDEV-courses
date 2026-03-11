// ExceptionHandlingDemo - Demonstrates Java Exception Handling
// Essential for building robust Java backends that Angular can communicate with

import java.util.Scanner;

public class ExceptionHandlingDemo {
    
    // ===== BASIC TRY-CATCH =====
    public static void basicTryCatch() {
        System.out.println("=== BASIC TRY-CATCH ===");
        
        try {
            int[] numbers = {1, 2, 3};
            System.out.println("Accessing index 5: " + numbers[5]);  // ArrayIndexOutOfBoundsException
        } catch (ArrayIndexOutOfBoundsException e) {
            System.out.println("Exception caught: " + e.getMessage());
        }
        
        System.out.println("Program continues after exception!\n");
    }
    
    // ===== MULTIPLE CATCH BLOCKS =====
    public static void multipleCatch() {
        System.out.println("=== MULTIPLE CATCH BLOCKS ===");
        
        try {
            int result = 10 / 0;  // ArithmeticException
        } catch (ArithmeticException e) {
            System.out.println("Arithmetic error: " + e.getMessage());
        } catch (Exception e) {
            System.out.println("General exception: " + e.getMessage());
        }
        
        System.out.println();
    }
    
    // ===== TRY-CATCH-FINALLY =====
    public static void tryCatchFinally() {
        System.out.println("=== TRY-CATCH-FINALLY ===");
        
        try {
            String str = null;
            System.out.println("Length: " + str.length());  // NullPointerException
        } catch (NullPointerException e) {
            System.out.println("Null pointer exception caught");
        } finally {
            System.out.println("Finally block - always executes!");
        }
        
        System.out.println();
    }
    
    // ===== THROW AND THROWS =====
    public static void throwAndThrows() throws IllegalArgumentException {
        System.out.println("=== THROW AND THROWS ===");
        
        int age = -5;
        
        if (age < 0) {
            throw new IllegalArgumentException("Age cannot be negative!");
        }
    }
    
    // ===== CUSTOM EXCEPTION =====
    public static void customExceptionDemo() {
        System.out.println("=== CUSTOM EXCEPTION ===");
        
        try {
            validateAge(-25);
        } catch (InvalidAgeException e) {
            System.out.println("Custom Exception: " + e.getMessage());
        }
    }
    
    public static void validateAge(int age) throws InvalidAgeException {
        if (age < 0 || age > 150) {
            throw new InvalidAgeException("Invalid age: " + age);
        }
        System.out.println("Valid age: " + age);
    }
    
    // ===== TRY-WITH-RESOURCES (Java 7+) =====
    public static void tryWithResourcesDemo() {
        System.out.println("\n=== TRY-WITH-RESOURCES ===");
        
        // This automatically closes resources
        try (Scanner scanner = new Scanner(System.in)) {
            System.out.println("Enter a number: ");
            // int num = scanner.nextInt();  // Uncomment to test
            System.out.println("Resource automatically closed!");
        } catch (Exception e) {
            System.out.println("Exception: " + e.getMessage());
        }
    }
    
    // ===== EXCEPTION HIERARCHY =====
    public static void exceptionHierarchy() {
        System.out.println("=== EXCEPTION HIERARCHY ===");
        System.out.println("Throwable");
        System.out.println("  ├── Error (unchecked) - System errors");
        System.out.println("  └── Exception");
        System.out.println("        ├── RuntimeException (unchecked)");
        System.out.println("        │     ├── NullPointerException");
        System.out.println("        │     ├── ArrayIndexOutOfBoundsException");
        System.out.println("        │     └── ArithmeticException");
        System.out.println("        └── Checked Exceptions");
        System.out.println("              ├── IOException");
        System.out.println("              └── SQLException");
    }
    
    // Main method
    public static void main(String[] args) {
        System.out.println("=== EXCEPTION HANDLING DEMO ===\n");
        
        basicTryCatch();
        multipleCatch();
        tryCatchFinally();
        
        try {
            throwAndThrows();
        } catch (IllegalArgumentException e) {
            System.out.println("Caught: " + e.getMessage());
        }
        
        customExceptionDemo();
        exceptionHierarchy();
        
        System.out.println("\n=== BEST PRACTICES ===");
        System.out.println("1. Catch specific exceptions first");
        System.out.println("2. Always use finally for cleanup");
        System.out.println("3. Use try-with-resources for AutoCloseable");
        System.out.println("4. Don't swallow exceptions - at least log them");
        System.out.println("5. Create custom exceptions for business rules");
    }
}

// Custom Exception Class
class InvalidAgeException extends Exception {
    public InvalidAgeException(String message) {
        super(message);
    }
}
