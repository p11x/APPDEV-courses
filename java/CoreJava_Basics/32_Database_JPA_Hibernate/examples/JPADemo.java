// JPADemo - Demonstrates JPA/Hibernate Concepts
// Object-Relational Mapping for database operations

import java.util.*;

public class JPADemo {
    
    public static void main(String[] args) {
        System.out.println("=== JPA/HIBERNATE DEMO ===\n");
        
        // Entity basics
        System.out.println("--- JPA Entity Annotations ---");
        System.out.println("@Entity - Marks class as database table");
        System.out.println("@Table - Specifies table name");
        System.out.println("@Id - Primary key");
        System.out.println("@GeneratedValue - Auto-generate ID");
        System.out.println("@Column - Column mapping");
        
        // Entity example
        System.out.println("\n--- Entity Example ---");
        System.out.println("@Entity");
        System.out.println("@Table(name = \"users\")");
        System.out.println("class User {");
        System.out.println("    @Id");
        System.out.println("    @GeneratedValue(strategy = GenerationType.IDENTITY)");
        System.out.println("    private Long id;");
        System.out.println("    ");
        System.out.println("    @Column(nullable = false)");
        System.out.println("    private String name;");
        System.out.println("    ");
        System.out.println("    @Column(unique = true)");
        System.out.println("    private String email;");
        System.out.println("}");
        
        // Relationships
        System.out.println("\n--- Relationship Annotations ---");
        System.out.println("@OneToOne - One to one relationship");
        System.out.println("@OneToMany - One to many (list/set)");
        System.out.println("@ManyToOne - Many to one");
        System.out.println("@ManyToMany - Many to many");
        System.out.println("@JoinColumn - Foreign key column");
        
        // CRUD Operations
        System.out.println("\n--- CRUD with Repository ---");
        System.out.println("save(entity) - Insert or Update");
        System.out.println("findById(id) - Find by primary key");
        System.out.println("findAll() - Find all entities");
        System.out.println("delete(entity) - Delete entity");
        System.out.println("deleteById(id) - Delete by ID");
        
        // Query Methods
        System.out.println("\n--- Query Methods ---");
        System.out.println("findByEmail(String email)");
        System.out.println("findByAgeGreaterThan(int age)");
        System.out.println("findByNameContaining(String name)");
        System.out.println("@Query(\"SELECT u FROM User u WHERE u.name = ?1\")");
        
        // Angular Integration
        System.out.println("\n=== ANGULAR INTEGRATION ===");
        System.out.println("Angular <-> REST API <-> JPA Repository <-> Database");
        System.out.println("");
        System.out.println("Angular calls: GET/POST/PUT/DELETE /api/users");
        System.out.println("Spring converts to: userRepository.findAll(), userRepository.save()");
        System.out.println("Database returns: Result set");
        System.out.println("JPA converts to: List<User>");
        System.out.println("REST converts to: JSON array");
        System.out.println("Angular receives: User[]");
        
        // Best Practices
        System.out.println("\n--- Best Practices ---");
        System.out.println("1. Use DTOs for API responses");
        System.out.println("2. Avoid exposing entities directly");
        System.out.println("3. Use pagination for large lists");
        System.out.println("4. Validate input with Bean Validation");
        System.out.println("5. Use transactions appropriately");
    }
}

// Example of a JPA Entity
// In real JPA, this would have @Entity annotation
class UserEntity {
    private Long id;
    private String name;
    private String email;
    private String password;
    private Date createdAt;
    private Date updatedAt;
    private boolean active;
    
    // Getters and setters
    public Long getId() { return id; }
    public void setId(Long id) { this.id = id; }
    
    public String getName() { return name; }
    public void setName(String name) { this.name = name; }
    
    public String getEmail() { return email; }
    public void setEmail(String email) { this.email = email; }
    
    public Date getCreatedAt() { return createdAt; }
    public void setCreatedAt(Date createdAt) { this.createdAt = createdAt; }
}
