/*
 * SUB TOPIC: Enhanced for Loop (for-each)
 * 
 * DEFINITION:
 * Enhanced for loop provides simpler iteration over collections and arrays.
 */

import java.util.*;

public class Example78 {
    public static void main(String[] args) {
        int[] nums = {1, 2, 3, 4, 5};
        for (int n : nums) System.out.print(n + " ");
        
        System.out.println();
        
        ArrayList<String> names = new ArrayList<>();
        names.add("John");
        names.add("Jane");
        for (String name : names) System.out.println(name);
    }
}
