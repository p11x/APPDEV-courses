// SerializationDemo - Object Serialization in Java
// Converting objects to byte streams and vice versa

import java.io.*;

class Person implements Serializable {
    private static final long serialVersionUID = 1L;
    
    private String name;
    private int age;
    private transient String password;  // Won't be serialized
    
    public Person(String name, int age, String password) {
        this.name = name;
        this.age = age;
        this.password = password;
    }
    
    public void display() {
        System.out.println("Name: " + name + ", Age: " + age);
    }
}

public class SerializationDemo {
    
    public static void main(String[] args) {
        System.out.println("=== SERIALIZATION DEMO ===\n");
        
        // What is serialization?
        System.out.println("--- What is Serialization? ---");
        System.out.println("Converting object state to byte stream");
        System.out.println("Used for: File storage, Network transfer, Session storage");
        
        // Serializable interface
        System.out.println("\n--- Requirements ---");
        System.out.println("1. Class must implement Serializable");
        System.out.println("2. All fields must be serializable");
        System.out.println("3. Use 'transient' to skip fields");
        
        // Object to file
        System.out.println("\n--- Serialization Process ---");
        System.out.println("Object -> ObjectOutputStream -> File");
        
        // Object from file
        System.out.println("\n--- Deserialization Process ---");
        System.out.println("File -> ObjectInputStream -> Object");
        
        // Use cases
        System.out.println("\n--- Use Cases ---");
        System.out.println("1. Storing objects to files");
        System.out.println("2. Sending objects over network");
        System.out.println("3. HTTP session replication");
        System.out.println("4. Cache persistence");
        
        // JSON vs Serialization
        System.out.println("\n--- JSON vs Serialization ---");
        System.out.println("JSON: Human-readable, language-independent");
        System.out.println("Java Serialization: Binary, Java-only, faster");
        System.out.println("For web apps: JSON is preferred (Angular compatibility)");
    }
}
