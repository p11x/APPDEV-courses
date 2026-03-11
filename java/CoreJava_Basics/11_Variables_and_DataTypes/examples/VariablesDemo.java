// VariablesDemo - Demonstrates Java Variables and Data Types
// Java has primitive types and reference types

public class VariablesDemo {
    
    // ===== PRIMITIVE DATA TYPES =====
    byte byteVar = 127;
    short shortVar = 32767;
    int intVar = 2147483647;
    long longVar = 9223372036854775807L;
    float floatVar = 3.14159f;
    double doubleVar = 3.14159265358979;
    char charVar = 'A';
    boolean booleanVar = true;
    
    // Reference types
    String stringVar = "Hello Java";
    int[] arrayVar = {1, 2, 3};
    
    public static void main(String[] args) {
        VariablesDemo demo = new VariablesDemo();
        
        System.out.println("=== VARIABLES AND DATA TYPES ===\n");
        System.out.println("byte: " + demo.byteVar);
        System.out.println("short: " + demo.shortVar);
        System.out.println("int: " + demo.intVar);
        System.out.println("long: " + demo.longVar);
        System.out.println("float: " + demo.floatVar);
        System.out.println("double: " + demo.doubleVar);
        System.out.println("char: " + demo.charVar);
        System.out.println("boolean: " + demo.booleanVar);
        
        System.out.println("\n--- Type Casting ---");
        int i = 100;
        long l = i;
        double d = i;
        System.out.println("Implicit: int to long = " + l);
        
        double pi = 3.14159;
        int intPi = (int) pi;
        System.out.println("Explicit: double to int = " + intPi);
        
        System.out.println("\n--- Constants ---");
        final double PI = 3.14159;
        System.out.println("Constant: " + PI);
        
        System.out.println("\n--- Naming Conventions ---");
        int studentCount = 25;
        String firstName = "John";
        int MAX_SIZE = 100;
        System.out.println("camelCase: studentCount = " + studentCount);
        System.out.println("UPPER_SNAKE: MAX_SIZE = " + MAX_SIZE);
    }
}
