# Environment Setup

## Concept Title and Overview

Before we can start building Spring Boot applications, we need to set up your development environment. In this lesson, you'll learn how to install and configure all the necessary tools: Java Development Kit (JDK), Maven, and a code editor. We'll also walk through creating your first Spring Boot project using Spring Initializr.

## Real-World Importance and Context

Think of setting up your development environment like preparing your kitchen before cooking. You need the right tools (knives, pots, pans) and ingredients (fresh produce, spices) before you can create a meal. Similarly, having a properly configured development environment is essential before you can start building applications.

A well-configured environment will:
- Save you countless hours of frustration
- Make debugging easier
- Enable modern development features like auto-reload
- Prepare you for professional development workflows

## Detailed Step-by-Step Explanation

### Installing Java Development Kit (JDK)

Java Development Kit (JDK) is the foundation for all Java development. It includes the Java compiler (javac), runtime environment, and development tools.

#### For Windows Users:

**Option 1: Oracle JDK (Recommended for Enterprise)**
1. Visit: https://www.oracle.com/java/technologies/downloads/
2. Download the Windows x64 installer (JDK 17 or 21 LTS versions)
3. Run the installer and follow the prompts
4. Set environment variables:
   - Right-click "This PC" → Properties → Advanced System Settings
   - Click "Environment Variables"
   - Under "System variables", find "Path" and click "Edit"
   - Add: `C:\Program Files\Java\jdk-21\bin`

**Option 2: Eclipse Temurin (OpenJDK - Free)**
1. Visit: https://adoptium.net/
2. Download Windows x64 JDK (LTS version)
3. Run the installer

**Verifying Installation:**
```cmd
C:\> java -version
java version "21.0.2" 2024-01-16 LTS
Java(TM) SE Runtime Environment (build 21.0.2+13-LTS-198)
Java HotSpot(TM) 64-Bit Server VM (build 21.0.2+13-LTS-198, mixed mode)

C:\> javac -version
javac 21.0.2
```

#### For macOS Users:

**Using Homebrew (Recommended):**
```bash
# Install Homebrew if not already installed
/bin/bash -c "$(curl -fsSL https://raw.githubusercontent.com/Homebrew/install/HEAD/install.sh)"

# Install OpenJDK 21
brew install openjdk@21

# Add to PATH (add to ~/.zshrc or ~/.bash_profile)
export PATH="/usr/local/opt/openjdk@21/bin:$PATH"
```

**Verifying Installation:**
```bash
java -version
javac -version
```

#### For Linux Users:

**Using apt (Ubuntu/Debian):**
```bash
# Update package index
sudo apt update

# Install OpenJDK 21
sudo apt install openjdk-21-jdk

# Verify installation
java -version
javac -version
```

### Installing Maven Build Tool

Maven is a build automation tool that manages project dependencies and builds. Think of it as a sophisticated package manager for Java projects.

#### For Windows:

1. Visit: https://maven.apache.org/download.cgi
2. Download apache-maven-3.9.x-bin.zip
3. Extract to `C:\Program Files\Apache\Maven`
4. Set environment variables:
   - Create system variable `MAVEN_HOME` = `C:\Program Files\Apache\Maven`
   - Add to Path: `%MAVEN_HOME%\bin`

**Verifying Installation:**
```cmd
C:\> mvn -version
Apache Maven 3.9.6 (3e885c2be3a9f13...)
Maven home: C:\Program Files\Apache\Maven
Java version: 21.0.2, vendor: Oracle Corporation
```

#### For macOS:

```bash
# Using Homebrew
brew install maven

# Verify
mvn -version
```

#### For Linux:

```bash
sudo apt install maven
mvn -version
```

### Installing IntelliJ IDEA or VS Code

#### Option 1: IntelliJ IDEA (Recommended for Java)

IntelliJ IDEA is the most popular IDE for Java development with excellent Spring Boot support.

**Installation:**
1. Download: https://www.jetbrains.com/idea/download/
2. Choose Community Edition (free) or Ultimate (paid, free for students)
3. Run the installer

**Essential Plugins:**
1. Open IntelliJ IDEA
2. Go to File → Settings → Plugins
3. Search and install:
   - Spring Boot Helper
   - Lombok
   - Maven Helper

#### Option 2: Visual Studio Code (Lightweight Alternative)

VS Code is lighter and works well for smaller projects.

**Installation:**
1. Download: https://code.visualstudio.com/
2. Run the installer

**Essential Extensions:**
1. Open VS Code
2. Go to Extensions (Ctrl+Shift+X)
3. Install:
   - Extension Pack for Java (Microsoft)
   - Spring Boot Extension Pack (VMware)
   - Maven for Java

**Key VS Code Commands:**
- `Ctrl+Shift+P` → "Java: Create Project" - Create new Java project
- `Ctrl+Shift+P` → "Spring Initializr: Create a Maven Project" - Create Spring Boot project

### Spring Initializr Configuration Walkthrough

Spring Initializr (https://start.spring.io) is the official way to generate Spring Boot projects. It creates a pre-configured project skeleton with all necessary dependencies.

#### Step-by-Step Workflow:

```
┌─────────────────────────────────────────────────────────────────────────┐
│                    SPRING INITIALIZR WORKFLOW                          │
├─────────────────────────────────────────────────────────────────────────┤
│                                                                         │
│  STEP 1: Choose Build Tool                                              │
│  ┌─────────────────────────────────────────────────────────────────┐   │
│  │  ○ Maven Project                                                 │   │
│  │  ○ Gradle Project                                               │   │
│  │                                                                  │   │
│  │  Recommendation: Maven (more common, better plugin support)   │   │
│  └─────────────────────────────────────────────────────────────────┘   │
│                                                                         │
│  STEP 2: Select Language                                                │
│  ┌─────────────────────────────────────────────────────────────────┐   │
│  │  ○ Java                                                         │   │
│  │  ○ Kotlin                                                       │   │
│  │  ○ Groovy                                                       │   │
│  │                                                                  │   │
│  │  Recommendation: Java (beginners)                              │   │
│  └─────────────────────────────────────────────────────────────────┘   │
│                                                                         │
│  STEP 3: Spring Boot Version                                           │
│  ┌─────────────────────────────────────────────────────────────────┐   │
│  │  ○ 3.2.x (Latest Stable)                                        │   │
│  │  ○ 3.1.x                                                        │   │
│  │  ○ 3.0.x                                                        │   │
│  │                                                                  │   │
│  │  Recommendation: Use the latest stable version                 │   │
│  └─────────────────────────────────────────────────────────────────┘   │
│                                                                         │
│  STEP 4: Project Metadata                                               │
│  ┌─────────────────────────────────────────────────────────────────┐   │
│  │  Group: com.example                                            │   │
│  │  Artifact: my-spring-app                                       │   │
│  │  Name: my-spring-app                                           │   │
│  │  Description: My first Spring Boot application                 │   │
│  │  Package name: com.example.my-spring-app                       │   │
│  └─────────────────────────────────────────────────────────────────┘   │
│                                                                         │
│  STEP 5: Select Dependencies                                            │
│  ┌─────────────────────────────────────────────────────────────────┐   │
│  │  □ Spring Web          (REST APIs, MVC, Embedded Tomcat)     │   │
│  │  □ Spring Data JPA     (Database access with Hibernate)       │   │
│  │  □ H2 Database         (In-memory database for testing)        │   │
│  │  □ Spring Security     (Authentication and authorization)     │   │
│  │  □ Validation          (Bean Validation API)                  │   │
│  │  □ Spring Boot DevTools(Auto-restart, live reload)            │   │
│  └─────────────────────────────────────────────────────────────────┘   │
│                                                                         │
│  STEP 6: Generate and Download                                         │
│  ┌─────────────────────────────────────────────────────────────────┐   │
│  │  Click "GENERATE" button                                       │   │
│  │  Download: my-spring-app.zip                                   │   │
│  │  Extract to your project folder                                │   │
│  └─────────────────────────────────────────────────────────────────┘   │
│                                                                         │
└─────────────────────────────────────────────────────────────────────────┘
```

### Complete Project Creation Workflow

Let's create a complete project step by step:

#### 1. Using Spring Initializr (Web)

1. Open https://start.spring.io in your browser
2. Select:
   - Maven Project
   - Java
   - Spring Boot 3.2.x
3. Fill in metadata:
   - Group: `com.example`
   - Artifact: `demo`
   - Package name: `com.example.demo`
4. Add dependencies:
   - Spring Web
   - Spring Boot DevTools
5. Click "Generate"
6. Extract the ZIP file

#### 2. Using IntelliJ IDEA

1. File → New → Project
2. Select "Spring Initializr" on the left
3. Configure:
   - Name: demo
   - Location: Choose your folder
   - Type: Maven Project
   - Language: Java
   - Group: com.example
   - Artifact: demo
   - Package name: com.example.demo
4. Click "Next"
5. Add dependencies (Spring Web)
6. Click "Finish"

#### 3. Using VS Code

1. Press `Ctrl+Shift+P`
2. Type "Spring Initializr: Create a Maven Project"
3. Select Spring Boot version (3.2.x)
4. Enter Group ID: com.example
5. Enter Artifact ID: demo
6. Enter Package name: com.example.demo
7. Select dependencies (use arrow keys and space to select):
   - Spring Web
   - Spring Boot DevTools

### Project Structure

After creating your project, you'll see this structure:

```
demo/
├── src/
│   ├── main/
│   │   ├── java/
│   │   │   └── com/
│   │   │       └── example/
│   │   │           └── demo/
│   │   │               └── DemoApplication.java    ← Main class
│   │   │
│   │   └── resources/
│   │       └── application.properties               ← Config file
│   │
│   └── test/
│       └── java/
│           └── com/
│               └── example/
│                   └── demo/
│                       └── DemoApplicationTests.java
│
├── pom.xml                                         ← Maven config
└── mvnw / mvnw.cmd                                ← Maven wrapper
```

### Running and Verifying Spring Boot Application

#### Using Command Line (Maven):

```bash
# Navigate to project directory
cd demo

# Run the application
./mvnw spring-boot:run

# Or on Windows
mvnw.cmd spring-boot:run
```

#### Using IntelliJ IDEA:

1. Open the project in IntelliJ
2. Find `DemoApplication.java` in the project explorer
3. Right-click and select "Run 'DemoApplication'"
4. Or press `Shift+F10` to run the last configuration

#### Using VS Code:

1. Open the project folder
2. Go to the "Run and Debug" view (Ctrl+Shift+D)
3. Click "Run Spring Boot" or press F5

### Expected Output

When your application starts successfully, you'll see output like:

```
  .   ____          _            __ _ _
 /\\ / ___'_ __ _ _(_)_ __  __ _ \ \ \ \
( ( )\___ | '_ | '_| | '_ \/ _` | \ \ \ \
 \\/  ___)| |_)| | | | | || (_| |  ) ) ) )
  '  |____| .__|_| |_|_| |_\__, | / / / /
 =========|_|==============|___/=/_/_/_/
 :: Spring Boot ::                (3.2.1)

2024-01-15T10:30:00.123  INFO 12345 --- [           main] com.example.demo.DemoApplication
2024-01-15T10:30:00.125  INFO 12345 --- [           main] Starting DemoApplication...
2024-01-15T10:30:02.456  INFO 12345 --- [           main] Started DemoApplication in 2.345 seconds
2024-01-15T10:30:02.789  INFO 12345 --- [           main] Application is running on port 8080
```

### Testing Your Application

Open your browser and visit:
- http://localhost:8080 → Should show Spring Boot's default error page (no mapping)
- http://localhost:8080/actuator/health → Should show `{"status":"UP"}`

## Annotated Code Examples

### pom.xml - Maven Configuration

The `pom.xml` (Project Object Model) is the heart of Maven projects:

```xml
<?xml version="1.0" encoding="UTF-8"?>
<project xmlns="http://maven.apache.org/POM/4.0.0"
         xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
         xsi:schemaLocation="http://maven.apache.org/POM/4.0.0 
         https://maven.apache.org/xsd/maven-4.0.0.xsd">
    
    <!-- Model version - always 4.0.0 for Maven 2+ -->
    <modelVersion>4.0.0</modelVersion>
    
    <!-- Parent POM - inherits from Spring Boot's parent -->
    <parent>
        <groupId>org.springframework.boot</groupId>
        <artifactId>spring-boot-starter-parent</artifactId>
        <version>3.2.1</version>
        <relativePath/>
    </parent>
    
    <!-- Your project's coordinates -->
    <groupId>com.example</groupId>
    <artifactId>demo</artifactId>
    <version>0.0.1-SNAPSHOT</version>
    <name>demo</name>
    <description>My first Spring Boot application</description>
    
    <!-- Java version configuration -->
    <properties>
        <java.version>21</java.version>
    </properties>
    
    <!-- Dependencies - this is where you add libraries -->
    <dependencies>
        <!-- Spring Boot Web Starter - includes Spring MVC, Tomcat, Jackson -->
        <dependency>
            <groupId>org.springframework.boot</groupId>
            <artifactId>spring-boot-starter-web</artifactId>
        </dependency>
        
        <!-- Spring Boot DevTools - automatic restart -->
        <dependency>
            <groupId>org.springframework.boot</groupId>
            <artifactId>spring-boot-devtools</artifactId>
            <scope>runtime</scope>
            <optional>true</optional>
        </dependency>
        
        <!-- Test dependencies -->
        <dependency>
            <groupId>org.springframework.boot</groupId>
            <artifactId>spring-boot-starter-test</artifactId>
            <scope>test</scope>
        </dependency>
    </dependencies>
    
    <!-- Build configuration -->
    <build>
        <plugins>
            <!-- Spring Boot Maven plugin -->
            <plugin>
                <groupId>org.springframework.boot</groupId>
                <artifactId>spring-boot-maven-plugin</artifactId>
            </plugin>
        </plugins>
    </build>
</project>
```

### Main Application Class

```java
package com.example.demo;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;

/**
 * Main entry point for the Spring Boot application.
 * 
 * @SpringBootApplication is a convenience annotation that combines:
 * - @Configuration: Marks this class as a source of bean definitions
 * - @EnableAutoConfiguration: Tells Spring Boot to auto-configure the application
 * - @ComponentScan: Tells Spring to scan for components in this package and subpackages
 */
@SpringBootApplication
public class DemoApplication {

    public static void main(String[] args) {
        // SpringApplication.run() does all the magic:
        // 1. Creates an ApplicationContext
        // 2. Scans for @Component, @Service, @Repository, @Controller
        // 3. Auto-configures based on classpath and properties
        // 4. Starts embedded Tomcat server
        // 5. Deploys your web application
        SpringApplication.run(DemoApplication.class, args);
    }
}
```

### application.properties

The main configuration file for your Spring Boot application:

```properties
# ==================
# Server Configuration
# ==================
# Port the application runs on (default is 8080)
server.port=8080

# ==================
# Application Name
# ==================
spring.application.name=demo

# ==================
# Logging Configuration
# ==================
# Set root logging level
logging.level.root=INFO
# Set package-specific logging
logging.level.com.example.demo=DEBUG

# ==================
# Development Features
# ==================
# Enable hot swapping (DevTools)
spring.devtools.restart.enabled=true
# Show auto-configuration report
debug=false
```

## Industry Best Practices and Common Pitfalls

### Best Practices

1. **Use LTS Versions** - Always use Long Term Support versions of Java (Java 17, 21) for stability

2. **Configure Environment Variables** - Never hardcode paths; use environment variables

3. **Use Maven Wrapper** - Include mvnw in your project so others don't need to install Maven

4. **Keep Dependencies Updated** - Regularly update to latest stable Spring Boot versions

5. **Configure IDE Properly** - Enable annotation processing for Lombok, configure proper JDK

### Common Pitfalls

1. **Wrong JAVA_HOME** - Make sure JAVA_HOME points to JDK, not JRE

2. **Port Conflicts** - Port 8080 might be used; change in application.properties

3. **Firewall Blocking** - Windows Firewall might block Tomcat; allow access when prompted

4. **Missing Dependencies** - Forgetting to add dependencies in pom.xml

5. **Wrong Package Structure** - Keep main class in root package for component scanning

## Student Hands-On Exercises

### Exercise 1: Verify Java Installation (Easy)
Open a terminal and run:
- `java -version`
- `javac -version`
- `mvn -version`

Take a screenshot of the output showing all three commands.

### Exercise 2: Create Your First Project (Easy)
Use Spring Initializr to create a new Spring Boot project with:
- Group: com.yourname
- Artifact: hello-world
- Dependency: Spring Web

Import it into your IDE and verify it runs on port 8080.

### Exercise 3: Customize Configuration (Medium)
Modify your project's application.properties to:
- Change the server port to 9090
- Set the application name to "My First App"
- Enable debug logging

Verify the changes take effect.

### Exercise 4: Add Dependencies (Medium)
Add the following dependencies to your pom.xml and verify the application still runs:
- Spring Boot Starter Validation
- Lombok (remember to install the IDE plugin)

### Exercise 5: Explore Spring Initializr (Hard)
Research the Spring Initializr API and create a Spring Boot project using curl command line. Include:
- Spring Web
- Spring Data JPA
- H2 Database

Generate and download the project programmatically.

---

## Summary

In this lesson, you've learned:
- How to install and configure Java Development Kit (JDK)
- How to install and use Maven build tool
- How to set up IntelliJ IDEA or VS Code for Spring Boot development
- How to use Spring Initializr to generate Spring Boot projects
- How to run and verify your Spring Boot application
- The structure of a Spring Boot project

Your development environment is now ready! In the next lesson, we'll explore the Spring Boot project structure in detail.

---

**Next Lesson**: In the next lesson, we'll explore [SpringBoot Project Structure](04_SpringBoot_Project_Structure.md) and understand how all the pieces fit together.
