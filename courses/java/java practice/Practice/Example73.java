/*
 * SUB TOPIC: Abstract Classes and Methods
 * 
 * DEFINITION:
 * Abstract classes cannot be instantiated and may contain abstract methods (without body) that subclasses must implement.
 */

abstract class Shape {
    abstract double area();
}

class Circle extends Shape {
    double r;
    Circle(double r) { this.r = r; }
    double area() { return Math.PI * r * r; }
}

public class Example73 {
    public static void main(String[] args) {
        Circle c = new Circle(5);
        System.out.println("Area: " + c.area());
    }
}
