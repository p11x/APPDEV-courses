import java.util.Scanner;

// Supermarket Inventory Checker
public class Own43 {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        
        // Declare arrays for 6 products
        String[] products = {"Sugar", "Rice", "Wheat", "Oil", "Salt", "Tea"};
        double[] prices = {40, 60, 45, 150, 20, 200};
        int[] stock = {50, 30, 40, 25, 60, 15};
        
        int choice;
        
        System.out.println("=== Supermarket Inventory Checker ===");
        System.out.println();
        
        // Menu loop
        do {
            System.out.println("===== Inventory Menu =====");
            System.out.println("1. View all products and stock");
            System.out.println("2. Purchase a product (reduce stock)");
            System.out.println("3. Restock a product (increase stock)");
            System.out.println("4. Find most expensive and cheapest product");
            System.out.println("5. Exit");
            System.out.print("Enter your choice: ");
            
            choice = scanner.nextInt();
            System.out.println();
            
            switch (choice) {
                case 1:
                    // View all products
                    System.out.println("=== Product Inventory ===");
                    System.out.println("Product    Price    Stock");
                    for (int i = 0; i < 6; i++) {
                        System.out.printf("%-10s Rs.%-6.2f %d%n", products[i], prices[i], stock[i]);
                        if (stock[i] < 5) {
                            System.out.println("  WARNING: Low stock!");
                        }
                    }
                    break;
                    
                case 2:
                    // Purchase
                    System.out.println("Products:");
                    for (int i = 0; i < 6; i++) {
                        System.out.println((i + 1) + ". " + products[i] + " (Stock: " + stock[i] + ")");
                    }
                    System.out.print("Select product number: ");
                    int purchaseIndex = scanner.nextInt();
                    
                    if (purchaseIndex >= 1 && purchaseIndex <= 6) {
                        purchaseIndex--;
                        System.out.print("Enter quantity to purchase: ");
                        int purchaseQty = scanner.nextInt();
                        
                        if (purchaseQty > 0 && purchaseQty <= stock[purchaseIndex]) {
                            stock[purchaseIndex] -= purchaseQty;
                            double cost = purchaseQty * prices[purchaseIndex];
                            System.out.println("Purchase successful!");
                            System.out.println("Cost: Rs. " + cost);
                            
                            if (stock[purchaseIndex] < 5) {
                                System.out.println("WARNING: Stock is low!");
                            }
                        } else {
                            System.out.println("Invalid quantity or insufficient stock!");
                        }
                    } else {
                        System.out.println("Invalid product!");
                    }
                    break;
                    
                case 3:
                    // Restock
                    System.out.println("Products:");
                    for (int i = 0; i < 6; i++) {
                        System.out.println((i + 1) + ". " + products[i] + " (Stock: " + stock[i] + ")");
                    }
                    System.out.print("Select product number: ");
                    int restockIndex = scanner.nextInt();
                    
                    if (restockIndex >= 1 && restockIndex <= 6) {
                        restockIndex--;
                        System.out.print("Enter quantity to restock: ");
                        int restockQty = scanner.nextInt();
                        
                        if (restockQty > 0) {
                            stock[restockIndex] += restockQty;
                            System.out.println("Restock successful!");
                            System.out.println("New stock: " + stock[restockIndex]);
                        } else {
                            System.out.println("Invalid quantity!");
                        }
                    } else {
                        System.out.println("Invalid product!");
                    }
                    break;
                    
                case 4:
                    // Find most expensive and cheapest
                    int maxIndex = 0;
                    int minIndex = 0;
                    
                    for (int i = 1; i < 6; i++) {
                        if (prices[i] > prices[maxIndex]) {
                            maxIndex = i;
                        }
                        if (prices[i] < prices[minIndex]) {
                            minIndex = i;
                        }
                    }
                    
                    System.out.println("Most Expensive Product: " + products[maxIndex] + " (Rs. " + prices[maxIndex] + ")");
                    System.out.println("Cheapest Product: " + products[minIndex] + " (Rs. " + prices[minIndex] + ")");
                    break;
                    
                case 5:
                    System.out.println("Thank you for using Inventory Checker!");
                    break;
                    
                default:
                    System.out.println("Invalid choice!");
            }
            
            System.out.println();
            
        } while (choice != 5);
        
        scanner.close();
    }
}
