/*
 * SUB TOPIC: Inheritance in Java
 * 
 * DEFINITION:
 * Inheritance allows a class to inherit properties and methods from another class. The subclass extends the superclass.
 */

class Animal {
    String name;
    void eat() { System.out.println("Eating..."); }
}

class Dog extends Animal {
    void bark() { System.out.println("Barking..."); }
}

public class Example70 {
    public static void main(String[] args) {
        Dog d = new Dog();
        d.name = "Buddy";
        d.eat();
        d.bark();
    }
}
