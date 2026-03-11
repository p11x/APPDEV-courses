// AngularServiceDemo - Demonstrates Method Types for Angular Service Layer
// Use case: Backend service methods that Angular services call

public class AngularServiceDemo {
    
    // Service class representing an Angular service
    static class UserService {
        
        // Static method - Configuration/factory
        public static UserService createService() {
            System.out.println("UserService created");
            return new UserService();
        }
        
        // Instance method - Regular operation
        public String getUserById(int id) {
            return "User-" + id;
        }
        
        // Instance method - Data processing
        public String[] getAllUsers() {
            return new String[]{"Alice", "Bob", "Charlie"};
        }
        
        // Method with parameters
        public boolean validateUser(String username, String password) {
            return username.length() >= 3 && password.length() >= 6;
        }
        
        // Method with return value
        public int getUserCount() {
            return 42;
        }
        
        // Method with multiple parameters
        public String createUser(String name, String email, String role) {
            return "Created: " + name + " (" + email + ") as " + role;
        }
        
        // Method overloading
        public String getUser(int id) { return "User-" + id; }
        public String getUser(int id, boolean withDetails) { 
            return withDetails ? "User-" + id + " [detailed]" : "User-" + id; 
        }
    }
    
    // Utility class with static methods
    static class ValidationUtils {
        
        public static boolean isValidEmail(String email) {
            return email != null && email.contains("@");
        }
        
        public static boolean isValidPassword(String password) {
            return password != null && password.length() >= 8;
        }
        
        public static String sanitize(String input) {
            return input == null ? "" : input.trim();
        }
    }
    
    public static void main(String[] args) {
        System.out.println("=== METHOD TYPES FOR ANGULAR SERVICES ===\n");
        
        // Static method usage
        System.out.println("--- Static Method (Factory) ---");
        UserService service = UserService.createService();
        
        // Instance methods
        System.out.println("\n--- Instance Methods ---");
        System.out.println("getUserById(1): " + service.getUserById(1));
        
        String[] users = service.getAllUsers();
        for (String user : users) {
            System.out.println("User: " + user);
        }
        
        // Parameter methods
        System.out.println("\n--- Parameter Methods ---");
        System.out.println("validateUser: " + service.validateUser("john", "pass123"));
        System.out.println("createUser: " + service.createUser("John", "john@email.com", "USER"));
        
        // Method overloading
        System.out.println("\n--- Method Overloading ---");
        System.out.println("getUser(1): " + service.getUser(1));
        System.out.println("getUser(1, true): " + service.getUser(1, true));
        
        // Static utilities
        System.out.println("\n--- Static Utilities ---");
        System.out.println("isValidEmail: " + ValidationUtils.isValidEmail("test@email.com"));
        System.out.println("isValidPassword: " + ValidationUtils.isValidPassword("password123"));
        System.out.println("sanitize: '" + ValidationUtils.sanitize("  hello  ") + "'");
        
        System.out.println("\n=== ANGULAR SERVICE PARALLELS ===");
        System.out.println("1. HTTP calls (getUser, getAllUsers)");
        System.out.println("2. Form validation (validateUser)");
        System.out.println("3. Data transformation (sanitize)");
        System.out.println("4. Factory methods (createService)");
        System.out.println("5. Utility functions (static methods)");
    }
}
