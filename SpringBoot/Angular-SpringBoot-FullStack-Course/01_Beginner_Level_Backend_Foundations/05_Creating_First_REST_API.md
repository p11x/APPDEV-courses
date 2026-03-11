# Creating First REST API

## Concept Title and Overview

Now it's time to build your first REST API! In this lesson, you'll learn how to create endpoints that your Angular frontend can consume. We'll start with a simple "Hello World" and progressively build more useful endpoints.

## Real-World Importance and Context

REST APIs are the backbone of modern web applications. They define how your Angular frontend communicates with your Spring Boot backend. Understanding how to create clean, RESTful APIs is essential for any full-stack developer.

Think of REST APIs like restaurant menus—they specify what you can order (endpoints), how to order (HTTP methods), and what you'll receive (responses).

## Detailed Step-by-Step Explanation

### The @RestController Annotation

The @RestController is the foundation of REST API development in Spring:

```java
package com.example.demo.controller;

import org.springframework.web.bind.annotation.RestController;

/**
 * @RestController tells Spring that this class handles HTTP requests
 * and the return values should be written directly to the response body.
 * 
 * This is essentially a combination of:
 * - @Controller: Marks this as a Spring MVC controller
 * - @ResponseBody: Return values are serialized to JSON/XML automatically
 * 
 * Before Spring 4, you had to use @Controller + @ResponseBody separately.
 */
@RestController
public class HelloController {
    // Your endpoints go here
}
```

### The @RequestMapping Annotation

@RequestMapping defines the base URL for your endpoints:

```java
@RestController
@RequestMapping("/api")  // Base path for all endpoints in this controller
public class GreetingController {
    
    // This endpoint will be at: GET /api/greeting
    @GetMapping("/greeting")
    public String greeting() {
        return "Hello!";
    }
}
```

### Creating a Simple GET Endpoint

Let's create our first endpoint:

```java
package com.example.demo.controller;

import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping("/api")
public class HelloController {

    // @GetMapping handles GET requests to /api/hello
    @GetMapping("/hello")
    public String sayHello() {
        return "Hello, World!";
    }
}
```

Now if you visit `http://localhost:8080/api/hello`, you'll see: `Hello, World!`

But this returns plain text. Let's make it return JSON:

```java
@GetMapping("/hello")
public Map<String, String> sayHello() {
    Map<String, String> response = new HashMap<>();
    response.put("message", "Hello, World!");
    response.put("timestamp", LocalDateTime.now().toString());
    return response;
}

// Returns JSON: {"message":"Hello, World!","timestamp":"2024-01-15T10:30:00"}
```

### Multiple @GetMapping Variations

You can use @GetMapping in several ways:

```java
@RestController
@RequestMapping("/api")
public class GreetingController {

    // Simple GET - /api/greet
    @GetMapping("/greet")
    public String greet() {
        return "Hello!";
    }

    // GET with custom status - /api/html
    @GetMapping(value = "/html", produces = MediaType.TEXT_HTML_VALUE)
    public String htmlGreeting() {
        return "<h1>Hello, World!</h1>";
    }

    // GET returning object (automatically JSON) - /api/json
    @GetMapping("/json")
    public Greeting jsonGreeting() {
        return new Greeting("Hello, World!", LocalDateTime.now());
    }
}

// Helper class for JSON response
public class Greeting {
    private String message;
    private LocalDateTime timestamp;
    
    public Greeting(String message, LocalDateTime timestamp) {
        this.message = message;
        this.timestamp = timestamp;
    }
    
    // Getters and setters (or use Lombok @Data)
    public String getMessage() { return message; }
    public void setMessage(String message) { this.message = message; }
    public LocalDateTime getTimestamp() { return timestamp; }
    public void setTimestamp(LocalDateTime timestamp) { this.timestamp = timestamp; }
}
```

## Angular Frontend Integration

Now let's see how Angular consumes this endpoint:

```typescript
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class GreetingService {
  private apiUrl = 'http://localhost:8080/api';

  constructor(private http: HttpClient) { }

  // Call GET /api/hello
  getHello(): Observable<any> {
    return this.http.get(`${this.apiUrl}/hello`);
  }

  // Call GET /api/json
  getJsonGreeting(): Observable<{message: string, timestamp: string}> {
    return this.http.get<{message: string, timestamp: string}>(`${this.apiUrl}/json`);
  }
}
```

Consuming in a component:

```typescript
import { Component, OnInit } from '@angular/core';
import { GreetingService } from './greeting.service';

@Component({
  selector: 'app-greeting',
  template: `
    <button (click)="loadGreeting()">Get Greeting</button>
    <div *ngIf="greeting">
      <p>{{ greeting.message }}</p>
      <p>{{ greeting.timestamp }}</p>
    </div>
  `
})
export class GreetingComponent implements OnInit {
  greeting: any;

  constructor(private greetingService: GreetingService) { }

  ngOnInit() {
    this.loadGreeting();
  }

  loadGreeting() {
    this.greetingService.getHello().subscribe({
      next: (data) => this.greeting = data,
      error: (err) => console.error('Error:', err)
    });
  }
}
```

## Complete Code Example

Here's a complete, functional Hello World API:

### Backend Code

**HelloController.java**
```java
package com.example.demo.controller;

import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import java.time.LocalDateTime;
import java.util.HashMap;
import java.util.Map;

/**
 * REST Controller for greeting endpoints.
 * 
 * @RestController combines @Controller and @ResponseBody
 * All methods return JSON by default.
 */
@RestController
@RequestMapping("/api")
public class HelloController {

    /**
     * Simple text greeting endpoint.
     * 
     * Access: GET /api/hello
     * Returns: "Hello, World!"
     */
    @GetMapping("/hello")
    public String sayHello() {
        return "Hello, World!";
    }

    /**
     * JSON greeting with timestamp.
     * 
     * Access: GET /api/welcome
     * Returns: {"message":"Welcome to our API!","timestamp":"2024-01-15T10:30:00"}
     */
    @GetMapping("/welcome")
    public Map<String, Object> getWelcome() {
        Map<String, Object> response = new HashMap<>();
        response.put("message", "Welcome to our API!");
        response.put("timestamp", LocalDateTime.now().toString());
        response.put("version", "1.0.0");
        return response;
    }

    /**
     * Personalized greeting with query parameter.
     * 
     * Access: GET /api/greet?name=John
     * Returns: {"message":"Hello, John!"}
     */
    @GetMapping("/greet")
    public Map<String, String> greetWithName(String name) {
        Map<String, String> response = new HashMap<>();
        String greetingName = (name != null && !name.isEmpty()) ? name : "World";
        response.put("message", "Hello, " + greetingName + "!");
        return response;
    }
}
```

**Greeting.java** (DTO for structured responses)
```java
package com.example.demo.dto;

/**
 * Data Transfer Object for greeting responses.
 * 
 * Using a DTO (Data Transfer Object) gives you:
 * - Control over what data is exposed
 * - Consistent response structure
 * - Ability to transform data before sending
 */
public class Greeting {
    
    private String message;
    private LocalDateTime timestamp;
    private String version;

    // Default constructor (required for JSON serialization)
    public Greeting() {}

    // Parameterized constructor
    public Greeting(String message) {
        this.message = message;
        this.timestamp = LocalDateTime.now();
        this.version = "1.0.0";
    }

    // Getters and Setters
    public String getMessage() { return message; }
    public void setMessage(String message) { this.message = message; }

    public LocalDateTime getTimestamp() { return timestamp; }
    public void setTimestamp(LocalDateTime timestamp) { this.timestamp = timestamp; }

    public String getVersion() { return version; }
    public void setVersion(String version) { this.version = version; }
}
```

### Frontend Code

**app.module.ts** (Required imports)
```typescript
import { HttpClientModule } from '@angular/common/http';

@NgModule({
  imports: [
    BrowserModule,
    HttpClientModule  // Required for HTTP requests
  ],
  declarations: [AppComponent],
  bootstrap: [AppComponent]
})
export class AppModule { }
```

**hello.service.ts**
```typescript
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface WelcomeResponse {
  message: string;
  timestamp: string;
  version: string;
}

export interface GreetResponse {
  message: string;
}

@Injectable({
  providedIn: 'root'
})
export class HelloService {
  private apiUrl = 'http://localhost:8080/api';

  constructor(private http: HttpClient) { }

  /**
   * Calls GET /api/hello
   * Returns simple string
   */
  getHello(): Observable<string> {
    return this.http.get<string>(`${this.apiUrl}/hello`);
  }

  /**
   * Calls GET /api/welcome
   * Returns JSON object with message, timestamp, version
   */
  getWelcome(): Observable<WelcomeResponse> {
    return this.http.get<WelcomeResponse>(`${this.apiUrl}/welcome`);
  }

  /**
   * Calls GET /api/greet?name=John
   * Returns personalized greeting
   */
  greet(name: string): Observable<GreetResponse> {
    // HttpClient automatically converts response to typed object
    return this.http.get<GreetResponse>(`${this.apiUrl}/greet?name=${name}`);
  }
}
```

**hello.component.ts**
```typescript
import { Component, OnInit } from '@angular/core';
import { HelloService, WelcomeResponse, GreetResponse } from './hello.service';

@Component({
  selector: 'app-hello',
  template: `
    <div class="container">
      <h1>Spring Boot + Angular Demo</h1>
      
      <button (click)="callHello()">Call /api/hello</button>
      <p>Result: {{ helloResult }}</p>
      
      <hr>
      
      <button (click)="callWelcome()">Call /api/welcome</button>
      <div *ngIf="welcomeResult">
        <p>Message: {{ welcomeResult.message }}</p>
        <p>Version: {{ welcomeResult.version }}</p>
        <p>Timestamp: {{ welcomeResult.timestamp }}</p>
      </div>
      
      <hr>
      
      <input [(ngModel)]="nameInput" placeholder="Enter your name">
      <button (click)="callGreet()">Greet Me</button>
      <p *ngIf="greetResult">Result: {{ greetResult.message }}</p>
    </div>
  `
})
export class HelloComponent implements OnInit {
  helloResult: string = '';
  welcomeResult: WelcomeResponse | null = null;
  greetResult: GreetResponse | null = null;
  nameInput: string = '';

  constructor(private helloService: HelloService) { }

  ngOnInit() {}

  callHello() {
    this.helloService.getHello().subscribe({
      next: (data) => this.helloResult = data,
      error: (err) => console.error('Error calling /hello:', err)
    });
  }

  callWelcome() {
    this.helloService.getWelcome().subscribe({
      next: (data) => this.welcomeResult = data,
      error: (err) => console.error('Error calling /welcome:', err)
    });
  }

  callGreet() {
    this.helloService.greet(this.nameInput).subscribe({
      next: (data) => this.greetResult = data,
      error: (err) => console.error('Error calling /greet:', err)
    });
  }
}
```

## REST API Endpoint Definitions

```
┌────────────────────────────────────────────────────────────────────────┐
│                      HELLO API ENDPOINTS                               │
├──────────────────┬──────────────┬─────────────────────────────────────┤
│       URL        │   METHOD     │           DESCRIPTION               │
├──────────────────┼──────────────┼─────────────────────────────────────┤
│ /api/hello       │     GET      │ Returns simple "Hello, World!"     │
│ /api/welcome     │     GET      │ Returns JSON with message,         │
│                  │              │ timestamp, and version             │
│ /api/greet       │     GET      │ Returns personalized greeting      │
│ ?name=John       │              │ (query parameter)                   │
└──────────────────┴──────────────┴─────────────────────────────────────┘

Example Responses:

GET /api/hello
HTTP/1.1 200 OK
Content-Type: text/plain

Hello, World!

────────────────────────────────────────
GET /api/welcome
HTTP/1.1 200 OK
Content-Type: application/json

{
  "message": "Welcome to our API!",
  "timestamp": "2024-01-15T10:30:00",
  "version": "1.0.0"
}

────────────────────────────────────────
GET /api/greet?name=John
HTTP/1.1 200 OK
Content-Type: application/json

{
  "message": "Hello, John!"
}
```

## Testing Your API

### Using Browser
- Open http://localhost:8080/api/hello

### Using curl
```bash
curl http://localhost:8080/api/hello
curl http://localhost:8080/api/welcome
curl "http://localhost:8080/api/greet?name=John"
```

## Industry Best Practices and Common Pitfalls

### Best Practices

1. **Use Proper HTTP Methods** - GET for fetching, POST for creating, etc.

2. **Return JSON Consistently** - Always return JSON, not mixed types

3. **Use DTOs** - Don't expose your database entities directly

4. **Version Your API** - Start with /api/v1/

5. **Document Your Endpoints** - Use Swagger/OpenAPI (we'll cover later)

### Common Pitfalls

1. **Returning null** - Always return an object or empty object `{}`

2. **Forgetting @ResponseBody** - Without it, Spring looks for a template

3. **Not Handling Errors** - Always have error handling in Angular

4. **CORS Issues** - Remember to configure CORS for frontend development

## Student Hands-On Exercises

### Exercise 1: Simple Endpoint (Easy)
Create a Spring Boot endpoint at GET /api/current-time that returns the current server time in JSON format: `{"time": "10:30:00"}`

### Exercise 2: Multiple Endpoints (Medium)
Create a CalculatorController with endpoints:
- GET /api/calc/add?a=5&b=3
- GET /api/calc/subtract?a=5&b=3
- GET /api/calc/multiply?a=5&b=3
- GET /api/calc/divide?a=6&b=3

### Exercise 3: Angular Integration (Medium)
Create an Angular component that displays 4 buttons, each calling one of the calculator endpoints and displaying the result.

### Exercise 4: REST Conventions (Hard)
Refactor the calculator to follow REST best practices:
- Use path variables: GET /api/calc/add/5/3
- Return consistent response format
- Handle edge cases (division by zero)

### Exercise 5: Error Handling (Hard)
Add error handling to your API:
- Return 400 Bad Request for invalid input
- Create a proper error response format: `{"error": "message", "code": 400}`

---

## Summary

In this lesson, you've learned:
- How to create a REST controller with @RestController
- How to use @GetMapping for GET endpoints
- How to return JSON from Spring Boot
- How Angular's HttpClient consumes these endpoints
- Complete code examples for both backend and frontend

Your first REST API is working! In the next lesson, we'll explore more advanced request handling.

---

**Next Lesson**: In the next lesson, we'll explore [Request Handling](06_Request_Handling.md) and learn how to handle path variables, query parameters, and request bodies.
