// GenericsDemo - Demonstrates Java Generics
// Generics enable type-safe collections and classes

public class GenericsDemo {
    
    // Generic class
    static class Box<T> {
        private T content;
        
        public void set(T content) { this.content = content; }
        public T get() { return content; }
    }
    
    // Generic method
    public static <T> void printArray(T[] array) {
        for (T element : array) {
            System.out.print(element + " ");
        }
        System.out.println();
    }
    
    // Bounded type parameter
    public static <T extends Number> double sum(T a, T b) {
        return a.doubleValue() + b.doubleValue();
    }
    
    public static void main(String[] args) {
        System.out.println("=== GENERICS DEMO ===\n");
        
        // Generic Box
        System.out.println("--- Generic Box ---");
        Box<String> stringBox = new Box<>();
        stringBox.set("Hello Generics");
        System.out.println("String box: " + stringBox.get());
        
        Box<Integer> intBox = new Box<>();
        intBox.set(42);
        System.out.println("Integer box: " + intBox.get());
        
        // Generic method
        System.out.println("\n--- Generic Method ---");
        Integer[] intArray = {1, 2, 3, 4, 5};
        String[] strArray = {"A", "B", "C"};
        
        System.out.print("Integer array: ");
        printArray(intArray);
        System.out.print("String array: ");
        printArray(strArray);
        
        // Bounded type
        System.out.println("\n--- Bounded Type ---");
        System.out.println("Sum of 5 and 10: " + sum(5, 10));
        System.out.println("Sum of 3.5 and 2.5: " + sum(3.5, 2.5));
        
        // Multiple type parameters
        System.out.println("\n--- Multiple Type Parameters ---");
        Pair<String, Integer> pair = new Pair<>("Age", 25);
        System.out.println("Key: " + pair.getKey() + ", Value: " + pair.getValue());
        
        // Wildcards
        System.out.println("\n--- Wildcards ---");
        java.util.List<Number> numbers = java.util.Arrays.asList(1, 2.5, 3);
        printList(numbers);
    }
    
    // Wildcard example
    public static void printList(java.util.List<?> list) {
        System.out.print("List: ");
        for (Object obj : list) {
            System.out.print(obj + " ");
        }
        System.out.println();
    }
}

// Pair class with two type parameters
class Pair<K, V> {
    private K key;
    private V value;
    
    public Pair(K key, V value) {
        this.key = key;
        this.value = value;
    }
    
    public K getKey() { return key; }
    public V getValue() { return value; }
}
