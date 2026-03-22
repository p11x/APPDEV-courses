/*
 * SUB TOPIC: Double Array Operations
 * 
 * DEFINITION:
 * A double array stores decimal values with precision of about 15 digits. It uses 8 bytes of memory
 * per element and is the default choice for decimal calculations. Used for financial data, scientific
 * calculations, and precise measurements.
 * 
 * FUNCTIONALITIES:
 * 1. Double array declaration and initialization
 * 2. Double range and precision
 * 3. Type conversions
 * 4. Mathematical operations
 * 5. Scientific calculations
 */

public class Example25 {
    public static void main(String[] args) {
        
        // Topic Explanation with Code: Double Array
        System.out.println("=== Double Array ===");
        
        double[] numbers = new double[5]; // Default value is 0.0
        numbers[0] = 10.5;
        numbers[1] = 20.3;
        numbers[2] = 30.7;
        numbers[3] = 40.1;
        numbers[4] = 50.9;
        
        for (int i = 0; i < numbers.length; i++) {
            System.out.println("numbers[" + i + "] = " + numbers[i]);
        }
        
        // Real-time Example 1: Double Range
        System.out.println("\n=== Double Range ===");
        
        double minValue = Double.MIN_VALUE;
        double maxValue = Double.MAX_VALUE;
        
        System.out.println("Minimum: " + minValue);
        System.out.println("Maximum: " + maxValue);
        System.out.println("Precision: ~15 decimal digits");
        
        // Real-time Example 2: Bank Account Balances
        System.out.println("\n=== Bank Account Balances ===");
        
        double[] balances = {1500.50, 2300.75, 500.25, 7500.00, 99.99};
        
        double total = 0;
        for (double balance : balances) {
            total += balance;
        }
        
        System.out.println("Total Balance: $" + String.format("%.2f", total));
        
        // Real-time Example 3: Circle Calculations
        System.out.println("\n=== Circle Calculations ===");
        
        double[] radii = {1.0, 2.5, 5.0, 10.0};
        
        for (double radius : radii) {
            double area = Math.PI * radius * radius;
            double circumference = 2 * Math.PI * radius;
            System.out.println("Radius: " + radius + " | Area: " + String.format("%.2f", area) + " | Circumference: " + String.format("%.2f", circumference));
        }
        
        // Real-time Example 4: Distance Formula
        System.out.println("\n=== Distance Formula ===");
        
        double[][] points = {{0, 0, 3, 4}, {1, 1, 4, 5}, {0, 0, 1, 1}};
        
        for (int i = 0; i < points.length; i++) {
            double x1 = points[i][0];
            double y1 = points[i][1];
            double x2 = points[i][2];
            double y2 = points[i][3];
            
            double distance = Math.sqrt(Math.pow(x2 - x1, 2) + Math.pow(y2 - y1, 2));
            System.out.println("(" + x1 + "," + y1 + ") to (" + x2 + "," + y2 + ") = " + String.format("%.2f", distance));
        }
        
        // Real-time Example 5: Math Functions
        System.out.println("\n=== Math Functions ===");
        
        double[] values = {0, 0.5, 1.0, 2.0, 3.14159};
        
        for (double v : values) {
            double sqrt = Math.sqrt(v);
            double power = Math.pow(v, 2);
            System.out.println("Value: " + v + " | Square Root: " + sqrt + " | Power 2: " + power);
        }
        
        // Real-time Example 6: Type Conversions
        System.out.println("\n=== Type Conversions ===");
        
        double num = 123.456;
        
        int intValue = (int) num; // Double to int (truncates)
        String stringValue = Double.toString(num); // Double to String
        float floatValue = (float) num; // Double to float
        
        System.out.println("Double to int: " + intValue);
        System.out.println("Double to String: " + stringValue);
        System.out.println("Double to float: " + floatValue);
    }
}
