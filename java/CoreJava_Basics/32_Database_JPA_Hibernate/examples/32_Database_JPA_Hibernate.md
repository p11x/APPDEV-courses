# JWT Authentication for Angular + Java

## Table of Contents
1. [Introduction to JWT](#introduction-to-jwt)
2. [JWT Structure](#jwt-structure)
3. [JWT in Java Backend](#jwt-in-java-backend)
4. [Angular JWT Integration](#angular-jwt-integration)
5. [Security Best Practices](#security-best-practices)

---

## 1. Introduction to JWT

### What is JWT?

**JSON Web Token (JWT)** is a compact, self-contained way to securely transmit information between parties as JSON.

```
┌─────────────────────────────────────────────────────────────┐
│                    JWT AUTHENTICATION                         │
├─────────────────────────────────────────────────────────────┤
│                                                              │
│   Flow:                                                      │
│   ┌─────────┐     ┌─────────────┐     ┌─────────────┐     │
│   │ Angular │ ──► │   Login    │ ──► │  Java API   │     │
│   │   App  │     │  Request   │     │  (Validate) │     │
│   └─────────┘     └─────────────┘     └─────────────┘     │
│        │                                        │            │
│        │ JWT Token                              │            │
│        ▼                                        ▼            │
│   ┌─────────┐     ┌─────────────┐     ┌─────────────┐     │
│   │ Store   │ ◄── │   JWT      │ ◄── │  Generate  │     │
│   │ Token   │     │  Response  │     │   Token    │     │
│   └─────────┘     └─────────────┘     └─────────────┘     │
│                                                              │
│   Uses:                                                      │
│   ✓ Authentication (who is the user)                        │
│   ✓ Authorization (what can the user do)                    │
│   ✓ Information exchange (secure data transfer)              │
│                                                              │
└─────────────────────────────────────────────────────────────┘
```

---

## 2. JWT Structure

### JWT Format

```
JWT = Header.Payload.Signature

Example:
eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c
```

### Three Parts

1. **Header** - Token type and signing algorithm
2. **Payload** - Claims (data)
3. **Signature** - Verification

### Payload Claims

```json
{
  "sub": "1234567890",    // Subject (user ID)
  "name": "John Doe",      // Name
  "email": "john@email.com",
  "role": "ADMIN",         // User role
  "iat": 1516239022,      // Issued at
  "exp": 1516242622       // Expiration
}
```

---

## 3. JWT in Java Backend

### JWT Utility Class

```java
import javax.crypto.SecretKey;
import javax.crypto.spec.SecretKeySpec;
import java.util.Base64;
import java.util.Date;

public class JwtUtil {
    
    // Secret key for signing (in production, use environment variable)
    private static final String SECRET = "mySecretKey12345678901234567890123456789012";
    private static final long EXPIRATION = 86400000; // 24 hours
    
    // Generate Token
    public static String generateToken(String username, String role) {
        Date now = new Date();
        Date expiryDate = new Date(now.getTime() + EXPIRATION);
        
        // In real implementation, use a JWT library like JJWT
        // This is a simplified example
        String payload = String.format(
            "{\"sub\":\"%s\",\"role\":\"%s\",\"iat\":%d,\"exp\":%d}",
            username, role, now.getTime()/1000, expiryDate.getTime()/1000
        );
        
        // Encode (simplified - use proper JWT library in production)
        String header = "{\"alg\":\"HS256\",\"typ\":\"JWT\"}";
        String encodedHeader = Base64.getUrlEncoder().encodeToString(header.getBytes());
        String encodedPayload = Base64.getUrlEncoder().encodeToString(payload.getBytes());
        String encodedSignature = Base64.getUrlEncoder().encodeToString(SECRET.getBytes());
        
        return encodedHeader + "." + encodedPayload + "." + encodedSignature;
    }
    
    // Validate Token
    public static boolean validateToken(String token) {
        try {
            String[] parts = token.split("\\.");
            if (parts.length != 3) return false;
            
            String payload = new String(Base64.getUrlDecoder().decode(parts[1]));
            
            // Check expiration
            if (payload.contains("exp")) {
                // Parse and check expiration
                // In production, use JWT library
            }
            
            return true;
        } catch (Exception e) {
            return false;
        }
    }
    
    // Get Username from Token
    public static String getUsernameFromToken(String token) {
        try {
            String[] parts = token.split("\\.");
            String payload = new String(Base64.getUrlDecoder().decode(parts[1]));
            
            // Extract username from JSON payload
            // In production, use proper JSON parsing
            return "user"; // Simplified
        } catch (Exception e) {
            return null;
        }
    }
    
    // Get Role from Token
    public static String getRoleFromToken(String token) {
        try {
            String[] parts = token.split("\\.");
            String payload = new String(Base64.getUrlDecoder().decode(parts[1]));
            
            // Extract role from JSON payload
            // In production, use proper JSON parsing
            return "USER"; // Simplified
        } catch (Exception e) {
            return null;
        }
    }
}
```

### Login Controller

```java
@RestController
@RequestMapping("/api/auth")
public class AuthController {
    
    @PostMapping("/login")
    public ResponseEntity<?> login(@RequestBody LoginRequest request) {
        // Validate credentials (in production, check against database)
        if ("admin".equals(request.getUsername()) && "password".equals(request.getPassword())) {
            
            // Generate JWT token
            String token = JwtUtil.generateToken(request.getUsername(), "ADMIN");
            
            // Return token
            Map<String, Object> response = new HashMap<>();
            response.put("token", token);
            response.put("username", request.getUsername());
            response.put("role", "ADMIN");
            
            return ResponseEntity.ok(response);
        }
        
        return ResponseEntity.status(401).body(Map.of("error", "Invalid credentials"));
    }
    
    @PostMapping("/register")
    public ResponseEntity<?> register(@RequestBody RegisterRequest request) {
        // In production: save user to database with hashed password
        return ResponseEntity.ok(Map.of("message", "User registered successfully"));
    }
}

// Request classes
class LoginRequest {
    private String username;
    private String password;
    
    public String getUsername() { return username; }
    public void setUsername(String username) { this.username = username; }
    public String getPassword() { return password; }
    public void setPassword(String password) { this.password = password; }
}

class RegisterRequest {
    private String username;
    private String password;
    private String email;
    
    // Getters and setters
}
```

### Protected Endpoint

```java
@RestController
@RequestMapping("/api/users")
public class UserController {
    
    // Public endpoint
    @GetMapping
    public List<User> getAllUsers() {
        return userService.findAll();
    }
    
    // Protected endpoint
    @GetMapping("/profile")
    public ResponseEntity<?> getProfile(@RequestHeader("Authorization") String authHeader) {
        if (authHeader == null || !authHeader.startsWith("Bearer ")) {
            return ResponseEntity.status(401).body(Map.of("error", "No token provided"));
        }
        
        String token = authHeader.substring(7);
        
        if (!JwtUtil.validateToken(token)) {
            return ResponseEntity.status(401).body(Map.of("error", "Invalid token"));
        }
        
        String username = JwtUtil.getUsernameFromToken(token);
        // Return user profile
        return ResponseEntity.ok(Map.of("username", username, "message", "Profile data"));
    }
}
```

---

## 4. Angular JWT Integration

### Auth Service

```typescript
// auth.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { tap } from 'rxjs/operators';

export interface LoginResponse {
  token: string;
  username: string;
  role: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'http://localhost:8080/api/auth';
  private tokenKey = 'auth_token';
  
  private loggedIn = new BehaviorSubject<boolean>(this.hasToken());
  isLoggedIn$ = this.loggedIn.asObservable();

  constructor(private http: HttpClient) {}

  login(username: string, password: string): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.apiUrl}/login`, { username, password })
      .pipe(
        tap(response => {
          this.setToken(response.token);
          this.loggedIn.next(true);
        })
      );
  }

  logout(): void {
    this.removeToken();
    this.loggedIn.next(false);
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  private setToken(token: string): void {
    localStorage.setItem(this.tokenKey, token);
  }

  private removeToken(): void {
    localStorage.removeItem(this.tokenKey);
  }

  private hasToken(): boolean {
    return !!this.getToken();
  }

  isLoggedIn(): boolean {
    return this.hasToken();
  }
}
```

### Auth Interceptor

```typescript
// auth.interceptor.ts
import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const token = localStorage.getItem('auth_token');
    
    if (token) {
      const cloned = req.clone({
        headers: req.headers.set('Authorization', `Bearer ${token}`)
      });
      return next.handle(cloned);
    }
    
    return next.handle(req);
  }
}
```

### Login Component

```typescript
// login.component.ts
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from './auth.service';

@Component({
  selector: 'app-login',
  template: `
    <div class="login-form">
      <h2>Login</h2>
      <input [(ngModel)]="username" placeholder="Username">
      <input type="password" [(ngModel)]="password" placeholder="Password">
      <button (click)="login()">Login</button>
      <p *ngIf="error" class="error">{{ error }}</p>
    </div>
  `
})
export class LoginComponent {
  username = '';
  password = '';
  error = '';

  constructor(private authService: AuthService, private router: Router) {}

  login(): void {
    this.authService.login(this.username, this.password).subscribe({
      next: () => {
        this.router.navigate(['/dashboard']);
      },
      error: (err) => {
        this.error = 'Invalid credentials';
      }
    });
  }
}
```

### Protected Route

```typescript
// auth.guard.ts
import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  
  constructor(private authService: AuthService, private router: Router) {}

  canActivate(): boolean {
    if (this.authService.isLoggedIn()) {
      return true;
    }
    this.router.navigate(['/login']);
    return false;
  }
}
```

### App Module Setup

```typescript
// app.module.ts
import { NgModule } from '@angular/core';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthInterceptor } from './auth.interceptor';

@NgModule({
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    }
  ]
})
export class AppModule { }
```

---

## 5. Security Best Practices

### Backend

1. **Use HTTPS** - Always use SSL/TLS
2. **Store secrets securely** - Use environment variables
3. **Set token expiration** - Short-lived tokens (15-60 min)
4. **Use strong signing algorithms** - HS256, RS256
5. **Validate tokens on every request**
6. **Store hashed passwords** - Never store plain text

### Frontend

1. **Store tokens securely** - Use httpOnly cookies when possible
2. **Implement token refresh** - Handle expired tokens
3. **Clear tokens on logout**
4. **Use HTTPS**

---

## Summary

### JWT Authentication Flow

1. **Login** - User submits credentials
2. **Generate** - Server creates JWT with user info
3. **Store** - Frontend stores JWT (localStorage or cookie)
4. **Send** - Frontend includes JWT in Authorization header
5. **Validate** - Server validates JWT on each request
6. **Authorize** - Server allows/denies access based on claims

---

*JWT Authentication Complete!*
