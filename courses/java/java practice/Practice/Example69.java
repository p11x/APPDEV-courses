/*
 * SUB TOPIC: Constructor and Constructor Overloading
 * 
 * DEFINITION:
 * Constructors are special methods used to initialize objects. They have the same name as the class and no return type.
 * Constructor overloading allows multiple constructors with different parameters.
 */

public class Example69 {
    String name;
    int age;
    
    Example69() {
        this.name = "Unknown";
        this.age = 0;
    }
    
    Example69(String name) {
        this.name = name;
        this.age = 0;
    }
    
    Example69(String name, int age) {
        this.name = name;
        this.age = age;
    }
    
    public static void main(String[] args) {
        Example69 p1 = new Example69();
        Example69 p2 = new Example69("John");
        Example69 p3 = new Example69("Jane", 25);
        
        System.out.println("p1: " + p1.name + ", " + p1.age);
        System.out.println("p2: " + p2.name + ", " + p2.age);
        System.out.println("p3: " + p3.name + ", " + p3.age);
    }
}
