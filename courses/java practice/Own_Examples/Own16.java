import java.util.Scanner;

// Number System Converter
public class Own16 {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        
        System.out.println("=== Number System Converter ===");
        System.out.println();
        
        // Input decimal number
        System.out.print("Enter a decimal number: ");
        int decimal = scanner.nextInt();
        
        // Convert to Binary (manual conversion using loops)
        String binary = "";
        int temp = decimal;
        
        if (temp == 0) {
            binary = "0";
        } else {
            while (temp > 0) {
                binary = (temp % 2) + binary;
                temp = temp / 2;
            }
        }
        
        // Convert to Octal (manual conversion using loops)
        String octal = "";
        temp = decimal;
        
        if (temp == 0) {
            octal = "0";
        } else {
            while (temp > 0) {
                octal = (temp % 8) + octal;
                temp = temp / 8;
            }
        }
        
        // Convert to Hexadecimal (manual conversion using loops)
        String hexadecimal = "";
        temp = decimal;
        
        if (temp == 0) {
            hexadecimal = "0";
        } else {
            while (temp > 0) {
                int remainder = temp % 16;
                if (remainder < 10) {
                    hexadecimal = remainder + hexadecimal;
                } else {
                    char hexChar = (char) ('A' + (remainder - 10));
                    hexadecimal = hexChar + hexadecimal;
                }
                temp = temp / 16;
            }
        }
        
        // Display results
        System.out.println();
        System.out.println("=== Conversion Results ===");
        System.out.println("Decimal Number: " + decimal);
        System.out.println("Binary: " + binary);
        System.out.println("Octal: " + octal);
        System.out.println("Hexadecimal: " + hexadecimal);
        
        scanner.close();
    }
}
