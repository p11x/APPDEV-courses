# Spring Data JPA

## Concept Title and Overview

In this lesson, you'll learn about Spring Data JPA, which makes database operations remarkably simple. JPA (Java Persistence API) is the standard way to work with databases in Java, and Spring Data JPA makes it even easier by providing ready-to-use implementations.

## Real-World Importance and Context

Every application needs to store and retrieve data. Without a database layer, your application would lose all data when it restarts.

Think of JPA as a translator between your Java code and the database. You speak Java (object-oriented), and the database speaks SQL (relational). JPA handles the translation between these two worlds.

## Detailed Step-by-Step Explanation

### Object-Relational Mapping (ORM) Concepts

ORM solves the "impedance mismatch" between object-oriented programming and relational databases:

```
┌─────────────────────────────────────────────────────────────────────────┐
│                    OBJECT vs RELATIONAL WORLD                           │
├─────────────────────────────────────────────────────────────────────────┤
│                                                                         │
│  JAVA OBJECTS                    RELATIONAL DATABASE                   │
│  ┌─────────────────┐           ┌─────────────────────┐                │
│  │    User         │           │     users           │                │
│  │  ┌───────────┐  │    ══►    │  ┌───────────────┐  │                │
│  │  │ id: Long  │  │           │  │ id (PK)       │  │                │
│  │  │ name:     │  │           │  │ name          │  │                │
│  │  │   String  │  │           │  │ email         │  │                │
│  │  │ email:    │  │           │  │ role          │  │                │
│  │  │   String  │  │           │  │ active        │  │                │
│  │  └───────────┘  │           │  └───────────────┘  │                │
│  └─────────────────┘           └─────────────────────┘                │
│                                                                         │
│  OBJECTS                                TABLES                         │
│  - Classes = Tables                    - Fields = Columns             │
│  - Instances = Rows                    - References = Foreign Keys   │
│  - Properties = Columns                - Methods = N/A                │
│                                                                         │
└─────────────────────────────────────────────────────────────────────────┘
```

### Hibernate as the JPA Implementation

Spring Data JPA uses Hibernate as its default implementation:

```
┌─────────────────────────────────────────────────────────────────────────┐
│                    JPA/HIBERNATE STACK                                  │
├─────────────────────────────────────────────────────────────────────────┤
│                                                                         │
│   Your Code                                                            │
│        │                                                               │
│        ▼                                                               │
│   ┌─────────────────────────────────────────────────────────────────┐  │
│   │              SPRING DATA JPA                                    │  │
│   │  • Repository interfaces                                       │  │
│   │  • Query method derivation                                     │  │
│   │  • Custom repository implementations                           │  │
│   └─────────────────────────┬───────────────────────────────────────┘  │
│                             │                                            │
│                             ▼                                            │
│   ┌─────────────────────────────────────────────────────────────────┐  │
│   │              JPA (Java Persistence API)                         │  │
│   │  • EntityManager                                               │  │
│   │  • JPQL queries                                                │  │
│   │  • Criteria API                                                │  │
│   └─────────────────────────┬───────────────────────────────────────┘  │
│                             │                                            │
│                             ▼                                            │
│   ┌─────────────────────────────────────────────────────────────────┐  │
│   │              HIBERNATE (Implementation)                         │  │
│   │  • SQL generation                                             │  │
│   │  • Connection pooling                                          │  │
│   │  • Caching                                                     │  │
│   └─────────────────────────┬───────────────────────────────────────┘  │
│                             │                                            │
│                             ▼                                            │
│   ┌─────────────────────────────────────────────────────────────────┐  │
│   │              DATABASE (MySQL, PostgreSQL, etc.)               │  │
│   └─────────────────────────────────────────────────────────────────┘  │
│                                                                         │
└─────────────────────────────────────────────────────────────────────────┘
```

### Essential JPA Annotations

Here's an overview of the key annotations:

```
┌─────────────────────────────────────────────────────────────────────────┐
│                    JPA ANNOTATIONS                                      │
├─────────────────────────────────────────────────────────────────────────┤
│                                                                         │
│  @Entity                                                               │
│  ┌─────────────────────────────────────────────────────────────────┐   │
│  │ Marks a class as a JPA entity (maps to a database table)       │   │
│  │ Placement: Class declaration                                     │   │
│  │                                                                 │   │
│  │ @Entity                                                         │   │
│  │ public class User { }                                         │   │
│  └─────────────────────────────────────────────────────────────────┘   │
│                                                                         │
│  @Id                                                                  │
│  ┌─────────────────────────────────────────────────────────────────┐   │
│  │ Marks the primary key field                                     │   │
│  │ Placement: Field declaration                                    │   │
│  │                                                                 │   │
│  │ @Id                                                             │   │
│  │ private Long id;                                               │   │
│  └─────────────────────────────────────────────────────────────────┘   │
│                                                                         │
│  @GeneratedValue                                                      │
│  ┌─────────────────────────────────────────────────────────────────┐   │
│  │ Configures primary key generation strategy                     │   │
│  │                                                                 │   │
│  │ @Id                                                             │   │
│  │ @GeneratedValue(strategy = GenerationType.IDENTITY)           │   │
│  │ private Long id;                                               │   │
│  │                                                                 │   │
│  │ Strategies: IDENTITY, SEQUENCE, TABLE, AUTO                    │   │
│  └─────────────────────────────────────────────────────────────────┘   │
│                                                                         │
│  @Table                                                               │
│  ┌─────────────────────────────────────────────────────────────────┐   │
│  │ Specifies the table name (optional if class name matches)      │   │
│  │                                                                 │   │
│  │ @Entity                                                         │   │
│  │ @Table(name = "app_users")                                     │   │
│  │ public class User { }                                         │   │
│  └─────────────────────────────────────────────────────────────────┘   │
│                                                                         │
│  @Column                                                              │
│  ┌─────────────────────────────────────────────────────────────────┐   │
│  │ Configures column mapping (optional if field name matches)      │   │
│  │                                                                 │   │
│  │ @Column(name = "user_email", nullable = false, length = 100)  │   │
│  │ private String email;                                         │   │
│  │                                                                 │   │
│  │ Options: name, nullable, length, unique, precision, scale       │   │
│  └─────────────────────────────────────────────────────────────────┘   │
│                                                                         │
└─────────────────────────────────────────────────────────────────────────┘
```

### Entity to Database Table Mapping

Here's how an entity maps to a database table:

```java
package com.example.demo.model;

import jakarta.persistence.*;
import java.time.LocalDateTime;

@Entity
@Table(name = "users")
public class User {
    
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;
    
    @Column(nullable = false, length = 100)
    private String name;
    
    @Column(unique = true, nullable = false)
    private String email;
    
    @Column(length = 50)
    private String role;
    
    @Column(name = "is_active")
    private boolean active;
    
    @Column(name = "created_at")
    private LocalDateTime createdAt;
    
    @Column(name = "updated_at")
    private LocalDateTime updatedAt;
    
    // Default constructor (required by JPA)
    public User() {}
    
    // Parameterized constructor
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
    
   () { return email public String getEmail; }
    public void setEmail(String email) { this.email = email; }
    
    public String getRole() { return role; }
    public void setRole(String role) { this.role = role; }
    
    public boolean isActive() { return active; }
    public void setActive(boolean active) { this.active = active; }
    
    public LocalDateTime getCreatedAt() { return createdAt; }
    public void setCreatedAt(LocalDateTime createdAt) { this.createdAt = createdAt; }
    
    public LocalDateTime getUpdatedAt() { return updatedAt; }
    public void setUpdatedAt(LocalDateTime updatedAt) { this.updatedAt = updatedAt; }
}
```

This maps to:

```sql
CREATE TABLE users (
    id BIGINT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    email VARCHAR(255) NOT NULL UNIQUE,
    role VARCHAR(50),
    is_active BOOLEAN DEFAULT TRUE,
    created_at TIMESTAMP,
    updated_at TIMESTAMP
);
```

## Complete Implementation Example

### 1. Add Dependencies to pom.xml

```xml
<!-- Spring Data JPA -->
<dependency>
    <groupId>org.springframework.boot</groupId>
    <artifactId>spring-boot-starter-data-jpa</artifactId>
</dependency>

<!-- H2 Database (for development/testing) -->
<dependency>
    <groupId>com.h2database</groupId>
    <artifactId>h2</artifactId>
    <scope>runtime</scope>
</dependency>

<!-- MySQL Driver (for production) -->
<dependency>
    <groupId>com.mysql</groupId>
    <artifactId>mysql-connector-j</artifactId>
    <scope>runtime</scope>
</dependency>
```

### 2. Configure application.properties

```properties
# H2 Database (development)
spring.datasource.url=jdbc:h2:mem:testdb
spring.datasource.driverClassName=org.h2.Driver
spring.datasource.username=sa
spring.datasource.password=

# JPA/Hibernate
spring.jpa.database-platform=org.hibernate.dialect.H2Dialect
spring.jpa.hibernate.ddl-auto=create-drop
spring.jpa.show-sql=true
spring.jpa.properties.hibernate.format_sql=true
```

### 3. Create the Entity

```java
package com.example.demo.model;

import jakarta.persistence.*;
import java.time.LocalDateTime;

@Entity
@Table(name = "employees")
public class Employee {
    
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;
    
    @Column(nullable = false)
    private String name;
    
    @Column(unique = true, nullable = false)
    private String email;
    
    @Column
    private String department;
    
    @Column
    private Double salary;
    
    @Column(name = "created_at")
    private LocalDateTime createdAt;
    
    @PrePersist  // Called before first save
    protected void onCreate() {
        createdAt = LocalDateTime.now();
    }
    
    // Constructors
    public Employee() {}
    
    public Employee(String name, String email, String department, Double salary) {
        this.name = name;
        this.email = email;
        this.department = department;
        this.salary = salary;
    }
    
    // Getters and Setters
    public Long getId() { return id; }
    public void setId(Long id) { this.id = id; }
    
    public String getName() { return name; }
    public void setName(String name) { this.name = name; }
    
    public String getEmail() { return email; }
    public void setEmail(String email) { this.email = email; }
    
    public String getDepartment() { return department; }
    public void setDepartment(String department) { this.department = department; }
    
    public Double getSalary() { return salary; }
    public void setSalary(Double salary) { this.salary = salary; }
    
    public LocalDateTime getCreatedAt() { return createdAt; }
    public void setCreatedAt(LocalDateTime createdAt) { this.createdAt = createdAt; }
}
```

### 4. Create the Repository

```java
package com.example.demo.repository;

import com.example.demo.model.Employee;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.List;
import java.util.Optional;

@Repository
public interface EmployeeRepository extends JpaRepository<Employee, Long> {
    
    // Spring Data JPA automatically implements these methods
    // based on the method name!
    
    // Find by email - returns Optional (might not exist)
    Optional<Employee> findByEmail(String email);
    
    // Find all employees in a department
    List<Employee> findByDepartment(String department);
    
    // Find employees with salary greater than
    List<Employee> findBySalaryGreaterThan(Double salary);
    
    // Find employees with salary between
    List<Employee> findBySalaryBetween(Double min, Double max);
    
    // Find by department and active
    List<Employee> findByDepartmentAndSalaryGreaterThan(String department, Double salary);
}
```

### 5. Complete CRUD Service

```java
package com.example.demo.service;

import com.example.demo.model.Employee;
import com.example.demo.repository.EmployeeRepository;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.List;
import java.util.Optional;

@Service
@Transactional
public class EmployeeService {
    
    private final EmployeeRepository employeeRepository;
    
    public EmployeeService(EmployeeRepository employeeRepository) {
        this.employeeRepository = employeeRepository;
    }
    
    // CREATE
    public Employee createEmployee(Employee employee) {
        // Check if email already exists
        if (employeeRepository.findByEmail(employee.getEmail()).isPresent()) {
            throw new IllegalArgumentException("Email already exists: " + employee.getEmail());
        }
        return employeeRepository.save(employee);
    }
    
    // READ ALL
    @Transactional(readOnly = true)
    public List<Employee> getAllEmployees() {
        return employeeRepository.findAll();
    }
    
    // READ ONE
    @Transactional(readOnly = true)
    public Optional<Employee> getEmployeeById(Long id) {
        return employeeRepository.findById(id);
    }
    
    // READ BY DEPARTMENT
    @Transactional(readOnly = true)
    public List<Employee> getEmployeesByDepartment(String department) {
        return employeeRepository.findByDepartment(department);
    }
    
    // UPDATE
    public Optional<Employee> updateEmployee(Long id, Employee updatedEmployee) {
        return employeeRepository.findById(id)
            .map(employee -> {
                employee.setName(updatedEmployee.getName());
                employee.setEmail(updatedEmployee.getEmail());
                employee.setDepartment(updatedEmployee.getDepartment());
                employee.setSalary(updatedEmployee.getSalary());
                return employeeRepository.save(employee);
            });
    }
    
    // DELETE
    public boolean deleteEmployee(Long id) {
        if (employeeRepository.existsById(id)) {
            employeeRepository.deleteById(id);
            return true;
        }
        return false;
    }
    
    // Custom queries
    @Transactional(readOnly = true)
    public List<Employee> getHighEarners(Double minSalary) {
        return employeeRepository.findBySalaryGreaterThan(minSalary);
    }
}
```

### 6. Complete Controller

```java
package com.example.demo.controller;

import com.example.demo.model.Employee;
import com.example.demo.service.EmployeeService;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("/api/employees")
public class EmployeeController {
    
    private final EmployeeService employeeService;
    
    public EmployeeController(EmployeeService employeeService) {
        this.employeeService = employeeService;
    }
    
    @GetMapping
    public List<Employee> getAllEmployees() {
        return employeeService.getAllEmployees();
    }
    
    @GetMapping("/{id}")
    public ResponseEntity<Employee> getEmployeeById(@PathVariable Long id) {
        return employeeService.getEmployeeById(id)
                .map(ResponseEntity::ok)
                .orElse(ResponseEntity.notFound().build());
    }
    
    @GetMapping("/department/{department}")
    public List<Employee> getEmployeesByDepartment(@PathVariable String department) {
        return employeeService.getEmployeesByDepartment(department);
    }
    
    @PostMapping
    public ResponseEntity<Employee> createEmployee(@RequestBody Employee employee) {
        try {
            Employee created = employeeService.createEmployee(employee);
            return ResponseEntity.status(HttpStatus.CREATED).body(created);
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().build();
        }
    }
    
    @PutMapping("/{id}")
    public ResponseEntity<Employee> updateEmployee(
            @PathVariable Long id,
            @RequestBody Employee employee
    ) {
        return employeeService.updateEmployee(id, employee)
                .map(ResponseEntity::ok)
                .orElse(ResponseEntity.notFound().build());
    }
    
    @DeleteMapping("/{id}")
    public ResponseEntity<Void> deleteEmployee(@PathVariable Long id) {
        if (employeeService.deleteEmployee(id)) {
            return ResponseEntity.noContent().build();
        }
        return ResponseEntity.notFound().build();
    }
}
```

## Angular Integration

```typescript
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Employee {
  id?: number;
  name: string;
  email: string;
  department: string;
  salary: number;
  createdAt?: string;
}

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {
  private apiUrl = 'http://localhost:8080/api/employees';

  constructor(private http: HttpClient) {}

  getAllEmployees(): Observable<Employee[]> {
    return this.http.get<Employee[]>(this.apiUrl);
  }

  getEmployeeById(id: number): Observable<Employee> {
    return this.http.get<Employee>(`${this.apiUrl}/${id}`);
  }

  getEmployeesByDepartment(department: string): Observable<Employee[]> {
    return this.http.get<Employee[]>(`${this.apiUrl}/department/${department}`);
  }

  createEmployee(employee: Employee): Observable<Employee> {
    return this.http.post<Employee>(this.apiUrl, employee);
  }

  updateEmployee(id: number, employee: Employee): Observable<Employee> {
    return this.http.put<Employee>(`${this.apiUrl}/${id}`, employee);
  }

  deleteEmployee(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
```

## Industry Best Practices and Common Pitfalls

### Best Practices

1. **Use proper entity relationships** - Map foreign keys with @OneToMany, @ManyToOne

2. **Always have a default constructor** - JPA needs it to instantiate entities

3. **Use @Transactional on write operations** - Ensures data consistency

4. **Use @Transactional(readOnly = true) for reads** - Better performance

5. **Handle exceptions properly** - Don't let database exceptions propagate

### Common Pitfalls

1. **Forgetting @Entity annotation** - Table won't be created

2. **Circular references** - Don't have two entities reference each other without proper configuration

3. **Lazy loading issues** - Accessing lazy collections outside transaction causes errors

4. **N+1 query problem** - Fetching entities one by one instead of batch

5. **Not handling null** - Use Optional for nullable returns

## Student Hands-On Exercises

### Exercise 1: Create an Entity (Easy)
Create a Product entity with:
- id (Long, auto-generated)
- name (String, required)
- price (Double, required)
- description (String, optional)
- inStock (Boolean)

### Exercise 2: Add Queries (Medium)
Add the following query methods to your ProductRepository:
- findByNameContaining(String name)
- findByPriceLessThan(Double price)
- findByInStockTrue()

### Exercise 3: Create Complete CRUD (Medium)
Build complete CRUD for the Product entity including:
- Entity
- Repository
- Service
- Controller
- Angular service

### Exercise 4: Relationships (Hard)
Create a one-to-many relationship:
- Author (one)
- Book (many)

Include proper JPA annotations.

### Exercise 5: Custom Queries (Hard)
Create custom JPQL queries for:
- Find products with price between min and max
- Find authors with more than N books

---

## Summary

In this lesson, you've learned:
- What ORM is and why it matters
- How JPA and Hibernate work together
- Essential JPA annotations (@Entity, @Id, @GeneratedValue, @Table, @Column)
- How entities map to database tables
- Complete implementation of CRUD operations
- Integration with Angular

You can now work with databases in Spring Boot! In the next lesson, we'll explore database configuration in more detail.

---

**Next Lesson**: In the next lesson, we'll explore [Database Configuration](10_Database_Configuration.md) and learn how to set up different databases.
