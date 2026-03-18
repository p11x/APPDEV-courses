// Example28: Polymorphism - Beginner Tutorial
// This explains polymorphism in Java

/*
 * WHAT IS POLYMORPHISM?
 * ---------------------
 * Polymorphism means "many forms". In Java, it allows the same method 
 * to behave differently based on the object calling it.
 * 
 * Types of Polymorphism:
 * 1. Compile-time Polymorphism (Method Overloading)
 * 2. Runtime Polymorphism (Method Overriding)
 * 
 * Benefits:
 * - One interface, multiple implementations
 * - Flexible and extensible code
 * - Easy to maintain
 */

// ===== PARENT CLASS =====

class Shape {
    protected String color;
    
    public Shape() {
        color = "Unknown";
    }
    
    public Shape(String color) {
        this.color = color;
    }
    
    // Method to calculate area - to be overridden
    public double area() {
        System.out.println("Calculating area...");
        return 0;
    }
    
    // Method to calculate perimeter - to be overridden
    public double perimeter() {
        System.out.println("Calculating perimeter...");
        return 0;
    }
    
    public void display() {
        System.out.println("Shape color: " + color);
    }
}

// ===== CHILD CLASSES =====

class Circle extends Shape {
    private double radius;
    
    public Circle(double radius) {
        super("Red");  // Default color
        this.radius = radius;
    }
    
    public Circle(double radius, String color) {
        super(color);
        this.radius = radius;
    }
    
    // Override area method
    @Override
    public double area() {
        return Math.PI * radius * radius;
    }
    
    // Override perimeter method
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
    
    public Rectangle(double width, double height, String color) {
        super(color);
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
    
    @Override
    public void display() {
        System.out.println("Rectangle - Width: " + width + ", Height: " + height + ", Color: " + color);
    }
}

class Triangle extends Shape {
    private double base;
    private double height;
    private double side1;
    private double side2;
    
    public Triangle(double base, double height, double side1, double side2) {
        super("Green");
        this.base = base;
        this.height = height;
        this.side1 = side1;
        this.side2 = side2;
    }
    
    @Override
    public double area() {
        return 0.5 * base * height;
    }
    
    @Override
    public double perimeter() {
        return base + side1 + side2;
    }
    
    @Override
    public void display() {
        System.out.println("Triangle - Base: " + base + ", Height: " + height + ", Color: " + color);
    }
}

// ===== METHOD OVERLOADING (Compile-time Polymorphism) =====

class Calculator {
    // Same method name, different parameters
    
    // Add two integers
    public int add(int a, int b) {
        System.out.println("Adding two integers: " + a + " + " + b);
        return a + b;
    }
    
    // Add three integers
    public int add(int a, int b, int c) {
        System.out.println("Adding three integers: " + a + " + " + b + " + " + c);
        return a + b + c;
    }
    
    // Add two doubles
    public double add(double a, double b) {
        System.out.println("Adding two doubles: " + a + " + " + b);
        return a + b;
    }
    
    // Add int and double
    public double add(int a, double b) {
        System.out.println("Adding int and double: " + a + " + " + b);
        return a + b;
    }
    
    // Concatenate strings
    public String add(String a, String b) {
        System.out.println("Concatenating strings: \"" + a + "\" + \"" + b + "\"");
        return a + b;
    }
}

// ===== ANOTHER OVERLOADING EXAMPLE =====

class Printer {
    // Print integer
    public void print(int num) {
        System.out.println("Printing integer: " + num);
    }
    
    // Print double
    public void print(double num) {
        System.out.println("Printing double: " + num);
    }
    
    // Print string
    public void print(String text) {
        System.out.println("Printing string: " + text);
    }
    
    // Print with message
    public void print(String message, int times) {
        for (int i = 0; i < times; i++) {
            System.out.println(message);
        }
    }
    
    // Print character
    public void print(char c) {
        System.out.println("Printing character: " + c);
    }
}

// ===== MAIN CLASS =====
public class Example28 {
    public static void main(String[] args) {
        
        System.out.println("=== RUNTIME POLYMORPHISM (Method Overriding) ===\n");
        
        // Create different shape objects
        Shape shape1 = new Circle(5.0);
        Shape shape2 = new Rectangle(4.0, 6.0);
        Shape shape3 = new Triangle(3.0, 4.0, 5.0, 5.0);
        
        // Call area() - different implementations based on actual object type
        System.out.println("Circle Area: " + shape1.area());
        System.out.println("Rectangle Area: " + shape2.area());
        System.out.println("Triangle Area: " + shape3.area());
        
        System.out.println();
        
        // Call perimeter
        System.out.println("Circle Perimeter: " + shape1.perimeter());
        System.out.println("Rectangle Perimeter: " + shape2.perimeter());
        System.out.println("Triangle Perimeter: " + shape3.perimeter());
        
        System.out.println("\n--- Using display() method ---");
        shape1.display();
        shape2.display();
        shape3.display();
        
        // ===== POLYMORPHISM WITH ARRAYS =====
        System.out.println("\n=== Polymorphism with Arrays ===\n");
        
        Shape[] shapes = new Shape[4];
        shapes[0] = new Circle(3.0);
        shapes[1] = new Rectangle(5.0, 3.0);
        shapes[2] = new Triangle(4.0, 3.0, 3.0, 4.0);
        shapes[3] = new Circle(2.5);
        
        System.out.println("All Shapes:");
        double totalArea = 0;
        for (Shape s : shapes) {
            s.display();
            System.out.println("Area: " + s.area());
            totalArea += s.area();
            System.out.println();
        }
        
        System.out.println("Total Area: " + totalArea);
        
        // ===== COMPILE-TIME POLYMORPHISM (Method Overloading) =====
        System.out.println("\n=== COMPILE-TIME POLYMORPHISM (Method Overloading) ===\n");
        
        Calculator calc = new Calculator();
        
        // Same method name, different parameters
        int result1 = calc.add(5, 3);
        System.out.println("Result: " + result1);
        
        int result2 = calc.add(5, 3, 2);
        System.out.println("Result: " + result2);
        
        double result3 = calc.add(5.5, 3.3);
        System.out.println("Result: " + result3);
        
        double result4 = calc.add(5, 3.5);
        System.out.println("Result: " + result4);
        
        String result5 = calc.add("Hello", " World");
        System.out.println("Result: " + result5);
        
        // ===== PRINTER OVERLOADING =====
        System.out.println("\n--- Printer Overloading ---");
        
        Printer printer = new Printer();
        
        printer.print(42);
        printer.print(3.14159);
        printer.print("Hello Java!");
        printer.print("Welcome", 3);
        printer.print('A');
        
        // ===== POLYMORPHISM WITH METHOD PARAMETERS =====
        System.out.println("\n=== Polymorphism with Method Parameters ===\n");
        
        Circle c = new Circle(5.0);
        Rectangle r = new Rectangle(4.0, 6.0);
        
        printArea(c);
        printArea(r);
        
        // Can pass any Shape
        printArea(new Triangle(3.0, 4.0, 3.0, 4.0));
        
        // ===== instanceof OPERATOR =====
        System.out.println("\n=== Using instanceof ===\n");
        
        Shape s1 = new Circle(5.0);
        Shape s2 = new Rectangle(4.0, 6.0);
        Shape s3 = new Triangle(3.0, 4.0, 3.0, 4.0);
        
        checkType(s1);
        checkType(s2);
        checkType(s3);
    }
    
    // Method that accepts any Shape (polymorphism)
    public static void printArea(Shape shape) {
        System.out.println("Shape type: " + shape.getClass().getSimpleName());
        System.out.println("Area: " + shape.area());
    }
    
    // Method to check object type
    public static void checkType(Shape shape) {
        if (shape instanceof Circle) {
            System.out.println("This is a Circle");
        } else if (shape instanceof Rectangle) {
            System.out.println("This is a Rectangle");
        } else if (shape instanceof Triangle) {
            System.out.println("This is a Triangle");
        } else {
            System.out.println("Unknown shape");
        }
    }
}

/*
 * KEY CONCEPTS FOR BEGINNERS:
 * 
 * 1. TYPES OF POLYMORPHISM:
 *    a) Compile-time (Overloading) - Same method name, different parameters
 *    b) Runtime (Overriding) - Same method signature, different implementation
 * 
 * 2. METHOD OVERLOADING:
 *    - Same method name
 *    - Different number or type of parameters
 *    - Compile time decision
 *    - Also called "Static Polymorphism"
 * 
 * 3. METHOD OVERRIDING:
 *    - Child class provides specific implementation
 *    - Same method signature as parent
 *    - Runtime decision
 *    - Also called "Dynamic Polymorphism"
 *    - Use @Override annotation
 * 
 * 4. WHY POLYMORPHISM?
 *    - One interface, many implementations
 *    - Code reusability
 *    - Flexibility
 *    - Easy to extend
 * 
 * 5. RULES FOR OVERRIDING:
 *    - Must have same method name
 *    - Must have same parameters
 *    - Cannot override private methods
 *    - Cannot override static methods
 *    - Return type must be compatible
 * 
 * 6. RULES FOR OVERLOADING:
 *    - Must have same method name
 *    - Must have different parameters
 *    - Can have different return types
 *    - Can have different access modifiers
 * 
 * 7. instanceof:
 *    - Checks if object is of certain type
 *    - Returns true/false
 *    - Used for safe casting
 */
