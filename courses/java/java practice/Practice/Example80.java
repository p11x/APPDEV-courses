/*
 * SUB TOPIC: HashMap Operations
 * 
 * DEFINITION:
 * HashMap stores key-value pairs with O(1) lookup time.
 */

import java.util.*;

public class Example80 {
    public static void main(String[] args) {
        HashMap<Integer, String> map = new HashMap<>();
        map.put(1, "One");
        map.put(2, "Two");
        map.put(3, "Three");
        
        System.out.println(map);
        System.out.println("Get 2: " + map.get(2));
        System.out.println("Contains 3: " + map.containsKey(3));
        
        for (Map.Entry<Integer, String> e : map.entrySet()) {
            System.out.println(e.getKey() + " = " + e.getValue());
        }
    }
}
