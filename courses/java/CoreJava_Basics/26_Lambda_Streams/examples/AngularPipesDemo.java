// AngularPipesDemo - Demonstrates Stream Operations as Angular Pipe Equivalents
// Use case: Data transformation in Java backend (like Angular pipes)

import java.util.*;
import java.util.stream.*;

public class AngularPipesDemo {
    
    static class User {
        private String name;
        private int age;
        private String department;
        private boolean active;
        
        public User(String name, int age, String department, boolean active) {
            this.name = name;
            this.age = age;
            this.department = department;
            this.active = active;
        }
        
        public String getName() { return name; }
        public int getAge() { return age; }
        public String getDepartment() { return department; }
        public boolean isActive() { return active; }
        
        @Override
        public String toString() {
            return name + " (" + age + ") - " + department;
        }
    }
    
    public static void main(String[] args) {
        // Sample data
        List<User> users = Arrays.asList(
            new User("Alice", 25, "Engineering", true),
            new User("Bob", 30, "Marketing", true),
            new User("Charlie", 35, "Engineering", false),
            new User("Diana", 28, "Sales", true),
            new User("Eve", 32, "Engineering", true)
        );
        
        System.out.println("=== STREAMS AS ANGULAR PIPES ===\n");
        
        // Filter (like Angular async pipe + filter)
        System.out.println("--- Filter (ngIf + filter) ---");
        List<User> activeUsers = users.stream()
            .filter(User::isActive)
            .collect(Collectors.toList());
        activeUsers.forEach(u -> System.out.println("Active: " + u));
        
        // Map (like Angular map pipe)
        System.out.println("\n--- Map (map pipe) ---");
        List<String> names = users.stream()
            .map(User::getName)
            .collect(Collectors.toList());
        System.out.println("Names: " + names);
        
        // Map with transformation
        System.out.println("\n--- Map with Transform ---");
        List<String> upperNames = users.stream()
            .map(u -> u.getName().toUpperCase())
            .collect(Collectors.toList());
        System.out.println("Upper: " + upperNames);
        
        // Sort (like Angular orderBy)
        System.out.println("\n--- Sort (orderBy pipe) ---");
        List<User> sortedByAge = users.stream()
            .sorted(Comparator.comparingInt(User::getAge))
            .collect(Collectors.toList());
        sortedByAge.forEach(u -> System.out.println("Age: " + u.getAge() + " - " + u.getName()));
        
        // Reduce (like Angular reduce pipe)
        System.out.println("\n--- Reduce (reduce pipe) ---");
        int totalAge = users.stream()
            .mapToInt(User::getAge)
            .sum();
        System.out.println("Total age: " + totalAge);
        
        double averageAge = users.stream()
            .mapToInt(User::getAge)
            .average()
            .orElse(0);
        System.out.println("Average age: " + averageAge);
        
        // Find (like Angular first/last)
        System.out.println("\n--- Find (first) ---");
        User firstEngineer = users.stream()
            .filter(u -> u.getDepartment().equals("Engineering"))
            .findFirst()
            .orElse(null);
        System.out.println("First engineer: " + firstEngineer);
        
        // Group by (like Angular groupBy)
        System.out.println("\n--- Group By ---");
        Map<String, List<User>> byDepartment = users.stream()
            .collect(Collectors.groupingBy(User::getDepartment));
        byDepartment.forEach((dept, deptUsers) -> {
            System.out.println(dept + ": " + deptUsers.size() + " users");
        });
        
        // Chaining operations (like pipe chain)
        System.out.println("\n--- Chain (pipe chain) ---");
        List<String> result = users.stream()
            .filter(User::isActive)
            .filter(u -> u.getAge() > 25)
            .sorted(Comparator.comparing(User::getName))
            .map(User::getName)
            .collect(Collectors.toList());
        System.out.println("Active over 25: " + result);
        
        System.out.println("\n=== ANGULAR PIPE EQUIVALENTS ===");
        System.out.println("1. filter -> .filter()");
        System.out.println("2. map -> .map()");
        System.out.println("3. async -> .findFirst()/findAny()");
        System.out.println("4. orderBy -> .sorted()");
        System.out.println("5. json -> .collect()");
        System.out.println("6. reduce -> .reduce()/.sum()");
        System.out.println("7. groupBy -> .groupingBy()");
    }
}
