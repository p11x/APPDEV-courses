# Section 7: Angular - Setup & Architecture

## Introduction to Angular

Angular is a powerful framework for building web applications. It's maintained by Google and provides a complete solution for frontend development.

### What You'll Learn

- Setting up Angular project
- Angular architecture
- Modules and components
- Directory structure

---

## 7.1 Creating an Angular Project

### Command to Create Project
```bash
ng new product-management --routing --style=css --skip-tests
```

### Project Structure
```
product-management/
├── src/
│   ├── app/
│   │   ├── app.component.ts      # Root component
│   │   ├── app.component.html
│   │   ├── app.component.css
│   │   ├── app.module.ts         # Root module
│   │   └── app-routing.module.ts # Routing
│   ├── assets/                   # Images, files
│   ├── environments/             # Environment config
│   ├── index.html
│   ├── main.ts                   # Entry point
│   └── styles.css                # Global styles
├── angular.json                  # Angular CLI config
├── package.json                  # Dependencies
└── tsconfig.json                 # TypeScript config
```

---

## 7.2 Angular Architecture

### Key Building Blocks

| Block | Purpose |
|-------|---------|
| **Module** | Groups related functionality |
| **Component** | UI element with logic and template |
| **Service** | Shared business logic |
| **Directive** | Custom behavior for elements |
| **Pipe** | Data transformation |
| **Router** | Navigation |

### Module System
```typescript
// app.module.ts
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { ProductListComponent } from './product-list/product-list.component';

@NgModule({
  declarations: [
    AppComponent,
    ProductListComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
```

---

## 7.3 Component Structure

### Component Files
```typescript
// product-list.component.ts
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css']
})
export class ProductListComponent implements OnInit {
  products: Product[] = [];
  loading = false;

  constructor() { }

  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts(): void {
    this.loading = true;
    // Load products
  }
}
```

```html
<!-- product-list.component.html -->
<div class="product-list">
    <h2>Products</h2>
    <div class="products">
        <div *ngFor="let product of products" class="product-card">
            <h3>{{ product.name }}</h3>
            <p>{{ product.price | currency }}</p>
        </div>
    </div>
</div>
```

---

## 7.4 Running the Application

### Development Server
```bash
ng serve
# or
npm start
```

### Access the App
- Open browser: http://localhost:4200

### Build for Production
```bash
ng build --configuration=production
```

---

## 7.5 Summary

### Key Takeaways

1. Angular CLI creates the project structure
2. Modules organize the application
3. Components are the building blocks
4. Use `ng serve` to run development server

### What's Next?

- **Section 8:** Angular Components & Templates
- **Section 9:** Angular Services & DI
- **Section 10:** Angular Routing
- **Section 11:** Angular Forms
