# API Documentation with Swagger/OpenAPI

## Concept Title and Overview

In this lesson, you'll learn how to create interactive API documentation using Swagger/OpenAPI. This allows developers to understand, test, and consume your REST API easily.

## Real-World Importance and Context

Imagine building a complex API without documentation—developers would need to read your code to understand how to use it. Swagger/OpenAPI provides a standardized way to document your API, making it easy for frontend developers (like Angular developers) to understand your endpoints without deep diving into backend code.

## Detailed Step-by-Step Explanation

### Understanding OpenAPI and Swagger

OpenAPI Specification (formerly known as Swagger) is a standard, language-agnostic specification for documenting REST APIs. It allows both humans and computers to discover and understand the capabilities of your service.

```
┌─────────────────────────────────────────────────────────────────────────┐
│                    SWAGGER BENEFITS                                      │
├─────────────────────────────────────────────────────────────────────────┤
│                                                                         │
│  1. INTERACTIVE DOCUMENTATION                                           │
│     ┌─────────────────────────────────────────────────────────────────┐│
│     │  Swagger UI provides a visual interface where developers       ││
│     │  can test endpoints directly from the browser                  ││
│     └─────────────────────────────────────────────────────────────────┘│
│                                                                         │
│  2. CODE GENERATION                                                     │
│     ┌─────────────────────────────────────────────────────────────────┐│
│     │  Generate client SDKs for various languages                   ││
│     │  (Angular, React, iOS, Android, etc.)                         ││
│     └─────────────────────────────────────────────────────────────────┘│
│                                                                         │
│  3. STANDARDIZATION                                                     │
│     ┌─────────────────────────────────────────────────────────────────┐│
│     │  OpenAPI is a vendor-neutral, widely-adopted standard         ││
│     └─────────────────────────────────────────────────────────────────┘│
│                                                                         │
│  4. AUTOMATION                                                         │
│     ┌─────────────────────────────────────────────────────────────────┐│
│     │  Documentation updates automatically when code changes         ││
│     └─────────────────────────────────────────────────────────────────┘│
│                                                                         │
└─────────────────────────────────────────────────────────────────────────┘
```

### Adding Swagger to Spring Boot

**1. Add Dependencies:**

```xml
<!-- SpringDoc OpenAPI -->
<dependency>
    <groupId>org.springdoc</groupId>
    <artifactId>springdoc-openapi-starter-webmvc-ui</artifactId>
    <version>2.3.0</version>
</dependency>
```

**2. Configure application.properties:**

```properties
# Swagger Configuration
springdoc.api-docs.path=/api-docs
springdoc.swagger-ui.path=/swagger-ui.html

# API Info
springdoc.info.title=Task Management API
springdoc.info.description=API for managing tasks and users
springdoc.info.version=1.0.0
springdoc.info.contact.name=API Support
springdoc.info.contact.email=support@example.com
```

### OpenAPI Configuration Class

```java
@Configuration
public class OpenApiConfig {
    
    @Bean
    public OpenAPI customOpenAPI() {
        return new OpenAPI()
            .info(new Info()
                .title("Task Management API")
                .description("REST API for Task Management System")
                .version("1.0.0")
                .contact(new Contact()
                    .name("API Support")
                    .email("support@example.com")))
            .addSecurityItem(new SecurityRequirement().addList("Bearer Authentication"))
            .components(new Components()
                .addSecuritySchemes("Bearer Authentication", 
                    new SecurityScheme()
                        .type(SecurityScheme.Type.HTTP)
                        .scheme("bearer")
                        .bearerFormat("JWT")));
    }
}
```

### Documenting Endpoints

```java
@RestController
@RequestMapping("/api/tasks")
@Tag(name = "Task Management", description = "APIs for managing tasks")
public class TaskController {
    
    @Operation(summary = "Get all tasks", description = "Returns a paginated list of tasks")
    @ApiResponses(value = {
        @ApiResponse(responseCode = "200", description = "Tasks found"),
        @ApiResponse(responseCode = "401", description = "Unauthorized"),
        @ApiResponse(responseCode = "500", description = "Internal server error")
    })
    @GetMapping
    public ResponseEntity<Page<Task>> getTasks(
            @Parameter(description = "Page number") @RequestParam(defaultValue = "0") int page,
            @Parameter(description = "Page size") @RequestParam(defaultValue = "10") int size) {
        // implementation
    }
    
    @Operation(summary = "Create task", description = "Creates a new task")
    @ApiResponse(responseCode = "201", description = "Task created successfully")
    @PostMapping
    public ResponseEntity<Task> createTask(
            @Parameter(description = "Task to create") @Valid @RequestBody TaskRequest request) {
        // implementation
    }
}
```

## Angular Integration

### Generating TypeScript from OpenAPI

Use openapi-generator to create Angular services:

```bash
npx @openapitools/openapi-generator-cli generate \
  -i http://localhost:8080/api-docs.yaml \
  -g typescript-angular \
  -o ./src/app/api
```

### Manual Angular Service

```typescript
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Task } from '../models/task.model';

@Injectable({ providedIn: 'root' })
export class TaskApiService {
  private baseUrl = 'http://localhost:8080/api/tasks';

  constructor(private http: HttpClient) {}

  getTasks(page: number = 0, size: number = 10): Observable<any> {
    return this.http.get(`${this.baseUrl}?page=${page}&size=${size}`);
  }

  getTask(id: number): Observable<Task> {
    return this.http.get<Task>(`${this.baseUrl}/${id}`);
  }

  createTask(task: Task): Observable<Task> {
    return this.http.post<Task>(this.baseUrl, task);
  }

  updateTask(id: number, task: Task): Observable<Task> {
    return this.http.put<Task>(`${this.baseUrl}/${id}`, task);
  }

  deleteTask(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
```

## Industry Best Practices and Common Pitfalls

### Best Practices

1. **Always document all endpoints** - Include descriptions, response codes, and examples

2. **Use descriptive operation IDs** - Makes generated code more readable

3. **Group endpoints with tags** - Logical organization for better documentation

4. **Include request/response examples** - Helps developers understand the API faster

5. **Secure your Swagger UI in production** - Don't expose internal APIs

### Common Pitfalls

1. **Forgetting to document error responses** - Developers need to know error formats

2. **Not versioning your API** - Makes breaking changes difficult

3. **Exposing sensitive information** - Don't document internal implementation details

4. **Missing validation annotations** - Document constraints like @NotNull, @Size

## Student Hands-On Exercises

### Exercise 1: Add Swagger (Easy)
Add SpringDoc to your project and access the Swagger UI at /swagger-ui.html

### Exercise 2: Document Endpoints (Medium)
Add OpenAPI annotations to all your controller endpoints

### Exercise 3: Generate Client (Hard)
Generate TypeScript client code from your OpenAPI specification

---

## Summary

In this lesson, you've learned:
- What OpenAPI/Swagger is and why it matters
- Adding Swagger to Spring Boot applications
- Documenting endpoints with annotations
- Angular integration patterns
- Best practices for API documentation

---

**Next Lesson**: In the next lesson, we'll explore WebSocket Real-Time Communication.
