// Product - E-commerce product class demonstrating OOP
// Used for building product catalogs in Java backend
// Maps directly to Angular Product interface

public class Product {
    // Fields
    private int productId;
    private String productName;
    private String category;
    private double price;
    private int stockQuantity;
    private boolean inStock;
    
    // Default constructor
    public Product() {
        this.productId = 0;
        this.productName = "Unknown";
        this.category = "General";
        this.price = 0.0;
        this.stockQuantity = 0;
        this.inStock = false;
    }
    
    // Parameterized constructor
    public Product(int productId, String productName, String category, 
                   double price, int stockQuantity) {
        this.productId = productId;
        this.productName = productName;
        this.category = category;
        this.price = price;
        this.stockQuantity = stockQuantity;
        this.inStock = stockQuantity > 0;
    }
    
    // Getters
    public int getProductId() { return productId; }
    public String getProductName() { return productName; }
    public String getCategory() { return category; }
    public double getPrice() { return price; }
    public int getStockQuantity() { return stockQuantity; }
    public boolean isInStock() { return inStock; }
    
    // Setters
    public void setPrice(double price) {
        if (price > 0) this.price = price;
    }
    
    public void setStockQuantity(int quantity) {
        this.stockQuantity = quantity;
        this.inStock = quantity > 0;
    }
    
    // Business methods
    public double calculateDiscount(double discountPercent) {
        return price * (1 - discountPercent / 100);
    }
    
    public boolean isAvailable(int requiredQuantity) {
        return stockQuantity >= requiredQuantity;
    }
    
    public void displayProduct() {
        System.out.println("=== PRODUCT DETAILS ===");
        System.out.println("ID: " + productId);
        System.out.println("Name: " + productName);
        System.out.println("Category: " + category);
        System.out.println("Price: $" + price);
        System.out.println("Stock: " + stockQuantity);
        System.out.println("In Stock: " + (inStock ? "Yes" : "No"));
    }
    
    public static void main(String[] args) {
        System.out.println("=== PRODUCT CLASS DEMO ===\n");
        
        Product laptop = new Product(101, "MacBook Pro", "Electronics", 1299.99, 50);
        laptop.displayProduct();
        
        System.out.println("\nDiscounted Price: $" + laptop.calculateDiscount(10));
        System.out.println("Available (5 units): " + laptop.isAvailable(5));
        
        Product outOfStock = new Product(102, "Vintage Camera", "Antiques", 500.00, 0);
        System.out.println("\n" + outOfStock.getProductName() + " in stock: " + outOfStock.isInStock());
    }
}
