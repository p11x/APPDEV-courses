// AdvancedGenericsDemo - More Generics Examples
// Generic classes, methods, and wildcards

// Generic class
class Container<T> {
    private T value;
    
    public void set(T value) { this.value = value; }
    public T get() { return value; }
}

// Bounded type - only accepts Number or subclasses
class NumberBox<T extends Number> {
    private T number;
    
    public void set(T number) { this.number = number; }
    public T get() { return number; }
    
    public double getDoubleValue() { return number.doubleValue(); }
}

// Generic method
class Utils {
    public static <T> void printArray(T[] array) {
        for (T item : array) {
            System.out.print(item + " ");
        }
        System.out.println();
    }
    
    public static <T extends Comparable<T>> T findMax(T a, T b) {
        return a.compareTo(b) > 0 ? a : b;
    }
}

// Multiple type parameters
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

public class AdvancedGenericsDemo {
    
    public static void main(String[] args) {
        System.out.println("=== ADVANCED GENERICS ===\n");
        
        // Generic class
        System.out.println("--- Generic Class ---");
        Container<String> strContainer = new Container<>();
        strContainer.set("Hello Generics");
        System.out.println("String: " + strContainer.get());
        
        Container<Integer> intContainer = new Container<>();
        intContainer.set(42);
        System.out.println("Integer: " + intContainer.get());
        
        // Bounded type
        System.out.println("\n--- Bounded Type ---");
        NumberBox<Integer> numBox = new NumberBox<>();
        numBox.set(100);
        System.out.println("Double value: " + numBox.getDoubleValue());
        
        // Generic method
        System.out.println("\n--- Generic Method ---");
        String[] names = {"Alice", "Bob", "Charlie"};
        Utils.printArray(names);
        
        Integer[] nums = {1, 2, 3, 4, 5};
        Utils.printArray(nums);
        
        // Multiple type parameters
        System.out.println("\n--- Multiple Type Parameters ---");
        Pair<String, Integer> pair = new Pair<>("Age", 25);
        System.out.println("Key: " + pair.getKey() + ", Value: " + pair.getValue());
        
        // Wildcards
        System.out.println("\n--- Wildcards ---");
        System.out.println("? extends Number - Upper bound");
        System.out.println("? super Integer - Lower bound");
        System.out.println("? - Unbounded");
    }
}
