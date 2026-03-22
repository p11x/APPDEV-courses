/*
 * =========================================================================================
 * SUB TOPIC: Basic Input/Output using Scanner Class
 * =========================================================================================
 * 
 * DEFINITION:
 * -----------
 * The Scanner class in Java is used to read input from various sources like keyboard (console),
 * files, or streams. It is part of the java.util package and provides methods to parse 
 * primitive types and strings from input sources.
 * 
 * FUNCTIONALITIES:
 * ----------------
 * 1. nextInt() - Reads an integer from input
 * 2. nextDouble() - Reads a double from input
 * 3. nextLine() - Reads a complete line of text
 * 4. next() - Reads a single word
 * 5. nextBoolean() - Reads a boolean value
 * 6. nextByte() - Reads a byte value
 * 7. nextShort() - Reads a short value
 * 8. nextFloat() - Reads a float value
 * 9. nextLong() - Reads a long value
 * 
 * =========================================================================================
 */

import java.util.Scanner; // Import Scanner class from java.util package

/**
 * Example1: Reading Basic Integer Input
 * 
 * Real-time Examples:
 * 1. User entering their age in a registration form
 * 2. Entering quantity in an e-commerce shopping cart
 * 3. Inputting a ticket number for booking confirmation
 * 4. Entering a room number in a hotel reservation system
 * 5. Providing a mobile number for OTP verification
 */
public class Example1 {
    // Main method - entry point of the Java program
    public static void main(String[] args) {
        // Step 1: Create a Scanner object to read input from standard input (keyboard)
        // System.in represents the standard input stream (keyboard input)
        Scanner scanner = new Scanner(System.in);
        
        // Step 2: Display a prompt message to the user
        // System.out.println prints text followed by a new line
        System.out.println("Enter your age: ");
        
        // Step 3: Read the integer input from the user
        // nextInt() method reads the next token as an integer
        // The program waits here until user enters a value and presses Enter
        int age = scanner.nextInt();
        
        // Step 4: Display the entered value back to the user
        // Using string concatenation with + operator
        System.out.println("You entered age: " + age);
        
        // Step 5: Close the Scanner to release resources
        // It's a good practice to close Scanner when done
        scanner.close();
    }
}

/*
 * STEP-BY-STEP EXPLANATION:
 * -------------------------
 * 
 * Step 1: Import Scanner
 *    - import java.util.Scanner; makes the Scanner class available
 *    - Without this import, the code won't compile
 * 
 * Step 2: Create Scanner Object
 *    - new Scanner(System.in) creates a Scanner that reads from keyboard
 *    - System.in is the standard input stream
 * 
 * Step 3: Display Prompt
 *    - System.out.println() shows a message to the user
 *    - This tells the user what input is expected
 * 
 * Step 4: Read Input
 *    - scanner.nextInt() reads an integer from the user
 *    - The method blocks (waits) until user enters data
 *    - The entered value is stored in the variable 'age'
 * 
 * Step 5: Display Output
 *    - System.out.println() displays the result
 *    - The + operator concatenates the string with the integer value
 * 
 * Step 6: Close Scanner
 *    - scanner.close() releases the system resources
 *    - Should always be called when done reading input
 * 
 * =========================================================================================
 * REAL-TIME USE CASES:
 * =========================================================================================
 * 
 * 1. REGISTRATION FORMS:
 *    When users sign up for a service, they often need to enter their age.
 *    The system uses nextInt() to read this value.
 * 
 * 2. E-COMMERCE APPLICATIONS:
 *    When ordering products, users enter quantity using similar input methods.
 * 
 * 3. BANKING APPLICATIONS:
 *    Account numbers, transaction amounts, and other numerical data are entered
 *    using Scanner or similar input mechanisms.
 * 
 * 4. EDUCATIONAL SYSTEMS:
 *    Student grades, roll numbers, and marks are entered through such inputs.
 * 
 * 5. TICKET BOOKING SYSTEMS:
 *    PNR numbers, seat numbers, and passenger counts are input using similar methods.
 */
