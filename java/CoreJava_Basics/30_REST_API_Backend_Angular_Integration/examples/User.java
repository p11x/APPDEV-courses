// User.java - Model class for REST API Demo
// Represents a user entity in the system

public class User {
    private int id;
    private String email;
    private String name;
    private String role;
    
    public User(int id, String email, String name, String role) {
        this.id = id;
        this.email = email;
        this.name = name;
        this.role = role;
    }
    
    public int getId() { return id; }
    public void setId(int id) { this.id = id; }
    public String getEmail() { return email; }
    public void setEmail(String email) { this.email = email; }
    public String getName() { return name; }
    public void setName(String name) { this.name = name; }
    public String getRole() { return role; }
    public void setRole(String role) { this.role = role; }
    
    // Convert to JSON format
    public String toJSON() {
        return String.format("{\"id\":%d,\"email\":\"%s\",\"name\":\"%s\",\"role\":\"%s\"}",
            id, email, name, role);
    }
}
