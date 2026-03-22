/*
 * SUB TOPIC: ArrayList in Java
 * 
 * DEFINITION:
 * ArrayList is a resizable array implementation of the List interface.
 */

import java.util.*;

public class Example79 {
    public static void main(String[] args) {
        ArrayList<String> list = new ArrayList<>();
        list.add("Apple");
        list.add("Banana");
        list.add("Cherry");
        
        System.out.println(list);
        System.out.println("Size: " + list.size());
        System.out.println("Get 0: " + list.get(0));
        list.remove(1);
        System.out.println("After remove: " + list);
    }
}
