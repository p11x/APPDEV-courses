import java.util.Scanner;

// Shopping Cart Simulator
public class Own19 {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        
        System.out.println("=== Shopping Cart Simulator ===");
        System.out.println();
        
        // Declare 5 products with fixed names and prices
        String[] products = {"Laptop", "Phone", "Tablet", "Headphones", "Camera"};
        double[] prices = {50000, 30000, 20000, 5000, 40000};
        int[] quantities = {0, 0, 0, 0, 0};
        
        // Display product list
        System.out.println("=== Product List ===");
        for (int i = 0; i < 5; i++) {
            System.out.println((i + 1) + ". " + products[i] + " - Rs. " + prices[i]);
        }
        System.out.println();
        
        // User selects products
        System.out.println("Select products to add to cart (enter product number, 0 to finish):");
        
        int productChoice;
        do {
            System.out.print("Enter product number (1-5) or 0 to finish: ");
            productChoice = scanner.nextInt();
            
            if (productChoice >= 1 && productChoice <= 5) {
                System.out.print("Enter quantity: ");
                int qty = scanner.nextInt();
                
                if (qty > 0) {
                    quantities[productChoice - 1] = quantities[productChoice - 1] + qty;
                    System.out.println("Added " + qty + " " + products[productChoice - 1] + "(s) to cart.");
                } else {
                    System.out.println("Quantity must be positive!");
                }
            } else if (productChoice != 0) {
                System.out.println("Invalid product number!");
            }
            
        } while (productChoice != 0);
        
        // Calculate subtotal for each item
        double[] subtotals = new double[5];
        double total = 0;
        
        for (int i = 0; i < 5; i++) {
            subtotals[i] = quantities[i] * prices[i];
            total = total + subtotals[i];
        }
        
        // Calculate GST (18%)
        double gst = total * 0.18;
        
        // Calculate final total
        double finalTotal = total + gst;
        
        // Display bill
        System.out.println();
        System.out.println("==============================================");
        System.out.println("             SHOPPING CART BILL              ");
        System.out.println("==============================================");
        System.out.println("Product        Qty    Price     Subtotal");
        System.out.println("----------------------------------------------");
        
        for (int i = 0; i < 5; i++) {
            if (quantities[i] > 0) {
                System.out.printf("%-14s %4d   Rs.%6.0f Rs.%8.2f%n", 
                                  products[i], quantities[i], prices[i], subtotals[i]);
            }
        }
        
        System.out.println("----------------------------------------------");
        System.out.printf("Subtotal:                      Rs.%.2f%n", total);
        System.out.printf("GST (18%%):                     Rs.%.2f%n", gst);
        System.out.println("==============================================");
        System.out.printf("TOTAL BILL:                    Rs.%.2f%n", finalTotal);
        System.out.println("==============================================");
        
        scanner.close();
    }
}
