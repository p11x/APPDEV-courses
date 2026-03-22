/*
 * SUB TOPIC: Java Design Patterns - Singleton, Factory, Observer
 * 
 * DEFINITION:
 * Design patterns are reusable solutions to common software design problems. Singleton ensures one instance,
 * Factory creates objects without specifying exact classes, Observer defines one-to-many dependency.
 * 
 * FUNCTIONALITIES:
 * 1. Singleton Pattern - Single instance
 * 2. Factory Pattern - Object creation
 * 3. Observer Pattern - Event handling
 * 4. Builder Pattern - Object construction
 * 5. Strategy Pattern - Interchangeable algorithms
 */

public class Example56 {
    
    // Singleton Pattern
    static class Singleton {
        private static Singleton instance;
        private String data;
        
        private Singleton() {
            data = "Singleton Data";
        }
        
        public static Singleton getInstance() {
            if (instance == null) {
                instance = new Singleton();
            }
            return instance;
        }
        
        public String getData() {
            return data;
        }
    }
    
    // Factory Pattern
    interface Payment {
        void pay(double amount);
    }
    
    static class CreditCard implements Payment {
        public void pay(double amount) {
            System.out.println("Paid $" + amount + " via Credit Card");
        }
    }
    
    static class PayPal implements Payment {
        public void pay(double amount) {
            System.out.println("Paid $" + amount + " via PayPal");
        }
    }
    
    static class PaymentFactory {
        public static Payment createPayment(String type) {
            if (type.equalsIgnoreCase("credit")) {
                return new CreditCard();
            } else if (type.equalsIgnoreCase("paypal")) {
                return new PayPal();
            }
            return null;
        }
    }
    
    public static void main(String[] args) {
        
        // Singleton
        System.out.println("=== Singleton ===");
        Singleton s1 = Singleton.getInstance();
        Singleton s2 = Singleton.getInstance();
        System.out.println("Same instance: " + (s1 == s2));
        System.out.println("Data: " + s1.getData());
        
        // Factory
        System.out.println("\n=== Factory ===");
        Payment credit = PaymentFactory.createPayment("credit");
        credit.pay(100);
        
        Payment paypal = PaymentFactory.createPayment("paypal");
        paypal.pay(200);
        
        // Observer Pattern
        System.out.println("\n=== Observer ===");
        
        interface Observer {
            void update(String message);
        }
        
        class User implements Observer {
            String name;
            
            User(String name) {
                this.name = name;
            }
            
            public void update(String message) {
                System.out.println(name + " received: " + message);
            }
        }
        
        class NewsChannel {
            private java.util.List<Observer> observers = new java.util.ArrayList<>();
            
            public void subscribe(Observer o) {
                observers.add(o);
            }
            
            public void notifyAll(String message) {
                for (Observer o : observers) {
                    o.update(message);
                }
            }
        }
        
        NewsChannel news = new NewsChannel();
        news.subscribe(new User("John"));
        news.subscribe(new User("Jane"));
        
        news.notifyAll("Breaking News!");
        
        // Real-time Example 1: Logger
        System.out.println("\n=== Example 1: Logger ===");
        
        class Logger {
            private static Logger logger;
            
            private Logger() {}
            
            public static Logger getInstance() {
                if (logger == null) {
                    logger = new Logger();
                }
                return logger;
            }
            
            public void log(String msg) {
                System.out.println("[LOG] " + msg);
            }
        }
        
        Logger.getInstance().log("App started");
        
        // Real-time Example 2: Shape Factory
        System.out.println("\n=== Example 2: Shape Factory ===");
        
        interface Shape {
            void draw();
        }
        
        class Circle2 implements Shape {
            public void draw() {
                System.out.println("Drawing Circle");
            }
        }
        
        class Rectangle2 implements Shape {
            public void draw() {
                System.out.println("Drawing Rectangle");
            }
        }
        
        class ShapeFactory2 {
            public static Shape getShape(String type) {
                if (type.equals("circle")) return new Circle2();
                if (type.equals("rectangle")) return new Rectangle2();
                return null;
            }
        }
        
        ShapeFactory2.getShape("circle").draw();
        
        // Real-time Example 3: Event Listener
        System.out.println("\n=== Example 3: Event System ===");
        
        class EventManager {
            private java.util.List<Runnable> listeners = new java.util.ArrayList<>();
            
            public void onEvent(Runnable r) {
                listeners.add(r);
            }
            
            public void trigger() {
                for (Runnable r : listeners) {
                    r.run();
                }
            }
        }
        
        EventManager events = new EventManager();
        events.onEvent(() -> System.out.println("Event 1"));
        events.onEvent(() -> System.out.println("Event 2"));
        events.trigger();
        
        // Real-time Example 4: Builder
        System.out.println("\n=== Example 4: Builder ===");
        
        class User2 {
            String name;
            int age;
            
            User2(String name, int age) {
                this.name = name;
                this.age = age;
            }
        }
        
        class UserBuilder {
            private String name = "";
            private int age = 0;
            
            public UserBuilder name(String name) {
                this.name = name;
                return this;
            }
            
            public UserBuilder age(int age) {
                this.age = age;
                return this;
            }
            
            public User2 build() {
                return new User2(name, age);
            }
        }
        
        User2 user = new UserBuilder()
            .name("John")
            .age(25)
            .build();
        
        System.out.println("User: " + user.name + ", " + user.age);
        
        // Real-time Example 5: Strategy
        System.out.println("\n=== Example 5: Strategy ===");
        
        interface PaymentStrategy {
            void pay(int amount);
        }
        
        class CardPayment implements PaymentStrategy {
            public void pay(int amount) {
                System.out.println("Card payment: $" + amount);
            }
        }
        
        class CashPayment implements PaymentStrategy {
            public void pay(int amount) {
                System.out.println("Cash payment: $" + amount);
            }
        }
        
        class Cart {
            private PaymentStrategy strategy;
            
            public void setStrategy(PaymentStrategy s) {
                this.strategy = s;
            }
            
            public void checkout(int amount) {
                strategy.pay(amount);
            }
        }
        
        Cart cart = new Cart();
        cart.setStrategy(new CardPayment());
        cart.checkout(100);
        
        // Real-time Example 6: Repository
        System.out.println("\n=== Example 6: Repository ===");
        
        interface Repository<T> {
            void save(T t);
            T findById(int id);
        }
        
        class InMemoryRepository<T> implements Repository<T> {
            private java.util.Map<Integer, T> data = new java.util.HashMap<>();
            private int id = 0;
            
            public void save(T t) {
                data.put(++id, t);
                System.out.println("Saved with id: " + id);
            }
            
            public T findById(int id) {
                return data.get(id);
            }
        }
        
        InMemoryRepository<String> repo = new InMemoryRepository<>();
        repo.save("Test Data");
    }
}
