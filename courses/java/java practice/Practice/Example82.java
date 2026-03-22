/*
 * SUB TOPIC: Stream API - Filter and Map
 * 
 * DEFINITION:
 * Stream API provides functional-style operations on collections.
 */

import java.util.*;

public class Example82 {
    public static void main(String[] args) {
        List<Integer> nums = Arrays.asList(1,2,3,4,5);
        
        nums.stream()
            .filter(n -> n % 2 == 0)
            .forEach(System.out::println);
    }
}
