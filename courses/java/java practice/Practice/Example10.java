/*
 * SUB TOPIC: Lucky Numbers and Special Number Series
 * 
 * DEFINITION:
 * Lucky numbers are numbers where the sum of digits is repeatedly calculated until a single digit
 * is obtained, and if that digit is 9, it's considered a lucky number. This concept is used in
 * various numerology and Astrology applications.
 * 
 * FUNCTIONALITIES:
 * 1. Find lucky numbers in a range
 * 2. Calculate digital root
 * 3. Find special numbers in series
 * 4. Number pattern recognition
 */

public class Example10 {
    
    // Method to calculate sum of digits
    public static int sumOfDigits(int n) {
        int sum = 0; // Initialize sum to 0
        while (n > 0) { // Loop until number becomes 0
            sum += n % 10; // Add last digit to sum
            n /= 10; // Remove last digit
        }
        return sum; // Return the sum
    }
    
    // Method to calculate digital root
    public static int digitalRoot(int n) {
        int sum = sumOfDigits(n); // Get sum of digits
        
        while (sum > 9) { // Continue until single digit
            sum = sumOfDigits(sum); // Sum again
        }
        
        return sum; // Return digital root
    }
    
    public static void main(String[] args) {
        
        // Topic Explanation with Code: Lucky Numbers
        System.out.println("=== Lucky Numbers ===");
        System.out.println("Lucky numbers from 4200 to 4500 are:");
        
        for (int n = 4200; n <= 4500; n++) { // Loop through range
            int sum = sumOfDigits(n); // Get sum of digits
            
            while (sum > 9) { // Continue until single digit
                sum = sumOfDigits(sum); // Calculate again
            }
            
            if (sum == 9) { // Check if digital root is 9
                System.out.print(n + "\t"); // Print lucky number
            }
        }
        
        // Real-time Example 1: Find all lucky numbers in range
        System.out.println("\n\n=== Lucky Numbers from 1 to 100 ===");
        
        for (int n = 1; n <= 100; n++) { // Loop from 1 to 100
            int sum = sumOfDigits(n); // Calculate sum
            
            while (sum > 9) { // Continue until single digit
                sum = sumOfDigits(sum);
            }
            
            if (sum == 9) { // Check if lucky
                System.out.print(n + " ");
            }
        }
        
        // Real-time Example 2: Digital Root Calculator
        System.out.println("\n\n=== Digital Root Calculator ===");
        
        int[] numbers = {123, 456, 789, 999, 1000}; // Sample numbers
        
        for (int num : numbers) { // Loop through array
            int dr = digitalRoot(num); // Get digital root
            System.out.println("Number: " + num + " -> Digital Root: " + dr);
        }
        
        // Real-time Example 3: Find Armstrong numbers in range
        System.out.println("\n=== Armstrong Numbers from 1 to 500 ===");
        
        for (int n = 1; n <= 500; n++) { // Loop through range
            int original = n; // Store original
            int sum = 0; // Initialize sum
            
            int temp = n; // Temporary variable
            int digits = 0;
            while (temp > 0) { // Count digits
                digits++;
                temp /= 10;
            }
            
            temp = n; // Reset temp
            while (temp > 0) { // Calculate sum of cubes
                int digit = temp % 10;
                int cube = 1;
                for (int i = 0; i < digits; i++) { // Power calculation
                    cube *= digit;
                }
                sum += cube;
                temp /= 10;
            }
            
            if (sum == original) { // Check Armstrong
                System.out.print(original + " ");
            }
        }
        
        // Real-time Example 4: Perfect numbers in range
        System.out.println("\n\n=== Perfect Numbers from 1 to 10000 ===");
        
        for (int n = 1; n <= 10000; n++) { // Loop through range
            int sum = 0; // Initialize sum
            
            for (int i = 1; i < n; i++) { // Check divisors
                if (n % i == 0) { // If divisor
                    sum += i; // Add to sum
                }
            }
            
            if (sum == n) { // Check perfect
                System.out.print(n + " ");
            }
        }
        
        // Real-time Example 5: Strong numbers in range
        System.out.println("\n\n=== Strong Numbers from 1 to 500 ===");
        
        // Method to find factorial
        for (int n = 1; n <= 500; n++) { // Loop through range
            int original = n; // Store original
            int sum = 0; // Initialize sum
            
            int temp = n; // Temporary
            while (temp > 0) { // Loop through digits
                int digit = temp % 10; // Get digit
                
                // Calculate factorial
                int fact = 1;
                for (int i = 1; i <= digit; i++) {
                    fact *= i;
                }
                
                sum += fact; // Add factorial
                temp /= 10; // Remove digit
            }
            
            if (sum == original) { // Check strong
                System.out.print(original + " ");
            }
        }
        
        // Real-time Example 6: Prime numbers in range
        System.out.println("\n\n=== Prime Numbers from 1 to 100 ===");
        
        for (int n = 2; n <= 100; n++) { // Start from 2
            boolean isPrime = true; // Assume prime
            
            for (int i = 2; i <= n / 2; i++) { // Check divisibility
                if (n % i == 0) { // If divisible
                    isPrime = false; // Not prime
                    break;
                }
            }
            
            if (isPrime) { // If prime
                System.out.print(n + " ");
            }
        }
    }
}
