/*
 * =========================================================================================
 * SUB TOPIC: Basic Input/Output using Scanner Class
 * =========================================================================================
 * 
 * DEFINITION:
 * -----------
 * Buffered input in Java provides more efficient way of reading input compared to 
 * unbuffered input. While Scanner is convenient for beginners, understanding input
 * handling is crucial. This example demonstrates reading various data types with
 * proper validation and error handling in real-world scenarios.
 * 
 * FUNCTIONALITIES:
 * ----------------
 * 1. Input validation - checking if input is valid before processing
 * 2. Exception handling - catching input mismatches
 * 3. Default values - providing fallback values for invalid input
 * 4. Input buffering - efficient reading of input
 * 5. Type conversion - converting between different data types
 * 
 * =========================================================================================
 */

import java.util.Scanner; // Import Scanner class for input handling

/**
 * Example3: Advanced Input Handling with Validation
 * 
 * Real-time Examples:
 * 1. Temperature conversion system - validating temperature inputs
 * 2. Currency converter - validating currency amounts
 * 3. Grade calculator - validating grade inputs
 * 4. Distance calculator - validating distance inputs
 * 5. Weight converter - validating weight inputs
 */
public class Example3 {
    // Main method - entry point of the Java program
    public static void main(String[] args) {
        // Create Scanner object to read input from keyboard
        Scanner scanner = new Scanner(System.in);
        
        // ========================================================================
        // EXAMPLE 1: Temperature Conversion System
        // ========================================================================
        // Real-world use: Weather applications, scientific calculations
        
        System.out.println("=== TEMPERATURE CONVERTER ===");
        
        // Prompt user for temperature in Celsius
        System.out.print("Enter temperature in Celsius: ");
        
        // Check if input is a valid integer
        if (scanner.hasNextInt()) {
            // Read the integer value
            int celsius = scanner.nextInt();
            
            // Convert Celsius to Fahrenheit using the formula: F = (C * 9/5) + 32
            // Multiplication has higher precedence than addition
            double fahrenheit = (celsius * 9.0 / 5.0) + 32;
            
            // Display the converted temperature
            System.out.println("Temperature in Fahrenheit: " + fahrenheit + "°F");
            
            // Additional: Convert to Kelvin
            // Formula: K = C + 273.15
            double kelvin = celsius + 273.15;
            System.out.println("Temperature in Kelvin: " + kelvin + "K");
        } else {
            // Handle invalid input
            System.out.println("Invalid input! Please enter a valid integer.");
            // Consume the invalid input to prevent infinite loop
            scanner.next();
        }
        
        // ========================================================================
        // EXAMPLE 2: Currency Converter
        // ========================================================================
        // Real-world use: Banking apps, international trade, travel applications
        
        System.out.println("\n=== CURRENCY CONVERTER ===");
        
        // Exchange rates (fixed for demonstration)
        double usdToInr = 83.0; // 1 USD = 83 INR
        double usdToEur = 0.92; // 1 USD = 0.92 EUR
        double usdToGbp = 0.79; // 1 USD = 0.79 GBP
        
        // Prompt for amount in USD
        System.out.print("Enter amount in USD: ");
        
        // Check if input is a valid double
        if (scanner.hasNextDouble()) {
            // Read the double value representing dollars
            double usdAmount = scanner.nextDouble();
            
            // Convert to different currencies
            double inrAmount = usdAmount * usdToInr;
            double eurAmount = usdAmount * usdToEur;
            double gbpAmount = usdAmount * usdToGbp;
            
            // Display converted amounts with proper formatting
            System.out.println("\nConverted Amounts:");
            System.out.println("USD " + usdAmount + " = INR " + inrAmount);
            System.out.println("USD " + usdAmount + " = EUR " + eurAmount);
            System.out.println("USD " + usdAmount + " = GBP " + gbpAmount);
        } else {
            // Handle invalid input
            System.out.println("Invalid input! Please enter a valid number.");
            scanner.next();
        }
        
        // ========================================================================
        // EXAMPLE 3: Grade Calculator
        // ========================================================================
        // Real-world use: Academic systems, online learning platforms
        
        System.out.println("\n=== GRADE CALCULATOR ===");
        
        // Prompt for marks obtained (out of 100)
        System.out.print("Enter marks obtained (0-100): ");
        
        // Read marks as integer
        int marks = scanner.nextInt();
        
        // Determine grade based on marks using conditional logic
        // Grading scale: A (90-100), B (80-89), C (70-79), D (60-69), F (<60)
        String grade; // Variable to store the grade
        
        if (marks >= 90 && marks <= 100) {
            // Marks between 90 and 100 inclusive get grade A
            grade = "A";
        } else if (marks >= 80) {
            // Marks between 80 and 89 get grade B
            grade = "B";
        } else if (marks >= 70) {
            // Marks between 70 and 79 get grade C
            grade = "C";
        } else if (marks >= 60) {
            // Marks between 60 and 69 get grade D
            grade = "D";
        } else if (marks >= 0) {
            // Marks below 60 get grade F
            grade = "F";
        } else {
            // Invalid marks (negative)
            grade = "Invalid";
            System.out.println("Error: Marks cannot be negative!");
        }
        
        // Display the calculated grade
        System.out.println("Grade: " + grade);
        
        // Calculate percentage and division
        double percentage = marks; // Since out of 100
        String division;
        
        if (percentage >= 60) {
            division = "First Division";
        } else if (percentage >= 45) {
            division = "Second Division";
        } else if (percentage >= 33) {
            division = "Third Division";
        } else {
            division = "Fail";
        }
        
        System.out.println("Percentage: " + percentage + "%");
        System.out.println("Division: " + division);
        
        // ========================================================================
        // EXAMPLE 4: Distance Converter
        // ========================================================================
        // Real-world use: Navigation apps, fitness trackers, construction
        
        System.out.println("\n=== DISTANCE CONVERTER ===");
        
        // Conversion factors
        double kmToMiles = 0.621371; // 1 km = 0.621371 miles
        double kmToMeters = 1000.0; // 1 km = 1000 meters
        double kmToFeet = 3280.84; // 1 km = 3280.84 feet
        
        // Prompt for distance in kilometers
        System.out.print("Enter distance in kilometers: ");
        
        // Read distance as double
        double km = scanner.nextDouble();
        
        // Convert to different units
        double miles = km * kmToMiles;
        double meters = km * kmToMeters;
        double feet = km * kmToFeet;
        
        // Display all conversions
        System.out.println("\nDistance Conversions:");
        System.out.println(km + " km = " + miles + " miles");
        System.out.println(km + " km = " + meters + " meters");
        System.out.println(km + " km = " + feet + " feet");
        
        // ========================================================================
        // EXAMPLE 5: Weight Converter
        // ========================================================================
        // Real-world use: Health apps, shipping logistics, cooking
        
        System.out.println("\n=== WEIGHT CONVERTER ===");
        
        // Conversion factors
        double kgToPounds = 2.20462; // 1 kg = 2.20462 pounds
        double kgToGrams = 1000.0; // 1 kg = 1000 grams
        double kgToOunces = 35.274; // 1 kg = 35.274 ounces
        
        // Prompt for weight in kilograms
        System.out.print("Enter weight in kilograms: ");
        
        // Read weight as double
        double weightKg = scanner.nextDouble();
        
        // Convert to different units
        double pounds = weightKg * kgToPounds;
        double grams = weightKg * kgToGrams;
        double ounces = weightKg * kgToOunces;
        
        // Display all conversions
        System.out.println("\nWeight Conversions:");
        System.out.println(weightKg + " kg = " + pounds + " pounds");
        System.out.println(weightKg + " kg = " + grams + " grams");
        System.out.println(weightKg + " kg = " + ounces + " ounces");
        
        // Close the scanner to free resources
        scanner.close();
        
        System.out.println("\n=== CONVERSION COMPLETE ===");
    }
}

/*
 * STEP-BY-STEP EXPLANATION:
 * -------------------------
 * 
 * Step 1: Import Scanner Class
 *    - import java.util.Scanner; is required for input operations
 *    - Scanner is in the java.util package
 * 
 * Step 2: Create Scanner Object
 *    - new Scanner(System.in) creates input stream from keyboard
 *    - System.in represents standard input (console/keyboard)
 * 
 * Step 3: Check Input Type Before Reading
 *    - hasNextInt() returns true if next token is an integer
 *    - hasNextDouble() returns true if next token is a double
 *    - This prevents runtime errors from invalid input
 * 
 * Step 4: Read Input Using Appropriate Method
 *    - nextInt() for integers
 *    - nextDouble() for decimal numbers
 *    - next() for single word
 *    - nextLine() for complete line
 * 
 * Step 5: Process the Input
 *    - Perform calculations using arithmetic operators
 *    - Apply formulas specific to each application
 * 
 * Step 6: Display Results
 *    - System.out.println() for output
 *    - Use string concatenation with +
 * 
 * Step 7: Handle Invalid Input
 *    - Use if-else to check input validity
 *    - Provide meaningful error messages
 *    - Clear invalid input from scanner buffer
 * 
 * Step 8: Close Scanner
 *    - Always close scanner after use
 *    - Prevents resource leaks
 * 
 * =========================================================================================
 * REAL-TIME USE CASES:
 * =========================================================================================
 * 
 * 1. WEATHER APPLICATIONS:
 *    - Convert temperatures between Celsius, Fahrenheit, Kelvin
 *    - Essential for international weather reports
 * 
 * 2. BANKING AND FINANCE:
 *    - Currency conversion for international transactions
 *    - Investment calculations across different currencies
 * 
 * 3. EDUCATIONAL SYSTEMS:
 *    - Grade calculation for students
 *    - GPA computation and academic performance tracking
 * 
 * 4. NAVIGATION AND MAPPING:
 *    - Distance conversion between metric and imperial units
 *    - GPS coordinates and distance calculations
 * 
 * 5. HEALTH AND FITNESS:
 *    - Weight tracking and conversion
 *    - BMI calculations and health assessments
 * 
 * 6. SHIPPING AND LOGISTICS:
 *    - Weight conversion for shipping costs
 *    - Distance calculation for delivery estimates
 * 
 * =========================================================================================
 */
