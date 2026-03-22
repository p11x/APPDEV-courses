// Simple Expression Evaluator
public class Own60 {
    public static void main(String[] args) {
        java.util.Scanner scanner = new java.util.Scanner(System.in);
        
        System.out.println("=== Simple Expression Evaluator ===");
        System.out.println();
        System.out.println("This program evaluates simple mathematical expressions.");
        System.out.println("Supported operators: +  -  *  /  %");
        System.out.println("Example: 23 + 45, 10 * 5, 100 / 4");
        System.out.println();
        
        System.out.print("Enter expression (or 'quit' to exit): ");
        String input = scanner.nextLine();
        
        if (input.equalsIgnoreCase("quit")) {
            System.out.println("Goodbye!");
            scanner.close();
            return;
        }
        
        try {
            double result = evaluateExpression(input.trim());
            System.out.println();
            System.out.println("Result: " + result);
        } catch (Exception e) {
            System.out.println("Error: Invalid expression!");
            System.out.println("Please use format: number operator number");
            System.out.println("Example: 23 + 45");
        }
        
        scanner.close();
    }
    
    static double evaluateExpression(String expr) throws Exception {
        expr = expr.replaceAll("\\s+", ""); // Remove all spaces
        
        // Find the operator
        char operator = ' ';
        int operatorIndex = -1;
        
        for (int i = 0; i < expr.length(); i++) {
            char c = expr.charAt(i);
            if (c == '+' || c == '-' || c == '*' || c == '/' || c == '%') {
                // Handle negative numbers
                if (i == 0 && (c == '-' || c == '+')) {
                    continue;
                }
                // Handle cases like "10+-5"
                if (i > 0 && (expr.charAt(i-1) == '+' || expr.charAt(i-1) == '-' || 
                              expr.charAt(i-1) == '*' || expr.charAt(i-1) == '/' || expr.charAt(i-1) == '%')) {
                    continue;
                }
                operator = c;
                operatorIndex = i;
                break;
            }
        }
        
        if (operatorIndex == -1) {
            throw new Exception("No operator found");
        }
        
        // Extract operands
        String num1Str = expr.substring(0, operatorIndex);
        String num2Str = expr.substring(operatorIndex + 1);
        
        if (num1Str.isEmpty() || num2Str.isEmpty()) {
            throw new Exception("Missing operand");
        }
        
        double num1 = Double.parseDouble(num1Str);
        double num2 = Double.parseDouble(num2Str);
        
        // Perform calculation
        switch (operator) {
            case '+':
                return num1 + num2;
            case '-':
                return num1 - num2;
            case '*':
                return num1 * num2;
            case '/':
                if (num2 == 0) {
                    throw new Exception("Division by zero");
                }
                return num1 / num2;
            case '%':
                if (num2 == 0) {
                    throw new Exception("Modulo by zero");
                }
                return num1 % num2;
            default:
                throw new Exception("Unknown operator");
        }
    }
}
