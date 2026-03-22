/*
 * SUB TOPIC: Throw Keyword
 */

public class Example86 {
    static void check(int age) throws Exception {
        if (age < 18) throw new Exception("Too young");
        System.out.println("Allowed");
    }
    
    public static void main(String[] args) {
        try { check(20); } catch (Exception e) { System.out.println(e.getMessage()); }
    }
}
