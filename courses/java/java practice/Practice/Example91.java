/*
 * SUB TOPIC: Iterator Interface in Java
 * 
 * DEFINITION:
 * Iterator is an interface in the Java Collections Framework that provides a way to access 
 * elements of a collection sequentially without exposing its underlying representation. 
 * It allows forward-only traversal and optional removal of elements during iteration.
 * 
 * FUNCTIONALITIES:
 * 1. hasNext() - Returns true if iteration has more elements
 * 2. next() - Returns the next element in the iteration
 * 3. remove() - Removes the last element returned by iterator (optional operation)
 * 4. forEachRemaining() - Performs action for each remaining element
 */

import java.util.*;

public class Example91 {
    public static void main(String[] args) {
        
        // Creating an ArrayList of names
        List<String> names = new ArrayList<>();
        names.add("Alice");
        names.add("Bob");
        names.add("Charlie");
        names.add("Diana");
        names.add("Eve");
        
        System.out.println("=== Basic Iterator Usage ===");
        // Getting iterator from collection
        Iterator<String> iterator = names.iterator();
        
        // Iterating through elements using hasNext() and next()
        while (iterator.hasNext()) {
            String name = iterator.next(); // Get next element
            System.out.println("Name: " + name);
        }
        
        System.out.println("\n=== Iterator with Remove ===");
        // Creating new iterator to demonstrate remove()
        Iterator<String> iter2 = names.iterator();
        while (iter2.hasNext()) {
            String name = iter2.next();
            // Remove names starting with 'A' or 'B'
            if (name.startsWith("A") || name.startsWith("B")) {
                iter2.remove(); // Safely removes element during iteration
            }
        }
        System.out.println("After removing A and B names: " + names);
        
        System.out.println("\n=== Iterator with forEachRemaining ===");
        List<Integer> numbers = new ArrayList<>(Arrays.asList(1, 2, 3, 4, 5));
        Iterator<Integer> numberIter = numbers.iterator();
        // Using forEachRemaining to process remaining elements
        numberIter.forEachRemaining(n -> System.out.println("Number: " + n * 10));
        
        // Real-time Example 1: Processing database records
        System.out.println("\n=== Example 1: Database Record Processing ===");
        List<Map<String, Object>> dbRecords = new ArrayList<>();
        Map<String, Object> record1 = new HashMap<>();
        record1.put("id", 1);
        record1.put("name", "Product A");
        record1.put("price", 99.99);
        dbRecords.add(record1);
        
        Map<String, Object> record2 = new HashMap<>();
        record2.put("id", 2);
        record2.put("name", "Product B");
        record2.put("price", 149.99);
        dbRecords.add(record2);
        
        Iterator<Map<String, Object>> dbIter = dbRecords.iterator();
        while (dbIter.hasNext()) {
            Map<String, Object> rec = dbIter.next();
            System.out.println("Processing: " + rec.get("name") + " - $" + rec.get("price"));
        }
        
        // Real-time Example 2: Filter and remove invalid entries
        System.out.println("\n=== Example 2: Validating User Entries ===");
        List<String> userEntries = new ArrayList<>(Arrays.asList("john", "jane", "", "admin", "  ", "guest"));
        Iterator<String> userIter = userEntries.iterator();
        while (userIter.hasNext()) {
            String entry = userIter.next();
            if (entry == null || entry.trim().isEmpty()) {
                userIter.remove(); // Remove empty/null entries
            }
        }
        System.out.println("Valid users: " + userEntries);
        
        // Real-time Example 3: Message queue processing
        System.out.println("\n=== Example 3: Message Queue Processing ===");
        Queue<String> messageQueue = new LinkedList<>();
        messageQueue.add("MSG001: Hello");
        messageQueue.add("MSG002: World");
        messageQueue.add("MSG003: Java");
        
        Iterator<String> msgIter = messageQueue.iterator();
        int processed = 0;
        while (msgIter.hasNext()) {
            String msg = msgIter.next();
            System.out.println("Processing message: " + msg);
            processed++;
        }
        System.out.println("Total messages processed: " + processed);
        
        // Real-time Example 4: Inventory stock check
        System.out.println("\n=== Example 4: Inventory Stock Check ===");
        Map<String, Integer> inventory = new LinkedHashMap<>();
        inventory.put("Laptop", 10);
        inventory.put("Mouse", 0);
        inventory.put("Keyboard", 5);
        inventory.put("Monitor", 0);
        
        Iterator<Map.Entry<String, Integer>> stockIter = inventory.entrySet().iterator();
        while (stockIter.hasNext()) {
            Map.Entry<String, Integer> item = stockIter.next();
            if (item.getValue() == 0) {
                System.out.println("OUT OF STOCK: " + item.getKey());
            } else {
                System.out.println("In Stock: " + item.getKey() + " (" + item.getValue() + " units)");
            }
        }
        
        // Real-time Example 5: Nested collection iteration
        System.out.println("\n=== Example 5: Nested Collection Iteration ===");
        List<List<String>> departments = new ArrayList<>();
        departments.add(Arrays.asList("Alice", "Bob", "Charlie"));
        departments.add(Arrays.asList("Diana", "Eve"));
        departments.add(Arrays.asList("Frank", "Grace", "Henry", "Ivy"));
        
        Iterator<List<String>> deptIter = departments.iterator();
        int totalEmployees = 0;
        while (deptIter.hasNext()) {
            List<String> dept = deptIter.next();
            Iterator<String> empIter = dept.iterator();
            System.out.print("Department " + (totalEmployees / 3 + 1) + ": ");
            while (empIter.hasNext()) {
                System.out.print(empIter.next() + " ");
                totalEmployees++;
            }
            System.out.println();
        }
        System.out.println("Total employees: " + totalEmployees);
        
        // Real-time Example 6: File line processing with iterator
        System.out.println("\n=== Example 6: Configuration Processing ===");
        List<String> configLines = new ArrayList<>(Arrays.asList(
            "server.port=8080",
            "db.host=localhost",
            "# This is a comment",
            "db.port=3306",
            "",
            "app.name=MyApp"
        ));
        
        Iterator<String> configIter = configLines.iterator();
        Properties configs = new Properties();
        while (configIter.hasNext()) {
            String line = configIter.next();
            if (!line.isEmpty() && !line.startsWith("#")) {
                String[] parts = line.split("=");
                if (parts.length == 2) {
                    configs.setProperty(parts[0], parts[1]);
                }
            }
        }
        System.out.println("Parsed config: " + configs);
    }
}
