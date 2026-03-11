// AngularDataTypeDemo - Demonstrates Wrapper Classes for Type Mappings
// Use case: Java wrapper types mapping to TypeScript types

public class AngularDataTypeDemo {
    
    public static void main(String[] args) {
        System.out.println("=== WRAPPER CLASSES FOR ANGULAR TYPES ===\n");
        
        // Integer (maps to number in TypeScript)
        System.out.println("--- Integer -> number ---");
        Integer userId = 42;
        int primitiveId = userId; // Auto-unboxing
        System.out.println("User ID: " + userId);
        System.out.println("Primitive: " + primitiveId);
        
        // Double (maps to number in TypeScript)
        System.out.println("\n--- Double -> number ---");
        Double price = 19.99;
        double primitivePrice = price;
        System.out.println("Price: " + price);
        System.out.println("Formatted: $" + String.format("%.2f", price));
        
        // Boolean (maps to boolean in TypeScript)
        System.out.println("\n--- Boolean -> boolean ---");
        Boolean isActive = true;
        boolean primitiveActive = isActive;
        System.out.println("Active: " + isActive);
        System.out.println("Primitive: " + primitiveActive);
        
        // String (maps to string in TypeScript)
        System.out.println("\n--- String -> string ---");
        String name = "John Doe";
        System.out.println("Name: " + name);
        System.out.println("Upper: " + name.toUpperCase());
        
        // Character (maps to string in TypeScript)
        System.out.println("\n--- Character -> string ---");
        Character grade = 'A';
        char primitiveGrade = grade;
        System.out.println("Grade: " + grade);
        
        // Null handling (important for Angular)
        System.out.println("\n--- Null Handling ---");
        Integer nullValue = null;
        // int unboxed = nullValue; // NullPointerException!
        System.out.println("Null value: " + nullValue);
        
        // Default values for Angular forms
        System.out.println("\n--- Default Values ---");
        Integer defaultId = 0;
        String defaultName = "";
        Boolean defaultActive = false;
        System.out.println("Default ID: " + defaultId);
        System.out.println("Default Name: '" + defaultName + "'");
        System.out.println("Default Active: " + defaultActive);
        
        // Parsing (from string input)
        System.out.println("\n--- Parsing Strings ---");
        String numStr = "123";
        int parsed = Integer.parseInt(numStr);
        System.out.println("Parsed int: " + parsed);
        
        String boolStr = "true";
        boolean parsedBool = Boolean.parseBoolean(boolStr);
        System.out.println("Parsed bool: " + parsedBool);
        
        // Conversion for JSON
        System.out.println("\n--- JSON Conversion ---");
        Integer[] ids = {1, 2, 3};
        System.out.println("Array length: " + ids.length);
        
        // Array to list
        java.util.List<Integer> idList = java.util.Arrays.asList(ids);
        System.out.println("List size: " + idList.size());
        
        System.out.println("\n=== TYPE SCRIPT MAPPINGS ===");
        System.out.println("Java          -> TypeScript");
        System.out.println("---------------------------");
        System.out.println("Integer       -> number");
        System.out.println("Double        -> number");
        System.out.println("Boolean       -> boolean");
        System.out.println("String        -> string");
        System.out.println("Character     -> string");
        System.out.println("List<T>       -> T[]");
        System.out.println("Map<K,V>      -> {[key: K]: V}");
    }
}
