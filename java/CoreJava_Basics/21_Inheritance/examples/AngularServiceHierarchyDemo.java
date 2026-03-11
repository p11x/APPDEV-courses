// AngularServiceHierarchyDemo - Demonstrates Inheritance for Service Hierarchy
// Use case: Angular service inheritance patterns with Java backend

public class AngularServiceHierarchyDemo {
    
    // Base service class
    static class BaseService {
        protected String baseUrl;
        
        public BaseService() {
            this.baseUrl = "https://api.example.com";
        }
        
        protected String getFullUrl(String endpoint) {
            return baseUrl + endpoint;
        }
        
        public void log(String message) {
            System.out.println("[BASE] " + message);
        }
    }
    
    // User service extending base
    static class UserService extends BaseService {
        
        public UserService() {
            super(); // Call parent constructor
        }
        
        public String getUsers() {
            log("Fetching users from: " + getFullUrl("/users"));
            return "User list data";
        }
        
        public String getUserById(int id) {
            log("Fetching user " + id);
            return "User-" + id;
        }
        
        // Method overriding
        public void log(String message) {
            System.out.println("[USER] " + message);
        }
    }
    
    // Product service extending base
    static class ProductService extends BaseService {
        
        public ProductService() {
            this.baseUrl = "https://api.example.com/products";
        }
        
        public String getProducts() {
            log("Fetching products from: " + getFullUrl("/all"));
            return "Product list data";
        }
        
        public String getProductById(int id) {
            log("Fetching product " + id);
            return "Product-" + id;
        }
        
        @Override
        public void log(String message) {
            System.out.println("[PRODUCT] " + message);
        }
    }
    
    // Admin service extending UserService
    static class AdminService extends UserService {
        
        public AdminService() {
            this.baseUrl = "https://admin.api.example.com";
        }
        
        public String getAllData() {
            log("Admin access to all data");
            return "All sensitive data";
        }
        
        public String deleteUser(int id) {
            log("Deleting user " + id);
            return "Deleted";
        }
    }
    
    public static void main(String[] args) {
        System.out.println("=== INHERITANCE FOR SERVICE HIERARCHY ===\n");
        
        // Regular user service
        System.out.println("--- User Service ---");
        UserService userService = new UserService();
        userService.getUsers();
        userService.getUserById(1);
        
        // Product service
        System.out.println("\n--- Product Service ---");
        ProductService productService = new ProductService();
        productService.getProducts();
        productService.getProductById(1);
        
        // Admin service (multi-level inheritance)
        System.out.println("\n--- Admin Service (Multi-level) ---");
        AdminService adminService = new AdminService();
        adminService.getUsers();  // Inherited from UserService
        adminService.getAllData(); // Own method
        adminService.deleteUser(5); // Admin specific
        
        // Demonstrate inheritance chain
        System.out.println("\n--- Inheritance Chain ---");
        System.out.println("AdminService extends UserService extends BaseService");
        System.out.println("AdminService has access to:");
        System.out.println("- BaseService methods: getFullUrl()");
        System.out.println("- UserService methods: getUsers(), getUserById()");
        System.out.println("- AdminService methods: getAllData(), deleteUser()");
        
        System.out.println("\n=== ANGULAR PARALLELS ===");
        System.out.println("1. UserService extends BaseHttpService");
        System.out.println("2. ProductService extends BaseHttpService");
        System.out.println("3. AdminService extends UserService (privileges)");
        System.out.println("4. Shared authentication logic in base");
        System.out.println("5. Override HTTP interceptors");
    }
}
