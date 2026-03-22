/*
 * SUB TOPIC: Object Class Methods - toString, equals, hashCode
 * 
 * DEFINITION:
 * The Object class is the root of all Java classes. Key methods include toString() (string representation),
 * equals() (comparison), and hashCode() (hash value for collections).
 * 
 * FUNCTIONALITIES:
 * 1. toString - String representation of object
 * 2. equals - Compare object equality
 * 3. hashCode - Hash value for HashSet/HashMap
 * 4. getClass - Runtime type information
 * 5. clone - Create object copy
 */

import java.util.*;

public class Example58 {
    
    class Person {
        String name;
        int age;
        
        Person(String name, int age) {
            this.name = name;
            this.age = age;
        }
        
        @Override
        public String toString() {
            return "Person{name='" + name + "', age=" + age + "}";
        }
        
        @Override
        public boolean equals(Object obj) {
            if (this == obj) return true;
            if (obj == null || getClass() != obj.getClass()) return false;
            Person other = (Person) obj;
            return age == other.age && Objects.equals(name, other.name);
        }
        
        @Override
        public int hashCode() {
            return Objects.hash(name, age);
        }
    }
    
    public static void main(String[] args) {
        Example58 demo = new Example58();
        
        // toString
        System.out.println("=== toString ===");
        Person p1 = demo.new Person("John", 25);
        System.out.println(p1);
        
        // equals
        System.out.println("\n=== equals ===");
        Person p2 = demo.new Person("John", 25);
        System.out.println("p1.equals(p2): " + p1.equals(p2));
        
        // hashCode
        System.out.println("\n=== hashCode ===");
        System.out.println("p1.hashCode(): " + p1.hashCode());
        System.out.println("p2.hashCode(): " + p2.hashCode());
        
        // getClass
        System.out.println("\n=== getClass ===");
        System.out.println("p1.getClass(): " + p1.getClass());
        
        // Using in HashSet
        System.out.println("\n=== HashSet ===");
        HashSet<Person> set = new HashSet<>();
        set.add(p1);
        set.add(p2);
        System.out.println("Set size (should be 1): " + set.size());
        
        // Real-time Example 1: Product
        System.out.println("\n=== Example 1: Product ===");
        
        class Product {
            String id;
            String name;
            double price;
            
            Product(String id, String name, double price) {
                this.id = id;
                this.name = name;
                this.price = price;
            }
            
            @Override
            public String toString() {
                return id + ": " + name + " - $" + price;
            }
        }
        
        Product prod = new Product("P001", "Laptop", 999);
        System.out.println(prod);
        
        // Real-time Example 2: Student equality
        System.out.println("\n=== Example 2: Student ===");
        
        class Student {
            int id;
            String name;
            
            Student(int id, String name) {
                this.id = id;
                this.name = name;
            }
            
            @Override
            public boolean equals(Object o) {
                if (this == o) return true;
                if (!(o instanceof Student)) return false;
                Student s = (Student) o;
                return id == s.id;
            }
            
            @Override
            public int hashCode() {
                return Integer.hashCode(id);
            }
        }
        
        Student s1 = new Student(1, "John");
        Student s2 = new Student(1, "Jane");
        System.out.println("Same ID: " + s1.equals(s2));
        
        // Real-time Example 3: HashMap key
        System.out.println("\n=== Example 3: HashMap ===");
        
        HashMap<Product, Integer> inventory = new HashMap<>();
        inventory.put(prod, 10);
        System.out.println("Laptop quantity: " + inventory.get(prod));
        
        // Real-time Example 4: Object array
        System.out.println("\n=== Example 4: Object Array ===");
        
        Object[] objects = {"String", 42, 3.14};
        for (Object obj : objects) {
            System.out.println(obj + " - " + obj.getClass().getSimpleName());
        }
        
        // Real-time Example 5: Clone
        System.out.println("\n=== Example 5: Clone ===");
        
        class Counter {
            int count = 0;
            
            Counter(int count) {
                this.count = count;
            }
            
            @Override
            protected Object clone() throws CloneNotSupportedException {
                return new Counter(this.count);
            }
        }
        
        try {
            Counter c1 = new Counter(10);
            Counter c2 = (Counter) c1.clone();
            System.out.println("Original: " + c1.count);
            System.out.println("Clone: " + c2.count);
        } catch (CloneNotSupportedException e) {
            e.printStackTrace();
        }
        
        // Real-time Example 6: toString debugging
        System.out.println("\n=== Example 6: Debug ===");
        
        class Order {
            String id;
            double total;
            String status;
            
            Order(String id, double total, String status) {
                this.id = id;
                this.total = total;
                this.status = status;
            }
            
            @Override
            public String toString() {
                return "Order{id=" + id + ", total=" + total + ", status=" + status + "}";
            }
        }
        
        Order order = new Order("ORD001", 150.00, "PENDING");
        System.out.println("Order: " + order);
    }
}
