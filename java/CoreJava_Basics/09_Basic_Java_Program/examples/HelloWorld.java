// HelloWorld - Your First Java Program
// This demonstrates the basic structure of a Java application

public class HelloWorld {
    
    // main() method - entry point of any Java application
    public static void main(String[] args) {
        // Print to console
        System.out.println("Hello, World!");
        System.out.println("Welcome to Java Programming!");
        
        // Print without newline
        System.out.print("This is ");
        System.out.print("print ");
        System.out.println("statement.");
        
        // Print with formatting (printf)
        System.out.printf("Number: %d, String: %s, Float: %.2f%n", 42, "Java", 3.14159);
        
        // Variables
        String name = "Developer";
        int age = 25;
        double salary = 50000.50;
        
        System.out.println("Name: " + name);
        System.out.println("Age: " + age);
        System.out.println("Salary: $" + salary);
    }
}
