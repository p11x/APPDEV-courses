# CRUD API Complete Project

## Concept Title and Overview

In this lesson, we'll build a complete Employee Management System that integrates everything we've learned. This is a practical project that demonstrates all the layers working together.

## Real-World Importance and Context

This project brings together all the concepts from previous lessons into a working application. You'll see how the controller, service, and repository layers work together to create a complete REST API.

## Project Structure

```
employee-manager/
├── src/main/java/com/example/employeemanager/
│   ├── EmployeeManagerApplication.java
│   ├── model/
│   │   └── Employee.java
│   ├── repository/
│   │   └── EmployeeRepository.java
│   ├── service/
│   │   └── EmployeeService.java
│   ├── controller/
│   │   └── EmployeeController.java
│   └── exception/
│       └── ResourceNotFoundException.java
├── src/main/resources/
│   └── application.properties
└── pom.xml
```

## Complete Implementation

### 1. Entity (Employee.java)

```java
package com.example.employeemanager.model;

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
    
    @Column(nullable = false, unique = true)
    private String email;
    
    private String jobTitle;
    
    private String phone;
    
    private Double salary;
    
    @Column(name = "created_at")
    private LocalDateTime createdAt;
    
    @PrePersist
    protected void onCreate() {
        createdAt = LocalDateTime.now();
    }
    
    // Constructors
    public Employee() {}
    
    public Employee(String name, String email, String jobTitle, String phone, Double salary) {
        this.name = name;
        this.email = email;
        this.jobTitle = jobTitle;
        this.phone = phone;
        this.salary = salary;
    }
    
    // Getters and Setters
    public Long getId() { return id; }
    public void setId(Long id) { this.id = id; }
    
    public String getName() { return name; }
    public void setName(String name) { this.name = name; }
    
    public String getEmail() { return email; }
    public void setEmail(String email) { this.email = email; }
    
    public String getJobTitle() { return jobTitle; }
    public void setJobTitle(String jobTitle) { this.jobTitle = jobTitle; }
    
    public String getPhone() { return phone; }
    public void setPhone(String phone) { this.phone = phone; }
    
    public Double getSalary() { return salary; }
    public void setSalary(Double salary) { this.salary = salary; }
    
    public LocalDateTime getCreatedAt() { return createdAt; }
    public void setCreatedAt(LocalDateTime createdAt) { this.createdAt = createdAt; }
}
```

### 2. Repository (EmployeeRepository.java)

```java
package com.example.employeemanager.repository;

import com.example.employeemanager.model.Employee;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.List;
import java.util.Optional;

@Repository
public interface EmployeeRepository extends JpaRepository<Employee, Long> {
    
    Optional<Employee> findByEmail(String email);
    
    List<Employee> findByJobTitle(String jobTitle);
    
    List<Employee> findByNameContaining(String name);
    
    boolean existsByEmail(String email);
}
```

### 3. Exception Class (ResourceNotFoundException.java)

```java
package com.example.employeemanager.exception;

import org.springframework.http.HttpStatus;
import org.springframework.web.bind.annotation.ResponseStatus;

@ResponseStatus(HttpStatus.NOT_FOUND)
public class ResourceNotFoundException extends RuntimeException {
    
    public ResourceNotFoundException(String message) {
        super(message);
    }
}
```

### 4. Service (EmployeeService.java)

```java
package com.example.employeemanager.service;

import com.example.employeemanager.exception.ResourceNotFoundException;
import com.example.employeemanager.model.Employee;
import com.example.employeemanager.repository.EmployeeRepository;
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
    
    // CREATE - Add new employee
    public Employee createEmployee(Employee employee) {
        // Check if email already exists
        if (employeeRepository.existsByEmail(employee.getEmail())) {
            throw new IllegalArgumentException("Email already exists: " + employee.getEmail());
        }
        return employeeRepository.save(employee);
    }
    
    // READ ALL - Get all employees
    @Transactional(readOnly = true)
    public List<Employee> getAllEmployees() {
        return employeeRepository.findAll();
    }
    
    // READ ONE - Get employee by ID
    @Transactional(readOnly = true)
    public Employee getEmployeeById(Long id) {
        return employeeRepository.findById(id)
                .orElseThrow(() -> new ResourceNotFoundException("Employee not found with id: " + id));
    }
    
    // UPDATE - Update employee
    public Employee updateEmployee(Long id, Employee employeeDetails) {
        Employee employee = getEmployeeById(id);
        
        employee.setName(employeeDetails.getName());
        employee.setEmail(employeeDetails.getEmail());
        employee.setJobTitle(employeeDetails.getJobTitle());
        employee.setPhone(employeeDetails.getPhone());
        employee.setSalary(employeeDetails.getSalary());
        
        return employeeRepository.save(employee);
    }
    
    // DELETE - Delete employee
    public void deleteEmployee(Long id) {
        Employee employee = getEmployeeById(id);
        employeeRepository.delete(employee);
    }
    
    // SEARCH - Search by name
    @Transactional(readOnly = true)
    public List<Employee> searchByName(String name) {
        return employeeRepository.findByNameContaining(name);
    }
    
    // GET BY JOB TITLE
    @Transactional(readOnly = true)
    public List<Employee> getEmployeesByJobTitle(String jobTitle) {
        return employeeRepository.findByJobTitle(jobTitle);
    }
}
```

### 5. Controller (EmployeeController.java)

```java
package com.example.employeemanager.controller;

import com.example.employeemanager.model.Employee;
import com.example.employeemanager.service.EmployeeService;
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
    
    // GET /api/employees - Get all employees
    @GetMapping
    public ResponseEntity<List<Employee>> getAllEmployees() {
        List<Employee> employees = employeeService.getAllEmployees();
        return ResponseEntity.ok(employees);
    }
    
    // GET /api/employees/{id} - Get employee by ID
    @GetMapping("/{id}")
    public ResponseEntity<Employee> getEmployeeById(@PathVariable Long id) {
        Employee employee = employeeService.getEmployeeById(id);
        return ResponseEntity.ok(employee);
    }
    
    // POST /api/employees - Create new employee
    @PostMapping
    public ResponseEntity<Employee> createEmployee(@RequestBody Employee employee) {
        try {
            Employee createdEmployee = employeeService.createEmployee(employee);
            return ResponseEntity.status(HttpStatus.CREATED).body(createdEmployee);
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().build();
        }
    }
    
    // PUT /api/employees/{id} - Update employee
    @PutMapping("/{id}")
    public ResponseEntity<Employee> updateEmployee(
            @PathVariable Long id,
            @RequestBody Employee employee) {
        try {
            Employee updatedEmployee = employeeService.updateEmployee(id, employee);
            return ResponseEntity.ok(updatedEmployee);
        } catch (Exception e) {
            return ResponseEntity.notFound().build();
        }
    }
    
    // DELETE /api/employees/{id} - Delete employee
    @DeleteMapping("/{id}")
    public ResponseEntity<Void> deleteEmployee(@PathVariable Long id) {
        try {
            employeeService.deleteEmployee(id);
            return ResponseEntity.noContent().build();
        } catch (Exception e) {
            return ResponseEntity.notFound().build();
        }
    }
    
    // GET /api/employees/search?name=John - Search by name
    @GetMapping("/search")
    public ResponseEntity<List<Employee>> searchByName(@RequestParam String name) {
        List<Employee> employees = employeeService.searchByName(name);
        return ResponseEntity.ok(employees);
    }
    
    // GET /api/employees/job/{jobTitle} - Get by job title
    @GetMapping("/job/{jobTitle}")
    public ResponseEntity<List<Employee>> getByJobTitle(@PathVariable String jobTitle) {
        List<Employee> employees = employeeService.getEmployeesByJobTitle(jobTitle);
        return ResponseEntity.ok(employees);
    }
}
```

### 6. Application Properties

```properties
# Application name
spring.application.name=employee-manager

# H2 Database
spring.datasource.url=jdbc:h2:mem:employeedb
spring.datasource.driverClassName=org.h2.Driver
spring.datasource.username=sa
spring.datasource.password=

# JPA/Hibernate
spring.jpa.database-platform=org.hibernate.dialect.H2Dialect
spring.jpa.hibernate.ddl-auto=create-drop
spring.jpa.show-sql=true
spring.jpa.properties.hibernate.format_sql=true

# H2 Console
spring.h2.console.enabled=true

# Server
server.port=8080
```

### pom.xml Dependencies

```xml
<?xml version="1.0" encoding="UTF-8"?>
<project xmlns="http://maven.apache.org/POM/4.0.0"
         xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
         xsi:schemaLocation="http://maven.apache.org/POM/4.0.0 
         https://maven.apache.org/xsd/maven-4.0.0.xsd">
    
    <modelVersion>4.0.0</modelVersion>
    
    <parent>
        <groupId>org.springframework.boot</groupId>
        <artifactId>spring-boot-starter-parent</artifactId>
        <version>3.2.1</version>
    </parent>
    
    <groupId>com.example</groupId>
    <artifactId>employee-manager</artifactId>
    <version>1.0.0</version>
    <name>employee-manager</name>
    
    <properties>
        <java.version>21</java.version>
    </properties>
    
    <dependencies>
        <!-- Spring Web -->
        <dependency>
            <groupId>org.springframework.boot</groupId>
            <artifactId>spring-boot-starter-web</artifactId>
        </dependency>
        
        <!-- Spring Data JPA -->
        <dependency>
            <groupId>org.springframework.boot</groupId>
            <artifactId>spring-boot-starter-data-jpa</artifactId>
        </dependency>
        
        <!-- H2 Database -->
        <dependency>
            <groupId>com.h2database</groupId>
            <artifactId>h2</artifactId>
            <scope>runtime</scope>
        </dependency>
        
        <!-- Validation -->
        <dependency>
            <groupId>org.springframework.boot</groupId>
            <artifactId>spring-boot-starter-validation</artifactId>
        </dependency>
        
        <!-- Lombok (optional) -->
        <dependency>
            <groupId>org.projectlombok</groupId>
            <artifactId>lombok</artifactId>
            <optional>true</optional>
        </dependency>
        
        <!-- Test -->
        <dependency>
            <groupId>org.springframework.boot</groupId>
            <artifactId>spring-boot-starter-test</artifactId>
            <scope>test</scope>
        </dependency>
    </dependencies>
    
    <build>
        <plugins>
            <plugin>
                <groupId>org.springframework.boot</groupId>
                <artifactId>spring-boot-maven-plugin</artifactId>
            </plugin>
        </plugins>
    </build>
</project>
```

## Angular Frontend Integration

### Employee Model

```typescript
export interface Employee {
  id?: number;
  name: string;
  email: string;
  jobTitle: string;
  phone: string;
  salary: number;
  createdAt?: string;
}
```

### Employee Service

```typescript
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Employee } from './employee.model';

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {
  private apiUrl = 'http://localhost:8080/api/employees';

  constructor(private http: HttpClient) {}

  // GET /api/employees - Get all employees
  getAllEmployees(): Observable<Employee[]> {
    return this.http.get<Employee[]>(this.apiUrl);
  }

  // GET /api/employees/{id} - Get employee by ID
  getEmployeeById(id: number): Observable<Employee> {
    return this.http.get<Employee>(`${this.apiUrl}/${id}`);
  }

  // POST /api/employees - Create employee
  createEmployee(employee: Employee): Observable<Employee> {
    return this.http.post<Employee>(this.apiUrl, employee);
  }

  // PUT /api/employees/{id} - Update employee
  updateEmployee(id: number, employee: Employee): Observable<Employee> {
    return this.http.put<Employee>(`${this.apiUrl}/${id}`, employee);
  }

  // DELETE /api/employees/{id} - Delete employee
  deleteEmployee(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  // GET /api/employees/search?name=... - Search by name
  searchByName(name: string): Observable<Employee[]> {
    return this.http.get<Employee[]>(`${this.apiUrl}/search?name=${name}`);
  }
}
```

### Employee List Component

```typescript
import { Component, OnInit } from '@angular/core';
import { EmployeeService, Employee } from './employee.service';

@Component({
  selector: 'app-employee-list',
  template: `
    <h2>Employee Management</h2>
    
    <!-- Create Button -->
    <button (click)="showCreateForm = true">Add Employee</button>
    
    <!-- Search -->
    <input [(ngModel)]="searchName" (keyup.enter)="search()" placeholder="Search by name">
    <button (click)="search()">Search</button>
    <button (click)="loadAll()">Show All</button>
    
    <!-- Employee List -->
    <table>
      <thead>
        <tr>
          <th>ID</th>
          <th>Name</th>
          <th>Email</th>
          <th>Job Title</th>
          <th>Phone</th>
          <th>Salary</th>
          <th>Actions</th>
        </tr>
      </thead>
      <tbody>
        <tr *ngFor="let emp of employees">
          <td>{{ emp.id }}</td>
          <td>{{ emp.name }}</td>
          <td>{{ emp.email }}</td>
          <td>{{ emp.jobTitle }}</td>
          <td>{{ emp.phone }}</td>
          <td>{{ emp.salary | currency }}</td>
          <td>
            <button (click)="editEmployee(emp)">Edit</button>
            <button (click)="deleteEmployee(emp.id!)">Delete</button>
          </td>
        </tr>
      </tbody>
    </table>
    
    <!-- Error Message -->
    <div *ngIf="error" class="error">{{ error }}</div>
  `
})
export class EmployeeListComponent implements OnInit {
  employees: Employee[] = [];
  searchName: string = '';
  showCreateForm: boolean = false;
  error: string = '';

  constructor(private employeeService: EmployeeService) {}

  ngOnInit() {
    this.loadAll();
  }

  loadAll() {
    this.employeeService.getAllEmployees().subscribe({
      next: (data) => this.employees = data,
      error: (err) => this.error = 'Error loading employees'
    });
  }

  search() {
    if (this.searchName) {
      this.employeeService.searchByName(this.searchName).subscribe({
        next: (data) => this.employees = data,
        error: (err) => this.error = 'Error searching'
      });
    }
  }

  editEmployee(employee: Employee) {
    // Navigate to edit page or show modal
  }

  deleteEmployee(id: number) {
    if (confirm('Are you sure?')) {
      this.employeeService.deleteEmployee(id).subscribe({
        next: () => {
          this.employees = this.employees.filter(e => e.id !== id);
        },
        error: (err) => this.error = 'Error deleting employee'
      });
    }
  }
}
```

## API Endpoint Summary

```
┌────────────────────────────────────────────────────────────────────────┐
│                     EMPLOYEE API ENDPOINTS                            │
├──────────────────┬──────────────┬─────────────────────────────────────┤
│       URL        │   METHOD     │           DESCRIPTION               │
├──────────────────┼──────────────┼─────────────────────────────────────┤
│ /api/employees   │     GET      │ Get all employees                  │
│ /api/employees   │    POST      │ Create new employee                │
│ /api/employees/1 │    GET      │ Get employee by ID                 │
│ /api/employees/1 │     PUT      │ Update employee                    │
│ /api/employees/1 │   DELETE     │ Delete employee                    │
│ /api/employees   │     GET      │ Search by name                     │
│   /search?name=  │              │                                     │
│ /api/employees  │     GET      │ Get by job title                    │
│   /job/{title}  │              │                                     │
└──────────────────┴──────────────┴─────────────────────────────────────┘
```

## Testing the API

### Create Employee
```
POST /api/employees
Content-Type: application/json

{
  "name": "John Smith",
  "email": "john@example.com",
  "jobTitle": "Software Engineer",
  "phone": "555-1234",
  "salary": 75000
}
```

### Get All Employees
```
GET /api/employees
```

### Update Employee
```
PUT /api/employees/1
Content-Type: application/json

{
  "name": "John Smith Updated",
  "email": "john.updated@example.com",
  "jobTitle": "Senior Engineer",
  "phone": "555-5678",
  "salary": 85000
}
```

### Delete Employee
```
DELETE /api/employees/1
```

## Student Hands-On Exercises

### Exercise 1: Run the Application (Easy)
Build and run the Employee Manager application. Test all endpoints using Postman.

### Exercise 2: Add Fields (Medium)
Add a "department" field to the Employee entity and update all layers.

### Exercise 3: Add Validation (Medium)
Add validation to the Employee entity:
- Name: required, min 2 characters
- Email: required, valid email format
- Salary: positive number

### Exercise 4: Error Handling (Medium)
Improve error handling to return proper error messages.

### Exercise 5: Full Angular UI (Hard)
Build a complete Angular UI with:
- List view
- Create form
- Edit form
- Delete confirmation

---

## Summary

In this lesson, you've built a complete CRUD API with:
- Spring Boot backend with all layers
- Angular frontend service
- Complete testing workflow

This Employee Management System demonstrates everything you've learned so far. In the next lessons, we'll add more advanced features.

---

**Next Lesson**: In the next lesson, we'll explore [DTO Pattern](13_DTO_Pattern.md) for better API design.
