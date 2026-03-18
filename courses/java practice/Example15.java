// Example15: Array of Abstract Classes - Simple Beginner Version
// This shows how to store different shapes in one array

// Step 1: Create an abstract class (a blueprint)
// Abstract classes cannot be used to create objects directly
abstract class Shape {
    // Abstract method - every shape must have an area
    public abstract double getArea();
}

// Step 2: Create Rectangle class that inherits from Shape
class Rectangle extends Shape {
    // Simple public variables (beginner-friendly)
    public double width;
    public double height;
    
    // Constructor to set values
    public Rectangle(double w, double h) {
        width = w;
        height = h;
    }
    
    // Override the abstract method
    public double getArea() {
        return width * height;
    }
}

// Step 3: Create Circle class that inherits from Shape
class Circle extends Shape {
    public double radius;
    
    public Circle(double r) {
        radius = r;
    }
    
    public double getArea() {
        return 3.14 * radius * radius;
    }
}

// Step 4: Create Triangle class that inherits from Shape
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

// Main class
public class Example15 {
    public static void main(String[] args) {
        // Step 5: Create an array that holds Shape objects
        // This array can hold Rectangle, Circle, or Triangle objects
        Shape[] shapes = new Shape[3];
        
        // Step 6: Add different shapes to the array
        shapes[0] = new Rectangle(10, 5);    // Rectangle with width 10, height 5
        shapes[1] = new Circle(7);            // Circle with radius 7
        shapes[2] = new Triangle(6, 4);      // Triangle with base 6, height 4
        
        // Step 7: Use a loop to call getArea() on each shape
        // This is polymorphism - each shape calculates its own area differently
        System.out.println("Areas of all shapes:");
        System.out.println("---------------------");
        
        for (int i = 0; i < shapes.length; i++) {
            System.out.println("Shape " + (i + 1) + " area: " + shapes[i].getArea());
        }
        
        // Alternative: Using for-each loop (also beginner-friendly)
        System.out.println("\nUsing for-each loop:");
        System.out.println("--------------------");
        int count = 1;
        for (Shape s : shapes) {
            System.out.println("Shape " + count + " area: " + s.getArea());
            count++;
        }
    }
}

/*
 * KEY CONCEPTS FOR BEGINNERS:
 * 
 * 1. ABSTRACT CLASS: A class that cannot be instantiated directly
 *    - Used as a blueprint for other classes
 *    - Can have abstract methods (without body)
 * 
 * 2. ARRAY OF ABSTRACT CLASS:
 *    - Shape[] shapes = new Shape[3];
 *    - Can hold any object that extends Shape
 * 
 * 3. POLYMORPHISM:
 *    - One method call (getArea()) works differently for each shape type
 *    - The correct method is called based on the actual object type
 * 
 * 4. extends KEYWORD:
 *    - Used to inherit from a class
 *    - Rectangle extends Shape means "Rectangle is a type of Shape"
 */
