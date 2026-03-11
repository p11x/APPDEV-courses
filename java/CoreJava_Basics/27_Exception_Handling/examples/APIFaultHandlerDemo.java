// APIFaultHandlerDemo - Demonstrates Exception Handling for Angular API Calls
// Use case: Proper error handling for Angular HTTP requests

public class APIFaultHandlerDemo {
    
    // Custom exception for API errors
    static class APIException extends Exception {
        private int statusCode;
        
        public APIException(String message, int statusCode) {
            super(message);
            this.statusCode = statusCode;
        }
        
        public int getStatusCode() {
            return statusCode;
        }
    }
    
    // Custom exception for validation
    static class ValidationException extends Exception {
        public ValidationException(String message) {
            super(message);
        }
    }
    
    // Simulate user service
    static class UserService {
        
        public String getUser(int id) throws ValidationException, APIException {
            if (id <= 0) {
                throw new ValidationException("Invalid user ID");
            }
            if (id > 1000) {
                throw new APIException("User not found", 404);
            }
            return "User-" + id;
        }
        
        public String createUser(String name) throws ValidationException, APIException {
            if (name == null || name.trim().isEmpty()) {
                throw new ValidationException("Name is required");
            }
            if (name.length() < 2) {
                throw new ValidationException("Name too short");
            }
            if (name.equals("error")) {
                throw new APIException("Server error", 500);
            }
            return "Created: " + name;
        }
    }
    
    // Handler with try-catch-finally
    public static void handleGetUser(int id) {
        UserService service = new UserService();
        
        try {
            String user = service.getUser(id);
            System.out.println("SUCCESS: " + user);
        } catch (ValidationException e) {
            System.out.println("VALIDATION ERROR: " + e.getMessage());
            // Send 400 to Angular
        } catch (APIException e) {
            System.out.println("API ERROR [" + e.getStatusCode() + "]: " + e.getMessage());
            // Send appropriate status to Angular
        } catch (Exception e) {
            System.out.println("UNKNOWN ERROR: " + e.getMessage());
            // Send 500 to Angular
        } finally {
            System.out.println("Request completed\n");
        }
    }
    
    // Handle create user
    public static void handleCreateUser(String name) {
        UserService service = new UserService();
        
        try {
            String result = service.createUser(name);
            System.out.println("SUCCESS: " + result);
        } catch (ValidationException e) {
            System.out.println("VALIDATION ERROR: " + e.getMessage());
        } catch (APIException e) {
            System.out.println("API ERROR [" + e.getStatusCode() + "]: " + e.getMessage());
        } catch (Exception e) {
            System.out.println("UNKNOWN ERROR: " + e.getMessage());
        } finally {
            System.out.println("Request completed\n");
        }
    }
    
    // Method with throws
    public static String getUserSafe(int id) throws ValidationException, APIException {
        UserService service = new UserService();
        return service.getUser(id);
    }
    
    public static void main(String[] args) {
        System.out.println("=== EXCEPTION HANDLING FOR ANGULAR API ===\n");
        
        // Test various scenarios
        System.out.println("--- Valid ID ---");
        handleGetUser(1);
        
        System.out.println("--- Invalid ID (0) ---");
        handleGetUser(0);
        
        System.out.println("--- Not Found (ID > 1000) ---");
        handleGetUser(1001);
        
        System.out.println("--- Create Valid User ---");
        handleCreateUser("John");
        
        System.out.println("--- Create Empty User ---");
        handleCreateUser("");
        
        System.out.println("--- Create User 'error' ---");
        handleCreateUser("error");
        
        // Using throws
        System.out.println("--- Using throws keyword ---");
        try {
            String user = getUserSafe(5);
            System.out.println("Got: " + user);
        } catch (ValidationException e) {
            System.out.println("Validation Error: " + e.getMessage());
        } catch (APIException e) {
            System.out.println("Error: " + e.getMessage());
        }
        
        System.out.println("\n=== ANGULAR ERROR HANDLING ===");
        System.out.println("1. HttpErrorResponse handling");
        System.out.println("2. Form validation errors");
        System.out.println("3. 400/401/403/404/500 status codes");
        System.out.println("4. User-friendly error messages");
        System.out.println("5. Error logging and reporting");
    }
}
