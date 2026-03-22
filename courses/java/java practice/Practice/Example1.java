/*
 * SUB TOPIC: Basic Input/Output using Scanner Class
 * 
 * DEFINITION:
 * The Scanner class in Java is used to read input from various sources like keyboard (console),
 * files, or streams. It is part of the java.util package and provides methods to parse 
 * primitive types and strings from input sources.
 * 
 * FUNCTIONALITIES:
 * 1. nextInt() - Reads an integer from input
 * 2. nextDouble() - Reads a double from input
 * 3. nextLine() - Reads a complete line of text
 * 4. next() - Reads a single word
 * 5. nextBoolean() - Reads a boolean value
 */

import java.util.Scanner; // Import Scanner class from java.util package

public class Example1 {
    public static void main(String[] args) {
        // Create Scanner object to read from keyboard - System.in represents standard input
        Scanner scanner = new Scanner(System.in);
        
        // Real-time Example 1: User entering age in registration form
        System.out.println("=== User Registration ===");
        System.out.print("Enter your age: ");
        int age = scanner.nextInt(); // Reads integer from user input
        System.out.println("Your age is: " + age);
        
        // Real-time Example 2: Entering quantity in e-commerce cart
        System.out.print("Enter quantity to order: ");
        int quantity = scanner.nextInt(); // Reads quantity value
        System.out.println("Quantity: " + quantity);
        
        // Real-time Example 3: Inputting ticket number for booking
        System.out.print("Enter ticket number: ");
        int ticketNumber = scanner.nextInt(); // Reads ticket number
        System.out.println("Ticket Number: " + ticketNumber);
        
        // Real-time Example 4: Entering room number in hotel reservation
        System.out.print("Enter room number: ");
        int roomNumber = scanner.nextInt(); // Reads room number
        System.out.println("Room Number: " + roomNumber);
        
        // Real-time Example 5: Providing mobile number for OTP
        System.out.print("Enter mobile number: ");
        long mobileNumber = scanner.nextLong(); // Reads long integer for mobile
        System.out.println("Mobile Number: " + mobileNumber);
        
        scanner.close(); // Close scanner to release resources
    }
}
