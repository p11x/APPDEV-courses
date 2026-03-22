/*
 * SUB TOPIC: Number Operations (Palindrome, Armstrong, Prime, Perfect)
 * 
 * DEFINITION:
 * Number operations are mathematical computations performed on integers to check various properties
 * like whether a number is palindrome, Armstrong, prime, perfect, etc. These are fundamental
 * concepts used in competitive programming and mathematical applications.
 * 
 * FUNCTIONALITIES:
 * 1. Palindrome - number that reads same forwards and backwards
 * 2. Armstrong - sum of cubes of digits equals the number
 * 3. Prime - divisible only by 1 and itself
 * 4. Perfect - sum of divisors equals the number
 * 5. Reverse - digits in reverse order
 * 6. Sum of digits - adding all digits
 */

import java.util.Scanner;

public class Example9 {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        
        // Topic Explanation with Code: Check Palindrome
        System.out.println("=== Check Palindrome Number ===");
        System.out.println("Enter the number: ");
        int number = scanner.nextInt(); // Read input number
        
        // Check palindrome
        int temp = number; // Store original number
        int rev = 0; // Initialize reverse
        
        while (number > 0) { // Loop until number becomes 0
            int digit = number % 10; // Get last digit
            rev = rev * 10 + digit; // Build reverse
            number /= 10; // Remove last digit
        }
        
        if (temp == rev) { // Compare original with reverse
            System.out.println("The number is a palindrome.");
        } else {
            System.out.println("The number is not a palindrome.");
        }
        
        // Real-time Example 1: Check Armstrong number
        System.out.println("\n=== Check Armstrong Number ===");
        System.out.print("Enter a number: ");
        int armstrongNum = scanner.nextInt(); // Read number
        int original = armstrongNum; // Store original
        int armstrongSum = 0; // Initialize sum
        
        while (armstrongNum > 0) { // Loop through digits
            int digit = armstrongNum % 10; // Get last digit
            armstrongSum += digit * digit * digit; // Add cube of digit
            armstrongNum /= 10; // Remove last digit
        }
        
        if (original == armstrongSum) { // Compare with original
            System.out.println("The number is an Armstrong number.");
        } else {
            System.out.println("The number is not an Armstrong number.");
        }
        
        // Real-time Example 2: Check Prime number
        System.out.println("\n=== Check Prime Number ===");
        System.out.print("Enter a number: ");
        int primeNum = scanner.nextInt(); // Read number
        boolean isPrime = true; // Assume prime
        
        if (primeNum <= 1) {
            isPrime = false; // 0 and 1 are not prime
        } else {
            for (int i = 2; i <= primeNum / 2; i++) { // Check divisibility
                if (primeNum % i == 0) { // If divisible
                    isPrime = false; // Not prime
                    break;
                }
            }
        }
        
        if (isPrime) {
            System.out.println("The number is a prime number.");
        } else {
            System.out.println("The number is not a prime number.");
        }
        
        // Real-time Example 3: Check Perfect number
        System.out.println("\n=== Check Perfect Number ===");
        System.out.print("Enter a number: ");
        int perfectNum = scanner.nextInt(); // Read number
        int sumFactors = 0; // Initialize sum
        
        for (int i = 1; i < perfectNum; i++) { // Check divisors
            if (perfectNum % i == 0) { // If divisor
                sumFactors += i; // Add to sum
            }
        }
        
        if (sumFactors == perfectNum) { // Compare sum with number
            System.out.println("The number is a perfect number.");
        } else {
            System.out.println("The number is not a perfect number.");
        }
        
        // Real-time Example 4: Sum of digits
        System.out.println("\n=== Sum of Digits ===");
        System.out.print("Enter a number: ");
        int sumDigitsNum = scanner.nextInt(); // Read number
        int sumDigits = 0; // Initialize sum
        
        while (sumDigitsNum > 0) { // Loop through digits
            int digit = sumDigitsNum % 10; // Get digit
            sumDigits += digit; // Add to sum
            sumDigitsNum /= 10; // Remove digit
        }
        
        System.out.println("The sum of digits is: " + sumDigits);
        
        // Real-time Example 5: Reverse a number
        System.out.println("\n=== Reverse a Number ===");
        System.out.print("Enter a number: ");
        int reverseNum = scanner.nextInt(); // Read number
        int reverse = 0; // Initialize reverse
        
        while (reverseNum > 0) { // Loop until number is 0
            int digit = reverseNum % 10; // Get last digit
            reverse = reverse * 10 + digit; // Build reverse
            reverseNum /= 10; // Remove last digit
        }
        
        System.out.println("The reverse of the number is: " + reverse);
        
        // Real-time Example 6: Count digits
        System.out.println("\n=== Count Digits ===");
        System.out.print("Enter a number: ");
        int countNum = scanner.nextInt(); // Read number
        int count = 0; // Initialize count
        
        while (countNum > 0) { // Loop until number is 0
            countNum /= 10; // Remove digit
            count++; // Increment count
        }
        
        System.out.println("Number of digits: " + count);
        
        scanner.close();
    }
}
