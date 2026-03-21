import java.util.Scanner;

// Temperature Converter
public class Own7 {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        
        System.out.println("=== Temperature Converter ===");
        System.out.println();
        
        // Get temperature value
        System.out.print("Enter temperature value: ");
        double temperature = scanner.nextDouble();
        
        // Get source unit
        System.out.println("Enter source unit:");
        System.out.println("C - Celsius");
        System.out.println("F - Fahrenheit");
        System.out.println("K - Kelvin");
        System.out.print("Enter unit (C/F/K): ");
        char unit = scanner.next().charAt(0);
        
        // Convert based on source unit
        // Formulas: F = C*9/5 + 32, K = C + 273.15
        double celsius, fahrenheit, kelvin;
        
        switch (unit) {
            case 'C':
            case 'c':
                celsius = temperature;
                fahrenheit = celsius * 9 / 5 + 32;
                kelvin = celsius + 273.15;
                break;
            case 'F':
            case 'f':
                fahrenheit = temperature;
                celsius = (fahrenheit - 32) * 5 / 9;
                kelvin = celsius + 273.15;
                break;
            case 'K':
            case 'k':
                kelvin = temperature;
                celsius = kelvin - 273.15;
                fahrenheit = celsius * 9 / 5 + 32;
                break;
            default:
                System.out.println("Invalid unit entered!");
                scanner.close();
                return;
        }
        
        // Display results
        System.out.println();
        System.out.println("=== Conversion Results ===");
        System.out.printf("Celsius: %.2f °C%n", celsius);
        System.out.printf("Fahrenheit: %.2f °F%n", fahrenheit);
        System.out.printf("Kelvin: %.2f K%n", kelvin);
        
        scanner.close();
    }
}
