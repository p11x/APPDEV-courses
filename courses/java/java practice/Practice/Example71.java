/*
 * SUB TOPIC: Polymorphism in Java
 * 
 * DEFINITION:
 * Polymorphism allows objects to take many forms. It includes method overloading (compile-time) and method overriding (runtime).
 */

class Vehicle {
    void run() { System.out.println("Running"); }
}

class Car extends Vehicle {
    @Override
    void run() { System.out.println("Car running"); }
}

class Bike extends Vehicle {
    @Override
    void run() { System.out.println("Bike running"); }
}

public class Example71 {
    public static void main(String[] args) {
        Vehicle v = new Car();
        v.run();
        
        Vehicle v2 = new Bike();
        v2.run();
    }
}
