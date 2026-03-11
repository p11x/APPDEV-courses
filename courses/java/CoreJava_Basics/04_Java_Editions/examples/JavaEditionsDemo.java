// JavaEditionsDemo - Java Editions and Platforms
// Different Java platforms for different purposes

public class JavaEditionsDemo {
    
    public static void main(String[] args) {
        System.out.println("=== JAVA EDITIONS ===\n");
        
        System.out.println("--- Java SE (Standard Edition) ---");
        System.out.println("Core Java platform");
        System.out.println("Includes: Core APIs, GUI, Networking, Database");
        System.out.println("Use for: Desktop applications, Applets");
        System.out.println("Package: java.*, javax.*");
        
        System.out.println("\n--- Java EE (Enterprise Edition) ---");
        System.out.println("Enterprise Java platform (now Jakarta EE)");
        System.out.println("Includes: Servlets, JSP, EJB, JPA, Web Services");
        System.out.println("Use for: Web apps, Enterprise systems");
        System.out.println("Package: javax.*, jakarta.*");
        
        System.out.println("\n--- Java ME (Micro Edition) ---");
        System.out.println("Lightweight platform for embedded devices");
        System.out.println("Includes: Limited APIs, CLDC, MIDP");
        System.out.println("Use for: Mobile phones, PDAs, Set-top boxes");
        
        System.out.println("\n--- Jakarta EE (Evolved from Java EE) ---");
        System.out.println("Open source enterprise platform");
        System.out.println("Backward compatible with Java EE");
        System.out.println("Now maintained by Eclipse Foundation");
        
        System.out.println("\n--- Java Card ---");
        System.out.println("Platform for secure smart cards");
        System.out.println("Use for: SIM cards, Payment cards");
        
        System.out.println("\n--- Quick Comparison ---");
        System.out.println("| Edition   | Target           | Use Case                |");
        System.out.println("|-----------|------------------|-------------------------|");
        System.out.println("| Java SE   | Desktop/Server  | Core programming        |");
        System.out.println("| Java EE   | Enterprise       | Web & Enterprise apps   |");
        System.out.println("| Java ME   | Embedded         | Mobile/IoT devices     |");
        System.out.println("| Jakarta EE| Enterprise       | Modern web services     |");
        
        System.out.println("\n--- Angular + Java EE/Jakarta EE ---");
        System.out.println("Spring Boot (based on Java EE/Jakarta EE) + Angular = Full-stack");
    }
}
