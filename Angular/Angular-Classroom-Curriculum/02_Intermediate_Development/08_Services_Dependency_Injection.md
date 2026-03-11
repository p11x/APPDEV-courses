# Services and Dependency Injection

## Learning Objectives

By the end of this lesson, students will be able to:

- [ ] Understand the concept of services in Angular
- [ ] Create and provide services using Angular's DI system
- [ ] Implement dependency injection with different provider scopes
- [ ] Use inject() function for cleaner code
- [ ] Apply service-based architecture for business logic

## Conceptual Explanation

**Visual Analogy**: Think of services as **specialized departments** in a company. The UI (components) are like front-line employees who interact with customers, while services are the back-office departments (HR, Finance, IT) that handle complex operations. Components don't need to know HOW to do everything - they just request help from the right service!

### What is a Service?

A service is a class that handles business logic, data fetching, and shared functionality. Services are injected into components and other services.

```
┌─────────────────────────────────────────────────────────────────────┐
│                    Service Architecture                             │
├─────────────────────────────────────────────────────────────────────┤
│                                                                     │
│   ┌─────────────────┐         ┌─────────────────┐                  │
│   │   Component A  │         │   Component B  │                  │
│   │                 │         │                 │                  │
│   │  - UI Logic    │         │  - UI Logic    │                  │
│   │  - User Input  │         │  - User Input  │                  │
│   └────────┬────────┘         └────────┬────────┘                  │
│            │                           │                            │
│            ▼                           ▼                            │
│   ┌─────────────────────────────────────────────┐                  │
│   │              DataService                     │                  │
│   │  - fetchData()                               │                  │
│   │  - saveData()                                │                  │
│   │  - Business logic                            │                  │
│   └─────────────────────────────────────────────┘                  │
│                                                                     │
└─────────────────────────────────────────────────────────────────────┘
```

### What is Dependency Injection?

Dependency Injection (DI) is a design pattern where a class receives its dependencies from external sources rather than creating them.

## Real-World Application Context

### Why Services & DI Matter

1. **Code Reusability**: Share logic across components
2. **Separation of Concerns**: UI separate from business logic
3. **Testability**: Easy to mock services in tests
4. **Maintainability**: Changes in one place affect all consumers

### Industry Use Cases

- **API Communication**: HTTP services
- **Authentication**: Auth services
- **State Management**: NgRx services
- **Logging**: Logging services
- **Utilities**: Date formatting, validation

## Step-by-Step Walkthrough

### Creating a Service

#### Step 1: Generate a Service

```bash
ng generate service services/user
# or shorthand
ng g s services/user
```

Expected output:
```
CREATE src/app/services/user.service.ts (x bytes)
CREATE src/app/services/user.service.spec.ts (x bytes)
```

#### Step 2: Define the Service

```typescript
// user.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, catchError, of, throwError } from 'rxjs';

export interface User {
  id: number;
  name: string;
  email: string;
}

@Injectable({
  providedIn: 'root'  // Makes service available app-wide
})
export class UserService {
  private apiUrl = 'https://api.example.com/users';
  
  constructor(private http: HttpClient) {}
  
  // Get all users
  getUsers(): Observable<User[]> {
    return this.http.get<User[]>(this.apiUrl)
      .pipe(
        catchError(this.handleError)
      );
  }
  
  // Get user by ID
  getUser(id: number): Observable<User> {
    return this.http.get<User>(`${this.apiUrl}/${id}`)
      .pipe(
        catchError(this.handleError)
      );
  }
  
  // Create user
  createUser(user: Omit<User, 'id'>): Observable<User> {
    return this.http.post<User>(this.apiUrl, user)
      .pipe(
        catchError(this.handleError)
      );
  }
  
  // Update user
  updateUser(id: number, user: Partial<User>): Observable<User> {
    return this.http.put<User>(`${this.apiUrl}/${id}`, user)
      .pipe(
        catchError(this.handleError)
      );
  }
  
  // Delete user
  deleteUser(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`)
      .pipe(
        catchError(this.handleError)
      );
  }
  
  // Error handling
  private handleError(error: any) {
    console.error('API Error:', error);
    return throwError(() => new Error('Something went wrong'));
  }
}
```

### Using the Service in a Component

#### Using constructor injection (Traditional)

```typescript
// user-list.component.ts
import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserService, User } from '../../services/user.service';

@Component({
  selector: 'app-user-list',
  standalone: true,
  imports: [CommonModule],
  template: `
    <h1>User Management</h1>
    
    @if (isLoading()) {
      <p>Loading users...</p>
    } @else {
      <ul>
        @for (user of users(); track user.id) {
          <li>
            {{ user.name }} - {{ user.email }}
            <button (click)="deleteUser(user.id)">Delete</button>
          </li>
        }
      </ul>
    }
    
    @if (error()) {
      <p class="error">{{ error() }}</p>
    }
  `
})
export class UserListComponent implements OnInit {
  // Using signals for reactive state
  users = signal<User[]>([]);
  isLoading = signal(true);
  error = signal<string | null>(null);
  
  // Inject service via constructor
  constructor(private userService: UserService) {}
  
  ngOnInit(): void {
    this.loadUsers();
  }
  
  loadUsers(): void {
    this.isLoading.set(true);
    this.error.set(null);
    
    this.userService.getUsers().subscribe({
      next: (data) => {
        this.users.set(data);
        this.isLoading.set(false);
      },
      error: (err) => {
        this.error.set('Failed to load users');
        this.isLoading.set(false);
      }
    });
  }
  
  deleteUser(id: number): void {
    if (confirm('Are you sure?')) {
      this.userService.deleteUser(id).subscribe({
        next: () => {
          this.users.update(users => users.filter(u => u.id !== id));
        },
        error: (err) => {
          this.error.set('Failed to delete user');
        }
      });
    }
  }
}
```

#### Using inject() function (Modern Angular 14+)

```typescript
// Using inject() - cleaner and more concise
import { Component, OnInit, inject, signal } from '@angular/core';
import { UserService, User } from '../../services/user.service';

@Component({
  selector: 'app-user-list',
  standalone: true,
  template: `...`
})
export class UserListComponent implements OnInit {
  // Using inject() function
  private userService = inject(UserService);
  
  users = signal<User[]>([]);
  isLoading = signal(true);
  
  ngOnInit(): void {
    this.userService.getUsers().subscribe(data => {
      this.users.set(data);
      this.isLoading.set(false);
    });
  }
}
```

### Dependency Injection Providers

#### 1. providedIn: 'root'

```typescript
@Injectable({
  providedIn: 'root'  // Singleton - one instance for entire app
})
export class UserService { }
```

#### 2. providedIn: 'any'

```typescript
@Injectable({
  providedIn: 'any'  // New instance per lazy-loaded module
})
export class FeatureService { }
```

#### 3. Component-level providers

```typescript
@Component({
  selector: 'app-user-list',
  standalone: true,
  providers: [UserService]  // New instance per component
})
export class UserListComponent { }
```

### Custom Provider Example

```typescript
// Using useClass
{
  provide: UserService,
  useClass: MockUserService  // Use mock instead of real
}

// Using useValue
{
  provide: API_CONFIG,
  useValue: { apiUrl: 'https://api.example.com' }
}

// Using useFactory
{
  provide: UserService,
  useFactory: (http: HttpClient, config: Config) => {
    return new UserService(http, config.apiUrl);
  },
  deps: [HttpClient, API_CONFIG]
}
```

## Complete Example: Data Service with Caching

```typescript
// cache.service.ts
import { Injectable, inject, computed, signal } from '@angular/core';

interface CacheEntry<T> {
  data: T;
  timestamp: number;
}

@Injectable({
  providedIn: 'root'
})
export class CacheService {
  private cache = new Map<string, CacheEntry<any>>();
  private cacheDuration = 5 * 60 * 1000; // 5 minutes
  
  get<T>(key: string): T | null {
    const entry = this.cache.get(key);
    if (!entry) return null;
    
    if (Date.now() - entry.timestamp > this.cacheDuration) {
      this.cache.delete(key);
      return null;
    }
    
    return entry.data as T;
  }
  
  set<T>(key: string, data: T): void {
    this.cache.set(key, { data, timestamp: Date.now() });
  }
  
  clear(): void {
    this.cache.clear();
  }
}

// user.service.ts (with caching)
import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of, tap } from 'rxjs';
import { CacheService } from './cache.service';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private http = inject(HttpClient);
  private cacheService = inject(CacheService);
  private apiUrl = 'https://api.example.com/users';
  
  getUsers(forceRefresh = false): Observable<User[]> {
    const cacheKey = 'users';
    
    // Check cache first
    if (!forceRefresh) {
      const cached = this.cacheService.get<User[]>(cacheKey);
      if (cached) {
        return of(cached);
      }
    }
    
    // Fetch and cache
    return this.http.get<User[]>(this.apiUrl).pipe(
      tap(data => this.cacheService.set(cacheKey, data))
    );
  }
}

// product.service.ts
export interface Product {
  id: number;
  name: string;
  price: number;
}

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private http = inject(HttpClient);
  private products = signal<Product[]>([]);
  
  // Computed signal for filtered products
  getFilteredProducts(category: string) {
    return computed(() => 
      category 
        ? this.products().filter(p => p.category === category)
        : this.products()
    );
  }
  
  loadProducts(): void {
    this.http.get<Product[]>('/api/products').subscribe(data => {
      this.products.set(data);
    });
  }
}
```

## Best Practices

### 1. Use providedIn: 'root'

```typescript
// Good: Application-wide singleton
@Injectable({
  providedIn: 'root'
})
export class UserService { }

// Avoid: Module-level unless necessary
@NgModule({
  providers: [UserService]
})
export class AppModule { }
```

### 2. Use inject() over Constructor

```typescript
// Good: Modern approach
export class MyComponent {
  private service = inject(MyService);
}

// Works but verbose
export class MyComponent {
  constructor(private service: MyService) {}
}
```

### 3. Keep Services Focused

```typescript
// Good: Single responsibility
@Injectable({ providedIn: 'root' })
export class UserService { /* user operations only */ }

@Injectable({ providedIn: 'root' })
export class ProductService { /* product operations only */ }

// Avoid: God service
@Injectable({ providedIn: 'root' })
export class EverythingService { /* everything */ }
```

### 4. Handle Errors in Services

```typescript
@Injectable({ providedIn: 'root' })
export class DataService {
  getData(): Observable<Data> {
    return this.http.get<Data>(url).pipe(
      catchError(error => {
        console.error('Error in service:', error);
        return throwError(() => error);
      })
    );
  }
}
```

## Common Pitfalls and Debugging

### Pitfall 1: Service Not Provided

```typescript
// Error: No provider for UserService
// Solution: Add providedIn: 'root' to the service
@Injectable({
  providedIn: 'root'  // This makes it available
})
export class UserService { }
```

### Pitfall 2: Circular Dependency

```typescript
// Problem: Service A imports Service B, Service B imports Service A
// Solution: Use inject() with forwardRef or restructure

// solution: Use inject() inside methods
export class ServiceA {
  private serviceB = inject(ServiceB);
}
```

### Pitfall 3: Forgetting to Unsubscribe

```typescript
// Problem: Memory leak
export class MyComponent implements OnInit {
  ngOnInit() {
    this.service.getData().subscribe(data => {
      this.data = data; // Never unsubscribed!
    });
  }
}

// Solution: Use takeUntilDestroyed or async pipe
export class MyComponent {
  private destroyRef = inject(DestroyRef);
  
  ngOnInit() {
    this.service.getData()
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe(data => this.data = data);
  }
}
```

## Hands-On Exercise

### Exercise 2.1: Service Implementation

**Objective**: Create a task management service

**Requirements**:
1. Create a TaskService with CRUD operations
2. Use signals for state management
3. Implement caching for better performance
4. Add proper error handling
5. Use inject() function

**Deliverable**: Complete TaskService with task operations

**Assessment Criteria**:
- [ ] All CRUD operations implemented
- [ ] Signals for reactive state
- [ ] Error handling in place
- [ ] TypeScript interfaces for data
- [ ] Code follows best practices

## Summary

- **Services** encapsulate business logic and data fetching
- **Dependency Injection** provides services to components
- Use `providedIn: 'root'` for application-wide singletons
- Prefer `inject()` over constructor injection
- Handle errors in services, not components

## Suggested Reading

- [Angular Dependency Injection](https://angular.io/guide/dependency-injection)
- "Dependency Injection in .NET" - Conceptual foundation

## Next Steps

In the next lecture, we'll explore Angular Router for navigation.
