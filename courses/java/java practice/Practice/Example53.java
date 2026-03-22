/*
 * SUB TOPIC: Java Annotations
 * 
 * DEFINITION:
 * Annotations provide metadata about code. They start with @ and can be used for compilation warnings,
 * runtime processing, or documentation. Common built-in annotations include @Override, @Deprecated, 
 * @SuppressWarnings.
 * 
 * FUNCTIONALITIES:
 * 1. @Override - Method overrides parent
 * 2. @Deprecated - Code is obsolete
 * 3. @SuppressWarnings - Ignore warnings
 * 4. Custom annotations
 * 5. Annotation retention policies
 */

import java.util.*;

public class Example53 {
    
    // Custom annotation
    @interface Author {
        String name();
        String date();
    }
    
    // Annotation with default value
    @interface Version {
        int major() default 1;
        int minor() default 0;
    }
    
    // Using custom annotation
    @Author(name = "John", date = "2024-01-01")
    @Version(major = 2, minor = 1)
    public static class MyClass {
        
        @Override
        public String toString() {
            return "MyClass";
        }
        
        @Deprecated
        public void oldMethod() {
            System.out.println("Old method");
        }
        
        @SuppressWarnings("deprecation")
        public void useOldMethod() {
            oldMethod();
        }
        
        public static void main(String[] args) {
            
            // Topic Explanation: Built-in Annotations
            
            System.out.println("=== @Override ===");
            MyClass obj = new MyClass();
            System.out.println("Override test: " + obj.toString());
            
            System.out.println("\n=== @Deprecated ===");
            obj.oldMethod();
            
            System.out.println("\n=== @SuppressWarnings ===");
            obj.useOldMethod();
            
            // Real-time Example 1: Override toString
            System.out.println("\n=== Example 1: toString ===");
            
            class Product {
                String name;
                double price;
                
                Product(String name, double price) {
                    this.name = name;
                    this.price = price;
                }
                
                @Override
                public String toString() {
                    return name + ": $" + price;
                }
            }
            
            Product p = new Product("Laptop", 999);
            System.out.println(p);
            
            // Real-time Example 2: Deprecated method
            System.out.println("\n=== Example 2: Deprecated ===");
            
            class Calculator {
                @Deprecated
                public int add(int a, int b) {
                    return a + b;
                }
                
                public int sum(int a, int b) {
                    return a + b;
                }
            }
            
            Calculator calc = new Calculator();
            System.out.println("Add (deprecated): " + calc.add(5, 3));
            System.out.println("Sum (new): " + calc.sum(5, 3));
            
            // Real-time Example 3: Suppress warnings
            System.out.println("\n=== Example 3: Suppress ===");
            
            @SuppressWarnings("unused")
            class UnusedClass {
                String unusedField = "test";
            }
            
            System.out.println("Warning suppressed");
            
            // Real-time Example 4: Custom annotation
            System.out.println("\n=== Example 4: Custom Annotation ===");
            
            @Author(name = "Jane", date = "2024-03-01")
            class UserService {
                public void process() {
                    System.out.println("Processing");
                }
            }
            
            System.out.println("Custom annotation applied");
            
            // Real-time Example 5: Override equals
            System.out.println("\n=== Example 5: equals Override ===");
            
            class Point {
                int x, y;
                
                Point(int x, int y) {
                    this.x = x;
                    this.y = y;
                }
                
                @Override
                public boolean equals(Object obj) {
                    if (this == obj) return true;
                    if (obj == null || getClass() != obj.getClass()) return false;
                    Point other = (Point) obj;
                    return x == other.x && y == other.y;
                }
                
                @Override
                public int hashCode() {
                    return Objects.hash(x, y);
                }
            }
            
            Point p1 = new Point(5, 10);
            Point p2 = new Point(5, 10);
            System.out.println("p1.equals(p2): " + p1.equals(p2));
            
            // Real-time Example 6: Override hashCode
            System.out.println("\n=== Example 6: HashSet with Objects ===");
            
            HashSet<Point> points = new HashSet<>();
            points.add(new Point(1, 2));
            points.add(new Point(1, 2));
            points.add(new Point(3, 4));
            
            System.out.println("Unique points: " + points.size());
        }
    }
}
