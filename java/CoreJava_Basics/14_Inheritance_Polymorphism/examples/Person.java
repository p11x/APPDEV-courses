// Person - Base class demonstrating inheritance
// This is the parent/superclass that other classes will inherit from

public class Person {
    // Protected fields - accessible by subclasses
    protected String name;
    protected int age;
    protected String address;
    protected String phoneNumber;
    
    // Constructor
    public Person(String name, int age, String address, String phoneNumber) {
        this.name = name;
        this.age = age;
        this.address = address;
        this.phoneNumber = phoneNumber;
    }
    
    // Default constructor
    public Person() {
        this.name = "Unknown";
        this.age = 0;
        this.address = "";
        this.phoneNumber = "";
    }
    
    // Getters
    public String getName() { return name; }
    public int getAge() { return age; }
    public String getAddress() { return address; }
    public String getPhoneNumber() { return phoneNumber; }
    
    // Setters
    public void setName(String name) { this.name = name; }
    public void setAge(int age) { this.age = age; }
    public void setAddress(String address) { this.address = address; }
    public void setPhoneNumber(String phoneNumber) { this.phoneNumber = phoneNumber; }
    
    // Common method - can be overridden
    public void displayInfo() {
        System.out.println("=== PERSON INFORMATION ===");
        System.out.println("Name: " + name);
        System.out.println("Age: " + age);
        System.out.println("Address: " + address);
        System.out.println("Phone: " + phoneNumber);
    }
    
    // Common behavior
    public void introduce() {
        System.out.println("Hello, my name is " + name + " and I am " + age + " years old.");
    }
    
    // Main for testing
    public static void main(String[] args) {
        System.out.println("=== PERSON CLASS DEMO ===\n");
        
        Person person = new Person("John Doe", 30, "123 Main St", "555-1234");
        person.displayInfo();
        person.introduce();
    }
}
