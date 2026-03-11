# Section 17: Authentication & Security

## Spring Security + JWT

### Security Configuration
```java
@Configuration
@EnableWebSecurity
public class SecurityConfig {

    @Bean
    public SecurityFilterChain filterChain(HttpSecurity http) throws Exception {
        http
            .csrf(csrf -> csrf.disable())
            .authorizeHttpRequests(auth -> auth
                .requestMatchers("/api/auth/**").permitAll()
                .requestMatchers("/api/products").authenticated()
                .anyRequest().authenticated()
            )
            .addFilterBefore(jwtFilter, UsernamePasswordAuthenticationFilter.class);
        
        return http.build();
    }
}
```

### JWT Token Generation
```java
public String generateToken(User user) {
    return Jwts.builder()
        .setSubject(user.getUsername())
        .claim("roles", user.getRoles())
        .setIssuedAt(new Date())
        .setExpiration(new Date(System.currentTimeMillis() + 86400000))
        .signWith(SignatureAlgorithm.HS512, "secretKey")
        .compact();
}
```

### Angular Auth Interceptor
```typescript
@Injectable()
export class AuthInterceptor implements HttpInterceptor {
    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        const token = this.authService.getToken();
        
        if (token) {
            const cloned = req.clone({
                headers: req.headers.set("Authorization", `Bearer ${token}`)
            });
            return next.handle(cloned);
        }
        
        return next.handle(req);
    }
}
```

---

## Summary

Security = Authentication + Authorization + JWT + HTTPS
