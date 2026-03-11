# Section 10: Angular Routing & Navigation

## Setting Up Routing

### 1. Define Routes
```typescript
// app-routing.module.ts
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductListComponent } from './product-list/product-list.component';
import { ProductDetailComponent } from './product-detail/product-detail.component';
import { ProductFormComponent } from './product-form/product-form.component';

const routes: Routes = [
  { path: '', redirectTo: '/products', pathMatch: 'full' },
  { path: 'products', component: ProductListComponent },
  { path: 'products/new', component: ProductFormComponent },
  { path: 'products/:id', component: ProductDetailComponent },
  { path: 'products/:id/edit', component: ProductFormComponent },
  { path: '**', component: NotFoundComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
```

### 2. Add Router Outlet
```html
<!-- app.component.html -->
<nav>
  <a routerLink="/products">Products</a>
  <a routerLink="/products/new">Add Product</a>
</nav>

<router-outlet></router-outlet>
```

### 3. Navigate in Code
```typescript
import { Router } from '@angular/router';

constructor(private router: Router) { }

// Navigate to products
this.router.navigate(['/products']);

// Navigate to product detail
this.router.navigate(['/products', product.id]);

// Navigate with query params
this.router.navigate(['/products'], { queryParams: { page: 2 } });
```

### 4. Get Route Parameters
```typescript
import { ActivatedRoute } from '@angular/router';

constructor(private route: ActivatedRoute) { }

ngOnInit(): void {
  this.route.paramMap.subscribe(params => {
    this.productId = Number(params.get('id'));
  });
}
```

---

## Summary

Router + Routes + RouterOutlet = Navigation
