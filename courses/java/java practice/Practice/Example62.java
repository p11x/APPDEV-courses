/*
 * SUB TOPIC: Java StringBuilder and StringBuffer - Detailed
 * 
 * DEFINITION:
 * StringBuilder and StringBuffer are mutable alternatives to String. StringBuilder is faster but not 
 * thread-safe, while StringBuffer is synchronized for multi-threaded use.
 * 
 * FUNCTIONALITIES:
 * 1. append - Add to end
 * 2. insert - Insert at position
 * 3. delete - Remove characters
 * 4. reverse - Reverse content
 * 5. replace - Replace portion
 * 6. capacity management
 */

public class Example62 {
    public static void main(String[] args) {
        
        // StringBuilder
        System.out.println("=== StringBuilder ===");
        
        StringBuilder sb = new StringBuilder();
        sb.append("Hello");
        sb.append(" World");
        System.out.println("Append: " + sb);
        
        // Insert
        System.out.println("\n=== Insert ===");
        sb = new StringBuilder("Hello World");
        sb.insert(5, ",");
        System.out.println("Insert: " + sb);
        
        // Delete
        System.out.println("\n=== Delete ===");
        sb = new StringBuilder("Hello World");
        sb.delete(5, 6);
        System.out.println("Delete: " + sb);
        
        // Reverse
        System.out.println("\n=== Reverse ===");
        sb = new StringBuilder("Java");
        sb.reverse();
        System.out.println("Reverse: " + sb);
        
        // Replace
        System.out.println("\n=== Replace ===");
        sb = new StringBuilder("Hello World");
        sb.replace(6, 11, "Java");
        System.out.println("Replace: " + sb);
        
        // Capacity
        System.out.println("\n=== Capacity ===");
        StringBuilder sb2 = new StringBuilder();
        System.out.println("Default capacity: " + sb2.capacity());
        
        sb2 = new StringBuilder(50);
        System.out.println("With size 50: " + sb2.capacity());
        
        // Real-time Example 1: Build URL
        System.out.println("\n=== Example 1: Build URL ===");
        
        StringBuilder url = new StringBuilder("https://api.example.com");
        url.append("/users");
        url.append("?page=1");
        url.append("&limit=10");
        System.out.println("URL: " + url);
        
        // Real-time Example 2: SQL query
        System.out.println("\n=== Example 2: SQL Query ===");
        
        String table = "users";
        StringBuilder query = new StringBuilder("SELECT * FROM ");
        query.append(table);
        query.append(" WHERE active=1");
        System.out.println("Query: " + query);
        
        // Real-time Example 3: JSON building
        System.out.println("\n=== Example 3: JSON ===");
        
        StringBuilder json = new StringBuilder("{");
        json.append("\"name\":\"John\",");
        json.append("\"age\":30");
        json.append("}");
        System.out.println("JSON: " + json);
        
        // Real-time Example 4: Password validation
        System.out.println("\n=== Example 4: Pattern ===");
        
        StringBuilder pattern = new StringBuilder();
        pattern.append("^");
        pattern.append("[a-zA-Z0-9]+");
        pattern.append("@");
        pattern.append("[a-zA-Z0-9]+");
        pattern.append("\\.[a-zA-Z]{2,}$");
        System.out.println("Pattern: " + pattern);
        
        // Real-time Example 5: Log builder
        System.out.println("\n=== Example 5: Log ===");
        
        StringBuilder log = new StringBuilder();
        log.append("[ERROR] ");
        log.append("2024-01-01 ");
        log.append("User login failed");
        System.out.println("Log: " + log);
        
        // Real-time Example 6: StringBuffer (thread-safe)
        System.out.println("\n=== StringBuffer ===");
        
        StringBuffer sbf = new StringBuffer("Thread");
        sbf.append("Safe");
        System.out.println("StringBuffer: " + sbf);
    }
}
