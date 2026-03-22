/*
 * SUB TOPIC: Java Introduction and Platform Independence
 * 
 * DEFINITION:
 * Java is a high-level, object-oriented programming language developed by Sun Microsystems (now Oracle).
 * It follows the "Write Once, Run Anywhere" (WORA) principle, meaning Java programs can run on any 
 * device that has a Java Virtual Machine (JVM) installed.
 * 
 * FUNCTIONALITIES:
 * 1. Platform independence through JVM
 * 2. Object-oriented programming features
 * 3. Automatic memory management (Garbage Collection)
 * 4. Rich API and extensive libraries
 * 5. Multi-threading support
 * 6. Security features
 */

public class Example46 {
    public static void main(String[] args) {
        
        // Topic Explanation: Java Platform Independence
        
        // How Java achieves platform independence:
        // 1. Source code (.java) -> Compiler -> Bytecode (.class)
        // 2. Bytecode is executed by JVM on any platform
        
        System.out.println("=== Java Introduction ===");
        System.out.println("Welcome to Java Programming!");
        
        // Simple Java program structure
        System.out.println("\n=== Basic Program Structure ===");
        
        // Class declaration
        // public class HelloWorld
        
        // Main method - entry point
        // public static void main(String[] args)
        
        // Print statement
        System.out.println("Hello, World!");
        
        // Java is object-oriented
        System.out.println("\n=== Object-Oriented Features ===");
        
        // Everything in Java is an object (except primitives)
        String message = "Java is OOP";
        System.out.println("Message: " + message);
        
        // Key features of Java
        System.out.println("\n=== Key Java Features ===");
        
        // 1. Simple - Easy to learn
        System.out.println("1. Simple: Clean syntax");
        
        // 2. Object-Oriented
        System.out.println("2. Object-Oriented: Classes and objects");
        
        // 3. Platform Independent
        System.out.println("3. Platform Independent: Run anywhere with JVM");
        
        // 4. Secured
        System.out.println("4. Secured: Security manager");
        
        // 5. Robust
        System.out.println("5. Robust: Exception handling, garbage collection");
        
        // 6. Multithreaded
        System.out.println("6. Multithreaded: Concurrent execution");
        
        // Real-time Example 1: Hello World Application
        System.out.println("\n=== Example 1: Hello World ===");
        System.out.println("This is a basic Java application");
        System.out.println("Java can run on Windows, Linux, Mac");
        
        // Real-time Example 2: Cross-platform Application
        System.out.println("\n=== Example 2: Platform Check ===");
        String os = System.getProperty("os.name");
        String javaVersion = System.getProperty("java.version");
        System.out.println("Operating System: " + os);
        System.out.println("Java Version: " + javaVersion);
        
        // Real-time Example 3: Simple Calculator
        System.out.println("\n=== Example 3: Simple Operations ===");
        int a = 10, b = 5;
        System.out.println("Addition: " + (a + b));
        System.out.println("Subtraction: " + (a - b));
        System.out.println("Multiplication: " + (a * b));
        System.out.println("Division: " + (a / b));
        
        // Real-time Example 4: String Manipulation
        System.out.println("\n=== Example 4: String Operations ===");
        String firstName = "John";
        String lastName = "Doe";
        String fullName = firstName + " " + lastName;
        System.out.println("Full Name: " + fullName);
        System.out.println("Uppercase: " + fullName.toUpperCase());
        System.out.println("Length: " + fullName.length());
        
        // Real-time Example 5: Array Basics
        System.out.println("\n=== Example 5: Array ===");
        int[] numbers = {1, 2, 3, 4, 5};
        System.out.print("Numbers: ");
        for (int num : numbers) {
            System.out.print(num + " ");
        }
        System.out.println();
        
        // Real-time Example 6: Class and Object
        System.out.println("\n=== Example 6: Class Example ===");
        
        class Car {
            String brand;
            int speed;
            
            void drive() {
                System.out.println(brand + " is driving at " + speed + " km/h");
            }
        }
        
        Car car = new Car();
        car.brand = "Toyota";
        car.speed = 120;
        car.drive();
    }
}
