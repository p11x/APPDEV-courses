// UserInputValidationDemo - Demonstrates String Methods for Input Validation
// Use case: Validating Angular form inputs on backend

public class UserInputValidationDemo {
    
    // Validate email format
    public static boolean validateEmail(String email) {
        if (email == null || email.isEmpty()) {
            return false;
        }
        
        // Check basic email format
        boolean hasAt = email.contains("@");
        boolean hasDot = email.contains(".");
        boolean hasSpace = email.contains(" ");
        
        // More sophisticated validation
        int atIndex = email.indexOf("@");
        int dotIndex = email.lastIndexOf(".");
        
        return hasAt && hasDot && !hasSpace 
            && atIndex > 0 && dotIndex > atIndex + 1
            && dotIndex < email.length() - 1;
    }
    
    // Validate password strength
    public static String validatePassword(String password) {
        if (password == null) {
            return "Password is required";
        }
        
        if (password.length() < 8) {
            return "Password must be at least 8 characters";
        }
        
        boolean hasUpper = false, hasLower = false, hasDigit = false, hasSpecial = false;
        
        for (char c : password.toCharArray()) {
            if (Character.isUpperCase(c)) hasUpper = true;
            else if (Character.isLowerCase(c)) hasLower = true;
            else if (Character.isDigit(c)) hasDigit = true;
            else hasSpecial = true;
        }
        
        if (!hasUpper) return "Password must contain uppercase letter";
        if (!hasLower) return "Password must contain lowercase letter";
        if (!hasDigit) return "Password must contain number";
        if (!hasSpecial) return "Password must contain special character";
        
        return "Password is strong";
    }
    
    // Sanitize user input (prevent XSS)
    public static String sanitizeInput(String input) {
        if (input == null) {
            return "";
        }
        
        // Remove script tags and dangerous patterns
        String result = input;
        result = removePattern(result, "<script>");
        result = removePattern(result, "</script>");
        result = removePattern(result, "javascript:");
        result = removePattern(result, "onerror=");
        return result.trim();
    }
    
    private static String removePattern(String text, String pattern) {
        return text.replace(pattern, "");
    }
    
    // Format phone number
    public static String formatPhoneNumber(String phone) {
        if (phone == null) return "";
        
        // Remove all non-digits
        String digits = phone.replaceAll("\\D", "");
        
        if (digits.length() == 10) {
            return String.format("(%s) %s-%s", 
                digits.substring(0, 3),
                digits.substring(3, 6),
                digits.substring(6));
        }
        
        return phone;
    }
    
    // Format username (lowercase, no spaces)
    public static String formatUsername(String username) {
        if (username == null) return "";
        
        return username
            .toLowerCase()
            .replaceAll("\\s+", "_")
            .replaceAll("[^a-z0-9_]", "");
    }
    
    public static void main(String[] args) {
        System.out.println("=== STRING VALIDATION FOR ANGULAR FORMS ===\n");
        
        // Email validation
        String[] emails = {"test@example.com", "invalid.email", "user@domain", ""};
        System.out.println("--- Email Validation ---");
        for (String email : emails) {
            System.out.println("'" + email + "': " + validateEmail(email));
        }
        
        // Password validation
        System.out.println("\n--- Password Validation ---");
        System.out.println("'Pass123': " + validatePassword("Pass123"));
        System.out.println("'StrongP@ss1': " + validatePassword("StrongP@ss1"));
        
        // Input sanitization
        System.out.println("\n--- Input Sanitization ---");
        String malicious = "<script>alert('xss')</script>";
        System.out.println("Original: " + malicious);
        System.out.println("Sanitized: " + sanitizeInput(malicious));
        
        // Phone formatting
        System.out.println("\n--- Phone Formatting ---");
        System.out.println("1234567890 -> " + formatPhoneNumber("1234567890"));
        System.out.println("(123) 456-7890 -> " + formatPhoneNumber("(123) 456-7890"));
        
        // Username formatting
        System.out.println("\n--- Username Formatting ---");
        System.out.println("John Doe -> " + formatUsername("John Doe"));
        System.out.println("User@123 -> " + formatUsername("User@123"));
        
        System.out.println("\n=== USE CASES ===");
        System.out.println("1. Form input validation (Reactive Forms)");
        System.out.println("2. XSS prevention");
        System.out.println("3. Data formatting for display");
        System.out.println("4. Username normalization");
    }
}
