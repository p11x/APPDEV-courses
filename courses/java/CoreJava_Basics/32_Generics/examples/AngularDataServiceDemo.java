// AngularDataServiceDemo - Demonstrates Generics for Type-Safe Data Services
// Use case: Generic data services for Angular HTTP responses

public class AngularDataServiceDemo {
    
    // Generic API Response class
    static class ApiResponse<T> {
        private boolean success;
        private T data;
        private String message;
        
        public ApiResponse(boolean success, T data, String message) {
            this.success = success;
            this.data = data;
            this.message = message;
        }
        
        public boolean isSuccess() { return success; }
        public T getData() { return data; }
        public String getMessage() { return message; }
        
        public static <T> ApiResponse<T> ok(T data) {
            return new ApiResponse<>(true, data, "Success");
        }
        
        public static <T> ApiResponse<T> error(String message) {
            return new ApiResponse<>(false, null, message);
        }
    }
    
    // Generic User class
    static class User {
        private int id;
        private String name;
        
        public User(int id, String name) {
            this.id = id;
            this.name = name;
        }
        
        public int getId() { return id; }
        public String getName() { return name; }
        
        @Override
        public String toString() {
            return "User{id=" + id + ", name='" + name + "'}";
        }
    }
    
    // Generic Repository
    static class Repository<T> {
        private java.util.List<T> items = new java.util.ArrayList<>();
        
        public void add(T item) {
            items.add(item);
        }
        
        public T findById(int id) {
            // Simple implementation
            return items.get(id);
        }
        
        public java.util.List<T> findAll() {
            return new java.util.ArrayList<>(items);
        }
        
        public void remove(int id) {
            if (id < items.size()) {
                items.remove(id);
            }
        }
    }
    
    // Generic Service
    static class UserService {
        private Repository<User> userRepo = new Repository<>();
        
        public void addUser(User user) {
            userRepo.add(user);
        }
        
        public User getUser(int id) {
            return userRepo.findById(id);
        }
        
        public ApiResponse<User> getUserResponse(int id) {
            User user = userRepo.findById(id);
            if (user != null) {
                return ApiResponse.ok(user);
            }
            return ApiResponse.error("User not found");
        }
        
        public ApiResponse<java.util.List<User>> getAllUsersResponse() {
            return ApiResponse.ok(userRepo.findAll());
        }
    }
    
    public static void main(String[] args) {
        System.out.println("=== GENERICS FOR ANGULAR DATA SERVICES ===\n");
        
        // Create user service
        UserService service = new UserService();
        
        // Add users
        service.addUser(new User(1, "Alice"));
        service.addUser(new User(2, "Bob"));
        service.addUser(new User(3, "Charlie"));
        
        // Get single user
        System.out.println("--- Single User ---");
        User user = service.getUser(0);
        System.out.println("Found: " + user);
        
        // Get response with generics
        System.out.println("\n--- API Response with Generics ---");
        ApiResponse<User> response = service.getUserResponse(1);
        System.out.println("Success: " + response.isSuccess());
        System.out.println("Data: " + response.getData());
        System.out.println("Message: " + response.getMessage());
        
        // Get all users response
        System.out.println("\n--- All Users Response ---");
        ApiResponse<java.util.List<User>> allResponse = service.getAllUsersResponse();
        System.out.println("Success: " + allResponse.isSuccess());
        for (User u : allResponse.getData()) {
            System.out.println("- " + u);
        }
        
        // Error response
        System.out.println("\n--- Error Response ---");
        ApiResponse<User> errorResponse = service.getUserResponse(999);
        System.out.println("Success: " + errorResponse.isSuccess());
        System.out.println("Message: " + errorResponse.getMessage());
        
        System.out.println("\n=== ANGULAR USE CASES ===");
        System.out.println("1. Type-safe HTTP responses");
        System.out.println("2. Generic services (UserService, ProductService)");
        System.out.println("3. Reusable data access layers");
        System.out.println("4. Type-safe caching");
        System.out.println("5. Generic form components");
    }
}
