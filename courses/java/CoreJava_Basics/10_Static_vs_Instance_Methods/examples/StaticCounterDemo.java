// StaticCounterDemo - Demonstrates Static Methods for Angular State Management
// Use case: Managing shared application state across components

public class StaticCounterDemo {
    // Static variable - shared across all instances
    private static int instanceCount = 0;
    
    // Instance variable - unique to each object
    private String instanceId;
    
    public StaticCounterDemo() {
        instanceCount++;
        instanceId = "Instance-" + instanceCount;
    }
    
    // Static method - called without creating instance
    public static int getInstanceCount() {
        return instanceCount;
    }
    
    // Static method for resetting counter
    public static void resetCounter() {
        instanceCount = 0;
    }
    
    // Instance method - requires object reference
    public String getInstanceId() {
        return instanceId;
    }
    
    public static void main(String[] args) {
        System.out.println("=== STATIC COUNTER FOR STATE MANAGEMENT ===\n");
        
        // Create multiple instances
        StaticCounterDemo obj1 = new StaticCounterDemo();
        StaticCounterDemo obj2 = new StaticCounterDemo();
        StaticCounterDemo obj3 = new StaticCounterDemo();
        
        // Access static method without instance
        System.out.println("Total instances: " + StaticCounterDemo.getInstanceCount());
        
        // Access instance method through object
        System.out.println("Object 1 ID: " + obj1.getInstanceId());
        System.out.println("Object 2 ID: " + obj2.getInstanceId());
        
        // Reset static counter
        StaticCounterDemo.resetCounter();
        System.out.println("\nAfter reset: " + StaticCounterDemo.getInstanceCount());
        
        System.out.println("\n=== ANGULAR USE CASE ===");
        System.out.println("Static methods are ideal for:");
        System.out.println("1. Global app state (user session)");
        System.out.println("2. API service counters");
        System.out.println("3. Configuration settings");
        System.out.println("4. Logging services");
    }
}
