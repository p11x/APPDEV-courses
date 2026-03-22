/*
 * SUB TOPIC: Variables and Data Types in Java
 * 
 * DEFINITION:
 * Variables are containers for storing data values. Java has two types of data types: primitive 
 * (byte, short, int, long, float, double, char, boolean) and reference (objects, arrays, interfaces).
 * 
 * FUNCTIONALITIES:
 * 1. Primitive data types - 8 basic types
 * 2. Reference data types - objects and arrays
 * 3. Type conversion - implicit and explicit
 * 4. Variables - local, instance, static
 * 5. Constants - final keyword
 */

public class Example48 {
    
    // Instance variable
    private int instanceVar = 10;
    
    // Static variable
    private static int staticVar = 20;
    
    public static void main(String[] args) {
        
        // Topic Explanation: Primitive Data Types
        
        // byte - 8-bit integer
        System.out.println("=== Primitive Data Types ===");
        byte byteVal = 100;
        System.out.println("byte: " + byteVal + " (Range: -128 to 127)");
        
        // short - 16-bit integer
        short shortVal = 10000;
        System.out.println("short: " + shortVal + " (Range: -32768 to 32767)");
        
        // int - 32-bit integer
        int intVal = 100000;
        System.out.println("int: " + intVal + " (Range: -2^31 to 2^31-1)");
        
        // long - 64-bit integer
        long longVal = 10000000000L;
        System.out.println("long: " + longVal + " (Range: -2^63 to 2^63-1)");
        
        // float - 32-bit floating point
        float floatVal = 10.5f;
        System.out.println("float: " + floatVal);
        
        // double - 64-bit floating point
        double doubleVal = 10.5;
        System.out.println("double: " + doubleVal);
        
        // char - 16-bit Unicode character
        char charVal = 'A';
        System.out.println("char: " + charVal + " (Unicode range: 0 to 65535)");
        
        // boolean - true or false
        boolean boolVal = true;
        System.out.println("boolean: " + boolVal);
        
        // Type Conversion
        System.out.println("\n=== Type Conversion ===");
        
        // Implicit (widening) conversion
        int i = 100;
        long l = i; // int to long - automatic
        System.out.println("Implicit: int to long = " + l);
        
        // Explicit (narrowing) conversion
        double d = 100.99;
        int ni = (int) d; // double to int - manual cast
        System.out.println("Explicit: double to int = " + ni);
        
        // Variables
        System.out.println("\n=== Variables ===");
        
        // Local variable
        int localVar = 50;
        System.out.println("Local variable: " + localVar);
        
        // Static variable
        System.out.println("Static variable: " + staticVar);
        
        // Constants
        System.out.println("\n=== Constants (final) ===");
        final double PI = 3.14159;
        System.out.println("PI: " + PI);
        
        // Real-time Example 1: Age calculation
        System.out.println("\n=== Example 1: Age ===");
        byte age = 25;
        System.out.println("Age: " + age);
        
        // Real-time Example 2: Bank balance
        System.out.println("\n=== Example 2: Bank Balance ===");
        double balance = 1500.75;
        System.out.println("Balance: $" + balance);
        
        // Real-time Example 3: Product price
        System.out.println("\n=== Example 3: Product Price ===");
        float price = 99.99f;
        System.out.println("Price: $" + price);
        
        // Real-time Example 4: Grade
        System.out.println("\n=== Example 4: Grade ===");
        char grade = 'A';
        System.out.println("Grade: " + grade);
        
        // Real-time Example 5: Is Active
        System.out.println("\n=== Example 5: Account Status ===");
        boolean isActive = true;
        System.out.println("Account Active: " + isActive);
        
        // Real-time Example 6: Count
        System.out.println("\n=== Example 6: Count ===");
        long count = 1000000000L;
        System.out.println("Total Count: " + count);
    }
}
