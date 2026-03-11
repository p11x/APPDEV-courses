// ProductCatalogDemo - Demonstrates Array Operations for Product Management
// Use case: Managing product listings in Angular e-commerce app backend

public class ProductCatalogDemo {
    
    // Product class
    static class Product {
        private int id;
        private String name;
        private double price;
        
        public Product(int id, String name, double price) {
            this.id = id;
            this.name = name;
            this.price = price;
        }
        
        public int getId() { return id; }
        public String getName() { return name; }
        public double getPrice() { return price; }
        
        @Override
        public String toString() {
            return "Product{id=" + id + ", name='" + name + "', price=" + price + "}";
        }
    }
    
    // Initialize product array
    public static Product[] initializeProducts() {
        Product[] products = new Product[5];
        products[0] = new Product(1, "Laptop", 999.99);
        products[1] = new Product(2, "Smartphone", 699.99);
        products[2] = new Product(3, "Tablet", 449.99);
        products[3] = new Product(4, "Headphones", 199.99);
        products[4] = new Product(5, "Smartwatch", 299.99);
        return products;
    }
    
    // Linear search for product
    public static Product findProductById(Product[] products, int id) {
        for (Product p : products) {
            if (p.getId() == id) {
                return p;
            }
        }
        return null;
    }
    
    // Find products in price range
    public static Product[] findByPriceRange(Product[] products, double min, double max) {
        // First pass: count matching products
        int count = 0;
        for (Product p : products) {
            if (p.getPrice() >= min && p.getPrice() <= max) {
                count++;
            }
        }
        
        // Second pass: populate result array
        Product[] result = new Product[count];
        int index = 0;
        for (Product p : products) {
            if (p.getPrice() >= min && p.getPrice() <= max) {
                result[index++] = p;
            }
        }
        
        return result;
    }
    
    // Sort products by price (bubble sort)
    public static Product[] sortByPrice(Product[] products) {
        Product[] sorted = products.clone();
        
        for (int i = 0; i < sorted.length - 1; i++) {
            for (int j = 0; j < sorted.length - i - 1; j++) {
                if (sorted[j].getPrice() > sorted[j + 1].getPrice()) {
                    // Swap
                    Product temp = sorted[j];
                    sorted[j] = sorted[j + 1];
                    sorted[j + 1] = temp;
                }
            }
        }
        
        return sorted;
    }
    
    // Calculate total inventory value
    public static double calculateTotalValue(Product[] products) {
        double total = 0;
        for (Product p : products) {
            total += p.getPrice();
        }
        return total;
    }
    
    // Apply discount to all products
    public static void applyDiscount(Product[] products, double discountPercent) {
        for (int i = 0; i < products.length; i++) {
            double newPrice = products[i].getPrice() * (1 - discountPercent / 100);
            products[i] = new Product(
                products[i].getId(),
                products[i].getName(),
                newPrice
            );
        }
    }
    
    public static void main(String[] args) {
        System.out.println("=== PRODUCT CATALOG WITH ARRAYS ===\n");
        
        // Initialize products
        Product[] products = initializeProducts();
        
        // Display all products
        System.out.println("--- All Products ---");
        for (Product p : products) {
            System.out.println(p);
        }
        
        // Search by ID
        System.out.println("\n--- Search by ID ---");
        Product found = findProductById(products, 3);
        System.out.println("Found: " + found);
        
        // Price range search
        System.out.println("\n--- Price Range ($200-$500) ---");
        Product[] inRange = findByPriceRange(products, 200, 500);
        for (Product p : inRange) {
            System.out.println(p);
        }
        
        // Sort by price
        System.out.println("\n--- Sorted by Price ---");
        Product[] sorted = sortByPrice(products);
        for (Product p : sorted) {
            System.out.println(p);
        }
        
        // Calculate total
        System.out.println("\n--- Total Inventory Value ---");
        System.out.println("Total: $" + calculateTotalValue(products));
        
        // Apply discount
        System.out.println("\n--- After 10% Discount ---");
        applyDiscount(products, 10);
        for (Product p : products) {
            System.out.println(p);
        }
        
        System.out.println("\n=== ANGULAR USE CASES ===");
        System.out.println("1. Product listing component (*ngFor)");
        System.out.println("2. Product filtering (price range slider)");
        System.out.println("3. Sorting (price, name, popularity)");
        System.out.println("4. Search functionality");
        System.out.println("5. Shopping cart calculations");
    }
}
