# Angular Router

## Learning Objectives

By the end of this lesson, students will be able to:

- [ ] Configure Angular Router with routes and navigation
- [ ] Implement route parameters and query parameters
- [ ] Use route guards for authentication and authorization
- [ ] Implement route resolvers for pre-loading data
- [ ] Apply lazy loading for performance optimization

## Conceptual Explanation

**Visual Analogy**: Think of the Angular Router as a **traffic controller** at a busy airport. Just as the controller directs planes to different gates (destinations), the router directs users to different pages (views) based on their requests. The router manages the navigation flow, ensures passengers (users) reach the right destinations (components), and can even check if they're allowed to board (guards)!

### How Angular Router Works

```
┌─────────────────────────────────────────────────────────────────────┐
│                    Angular Router Flow                              │
├─────────────────────────────────────────────────────────────────────┤
│                                                                     │
│   User clicks URL or link                                           │
│            │                                                        │
│            ▼                                                        │
│   ┌─────────────────┐                                                │
│   │   Router       │  Parses URL                                    │
│   │   (Traffic     │  Matches route                                │
│   │   Controller)  │  pattern                                      │
│   └────────┬────────┘                                                │
│            │                                                        │
│            ▼                                                        │
│   ┌─────────────────┐                                                │
│   │   Guards        │  Check permissions                           │
│   │   (Security)    │  Can user access?                           │
│   └────────┬────────┘                                                │
│            │                                                        │
│            ▼                                                        │
│   ┌─────────────────┐                                                │
│   │   Resolvers     │  Pre-load data                               │
│   │   (Data Prep)   │  before rendering                            │
│   └────────┬────────┘                                                │
│            │                                                        │
│            ▼                                                        │
│   ┌─────────────────┐                                                │
│   │   Components    │  Render the view                             │
│   │   (Destination) │                                                │
│   └─────────────────┘                                                │
│                                                                     │
└─────────────────────────────────────────────────────────────────────┘
```

## Real-World Application Context

### Why Router Matters

1. **SPA Navigation**: Navigate without page reloads
2. **Deep Linking**: Share specific views via URLs
3. **SEO**: Search engines can index pages
4. **History Management**: Browser back/forward works
5. **Code Splitting**: Load only needed code

### Industry Use Cases

- **E-commerce**: Product pages, cart, checkout flows
- **Dashboards**: Admin panels with role-based access
- **Social Media**: User profiles, feeds
- **CRUD Apps**: List, detail, edit, create forms

## Step-by-Step Walkthrough

### Basic Route Configuration

#### Step 1: Create Route File

```typescript
// app.routes.ts
import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: '', redirectTo: 'home', pathMatch: 'full' },
  { path: 'home', loadComponent: () => import('./home/home.component').then(m => m.HomeComponent) },
  { path: 'about', loadComponent: () => import('./about/about.component').then(m => m.AboutComponent) },
  { path: '**', loadComponent: () => import('./not-found/not-found.component').then(m => m.NotFoundComponent) }
];
```

#### Step 2: Configure Router

```typescript
// app.config.ts
import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes)
  ]
};
```

#### Step 3: Add Router Outlet

```typescript
// app.component.ts
import { Component } from '@angular/core';
import { RouterOutlet, RouterLink, RouterLinkActive } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, RouterLink, RouterLinkActive],
  template: `
    <nav>
      <a routerLink="/home" routerLinkActive="active">Home</a>
      <a routerLink="/about" routerLinkActive="active">About</a>
      <a routerLink="/users" routerLinkActive="active">Users</a>
    </nav>
    
    <main>
      <router-outlet></router-outlet>
    </main>
  `,
  styles: [`
    nav { display: flex; gap: 1rem; padding: 1rem; background: #f5f5f5; }
    a { text-decoration: none; color: #333; padding: 0.5rem 1rem; border-radius: 4px; }
    a.active { background: #1976d2; color: white; }
    main { padding: 1rem; }
  `]
})
export class AppComponent {}
```

### Route Parameters

```typescript
// product.routes.ts
export const routes: Routes = [
  { 
    path: 'products', 
    loadComponent: () => import('./product-list/product-list.component').then(m => m.ProductListComponent) 
  },
  { 
    path: 'products/:id', 
    loadComponent: () => import('./product-detail/product-detail.component').then(m => m.ProductDetailComponent) 
  }
];

// product-detail.component.ts
import { Component, inject } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-product-detail',
  standalone: true,
  imports: [CommonModule, RouterLink],
  template: `
    <h1>Product Details</h1>
    
    @if (product) {
      <div class="product">
        <h2>{{ product.name }}</h2>
        <p>Price: {{ product.price | currency }}</p>
        <p>Category: {{ product.category }}</p>
        <a routerLink="/products">Back to Products</a>
      </div>
    } @else {
      <p>Product not found</p>
    }
  `
})
export class ProductDetailComponent {
  private route = inject(ActivatedRoute);
  product: any;
  
  ngOnInit(): void {
    // Get route parameter
    const id = this.route.snapshot.paramMap.get('id');
    console.log('Product ID:', id);
    
    // Or use observable for reactive updates
    this.route.paramMap.subscribe(params => {
      const productId = params.get('id');
      this.loadProduct(productId);
    });
  }
  
  loadProduct(id: string | null): void {
    // In real app, call service
    this.product = { id, name: 'Sample Product', price: 99.99, category: 'Electronics' };
  }
}
```

### Query Parameters

```typescript
// Using query parameters
<a [routerLink]="['/products']" [queryParams]="{ category: 'electronics', sort: 'price' }">
  Electronics
</a>

// In component
import { ActivatedRoute } from '@angular/router';

@Component({...})
export class ProductListComponent {
  private route = inject(ActivatedRoute);
  
  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      const category = params['category'];
      const sort = params['sort'];
      console.log('Category:', category, 'Sort:', sort);
    });
  }
}

// Programmatic navigation with query params
import { Router } from '@angular/router';

export class SomeComponent {
  private router = inject(Router);
  
  search(term: string): void {
    this.router.navigate(['/search'], { queryParams: { q: term } });
  }
}
```

### Route Guards

#### CanActivate Guard

```typescript
// auth.guard.ts
import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';
import { AuthService } from './auth.service';

export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);
  
  if (authService.isLoggedIn()) {
    return true;
  }
  
  // Redirect to login with return URL
  return router.createUrlTree(['/login'], { 
    queryParams: { returnUrl: state.url } 
  });
};

// Using the guard
export const routes: Routes = [
  { 
    path: 'dashboard', 
    loadComponent: () => import('./dashboard/dashboard.component').then(m => m.DashboardComponent),
    canActivate: [authGuard]
  }
];
```

#### CanMatch Guard

```typescript
// admin.guard.ts
export const adminGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  return authService.hasRole('admin') || router.createUrlTree(['/']);
};

// Prevent access to route entirely
export const routes: Routes = [
  { 
    path: 'admin', 
    canMatch: [() => inject(AuthService).hasRole('admin')],
    loadChildren: () => import('./admin/admin.routes').then(m => m.ADMIN_ROUTES)
  }
];
```

#### CanDeactivate Guard

```typescript
// can-deactivate.guard.ts
import { CanDeactivateFn } from '@angular/router';
import { inject } from '@angular/core';

export interface HasUnsavedChanges {
  hasUnsavedChanges(): boolean;
}

export const unsavedChangesGuard: CanDeactivateFn<HasUnsavedChanges> = (component) => {
  if (component.hasUnsavedChanges()) {
    return confirm('You have unsaved changes. Are you sure you want to leave?');
  }
  return true;
};

// Using the guard
export const routes: Routes = [
  { 
    path: 'edit/:id', 
    loadComponent: () => import('./edit/edit.component').then(m => m.EditComponent),
    canDeactivate: [unsavedChangesGuard]
  }
];
```

### Route Resolvers

```typescript
// product.resolver.ts
import { ResolveFn } from '@angular/router';
import { inject } from '@angular/core';
import { ProductService } from './product.service';

export const productResolver: ResolveFn<Product> = (route, state) => {
  const productService = inject(ProductService);
  const id = route.paramMap.get('id');
  
  // Returns data before navigation completes
  return productService.getProduct(id!);
};

// Using resolver
export const routes: Routes = [
  { 
    path: 'products/:id', 
    loadComponent: () => import('./product-detail/product-detail.component').then(m => m.ProductDetailComponent),
    resolve: { product: productResolver }
  }
];

// Accessing resolved data
@Component({...})
export class ProductDetailComponent {
  private route = inject(ActivatedRoute);
  
  ngOnInit(): void {
    // Data is already loaded!
    const product = this.route.snapshot.data['product'];
    // Or use observable
    this.route.data.subscribe(data => {
      const product = data['product'];
    });
  }
}
```

### Lazy Loading

```typescript
// Lazy load a single component
{ 
  path: 'products', 
  loadComponent: () => import('./products/products.component').then(m => m.ProductsComponent) 
}

// Lazy load multiple routes (feature module)
{ 
  path: 'admin', 
  loadChildren: () => import('./admin/admin.routes').then(m => m.ADMIN_ROUTES) 
}

// In admin.routes.ts
export const ADMIN_ROUTES: Routes = [
  { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
  { path: 'dashboard', loadComponent: () => import('./admin-dashboard.component').then(m => m.AdminDashboardComponent) },
  { path: 'users', loadComponent: () => import('./admin-users.component').then(m => m.AdminUsersComponent) }
];

// Preloading
import { provideRouter, withPreloading, PreloadAllModules } from '@angular/router';

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes, withPreloading(PreloadAllModules))
  ]
};
```

## Complete Example: E-commerce Router

```typescript
// app.routes.ts
import { Routes } from '@angular/router';
import { authGuard } from './guards/auth.guard';
import { adminGuard } from './guards/admin.guard';

export const routes: Routes = [
  // Public routes
  { 
    path: '', 
    redirectTo: 'home', 
    pathMatch: 'full' 
  },
  { 
    path: 'home', 
    loadComponent: () => import('./home/home.component').then(m => m.HomeComponent) 
  },
  { 
    path: 'products', 
    loadComponent: () => import('./products/product-list.component').then(m => m.ProductListComponent) 
  },
  { 
    path: 'products/:id', 
    loadComponent: () => import('./products/product-detail.component').then(m => m.ProductDetailComponent) 
  },
  
  // Protected routes
  { 
    path: 'cart', 
    loadComponent: () => import('./cart/cart.component').then(m => m.CartComponent),
    canActivate: [authGuard]
  },
  { 
    path: 'checkout', 
    loadComponent: () => import('./checkout/checkout.component').then(m => m.CheckoutComponent),
    canActivate: [authGuard]
  },
  { 
    path: 'profile', 
    loadComponent: () => import('./profile/profile.component').then(m => m.ProfileComponent),
    canActivate: [authGuard]
  },
  
  // Admin routes (lazy loaded)
  { 
    path: 'admin', 
    loadChildren: () => import('./admin/admin.routes').then(m => m.ADMIN_ROUTES),
    canMatch: [adminGuard]
  },
  
  // 404
  { 
    path: '**', 
    loadComponent: () => import('./not-found/not-found.component').then(m => m.NotFoundComponent) 
  }
];
```

## Best Practices

### 1. Use Functional Guards (Angular 14+)

```typescript
// Good: Functional guard
export const authGuard: CanActivateFn = (route, state) => {
  return inject(AuthService).isLoggedIn();
};

// Avoid: Class-based guard (deprecated)
@Injectable()
export class AuthGuard implements CanActivate {
  canActivate(): boolean { return true; }
}
```

### 2. Use Lazy Loading

```typescript
// Good: Lazy loading
{ path: 'admin', loadChildren: () => import('./admin/admin.module').then(m => m.AdminModule) }

// Avoid: Eager loading everything
{ path: 'admin', component: AdminComponent } // Loads immediately
```

### 3. Handle Route Parameters Properly

```typescript
// Good: Subscribe to param changes
ngOnInit() {
  this.route.paramMap.subscribe(params => {
    this.loadProduct(params.get('id')!);
  });
}

// Avoid: Only getting once
ngOnInit() {
  const id = this.route.snapshot.paramMap.get('id'); // Won't update on navigation
}
```

### 4. Use Path Match Correctly

```typescript
// For empty path - must use pathMatch: 'full'
{ path: '', redirectTo: 'home', pathMatch: 'full' }

// For wildcard - default is 'prefix'
{ path: '**', component: NotFoundComponent }
```

## Common Pitfalls and Debugging

### Pitfall 1: Route Not Found (404)

```typescript
// Problem: ** route catches everything incorrectly
{ path: '**', component: NotFoundComponent },
{ path: 'home', component: HomeComponent } // Never reached!

// Solution: Place wildcard last
{ path: 'home', component: HomeComponent },
{ path: '**', component: NotFoundComponent } // Must be last!
```

### Pitfall 2: Route Params Not Available

```typescript
// Problem: Trying to access params before they're available
ngOnInit() {
  const id = this.route.snapshot.paramMap.get('id'); // Returns null sometimes
}

// Solution: Subscribe to paramMap
ngOnInit() {
  this.route.paramMap.subscribe(params => {
    const id = params.get('id');
  });
}
```

### Pitfall 3: Guard Not Working

```typescript
// Problem: Guard not added to route
{ path: 'protected', component: ProtectedComponent } // No guard!

// Solution: Add canActivate
{ 
  path: 'protected', 
  component: ProtectedComponent,
  canActivate: [authGuard]
}
```

## Hands-On Exercise

### Exercise 2.2: Router Implementation

**Objective**: Build a complete navigation system

**Requirements**:
1. Create routes for home, about, products, product detail
2. Implement route parameters for product ID
3. Add query parameter filtering
4. Create auth guard for protected routes
5. Implement lazy loading for a feature

**Deliverable**: Complete router configuration

**Assessment Criteria**:
- [ ] At least 5 routes configured
- [ ] Route parameters working
- [ ] Query parameters working
- [ ] Guard implemented
- [ ] Lazy loading working

## Summary

- **Routes** define URL-to-component mappings
- **Route parameters** capture dynamic URL segments
- **Query parameters** for optional filters
- **Guards** protect routes (authGuard, adminGuard)
- **Resolvers** pre-load data before navigation
- **Lazy loading** improves initial load performance

## Suggested Reading

- [Angular Router Documentation](https://angular.io/guide/router)
- "Routing in Angular" - Official Guide

## Next Steps

In the next lecture, we'll explore HTTP Client and REST API integration.
