/*
 * SUB TOPIC: Lambda Expressions Basics
 * 
 * DEFINITION:
 * Lambda expressions provide inline implementation of functional interfaces.
 */

import java.util.*;
import java.util.function.*;

public class Example81 {
    public static void main(String[] args) {
        Runnable r = () -> System.out.println("Hello");
        r.run();
        
        Consumer<String> c = s -> System.out.println(s);
        c.accept("World");
    }
}
