// Example25: Array of Doubles - Beginner Tutorial
// This shows different ways to work with double arrays

public class Example25 {
    public static void main(String[] args) {
        
        // ===== METHOD 1: Create double array with size =====
        System.out.println("=== Method 1: Create with size ===");
        
        double[] numbers = new double[5];
        
        // Default value is 0.0
        numbers[0] = 10.5;
        numbers[1] = 20.3;
        numbers[2] = 30.7;
        numbers[3] = 40.1;
        numbers[4] = 50.9;
        
        System.out.println("Double values:");
        for (int i = 0; i < numbers.length; i++) {
            System.out.println("  Index " + i + ": " + numbers[i]);
        }
        
        // ===== METHOD 2: Direct initialization =====
        System.out.println("\n=== Method 2: Direct initialization ===");
        
        double[] prices = {19.99, 29.99, 9.99, 49.99, 99.99};
        
        System.out.print("Prices: ");
        for (double p : prices) {
            System.out.print(p + " ");
        }
        System.out.println();
        
        // ===== DOUBLE RANGE AND PRECISION =====
        System.out.println("\n=== Double Range and Precision ===");
        
        // Double range: ±1.8e308 (huge!)
        // Precision: about 15 decimal digits
        
        double minValue = Double.MIN_VALUE;
        double maxValue = Double.MAX_VALUE;
        
        System.out.println("Minimum double: " + minValue);
        System.out.println("Maximum double: " + maxValue);
        System.out.println("Precision: ~15 decimal digits");
        
        // ===== PRACTICAL: Bank Account Balances =====
        System.out.println("\n=== Practical: Bank Account Balances ===");
        
        // Perfect for money - more precise than float
        double[] balances = {1500.50, 2300.75, 500.25, 7500.00, 99.99};
        
        System.out.println("Account balances:");
        for (double balance : balances) {
            System.out.println("  $" + String.format("%.2f", balance));
        }
        
        // Calculate total and average
        double total = 0;
        for (double balance : balances) {
            total += balance;
        }
        
        System.out.println("\nTotal: $" + String.format("%.2f", total));
        System.out.println("Average: $" + String.format("%.2f", total / balances.length));
        
        // Interest calculation (5%)
        System.out.println("\nAfter 5% interest:");
        for (double balance : balances) {
            double interest = balance * 0.05;
            double newBalance = balance + interest;
            System.out.println("  $" + String.format("%.2f", balance) + 
                             " -> $" + String.format("%.2f", newBalance));
        }
        
        // ===== PRACTICAL: Scientific Calculations =====
        System.out.println("\n=== Practical: Scientific Calculations ===");
        
        double[] measurements = {1.234567890, 2.345678901, 3.456789012};
        
        System.out.println("Precise measurements:");
        for (double m : measurements) {
            System.out.println("  " + m);
        }
        
        // Calculate mean
        double sum = 0;
        for (double m : measurements) {
            sum += m;
        }
        double mean = sum / measurements.length;
        
        System.out.println("Mean: " + mean);
        
        // ===== PRACTICAL: Circle Calculations =====
        System.out.println("\n=== Practical: Circle Calculations ===");
        
        double[] radii = {1.0, 2.5, 5.0, 10.0};
        
        System.out.println("Radius | Area | Circumference");
        System.out.println("-------|------|--------------");
        
        for (double radius : radii) {
            double area = Math.PI * radius * radius;
            double circumference = 2 * Math.PI * radius;
            
            System.out.println(radius + "     | " + 
                             String.format("%.2f", area) + " | " + 
                             String.format("%.2f", circumference));
        }
        
        // ===== PRACTICAL: Distance Calculations =====
        System.out.println("\n=== Practical: Distance Formula ===");
        
        // Points: (x1, y1) to (x2, y2)
        double[][] points = {
            {0, 0, 3, 4},    // Distance = 5
            {1, 1, 4, 5},    // Distance = 5
            {0, 0, 1, 1},   // Distance = sqrt(2)
            {2, 3, 6, 8}    // Distance = sqrt(41) ≈ 6.4
        };
        
        System.out.println("Point pairs and distances:");
        for (int i = 0; i < points.length; i++) {
            double x1 = points[i][0];
            double y1 = points[i][1];
            double x2 = points[i][2];
            double y2 = points[i][3];
            
            double distance = Math.sqrt(Math.pow(x2 - x1, 2) + Math.pow(y2 - y1, 2));
            
            System.out.println("(" + x1 + "," + y1 + ") to (" + x2 + "," + y2 + 
                             ") = " + String.format("%.2f", distance));
        }
        
        // ===== ROUNDING DOUBLES =====
        System.out.println("\n=== Rounding Doubles ===");
        
        double value1 = 3.14159265359;
        double value2 = 99.99999999;
        
        // Round to different decimal places
        System.out.println("Original: " + value1);
        System.out.println("Rounded to 2: " + String.format("%.2f", value1));
        System.out.println("Rounded to 4: " + String.format("%.4f", value1));
        System.out.println("Rounded to 6: " + String.format("%.6f", value1));
        
        // Using Math.round
        System.out.println("\nUsing Math.round:");
        System.out.println(value1 + " -> " + Math.round(value1 * 100) / 100.0);
        
        // ===== MATH FUNCTIONS =====
        System.out.println("\n=== Math Functions with Arrays ===");
        
        double[] values = {0, 0.5, 1.0, 2.0, 3.14159};
        
        System.out.println("Value | SquareRoot | Power(2) | Sin");
        System.out.println("------|-------------|----------|-----");
        
        for (double v : values) {
            double sqrt = Math.sqrt(v);
            double power = Math.pow(v, 2);
            double sin = Math.sin(v);
            
            System.out.println(v + "   | " + 
                             String.format("%.2f", sqrt) + "      | " + 
                             String.format("%.2f", power) + "     | " + 
                             String.format("%.2f", sin));
        }
        
        // ===== CONVERT DOUBLE TO OTHER TYPES =====
        System.out.println("\n=== Convert Double to Other Types ===");
        
        double num = 123.456;
        
        // Double to int (truncates decimal)
        int intValue = (int) num;
        System.out.println("Double to int: " + num + " -> " + intValue);
        
        // Double to String
        String stringValue = Double.toString(num);
        System.out.println("Double to String: " + num + " -> \"" + stringValue + "\"");
        
        // Double to float (narrowing)
        float floatValue = (float) num;
        System.out.println("Double to float: " + num + " -> " + floatValue);
        
        // ===== CONVERT TO DOUBLE =====
        System.out.println("\n=== Convert to Double ===");
        
        // String to double
        String strNum = "99.999";
        double doubleFromString = Double.parseDouble(strNum);
        System.out.println("String to double: \"" + strNum + "\" -> " + doubleFromString);
        
        // Int to double
        int intNum = 100;
        double doubleFromInt = intNum;
        System.out.println("Int to double: " + intNum + " -> " + doubleFromInt);
        
        // ===== WHY USE DOUBLE? =====
        System.out.println("\n=== Why Use Double? ===");
        
        // Double is better than float for most cases
        System.out.println("Double is used for:");
        System.out.println("  - Money and financial calculations");
        System.out.println("  - Scientific calculations");
        System.out.println("  - Precise measurements");
        System.out.println("  - Any calculation needing 15 digits");
        
        // Precision comparison
        System.out.println("\n=== Precision Comparison ===");
        
        float floatPi = 3.14159265358979f;
        double doublePi = 3.14159265358979;
        
        System.out.println("Float Pi: " + floatPi);
        System.out.println("Double Pi: " + doublePi);
        System.out.println("True Pi: 3.141592653589793...");
    }
}

/*
 * KEY CONCEPTS FOR BEGINNERS:
 * 
 * 1. DECLARING DOUBLE ARRAYS:
 *    double[] array = new double[size];
 *    double[] array = {1.5, 2.5, 3.5};
 * 
 * 2. DOUBLE RANGE AND PRECISION:
 *    - Range: ±1.8e308 (huge!)
 *    - Precision: ~15 decimal digits
 *    - Takes 8 bytes (64 bits)
 *    - No suffix needed
 * 
 * 3. WHY USE DOUBLE?
 *    - More precise than float (15 vs 7 digits)
 *    - Default for decimal numbers
 *    - Perfect for:
 *      * Money (use BigDecimal for perfect precision)
 *      * Scientific calculations
 *      * Geometric calculations
 *      * Any precise measurement
 * 
 * 4. IMPORTANT NOTES:
 *    - Default value is 0.0
 *    - More accurate than float
 *    - Don't compare doubles with ==
 *    - Use String.format() for display
 *    - For money, consider BigDecimal
 * 
 * 5. COMMON MATH FUNCTIONS:
 *    - Math.sqrt()    - square root
 *    - Math.pow()     - power
 *    - Math.sin()     - sine
 *    - Math.cos()     - cosine
 *    - Math.PI        - pi constant
 *    - Math.round()   - rounding
 * 
 * 6. TIPS:
 *    - Double is the default choice for decimals
 *    - Use float only for memory-constrained apps
 *    - Always format money for display
 *    - Be careful with floating point comparisons
 */
