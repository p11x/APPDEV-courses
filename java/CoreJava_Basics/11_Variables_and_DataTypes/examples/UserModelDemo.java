// UserModelDemo - Demonstrates Data Types for Angular User Models
// Use case: Type-safe user data handling

public class UserModelDemo {
    // Primitive types for simple data
    private int userId;
    private boolean isActive;
    private double accountBalance;
    
    // Reference types for complex data
    private String userName;
    private String email;
    private String[] roles;
    
    // Constructor
    public UserModelDemo(int userId, String userName, String email, boolean isActive) {
        this.userId = userId;
        this.userName = userName;
        this.email = email;
        this.isActive = isActive;
        this.accountBalance = 0.0;
        this.roles = new String[] {"USER"};
    }
    
    // Getters and Setters
    public int getUserId() { return userId; }
    public void setUserId(int id) { this.userId = id; }
    
    public String getUserName() { return userName; }
    public void setUserName(String name) { this.userName = name; }
    
    public String getEmail() { return email; }
    public void setEmail(String email) { this.email = email; }
    
    public boolean isActive() { return isActive; }
    public void setActive(boolean active) { this.isActive = active; }
    
    public double getAccountBalance() { return accountBalance; }
    public void setAccountBalance(double balance) { this.accountBalance = balance; }
    
    public String[] getRoles() { return roles; }
    public void addRole(String role) {
        String[] newRoles = new String[roles.length + 1];
        System.arraycopy(roles, 0, newRoles, 0, roles.length);
        newRoles[roles.length] = role;
        this.roles = newRoles;
    }
    
    // Convert to JSON-like string for Angular
    public String toJSON() {
        StringBuilder json = new StringBuilder();
        json.append("{");
        json.append("\"userId\":").append(userId).append(",");
        json.append("\"userName\":\"").append(userName).append("\",");
        json.append("\"email\":\"").append(email).append("\",");
        json.append("\"isActive\":").append(isActive).append(",");
        json.append("\"accountBalance\":").append(accountBalance);
        json.append("}");
        return json.toString();
    }
    
    public static void main(String[] args) {
        System.out.println("=== DATA TYPES FOR ANGULAR MODELS ===\n");
        
        // Create user
        UserModelDemo user = new UserModelDemo(1, "Alice", "alice@example.com", true);
        user.setAccountBalance(100.50);
        
        // Display user data
        System.out.println("User ID: " + user.getUserId());
        System.out.println("Name: " + user.getUserName());
        System.out.println("Email: " + user.getEmail());
        System.out.println("Active: " + user.isActive());
        System.out.println("Balance: $" + user.getAccountBalance());
        
        // Send to Angular
        System.out.println("\n--- JSON Output ---");
        System.out.println(user.toJSON());
        
        System.out.println("\n=== DATA TYPE MAPPING ===");
        System.out.println("int -> number (TypeScript)");
        System.out.println("boolean -> boolean");
        System.out.println("double -> number");
        System.out.println("String -> string");
        System.out.println("String[] -> string[]");
    }
}
