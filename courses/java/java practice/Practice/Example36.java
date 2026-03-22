/*
 * SUB TOPIC: Generics in Java
 * 
 * DEFINITION:
 * Generics enable classes, interfaces, and methods to operate on objects of various types while 
 * providing compile-time type safety. They allow you to create reusable code that works with 
 * different data types without casting.
 * 
 * FUNCTIONALITIES:
 * 1. Generic Classes - Create classes with type parameters
 * 2. Generic Methods - Methods that can work with any type
 * 3. Bounded Type Parameters - Restrict types that can be used
 * 4. Wildcards - ? for unknown types
 * 5. Type Erasure - How generics work at runtime
 */

public class Example36 {
    
    // Generic class - Box that can hold any type
    static class Box<T> {
        private T content;
        
        public void set(T content) {
            this.content = content;
        }
        
        public T get() {
            return content;
        }
    }
    
    // Generic class with multiple type parameters
    static class Pair<K, V> {
        private K key;
        private V value;
        
        public Pair(K key, V value) {
            this.key = key;
            this.value = value;
        }
        
        public K getKey() { return key; }
        public V getValue() { return value; }
    }
    
    // Generic method - Print any array
    public static <T> void printArray(T[] array) {
        for (T element : array) {
            System.out.print(element + " ");
        }
        System.out.println();
    }
    
    // Generic method with bounded type - Find maximum
    public static <T extends Comparable<T>> T findMax(T[] array) {
        if (array == null || array.length == 0) {
            return null;
        }
        T max = array[0];
        for (int i = 1; i < array.length; i++) {
            if (array[i].compareTo(max) > 0) {
                max = array[i];
            }
        }
        return max;
    }
    
    public static void main(String[] args) {
        
        // Topic Explanation: Generic Class
        
        // Using Box with Integer
        System.out.println("=== Generic Box with Integer ===");
        Box<Integer> intBox = new Box<>();
        intBox.set(100);
        System.out.println("Value: " + intBox.get());
        
        // Using Box with String
        System.out.println("\n=== Generic Box with String ===");
        Box<String> strBox = new Box<>();
        strBox.set("Hello Generics");
        System.out.println("Value: " + strBox.get());
        
        // Using Pair with multiple types
        System.out.println("\n=== Generic Pair ===");
        Pair<String, Integer> student = new Pair<>("Alice", 95);
        System.out.println("Name: " + student.getKey());
        System.out.println("Score: " + student.getValue());
        
        // Generic method usage
        System.out.println("\n=== Generic Method ===");
        Integer[] nums = {1, 2, 3, 4, 5};
        String[] words = {"apple", "banana", "cherry"};
        
        System.out.print("Numbers: ");
        printArray(nums);
        
        System.out.print("Words: ");
        printArray(words);
        
        // Bounded type parameter - Find maximum
        System.out.println("\n=== Bounded Type - Find Max ===");
        Integer[] numbers = {5, 2, 8, 1, 9};
        System.out.println("Max number: " + findMax(numbers));
        
        String[] fruits = {"apple", "banana", "cherry"};
        System.out.println("Max fruit: " + findMax(fruits));
        
        // Real-time Example 1: Generic Repository for any entity
        System.out.println("\n=== Example 1: Generic Repository ===");
        class GenericRepository<T> {
            private T data;
            
            public void save(T data) {
                this.data = data;
            }
            
            public T get() {
                return data;
            }
        }
        
        GenericRepository<String> stringRepo = new GenericRepository<>();
        stringRepo.save("User data");
        System.out.println("Saved: " + stringRepo.get());
        
        // Real-time Example 2: Generic Cache
        System.out.println("\n=== Example 2: Generic Cache ===");
        class Cache<K, V> {
            private K key;
            private V value;
            
            public void put(K key, V value) {
                this.key = key;
                this.value = value;
            }
            
            public V get(K searchKey) {
                if (key.equals(searchKey)) {
                    return value;
                }
                return null;
            }
        }
        
        Cache<String, Integer> scoreCache = new Cache<>();
        scoreCache.put("Math", 95);
        scoreCache.put("Science", 88);
        System.out.println("Math score: " + scoreCache.get("Math"));
        
        // Real-time Example 3: Generic List Container
        System.out.println("\n=== Example 3: Generic List Container ===");
        class Container<T> {
            private java.util.List<T> items = new java.util.ArrayList<>();
            
            public void add(T item) {
                items.add(item);
            }
            
            public T get(int index) {
                return items.get(index);
            }
            
            public int size() {
                return items.size();
            }
        }
        
        Container<Double> prices = new Container<>();
        prices.add(19.99);
        prices.add(29.99);
        prices.add(9.99);
        System.out.println("Price 1: " + prices.get(0));
        System.out.println("Total prices: " + prices.size());
        
        // Real-time Example 4: Generic Calculator
        System.out.println("\n=== Example 4: Generic Calculator ===");
        class Calculator<T extends Number> {
            public double sum(T a, T b) {
                return a.doubleValue() + b.doubleValue();
            }
            
            public double multiply(T a, T b) {
                return a.doubleValue() * b.doubleValue();
            }
        }
        
        Calculator<Integer> intCalc = new Calculator<>();
        System.out.println("10 + 5 = " + intCalc.sum(10, 5));
        System.out.println("10 * 5 = " + intCalc.multiply(10, 5));
        
        Calculator<Double> doubleCalc = new Calculator<>();
        System.out.println("10.5 + 5.5 = " + doubleCalc.sum(10.5, 5.5));
        
        // Real-time Example 5: Generic Stack
        System.out.println("\n=== Example 5: Generic Stack ===");
        class Stack<T> {
            private java.util.ArrayList<T> elements = new java.util.ArrayList<>();
            
            public void push(T item) {
                elements.add(item);
            }
            
            public T pop() {
                if (elements.isEmpty()) {
                    return null;
                }
                return elements.remove(elements.size() - 1);
            }
            
            public boolean isEmpty() {
                return elements.isEmpty();
            }
        }
        
        Stack<String> stack = new Stack<>();
        stack.push("First");
        stack.push("Second");
        stack.push("Third");
        
        System.out.println("Pop: " + stack.pop());
        System.out.println("Pop: " + stack.pop());
        System.out.println("Is empty: " + stack.isEmpty());
        
        // Real-time Example 6: Wildcard usage
        System.out.println("\n=== Example 6: Wildcard Usage ===");
        
        // ? extends Number - Read as Number
        java.util.List<Integer> ints = java.util.Arrays.asList(1, 2, 3);
        java.util.List<Double> doubles = java.util.Arrays.asList(1.1, 2.2, 3.3);
        
        printNumbers(ints);
        printNumbers(doubles);
    }
    
    // Wildcard method - accepts any type that extends Number
    public static void printNumbers(java.util.List<? extends Number> list) {
        System.out.print("Numbers: ");
        for (Number n : list) {
            System.out.print(n + " ");
        }
        System.out.println();
    }
}
