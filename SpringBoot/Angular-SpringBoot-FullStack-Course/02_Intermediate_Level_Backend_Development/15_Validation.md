# Validation

## Concept Title and Overview

In this lesson, you'll learn how to validate input data in Spring Boot using Bean Validation (JSR-380). Validation ensures your API receives valid data.

## Real-World Importance and Context

Never trust user input! Validation protects your application from invalid data, prevents security issues, and provides better user feedback.

## Detailed Step-by-Step Explanation

### Common Validation Annotations

```
┌─────────────────────────────────────────────────────────────────────────┐
│                    VALIDATION ANNOTATIONS                                │
├─────────────────────────────────────────────────────────────────────────┤
│                                                                         │
│  @NotNull       - Cannot be null                                        │
│  @NotEmpty      - Cannot be null or empty (strings, collections)      │
│  @NotBlank      - Cannot be null, empty, or whitespace only           │
│  @Size          - Size constraints (min, max)                         │
│  @Email         - Must be valid email format                           │
│  @Min / @Max    - Numeric constraints                                  │
│  @Positive      - Must be positive                                     │
│  @Pattern       - Must match regex pattern                            │
│  @Past          - Must be past date                                     │
│  @Future        - Must be future date                                  │
│  @Valid         - Trigger nested validation                            │
│                                                                         │
└─────────────────────────────────────────────────────────────────────────┘
```

### Adding Validation Dependencies

```xml
<dependency>
    <groupId>org.springframework.boot</groupId>
    <artifactId>spring-boot-starter-validation</artifactId>
</dependency>
```

### Validated DTOs

```java
public class CreateEmployeeRequest {
    @NotBlank(message = "Name is required")
    @Size(min = 2, max = 100, message = "Name must be between 2 and 100 characters")
    private String name;
    
    @NotBlank(message = "Email is required")
    @Email(message = "Invalid email format")
    private String email;
    
    @NotNull(message = "Salary is required")
    @Min(value = 0, message = "Salary must be positive")
    private Double salary;
    
    // Getters and Setters
}
```

### Controller with @Valid

```java
@PostMapping
public ResponseEntity<Employee> createEmployee(@Valid @RequestBody CreateEmployeeRequest request) {
    Employee employee = employeeService.create(request);
    return ResponseEntity.status(HttpStatus.CREATED).body(employee);
}
```

### Handling Validation Errors

```java
@RestControllerAdvice
public class ValidationExceptionHandler {
    
    @ExceptionHandler(MethodArgumentNotValidException.class)
    public ResponseEntity<Map<String, Object>> handleValidationErrors(MethodArgumentNotValidException ex) {
        Map<String, String> errors = new HashMap<>();
        ex.getBindingResult().getFieldErrors().forEach(error -> 
            errors.put(error.getField(), error.getDefaultMessage())
        );
        
        Map<String, Object> response = new HashMap<>();
        response.put("status", 400);
        response.put("errors", errors);
        
        return ResponseEntity.badRequest().body(response);
    }
}
```

## Angular Display Validation

```typescript
createEmployee(request: CreateEmployeeRequest): Observable<Employee> {
  return this.http.post<Employee>(this.apiUrl, request).pipe(
    catchError(error => {
      if (error.status === 400 && error.error.errors) {
        // Display validation errors
        this.displayErrors(error.error.errors);
      }
      throw error;
    })
  );
}
```

## Student Hands-On Exercises

### Exercise 1: Add Validation (Easy)
Add validation to your Employee DTOs.

### Exercise 2: Handle Errors (Medium)
Handle validation errors in the exception handler.

### Exercise 3: Custom Validator (Hard)
Create a custom validation annotation.

---

## Summary

You've learned:
- Bean Validation annotations
- @Valid for triggering validation
- Handling validation errors
- Angular error handling

---

**Next Lesson**: In the next lesson, we'll explore [CORS Configuration](16_CORS_Configuration.md).
