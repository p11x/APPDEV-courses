// ProductInventoryDemo - Demonstrates Wrapper Classes and Null Handling
// Use case: E-commerce product data with nullable fields

public class ProductInventoryDemo {
    // Primitive wrapper types (can be null)
    private Integer productId;
    private Integer stockQuantity;
    private Double price;
    private Boolean isAvailable;
    
    // Reference type
    private String productName;
    private String category;
    
    // Constructor with null handling
    public ProductInventoryDemo(Integer productId, String productName, Double price, Integer stock) {
        this.productId = productId;
        this.productName = productName;
        this.price = price;
        this.stockQuantity = stock;
        this.isAvailable = (stock != null && stock > 0);
        this.category = "General";
    }
    
    // Getters with null-safe returns
    public Integer getProductId() { 
        return productId; 
    }
    
    public String getProductName() { 
        return productName; 
    }
    
    public Double getPrice() {
        return price != null ? price : 0.0;
    }
    
    public Integer getStockQuantity() {
        return stockQuantity != null ? stockQuantity : 0;
    }
    
    public Boolean isAvailable() {
        return isAvailable;
    }
    
    public String getCategory() {
        return category;
    }
    
    // Calculate total value
    public double getTotalValue() {
        return getPrice() * getStockQuantity();
    }
    
    // Check if product needs restock
    public boolean needsRestock() {
        return getStockQuantity() < 10;
    }
    
    public static void main(String[] args) {
        System.out.println("=== WRAPPER CLASSES FOR NULL SAFETY ===\n");
        
        // Product with full data
        ProductInventoryDemo product1 = new ProductInventoryDemo(1, "Laptop", 999.99, 50);
        System.out.println("Product: " + product1.getProductName());
        System.out.println("Price: $" + product1.getPrice());
        System.out.println("Stock: " + product1.getStockQuantity());
        System.out.println("Available: " + product1.isAvailable());
        System.out.println("Total Value: $" + product1.getTotalValue());
        System.out.println("Needs Restock: " + product1.needsRestock());
        
        // Product with null stock (out of stock)
        ProductInventoryDemo product2 = new ProductInventoryDemo(2, "Phone", 699.99, null);
        System.out.println("\n--- Product with null stock ---");
        System.out.println("Product: " + product2.getProductName());
        System.out.println("Stock: " + product2.getStockQuantity());
        System.out.println("Available: " + product2.isAvailable());
        
        System.out.println("\n=== ANGULAR USE CASE ===");
        System.out.println("Wrapper classes are essential for:");
        System.out.println("1. Handling optional API fields");
        System.out.println("2. Managing null from database");
        System.out.println("3. Form field validation");
        System.out.println("4. TypeScript 'number | null' compatibility");
    }
}
