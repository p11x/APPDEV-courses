// JDBCDemo - Java Database Connectivity
// Demonstrates database connections in Java

public class JDBCDemo {
    
    public static void main(String[] args) {
        System.out.println("=== JDBC DEMO ===\n");
        
        // JDBC Steps
        System.out.println("--- JDBC Steps ---");
        System.out.println("1. Load driver");
        System.out.println("2. Establish connection");
        System.out.println("3. Create statement");
        System.out.println("4. Execute query");
        System.out.println("5. Process results");
        System.out.println("6. Close connection");
        
        // Connection URL format
        System.out.println("\n--- Connection URL ---");
        System.out.println("MySQL: jdbc:mysql://localhost:3306/mydb");
        System.out.println("PostgreSQL: jdbc:postgresql://localhost:5432/mydb");
        System.out.println("Oracle: jdbc:oracle:thin:@localhost:1521:mydb");
        
        // SQL Operations
        System.out.println("\n--- SQL Operations ---");
        System.out.println("SELECT -> ResultSet");
        System.out.println("INSERT/UPDATE/DELETE -> int (affected rows)");
        
        // Example queries (pseudo-code)
        System.out.println("\n--- Example Queries ---");
        System.out.println("SELECT * FROM users WHERE id = ?");
        System.out.println("INSERT INTO users (name, email) VALUES (?, ?)");
        System.out.println("UPDATE users SET name = ? WHERE id = ?");
        System.out.println("DELETE FROM users WHERE id = ?");
        
        // Try-with-resources (recommended)
        System.out.println("\n--- Modern Approach ---");
        System.out.println("try (Connection conn = DriverManager.getConnection(url)) {");
        System.out.println("    // code");
        System.out.println("} // Auto-closes resources");
        
        // Angular Integration
        System.out.println("\n=== ANGULAR INTEGRATION ===");
        System.out.println("Java JDBC -> Repository Layer");
        System.out.println("Angular HTTP calls -> REST API endpoints");
        System.out.println("Database <-> JDBC <-> Java <-> REST <-> Angular");
    }
}
