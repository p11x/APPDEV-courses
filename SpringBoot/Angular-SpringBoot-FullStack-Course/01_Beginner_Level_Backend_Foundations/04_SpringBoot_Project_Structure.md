# Spring Boot Project Structure

## Concept Title and Overview

Understanding the Spring Boot project structure is essential for navigating and organizing your code effectively. In this lesson, you'll learn how Maven organizes projects, what each directory does, and how all the components work together at startup.

## Real-World Importance and Context

Imagine walking into a well-organized library versus a chaotic one. In a well-organized library, you know exactly where to find books, magazines, and reference materials. Similarly, a well-structured Spring Boot project makes it easy to find and maintain code.

The Maven directory layout isn't arbitrary—it's a widely adopted standard that:
- Makes projects consistent across teams
- Enables tools to find and process files automatically
- Simplifies build and deployment processes

## Detailed Step-by-Step Explanation

### Maven Standard Directory Layout

Maven defines a standard directory structure that all projects follow:

```
 Maven┌─────────────────────────────────────────────────────────────────────────┐
│                     MAVEN STANDARD DIRECTORY LAYOUT                     │
├─────────────────────────────────────────────────────────────────────────┤
│                                                                         │
│  project-name/                                                         │
│  ├── pom.xml                    ← Project configuration (required)   │
│  │                                                                     │
│  ├── src/                       ← All source code goes here           │
│  │   ├── main/                  ← Main application code               │
│  │   │   ├── java/              ← Java source files                    │
│  │   │   │   └── com/           ← Package hierarchy starts here       │
│  │   │   │       └── example/   ← Your company/organization           │
│  │   │   │           └── app/  ← Your application name                │
│  │   │   │               ├── App.java                                │
│  │   │   │               ├── controller/   ← HTTP request handlers   │
│  │   │   │               ├── service/      ← Business logic          │
│  │   │   │               ├── repository/   ← Data access            │
│  │   │   │               ├── model/        ← Data entities          │
│  │   │   │               ├── dto/          ← Data transfer objects  │
│  │   │   │               └── config/       ← Configuration classes   │
│  │   │   │                                                         │
│  │   │   └── resources/            ← Non-Java resources              │
│  │   │       ├── application.properties  ← App configuration        │
│  │   │       ├── application.yml         ← Alt config (YAML)        │
│  │   │       ├── static/            ← Static files (CSS, JS, IMG)   │
│  │   │       └── templates/         ← Template files (Thymeleaf)   │
│  │   │                                                                  │
│  │   └── test/                     ← Test code                        │
│  │       ├── java/                 ← Test Java files                  │
│  │       │   └── com/                                              │
│  │       │       └── example/                                       │
│  │       │           └── app/                                      │
│  │       │               └── ... (same structure as main)           │
│  │       │                                                            │
│  │       └── resources/            ← Test resources                  │
│  │           └── application.properties  ← Test config              │
│  │                                                                     │
│  ├── target/                     ← Generated files (don't edit)       │
│  │   ├── classes/               ← Compiled .class files              │
│  │   ├── test-classes/          ← Compiled test classes              │
│  │   └── demo.jar               ← Final executable JAR              │
│  │                                                                     │
│  └── mvnw, mvnw.cmd             ← Maven wrapper scripts              │
│                                                                         │
└─────────────────────────────────────────────────────────────────────────┘
```

### The Purpose of src/main/java

This is where all your Java source code lives. The package structure follows Java conventions:

```
com.example.myapp/
├── MyApplication.java           ← Main class with @SpringBootApplication
│
├── controller/                  ← Controllers handle HTTP requests
│   ├── UserController.java
│   └── ProductController.java
│
├── service/                    ← Business logic lives here
│   ├── UserService.java
│   └── ProductService.java
│
├── repository/                 ← Data access layer
│   ├── UserRepository.java
│   └── ProductRepository.java
│
├── model/                      ← Domain entities
│   ├── User.java
│   └── Product.java
│
├── dto/                        ← Data Transfer Objects
│   ├── UserDto.java
│   └── ProductDto.java
│
└── config/                     ← Configuration classes
    ├── SecurityConfig.java
    └── WebConfig.java
```

### The Purpose of src/main/resources

This directory contains non-Java resources that your application needs:

```
resources/
├── application.properties      ← Primary configuration file
│
├── application.yml            ← Alternative to properties (supports hierarchy)
│
├── application-dev.properties← Environment-specific config
├── application-prod.properties│
│
├── static/                    ← Static web resources (served as-is)
│   ├── css/
│   │   └── styles.css
│   ├── js/
│   │   └── app.js
│   └── images/
│       └── logo.png
│
└── templates/                 ← Template files (for server-side rendering)
    ├── home.html
    └── user-form.html
```

### application.properties Configuration

This is your main configuration file. Here's what you can configure:

```properties
# ==================== Server Configuration ====================
server.port=8080                    # HTTP port
server.servlet.context-path=/api   # Context path
server.error.include-message=always# Show error messages

# ==================== Application Configuration ====================
spring.application.name=myapp
spring.profiles.active=dev          # Active profile

# ==================== DataSource Configuration ====================
spring.datasource.url=jdbc:mysql://localhost:3306/mydb
spring.datasource.username=root
spring.datasource.password=secret
spring.datasource.driver-class-name=com.mysql.cj.jdbc.Driver

# ==================== JPA/Hibernate Configuration ====================
spring.jpa.show-sql=true            # Show SQL in logs
spring.jpa.hibernate.ddl-auto=update # auto create/update tables
spring.jpa.properties.hibernate.format_sql=true

# ==================== Logging Configuration ====================
logging.level.root=INFO
logging.level.com.example.myapp=DEBUG
logging.pattern.console=%d{yyyy-MM-dd} %-5level %logger{36} - %msg%

# ==================== File Upload Configuration ====================
spring.servlet.multipart.max-file-size=10MB
spring.servlet.multipart.max-request-size=10MB
```

### pom.xml Dependency Management

The pom.xml manages all your project's dependencies:

```xml
<?xml version="1.0" encoding="UTF-8"?>
<project xmlns="http://maven.apache.org/POM/4.0.0"
         xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
         xsi:schemaLocation="http://maven.apache.org/POM/4.0.0 
         https://maven.apache.org/xsd/maven-4.0.0.xsd">
    
    <modelVersion>4.0.0</modelVersion>
    
    <!-- Inherit defaults from Spring Boot -->
    <parent>
        <groupId>org.springframework.boot</groupId>
        <artifactId>spring-boot-starter-parent</artifactId>
        <version>3.2.1</version>
        <relativePath/>
    </parent>
    
    <groupId>com.example</groupId>
    <artifactId>myapp</artifactId>
    <version>1.0.0</version>
    
    <!-- Properties - version variables -->
    <properties>
        <java.version>21</java.version>
        <project.build.sourceEncoding>UTF-8</project.build.sourceEncoding>
    </properties>
    
    <!-- Dependencies - the libraries your project needs -->
    <dependencies>
        <!-- Web starter - REST APIs -->
        <dependency>
            <groupId>org.springframework.boot</groupId>
            <artifactId>spring-boot-starter-web</artifactId>
        </dependency>
        
        <!-- Data JPA starter - Database access -->
        <dependency>
            <groupId>org.springframework.boot</groupId>
            <artifactId>spring-boot-starter-data-jpa</artifactId>
        </dependency>
        
        <!-- MySQL driver -->
        <dependency>
            <groupId>com.mysql</groupId>
            <artifactId>mysql-connector-j</artifactId>
            <scope>runtime</scope>
        </dependency>
        
        <!-- Validation starter -->
        <dependency>
            <groupId>org.springframework.boot</groupId>
            <artifactId>spring-boot-starter-validation</artifactId>
        </dependency>
        
        <!-- Security starter -->
        <dependency>
            <groupId>org.springframework.boot</groupId>
            <artifactId>spring-boot-starter-security</artifactId>
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
            <!-- Spring Boot Maven Plugin -->
            <plugin>
                <groupId>org.springframework.boot</groupId>
                <artifactId>spring-boot-maven-plugin</artifactId>
            </plugin>
        </plugins>
    </build>
</project>
```

### The Role of @SpringBootApplication

The main class is the entry point of your application:

```java
package com.example.myapp;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;

/**
 * Main application entry point.
 * 
 * @SpringBootApplication combines three annotations:
 * 
 * 1. @Configuration
 *    - Marks this class as a source of bean definitions
 *    - You can define @Bean methods here for Spring to manage
 *    - Similar to Spring XML configuration, but in Java
 * 
 * 2. @EnableAutoConfiguration
 *    - Tells Spring Boot to automatically configure your application
 *    - Based on dependencies in pom.xml
 *    - Example: If spring-boot-starter-web is present, configures Tomcat
 * 
 * 3. @ComponentScan
 *    - Tells Spring to scan for components in this package and subpackages
 *    - Finds @Component, @Service, @Repository, @Controller
 *    - This is why package structure matters!
 */
@SpringBootApplication
public class MyApplication {

    public static void main(String[] args) {
        // SpringApplication.run() does the heavy lifting:
        // 1. Creates an ApplicationContext (Spring container)
        // 2. Scans for @Component classes and creates beans
        // 3. Auto-configures based on classpath
        // 4. Starts embedded Tomcat server
        // 5. Deploys your web application
        SpringApplication.run(MyApplication.class, args);
    }
}
```

### How Components Work Together at Startup

Here's the complete startup process:

```
┌─────────────────────────────────────────────────────────────────────────┐
│                  SPRING BOOT STARTUP FLOW                              │
├─────────────────────────────────────────────────────────────────────────┤
│                                                                         │
│  1. MAIN METHOD EXECUTES                                                │
│     ┌─────────────────────────────────────────────────────────────┐   │
│     │ SpringApplication.run(MyApplication.class, args)           │   │
│     └─────────────────────────────────────────────────────────────┘   │
│                                │                                        │
│                                ▼                                        │
│  2. COMPONENT SCANNING                                                  │
│     ┌─────────────────────────────────────────────────────────────┐   │
│     │ Spring scans packages: com.example.myapp.*                  │   │
│     │                                                                 │   │
│     │ Finds:                                                         │   │
│     │ @Component  → UserController                                  │   │
│     │ @Service    → UserService                                     │   │
│     │ @Repository → UserRepository                                  │   │
│     │ @Component → WebConfig                                        │   │
│     └─────────────────────────────────────────────────────────────┘   │
│                                │                                        │
│                                ▼                                        │
│  3. AUTO-CONFIGURATION                                                  │
│     ┌─────────────────────────────────────────────────────────────┐   │
│     │ Based on classpath dependencies:                             │   │
│     │                                                                 │   │
│     │ • spring-boot-starter-web → Embedded Tomcat                  │   │
│     │ • spring-boot-starter-data-jpa → EntityManager               │   │
│     │ • spring-boot-starter-security → Security Filter Chain       │   │
│     │ • Database driver → DataSource                               │   │
│     └─────────────────────────────────────────────────────────────┘   │
│                                │                                        │
│                                ▼                                        │
│  4. BEAN CREATION                                                       │
│     ┌─────────────────────────────────────────────────────────────┐   │
│     │ Spring creates instances and injects dependencies:          │   │
│     │                                                                 │   │
│     │ UserController ◄──┐                                          │   │
│     │       │            │                                          │   │
│     │       ▼            │                                          │   │
│     │ UserService ◄──────┤                                          │   │
│     │       │            │                                          │   │
│     │       ▼            │                                          │   │
│     │ UserRepository ◄──┘                                          │   │
│     │       │                                                   │   │
│     │       ▼                                                   │   │
│     │ DataSource ◄───────────────────────────────────────────────┘   │
│     └─────────────────────────────────────────────────────────────┘   │
│                                │                                        │
│                                ▼                                        │
│  5. SERVER STARTUP                                                      │
│     ┌─────────────────────────────────────────────────────────────┐   │
│     │ Embedded Tomcat starts on port 8080                         │   │
│     │ Application is ready to accept HTTP requests                 │   │
│     │                                                                 │   │
│     │ Started MyApplication in 2.345 seconds                       │   │
│     └─────────────────────────────────────────────────────────────┘   │
│                                                                         │
└─────────────────────────────────────────────────────────────────────────┘
```

## Annotated Code Examples

### Example: Complete Layered Architecture

Let's see how all the pieces fit together with a User example:

**1. Model/Entity** (`src/main/java/com/example/myapp/model/User.java`)
```java
package com.example.myapp.model;

import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.Table;

/**
 * User entity - represents a database table.
 * 
 * @Entity tells JPA this class maps to a database table
 * @Table specifies the exact table name (optional if same as class name)
 * @Id marks the primary key
 * @GeneratedValue auto-generates the ID
 */
@Entity
@Table(name = "users")
public class User {
    
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;
    
    private String name;
    
    private String email;
    
    private String password;
    
    // Constructors
    public User() {}
    
    public User(String name, String email, String password) {
        this.name = name;
        this.email = email;
        this.password = password;
    }
    
    // Getters and Setters
    public Long getId() { return id; }
    public void setId(Long id) { this.id = id; }
    
    public String getName() { return name; }
    public void setName(String name) { this.name = name; }
    
    public String getEmail() { return email; }
    public void setEmail(String email) { this.email = email; }
    
    public String getPassword() { return password; }
    public void setPassword(String password) { this.password = password; }
}
```

**2. Repository** (`src/main/java/com/example/myapp/repository/UserRepository.java`)
```java
package com.example.myapp.repository;

import com.example.myapp.model.User;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.Optional;

/**
 * Repository - handles database operations.
 * 
 * @Repository marks this as a Spring-managed data access component
 * JpaRepository provides built-in CRUD methods
 * 
 * By extending JpaRepository<User, Long>:
 * - User is the entity type
 * - Long is the primary key type
 * - We get: findAll(), findById(), save(), delete() automatically
 */
@Repository
public interface UserRepository extends JpaRepository<User, Long> {
    
    // Spring Data JPA automatically implements this based on method name
    Optional<User> findByEmail(String email);
}
```

**3. Service** (`src/main/java/com/example/myapp/service/UserService.java`)
```java
package com.example.myapp.service;

import com.example.myapp.model.User;
import com.example.myapp.repository.UserRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.List;
import java.util.Optional;

/**
 * Service - contains business logic.
 * 
 * @Service marks this as a Spring-managed service component
 * This is where you'd add business rules, validation, etc.
 */
@Service
public class UserService {
    
    // Constructor injection (preferred over @Autowired on field)
    private final UserRepository userRepository;
    
    public UserService(UserRepository userRepository) {
        this.userRepository = userRepository;
    }
    
    public List<User> getAllUsers() {
        return userRepository.findAll();
    }
    
    public Optional<User> getUserById(Long id) {
        return userRepository.findById(id);
    }
    
    public User createUser(User user) {
        // Business logic could go here
        return userRepository.save(user);
    }
    
    public void deleteUser(Long id) {
        userRepository.deleteById(id);
    }
}
```

**4. Controller** (`src/main/java/com/example/myapp/controller/UserController.java`)
```java
package com.example.myapp.controller;

import com.example.myapp.model.User;
import com.example.myapp.service.UserService;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;

/**
 * Controller - handles HTTP requests and responses.
 * 
 * @RestController = @Controller + @ResponseBody
 * @RequestMapping defines the base URL for all endpoints
 */
@RestController
@RequestMapping("/api/users")
public class UserController {
    
    private final UserService userService;
    
    public UserController(UserService userService) {
        this.userService = userService;
    }
    
    // GET /api/users
    @GetMapping
    public List<User> getAllUsers() {
        return userService.getAllUsers();
    }
    
    // GET /api/users/1
    @GetMapping("/{id}")
    public ResponseEntity<User> getUserById(@PathVariable Long id) {
        return userService.getUserById(id)
                .map(ResponseEntity::ok)
                .orElse(ResponseEntity.notFound().build());
    }
    
    // POST /api/users
    @PostMapping
    public User createUser(@RequestBody User user) {
        return userService.createUser(user);
    }
    
    // DELETE /api/users/1
    @DeleteMapping("/{id}")
    public ResponseEntity<Void> deleteUser(@PathVariable Long id) {
        userService.deleteUser(id);
        return ResponseEntity.noContent().build();
    }
}
```

## Industry Best Practices and Common Pitfalls

### Best Practices

1. **Follow Package Conventions** - Keep the main class in the root package
   ```
   com.example.myapp/
   ├── MyApplication.java       ← In root package
   ├── controller/
   ├── service/
   └── repository/
   ```

2. **Use Constructor Injection** - It's more testable and explicit
   ```java
   // Good
   public UserService(UserRepository repository) {
       this.repository = repository;
   }
   ```

3. **Separate Concerns** - Keep controllers thin, business logic in services

4. **Name Things Clearly** - Use descriptive names: `UserService`, not `Service`

### Common Pitfalls

1. **Wrong Package Location** - If your main class is in `com.example`, Spring won't scan `com.other.package`

2. **Forgetting @ComponentScan** - It's automatic when using @SpringBootApplication

3. **Circular Dependencies** - Don't have A depend on B and B depend on A

4. **Missing @Transactional** - For database operations that modify data

## Student Hands-On Exercises

### Exercise 1: Map the Structure (Easy)
Draw the directory structure of a typical Spring Boot project from memory. Label each directory and explain what goes in it.

### Exercise 2: Create Package Structure (Easy)
Create a new Spring Boot project and manually create the following packages:
- controller
- service
- repository
- model
- dto
- config

### Exercise 3: Add Configuration (Medium)
Add the following to your application.properties:
- Server port 9090
- Application name "Employee Manager"
- Show SQL queries in console
- Log level DEBUG for your main package

### Exercise 4: Trace the Startup (Hard)
Add a breakpoint in your main application class and trace through what happens when you run the application. Document all the Spring beans that get created.

### Exercise 5: Add a New Layer (Hard)
Add a new "exception" package and create a custom exception class. Then modify your UserService to throw this exception when a user is not found. Update the controller to handle this exception.

---

## Summary

In this lesson, you've learned:
- Maven's standard directory layout
- The purpose of src/main/java and src/main/resources folders
- How application.properties configures your application
- The role of pom.xml in dependency management
- How @SpringBootApplication works
- The complete startup flow
- Layered architecture patterns

You now understand how Spring Boot projects are organized. In the next lesson, we'll create our first REST API!

---

**Next Lesson**: In the next lesson, we'll explore [Creating First REST API](05_Creating_First_REST_API.md) and build our first endpoint.
