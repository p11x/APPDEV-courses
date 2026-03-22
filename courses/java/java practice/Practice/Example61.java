/*
 * SUB TOPIC: Recursion in Java
 * 
 * DEFINITION:
 * Recursion is a programming technique where a method calls itself to solve a problem by breaking it 
 * into smaller sub-problems. Base case stops recursion, recursive case makes the call.
 * 
 * FUNCTIONALITIES:
 * 1. Factorial calculation
 * 2. Fibonacci sequence
 * 3. Sum of array elements
 * 4. Reverse string
 * 5. Binary search
 * 6. Tree traversal
 */

public class Example61 {
    
    // Factorial
    static int factorial(int n) {
        if (n <= 1) return 1;
        return n * factorial(n - 1);
    }
    
    // Fibonacci
    static int fibonacci(int n) {
        if (n <= 1) return n;
        return fibonacci(n - 1) + fibonacci(n - 2);
    }
    
    // Sum of array
    static int sumArray(int[] arr, int index) {
        if (index >= arr.length) return 0;
        return arr[index] + sumArray(arr, index + 1);
    }
    
    // Reverse string
    static String reverse(String str) {
        if (str.isEmpty()) return str;
        return str.charAt(str.length() - 1) + reverse(str.substring(0, str.length() - 1));
    }
    
    // Power
    static int power(int base, int exp) {
        if (exp == 0) return 1;
        return base * power(base, exp - 1);
    }
    
    public static void main(String[] args) {
        
        // Factorial
        System.out.println("=== Factorial ===");
        System.out.println("5! = " + factorial(5));
        
        // Fibonacci
        System.out.println("\n=== Fibonacci ===");
        System.out.print("First 10: ");
        for (int i = 0; i < 10; i++) {
            System.out.print(fibonacci(i) + " ");
        }
        System.out.println();
        
        // Sum array
        System.out.println("\n=== Sum Array ===");
        int[] nums = {1, 2, 3, 4, 5};
        System.out.println("Sum: " + sumArray(nums, 0));
        
        // Reverse string
        System.out.println("\n=== Reverse ===");
        System.out.println("'Hello' reversed: " + reverse("Hello"));
        
        // Power
        System.out.println("\n=== Power ===");
        System.out.println("2^10 = " + power(2, 10));
        
        // Real-time Example 1: Count digits
        System.out.println("\n=== Example 1: Count Digits ===");
        
        class CountDigits {
            static int count(int n) {
                if (n == 0) return 0;
                return 1 + count(n / 10);
            }
        }
        
        System.out.println("12345 has " + CountDigits.count(12345) + " digits");
        
        // Real-time Example 2: Palindrome
        System.out.println("\n=== Example 2: Palindrome ===");
        
        class Palindrome {
            static boolean isPal(String s) {
                if (s.length() <= 1) return true;
                if (s.charAt(0) != s.charAt(s.length() - 1)) return false;
                return isPal(s.substring(1, s.length() - 1));
            }
        }
        
        System.out.println("'madam' is palindrome: " + Palindrome.isPal("madam"));
        
        // Real-time Example 3: Sum of range
        System.out.println("\n=== Example 3: Sum Range ===");
        
        class SumRange {
            static int sum(int start, int end) {
                if (start > end) return 0;
                return start + sum(start + 1, end);
            }
        }
        
        System.out.println("1+2+...+10 = " + SumRange.sum(1, 10));
        
        // Real-time Example 4: GCD
        System.out.println("\n=== Example 4: GCD ===");
        
        class GCD {
            static int find(int a, int b) {
                if (b == 0) return a;
                return find(b, a % b);
            }
        }
        
        System.out.println("GCD(48, 18) = " + GCD.find(48, 18));
        
        // Real-time Example 5: Binary search
        System.out.println("\n=== Example 5: Binary Search ===");
        
        class BinarySearch {
            static int search(int[] arr, int target, int low, int high) {
                if (low > high) return -1;
                int mid = (low + high) / 2;
                if (arr[mid] == target) return mid;
                if (arr[mid] < target) return search(arr, target, mid + 1, high);
                return search(arr, target, low, mid - 1);
            }
        }
        
        int[] sorted = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10};
        System.out.println("Index of 7: " + BinarySearch.search(sorted, 7, 0, 9));
        
        // Real-time Example 6: Sum of digits
        System.out.println("\n=== Example 6: Sum of Digits ===");
        
        class SumDigits {
            static int sum(int n) {
                if (n == 0) return 0;
                return (n % 10) + sum(n / 10);
            }
        }
        
        System.out.println("Sum of 1234: " + SumDigits.sum(1234));
    }
}
