/*
 * SUB TOPIC: Abstract Classes and Polymorphism
 * 
 * DEFINITION:
 * An abstract class is a class that cannot be instantiated directly and may contain abstract methods
 * (methods without implementation). It serves as a blueprint for other classes. Polymorphism allows
 * objects of different classes to be treated as objects of a common superclass.
 * 
 * FUNCTIONALITIES:
 * 1. Creating abstract classes with abstract methods
 * 2. Implementing polymorphism
 * 3. Method overriding
 * 4. Using inheritance with abstract classes
 */

// Abstract class Shape
abstract class Shape {
    // Abstract method - no implementation
    public abstract double getArea(); // Must be implemented by subclasses
}

// Rectangle class extending Shape
class Rectangle extends Shape {
    public double width;
    public double height;
    
    public Rectangle(double w, double h) {
        width = w;
        height = h;
    }
    
    // Implement abstract method
    public double getArea() {
        return width * height;
    }
}

// Circle class extending Shape
class Circle extends Shape {
    public double radius;
    
    public Circle(double r) {
        radius = r;
    }
    
    public double getArea() {
        return 3.14 * radius * radius;
    }
}

// Triangle class extending Shape
class Triangle extends Shape {
    public double base;
    public double height;
    
    public Triangle(double b, double h) {
        base = b;
        height = h;
    }
    
    public double getArea() {
        return 0.5 * base * height;
    }
}

public class Example15 {
    public static void main(String[] args) {
        
        // Topic Explanation with Code: Abstract Class and Polymorphism
        System.out.println("=== Abstract Class and Polymorphism ===");
        
        Shape[] shapes = new Shape[3]; // Array of abstract class type
        shapes[0] = new Rectangle(10, 5); // Rectangle object
        shapes[1] = new Circle(7); // Circle object
        shapes[2] = new Triangle(6, 4); // Triangle object
        
        System.out.println("Areas of shapes:");
        for (int i = 0; i < shapes.length; i++) {
            System.out.println("Shape " + (i + 1) + ": " + shapes[i].getArea());
        }
        
        // Real-time Example 1: Calculate area of rectangle
        System.out.println("\n=== Rectangle Area ===");
        Rectangle rect = new Rectangle(15, 10);
        System.out.println("Width: " + rect.width);
        System.out.println("Height: " + rect.height);
        System.out.println("Area: " + rect.getArea());
        
        // Real-time Example 2: Calculate area of circle
        System.out.println("\n=== Circle Area ===");
        Circle circle = new Circle(5);
        System.out.println("Radius: " + circle.radius);
        System.out.println("Area: " + circle.getArea());
        
        // Real-time Example 3: Calculate area of triangle
        System.out.println("\n=== Triangle Area ===");
        Triangle triangle = new Triangle(8, 6);
        System.out.println("Base: " + triangle.base);
        System.out.println("Height: " + triangle.height);
        System.out.println("Area: " + triangle.getArea());
        
        // Real-time Example 4: Array of different shapes
        System.out.println("\n=== Multiple Shapes ===");
        Shape[] shapesArray = {
            new Rectangle(20, 10),
            new Circle(10),
            new Triangle(15, 8),
            new Rectangle(5, 5),
            new Circle(3)
        };
        
        for (Shape s : shapesArray) {
            System.out.println("Area: " + s.getArea());
        }
        
        // Real-time Example 5: Calculate total area
        System.out.println("\n=== Total Area ===");
        double totalArea = 0;
        for (Shape s : shapesArray) {
            totalArea += s.getArea(); // Add each area
        }
        System.out.println("Total Area: " + totalArea);
        
        // Real-time Example 6: Find largest shape
        System.out.println("\n=== Largest Shape ===");
        double maxArea = 0;
        String largestShape = "";
        
        for (Shape s : shapesArray) {
            if (s instanceof Rectangle) {
                largestShape = "Rectangle";
            } else if (s instanceof Circle) {
                largestShape = "Circle";
            } else if (s instanceof Triangle) {
                largestShape = "Triangle";
            }
            
            if (s.getArea() > maxArea) {
                maxArea = s.getArea();
            }
        }
        
        System.out.println("Largest Area: " + maxArea);
    }
}
