import java.util.Scanner;

// Star Pattern Printer
public class Own4 {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        
        System.out.println("=== Star Pattern Printer ===");
        System.out.println();
        
        // Pattern 1: Right-angled triangle
        System.out.println("Pattern 1: Right-angled Triangle");
        System.out.println("*");
        System.out.println("**");
        System.out.println("***");
        System.out.println("****");
        System.out.println("*****");
        System.out.println();
        
        // Pattern 2: Inverted triangle
        System.out.println("Pattern 2: Inverted Triangle");
        System.out.println("*****");
        System.out.println("****");
        System.out.println("***");
        System.out.println("**");
        System.out.println("*");
        System.out.println();
        
        // Pattern 3: Pyramid
        System.out.println("Pattern 3: Pyramid");
        System.out.println("    *");
        System.out.println("   ***");
        System.out.println("  *****");
        System.out.println(" *******");
        System.out.println("*********");
        System.out.println();
        
        // Pattern 4: Diamond shape
        System.out.println("Pattern 4: Diamond Shape");
        System.out.println("    *");
        System.out.println("   ***");
        System.out.println("  *****");
        System.out.println(" *******");
        System.out.println("*********");
        System.out.println(" *******");
        System.out.println("  *****");
        System.out.println("   ***");
        System.out.println("    *");
        System.out.println();
        
        // User can also input for custom patterns
        System.out.println("=== Custom Patterns ===");
        System.out.print("Enter number of rows for custom patterns: ");
        int rows = scanner.nextInt();
        
        // Custom Right-angled triangle
        System.out.println();
        System.out.println("Custom Pattern 1: Right-angled Triangle");
        for (int i = 1; i <= rows; i++) {
            for (int j = 1; j <= i; j++) {
                System.out.print("*");
            }
            System.out.println();
        }
        
        // Custom Inverted triangle
        System.out.println("Custom Pattern 2: Inverted Triangle");
        for (int i = rows; i >= 1; i--) {
            for (int j = 1; j <= i; j++) {
                System.out.print("*");
            }
            System.out.println();
        }
        
        // Custom Pyramid
        System.out.println("Custom Pattern 3: Pyramid");
        for (int i = 1; i <= rows; i++) {
            // Print spaces
            for (int j = 1; j <= rows - i; j++) {
                System.out.print(" ");
            }
            // Print stars
            for (int k = 1; k <= 2 * i - 1; k++) {
                System.out.print("*");
            }
            System.out.println();
        }
        
        scanner.close();
    }
}
