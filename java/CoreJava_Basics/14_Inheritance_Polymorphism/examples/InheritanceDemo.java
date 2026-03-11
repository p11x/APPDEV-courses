// EmployeeInheritanceDemo - Demonstrates inheritance and polymorphism
// Contains both parent and child classes in one file for easy compilation

// ===== PARENT CLASS =====
class Person {
    protected String name;
    protected int age;
    protected String address;
    protected String phoneNumber;
    
    public Person(String name, int age, String address, String phoneNumber) {
        this.name = name;
        this.age = age;
        this.address = address;
        this.phoneNumber = phoneNumber;
    }
    
    public Person() {
        this.name = "Unknown";
        this.age = 0;
        this.address = "";
        this.phoneNumber = "";
    }
    
    public String getName() { return name; }
    public int getAge() { return age; }
    public String getAddress() { return address; }
    public String getPhoneNumber() { return phoneNumber; }
    
    public void setName(String name) { this.name = name; }
    public void setAge(int age) { this.age = age; }
    public void setAddress(String address) { this.address = address; }
    public void setPhoneNumber(String phoneNumber) { this.phoneNumber = phoneNumber; }
    
    public void displayInfo() {
        System.out.println("Name: " + name + ", Age: " + age + 
                         ", Address: " + address + ", Phone: " + phoneNumber);
    }
    
    public void introduce() {
        System.out.println("Hello, I'm " + name + " and I'm " + age + " years old.");
    }
}

// ===== CHILD CLASS 1: Employee =====
class Employee extends Person {
    private int employeeId;
    private String department;
    private String jobTitle;
    private double salary;
    
    public Employee(String name, int age, String address, String phoneNumber,
                   int employeeId, String department, String jobTitle, double salary) {
        super(name, age, address, phoneNumber);
        this.employeeId = employeeId;
        this.department = department;
        this.jobTitle = jobTitle;
        this.salary = salary;
    }
    
    public int getEmployeeId() { return employeeId; }
    public String getDepartment() { return department; }
    public String getJobTitle() { return jobTitle; }
    public double getSalary() { return salary; }
    
    public double calculateAnnualSalary() {
        return salary * 12;
    }
    
    // Method overriding - demonstrates polymorphism
    @Override
    public void displayInfo() {
        super.displayInfo();
        System.out.println("  -> Employee ID: " + employeeId + ", Dept: " + 
                          department + ", Title: " + jobTitle + ", Salary: $" + salary);
    }
    
    @Override
    public void introduce() {
        System.out.println("Hi, I'm " + name + ", a " + jobTitle + " in " + department);
    }
}

// ===== CHILD CLASS 2: Student =====
class Student extends Person {
    private int studentId;
    private String major;
    private double gpa;
    
    public Student(String name, int age, String address, String phoneNumber,
                   int studentId, String major, double gpa) {
        super(name, age, address, phoneNumber);
        this.studentId = studentId;
        this.major = major;
        this.gpa = gpa;
    }
    
    public int getStudentId() { return studentId; }
    public String getMajor() { return major; }
    public double getGpa() { return gpa; }
    
    public boolean isHonorStudent() {
        return gpa >= 3.5;
    }
    
    // Method overriding
    @Override
    public void displayInfo() {
        super.displayInfo();
        System.out.println("  -> Student ID: " + studentId + ", Major: " + 
                          major + ", GPA: " + gpa);
    }
    
    @Override
    public void introduce() {
        System.out.println("Hey, I'm " + name + ", studying " + major + " at university");
    }
}

// ===== MAIN CLASS =====
public class InheritanceDemo {
    public static void main(String[] args) {
        System.out.println("=== INHERITANCE & POLYMORPHISM DEMO ===\n");
        
        // Create Employee object
        Employee emp = new Employee("Alice Smith", 28, "456 Oak Ave", "555-5678",
                                    1001, "Engineering", "Software Engineer", 7500.00);
        System.out.println("--- Employee ---");
        emp.displayInfo();
        emp.introduce();
        System.out.println("Annual Salary: $" + emp.calculateAnnualSalary());
        
        // Create Student object
        Student student = new Student("Bob Johnson", 20, "789 Pine St", "555-9999",
                                       2001, "Computer Science", 3.8);
        System.out.println("\n--- Student ---");
        student.displayInfo();
        student.introduce();
        System.out.println("Honor Student: " + student.isHonorStudent());
        
        // Demonstrate polymorphism - same method, different behavior
        System.out.println("\n=== POLYMORPHISM DEMO ===");
        Person[] people = {emp, student};
        for (Person p : people) {
            p.introduce();  // Different output based on actual type
        }
    }
}
