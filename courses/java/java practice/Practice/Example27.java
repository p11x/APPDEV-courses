/*
 * SUB TOPIC: Inheritance
 * 
 * DEFINITION:
 * Inheritance is a mechanism where a new class (child/subclass) inherits properties and behaviors 
 * from an existing class (parent/superclass). It promotes code reusability and creates a class hierarchy.
 * 
 * FUNCTIONALITIES:
 * 1. Single inheritance
 * 2. Multilevel inheritance
 * 3. Method overriding
 * 4. Using super keyword
 * 5. Polymorphism with inheritance
 */

// Parent class - Animal
class Animal {
    protected String name;
    protected int age;
    protected String color;
    
    public Animal(String name, int age, String color) {
        this.name = name;
        this.age = age;
        this.color = color;
    }
    
    public void eat() {
        System.out.println(name + " is eating...");
    }
    
    public void sleep() {
        System.out.println(name + " is sleeping...");
    }
    
    public void displayInfo() {
        System.out.println("Name: " + name);
        System.out.println("Age: " + age);
        System.out.println("Color: " + color);
    }
}

// Dog inherits from Animal
class Dog extends Animal {
    private String breed;
    
    public Dog(String name, int age, String color, String breed) {
        super(name, age, color);
        this.breed = breed;
    }
    
    public void bark() {
        System.out.println(name + " says: Woof! Woof!");
    }
    
    @Override
    public void displayInfo() {
        super.displayInfo();
        System.out.println("Breed: " + breed);
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
    
    @Override
    public void displayInfo() {
        super.displayInfo();
        System.out.println("Type: " + (isIndoor ? "Indoor" : "Outdoor"));
    }
}

// Vehicle class for multilevel inheritance
class Vehicle {
    protected String brand;
    protected String model;
    
    public Vehicle(String brand, String model) {
        this.brand = brand;
        this.model = model;
    }
    
    public void start() {
        System.out.println(brand + " " + model + " is starting...");
    }
}

// Car inherits from Vehicle
class Car extends Vehicle {
    private int numDoors;
    
    public Car(String brand, String model, int doors) {
        super(brand, model);
        this.numDoors = doors;
    }
    
    public void drive() {
        System.out.println(brand + " " + model + " is driving...");
    }
    
    public void displayInfo() {
        System.out.println("Brand: " + brand);
        System.out.println("Model: " + model);
        System.out.println("Doors: " + numDoors);
    }
}

public class Example27 {
    public static void main(String[] args) {
        
        // Topic Explanation with Code: Inheritance
        System.out.println("=== Single Inheritance ===");
        
        Dog dog = new Dog("Buddy", 3, "Golden", "Golden Retriever");
        dog.displayInfo();
        dog.eat(); // Inherited method
        dog.bark(); // Dog's own method
        
        // Real-time Example 1: Cat inheritance
        System.out.println("\n=== Cat ===");
        
        Cat cat = new Cat("Whiskers", 2, "Orange", true);
        cat.displayInfo();
        cat.meow();
        
        // Real-time Example 2: Multilevel inheritance
        System.out.println("\n=== Multilevel Inheritance ===");
        
        Car car = new Car("Toyota", "Camry", 4);
        car.displayInfo();
        car.start();
        car.drive();
        
        // Real-time Example 3: Array of Animals
        System.out.println("\n=== Array of Animals ===");
        
        Animal[] animals = new Animal[3];
        animals[0] = new Dog("Rex", 5, "Black", "Labrador");
        animals[1] = new Cat("Mittens", 3, "White", false);
        animals[2] = new Dog("Max", 2, "Brown", "Beagle");
        
        for (Animal a : animals) {
            a.displayInfo();
            a.eat();
        }
        
        // Real-time Example 4: instanceof operator
        System.out.println("\n=== instanceof ===");
        
        Animal animal = new Dog("Rex", 5, "Black", "Labrador");
        System.out.println("Is Dog? " + (animal instanceof Dog));
        System.out.println("Is Animal? " + (animal instanceof Animal));
        
        // Real-time Example 5: Method overriding
        System.out.println("\n=== Method Overriding ===");
        
        Animal a1 = new Animal("Generic", 5, "Brown");
        Animal a2 = new Dog("Buddy", 3, "Golden", "Retriever");
        
        a1.eat(); // Calls Animal's eat
        a2.eat(); // Calls Dog's eat (overridden)
        
        // Real-time Example 6: Using super keyword
        System.out.println("\n=== Using super ===");
        
        Dog dog2 = new Dog("Charlie", 4, "Black", "Poodle");
        dog2.displayInfo(); // Calls Dog's displayInfo which calls super.displayInfo()
    }
}
