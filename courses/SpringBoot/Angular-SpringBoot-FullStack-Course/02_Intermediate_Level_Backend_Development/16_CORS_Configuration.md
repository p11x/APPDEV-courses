# CORS Configuration

## Concept Title and Overview

In this lesson, you'll learn about Cross-Origin Resource Sharing (CORS) and how to configure it in Spring Boot. This is essential when your Angular frontend runs on a different port than your backend.

## Real-World Importance and Context

Browsers block requests from your Angular app (one origin) to your Spring Boot API (different origin) for security. CORS allows you to explicitly permit these cross-origin requests.

## Detailed Step-by-Step Explanation

### Understanding CORS

```
┌─────────────────────────────────────────────────────────────────────────┐
│                    CORS EXPLAINED                                        │
├─────────────────────────────────────────────────────────────────────────┤
│                                                                         │
│  Same Origin:                    Different Origin (CORS needed):        │
│  ┌──────────────┐               ┌──────────────┐                      │
│  │ Angular App  │               │ Angular App  │                      │
│  │ localhost:4200│               │ localhost:4200│                     │
│  └──────┬───────┘               └──────┬───────┘                      │
│         │                                │                             │
│         ▼                                ▼                             │
│  ┌──────────────┐               ┌──────────────┐                      │
│  │ Spring Boot  │               │ Spring Boot  │                      │
│  │ localhost:4200│               │ localhost:8080│ ◄── Blocked!        │
│  └──────────────┘               └──────────────┘                      │
│                                                                         │
│  CORS headers tell the browser:                                         │
│  "It's okay for localhost:4200 to access localhost:8080"               │
│                                                                         │
└─────────────────────────────────────────────────────────────────────────┘
```

### Controller-Level CORS

```java
// Apply to single controller
@CrossOrigin(origins = "http://localhost:4200")
@RestController
@RequestMapping("/api/employees")
public class EmployeeController {
    // All endpoints allow CORS from localhost:4200
}
```

### Method-Level CORS

```java
@RestController
@RequestMapping("/api/employees")
public class EmployeeController {
    
    @CrossOrigin(origins = "http://localhost:4200")
    @GetMapping
    public List<Employee> getAll() {
        return employeeService.findAll();
    }
    
    @CrossOrigin(origins = "*")  // Allow all origins for this method
    @PostMapping
    public Employee create(@RequestBody Employee employee) {
        return employeeService.save(employee);
    }
}
```

### Global CORS Configuration

```java
@Configuration
public class WebConfig implements WebMvcConfigurer {
    
    @Override
    public void addCorsMappings(CorsRegistry registry) {
        registry.addMapping("/api/**")
            .allowedOrigins("http://localhost:4200")
            .allowedMethods("GET", "POST", "PUT", "DELETE", "PATCH")
            .allowedHeaders("*")
            .allowCredentials(true)
            .maxAge(3600);
    }
}
```

## Angular Proxy Alternative

Instead of CORS, you can use Angular proxy:

```json
// proxy.conf.json
{
  "/api": {
    "target": "http://localhost:8080",
    "secure": false
  }
}
```

```json
// angular.json
{
  "architect": {
    "serve": {
      "options": {
        "proxyConfig": "proxy.conf.json"
      }
    }
  }
}
```

Then in Angular, use relative paths:
```typescript
private apiUrl = '/api/employees';  // No need for full URL
```

## Student Hands-On Exercises

### Exercise 1: Configure CORS (Easy)
Add CORS configuration to allow your Angular app to access your API.

### Exercise 2: Try Proxy (Medium)
Set up Angular proxy and verify it works.

---

## Summary

You've learned:
- What CORS is and why it's needed
- Controller-level CORS
- Global CORS configuration
- Angular proxy as an alternative

---

This completes the Intermediate Level! You've learned all the core concepts needed to build production-ready Spring Boot APIs with Angular.

---

**Next**: In the Advanced Level, we'll cover security, deployment, and more!
