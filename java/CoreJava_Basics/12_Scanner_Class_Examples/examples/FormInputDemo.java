// FormInputDemo - Demonstrates Scanner for Angular Form Data Processing
// Use case: Processing user input from forms

import java.util.*;

public class FormInputDemo {
    private Scanner scanner;
    
    public FormInputDemo() {
        this.scanner = new Scanner(System.in);
    }
    
    // Simulate reading form data
    public Map<String, Object> readUserForm() {
        Map<String, Object> formData = new HashMap<>();
        
        System.out.println("=== ANGULAR FORM INPUT ===");
        
        // Read text input
        System.out.print("Enter name: ");
        String name = scanner.nextLine();
        formData.put("name", name);
        
        // Read email
        System.out.print("Enter email: ");
        String email = scanner.nextLine();
        formData.put("email", email);
        
        // Read age
        System.out.print("Enter age: ");
        int age = scanner.nextInt();
        formData.put("age", age);
        
        // Read subscription preference
        System.out.print("Subscribe to newsletter (true/false): ");
        boolean subscribed = scanner.nextBoolean();
        formData.put("subscribed", subscribed);
        
        return formData;
    }
    
    // Validate form data
    public boolean validateForm(Map<String, Object> formData) {
        String name = (String) formData.get("name");
        String email = (String) formData.get("email");
        int age = (int) formData.get("age");
        
        if (name == null || name.trim().isEmpty()) {
            System.out.println("Error: Name is required");
            return false;
        }
        
        if (email == null || !email.contains("@")) {
            System.out.println("Error: Valid email required");
            return false;
        }
        
        if (age < 0 || age > 150) {
            System.out.println("Error: Invalid age");
            return false;
        }
        
        return true;
    }
    
    // Convert to JSON for Angular
    public String toJSON(Map<String, Object> formData) {
        StringBuilder json = new StringBuilder();
        json.append("{");
        json.append("\"name\":\"").append(formData.get("name")).append("\",");
        json.append("\"email\":\"").append(formData.get("email")).append("\",");
        json.append("\"age\":").append(formData.get("age")).append(",");
        json.append("\"subscribed\":").append(formData.get("subscribed"));
        json.append("}");
        return json.toString();
    }
    
    public void close() {
        scanner.close();
    }
    
    public static void main(String[] args) {
        FormInputDemo demo = new FormInputDemo();
        
        System.out.println("\n=== FORM INPUT DEMO ===\n");
        
        // Simulated form data (in real use, uncomment readUserForm)
        Map<String, Object> formData = new HashMap<>();
        formData.put("name", "John Doe");
        formData.put("email", "john@example.com");
        formData.put("age", 25);
        formData.put("subscribed", true);
        
        // Validate
        if (demo.validateForm(formData)) {
            System.out.println("Form is valid!");
            
            // Convert to JSON
            String json = demo.toJSON(formData);
            System.out.println("\nJSON for Angular: " + json);
        }
        
        demo.close();
    }
}
