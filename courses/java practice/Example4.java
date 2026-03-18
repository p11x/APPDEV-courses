public class Example4 {
    
    public static void main(String[] args) {
        System.out.println("Odd Numbers from 1 to 25:");
        System.out.println("---------------------------");
        
        for (int i = 1; i <= 25; i++) {
            // Check if the number is odd (not divisible by 2)
            if (i % 2 != 0) {
                System.out.println(i);
            }
            // Even numbers are completely skipped
        }
        
        System.out.println("---------------------------");
        System.out.println("All odd numbers displayed!");
    }
}