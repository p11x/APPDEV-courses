# Dependency Injection

## Concept Title and Overview

In this lesson, you'll learn about one of Spring's most powerful features: Dependency Injection (DI). This fundamental concept is the backbone of Spring applications and understanding it will make you a much more effective backend developer.

## Real-World Importance and Context

Before we dive into the technical details, let's understand why dependency injection matters so much.

Think about building a car. In a traditional approach, the Car class would create its own Engine, Wheels, and Transmission. This creates tight coupling—you can't easily swap components or test individual parts.

Now imagine a car factory where you specify what engine, wheels, and transmission you want, and the factory assembles everything together. That's dependency injection!

In software terms, DI means:
- You declare what components you need
- Spring creates and manages them
- You receive ready-to-use components
- Easy to test and modify

## Detailed Step-by-Step Explanation

### The Problem: Tight Coupling

Here's what happens without dependency injection:

```java
// Without DI - tight coupling, hard to test
public class UserService {
    // UserService creates its own dependencies
    private UserRepository repository = new UserRepository();
    private EmailService emailService = new EmailService();
    
    public void createUser(User user) {
        repository.save(user);
        emailService.sendWelcomeEmail(user.getEmail());
    }
}
```

**Problems with this approach:**
1. Can't switch to a different repository implementation
2. Can't test without a real database
3. Hard to change email service
4. Violates Single Responsibility Principle

### The Solution: Dependency Injection

```java
// With DI - loose coupling, easy to test
public class UserService {
    // Dependencies are "injected" from outside
    private final UserRepository repository;
    private final EmailService emailService;
    
    // Constructor injection - the cleanest approach
    public UserService(UserRepository repository, EmailService emailService) {
        this.repository = repository;
        this.emailService = emailService;
    }
    
    public void createUser(User user) {
        repository.save(user);
        emailService.sendWelcomeEmail(user.getEmail());
    }
}
```

**Benefits:**
1. Easy to swap implementations
2. Easy to test with mock objects
3. Clear dependencies
4. Follows SOLID principles

### Spring's Dependency Injection Container

The Spring container is like a central registry that manages all your application's components:

```
┌─────────────────────────────────────────────────────────────────────────┐
│              SPRING DEPENDENCY INJECTION CONTAINER                      │
├─────────────────────────────────────────────────────────────────────────┤
│                                                                         │
│   Your Code declares what it needs                                     │
│                                                                         │
│   ┌─────────────────────────────┐                                      │
│   │  @Component                │                                      │
│   │  public class UserService {│                                      │
│   │    public UserService(     │ ◄─── "I need UserRepository         │
│   │       UserRepository repo │                                      │
│   │    ) { ... }               │                                      │
│   │  }                         │                                      │
│   └──────────────┬──────────────┘                                      │
│                  │                                                      │
│                  ▼                                                      │
│   ┌──────────────────────────────────────────┐                         │
│   │        SPRING CONTAINER                   │                         │
│   │                                          │                         │
│   │   ┌─────────────────────────────┐       │                         │
│   │   │  Bean Registry              │       │                         │
│   │   │                             │       │                         │
│   │   │  UserRepository ──────►   │       │                         │
│   │   │  UserService         ◄───┘       │                         │
│   │   │                             │       │                         │
│   │   │  EmailService ──────────►  │       │                         │
│   │   │  UserService         ◄───┘       │                         │
│   │   └─────────────────────────────┘       │                         │
│   │                                          │                         │
│   │   Spring creates and injects all beans  │                         │
│   └──────────────┬──────────────────────────┘                        │
│                  │                                                      │
│                  ▼                                                      │
│   Your class receives ready-to-use objects                            │
│                                                                         │
└─────────────────────────────────────────────────────────────────────────┘
```

### Understanding Stereotype Annotations

Spring provides different annotations for different types of components:

```
┌─────────────────────────────────────────────────────────────────────────┐
│                   SPRING STEREOTYPE ANNOTATIONS                        │
├─────────────────────────────────────────────────────────────────────────┤
│                                                                         │
│  @Component                                                            │
│  ┌─────────────────────────────────────────────────────────────────┐   │
│  │ Generic Spring-managed component                                 │   │
│  │ Use for: Utility classes, helper classes                        │   │
│  │                                                                 │   │
│  │ Example: @Component public class DateUtils { ... }             │   │
│  └─────────────────────────────────────────────────────────────────┘   │
│                                                                         │
│  @Service                                                              │
│  ┌─────────────────────────────────────────────────────────────────┐   │
│  │ Business logic layer                                            │   │
│  │ Use for: Service classes containing business logic              │   │
│  │                                                                 │   │
│  │ Example: @Service public class UserService { ... }             │   │
│  └─────────────────────────────────────────────────────────────────┘   │
│                                                                         │
│  @Repository                                                           │
│  ┌─────────────────────────────────────────────────────────────────┐   │
│  │ Data access layer                                               │   │
│  │ Use for: Repository classes, DAO classes                        │   │
│  │ Includes automatic exception translation                        │   │
│  │                                                                 │   │
│  │ Example: @Repository public class UserDao { ... }             │   │
│  └─────────────────────────────────────────────────────────────────┘   │
│                                                                         │
│  @Controller / @RestController                                         │
│  ┌─────────────────────────────────────────────────────────────────┐   │
│  │ Presentation layer (web controllers)                           │   │
│  │ Use for: Handling HTTP requests                                │   │
│  │                                                                 │   │
│  │ Example: @RestController public class UserController { ... }   │   │
│  └─────────────────────────────────────────────────────────────────┘   │
│                                                                         │
│  ALL ARE SPECIALIZATIONS OF @Component!                               │
│                                                                         │
└─────────────────────────────────────────────────────────────────────────┘
```

### Constructor Injection vs Setter Injection

**Constructor Injection (Recommended):**

```java
@Service
public class UserService {
    private final UserRepository userRepository;
    private final EmailService emailService;
    
    // All dependencies required via constructor
    public UserService(UserRepository userRepository, EmailService emailService) {
        this.userRepository = userRepository;
        this.emailService = emailService;
    }
}
```

**Advantages:**
- Dependencies are clearly visible
- Cannot create object without dependencies (immutable)
- Easy to test with mocks
- Preferred by most developers

**Setter Injection (Optional):**

```java
@Service
public class UserService {
    private UserRepository userRepository;
    private EmailService emailService;
    
    // Dependencies are optional, set via setters
    @Autowired
    public void setUserRepository(UserRepository userRepository) {
        this.userRepository = userRepository;
    }
    
    @Autowired
    public void setEmailService(EmailService emailService) {
        this.emailService = emailService;
    }
}
```

**When to use setter injection:**
- Optional dependencies
- For flexibility in configuration
- When you need to reconfigure at runtime

### The Layered Architecture Pattern

Spring applications typically follow a layered architecture:

```
┌─────────────────────────────────────────────────────────────────────────┐
│                    LAYERED ARCHITECTURE                                  │
├─────────────────────────────────────────────────────────────────────────┤
│                                                                         │
│  ┌─────────────────────────────────────────────────────────────────┐   │
│  │                    CONTROLLER LAYER                             │   │
│  │  @RestController                                               │   │
│  │  - Handles HTTP requests                                       │   │
│  │  - Input validation                                            │   │
│  │  - Calls service layer                                         │   │
│  │  - Returns HTTP responses                                      │   │
│  └─────────────────────────────┬───────────────────────────────────┘   │
│                                │                                         │
│                                ▼                                         │
│  ┌─────────────────────────────────────────────────────────────────┐   │
│  │                    SERVICE LAYER                               │   │
│  │  @Service                                                      │   │
│  │  - Business logic                                              │   │
│  │  - Transaction management                                      │   │
│  │  - Coordinates operations                                      │   │
│  │  - Calls repository layer                                      │   │
│  └─────────────────────────────┬───────────────────────────────────┘   │
│                                │                                         │
│                                ▼                                         │
│  ┌─────────────────────────────────────────────────────────────────┐   │
│  │                  REPOSITORY LAYER                              │   │
│  │  @Repository                                                   │   │
│  │  - Data access                                                 │   │
│  │  - Database queries                                            │   │
│  │  - CRUD operations                                             │   │
│  │  - Returns domain objects                                      │   │
│  └─────────────────────────────┬───────────────────────────────────┘   │
│                                │                                         │
│                                ▼                                         │
│  ┌─────────────────────────────────────────────────────────────────┐   │
│  │                    DATABASE                                    │   │
│  │  - Stores data                                                 │   │
│  │  - MySQL, PostgreSQL, H2, etc.                                │   │
│  └─────────────────────────────────────────────────────────────────┘   │
│                                                                         │
└─────────────────────────────────────────────────────────────────────────┘
```

## Practical Examples

### Complete Layered Implementation

**1. The Entity (Model)**

```java
package com.example.demo.model;

import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.Table;

@Entity
@Table(name = "users")
public class User {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;
    
    private String name;
    private String email;
    private String role;
    private boolean active;
    
    // Constructors, getters, setters
    public User() {}
    
    public User(String name, String email, String role) {
        this.name = name;
        this.email = email;
        this.role = role;
        this.active = true;
    }
    
    // Getters and Setters
    public Long getId() { return id; }
    public void setId(Long id) { this.id = id; }
    
    public String getName() { return name; }
    public void setName(String name) { this.name = name; }
    
    public String getEmail() { return email; }
    public void setEmail(String email) { this.email = email; }
    
    public String getRole() { return role; }
    public void setRole(String role) { this.role = role; }
    
    public boolean isActive() { return active; }
    public void setActive(boolean active) { this.active = active; }
}
```

**2. The Repository**

```java
package com.example.demo.repository;

import com.example.demo.model.User;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.List;
import java.util.Optional;

@Repository
public interface UserRepository extends JpaRepository<User, Long> {
    // Spring Data JPA automatically implements these methods
    // Based on the method naming convention
    
    Optional<User> findByEmail(String email);
    
    List<User> findByRole(String role);
    
    List<User> findByActive(boolean active);
    
    List<User> findByRoleAndActive(String role, boolean active);
}
```

**3. The Service**

```java
package com.example.demo.service;

import com.example.demo.model.User;
import com.example.demo.repository.UserRepository;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.List;
import java.util.Optional;

/**
 * Service layer - contains business logic.
 * 
 * @Service marks this as a Spring-managed service component
 */
@Service
@Transactional  // Ensures all methods run within a database transaction
public class UserService {
    
    // Constructor injection - recommended approach
    private final UserRepository userRepository;
    
    public UserService(UserRepository userRepository) {
        this.userRepository = userRepository;
    }
    
    /**
     * Get all users from database.
     * Read-only transaction for better performance.
     */
    @Transactional(readOnly = true)
    public List<User> getAllUsers() {
        return userRepository.findAll();
    }
    
    /**
     * Get user by ID.
     */
    @Transactional(readOnly = true)
    public Optional<User> getUserById(Long id) {
        return userRepository.findById(id);
    }
    
    /**
     * Create a new user.
     */
    public User createUser(String name, String email, String role) {
        // Business logic can go here
        if (userRepository.findByEmail(email).isPresent()) {
            throw new IllegalArgumentException("Email already exists: " + email);
        }
        
        User user = new User(name, email, role);
        return userRepository.save(user);
    }
    
    /**
     * Update existing user.
     */
    public Optional<User> updateUser(Long id, String name, String email, boolean active) {
        return userRepository.findById(id)
            .map(user -> {
                user.setName(name);
                user.setEmail(email);
                user.setActive(active);
                return userRepository.save(user);
            });
    }
    
    /**
     * Delete user by ID.
     */
    public boolean deleteUser(Long id) {
        if (userRepository.existsById(id)) {
            userRepository.deleteById(id);
            return true;
        }
        return false;
    }
}
```

**4. The Controller**

```java
package com.example.demo.controller;

import com.example.demo.model.User;
import com.example.demo.service.UserService;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;

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
    public ResponseEntity<User> createUser(
            @RequestParam String name,
            @RequestParam String email,
            @RequestParam String role
    ) {
        try {
            User user = userService.createUser(name, email, role);
            return ResponseEntity.status(HttpStatus.CREATED).body(user);
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().build();
        }
    }
    
    // PUT /api/users/1
    @PutMapping("/{id}")
    public ResponseEntity<User> updateUser(
            @PathVariable Long id,
            @RequestParam String name,
            @RequestParam String email,
            @RequestParam boolean active
    ) {
        return userService.updateUser(id, name, email, active)
                .map(ResponseEntity::ok)
                .orElse(ResponseEntity.notFound().build());
    }
    
    // DELETE /api/users/1
    @DeleteMapping("/{id}")
    public ResponseEntity<Void> deleteUser(@PathVariable Long id) {
        if (userService.deleteUser(id)) {
            return ResponseEntity.noContent().build();
        }
        return ResponseEntity.notFound().build();
    }
}
```

## Angular Service Integration

Here's how Angular consumes this backend:

```typescript
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface User {
  id: number;
  name: string;
  email: string;
  role: string;
  active: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class UserApiService {
  private apiUrl = 'http://localhost:8080/api/users';

  constructor(private http: HttpClient) {}

  // GET /api/users
  getAllUsers(): Observable<User[]> {
    return this.http.get<User[]>(this.apiUrl);
  }

  // GET /api/users/1
  getUserById(id: number): Observable<User> {
    return this.http.get<User>(`${this.apiUrl}/${id}`);
  }

  // POST /api/users
  createUser(name: string, email: string, role: string): Observable<User> {
    return this.http.post<User>(this.apiUrl, null, {
      params: { name, email, role }
    });
  }

  // PUT /api/users/1
  updateUser(id: number, name: string, email: string, active: boolean): Observable<User> {
    return this.http.put<User>(`${this.apiUrl}/${id}`, null, {
      params: { name, email, active: active.toString() }
    });
  }

  // DELETE /api/users/1
  deleteUser(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
```

## Industry Best Practices and Common Pitfalls

### Best Practices

1. **Always use constructor injection** - It's explicit and testable

2. **Use @Service for business logic** - Better semantic meaning than @Component

3. **Keep controllers thin** - They should only handle HTTP, not business logic

4. **Use interfaces for repositories** - Enables easy testing and switching implementations

5. **One public method per transaction** - In service layer

### Common Pitfalls

1. **Circular dependencies** - A needs B, B needs A (Spring will error)

2. **Forgetting @Transactional** - Database operations may not work correctly

3. **Using @Autowired on fields** - Makes testing difficult

4. **Mixing responsibilities** - Don't put business logic in controllers

5. **Not handling exceptions** - Service should throw, controller should catch

## Student Hands-On Exercises

### Exercise 1: Identify Dependencies (Easy)
For the following class, identify what dependencies it should receive via constructor injection:

```java
public class OrderService {
    private OrderDao orderDao;
    private CustomerDao customerDao;
    private EmailService emailService;
    private PaymentService paymentService;
    private InventoryService inventoryService;
    
    // Add constructor
}
```

### Exercise 2: Create a Service (Medium)
Create a ProductService with:
- ProductRepository dependency (you'll create the interface)
- Methods: getAllProducts(), getProductById(), createProduct(), deleteProduct()
- Add business logic: product price must be positive

### Exercise 3: Add a Layer (Medium)
Extend the user system by adding:
- A new DTO (Data Transfer Object) for user creation
- Update the controller to use the DTO instead of @RequestParam
- Add validation

### Exercise 4: Constructor vs Setter (Hard)
Create two versions of a service:
- One using constructor injection
- One using setter injection

Explain when you would use each.

### Exercise 5: Architecture Design (Hard)
Design a complete system for a Library Management System with:
- Book, Member, Loan entities
- Appropriate repositories
- Services with business logic
- Controllers for REST API

Document the dependencies between layers.

---

## Summary

In this lesson, you've learned:
- The problem of tight coupling and why it matters
- How dependency injection solves the problem
- Spring's DI container and how it works
- The different stereotype annotations (@Component, @Service, @Repository, @RestController)
- Constructor injection vs setter injection
- The layered architecture pattern
- Complete implementation examples

You now understand the foundation of Spring applications. In the next lesson, we'll explore Spring Data JPA for database operations.

---

**Next Lesson**: In the next lesson, we'll explore [Spring Data JPA](09_Spring_Data_JPA.md) and learn how to work with databases in Spring Boot.
