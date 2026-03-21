import java.util.Scanner;

// Simple Bill Generator
public class Own12 {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        
        System.out.println("=== Simple Bill Generator ===");
        System.out.println();
        
        // Number of items
        int numItems = 5;
        
        // Arrays to store item details
        String[] itemNames = new String[numItems];
        int[] quantities = new int[numItems];
        double[] prices = new double[numItems];
        double[] subtotals = new double[numItems];
        
        // Input item details
        System.out.println("Enter details for 5 items:");
        System.out.println();
        
        for (int i = 0; i < numItems; i++) {
            System.out.println("Item " + (i + 1) + ":");
            System.out.print("  Name: ");
            itemNames[i] = scanner.next();
            System.out.print("  Quantity: ");
            quantities[i] = scanner.nextInt();
            System.out.print("  Price per unit: Rs. ");
            prices[i] = scanner.nextDouble();
            
            // Calculate subtotal
            subtotals[i] = quantities[i] * prices[i];
            System.out.println();
        }
        
        // Calculate total bill
        double total = 0;
        for (int i = 0; i < numItems; i++) {
            total = total + subtotals[i];
        }
        
        // Calculate GST (18%)
        double gst = total * 0.18;
        
        // Calculate final total
        double finalTotal = total + gst;
        
        // Display itemized bill
        System.out.println();
        System.out.println("==============================================");
        System.out.println("              BILL RECEIPT                   ");
        System.out.println("==============================================");
        System.out.println("Item Name     Qty    Price     Subtotal");
        System.out.println("----------------------------------------------");
        
        for (int i = 0; i < numItems; i++) {
            System.out.printf("%-12s %4d   Rs.%-6.2f Rs.%.2f%n", 
                              itemNames[i], quantities[i], prices[i], subtotals[i]);
        }
        
        System.out.println("----------------------------------------------");
        System.out.printf("Subtotal:                     Rs.%.2f%n", total);
        System.out.printf("GST (18%%):                    Rs.%.2f%n", gst);
        System.out.println("==============================================");
        System.out.printf("TOTAL BILL:                   Rs.%.2f%n", finalTotal);
        System.out.println("==============================================");
        
        scanner.close();
    }
}
