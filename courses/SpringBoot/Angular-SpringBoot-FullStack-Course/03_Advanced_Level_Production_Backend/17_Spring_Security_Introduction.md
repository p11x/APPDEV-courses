# Spring Security Introduction

## Concept Title and Overview

In this lesson, you'll learn the fundamentals of Spring Security, which is essential for protecting your APIs. We'll cover authentication, authorization, and the security filter chain.

## Real-World Importance and Context

Every production API needs security. Without it, anyone can access your data. Spring Security provides comprehensive security for Spring applications.

## Detailed Step-by-Step Explanation

### Authentication vs Authorization

```
┌─────────────────────────────────────────────────────────────────────────┐
│                 AUTHENTICATION vs AUTHORIZATION                          │
├─────────────────────────────────────────────────────────────────────────┤
│                                                                         │
│  AUTHENTICATION (WHO are you?)                                         │
│  ┌─────────────────────────────────────────────────────────────────┐   │
│  │ Verifying user identity                                          │   │
│  │ • Login with username/password                                   │   │
│  │ • Login with Google/Facebook                                     │   │
│  │ • Login with JWT token                                          │   │
│  │                                                                 │   │
│  │ "Prove you are who you claim to be"                            │   │
│  └─────────────────────────────────────────────────────────────────┘   │
│                                                                         │
│  AUTHORIZATION (WHAT can you do?)                                      │
│  ┌─────────────────────────────────────────────────────────────────┐   │
│  │ Verifying user has permission                                    │   │
│  │ • Admin can delete users                                        │   │
│  │ • User can only view their own data                             │   │
│  │ • Editor can modify content                                     │   │
│  │                                                                 │   │
│  │ "Are you allowed to do this?"                                  │   │
│  └─────────────────────────────────────────────────────────────────┘   │
│                                                                         │
└─────────────────────────────────────────────────────────────────────────┘
```

### Adding Spring Security

```xml
<dependency>
    <groupId>org.springframework.boot</groupId>
    <artifactId>spring-boot-starter-security</artifactId>
</dependency>
```

### Default Security Behavior

By default, Spring Security:
- Protects all endpoints
- Provides a login form (for web)
- Provides HTTP Basic auth (for APIs)
- Requires authentication for everything

### Basic Security Configuration

```java
@Configuration
@EnableWebSecurity
public class SecurityConfig {
    
    @Bean
    public SecurityFilterChain securityFilterChain(HttpSecurity http) throws Exception {
        http
            .csrf(csrf -> csrf.disable())  // Disable for REST APIs
            .authorizeHttpRequests(auth -> auth
                .requestMatchers("/api/public/**").permitAll()
                .requestMatchers("/api/admin/**").hasRole("ADMIN")
                .anyRequest().authenticated()
            )
            .httpBasic(basic -> {});  // Enable HTTP Basic
            
        return http.build();
    }
}
```

### In-Memory User Details

```java
@Bean
public UserDetailsService userDetailsService() {
    UserDetails user = User.builder()
        .username("user")
        .password("{noop}password")  // {noop} = no encoding
        .roles("USER")
        .build();
    
    UserDetails admin = User.builder()
        .username("admin")
        .password("{noop}admin123")
        .roles("ADMIN", "USER")
        .build();
    
    return new InMemoryUserDetailsManager(user, admin);
}
```

## Complete Security Example

```java
@Configuration
@EnableWebSecurity
@EnableMethodSecurity
public class SecurityConfig {
    
    @Bean
    public SecurityFilterChain filterChain(HttpSecurity http) throws Exception {
        http
            .csrf(csrf -> csrf.disable())
            .authorizeHttpRequests(auth -> auth
                .requestMatchers("/api/auth/**").permitAll()
                .requestMatchers("/api/admin/**").hasRole("ADMIN")
                .anyRequest().authenticated()
            )
            .sessionManagement(session -> session
                .sessionCreationPolicy(SessionCreationPolicy.STATELESS))
            .httpBasic(basic -> {});
        
        return http.build();
    }
}
```

## Angular Authentication Service

```typescript
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'http://localhost:8080/api/auth';
  private tokenKey = 'auth_token';

  constructor(private http: HttpClient) {}

  login(username: string, password: string): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/login`, { username, password })
      .pipe(tap(response => {
        localStorage.setItem(this.tokenKey, response.token);
      }));
  }

  logout(): void {
    localStorage.removeItem(this.tokenKey);
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  isAuthenticated(): boolean {
    return !!this.getToken();
  }
}
```

## Student Hands-On Exercises

### Exercise 1: Add Security (Easy)
Add Spring Security to your project and verify endpoints require authentication.

### Exercise 2: Configure Roles (Medium)
Set up different roles (USER, ADMIN) with appropriate access.

### Exercise 3: Method Security (Hard)
Use @PreAuthorize for method-level security.

---

## Summary

You've learned:
- Authentication vs authorization
- Adding Spring Security
- Basic configuration
- Role-based access control
- Angular authentication

---

**Next Lesson**: In the next lesson, we'll explore [JWT Authentication](18_JWT_Authentication.md).
