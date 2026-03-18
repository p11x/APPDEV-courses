// Example27: Inheritance - Beginner Tutorial
// This explains inheritance in Java

/*
 * WHAT IS INHERITANCE?
 * --------------------
 * Inheritance is a mechanism where a new class (child/subclass) 
 * inherits properties and behaviors from an existing class (parent/superclass).
 * 
 * Benefits:
 * - Code reusability
 * - Avoids duplicating code
 * - Creates logical hierarchy
 * - Supports polymorphism
 * 
 * Key Points:
 * - Use 'extends' keyword to inherit
 * - A class can only extend one class (single inheritance)
 * - Private members are NOT inherited
 * - Constructors are NOT inherited
 */

// ===== PARENT CLASS (SUPERCLASS) =====

// Base class - Parent
class Animal {
    // These fields will be inherited by subclasses
    protected String name;      // protected = accessible in subclasses
    protected int age;
    protected String color;
    
    // Constructor
    public Animal(String name, int age, String color) {
        this.name = name;
        this.age = age;
        this.color = color;
    }
    
    // Default constructor
    public Animal() {
        name = "Unknown";
        age = 0;
        color = "Unknown";
    }
    
    // Common method - will be inherited
    public void eat() {
        System.out.println(name + " is eating...");
    }
    
    public void sleep() {
        System.out.println(name + " is sleeping...");
    }
    
    // Common method to display info
    public void displayInfo() {
        System.out.println("Name: " + name);
        System.out.println("Age: " + age);
        System.out.println("Color: " + color);
    }
    
    // Getter methods
    public String getName() {
        return name;
    }
    
    public int getAge() {
        return age;
    }
}

// ===== CHILD CLASSES (SUBCLASSES) =====

// Dog inherits from Animal
class Dog extends Animal {
    private String breed;
    
    // Constructor - use super() to call parent constructor
    public Dog(String name, int age, String color, String breed) {
        super(name, age, color);  // Call parent constructor
        this.breed = breed;
    }
    
    // Subclass-specific method
    public void bark() {
        System.out.println(name + " says: Woof! Woof!");
    }
    
    // Override parent method
    @Override
    public void displayInfo() {
        super.displayInfo();  // Call parent's displayInfo
        System.out.println("Breed: " + breed);
    }
    
    // Getter for breed
    public String getBreed() {
        return breed;
    }
}

// Cat inherits from Animal
class Cat extends Animal {
    private boolean isIndoor;
    
    public Cat(String name, int age, String color, boolean indoor) {
        super(name, age, color);
        this.isIndoor = indoor;
    }
    
    public void meow() {
        System.out.println(name + " says: Meow!");
    }
    
    public void purr() {
        System.out.println(name + " is purring...");
    }
    
    @Override
    public void displayInfo() {
        super.displayInfo();
        System.out.println("Type: " + (isIndoor ? "Indoor" : "Outdoor"));
    }
    
    public boolean isIndoor() {
        return isIndoor;
    }
}

// Bird inherits from Animal
class Bird extends Animal {
    private boolean canFly;
    
    public Bird(String name, int age, String color, boolean canFly) {
        super(name, age, color);
        this.canFly = canFly;
    }
    
    public void sing() {
        System.out.println(name + " is singing...");
    }
    
    @Override
    public void eat() {
        System.out.println(name + " is eating seeds...");
    }
    
    @Override
    public void displayInfo() {
        super.displayInfo();
        System.out.println("Can fly: " + canFly);
    }
}

// ===== MULTILEVEL INHERITANCE =====

// Parent class
class Vehicle {
    protected String brand;
    protected String model;
    protected int year;
    
    public Vehicle(String brand, String model, int year) {
        this.brand = brand;
        this.model = model;
        this.year = year;
    }
    
    public void start() {
        System.out.println(brand + " " + model + " is starting...");
    }
    
    public void stop() {
        System.out.println(brand + " " + model + " is stopping...");
    }
    
    public void displayInfo() {
        System.out.println("Brand: " + brand);
        System.out.println("Model: " + model);
        System.out.println("Year: " + year);
    }
}

// Car inherits from Vehicle
class Car extends Vehicle {
    private int numDoors;
    
    public Car(String brand, String model, int year, int doors) {
        super(brand, model, year);
        this.numDoors = doors;
    }
    
    public void drive() {
        System.out.println(brand + " " + model + " is driving...");
    }
    
    @Override
    public void displayInfo() {
        super.displayInfo();
        System.out.println("Doors: " + numDoors);
    }
}

// SportsCar inherits from Car (Multilevel)
class SportsCar extends Car {
    private int topSpeed;
    
    public SportsCar(String brand, String model, int year, int doors, int speed) {
        super(brand, model, year, doors);
        this.topSpeed = speed;
    }
    
    public void race() {
        System.out.println(brand + " is racing at " + topSpeed + " mph!");
    }
    
    @Override
    public void displayInfo() {
        super.displayInfo();
        System.out.println("Top Speed: " + topSpeed + " mph");
    }
}

// ===== MAIN CLASS =====
public class Example27 {
    public static void main(String[] args) {
        
        System.out.println("=== SINGLE INHERITANCE ===\n");
        
        // Create Animal object
        Animal animal = new Animal("Generic Animal", 5, "Brown");
        animal.displayInfo();
        animal.eat();
        animal.sleep();
        
        System.out.println("\n--- Dog Object ---");
        Dog dog = new Dog("Buddy", 3, "Golden", "Golden Retriever");
        dog.displayInfo();
        dog.eat();      // Inherited from Animal
        dog.sleep();    // Inherited from Animal
        dog.bark();     // Dog's own method
        
        System.out.println("\n--- Cat Object ---");
        Cat cat = new Cat("Whiskers", 2, "Orange", true);
        cat.displayInfo();
        cat.eat();
        cat.meow();
        cat.purr();
        
        System.out.println("\n--- Bird Object ---");
        Bird bird = new Bird("Tweety", 1, "Yellow", true);
        bird.displayInfo();
        bird.sing();
        
        System.out.println("\n=== MULTILEVEL INHERITANCE ===\n");
        
        // Vehicle -> Car -> SportsCar
        Vehicle vehicle = new Vehicle("Generic", "Model", 2020);
        vehicle.displayInfo();
        
        System.out.println("\n--- Car Object ---");
        Car car = new Car("Toyota", "Camry", 2022, 4);
        car.displayInfo();
        car.start();
        car.drive();
        
        System.out.println("\n--- SportsCar Object ---");
        SportsCar sportsCar = new SportsCar("Ferrari", "488", 2023, 2, 200);
        sportsCar.displayInfo();
        sportsCar.start();
        sportsCar.race();
        
        System.out.println("\n=== USING ARRAYS WITH INHERITANCE ===\n");
        
        // Create array of Animals (polymorphism)
        Animal[] animals = new Animal[4];
        animals[0] = new Dog("Rex", 5, "Black", "Labrador");
        animals[1] = new Cat("Mittens", 3, "White", false);
        animals[2] = new Bird("Polly", 2, "Green", true);
        animals[3] = new Dog("Max", 2, "Brown", "Beagle");
        
        System.out.println("All Animals:");
        for (Animal a : animals) {
            System.out.println("\n--- " + a.getName() + " ---");
            a.displayInfo();
            a.eat();
            a.sleep();
            
            // instanceof - check what type of animal
            if (a instanceof Dog) {
                Dog d = (Dog) a;  // Cast to Dog
                d.bark();
            } else if (a instanceof Cat) {
                Cat c = (Cat) a;
                c.meow();
            } else if (a instanceof Bird) {
                Bird b = (Bird) a;
                b.sing();
            }
        }
        
        System.out.println("\n=== KEY BENEFITS OF INHERITANCE ===\n");
        
        System.out.println("1. Code Reusability: Child classes inherit parent's code");
        System.out.println("2. Avoid Duplication: Common code in one place");
        System.out.println("3. Easy Maintenance: Change in parent affects all children");
        System.out.println("4. Polymorphism: Treat different objects as same type");
    }
}

/*
 * KEY CONCEPTS FOR BEGINNERS:
 * 
 * 1. INHERITANCE BASICS:
 *    - extends keyword creates child from parent
 *    - Child inherits all public and protected members
 *    - Private members are NOT inherited
 *    - Constructors are NOT inherited (but called using super())
 * 
 * 2. SUPER KEYWORD:
 *    - super() - calls parent constructor
 *    - super.methodName() - calls parent method
 *    - Must be first statement in constructor
 * 
 * 3. OVERRIDING METHODS:
 *    - Same method signature in child class
 *    - @Override annotation (recommended)
 *    - Can call parent version with super.methodName()
 * 
 * 4. TYPES OF INHERITANCE:
 *    - Single: One parent, one child
 *    - Multilevel: Grandparent -> Parent -> Child
 *    - Hierarchical: One parent, multiple children
 *    - Java does NOT support multiple inheritance (one class extends only one)
 * 
 * 5. instanceof OPERATOR:
 *    - Checks if object is of certain type
 *    - Used for safe type casting
 * 
 * 6. POLYMORHISM:
 *    - Parent reference can hold child objects
 *    - Animal[] can hold Dog, Cat, Bird objects
 *    - Method call depends on actual object type
 */
