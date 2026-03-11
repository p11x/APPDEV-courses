// ModernJavaFeaturesDemo - Demonstrates Modern Java Features for Angular Backend
// Use case: Modern Java features for Spring Boot backend

public class ModernJavaFeaturesDemo {
    
    // Simple DTO class (alternative to records for older Java)
    static class UserDTO {
        private final int id;
        private final String name;
        private final String email;
        
        public UserDTO(int id, String name, String email) {
            this.id = id;
            this.name = name;
            this.email = email;
        }
        
        public int getId() { return id; }
        public String getName() { return name; }
        public String getEmail() { return email; }
        
        // Equals, hashCode, toString generated
        @Override
        public String toString() {
            return "UserDTO{id=" + id + ", name='" + name + "', email='" + email + "'}";
        }
    }
    
    // Functional interface for processing
    @FunctionalInterface
    interface UserProcessor {
        String process(UserDTO user);
    }
    
    // Switch expression (Java 14+)
    public static String getDayType(String day) {
        return switch (day) {
            case "Saturday", "Sunday" -> "Weekend";
            case "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" -> "Weekday";
            default -> "Invalid";
        };
    }
    
    // Text block (Java 15+)
    public static String createJSON(String name, int age) {
        return """
                {
                    "name": "%s",
                    "age": %d,
                    "active": true
                }
                """.formatted(name, age);
    }
    
    // Var keyword (Java 10+)
    public static void demonstrateVar() {
        // Type inference with var
        var users = java.util.List.of(
            new UserDTO(1, "Alice", "alice@email.com"),
            new UserDTO(2, "Bob", "bob@email.com")
        );
        
        // Enhanced for loop with var
        for (var user : users) {
            System.out.println("ID: " + user.getId() + ", Name: " + user.getName());
        }
    }
    
    // Lambda with var
    public static void demonstrateLambda() {
        UserProcessor upper = (var u) -> u.getName().toUpperCase();
        
        UserDTO user = new UserDTO(1, "John", "john@email.com");
        System.out.println("Processed: " + upper.process(user));
    }
    
    // Stream improvements
    public static void demonstrateStreams() {
        var users = java.util.List.of(
            new UserDTO(1, "Alice", "alice@email.com"),
            new UserDTO(2, "Bob", "bob@email.com"),
            new UserDTO(3, "Charlie", "charlie@email.com")
        );
        
        System.out.println("\n--- Stream Operations ---");
        
        // Filter and map
        users.stream()
            .filter(u -> u.getId() > 1)
            .map(UserDTO::getName)
            .forEach(System.out::println);
        
        // Method reference
        users.stream()
            .map(Object::toString)
            .forEach(System.out::println);
    }
    
    // Optional improvements
    public static void demonstrateOptional() {
        var users = java.util.List.of(
            new UserDTO(1, "Alice", "alice@email.com"),
            new UserDTO(2, "Bob", "bob@email.com")
        );
        
        // Find first
        var found = users.stream()
            .filter(u -> u.getId() == 1)
            .findFirst();
        
        found.ifPresentOrElse(
            u -> System.out.println("Found: " + u.getName()),
            () -> System.out.println("Not found")
        );
    }
    
    public static void main(String[] args) {
        System.out.println("=== MODERN JAVA FEATURES ===\n");
        
        // User DTO
        System.out.println("--- User DTO ---");
        var user = new UserDTO(1, "John", "john@email.com");
        System.out.println("User: " + user);
        
        // Switch expressions
        System.out.println("\n--- Switch Expressions ---");
        System.out.println("Monday: " + getDayType("Monday"));
        System.out.println("Saturday: " + getDayType("Saturday"));
        
        // Text blocks
        System.out.println("\n--- Text Blocks (JSON) ---");
        String json = createJSON("Alice", 25);
        System.out.println(json);
        
        // Var keyword
        System.out.println("\n--- Var Keyword ---");
        demonstrateVar();
        
        // Lambda
        System.out.println("\n--- Lambda with Var ---");
        demonstrateLambda();
        
        // Streams
        demonstrateStreams();
        
        // Optional
        System.out.println("\n--- Optional ---");
        demonstrateOptional();
        
        System.out.println("\n=== ANGULAR USE CASES ===");
        System.out.println("1. DTOs for API responses");
        System.out.println("2. Lambda expressions for streams");
        System.out.println("3. Text blocks for JSON generation");
        System.out.println("4. Switch expressions for status codes");
        System.out.println("5. Optional for null safety");
    }
}
