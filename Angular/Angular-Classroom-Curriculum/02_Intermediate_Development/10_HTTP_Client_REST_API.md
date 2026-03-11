# HTTP Client and REST API Integration

## Learning Objectives

By the end of this lesson, students will be able to:

- [ ] Configure Angular HttpClient
- [ ] Make HTTP requests (GET, POST, PUT, DELETE)
- [ ] Handle HTTP responses and errors
- [ ] Use HTTP interceptors for request/response processing
- [ ] Implement request/response logging and error handling

## Conceptual Explanation

**Visual Analogy**: Think of HttpClient as a **professional messenger** in an office. When your Angular application needs to talk to a server (like an API), the messenger delivers the request, waits for the response, and brings back the data. If there's an issue (server down, wrong address), the messenger reports the error. Interceptors are like **message filters** that can add authentication tokens, log messages, or transform data before delivery!

### HTTP Request Flow

```
┌─────────────────────────────────────────────────────────────────────┐
│                    HTTP Request Flow                                 │
├─────────────────────────────────────────────────────────────────────┤
│                                                                     │
│   Component ──▶ Service ──▶ Interceptors ──▶ Server               │
│                              │                   │                  │
│                              │                   │                  │
│   Component ◀── Service ◀── Interceptors ◀── Response              │
│                                                                     │
│   Request Flow:                                                    │
│   1. Component calls Service method                                │
│   2. Service prepares HTTP request                                │
│   3. Request interceptors process (add token, log)               │
│   4. Request sent to server                                       │
│   5. Server processes request                                     │
│   6. Response interceptors process (parse, error handling)       │
│   7. Service returns Observable to component                      │
│   8. Component subscribes to data                                  │
│                                                                     │
└─────────────────────────────────────────────────────────────────────┘
```

## Real-World Application Context

### Why HTTP Client Matters

1. **API Communication**: Connect frontend to backend
2. **Real-time Data**: Live updates from servers
3. **Third-party Services**: Integrate with external APIs
4. **CRUD Operations**: Create, Read, Update, Delete data

### Industry Use Cases

- **E-commerce**: Product catalogs, orders, payments
- **Social Media**: Feeds, profiles, messaging
- **Dashboards**: Analytics, reports, data visualization
- **CMS**: Content management, media uploads

## Step-by-Step Walkthrough

### Configuring HttpClient

#### Step 1: Import HttpClientModule

```typescript
// app.config.ts
import { ApplicationConfig } from '@angular/core';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { routes } from './app.routes';

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideHttpClient()  // Enable HttpClient
  ]
};
```

#### Step 2: Create API Service

```typescript
// api.service.ts
import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface User {
  id: number;
  name: string;
  email: string;
  phone: string;
}

export interface ApiResponse<T> {
  data: T;
  message?: string;
  success: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private http = inject(HttpClient);
  private baseUrl = 'https://api.example.com';
  
  // GET request
  getUsers(): Observable<User[]> {
    return this.http.get<User[]>(`${this.baseUrl}/users`);
  }
  
  // GET with parameters
  getUsersPaginated(page: number, limit: number): Observable<ApiResponse<User[]>> {
    const params = new HttpParams()
      .set('page', page.toString())
      .set('limit', limit.toString());
    
    return this.http.get<ApiResponse<User[]>>(`${this.baseUrl}/users`, { params });
  }
  
  // GET single resource
  getUser(id: number): Observable<User> {
    return this.http.get<User>(`${this.baseUrl}/users/${id}`);
  }
  
  // POST - Create
  createUser(user: Omit<User, 'id'>): Observable<User> {
    return this.http.post<User>(`${this.baseUrl}/users`, user);
  }
  
  // PUT - Update (full)
  updateUser(id: number, user: Partial<User>): Observable<User> {
    return this.http.put<User>(`${this.baseUrl}/users/${id}`, user);
  }
  
  // PATCH - Update (partial)
  patchUser(id: number, user: Partial<User>): Observable<User> {
    return this.http.patch<User>(`${this.baseUrl}/users/${id}`, user);
  }
  
  // DELETE
  deleteUser(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/users/${id}`);
  }
}
```

### Error Handling

```typescript
// error-handling.service.ts
import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, catchError, throwError, retry } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DataService {
  private http = inject(HttpClient);
  
  getData(): Observable<any> {
    return this.http.get('/api/data').pipe(
      retry(3), // Retry 3 times before failing
      catchError(this.handleError)
    );
  }
  
  private handleError(error: HttpErrorResponse): Observable<never> {
    let errorMessage = 'An error occurred';
    
    if (error.error instanceof ErrorEvent) {
      // Client-side error
      errorMessage = `Error: ${error.error.message}`;
    } else {
      // Server-side error
      switch (error.status) {
        case 0:
          errorMessage = 'Network error - please check your connection';
          break;
        case 400:
          errorMessage = 'Bad request - please check your input';
          break;
        case 401:
          errorMessage = 'Unauthorized - please log in';
          break;
        case 403:
          errorMessage = 'Forbidden - you don\'t have permission';
          break;
        case 404:
          errorMessage = 'Resource not found';
          break;
        case 500:
          errorMessage = 'Server error - please try again later';
          break;
        default:
          errorMessage = `Error ${error.status}: ${error.message}`;
      }
    }
    
    console.error('HTTP Error:', errorMessage);
    return throwError(() => new Error(errorMessage));
  }
}
```

### HTTP Interceptors

#### Request Interceptor

```typescript
// auth.interceptor.ts
import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from './auth.service';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const token = authService.getToken();
  
  if (token) {
    // Clone request and add authorization header
    const authReq = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
    return next(authReq);
  }
  
  return next(req);
};
```

#### Error Interceptor

```typescript
// error.interceptor.ts
import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);
  
  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      let errorMessage = 'An unknown error occurred';
      
      if (error.error instanceof ErrorEvent) {
        errorMessage = error.error.message;
      } else {
        if (error.status === 401) {
          router.navigate(['/login']);
        } else if (error.status === 403) {
          errorMessage = 'Access denied';
        } else if (error.status === 404) {
          errorMessage = 'Resource not found';
        } else if (error.status >= 500) {
          errorMessage = 'Server error';
        }
      }
      
      console.error('HTTP Error:', errorMessage);
      return throwError(() => new Error(errorMessage));
    })
  );
};
```

#### Logging Interceptor

```typescript
// logging.interceptor.ts
import { HttpInterceptorFn, HttpRequest, HttpResponse } from '@angular/common/http';

export const loggingInterceptor: HttpInterceptorFn = (req, next) => {
  const startTime = Date.now();
  
  return next(req).pipe(
    // Log request
    tap({
      next: (event) => {
        if (event instanceof HttpResponse) {
          const elapsed = Date.now() - startTime;
          console.log(`[${req.method}] ${req.url} - ${event.status} (${elapsed}ms)`);
        }
      },
      error: (error) => {
        const elapsed = Date.now() - startTime;
        console.error(`[${req.method}] ${req.url} - Error (${elapsed}ms)`, error);
      }
    })
  );
};
```

#### Registering Interceptors

```typescript
// app.config.ts
import { ApplicationConfig } from '@angular/core';
import { provideHttpClient, withInterceptorsFromDi, HTTP_INTERCEPTORS } from '@angular/common/http';
import { authInterceptor } from './interceptors/auth.interceptor';
import { errorInterceptor } from './interceptors/error.interceptor';
import { loggingInterceptor } from './interceptors/logging.interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideHttpClient(
      withInterceptorsFromDi(),
      withInterceptors([authInterceptor, errorInterceptor, loggingInterceptor])
    )
  ]
};
```

### Complete Example: User Management

```typescript
// user.model.ts
export interface User {
  id: number;
  name: string;
  email: string;
  phone?: string;
  website?: string;
  address?: {
    street: string;
    city: string;
    zipcode: string;
  };
}

// user.service.ts
import { Injectable, inject, signal } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, tap, catchError, of } from 'rxjs';
import { User } from './user.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private http = inject(HttpClient);
  private apiUrl = 'https://jsonplaceholder.typicode.com'; // Free API for testing
  
  // Reactive state with signals
  users = signal<User[]>([]);
  currentUser = signal<User | null>(null);
  isLoading = signal(false);
  error = signal<string | null>(null);
  
  // Get all users
  loadUsers(): void {
    this.isLoading.set(true);
    this.error.set(null);
    
    this.http.get<User[]>(`${this.apiUrl}/users`).pipe(
      tap(users => {
        this.users.set(users);
        this.isLoading.set(false);
      }),
      catchError(err => {
        this.error.set('Failed to load users');
        this.isLoading.set(false);
        return of([]);
      })
    ).subscribe();
  }
  
  // Get single user
  getUser(id: number): Observable<User> {
    return this.http.get<User>(`${this.apiUrl}/users/${id}`).pipe(
      tap(user => this.currentUser.set(user))
    );
  }
  
  // Create user
  createUser(user: Omit<User, 'id'>): Observable<User> {
    return this.http.post<User>(`${this.apiUrl}/users`, user).pipe(
      tap(newUser => {
        this.users.update(users => [...users, newUser]);
      })
    );
  }
  
  // Update user
  updateUser(id: number, user: Partial<User>): Observable<User> {
    return this.http.put<User>(`${this.apiUrl}/users/${id}`, user).pipe(
      tap(updatedUser => {
        this.users.update(users => 
          users.map(u => u.id === id ? updatedUser : u)
        );
      })
    );
  }
  
  // Delete user
  deleteUser(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/users/${id}`).pipe(
      tap(() => {
        this.users.update(users => users.filter(u => u.id !== id));
      })
    );
  }
  
  // Search users
  searchUsers(query: string): Observable<User[]> {
    const params = new HttpParams().set('q', query);
    return this.http.get<User[]>(`${this.apiUrl}/users`, { params });
  }
}

// user-list.component.ts
import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { UserService, User } from './user.service';

@Component({
  selector: 'app-user-list',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="user-management">
      <h1>User Management</h1>
      
      <!-- Search -->
      <div class="search-bar">
        <input 
          [(ngModel)]="searchTerm" 
          (input)="onSearch()"
          placeholder="Search users...">
      </div>
      
      <!-- Loading State -->
      @if (isLoading()) {
        <div class="loading">Loading users...</div>
      }
      
      <!-- Error State -->
      @if (error()) {
        <div class="error">{{ error() }}</div>
      }
      
      <!-- User List -->
      @if (!isLoading() && !error()) {
        <div class="user-grid">
          @for (user of users(); track user.id) {
            <div class="user-card">
              <h3>{{ user.name }}</h3>
              <p>📧 {{ user.email }}</p>
              <p>📱 {{ user.phone }}</p>
              <p>🌐 {{ user.website }}</p>
              <div class="actions">
                <button (click)="viewUser(user.id)">View</button>
                <button (click)="editUser(user.id)">Edit</button>
                <button (click)="deleteUser(user.id)" class="delete">Delete</button>
              </div>
            </div>
          } @empty {
            <p>No users found</p>
          }
        </div>
      }
    </div>
  `,
  styles: [`
    .user-management { padding: 1rem; }
    .search-bar { margin-bottom: 1rem; }
    .search-bar input { 
      padding: 0.5rem; 
      width:       font-size:300px; 
 1rem; 
    }
    .user-grid { 
      display: grid; 
      grid-template-columns: repeat(auto-fill, minmax(250px, 1fr)); 
      gap: 1rem; 
    }
    .user-card { 
      border: 1px solid #ddd; 
      padding: 1rem; 
      border-radius: 8px; 
    }
    .actions { 
      display: flex; 
      gap: 0.5rem; 
      margin-top: 1rem; 
    }
    button { 
      padding: 0.5rem 1rem; 
      cursor: pointer; 
    }
    .delete { 
      background: #f44336; 
      color: white; 
      border: none; 
    }
    .loading, .error { 
      padding: 1rem; 
      margin-bottom: 1rem; 
    }
    .error { 
      background: #ffebee; 
      color: #c62828; 
    }
  `]
})
export class UserListComponent implements OnInit {
  private userService = inject(UserService);
  
  users = this.userService.users;
  isLoading = this.userService.isLoading;
  error = this.userService.error;
  
  searchTerm = '';
  
  ngOnInit(): void {
    this.userService.loadUsers();
  }
  
  onSearch(): void {
    if (this.searchTerm) {
      this.userService.searchUsers(this.searchTerm).subscribe();
    } else {
      this.userService.loadUsers();
    }
  }
  
  viewUser(id: number): void {
    console.log('View user:', id);
  }
  
  editUser(id: number): void {
    console.log('Edit user:', id);
  }
  
  deleteUser(id: number): void {
    if (confirm('Are you sure you want to delete this user?')) {
      this.userService.deleteUser(id).subscribe();
    }
  }
}
```

## Best Practices

### 1. Use Interceptors for Cross-Cutting Concerns

```typescript
// Good: Use interceptor for auth
export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const token = getToken();
  if (token) {
    return next(req.clone({ setHeaders: { Authorization: `Bearer ${token}` } }));
  }
  return next(req);
};

// Avoid: Adding headers in every request
getData() {
  return this.http.get('/api/data', {
    headers: { Authorization: `Bearer ${getToken()}` } // Repetitive!
  });
}
```

### 2. Handle Errors Globally

```typescript
// Good: Centralized error handling
@Injectable({ providedIn: 'root' })
export class ErrorHandlerService {
  handleError(error: HttpErrorResponse): Observable<never> {
    // Log to monitoring service
    // Show user-friendly message
    // Return safe observable
    return throwError(() => error);
  }
}
```

### 3. Use Proper Typing

```typescript
// Good: Strongly typed responses
interface ApiResponse<T> {
  data: T;
  message: string;
  status: number;
}

getUsers(): Observable<ApiResponse<User[]>> {
  return this.http.get<ApiResponse<User[]>>('/api/users');
}
```

### 4. Consider Using Async Pipe

```typescript
// Good: Async pipe handles subscription
@Component({
  template: `
    @for (user of users$ | async; track user.id) {
      {{ user.name }}
    }
  `
})
export class UserListComponent {
  users$ = this.userService.getUsers();
}

// Avoid: Manual subscription
users: User[] = [];
ngOnInit() {
  this.userService.getUsers().subscribe(users => this.users = users);
}
```

## Common Pitfalls and Debugging

### Pitfall 1: CORS Errors

```typescript
// Problem: CORS error when calling API
// Access to fetch at 'http://localhost:3000' from origin 'http://localhost:4200' 
// has been blocked by CORS policy

// Solutions:
// 1. Enable CORS on server
// 2. Use proxy configuration (Angular)
// 3. Use JSONP for GET requests (limited)
```

Create proxy:
```json
// proxy.conf.json
{
  "/api": {
    "target": "http://localhost:3000",
    "secure": false,
    "changeOrigin": true
  }
}
```
Run with: `ng serve --proxy-config proxy.conf.json`

### Pitfall 2: Not Handling Null Responses

```typescript
// Problem: Error when response is null
this.http.get<User>('/api/user').pipe(
  map(user => user.name.toUpperCase()) // Error if user is null
);

// Solution: Check for null
this.http.get<User>('/api/user').pipe(
  map(user => user?.name?.toUpperCase() ?? '')
);
```

### Pitfall 3: Memory Leaks

```typescript
// Problem: Subscription not cleaned up
export class MyComponent implements OnInit {
  ngOnInit() {
    this.http.get('/api/data').subscribe(data => {
      this.data = data;
    }); // Memory leak!
  }
}

// Solution: Use takeUntilDestroyed or async pipe
export class MyComponent {
  private destroyRef = inject(DestroyRef);
  
  ngOnInit() {
    this.http.get('/api/data')
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe(data => this.data = data);
  }
}
```

## Hands-On Exercise

### Exercise 2.3: HTTP Implementation

**Objective**: Build a complete API integration

**Requirements**:
1. Create CRUD service for products
2. Add error handling
3. Implement request interceptor for auth
4. Implement response interceptor for logging
5. Use signals for state management

**Deliverable**: Complete HTTP service

**Assessment Criteria**:
- [ ] All CRUD operations implemented
- [ ] Error handling in place
- [ ] Interceptors working
- [ ] TypeScript typing
- [ ] Async pipe usage

## Summary

- **HttpClient** is Angular's built-in HTTP client
- **Observables** handle async HTTP requests
- **Interceptors** process requests/responses globally
- **Error handling** should be centralized
- Use **async pipe** to avoid memory leaks
- Always **type HTTP responses**

## Suggested Reading

- [Angular HttpClient Documentation](https://angular.io/guide/http)
- RESTful API Design Best Practices

## Next Steps

In the next lecture, we'll explore RxJS Observables for reactive programming.
