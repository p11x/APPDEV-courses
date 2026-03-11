// UserModelDemo - Demonstrates Encapsulation for Angular Data Models
// Use case: Protected data models for Angular frontend

public class UserModelDemo {
    
    // Fully encapsulated User class
    static class User {
        // Private fields (data hiding)
        private int id;
        private String name;
        private String email;
        private String password;
        private boolean active;
        
        // Public constructor
        public User(int id, String name, String email) {
            this.id = id;
            this.name = name;
            this.email = email;
            this.active = true;
        }
        
        // Public getters (read access)
        public int getId() { return id; }
        public String getName() { return name; }
        public String getEmail() { return email; }
        public boolean isActive() { return active; }
        
        // Setters with validation
        public void setName(String name) {
            if (name != null && !name.trim().isEmpty()) {
                this.name = name;
            }
        }
        
        public void setEmail(String email) {
            if (email != null && email.contains("@")) {
                this.email = email;
            }
        }
        
        public void setActive(boolean active) {
            this.active = active;
        }
        
        // Secure password setter
        public void setPassword(String password) {
            // Could add encryption here
            this.password = password;
        }
        
        // No getter for password (security)
        public boolean verifyPassword(String password) {
            return this.password != null && this.password.equals(password);
        }
    }
    
    // User DTO for API responses (without sensitive data)
    static class UserDTO {
        private int id;
        private String name;
        private String email;
        
        public UserDTO(int id, String name, String email) {
            this.id = id;
            this.name = name;
            this.email = email;
        }
        
        public int getId() { return id; }
        public String getName() { return name; }
        public String getEmail() { return email; }
        
        // Factory method to create DTO from entity
        public static UserDTO fromUser(User user) {
            return new UserDTO(user.getId(), user.getName(), user.getEmail());
        }
    }
    
    public static void main(String[] args) {
        System.out.println("=== ENCAPSULATION FOR DATA MODELS ===\n");
        
        // Create user with constructor
        User user = new User(1, "John", "john@email.com");
        user.setPassword("secret123");
        
        // Access through public methods
        System.out.println("--- Reading Data ---");
        System.out.println("ID: " + user.getId());
        System.out.println("Name: " + user.getName());
        System.out.println("Email: " + user.getEmail());
        System.out.println("Active: " + user.isActive());
        
        // Update through setters with validation
        System.out.println("\n--- Updating Data ---");
        user.setName("John Doe");
        System.out.println("Updated name: " + user.getName());
        
        // Invalid update (empty name)
        user.setName("");
        System.out.println("After empty name: " + user.getName());
        
        // Password handling (no getter!)
        System.out.println("\n--- Password Handling ---");
        System.out.println("Password verify (correct): " + user.verifyPassword("secret123"));
        System.out.println("Password verify (wrong): " + user.verifyPassword("wrong"));
        
        // Create DTO from entity (for API response)
        System.out.println("\n--- DTO for Angular ---");
        UserDTO dto = UserDTO.fromUser(user);
        System.out.println("DTO ID: " + dto.getId());
        System.out.println("DTO Name: " + dto.getName());
        
        // Trying to access password would fail - not available!
        // user.getPassword() - compile error
        
        System.out.println("\n=== ANGULAR BENEFITS ===");
        System.out.println("1. Protected API responses (no passwords)");
        System.out.println("2. Validation before updates");
        System.out.println("3. Data integrity (private fields)");
        System.out.println("4. Controlled access to sensitive data");
        System.out.println("5. Type safety with DTOs");
    }
}
