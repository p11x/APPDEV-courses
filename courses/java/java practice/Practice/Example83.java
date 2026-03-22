/*
 * SUB TOPIC: Exception Handling - Try-Catch
 */

public class Example83 {
    public static void main(String[] args) {
        try {
            int x = 10 / 0;
        } catch (ArithmeticException e) {
            System.out.println("Error: " + e.getMessage());
        }
        System.out.println("Program continues");
    }
}
