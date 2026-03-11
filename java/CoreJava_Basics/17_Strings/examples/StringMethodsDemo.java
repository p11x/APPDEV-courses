// StringMethodsDemo - Demonstrates String operations in Java
// Important for text processing in backend applications

public class StringMethodsDemo {
    
    public static void main(String[] args) {
        System.out.println("=== STRING METHODS DEMO ===\n");
        
        String str = "Hello, World!";
        String empty = "";
        String spaces = "   Trim me   ";
        String numbers = "12345";
        String mixed = "JaVaScRiPt";
        
        // Basic methods
        System.out.println("--- Basic Methods ---");
        System.out.println("Original: " + str);
        System.out.println("Length: " + str.length());
        System.out.println("Char at 0: " + str.charAt(0));
        System.out.println("Substring (0,5): " + str.substring(0, 5));
        System.out.println("Uppercase: " + str.toUpperCase());
        System.out.println("Lowercase: " + str.toLowerCase());
        
        // Search methods
        System.out.println("\n--- Search Methods ---");
        System.out.println("Contains 'World': " + str.contains("World"));
        System.out.println("Index of 'o': " + str.indexOf('o'));
        System.out.println("Last index of 'o': " + str.lastIndexOf('o'));
        System.out.println("Starts with 'Hello': " + str.startsWith("Hello"));
        System.out.println("Ends with '!': " + str.endsWith("!"));
        
        // Modification methods
        System.out.println("\n--- Modification Methods ---");
        System.out.println("Replace 'World' with 'Java': " + str.replace("World", "Java"));
        System.out.println("Trimmed: '" + spaces.trim() + "'");
        System.out.println("Concatenate: " + str.concat(" - Added"));
        
        // Comparison methods
        System.out.println("\n--- Comparison Methods ---");
        String s1 = "hello";
        String s2 = "hello";
        String s3 = "Hello";
        System.out.println("s1 == s2: " + (s1 == s2));
        System.out.println("s1.equals(s2): " + s1.equals(s2));
        System.out.println("s1.equalsIgnoreCase(s3): " + s1.equalsIgnoreCase(s3));
        System.out.println("CompareTo: " + s1.compareTo(s3));
        
        // StringBuilder (mutable)
        System.out.println("\n--- StringBuilder ---");
        StringBuilder sb = new StringBuilder("Hello");
        sb.append(" World");
        sb.insert(5, ",");
        sb.reverse();
        System.out.println("StringBuilder result: " + sb.toString());
        
        // String splitting
        System.out.println("\n--- String Split ---");
        String csv = "apple,banana,cherry,date";
        String[] fruits = csv.split(",");
        System.out.print("Split result: ");
        for (String f : fruits) {
            System.out.print(f + " ");
        }
        System.out.println();
        
        // Angular integration note
        System.out.println("\n=== ANGULAR INTEGRATION ===");
        System.out.println("String -> JSON string");
        System.out.println("String methods map to JavaScript String methods");
    }
}
