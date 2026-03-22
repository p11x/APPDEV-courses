// Warehouse Stock Optimizer
public class Own59 {
    public static void main(String[] args) {
        java.util.Scanner scanner = new java.util.Scanner(System.in);
        
        System.out.println("=== Warehouse Stock Optimizer ===");
        System.out.println();
        
        // Product data
        String[] products = {"Laptop", "Mouse", "Keyboard", "Monitor", "Headphones", "Webcam"};
        int[] stock = {15, 8, 25, 5, 30, 3};
        int[] minStock = {10, 15, 10, 8, 20, 10};
        
        // Display current stock
        System.out.println("Current Stock Status:");
        System.out.println("------------------------------------------");
        System.out.printf("%-12s | %-8s | %-10s | Status%n", "Product", "Stock", "Min Stock");
        System.out.println("------------------------------------------");
        
        for (int i = 0; i < products.length; i++) {
            String status;
            if (stock[i] == 0) {
                status = "OUT OF STOCK";
            } else if (stock[i] <= minStock[i] / 2) {
                status = "CRITICAL";
            } else if (stock[i] <= minStock[i]) {
                status = "LOW";
            } else {
                status = "OK";
            }
            System.out.printf("%-12s | %-8d | %-10d | %s%n", products[i], stock[i], minStock[i], status);
        }
        
        System.out.println();
        
        // Sort by urgency (bubble sort)
        System.out.println("=== Stock Sorted by Urgency ===");
        System.out.println();
        
        String[] sortedProducts = products.clone();
        int[] sortedStock = stock.clone();
        int[] sortedMinStock = minStock.clone();
        
        // Calculate urgency score (lower stock = higher urgency)
        int[] urgency = new int[products.length];
        for (int i = 0; i < products.length; i++) {
            urgency[i] = sortedMinStock[i] - sortedStock[i];
        }
        
        // Bubble sort by urgency
        for (int i = 0; i < products.length - 1; i++) {
            for (int j = 0; j < products.length - i - 1; j++) {
                if (urgency[j] < urgency[j + 1]) {
                    // Swap urgency
                    int tempUrgency = urgency[j];
                    urgency[j] = urgency[j + 1];
                    urgency[j + 1] = tempUrgency;
                    
                    // Swap products
                    String tempProduct = sortedProducts[j];
                    sortedProducts[j] = sortedProducts[j + 1];
                    sortedProducts[j + 1] = tempProduct;
                    
                    // Swap stock
                    int tempStock = sortedStock[j];
                    sortedStock[j] = sortedStock[j + 1];
                    sortedStock[j + 1] = tempStock;
                    
                    // Swap minStock
                    int tempMinStock = sortedMinStock[j];
                    sortedMinStock[j] = sortedMinStock[j + 1];
                    sortedMinStock[j + 1] = tempMinStock;
                }
            }
        }
        
        // Display sorted by urgency
        System.out.println("------------------------------------------");
        System.out.printf("%-12s | %-8s | %-10s | Urgency%n", "Product", "Stock", "Min Stock");
        System.out.println("------------------------------------------");
        
        for (int i = 0; i < products.length; i++) {
            String urgencyLevel;
            if (urgency[i] > 10) {
                urgencyLevel = "VERY HIGH";
            } else if (urgency[i] > 5) {
                urgencyLevel = "HIGH";
            } else if (urgency[i] > 0) {
                urgencyLevel = "MEDIUM";
            } else {
                urgencyLevel = "LOW";
            }
            System.out.printf("%-12s | %-8d | %-10d | %s%n", sortedProducts[i], sortedStock[i], sortedMinStock[i], urgencyLevel);
        }
        
        System.out.println();
        
        // Restocking recommendations
        System.out.println("=== Restocking Recommendations ===");
        System.out.println();
        
        int totalItemsToOrder = 0;
        int totalCost = 0;
        int[] prices = {500, 20, 50, 200, 30, 80};
        
        for (int i = 0; i < products.length; i++) {
            int reorderQty = minStock[i] * 2 - stock[i];
            if (reorderQty > 0) {
                System.out.println("Order " + reorderQty + " units of " + products[i] + 
                    " (Cost: $" + (reorderQty * prices[i]) + ")");
                totalItemsToOrder += reorderQty;
                totalCost += reorderQty * prices[i];
            }
        }
        
        System.out.println();
        System.out.println("Total items to order: " + totalItemsToOrder);
        System.out.println("Total estimated cost: $" + totalCost);
        
        scanner.close();
    }
}
