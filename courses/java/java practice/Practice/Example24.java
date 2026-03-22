/*
 * SUB TOPIC: Float Array Operations
 * 
 * DEFINITION:
 * A float array stores decimal values with precision of about 7 digits. It uses 4 bytes of memory
 * per element, making it more memory efficient than double. Used for prices, measurements, and
 * scientific calculations.
 * 
 * FUNCTIONALITIES:
 * 1. Float array declaration and initialization
 * 2. Float range and precision
 * 3. Type conversions
 * 4. Mathematical operations
 * 5. Rounding and comparison
 */

public class Example24 {
    public static void main(String[] args) {
        
        // Topic Explanation with Code: Float Array
        System.out.println("=== Float Array ===");
        
        float[] numbers = new float[5]; // Default value is 0.0f
        numbers[0] = 10.5f;
        numbers[1] = 20.3f;
        numbers[2] = 30.7f;
        numbers[3] = 40.1f;
        numbers[4] = 50.9f;
        
        for (int i = 0; i < numbers.length; i++) {
            System.out.println("numbers[" + i + "] = " + numbers[i]);
        }
        
        // Real-time Example 1: Float Range
        System.out.println("\n=== Float Range ===");
        
        float minValue = Float.MIN_VALUE;
        float maxValue = Float.MAX_VALUE;
        
        System.out.println("Minimum: " + minValue);
        System.out.println("Maximum: " + maxValue);
        
        // Real-time Example 2: Student GPAs
        System.out.println("\n=== Student GPAs ===");
        
        float[] gpas = {3.5f, 3.8f, 2.9f, 4.0f, 3.2f};
        String[] students = {"Alice", "Bob", "Charlie", "Diana", "Eve"};
        
        float totalGPA = 0;
        for (float gpa : gpas) {
            totalGPA += gpa;
        }
        
        for (int i = 0; i < students.length; i++) {
            System.out.println(students[i] + ": " + gpas[i]);
        }
        
        System.out.println("Average GPA: " + (totalGPA / gpas.length));
        
        // Real-time Example 3: Product Prices
        System.out.println("\n=== Product Prices ===");
        
        float[] prices = {29.99f, 149.99f, 9.99f, 199.99f};
        
        float total = 0;
        for (float price : prices) {
            total += price;
        }
        
        System.out.println("Total: $" + total);
        
        // Real-time Example 4: Temperature Conversion
        System.out.println("\n=== Celsius to Fahrenheit ===");
        
        float[] celsius = {0f, 20f, 37f, 100f};
        
        for (float c : celsius) {
            float fahrenheit = (c * 9/5) + 32;
            System.out.println(c + "C = " + fahrenheit + "F");
        }
        
        // Real-time Example 5: Rounding
        System.out.println("\n=== Rounding ===");
        
        float value = 3.14159f;
        float rounded = Math.round(value * 100) / 100f;
        System.out.println("Original: " + value);
        System.out.println("Rounded: " + rounded);
        
        // Real-time Example 6: Comparing Floats
        System.out.println("\n=== Comparing Floats ===");
        
        float a = 0.1f;
        float b = 0.2f;
        float sum = a + b;
        
        float tolerance = 0.0001f;
        boolean isEqual = Math.abs(sum - 0.3f) < tolerance;
        System.out.println("0.1 + 0.2 = " + sum);
        System.out.println("Equal with tolerance: " + isEqual);
    }
}
