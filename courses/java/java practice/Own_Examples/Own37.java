import java.util.Scanner;

// Prime Factorization
public class Own37 {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        
        System.out.println("=== Prime Factorization ===");
        System.out.println();
        
        // Input a number
        System.out.print("Enter a number: ");
        int number = scanner.nextInt();
        
        // Validate input
        while (number <= 1) {
            System.out.print("Invalid! Enter a number greater than 1: ");
            number = scanner.nextInt();
        }
        
        int originalNumber = number;
        
        // Find prime factors
        System.out.println();
        System.out.println("Prime factors of " + originalNumber + ":");
        
        String factors = "";
        int temp = number;
        
        // Handle factor of 2
        int exponent2 = 0;
        while (temp % 2 == 0) {
            exponent2++;
            temp = temp / 2;
        }
        if (exponent2 > 0) {
            factors = factors + "2";
            if (exponent2 > 1) {
                factors = factors + "^" + exponent2;
            }
        }
        
        // Handle odd factors
        for (int i = 3; i * i <= temp; i = i + 2) {
            int exponent = 0;
            while (temp % i == 0) {
                exponent++;
                temp = temp / i;
            }
            if (exponent > 0) {
                if (!factors.isEmpty()) {
                    factors = factors + " x ";
                }
                factors = factors + i;
                if (exponent > 1) {
                    factors = factors + "^" + exponent;
                }
            }
        }
        
        // If remaining temp is greater than 1, it's also a prime factor
        if (temp > 1) {
            if (!factors.isEmpty()) {
                factors = factors + " x ";
            }
            factors = factors + temp + "^1";
        }
        
        System.out.println(factors);
        
        // Also display in format: 2 x 2 x 2 x 3 x 3 x 5
        System.out.println();
        System.out.println("Expanded form:");
        temp = number;
        
        String expanded = "";
        // Factor of 2
        while (temp % 2 == 0) {
            if (!expanded.isEmpty()) {
                expanded = expanded + " x ";
            }
            expanded = expanded + "2";
            temp = temp / 2;
        }
        
        // Odd factors
        for (int i = 3; i * i <= temp; i = i + 2) {
            while (temp % i == 0) {
                if (!expanded.isEmpty()) {
                    expanded = expanded + " x ";
                }
                expanded = expanded + i;
                temp = temp / i;
            }
        }
        
        if (temp > 1) {
            if (!expanded.isEmpty()) {
                expanded = expanded + " x ";
            }
            expanded = expanded + temp;
        }
        
        System.out.println(originalNumber + " = " + expanded);
        
        scanner.close();
    }
}
