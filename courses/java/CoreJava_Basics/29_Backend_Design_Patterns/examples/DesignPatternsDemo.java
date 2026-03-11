// DesignPatternsDemo - Common Design Patterns for Backend Development
// Important for building maintainable Java applications

import java.util.*;

public class DesignPatternsDemo {
    
    public static void main(String[] args) {
        System.out.println("=== DESIGN PATTERNS DEMO ===\n");
        
        // Singleton Pattern
        System.out.println("--- Singleton Pattern ---");
        DatabaseConnection conn1 = DatabaseConnection.getInstance();
        DatabaseConnection conn2 = DatabaseConnection.getInstance();
        System.out.println("Same instance: " + (conn1 == conn2));
        
        // Factory Pattern
        System.out.println("\n--- Factory Pattern ---");
        Notification email = NotificationFactory.create("EMAIL");
        email.send("Welcome!");
        Notification sms = NotificationFactory.create("SMS");
        sms.send("Your code is 1234");
        
        // Observer Pattern
        System.out.println("\n--- Observer Pattern ---");
        NewsAgency agency = new NewsAgency();
        NewsChannel channel1 = new NewsChannel("Channel 1");
        NewsChannel channel2 = new NewsChannel("Channel 2");
        agency.addObserver(channel1);
        agency.addObserver(channel2);
        agency.publishNews("Breaking: Java 21 Released!");
        
        // Repository Pattern
        System.out.println("\n--- Repository Pattern ---");
        UserRepository repo = new UserRepository();
        User user = new User(1, "john@email.com", "John");
        repo.save(user);
        User found = repo.findById(1);
        System.out.println("Found: " + found.getName());
        
        // Strategy Pattern
        System.out.println("\n--- Strategy Pattern ---");
        PaymentContext context = new PaymentContext();
        context.setStrategy(new CreditCardPayment());
        context.pay(100);
        context.setStrategy(new PayPalPayment());
        context.pay(50);
    }
}

// ===== SINGLETON PATTERN =====
class DatabaseConnection {
    private static DatabaseConnection instance;
    
    private DatabaseConnection() {}
    
    public static synchronized DatabaseConnection getInstance() {
        if (instance == null) {
            instance = new DatabaseConnection();
        }
        return instance;
    }
    
    public void query(String sql) {
        System.out.println("Executing: " + sql);
    }
}

// ===== FACTORY PATTERN =====
interface Notification {
    void send(String message);
}

class EmailNotification implements Notification {
    public void send(String message) {
        System.out.println("Email sent: " + message);
    }
}

class SMSNotification implements Notification {
    public void send(String message) {
        System.out.println("SMS sent: " + message);
    }
}

class NotificationFactory {
    public static Notification create(String type) {
        switch (type) {
            case "EMAIL": return new EmailNotification();
            case "SMS": return new SMSNotification();
            default: throw new IllegalArgumentException("Unknown type");
        }
    }
}

// ===== OBSERVER PATTERN =====

interface Observer {
    void update(String news);
}

class NewsAgency {
    private List<Observer> observers = new ArrayList<>();
    
    public void addObserver(Observer o) { observers.add(o); }
    public void removeObserver(Observer o) { observers.remove(o); }
    
    public void publishNews(String news) {
        System.out.println("Agency publishes: " + news);
        for (Observer o : observers) {
            o.update(news);
        }
    }
}

class NewsChannel implements Observer {
    private String name;
    
    public NewsChannel(String name) { this.name = name; }
    
    public void update(String news) {
        System.out.println(name + " received: " + news);
    }
}

// ===== REPOSITORY PATTERN =====
class User {
    private int id;
    private String email;
    private String name;
    
    public User(int id, String email, String name) {
        this.id = id;
        this.email = email;
        this.name = name;
    }
    
    public int getId() { return id; }
    public String getEmail() { return email; }
    public String getName() { return name; }
}

class UserRepository {
    private Map<Integer, User> users = new HashMap<>();
    
    public void save(User user) {
        users.put(user.getId(), user);
        System.out.println("Saved user: " + user.getName());
    }
    
    public User findById(int id) {
        return users.get(id);
    }
    
    public List<User> findAll() {
        return new ArrayList<>(users.values());
    }
}

// ===== STRATEGY PATTERN =====
interface PaymentStrategy {
    void pay(int amount);
}

class CreditCardPayment implements PaymentStrategy {
    public void pay(int amount) {
        System.out.println("Paid $" + amount + " via Credit Card");
    }
}

class PayPalPayment implements PaymentStrategy {
    public void pay(int amount) {
        System.out.println("Paid $" + amount + " via PayPal");
    }
}

class PaymentContext {
    private PaymentStrategy strategy;
    
    public void setStrategy(PaymentStrategy strategy) {
        this.strategy = strategy;
    }
    
    public void pay(int amount) {
        strategy.pay(amount);
    }
}
