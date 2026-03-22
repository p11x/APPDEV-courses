/*
 * =========================================================================================
 * SUB TOPIC: Basic Input/Output using Scanner Class
 * =========================================================================================
 * 
 * DEFINITION:
 * -----------
 * The Scanner class provides various methods to read different types of input from the user.
 * This example demonstrates reading multiple types of data in a single program - integers,
 * doubles, and strings. This is commonly used in real-world applications like registration
 * forms, banking systems, and data entry applications.
 * 
 * FUNCTIONALITIES:
 * ----------------
 * 1. nextInt() - Reads an integer value
 * 2. nextDouble() - Reads a decimal number
 * 3. nextLine() - Reads an entire line of text
 * 4. next() - Reads a single word (up to whitespace)
 * 5. Displaying formatted output using System.out.println()
 * 
 * =========================================================================================
 */

import java.util.Scanner; // Import Scanner class from java.util package

/**
 * Example2: Reading Multiple Types of Input
 * 
 * Real-time Examples:
 * 1. Student registration system - collecting name, age, and marks
 * 2. Employee payroll system - collecting employee ID, salary, and department
 * 3. Product order system - collecting product name, quantity, and price
 * 4. Banking account creation - collecting account number, name, and balance
 * 5. Library book issue system - collecting book ID, member name, and due date
 */
public class Example2 {
    // Main method - entry point of the Java program
    public static void main(String[] args) {
        // Create a Scanner object to read input from keyboard
        // System.in represents standard input (keyboard)
        Scanner scanner = new Scanner(System.in);
        
        // ========================================================================
        // EXAMPLE 1: Student Registration Form
        // ========================================================================
        // This section demonstrates reading student information
        
        System.out.println("=== STUDENT REGISTRATION ===");
        
        // Display prompt for student name
        // nextLine() reads the entire line including spaces
        System.out.print("Enter Student Name: ");
        String studentName = scanner.nextLine(); // Reads full name with spaces
        
        // Display prompt for student age (integer)
        // nextInt() reads the next token as an integer
        System.out.print("Enter Student Age: ");
        int studentAge = scanner.nextInt(); // Reads integer value
        
        // Display prompt for student marks (decimal)
        // nextDouble() reads the next token as a double
        System.out.print("Enter Student Marks: ");
        double studentMarks = scanner.nextDouble(); // Reads decimal value
        
        // Consume the newline character left by nextInt() or nextDouble()
        // This is necessary before using nextLine() again
        scanner.nextLine(); // Clear the newline buffer
        
        // Display the collected information
        System.out.println("\n--- Student Details ---");
        System.out.println("Name: " + studentName); // String concatenation
        System.out.println("Age: " + studentAge);
        System.out.println("Marks: " + studentMarks);
        
        // ========================================================================
        // EXAMPLE 2: Employee Payroll System
        // ========================================================================
        // This section demonstrates reading employee information
        
        System.out.println("\n=== EMPLOYEE PAYROLL ===");
        
        System.out.print("Enter Employee ID: ");
        int empId = scanner.nextInt(); // Reads employee ID
        
        System.out.print("Enter Employee Name: ");
        scanner.nextLine(); // Clear buffer
        String empName = scanner.nextLine(); // Reads full name
        
        System.out.print("Enter Department: ");
        String department = scanner.nextLine(); // Reads department name
        
        System.out.print("Enter Salary: ");
        double salary = scanner.nextDouble(); // Reads salary
        
        // Display employee details
        System.out.println("\n--- Employee Details ---");
        System.out.println("Employee ID: " + empId);
        System.out.println("Name: " + empName);
        System.out.println("Department: " + department);
        System.out.println("Salary: $" + salary);
        
        // ========================================================================
        // EXAMPLE 3: Product Order System
        // ========================================================================
        
        System.out.println("\n=== PRODUCT ORDER ===");
        
        System.out.print("Enter Product Name: ");
        scanner.nextLine(); // Clear buffer
        String productName = scanner.nextLine();
        
        System.out.print("Enter Quantity: ");
        int quantity = scanner.nextInt(); // Reads quantity
        
        System.out.print("Enter Unit Price: ");
        double unitPrice = scanner.nextDouble(); // Reads price
        
        // Calculate total price
        double totalPrice = quantity * unitPrice; // Mathematical calculation
        
        // Display order details
        System.out.println("\n--- Order Details ---");
        System.out.println("Product: " + productName);
        System.out.println("Quantity: " + quantity);
        System.out.println("Unit Price: $" + unitPrice);
        System.out.println("Total Price: $" + totalPrice);
        
        // ========================================================================
        // EXAMPLE 4: Banking Account Creation
        // ========================================================================
        
        System.out.println("\n=== BANK ACCOUNT CREATION ===");
        
        System.out.print("Enter Account Number: ");
        long accountNumber = scanner.nextLong(); // Reads long integer
        
        System.out.print("Enter Account Holder Name: ");
        scanner.nextLine(); // Clear buffer
        String accountHolder = scanner.nextLine();
        
        System.out.print("Enter Initial Balance: ");
        double balance = scanner.nextDouble();
        
        System.out.print("Enter Account Type (Savings/Current): ");
        scanner.nextLine(); // Clear buffer
        String accountType = scanner.nextLine();
        
        // Display account information
        System.out.println("\n--- Account Information ---");
        System.out.println("Account Number: " + accountNumber);
        System.out.println("Holder Name: " + accountHolder);
        System.out.println("Balance: $" + balance);
        System.out.println("Account Type: " + accountType);
        
        // ========================================================================
        // EXAMPLE 5: Library Book Issue System
        // ========================================================================
        
        System.out.println("\n=== LIBRARY BOOK ISSUE ===");
        
        System.out.print("Enter Book ID: ");
        int bookId = scanner.nextInt(); // Reads book ID
        
        System.out.print("Enter Member Name: ");
        scanner.nextLine(); // Clear buffer
        String memberName = scanner.nextLine();
        
        System.out.print("Enter Number of Days: ");
        int days = scanner.nextInt(); // Reads number of days
        
        // Calculate fine if any (example: Rs. 2 per day after 14 days)
        int freeDays = 14; // Free period
        int extraDays = 0; // Days beyond free period
        double fine = 0; // Fine amount
        
        if (days > freeDays) {
            // Calculate extra days and fine
            extraDays = days - freeDays;
            fine = extraDays * 2.0; // Rs. 2 per extra day
        }
        
        // Display library details
        System.out.println("\n--- Issue Details ---");
        System.out.println("Book ID: " + bookId);
        System.out.println("Member Name: " + memberName);
        System.out.println("Days Borrowed: " + days);
        System.out.println("Extra Days: " + extraDays);
        System.out.println("Fine Amount: Rs. " + fine);
        
        // Close the scanner to prevent resource leak
        scanner.close();
        
        System.out.println("\n=== DATA COLLECTION COMPLETE ===");
    }
}

/*
 * STEP-BY-STEP EXPLANATION:
 * -------------------------
 * 
 * Step 1: Import Required Package
 *    - import java.util.Scanner; allows us to use Scanner class
 *    - Scanner is not part of Java's core, so it must be imported
 * 
 * Step 2: Create Scanner Object
 *    - new Scanner(System.in) connects to keyboard input
 *    - System.in is the standard input stream
 * 
 * Step 3: Read Different Data Types
 *    - nextInt() - reads integers (whole numbers)
 *    - nextDouble() - reads decimal numbers
 *    - nextLong() - reads large integers
 *    - nextLine() - reads complete lines with spaces
 *    - next() - reads single words
 * 
 * Step 4: Handle Buffer Issues
 *    - After using nextInt(), nextDouble(), etc., a newline remains in buffer
 *    - scanner.nextLine() clears this newline character
 *    - Without this, the next nextLine() call would read an empty string
 * 
 * Step 5: Process and Display Data
 *    - Use System.out.println() to display output
 *    - Use + operator to concatenate strings with variables
 * 
 * Step 6: Close Scanner
 *    - Always close scanner to free system resources
 *    - Prevents resource leaks in long-running applications
 * 
 * =========================================================================================
 * REAL-TIME USE CASES:
 * =========================================================================================
 * 
 * 1. STUDENT INFORMATION SYSTEMS:
 *    - Universities collect student details for enrollment
 *    - Schools maintain student records with multiple data types
 * 
 * 2. HUMAN RESOURCE MANAGEMENT:
 *    - Employee databases store various employee attributes
 *    - Payroll systems process salary and deduction information
 * 
 * 3. E-COMMERCE PLATFORMS:
 *    - Order management systems track products and quantities
 *    - Inventory systems monitor stock levels
 * 
 * 4. BANKING APPLICATIONS:
 *    - Account management requires multiple data types
 *    - Transaction processing uses various numeric types
 * 
 * 5. LIBRARY MANAGEMENT SYSTEMS:
 *    - Book issue/return tracking
 *    - Member subscription management
 * 
 * =========================================================================================
 */
