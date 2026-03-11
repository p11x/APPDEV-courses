// AngularIntegrationDemo - Demonstrates Stream API for Angular Data Processing
// Shows how to process JSON-like data from Angular frontends

import java.util.*;
import java.util.stream.*;

public class AngularIntegrationDemo {
    
    public static void main(String[] args) {
        System.out.println("=== ANGULAR INTEGRATION DEMO ===\n");
        
        // Simulating JSON data from Angular/API
        List<Map<String, Object>> users = new ArrayList<>();
        
        Map<String, Object> user1 = new HashMap<>();
        user1.put("id", 1);
        user1.put("name", "Alice");
        user1.put("age", 25);
        user1.put("active", true);
        users.add(user1);
        
        Map<String, Object> user2 = new HashMap<>();
        user2.put("id", 2);
        user2.put("name", "Bob");
        user2.put("age", 17);
        user2.put("active", true);
        users.add(user2);
        
        Map<String, Object> user3 = new HashMap<>();
        user3.put("id", 3);
        user3.put("name", "Charlie");
        user3.put("age", 30);
        user3.put("active", false);
        users.add(user3);
        
        // Get active users above 18
        List<Map<String, Object>> activeAdults = users.stream()
            .filter(u -> (Boolean)u.get("active"))
            .filter(u -> (Integer)u.get("age") >= 18)
            .collect(Collectors.toList());
        
        System.out.println("Active adults: " + activeAdults);
        
        // Get names only
        List<String> names = users.stream()
            .map(u -> (String)u.get("name"))
            .collect(Collectors.toList());
        System.out.println("All names: " + names);
        
        System.out.println("\n=== USE CASES ===");
        System.out.println("1. Filter API responses based on user criteria");
        System.out.println("2. Transform data for Angular components");
        System.out.println("3. Process form submissions");
        System.out.println("4. Aggregate data for dashboards");
    }
}
