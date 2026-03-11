# Java Exception Handling

## Table of Contents
1. [Introduction to Exceptions](#introduction-to-exceptions)
2. [Exception Types](#exception-types)
3. [Try-Catch-Finally](#try-catch-finally)
4. [Throwing Exceptions](#throwing-exceptions)
5. [Custom Exceptions](#custom-exceptions)
6. [Best Practices](#best-practices)
7. [Code Examples](#code-examples)
8. [Exercises](#exercises)
9. [Solutions](#solutions)

---

## 1. Introduction to Exceptions

### What is an Exception?

An **exception** is an event that disrupts the normal flow of program execution.

```
┌─────────────────────────────────────────────────────────────┐
│                    EXCEPTION HIERARCHY                       │
├─────────────────────────────────────────────────────────────┤
│                                                              │
│                    Throwable                                 │
│                    ├────── Error                            │
│                    │     (JVM errors)                       │
│                    └────── Exception                        │
│                          ├────── RuntimeException           │
│                          │     (Unchecked)                  │
│                          └────── Checked Exceptions         │
│                                (IOException, etc.)          │
│                                                              │
└─────────────────────────────────────────────────────────────┘
```

---

## 2. Exception Types

### Checked vs Unchecked

| Type | Description | Examples |
|------|-------------|----------|
| **Checked** | Compile-time checked | IOException, SQLException |
| **Unchecked** | Runtime exceptions | NullPointerException, ArrayIndexOutOfBoundsException |

### Common Exceptions

```java
// Arithmetic
ArithmeticException: / by zero

// Null Pointer
NullPointerException: accessing null object

// Array
ArrayIndexOutOfBoundsException
ArrayStoreException

// I/O
FileNotFoundException
IOException

// Number
NumberFormatException
```

---

## 3. Try-Catch-Finally

### Basic Syntax

```java
try {
    // Code that might throw exception
} catch (ExceptionType1 e) {
    // Handle ExceptionType1
} catch (ExceptionType2 e) {
    // Handle ExceptionType2
} finally {
    // Always executes (cleanup)
}
```

---

## 4. Throwing Exceptions

### Using throw and throws

```java
public void method() throws IOException {
    // throws declares exceptions
    
    if (somethingWrong) {
        throw new IOException("Error message");  // throw throws the exception
    }
}
```

---

## 5. Custom Exceptions

### Creating Custom Exceptions

```java
// Custom unchecked exception
class InvalidAgeException extends RuntimeException {
    public InvalidAgeException(String message) {
        super(message);
    }
}

// Custom checked exception
class BusinessException extends Exception {
    public BusinessException(String message) {
        super(message);
    }
}
```

---

## 6. Best Practices

1. Catch specific exceptions, not Exception
2. Don't swallow exceptions
3. Use finally for cleanup
4. Throw early, catch late
5. Document exceptions with throws

---

## 7. Code Examples

### Example 1: Basic Exception Handling

```java
import java.util.Scanner;

/**
 * BasicExceptionHandling - Core exception handling patterns
 */
public class BasicExceptionHandling {
    
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        
        System.out.println("=== BASIC EXCEPTION HANDLING ===\n");
        
        // Division with try-catch
        System.out.print("Enter dividend: ");
        int dividend = scanner.nextInt();
        System.out.print("Enter divisor: ");
        int divisor = scanner.nextInt();
        
        try {
            int result = dividend / divisor;
            System.out.println("Result: " + result);
        } catch (ArithmeticException e) {
            System.out.println("Error: Cannot divide by zero!");
        }
        
        // Array access
        System.out.println("\n--- Array Access ---");
        int[] numbers = {1, 2, 3, 4, 5};
        
        try {
            System.out.println("Element at index 10: " + numbers[10]);
        } catch (ArrayIndexOutOfBoundsException e) {
            System.out.println("Error: Index out of bounds!");
        }
        
        // Multiple catch blocks
        System.out.println("\n--- Multiple Catch ---");
        String[] inputs = {"5", "abc", "null"};
        
        for (String input : inputs) {
            try {
                int num = Integer.parseInt(input);
                System.out.println("Parsed: " + num);
            } catch (NumberFormatException e) {
                System.out.println("Cannot parse '" + input + "': " + e.getMessage());
            }
        }
        
        scanner.close();
    }
}
```

---

### Example 2: Try-With-Resources

```java
import java.io.*;

/**
 * TryWithResources - Automatic resource management
 * (Java 7+)
 */
public class TryWithResources {
    
    public static void main(String[] args) {
        System.out.println("=== TRY WITH RESOURCES ===\n");
        
        // Read file with try-with-resources
        readFile("nonexistent.txt");
        
        // Create and read temp file
        createAndReadTempFile();
    }
    
    public static void readFile(String filename) {
        // AutoCloseable resources
        try (FileReader reader = new FileReader(filename)) {
            int ch;
            while ((ch = reader.read()) != -1) {
                System.out.print((char) ch);
            }
        } catch (FileNotFoundException e) {
            System.out.println("File not found: " + filename);
        } catch (IOException e) {
            System.out.println("IO Error: " + e.getMessage());
        }
    }
    
    public static void createAndReadTempFile() {
        File tempFile = new File("temp.txt");
        
        // Write to file
        try (FileWriter writer = new FileWriter(tempFile)) {
            writer.write("Hello, World!\n");
            writer.write("Java Exception Handling");
            System.out.println("Written to file successfully");
        } catch (IOException e) {
            System.out.println("Write error: " + e.getMessage());
        }
        
        // Read from file
        try (BufferedReader reader = new BufferedReader(new FileReader(tempFile))) {
            String line;
            System.out.println("\nFile contents:");
            while ((line = reader.readLine()) != null) {
                System.out.println("  " + line);
            }
        } catch (IOException e) {
            System.out.println("Read error: " + e.getMessage());
        } finally {
            // Cleanup
            if (tempFile.exists()) {
                tempFile.delete();
                System.out.println("\nTemp file deleted");
            }
        }
    }
}
```

---

### Example 3: Custom Exceptions

```java
/**
 * Custom exception for validation
 */
class InvalidInputException extends Exception {
    public InvalidInputException(String message) {
        super(message);
    }
}

/**
 * Custom unchecked exception
 */
class ValidationException extends RuntimeException {
    public ValidationException(String message) {
        super(message);
    }
}

/**
 * BankAccount with exception handling
 */
class BankAccount {
    private double balance;
    private String accountNumber;
    
    public BankAccount(String accountNumber, double initialBalance) {
        this.accountNumber = accountNumber;
        if (initialBalance < 0) {
            throw new ValidationException("Initial balance cannot be negative");
        }
        this.balance = initialBalance;
    }
    
    public void withdraw(double amount) throws InvalidInputException {
        if (amount <= 0) {
            throw new InvalidInputException("Withdrawal amount must be positive");
        }
        if (amount > balance) {
            throw new InvalidInputException("Insufficient balance. Available: " + balance);
        }
        balance -= amount;
        System.out.println("Withdrawn: $" + amount);
    }
    
    public void deposit(double amount) throws InvalidInputException {
        if (amount <= 0) {
            throw new InvalidInputException("Deposit amount must be positive");
        }
        balance += amount;
        System.out.println("Deposited: $" + amount);
    }
    
    public double getBalance() {
        return balance;
    }
}

/**
 * CustomExceptionDemo - Demonstrates custom exceptions
 */
public class CustomExceptionDemo {
    
    public static void main(String[] args) {
        System.out.println("=== CUSTOM EXCEPTIONS DEMO ===\n");
        
        BankAccount account = new BankAccount("ACC123", 1000);
        System.out.println("Initial balance: $" + account.getBalance());
        
        // Valid deposit
        try {
            account.deposit(500);
        } catch (InvalidInputException e) {
            System.out.println("Error: " + e.getMessage());
        }
        
        // Invalid deposit
        try {
            account.deposit(-100);
        } catch (InvalidInputException e) {
            System.out.println("Error: " + e.getMessage());
        }
        
        // Valid withdrawal
        try {
            account.withdraw(300);
        } catch (InvalidInputException e) {
            System.out.println("Error: " + e.getMessage());
        }
        
        // Overdraft attempt
        try {
            account.withdraw(5000);
        } catch (InvalidInputException e) {
            System.out.println("Error: " + e.getMessage());
        }
        
        System.out.println("\nFinal balance: $" + account.getBalance());
        
        // Test validation exception (unchecked)
        System.out.println("\n--- Unchecked Exception ---");
        try {
            BankAccount badAccount = new BankAccount("ACC999", -100);
        } catch (ValidationException e) {
            System.out.println("Caught validation error: " + e.getMessage());
        }
    }
}
```

---

## 8. Exercises

### Exercise 1: Calculator with Exception Handling

**Requirements:**
1. Create calculator with divide operation
2. Handle division by zero
3. Handle invalid input
4. Use custom exception

---

### Exercise 2: Student Grade Validator

**Requirements:**
1. Validate grade (0-100)
2. Throw custom exception for invalid grades
3. Handle and display appropriate messages

---

## 9. Solutions

### Solution 1: Calculator with Exception Handling

```java
class CalculatorException extends Exception {
    public CalculatorException(String message) {
        super(message);
    }
}

class Calculator {
    public double divide(double a, double b) throws CalculatorException {
        if (b == 0) {
            throw new CalculatorException("Cannot divide by zero");
        }
        return a / b;
    }
}

public class CalculatorDemo {
    public static void main(String[] args) {
        Calculator calc = new Calculator();
        
        try {
            System.out.println("10 / 2 = " + calc.divide(10, 2));
            System.out.println("10 / 0 = " + calc.divide(10, 0));
        } catch (CalculatorException e) {
            System.out.println("Error: " + e.getMessage());
        }
    }
}
```

---

### Solution 2: Student Grade Validator

```java
class InvalidGradeException extends Exception {
    public InvalidGradeException(String message) {
        super(message);
    }
}

class GradeValidator {
    public void validateGrade(int grade) throws InvalidGradeException {
        if (grade < 0 || grade > 100) {
            throw new InvalidGradeException("Grade must be between 0 and 100, got: " + grade);
        }
        System.out.println("Valid grade: " + grade);
    }
}

public class GradeDemo {
    public static void main(String[] args) {
        GradeValidator validator = new GradeValidator();
        int[] grades = {85, -5, 100, 150};
        
        for (int grade : grades) {
            try {
                validator.validateGrade(grade);
            } catch (InvalidGradeException e) {
                System.out.println("Error: " + e.getMessage());
            }
        }
    }
}
```

---

## Summary

### Key Takeaways

1. **Throwable** is the parent of all exceptions
2. **Checked** exceptions must be declared or caught
3. **Unchecked** exceptions occur at runtime
4. **try-with-resources** auto-closes resources (Java 7+)
5. **Custom exceptions** for domain-specific errors

---

*Happy Coding! 🚀*
