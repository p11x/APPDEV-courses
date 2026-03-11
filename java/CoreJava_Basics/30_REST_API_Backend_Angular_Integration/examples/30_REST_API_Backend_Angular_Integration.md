# Java Backend and Angular Frontend Integration

## Table of Contents
1. [Introduction to REST API](#introduction-to-rest-api)
2. [JSON and Java Objects](#json-and-java-objects)
3. [RESTful Controller Design](#restful-controller-design)
4. [Angular Service Integration](#angular-service-integration)
5. [Complete Full-Stack Example](#complete-full-stack-example)
6. [Best Practices](#best-practices)

---

## 1. Introduction to REST API

### What is REST?

**REST (Representational State Transfer)** is an architectural style for building web services.

```
┌─────────────────────────────────────────────────────────────┐
│                     REST API BASICS                           │
├─────────────────────────────────────────────────────────────┤
│                                                              │
│   HTTP Methods:                                             │
│   ┌──────────┬────────────────┬────────────────────────┐  │
│   │  Method  │   Description  │     Examples           │  │
│   ├──────────┼────────────────┼────────────────────────┤  │
│   │   GET    │  Read data     │  Get users, products   │  │
│   │   POST   │  Create data   │  Create new user       │  │
│   │   PUT    │  Update data   │  Update user info     │  │
│   │  DELETE  │  Delete data   │  Delete user          │  │
│   └──────────┴────────────────┴────────────────────────┘  │
│                                                              │
│   Status Codes:                                             │
│   200 - OK           201 - Created                          │
│   400 - Bad Request  404 - Not Found                        │
│   500 - Server Error                                         │
│                                                              │
└─────────────────────────────────────────────────────────────┘
```

### REST Endpoints Example

```
GET    /api/users          - Get all users
GET    /api/users/1        - Get user by ID
POST   /api/users          - Create new user
PUT    /api/users/1        - Update user
DELETE /api/users/1        - Delete user
```

---

## 2. JSON and Java Objects

### JSON Basics

```json
{
    "id": 1,
    "name": "John Doe",
    "email": "john@example.com",
    "age": 25,
    "isActive": true,
    "roles": ["admin", "user"]
}
```

### Java Class to JSON

```java
// Java POJO (Plain Old Java Object)
class User {
    private int id;
    private String name;
    private String email;
    private int age;
    private boolean isActive;
    private List<String> roles;
    
    // Constructors, getters, setters...
    
    // toString for display
}

// JSON Response (what Angular receives)
{
    "id": 1,
    "name": "John Doe",
    "email": "john@example.com",
    "age": 25,
    "isActive": true,
    "roles": ["admin", "user"]
}
```

---

## 3. RESTful Controller Design

### Simple REST Controller (without Spring)

```java
import java.util.*;

/**
 * User entity - Model for user data
 */
class User {
    private int id;
    private String name;
    private String email;
    
    public User(int id, String name, String email) {
        this.id = id;
        this.name = name;
        this.email = email;
    }
    
    // Getters and setters
    public int getId() { return id; }
    public void setId(int id) { this.id = id; }
    public String getName() { return name; }
    public void setName(String name) { this.name = name; }
    public String getEmail() { return email; }
    public void setEmail(String email) { this.email = email; }
    
    // Convert to JSON-like string
    public String toJson() {
        return "{\"id\":" + id + ",\"name\":\"" + name + "\",\"email\":\"" + email + "\"}";
    }
}

/**
 * Simple REST Controller simulation
 */
class UserController {
    private List<User> users = new ArrayList<>();
    private int nextId = 1;
    
    public UserController() {
        // Initialize with sample data
        users.add(new User(nextId++, "John Doe", "john@example.com"));
        users.add(new User(nextId++, "Jane Smith", "jane@example.com"));
    }
    
    // GET /api/users - Get all users
    public String getAllUsers() {
        StringBuilder json = new StringBuilder("[");
        for (int i = 0; i < users.size(); i++) {
            json.append(users.get(i).toJson());
            if (i < users.size() - 1) json.append(",");
        }
        json.append("]");
        return json.toString();
    }
    
    // GET /api/users/{id} - Get user by ID
    public String getUserById(int id) {
        for (User user : users) {
            if (user.getId() == id) {
                return user.toJson();
            }
        }
        return "{\"error\":\"User not found\"}";
    }
    
    // POST /api/users - Create user
    public String createUser(String name, String email) {
        User newUser = new User(nextId++, name, email);
        users.add(newUser);
        return newUser.toJson();
    }
    
    // PUT /api/users/{id} - Update user
    public String updateUser(int id, String name, String email) {
        for (User user : users) {
            if (user.getId() == id) {
                user.setName(name);
                user.setEmail(email);
                return user.toJson();
            }
        }
        return "{\"error\":\"User not found\"}";
    }
    
    // DELETE /api/users/{id} - Delete user
    public String deleteUser(int id) {
        for (int i = 0; i < users.size(); i++) {
            if (users.get(i).getId() == id) {
                users.remove(i);
                return "{\"message\":\"User deleted\"}";
            }
        }
        return "{\"error\":\"User not found\"}";
    }
}

/**
 * REST Controller Demo
 */
public class RESTControllerDemo {
    public static void main(String[] args) {
        UserController controller = new UserController();
        
        System.out.println("=== REST API DEMO ===\n");
        
        // GET all users
        System.out.println("GET /api/users");
        System.out.println(controller.getAllUsers());
        
        // GET user by ID
        System.out.println("\nGET /api/users/1");
        System.out.println(controller.getUserById(1));
        
        // POST - Create user
        System.out.println("\nPOST /api/users (Creating new user)");
        System.out.println(controller.createUser("Alice Brown", "alice@example.com"));
        
        // GET all users again
        System.out.println("\nGET /api/users (After creation)");
        System.out.println(controller.getAllUsers());
        
        // PUT - Update user
        System.out.println("\nPUT /api/users/1 (Updating user)");
        System.out.println(controller.updateUser(1, "John Updated", "john.new@example.com"));
        
        // DELETE - Delete user
        System.out.println("\nDELETE /api/users/2");
        System.out.println(controller.deleteUser(2));
        
        // GET all users again
        System.out.println("\nGET /api/users (Final)");
        System.out.println(controller.getAllUsers());
    }
}
```

---

## 4. Angular Service Integration

### How Angular Consumes REST APIs

```typescript
// Angular Service Example (TypeScript)
// This is what you'd create in your Angular frontend

import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface User {
  id: number;
  name: string;
  email: string;
}

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private apiUrl = 'http://localhost:8080/api/users';

  constructor(private http: HttpClient) { }

  // GET - Fetch all users
  getUsers(): Observable<User[]> {
    return this.http.get<User[]>(this.apiUrl);
  }

  // GET - Fetch single user
  getUser(id: number): Observable<User> {
    return this.http.get<User>(`${this.apiUrl}/${id}`);
  }

  // POST - Create user
  createUser(user: User): Observable<User> {
    return this.http.post<User>(this.apiUrl, user);
  }

  // PUT - Update user
  updateUser(id: number, user: User): Observable<User> {
    return this.http.put<User>(`${this.apiUrl}/${id}`, user);
  }

  // DELETE - Delete user
  deleteUser(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
```

### Angular Component Example

```typescript
// Angular Component Example
import { Component, OnInit } from '@angular/core';
import { UserService, User } from './user.service';

@Component({
  selector: 'app-user-list',
  template: `
    <h2>Users</h2>
    <button (click)="loadUsers()">Load Users</button>
    
    <div *ngFor="let user of users">
      <p>{{ user.id }} - {{ user.name }} - {{ user.email }}</p>
    </div>
  `
})
export class UserListComponent implements OnInit {
  users: User[] = [];

  constructor(private userService: UserService) { }

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers(): void {
    this.userService.getUsers().subscribe(data => {
      this.users = data;
      console.log('Users loaded:', this.users);
    });
  }
}
```

---

## 5. Complete Full-Stack Example

### Java Backend: Product Catalog

```java
import java.util.*;

/**
 * Product - Represents a product in e-commerce
 */
class Product {
    private int id;
    private String name;
    private String description;
    private double price;
    private String category;
    private int stock;
    
    public Product(int id, String name, String description, 
                  double price, String category, int stock) {
        this.id = id;
        this.name = name;
        this.description = description;
        this.price = price;
        this.category = category;
        this.stock = stock;
    }
    
    // Getters
    public int getId() { return id; }
    public String getName() { return name; }
    public String getDescription() { return description; }
    public double getPrice() { return price; }
    public String getCategory() { return category; }
    public int getStock() { return stock; }
    
    // Setters
    public void setName(String name) { this.name = name; }
    public void setDescription(String description) { this.description = description; }
    public void setPrice(double price) { this.price = price; }
    public void setCategory(String category) { this.category = category; }
    public void setStock(int stock) { this.stock = stock; }
    
    // Convert to JSON
    public String toJson() {
        return String.format(
            "{\"id\":%d,\"name\":\"%s\",\"description\":\"%s\",\"price\":%.2f,\"category\":\"%s\",\"stock\":%d}",
            id, name, description, price, category, stock
        );
    }
    
    // Create from JSON (simplified)
    public static Product fromJson(String json) {
        // Simplified parsing - in real app use Jackson/Gson
        return new Product(0, "Parsed", "From JSON", 0, "Unknown", 0);
    }
}

/**
 * ProductController - REST API endpoints
 */
class ProductController {
    private List<Product> products = new ArrayList<>();
    private int nextId = 1;
    
    public ProductController() {
        // Initialize sample products
        addSampleData();
    }
    
    private void addSampleData() {
        products.add(new Product(nextId++, "MacBook Pro", "Apple laptop", 2499.99, "Electronics", 50));
        products.add(new Product(nextId++, "iPhone 15", "Apple smartphone", 999.99, "Electronics", 100));
        products.add(new Product(nextId++, "AirPods Pro", "Wireless earbuds", 249.99, "Electronics", 200));
        products.add(new Product(nextId++, "T-Shirt", "Cotton t-shirt", 29.99, "Clothing", 500));
        products.add(new Product(nextId++, "Jeans", "Denim jeans", 79.99, "Clothing", 150));
    }
    
    // GET /api/products - Get all products
    public String getAllProducts() {
        StringBuilder json = new StringBuilder("[");
        for (int i = 0; i < products.size(); i++) {
            json.append(products.get(i).toJson());
            if (i < products.size() - 1) json.append(",");
        }
        json.append("]");
        return json.toString();
    }
    
    // GET /api/products/{id} - Get product by ID
    public String getProductById(int id) {
        for (Product p : products) {
            if (p.getId() == id) {
                return p.toJson();
            }
        }
        return "{\"error\":\"Product not found\"}";
    }
    
    // GET /api/products/category/{category} - Get products by category
    public String getProductsByCategory(String category) {
        StringBuilder json = new StringBuilder("[");
        boolean first = true;
        for (Product p : products) {
            if (p.getCategory().equalsIgnoreCase(category)) {
                if (!first) json.append(",");
                json.append(p.toJson());
                first = false;
            }
        }
        json.append("]");
        return json.toString();
    }
    
    // POST /api/products - Create product
    public String createProduct(String name, String description, 
                               double price, String category, int stock) {
        Product newProduct = new Product(nextId++, name, description, price, category, stock);
        products.add(newProduct);
        return newProduct.toJson();
    }
    
    // PUT /api/products/{id} - Update product
    public String updateProduct(int id, String name, String description,
                               double price, String category, int stock) {
        for (Product p : products) {
            if (p.getId() == id) {
                p.setName(name);
                p.setDescription(description);
                p.setPrice(price);
                p.setCategory(category);
                p.setStock(stock);
                return p.toJson();
            }
        }
        return "{\"error\":\"Product not found\"}";
    }
    
    // DELETE /api/products/{id} - Delete product
    public String deleteProduct(int id) {
        for (int i = 0; i < products.size(); i++) {
            if (products.get(i).getId() == id) {
                products.remove(i);
                return "{\"message\":\"Product deleted successfully\"}";
            }
        }
        return "{\"error\":\"Product not found\"}";
    }
    
    // GET /api/products/search?name={name} - Search products
    public String searchProducts(String name) {
        StringBuilder json = new StringBuilder("[");
        boolean first = true;
        for (Product p : products) {
            if (p.getName().toLowerCase().contains(name.toLowerCase())) {
                if (!first) json.append(",");
                json.append(p.toJson());
                first = false;
            }
        }
        json.append("]");
        return json.toString();
    }
}

/**
 * FullStackDemo - Demonstrates complete flow
 */
public class FullStackDemo {
    public static void main(String[] args) {
        ProductController controller = new ProductController();
        
        System.out.println("=== FULL-STACK DEMO: E-COMMERCE API ===\n");
        
        // Get all products
        System.out.println("1. GET /api/products");
        System.out.println(controller.getAllProducts());
        
        // Get products by category
        System.out.println("\n2. GET /api/products/category/Electronics");
        System.out.println(controller.getProductsByCategory("Electronics"));
        
        // Search products
        System.out.println("\n3. GET /api/products/search?name=Phone");
        System.out.println(controller.searchProducts("Phone"));
        
        // Create new product
        System.out.println("\n4. POST /api/products - Create new product");
        System.out.println(controller.createProduct(
            "iPad Pro", 
            "Apple tablet", 
            1099.99, 
            "Electronics", 
            75
        ));
        
        // Update product
        System.out.println("\n5. PUT /api/products/1 - Update product");
        System.out.println(controller.updateProduct(
            1, 
            "MacBook Pro M3", 
            "Updated Apple laptop", 
            2799.99, 
            "Electronics", 
            30
        ));
        
        // Delete product
        System.out.println("\n6. DELETE /api/products/3");
        System.out.println(controller.deleteProduct(3));
        
        // Get all products again
        System.out.println("\n7. GET /api/products - Final state");
        System.out.println(controller.getAllProducts());
        
        System.out.println("\n=== Integration with Angular ===");
        System.out.println("In Angular, you would call these endpoints:");
        System.out.println("  - this.http.get<Product[]>('/api/products')");
        System.out.println("  - this.http.get<Product[]>('/api/products/category/Electronics')");
        System.out.println("  - this.http.post<Product>('/api/products', productData)");
    }
}
```

---

## 6. Best Practices

### Java Backend Best Practices

1. **Use proper HTTP methods** - GET for reading, POST for creating, PUT for updating, DELETE for deleting
2. **Return appropriate status codes** - 200 OK, 201 Created, 404 Not Found, 500 Server Error
3. **Use proper JSON serialization** - Use Jackson or Gson library
4. **Validate input** - Check for null values and invalid data
5. **Use DTOs** - Data Transfer Objects for API responses

### Angular Frontend Best Practices

1. **Use services** - Centralize HTTP calls in services
2. **Handle errors** - Always handle error responses
3. **Use types** - Define interfaces for type safety
4. **Subscribe properly** - Handle subscriptions and unsubscriptions
5. **Use async pipe** - Prefer async pipe for displaying data

### JSON Naming Convention

```java
// Java uses camelCase
class User {
    private String firstName;
    private String lastName;
    private Date createdAt;
    
    // Jackson annotations for JSON
    @JsonProperty("first_name")  // snake_case in JSON
    public String getFirstName() { return firstName; }
}
```

---

## Summary

### Key Takeaways

1. **REST API** - Standard way for Angular to communicate with Java backend
2. **JSON** - Data format exchanged between frontend and backend
3. **HTTP Methods** - GET (read), POST (create), PUT (update), DELETE (delete)
4. **Status Codes** - 200 (OK), 201 (Created), 404 (Not Found), 500 (Error)
5. **Integration** - Angular HttpClient maps to Java REST endpoints

### Full-Stack Architecture

```
┌─────────────────────────────────────────────────────────────┐
│                   FULL-STACK ARCHITECTURE                    │
├─────────────────────────────────────────────────────────────┤
│                                                              │
│   ANGULAR FRONTEND                                          │
│   ┌─────────────────┐                                      │
│   │   Components    │  - UI components                      │
│   │   Services      │  - HTTP calls                         │
│   │   Models        │  - TypeScript interfaces              │
│   └────────┬────────┘                                      │
│            │ HTTP (JSON)                                    │
│            ▼                                                │
│   ┌─────────────────┐                                      │
│   │  REST API       │  - Endpoints                          │
│   │  Controllers    │  - Request handling                   │
│   └────────┬────────┘                                      │
│            │                                                │
│   ┌────────▼────────┐                                      │
│   │  Java Backend   │  - Business logic                    │
│   │  Services       │  - Data processing                   │
│   │  Repositories   │  - Database access                   │
│   └─────────────────┘                                      │
│                                                              │
└─────────────────────────────────────────────────────────────┘
```

---

*This completes your Java learning journey! You now have all the fundamentals needed to build full-stack applications with Angular and Java!*

**Happy Coding! 🚀**
