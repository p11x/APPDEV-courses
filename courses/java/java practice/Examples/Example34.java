// Example34: Exception Handling - Beginner Tutorial
// This explains how to handle errors in Java

/*
 * WHAT IS AN EXCEPTION?
 * -------------------
 * An exception is an event that disrupts the normal flow of the program.
 * It's an error that occurs at runtime.
 * 
 * Types of Exceptions:
 * 1. Checked Exceptions   - Compile-time (IOException, SQLException)
 * 2. Unchecked Exceptions - Runtime (NullPointerException, ArrayIndexOutOfBounds)
 * 3. Errors              - JVM errors (OutOfMemoryError)
 * 
 * Exception Hierarchy:
 * Throwable
 *   - Error
 *   - Exception
 *       - RuntimeException (unchecked)
 *       - Other Exceptions (checked)
 */

public class Example34 {
    
    // Method that throws exception
    public static void divideByZero() {
        int result = 10 / 0;  // ArithmeticException!
    }
    
    public static void arrayIndexOutOfBounds() {
        int[] arr = {1, 2, 3};
        int x = arr[10];  // ArrayIndexOutOfBoundsException!
    }
    
    public static void nullPointer() {
        String str = null;
        System.out.println(str.length());  // NullPointerException!
    }
    
    public static void main(String[] args) {
        
        // ===== TRY-CATCH BASICS =====
        System.out.println("=== Try-Catch Basics ===\n");
        
        try {
            int result = 10 / 0;  // This will throw an exception
            System.out.println("Result: " + result);  // This won't execute
        } catch (ArithmeticException e) {
            System.out.println("Caught exception: " + e.getMessage());
            System.out.println("Division by zero is not allowed!");
        }
        
        System.out.println("Program continues after handling exception!");
        
        // ===== MULTIPLE CATCH BLOCKS =====
        System.out.println("\n=== Multiple Catch Blocks ===\n");
        
        int[] numbers = {1, 2, 3};
        
        try {
            // This will cause ArrayIndexOutOfBoundsException
            System.out.println("Value: " + numbers[10]);
        } catch (ArithmeticException e) {
            System.out.println("Arithmetic exception: " + e.getMessage());
        } catch (ArrayIndexOutOfBoundsException e) {
            System.out.println("Array index exception: " + e.getMessage());
        } catch (Exception e) {
            System.out.println("Any other exception: " + e.getMessage());
        }
        
        // ===== TRY-CATCH-FINALLY =====
        System.out.println("\n=== Try-Catch-Finally ===\n");
        
        try {
            int result = 10 / 2;
            System.out.println("Division result: " + result);
        } catch (ArithmeticException e) {
            System.out.println("Exception: " + e.getMessage());
        } finally {
            // This ALWAYS executes
            System.out.println("This always executes - finally block!");
        }
        
        // Another example
        System.out.println("\n--- Another finally example ---");
        
        try {
            System.out.println("Opening file...");
            int data = 100 / 0;
            System.out.println("Reading data...");  // Won't execute
        } catch (Exception e) {
            System.out.println("Error occurred: " + e.getMessage());
        } finally {
            System.out.println("Closing file (always runs)!");
        }
        
        // ===== THROW KEYWORD =====
        System.out.println("\n=== Throw Keyword ===\n");
        
        int age = -5;
        
        try {
            if (age < 0) {
                throw new IllegalArgumentException("Age cannot be negative!");
            }
            System.out.println("Age is valid: " + age);
        } catch (IllegalArgumentException e) {
            System.out.println("Caught: " + e.getMessage());
        }
        
        // ===== THROWS KEYWORD =====
        System.out.println("\n=== Throws Keyword ===\n");
        
        System.out.println("The 'throws' keyword declares that a method may throw an exception");
        System.out.println("It defers the handling to the calling method");
        
        // Example without throws (handled internally)
        try {
            System.out.println(10 / 0);
        } catch (ArithmeticException e) {
            System.out.println("Handled internally");
        }
        
        // ===== CUSTOM EXCEPTION =====
        System.out.println("\n=== Custom Exception ===\n");
        
        // Using custom exception
        try {
            validateAge(-5);
        } catch (InvalidAgeException e) {
            System.out.println("Custom exception caught: " + e.getMessage());
        }
        
        // ===== COMMON EXCEPTIONS =====
        System.out.println("\n=== Common Exceptions ===\n");
        
        // NullPointerException
        try {
            String s = null;
            s.length();
        } catch (NullPointerException e) {
            System.out.println("NullPointerException: " + e.getClass().getSimpleName());
        }
        
        // ArrayIndexOutOfBoundsException
        try {
            int[] arr = new int[5];
            arr[10] = 100;
        } catch (ArrayIndexOutOfBoundsException e) {
            System.out.println("ArrayIndexOutOfBoundsException: " + e.getClass().getSimpleName());
        }
        
        // NumberFormatException
        try {
            int num = Integer.parseInt("abc");
        } catch (NumberFormatException e) {
            System.out.println("NumberFormatException: " + e.getClass().getSimpleName());
        }
        
        // StringIndexOutOfBoundsException
        try {
            String str = "Hello";
            char c = str.charAt(10);
        } catch (StringIndexOutOfBoundsException e) {
            System.out.println("StringIndexOutOfBoundsException: " + e.getClass().getSimpleName());
        }
        
        // ===== NESTED TRY-CATCH =====
        System.out.println("\n=== Nested Try-Catch ===\n");
        
        try {
            try {
                int[] arr = {1, 2, 3};
                System.out.println(arr[5]);  // Will cause exception
            } catch (ArrayIndexOutOfBoundsException e) {
                System.out.println("Inner catch: Array problem - " + e.getMessage());
                
                // Throw another exception
                throw new RuntimeException("New exception from inner catch");
            }
        } catch (RuntimeException e) {
            System.out.println("Outer catch: " + e.getMessage());
        }
        
        // ===== BEST PRACTICES =====
        System.out.println("\n=== Best Practices ===\n");
        
        System.out.println("1. Always handle exceptions gracefully");
        System.out.println("2. Use specific exceptions, not Exception class");
        System.out.println("3. Don't catch exceptions just to print them");
        System.out.println("4. Close resources in finally block or use try-with-resources");
        System.out.println("5. Validate input to prevent exceptions");
        System.out.println("6. Document what exceptions your method can throw");
    }
    
    // Method that throws custom exception
    static void validateAge(int age) throws InvalidAgeException {
        if (age < 0) {
            throw new InvalidAgeException("Age cannot be negative: " + age);
        }
        System.out.println("Age is valid: " + age);
    }
}

// Custom Exception Class
class InvalidAgeException extends Exception {
    public InvalidAgeException(String message) {
        super(message);
    }
}

/*
 * KEY CONCEPTS FOR BEGINNERS:
 * 
 * 1. TRY-CATCH:
 *    - try: Code that might throw exception
 *    - catch: Handle the exception
 *    - finally: Always executes
 * 
 * 2. EXCEPTION TYPES:
 *    - Checked: Compile-time (must handle or declare)
 *    - Unchecked: Runtime (ArithmeticException, NullPointerException)
 * 
 * 3. THROW vs THROWS:
 *    - throw: Throw an exception explicitly
 *    - throws: Declare that method may throw exception
 * 
 * 4. EXCEPTION HIERARCHY:
 *    Throwable
 *      - Error (don't catch)
 *      - Exception
 *          - RuntimeException (unchecked)
 *          - Other exceptions (checked)
 * 
 * 5. GETTING EXCEPTION INFO:
 *    - getMessage(): Brief description
 *    - getCause(): Root cause
 *    - printStackTrace(): Full trace
 */
