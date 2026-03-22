import java.util.Scanner;

// Caesar Cipher Encoder/Decoder
public class Own5 {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        
        System.out.println("=== Caesar Cipher Encoder/Decoder ===");
        System.out.println();
        
        // Get message input
        System.out.print("Enter a message: ");
        String message = scanner.nextLine();
        
        // Get shift value
        System.out.print("Enter shift value (1-25): ");
        int shift = scanner.nextInt();
        
        // Validate shift value
        while (shift < 1 || shift > 25) {
            System.out.print("Invalid shift! Enter value between 1 and 25: ");
            shift = scanner.nextInt();
        }
        
        // Encode the message
        String encoded = encode(message, shift);
        System.out.println();
        System.out.println("Encoded message: " + encoded);
        
        // Decode the message
        String decoded = decode(encoded, shift);
        System.out.println("Decoded message: " + decoded);
        
        scanner.close();
    }
    
    // Method to encode using Caesar Cipher
    public static String encode(String text, int shift) {
        StringBuilder result = new StringBuilder();
        
        for (int i = 0; i < text.length(); i++) {
            char ch = text.charAt(i);
            
            if (Character.isLetter(ch)) {
                // Determine if uppercase or lowercase
                if (Character.isUpperCase(ch)) {
                    char encodedChar = (char) (((ch - 'A' + shift) % 26) + 'A');
                    result.append(encodedChar);
                } else {
                    char encodedChar = (char) (((ch - 'a' + shift) % 26) + 'a');
                    result.append(encodedChar);
                }
            } else {
                // Non-alphabet characters remain unchanged
                result.append(ch);
            }
        }
        
        return result.toString();
    }
    
    // Method to decode by reversing the shift
    public static String decode(String text, int shift) {
        StringBuilder result = new StringBuilder();
        
        for (int i = 0; i < text.length(); i++) {
            char ch = text.charAt(i);
            
            if (Character.isLetter(ch)) {
                // Determine if uppercase or lowercase
                if (Character.isUpperCase(ch)) {
                    char decodedChar = (char) (((ch - 'A' - shift + 26) % 26) + 'A');
                    result.append(decodedChar);
                } else {
                    char decodedChar = (char) (((ch - 'a' - shift + 26) % 26) + 'a');
                    result.append(decodedChar);
                }
            } else {
                // Non-alphabet characters remain unchanged
                result.append(ch);
            }
        }
        
        return result.toString();
    }
}
