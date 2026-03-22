/*
 * SUB TOPIC: Wrapper Classes - Autoboxing and Unboxing
 * 
 * DEFINITION:
 * Wrapper classes convert primitives to objects. Autoboxing converts primitive to wrapper, unboxing does reverse.
 */

public class Example76 {
    public static void main(String[] args) {
        Integer num = 10; // Autoboxing
        int n = num; // Unboxing
        
        System.out.println("Autoboxed: " + num);
        System.out.println("Unboxed: " + n);
        
        // ArrayList uses autoboxing
        java.util.ArrayList<Integer> list = new java.util.ArrayList<>();
        list.add(5); // Autoboxing
        int x = list.get(0); // Unboxing
        System.out.println("List: " + x);
    }
}
