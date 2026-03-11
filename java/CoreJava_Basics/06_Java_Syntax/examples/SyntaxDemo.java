// SyntaxDemo - Basic Java Syntax Rules
// Fundamental syntax elements of Java programming

public class SyntaxDemo {
    
    // Class declaration (PascalCase)
    static String classVariable = "I am a class";
    
    // Main method - entry point
    public static void main(String[] args) {
        
        // Variable declaration (camelCase)
        String message = "Hello, Java!";
        int number = 42;
        double decimal = 3.14;
        
        System.out.println("=== JAVA SYNTAX RULES ===\n");
        
        System.out.println("--- Case Sensitivity ---");
        System.out.println("message vs Message vs MESSAGE are different");
        
        System.out.println("\n--- Class Names ---");
        System.out.println("MyFirstClass (PascalCase)");
        
        System.out.println("\n--- Method Names ---");
        System.out.println("myFirstMethod() (camelCase)");
        
        System.out.println("\n--- Variable Names ---");
        System.out.println("myFirstVariable (camelCase)");
        
        System.out.println("\n--- Constants ---");
        System.out.println("MAX_VALUE (UPPER_SNAKE_CASE)");
        
        System.out.println("\n--- Statement Termination ---");
        int a = 5;  // Statement ends with semicolon
        int b = 10; // Another statement
        
        System.out.println("\n--- Code Blocks ---");
        if (a < b) {
            System.out.println("a is less than b");
        } else {
            System.out.println("a is greater or equal");
        }
        
        System.out.println("\n--- Identifiers Rules ---");
        System.out.println("Can start with: letter, $, _");
        System.out.println("Can contain: letter, digit, $, _");
        System.out.println("Cannot: be keyword, start with digit");
        
        System.out.println("\n--- Comments ---");
        // This is single-line comment
        /* This is multi-line comment */
        /** This is Javadoc comment */
        System.out.println("Comments are ignored by compiler");
    }
}
