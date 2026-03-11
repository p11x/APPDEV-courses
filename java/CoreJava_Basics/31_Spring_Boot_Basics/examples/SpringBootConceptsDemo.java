// SpringBootConceptsDemo - Demonstrates Spring Boot Concepts
// Conceptual demo of Spring Boot for Angular developers

import java.util.*;

public class SpringBootConceptsDemo {
    
    public static void main(String[] args) {
        System.out.println("=== SPRING BOOT CONCEPTS DEMO ===\n");
        
        // Dependency Injection (Conceptual)
        System.out.println("--- Dependency Injection ---");
        // Instead of: UserService service = new UserService();
        // Spring provides: @Autowired UserService service
        
        // Inversion of Control Container
        System.out.println("\n--- IoC Container ---");
        System.out.println("Spring manages object lifecycle");
        System.out.println("Beans are created and injected automatically");
        
        // Bean Scopes
        System.out.println("\n--- Bean Scopes ---");
        System.out.println("@Singleton - One instance per container");
        System.out.println("@Prototype - New instance each time");
        System.out.println("@Request - One per HTTP request");
        System.out.println("@Session - One per HTTP session");
        
        // REST Controller
        System.out.println("\n--- REST Controller Pattern ---");
        System.out.println("@RestController = @Controller + @ResponseBody");
        
        // HTTP Methods mapping
        System.out.println("\n--- HTTP Method Annotations ---");
        System.out.println("@GetMapping - Read data");
        System.out.println("@PostMapping - Create data");
        System.out.println("@PutMapping - Update data");
        System.out.println("@DeleteMapping - Delete data");
        
        // Request/Response
        System.out.println("\n--- Request/Response Annotations ---");
        System.out.println("@RequestBody - JSON -> Java Object");
        System.out.println("@ResponseBody - Java Object -> JSON");
        System.out.println("@PathVariable - URL path parameters");
        System.out.println("@RequestParam - Query parameters");
        
        // Service Layer
        System.out.println("\n--- Service Layer ---");
        System.out.println("@Service - Business logic annotation");
        System.out.println("Transaction management with @Transactional");
        
        // Repository Layer
        System.out.println("\n--- Repository Layer ---");
        System.out.println("@Repository - Data access annotation");
        System.out.println("Spring Data JPA automatically generates queries");
        
        // Configuration
        System.out.println("\n--- Configuration ---");
        System.out.println("@Configuration - Java-based config");
        System.out.println("@Value - Inject properties");
        System.out.println("@ConfigurationProperties - Type-safe properties");
        
        // Angular Integration
        System.out.println("\n=== ANGULAR INTEGRATION ===");
        System.out.println("Angular HttpClient <-> Spring @RestController");
        System.out.println("JSON serialization/deserialization automatic");
        System.out.println("CORS configuration for cross-origin requests");
    }
}

// Conceptual example of how Spring would handle requests
// In real Spring Boot, these would be annotations

class UserControllerConcept {
    
    // @GetMapping("/api/users")
    public List<UserConcept> getAllUsers() {
        return Arrays.asList(
            new UserConcept(1, "alice@email.com"),
            new UserConcept(2, "bob@email.com")
        );
    }
    
    // @GetMapping("/api/users/{id}")
    public UserConcept getUserById(int id) {
        return new UserConcept(id, "user@email.com");
    }
    
    // @PostMapping("/api/users")
    public UserConcept createUser(String email) {
        return new UserConcept(99, email);
    }
}

class UserConcept {
    private int id;
    private String email;
    
    public UserConcept(int id, String email) {
        this.id = id;
        this.email = email;
    }
    
    public int getId() { return id; }
    public String getEmail() { return email; }
}
