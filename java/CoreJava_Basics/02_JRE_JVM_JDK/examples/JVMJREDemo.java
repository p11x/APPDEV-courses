// JVMJREDemo - Demonstrates JVM, JRE, JDK concepts
// Shows the Java platform architecture

public class JVMJREDemo {
    
    public static void main(String[] args) {
        System.out.println("=== JVM, JRE, JDK DEMO ===\n");
        
        // Show Java version (runtime info)
        System.out.println("--- Java Runtime Information ---");
        System.out.println("Java Version: " + System.getProperty("java.version"));
        System.out.println("Java Vendor: " + System.getProperty("java.vendor"));
        System.out.println("Java Home: " + System.getProperty("java.home"));
        System.out.println("OS Name: " + System.getProperty("os.name"));
        System.out.println("OS Arch: " + System.getProperty("os.arch"));
        
        System.out.println("\n--- JDK vs JRE vs JVM ---");
        System.out.println("JDK (Java Development Kit):");
        System.out.println("  - Includes JRE + Development tools");
        System.out.println("  - Contains: javac, java, jar, jdb, etc.");
        System.out.println("  - Used for: Writing and compiling Java code");
        
        System.out.println("\nJRE (Java Runtime Environment):");
        System.out.println("  - Includes JVM + Libraries");
        System.out.println("  - Used for: Running Java applications");
        
        System.out.println("\nJVM (Java Virtual Machine):");
        System.out.println("  - Executes bytecode (.class files)");
        System.out.println("  - Provides platform independence");
        System.out.println("  - Components: Class Loader, Bytecode Verifier, JIT Compiler");
        
        System.out.println("\n--- How Java Works ---");
        System.out.println("1. Write: Developer writes .java source file");
        System.out.println("2. Compile: javac compiles to .class bytecode");
        System.out.println("3. Load: ClassLoader loads .class into JVM");
        System.out.println("4. Verify: BytecodeVerifier checks code");
        System.out.println("5. Execute: JIT compiles to native machine code");
        
        // Memory info
        System.out.println("\n--- JVM Memory ---");
        Runtime rt = Runtime.getRuntime();
        System.out.println("Max Memory: " + rt.maxMemory() / 1024 / 1024 + " MB");
        System.out.println("Total Memory: " + rt.totalMemory() / 1024 / 1024 + " MB");
        System.out.println("Free Memory: " + rt.freeMemory() / 1024 / 1024 + " MB");
    }
}
