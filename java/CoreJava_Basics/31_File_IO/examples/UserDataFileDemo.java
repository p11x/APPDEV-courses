// UserDataFileDemo - Demonstrates File I/O for User Data Storage
// Use case: Reading/writing user data files for Angular app

import java.io.*;
import java.util.*;

public class UserDataFileDemo {
    
    // Simple user class
    static class User implements Serializable {
        private int id;
        private String name;
        private String email;
        
        public User(int id, String name, String email) {
            this.id = id;
            this.name = name;
            this.email = email;
        }
        
        public int getId() { return id; }
        public String getName() { return name; }
        public String getEmail() { return email; }
        
        @Override
        public String toString() {
            return id + "," + name + "," + email;
        }
        
        public static User fromString(String line) {
            String[] parts = line.split(",");
            return new User(
                Integer.parseInt(parts[0]),
                parts[1],
                parts[2]
            );
        }
    }
    
    // Write users to CSV file
    public static void writeUsersToFile(String filename, List<User> users) {
        try (PrintWriter writer = new PrintWriter(new FileWriter(filename))) {
            for (User user : users) {
                writer.println(user);
            }
            System.out.println("Written " + users.size() + " users to " + filename);
        } catch (IOException e) {
            System.out.println("Error writing: " + e.getMessage());
        }
    }
    
    // Read users from CSV file
    public static List<User> readUsersFromFile(String filename) {
        List<User> users = new ArrayList<>();
        
        try (BufferedReader reader = new BufferedReader(new FileReader(filename))) {
            String line;
            while ((line = reader.readLine()) != null) {
                users.add(User.fromString(line));
            }
            System.out.println("Read " + users.size() + " users from " + filename);
        } catch (IOException e) {
            System.out.println("Error reading: " + e.getMessage());
        }
        
        return users;
    }
    
    // Serialize users to binary file
    public static void serializeUsers(String filename, List<User> users) {
        try (ObjectOutputStream oos = new ObjectOutputStream(
                new FileOutputStream(filename))) {
            oos.writeObject(users);
            System.out.println("Serialized " + users.size() + " users");
        } catch (IOException e) {
            System.out.println("Error serializing: " + e.getMessage());
        }
    }
    
    // Deserialize users from binary file
    @SuppressWarnings("unchecked")
    public static List<User> deserializeUsers(String filename) {
        try (ObjectInputStream ois = new ObjectInputStream(
                new FileInputStream(filename))) {
            List<User> users = (List<User>) ois.readObject();
            System.out.println("Deserialized " + users.size() + " users");
            return users;
        } catch (IOException | ClassNotFoundException e) {
            System.out.println("Error deserializing: " + e.getMessage());
            return new ArrayList<>();
        }
    }
    
    // Append single user to file
    public static void appendUser(String filename, User user) {
        try (PrintWriter writer = new PrintWriter(
                new FileWriter(filename, true))) {
            writer.println(user);
            System.out.println("Appended user: " + user.getName());
        } catch (IOException e) {
            System.out.println("Error appending: " + e.getMessage());
        }
    }
    
    public static void main(String[] args) {
        System.out.println("=== FILE I/O FOR USER DATA ===\n");
        
        // Create sample users
        List<User> users = Arrays.asList(
            new User(1, "Alice", "alice@email.com"),
            new User(2, "Bob", "bob@email.com"),
            new User(3, "Charlie", "charlie@email.com")
        );
        
        // Write to CSV
        String csvFile = "users.csv";
        System.out.println("--- Writing CSV ---");
        writeUsersToFile(csvFile, users);
        
        // Read from CSV
        System.out.println("\n--- Reading CSV ---");
        List<User> readUsers = readUsersFromFile(csvFile);
        for (User u : readUsers) {
            System.out.println(u.getId() + ": " + u.getName() + " (" + u.getEmail() + ")");
        }
        
        // Serialize
        String binFile = "users.dat";
        System.out.println("\n--- Serializing ---");
        serializeUsers(binFile, users);
        
        // Deserialize
        System.out.println("\n--- Deserializing ---");
        List<User> deserialized = deserializeUsers(binFile);
        for (User u : deserialized) {
            System.out.println(u.getId() + ": " + u.getName());
        }
        
        // Append
        System.out.println("\n--- Appending ---");
        appendUser(csvFile, new User(4, "Diana", "diana@email.com"));
        
        // Read again
        System.out.println("\n--- After Append ---");
        readUsersFromFile(csvFile);
        
        System.out.println("\n=== ANGULAR USE CASES ===");
        System.out.println("1. Export data to CSV");
        System.out.println("2. Import user lists");
        System.out.println("3. Cache data locally");
        System.out.println("4. Backup/restore functionality");
        System.out.println("5. Log file management");
    }
}
