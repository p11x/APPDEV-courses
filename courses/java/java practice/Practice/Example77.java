/*
 * SUB TOPIC: VarArgs - Variable Arguments
 * 
 * DEFINITION:
 * VarArgs allows methods to accept variable number of arguments using ellipsis (...).
 */

public class Example77 {
    static void print(String... names) {
        for (String n : names) System.out.println(n);
    }
    
    public static void main(String[] args) {
        print("John", "Jane", "Mike");
    }
}
