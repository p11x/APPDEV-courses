// AngularServiceDemo - Demonstrates Inheritance for Angular Services
// Use case: Service hierarchy with inheritance

// Base Service
class BaseService {
    protected String baseUrl;
    protected int timeout;
    
    public BaseService() {
        this.baseUrl = "https://api.example.com";
        this.timeout = 30000;
    }
    
    public String getBaseUrl() {
        return baseUrl;
    }
    
    public int getTimeout() {
        return timeout;
    }
    
    // Common method for all services
    public void logRequest(String endpoint) {
        System.out.println("Request to: " + baseUrl + endpoint);
    }
}

// UserService inherits from BaseService
class UserService extends BaseService {
    private String[] userEndpoints;
    
    public UserService() {
        super(); // Call parent constructor
        this.userEndpoints = new String[] {"/users", "/users/:id", "/users/profile"};
    }
    
    public String getAllUsers() {
        logRequest("/users");
        return "GET /users - Returns all users";
    }
    
    public String getUserById(int id) {
        logRequest("/users/" + id);
        return "GET /users/" + id + " - Returns user " + id;
    }
    
    public String createUser(String userData) {
        logRequest("/users");
        return "POST /users - Creates user: " + userData;
    }
}

// AdminService extends UserService with more functionality
class AdminService extends UserService {
    private boolean isAdminMode;
    
    public AdminService() {
        super(); // Call parent constructor
        this.isAdminMode = true;
    }
    
    // Override parent method
    @Override
    public String getAllUsers() {
        logRequest("/admin/users");
        return "GET /admin/users - Returns all users (admin view)";
    }
    
    public String deleteUser(int id) {
        logRequest("/admin/users/" + id);
        return "DELETE /admin/users/" + id + " - Deletes user " + id;
    }
    
    public String getSystemStats() {
        return "System stats: 1000 users, 50 admins, 10K requests";
    }
}

public class AngularServiceDemo {
    public static void main(String[] args) {
        System.out.println("=== INHERITANCE FOR ANGULAR SERVICES ===\n");
        
        // Base service
        BaseService base = new BaseService();
        System.out.println("Base URL: " + base.getBaseUrl());
        
        // User service
        UserService userService = new UserService();
        System.out.println("\n--- UserService ---");
        System.out.println(userService.getAllUsers());
        System.out.println(userService.getUserById(1));
        
        // Admin service - inherits from UserService
        AdminService adminService = new AdminService();
        System.out.println("\n--- AdminService ---");
        System.out.println(adminService.getAllUsers()); // Overridden
        System.out.println(adminService.getUserById(1)); // Inherited
        System.out.println(adminService.deleteUser(5));
        System.out.println(adminService.getSystemStats());
        
        // Polymorphism
        System.out.println("\n--- Polymorphism ---");
        BaseService service = new AdminService();
        service.logRequest("/users"); // Calls overridden method
        
        System.out.println("\n=== USE CASES ===");
        System.out.println("1. BaseService -> HttpService");
        System.out.println("2. UserService extends BaseService");
        System.out.println("3. AdminService extends UserService");
        System.out.println("4. Override methods for custom behavior");
    }
}
