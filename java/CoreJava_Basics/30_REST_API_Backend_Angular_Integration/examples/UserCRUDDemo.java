// UserCRUDDemo.java - REST API Create, Update, Delete operations Demo
// Shows POST, PUT, DELETE operations for Angular integration

import java.util.*;

public class UserCRUDDemo {
    
    // Simulated database
    private static List<User> users = new ArrayList<>();
    
    static {
        users.add(new User(1, "alice@email.com", "Alice Smith", "USER"));
        users.add(new User(2, "bob@email.com", "Bob Johnson", "ADMIN"));
        users.add(new User(3, "charlie@email.com", "Charlie Brown", "USER"));
    }
    
    // ===== CREATE USER (POST /api/users) =====
    public static User createUser(String email, String name, String role) {
        int newId = users.size() + 1;
        User newUser = new User(newId, email, name, role);
        users.add(newUser);
        return newUser;
    }
    
    // ===== UPDATE USER (PUT /api/users/{id}) =====
    public static User updateUser(int id, String name, String role) {
        for (User user : users) {
            if (user.getId() == id) {
                user.setName(name);
                user.setRole(role);
                return user;
            }
        }
        return null;
    }
    
    // ===== DELETE USER (DELETE /api/users/{id}) =====
    public static boolean deleteUser(int id) {
        return users.removeIf(u -> u.getId() == id);
    }
    
    // Main demo for CRUD operations
    public static void main(String[] args) {
        System.out.println("=== REST API CRUD DEMO ===\n");
        
        // POST create
        System.out.println("--- POST /api/users ---");
        User newUser = createUser("diana@email.com", "Diana Prince", "USER");
        System.out.println(newUser.toJSON());
        
        // PUT update
        System.out.println("\n--- PUT /api/users/3 ---");
        User updated = updateUser(3, "Charlie Wilson", "ADMIN");
        System.out.println(updated.toJSON());
        
        // DELETE
        System.out.println("\n--- DELETE /api/users/2 ---");
        boolean deleted = deleteUser(2);
        System.out.println("Deleted: " + deleted);
        
        System.out.println("\n=== FINAL USER LIST ===");
        for (User u : users) {
            System.out.println(u.toJSON());
        }
        
        System.out.println("\n=== KEY HTTP METHODS ===");
        System.out.println("POST /api/users - Create new user");
        System.out.println("PUT /api/users/{id} - Update existing user");
        System.out.println("DELETE /api/users/{id} - Delete user");
    }
}
