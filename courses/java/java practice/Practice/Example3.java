/*
 * SUB TOPIC: Basic Input/Output using Scanner Class
 * 
 * DEFINITION:
 * This example demonstrates reading various data types with proper validation 
 * and error handling in real-world scenarios.
 * 
 * FUNCTIONALITIES:
 * 1. Input validation - checking if input is valid before processing
 * 2. Exception handling - catching input mismatches
 * 3. Type conversion - converting between different data types
 */

import java.util.Scanner;

public class Example3 {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        
        // Real-time Example 1: Temperature conversion system
        System.out.println("=== TEMPERATURE CONVERTER ===");
        System.out.print("Enter temperature in Celsius: ");
        
        if (scanner.hasNextInt()) {
            int celsius = scanner.nextInt(); // Read integer value
            double fahrenheit = (celsius * 9.0 / 5.0) + 32; // Formula: F = (C * 9/5) + 32
            double kelvin = celsius + 273.15; // Formula: K = C + 273.15
            
            System.out.println("Temperature in Fahrenheit: " + fahrenheit + "°F");
            System.out.println("Temperature in Kelvin: " + kelvin + "K");
        } else {
            System.out.println("Invalid input! Please enter a valid integer.");
            scanner.next();
        }
        
        // Real-time Example 2: Currency converter
        System.out.println("\n=== CURRENCY CONVERTER ===");
        double usdToInr = 83.0;
        double usdToEur = 0.92;
        double usdToGbp = 0.79;
        
        System.out.print("Enter amount in USD: ");
        
        if (scanner.hasNextDouble()) {
            double usdAmount = scanner.nextDouble(); // Read double value
            double inrAmount = usdAmount * usdToInr; // Convert to INR
            double eurAmount = usdAmount * usdToEur; // Convert to EUR
            double gbpAmount = usdAmount * usdToGbp; // Convert to GBP
            
            System.out.println("USD " + usdAmount + " = INR " + inrAmount);
            System.out.println("USD " + usdAmount + " = EUR " + eurAmount);
            System.out.println("USD " + usdAmount + " = GBP " + gbpAmount);
        }
        
        // Real-time Example 3: Grade calculator
        System.out.println("\n=== GRADE CALCULATOR ===");
        System.out.print("Enter marks obtained (0-100): ");
        
        int marks = scanner.nextInt(); // Read marks
        String grade;
        
        if (marks >= 90 && marks <= 100) {
            grade = "A";
        } else if (marks >= 80) {
            grade = "B";
        } else if (marks >= 70) {
            grade = "C";
        } else if (marks >= 60) {
            grade = "D";
        } else if (marks >= 0) {
            grade = "F";
        } else {
            grade = "Invalid";
            System.out.println("Error: Marks cannot be negative!");
        }
        
        System.out.println("Grade: " + grade);
        
        String division;
        if (marks >= 60) {
            division = "First Division";
        } else if (marks >= 45) {
            division = "Second Division";
        } else if (marks >= 33) {
            division = "Third Division";
        } else {
            division = "Fail";
        }
        
        System.out.println("Division: " + division);
        
        // Real-time Example 4: Distance converter
        System.out.println("\n=== DISTANCE CONVERTER ===");
        double kmToMiles = 0.621371;
        double kmToMeters = 1000.0;
        
        System.out.print("Enter distance in kilometers: ");
        double km = scanner.nextDouble(); // Read distance
        
        double miles = km * kmToMiles; // Convert to miles
        double meters = km * kmToMeters; // Convert to meters
        
        System.out.println(km + " km = " + miles + " miles");
        System.out.println(km + " km = " + meters + " meters");
        
        // Real-time Example 5: Weight converter
        System.out.println("\n=== WEIGHT CONVERTER ===");
        double kgToPounds = 2.20462;
        double kgToGrams = 1000.0;
        
        System.out.print("Enter weight in kilograms: ");
        double weightKg = scanner.nextDouble(); // Read weight
        
        double pounds = weightKg * kgToPounds; // Convert to pounds
        double grams = weightKg * kgToGrams; // Convert to grams
        
        System.out.println(weightKg + " kg = " + pounds + " pounds");
        System.out.println(weightKg + " kg = " + grams + " grams");
        
        scanner.close();
    }
}
