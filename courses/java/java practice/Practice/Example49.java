/*
 * SUB TOPIC: Java Output Statements
 * 
 * DEFINITION:
 * Java provides several ways to output data: System.out.print(), System.out.println(), System.out.printf(),
 * and logging frameworks. Each method serves different purposes - print adds no newline, println adds
 * newline, and printf provides formatted output.
 * 
 * FUNCTIONALITIES:
 * 1. System.out.print() - Output without newline
 * 2. System.out.println() - Output with newline
 * 3. System.out.printf() - Formatted output
 * 4. Format specifiers - %d, %s, %f, etc.
 * 5. String.format() - Create formatted strings
 */

public class Example49 {
    public static void main(String[] args) {
        
        // Topic Explanation: Output Methods
        
        // print() - no newline
        System.out.print("Hello ");
        System.out.print("World");
        System.out.println(); // Empty line
        
        // println() - with newline
        System.out.println("Line 1");
        System.out.println("Line 2");
        
        // printf() - formatted output
        System.out.println("\n=== printf Examples ===");
        
        // Integer
        System.out.printf("Integer: %d%n", 42);
        
        // Float/Double
        System.out.printf("Float: %f%n", 3.14159);
        System.out.printf("Float (2 decimals): %.2f%n", 3.14159);
        
        // String
        System.out.printf("String: %s%n", "Hello");
        
        // Character
        System.out.printf("Character: %c%n", 'A');
        
        // Multiple values
        System.out.printf("Name: %s, Age: %d%n", "John", 25);
        
        // Format specifiers
        System.out.println("\n=== Format Specifiers ===");
        System.out.printf("Decimal: %d%n", 255);
        System.out.printf("Hex: %x%n", 255);
        System.out.printf("Octal: %o%n", 255);
        System.out.printf("Float: %f%n", 123.456);
        System.out.printf("Scientific: %e%n", 123.456);
        
        // Width and alignment
        System.out.println("\n=== Width and Alignment ===");
        System.out.printf("|%10d|%n", 123);  // Right align
        System.out.printf("|%-10d|%n", 123); // Left align
        System.out.printf("|%10s|%n", "Hello");
        
        // String.format()
        System.out.println("\n=== String.format() ===");
        String formatted = String.format("Price: $%.2f", 99.99);
        System.out.println(formatted);
        
        // Real-time Example 1: Receipt
        System.out.println("\n=== Example 1: Receipt ===");
        String item = "Laptop";
        double price = 999.99;
        int qty = 2;
        System.out.println("=== RECEIPT ===");
        System.out.printf("Item: %s%n", item);
        System.out.printf("Price: $%.2f%n", price);
        System.out.printf("Quantity: %d%n", qty);
        System.out.printf("Total: $%.2f%n", price * qty);
        
        // Real-time Example 2: Student Report
        System.out.println("\n=== Example 2: Student Report ===");
        String name = "Alice";
        int marks = 85;
        System.out.printf("Student: %s%n", name);
        System.out.printf("Marks: %d%n", marks);
        System.out.printf("Grade: %c%n", marks >= 90 ? 'A' : marks >= 80 ? 'B' : 'C');
        
        // Real-time Example 3: Time Display
        System.out.println("\n=== Example 3: Time ===");
        int hours = 14;
        int minutes = 30;
        int seconds = 45;
        System.out.printf("Time: %02d:%02d:%02d%n", hours, minutes, seconds);
        
        // Real-time Example 4: Table
        System.out.println("\n=== Example 4: Table ===");
        System.out.printf("|%-10s|%6s|%6s|%n", "Product", "Qty", "Price");
        System.out.printf("|%-10s|%6d|%6.2f|%n", "Apple", 10, 5.50);
        System.out.printf("|%-10s|%6d|%6.2f|%n", "Banana", 20, 2.00);
        
        // Real-time Example 5: Percentage
        System.out.println("\n=== Example 5: Percentage ===");
        int correct = 85;
        int total = 100;
        System.out.printf("Score: %d/%d%n", correct, total);
        System.out.printf("Percentage: %d%%%n", (correct * 100) / total);
        
        // Real-time Example 6: Date Format
        System.out.println("\n=== Example 6: Date ===");
        int day = 22;
        int month = 3;
        int year = 2024;
        System.out.printf("Date: %02d/%02d/%04d%n", day, month, year);
    }
}
