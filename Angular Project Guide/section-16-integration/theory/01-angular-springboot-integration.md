# Section 16: Integration - Angular + Spring Boot

## Connecting Frontend and Backend

### 1. Enable CORS in Spring Boot
```java
@CrossOrigin(origins = "http://localhost:4200")
@RestController
@RequestMapping("/api/products")
public class ProductController {
    // ...
}
```

### 2. Configure Proxy in Angular (optional)
```json
// proxy.conf.json
{
  "/api": {
    "target": "http://localhost:8080",
    "secure": false
  }
}
```

### 3. Angular Service with HttpClient
```typescript
// product.service.ts
@Injectable({ providedIn: 'root' })
export class ProductService {
  private apiUrl = '/api/products';

  constructor(private http: HttpClient) { }

  getProducts(): Observable<Product[]> {
    return this.http.get<Product[]>(this.apiUrl);
  }
}
```

---

## Complete Data Flow

```
User Action → Angular Component → Service → HTTP → Spring Boot → JPA → SQL Server
                                                                           ↓
User View ← Angular Component ← Service ← HTTP ← Spring Boot ← JPA ← Result
```

---

## Summary

Integration = HTTP + JSON + REST + CORS
