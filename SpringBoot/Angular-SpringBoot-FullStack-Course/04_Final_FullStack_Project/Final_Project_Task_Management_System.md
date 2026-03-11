# Final Project: Task Management System

## Project Overview

Welcome to the final project! In this comprehensive task, you'll build a complete Task Management System that integrates all the concepts learned throughout this course. This project will demonstrate your ability to build production-ready full-stack applications.

## Project Requirements

### Backend (Spring Boot)

Create a REST API with the following features:

1. **User Authentication**
   - User registration
   - Login with JWT token generation
   - Protected routes with JWT validation
   - Role-based access (USER, ADMIN)

2. **Task CRUD Operations**
   - Create new tasks
   - Read all tasks (with pagination)
   - Read single task
   - Update task
   - Delete task

3. **File Attachment Support**
   - Upload files to tasks
   - Retrieve attached files

4. **Proper Exception Handling**
   - Global exception handler
   - Custom exception classes
   - Validation errors

### Frontend (Angular)

Create a responsive web application with:

1. **User Authentication**
   - Registration page
   - Login page with JWT token
   - Token storage (localStorage)
   - Protected routes (guards)

2. **Dashboard**
   - Display paginated tasks
   - Filter and sort options

3. **Task Management**
   - Create task form with validation
   - Edit task form
   - Delete task functionality

4. **File Upload**
   - Upload files to tasks
   - View attached files

## Project Structure

```
task-manager/
├── backend/                          # Spring Boot Application
│   ├── src/main/java/com/example/taskmanager/
│   │   ├── config/                  # Configuration classes
│   │   ├── controller/              # REST controllers
│   │   ├── service/                 # Business logic
│   │   ├── repository/              # Data access
│   │   ├── model/                   # Entities
│   │   ├── dto/                     # Data transfer objects
│   │   ├── security/               # JWT and security
│   │   └── exception/               # Exception handlers
│   ├── src/main/resources/
│   │   └── application.properties
│   └── pom.xml
│
└── frontend/                        # Angular Application
    └── src/app/
        ├── services/                # API services
        ├── components/              # UI components
        ├── guards/                  # Route guards
        └── models/                  # TypeScript interfaces
```

## API Specification

### Authentication Endpoints

```
┌────────────────────────────────────────────────────────────────────────┐
│                     AUTHENTICATION API                                 │
├──────────────────┬──────────────┬─────────────────────────────────────┤
│       URL        │   METHOD     │           DESCRIPTION               │
├──────────────────┼──────────────┼─────────────────────────────────────┤
│ /api/auth/register│    POST     │ Register new user                  │
│ /api/auth/login   │    POST     │ Login and get JWT token            │
└──────────────────┴──────────────┴─────────────────────────────────────┘
```

### Task Endpoints

```
┌────────────────────────────────────────────────────────────────────────┐
│                        TASK API                                        │
├──────────────────┬──────────────┬─────────────────────────────────────┤
│       URL        │   METHOD     │           DESCRIPTION               │
├──────────────────┼──────────────┼─────────────────────────────────────┤
│ /api/tasks       │     GET      │ Get all tasks (paginated)          │
│ /api/tasks       │    POST      │ Create new task                    │
│ /api/tasks/{id}  │    GET      │ Get task by ID                     │
│ /api/tasks/{id}  │     PUT      │ Update task                        │
│ /api/tasks/{id}  │   DELETE     │ Delete task                        │
│ /api/tasks/{id}/ │    POST      │ Upload file to task                │
│   upload         │              │                                     │
│ /api/tasks/{id}/ │    GET      │ Get files for task                 │
│   files          │              │                                     │
└──────────────────┴──────────────┴─────────────────────────────────────┘
```

## Database Schema

### Users Table

```sql
CREATE TABLE users (
    id BIGINT AUTO_INCREMENT PRIMARY KEY,
    username VARCHAR(50) NOT NULL UNIQUE,
    email VARCHAR(100) NOT NULL UNIQUE,
    password VARCHAR(255) NOT NULL,
    role VARCHAR(20) DEFAULT 'USER',
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
```

### Tasks Table

```sql
CREATE TABLE tasks (
    id BIGINT AUTO_INCREMENT PRIMARY KEY,
    title VARCHAR(200) NOT NULL,
    description TEXT,
    status VARCHAR(20) DEFAULT 'PENDING',
    priority VARCHAR(20) DEFAULT 'MEDIUM',
    user_id BIGINT NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE CASCADE
);
```

### Task Attachments Table

```sql
CREATE TABLE task_attachments (
    id BIGINT AUTO_INCREMENT PRIMARY KEY,
    task_id BIGINT NOT NULL,
    file_name VARCHAR(255) NOT NULL,
    file_path VARCHAR(500) NOT NULL,
    file_size BIGINT,
    content_type VARCHAR(100),
    uploaded_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (task_id) REFERENCES tasks(id) ON DELETE CASCADE
);
```

## Step-by-Step Implementation Guide

### Phase 1: Backend Setup

1. **Create Spring Boot Project**
   - Use Spring Initializr
   - Add: Web, Data JPA, Security, Validation, MySQL/H2

2. **Configure Database**
   - Set up application.properties
   - Configure HikariCP connection pool

3. **Create Entities**
   - User entity with JPA annotations
   - Task entity
   - TaskAttachment entity

4. **Create Repositories**
   - UserRepository
   - TaskRepository
   - AttachmentRepository

5. **Implement Security**
   - JWT configuration
   - Security filter chain
   - UserDetailsService

### Phase 2: Backend Implementation

6. **Create DTOs**
   - RegisterRequest, LoginRequest
   - TaskRequest, TaskResponse

7. **Implement Services**
   - AuthService for authentication
   - TaskService for task CRUD

8. **Create Controllers**
   - AuthController
   - TaskController

9. **Add Exception Handling**
   - GlobalExceptionHandler
   - Custom exceptions

### Phase 3: Frontend Setup

10. **Create Angular Project**
    - Install Angular CLI
    - Generate new project
    - Add HttpClientModule

11. **Create Services**
    - AuthService (login, register, token)
    - TaskService (CRUD operations)

12. **Create Components**
    - LoginComponent
    - RegisterComponent
    - DashboardComponent
    - TaskFormComponent

### Phase 4: Integration

13. **Implement Guards**
    - AuthGuard for protected routes

14. **Connect Frontend to Backend**
    - Configure API URLs
    - Handle JWT tokens

15. **Test Complete Flow**
    - User registration
    - User login
    - Task CRUD operations
    - File upload

## Code Examples

### Backend: User Entity

```java
@Entity
@Table(name = "users")
public class User {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;
    
    @Column(unique = true, nullable = false)
    private String username;
    
    @Column(unique = true, nullable = false)
    private String email;
    
    @Column(nullable = false)
    private String password;
    
    private String role;
    
    @Column(name = "created_at")
    private LocalDateTime createdAt;
    
    // Constructors, getters, setters
}
```

### Backend: Task Controller

```java
@RestController
@RequestMapping("/api/tasks")
@PreAuthorize("isAuthenticated()")
public class TaskController {
    
    private final TaskService taskService;
    
    @GetMapping
    public ResponseEntity<Page<TaskResponse>> getTasks(
            @PageableDefault(size = 10) Pageable pageable) {
        return ResponseEntity.ok(taskService.getTasks(pageable));
    }
    
    @PostMapping
    public ResponseEntity<TaskResponse> createTask(@Valid @RequestBody TaskRequest request) {
        return ResponseEntity.status(HttpStatus.CREATED).body(taskService.createTask(request));
    }
    
    @GetMapping("/{id}")
    public ResponseEntity<TaskResponse> getTask(@PathVariable Long id) {
        return taskService.getTaskById(id)
                .map(ResponseEntity::ok)
                .orElse(ResponseEntity.notFound().build());
    }
    
    @PutMapping("/{id}")
    public ResponseEntity<TaskResponse> updateTask(@PathVariable Long id, 
            @Valid @RequestBody TaskRequest request) {
        return taskService.updateTask(id, request)
                .map(ResponseEntity::ok)
                .orElse(ResponseEntity.notFound().build());
    }
    
    @DeleteMapping("/{id}")
    public ResponseEntity<Void> deleteTask(@PathVariable Long id) {
        taskService.deleteTask(id);
        return ResponseEntity.noContent().build();
    }
}
```

### Frontend: Auth Service

```typescript
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, tap } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private apiUrl = 'http://localhost:8080/api/auth';
  private tokenKey = 'taskmanager_token';
  
  private isLoggedInSubject = new BehaviorSubject<boolean>(this.hasToken());
  isLoggedIn$ = this.isLoggedInSubject.asObservable();

  constructor(private http: HttpClient) {}

  register(username: string, email: string, password: string) {
    return this.http.post<any>(`${this.apiUrl}/register`, { username, email, password });
  }

  login(username: string, password: string) {
    return this.http.post<any>(`${this.apiUrl}/login`, { username, password }).pipe(
      tap(response => {
        localStorage.setItem(this.tokenKey, response.token);
        this.isLoggedInSubject.next(true);
      })
    );
  }

  logout() {
    localStorage.removeItem(this.tokenKey);
    this.isLoggedInSubject.next(false);
  }

  getToken() {
    return localStorage.getItem(this.tokenKey);
  }

  private hasToken(): boolean {
    return !!localStorage.getItem(this.tokenKey);
  }
}
```

### Frontend: Task Service

```typescript
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Task {
  id?: number;
  title: string;
  description: string;
  status: string;
  priority: string;
}

@Injectable({ providedIn: 'root' })
export class TaskService {
  private apiUrl = 'http://localhost:8080/api/tasks';

  constructor(private http: HttpClient) {}

  getTasks(page: number = 0, size: number = 10): Observable<any> {
    const params = new HttpParams()
      .set('page', page.toString())
      .set('size', size.toString());
    return this.http.get<any>(this.apiUrl, { params });
  }

  getTask(id: number): Observable<Task> {
    return this.http.get<Task>(`${this.apiUrl}/${id}`);
  }

  createTask(task: Task): Observable<Task> {
    return this.http.post<Task>(this.apiUrl, task);
  }

  updateTask(id: number, task: Task): Observable<Task> {
    return this.http.put<Task>(`${this.apiUrl}/${id}`, task);
  }

  deleteTask(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  uploadFile(taskId: number, file: File): Observable<any> {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post(`${this.apiUrl}/${taskId}/upload`, formData);
  }
}
```

### Frontend: Auth Guard

```typescript
import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AuthService } from './auth.service';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) {}

  canActivate(): boolean {
    if (this.authService.isLoggedIn$) {
      return true;
    }
    this.router.navigate(['/login']);
    return false;
  }
}
```

## Implementation Checklist

### Backend Checklist
- [ ] Project setup with Spring Initializr
- [ ] Database configuration
- [ ] User entity and repository
- [ ] Task entity and repository
- [ ] Attachment support
- [ ] JWT authentication
- [ ] Task CRUD with pagination
- [ ] Exception handling
- [ ] CORS configuration

### Frontend Checklist
- [ ] Angular project setup
- [ ] Auth service
- [ ] Task service
- [ ] Login component
- [ ] Register component
- [ ] Dashboard with pagination
- [ ] Task form
- [ ] Auth guard
- [ ] File upload

## Testing Your Application

### Test Authentication
```bash
# Register
curl -X POST http://localhost:8080/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{"username":"john","email":"john@example.com","password":"password123"}'

# Login
curl -X POST http://localhost:8080/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"john","password":"password123"}'
```

### Test Tasks (with JWT)
```bash
# Get tasks
curl http://localhost:8080/api/tasks \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"

# Create task
curl -X POST http://localhost:8080/api/tasks \
  -H "Authorization: Bearer YOUR_JWT_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{"title":"My Task","description":"Task description","priority":"HIGH"}'
```

## Exercises for Students

### Exercise 1: Basic Setup (Easy)
Set up the Spring Boot project with all dependencies.

### Exercise 2: User Authentication (Medium)
Implement complete JWT authentication.

### Exercise 3: Task CRUD (Medium)
Implement task CRUD with pagination.

### Exercise 4: Frontend Integration (Medium)
Build Angular frontend with login and dashboard.

### Exercise 5: File Upload (Hard)
Add file upload functionality to tasks.

### Exercise 6: Polish and Deploy (Hard)
Add styling, error handling, and deploy to cloud.

---

## Summary

Congratulations! You've completed the full-stack development course. This Task Management System project demonstrates:

- Spring Boot REST API development
- JWT authentication and security
- Database integration with JPA
- Angular frontend development
- File upload handling
- Production deployment

This comprehensive project brings together all the concepts from the course and prepares you for real-world full-stack development.

## Congratulations!

You've completed the Angular-Spring Boot Full-Stack Development Course! You now have the skills to build production-ready full-stack applications.

---

**Keep Learning!** The technology world is always evolving. Continue exploring:
- Spring Boot Microservices
- Angular Advanced Topics
- Cloud Deployment (AWS, GCP, Azure)
- CI/CD Pipelines

Good luck with your development career!
