/*
 * SUB TOPIC: Static vs Instance Methods in Java
 * 
 * DEFINITION:
 * Static methods belong to the class rather than instances and can be called without creating objects.
 * Instance methods require an object to be called and can access instance variables. Static methods
 * are stored in method area, instance methods in stack with objects.
 * 
 * FUNCTIONALITIES:
 * 1. Static methods - class-level, no object needed
 * 2. Instance methods - object-level, can access all members
 * 3. Static variables - shared across all instances
 * 4. Instance variables - unique per object
 * 5. Static initialization block
 */

public class Example50 {
    
    // Static variable - shared across all instances
    private static int staticCount = 0;
    
    // Instance variable - unique per object
    private int instanceCount = 0;
    private String name;
    
    // Constructor
    public Example50(String name) {
        this.name = name;
        staticCount++;
        instanceCount = 1;
    }
    
    // Static method - can be called without object
    public static int getStaticCount() {
        return staticCount;
    }
    
    // Instance method - requires object
    public void increment() {
        instanceCount++;
    }
    
    public int getInstanceCount() {
        return instanceCount;
    }
    
    public static void main(String[] args) {
        
        // Topic Explanation: Static vs Instance
        
        System.out.println("=== Static Methods ===");
        // Call static method without object
        System.out.println("Static count: " + Example50.getStaticCount());
        
        // Create instances
        Example50 obj1 = new Example50("Object1");
        Example50 obj2 = new Example50("Object2");
        
        System.out.println("\n=== Instance Methods ===");
        // Call instance method using object
        obj1.increment();
        obj1.increment();
        
        System.out.println(obj1.name + " count: " + obj1.getInstanceCount());
        System.out.println(obj2.name + " count: " + obj2.getInstanceCount());
        
        // Static variable is shared
        System.out.println("\n=== Static Variable (Shared) ===");
        System.out.println("Total objects created: " + Example50.getStaticCount());
        
        // Real-time Example 1: Counter
        System.out.println("\n=== Example 1: Application Counter ===");
        
        class AppCounter {
            private static int totalUsers = 0;
            private String username;
            
            public AppCounter(String username) {
                this.username = username;
                totalUsers++;
            }
            
            public static int getTotalUsers() {
                return totalUsers;
            }
        }
        
        new AppCounter("John");
        new AppCounter("Jane");
        new AppCounter("Mike");
        System.out.println("Total users: " + AppCounter.getTotalUsers());
        
        // Real-time Example 2: Math utilities
        System.out.println("\n=== Example 2: Math Utils ===");
        // Static methods from Math class
        System.out.println("Max(10, 20): " + Math.max(10, 20));
        System.out.println("Min(10, 20): " + Math.min(10, 20));
        System.out.println("Abs(-5): " + Math.abs(-5));
        System.out.println("Sqrt(16): " + Math.sqrt(16));
        
        // Real-time Example 3: Database connection
        System.out.println("\n=== Example 3: Connection Manager ===");
        
        class ConnectionManager {
            private static int activeConnections = 0;
            private static final int MAX_CONNECTIONS = 10;
            
            public ConnectionManager() {
                if (activeConnections < MAX_CONNECTIONS) {
                    activeConnections++;
                    System.out.println("Connection established");
                }
            }
            
            public static int getActiveConnections() {
                return activeConnections;
            }
            
            public static boolean isAvailable() {
                return activeConnections < MAX_CONNECTIONS;
            }
        }
        
        new ConnectionManager();
        new ConnectionManager();
        System.out.println("Active: " + ConnectionManager.getActiveConnections());
        
        // Real-time Example 4: Configuration
        System.out.println("\n=== Example 4: Config ===");
        
        class Config {
            private static String APP_NAME = "MyApp";
            private static String VERSION = "1.0";
            
            public static String getAppName() {
                return APP_NAME;
            }
            
            public static String getVersion() {
                return VERSION;
            }
        }
        
        System.out.println("App: " + Config.getAppName());
        System.out.println("Version: " + Config.getVersion());
        
        // Real-time Example 5: Logger
        System.out.println("\n=== Example 5: Logger ===");
        
        class Logger {
            private static int logCount = 0;
            
            public static void log(String message) {
                logCount++;
                System.out.println("[" + logCount + "] " + message);
            }
        }
        
        Logger.log("App started");
        Logger.log("User logged in");
        Logger.log("Data loaded");
        
        // Real-time Example 6: Singleton pattern
        System.out.println("\n=== Example 6: Singleton ===");
        System.out.println("Singleton pattern ensures single instance");
        System.out.println("Used for: Config, Database, Logger");
    }
}

class SingletonDemo {
    private static SingletonDemo instance;
    private String data;
    
    private SingletonDemo() {
        data = "Singleton data";
    }
    
    public static SingletonDemo getInstance() {
        if (instance == null) {
            instance = new SingletonDemo();
        }
        return instance;
    }
    
    public String getData() {
        return data;
    }
}
