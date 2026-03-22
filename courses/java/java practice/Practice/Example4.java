/*
 * SUB TOPIC: Loop-Based Input Collection
 * 
 * DEFINITION:
 * This example demonstrates reading input in a loop, allowing users to enter multiple
 * values. This is essential for applications that need to process batch data.
 * 
 * FUNCTIONALITIES:
 * 1. For loop-based input collection
 * 2. While loop-based input collection
 * 3. Running sum and average calculations
 * 4. Finding minimum and maximum values
 */

import java.util.Scanner;

public class Example4 {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        
        // Real-time Example 1: Attendance management system
        System.out.println("=== ATTENDANCE MANAGEMENT SYSTEM ===");
        System.out.print("Enter number of days to track: ");
        int numberOfDays = scanner.nextInt();
        
        boolean[] attendance = new boolean[numberOfDays]; // Boolean array for attendance
        
        for (int day = 0; day < numberOfDays; day++) {
            System.out.print("Day " + (day + 1) + " - Was present? (true/false): ");
            attendance[day] = scanner.nextBoolean(); // Read boolean for each day
        }
        
        int presentDays = 0;
        int absentDays = 0;
        
        for (int i = 0; i < numberOfDays; i++) {
            if (attendance[i]) {
                presentDays++;
            } else {
                absentDays++;
            }
        }
        
        double attendancePercentage = (presentDays * 100.0) / numberOfDays; // Calculate percentage
        System.out.println("Days Present: " + presentDays);
        System.out.println("Days Absent: " + absentDays);
        System.out.println("Attendance Percentage: " + attendancePercentage + "%");
        
        // Real-time Example 2: Sales recording system
        System.out.println("\n=== SALES RECORDING SYSTEM ===");
        System.out.print("Enter number of products: ");
        int numberOfProducts = scanner.nextInt();
        
        double[] sales = new double[numberOfProducts]; // Double array for sales amounts
        
        for (int i = 0; i < numberOfProducts; i++) {
            System.out.print("Enter sales for product " + (i + 1) + ": $");
            sales[i] = scanner.nextDouble(); // Read sales for each product
        }
        
        double totalSales = 0;
        for (double sale : sales) {
            totalSales += sale; // Calculate total sales
        }
        
        double averageSales = totalSales / numberOfProducts; // Calculate average
        System.out.println("Total Sales: $" + totalSales);
        System.out.println("Average Sales: $" + averageSales);
        
        // Real-time Example 3: Survey application
        System.out.println("\n=== SURVEY APPLICATION ===");
        System.out.print("Enter number of respondents: ");
        int respondents = scanner.nextInt();
        
        int[] responses = new int[respondents]; // Integer array for ratings
        
        for (int i = 0; i < respondents; i++) {
            System.out.print("Respondent " + (i + 1) + " rating (1-5): ");
            int rating = scanner.nextInt();
            
            while (rating < 1 || rating > 5) { // Validate rating
                System.out.print("Invalid! Enter rating between 1-5: ");
                rating = scanner.nextInt();
            }
            
            responses[i] = rating;
        }
        
        int totalRating = 0;
        for (int response : responses) {
            totalRating += response; // Sum all ratings
        }
        
        double averageRating = (double) totalRating / respondents; // Calculate average
        System.out.println("Average Rating: " + averageRating);
        
        // Real-time Example 4: Inventory management
        System.out.println("\n=== INVENTORY MANAGEMENT ===");
        System.out.print("Enter number of items: ");
        int itemCount = scanner.nextInt();
        
        String[] itemNames = new String[itemCount];
        int[] quantities = new int[itemCount];
        double[] prices = new double[itemCount];
        
        for (int i = 0; i < itemCount; i++) {
            scanner.nextLine();
            System.out.print("Enter name of item " + (i + 1) + ": ");
            itemNames[i] = scanner.nextLine();
            System.out.print("Enter quantity: ");
            quantities[i] = scanner.nextInt();
            System.out.print("Enter unit price: $");
            prices[i] = scanner.nextDouble();
        }
        
        double totalInventoryValue = 0;
        for (int i = 0; i < itemCount; i++) {
            totalInventoryValue += quantities[i] * prices[i]; // Calculate total value
        }
        
        System.out.println("Total Inventory Value: $" + totalInventoryValue);
        
        // Real-time Example 5: Grade recording
        System.out.println("\n=== GRADE RECORDING ===");
        System.out.print("Enter number of subjects: ");
        int numSubjects = scanner.nextInt();
        
        String[] subjects = new String[numSubjects];
        int[] marks = new int[numSubjects];
        
        for (int i = 0; i < numSubjects; i++) {
            scanner.nextLine();
            System.out.print("Enter subject name: ");
            subjects[i] = scanner.nextLine();
            System.out.print("Enter marks for " + subjects[i] + ": ");
            marks[i] = scanner.nextInt();
        }
        
        int totalMarks = 0;
        for (int mark : marks) {
            totalMarks += mark; // Sum all marks
        }
        
        double averageMarks = (double) totalMarks / numSubjects; // Calculate average
        System.out.println("Total Marks: " + totalMarks);
        System.out.println("Average Marks: " + averageMarks);
        
        scanner.close();
    }
}
