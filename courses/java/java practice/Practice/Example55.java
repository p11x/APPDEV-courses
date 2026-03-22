/*
 * SUB TOPIC: Method References in Java
 * 
 * DEFINITION:
 * Method references are shorthand notations of lambda expressions to call a method. They provide easy 
 * ways to refer to methods. Types include: static methods, instance methods of objects, 
 * and instance methods of arbitrary objects.
 * 
 * FUNCTIONALITIES:
 * 1. Static method reference - ClassName::staticMethod
 * 2. Instance method of object - object::instanceMethod
 * 3. Instance method of type - ClassName::instanceMethod
 * 4. Constructor reference - ClassName::new
 */

import java.util.*;
import java.util.function.*;

public class Example55 {
    
    // Static method
    static int add(int a, int b) {
        return a + b;
    }
    
    // Instance method
    int multiply(int a, int b) {
        return a * b;
    }
    
    public static void main(String[] args) {
        
        // Static method reference
        System.out.println("=== Static Method Reference ===");
        
        BiFunction<Integer, Integer, Integer> adder = Example55::add;
        System.out.println("5 + 3 = " + adder.apply(5, 3));
        
        // Instance method reference
        System.out.println("\n=== Instance Method Reference ===");
        
        Example55 obj = new Example55();
        BiFunction<Integer, Integer, Integer> multiplier = obj::multiply;
        System.out.println("5 * 3 = " + multiplier.apply(5, 3));
        
        // Constructor reference
        System.out.println("\n=== Constructor Reference ===");
        
        Supplier<ArrayList<String>> listSupplier = ArrayList::new;
        ArrayList<String> list = listSupplier.get();
        list.add("Item");
        System.out.println("List: " + list);
        
        // Method reference with built-in functional interfaces
        System.out.println("\n=== Built-in Interfaces ===");
        
        Function<String, String> upper = String::toUpperCase;
        System.out.println("hello -> " + upper.apply("hello"));
        
        Predicate<String> isEmpty = String::isEmpty;
        System.out.println("Empty '': " + isEmpty.test(""));
        System.out.println("Empty 'a': " + isEmpty.test("a"));
        
        // Real-time Example 1: List operations
        System.out.println("\n=== Example 1: List Operations ===");
        
        List<String> names = Arrays.asList("john", "jane", "mike");
        
        names.forEach(System.out::println);
        
        // Real-time Example 2: Sorting
        System.out.println("\n=== Example 2: Sorting ===");
        
        List<Integer> nums = Arrays.asList(5, 2, 8, 1, 9);
        nums.sort(Integer::compareTo);
        System.out.println("Sorted: " + nums);
        
        // Real-time Example 3: Mapping
        System.out.println("\n=== Example 3: Mapping ===");
        
        List<String> words = Arrays.asList("hello", "world", "java");
        List<Integer> lengths = new ArrayList<>();
        
        words.stream()
            .map(String::length)
            .forEach(lengths::add);
        
        System.out.println("Lengths: " + lengths);
        
        // Real-time Example 4: Factory pattern
        System.out.println("\n=== Example 4: Factory ===");
        
        class Product {
            String name;
            double price;
            
            Product(String name, double price) {
                this.name = name;
                this.price = price;
            }
        }
        
        Function<String, Product> productFactory = Product::new;
        
        Product laptop = productFactory.apply("Laptop");
        System.out.println("Created: " + laptop.name);
        
        // Real-time Example 5: Comparing
        System.out.println("\n=== Example 5: Comparing ===");
        
        Comparator<String> comp = String::compareTo;
        int result = comp.compare("apple", "banana");
        System.out.println("Compare: " + result);
        
        // Real-time Example 6: Function composition
        System.out.println("\n=== Example 6: Chaining ===");
        
        Function<String, String> trim = String::trim;
        Function<String, String> upper2 = String::toUpperCase;
        
        Function<String, String> combined = trim.andThen(upper2);
        
        System.out.println("Result: " + combined.apply("  hello  "));
    }
}
