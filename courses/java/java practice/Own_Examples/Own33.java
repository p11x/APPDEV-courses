import java.util.Scanner;

// Number to Roman Numeral Converter
public class Own33 {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        
        System.out.println("=== Number to Roman Numeral Converter ===");
        System.out.println();
        
        // Input a number
        System.out.print("Enter a number (1-3999): ");
        int number = scanner.nextInt();
        
        // Validate input
        while (number < 1 || number > 3999) {
            System.out.print("Invalid! Enter a number between 1 and 3999: ");
            number = scanner.nextInt();
        }
        
        // Arrays for conversion
        int[] values = {1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1};
        String[] symbols = {"M", "CM", "D", "CD", "C", "XC", "L", "XL", "X", "IX", "V", "IV", "I"};
        
        // Convert to Roman numeral
        String roman = "";
        int temp = number;
        
        for (int i = 0; i < values.length; i++) {
            while (temp >= values[i]) {
                roman = roman + symbols[i];
                temp = temp - values[i];
            }
        }
        
        // Display result
        System.out.println();
        System.out.println("Number: " + number);
        System.out.println("Roman Numeral: " + roman);
        
        scanner.close();
    }
}
