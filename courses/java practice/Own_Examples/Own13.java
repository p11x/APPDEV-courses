import java.util.Scanner;

// BMI Calculator
public class Own13 {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        
        System.out.println("=== BMI Calculator ===");
        System.out.println();
        
        // Input weight and height
        System.out.print("Enter weight (kg): ");
        double weight = scanner.nextDouble();
        
        System.out.print("Enter height (cm): ");
        double heightCm = scanner.nextDouble();
        
        // Validate inputs
        while (weight <= 0 || heightCm <= 0) {
            System.out.println("Invalid input! Values must be positive.");
            System.out.print("Enter weight (kg): ");
            weight = scanner.nextDouble();
            System.out.print("Enter height (cm): ");
            heightCm = scanner.nextDouble();
        }
        
        // Convert height from cm to meters
        double heightM = heightCm / 100;
        
        // Calculate BMI = weight / (height in meters)^2
        double bmi = weight / (heightM * heightM);
        
        // Determine category
        String category;
        String advice;
        
        if (bmi < 18.5) {
            category = "Underweight";
            advice = "You should eat more nutritious food and consult a doctor.";
        } else if (bmi < 25) {
            category = "Normal";
            advice = "Great! Maintain a healthy lifestyle with balanced diet and exercise.";
        } else if (bmi < 30) {
            category = "Overweight";
            advice = "You should exercise regularly and watch your diet.";
        } else {
            category = "Obese";
            advice = "Please consult a doctor and start a weight management program.";
        }
        
        // Display results
        System.out.println();
        System.out.println("=== BMI Results ===");
        System.out.printf("Your BMI: %.2f%n", bmi);
        System.out.println("Category: " + category);
        System.out.println("Advice: " + advice);
        
        scanner.close();
    }
}
