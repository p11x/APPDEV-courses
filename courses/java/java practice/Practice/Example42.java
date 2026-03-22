/*
 * SUB TOPIC: Wrapper Classes in Java
 * 
 * DEFINITION:
 * Wrapper classes are object representations of primitive data types in Java. They provide a way to 
 * use primitive values as objects. Each primitive type has a corresponding wrapper class (e.g., int 
 * has Integer, double has Double). Wrapper classes enable collections to store primitives and provide 
 * utility methods for conversion.
 * 
 * FUNCTIONALITIES:
 * 1. Autoboxing - Automatic conversion from primitive to wrapper
 * 2. Unboxing - Automatic conversion from wrapper to primitive
 * 3. Conversion methods - parseXxx() and valueOf()
 * 4. Constants - MIN_VALUE, MAX_VALUE
 * 5. Utility methods - toString(), compareTo()
 */

public class Example42 {
    public static void main(String[] args) {
        
        // Topic Explanation: Wrapper Classes
        
        // Autoboxing - primitive to wrapper
        System.out.println("=== Autoboxing ===");
        Integer wrapped = 10; // Autoboxing: int to Integer
        System.out.println("Wrapped value: " + wrapped);
        
        // Unboxing - wrapper to primitive
        System.out.println("\n=== Unboxing ===");
        Integer num = 100;
        int primitive = num; // Unboxing: Integer to int
        System.out.println("Unboxed value: " + primitive);
        
        // Integer wrapper class
        System.out.println("\n=== Integer Wrapper ===");
        Integer intObj = Integer.valueOf(42); // Create from int
        int intVal = Integer.parseInt("123"); // Parse string to int
        
        System.out.println("ValueOf: " + intObj);
        System.out.println("ParseInt: " + intVal);
        
        // Constants
        System.out.println("\n=== Constants ===");
        System.out.println("Integer.MAX_VALUE: " + Integer.MAX_VALUE);
        System.out.println("Integer.MIN_VALUE: " + Integer.MIN_VALUE);
        System.out.println("Double.MAX_VALUE: " + Double.MAX_VALUE);
        
        // Conversion methods
        System.out.println("\n=== Conversion Methods ===");
        
        // String to primitive
        int p = Integer.parseInt("555");
        System.out.println("parseInt: " + p);
        
        // Primitive to String
        String s = Integer.toString(777);
        System.out.println("toString: " + s);
        
        // Using valueOf
        Integer obj1 = Integer.valueOf(100);
        Integer obj2 = Integer.valueOf("200");
        System.out.println("valueOf(int): " + obj1);
        System.out.println("valueOf(String): " + obj2);
        
        // Double wrapper
        System.out.println("\n=== Double Wrapper ===");
        Double d1 = 3.14; // Autoboxing
        double d2 = Double.parseDouble("2.71828");
        System.out.println("Autoboxed: " + d1);
        System.out.println("Parse: " + d2);
        
        // Boolean wrapper
        System.out.println("\n=== Boolean Wrapper ===");
        Boolean b1 = Boolean.parseBoolean("true");
        boolean b2 = Boolean.TRUE;
        System.out.println("Parsed: " + b1);
        System.out.println("Boolean.TRUE: " + b2);
        
        // Character wrapper
        System.out.println("\n=== Character Wrapper ===");
        Character c1 = 'A';
        char c2 = Character.toLowerCase('B');
        boolean isDigit = Character.isDigit('5');
        boolean isLetter = Character.isLetter('x');
        
        System.out.println("Char: " + c1);
        System.out.println("toLowerCase: " + c2);
        System.out.println("isDigit: " + isDigit);
        System.out.println("isLetter: " + isLetter);
        
        // Real-time Example 1: User input validation
        System.out.println("\n=== Example 1: Input Validation ===");
        String ageInput = "25";
        
        try {
            int age = Integer.parseInt(ageInput);
            if (age >= 18) {
                System.out.println("Adult");
            } else {
                System.out.println("Minor");
            }
        } catch (NumberFormatException e) {
            System.out.println("Invalid age");
        }
        
        // Real-time Example 2: Currency conversion
        System.out.println("\n=== Example 2: Currency Parsing ===");
        String priceStr = "$19.99";
        String cleaned = priceStr.replace("$", "");
        double price = Double.parseDouble(cleaned);
        double withTax = price * 1.1;
        
        System.out.println("Original: " + priceStr);
        System.out.println("Price: " + price);
        System.out.println("With 10% tax: " + withTax);
        
        // Real-time Example 3: Form validation
        System.out.println("\n=== Example 3: Form Validation ===");
        String phone = "9876543210";
        
        boolean isValidLength = phone.length() == 10;
        boolean isNumeric = phone.matches("\\d+");
        
        System.out.println("Valid length: " + isValidLength);
        System.out.println("All digits: " + isNumeric);
        System.out.println("Valid phone: " + (isValidLength && isNumeric));
        
        // Real-time Example 4: Math operations with wrappers
        System.out.println("\n=== Example 4: Math Operations ===");
        Double radius = 5.0;
        Double area = Math.PI * radius * radius;
        
        System.out.println("Radius: " + radius);
        System.out.println("Area: " + area);
        
        // Real-time Example 5: Working with collections
        System.out.println("\n=== Example 5: Collections with Primitives ===");
        
        java.util.ArrayList<Integer> numbers = new java.util.ArrayList<>();
        numbers.add(10); // Autoboxing
        numbers.add(20);
        numbers.add(30);
        
        int sum = 0;
        for (Integer n : numbers) {
            sum += n; // Unboxing
        }
        
        System.out.println("Numbers: " + numbers);
        System.out.println("Sum: " + sum);
        
        // Real-time Example 6: Null handling with wrappers
        System.out.println("\n=== Example 6: Null Handling ===");
        
        Integer nullable = null;
        
        // Safe null check
        if (nullable != null) {
            System.out.println("Value: " + nullable);
        } else {
            System.out.println("Value is null - using default");
            int safeValue = nullable != null ? nullable : 0;
            System.out.println("Default value: " + safeValue);
        }
        
        // Using Optional as alternative
        System.out.println("\n=== Using Optional ===");
        Integer value = 42;
        java.util.Optional<Integer> optional = java.util.Optional.ofNullable(value);
        
        System.out.println("Has value: " + optional.isPresent());
        System.out.println("Value: " + optional.orElse(0));
        
        // Bonus: Type comparison
        System.out.println("\n=== Bonus: equals() vs == ===");
        Integer a = 127;
        Integer b = 127;
        Integer c = 128;
        Integer d = 128;
        
        // == compares references, equals compares values
        System.out.println("127 == 127: " + (a == b)); // true (cached)
        System.out.println("127 equals 127: " + a.equals(b)); // true
        System.out.println("128 == 128: " + (c == d)); // false (not cached)
        System.out.println("128 equals 128: " + c.equals(d)); // true
    }
}
