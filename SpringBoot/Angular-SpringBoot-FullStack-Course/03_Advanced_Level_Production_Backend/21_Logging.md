# Logging

## Concept Title and Overview

In this lesson, you'll learn about logging in Spring Boot applications, essential for debugging and monitoring.

## Real-World Importance and Context

Logging helps you understand what's happening in your application, debug issues, and monitor production systems.

## Detailed Step-by-Step Explanation

### SLF4J Logging

Spring Boot uses SLF4J as the logging facade:

```java
@RestController
public class EmployeeController {
    
    private static final Logger logger = LoggerFactory.getLogger(EmployeeController.class);
    
    @GetMapping
    public List<Employee> getAll() {
        logger.info("Fetching all employees");
        List<Employee> employees = employeeService.findAll();
        logger.debug("Found {} employees", employees.size());
        return employees;
    }
}
```

### Log Levels

```
TRACE - Most detailed, usually for debugging
DEBUG - Detailed information for debugging
INFO  - General informational messages
WARN  - Warning messages
ERROR - Error messages
```

### Configuration

```properties
# application.properties
logging.level.root=INFO
logging.level.com.example.demo=DEBUG
logging.pattern.console=%d{yyyy-MM-dd HH:mm:ss} - %msg%n
logging.file=logs/application.log
```

## Student Hands-On Exercises

### Exercise 1: Add Logging (Easy)
Add logging to your controllers and services.

### Exercise 2: Configure Levels (Medium)
Set up different log levels for different environments.

---

## Summary

You've learned:
- SLF4J logging
- Log levels
- Configuration

---

**Next Lesson**: In the next lesson, we'll explore [SpringBoot Actuator](22_SpringBoot_Actuator.md).
