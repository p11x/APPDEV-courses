// OutputDemo - Java Output Statements
// Different ways to display output in Java

public class OutputDemo {
    
    public static void main(String[] args) {
        System.out.println("=== JAVA OUTPUT METHODS ===\n");
        
        // System.out.println() - prints with newline
        System.out.println("--- println() ---");
        System.out.println("Line 1");
        System.out.println("Line 2");
        System.out.println("This prints and moves to new line");
        
        // System.out.print() - prints without newline
        System.out.println("\n--- print() ---");
        System.out.print("Hello ");
        System.out.print("World!");
        System.out.println(" (no new line in first two)");
        
        // System.out.printf() - formatted output
        System.out.println("\n--- printf() ---");
        String name = "John";
        int age = 30;
        double salary = 50000.50;
        
        System.out.printf("Name: %s%n", name);
        System.out.printf("Age: %d%n", age);
        System.out.printf("Salary: %.2f%n", salary);
        System.out.printf("Multiple: %s is %d years old%n", name, age);
        
        // Format specifiers
        System.out.println("\n--- Format Specifiers ---");
        System.out.printf("%%d = integer: %d%n", 42);
        System.out.printf("%%f = float: %.3f%n", 3.14159);
        System.out.printf("%%s = string: %s%n", "Java");
        System.out.printf("%%c = char: %c%n", 'A');
        System.out.printf("%%b = boolean: %b%n", true);
        
        // String.format()
        System.out.println("\n--- String.format() ---");
        String formatted = String.format("Welcome %s, age %d", "Alice", 25);
        System.out.println(formatted);
        
        // Angular Note
        System.out.println("\n--- Angular Note ---");
        System.out.println("In web apps, use console.log() in TypeScript");
        System.out.println("Angular displays data via templates, not console");
    }
}
