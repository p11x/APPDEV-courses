// ServiceFactoryDemo - Demonstrates Design Patterns for Angular Services
// Use case: Backend design patterns that Angular developers should understand

public class ServiceFactoryDemo {
    
    // Singleton pattern
    static class DatabaseService {
        private static DatabaseService instance;
        
        private DatabaseService() {
            System.out.println("Database connected");
        }
        
        public static DatabaseService getInstance() {
            if (instance == null) {
                instance = new DatabaseService();
            }
            return instance;
        }
        
        public String query(String sql) {
            return "Result: " + sql;
        }
    }
    
    // Factory pattern
    interface DataService {
        String fetch();
    }
    
    static class UserDataService implements DataService {
        public String fetch() { return "User data"; }
    }
    
    static class ProductDataService implements DataService {
        public String fetch() { return "Product data"; }
    }
    
    static class DataServiceFactory {
        public static DataService getService(String type) {
            switch (type) {
                case "user": return new UserDataService();
                case "product": return new ProductDataService();
                default: return null;
            }
        }
    }
    
    // Builder pattern
    static class UserBuilder {
        private String name = "";
        private String email = "";
        private String role = "USER";
        
        public UserBuilder name(String name) {
            this.name = name;
            return this;
        }
        
        public UserBuilder email(String email) {
            this.email = email;
            return this;
        }
        
        public UserBuilder role(String role) {
            this.role = role;
            return this;
        }
        
        public String build() {
            return "User: " + name + ", " + email + ", " + role;
        }
    }
    
    // Observer pattern
    interface Observer {
        void update(String message);
    }
    
    static class UserObserver implements Observer {
        private String name;
        
        public UserObserver(String name) {
            this.name = name;
        }
        
        public void update(String message) {
            System.out.println(name + " received: " + message);
        }
    }
    
    static class NotificationService {
        private java.util.List<Observer> observers = new java.util.ArrayList<>();
        
        public void addObserver(Observer o) {
            observers.add(o);
        }
        
        public void notifyAll(String message) {
            for (Observer o : observers) {
                o.update(message);
            }
        }
    }
    
    public static void main(String[] args) {
        System.out.println("=== DESIGN PATTERNS FOR BACKEND ===\n");
        
        // Singleton
        System.out.println("--- Singleton ---");
        DatabaseService db1 = DatabaseService.getInstance();
        DatabaseService db2 = DatabaseService.getInstance();
        System.out.println("Same instance: " + (db1 == db2));
        
        // Factory
        System.out.println("\n--- Factory ---");
        DataService userService = DataServiceFactory.getService("user");
        DataService productService = DataServiceFactory.getService("product");
        System.out.println("User: " + userService.fetch());
        System.out.println("Product: " + productService.fetch());
        
        // Builder
        System.out.println("\n--- Builder ---");
        String user1 = new UserBuilder()
            .name("Alice")
            .email("alice@email.com")
            .role("ADMIN")
            .build();
        System.out.println(user1);
        
        String user2 = new UserBuilder()
            .name("Bob")
            .email("bob@email.com")
            .build();
        System.out.println(user2);
        
        // Observer
        System.out.println("\n--- Observer ---");
        NotificationService notifications = new NotificationService();
        notifications.addObserver(new UserObserver("Angular App"));
        notifications.addObserver(new UserObserver("Admin Panel"));
        notifications.notifyAll("New user registered!");
        
        System.out.println("\n=== ANGULAR EQUIVALENTS ===");
        System.out.println("Java Pattern     -> Angular");
        System.out.println("-----------------------------");
        System.out.println("Singleton        -> providedIn: 'root'");
        System.out.println("Factory          -> Service with switch");
        System.out.println("Builder          -> FormBuilder");
        System.out.println("Observer         -> RxJS Observables");
    }
}
