/*
 * SUB TOPIC: Abstraction
 * 
 * DEFINITION:
 * Abstraction is the process of hiding implementation details and showing only essential features to the user.
 * In Java, abstraction is achieved through abstract classes and interfaces. It shows "WHAT" without showing "HOW".
 * 
 * FUNCTIONALITIES:
 * 1. Abstract classes
 * 2. Abstract methods
 * 3. Interfaces
 * 4. Implementing abstraction
 * 5. Real-world examples
 */

abstract class Vehicle {
    protected String brand;
    protected String model;
    
    public Vehicle(String brand, String model) {
        this.brand = brand;
        this.model = model;
    }
    
    public void displayInfo() {
        System.out.println("Brand: " + brand);
        System.out.println("Model: " + model);
    }
    
    // Abstract method - no implementation
    public abstract void move();
    public abstract int getMaxSpeed();
}

class Car extends Vehicle {
    private int speed;
    
    public Car(String brand, String model, int speed) {
        super(brand, model);
        this.speed = speed;
    }
    
    @Override
    public void move() {
        System.out.println(brand + " " + model + " is driving on road...");
    }
    
    @Override
    public int getMaxSpeed() {
        return speed;
    }
}

class Motorcycle extends Vehicle {
    private int speed;
    
    public Motorcycle(String brand, String model, int speed) {
        super(brand, model);
        this.speed = speed;
    }
    
    @Override
    public void move() {
        System.out.println(brand + " " + model + " is riding on road...");
    }
    
    @Override
    public int getMaxSpeed() {
        return speed;
    }
}

abstract class Shape {
    protected String color;
    
    public Shape(String color) {
        this.color = color;
    }
    
    public abstract double area();
    public abstract double perimeter();
}

class Square extends Shape {
    private double side;
    
    public Square(double side) {
        super("Red");
        this.side = side;
    }
    
    @Override
    public double area() {
        return side * side;
    }
    
    @Override
    public double perimeter() {
        return 4 * side;
    }
}

public class Example29 {
    public static void main(String[] args) {
        
        // Topic Explanation with Code: Abstraction
        System.out.println("=== Abstraction ===");
        
        Car car = new Car("Toyota", "Camry", 180);
        car.displayInfo();
        car.move();
        System.out.println("Max Speed: " + car.getMaxSpeed() + " km/h");
        
        // Real-time Example 1: Motorcycle
        System.out.println("\n=== Motorcycle ===");
        
        Motorcycle bike = new Motorcycle("Honda", "CBR", 200);
        bike.move();
        System.out.println("Max Speed: " + bike.getMaxSpeed() + " km/h");
        
        // Real-time Example 2: Array of Vehicles
        System.out.println("\n=== Array of Vehicles ===");
        
        Vehicle[] vehicles = new Vehicle[2];
        vehicles[0] = new Car("Ford", "Mustang", 250);
        vehicles[1] = new Motorcycle("Yamaha", "R1", 300);
        
        for (Vehicle v : vehicles) {
            v.displayInfo();
            v.move();
        }
        
        // Real-time Example 3: Square (Abstract class)
        System.out.println("\n=== Square ===");
        
        Square square = new Square(5);
        System.out.println("Area: " + square.area());
        System.out.println("Perimeter: " + square.perimeter());
        
        // Real-time Example 4: Calculate total area
        System.out.println("\n=== Calculate Areas ===");
        
        Square[] squares = {new Square(3), new Square(4), new Square(5)};
        double totalArea = 0;
        for (Square s : squares) {
            totalArea += s.area();
        }
        System.out.println("Total Area: " + totalArea);
        
        // Real-time Example 5: Polymorphism with abstraction
        System.out.println("\n=== Polymorphism ===");
        
        Vehicle v1 = new Car("BMW", "X5", 240);
        Vehicle v2 = new Motorcycle("Ducati", "Panigale", 350);
        
        Vehicle[] vehicles2 = {v1, v2};
        for (Vehicle v : vehicles2) {
            System.out.println(v.brand + " speed: " + v.getMaxSpeed());
        }
        
        // Real-time Example 6: Interface-like behavior
        System.out.println("\n=== Multiple Implementations ===");
        
        printVehicleInfo(new Car("Audi", "A4", 220));
        printVehicleInfo(new Motorcycle("Kawasaki", "Ninja", 300));
    }
    
    public static void printVehicleInfo(Vehicle v) {
        v.displayInfo();
        v.move();
    }
}
