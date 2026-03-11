// StaticVsInstanceDemo - Demonstrates Static vs Instance methods
// Static methods belong to the class, instance methods belong to objects

public class StaticVsInstanceDemo {
    
    // Instance variable - each object has its own copy
    private String name;
    
    // Static variable - shared across all objects
    private static int instanceCount = 0;
    
    // Constructor
    public StaticVsInstanceDemo(String name) {
        this.name = name;
        instanceCount++;  // Increment for each new object
    }
    
    // Instance method - requires an object to call
    public void instanceMethod() {
        System.out.println("Instance method called by: " + name);
        System.out.println("Can access instance variable: " + this.name);
        System.out.println("Can access static variable: " + instanceCount);
    }
    
    // Static method - belongs to class, not object
    public static void staticMethod() {
        System.out.println("Static method called");
        // Cannot access instance variable directly:
        // System.out.println(name);  // ERROR!
        
        // Can access static variable:
        System.out.println("Instance count: " + instanceCount);
    }
    
    // Static factory method - alternative to constructor
    public static StaticVsInstanceDemo createInstance(String name) {
        return new StaticVsInstanceDemo(name);
    }
    
    // Main method - static, entry point
    public static void main(String[] args) {
        System.out.println("=== STATIC VS INSTANCE DEMO ===\n");
        
        // Calling static method (no object needed)
        System.out.println("--- Static Method Call ---");
        staticMethod();
        StaticVsInstanceDemo.staticMethod();
        
        // Calling instance method (requires object)
        System.out.println("\n--- Instance Method Call ---");
        StaticVsInstanceDemo obj1 = new StaticVsInstanceDemo("Object 1");
        obj1.instanceMethod();
        
        StaticVsInstanceDemo obj2 = new StaticVsInstanceDemo("Object 2");
        obj2.instanceMethod();
        
        // Static variable is shared
        System.out.println("\n--- Static Variable ---");
        System.out.println("Total instances: " + instanceCount);
        
        // Static factory method
        System.out.println("\n--- Static Factory Method ---");
        StaticVsInstanceDemo obj3 = createInstance("Factory Object");
        obj3.instanceMethod();
        
        // Key differences
        System.out.println("\n=== KEY DIFFERENCES ===");
        System.out.println("| Static                    | Instance                 |");
        System.out.println("|---------------------------|--------------------------|");
        System.out.println("| Class-level               | Object-level             |");
        System.out.println("| Shared across objects    | Unique per object        |");
        System.out.println("| Called via ClassName     | Called via object        |");
        System.out.println("| Cannot use 'this'        | Can use 'this'           |");
    }
}
