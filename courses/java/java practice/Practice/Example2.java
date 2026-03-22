/*
 * SUB TOPIC: Basic Input/Output using Scanner Class
 * 
 * DEFINITION:
 * The Scanner class provides various methods to read different types of input from the user.
 * This example demonstrates reading multiple types of data - integers, doubles, and strings.
 * 
 * FUNCTIONALITIES:
 * 1. nextInt() - Reads an integer value
 * 2. nextDouble() - Reads a decimal number
 * 3. nextLine() - Reads an entire line of text
 * 4. next() - Reads a single word (up to whitespace)
 */

import java.util.Scanner;

public class Example2 {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        
        // Real-time Example 1: Student registration system
        System.out.println("=== STUDENT REGISTRATION ===");
        System.out.print("Enter Student Name: ");
        String studentName = scanner.nextLine(); // Reads full name with spaces
        System.out.print("Enter Student Age: ");
        int studentAge = scanner.nextInt(); // Reads integer value
        System.out.print("Enter Student Marks: ");
        double studentMarks = scanner.nextDouble(); // Reads decimal value
        scanner.nextLine(); // Clear newline buffer
        
        System.out.println("Name: " + studentName);
        System.out.println("Age: " + studentAge);
        System.out.println("Marks: " + studentMarks);
        
        // Real-time Example 2: Employee payroll system
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
        
        System.out.println("Employee ID: " + empId);
        System.out.println("Name: " + empName);
        System.out.println("Department: " + department);
        System.out.println("Salary: $" + salary);
        
        // Real-time Example 3: Product order system
        System.out.println("\n=== PRODUCT ORDER ===");
        System.out.print("Enter Product Name: ");
        scanner.nextLine(); // Clear buffer
        String productName = scanner.nextLine();
        System.out.print("Enter Quantity: ");
        int quantity = scanner.nextInt(); // Reads quantity
        System.out.print("Enter Unit Price: ");
        double unitPrice = scanner.nextDouble(); // Reads price
        
        double totalPrice = quantity * unitPrice; // Calculate total
        System.out.println("Product: " + productName);
        System.out.println("Quantity: " + quantity);
        System.out.println("Total Price: $" + totalPrice);
        
        // Real-time Example 4: Banking account creation
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
        
        System.out.println("Account Number: " + accountNumber);
        System.out.println("Holder Name: " + accountHolder);
        System.out.println("Balance: $" + balance);
        System.out.println("Account Type: " + accountType);
        
        // Real-time Example 5: Library book issue system
        System.out.println("\n=== LIBRARY BOOK ISSUE ===");
        System.out.print("Enter Book ID: ");
        int bookId = scanner.nextInt(); // Reads book ID
        System.out.print("Enter Member Name: ");
        scanner.nextLine(); // Clear buffer
        String memberName = scanner.nextLine();
        System.out.print("Enter Number of Days: ");
        int days = scanner.nextInt(); // Reads number of days
        
        int freeDays = 14;
        int extraDays = 0;
        double fine = 0;
        
        if (days > freeDays) {
            extraDays = days - freeDays;
            fine = extraDays * 2.0; // Fine calculation
        }
        
        System.out.println("Book ID: " + bookId);
        System.out.println("Member Name: " + memberName);
        System.out.println("Days Borrowed: " + days);
        System.out.println("Fine Amount: Rs. " + fine);
        
        scanner.close();
    }
}
