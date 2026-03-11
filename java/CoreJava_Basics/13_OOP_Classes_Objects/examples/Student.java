// Student - Student class demonstrating OOP
// Used for student management systems in Java backend
// Maps to Angular Student interface

public class Student {
    // Fields - private for encapsulation
    private int studentId;
    private String name;
    private String email;
    private int age;
    private String major;
    private double gpa;
    
    // Default constructor
    public Student() {
        this.studentId = 0;
        this.name = "Unknown";
        this.email = "";
        this.age = 0;
        this.major = "Undeclared";
        this.gpa = 0.0;
    }
    
    // Parameterized constructor
    public Student(int studentId, String name, String email, 
                   int age, String major, double gpa) {
        this.studentId = studentId;
        this.name = name;
        this.email = email;
        this.age = age;
        this.major = major;
        this.gpa = gpa;
    }
    
    // Getters
    public int getStudentId() { return studentId; }
    public String getName() { return name; }
    public String getEmail() { return email; }
    public int getAge() { return age; }
    public String getMajor() { return major; }
    public double getGpa() { return gpa; }
    
    // Setters
    public void setName(String name) { this.name = name; }
    public void setEmail(String email) { this.email = email; }
    public void setMajor(String major) { this.major = major; }
    
    public void setGpa(double gpa) {
        if (gpa >= 0.0 && gpa <= 4.0) {
            this.gpa = gpa;
        }
    }
    
    // Business methods
    public String getGradeLevel() {
        if (gpa >= 3.5) return "Dean's List";
        if (gpa >= 3.0) return "Good Standing";
        if (gpa >= 2.0) return "Satisfactory";
        return "Probation";
    }
    
    public boolean isHonorStudent() {
        return gpa >= 3.5;
    }
    
    public void displayInfo() {
        System.out.println("=== STUDENT INFORMATION ===");
        System.out.println("ID: " + studentId);
        System.out.println("Name: " + name);
        System.out.println("Email: " + email);
        System.out.println("Age: " + age);
        System.out.println("Major: " + major);
        System.out.println("GPA: " + gpa);
        System.out.println("Status: " + getGradeLevel());
    }
    
    public static void main(String[] args) {
        System.out.println("=== STUDENT CLASS DEMO ===\n");
        
        Student student1 = new Student(1001, "Alice Johnson", 
            "alice@school.edu", 20, "Computer Science", 3.8);
        student1.displayInfo();
        
        System.out.println("\n--- Second Student ---");
        Student student2 = new Student(1002, "Bob Smith", 
            "bob@school.edu", 19, "Mathematics", 2.5);
        student2.displayInfo();
        System.out.println("Honor Student: " + student2.isHonorStudent());
    }
}
