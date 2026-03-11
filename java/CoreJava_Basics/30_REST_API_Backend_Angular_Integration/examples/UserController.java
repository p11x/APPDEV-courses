// UserController.java - REST API GET operations Demo
// Shows how Java backend returns JSON for Angular

import java.util.*;
import java.util.stream.*;

public class UserController {
    
    // Simulated database
    private static List<User> users = new ArrayList<>();
    
    static {
        users.add(new User(1, "alice@email.com", "Alice Smith", "USER"));
        users.add(new User(2, "bob@email.com", "Bob Johnson", "ADMIN"));
        users.add(new User(3, "charlie@email.com", "Charlie Brown", "USER"));
    }
    
    // ===== GET ALL USERS (GET /api/users) =====
    public static List<User> getAllUsers() {
        return users;
    }
    
    // ===== GET USER BY ID (GET /api/users/{id}) =====
    public static User getUserById(int id) {
        return users.stream()
            .filter(u -> u.getId() == id)
            .findFirst()
            .orElse(null);
    }
    
    // ===== SEARCH USERS (GET /api/users?role=ADMIN) =====
    public static List<User> searchByRole(String role) {
        return users.stream()
            .filter(u -> u.getRole().equalsIgnoreCase(role))
            .collect(Collectors.toList());
    }
    
    // Main demo for GET operations
    public static void main(String[] args) {
        System.out.println("=== REST API GET DEMO ===\n");
        
        // GET all users
        System.out.println("--- GET /api/users ---");
        List<User> allUsers = getAllUsers();
        for (User u : allUsers) {
            System.out.println(u.toJSON());
        }
        
        // GET by ID
        System.out.println("\n--- GET /api/users/1 ---");
        User user1 = getUserById(1);
        System.out.println(user1.toJSON());
        
        // Search by role
        System.out.println("\n--- GET /api/users?role=ADMIN ---");
        List<User> admins = searchByRole("ADMIN");
        for (User u : admins) {
            System.out.println(u.toJSON());
        }
    }
}
