# Request Handling

## Concept Title and Overview

In this lesson, you'll learn how Spring Boot handles different types of client requests. We'll cover path variables (URL parameters), query parameters (filtering), request bodies (POST/PUT data), and request headers.

## Real-World Importance and Context

When your Angular app communicates with your Spring Boot backend, it needs to send various types of data:

- **Path Variables**: Like `/users/123` - identifying a specific resource
- **Query Parameters**: Like `/users?page=2&size=10` - filtering or pagination
- **Request Body**: When creating or updating - sending complex data
- **Request Headers**: Authentication tokens, content types

Understanding how to handle each type is essential for building real-world APIs.

## Detailed Step-by-Step Explanation

### Path Variables with @PathVariable

Path variables are part of the URL path itself. They're perfect for identifying specific resources:

```
┌─────────────────────────────────────────────────────────────────────────┐
│                      PATH VARIABLES                                      │
├─────────────────────────────────────────────────────────────────────────┤
│                                                                         │
│  URL Pattern:  /api/users/{id}                                          │
│                                                                         │
│  Example URLs:                                                          │
│  • /api/users/1       → id = 1                                         │
│  • /api/users/123     → id = 123                                       │
│  • /api/users/abc     → ERROR! (id must be a number)                   │
│                                                                         │
│  Use Cases:                                                             │
│  • GET /api/users/1         → Get user with ID 1                      │
│  • PUT /api/products/5      → Update product with ID 5               │
│  • DELETE /api/orders/10    → Delete order with ID 10                 │
│                                                                         │
└─────────────────────────────────────────────────────────────────────────┘
```

**Example Implementation:**

```java
@GetMapping("/users/{id}")
public User getUserById(@PathVariable Long id) {
    return userService.findById(id);
}

// With explicit variable name mapping
@GetMapping("/users/{userId}")
public User getUserById(@PathVariable("userId") Long id) {
    return userService.findById(id);
}

// Multiple path variables
@GetMapping("/users/{userId}/orders/{orderId}")
public Order getOrder(@PathVariable Long userId, @PathVariable Long orderId) {
    return orderService.findByUserAndId(userId, orderId);
}
```

### Query Parameters with @RequestParam

Query parameters come after the `?` in the URL. They're great for filtering, pagination, and optional parameters:

```
┌─────────────────────────────────────────────────────────────────────────┐
│                      QUERY PARAMETERS                                   │
├─────────────────────────────────────────────────────────────────────────┤
│                                                                         │
│  URL:  /api/users?page=0&size=10&sort=name                             │
│                                                                         │
│  Parameters:                                                           │
│  • page = 0           → First page                                    │
│  • size = 10          → 10 items per page                            │
│  • sort = name        → Sort by name field                           │
│                                                                         │
│  Use Cases:                                                            │
│  • /api/products?category=electronics  → Filter by category          │
│  • /api/search?q=iphone&limit=5         → Search with limit          │
│  • /api/users?active=true                → Filter active users        │
│                                                                         │
└─────────────────────────────────────────────────────────────────────────┘
```

**Example Implementation:**

```java
// Basic query parameter
@GetMapping("/users")
public List<User> getUsers(@RequestParam String category) {
    return userService.findByCategory(category);
}

// Optional query parameter with default value
@GetMapping("/search")
public List<Product> search(
    @RequestParam String query,
    @RequestParam(defaultValue = "10") int limit
) {
    return productService.search(query, limit);
}

// All parameters optional
@GetMapping("/products")
public List<Product> getProducts(
    @RequestParam(required = false) String category,
    @RequestParam(required = false) Double minPrice,
    @RequestParam(required = false) Double maxPrice
) {
    return productService.findByFilters(category, minPrice, maxPrice);
}
```

### Request Body with @RequestBody

Request bodies are used when sending complex data, typically with POST and PUT requests:

```
┌─────────────────────────────────────────────────────────────────────────┐
│                      REQUEST BODY                                        │
├─────────────────────────────────────────────────────────────────────────┤
│                                                                         │
│  POST /api/users                                                        │
│  Content-Type: application/json                                         │
│                                                                         │
│  Request Body (JSON):                                                   │
│  {                                                                      │
│    "name": "John Doe",                                                 │
│    "email": "john@example.com",                                       │
│    "age": 30                                                           │
│  }                                                                      │
│                                                                         │
│  Spring automatically:                                                 │
│  1. Reads the HTTP request body                                        │
│  2. Deserializes JSON to Java object                                   │
│  3. Binds to the method parameter                                       │
│                                                                         │
└─────────────────────────────────────────────────────────────────────────┘
```

**Example Implementation:**

```java
// POST - Creating a new resource
@PostMapping("/users")
public User createUser(@RequestBody CreateUserRequest request) {
    // request contains the deserialized JSON data
    return userService.create(request.getName(), request.getEmail());
}

// PUT - Complete update
@PutMapping("/users/{id}")
public User updateUser(@PathVariable Long id, @RequestBody UpdateUserRequest request) {
    return userService.update(id, request);
}

// PATCH - Partial update
@PatchMapping("/users/{id}")
public User patchUser(@PathVariable Long id, @RequestBody Map<String, Object> updates) {
    return userService.patch(id, updates);
}
```

### Request Headers Access

Headers contain metadata about the request, like authentication tokens:

```
┌─────────────────────────────────────────────────────────────────────────┐
│                      REQUEST HEADERS                                     │
├─────────────────────────────────────────────────────────────────────────┤
│                                                                         │
│  Common Headers:                                                        │
│  • Authorization    → Bearer token for authentication                  │
│  • Content-Type    → What type of data (application/json)             │
│  • Accept          → What response format client wants                  │
│  • X-Request-ID    → Custom tracking ID                                │
│  • Cookie          → Session information                                │
│                                                                         │
│  Example Request:                                                       │
│  GET /api/protected HTTP/1.1                                           │
│  Host: localhost:8080                                                  │
│  Authorization: Bearer eyJhbGciOiJIUzI1NiJ9...                        │
│  Accept: application/json                                              │
│                                                                         │
└─────────────────────────────────────────────────────────────────────────┘
```

**Example Implementation:**

```java
// Single header
@GetMapping("/profile")
public Profile getProfile(@RequestHeader("Authorization") String token) {
    // Extract user from token
    return userService.getProfileFromToken(token);
}

// Multiple headers
@PostMapping("/upload")
public UploadResponse upload(
    @RequestHeader("Authorization") String token,
    @RequestHeader("Content-Type") String contentType
) {
    return fileService.upload(token, contentType);
}

// Header with default value
@GetMapping("/version")
public VersionInfo getVersion(
    @RequestHeader(value = "X-API-Version", defaultValue = "v1") String version
) {
    return versionService.getVersion(version);
}

// All headers as a Map
@GetMapping("/debug")
public Map<String, String> debug(@RequestHeader Map<String, String> headers) {
    headers.forEach((key, value) -> {
        System.out.println(key + ": " + value);
    });
    return headers;
}
```

## Complete Working Examples

### UserController - Complete Example

```java
package com.example.demo.controller;

import com.example.demo.dto.CreateUserRequest;
import com.example.demo.dto.UpdateUserRequest;
import com.example.demo.dto.UserResponse;
import com.example.demo.model.User;
import com.example.demo.service.UserService;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("/api/users")
public class UserController {

    private final UserService userService;

    public UserController(UserService userService) {
        this.userService = userService;
    }

    // ┌─────────────────────────────────────────────────────────────────┐
    // │ GET /api/users - Get all users with optional filtering        │
    // └─────────────────────────────────────────────────────────────────┘
    @GetMapping
    public ResponseEntity<List<UserResponse>> getAllUsers(
            @RequestParam(required = false) String role,
            @RequestParam(required = false) Boolean active,
            @RequestParam(defaultValue = "0") int page,
            @RequestParam(defaultValue = "10") int size
    ) {
        List<User> users = userService.findAll(role, active, page, size);
        List<UserResponse> response = users.stream()
                .map(UserResponse::fromEntity)
                .toList();
        return ResponseEntity.ok(response);
    }

    // ┌─────────────────────────────────────────────────────────────────┐
    // │ GET /api/users/{id} - Get user by ID (path variable)           │
    // └─────────────────────────────────────────────────────────────────┘
    @GetMapping("/{id}")
    public ResponseEntity<UserResponse> getUserById(@PathVariable Long id) {
        return userService.findById(id)
                .map(user -> ResponseEntity.ok(UserResponse.fromEntity(user)))
                .orElse(ResponseEntity.notFound().build());
    }

    // ┌─────────────────────────────────────────────────────────────────┐
    // │ POST /api/users - Create new user (request body)               │
    // └─────────────────────────────────────────────────────────────────┘
    @PostMapping
    public ResponseEntity<UserResponse> createUser(@RequestBody CreateUserRequest request) {
        User user = userService.create(
                request.getName(),
                request.getEmail(),
                request.getRole()
        );
        return ResponseEntity
                .status(HttpStatus.CREATED)
                .body(UserResponse.fromEntity(user));
    }

    // ┌─────────────────────────────────────────────────────────────────┐
    // │ PUT /api/users/{id} - Complete update (path variable + body)  │
    // └─────────────────────────────────────────────────────────────────┘
    @PutMapping("/{id}")
    public ResponseEntity<UserResponse> updateUser(
            @PathVariable Long id,
            @RequestBody UpdateUserRequest request
    ) {
        return userService.update(id, request.getName(), request.getEmail(), request.isActive())
                .map(user -> ResponseEntity.ok(UserResponse.fromEntity(user)))
                .orElse(ResponseEntity.notFound().build());
    }

    // ┌─────────────────────────────────────────────────────────────────┐
    // │ DELETE /api/users/{id} - Delete user                           │
    // └─────────────────────────────────────────────────────────────────┘
    @DeleteMapping("/{id}")
    public ResponseEntity<Void> deleteUser(@PathVariable Long id) {
        if (userService.delete(id)) {
            return ResponseEntity.noContent().build();
        }
        return ResponseEntity.notFound().build();
    }

    // ┌─────────────────────────────────────────────────────────────────┐
    // │ PATCH /api/users/{id}/activate - Activate user (custom action)│
    // └─────────────────────────────────────────────────────────────────┘
    @PatchMapping("/{id}/activate")
    public ResponseEntity<UserResponse> activateUser(@PathVariable Long id) {
        return userService.activate(id)
                .map(user -> ResponseEntity.ok(UserResponse.fromEntity(user)))
                .orElse(ResponseEntity.notFound().build());
    }
}
```

### Angular Service for Consuming These Endpoints

```typescript
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface User {
  id: number;
  name: string;
  email: string;
  role: string;
  active: boolean;
}

export interface CreateUserRequest {
  name: string;
  email: string;
  role: string;
}

export interface UpdateUserRequest {
  name: string;
  email: string;
  active: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private apiUrl = 'http://localhost:8080/api/users';

  constructor(private http: HttpClient) {}

  // GET /api/users
  getUsers(role?: string, active?: boolean, page: number = 0, size: number = 10): Observable<User[]> {
    let params = new HttpParams()
      .set('page', page.toString())
      .set('size', size.toString());

    if (role) params = params.set('role', role);
    if (active !== undefined) params = params.set('active', active.toString());

    return this.http.get<User[]>(this.apiUrl, { params });
  }

  // GET /api/users/1
  getUserById(id: number): Observable<User> {
    return this.http.get<User>(`${this.apiUrl}/${id}`);
  }

  // POST /api/users
  createUser(request: CreateUserRequest): Observable<User> {
    return this.http.post<User>(this.apiUrl, request);
  }

  // PUT /api/users/1
  updateUser(id: number, request: UpdateUserRequest): Observable<User> {
    return this.http.put<User>(`${this.apiUrl}/${id}`, request);
  }

  // DELETE /api/users/1
  deleteUser(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  // PATCH /api/users/1/activate
  activateUser(id: number): Observable<User> {
    return this.http.patch<User>(`${this.apiUrl}/${id}/activate`, {});
  }
}
```

### Angular Component Example

```typescript
import { Component, OnInit } from '@angular/core';
import { UserService, User, CreateUserRequest, UpdateUserRequest } from './user.service';

@Component({
  selector: 'app-user-manager',
  template: `
    <h2>User Management</h2>
    
    <!-- Filter Section -->
    <div class="filters">
      <input [(ngModel)]="filterRole" placeholder="Filter by role">
      <button (click)="loadUsers()">Apply Filters</button>
    </div>

    <!-- User List -->
    <table>
      <tr *ngFor="let user of users">
        <td>{{ user.name }}</td>
        <td>{{ user.email }}</td>
        <td>{{ user.role }}</td>
        <td>{{ user.active ? 'Active' : 'Inactive' }}</td>
        <td>
          <button (click)="deleteUser(user.id)">Delete</button>
        </td>
      </tr>
    </table>

    <!-- Create User Form -->
    <h3>Create New User</h3>
    <form (ngSubmit)="createUser()">
      <input [(ngModel)]="newUser.name" name="name" placeholder="Name">
      <input [(ngModel)]="newUser.email" name="email" placeholder="Email">
      <select [(ngModel)]="newUser.role" name="role">
        <option value="USER">User</option>
        <option value="ADMIN">Admin</option>
      </select>
      <button type="submit">Create</button>
    </form>
  `
})
export class UserManagerComponent implements OnInit {
  users: User[] = [];
  filterRole: string = '';
  newUser: CreateUserRequest = { name: '', email: '', role: 'USER' };

  constructor(private userService: UserService) {}

  ngOnInit() {
    this.loadUsers();
  }

  loadUsers() {
    this.userService.getUsers(this.filterRole || undefined).subscribe({
      next: (users) => this.users = users,
      error: (err) => console.error('Error loading users:', err)
    });
  }

  createUser() {
    this.userService.createUser(this.newUser).subscribe({
      next: (user) => {
        this.users.push(user);
        this.newUser = { name: '', email: '', role: 'USER' };
      },
      error: (err) => console.error('Error creating user:', err)
    });
  }

  deleteUser(id: number) {
    this.userService.deleteUser(id).subscribe({
      next: () => {
        this.users = this.users.filter(u => u.id !== id);
      },
      error: (err) => console.error('Error deleting user:', err)
    });
  }
}
```

## Summary of Request Types

```
┌────────────────────────────────────────────────────────────────────────┐
│                    REQUEST HANDLING SUMMARY                           │
├──────────────┬───────────────────┬───────────────────────────────────┤
│   TYPE       │   ANNOTATION      │         EXAMPLE URL               │
├──────────────┼───────────────────┼───────────────────────────────────┤
│ Path         │ @PathVariable     │ GET /users/123                    │
│ Variable     │                   │                                   │
├──────────────┼───────────────────┼───────────────────────────────────┤
│ Query        │ @RequestParam     │ GET /users?role=admin&active=true │
│ Parameter    │                   │                                   │
├──────────────┼───────────────────┼───────────────────────────────────┤
│ Request      │ @RequestBody      │ POST /users (with JSON body)     │
│ Body         │                   │                                   │
├──────────────┼───────────────────┼───────────────────────────────────┤
│ Header       │ @RequestHeader    │ Authorization: Bearer token       │
│              │                   │                                   │
└──────────────┴───────────────────┴───────────────────────────────────┘
```

## Industry Best Practices and Common Pitfalls

### Best Practices

1. **Use Path Variables for Resource IDs** - `/users/1` not `/users?id=1`

2. **Use Query Parameters for Filtering** - `/users?role=admin` for optional filters

3. **Use Request Body for Complex Data** - POST/PUT with structured JSON

4. **Validate Input** - Always validate path variables and request parameters

5. **Document Your API** - Specify which parameters are required vs optional

### Common Pitfalls

1. **Type Mismatches** - Path variable `{id}` must match Long/Integer parameter type

2. **Missing Parameters** - Don't forget required @RequestParam values

3. **JSON Parsing Errors** - Ensure request body matches expected format

4. **Null Values** - Handle optional parameters properly

## Student Hands-On Exercises

### Exercise 1: Path Variables (Easy)
Create an endpoint GET /api/products/{id} that returns product details. What happens when the product doesn't exist?

### Exercise 2: Query Parameters (Easy)
Create GET /api/search?q=keyword&limit=5 that searches products and returns maximum `limit` results.

### Exercise 3: Request Body (Medium)
Create POST /api/orders with a request body containing:
```json
{
  "productId": 1,
  "quantity": 3,
  "shippingAddress": "123 Main St"
}
```

### Exercise 4: Combined Parameters (Medium)
Create GET /api/orders/{orderId}/items/{itemId} that retrieves a specific item from a specific order.

### Exercise 5: Angular Form Integration (Hard)
Create an Angular form that allows users to:
- Search products by name (query param)
- Filter by category (query param)
- Sort by price (query param)
- Navigate to product details (path variable)

---

## Summary

In this lesson, you've learned:
- How to use @PathVariable for URL path parameters
- How to use @RequestParam for query parameters
- How to use @RequestBody for POST/PUT request data
- How to use @RequestHeader for HTTP headers
- Complete examples for both Spring Boot and Angular

You now know how to handle all types of client requests. In the next lesson, we'll explore how to test these APIs.

---

**Next Lesson**: In the next lesson, we'll explore [API Testing](07_API_Testing.md) and learn how to use Postman and VS Code extensions to test your APIs.
