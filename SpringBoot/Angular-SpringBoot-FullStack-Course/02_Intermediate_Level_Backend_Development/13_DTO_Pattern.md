# DTO Pattern

## Concept Title and Overview

In this lesson, you'll learn about Data Transfer Objects (DTOs) and why they're essential for clean API design. DTOs separate your internal data model from the external API contract.

## Real-World Importance and Context

Your database entities contain internal fields that shouldn't be exposed to API clients (like passwords, internal IDs, or audit fields). DTOs let you control exactly what data is sent between frontend and backend.

## Detailed Step-by-Step Explanation

### Why Use DTOs?

```
┌─────────────────────────────────────────────────────────────────────────┐
│                    ENTITY vs DTO                                         │
├─────────────────────────────────────────────────────────────────────────┤
│                                                                         │
│  DATABASE ENTITY                    API RESPONSE (DTO)                 │
│  ┌─────────────────────┐          ┌─────────────────────┐             │
│  │ User                │          │ UserDto             │             │
│  │ - id                │          │ - id                │             │
│  │ - name              │          │ - name              │             │
│  │ - email             │          │ - email             │             │
│  │ - password (HASH)  │  ─────►   │ - (excluded)       │             │
│  │ - createdAt         │          │ - createdAt        │             │
│  │ - updatedAt        │          │ - (excluded)       │             │
│  │ - deletedAt        │          │ - (excluded)       │             │
│  │ - internalNote     │          │ - (excluded)       │             │
│  └─────────────────────┘          └─────────────────────┘             │
│                                                                         │
│  Why filter?                                                            │
│  • Security: Don't expose sensitive data                                │
│  • Performance: Don't send unnecessary data                             │
│  • Versioning: Change internal structure without breaking clients      │
│  • Validation: Separate input/output validation                        │
│                                                                         │
└─────────────────────────────────────────────────────────────────────────┘
```

### Creating DTOs

```java
package com.example.demo.dto;

// Request DTO for creating a user
public class CreateUserRequest {
    private String name;
    private String email;
    private String password;
    
    // Getters and Setters
    public String getName() { return name; }
    public void setName(String name) { this.name = name; }
    
    public String getEmail() { return email; }
    public void setEmail(String email) { this.email = email; }
    
    public String getPassword() { return password; }
    public void setPassword(String password) { this.password = password; }
}

// Response DTO for user data
public class UserResponse {
    private Long id;
    private String name;
    private String email;
    private String role;
    private LocalDateTime createdAt;
    
    // Getters and Setters
    public Long getId() { return id; }
    public void setId(Long id) { this.id = id; }
    
    public String getName() { return name; }
    public void setName(String name) { this.name = name; }
    
    public String getEmail() { return email; }
    public void setEmail(String email) { this.email = email; }
    
    public String getRole() { return role; }
    public void setRole(String role) { this.role = role; }
    
    public LocalDateTime getCreatedAt() { return createdAt; }
    public void setCreatedAt(LocalDateTime createdAt) { this.createdAt = createdAt; }
}
```

### Using ModelMapper

```java
// Add dependency
// <dependency>
//     <groupId>org.modelmapper</groupId>
//     <artifactId>modelmapper</artifactId>
//     <version>3.2.0</version>
// </dependency>

// Configuration
@Bean
public ModelMapper modelMapper() {
    return new ModelMapper();
}

// Using in service
@Service
public class UserService {
    private final ModelMapper modelMapper;
    
    public UserService(ModelMapper modelMapper) {
        this.modelMapper = modelMapper;
    }
    
    public UserResponse toDto(User user) {
        return modelMapper.map(user, UserResponse.class);
    }
    
    public User toEntity(CreateUserRequest request) {
        return modelMapper.map(request, User.class);
    }
}
```

### Manual Mapping

```java
// Sometimes manual mapping is clearer
public UserResponse toDto(User user) {
    UserResponse dto = new UserResponse();
    dto.setId(user.getId());
    dto.setName(user.getName());
    dto.setEmail(user.getEmail());
    dto.setRole(user.getRole());
    dto.setCreatedAt(user.getCreatedAt());
    return dto;
}
```

### Complete Implementation

**Entity:**
```java
@Entity
@Table(name = "users")
public class User {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;
    
    @Column(nullable = false)
    private String name;
    
    @Column(nullable = false, unique = true)
    private String email;
    
    @Column(nullable = false)
    private String password; // Hashed
    
    private String role;
    
    private boolean active;
    
    @Column(name = "created_at")
    private LocalDateTime createdAt;
    
    @Column(name = "updated_at")
    private LocalDateTime updatedAt;
    
    // Constructors, getters, setters
}
```

**DTOs:**
```java
// For creating - excludes ID, createdAt
public class CreateUserRequest {
    @NotBlank
    @Size(min = 2, max = 50)
    private String name;
    
    @NotBlank
    @Email
    private String email;
    
    @NotBlank
    @Size(min = 6)
    private String password;
    
    private String role;
    
    // Getters and Setters
}

// For updating
public class UpdateUserRequest {
    @NotBlank
    private String name;
    
    @Email
    private String email;
    
    private boolean active;
    
    // Getters and Setters
}

// Response - excludes password
public class UserResponse {
    private Long id;
    private String name;
    private String email;
    private String role;
    private boolean active;
    private LocalDateTime createdAt;
    
    // Getters and Setters
}
```

**Service:**
```java
@Service
public class UserService {
    private final UserRepository userRepository;
    private final ModelMapper modelMapper;
    
    public UserService(UserRepository userRepository, ModelMapper modelMapper) {
        this.userRepository = userRepository;
        this.modelMapper = modelMapper;
    }
    
    public UserResponse createUser(CreateUserRequest request) {
        // Convert DTO to entity
        User user = modelMapper.map(request, User.class);
        user.setPassword(hashPassword(request.getPassword()));
        user.setActive(true);
        
        // Save
        User saved = userRepository.save(user);
        
        // Convert to response DTO
        return toDto(saved);
    }
    
    public UserResponse toDto(User user) {
        UserResponse dto = new UserResponse();
        dto.setId(user.getId());
        dto.setName(user.getName());
        dto.setEmail(user.getEmail());
        dto.setRole(user.getRole());
        dto.setActive(user.isActive());
        dto.setCreatedAt(user.getCreatedAt());
        return dto;
    }
}
```

## Angular DTO Usage

```typescript
// Create Request DTO
export interface CreateUserRequest {
  name: string;
  email: string;
  password: string;
  role?: string;
}

// Response DTO
export interface UserResponse {
  id: number;
  name: string;
  email: string;
  role: string;
  active: boolean;
  createdAt: string;
}

// Service
createUser(request: CreateUserRequest): Observable<UserResponse> {
  return this.http.post<UserResponse>(this.apiUrl, request);
}
```

## Student Hands-On Exercises

### Exercise 1: Create DTOs (Easy)
Create DTOs for the Employee entity: CreateEmployeeRequest, UpdateEmployeeRequest, EmployeeResponse.

### Exercise 2: Add Validation (Medium)
Add validation annotations to your DTOs.

### Exercise 3: Implement Mapping (Medium)
Update the service layer to use DTOs.

### Exercise 4: Nested Objects (Hard)
Create DTOs for entities with relationships (e.g., Department with Employees).

---

## Summary

In this lesson, you've learned:
- Why DTOs are important
- How to create DTOs
- Using ModelMapper for automatic mapping
- Manual mapping when needed
- Integration with Angular

---

**Next Lesson**: In the next lesson, we'll explore [Exception Handling](14_Exception_Handling.md).
