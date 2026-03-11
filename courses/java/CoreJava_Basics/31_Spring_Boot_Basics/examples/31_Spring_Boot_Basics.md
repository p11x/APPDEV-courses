# Spring Boot for Angular Developers

## Table of Contents
1. [Introduction to Spring Boot](#introduction-to-spring-boot)
2. [Spring Boot REST Controllers](#spring-boot-rest-controllers)
3. [Spring Data JPA](#spring-data-jpa)
4. [Dependency Injection](#dependency-injection)
5. [Spring Boot Application Properties](#spring-boot-application-properties)
6. [Building a REST API](#building-a-rest-api)
7. [Angular Integration](#angular-integration)

---

## 1. Introduction to Spring Boot

### What is Spring Boot?

Spring Boot makes it easy to create stand-alone, production-grade Spring-based applications.

```
┌─────────────────────────────────────────────────────────────┐
│                   SPRING BOOT OVERVIEW                        │
├─────────────────────────────────────────────────────────────┤
│                                                              │
│   Key Features:                                              │
│   ✓ Auto-configuration                                     │
│   ✓ Embedded servers (Tomcat, Jetty)                       │
│   ✓ Production-ready features                              │
│   ✓ Spring Initializr (project generator)                   │
│   ✓ Starter POMs (dependencies made easy)                  │
│                                                              │
│   Typical Project Structure:                                 │
│   ┌─────────────────────────────────────────────────┐      │
│   │ src/main/java/com/example/demo/                  │      │
│   │   ├── DemoApplication.java (main class)        │      │
│   │   ├── controller/                               │      │
│   │   ├── service/                                   │      │
│   │   ├── repository/                                │      │
│   │   └── model/                                     │      │
│   └─────────────────────────────────────────────────┘      │
│                                                              │
└─────────────────────────────────────────────────────────────┘
```

---

## 2. Spring Boot REST Controllers

### Creating a REST Controller

```java
@RestController
@RequestMapping("/api/users")
public class UserController {
    
    @GetMapping
    public List<User> getAllUsers() {
        return userService.findAll();
    }
    
    @GetMapping("/{id}")
    public User getUserById(@PathVariable Long id) {
        return userService.findById(id);
    }
    
    @PostMapping
    public User createUser(@RequestBody User user) {
        return userService.save(user);
    }
    
    @PutMapping("/{id}")
    public User updateUser(@PathVariable Long id, @RequestBody User user) {
        return userService.update(id, user);
    }
    
    @DeleteMapping("/{id}")
    public void deleteUser(@PathVariable Long id) {
        userService.delete(id);
    }
}
```

### HTTP Method Annotations

| Annotation | HTTP Method | Purpose |
|------------|------------|---------|
| @GetMapping | GET | Retrieve data |
| @PostMapping | POST | Create new resource |
| @PutMapping | PUT | Update existing resource |
| @PatchMapping | PATCH | Partial update |
| @DeleteMapping | DELETE | Delete resource |

---

## 3. Spring Data JPA

### Entity Classes

```java
@Entity
@Table(name = "users")
public class User {
    
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;
    
    @Column(nullable = false)
    private String name;
    
    @Column(unique = true)
    private String email;
    
    private int age;
    
    // Constructors, getters, setters
}
```

### Repository Interface

```java
public interface UserRepository extends JpaRepository<User, Long> {
    // Custom queries
    List<User> findByName(String name);
    
    @Query("SELECT u FROM User u WHERE u.email = ?1")
    User findByEmail(String email);
    
    List<User> findByAgeGreaterThan(int age);
}
```

---

## 4. Dependency Injection

### @Service and @Autowired

```java
@Service
public class UserService {
    
    @Autowired
    private UserRepository userRepository;
    
    public List<User> findAll() {
        return userRepository.findAll();
    }
    
    public User save(User user) {
        return userRepository.save(user);
    }
}
```

### Constructor Injection (Recommended)

```java
@Service
public class UserService {
    
    private final UserRepository userRepository;
    
    public UserService(UserRepository userRepository) {
        this.userRepository = userRepository;
    }
}
```

---

## 5. Spring Boot Application Properties

### application.properties

```properties
# Server
server.port=8080

# Database
spring.datasource.url=jdbc:mysql://localhost:3306/mydb
spring.datasource.username=root
spring.datasource.password=password

# JPA
spring.jpa.hibernate.ddl-auto=update
spring.jpa.show-sql=true

# CORS (for Angular)
server.cors.allowed-origins=http://localhost:4200
```

---

## 6. Building a REST API

### Complete Example

```java
package com.example.demo;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;

@SpringBootApplication
public class DemoApplication {
    public static void main(String[] args) {
        SpringApplication.run(DemoApplication.class, args);
    }
}

// ============ USER ENTITY ============
package com.example.demo.model;

import javax.persistence.*;

@Entity
@Table(name = "users")
public class User {
    
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;
    
    @Column(nullable = false)
    private String name;
    
    @Column(unique = true)
    private String email;
    
    private int age;
    
    // Constructors
    public User() {}
    
    public User(String name, String email, int age) {
        this.name = name;
        this.email = email;
        this.age = age;
    }
    
    // Getters and Setters
    public Long getId() { return id; }
    public void setId(Long id) { this.id = id; }
    
    public String getName() { return name; }
    public void setName(String name) { this.name = name; }
    
    public String getEmail() { return email; }
    public void setEmail(String email) { this.email = email; }
    
    public int getAge() { return age; }
    public void setAge(int age) { this.age = age; }
}

// ============ USER REPOSITORY ============
package com.example.demo.repository;

import com.example.demo.model.User;
import org.springframework.data.jpa.repository.JpaRepository;
import java.util.List;

public interface UserRepository extends JpaRepository<User, Long> {
    List<User> findByName(String name);
    List<User> findByAgeGreaterThan(int age);
}

// ============ USER SERVICE ============
package com.example.demo.service;

import com.example.demo.model.User;
import com.example.demo.repository.UserRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import java.util.List;
import java.util.Optional;

@Service
public class UserService {
    
    @Autowired
    private UserRepository userRepository;
    
    public List<User> findAll() {
        return userRepository.findAll();
    }
    
    public Optional<User> findById(Long id) {
        return userRepository.findById(id);
    }
    
    public User save(User user) {
        return userRepository.save(user);
    }
    
    public void delete(Long id) {
        userRepository.deleteById(id);
    }
}

// ============ USER CONTROLLER ============
package com.example.demo.controller;

import com.example.demo.model.User;
import com.example.demo.service.UserService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import java.util.List;

@RestController
@RequestMapping("/api/users")
@CrossOrigin(origins = "http://localhost:4200")  // Angular URL
public class UserController {
    
    @Autowired
    private UserService userService;
    
    // GET /api/users
    @GetMapping
    public List<User> getAllUsers() {
        return userService.findAll();
    }
    
    // GET /api/users/1
    @GetMapping("/{id}")
    public ResponseEntity<User> getUserById(@PathVariable Long id) {
        return userService.findById(id)
            .map(user -> ResponseEntity.ok(user))
            .orElse(ResponseEntity.notFound().build());
    }
    
    // POST /api/users
    @PostMapping
    public ResponseEntity<User> createUser(@RequestBody User user) {
        User saved = userService.save(user);
        return ResponseEntity.status(HttpStatus.CREATED).body(saved);
    }
    
    // PUT /api/users/1
    @PutMapping("/{id}")
    public ResponseEntity<User> updateUser(@PathVariable Long id, @RequestBody User user) {
        if (!userService.findById(id).isPresent()) {
            return ResponseEntity.notFound().build();
        }
        user.setId(id);
        return ResponseEntity.ok(userService.save(user));
    }
    
    // DELETE /api/users/1
    @DeleteMapping("/{id}")
    public ResponseEntity<Void> deleteUser(@PathVariable Long id) {
        if (!userService.findById(id).isPresent()) {
            return ResponseEntity.notFound().build();
        }
        userService.delete(id);
        return ResponseEntity.noContent().build();
    }
}
```

---

## 7. Angular Integration

### Angular Service

```typescript
// user.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface User {
  id?: number;
  name: string;
  email: string;
  age: number;
}

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private apiUrl = 'http://localhost:8080/api/users';

  constructor(private http: HttpClient) { }

  getUsers(): Observable<User[]> {
    return this.http.get<User[]>(this.apiUrl);
  }

  getUser(id: number): Observable<User> {
    return this.http.get<User>(`${this.apiUrl}/${id}`);
  }

  createUser(user: User): Observable<User> {
    return this.http.post<User>(this.apiUrl, user);
  }

  updateUser(id: number, user: User): Observable<User> {
    return this.http.put<User>(`${this.apiUrl}/${id}`, user);
  }

  deleteUser(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
```

### Angular Component

```typescript
// user-list.component.ts
import { Component, OnInit } from '@angular/core';
import { UserService, User } from './user.service';

@Component({
  selector: 'app-user-list',
  template: `
    <h2>Users</h2>
    <button (click)="loadUsers()">Refresh</button>
    <button (click)="showCreateForm = true">Add User</button>
    
    <div *ngFor="let user of users">
      {{ user.name }} - {{ user.email }}
      <button (click)="deleteUser(user.id!)">Delete</button>
    </div>
  `
})
export class UserListComponent implements OnInit {
  users: User[] = [];
  showCreateForm = false;

  constructor(private userService: UserService) { }

  ngOnInit() {
    this.loadUsers();
  }

  loadUsers() {
    this.userService.getUsers().subscribe(data => {
      this.users = data;
    });
  }

  deleteUser(id: number) {
    this.userService.deleteUser(id).subscribe(() => {
      this.loadUsers();
    });
  }
}
```

---

## Summary

### Spring Boot Key Concepts

1. **@SpringBootApplication** - Main class annotation
2. **@RestController** - REST API controller
3. **@Service** - Business logic layer
4. **@Repository** - Data access layer
5. **@Entity** - JPA entity class
6. **JpaRepository** - Database operations
7. **@CrossOrigin** - CORS for Angular
8. **@RequestBody/@ResponseBody** - JSON conversion

---

*Spring Boot Complete!*
