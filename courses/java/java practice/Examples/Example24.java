// Example24: Array of Floats - Beginner Tutorial
// This shows different ways to work with float arrays

public class Example24 {
    public static void main(String[] args) {
        
        // ===== METHOD 1: Create float array with size =====
        System.out.println("=== Method 1: Create with size ===");
        
        float[] numbers = new float[5];
        
        // Default value is 0.0f
        numbers[0] = 10.5f;
        numbers[1] = 20.3f;
        numbers[2] = 30.7f;
        numbers[3] = 40.1f;
        numbers[4] = 50.9f;
        
        System.out.println("Float values:");
        for (int i = 0; i < numbers.length; i++) {
            System.out.println("  Index " + i + ": " + numbers[i]);
        }
        
        // ===== METHOD 2: Direct initialization =====
        System.out.println("\n=== Method 2: Direct initialization ===");
        
        // Note: Float values need 'f' or 'F' suffix
        float[] prices = {19.99f, 29.99f, 9.99f, 49.99f, 99.99f};
        
        System.out.print("Prices: ");
        for (float p : prices) {
            System.out.print(p + " ");
        }
        System.out.println();
        
        // ===== FLOAT RANGE AND PRECISION =====
        System.out.println("\n=== Float Range and Precision ===");
        
        // Float range: ±3.4e38 (approx)
        // Precision: about 7 decimal digits
        
        float minValue = Float.MIN_VALUE;
        float maxValue = Float.MAX_VALUE;
        
        System.out.println("Minimum float: " + minValue);
        System.out.println("Maximum float: " + maxValue);
        
        // ===== PRACTICAL: Student Grades =====
        System.out.println("\n=== Practical: Student Grades (GPAs) ===");
        
        float[] gpas = {3.5f, 3.8f, 2.9f, 4.0f, 3.2f};
        String[] students = {"Alice", "Bob", "Charlie", "Diana", "Eve"};
        
        System.out.println("Student GPAs:");
        for (int i = 0; i < students.length; i++) {
            System.out.println("  " + students[i] + ": " + gpas[i]);
        }
        
        // Calculate average GPA
        float totalGPA = 0;
        for (float gpa : gpas) {
            totalGPA += gpa;
        }
        
        float averageGPA = totalGPA / gpas.length;
        System.out.println("Class Average GPA: " + averageGPA);
        
        // ===== PRACTICAL: Product Prices =====
        System.out.println("\n=== Practical: Product Prices ===");
        
        float[] productPrices = {29.99f, 149.99f, 9.99f, 199.99f, 49.99f};
        
        System.out.println("Product prices:");
        for (float price : productPrices) {
            System.out.println("  $" + price);
        }
        
        // Calculate total and average
        float total = 0;
        for (float price : productPrices) {
            total += price;
        }
        
        System.out.println("Total: $" + total);
        System.out.println("Average: $" + (total / productPrices.length));
        
        // Apply 10% discount
        System.out.println("\nAfter 10% discount:");
        for (float price : productPrices) {
            float discount = price * 0.9f;
            System.out.println("  Original: $" + price + " -> Sale: $" + discount);
        }
        
        // ===== PRACTICAL: Temperature Conversion =====
        System.out.println("\n=== Practical: Celsius to Fahrenheit ===");
        
        float[] celsius = {0f, 20f, 37f, 100f};
        
        System.out.println("Celsius to Fahrenheit:");
        for (float c : celsius) {
            float fahrenheit = (c * 9/5) + 32;
            System.out.println("  " + c + "°C = " + fahrenheit + "°F");
        }
        
        // ===== PRACTICAL: Measurements =====
        System.out.println("\n=== Practical: Height in Meters ===");
        
        float[] heights = {1.75f, 1.82f, 1.65f, 1.90f, 1.70f};
        
        System.out.println("Heights (in meters):");
        for (float h : heights) {
            System.out.println("  " + h + "m");
        }
        
        // Convert to feet (1 meter = 3.28084 feet)
        System.out.println("\nIn feet:");
        for (float h : heights) {
            float feet = h * 3.28084f;
            System.out.println("  " + h + "m = " + feet + "ft");
        }
        
        // ===== ROUNDING FLOATS =====
        System.out.println("\n=== Rounding Floats ===");
        
        float value1 = 3.14159f;
        float value2 = 99.9999f;
        
        // Round to 2 decimal places
        System.out.println("Original: " + value1 + " -> Rounded: " + Math.round(value1 * 100) / 100f);
        System.out.println("Original: " + value2 + " -> Rounded: " + Math.round(value2 * 100) / 100f);
        
        // Using String.format
        System.out.println("Using format: " + String.format("%.2f", value1));
        System.out.println("Using format: " + String.format("%.2f", value2));
        
        // ===== COMPARING FLOATS =====
        System.out.println("\n=== Comparing Floats ===");
        
        float a = 0.1f;
        float b = 0.2f;
        float sum = a + b;
        
        System.out.println("0.1 + 0.2 = " + sum);
        System.out.println("Expected: 0.3");
        System.out.println("Are they equal? " + (sum == 0.3f));
        
        // Better way to compare (with tolerance)
        float tolerance = 0.0001f;
        boolean isEqual = Math.abs(sum - 0.3f) < tolerance;
        System.out.println("Using tolerance: " + isEqual);
        
        // ===== CONVERT FLOAT TO OTHER TYPES =====
        System.out.println("\n=== Convert Float to Other Types ===");
        
        float num = 123.45f;
        
        // Float to int (truncates decimal)
        int intValue = (int) num;
        System.out.println("Float to int: " + num + " -> " + intValue);
        
        // Float to String
        String stringValue = Float.toString(num);
        System.out.println("Float to String: " + num + " -> \"" + stringValue + "\"");
        
        // Float to double (widening)
        double doubleValue = num;
        System.out.println("Float to double: " + num + " -> " + doubleValue);
        
        // ===== CONVERT TO FLOAT =====
        System.out.println("\n=== Convert to Float ===");
        
        // String to float
        String strNum = "99.99";
        float floatFromString = Float.parseFloat(strNum);
        System.out.println("String to float: \"" + strNum + "\" -> " + floatFromString);
        
        // Int to float
        int intNum = 100;
        float floatFromInt = intNum;
        System.out.println("Int to float: " + intNum + " -> " + floatFromInt);
        
        // ===== WHY USE FLOAT? =====
        System.out.println("\n=== Why Use Float? ===");
        
        // Good for: decimals, scientific values, memory efficiency
        System.out.println("Float is used for:");
        System.out.println("  - Prices (less precise money)");
        System.out.println("  - Measurements");
        System.out.println("  - Scientific calculations");
        System.out.println("  - GPAs and grades");
        System.out.println("  - Temperatures");
        
        // Memory comparison
        float[] floatArray = new float[1000];
        double[] doubleArray = new double[1000];
        
        System.out.println("\nMemory comparison for 1000 elements:");
        System.out.println("  float array: " + floatArray.length * 4 + " bytes");
        System.out.println("  double array: " + doubleArray.length * 8 + " bytes");
        System.out.println("  Float uses half the memory of double!");
    }
}

/*
 * KEY CONCEPTS FOR BEGINNERS:
 * 
 * 1. DECLARING FLOAT ARRAYS:
 *    float[] array = new float[size];
 *    float[] array = {1.5f, 2.5f, 3.5f};
 * 
 * 2. FLOAT RANGE AND PRECISION:
 *    - Range: ±3.4e38 (very large!)
 *    - Precision: ~7 decimal digits
 *    - Takes 4 bytes (32 bits)
 *    - Use 'f' or 'F' suffix
 * 
 * 3. WHY USE FLOAT?
 *    - Good precision for most calculations
 *    - Uses less memory than double (half!)
 *    - Perfect for:
 *      * Prices (money needs double though)
 *      * Measurements
 *      * Scientific values
 *      * GPAs, grades
 *      * Temperatures
 * 
 * 4. IMPORTANT NOTES:
 *    - Default value is 0.0f
 *    - Always use 'f' suffix
 *    - Don't compare floats with ==
 *    - Use tolerance for comparison
 *    - Truncates when converting to int
 * 
 * 5. TIPS:
 *    - Use double for money (precision)
 *    - Use float for memory efficiency
 *    - Round decimals for display
 *    - Be careful with floating point math
 */
