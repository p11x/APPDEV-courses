# Exception Handling

## Concept Title and Overview

In this lesson, you'll learn how to handle exceptions properly in Spring Boot. Good error handling is crucial for providing a good API experience to your frontend clients.

## Real-World Importance and Context

When something goes wrong, your API should return meaningful error messages, not stack traces. Proper exception handling helps debugging and provides better UX.

## Detailed Step-by-Step Explanation

### Spring Boot's Default Error Handling

By default, Spring returns a white-label error page. Let's customize this.

### Custom Exception Classes

```java
@ResponseStatus(HttpStatus.NOT_FOUND)
public class ResourceNotFoundException extends RuntimeException {
    public ResourceNotFoundException(String message) {
        super(message);
    }
}

@ResponseStatus(HttpStatus.BAD_REQUEST)
public class BadRequestException extends RuntimeException {
    public BadRequestException(String message) {
        super(message);
    }
}
```

### Global Exception Handler with @ControllerAdvice

```java
@RestControllerAdvice
public class GlobalExceptionHandler {
    
    @ExceptionHandler(ResourceNotFoundException.class)
    public ResponseEntity<ErrorResponse> handleNotFound(ResourceNotFoundException ex) {
        ErrorResponse error = new ErrorResponse(
            HttpStatus.NOT_FOUND.value(),
            ex.getMessage(),
            System.currentTimeMillis()
        );
        return ResponseEntity.status(HttpStatus.NOT_FOUND).body(error);
    }
    
    @ExceptionHandler(BadRequestException.class)
    public ResponseEntity<ErrorResponse> handleBadRequest(BadRequestException ex) {
        ErrorResponse error = new ErrorResponse(
            HttpStatus.BAD_REQUEST.value(),
            ex.getMessage(),
            System.currentTimeMillis()
        );
        return ResponseEntity.badRequest().body(error);
    }
    
    @ExceptionHandler(Exception.class)
    public ResponseEntity<ErrorResponse> handleGeneral(Exception ex) {
        ErrorResponse error = new ErrorResponse(
            HttpStatus.INTERNAL_SERVER_ERROR.value(),
            "An unexpected error occurred",
            System.currentTimeMillis()
        );
        return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(error);
    }
}
```

### Error Response Class

```java
public class ErrorResponse {
    private int status;
    private String message;
    private long timestamp;
    
    public ErrorResponse(int status, String message, long timestamp) {
        this.status = status;
        this.message = message;
        this.timestamp = timestamp;
    }
    
    // Getters and Setters
}
```

## Complete Example

```java
// Custom Exception
public class EmployeeNotFoundException extends RuntimeException {
    public EmployeeNotFoundException(Long id) {
        super("Employee not found with id: " + id);
    }
}

// Global Handler
@RestControllerAdvice
public class GlobalExceptionHandler {
    
    @ExceptionHandler(EmployeeNotFoundException.class)
    public ResponseEntity<ErrorResponse> handleEmployeeNotFound(EmployeeNotFoundException ex) {
        ErrorResponse error = new ErrorResponse(404, ex.getMessage(), System.currentTimeMillis());
        return ResponseEntity.status(HttpStatus.NOT_FOUND).body(error);
    }
    
    @ExceptionHandler(IllegalArgumentException.class)
    public ResponseEntity<ErrorResponse> handleIllegalArgument(IllegalArgumentException ex) {
        ErrorResponse error = new ErrorResponse(400, ex.getMessage(), System.currentTimeMillis());
        return ResponseEntity.badRequest().body(error);
    }
}

// Service throws exception
public Employee getEmployeeById(Long id) {
    return employeeRepository.findById(id)
        .orElseThrow(() -> new EmployeeNotFoundException(id));
}
```

## Angular Error Handling

```typescript
getEmployee(id: number): Observable<Employee> {
  return this.http.get<Employee>(`${this.apiUrl}/${id}`).pipe(
    catchError(error => {
      console.error('Error:', error);
      throw error;
    })
  );
}
```

## Student Hands-On Exercises

### Exercise 1: Create Custom Exceptions (Easy)
Create custom exceptions for your Employee API.

### Exercise 2: Implement Global Handler (Medium)
Implement a @ControllerAdvice for centralized exception handling.

### Exercise 3: Handle Validation Errors (Medium)
Handle validation errors and return meaningful messages.

---

## Summary

You've learned:
- Custom exception classes
- @ControllerAdvice for global handling
- Consistent error response format
- Integration with Angular error handling

---

**Next Lesson**: In the next lesson, we'll explore [Validation](15_Validation.md).
