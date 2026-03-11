# Java Editions

## Short Definition

Java comes in different editions, each designed for a specific type of application development. Understanding these editions helps you choose the right one for your project.

---

## Key Bullet Points

- **Java SE (Standard Edition)**: For desktop and basic server applications
- **Java EE (Enterprise Edition)**: For large-scale enterprise applications (now called Jakarta EE)
- **Java ME (Micro Edition)**: For mobile and embedded devices
- Each edition includes different libraries and tools

---

## Java Editions Explained

### 1. Java SE (Standard Edition)

**Purpose:** Core Java platform for desktop and basic server applications.

**What's included:**
- Core libraries and APIs
- Basic language features
- GUI (Graphical User Interface) libraries (AWT, Swing, JavaFX)
- Database connectivity (JDBC)

**Use for:**
- Learning Java fundamentals
- Desktop applications
- Simple server applications
- Console-based programs

**Example:** `java.lang`, `java.util`, `java.io`, `java.awt`, `java.swing`

---

### 2. Java EE (Enterprise Edition) - Now Jakarta EE

**Purpose:** For building large-scale, distributed, enterprise-level applications.

**What's included:**
- All Java SE features plus
- Enterprise APIs (EJB, Servlet, JSP)
- Web services (JAX-WS, JAX-RS)
- Messaging (JMS)
- Transaction management
- Security

**Use for:**
- Web applications
- Enterprise systems
- Cloud-based applications

**Note:** Oracle transferred Java EE to the Eclipse Foundation, which renamed it to **Jakarta EE**.

---

### 3. Java ME (Micro Edition)

**Purpose:** For mobile phones, PDAs, and embedded devices.

**What's included:**
- Lightweight libraries
- Limited memory and processing requirements
- Mobile-specific APIs

**Use for:**
- Mobile applications (older feature phones)
- Embedded systems
- IoT devices

---

## Comparison Table

| Edition | Full Name | Target | Use Case |
|---------|-----------|--------|----------|
| **Java SE** | Standard Edition | Desktop/Server | Learning, basic apps |
| **Java EE** | Enterprise Edition | Enterprise | Large web apps, distributed systems |
| **Java ME** | Micro Edition | Mobile/Embedded | Mobile phones, IoT |

---

## Simple Example (Java SE)

```java
// HelloWorld.java (Java SE)
public class HelloWorld {
    public static void main(String[] args) {
        System.out.println("Hello from Java SE!");
    }
}
```

**Expected Output:**
```
Hello from Java SE!
```

---

## Which Edition to Learn?

**For beginners:** Start with Java SE
- Learn the fundamentals
- Understand object-oriented programming
- Practice with basic programs

**After mastering Java SE:**
- Move to Java EE/Jakarta EE for web development
- Use frameworks like Spring for modern enterprise apps
