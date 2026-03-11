# Single Page Application (SPA) Architecture

## Learning Objectives

By the end of this lesson, students will be able to:

- [ ] Explain the concept of Single Page Applications and their benefits
- [ ] Compare SPA with traditional multi-page applications
- [ ] Understand how Angular implements SPA architecture
- [ ] Identify the key components of SPA architecture
- [ ] Analyze the request/response cycle in SPAs

## Conceptual Explanation

**Visual Analogy**: Imagine a **magic book** where when you turn a page, the entire book doesn't reload - only the content on that specific page changes. The book cover, page numbers, and navigation remain the same, but the content transforms instantly. This is exactly how a Single Page Application works!

### What is a Single Page Application?

A Single Page Application (SPA) is a web application that loads a single HTML page and dynamically updates that page as the user interacts with the application. Instead of loading completely new pages from the server, SPAs use JavaScript to modify the current page.

### Traditional Web Architecture vs SPA

#### Traditional Multi-Page Application (MPA)

```
┌─────────────────────────────────────────────────────────────┐
│                     Traditional Web App                       │
├─────────────────────────────────────────────────────────────┤
│  Browser → Request → Server → Render HTML → Response        │
│                                                              │
│  User clicks link:                                           │
│  1. Browser sends request to server                         │
│  2. Server processes request                                 │
│  3. Server generates new HTML page                           │
│  4. Server sends complete HTML back                          │
│  5. Browser reloads entire page                              │
│                                                              │
│  Each page refresh = Full reload                            │
└─────────────────────────────────────────────────────────────┘
```

#### Single Page Application

```
┌─────────────────────────────────────────────────────────────┐
│                    Single Page Application                   │
├─────────────────────────────────────────────────────────────┤
│  Initial Load: Browser → Request → Server → SPA Bundle      │
│                                                              │
│  User navigates:                                            │
│  1. JavaScript intercepts navigation                         │
│  2. Router determines what to show                          │
│  3. Components render/change dynamically                     │
│  4. Only affected parts update - NO page reload              │
│                                                              │
│  Subsequent navigations = Instant (client-side)            │
└─────────────────────────────────────────────────────────────┘
```

## Real-World Application Context

### Why SPAs Matter in Modern Development

1. **User Experience**: Faster, more responsive applications feel like desktop software
2. **Mobile-First**: SPAs work seamlessly on mobile devices
3. **Developer Productivity**: Faster development cycles, easier debugging
4. **Real-time Applications**: Perfect for chat, dashboards, collaborative tools

### Industry Use Cases

- **Gmail**: Email without page reloads
- **Google Maps**: Interactive maps with smooth transitions
- **Facebook/Twitter**: Infinite scroll and real-time updates
- **Trello**: Drag-and-drop project management
- **Netflix**: Smooth video streaming interface

## Angular's SPA Implementation

### How Angular Implements SPA

Angular provides a complete SPA solution through:

```
┌─────────────────────────────────────────────────────────────┐
│                    Angular SPA Architecture                 │
├─────────────────────────────────────────────────────────────┤
│                                                              │
│  ┌─────────────┐    ┌─────────────┐    ┌─────────────┐       │
│  │   Router   │───▶│  Component  │───▶│   View      │       │
│  │  (URL Hub) │    │ (Logic)     │    │ (Template)  │       │
│  └─────────────┘    └─────────────┘    └─────────────┘       │
│         │                                      │             │
│         ▼                                      ▼             │
│  ┌─────────────┐                       ┌─────────────┐       │
│  │   Guards    │                       │   Services  │       │
│  │  (Security) │                       │  (Data)     │       │
│  └─────────────┘                       └─────────────┘       │
│                                                              │
└─────────────────────────────────────────────────────────────┘
```

### Key Components of Angular SPA

1. **Router**: Manages navigation and URL changes
2. **Components**: Self-contained UI building blocks
3. **Templates**: Define the view structure
4. **Services**: Handle business logic and data
5. **Modules**: Organize application parts (optional in Angular 17+)

## Step-by-Step Walkthrough

### Creating a Basic SPA Structure

Let's create a simple Angular SPA to understand the architecture:

#### Step 1: Create a New Project

```bash
ng new spa-demo --routing --style=scss
cd spa-demo
```

Expected output:
```
√ Package installation completed successfully.
√ Repository initialization completed successfully.
√ Created spa-demo/
```

#### Step 2: Generate Components

```bash
ng generate component home
ng generate component about
ng generate component contact
```

Expected output:
```
CREATE src/app/home/home.component.html (x bytes)
CREATE src/app/home/home.component.ts (x bytes)
CREATE src/app/home/home.component.spec.ts (x bytes)
CREATE src/app/home/home.component.scss (x bytes)
```

#### Step 3: Configure Routing

Edit `src/app/app.routes.ts`:

```typescript
import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { AboutComponent } from './about/about.component';
import { ContactComponent } from './contact/contact.component';

export const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: 'home', component: HomeComponent },
  { path: 'about', component: AboutComponent },
  { path: 'contact', component: ContactComponent },
  { path: '**', redirectTo: '/home' }
];
```

#### Step 4: Create Navigation

Edit `src/app/app.component.html`:

```html
<!-- Navigation -->
<nav>
  <a routerLink="/home" routerLinkActive="active">Home</a>
  <a routerLink="/about" routerLinkActive="active">About</a>
  <a routerLink="/contact" routerLinkActive="active">Contact</a>
</nav>

<!-- Route Outlet - Where views render -->
<router-outlet></router-outlet>
```

#### Step 5: Add Basic Styling

Edit `src/app/app.component.scss`:

```scss
nav {
  display: flex;
  gap: 1rem;
  padding: 1rem;
  background: #f5f5f5;
  
  a {
    padding: 0.5rem 1rem;
    text-decoration: none;
    color: #333;
    border-radius: 4px;
    
    &.active {
      background: #1976d2;
      color: white;
    }
  }
}
```

#### Step 6: Run the Application

```bash
ng serve
```

Expected output:
```
● Local: http://localhost:4200/
● Hot Module Replacement (HMR) enabled
```

Open your browser to http://localhost:4200. Navigate between links and notice **no page reload occurs**!

## Code Example: Component Communication in SPA

Here's a complete example showing component interaction in Angular's SPA:

```typescript
// src/app/home/home.component.ts
import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, RouterLinkActive } from '@angular/router';

interface NavItem {
  path: string;
  label: string;
  icon: string;
}

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, RouterLink, RouterLinkActive],
  template: `
    <div class="home-container">
      <h1>Welcome to Our SPA</h1>
      <p>This is a Single Page Application built with Angular</p>
      
      <div class="features">
        <div class="feature-card" *ngFor="let feature of features">
          <h3>{{ feature.title }}</h3>
          <p>{{ feature.description }}</p>
        </div>
      </div>
      
      <div class="navigation-demo">
        <h2>Navigation Demo</h2>
        <p>Click the links above - no page reload!</p>
        <button (click)="navigateToAbout()">Go to About</button>
      </div>
    </div>
  `,
  styles: [`
    .home-container {
      padding: 2rem;
      max-width: 800px;
      margin: 0 auto;
    }
    .features {
      display: grid;
      grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
      gap: 1rem;
      margin: 2rem 0;
    }
    .feature-card {
      padding: 1.5rem;
      border: 1px solid #ddd;
      border-radius: 8px;
      background: #fafafa;
    }
    button {
      padding: 0.75rem 1.5rem;
      background: #1976d2;
      color: white;
      border: none;
      border-radius: 4px;
      cursor: pointer;
    }
  `]
})
export class HomeComponent {
  features = [
    { title: 'Fast', description: 'Instant page transitions' },
    { title: 'Responsive', description: 'Works on all devices' },
    { title: 'Interactive', description: 'Rich user experience' }
  ];

  navigateToAbout() {
    // Programmatically navigate - SPA style
    // This requires Router injection
    console.log('Navigation would happen here');
  }
}
```

## Best Practices

### 1. Lazy Loading Routes
Load modules only when needed to improve initial load time:

```typescript
// Good: Lazy loading
export const routes: Routes = [
  {
    path: 'admin',
    loadChildren: () => import('./admin/admin.module')
      .then(m => m.AdminModule)
  }
];

// Avoid: Eager loading everything
export const routes: Routes = [
  { path: 'admin', component: AdminComponent } // Loads immediately
];
```

### 2. Use Router Guards
Protect routes and control access:

```typescript
// auth.guard.ts
export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  return authService.isLoggedIn() || inject(Router).createUrlTree(['/login']);
};
```

### 3. Handle Browser Back/Forward
Angular Router handles browser history automatically:

```typescript
// This works out of the box with Angular Router
// Users can use browser back/forward buttons
```

## Common Pitfalls and Debugging

### Pitfall 1: Memory Leaks
**Problem**: Subscriptions not cleaned up cause memory leaks
**Solution**: Use `AsyncPipe` or `takeUntilDestroyed()`

```typescript
// Bad: Memory leak
export class MyComponent {
  ngOnInit() {
    this.dataService.getData().subscribe(data => {
      this.data = data;
    });
    // Subscription never cleaned up!
  }
}

// Good: Proper cleanup
export class MyComponent implements OnDestroy {
  private destroyRef = inject(DestroyRef);
  
  ngOnInit() {
    this.dataService.getData()
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe(data => {
        this.data = data;
      });
  }
}
```

### Pitfall 2: State Not Persisting
**Problem**: State lost on navigation
**Solution**: Use services with BehaviorSubjects or NgRx

### Pitfall 3: Deep Linking Issues
**Problem**: Refreshing a route shows 404
**Solution**: Configure server to redirect all routes to index.html

## Hands-On Exercise

### Exercise 1.2: Build a Simple SPA

**Objective**: Create a personal portfolio SPA with multiple pages

**Requirements**:
1. Create a new Angular project
2. Add 4 routes: Home, About, Projects, Contact
3. Each route should have a distinct component
4. Implement a navigation bar that works without page reloads
5. Add at least one feature to each page

**Deliverable**: A working SPA with functional navigation

**Assessment Criteria**:
- [ ] All 4 routes work correctly
- [ ] No page reloads on navigation
- [ ] Navigation highlights active route
- [ ] 404 redirect works for unknown routes
- [ ] Code follows Angular style guide

## Extension Challenge

**Challenge**: Add route parameters to your SPA

```typescript
// Add this route
{ path: 'projects/:id', component: ProjectDetailComponent }

// Create a component that reads the route parameter
@Component({...})
export class ProjectDetailComponent {
  private route = inject(ActivatedRoute);
  
  ngOnInit() {
    this.route.params.subscribe(params => {
      console.log('Project ID:', params['id']);
    });
  }
}
```

## Summary

- SPAs provide a desktop-like experience in web applications
- Angular's router manages navigation without page reloads
- Components are the building blocks of Angular SPAs
- Lazy loading improves performance
- Always clean up subscriptions to prevent memory leaks

## Next Steps

In the next lecture, we'll cover TypeScript fundamentals required for Angular development.
