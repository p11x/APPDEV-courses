// EncapsulationDemo - Demonstrates Encapsulation and Abstraction
// Encapsulation: hiding data and methods | Abstraction: hiding complexity

public class EncapsulationDemo {
    
    // ===== ENCAPSULATION =====
    // Private fields - hidden from outside
    private String name;
    private int age;
    private double salary;
    
    // Public getters and setters - controlled access
    public String getName() { return name; }
    public void setName(String name) { this.name = name; }
    
    public int getAge() { return age; }
    public void setAge(int age) {
        if (age > 0 && age < 150) {
            this.age = age;
        }
    }
    
    public double getSalary() { return salary; }
    public void setSalary(double salary) {
        if (salary > 0) {
            this.salary = salary;
        }
    }
    
    // ===== ABSTRACTION =====
    // Abstract concept - user doesn't know internal details
    public void displayInfo() {
        System.out.println("Name: " + name + ", Age: " + age);
    }
    
    public static void main(String[] args) {
        System.out.println("=== ENCAPSULATION & ABSTRACTION ===\n");
        
        EncapsulationDemo emp = new EncapsulationDemo();
        
        // Set values through setters (controlled access)
        emp.setName("John Doe");
        emp.setAge(30);
        emp.setSalary(50000);
        
        // Get values through getters
        System.out.println("Name: " + emp.getName());
        System.out.println("Age: " + emp.getAge());
        System.out.println("Salary: $" + emp.getSalary());
        
        // Validation in setter
        System.out.println("\n--- Validation ---");
        emp.setAge(-5);  // Won't set due to validation
        System.out.println("After invalid age: " + emp.getAge());
        
        System.out.println("\n--- Benefits ---");
        System.out.println("1. Data hiding - private fields");
        System.out.println("2. Validation - in setters");
        System.out.println("3. Flexibility - can change internal code");
        System.out.println("4. Reusability - reusable components");
        
        System.out.println("\n--- Angular Note ---");
        System.out.println("Java classes map to TypeScript interfaces/classes");
        System.out.println("Encapsulation in Java = private fields + getters");
    }
}
