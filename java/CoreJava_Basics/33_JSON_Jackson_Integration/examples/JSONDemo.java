// JSONDemo - Demonstrates JSON handling in Java
// JSON is the data format exchanged between Angular and Java backend

import java.util.*;

public class JSONDemo {
    
    public static void main(String[] args) {
        System.out.println("=== JSON CONCEPTS DEMO ===\n");
        
        // JSON Data Types mapping
        System.out.println("--- JSON to Java Mapping ---");
        System.out.println("string -> String");
        System.out.println("number -> Integer, Long, Double");
        System.out.println("boolean -> Boolean");
        System.out.println("null -> null");
        System.out.println("array -> List<T>");
        System.out.println("object -> Map<String, Object> or custom class");
        
        // Example: JSON Object -> Java Class
        System.out.println("\n--- Example JSON to Java ---");
        System.out.println("JSON: {");
        System.out.println("  \"id\": 1,");
        System.out.println("  \"name\": \"Alice\",");
        System.out.println("  \"email\": \"alice@example.com\",");
        System.out.println("  \"active\": true,");
        System.out.println("  \"roles\": [\"USER\", \"ADMIN\"]");
        System.out.println("}");
        
        System.out.println("\nJava class:");
        System.out.println("class User {");
        System.out.println("  private int id;");
        System.out.println("  private String name;");
        System.out.println("  private String email;");
        System.out.println("  private boolean active;");
        System.out.println("  private List<String> roles;");
        System.out.println("}");
        
        // Jackson Annotations (conceptual)
        System.out.println("\n--- Jackson Annotations ---");
        System.out.println("@JsonProperty(\"user_name\") - Property name mapping");
        System.out.println("@JsonIgnore - Ignore property");
        System.out.println("@JsonFormat - Date formatting");
        System.out.println("@JsonSerialize - Custom serialization");
        System.out.println("@JsonDeserialize - Custom deserialization");
        
        // REST API Response patterns
        System.out.println("\n--- REST API Response Patterns ---");
        
        // Single object response
        System.out.println("\nSingle object:");
        System.out.println("{ \"id\": 1, \"name\": \"Alice\" }");
        
        // List response
        System.out.println("\nList response:");
        System.out.println("[");
        System.out.println("  { \"id\": 1, \"name\": \"Alice\" },");
        System.out.println("  { \"id\": 2, \"name\": \"Bob\" }");
        System.out.println("]");
        
        // Paginated response
        System.out.println("\nPaginated response:");
        System.out.println("{");
        System.out.println("  \"content\": [... ],");
        System.out.println("  \"page\": 0,");
        System.out.println("  \"size\": 10,");
        System.out.println("  \"totalElements\": 100,");
        System.out.println("  \"totalPages\": 10");
        System.out.println("}");
        
        // Error response
        System.out.println("\nError response:");
        System.out.println("{");
        System.out.println("  \"status\": 404,");
        System.out.println("  \"error\": \"Not Found\",");
        System.out.println("  \"message\": \"User not found\",");
        System.out.println("  \"timestamp\": \"2024-01-15T10:30:00\"");
        System.out.println("}");
        
        // Angular HttpClient usage
        System.out.println("\n=== ANGULAR INTEGRATION ===");
        System.out.println("// Angular service call");
        System.out.println("this.http.get<User[]>('/api/users').subscribe(users => ...)");
        System.out.println("");
        System.out.println("// Angular POST");
        System.out.println("this.http.post('/api/users', userData).subscribe(...)");
    }
}

// Example: What a typical Angular interface maps to in Java
class UserEntity {
    private int id;
    private String name;
    private String email;
    private boolean active;
    private List<String> roles;
    private Date createdAt;
    
    // Getters and setters (Jackson uses these)
    public int getId() { return id; }
    public void setId(int id) { this.id = id; }
    
    public String getName() { return name; }
    public void setName(String name) { this.name = name; }
    
    public String getEmail() { return email; }
    public void setEmail(String email) { this.email = email; }
    
    public boolean isActive() { return active; }
    public void setActive(boolean active) { this.active = active; }
    
    public List<String> getRoles() { return roles; }
    public void setRoles(List<String> roles) { this.roles = roles; }
    
    public Date getCreatedAt() { return createdAt; }
    public void setCreatedAt(Date createdAt) { this.createdAt = createdAt; }
}
