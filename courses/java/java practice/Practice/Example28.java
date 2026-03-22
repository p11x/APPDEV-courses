/*
 * SUB TOPIC: Polymorphism
 * 
 * DEFINITION:
 * Polymorphism means "many forms". In Java, it allows the same method to behave differently based on 
 * the object calling it. There are two types: compile-time (method overloading) and runtime (method overriding).
 * 
 * FUNCTIONALITIES:
 * 1. Method overloading
 * 2. Method overriding
 * 3. Runtime polymorphism
 * 4. Using polymorphism with arrays
 * 5. Using instanceof operator
 */

class Shape {
    protected String color;
    
    public Shape(String color) {
        this.color = color;
    }
    
    public double area() {
        return 0;
    }
    
    public double perimeter() {
        return 0;
    }
    
    public void display() {
        System.out.println("Shape color: " + color);
    }
}

class Circle extends Shape {
    private double radius;
    
    public Circle(double radius) {
        super("Red");
        this.radius = radius;
    }
    
    @Override
    public double area() {
        return Math.PI * radius * radius;
    }
    
    @Override
    public double perimeter() {
        return 2 * Math.PI * radius;
    }
    
    @Override
    public void display() {
        System.out.println("Circle - Radius: " + radius + ", Color: " + color);
    }
}

class Rectangle extends Shape {
    private double width;
    private double height;
    
    public Rectangle(double width, double height) {
        super("Blue");
        this.width = width;
        this.height = height;
    }
    
    @Override
    public double area() {
        return width * height;
    }
    
    @Override
    public double perimeter() {
        return 2 * (width + height);
    }
}

class Calculator {
    // Method overloading - same name, different parameters
    public int add(int a, int b) {
        return a + b;
    }
    
    public int add(int a, int b, int c) {
        return a + b + c;
    }
    
    public double add(double a, double b) {
        return a + b;
    }
}

public class Example28 {
    public static void main(String[] args) {
        
        // Topic Explanation with Code: Runtime Polymorphism
        System.out.println("=== Runtime Polymorphism ===");
        
        Shape shape1 = new Circle(5.0);
        Shape shape2 = new Rectangle(4.0, 6.0);
        
        System.out.println("Circle Area: " + shape1.area());
        System.out.println("Rectangle Area: " + shape2.area());
        
        // Real-time Example 1: Method Overriding
        System.out.println("\n=== Method Overriding ===");
        
        Circle circle = new Circle(3.0);
        circle.display();
        
        // Real-time Example 2: Method Overloading
        System.out.println("\n=== Method Overloading ===");
        
        Calculator calc = new Calculator();
        System.out.println("Add 2 ints: " + calc.add(5, 3));
        System.out.println("Add 3 ints: " + calc.add(5, 3, 2));
        System.out.println("Add 2 doubles: " + calc.add(5.5, 3.3));
        
        // Real-time Example 3: Array of Shapes
        System.out.println("\n=== Array of Shapes ===");
        
        Shape[] shapes = new Shape[3];
        shapes[0] = new Circle(2.0);
        shapes[1] = new Rectangle(3.0, 4.0);
        shapes[2] = new Circle(5.0);
        
        double totalArea = 0;
        for (Shape s : shapes) {
            totalArea += s.area();
        }
        System.out.println("Total Area: " + totalArea);
        
        // Real-time Example 4: Using instanceof
        System.out.println("\n=== instanceof Operator ===");
        
        Shape s1 = new Circle(5.0);
        System.out.println("Is Circle? " + (s1 instanceof Circle));
        System.out.println("Is Shape? " + (s1 instanceof Shape));
        
        // Real-time Example 5: Passing Shapes to methods
        System.out.println("\n=== Polymorphism in Methods ===");
        
        printArea(new Circle(3.0));
        printArea(new Rectangle(4.0, 5.0));
        
        // Real-time Example 6: Total perimeter calculation
        System.out.println("\n=== Total Perimeter ===");
        
        Shape[] shapes2 = {new Circle(2.0), new Rectangle(3.0, 4.0)};
        double totalPerimeter = 0;
        for (Shape s : shapes2) {
            totalPerimeter += s.perimeter();
        }
        System.out.println("Total Perimeter: " + totalPerimeter);
    }
    
    public static void printArea(Shape shape) {
        System.out.println("Area: " + shape.area());
    }
}
