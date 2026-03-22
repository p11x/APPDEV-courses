/*
 * SUB TOPIC: Interfaces in Java
 * 
 * DEFINITION:
 * An interface in Java is a blueprint of a class that contains abstract methods (methods without body). 
 * It is used to achieve abstraction and multiple inheritance in Java. From Java 8, interfaces can also 
 * have default and static methods with implementations.
 * 
 * FUNCTIONALITIES:
 * 1. Abstract methods - Methods without implementation
 * 2. Default methods - Methods with implementation (Java 8+)
 * 3. Static methods - Static methods with implementation (Java 8+)
 * 4. Constants - Fields are implicitly public, static, final
 * 5. Multiple inheritance - A class can implement multiple interfaces
 * 6. Functional interfaces - Single abstract method interfaces
 */

public class Example43 {
    
    // Basic interface with abstract methods
    interface Drawable {
        void draw(); // Abstract method
        
        default void display() { // Default method
            System.out.println("Displaying shape");
        }
        
        static void info() { // Static method
            System.out.println("Drawable interface");
        }
    }
    
    // Interface with single abstract method (Functional Interface)
    @FunctionalInterface
    interface Calculator {
        int calculate(int a, int b);
    }
    
    // Another functional interface
    interface StringProcessor {
        String process(String input);
    }
    
    // Class implementing interface
    static class Circle implements Drawable {
        @Override
        public void draw() {
            System.out.println("Drawing Circle");
        }
    }
    
    // Class implementing multiple interfaces
    static class Rectangle implements Drawable, Printable {
        @Override
        public void draw() {
            System.out.println("Drawing Rectangle");
        }
        
        @Override
        public void print() {
            System.out.println("Printing Rectangle");
        }
    }
    
    // Second interface
    interface Printable {
        void print();
    }
    
    public static void main(String[] args) {
        
        // Topic Explanation: Using Interfaces
        
        // Create object of class implementing interface
        System.out.println("=== Interface Implementation ===");
        Circle circle = new Circle();
        circle.draw();
        circle.display(); // Default method
        
        // Interface reference pointing to implementation
        System.out.println("\n=== Interface Reference ===");
        Drawable d = new Circle();
        d.draw();
        
        // Using functional interface with lambda
        System.out.println("\n=== Functional Interface ===");
        Calculator add = (a, b) -> a + b;
        Calculator multiply = (a, b) -> a * b;
        
        System.out.println("10 + 5 = " + add.calculate(10, 5));
        System.out.println("10 * 5 = " + multiply.calculate(10, 5));
        
        // String processor
        System.out.println("\n=== String Processor ===");
        StringProcessor upper = s -> s.toUpperCase();
        StringProcessor reverse = s -> new StringBuilder(s).reverse().toString();
        
        System.out.println("Uppercase: " + upper.process("hello"));
        System.out.println("Reversed: " + reverse.process("hello"));
        
        // Multiple interfaces
        System.out.println("\n=== Multiple Interfaces ===");
        Rectangle rect = new Rectangle();
        rect.draw();
        rect.print();
        
        // Interface constant
        System.out.println("\n=== Interface Constants ===");
        System.out.println("MAX_SIZE from interface: " + Shape.MAX_SIZE);
        
        // Static method
        System.out.println("\n=== Static Method ===");
        Drawable.info();
        
        // Real-time Example 1: Payment Gateway
        System.out.println("\n=== Example 1: Payment Gateway ===");
        
        interface PaymentGateway {
            void pay(double amount);
            boolean verify(String transactionId);
        }
        
        class CreditCard implements PaymentGateway {
            @Override
            public void pay(double amount) {
                System.out.println("Processing credit card payment: $" + amount);
            }
            
            @Override
            public boolean verify(String transactionId) {
                return transactionId != null && transactionId.length() > 5;
            }
        }
        
        class PayPal implements PaymentGateway {
            @Override
            public void pay(double amount) {
                System.out.println("Processing PayPal payment: $" + amount);
            }
            
            @Override
            public boolean verify(String transactionId) {
                return transactionId.startsWith("PP");
            }
        }
        
        PaymentGateway payment = new CreditCard();
        payment.pay(99.99);
        System.out.println("Verified: " + payment.verify("TXN12345"));
        
        // Real-time Example 2: Notification System
        System.out.println("\n=== Example 2: Notification System ===");
        
        interface Notifier {
            void send(String message);
            String getType();
        }
        
        class EmailNotifier implements Notifier {
            public void send(String message) {
                System.out.println("Sending EMAIL: " + message);
            }
            public String getType() { return "Email"; }
        }
        
        class SMSNotifier implements Notifier {
            public void send(String message) {
                System.out.println("Sending SMS: " + message);
            }
            public String getType() { return "SMS"; }
        }
        
        Notifier[] notifiers = { new EmailNotifier(), new SMSNotifier() };
        for (Notifier n : notifiers) {
            n.send("Hello via " + n.getType());
        }
        
        // Real-time Example 3: Data Storage
        System.out.println("\n=== Example 3: Data Storage ===");
        
        interface Storage {
            void save(String key, String value);
            String get(String key);
            void delete(String key);
        }
        
        class InMemoryStorage implements Storage {
            private java.util.Map<String, String> data = new java.util.HashMap<>();
            
            public void save(String key, String value) {
                data.put(key, value);
                System.out.println("Saved: " + key);
            }
            
            public String get(String key) {
                return data.get(key);
            }
            
            public void delete(String key) {
                data.remove(key);
                System.out.println("Deleted: " + key);
            }
        }
        
        Storage storage = new InMemoryStorage();
        storage.save("user1", "John");
        System.out.println("Retrieved: " + storage.get("user1"));
        
        // Real-time Example 4: Comparator
        System.out.println("\n=== Example 4: Comparator ===");
        
        interface Comparator<T> {
            int compare(T a, T b);
        }
        
        Comparator<Integer> intComp = (a, b) -> a - b;
        System.out.println("Compare 5 and 10: " + intComp.compare(5, 10));
        
        // Real-time Example 5: Event Handler
        System.out.println("\n=== Example 5: Event Handler ===");
        
        interface EventHandler {
            void handle(String event);
        }
        
        class ClickHandler implements EventHandler {
            public void handle(String event) {
                System.out.println("Click event: " + event);
            }
        }
        
        EventHandler handler = new ClickHandler();
        handler.handle("Button clicked");
        
        // Using lambda
        EventHandler lambdaHandler = e -> System.out.println("Lambda event: " + e);
        lambdaHandler.handle("Link clicked");
        
        // Real-time Example 6: Strategy Pattern
        System.out.println("\n=== Example 6: Strategy Pattern ===");
        
        interface SortStrategy {
            void sort(int[] array);
        }
        
        class BubbleSort implements SortStrategy {
            public void sort(int[] array) {
                System.out.println("Sorting using Bubble Sort");
            }
        }
        
        class QuickSort implements SortStrategy {
            public void sort(int[] array) {
                System.out.println("Sorting using Quick Sort");
            }
        }
        
        int[] data = {5, 2, 8, 1, 9};
        SortStrategy sorter = new QuickSort();
        sorter.sort(data);
    }
}

interface Shape {
    int MAX_SIZE = 100;
}

// Interface with constant
interface Drawable extends Shape {
    void draw();
}
