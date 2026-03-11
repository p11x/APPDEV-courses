# Section 18: Testing & Debugging

## Testing Types

### Unit Testing (Angular)
```typescript
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ProductListComponent } from './product-list.component';

describe('ProductListComponent', () => {
  let component: ProductListComponent;
  let fixture: ComponentFixture<ProductListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProductListComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProductListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load products on init', () => {
    expect(component.products.length).toBeGreaterThan(0);
  });
});
```

### Unit Testing (Spring Boot)
```java
@SpringBootTest
class ProductServiceTest {

    @Autowired
    private ProductService productService;

    @Test
    void testGetAllProducts() {
        List<Product> products = productService.getAllProducts();
        assertNotNull(products);
    }
}
```

---

## Debugging

### Angular DevTools
- Install Angular DevTools extension
- Inspect components, change detection

### Browser DevTools
- Console for errors
- Network tab for API calls
- Elements for DOM

---

## Summary

Testing = Unit Tests + Integration Tests + Debugging
