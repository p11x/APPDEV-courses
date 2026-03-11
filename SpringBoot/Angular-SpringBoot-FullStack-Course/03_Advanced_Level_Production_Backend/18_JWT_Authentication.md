# JWT Authentication

## Concept Title and Overview

In this lesson, you'll learn about JSON Web Tokens (JWT), the industry standard for stateless authentication in REST APIs.

## Real-World Importance and Context

JWT allows stateless authentication—each request contains all the information needed to verify identity. This is perfect for REST APIs.

## Detailed Step-by-Step Explanation

### JWT Token Structure

```
┌─────────────────────────────────────────────────────────────────────────┐
│                    JWT TOKEN STRUCTURE                                   │
├─────────────────────────────────────────────────────────────────────────┤
│                                                                         │
│  eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9                                 │
│  .eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2Mj │
│  M5MDIyfQ                                                             │
│  .SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c                        │
│                                                                         │
│  ┌─────────────────────────────────────────────────────────────────┐   │
│  │ HEADER: Algorithm & Token Type                                 │   │
│  │ { "alg": "HS256", "typ": "JWT" }                             │   │
│  └─────────────────────────────────────────────────────────────────┘   │
│  .                                                                    │
│  ┌─────────────────────────────────────────────────────────────────┐   │
│  │ PAYLOAD: Claims (Data)                                          │   │
│  │ { "sub": "123", "name": "John Doe", "role": "USER",           │   │
│  │   "iat": 1516239022 }                                          │   │
│  └─────────────────────────────────────────────────────────────────┘   │
│  .                                                                    │
│  ┌─────────────────────────────────────────────────────────────────┐   │
│  │ SIGNATURE: Verify token                                         │   │
│  │ HMACSHA256(base64UrlEncode(header) + "." +                    │   │
│  │ base64UrlEncode(payload), secret)                              │   │
│  └─────────────────────────────────────────────────────────────────┘   │
│                                                                         │
└─────────────────────────────────────────────────────────────────────────┘
```

### JWT Dependencies

```xml
<dependency>
    <groupId>io.jsonwebtoken</groupId>
    <artifactId>jjwt-api</artifactId>
    <version>0.12.3</version>
</dependency>
<dependency>
    <groupId>io.jsonwebtoken</groupId>
    <artifactId>jjwt-impl</artifactId>
    <version>0.12.3</version>
    <scope>runtime</scope>
</dependency>
<dependency>
    <groupId>io.jsonwebtoken</groupId>
    <artifactId>jjwt-jackson</artifactId>
    <version>0.12.3</version>
    <scope>runtime</scope>
</dependency>
```

### JWT Utility Class

```java
@Component
public class JwtUtil {
    
    @Value("${jwt.secret}")
    private String secret;
    
    @Value("${jwt.expiration}")
    private Long expiration;
    
    public String generateToken(String username) {
        return Jwts.builder()
            .subject(username)
            .issuedAt(new Date())
            .expiration(new Date(System.currentTimeMillis() + expiration))
            .signWith(SignatureAlgorithm.HS256, secret)
            .compact();
    }
    
    public String extractUsername(String token) {
        return Jwts.parser()
            .verifyWith(Keys.hmacShaKeyFor(secret.getBytes()))
            .build()
            .parseSignedClaims(token)
            .getPayload()
            .getSubject();
    }
    
    public boolean validateToken(String token, String username) {
        return extractUsername(token).equals(username) && !isTokenExpired(token);
    }
    
    private boolean isTokenExpired(String token) {
        return Jwts.parser()
            .verifyWith(Keys.hmacShaKeyFor(secret.getBytes()))
            .build()
            .parseSignedClaims(token)
            .getPayload()
            .getExpiration()
            .before(new Date());
    }
}
```

### Authentication Controller

```java
@RestController
@RequestMapping("/api/auth")
public class AuthController {
    
    private final AuthenticationManager authenticationManager;
    private final JwtUtil jwtUtil;
    
    public AuthController(AuthenticationManager authenticationManager, JwtUtil jwtUtil) {
        this.authenticationManager = authenticationManager;
        this.jwtUtil = jwtUtil;
    }
    
    @PostMapping("/login")
    public ResponseEntity<?> login(@RequestBody LoginRequest request) {
        try {
            authenticationManager.authenticate(
                new UsernamePasswordAuthenticationToken(request.getUsername(), request.getPassword())
            );
            String token = jwtUtil.generateToken(request.getUsername());
            return ResponseEntity.ok(new JwtResponse(token));
        } catch (Exception e) {
            return ResponseEntity.status(401).body(new ErrorResponse("Invalid credentials"));
        }
    }
}
```

### JWT Filter

```java
@Component
public class JwtFilter extends OncePerRequestFilter {
    
    private final JwtUtil jwtUtil;
    private final UserDetailsService userDetailsService;
    
    @Override
    protected void doFilterInternal(HttpServletRequest request, HttpServletResponse response, 
            FilterChain filterChain) throws ServletException, IOException {
        
        String authHeader = request.getHeader("Authorization");
        
        if (authHeader != null && authHeader.startsWith("Bearer ")) {
            String token = authHeader.substring(7);
            try {
                String username = jwtUtil.extractUsername(token);
                UserDetails userDetails = userDetailsService.loadUserByUsername(username);
                
                if (jwtUtil.validateToken(token, username)) {
                    UsernamePasswordAuthenticationToken auth = 
                        new UsernamePasswordAuthenticationToken(userDetails, null, userDetails.getAuthorities());
                    SecurityContextHolder.getContext().setAuthentication(auth);
                }
            } catch (Exception e) {
                // Invalid token
            }
        }
        
        filterChain.doFilter(request, response);
    }
}
```

## Angular JWT Integration

```typescript
// Auth Interceptor
import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler } from '@angular/common/http';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
  intercept(req: HttpRequest<any>, next: HttpHandler) {
    const token = localStorage.getItem('token');
    if (token) {
      const cloned = req.clone({
        headers: req.headers.set('Authorization', `Bearer ${token}`)
      });
      return next.handle(cloned);
    }
    return next.handle(req);
  }
}

// In app.module.ts providers
{ provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true }
```

## Student Hands-On Exercises

### Exercise 1: Add JWT (Easy)
Add JWT authentication to your project.

### Exercise 2: Secure Endpoints (Medium)
Secure your API endpoints with JWT.

### Exercise 3: Angular Integration (Medium)
Complete the Angular login flow with token storage.

---

## Summary

You've learned:
- JWT token structure
- Generating and validating tokens
- JWT filter for request authentication
- Angular JWT integration

---

**Next Lesson**: In the next lesson, we'll explore [Pagination and Sorting](19_Pagination_and_Sorting.md).
