/*
 * =========================================================================================
 * SUB TOPIC: Basic Input/Output using Scanner Class
 * =========================================================================================
 * 
 * DEFINITION:
 * -----------
 * This example demonstrates reading input in a loop, allowing users to enter multiple
 * values. This is essential for applications that need to process batch data, collect
 * survey responses, or perform repeated operations. Loop-based input is fundamental
 * in menu-driven applications and data processing systems.
 * 
 * FUNCTIONALITIES:
 * ----------------
 * 1. For loop-based input collection
 * 2. While loop-based input collection
 * 3. Menu-driven input systems
 * 4. Running sum and average calculations
 * 5. Finding minimum and maximum values from user input
 * 
 * =========================================================================================
 */

import java.util.Scanner; // Import Scanner class for input handling

/**
 * Example4: Loop-Based Input Collection
 * 
 * Real-time Examples:
 * 1. Attendance management system - collecting daily attendance for multiple days
 * 2. Sales recording system - recording sales for multiple products
 * 3. Survey application - collecting responses from multiple participants
 * 4. Inventory management - recording stock levels for multiple items
 * 5. Grade recording - entering marks for multiple subjects
 */
public class Example4 {
    // Main method - entry point of the Java program
    public static void main(String[] args) {
        // Create Scanner object to read input from keyboard
        Scanner scanner = new Scanner(System.in);
        
        // ========================================================================
        // EXAMPLE 1: Attendance Management System
        // ========================================================================
        // Real-world use: Employee attendance, school attendance tracking
        
        System.out.println("=== ATTENDANCE MANAGEMENT SYSTEM ===");
        
        // Prompt for number of days to track
        System.out.print("Enter number of days to track: ");
        int numberOfDays = scanner.nextInt(); // Read the number of days
        
        // Array to store attendance (true = present, false = absent)
        boolean[] attendance = new boolean[numberOfDays]; // Boolean array initialization
        
        // Loop to collect attendance for each day
        for (int day = 0; day < numberOfDays; day++) {
            // Display current day number (1-indexed for user-friendliness)
            System.out.print("Day " + (day + 1) + " - Was present? (true/false): ");
            // Read boolean input from user
            attendance[day] = scanner.nextBoolean(); 
        }
        
        // Count present and absent days
        int presentDays = 0; // Counter for present days
        int absentDays = 0; // Counter for absent days
        
        // Loop through attendance array to count
        for (int i = 0; i < numberOfDays; i++) {
            // If attendance is true, increment presentDays
            if (attendance[i]) {
                presentDays++; // Increment present count
            } else {
                absentDays++; // Increment absent count
            }
        }
        
        // Display attendance summary
        System.out.println("\n--- Attendance Summary ---");
        System.out.println("Total Days: " + numberOfDays);
        System.out.println("Days Present: " + presentDays);
        System.out.println("Days Absent: " + absentDays);
        
        // Calculate attendance percentage
        double attendancePercentage = (presentDays * 100.0) / numberOfDays;
        System.out.println("Attendance Percentage: " + attendancePercentage + "%");
        
        // ========================================================================
        // EXAMPLE 2: Sales Recording System
        // ========================================================================
        // Real-world use: Retail stores, e-commerce, sales departments
        
        System.out.println("\n=== SALES RECORDING SYSTEM ===");
        
        // Prompt for number of products
        System.out.print("Enter number of products: ");
        int numberOfProducts = scanner.nextInt(); // Read number of products
        
        // Array to store sales amounts
        double[] sales = new double[numberOfProducts]; // Double array for currency
        
        // Loop to collect sales for each product
        for (int i = 0; i < numberOfProducts; i++) {
            // Prompt for product number (1-indexed)
            System.out.print("Enter sales for product " + (i + 1) + ": $");
            // Read the sales amount as double
            sales[i] = scanner.nextDouble(); 
        }
        
        // Calculate total sales using a for-each loop
        double totalSales = 0; // Initialize total to zero
        // Enhanced for-loop (for-each) to iterate through array
        for (double sale : sales) {
            totalSales += sale; // Add each sale to total
        }
        
        // Calculate average sales
        double averageSales = totalSales / numberOfProducts;
        
        // Find highest and lowest sales
        double highestSale = sales[0]; // Assume first element is highest
        double lowestSale = sales[0]; // Assume first element is lowest
        
        // Loop to find highest and lowest
        for (int i = 1; i < sales.length; i++) {
            // Compare current element with highest
            if (sales[i] > highestSale) {
                highestSale = sales[i]; // Update highest if current is greater
            }
            // Compare current element with lowest
            if (sales[i] < lowestSale) {
                lowestSale = sales[i]; // Update lowest if current is smaller
            }
        }
        
        // Display sales summary
        System.out.println("\n--- Sales Summary ---");
        System.out.println("Total Sales: $" + totalSales);
        System.out.println("Average Sales: $" + averageSales);
        System.out.println("Highest Sale: $" + highestSale);
        System.out.println("Lowest Sale: $" + lowestSale);
        
        // ========================================================================
        // EXAMPLE 3: Survey Application
        // ========================================================================
        // Real-world use: Market research, customer feedback, polls
        
        System.out.println("\n=== SURVEY APPLICATION ===");
        
        // Prompt for number of respondents
        System.out.print("Enter number of respondents: ");
        int respondents = scanner.nextInt(); // Read number of respondents
        
        // Array to store responses (1-5 scale)
        int[] responses = new int[respondents]; // Integer array for ratings
        
        // Loop to collect responses
        for (int i = 0; i < respondents; i++) {
            // Prompt for rating with validation
            System.out.print("Respondent " + (i + 1) + " rating (1-5): ");
            int rating = scanner.nextInt(); // Read rating
            
            // Validate rating (ensure it's between 1 and 5)
            while (rating < 1 || rating > 5) {
                // If invalid, ask again
                System.out.print("Invalid! Enter rating between 1-5: ");
                rating = scanner.nextInt(); // Read valid rating
            }
            
            // Store the validated rating
            responses[i] = rating; 
        }
        
        // Calculate statistics
        int totalRating = 0; // Sum of all ratings
        int[] ratingCounts = new int[5]; // Count for each rating (1-5)
        
        // Loop through responses
        for (int response : responses) {
            // Add to total
            totalRating += response; 
            // Count this rating (subtract 1 for 0-based index)
            ratingCounts[response - 1]++; 
        }
        
        // Calculate average
        double averageRating = (double) totalRating / respondents;
        
        // Find most common rating
        int maxCount = 0; // Track maximum count
        int mostCommonRating = 1; // Default to rating 1
        
        for (int i = 0; i < ratingCounts.length; i++) {
            // Check if current rating count is greater than max
            if (ratingCounts[i] > maxCount) {
                maxCount = ratingCounts[i]; // Update max count
                mostCommonRating = i + 1; // Update most common rating (add 1 for 1-based)
            }
        }
        
        // Display survey results
        System.out.println("\n--- Survey Results ---");
        System.out.println("Average Rating: " + averageRating);
        System.out.println("Most Common Rating: " + mostCommonRating);
        
        // Display rating distribution
        System.out.println("\nRating Distribution:");
        for (int i = 0; i < ratingCounts.length; i++) {
            // Display count for each rating with visual bar
            System.out.print("Rating " + (i + 1) + ": ");
            // Print stars to represent count
            for (int j = 0; j < ratingCounts[i]; j++) {
                System.out.print("*"); // Print star for each response
            }
            System.out.println(" (" + ratingCounts[i] + ")"); // Show actual count
        }
        
        // ========================================================================
        // EXAMPLE 4: Inventory Management
        // ========================================================================
        // Real-world use: Warehouse management, retail stock control
        
        System.out.println("\n=== INVENTORY MANAGEMENT ===");
        
        // Prompt for number of items
        System.out.print("Enter number of items: ");
        int itemCount = scanner.nextInt(); // Read number of items
        
        // Arrays to store item information
        String[] itemNames = new String[itemCount]; // Item names
        int[] quantities = new int[itemCount]; // Stock quantities
        double[] prices = new double[itemCount]; // Unit prices
        
        // Loop to collect item details
        for (int i = 0; i < itemCount; i++) {
            // Clear the newline buffer before reading string
            scanner.nextLine(); 
            
            // Get item name
            System.out.print("Enter name of item " + (i + 1) + ": ");
            itemNames[i] = scanner.nextLine(); // Read full line with spaces
            
            // Get quantity
            System.out.print("Enter quantity for " + itemNames[i] + ": ");
            quantities[i] = scanner.nextInt(); // Read integer quantity
            
            // Get price
            System.out.print("Enter unit price for " + itemNames[i] + ": $");
            prices[i] = scanner.nextDouble(); // Read double price
        }
        
        // Calculate total inventory value
        double totalInventoryValue = 0; // Initialize total
        
        // Loop to calculate total value
        for (int i = 0; i < itemCount; i++) {
            // Add (quantity * price) to total
            totalInventoryValue += quantities[i] * prices[i];
        }
        
        // Display inventory
        System.out.println("\n--- Inventory Details ---");
        System.out.println("Item Name\t\tQuantity\tPrice\t\tTotal Value");
        
        // Loop to display each item
        for (int i = 0; i < itemCount; i++) {
            // Calculate total value for this item
            double itemTotal = quantities[i] * prices[i];
            // Display item details in formatted table
            System.out.println(itemNames[i] + "\t\t" + quantities[i] + "\t\t$" + prices[i] + "\t\t$" + itemTotal);
        }
        
        System.out.println("------------------------------------------------");
        System.out.println("Total Inventory Value: $" + totalInventoryValue);
        
        // Close the scanner
        scanner.close();
        
        System.out.println("\n=== DATA COLLECTION COMPLETE ===");
    }
}

/*
 * STEP-BY-STEP EXPLANATION:
 * -------------------------
 * 
 * Step 1: Import Required Package
 *    - import java.util.Scanner; imports the Scanner class
 *    - Scanner is essential for reading user input in Java
 * 
 * Step 2: Create Scanner Object
 *    - new Scanner(System.in) creates input stream from keyboard
 *    - System.in represents standard input device
 * 
 * Step 3: Determine Number of Entries
 *    - Ask user how many items to process
 *    - Create appropriate array size based on input
 * 
 * Step 4: Loop for Input Collection
 *    - Use for loop when number of iterations is known
 *    - Use while loop for condition-based iteration
 *    - Use enhanced for-loop (for-each) for reading array elements
 * 
 * Step 5: Validate Input
 *    - Check if input meets required criteria
 *    - Use while loops for input validation
 *    - Provide clear error messages
 * 
 * Step 6: Process Collected Data
 *    - Calculate sums, averages, min, max
 *    - Perform statistical analysis
 *    - Generate reports
 * 
 * Step 7: Display Results
 *    - Use formatted output for readability
 *    - Include labels and units
 *    - Show summaries and totals
 * 
 * Step 8: Clean Up Resources
 *    - Always close Scanner after use
 *    - Prevents resource leaks
 * 
 * =========================================================================================
 * REAL-TIME USE CASES:
 * =========================================================================================
 * 
 * 1. ATTENDANCE SYSTEMS:
 *    - Schools track student attendance daily
 *    - Companies monitor employee attendance
 *    - Calculate attendance percentages for performance
 * 
 * 2. SALES TRACKING:
 *    - Record daily sales for multiple products
 *    - Analyze sales trends over time
 *    - Calculate commissions and bonuses
 * 
 * 3. SURVEY AND POLLS:
 *    - Collect customer feedback
 *    - Analyze market research data
 *    - Generate statistical reports
 * 
 * 4. INVENTORY MANAGEMENT:
 *    - Track stock levels for products
 *    - Calculate inventory values
 *    - Reorder point calculations
 * 
 * 5. EDUCATIONAL SYSTEMS:
 *    - Record student grades across subjects
 *    - Calculate GPA and CGPA
 *    - Generate academic reports
 * 
 * =========================================================================================
 */
