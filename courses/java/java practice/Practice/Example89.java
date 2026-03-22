/*
 * SUB TOPIC: Comparator Interface
 */

import java.util.*;

public class Example89 {
    public static void main(String[] args) {
        List<String> names = Arrays.asList("John", "Jane", "Bob");
        Collections.sort(names, (a,b) -> b.compareTo(a));
        System.out.println(names);
    }
}
