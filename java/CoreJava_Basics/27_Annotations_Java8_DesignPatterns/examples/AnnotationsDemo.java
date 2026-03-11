// AnnotationsDemo - Demonstrates Java Annotations
// Annotations are metadata that provide information about the program

import java.lang.annotation.*;
import java.lang.reflect.*;

// Custom annotation
@Retention(RetentionPolicy.RUNTIME)
@Target(ElementType.METHOD)
@interface MyAnnotation {
    String value() default "";
    int priority() default 1;
}

// Marker annotation
@Retention(RetentionPolicy.RUNTIME)
@interface Deprecated {
}

// Custom annotation with multiple fields
@Retention(RetentionPolicy.RUNTIME)
@Target(ElementType.TYPE)
@interface Entity {
    String tableName();
    String schema() default "default";
}

// Usage of annotations
@Entity(tableName = "users", schema = "app")
class User {
    private int id;
    private String name;
    
    @MyAnnotation(value = "Getter for ID")
    public int getId() { return id; }
    
    @Deprecated
    public void oldMethod() {
        System.out.println("This method is deprecated!");
    }
}

public class AnnotationsDemo {
    
    @MyAnnotation(value = "Main method", priority = 10)
    public static void main(String[] args) {
        System.out.println("=== ANNOTATIONS DEMO ===\n");
        
        // Read annotations via reflection
        System.out.println("--- Reading Annotations ---");
        
        // Class-level annotation
        Entity entity = User.class.getAnnotation(Entity.class);
        if (entity != null) {
            System.out.println("Table: " + entity.tableName());
            System.out.println("Schema: " + entity.schema());
        }
        
        // Method-level annotation
        try {
            Method method = User.class.getMethod("getId");
            MyAnnotation annotation = method.getAnnotation(MyAnnotation.class);
            if (annotation != null) {
                System.out.println("Method annotation: " + annotation.value());
                System.out.println("Priority: " + annotation.priority());
            }
        } catch (NoSuchMethodException e) {
            e.printStackTrace();
        }
        
        // Built-in annotations
        System.out.println("\n--- Built-in Annotations ---");
        System.out.println("@Override - Marks method override");
        System.out.println("@Deprecated - Marks deprecated code");
        System.out.println("@SuppressWarnings - Suppresses warnings");
        System.out.println("@FunctionalInterface - Marks functional interface");
        
        // Angular/Spring annotations (conceptual)
        System.out.println("\n--- Spring Annotations (Conceptual) ---");
        System.out.println("@Controller - REST controller");
        System.out.println("@Service - Business logic");
        System.out.println("@Repository - Data access");
        System.out.println("@Autowired - Dependency injection");
        System.out.println("@GetMapping, @PostMapping - HTTP methods");
        System.out.println("@RequestBody - Request deserialization");
        System.out.println("@ResponseBody - Response serialization");
    }
}
