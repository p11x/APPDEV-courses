# Spring Boot Actuator

## Concept Title and Overview

In this lesson, you'll learn about Spring Boot Actuator, which provides production-ready monitoring and management endpoints.

## Real-World Importance and Context

Actuator endpoints help you monitor your application health, view metrics, and manage the application in production.

## Detailed Step-by-Step Explanation

### Adding Actuator

```xml
<dependency>
    <groupId>org.springframework.boot</groupId>
    <artifactId>spring-boot-starter-actuator</artifactId>
</dependency>
```

### Configuration

```properties
management.endpoints.web.exposure.include=health,info,metrics
management.endpoint.health.show-details=always
```

### Available Endpoints

```
/actuator/health  - Application health
/actuator/info    - Application information
/actuator/metrics - Application metrics
/actuator/env     - Environment properties
/actuator/beans   - All Spring beans
/actuator/mappings - All request mappings
```

## Student Hands-On Exercises

### Exercise 1: Enable Actuator (Easy)
Add Actuator to your project and explore endpoints.

### Exercise 2: Custom Health (Medium)
Create a custom health indicator.

---

## Summary

You've learned:
- Adding Actuator
- Configuring endpoints
- Health checks and metrics

---

**Next Lesson**: In the next lesson, we'll explore [Testing SpringBoot APIs](23_Testing_SpringBoot_APIs.md).
