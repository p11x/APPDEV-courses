// AdvancedStreamDemo - Advanced Stream API Operations
// Collectors, grouping, partitioning, and parallel streams

import java.util.*;
import java.util.stream.*;

public class AdvancedStreamDemo {
    
    public static void main(String[] args) {
        System.out.println("=== ADVANCED STREAM OPERATIONS ===\n");
        
        List<Person> people = Arrays.asList(
            new Person("Alice", 25, "NYC"),
            new Person("Bob", 30, "LA"),
            new Person("Charlie", 25, "NYC"),
            new Person("Diana", 30, "LA"),
            new Person("Eve", 25, "NYC")
        );
        
        // Grouping by
        System.out.println("--- Grouping By ---");
        Map<Integer, List<Person>> byAge = people.stream()
            .collect(Collectors.groupingBy(Person::getAge));
        System.out.println("By age: " + byAge);
        
        // Grouping by with count
        Map<String, Long> countByCity = people.stream()
            .collect(Collectors.groupingBy(Person::getCity, Collectors.counting()));
        System.out.println("Count by city: " + countByCity);
        
        // Partitioning
        System.out.println("\n--- Partitioning ---");
        Map<Boolean, List<Person>> partitioned = people.stream()
            .collect(Collectors.partitioningBy(p -> p.getAge() >= 25));
        System.out.println("Age >= 25: " + partitioned);
        
        // Summarizing
        System.out.println("\n--- Summarizing ---");
        IntSummaryStatistics stats = people.stream()
            .collect(Collectors.summarizingInt(Person::getAge));
        System.out.println("Age stats: " + stats);
        
        // Parallel streams
        System.out.println("\n--- Parallel Streams ---");
        long start = System.currentTimeMillis();
        long sum = people.parallelStream()
            .mapToInt(Person::getAge)
            .sum();
        long time = System.currentTimeMillis() - start;
        System.out.println("Parallel sum: " + sum + " (took " + time + "ms)");
        
        // Mapping and collecting
        System.out.println("\n--- Mapping and Collecting ---");
        Set<String> names = people.stream()
            .map(Person::getName)
            .collect(Collectors.toSet());
        System.out.println("Names set: " + names);
        
        // Joining
        String allNames = people.stream()
            .map(Person::getName)
            .collect(Collectors.joining(", "));
        System.out.println("Joined: " + allNames);
    }
}

class Person {
    private String name;
    private int age;
    private String city;
    
    public Person(String name, int age, String city) {
        this.name = name;
        this.age = age;
        this.city = city;
    }
    
    public String getName() { return name; }
    public int getAge() { return age; }
    public String getCity() { return city; }
    
    public String toString() { return name + "(" + age + ")"; }
}
