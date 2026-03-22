/*
 * SUB TOPIC: Finally Block
 */

public class Example85 {
    public static void main(String[] args) {
        try {
            System.out.println("In try");
        } catch (Exception e) {
            System.out.println("In catch");
        } finally {
            System.out.println("In finally");
        }
    }
}
