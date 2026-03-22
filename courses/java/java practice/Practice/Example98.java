/*
 * SUB TOPIC: DecimalFormat in Java
 * 
 * DEFINITION:
 * DecimalFormat is a concrete subclass of NumberFormat that formats decimal numbers. 
 * It allows formatting of numbers with specific patterns including decimal places, 
 * thousands separators, and various number notations. It uses pattern strings to 
 * control output format.
 * 
 * FUNCTIONALITIES:
 * 1. format() - Format a number to string
 * 2. parse() - Parse string to number
 * 3. setMinimumFractionDigits() / setMaximumFractionDigits() - Control decimal places
 * 4. setMinimumIntegerDigits() / setMaximumIntegerDigits() - Control integer digits
 * 5. setGroupingUsed() - Enable/disable thousands separator
 * 6. setRoundingMode() - Set rounding behavior
 */

import java.text.*;
import java.util.*;
import java.math.RoundingMode;

public class Example98 {
    public static void main(String[] args) throws ParseException {
        
        // Basic decimal formatting
        System.out.println("=== Basic DecimalFormat ===");
        
        double number = 1234567.890;
        
        // Using format patterns
        DecimalFormat df1 = new DecimalFormat("#,###.##");
        System.out.println("#,###.## : " + df1.format(number)); // 1,234,567.89
        
        DecimalFormat df2 = new DecimalFormat("000,000.000");
        System.out.println("000,000.000: " + df2.format(number)); // 001,234,567.890
        
        DecimalFormat df3 = new DecimalFormat("###.##");
        System.out.println("###.##    : " + df3.format(99.9)); // 99.9
        
        // Currency formatting
        System.out.println("\n=== Currency Format ===");
        double price = 1999.99;
        
        DecimalFormat currency = new DecimalFormat("$#,###.00");
        System.out.println("Price: " + currency.format(price)); // $1,999.99
        
        DecimalFormat currency2 = new DecimalFormat("USD #,###.00");
        System.out.println("Price2: " + currency2.format(price)); // USD 1,999.99
        
        // Percentage formatting
        System.out.println("\n=== Percentage Format ===");
        double percentage = 0.7569;
        
        DecimalFormat percent = new DecimalFormat("0.00%");
        System.out.println("Percentage: " + percent.format(percentage)); // 75.69%
        
        DecimalFormat percent2 = new DecimalFormat("#%");
        System.out.println("Percentage2: " + percent2.format(percentage)); // 76%
        
        // Scientific notation
        System.out.println("\n=== Scientific Notation ===");
        double scientific = 123456.789;
        
        DecimalFormat sci = new DecimalFormat("0.###E0");
        System.out.println("Scientific: " + sci.format(scientific)); // 1.235E5
        
        // Real-time Example 1: Financial report
        System.out.println("\n=== Example 1: Financial Report ===");
        double[] revenues = {150000.50, 250000.75, 180000.25, 320000.00};
        String[] quarters = {"Q1", "Q2", "Q3", "Q4"};
        
        DecimalFormat money = new DecimalFormat("$#,##0.00");
        
        System.out.println("Revenue Report:");
        for (int i = 0; i < revenues.length; i++) {
            System.out.println(quarters[i] + ": " + money.format(revenues[i]));
        }
        
        double total = 0;
        for (double r : revenues) total += r;
        System.out.println("Total: " + money.format(total));
        
        // Real-time Example 2: Grade calculator
        System.out.println("\n=== Example 2: Grade Calculator ===");
        double[] scores = {95.678, 87.234, 92.5, 100.0, 76.891};
        
        DecimalFormat gradeFormat = new DecimalFormat("0.00");
        
        System.out.println("Student Scores:");
        for (int i = 0; i < scores.length; i++) {
            System.out.println("Student " + (i+1) + ": " + gradeFormat.format(scores[i]));
        }
        
        double avg = Arrays.stream(scores).average().orElse(0);
        System.out.println("Average: " + gradeFormat.format(avg));
        
        // Real-time Example 3: Inventory quantities
        System.out.println("\n=== Example 3: Inventory Quantities ===");
        int[] quantities = {1000, 5000, 15000, 100000, 500};
        
        DecimalFormat qty = new DecimalFormat("#,### units");
        
        System.out.println("Inventory:");
        for (int i = 0; i < quantities.length; i++) {
            System.out.println("Product " + (i+1) + ": " + qty.format(quantities[i]));
        }
        
        // Real-time Example 4: Scientific measurements
        System.out.println("\n=== Example 4: Scientific Data ===");
        double[] measurements = {0.0012345, 12.34567, 123.4567, 1234.567};
        
        DecimalFormat sciFormat = new DecimalFormat("0.0000E0");
        
        System.out.println("Measurements:");
        for (double m : measurements) {
            System.out.println(sciFormat.format(m));
        }
        
        // Real-time Example 5: Population statistics
        System.out.println("\n=== Example 5: Population Stats ===");
        long[] populations = {331000000L, 1440000000L, 1380000000L, 212000000L};
        String[] countries = {"USA", "China", "India", "Brazil"};
        
        DecimalFormat pop = new DecimalFormat("#,###");
        
        System.out.println("Country Populations:");
        for (int i = 0; i < populations.length; i++) {
            System.out.println(countries[i] + ": " + pop.format(populations[i]));
        }
        
        // Real-time Example 6: Discount calculations
        System.out.println("\n=== Example 6: Discount Calculator ===");
        double[] prices = {99.99, 149.99, 199.99, 299.99};
        double discountRate = 0.15; // 15% off
        
        DecimalFormat priceFormat = new DecimalFormat("$##0.00");
        DecimalFormat discountFormat = new DecimalFormat("0%");
        
        System.out.println("Discount Prices:");
        for (double p : prices) {
            double discountAmount = p * discountRate;
            double finalPrice = p - discountAmount;
            System.out.println("Original: " + priceFormat.format(p) + 
                             " -> Discount: " + discountFormat.format(discountRate) +
                             " -> Final: " + priceFormat.format(finalPrice));
        }
        
        // Additional operations
        System.out.println("\n=== Additional Operations ===");
        
        // Controlling decimal places
        DecimalFormat df = new DecimalFormat("###.##");
        df.setMinimumFractionDigits(2);
        df.setMaximumFractionDigits(4);
        System.out.println("Min/Max decimals: " + df.format(12.5)); // 12.50
        System.out.println("Max decimals: " + df.format(12.123456)); // 12.1235
        
        // Grouping control
        DecimalFormat noGroup = new DecimalFormat("####.##");
        noGroup.setGroupingUsed(false);
        System.out.println("No grouping: " + noGroup.format(1234567.89)); // 1234567.89
        
        // Leading zeros
        DecimalFormat leadZeros = new DecimalFormat("0000.00");
        System.out.println("Leading zeros: " + leadZeros.format(5.5)); // 0005.50
        
        // Parsing strings to numbers
        System.out.println("\n=== Parsing ===");
        String strNum = "1,234.56";
        DecimalFormat parseFormat = new DecimalFormat("#,###.##");
        Number parsed = parseFormat.parse(strNum);
        System.out.println("Parsed: " + parsed.doubleValue() * 2); // 2469.12
        
        // Rounding modes
        System.out.println("\n=== Rounding Modes ===");
        double roundTest = 12.345;
        
        DecimalFormat roundUp = new DecimalFormat("0.0");
        roundUp.setRoundingMode(RoundingMode.UP);
        System.out.println("Round UP: " + roundUp.format(roundTest)); // 12.4
        
        DecimalFormat roundDown = new DecimalFormat("0.0");
        roundDown.setRoundingMode(RoundingMode.DOWN);
        System.out.println("Round DOWN: " + roundDown.format(roundTest)); // 12.3
        
        DecimalFormat roundHalfUp = new DecimalFormat("0.0");
        roundHalfUp.setRoundingMode(RoundingMode.HALF_UP);
        System.out.println("Half UP: " + roundHalfUp.format(roundTest)); // 12.3
        
        DecimalFormat roundHalf2 = new DecimalFormat("0.0");
        roundHalf2.setRoundingMode(RoundingMode.HALF_UP);
        System.out.println("Half UP 12.35: " + roundHalf2.format(12.35)); // 12.4
    }
}
