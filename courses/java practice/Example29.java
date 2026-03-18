// Example29: Abstraction - Beginner Tutorial
// This explains abstraction in Java

/*
 * WHAT IS ABSTRACTION?
 * -------------------
 * Abstraction is the process of hiding implementation details and showing 
 * only the essential features to the user.
 * 
 * In simpler terms: "Show WHAT, not HOW"
 * 
 * Real-world examples:
 * - TV Remote: You press buttons, don't know internal circuits
 * - Car: You drive, don't know engine details
 * - Phone: You make calls, don't know network protocols
 * 
 * In Java, abstraction is achieved through:
 * 1. Abstract Classes
 * 2. Interfaces
 */

// ===== ABSTRACT CLASS =====

/*
 * ABSTRACT CLASS:
 * - Use 'abstract' keyword
 * - Cannot be instantiated (cannot create object)
 * - Can have both abstract and non-abstract methods
 * - Abstract methods have no body, just signature
 * - Used as a blueprint for subclasses
 */

// Abstract class - blueprint for all vehicles
abstract class Vehicle {
    protected String brand;
    protected String model;
    protected int year;
    
    // Constructor
    public Vehicle(String brand, String model, int year) {
        this.brand = brand;
        this.model = model;
        this.year = year;
    }
    
    // Non-abstract method (concrete method)
    public void displayInfo() {
        System.out.println("Brand: " + brand);
        System.out.println("Model: " + model);
        System.out.println("Year: " + year);
    }
    
    // Abstract methods - no body, must be implemented by subclasses
    public abstract void start();
    public abstract void stop();
    public abstract void move();
    
    // Another abstract method
    public abstract int getMaxSpeed();
}

// Concrete class - extends abstract class
class Car extends Vehicle {
    private int numDoors;
    private int currentSpeed;
    
    public Car(String brand, String model, int year, int doors) {
        super(brand, model, year);
        this.numDoors = doors;
        this.currentSpeed = 0;
    }
    
    // Must implement all abstract methods
    @Override
    public void start() {
        System.out.println(brand + " " + model + " is starting...");
        System.out.println("Engine started!");
    }
    
    @Override
    public void stop() {
        System.out.println(brand + " " + model + " is stopping...");
        currentSpeed = 0;
        System.out.println("Car stopped!");
    }
    
    @Override
    public void move() {
        currentSpeed = 60;
        System.out.println(brand + " " + model + " is moving at " + currentSpeed + " mph");
    }
    
    @Override
    public int getMaxSpeed() {
        return 120;
    }
    
    // Own method
    public void honk() {
        System.out.println("Beep! Beep!");
    }
}

class Motorcycle extends Vehicle {
    private boolean hasSidecar;
    private int currentSpeed;
    
    public Motorcycle(String brand, String model, int year, boolean sidecar) {
        super(brand, model, year);
        this.hasSidecar = sidecar;
        this.currentSpeed = 0;
    }
    
    @Override
    public void start() {
        System.out.println(brand + " " + model + " is starting...");
        System.out.println("Motorcycle engine started!");
    }
    
    @Override
    public void stop() {
        System.out.println(brand + " " + model + " is stopping...");
        currentSpeed = 0;
    }
    
    @Override
    public void move() {
        currentSpeed = 80;
        System.out.println(brand + " " + model + " is moving at " + currentSpeed + " mph");
    }
    
    @Override
    public int getMaxSpeed() {
        return 150;
    }
    
    public void wheelie() {
        System.out.println("Doing a wheelie!");
    }
}

class Bicycle extends Vehicle {
    private int numGears;
    private int currentSpeed;
    
    public Bicycle(String brand, String model, int year, int gears) {
        super(brand, model, year);
        this.numGears = gears;
        this.currentSpeed = 0;
    }
    
    @Override
    public void start() {
        System.out.println(brand + " " + model + " is ready to ride!");
    }
    
    @Override
    public void stop() {
        System.out.println(brand + " " + model + " stopped!");
        currentSpeed = 0;
    }
    
    @Override
    public void move() {
        currentSpeed = 15;
        System.out.println(brand + " " + model + " is pedaling at " + currentSpeed + " mph");
    }
    
    @Override
    public int getMaxSpeed() {
        return 25;
    }
    
    public void ringBell() {
        System.out.println("Ring! Ring!");
    }
}

// ===== INTERFACE =====

/*
 * INTERFACE:
 * - Use 'interface' keyword
 * - All methods are abstract by default (Java 7)
 * - Java 8+: can have default and static methods
 * - Java 9+: can have private methods
 * - Variables are public static final by default
 * - A class can implement multiple interfaces
 * - More abstract than abstract class
 */

// Interface for printable items
interface Printable {
    // Abstract method
    void print();
    
    // Default method (Java 8+)
    default void printInfo() {
        System.out.println("This is a printable item");
    }
}

// Interface for things that can be copied
interface Copyable {
    void copy();
}

// Class implementing multiple interfaces
class Document implements Printable, Copyable {
    private String content;
    
    public Document(String content) {
        this.content = content;
    }
    
    @Override
    public void print() {
        System.out.println("Printing document: " + content);
    }
    
    @Override
    public void copy() {
        System.out.println("Copying document: " + content);
    }
    
    // Own method
    public void edit() {
        System.out.println("Editing document...");
    }
}

// Interface with inheritance
interface Drawable extends Printable {
    void draw();
}

class Image implements Drawable {
    private String name;
    
    public Image(String name) {
        this.name = name;
    }
    
    @Override
    public void print() {
        System.out.println("Printing image: " + name);
    }
    
    @Override
    public void draw() {
        System.out.println("Drawing image: " + name);
    }
}

// ===== MULTIPLE INTERFACES =====

interface Readable {
    void read();
}

interface Writable {
    void write();
}

// Class implementing multiple interfaces
class FileHandler implements Readable, Writable {
    private String filename;
    
    public FileHandler(String filename) {
        this.filename = filename;
    }
    
    @Override
    public void read() {
        System.out.println("Reading from file: " + filename);
    }
    
    @Override
    public void write() {
        System.out.println("Writing to file: " + filename);
    }
}

// ===== ABSTRACT CLASS WITH INTERFACE =====

abstract class Shape implements Drawable {
    protected String color;
    
    public Shape(String color) {
        this.color = color;
    }
    
    // Abstract method for area
    public abstract double area();
    
    // Concrete method
    public void displayColor() {
        System.out.println("Color: " + color);
    }
}

class Square extends Shape {
    private double side;
    
    public Square(double side, String color) {
        super(color);
        this.side = side;
    }
    
    @Override
    public double area() {
        return side * side;
    }
    
    @Override
    public void draw() {
        System.out.println("Drawing a square with side " + side);
    }
    
    @Override
    public void print() {
        System.out.println("Printing square with area: " + area());
    }
}

// ===== MAIN CLASS =====
public class Example29 {
    public static void main(String[] args) {
        
        System.out.println("=== ABSTRACT CLASS ===\n");
        
        // Cannot create object of abstract class
        // Vehicle v = new Vehicle();  // ERROR!
        
        // But can create reference
        Vehicle vehicle;
        
        // Create car object
        System.out.println("--- Car ---");
        Car car = new Car("Toyota", "Camry", 2022, 4);
        car.displayInfo();
        car.start();
        car.move();
        car.stop();
        System.out.println("Max Speed: " + car.getMaxSpeed() + " mph");
        
        System.out.println("\n--- Motorcycle ---");
        Motorcycle bike = new Motorcycle("Harley-Davidson", "Sportster", 2021, false);
        bike.displayInfo();
        bike.start();
        bike.move();
        bike.wheelie();
        
        System.out.println("\n--- Bicycle ---");
        Bicycle bike2 = new Bicycle("Trek", "Mountain", 2023, 21);
        bike2.displayInfo();
        bike2.start();
        bike2.move();
        
        // ===== POLYMORPHISM WITH ABSTRACT CLASS =====
        System.out.println("\n=== Polymorphism with Abstract Class ===\n");
        
        Vehicle[] vehicles = new Vehicle[3];
        vehicles[0] = new Car("Honda", "Civic", 2021, 4);
        vehicles[1] = new Motorcycle("Yamaha", "R1", 2022, false);
        vehicles[2] = new Bicycle("Giant", "Road", 2023, 16);
        
        for (Vehicle v : vehicles) {
            System.out.println("\n--- " + v.getClass().getSimpleName() + " ---");
            v.displayInfo();
            v.start();
            v.move();
            v.stop();
            System.out.println("Max Speed: " + v.getMaxSpeed());
        }
        
        // ===== INTERFACE =====
        System.out.println("\n=== INTERFACE ===\n");
        
        Document doc = new Document("Hello World");
        doc.print();
        doc.copy();
        doc.edit();
        
        System.out.println("\n--- Multiple Interfaces ---");
        Image img = new Image("Sunset");
        img.print();
        img.draw();
        
        System.out.println("\n--- File Handler ---");
        FileHandler file = new FileHandler("data.txt");
        file.read();
        file.write();
        
        // ===== ABSTRACT CLASS WITH INTERFACE =====
        System.out.println("\n=== Abstract Class Implementing Interface ===\n");
        
        Square square = new Square(5, "Blue");
        square.displayColor();
        System.out.println("Area: " + square.area());
        square.draw();
        square.print();
        
        // ===== KEY DIFFERENCES =====
        System.out.println("\n=== Abstract Class vs Interface ===\n");
        
        System.out.println("ABSTRACT CLASS:");
        System.out.println("- Can have constructors");
        System.out.println("- Can have instance variables");
        System.out.println("- Can have concrete methods");
        System.out.println("- Can have abstract methods");
        System.out.println("- Single inheritance only");
        
        System.out.println("\nINTERFACE:");
        System.out.println("- Cannot have constructors");
        System.out.println("- Variables are final by default");
        System.out.println("- Java 8+: can have default methods");
        System.out.println("- All methods abstract (until Java 8)");
        System.out.println("- Multiple inheritance supported");
    }
}

/*
 * KEY CONCEPTS FOR BEGINNERS:
 * 
 * 1. WHAT IS ABSTRACTION?
 *    - Hiding implementation details
 *    - Showing only essential features
 *    - "Show WHAT, not HOW"
 * 
 * 2. ABSTRACT CLASS:
 *    - Cannot be instantiated
 *    - Use 'abstract' keyword
 *    - Can have abstract methods (no body)
 *    - Can have concrete methods (with body)
 *    - Used as blueprint
 *    - Can have constructors
 * 
 * 3. INTERFACE:
 *    - Cannot be instantiated
 *    - Use 'interface' keyword
 *    - All methods are abstract (pre-Java 8)
 *    - Variables are public static final
 *    - Class can implement multiple interfaces
 *    - Java 8+: default and static methods
 * 
 * 4. WHEN TO USE:
 *    - Abstract Class: When sharing code/data between related classes
 *    - Interface: When defining capabilities/contracts
 * 
 * 5. ABSTRACT METHODS:
 *    - No body, just declaration
 *    - Must be implemented by subclasses
 *    - Syntax: public abstract void methodName();
 * 
 * 6. BENEFITS:
 *    - Reduce complexity
 *    - Hide implementation details
 *    - Easy to change implementation
 *    - Code reusability
 */
