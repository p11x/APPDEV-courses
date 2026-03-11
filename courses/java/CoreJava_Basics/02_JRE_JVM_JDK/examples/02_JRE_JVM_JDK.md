# JRE, JVM, and JDK

## Short Definition

Understanding the difference between JVM, JRE, and JDK is essential for Java developers. These are the three main components that enable Java programs to run on any platform.

---

## Key Bullet Points

- **JVM (Java Virtual Machine)**: The engine that executes Java bytecode. It converts bytecode into machine-specific instructions.
- **JRE (Java Runtime Environment)**: Contains the JVM and libraries needed to run Java applications. It's what end-users need.
- **JDK (Java Development Kit)**: Contains JRE plus development tools (compiler, debugger). It's what developers need to create Java programs.
- All three work together to enable Java's platform-independent feature.

---

## Relationship Diagram

```
┌─────────────────────────────────────────────────────┐
│                      JDK                             │
│  ┌─────────────────────────────────────────────┐   │
│  │                   JRE                          │   │
│  │  ┌───────────────────────────────────────┐   │   │
│  │  │                 JVM                     │   │   │
│  │  │  ┌─────────────────────────────────┐  │   │   │
│  │  │  │     Your Java Bytecode          │  │   │   │
│  │  │  │         (.class files)           │  │   │   │
│  │  │  └─────────────────────────────────┘  │   │   │   │
│  │  └───────────────────────────────────────┘   │   │
│  └─────────────────────────────────────────────┘   │
│  + Compiler (javac), Debugger, Tools               │
└─────────────────────────────────────────────────────┘
```

---

## Brief Comparison Table

| Component | Full Name | Purpose | What It Contains |
|-----------|-----------|---------|-------------------|
| **JVM** | Java Virtual Machine | Executes bytecode | Interpreter, JIT Compiler, Garbage Collector |
| **JRE** | Java Runtime Environment | Runs Java applications | JVM + Core Libraries |
| **JDK** | Java Development Kit | Develop Java programs | JRE + Development Tools (javac, java, jar, etc.) |

---

## Simple Example

```java
// Test.java
public class Test {
    public static void main(String[] args) {
        System.out.println("Java is platform independent!");
    }
}
```

**How it works:**

1. **Write**: You write the code in a `.java` file
2. **Compile**: JDK's `javac` compiler converts `.java` to `.class` (bytecode)
3. **Run**: JRE's JVM reads the `.class` file and executes it

---

## Key Takeaway

- **For running Java apps**: Install JRE
- **For developing Java apps**: Install JDK
- **JVM**: Included in both JRE and JDK
