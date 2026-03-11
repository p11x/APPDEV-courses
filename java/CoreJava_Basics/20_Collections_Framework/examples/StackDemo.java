// StackDemo - Demonstrates Stack in Java Collections Framework
// Important for LIFO (Last In First Out) operations

import java.util.*;

public class StackDemo {
    
    public static void main(String[] args) {
        System.out.println("=== STACK DEMO ===");
        
        Stack<String> stack = new Stack<>();
        
        stack.push("First");
        stack.push("Second");
        stack.push("Third");
        
        System.out.println("Stack: " + stack);
        System.out.println("Peek: " + stack.peek());
        System.out.println("Pop: " + stack.pop());
        System.out.println("After pop: " + stack);
        
        System.out.println("\n=== USE CASES ===");
        System.out.println("1. Undo operations in editors");
        System.out.println("2. Browser back button");
        System.out.println("3. Expression evaluation");
    }
}
