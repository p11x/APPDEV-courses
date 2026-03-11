// UserFactoryDemo - Demonstrates Static Factory Methods for Angular Services
// Use case: Creating service instances with predefined configurations

public class UserFactoryDemo {
    private String name;
    private String role;
    private boolean isActive;
    
    // Private constructor - must use factory
    private UserFactoryDemo(String name, String role, boolean isActive) {
        this.name = name;
        this.role = role;
        this.isActive = isActive;
    }
    
    // Static factory method - creates admin users
    public static UserFactoryDemo createAdmin(String name) {
        return new UserFactoryDemo(name, "ADMIN", true);
    }
    
    // Static factory method - creates regular users
    public static UserFactoryDemo createUser(String name) {
        return new UserFactoryDemo(name, "USER", true);
    }
    
    // Static factory method - creates guest users
    public static UserFactoryDemo createGuest() {
        return new UserFactoryDemo("Guest", "GUEST", false);
    }
    
    // Static factory method - creates user from JSON (Angular data)
    public static UserFactoryDemo fromJSON(String json) {
        // Simulate parsing JSON from Angular
        String[] parts = json.replace("{\"name\":\"", "").replace("\"}", "").split("\",\"role\":\"");
        return new UserFactoryDemo(parts[0], parts[1], true);
    }
    
    public void display() {
        System.out.println("User: " + name + " | Role: " + role + " | Active: " + isActive);
    }
    
    public static void main(String[] args) {
        System.out.println("=== STATIC FACTORY METHODS FOR SERVICES ===\n");
        
        // Use factory methods
        UserFactoryDemo admin = UserFactoryDemo.createAdmin("Alice");
        UserFactoryDemo user = UserFactoryDemo.createUser("Bob");
        UserFactoryDemo guest = UserFactoryDemo.createGuest();
        
        admin.display();
        user.display();
        guest.display();
        
        // Create from Angular JSON
        System.out.println("\n--- From Angular JSON ---");
        UserFactoryDemo fromJson = UserFactoryDemo.fromJSON("{\"name\":\"Charlie\",\"role\":\"USER\"}");
        fromJson.display();
        
        System.out.println("\n=== ANGULAR USE CASE ===");
        System.out.println("Factory methods are perfect for:");
        System.out.println("1. Creating standardized API responses");
        System.out.println("2. Building user roles (Admin, User, Guest)");
        System.out.println("3. Converting JSON to Java objects");
        System.out.println("4. Service layer object creation");
    }
}
