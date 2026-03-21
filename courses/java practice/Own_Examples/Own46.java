import java.util.Scanner;

// Currency Converter
public class Own46 {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        
        System.out.println("=== Currency Converter ===");
        System.out.println();
        
        // Exchange rates vs INR
        String[] currencies = {"USD", "EUR", "GBP", "JPY", "AED"};
        double[] rates = {83.5, 90.2, 105.3, 0.56, 22.7};
        
        // Show menu
        System.out.println("Available currencies:");
        System.out.println("1. INR to Foreign Currency");
        System.out.println("2. Foreign Currency to INR");
        System.out.print("Enter choice: ");
        int choice = scanner.nextInt();
        
        if (choice == 1) {
            // INR to Foreign
            System.out.print("Enter amount in INR: ");
            double inrAmount = scanner.nextDouble();
            
            System.out.println();
            System.out.println("=== Conversion Results ===");
            System.out.println("INR: " + inrAmount);
            
            for (int i = 0; i < 5; i++) {
                double converted = inrAmount / rates[i];
                System.out.printf("%s: %.2f%n", currencies[i], converted);
            }
            
        } else if (choice == 2) {
            // Foreign to INR
            System.out.println("Select currency:");
            for (int i = 0; i < 5; i++) {
                System.out.println((i + 1) + ". " + currencies[i] + " (Rate: " + rates[i] + ")");
            }
            System.out.print("Enter currency choice (1-5): ");
            int currencyChoice = scanner.nextInt();
            
            if (currencyChoice >= 1 && currencyChoice <= 5) {
                System.out.print("Enter amount in " + currencies[currencyChoice - 1] + ": ");
                double foreignAmount = scanner.nextDouble();
                
                double inrConverted = foreignAmount * rates[currencyChoice - 1];
                
                System.out.println();
                System.out.println("=== Conversion Result ===");
                System.out.println(currencies[currencyChoice - 1] + ": " + foreignAmount);
                System.out.println("INR: " + inrConverted);
            } else {
                System.out.println("Invalid choice!");
            }
            
        } else {
            System.out.println("Invalid choice!");
        }
        
        scanner.close();
    }
}
