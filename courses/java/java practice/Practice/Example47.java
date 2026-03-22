/*
 * SUB TOPIC: JRE, JVM, and JDK - Understanding Java Architecture
 * 
 * DEFINITION:
 * JVM (Java Virtual Machine) executes Java bytecode. JRE (Java Runtime Environment) contains JVM and 
 * libraries needed to run Java applications. JDK (Java Development Kit) includes JRE plus development 
 * tools like compiler (javac) for creating Java programs.
 * 
 * FUNCTIONALITIES:
 * 1. JVM - Executes bytecode, provides platform independence
 * 2. JRE - Runtime environment with JVM and libraries
 * 3. JDK - Development kit with compiler and tools
 * 4. Bytecode - Platform-independent intermediate code
 * 5. JIT Compiler - Just-in-time compilation for performance
 */

public class Example47 {
    public static void main(String[] args) {
        
        // Topic Explanation: JVM, JRE, JDK
        
        System.out.println("=== Understanding JVM, JRE, JDK ===\n");
        
        // JVM (Java Virtual Machine)
        System.out.println("JVM - Java Virtual Machine");
        System.out.println("  - Executes Java bytecode (.class files)");
        System.out.println("  - Converts bytecode to machine code");
        System.out.println("  - Provides platform independence");
        System.out.println("  - Manages memory (heap, stack)");
        
        // JRE (Java Runtime Environment)
        System.out.println("\nJRE - Java Runtime Environment");
        System.out.println("  - Contains JVM");
        System.out.println("  - Includes core libraries");
        System.out.println("  - Needed to RUN Java applications");
        
        // JDK (Java Development Kit)
        System.out.println("\nJDK - Java Development Kit");
        System.out.println("  - Contains JRE");
        System.out.println("  - Includes compiler (javac)");
        System.out.println("  - Includes debugger and tools");
        System.out.println("  - Needed to DEVELOP Java applications");
        
        // How Java code executes
        System.out.println("\n=== How Java Code Executes ===");
        System.out.println("1. Write: Create .java file");
        System.out.println("2. Compile: javac converts to .class (bytecode)");
        System.out.println("3. Run: JVM interprets bytecode");
        
        // Get JVM information
        System.out.println("\n=== JVM Information ===");
        System.out.println("Java Version: " + System.getProperty("java.version"));
        System.out.println("Java Vendor: " + System.getProperty("java.vendor"));
        System.out.println("JVM Name: " + System.getProperty("java.vm.name"));
        System.out.println("JVM Version: " + System.getProperty("java.vm.version"));
        
        // Memory information
        System.out.println("\n=== Memory Information ===");
        Runtime runtime = Runtime.getRuntime();
        System.out.println("Max Memory: " + (runtime.maxMemory() / 1024 / 1024) + " MB");
        System.out.println("Total Memory: " + (runtime.totalMemory() / 1024 / 1024) + " MB");
        System.out.println("Free Memory: " + (runtime.freeMemory() / 1024 / 1024) + " MB");
        
        // Real-time Example 1: Check available processors
        System.out.println("\n=== Example 1: System Processors ===");
        int processors = runtime.availableProcessors();
        System.out.println("Available Processors: " + processors);
        
        // Real-time Example 2: Memory usage
        System.out.println("\n=== Example 2: Memory Usage ===");
        long usedMemory = runtime.totalMemory() - runtime.freeMemory();
        System.out.println("Used Memory: " + (usedMemory / 1024 / 1024) + " MB");
        
        // Force garbage collection
        System.out.println("\n=== Example 3: Garbage Collection ===");
        runtime.gc();
        System.out.println("Garbage collection triggered");
        System.out.println("Free Memory after GC: " + (runtime.freeMemory() / 1024 / 1024) + " MB");
        
        // Real-time Example 4: Classpath
        System.out.println("\n=== Example 4: Java Classpath ===");
        String classpath = System.getProperty("java.class.path");
        System.out.println("Classpath length: " + classpath.length() + " characters");
        
        // Real-time Example 5: OS Information
        System.out.println("\n=== Example 5: OS Information ===");
        System.out.println("OS Name: " + System.getProperty("os.name"));
        System.out.println("OS Version: " + System.getProperty("os.version"));
        System.out.println("OS Architecture: " + System.getProperty("os.arch"));
        
        // Real-time Example 6: Working Directory
        System.out.println("\n=== Example 6: Working Directory ===");
        System.out.println("User Directory: " + System.getProperty("user.dir"));
        System.out.println("User Home: " + System.getProperty("user.home"));
    }
}
