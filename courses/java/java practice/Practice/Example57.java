/*
 * SUB TOPIC: Java Math Class and Utilities
 * 
 * DEFINITION:
 * The Math class provides mathematical functions and constants. It includes methods for basic arithmetic,
 * trigonometry, logarithms, and more. All methods are static, so no object creation needed.
 * 
 * FUNCTIONALITIES:
 * 1. Basic math - abs, min, max, pow, sqrt
 * 2. Trigonometric - sin, cos, tan
 * 3. Logarithmic - log, log10, exp
 * 4. Rounding - round, floor, ceil
 * 5. Random - random
 * 6. Constants - PI, E
 */

public class Example57 {
    public static void main(String[] args) {
        
        // Basic Math Methods
        System.out.println("=== Basic Math ===");
        System.out.println("abs(-5): " + Math.abs(-5));
        System.out.println("min(10, 20): " + Math.min(10, 20));
        System.out.println("max(10, 20): " + Math.max(10, 20));
        
        // Power and Square Root
        System.out.println("\n=== Power & Root ===");
        System.out.println("pow(2, 3): " + Math.pow(2, 3));
        System.out.println("sqrt(16): " + Math.sqrt(16));
        System.out.println("cbrt(27): " + Math.cbrt(27));
        
        // Trigonometric
        System.out.println("\n=== Trigonometric ===");
        System.out.println("sin(0): " + Math.sin(0));
        System.out.println("cos(0): " + Math.cos(0));
        System.out.println("tan(0): " + Math.tan(0));
        
        // Logarithmic
        System.out.println("\n=== Logarithmic ===");
        System.out.println("log(2.718): " + Math.log(2.718));
        System.out.println("log10(100): " + Math.log10(100));
        System.out.println("exp(1): " + Math.exp(1));
        
        // Rounding
        System.out.println("\n=== Rounding ===");
        System.out.println("floor(5.7): " + Math.floor(5.7));
        System.out.println("ceil(5.2): " + Math.ceil(5.2));
        System.out.println("round(5.5): " + Math.round(5.5));
        
        // Constants
        System.out.println("\n=== Constants ===");
        System.out.println("PI: " + Math.PI);
        System.out.println("E: " + Math.E);
        
        // Random
        System.out.println("\n=== Random ===");
        System.out.println("random(): " + Math.random());
        System.out.println("random() * 10: " + Math.random() * 10);
        
        // Real-time Example 1: Calculate distance
        System.out.println("\n=== Example 1: Distance ===");
        
        class Point {
            double x, y;
            
            Point(double x, double y) {
                this.x = x;
                this.y = y;
            }
            
            double distance(Point other) {
                double dx = this.x - other.x;
                double dy = this.y - other.y;
                return Math.sqrt(dx * dx + dy * dy);
            }
        }
        
        Point p1 = new Point(0, 0);
        Point p2 = new Point(3, 4);
        System.out.println("Distance: " + p1.distance(p2));
        
        // Real-time Example 2: Circle area
        System.out.println("\n=== Example 2: Circle Area ===");
        
        double radius = 5;
        double area = Math.PI * Math.pow(radius, 2);
        System.out.println("Radius: " + radius);
        System.out.println("Area: " + area);
        
        // Real-time Example 3: Random number in range
        System.out.println("\n=== Example 3: Random Range ===");
        
        int min = 1;
        int max = 100;
        int randomNum = (int)(Math.random() * (max - min + 1)) + min;
        System.out.println("Random 1-100: " + randomNum);
        
        // Real-time Example 4: Compound interest
        System.out.println("\n=== Example 4: Interest ===");
        
        double principal = 1000;
        double rate = 0.05;
        int years = 10;
        
        double amount = principal * Math.pow(1 + rate, years);
        System.out.println("Principal: $" + principal);
        System.out.println("Amount after 10 years: $" + amount);
        
        // Real-time Example 5: Hypotenuse
        System.out.println("\n=== Example 5: Hypotenuse ===");
        
        double a = 3;
        double b = 4;
        double c = Math.hypot(a, b);
        System.out.println("a=" + a + ", b=" + b + ", c=" + c);
        
        // Real-time Example 6: Angle conversion
        System.out.println("\n=== Example 6: Angle ===");
        
        double degrees = 90;
        double radians = Math.toRadians(degrees);
        System.out.println(degrees + " degrees = " + radians + " radians");
        
        double back = Math.toDegrees(radians);
        System.out.println(back + " degrees");
    }
}
