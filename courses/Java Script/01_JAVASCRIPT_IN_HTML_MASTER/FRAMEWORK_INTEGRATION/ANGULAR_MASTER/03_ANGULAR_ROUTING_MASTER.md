# 🔷 Angular Routing Complete Guide

## Navigation in Angular

---

## Table of Contents

1. [Router Setup](#router-setup)
2. [Route Configuration](#route-configuration)
3. [Router Outlet](#router-outlet)
4. [Route Parameters](#route-parameters)
5. [Route Guards](#route-guards)
6. [Lazy Loading](#lazy-loading)

---

## Router Setup

### Basic Setup

```typescript
// app.routes.ts
import { Routes } from '@angular/router'
import { HomeComponent } from './home/home.component'
import { AboutComponent } from './about/about.component'

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'about', component: AboutComponent },
  { path: '**', redirectTo: '' }
]
```

```typescript
// app.config.ts
import { ApplicationConfig } from '@angular/core'
import { provideRouter } from '@angular/router'
import { routes } from './app.routes'

export const appConfig: ApplicationConfig = {
  providers: [provideRouter(routes)]
}
```

---

## Route Configuration

### Route with Children

```typescript
export const routes: Routes = [
  {
    path: 'dashboard',
    component: DashboardComponent,
    children: [
      { path: '', component: DashboardHomeComponent },
      { path: 'settings', component: DashboardSettingsComponent }
    ]
  }
]
```

---

## Router Outlet

### Using Router Outlet

```html
<!-- app.component.html -->
<nav>
  <a routerLink="/">Home</a>
  <a routerLink="/about">About</a>
</nav>

<router-outlet></router-outlet>
```

---

## Route Parameters

### Reading Parameters

```typescript
import { ActivatedRoute } from '@angular/router'
import { Component, inject } from '@angular/core'

@Component({
  selector: 'app-user',
  template: `<p>{{ userId }}</p>`
})
export class UserComponent {
  private route = inject(ActivatedRoute)
  userId = this.route.snapshot.paramMap.get('id')
}
```

---

## Route Guards

### CanActivate

```typescript
import { inject } from '@angular/core'
import { Router, CanActivateFn } from '@angular/router'

export const authGuard: CanActivateFn = () => {
  const router = inject(Router)
  
  const isAuthenticated = true // Check auth
  
  if (isAuthenticated) {
    return true
  }
  
  return router.parseUrl('/login')
}
```

```typescript
// routes
{
  path: 'dashboard',
  component: DashboardComponent,
  canActivate: [authGuard]
}
```

---

## Lazy Loading

### Lazy Loading Routes

```typescript
export const routes: Routes = [
  {
    path: 'admin',
    loadComponent: () => import('./admin/admin.component')
      .then(m => m.AdminComponent)
  }
]
```

### Lazy Loading with Children

```typescript
{
  path: 'dashboard',
  loadChildren: () => import('./dashboard/dashboard.routes')
    .then(m => m.DASHBOARD_ROUTES)
}
```

---

## Summary

### Key Takeaways

1. **Routes**: Define navigation
2. **Outlet**: Router view
3. **Parameters**: Dynamic routes
4. **Guards**: Protection
5. **Lazy Loading**: Performance

### Next Steps

- Continue with Browser APIs: [ADVANCED_BROWSER_APIS/01_WEB_WORKERS_ADVANCED.md](../../ADVANCED_BROWSER_APIS/01_WEB_WORKERS_ADVANCED.md)
- Study route resolvers
- Implement module federation

---

## Cross-References

- **Previous**: [02_ANGULAR_COMPONENTS_MASTER.md](02_ANGULAR_COMPONENTS_MASTER.md)

---

*Last updated: 2024*